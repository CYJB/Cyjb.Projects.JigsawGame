using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 报告异常的窗体。
	/// </summary>
	public partial class BugReportForm : Form
	{
		/// <summary>
		/// 初始化 <see cref="BugReportForm"/> 类的新实例。
		/// </summary>
		/// <param name="ex">异常对象。</param>
		public BugReportForm(Exception ex)
		{
			InitializeComponent();
			if (ex == null)
			{
				tbxException.Text = "无异常信息。";
			}
			else
			{
				StringBuilder text = new StringBuilder();
				FormatException(ex, text);
				tbxException.Text = text.ToString();
			}
		}
		/// <summary>
		/// 格式化异常。
		/// </summary>
		/// <param name="ex">要格式化的异常对象。</param>
		/// <param name="text">格式化后的文本。</param>
		private void FormatException(Exception ex, StringBuilder text)
		{
			text.Append(ex.GetType());
			text.Append(": ");
			text.AppendLine(ex.Message);
			text.AppendLine(ex.StackTrace);
			if (ex.InnerException != null)
			{
				text.AppendLine();
				text.AppendLine("InnerException:");
				FormatException(ex.InnerException, text);
			}
		}
		/// <summary>
		/// 报告异常的链接。
		/// </summary>
		private void linkReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.cnblogs.com/cyjb/p/JigsawGame.html");
		}
	}
}
