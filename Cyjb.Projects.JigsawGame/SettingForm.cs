using System;
using System.Globalization;
using System.Windows.Forms;
using Cyjb.Projects.JigsawGame.Renderer;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 设置窗体。
	/// </summary>
	public partial class SettingForm : Form
	{
		/// <summary>
		/// 构造函数。
		/// </summary>
		/// <param name="enabledEffect">是否可以使用 Effect 拼图渲染器。</param>
		public SettingForm(bool enabledEffect)
		{
			InitializeComponent();
			this.tbarBackgroundAlpha.Value = (int)(JigsawSetting.Default.BackgroundAlpha * 100);
			this.pbxBackgroundColor.BackColor = JigsawSetting.Default.BackgroundColor;
			if (enabledEffect)
			{
				this.RendererType = EnumExt.Parse<JigsawRendererType>(JigsawSetting.Default.Renderer);
				if (this.RendererType == JigsawRendererType.Effect)
				{
					rbtnEffect.Checked = true;
				}
				this.lblEffectWarn.Visible = false;
			}
			else
			{
				this.rbtnSample.Checked = true;
				this.rbtnEffect.Enabled = false;
				this.lblEffectWarn.Visible = true;
			}
		}
		/// <summary>
		/// 获取设置的拼图渲染器。
		/// </summary>
		public JigsawRendererType RendererType { get; private set; }
		/// <summary>
		/// 背景不透明度被改变的事件。
		/// </summary>
		private void tbarBackgroundAlpha_Scroll(object sender, EventArgs e)
		{
			lblBackgroundAlphaInfo.Text = tbarBackgroundAlpha.Value.ToString(CultureInfo.CurrentCulture) + "%";
			JigsawSetting.Default.BackgroundAlpha = (float)tbarBackgroundAlpha.Value / 100;
		}
		/// <summary>
		/// 打开颜色设置对话框。
		/// </summary>
		private void pbxBackgroundColor_Click(object sender, EventArgs e)
		{
			this.backgroundColorDialog.Color = JigsawSetting.Default.BackgroundColor;
			if (this.backgroundColorDialog.ShowDialog() == DialogResult.OK)
			{
				JigsawSetting.Default.BackgroundColor = this.backgroundColorDialog.Color;
				this.pbxBackgroundColor.BackColor = JigsawSetting.Default.BackgroundColor;
			}
		}
		/// <summary>
		/// 拼图渲染器被改变的事件。
		/// </summary>
		private void rbtnSample_CheckedChanged(object sender, EventArgs e)
		{
			if (rbtnSample.Checked)
			{
				this.RendererType = JigsawRendererType.Simple;
			}
			else
			{
				this.RendererType = JigsawRendererType.Effect;
			}
		}
	}
}
