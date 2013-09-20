using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 表示一条直线路径段。
	/// </summary>
	[Serializable]
	public sealed class LineSegment : PathSegment
	{
		/// <summary>
		/// 使用指定的终结点初始化 <see cref="PathSegment"/> 类的新实例。
		/// </summary>
		/// <param name="end">直线路径段的终结点。</param>
		public LineSegment(Vector2 end)
			: base(PathType.Line, end)
		{ }
		/// <summary>
		/// 使用当前的路径填充指定的路径几何。
		/// </summary>
		/// <param name="sink">要填充的路径几何。</param>
		public override void FillGeometry(GeometrySink sink)
		{
			sink.AddLine(this.EndPoint);
		}
	}
}
