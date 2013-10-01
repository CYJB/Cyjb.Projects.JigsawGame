using System;
using System.ComponentModel;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Windows;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Cyjb.Projects.JigsawGame.Renderer
{
	/// <summary>
	/// 用于渲染拼图游戏的控件。
	/// </summary>
	public sealed class JigsawRenderPanel : RenderControl
	{
		/// <summary>
		/// 设备管理器。
		/// </summary>
		private DeviceManager devices;
		/// <summary>
		/// 自动滚动的定时器。
		/// </summary>
		private Timer autoScrollTimer;
		/// <summary>
		/// 是否自动根据鼠标位置滚动。
		/// </summary>
		private bool autoMouseScroll;
		/// <summary>
		/// 拼图自动滚动的时间间隔。
		/// </summary>
		private int autoMouseScrollInterval = 16;
		/// <summary>
		/// 拼图自动滚动的最小距离。
		/// </summary>
		private int autoMouseScrollMargin = 20;
		/// <summary>
		/// 拼图图片至边距的最小距离。
		/// </summary>
		private float jigsawMargin = 100f;
		/// <summary>
		/// 拼图游戏的矩形范围。
		/// </summary>
		private RectangleF jigsawRegion;
		/// <summary>
		/// 拼图图片的尺寸。
		/// </summary>
		private Size2F imageSize;
		/// <summary>
		/// 拼图游戏的缩放比例。
		/// </summary>
		private float jigsawScale = 1f;
		/// <summary>
		/// 拼图游戏的矩形范围被改变的事件。
		/// </summary>
		public event EventHandler JigsawRegionChanged;
		/// <summary>
		/// 拼图游戏的缩放比例被改变之前的事件。
		/// </summary>
		public event EventHandler<JigsawScaleChangedEventArgs> JigsawScaleChanging;
		/// <summary>
		/// 拼图游戏的缩放比例被改变的事件。
		/// </summary>
		public event EventHandler JigsawScaleChanged;
		/// <summary>
		/// 获取或设置设备管理器。
		/// </summary>
		public DeviceManager Devices
		{
			get { return this.devices; }
			set
			{
				this.devices = value;
				devices.CreateRenderTarget(this);
			}
		}
		/// <summary>
		/// 获取或设置拼图图片至边距的最小距离。
		/// </summary>
		[Description("拼图图片至边距的最小距离。")]
		[Category("Layout")]
		[DefaultValue(100f)]
		public float JigsawMargin
		{
			get { return this.jigsawMargin; }
			set
			{
				this.jigsawMargin = value;
				this.UpdateSize();
			}
		}
		/// <summary>
		/// 获取拼图游戏的矩形范围。
		/// </summary>
		[Browsable(false)]
		public RectangleF JigsawRegion
		{
			get { return this.jigsawRegion; }
		}
		/// <summary>
		/// 获取或设置拼图图片的尺寸。
		/// </summary>
		[Browsable(false)]
		public Size2F ImageSize
		{
			get { return this.imageSize; }
			set
			{
				this.imageSize = value;
				this.UpdateSize();
			}
		}
		/// <summary>
		/// 获取拼图游戏的缩放比例。
		/// </summary>
		[Browsable(false)]
		public float JigsawScale
		{
			get { return this.jigsawScale; }
		}
		/// <summary>
		/// 获取渲染区域的转换矩阵。
		/// </summary>
		[Browsable(false)]
		public Matrix3x2 RenderTargetTansform
		{
			get
			{
				return Matrix3x2.Translation(this.AutoScrollPosition.X - jigsawRegion.X,
					this.AutoScrollPosition.Y - jigsawRegion.Y);
			}
		}

		#region 尺寸变化

		/// <summary>
		/// 设置拼图的缩放比例。
		/// </summary>
		/// <param name="scale">缩放比例。</param>
		/// <param name="point">缩放的中心点，是当前的控件坐标。</param>
		public void SetJigsawScale(float scale, Point center)
		{
			EventHandler<JigsawScaleChangedEventArgs> scaleChanging = this.JigsawScaleChanging;
			if (scaleChanging != null)
			{
				JigsawScaleChangedEventArgs eventArgs = new JigsawScaleChangedEventArgs(scale);
				scaleChanging(this, eventArgs);
				scale = eventArgs.Scale;
			}
			if (this.jigsawScale == scale)
			{
				return;
			}
			this.jigsawScale = scale;
			// 更新尺寸的同时移动滚动条，使缩放的中心点尽可能不变。
			float oldWidth = jigsawRegion.Width;
			Point scroll = new Point(this.HorizontalScroll.Value + center.X,
				this.VerticalScroll.Value + center.Y);
			UpdateSize();
			// 这里强制重绘一次，防止出现闪烁。
			this.Invalidate();
			float rate = jigsawRegion.Width / oldWidth;
			this.AutoScrollPosition = new Point((int)(scroll.X * rate - center.X), (int)(scroll.Y * rate - center.Y));
			EventHandler scaleChanged = this.JigsawScaleChanged;
			if (scaleChanged != null)
			{
				scaleChanged(this, null);
			}
		}
		/// <summary>
		/// 缩放和滚动拼图，使得图片恰好居中显示在界面中。
		/// </summary>
		public void ShowImage()
		{
			float sw = this.ClientSize.Width / imageSize.Width;
			float sh = this.ClientSize.Height / imageSize.Height;
			float scale = 1f;
			if (sw < sh)
			{
				scale = sw;
				sw = this.jigsawMargin * scale;
				sh = -(this.ClientSize.Height - imageSize.Height * scale) / 2 + this.jigsawMargin * scale;
			}
			else
			{
				scale = sh;
				sw = -(this.ClientSize.Width - imageSize.Width * scale) / 2 + this.jigsawMargin * scale;
				sh = this.jigsawMargin * scale;
			}
			this.SetJigsawScale(scale, new Point());
			this.AutoScrollPosition = new Point((int)sw, (int)sh);
		}
		/// <summary>
		/// 控件尺寸改变的事件。
		/// </summary>
		protected override void OnSizeChanged(EventArgs e)
		{
			if (devices != null)
			{
				devices.ResizeRenderTarget(this);
				this.UpdateSize();
			}
			base.OnSizeChanged(e);
		}
		/// <summary>
		/// 更新拼图的尺寸。
		/// </summary>
		private void UpdateSize()
		{
			jigsawRegion = new RectangleF(-jigsawMargin * jigsawScale, -jigsawMargin * jigsawScale,
				(imageSize.Width + 2 * jigsawMargin) * jigsawScale, (imageSize.Height + 2 * jigsawMargin) * jigsawScale);
			Size autoScrollMinSize = new Size(0, 0);
			if (jigsawRegion.Width < this.ClientSize.Width)
			{
				jigsawRegion.Left = -(this.ClientSize.Width - imageSize.Width * this.jigsawScale) / 2;
				jigsawRegion.Right = jigsawRegion.Left + this.ClientSize.Width;
			}
			else
			{
				autoScrollMinSize.Width = (int)jigsawRegion.Width;
			}
			if (jigsawRegion.Height < this.ClientSize.Height)
			{
				jigsawRegion.Top = -(this.ClientSize.Height - imageSize.Height * this.jigsawScale) / 2;
				jigsawRegion.Bottom = jigsawRegion.Top + this.ClientSize.Height;
			}
			else
			{
				autoScrollMinSize.Height = (int)jigsawRegion.Height;
			}
			EventHandler jigsawRegionChanged = this.JigsawRegionChanged;
			if (jigsawRegionChanged != null)
			{
				jigsawRegionChanged(this, null);
			}
			this.AutoScrollMinSize = autoScrollMinSize;
		}

		#endregion // 尺寸变化

		#region 坐标映射

		/// <summary>
		/// 将指定渲染区域的坐标转换为拼图的坐标。
		/// </summary>
		/// <param name="location">渲染区域的坐标。</param>
		/// <returns>拼图的坐标。</returns>
		public Vector2 PointToJigsaw(Point location)
		{
			return new Vector2(location.X - this.AutoScrollPosition.X + this.jigsawRegion.X,
				location.Y - this.AutoScrollPosition.Y + this.jigsawRegion.Y);
		}
		/// <summary>
		/// 将指定的拼图坐标转换为渲染区域的坐标。
		/// </summary>
		/// <param name="location">拼图坐标。</param>
		/// <returns>渲染区域的坐标。</returns>
		public Point PointToClient(Vector2 location)
		{
			return new Point((int)(location.X + this.AutoScrollPosition.X - this.jigsawRegion.X),
				(int)(location.Y + this.AutoScrollPosition.Y - this.jigsawRegion.Y));
		}

		#endregion // 坐标映射

		/// <summary>
		/// 鼠标滚轮滚动的事件。
		/// </summary>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (Control.ModifierKeys == Keys.Shift)
			{
				if (this.HorizontalScroll.Visible)
				{
					this.AutoScrollPosition = new Point(this.HorizontalScroll.Value - e.Delta / 4,
						this.VerticalScroll.Value);
				}
			}
			else if (Control.ModifierKeys == Keys.Control)
			{
				this.SetJigsawScale(this.jigsawScale + (float)e.Delta / 1200, e.Location);
			}
			else
			{
				if (this.VerticalScroll.Visible)
				{
					this.AutoScrollPosition = new Point(this.HorizontalScroll.Value,
						this.VerticalScroll.Value - e.Delta / 4);
				}
			}
			// 屏蔽父控件的鼠标滚轮事件，也不再引发 MouseWheel 事件。
		}

		#region 自动滚动

		/// <summary>
		/// 获取或设置拼图自动滚动的最小距离。
		/// </summary>
		[Description("拼图自动滚动的最小距离。")]
		[Category("Layout")]
		[DefaultValue(20)]
		public int AutoMouseScrollMargin
		{
			get { return this.autoMouseScrollMargin; }
			set { this.autoMouseScrollMargin = value; }
		}
		/// <summary>
		/// 获取或设置拼图自动滚动的时间间隔。
		/// </summary>
		[Description("拼图自动滚动的时间间隔。")]
		[Category("Behavior")]
		[DefaultValue(16)]
		public int AutoMouseScrollInterval
		{
			get { return this.autoMouseScrollInterval; }
			set
			{
				this.autoMouseScrollInterval = value;
				if (this.autoScrollTimer != null)
				{
					this.autoScrollTimer.Interval = value;
				}
			}
		}
		/// <summary>
		/// 获取或设置当前控件是否根据鼠标坐标自动滚动。
		/// </summary>
		[Description("当前控件是否根据鼠标坐标自动滚动。")]
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool AutoMouseScroll
		{
			get { return this.autoMouseScroll; }
			set
			{
				this.autoMouseScroll = value;
				if (this.autoScrollTimer == null)
				{
					this.autoScrollTimer = new Timer();
					this.autoScrollTimer.Interval = autoMouseScrollInterval;
					this.autoScrollTimer.Tick += this.UpdateAutoScrollPosition;
				}
				if (value)
				{
					this.autoScrollTimer.Start();
				}
				else
				{
					this.autoScrollTimer.Stop();
				}
			}
		}
		/// <summary>
		/// 更新自动滚动的位置。
		/// </summary>
		private void UpdateAutoScrollPosition(object sender, EventArgs e)
		{
			if (this.HorizontalScroll.Visible || this.VerticalScroll.Visible)
			{
				Point point = this.PointToClient(Control.MousePosition);
				int tx = 0, ty = 0;
				int tw = this.ClientSize.Width - autoMouseScrollMargin;
				int th = this.ClientSize.Height - autoMouseScrollMargin;
				if (point.X <= autoMouseScrollMargin)
				{
					tx = point.X - autoMouseScrollMargin;
				}
				else if (point.X >= tw)
				{
					tx = point.X - tw;
				}
				if (point.Y <= autoMouseScrollMargin)
				{
					ty = point.Y - autoMouseScrollMargin;
				}
				else if (point.Y >= th)
				{
					ty = point.Y - th;
				}
				if (tx != 0 || ty != 0)
				{
					this.AutoScrollPosition = new Point(this.HorizontalScroll.Value + tx,
						this.VerticalScroll.Value + ty);
					this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 1, point.X, point.Y, 0));
				}
			}
		}

		#endregion // 自动滚动

	}
}
