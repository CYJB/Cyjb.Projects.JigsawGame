using System;
using SharpDX;
using SharpDX.Direct2D1;
using D2D1 = SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 表示一条三次贝塞尔曲线路径段。
	/// </summary>
	[Serializable]
	public sealed class BezierSegment : PathSegment
	{
		/// <summary>
		/// 使用指定的设置初始化 <see cref="BezierSegment"/> 类的新实例。
		/// </summary>
		/// <param name="end">三次贝塞尔曲线路径段的终结点。</param>
		/// <param name="point1">三次贝塞尔曲线路径的第一个控制点。</param>
		/// <param name="point2">三次贝塞尔曲线路径的第二个控制点。</param>
		public BezierSegment(Vector2 end, Vector2 point1, Vector2 point2)
			: base(PathType.Arc, end)
		{
			this.Point1 = point1;
			this.Point2 = point2;
		}
		/// <summary>
		/// 获取第一个控制点。
		/// </summary>
		public Vector2 Point1 { get; private set; }
		/// <summary>
		/// 获取第二个控制点。
		/// </summary>
		public Vector2 Point2 { get; private set; }
		/// <summary>
		/// 使用当前的路径填充指定的路径几何。
		/// </summary>
		/// <param name="sink">要填充的路径几何。</param>
		public override void FillGeometry(GeometrySink sink)
		{
			D2D1.BezierSegment bezier = new D2D1.BezierSegment()
			{
				Point1 = this.Point1,
				Point2 = this.Point2,
				Point3 = this.EndPoint
			};
			sink.AddBezier(bezier);
		}
	}
}
