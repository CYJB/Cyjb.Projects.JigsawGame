using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 缩略图窗口。
	/// </summary>
	public partial class ThumbForm : Form
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

		#region 消息循环

		/// <summary>
		/// 设置活动窗口。
		/// </summary>
		/// <param name="handle">活动窗口的句柄。</param>
		/// <returns>返回值。</returns>
		[DllImport("user32.dll")]
		private extern static IntPtr SetActiveWindow(IntPtr handle);
		/// <summary>
		/// WM_ACTIVATE 消息。
		/// </summary>
		private const int WM_ACTIVATE = 0x006;
		/// <summary>
		/// WM_ACTIVATEAPP 消息。
		/// </summary>
		private const int WM_ACTIVATEAPP = 0x01C;
		/// <summary>
		/// WM_NCACTIVATE 消息。
		/// </summary>
		private const int WM_NCACTIVATE = 0x086;
		/// <summary>
		/// WA_INACTIVE 消息。
		/// </summary>
		private const int WA_INACTIVE = 0;
		/// <summary>
		/// WM_MOUSEACTIVATE 消息。
		/// </summary>
		private const int WM_MOUSEACTIVATE = 0x21;
		/// <summary>
		/// MA_NOACTIVATE 消息。
		/// </summary>
		private const int MA_NOACTIVATE = 3;
		/// <summary>
		/// 窗口消息循环重载，用于使缩略图窗口不接受焦点。
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			// 鼠标左键按下
			if (m.Msg == 0x0201)
			{
				// 当不能拖动图片时。
				if (this.currentScale == this.minScale)
				{
					// 改消息为非客户区按下鼠标
					m.Msg = 0x00a1;
					// 默认值
					m.LParam = IntPtr.Zero;
					// 鼠标放在标题栏内
					m.WParam = new IntPtr(2);
				}
			}
			else if (m.Msg == WM_MOUSEACTIVATE)
			{
				m.Result = new IntPtr(MA_NOACTIVATE);
				return;
			}
			else if (m.Msg == WM_NCACTIVATE)
			{
				if (((int)m.WParam & 0xFFFF) != WA_INACTIVE)
				{
					if (m.LParam != IntPtr.Zero)
					{
						SetActiveWindow(m.LParam);
					}
					else
					{
						SetActiveWindow(IntPtr.Zero);
					}
				}
			}
			base.WndProc(ref m);
		}

		#endregion // 消息循环

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
			if (scale < minScale)
			{
				scale = minScale;
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
