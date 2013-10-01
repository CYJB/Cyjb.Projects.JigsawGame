using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 工具窗体的基类。
	/// </summary>
	public partial class ToolForm : Form
	{
		/// <summary>
		/// 初始化 <see cref="ToolForm"/> 类的新实例。
		/// </summary>
		public ToolForm()
		{
			InitializeComponent();
		}
		/// <summary>
		/// 获取或设置是否可以在工作区拖动窗体。
		/// </summary>
		protected bool ClientDraggable { get; set; }

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
				// 当可以在工作区拖动窗体时。
				if (this.ClientDraggable)
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

	}
}
