using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 拼图的标准圆形。
	/// </summary>
	public sealed class JigsawStandardCircleShape : JigsawShape
	{
		/// <summary>
		/// 初始化 <see cref="JigsawStandardCircleShape"/> 的新实例。
		/// </summary>
		public JigsawStandardCircleShape() : base(0) { }
		/// <summary>
		/// 向拼图碎片的路径中添加一条边，路径的当前节点总是在起始点。
		/// </summary>
		/// <param name="path">路径。</param>
		/// <param name="startPoint">边的起始点。</param>
		/// <param name="endPoint">边的结束点。</param>
		/// <param name="randoms">该边的凹凸性。</param>
		/// <param name="border">与该条边相关的一组随机数，范围都是 [0, 1)。</param>
		protected override void AddBorder(Path path, Vector2 startPoint, Vector2 endPoint,
			bool border, float[] randoms)
		{
			Vector2 sp = Vector2.Lerp(startPoint, endPoint, 1f / 3);
			Vector2 ep = Vector2.Lerp(startPoint, endPoint, 2f / 3);
			path.AddLine(sp);
			double angle = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X) * 180 / Math.PI + 180;
			float len = Vector2.Distance(startPoint, endPoint) / 6;
			path.AddArc(ep, new Size2F(len, len), (float)angle,
				border ? SweepDirection.CounterClockwise : SweepDirection.Clockwise, ArcSize.Small);
			path.AddLine(endPoint);
		}
	}
}
