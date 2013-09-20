namespace Cyjb.Projects.JigsawGame
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mainToolStrip = new System.Windows.Forms.ToolStrip();
			this.newGameTSBtn = new System.Windows.Forms.ToolStripButton();
			this.openGameTSBtn = new System.Windows.Forms.ToolStripButton();
			this.saveGameTSBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.showThumbTSBtn = new System.Windows.Forms.ToolStripButton();
			this.showBackgroundSTBtn = new System.Windows.Forms.ToolStripButton();
			this.showBorderSTBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.settingsTSBtn = new System.Windows.Forms.ToolStripButton();
			this.helpTSBtn = new System.Windows.Forms.ToolStripButton();
			this.scaleSTBtn = new System.Windows.Forms.ToolStripDropDownButton();
			this.finishSTLabel = new System.Windows.Forms.ToolStripLabel();
			this.timeSTLabel = new System.Windows.Forms.ToolStripLabel();
			this.gameTimer = new System.Windows.Forms.Timer(this.components);
			this.gameSaveDialog = new System.Windows.Forms.SaveFileDialog();
			this.gameOpenDialog = new System.Windows.Forms.OpenFileDialog();
			this.renderPanel = new Cyjb.Projects.JigsawGame.Renderer.JigsawRenderPanel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mainToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainToolStrip
			// 
			this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameTSBtn,
            this.openGameTSBtn,
            this.saveGameTSBtn,
            this.toolStripSeparator,
            this.showThumbTSBtn,
            this.showBackgroundSTBtn,
            this.showBorderSTBtn,
            this.toolStripSeparator1,
            this.settingsTSBtn,
            this.helpTSBtn,
            this.toolStripSeparator2,
            this.scaleSTBtn,
            this.finishSTLabel,
            this.timeSTLabel});
			this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new System.Drawing.Size(584, 25);
			this.mainToolStrip.TabIndex = 0;
			// 
			// newGameTSBtn
			// 
			this.newGameTSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newGameTSBtn.Image = ((System.Drawing.Image)(resources.GetObject("newGameTSBtn.Image")));
			this.newGameTSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newGameTSBtn.Name = "newGameTSBtn";
			this.newGameTSBtn.Size = new System.Drawing.Size(23, 22);
			this.newGameTSBtn.Text = "新建游戏(Ctrl+N)";
			this.newGameTSBtn.Click += new System.EventHandler(this.newGameTSBtn_Click);
			// 
			// openGameTSBtn
			// 
			this.openGameTSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.openGameTSBtn.Image = ((System.Drawing.Image)(resources.GetObject("openGameTSBtn.Image")));
			this.openGameTSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openGameTSBtn.Name = "openGameTSBtn";
			this.openGameTSBtn.Size = new System.Drawing.Size(23, 22);
			this.openGameTSBtn.Text = "打开游戏(Ctrl+O)";
			this.openGameTSBtn.Click += new System.EventHandler(this.openGameTSBtn_Click);
			// 
			// saveGameTSBtn
			// 
			this.saveGameTSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveGameTSBtn.Image = ((System.Drawing.Image)(resources.GetObject("saveGameTSBtn.Image")));
			this.saveGameTSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveGameTSBtn.Name = "saveGameTSBtn";
			this.saveGameTSBtn.Size = new System.Drawing.Size(23, 22);
			this.saveGameTSBtn.Text = "保存游戏(Ctrl+S)";
			this.saveGameTSBtn.Click += new System.EventHandler(this.saveGameTSBtn_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// showThumbTSBtn
			// 
			this.showThumbTSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.showThumbTSBtn.Image = global::Cyjb.Projects.JigsawGame.Resources.ShowThumb;
			this.showThumbTSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showThumbTSBtn.Name = "showThumbTSBtn";
			this.showThumbTSBtn.Size = new System.Drawing.Size(23, 22);
			this.showThumbTSBtn.Text = "显示缩略图(F2)";
			this.showThumbTSBtn.Click += new System.EventHandler(this.showThumbTSBtn_Click);
			// 
			// showBackgroundSTBtn
			// 
			this.showBackgroundSTBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.showBackgroundSTBtn.Image = global::Cyjb.Projects.JigsawGame.Resources.ShowBackground;
			this.showBackgroundSTBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showBackgroundSTBtn.Name = "showBackgroundSTBtn";
			this.showBackgroundSTBtn.Size = new System.Drawing.Size(23, 22);
			this.showBackgroundSTBtn.Text = "显示背景图片(F3)";
			this.showBackgroundSTBtn.Click += new System.EventHandler(this.showBackgroundSTBtn_Click);
			// 
			// showBorderSTBtn
			// 
			this.showBorderSTBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.showBorderSTBtn.Image = global::Cyjb.Projects.JigsawGame.Resources.ShowBorderOnly;
			this.showBorderSTBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showBorderSTBtn.Name = "showBorderSTBtn";
			this.showBorderSTBtn.Size = new System.Drawing.Size(23, 22);
			this.showBorderSTBtn.Text = "显示边框碎片(F4)";
			this.showBorderSTBtn.Click += new System.EventHandler(this.showBorderSTBtn_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// settingsTSBtn
			// 
			this.settingsTSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.settingsTSBtn.Image = global::Cyjb.Projects.JigsawGame.Resources.Settings;
			this.settingsTSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.settingsTSBtn.Name = "settingsTSBtn";
			this.settingsTSBtn.Size = new System.Drawing.Size(23, 22);
			this.settingsTSBtn.Text = "设置(F5)";
			this.settingsTSBtn.Click += new System.EventHandler(this.settingsTSBtn_Click);
			// 
			// helpTSBtn
			// 
			this.helpTSBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.helpTSBtn.Image = ((System.Drawing.Image)(resources.GetObject("helpTSBtn.Image")));
			this.helpTSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.helpTSBtn.Name = "helpTSBtn";
			this.helpTSBtn.Size = new System.Drawing.Size(23, 22);
			this.helpTSBtn.Text = "帮助(F1)";
			this.helpTSBtn.Click += new System.EventHandler(this.helpTSBtn_Click);
			// 
			// scaleSTBtn
			// 
			this.scaleSTBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.scaleSTBtn.Image = ((System.Drawing.Image)(resources.GetObject("scaleSTBtn.Image")));
			this.scaleSTBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.scaleSTBtn.Name = "scaleSTBtn";
			this.scaleSTBtn.Size = new System.Drawing.Size(112, 22);
			this.scaleSTBtn.Text = "显示比例：100%";
			this.scaleSTBtn.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.scaleSTBtn_DropDownItemClicked);
			// 
			// finishSTLabel
			// 
			this.finishSTLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.finishSTLabel.Name = "finishSTLabel";
			this.finishSTLabel.Size = new System.Drawing.Size(87, 22);
			this.finishSTLabel.Text = "完成度：100%";
			// 
			// timeSTLabel
			// 
			this.timeSTLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.timeSTLabel.Name = "timeSTLabel";
			this.timeSTLabel.Size = new System.Drawing.Size(115, 22);
			this.timeSTLabel.Text = "游戏用时：00:00:00";
			// 
			// gameTimer
			// 
			this.gameTimer.Tick += new System.EventHandler(this.UpdateTime);
			// 
			// gameSaveDialog
			// 
			this.gameSaveDialog.DefaultExt = "*.jig";
			this.gameSaveDialog.FileName = "SavedJigsaw.jig";
			this.gameSaveDialog.Filter = "拼图游戏存档|*.jig";
			this.gameSaveDialog.Title = "保存游戏";
			// 
			// gameOpenDialog
			// 
			this.gameOpenDialog.DefaultExt = "*.jig";
			this.gameOpenDialog.FileName = "SavedJigsaw.jig";
			this.gameOpenDialog.Filter = "拼图游戏存档|*.jig";
			this.gameOpenDialog.Title = "打开游戏";
			// 
			// renderPanel
			// 
			this.renderPanel.AutoScroll = true;
			this.renderPanel.AutoScrollMinSize = new System.Drawing.Size(200, 200);
			this.renderPanel.D2DFactory = null;
			this.renderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderPanel.ImageSize = ((SharpDX.Size2F)(resources.GetObject("renderPanel.ImageSize")));
			this.renderPanel.Location = new System.Drawing.Point(0, 25);
			this.renderPanel.Name = "renderPanel";
			this.renderPanel.Size = new System.Drawing.Size(584, 336);
			this.renderPanel.TabIndex = 1;
			this.renderPanel.TabStop = false;
			this.renderPanel.JigsawScaleChanged += new System.EventHandler(this.renderPanel_JigsawScaleChanged);
			this.renderPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.renderPanel_KeyDown);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 361);
			this.Controls.Add(this.renderPanel);
			this.Controls.Add(this.mainToolStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "拼图游戏";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
			this.Move += new System.EventHandler(this.MainForm_Move);
			this.mainToolStrip.ResumeLayout(false);
			this.mainToolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip mainToolStrip;
		private System.Windows.Forms.ToolStripButton newGameTSBtn;
		private System.Windows.Forms.ToolStripButton openGameTSBtn;
		private System.Windows.Forms.ToolStripButton saveGameTSBtn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripButton showThumbTSBtn;
		private System.Windows.Forms.ToolStripButton showBackgroundSTBtn;
		private System.Windows.Forms.ToolStripButton showBorderSTBtn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton helpTSBtn;
		private System.Windows.Forms.ToolStripButton settingsTSBtn;
		private Cyjb.Projects.JigsawGame.Renderer.JigsawRenderPanel renderPanel;
		private System.Windows.Forms.ToolStripDropDownButton scaleSTBtn;
		private System.Windows.Forms.ToolStripLabel finishSTLabel;
		private System.Windows.Forms.Timer gameTimer;
		private System.Windows.Forms.ToolStripLabel timeSTLabel;
		private System.Windows.Forms.SaveFileDialog gameSaveDialog;
		private System.Windows.Forms.OpenFileDialog gameOpenDialog;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

	}
}

