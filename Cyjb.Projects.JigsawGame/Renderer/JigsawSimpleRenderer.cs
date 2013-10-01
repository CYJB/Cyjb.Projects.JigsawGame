using System.Threading;
using Cyjb.Projects.JigsawGame.Jigsaw;
using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Renderer
{
	/// <summary>
	/// 拼图的简单渲染器。
	/// </summary>
	public sealed class JigsawSimpleRenderer : JigsawRenderer
	{
		/// <summary>
		/// 绘制拼图碎片的笔刷。
		/// </summary>
		private BitmapBrush brush;
		/// <summary>
		/// 普通边界的笔刷。
		/// </summary>
		private Color normalColor = Color.Black;
		/// <summary>
		/// 高亮边界的笔刷。
		/// </summary>
		private Color highlightColor = Color.Red;
		/// <summary>
		/// 被选择的边界的笔刷。
		/// </summary>
		private Color selectedColor = Color.Red;
		/// <summary>
		/// 普通边界的笔刷。
		/// </summary>
		private SolidColorBrush normalBrush;
		/// <summary>
		/// 高亮边界的笔刷。
		/// </summary>
		private SolidColorBrush highlightBrush;
		/// <summary>
		/// 被选择时的画笔。
		/// </summary>
		private SolidColorBrush selectedBrush;
		/// <summary>
		/// 使用指定的设备管理器初始化 <see cref="JigsawRenderer"/> 类的新实例。
		/// </summary>
		/// <param name="deviceManager">设备管理器。</param>
		public JigsawSimpleRenderer(DeviceManager deviceManager)
			: base(deviceManager)
		{
			InitBrushes();
		}
		/// <summary>
		/// 初始化笔刷。
		/// </summary>
		private void InitBrushes()
		{
			this.normalBrush = new SolidColorBrush(this.RenderTarget, normalColor);
			this.highlightBrush = new SolidColorBrush(this.RenderTarget, highlightColor);
			this.selectedBrush = new SolidColorBrush(this.RenderTarget, selectedColor);
		}

		#region IDisposable 成员

		/// <summary>
		/// 释放对象占用的资源。
		/// </summary>
		/// <param name="disposing">是否释放托管资源。</param>
		protected override void Dispose(bool disposing)
		{
			this.normalBrush.Dispose();
			this.highlightBrush.Dispose();
			this.selectedBrush.Dispose();
			if (this.brush != null)
			{
				this.brush.Dispose();
			}
			base.Dispose(true);
		}

		#endregion // IDisposable 成员

		#region 渲染器属性

		/// <summary>
		/// 获取拼图渲染器的类型。
		/// </summary>
		public override JigsawRendererType RendererType { get { return JigsawRendererType.Simple; } }
		/// <summary>
		/// 获取或设置普通状态的拼图边框颜色。
		/// </summary>
		public Color NormalBorderColor
		{
			get { return normalColor; }
			set
			{
				normalColor = value;
				normalBrush.Color = value;
			}
		}
		/// <summary>
		/// 获取或设置高亮状态的拼图边框颜色。
		/// </summary>
		public Color HighlightBorderColor
		{
			get { return highlightColor; }
			set
			{
				highlightColor = value;
				highlightBrush.Color = value;
			}
		}
		/// <summary>
		/// 获取或设置被选择的状态的拼图边框颜色。
		/// </summary>
		public Color SelectedBorderColor
		{
			get { return selectedColor; }
			set
			{
				selectedColor = value;
				selectedBrush.Color = value;
			}
		}

		#endregion // 渲染器属性

		/// <summary>
		/// 准备渲染拼图碎片。
		/// </summary>
		/// <param name="imageData">拼图使用的图片数据。</param>
		/// <param name="pieces">所有拼图碎片的集合。</param>
		/// <param name="rotatable">拼图碎片是否可以旋转。</param>
		/// <param name="ct">取消任务的通知。</param>
		public override void PrepareRender(byte[] imageData, JigsawPieceCollection pieces, bool rotatable,
			CancellationToken ct)
		{
			base.PrepareRender(imageData, pieces, rotatable, ct);
			if (this.brush == null)
			{
				this.brush = new BitmapBrush(this.RenderTarget, this.Image);
			}
			else
			{
				this.brush.Bitmap = this.Image;
			}
		}
		/// <summary>
		/// 清除渲染使用的资源。
		/// </summary>
		public override void ClearResources()
		{
			base.ClearResources();
			if (this.brush != null)
			{
				this.brush.Dispose();
				this.brush = null;
			}
		}
		/// <summary>
		/// 渲染拼图碎片。
		/// </summary>
		protected override void Render()
		{
			int idx = CurrentPieces.Count - 1;
			for (; idx >= 0; idx--)
			{
				JigsawPiece piece = CurrentPieces[idx];
				if (!piece.Visible)
				{
					continue;
				}
				brush.Transform = piece.TransformMatrix;
				this.RenderTarget.FillGeometry(piece.Path, brush);
				Brush lineBrush = normalBrush;
				if ((piece.State & JigsawPieceState.Selected) == JigsawPieceState.Selected)
				{
					lineBrush = selectedBrush;
				}
				else if ((piece.State & JigsawPieceState.Highlight) == JigsawPieceState.Highlight)
				{
					lineBrush = highlightBrush;
				}
				this.RenderTarget.DrawGeometry(piece.Path, lineBrush, 1);
			}
		}
	}
}
