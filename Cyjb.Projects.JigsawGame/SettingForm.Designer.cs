namespace Cyjb.Projects.JigsawGame
{
	partial class SettingForm
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
			this.lblBackgroundAlpha = new System.Windows.Forms.Label();
			this.tbarBackgroundAlpha = new System.Windows.Forms.TrackBar();
			this.lblBackgroundAlphaInfo = new System.Windows.Forms.Label();
			this.lblBackgroundColor = new System.Windows.Forms.Label();
			this.pbxBackgroundColor = new System.Windows.Forms.PictureBox();
			this.backgroundColorDialog = new System.Windows.Forms.ColorDialog();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblRenderer = new System.Windows.Forms.Label();
			this.rbtnSample = new System.Windows.Forms.RadioButton();
			this.rbtnEffect = new System.Windows.Forms.RadioButton();
			this.lblEffectWarn = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.tbarBackgroundAlpha)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbxBackgroundColor)).BeginInit();
			this.SuspendLayout();
			// 
			// lblBackgroundAlpha
			// 
			this.lblBackgroundAlpha.AutoSize = true;
			this.lblBackgroundAlpha.Location = new System.Drawing.Point(12, 17);
			this.lblBackgroundAlpha.Name = "lblBackgroundAlpha";
			this.lblBackgroundAlpha.Size = new System.Drawing.Size(113, 12);
			this.lblBackgroundAlpha.TabIndex = 0;
			this.lblBackgroundAlpha.Text = "背景图片不透明度：";
			// 
			// tbarBackgroundAlpha
			// 
			this.tbarBackgroundAlpha.Location = new System.Drawing.Point(131, 12);
			this.tbarBackgroundAlpha.Maximum = 100;
			this.tbarBackgroundAlpha.Name = "tbarBackgroundAlpha";
			this.tbarBackgroundAlpha.Size = new System.Drawing.Size(341, 45);
			this.tbarBackgroundAlpha.TabIndex = 0;
			this.tbarBackgroundAlpha.TickFrequency = 10;
			this.tbarBackgroundAlpha.Value = 30;
			this.tbarBackgroundAlpha.Scroll += new System.EventHandler(this.tbarBackgroundAlpha_Scroll);
			// 
			// lblBackgroundAlphaInfo
			// 
			this.lblBackgroundAlphaInfo.AutoSize = true;
			this.lblBackgroundAlphaInfo.Location = new System.Drawing.Point(129, 45);
			this.lblBackgroundAlphaInfo.Name = "lblBackgroundAlphaInfo";
			this.lblBackgroundAlphaInfo.Size = new System.Drawing.Size(23, 12);
			this.lblBackgroundAlphaInfo.TabIndex = 1;
			this.lblBackgroundAlphaInfo.Text = "30%";
			// 
			// lblBackgroundColor
			// 
			this.lblBackgroundColor.AutoSize = true;
			this.lblBackgroundColor.Location = new System.Drawing.Point(30, 69);
			this.lblBackgroundColor.Name = "lblBackgroundColor";
			this.lblBackgroundColor.Size = new System.Drawing.Size(95, 12);
			this.lblBackgroundColor.TabIndex = 3;
			this.lblBackgroundColor.Text = "游戏背景颜色 ：";
			// 
			// pbxBackgroundColor
			// 
			this.pbxBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbxBackgroundColor.Location = new System.Drawing.Point(131, 65);
			this.pbxBackgroundColor.Name = "pbxBackgroundColor";
			this.pbxBackgroundColor.Size = new System.Drawing.Size(50, 20);
			this.pbxBackgroundColor.TabIndex = 4;
			this.pbxBackgroundColor.TabStop = false;
			this.pbxBackgroundColor.Click += new System.EventHandler(this.pbxBackgroundColor_Click);
			// 
			// backgroundColorDialog
			// 
			this.backgroundColorDialog.AnyColor = true;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(205, 121);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "关闭";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// lblRenderer
			// 
			this.lblRenderer.AutoSize = true;
			this.lblRenderer.Location = new System.Drawing.Point(60, 97);
			this.lblRenderer.Name = "lblRenderer";
			this.lblRenderer.Size = new System.Drawing.Size(65, 12);
			this.lblRenderer.TabIndex = 6;
			this.lblRenderer.Text = "拼图样式：";
			// 
			// rbtnSample
			// 
			this.rbtnSample.AutoSize = true;
			this.rbtnSample.Checked = true;
			this.rbtnSample.Location = new System.Drawing.Point(131, 95);
			this.rbtnSample.Name = "rbtnSample";
			this.rbtnSample.Size = new System.Drawing.Size(47, 16);
			this.rbtnSample.TabIndex = 7;
			this.rbtnSample.TabStop = true;
			this.rbtnSample.Text = "简单";
			this.rbtnSample.UseVisualStyleBackColor = true;
			this.rbtnSample.CheckedChanged += new System.EventHandler(this.rbtnSample_CheckedChanged);
			// 
			// rbtnEffect
			// 
			this.rbtnEffect.AutoSize = true;
			this.rbtnEffect.Location = new System.Drawing.Point(184, 95);
			this.rbtnEffect.Name = "rbtnEffect";
			this.rbtnEffect.Size = new System.Drawing.Size(47, 16);
			this.rbtnEffect.TabIndex = 8;
			this.rbtnEffect.Text = "特效";
			this.rbtnEffect.UseVisualStyleBackColor = true;
			// 
			// lblEffectWarn
			// 
			this.lblEffectWarn.AutoSize = true;
			this.lblEffectWarn.Enabled = false;
			this.lblEffectWarn.Location = new System.Drawing.Point(224, 97);
			this.lblEffectWarn.Name = "lblEffectWarn";
			this.lblEffectWarn.Size = new System.Drawing.Size(161, 12);
			this.lblEffectWarn.TabIndex = 9;
			this.lblEffectWarn.Text = "（您的电脑不支持特效样式）";
			// 
			// SettingForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(484, 157);
			this.Controls.Add(this.rbtnEffect);
			this.Controls.Add(this.lblEffectWarn);
			this.Controls.Add(this.rbtnSample);
			this.Controls.Add(this.lblRenderer);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.pbxBackgroundColor);
			this.Controls.Add(this.lblBackgroundColor);
			this.Controls.Add(this.lblBackgroundAlphaInfo);
			this.Controls.Add(this.tbarBackgroundAlpha);
			this.Controls.Add(this.lblBackgroundAlpha);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "游戏设置";
			((System.ComponentModel.ISupportInitialize)(this.tbarBackgroundAlpha)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbxBackgroundColor)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblBackgroundAlpha;
		private System.Windows.Forms.TrackBar tbarBackgroundAlpha;
		private System.Windows.Forms.Label lblBackgroundAlphaInfo;
		private System.Windows.Forms.Label lblBackgroundColor;
		private System.Windows.Forms.PictureBox pbxBackgroundColor;
		private System.Windows.Forms.ColorDialog backgroundColorDialog;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblRenderer;
		private System.Windows.Forms.RadioButton rbtnSample;
		private System.Windows.Forms.RadioButton rbtnEffect;
		private System.Windows.Forms.Label lblEffectWarn;
	}
}