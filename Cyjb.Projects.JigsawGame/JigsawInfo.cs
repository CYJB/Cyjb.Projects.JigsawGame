using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 表示拼图游戏的相关信息。
	/// </summary>
	[Serializable]
	public sealed class JigsawInfo
	{
		/// <summary>
		/// 获取或设置是否允许旋转拼图。
		/// </summary>
		public bool Rotatable { get; set; }
		/// <summary>
		/// 获取或设置是否允吸附到背景。
		/// </summary>
		public bool AnchorToBackground { get; set; }
		/// <summary>
		/// 获取或设置吸附的半径。
		/// </summary>
		public float AnchorRadius { get; set; }
		/// <summary>
		/// 获取或设置拼图碎片的总个数。
		/// </summary>
		public int PieceSum { get; set; }
		/// <summary>
		/// 获取或设置当前游戏的缩放比例。
		/// </summary>
		public float Scale { get; set; }
		/// <summary>
		/// 获取或设置当前游戏窗口的滚动位置。
		/// </summary>
		public Point ScrollPosition { get; set; }
		/// <summary>
		/// 获取或设置拼图的图像数据。
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public byte[] ImageData { get; set; }
		/// <summary>
		/// 获取或设置已被使用的时间。
		/// </summary>
		public TimeSpan UsedTime { get; set; }
	}
}
