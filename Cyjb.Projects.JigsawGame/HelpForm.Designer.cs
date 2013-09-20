namespace Cyjb.Projects.JigsawGame
{
	partial class HelpForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnClose = new System.Windows.Forms.Button();
			this.pbxLink = new System.Windows.Forms.PictureBox();
			this.pbxLicense = new System.Windows.Forms.PictureBox();
			this.pbxHelpLink = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbxLink)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbxLicense)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbxHelpLink)).BeginInit();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(499, 319);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "关闭";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// pbxLink
			// 
			this.pbxLink.BackColor = System.Drawing.Color.Transparent;
			this.pbxLink.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbxLink.Location = new System.Drawing.Point(279, 7);
			this.pbxLink.Name = "pbxLink";
			this.pbxLink.Size = new System.Drawing.Size(174, 25);
			this.pbxLink.TabIndex = 5;
			this.pbxLink.TabStop = false;
			this.pbxLink.Click += new System.EventHandler(this.pbxLink_Click);
			// 
			// pbxLicense
			// 
			this.pbxLicense.BackColor = System.Drawing.Color.Transparent;
			this.pbxLicense.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbxLicense.Location = new System.Drawing.Point(72, 33);
			this.pbxLicense.Name = "pbxLicense";
			this.pbxLicense.Size = new System.Drawing.Size(380, 25);
			this.pbxLicense.TabIndex = 5;
			this.pbxLicense.TabStop = false;
			this.pbxLicense.Click += new System.EventHandler(this.pbxLicense_Click);
			// 
			// pbxHelpLink
			// 
			this.pbxHelpLink.BackColor = System.Drawing.Color.Transparent;
			this.pbxHelpLink.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbxHelpLink.Location = new System.Drawing.Point(139, 327);
			this.pbxHelpLink.Name = "pbxHelpLink";
			this.pbxHelpLink.Size = new System.Drawing.Size(66, 25);
			this.pbxHelpLink.TabIndex = 5;
			this.pbxHelpLink.TabStop = false;
			this.pbxHelpLink.Click += new System.EventHandler(this.pbxHelpLink_Click);
			// 
			// HelpForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::Cyjb.Projects.JigsawGame.Resources.JigsawHelp;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(586, 354);
			this.Controls.Add(this.pbxLicense);
			this.Controls.Add(this.pbxHelpLink);
			this.Controls.Add(this.pbxLink);
			this.Controls.Add(this.btnClose);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HelpForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "游戏帮助";
			((System.ComponentModel.ISupportInitialize)(this.pbxLink)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbxLicense)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbxHelpLink)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.PictureBox pbxLink;
		private System.Windows.Forms.PictureBox pbxLicense;
		private System.Windows.Forms.PictureBox pbxHelpLink;
	}
}