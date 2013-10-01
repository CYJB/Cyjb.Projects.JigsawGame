using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 缩略图窗口。
	/// </summary>
	public partial class ThumbForm : ToolForm
	{
		/// <summary>
		/// 显示的缩略图。
		/// </summary>
		private Image image;
		/// <summary>
		/// 图片的缩放比例。
		/// </summary>
		private float imageScale;
		/// <summary>
		/// 最小的可用缩放比例。
		/// </summary>
		private float minScale;
		/// <summary>
		/// 图片的当前缩放比例。
		/// </summary>
		private float currentScale;
		/// <summary>
		/// 上一次的鼠标位置。
		/// </summary>
		private Point lastMouseLocation;
		/// <summary>
		/// 图片的 X 坐标。
		/// </summary>
		private float imageX;
		/// <summary>
		/// 图片的 Y 坐标。
		/// </summary>
		private float imageY;
		/// <summary>
		/// 构造函数。
		/// </summary>
		public ThumbForm()
		{
			InitializeComponent();
		}
		/// <summary>
		/// 获取或设置显示的缩略图。
		/// </summary>
		public Image Image
		{
			get { return this.image; }
			set
			{
				this.image = value;
				if (this.image != null)
				{
					minScale = Math.Min((float)this.ClientRectangle.Width / image.Width,
						(float)this.ClientRectangle.Height / image.Height);
					imageScale = float.NegativeInfinity;
					imageX = imageY = 0f;
				}
				this.Invalidate();
			}
		}

		#region 控件事件

		/// <summary>
		/// 鼠标按下的事件。
		/// </summary>
		private void ThumbForm_MouseDown(object sender, MouseEventArgs e)
		{
			lastMouseLocation = e.Location;
		}
		/// <summary>
		/// 鼠标移动的事件。
		/// </summary>
		private void ThumbForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.image == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left)
			{
				imageX -= (e.Location.X - lastMouseLocation.X) / this.currentScale;
				imageY -= (e.Location.Y - lastMouseLocation.Y) / this.currentScale;
				this.Invalidate();
			}
			else if (e.Button == MouseButtons.Right)
			{
				float rate = this.currentScale;
				this.imageScale = this.currentScale + (e.Location.X - lastMouseLocation.X) / 200.0f;
				rate = this.imageScale / rate - 1;
				this.imageX += this.ClientRectangle.Width / 2 * rate;
				this.imageY += this.ClientRectangle.Height / 2 * rate;
				this.Invalidate();
			}
			lastMouseLocation = e.Location;
		}
		/// <summary>
		/// 绘制窗体的事件。
		/// </summary>
		private void ThumbForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(JigsawSetting.Default.BackgroundColor);
			if (image == null)
			{
				return;
			}
			float scale = imageScale;
			this.ClientDraggable = false;
			if (scale < minScale)
			{
				scale = minScale;
				this.ClientDraggable = true;
			}
			else if (scale > 4f)
			{
				scale = 4f;
			}
			currentScale = scale;
			float iw = this.ClientRectangle.Width / scale;
			float ih = this.ClientRectangle.Height / scale;
			if (imageX < 0)
			{
				imageX = 0;
			}
			if (imageX > image.Width - iw)
			{
				imageX = image.Width - iw;
				if (imageX < 0)
				{
					// 令图片居中对齐。
					imageX /= 2;
				}
			}
			if (imageY < 0)
			{
				imageY = 0;
			}
			if (imageY > image.Height - ih)
			{
				imageY = image.Height - ih;
				if (imageY < 0)
				{
					// 令图片居中对齐。
					imageY /= 2;
				}
			}
			RectangleF imageRect = new RectangleF(imageX, imageY, iw, ih);
			e.Graphics.DrawImage(image, this.ClientRectangle, imageRect, GraphicsUnit.Pixel);
		}
		/// <summary>
		/// 窗体大小被改变的事件。
		/// </summary>
		private void ThumbForm_SizeChanged(object sender, EventArgs e)
		{
			if (this.image == null)
			{
				return;
			}
			minScale = Math.Min((float)this.ClientRectangle.Width / image.Width,
				(float)this.ClientRectangle.Height / image.Height);
			this.Invalidate();
		}

		#endregion //控件事件

	}
}
