using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 结束当前形状的路径段。
	/// </summary>
	[Serializable]
	public sealed class EndFigureSegment : PathSegment
	{
		/// <summary>
		/// 使用指定的终结点初始化 <see cref="EndFigureSegment"/> 类的新实例。
		/// </summary>
		/// <param name="end">形状结束段的终结点。</param>
		public EndFigureSegment(Vector2 endPoint)
			: base(PathType.EndFigure, endPoint)
		{ }
		/// <summary>
		/// 使用当前的路径填充指定的路径几何。
		/// </summary>
		/// <param name="sink">要填充的路径几何。</param>
		public override void FillGeometry(GeometrySink sink)
		{
			sink.EndFigure(FigureEnd.Closed);
			sink.BeginFigure(EndPoint, FigureBegin.Filled);
		}
	}
}
