using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 等待窗体。
	/// </summary>
	public partial class LoadingForm : Form
	{
		/// <summary>
		/// 构造函数。
		/// </summary>
		public LoadingForm()
		{
			this.BackColor = JigsawSetting.Default.BackgroundColor;
			InitializeComponent();
		}
	}
}
