using System;
using System.Collections.Generic;
using Cyjb.Collections.ObjectModel;
using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 表示一条路径。
	/// </summary>
	[Serializable]
	public sealed class Path : ListBase<PathSegment>
	{
		/// <summary>
		/// 当前路径包含的图形的数目。
		/// </summary>
		private int figureCount = 1;
		/// <summary>
		/// 使用指定的起始点、重心和权重初始化 <see cref="Path"/> 类的新实例。
		/// </summary>
		/// <param name="start">路径的起始点。</param>
		public Path(Vector2 start)
		{
			this.StartPoint = start;
		}
		/// <summary>
		/// 获取路径的起始点。
		/// </summary>
		public Vector2 StartPoint { get; private set; }
		/// <summary>
		/// 获取或设置路径的重心。
		/// </summary>
		public Vector2 Center { get; set; }
		/// <summary>
		/// 获取路或设置径的权重。
		/// </summary>
		public float Weight { get; set; }
		/// <summary>
		/// 向当前路径中添加一条直线段。
		/// </summary>
		/// <param name="end">直线段的终结点。</param>
		public void AddLine(Vector2 end)
		{
			this.Add(new LineSegment(end));
		}
		/// <summary>
		/// 向当前路径中添加一条弧线段。
		/// </summary>
		/// <param name="end">弧线路径段的终结点。</param>
		/// <param name="size">弧线路径段的尺寸。</param>
		/// <param name="rotationAngel">弧线路径段的旋转角度。</param>
		/// <param name="sweepDirection">弧线路径段的绘制方向。</param>
		/// <param name="arcSize">弧线路径段是否大于 180 度。</param>
		public void AddArc(Vector2 end, Size2F size, float rotationAngel,
			SweepDirection sweepDirection, ArcSize arcSize)
		{
			this.Add(new ArcSegment(end, size, rotationAngel, sweepDirection, arcSize));
		}
		/// <summary>
		/// 向当前路径中添加一条三次贝塞尔曲线。
		/// </summary>
		/// <param name="end">三次贝塞尔曲线段的终结点。</param>
		/// <param name="point1">三次贝塞尔曲线路径的第一个控制点。</param>
		/// <param name="point2">三次贝塞尔曲线路径的第二个控制点。</param>
		public void AddBezier(Vector2 end, Vector2 point1, Vector2 point2)
		{
			this.Add(new BezierSegment(end, point1, point2));
		}
		/// <summary>
		/// 将指定集合中的路径段添加到当前集合中。
		/// </summary>
		/// <param name="other">要添加的路径段集合。</param>
		public void AddRange(IEnumerable<PathSegment> other)
		{
			foreach (PathSegment p in other)
			{
				this.Add(p);
			}
		}
		/// <summary>
		/// 根据当前的路径生成几何组。
		/// </summary>
		/// <param name="factory">Direct2D 工厂。</param>
		/// <returns>生成的几何组。</returns>
		public GeometryGroup GetGeometryGroup(Factory factory)
		{
			Geometry[] geometries = new Geometry[this.figureCount];
			PathGeometry path = new PathGeometry(factory);
			GeometrySink sink = path.Open();
			sink.SetFillMode(FillMode.Winding);
			sink.BeginFigure(this.StartPoint, FigureBegin.Filled);
			int cnt = this.Count;
			int idx = 0;
			for (int i = 0; i < cnt; i++)
			{
				EndFigureSegment end = this[i] as EndFigureSegment;
				if (end == null)
				{
					this[i].FillGeometry(sink);
				}
				else
				{
					sink.EndFigure(FigureEnd.Closed);
					sink.Close();
					geometries[idx++] = path;
					path = new PathGeometry(factory);
					sink = path.Open();
					sink.SetFillMode(FillMode.Winding);
					sink.BeginFigure(end.EndPoint, FigureBegin.Filled);
				}
			}
			sink.EndFigure(FigureEnd.Closed);
			sink.Close();
			geometries[idx++] = path;
			return new GeometryGroup(factory, FillMode.Winding, geometries);
		}
		/// <summary>
		/// 将指定的路径与当前的路径合并。
		/// </summary>
		/// <param name="path">要合并的路径。</param>
		public void Merge(Path path)
		{
			float sum = this.Weight + path.Weight;
			this.Center = new Vector2((this.Center.X * this.Weight + path.Center.X * path.Weight) / sum,
				(this.Center.Y * this.Weight + path.Center.Y * path.Weight) / sum);
			this.Weight = sum;
			this.Add(new EndFigureSegment(path.StartPoint));
			this.AddRange(path);
			this.figureCount += path.figureCount;
		}
	}
}
