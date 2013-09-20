namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 路径的类型。
	/// </summary>
	public enum PathType
	{
		/// <summary>
		/// 直线段。
		/// </summary>
		Line,
		/// <summary>
		/// 弧线。
		/// </summary>
		Arc,
		/// <summary>
		/// 三次贝塞尔曲线。
		/// </summary>
		Bezier,
		/// <summary>
		/// 结束形状。
		/// </summary>
		EndFigure
	}
}
