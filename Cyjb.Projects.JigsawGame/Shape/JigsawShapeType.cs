using System.ComponentModel;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 拼图的形状类型
	/// </summary>
	public enum JigsawShapeType
	{
		/// <summary>
		/// 四边形。
		/// </summary>
		[Description("四边形")]
		Square,
		/// <summary>
		/// 标准圆形。
		/// </summary>
		[Description("标准圆形")]
		StandardCirle,
		/// <summary>
		/// 标准形状。
		/// </summary>
		[Description("标准形状")]
		Standard,
		/// <summary>
		/// 标准平滑形。
		/// </summary>
		[Description("标准平滑形")]
		StandardSmooth,
	}
}
