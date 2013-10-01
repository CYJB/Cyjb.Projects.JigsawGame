using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cyjb.Projects.JigsawGame.Jigsaw;
using Cyjb.Projects.JigsawGame.Renderer;
using SharpDX;
using SharpDX.Direct2D1;
using Point = System.Drawing.Point;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 拼图游戏的管理器。
	/// </summary>
	public sealed class GameManager : IDisposable
	{

		#region 渲染属性

		/// <summary>
		/// 设备管理器。
		/// </summary>
		private DeviceManager devices;
		/// <summary>
		/// Direct2D 的渲染目标。
		/// </summary>
		private RenderTarget renderTarget;
		/// <summary>
		/// 拼图的渲染控件。
		/// </summary>
		private JigsawRenderPanel renderPanel;
		/// <summary>
		/// 选择框的画笔。
		/// </summary>
		private SolidColorBrush selectionRectBrush;
		/// <summary>
		/// 选择框的样式。
		/// </summary>
		private StrokeStyle selectionRectStyle;
		/// <summary>
		/// 拼图渲染器。
		/// </summary>
		private JigsawRenderer renderer;
		/// <summary>
		/// 拼图渲染器的类型。
		/// </summary>
		private JigsawRendererType rendererType;

		#endregion // 渲染属性

		#region 拼图数据

		/// <summary>
		/// 拼图碎片集合。
		/// </summary>
		private JigsawPieceCollection pieces;
		/// <summary>
		/// 拼图信息。
		/// </summary>
		private JigsawInfo gameInfo;
		/// <summary>
		/// 拼图的背景图片。
		/// </summary>
		private Bitmap background;

		#endregion // 拼图数据

		#region 游戏属性

		/// <summary>
		/// 背景颜色。
		/// </summary>
		private Color4 backgroundColor = Color.White;
		/// <summary>
		/// 是否只显示边界拼图碎片。
		/// </summary>
		private bool showBorder = false;
		/// <summary>
		/// 是否正在选择多个拼图碎片。
		/// </summary>
		private bool isSelecting;
		/// <summary>
		/// 是否正在拖拽拼图碎片。
		/// </summary>
		private bool isDraging;
		/// <summary>
		/// 鼠标上次所在的位置。
		/// </summary>
		private Vector2 lastMousePoint;
		/// <summary>
		/// 选择多块拼图碎片的矩形范围。
		/// </summary>
		private RectangleF selectRect = new RectangleF(0, 0, -1, -1);
		/// <summary>
		/// 被选择的拼图碎片集合。
		/// </summary>
		private JigsawPieceSelectedCollection selectedPieces = new JigsawPieceSelectedCollection();
		/// <summary>
		/// 最后一个被高亮或被拖动的拼图碎片。
		/// </summary>
		private JigsawPiece lastPiece;
		/// <summary>
		/// 拼图图片的尺寸。
		/// </summary>
		private Size2F imageSize;
		/// <summary>
		/// 是否存在正在进行的游戏。
		/// </summary>
		private bool hasGame = false;
		/// <summary>
		/// 游戏的启始时间。
		/// </summary>
		private DateTime startTime;
		/// <summary>
		/// 游戏是否发生了变化。
		/// </summary>
		private bool gameChanged = false;

		#endregion // 游戏属性

		/// <summary>
		/// 游戏完成的比例被改变的事件。
		/// </summary>
		public event EventHandler FinishedPercentChanged;
		/// <summary>
		/// 初始化 <see cref="GameManager"/> 类的新实例。
		/// </summary>
		/// <param name="devices">设备管理器。</param>
		public GameManager(DeviceManager devices, JigsawRenderPanel renderPanel, JigsawRendererType rendererType)
		{
			this.devices = devices;
			this.renderPanel = renderPanel;
			// 初始化设备。
			this.renderPanel.Devices = devices;
			this.renderPanel.Paint += this.renderPanel_Paint;
			this.renderTarget = devices.RenderTarget;
			// 初始化画刷。
			selectionRectBrush = new SolidColorBrush(renderTarget, Color.Black);
			selectionRectStyle = new StrokeStyle(this.devices.D2DFactory,
				new StrokeStyleProperties() { DashStyle = DashStyle.Dash });
			this.RendererType = rendererType;
		}

		#region 游戏状态

		/// <summary>
		/// 获取或设置拼图渲染器的类型。
		/// </summary>
		public JigsawRendererType RendererType
		{
			get { return this.rendererType; }
			set
			{
				if (!this.devices.SupportD3D)
				{
					// 不支持 Direct3D，则只创建简单的拼图渲染器。
					value = JigsawRendererType.Simple;
				}
				if (this.rendererType != value || this.renderer == null)
				{
					this.rendererType = value;
					if (this.renderer != null)
					{
						this.renderer.Dispose();
					}
					this.renderer = JigsawRenderer.CreateRenderer(value, this.devices);
					if (this.hasGame)
					{
						this.renderer.PrepareRender(gameInfo.ImageData, pieces, gameInfo.Rotatable, CancellationToken.None);
					}
				}
			}
		}
		/// <summary>
		/// 获取或设置是否显示背景图片。
		/// </summary>
		public bool ShowBackground { get; set; }
		/// <summary>
		/// 获取或设置背景图片的透明度。
		/// </summary>
		public float BackgroundAlpha { get; set; }
		/// <summary>
		/// 获取或设置背景颜色。
		/// </summary>
		public System.Drawing.Color BackgroundColor
		{
			get { return this.backgroundColor.ToColor(); }
			set { this.backgroundColor = value.ToColor4(); }
		}
		/// <summary>
		/// 获取或设置是否只显示边界拼图碎片。
		/// </summary>
		public bool ShowBorder
		{
			get { return this.showBorder; }
			set
			{
				if (this.showBorder != value)
				{
					this.showBorder = value;
					if (this.hasGame)
					{
						this.UpdatePiecesVisible();
						this.InvalidateAll();
					}
				}
			}
		}
		/// <summary>
		/// 获取游戏完成的百分比。
		/// </summary>
		/// <value>游戏完成的百分比。</value>
		public int FinishedPercent
		{
			get
			{
				if (pieces == null)
				{
					return 0;
				}
				else
				{
					return 100 * (gameInfo.PieceSum - pieces.Count) / (gameInfo.PieceSum - 1);
				}
			}
		}
		/// <summary>
		/// 获取当前的游戏用时。
		/// </summary>
		/// <value>当前的游戏用时。</value>
		public TimeSpan UsedTime
		{
			get { return DateTime.Now - startTime; }
		}
		/// <summary>
		/// 获取是否存在正在进行的游戏。
		/// </summary>
		public bool HasGame
		{
			get { return this.hasGame; }
		}
		/// <summary>
		/// 获取游戏是否发生了变化。
		/// </summary>
		public bool GameChanged
		{
			get { return this.gameChanged; }
		}
		/// <summary>
		/// 获取拼图游戏信息。
		/// </summary>
		public JigsawInfo GameInfo
		{
			get { return this.gameInfo; }
		}

		#endregion // 游戏状态

		#region IDisposable 成员

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		public void Dispose()
		{
			if (this.pieces != null)
			{
				this.pieces.Dispose();
			}
			this.renderer.Dispose();
			this.selectionRectBrush.Dispose();
			this.selectionRectStyle.Dispose();
			this.renderTarget.Dispose();
			this.devices.Dispose();
			GC.SuppressFinalize(this);
		}

		#endregion // IDisposable 成员

		#region 拼图游戏

		/// <summary>
		/// 开始一个拼图游戏。
		/// </summary>
		/// <remarks>开始游戏前，需要保证之前的游戏已结束。</remarks>
		/// <param name="jigsawPieces">拼图碎片集合。</param>
		/// <param name="info">游戏信息。</param>
		/// <param name="ct">取消任务的通知。</param>
		/// <returns>开始拼图游戏的任务。</returns>
		public Task StartGame(JigsawPieceCollection jigsawPieces, JigsawInfo info, CancellationToken ct)
		{
			Debug.Assert(!this.hasGame);
			this.pieces = jigsawPieces;
			this.gameInfo = info;
			return new Task(() => this.StartGame(ct), ct);
		}
		/// <summary>
		/// 开始一个拼图游戏。
		/// </summary>
		/// <param name="ct">取消任务的通知。</param>
		private void StartGame(CancellationToken ct)
		{
			this.background = devices.LoadBitmapFromBytes(gameInfo.ImageData);
			this.imageSize = background.Size;
			this.isDraging = this.isSelecting = false;
			// 添加事件侦听器。
			this.renderPanel.JigsawRegionChanged += this.renderPanel_JigsawRegionChanged;
			this.renderPanel.JigsawScaleChanging += this.renderPanel_JigsawScaleChanging;
			// 设置渲染区域。
			this.renderPanel.Invoke(new Action(() =>
			{
				this.renderPanel.ImageSize = imageSize;
				this.renderPanel.SetJigsawScale(this.gameInfo.Scale, new Point(0, 0));
				this.renderPanel.AutoScrollPosition = this.gameInfo.ScrollPosition;
			}));
			if (this.gameInfo.UsedTime.Milliseconds == 0)
			{
				// 对于新游戏，重新分布拼图碎片。
				this.pieces.SpreadPieces(renderPanel.JigsawRegion, this.gameInfo.Rotatable);
			}
			this.UpdatePiecesVisible();
			ct.ThrowIfCancellationRequested();
			this.renderer.PrepareRender(gameInfo.ImageData, pieces, gameInfo.Rotatable, ct);
			ct.ThrowIfCancellationRequested();
			this.hasGame = true;
			this.gameChanged = true;
			this.InvalidateAll();
			this.startTime = DateTime.Now - gameInfo.UsedTime;
			this.UpdateFinishedPercent();
			this.renderPanel.MouseDown += this.renderPanel_MouseDown;
			this.renderPanel.MouseMove += this.renderPanel_MouseMove;
			this.renderPanel.MouseUp += this.renderPanel_MouseUp;
		}
		/// <summary>
		/// 结束拼图游戏。
		/// </summary>
		/// <returns>拼图游戏是否被结束。</returns>
		public void StopGame()
		{
			this.hasGame = false;
			renderPanel.SetJigsawScale(1f, new Point());
			renderPanel.ImageSize = new Size2F();
			// 移除界面的事件侦听器。
			this.renderPanel.JigsawRegionChanged -= this.renderPanel_JigsawRegionChanged;
			this.renderPanel.JigsawScaleChanging -= this.renderPanel_JigsawScaleChanging;
			this.renderPanel.MouseDown -= this.renderPanel_MouseDown;
			this.renderPanel.MouseMove -= this.renderPanel_MouseMove;
			this.renderPanel.MouseUp -= this.renderPanel_MouseUp;
			this.renderer.ClearResources();
			this.pieces.Dispose();
			this.pieces = null;
		}
		/// <summary>
		/// 打开拼图游戏。
		/// </summary>
		/// <param name="fileName">要打开的拼图游戏存档路径。</param>
		/// <param name="ct">取消任务的通知。</param>
		/// <returns>打开拼图游戏的任务。</returns>
		public Task OpenGame(string fileName, CancellationToken ct)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				JigsawSerializeContext context = new JigsawSerializeContext(this.devices);
				BinaryFormatter formatter = new BinaryFormatter(null,
					new StreamingContext(StreamingContextStates.File, context));
				this.gameInfo = (JigsawInfo)formatter.Deserialize(stream);
				this.pieces = (JigsawPieceCollection)formatter.Deserialize(stream);
			}
			this.gameChanged = false;
			return this.StartGame(this.pieces, this.gameInfo, ct);
		}
		/// <summary>
		/// 保存拼图游戏。
		/// </summary>
		/// <param name="fileName">要保存的拼图游戏存档路径。</param>
		public void SaveGame(string fileName)
		{
			// 更新游戏信息。
			gameInfo.ScrollPosition = this.renderPanel.AutoScrollPosition;
			gameInfo.UsedTime = DateTime.Now - startTime;
			using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, this.gameInfo);
				formatter.Serialize(stream, this.pieces);
			}
			this.gameChanged = false;
		}
		/// <summary>
		/// 暂停当前游戏。
		/// </summary>
		public void PauseGame()
		{
			if (gameInfo != null)
			{
				gameInfo.UsedTime = DateTime.Now - startTime;
			}
		}
		/// <summary>
		/// 继续当前游戏。
		/// </summary>
		public void ResumeGame()
		{
			if (gameInfo != null)
			{
				this.startTime = DateTime.Now - gameInfo.UsedTime;
			}
		}
		/// <summary>
		/// 重绘所有的拼图碎片。
		/// </summary>
		public void InvalidateAll()
		{
			renderPanel.Invalidate();
		}
		/// <summary>
		/// 更新拼图碎片的可见性。
		/// </summary>
		private void UpdatePiecesVisible()
		{
			if (this.pieces != null)
			{
				foreach (JigsawPiece piece in this.pieces)
				{
					if (ShowBorder && !piece.Frozen)
					{
						piece.Visible = (piece.PieceType == JigsawPieceType.Border);
					}
					else
					{
						piece.Visible = true;
					}
				}
			}
		}
		/// <summary>
		/// 拼图的区域被改变的事件。
		/// </summary>
		private void renderPanel_JigsawRegionChanged(object sender, EventArgs e)
		{
			this.pieces.CollectInRectangle(this.renderPanel.JigsawRegion);
		}
		/// <summary>
		/// 拼图的缩放比例被改变之前的事件。
		/// </summary>
		private void renderPanel_JigsawScaleChanging(object sender, JigsawScaleChangedEventArgs e)
		{
			if (e.Scale < 0.1f)
			{
				e.Scale = 0.1f;
			}
			else if (e.Scale > 4f)
			{
				e.Scale = 4f;
			}
			float scale = e.Scale;
			// 更新旧的鼠标位置。
			float rate = scale / pieces.Scale;
			lastMousePoint = new Vector2(lastMousePoint.X * rate, lastMousePoint.Y * rate);
			// 更新阴影的偏移。
			if (scale <= 1)
			{
				JigsawEffectRenderer effectRenderer = this.renderer as JigsawEffectRenderer;
				if (effectRenderer != null)
				{
					effectRenderer.ShadowOffset = new Vector2(5 * scale, 5 * scale);
				}
			}
			// 更新缩放比例。
			this.gameInfo.Scale = scale;
			this.pieces.Scale = scale;
		}
		/// <summary>
		/// 渲染控件绘制的事件。
		/// </summary>
		private void renderPanel_Paint(object sender, PaintEventArgs e)
		{
			devices.BeginDraw();
			renderTarget.Clear(this.backgroundColor);
			if (this.hasGame)
			{
				renderTarget.Transform = renderPanel.RenderTargetTansform;
				// 绘制背景图片。
				if (ShowBackground)
				{
					float scale = renderPanel.JigsawScale;
					this.renderTarget.DrawBitmap(this.background,
						new RectangleF(0, 0, imageSize.Width * scale, imageSize.Height * scale),
						BackgroundAlpha, BitmapInterpolationMode.Linear);
				}
				// 渲染拼图碎片。
				renderer.Render(pieces);
				// 绘制选择框。
				if (isSelecting)
				{
					renderTarget.DrawRectangle(selectRect, selectionRectBrush, 1f, selectionRectStyle);
				}
			}
			devices.EndDraw();
		}
		/// <summary>
		/// 更新完成比例。
		/// </summary>
		private void UpdateFinishedPercent()
		{
			if (this.FinishedPercent == 100)
			{
				selectedPieces.Clear();
				pieces.Reset();
			}
			// 完成比例发生了改变。
			EventHandler handler = FinishedPercentChanged;
			if (handler != null)
			{
				handler(this, null);
			}
			if (this.FinishedPercent == 100)
			{
				StopGame();
			}
		}

		#endregion // 拼图游戏

		#region 鼠标事件

		/// <summary>
		/// 鼠标按下。
		/// </summary>
		private void renderPanel_MouseDown(object sender, MouseEventArgs e)
		{
			isSelecting = false;
			if (e.Button == MouseButtons.Left)
			{
				lastMousePoint = renderPanel.PointToJigsaw(e.Location);
				lastPiece = pieces.GetPiece(lastMousePoint);
				if (lastPiece != null && lastPiece.Frozen)
				{
					lastPiece = null;
				}
				if (lastPiece == null)
				{
					selectedPieces.Clear();
					isSelecting = true;
					selectRect = new RectangleF(lastMousePoint.X, lastMousePoint.Y, 0f, 0f);
				}
				else
				{
					renderPanel.Cursor = Cursors.Hand;
					if (!selectedPieces.Contains(lastPiece))
					{
						// 拖动单个拼图碎片
						selectedPieces.Clear();
						selectedPieces.Add(lastPiece);
					}
					StartDrag();
					renderPanel.AutoMouseScroll = true;
				}
			}
			else
			{
				selectedPieces.Clear();
			}
			InvalidateAll();
		}
		/// <summary>
		/// 鼠标弹起。
		/// </summary>
		private void renderPanel_MouseUp(object sender, MouseEventArgs e)
		{
			renderPanel.Cursor = Cursors.Default;
			if (e.Button == MouseButtons.Left)
			{
				if (isDraging)
				{
					// 鼠标弹起，放下拼图碎片。
					StopDrag();
					CheckAnchor();
				}
				else if (isSelecting)
				{
					isSelecting = false;
				}
				// 把拼图碎片放到最底层。
				if (Control.ModifierKeys == Keys.Control)
				{
					if (lastPiece != null)
					{
						pieces.BringToBack(lastPiece);
					}
				}
			}
			else if (e.Button == MouseButtons.Right && gameInfo.Rotatable)
			{
				// 右键旋转
				Vector2 loc = renderPanel.PointToJigsaw(e.Location);
				lastPiece = pieces.GetPiece(loc);
				if (lastPiece != null && !lastPiece.Frozen)
				{
					pieces.BringToFront(lastPiece);
					if (Control.ModifierKeys == Keys.Control)
					{
						lastPiece.Rotate = (lastPiece.Rotate + 270) % 360;
					}
					else
					{
						lastPiece.Rotate = (lastPiece.Rotate + 90) % 360;
					}
					CheckAnchor();
				}
			}
			InvalidateAll();
		}
		/// <summary>
		/// 检查当前拼图碎片能否与其它拼图碎片合并。
		/// </summary>
		private void CheckAnchor()
		{
			int finished = this.FinishedPercent;
			lastPiece = pieces.Merge(lastPiece, gameInfo.AnchorRadius);
			// 移除被合并的拼图碎片。
			this.selectedPieces.IntersectWith(pieces);
			// 吸附到正确的位置。
			if (gameInfo.AnchorToBackground && lastPiece.Rotate == 0)
			{
				float tx = lastPiece.Offset.X;
				float ty = lastPiece.Offset.Y;
				if ((tx * tx + ty * ty) <= gameInfo.AnchorRadius * gameInfo.AnchorRadius)
				{
					lastPiece.Frozen = true;
					lastPiece.Offset = new Vector2();
					pieces.BringToBack(lastPiece);
				}
			}
			pieces.CollectInRectangle(renderPanel.JigsawRegion, this.selectedPieces);
			if (selectedPieces.SingleSelected)
			{
				selectedPieces.Clear();
			}
			renderPanel.AutoMouseScroll = false;
			gameChanged = true;
			if (this.FinishedPercent != finished)
			{
				UpdateFinishedPercent();
			}
		}
		/// <summary>
		/// 鼠标移动。
		/// </summary>
		private void renderPanel_MouseMove(object sender, MouseEventArgs e)
		{
			Vector2 loc = renderPanel.PointToJigsaw(e.Location);
			if (isDraging)
			{
				// 拖动被选中的拼图碎片
				Drag(loc);
				lastMousePoint = loc;
			}
			else if (isSelecting)
			{
				// 选择图片。
				if (lastMousePoint.X < loc.X)
				{
					selectRect.Left = lastMousePoint.X;
					selectRect.Right = loc.X;
				}
				else
				{
					selectRect.Left = loc.X;
					selectRect.Right = lastMousePoint.X;
				}
				if (lastMousePoint.Y < loc.Y)
				{
					selectRect.Top = lastMousePoint.Y;
					selectRect.Bottom = loc.Y;
				}
				else
				{
					selectRect.Top = loc.Y;
					selectRect.Bottom = lastMousePoint.Y;
				}
				selectedPieces.Clear();
				selectedPieces.AddRange(pieces.GetPiece(selectRect));
			}
			else
			{
				// 鼠标经过时高亮拼图碎片
				if (lastPiece != null)
				{
					lastPiece.State &= ~JigsawPieceState.Highlight;
				}
				lastPiece = pieces.GetPiece(loc);
				if (lastPiece != null && !lastPiece.Frozen)
				{
					lastPiece.State |= JigsawPieceState.Highlight;
				}
			}
			InvalidateAll();
		}

		#endregion // 鼠标事件

		#region 拼图碎片拖动

		/// <summary>
		/// 开始拖动被选中的拼图碎片。
		/// </summary>
		private void StartDrag()
		{
			if (isDraging)
			{
				StopDrag();
			}
			isDraging = true;
			pieces.BringToFront(selectedPieces);
			foreach (JigsawPiece piece in selectedPieces)
			{
				piece.State |= JigsawPieceState.Draging;
			}
			InvalidateAll();
		}
		/// <summary>
		/// 将正在拖动的拼图碎片拖动到指定坐标。
		/// </summary>
		/// <param name="location">鼠标的坐标。</param>
		private void Drag(Vector2 location)
		{
			if (isDraging)
			{
				float ox = location.X - lastMousePoint.X;
				float oy = location.Y - lastMousePoint.Y;
				foreach (JigsawPiece piece in selectedPieces)
				{
					piece.Offset = new Vector2(piece.Offset.X + ox, piece.Offset.Y + oy);
				}
				InvalidateAll();
			}
		}
		/// <summary>
		/// 停止拖动拼图碎片。
		/// </summary>
		private void StopDrag()
		{
			isDraging = false;
			foreach (JigsawPiece piece in selectedPieces)
			{
				piece.State &= ~JigsawPieceState.Draging;
			}
			pieces.CollectInRectangle(renderPanel.JigsawRegion, selectedPieces);
			InvalidateAll();
		}

		#endregion

	}
}
