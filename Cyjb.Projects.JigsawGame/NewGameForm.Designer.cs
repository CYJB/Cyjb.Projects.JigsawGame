namespace Cyjb.Projects.JigsawGame
{
	partial class NewGameForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblPic = new System.Windows.Forms.Label();
			this.btnPic = new System.Windows.Forms.Button();
			this.lblDifficulty = new System.Windows.Forms.Label();
			this.tbarDifficulty = new System.Windows.Forms.TrackBar();
			this.combDifficulty = new System.Windows.Forms.ComboBox();
			this.lblPieces = new System.Windows.Forms.Label();
			this.lblRand = new System.Windows.Forms.Label();
			this.tbarRand = new System.Windows.Forms.TrackBar();
			this.lblRandInfo = new System.Windows.Forms.Label();
			this.cbxRotate = new System.Windows.Forms.CheckBox();
			this.cbxAnchor = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.picture = new System.Windows.Forms.PictureBox();
			this.combShape = new System.Windows.Forms.ComboBox();
			this.imageOpenDialog = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.tbarDifficulty)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbarRand)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
			this.SuspendLayout();
			// 
			// lblPic
			// 
			this.lblPic.AutoSize = true;
			this.lblPic.Location = new System.Drawing.Point(12, 17);
			this.lblPic.Name = "lblPic";
			this.lblPic.Size = new System.Drawing.Size(65, 12);
			this.lblPic.TabIndex = 0;
			this.lblPic.Text = "选择图片：";
			// 
			// btnPic
			// 
			this.btnPic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPic.Location = new System.Drawing.Point(427, 12);
			this.btnPic.Name = "btnPic";
			this.btnPic.Size = new System.Drawing.Size(75, 23);
			this.btnPic.TabIndex = 1;
			this.btnPic.Text = "浏览(&V)";
			this.btnPic.UseVisualStyleBackColor = true;
			this.btnPic.Click += new System.EventHandler(this.btnPic_Click);
			// 
			// lblDifficulty
			// 
			this.lblDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblDifficulty.AutoSize = true;
			this.lblDifficulty.Location = new System.Drawing.Point(12, 328);
			this.lblDifficulty.Name = "lblDifficulty";
			this.lblDifficulty.Size = new System.Drawing.Size(65, 12);
			this.lblDifficulty.TabIndex = 3;
			this.lblDifficulty.Text = "游戏难度：";
			// 
			// tbarDifficulty
			// 
			this.tbarDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbarDifficulty.Enabled = false;
			this.tbarDifficulty.Location = new System.Drawing.Point(83, 328);
			this.tbarDifficulty.Maximum = 100;
			this.tbarDifficulty.Name = "tbarDifficulty";
			this.tbarDifficulty.Size = new System.Drawing.Size(354, 45);
			this.tbarDifficulty.TabIndex = 4;
			this.tbarDifficulty.TickFrequency = 5;
			this.tbarDifficulty.Scroll += new System.EventHandler(this.tbarDifficulty_Scroll);
			// 
			// combDifficulty
			// 
			this.combDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.combDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.combDifficulty.Enabled = false;
			this.combDifficulty.FormattingEnabled = true;
			this.combDifficulty.Items.AddRange(new object[] {
            "普通",
            "噩梦",
            "地狱",
            "炼狱"});
			this.combDifficulty.Location = new System.Drawing.Point(443, 325);
			this.combDifficulty.Name = "combDifficulty";
			this.combDifficulty.Size = new System.Drawing.Size(60, 20);
			this.combDifficulty.TabIndex = 5;
			this.combDifficulty.SelectedIndexChanged += new System.EventHandler(this.combDifficulty_SelectedIndexChanged);
			// 
			// lblPieces
			// 
			this.lblPieces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPieces.AutoSize = true;
			this.lblPieces.Location = new System.Drawing.Point(81, 361);
			this.lblPieces.Name = "lblPieces";
			this.lblPieces.Size = new System.Drawing.Size(95, 12);
			this.lblPieces.TabIndex = 6;
			this.lblPieces.Text = "拼图碎片数目：0";
			// 
			// lblRand
			// 
			this.lblRand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblRand.AutoSize = true;
			this.lblRand.Location = new System.Drawing.Point(12, 379);
			this.lblRand.Name = "lblRand";
			this.lblRand.Size = new System.Drawing.Size(65, 12);
			this.lblRand.TabIndex = 7;
			this.lblRand.Text = "随机程度：";
			// 
			// tbarRand
			// 
			this.tbarRand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbarRand.Enabled = false;
			this.tbarRand.Location = new System.Drawing.Point(83, 379);
			this.tbarRand.Maximum = 100;
			this.tbarRand.Name = "tbarRand";
			this.tbarRand.Size = new System.Drawing.Size(419, 45);
			this.tbarRand.TabIndex = 8;
			this.tbarRand.TickFrequency = 5;
			this.tbarRand.Value = 30;
			this.tbarRand.Scroll += new System.EventHandler(this.tbarRand_Scroll);
			// 
			// lblRandInfo
			// 
			this.lblRandInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblRandInfo.AutoSize = true;
			this.lblRandInfo.Location = new System.Drawing.Point(81, 412);
			this.lblRandInfo.Name = "lblRandInfo";
			this.lblRandInfo.Size = new System.Drawing.Size(23, 12);
			this.lblRandInfo.TabIndex = 9;
			this.lblRandInfo.Text = "30%";
			// 
			// cbxRotate
			// 
			this.cbxRotate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbxRotate.AutoSize = true;
			this.cbxRotate.Enabled = false;
			this.cbxRotate.Location = new System.Drawing.Point(12, 430);
			this.cbxRotate.Name = "cbxRotate";
			this.cbxRotate.Size = new System.Drawing.Size(96, 16);
			this.cbxRotate.TabIndex = 10;
			this.cbxRotate.Text = "允许拼图旋转";
			this.cbxRotate.UseVisualStyleBackColor = true;
			// 
			// cbxAnchor
			// 
			this.cbxAnchor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbxAnchor.AutoSize = true;
			this.cbxAnchor.Enabled = false;
			this.cbxAnchor.Location = new System.Drawing.Point(114, 430);
			this.cbxAnchor.Name = "cbxAnchor";
			this.cbxAnchor.Size = new System.Drawing.Size(108, 16);
			this.cbxAnchor.TabIndex = 11;
			this.cbxAnchor.Text = "吸附到正确位置";
			this.cbxAnchor.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Enabled = false;
			this.btnOK.Location = new System.Drawing.Point(346, 426);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 13;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(427, 426);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 14;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// picture
			// 
			this.picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.picture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picture.Location = new System.Drawing.Point(12, 41);
			this.picture.Name = "picture";
			this.picture.Size = new System.Drawing.Size(490, 278);
			this.picture.TabIndex = 2;
			this.picture.TabStop = false;
			this.picture.Paint += new System.Windows.Forms.PaintEventHandler(this.picture_Paint);
			this.picture.Resize += new System.EventHandler(this.picture_Resize);
			// 
			// combShape
			// 
			this.combShape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.combShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.combShape.Enabled = false;
			this.combShape.FormattingEnabled = true;
			this.combShape.Location = new System.Drawing.Point(228, 428);
			this.combShape.Name = "combShape";
			this.combShape.Size = new System.Drawing.Size(112, 20);
			this.combShape.TabIndex = 12;
			// 
			// imageOpenDialog
			// 
			this.imageOpenDialog.FileName = "Image.png";
			this.imageOpenDialog.Filter = "位图文件|*.bmp|可交换图像文件格式|*.gif|JPEG 文件|*.jpg;*.jpeg;*.jpe|可移植网络图形|*.png|所有图像文件|*.bmp;" +
    "*.gif;*.jpg;*.jpeg;*.jpe;*.png";
			this.imageOpenDialog.FilterIndex = 5;
			this.imageOpenDialog.Title = "选择图片";
			// 
			// NewGameForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(514, 461);
			this.Controls.Add(this.combShape);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cbxAnchor);
			this.Controls.Add(this.cbxRotate);
			this.Controls.Add(this.lblRandInfo);
			this.Controls.Add(this.tbarRand);
			this.Controls.Add(this.lblRand);
			this.Controls.Add(this.lblPieces);
			this.Controls.Add(this.combDifficulty);
			this.Controls.Add(this.tbarDifficulty);
			this.Controls.Add(this.lblDifficulty);
			this.Controls.Add(this.picture);
			this.Controls.Add(this.btnPic);
			this.Controls.Add(this.lblPic);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 500);
			this.Name = "NewGameForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "创建新游戏";
			((System.ComponentModel.ISupportInitialize)(this.tbarDifficulty)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbarRand)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPic;
		private System.Windows.Forms.Button btnPic;
		private System.Windows.Forms.PictureBox picture;
		private System.Windows.Forms.Label lblDifficulty;
		private System.Windows.Forms.TrackBar tbarDifficulty;
		private System.Windows.Forms.ComboBox combDifficulty;
		private System.Windows.Forms.Label lblPieces;
		private System.Windows.Forms.Label lblRand;
		private System.Windows.Forms.TrackBar tbarRand;
		private System.Windows.Forms.Label lblRandInfo;
		private System.Windows.Forms.CheckBox cbxRotate;
		private System.Windows.Forms.CheckBox cbxAnchor;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ComboBox combShape;
		private System.Windows.Forms.OpenFileDialog imageOpenDialog;
	}
}