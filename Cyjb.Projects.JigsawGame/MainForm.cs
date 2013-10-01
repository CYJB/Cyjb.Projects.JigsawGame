using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cyjb.Projects.JigsawGame.Jigsaw;
using Cyjb.Projects.JigsawGame.Renderer;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 拼图游戏主窗口。
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// 拼图可用的缩放比例列表。
		/// </summary>
		private static float[] ScaleSizes = new float[] { 0.1f, 0.25f, 0.5f, 0.75f, 1f, 1.25f, 1.5f, 2f, 3f, 4f };
		/// <summary>
		/// 设备管理器。
		/// </summary>
		private DeviceManager deviceManager;
		/// <summary>
		/// 游戏管理器。
		/// </summary>
		private GameManager gameManager;
		/// <summary>
		/// 当前游戏的保存路径。
		/// </summary>
		private string gamePath;
		/// <summary>
		/// 拼图的缩略图片。
		/// </summary>
		private Bitmap thumb;
		/// <summary>
		/// 缩略图窗口。
		/// </summary>
		private ThumbForm thumbForm;
		/// <summary>
		/// 等待窗体。
		/// </summary>
		private LoadingForm loadingForm;
		/// <summary>
		/// 取消任务的通知。
		/// </summary>
		private CancellationTokenSource tokenCancelSource;
		/// <summary>
		/// 初始化主窗体。
		/// </summary>
		/// <param name="devices">设备管理器。</param>
		public MainForm(DeviceManager devices)
		{
			this.Location = JigsawSetting.Default.MainLocation;
			InitializeComponent();
			this.deviceManager = devices;
			this.ClientSize = JigsawSetting.Default.MainSize;
			this.WindowState = JigsawSetting.Default.MainState;
			this.gameManager = new GameManager(devices, this.renderPanel,
				EnumExt.Parse<JigsawRendererType>(JigsawSetting.Default.Renderer));
			InitGameManager();
			InitToolStrip();
			InitScaleMenu();
		}
		/// <summary>
		/// 使用要打开的文件初始化主窗体。
		/// </summary>
		/// <param name="fileNames">要打开的文件。</param>
		/// <param name="device">设备管理器。</param>
		public MainForm(DeviceManager device, string[] fileNames)
			: this(device)
		{
			if (fileNames.Length > 0)
			{
				if (File.Exists(fileNames[0]))
				{
					this.OpenGame(fileNames[0]);
				}
			}
		}

		#region 初始化

		/// <summary>
		/// 初始化游戏管理器。
		/// </summary>
		private void InitGameManager()
		{
			this.gameManager.FinishedPercentChanged += UpdateFinishedPercent;
			this.gameManager.ShowBackground = JigsawSetting.Default.ShowBackground;
			this.gameManager.BackgroundColor = JigsawSetting.Default.BackgroundColor;
			this.gameManager.BackgroundAlpha = JigsawSetting.Default.BackgroundAlpha;
			this.gameManager.ShowBorder = JigsawSetting.Default.ShowBorder;
		}
		/// <summary>
		/// 初始化缩放菜单。
		/// </summary>
		private void InitScaleMenu()
		{
			for (int i = 0; i < ScaleSizes.Length; i++)
			{
				ToolStripMenuItem item = new ToolStripMenuItem(
					(ScaleSizes[i] * 100).ToString(CultureInfo.CurrentCulture) + "%");
				item.Tag = ScaleSizes[i];
				if (ScaleSizes[i] == 1f)
				{
					item.Checked = true;
				}
				scaleSTBtn.DropDown.Items.Add(item);
			}
		}
		/// <summary>
		/// 初始化工具条。
		/// </summary>
		private void InitToolStrip()
		{
			if (JigsawSetting.Default.ShowThumb)
			{
				showThumbTSBtn_Click(this, null);
			}
			showBackgroundSTBtn.Checked = JigsawSetting.Default.ShowBackground;
			showBorderSTBtn.Checked = JigsawSetting.Default.ShowBorder;
			this.ResetToolStrip();
		}
		/// <summary>
		/// 重置工具条。
		/// </summary>
		private void ResetToolStrip()
		{
			saveGameTSBtn.Enabled = scaleSTBtn.Enabled = timeSTLabel.Enabled = finishSTLabel.Enabled = false;
			timeSTLabel.Text = "游戏用时：00:00:00";
			finishSTLabel.Text = "完成度：0%";
		}
		/// <summary>
		/// 保存程序设置。
		/// </summary>
		private void SaveSetting()
		{
			JigsawSetting.Default.MainState = this.WindowState;
			JigsawSetting.Default.ShowBackground = gameManager.ShowBackground;
			JigsawSetting.Default.ShowBorder = gameManager.ShowBorder;
			JigsawSetting.Default.ShowThumb = showThumbTSBtn.Checked;
			JigsawSetting.Default.Save();
		}

		#endregion // 初始化

		#region IDisposable 成员

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.components != null)
				{
					this.components.Dispose();
				}
				if (this.loadingForm != null)
				{
					this.loadingForm.Dispose();
				}
				if (this.tokenCancelSource != null)
				{
					this.tokenCancelSource.Dispose();
				}
				this.gameManager.Dispose();
				this.deviceManager.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion // IDisposable 成员

		#region 拼图游戏

		/// <summary>
		/// 更新游戏用时。
		/// </summary>
		private void UpdateTime(object sender, EventArgs e)
		{
			TimeSpan time = gameManager.UsedTime;
			timeSTLabel.Text = string.Format(CultureInfo.CurrentCulture,
				"游戏用时：{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
		}
		/// <summary>
		/// 更新游戏完成比例。
		/// </summary>
		private void UpdateFinishedPercent(object sender, EventArgs e)
		{
			int finished = gameManager.FinishedPercent;
			finishSTLabel.Text = string.Format(CultureInfo.CurrentCulture, "完成度：{0}%", finished);
			if (finished == 100)
			{
				gameTimer.Stop();
				UpdateThumbImage(true);
				ResetToolStrip();
				// 将拼图完整的显示出来。
				renderPanel.ShowImage();
				gameManager.InvalidateAll();
				// 构造一个比较友好的提示框。
				TimeSpan time = gameManager.UsedTime;
				StringBuilder text = new StringBuilder();
				text.Append("游戏成功完成！用时 ");
				if (time.Hours != 0)
				{
					text.Append(time.Hours);
					text.Append(" 小时 ");
				}
				if (time.Minutes != 0)
				{
					text.Append(time.Minutes);
					text.Append(" 分 ");
				}
				text.Append(time.Seconds);
				text.Append(" 秒！");
				MessageBox.Show(text.ToString(), "拼图游戏", MessageBoxButtons.OK, MessageBoxIcon.Information);
				GC.Collect();
			}
		}
		/// <summary>
		/// 开始一个拼图游戏。
		/// </summary>
		/// <param name="pieces">拼图碎片集合。</param>
		/// <param name="info">游戏信息。</param>
		public void StartGame(JigsawPieceCollection pieces, JigsawInfo info)
		{
			this.StopGame(true);
			this.gamePath = null;
			this.ShowLoadingForm();
			Task task = gameManager.StartGame(pieces, info, this.tokenCancelSource.Token);
			task.ContinueWith(t => BeginGame(), TaskContinuationOptions.OnlyOnRanToCompletion);
			task.ContinueWith(t => HideLoadingForm());
			task.Start();
		}
		/// <summary>
		/// 打开拼图游戏。
		/// </summary>
		/// <param name="fileName">拼图游戏的路径。</param>
		public void OpenGame(string fileName)
		{
			this.StopGame(true);
			Task task = null;
			this.ShowLoadingForm();
			try
			{
				task = gameManager.OpenGame(fileName, this.tokenCancelSource.Token);
				JigsawSetting.Default.FileName = this.gamePath = fileName;
			}
			catch (SecurityException)
			{
				this.HideLoadingForm();
				MessageBox.Show("游戏没有读取该文件的权限", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			catch (SerializationException)
			{
				this.HideLoadingForm();
				MessageBox.Show("指定的游戏存档文件无效", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			task.ContinueWith(t => BeginGame(), TaskContinuationOptions.OnlyOnRanToCompletion);
			task.ContinueWith(t => HideLoadingForm());
			task.Start();
		}
		/// <summary>
		/// 开始拼图游戏。
		/// </summary>
		private void BeginGame()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new Action(BeginGame));
				return;
			}
			UpdateTime(this, null);
			saveGameTSBtn.Enabled = scaleSTBtn.Enabled = timeSTLabel.Enabled = finishSTLabel.Enabled = true;
			UpdateThumbImage(true);
			gameManager.InvalidateAll();
			gameTimer.Start();
		}
		/// <summary>
		/// 结束拼图游戏。
		/// </summary>
		/// <param name="withoutSave">是否不需要保存游戏。</param>
		/// <returns>拼图游戏是否被结束。</returns>
		public bool StopGame(bool withoutSave)
		{
			if (!gameManager.HasGame)
			{
				return true;
			}
			if (!withoutSave && !SaveGame(true))
			{
				return false;
			}
			gameTimer.Stop();
			gameManager.StopGame();
			UpdateThumbImage(true);
			ResetToolStrip();
			return true;
		}
		/// <summary>
		/// 暂停当前游戏。
		/// </summary>
		public void PauseGame()
		{
			if (gameManager.HasGame)
			{
				gameTimer.Stop();
				gameManager.PauseGame();
			}
		}
		/// <summary>
		/// 继续当前游戏。
		/// </summary>
		public void ResumeGame()
		{
			if (gameManager.HasGame)
			{
				gameTimer.Start();
				gameManager.ResumeGame();
			}
		}
		/// <summary>
		/// 显示等待窗体。
		/// </summary>
		private void ShowLoadingForm()
		{
			this.newGameTSBtn.Enabled = this.openGameTSBtn.Enabled = false;
			if (this.loadingForm == null)
			{
				this.loadingForm = new LoadingForm();
			}
			this.loadingForm.Show(this);
			this.loadingForm.CenterParent();
			// 创建新的任务取消的通知。
			if (this.tokenCancelSource != null)
			{
				this.tokenCancelSource.Cancel();
				this.tokenCancelSource.Dispose();
			}
			this.tokenCancelSource = new CancellationTokenSource();
		}
		/// <summary>
		/// 隐藏等待窗体。
		/// </summary>
		private void HideLoadingForm()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new Action(HideLoadingForm));
				return;
			}
			this.newGameTSBtn.Enabled = this.openGameTSBtn.Enabled = true;
			if (this.loadingForm != null)
			{
				this.loadingForm.Close();
				this.loadingForm.Dispose();
				this.loadingForm = null;
			}
			if (this.tokenCancelSource != null)
			{
				this.tokenCancelSource.Dispose();
				this.tokenCancelSource = null;
			}
		}
		/// <summary>
		/// 保存拼图游戏。
		/// </summary>
		/// <param name="confirm">是否确认保存。</param>
		/// <returns>如果游戏已被保存或不保存，则为 <c>true</c>；如果取消保存则为 <c>false</c>。</returns>
		private bool SaveGame(bool confirm)
		{
			if (!gameManager.HasGame)
			{
				return true;
			}
			this.PauseGame();
			bool result = true;
			// 询问是否保存未完成的游戏。
			if (gameManager.FinishedPercent < 100 && gameManager.GameChanged)
			{
				DialogResult dr = DialogResult.Yes;
				if (confirm)
				{
					dr = MessageBox.Show("是否要保存当前游戏？", "确认",
						MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				}
				result = (dr != DialogResult.Cancel);
				if (dr == DialogResult.Yes)
				{
					if (gamePath == null)
					{
						gameSaveDialog.InitFileName(JigsawSetting.Default.FileName);
						if (gameSaveDialog.ShowDialog() == DialogResult.OK)
						{
							gamePath = gameSaveDialog.FileName;
							JigsawSetting.Default.FileName = gamePath;
						}
						else
						{
							result = false;
						}
					}
					if (result)
					{
						try
						{
							gameManager.SaveGame(this.gamePath);
						}
						catch (SecurityException)
						{
							loadingForm.Hide();
							MessageBox.Show("游戏没有写入该文件的权限", "警告",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
							result = false;
						}
					}
				}
			}
			this.ResumeGame();
			return result;
		}
		/// <summary>
		/// 拼图的缩放比例被改变的事件。
		/// </summary>
		private void renderPanel_JigsawScaleChanged(object sender, EventArgs e)
		{
			float scale = renderPanel.JigsawScale;
			scaleSTBtn.Text = "显示比例：" + ((int)(scale * 100)).ToString(CultureInfo.CurrentCulture) + "%";
			bool find = true;
			for (int i = 0; i < ScaleSizes.Length; i++)
			{
				if (find && scale >= ScaleSizes[i] - 0.01 && scale <= ScaleSizes[i] + 0.01)
				{
					find = false;
					((ToolStripMenuItem)scaleSTBtn.DropDown.Items[i]).Checked = true;
				}
				else
				{
					((ToolStripMenuItem)scaleSTBtn.DropDown.Items[i]).Checked = false;
				}
			}
		}
		/// <summary>
		/// 更新缩略图片。
		/// </summary>
		/// <param name="refresh">是否需要刷新缩略图。</param>
		private void UpdateThumbImage(bool refresh)
		{
			if (thumbForm != null)
			{
				if (gameManager.HasGame && gameManager.FinishedPercent < 100)
				{
					if (thumb == null || refresh)
					{
						using (MemoryStream stream = new MemoryStream(gameManager.GameInfo.ImageData))
						{
							thumb = (Bitmap)Bitmap.FromStream(stream);
						}
					}
					thumbForm.Image = thumb;
				}
				else
				{
					thumbForm.Image = null;
				}
			}
		}

		#endregion // 拼图游戏

		#region 控件事件

		/// <summary>
		/// 通过下拉菜单更改缩放比例。
		/// </summary>
		private void scaleSTBtn_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			// 以渲染区域中心为中心进行缩放。
			renderPanel.SetJigsawScale((float)e.ClickedItem.Tag,
				new Point(renderPanel.ClientSize.Width / 2, renderPanel.ClientSize.Height / 2));
		}
		/// <summary>
		/// 新建游戏的事件。
		/// </summary>
		private void newGameTSBtn_Click(object sender, EventArgs e)
		{
			if (this.SaveGame(true))
			{
				this.PauseGame();
				using (NewGameForm form = new NewGameForm())
				{
					if (form.ShowDialog() == DialogResult.OK)
					{
						// 生成拼图形状。
						JigsawPieceCollection pieces = new JigsawPieceCollection(this.deviceManager.D2DFactory, form.JigsawShape);
						this.StartGame(pieces, form.JigsawInfo);
					}
					else
					{
						this.ResumeGame();
					}
				}
			}
		}
		/// <summary>
		/// 打开游戏存档的事件。
		/// </summary>
		private void openGameTSBtn_Click(object sender, EventArgs e)
		{
			if (this.SaveGame(true))
			{
				this.PauseGame();
				gameOpenDialog.InitFileName(JigsawSetting.Default.FileName);
				if (gameOpenDialog.ShowDialog() == DialogResult.OK)
				{
					this.OpenGame(gameOpenDialog.FileName);
				}
				else
				{
					this.ResumeGame();
				}
			}
		}
		/// <summary>
		/// 保存当前游戏的事件。
		/// </summary>
		private void saveGameTSBtn_Click(object sender, EventArgs e)
		{
			this.SaveGame(false);
		}
		/// <summary>
		/// 切换显示缩略图窗口的事件。
		/// </summary>
		private void showThumbTSBtn_Click(object sender, EventArgs e)
		{
			if (!showThumbTSBtn.Checked)
			{
				showThumbTSBtn.Checked = true;
				thumbForm = new ThumbForm();
				thumbForm.FormClosing += thumbForm_FormClosing;
				thumbForm.Move += thumbForm_Move;
				UpdateThumbImage(false);
				thumbForm.Size = JigsawSetting.Default.ThumbSize;
				thumbForm.Location = new Point(JigsawSetting.Default.ThumbLocation.X + this.Location.X,
					JigsawSetting.Default.ThumbLocation.Y + this.Location.Y);
				thumbForm.Show(this);
			}
			else
			{
				thumbForm.Close();
			}
		}
		/// <summary>
		/// 缩略图窗口关闭的事件。
		/// </summary>
		private void thumbForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			showThumbTSBtn.Checked = false;
			thumbForm.FormClosing -= thumbForm_FormClosing;
			thumbForm.Move -= thumbForm_Move;
			JigsawSetting.Default.ThumbLocation = new Point(thumbForm.Location.X - this.Location.X,
				thumbForm.Location.Y - this.Location.Y);
			JigsawSetting.Default.ThumbSize = thumbForm.Size;
		}
		/// <summary>
		/// 缩略图窗口移动的事件。
		/// </summary>
		private void thumbForm_Move(object sender, EventArgs e)
		{
			if (thumbForm != null)
			{
				JigsawSetting.Default.ThumbLocation = new Point(thumbForm.Location.X - this.Location.X,
					thumbForm.Location.Y - this.Location.Y);
			}
		}
		/// <summary>
		/// 切换显示背景图片的事件。
		/// </summary>
		private void showBackgroundSTBtn_Click(object sender, EventArgs e)
		{
			gameManager.ShowBackground = !gameManager.ShowBackground;
			showBackgroundSTBtn.Checked = gameManager.ShowBackground;
			gameManager.InvalidateAll();
		}
		/// <summary>
		/// 切换显示边框拼图碎片的事件。
		/// </summary>
		private void showBorderSTBtn_Click(object sender, EventArgs e)
		{
			gameManager.ShowBorder = !gameManager.ShowBorder;
			showBorderSTBtn.Checked = gameManager.ShowBorder;
			gameManager.InvalidateAll();
		}
		/// <summary>
		/// 显示设置窗口。
		/// </summary>
		private void settingsTSBtn_Click(object sender, EventArgs e)
		{
			using (SettingForm form = new SettingForm(this.deviceManager.SupportD3D))
			{
				form.ShowDialog();
				string renderType = form.RendererType.ToString();
				if (JigsawSetting.Default.Renderer != renderType)
				{
					JigsawSetting.Default.Renderer = renderType;
					gameManager.RendererType = form.RendererType;
				}
				gameManager.BackgroundColor = JigsawSetting.Default.BackgroundColor;
				gameManager.BackgroundAlpha = JigsawSetting.Default.BackgroundAlpha;
				gameManager.InvalidateAll();
			}
		}
		/// <summary>
		/// 显示帮助窗口。
		/// </summary>
		private void helpTSBtn_Click(object sender, EventArgs e)
		{
			using (HelpForm form = new HelpForm())
			{
				form.ShowDialog();
			}
		}
		/// <summary>
		/// 快捷键。
		/// </summary>
		private void renderPanel_KeyDown(object sender, KeyEventArgs e)
		{
			Console.WriteLine(e.KeyData);
			switch (e.KeyCode)
			{
				case Keys.N:
					if (e.Control && !e.Shift && !e.Alt)
					{
						newGameTSBtn_Click(this, null);
					}
					break;
				case Keys.O:
					if (e.Control && !e.Shift && !e.Alt)
					{
						openGameTSBtn_Click(this, null);
					}
					break;
				case Keys.S:
					if (e.Control && !e.Shift && !e.Alt)
					{
						saveGameTSBtn_Click(this, null);
					}
					break;
				case Keys.F1:
					if (!e.Control && !e.Shift && !e.Alt)
					{
						helpTSBtn_Click(this, null);
					}
					break;
				case Keys.F2:
					if (!e.Control && !e.Shift && !e.Alt)
					{
						showThumbTSBtn_Click(this, null);
					}
					break;
				case Keys.F3:
					if (!e.Control && !e.Shift && !e.Alt)
					{
						showBackgroundSTBtn_Click(this, null);
					}
					break;
				case Keys.F4:
					if (!e.Control && !e.Shift && !e.Alt)
					{
						showBorderSTBtn_Click(this, null);
					}
					break;
				case Keys.F5:
					if (!e.Control && !e.Shift && !e.Alt)
					{
						settingsTSBtn_Click(this, null);
					}
					break;
			}
		}
		/// <summary>
		/// 主窗体关闭之前的事件。
		/// </summary>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.FormOwnerClosing)
			{
				if (!StopGame(false))
				{
					e.Cancel = true;
					return;
				}
			}
			else
			{
				StopGame(true);
			}
			if (tokenCancelSource != null)
			{
				tokenCancelSource.Cancel();
				tokenCancelSource.Dispose();
				tokenCancelSource = null;
			}
			// 关闭缩略图窗口。
			if (thumbForm != null)
			{
				thumbForm.Close();
			}
			SaveSetting();
		}
		/// <summary>
		/// 主窗体移动的事件。
		/// </summary>
		private void MainForm_Move(object sender, EventArgs e)
		{
			if (thumbForm != null)
			{
				thumbForm.Location = new Point(JigsawSetting.Default.ThumbLocation.X + this.Location.X,
					JigsawSetting.Default.ThumbLocation.Y + this.Location.Y);
			}
			if (loadingForm != null)
			{
				loadingForm.CenterParent();
			}
			if (this.WindowState == FormWindowState.Normal)
			{
				// 仅记录普通状态下的窗体尺寸和位置。
				JigsawSetting.Default.MainSize = this.ClientSize;
				JigsawSetting.Default.MainLocation = this.Location;
			}
		}
		/// <summary>
		/// 主窗体改变大小的事件。
		/// </summary>
		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			if (loadingForm != null)
			{
				loadingForm.CenterParent();
			}
			if (this.WindowState == FormWindowState.Normal)
			{
				// 仅记录普通状态下的窗体尺寸和位置。
				JigsawSetting.Default.MainSize = this.ClientSize;
				JigsawSetting.Default.MainLocation = this.Location;
			}
		}

		#endregion // 控件事件

	}
}
