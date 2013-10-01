using System.Drawing;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 等待窗体。
	/// </summary>
	public partial class LoadingForm : ToolForm
	{
		/// <summary>
		/// 构造函数。
		/// </summary>
		public LoadingForm()
		{
			this.BackColor = JigsawSetting.Default.BackgroundColor;
			InitializeComponent();
		}
		/// <summary>
		/// 将窗体置于父窗体的中心。
		/// </summary>
		public void CenterParent()
		{
			this.Location = new Point(this.Owner.Location.X + (this.Owner.Size.Width - this.Width) / 2,
				this.Owner.Location.Y + (this.Owner.Size.Height - this.Height) / 2);
		}
	}
}
