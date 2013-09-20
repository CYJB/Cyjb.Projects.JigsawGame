using System;
using SharpDX;
using SharpDX.Direct2D1;
using D2D1 = SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 表示一条弧线路径段。
	/// </summary>
	[Serializable]
	public sealed class ArcSegment : PathSegment
	{
		/// <summary>
		/// 使用指定的设置初始化 <see cref="ArcSegment"/> 类的新实例。
		/// </summary>
		/// <param name="end">弧线路径段的终结点。</param>
		/// <param name="size">弧线路径段的尺寸。</param>
		/// <param name="rotationAngel">弧线路径段的旋转角度。</param>
		/// <param name="sweepDirection">弧线路径段的绘制方向。</param>
		/// <param name="arcSize">弧线路径段是否大于 180 度。</param>
		public ArcSegment(Vector2 end, Size2F size, float rotationAngel,
			SweepDirection sweepDirection, ArcSize arcSize)
			: base(PathType.Arc, end)
		{
			this.Size = size;
			this.RotationAngle = rotationAngel;
			this.SweepDirection = sweepDirection;
			this.ArcSize = arcSize;
		}
		/// <summary>
		/// 获取弧线路径段的尺寸。
		/// </summary>
		public Size2F Size { get; private set; }
		/// <summary>
		/// 获取弧线路径段的旋转角度。
		/// </summary>
		public float RotationAngle { get; private set; }
		/// <summary>
		/// 获取弧线路径段的绘制方向。
		/// </summary>
		public SweepDirection SweepDirection { get; private set; }
		/// <summary>
		/// 获取弧线路径段是否大于 180 度。
		/// </summary>
		public ArcSize ArcSize { get; private set; }
		/// <summary>
		/// 使用当前的路径填充指定的路径几何。
		/// </summary>
		/// <param name="sink">要填充的路径几何。</param>
		public override void FillGeometry(GeometrySink sink)
		{
			D2D1.ArcSegment arc = new D2D1.ArcSegment()
			{
				Point = EndPoint,
				Size = Size,
				RotationAngle = RotationAngle,
				SweepDirection = SweepDirection,
				ArcSize = ArcSize
			};
			sink.AddArc(arc);
		}
	}
}
