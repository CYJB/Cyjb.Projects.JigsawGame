using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 包含 SharpDX 的实用方法。
	/// </summary>
	public static class SharpDXUtility
	{

		#region 几何操作

		/// <summary>
		/// 获取给定点集合中心（坐标平均值）。
		/// </summary>
		/// <param name="args">要计算的点集合。</param>
		/// <returns>点集合的中心。</returns>
		public static Vector2 GetCenter(params Vector2[] args)
		{
			float x = 0, y = 0;
			for (int i = 0; i < args.Length; i++)
			{
				x += args[i].X;
				y += args[i].Y;
			}
			return new Vector2(x / args.Length, y / args.Length);
		}
		/// <summary>
		/// 计算三角形的面积（的二倍）。
		/// </summary>
		/// <param name="a">三角形的第一个顶点坐标。</param>
		/// <param name="b">三角形的第二个顶点坐标。</</param>
		/// <param name="c">三角形的第三个顶点坐标。</</param>
		/// <returns></returns>
		public static float Area(Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float area = (p1.X - p2.X) * (p1.Y - p3.Y) - (p1.X - p3.X) * (p1.Y - p2.Y);
			return area < 0 ? -area : area;
		}
		/// <summary>
		/// 将给定的几何图形组合并为一个组。
		/// </summary>
		/// <param name="geometies">要合并集合图形组。</param>
		/// <returns>合并得到的集合图形组。</returns>
		public static GeometryGroup Merge(params GeometryGroup[] geometies)
		{
			int[] idx = new int[geometies.Length];
			for (int i = 0; i < geometies.Length; i++)
			{
				if (i == 0)
				{
					idx[i] = geometies[i].SourceGeometryCount;
				}
				else
				{
					idx[i] = idx[i - 1] + geometies[i].SourceGeometryCount;
				}
			}
			Geometry[] geoms = new Geometry[idx[idx.Length - 1]];
			for (int i = 0; i < geometies.Length; i++)
			{
				if (i == 0)
				{
					geometies[i].GetSourceGeometry().CopyTo(geoms, 0);
				}
				else
				{
					geometies[i].GetSourceGeometry().CopyTo(geoms, idx[i - 1]);
				}
			}
			return new GeometryGroup(geometies[0].Factory, FillMode.Winding, geoms);
		}

		#endregion // 几何操作

		#region 颜色操作

		/// <summary>
		/// 将指定的 <see cref="System.Drawing.Color"/> 对象转换为等价的 
		/// <see cref="SharpDX.Color4"/> 对象。
		/// </summary>
		/// <param name="color">要转换的 <see cref="System.Drawing.Color"/> 对象。</param>
		/// <returns>转换得到的 <see cref="SharpDX.Color4"/> 对象。</returns>
		public static Color4 ToColor4(this System.Drawing.Color color)
		{
			uint argb = (uint)color.ToArgb();
			// Color4 的 R, G, B, A 是从小端到大端排列。
			uint rgba = (argb & 0xFF00FF00) | ((argb >> 16) & 0x000000FF) | ((argb << 16) & 0x00FF0000);
			return new Color4(rgba);
		}
		/// <summary>
		/// 将指定的 <see cref="SharpDX.Color4"/> 对象转换为等价的 <see cref="System.Drawing.Color"/> 对象。
		/// </summary>
		/// <param name="color">要转换的 <see cref="SharpDX.Color4"/> 对象。</param>
		/// <returns>转换得到的 <see cref="System.Drawing.Color"/> 对象。</returns>
		public static System.Drawing.Color ToColor(this Color4 color)
		{
			uint rgba = (uint)color.ToRgba();
			// Color4 的 R, G, B, A 是从小端到大端排列。
			uint argb = (rgba & 0xFF00FF00) | ((rgba >> 16) & 0x000000FF) | ((rgba << 16) & 0x00FF0000);
			return System.Drawing.Color.FromArgb((int)argb);
		}

		#endregion // 颜色操作

	}
}
