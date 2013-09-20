using System;

namespace Cyjb.Projects.JigsawGame.Jigsaw
{
	/// <summary>
	/// 拼图碎片所处的状态。
	/// </summary>
	[Flags]
	[Serializable]
	public enum JigsawPieceState
	{
		/// <summary>
		/// 普通状态。
		/// </summary>
		None = 0x0,
		/// <summary>
		/// 高亮状态。
		/// </summary>
		Highlight = 0x1,
		/// <summary>
		/// 被选中的状态。
		/// </summary>
		Selected = 0x2,
		/// <summary>
		/// 正在拖动状态。
		/// </summary>
		Draging = 0x4,
		/// <summary>
		/// 被选中且拖动状态。
		/// </summary>
		SelectedDraging = 0x6
	}
}
