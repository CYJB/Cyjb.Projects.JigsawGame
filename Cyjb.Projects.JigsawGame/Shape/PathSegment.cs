using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 表示一条路径段。
	/// </summary>
	[Serializable]
	public abstract class PathSegment
	{
		/// <summary>
		/// 使用指定的路径段类型和终结点初始化 <see cref="PathSegment"/> 类的新实例。
		/// </summary>
		/// <param name="type">路径段的类型。</param>
		/// <param name="end">路径的终结点。</param>
		protected PathSegment(PathType type, Vector2 end)
		{
			this.PathType = type;
			this.EndPoint = end;
		}
		/// <summary>
		/// 获取路径段的类型。
		/// </summary>
		public PathType PathType { get; private set; }
		/// <summary>
		/// 获取路径的终结点。
		/// </summary>
		public Vector2 EndPoint { get; private set; }
		/// <summary>
		/// 使用当前的路径填充指定的路径几何。
		/// </summary>
		/// <param name="sink">要填充的路径几何。</param>
		public abstract void FillGeometry(GeometrySink sink);
		/// <summary>
		/// 返回当前对象的字符串表示形式。
		/// </summary>
		/// <returns>当前对象的字符串表示形式。</returns>
		public override string ToString()
		{
			return string.Concat(PathType, " (", EndPoint.X, ", ", EndPoint.Y, ")");
		}
	}
}
