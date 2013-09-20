namespace Cyjb.Projects.JigsawGame
{
	partial class BugReportForm
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
			this.lblTitle = new System.Windows.Forms.Label();
			this.tbxException = new System.Windows.Forms.TextBox();
			this.linkReport = new System.Windows.Forms.LinkLabel();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(12, 9);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(161, 12);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "游戏出现了无法处理的异常：";
			// 
			// tbxException
			// 
			this.tbxException.Location = new System.Drawing.Point(12, 34);
			this.tbxException.Multiline = true;
			this.tbxException.Name = "tbxException";
			this.tbxException.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbxException.Size = new System.Drawing.Size(426, 186);
			this.tbxException.TabIndex = 1;
			// 
			// linkReport
			// 
			this.linkReport.AutoSize = true;
			this.linkReport.Location = new System.Drawing.Point(12, 231);
			this.linkReport.Name = "linkReport";
			this.linkReport.Size = new System.Drawing.Size(347, 12);
			this.linkReport.TabIndex = 2;
			this.linkReport.TabStop = true;
			this.linkReport.Text = "到 http://www.cnblogs.com/cyjb/p/JigsawGame.html 报告异常";
			this.linkReport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkReport_LinkClicked);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(365, 226);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "关闭";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// BugReportForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(450, 261);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.linkReport);
			this.Controls.Add(this.tbxException);
			this.Controls.Add(this.lblTitle);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BugReportForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "异常报告";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox tbxException;
		private System.Windows.Forms.LinkLabel linkReport;
		private System.Windows.Forms.Button btnClose;
	}
}