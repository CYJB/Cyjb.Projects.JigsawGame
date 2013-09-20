using System;
using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] fileNames)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.ThreadException += (sender, e) =>
			{
				new BugReportForm(e.Exception).ShowDialog();
				Application.Exit();
			};
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				new BugReportForm(e.ExceptionObject as Exception).ShowDialog();
			};
			DeviceManager device = new DeviceManager();
			if (!device.SupportD2D)
			{
				MessageBox.Show("您的操作系统不支持 Direct2D 特性，不能运行此游戏");
				return;
			}
			Application.Run(new MainForm(device, fileNames));
		}
	}
}
