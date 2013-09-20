using System;
using System.Threading;
using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 提供在 UI 线程进行大量计算时显示特殊的等待窗体的功能。
	/// </summary>
	public sealed class WaitForm : IDisposable
	{
		/// <summary>
		/// 额外的线程。
		/// </summary>
		private Thread thread;
		/// <summary>
		/// 运行窗体的对象。
		/// </summary>
		private FormRunner runner;
		/// <summary>
		/// 创建一个新的等待窗体。
		/// </summary>
		public WaitForm()
			: this(null)
		{ }
		/// <summary>
		/// 创建一个新的等待窗体。
		/// </summary>
		/// <param name="form">等待时使用的窗体。</param>
		public WaitForm(Form form)
		{
			this.Form = form;
		}

		#region IDisposable 成员

		/// <summary>
		/// 执行与释放或重置非托管资源相关的应用程序定义的任务。
		/// </summary>
		public void Dispose()
		{
			Hide();
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// 获取或设置等待时使用的窗体。
		/// </summary>
		public Form Form { get; set; }
		/// <summary>
		/// 显示等待窗体。
		/// </summary>
		public void Show()
		{
			if (thread == null && Form != null && !Form.IsDisposed && !Form.Visible)
			{
				runner = new FormRunner();
				runner.Form = Form;
				thread = new Thread(new ThreadStart(runner.Show));
				thread.Start();
			}
		}
		/// <summary>
		/// 隐藏等待窗体。
		/// </summary>
		public void Hide()
		{
			if (thread != null)
			{
				if (runner != null)
				{
					runner.Hide();
				}
				else
				{
					runner = null;
				}
				thread = null;
			}
		}
		/// <summary>
		/// 运行等待窗体的类。
		/// </summary>
		private class FormRunner
		{
			/// <summary>
			/// 是否要求当前线程停止。
			/// </summary>
			private bool CancellationRequest = false;
			/// <summary>
			/// 是否已经调用了 ShowDialog。
			/// </summary>
			private bool CalledShowDialog = false;
			/// <summary>
			/// 显示等待窗体。
			/// </summary>
			public void Show()
			{
				if (CancellationRequest)
				{
					return;
				}
				if (Form != null && !Form.IsDisposed && !Form.Visible)
				{
					CalledShowDialog = true;
					Form.ShowDialog();
				}
			}
			/// <summary>
			/// 关闭等待窗体。
			/// </summary>
			public void Hide()
			{
				if (Form != null && !Form.IsDisposed && CalledShowDialog)
				{
					// 已经 ShowDialog，需要等待窗体句柄创建。
					while (!Form.IsHandleCreated)
					{
						Thread.Sleep(10);
					}
					Form.Invoke(new Action(Form.Hide));
					CalledShowDialog = false;
				}
				else
				{
					CancellationRequest = true;
				}
			}
			/// <summary>
			/// 获取或设置要显示的等待窗体。
			/// </summary>
			public Form Form { get; set; }
		}

	}
}
