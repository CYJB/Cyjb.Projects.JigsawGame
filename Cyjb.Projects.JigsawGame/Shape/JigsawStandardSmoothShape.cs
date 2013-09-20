using SharpDX;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 拼图的标准平滑形状。
	/// </summary>
	public sealed class JigsawStandardSmoothShape : JigsawShape
	{
		/// <summary>
		/// 初始化 <see cref="JigsawStandardSmoothShape"/> 的新实例。
		/// </summary>
		public JigsawStandardSmoothShape() : base(5) { }
		/// <summary>
		/// 向拼图碎片的路径中添加一条边，路径的当前节点总是在起始点。
		/// </summary>
		/// <param name="path">路径。</param>
		/// <param name="startPoint">边的起始点。</param>
		/// <param name="endPoint">边的结束点。</param>
		/// <param name="randoms">该边的凹凸性。</param>
		/// <param name="border">与该条边相关的一组随机数，范围都是 [0, 1)。</param>
		protected override void AddBorder(Path path, Vector2 startPoint, Vector2 endPoint, bool border, float[] randoms)
		{
			// 贝塞尔曲线的起始和结束点。
			float rate1 = 1f / 3 + randoms[0] / 12;
			Vector2 sp = Vector2.Lerp(startPoint, endPoint, rate1);
			Vector2 ep = Vector2.Lerp(startPoint, endPoint, 1 - rate1);
			// 贝塞尔曲线的控制点。
			float rate2 = rate1 - randoms[1] / 4;
			Vector2 c1 = Vector2.Lerp(startPoint, endPoint, rate2);
			Vector2 c2 = Vector2.Lerp(startPoint, endPoint, 1 - rate2);
			// 与边框垂直的向量。
			Vector2 cross = new Vector2(endPoint.Y - startPoint.Y, startPoint.X - endPoint.X);
			if (!border)
			{
				cross.X *= -1;
				cross.Y *= -1;
			}
			cross *= 1f / 4 + randoms[2] / 8;
			c1 += cross;
			c2 += cross;
			// 连接边框的贝塞尔曲线。
			float rate3 = 1f / 6 + randoms[3] / 6;
			Vector2 sc2 = sp + (sp - c1) * rate3;
			Vector2 sc1 = Vector2.Lerp(startPoint, sc2, randoms[4]);
			Vector2 ec1 = ep + (ep - c2) * rate3;
			Vector2 ec2 = Vector2.Lerp(endPoint, ec1, randoms[4]);
			path.AddBezier(sp, sc1, sc2);
			path.AddBezier(ep, c1, c2);
			path.AddBezier(endPoint, ec1, ec2);
		}
	}
}
