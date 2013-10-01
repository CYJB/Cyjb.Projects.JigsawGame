using System.IO;
using System.Windows.Forms;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 包含 WinForm 的实用方法。
	/// </summary>
	public static class WinFormUtility
	{
		/// <summary>
		/// 初始化文件对话框的文件名。
		/// </summary>
		/// <param name="dialog">对话框。</param>
		/// <param name="fileName">文件名。</param>
		public static void InitFileName(this FileDialog dialog, string fileName)
		{
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				dialog.FileName = Path.GetFileName(fileName);
				dialog.InitialDirectory = Path.GetDirectoryName(fileName);
			}
		}
	}
}
