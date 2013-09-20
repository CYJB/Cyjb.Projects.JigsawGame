using System.Diagnostics;
using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 帮助窗口。
	/// </summary>
	public partial class HelpForm : Form
	{
		/// <summary>
		/// 构造函数。
		/// </summary>
		public HelpForm()
		{
			InitializeComponent();
		}
		/// <summary>
		/// 打开链接的事件。
		/// </summary>
		private void pbxLink_Click(object sender, System.EventArgs e)
		{
			Process.Start("http://www.cnblogs.com/cyjb/");
		}
		/// <summary>
		/// 打开协议的事件。
		/// </summary>
		private void pbxLicense_Click(object sender, System.EventArgs e)
		{
			Process.Start("http://creativecommons.org/licenses/by-nc-nd/3.0/cn/");
		}
		/// <summary>
		/// 打开帮助链接的事件。
		/// </summary>
		private void pbxHelpLink_Click(object sender, System.EventArgs e)
		{
			Process.Start("http://www.cnblogs.com/cyjb/p/JigsawGame.html");
		}
	}
}
