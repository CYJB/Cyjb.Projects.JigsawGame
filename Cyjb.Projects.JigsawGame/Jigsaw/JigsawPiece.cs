using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Cyjb.Projects.JigsawGame.Shape;
using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Jigsaw
{
	/// <summary>
	/// 表示拼图的碎片。
	/// </summary>
	[Serializable]
	public sealed class JigsawPiece : IDisposable, ISerializable
	{
		/// <summary>
		/// Direc2D 工厂实例。
		/// </summary>
		private Factory factory;
		/// <summary>
		/// 拼图形状。
		/// </summary>
		private Path shape;
		/// <summary>
		/// 原始轮廓路径，是原始坐标。
		/// </summary>
		private GeometryGroup originalPath;
		/// <summary>
		/// 当前轮廓路径，被变换到了拼图坐标。
		/// </summary>
		private TransformedGeometry path;
		/// <summary>
		/// 相邻的拼图碎片。
		/// </summary>
		private HashSet<JigsawPiece> neighbors = new HashSet<JigsawPiece>();
		/// <summary>
		/// 拼图碎片的变换矩阵。
		/// </summary>
		private Matrix3x2 matrix = Matrix3x2.Scaling(1f);
		/// <summary>
		/// 拼图碎片的类型。
		/// </summary>
		private JigsawPieceType pieceType = JigsawPieceType.Normal;
		/// <summary>
		/// 拼图碎片的位置偏移量。
		/// </summary>
		private Vector2 offset = new Vector2();
		/// <summary>
		/// 拼图碎片的旋转角度。
		/// </summary>
		private int rotate = 0;
		/// <summary>
		/// 拼图碎片的旋转弧度。
		/// </summary>
		private float rotateRadian = 0;
		/// <summary>
		/// 拼图碎片的缩放。
		/// </summary>
		private float scale = 1f;
		/// <summary>
		/// 拼图碎片的边界。
		/// </summary>
		private RectangleF bounds;
		/// <summary>
		/// 初始化 <see cref="JigsawPiece"/> 类的新实例。
		/// </summary>
		/// <param name="factory">Direc2D 工厂实例。</param>
		/// <param name="path">拼图碎片的轮廓路径。</param>
		/// <param name="type">拼图碎片的类型。</param>
		public JigsawPiece(Factory factory, Path path, JigsawPieceType type)
		{
			if (factory == null)
			{
				throw CommonExceptions.ArgumentNull("factory");
			}
			if (path == null)
			{
				throw CommonExceptions.ArgumentNull("path");
			}
			this.factory = factory;
			this.shape = path;
			this.originalPath = this.shape.GetGeometryGroup(this.factory);
			this.pieceType = type;
			this.Visible = true;
			this.Frozen = false;
			this.State = JigsawPieceState.None;
			UpdatePath();
		}
		/// <summary>
		/// 用指定的序列化信息和上下文初始化 <see cref="JigsawPiece"/> 类的新实例。
		/// </summary>
		/// <param name="info"><see cref="System.Runtime.Serialization.SerializationInfo"/> 对象，
		/// 包含序列化 <see cref="JigsawPiece"/> 所需的信息。</param>
		/// <param name="context"><see cref="System.Runtime.Serialization.StreamingContext"/> 对象，
		/// 该对象包含与 <see cref="JigsawPiece"/> 相关联的序列化流的源和目标。</param>
		/// <exception cref="System.ArgumentNullException">info 参数为 <c>null</c>。</exception>
		private JigsawPiece(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw CommonExceptions.ArgumentNull("info");
			}
			this.factory = ((JigsawSerializeContext)context.Context).Factory;
			this.shape = (Path)info.GetValue("Shape", typeof(Path));
			this.pieceType = (JigsawPieceType)info.GetValue("Type", typeof(JigsawPieceType));
			this.offset = (Vector2)info.GetValue("Offset", typeof(Vector2));
			this.rotate = info.GetInt32("Rotate");
			this.rotateRadian = (float)(this.rotate * Math.PI / 180);
			this.scale = info.GetSingle("Scale");
			this.Next = (JigsawPiece)info.GetValue("Next", typeof(JigsawPiece));
			this.Prev = (JigsawPiece)info.GetValue("Prev", typeof(JigsawPiece));
			this.Frozen = info.GetBoolean("Frozen");
			this.neighbors = (HashSet<JigsawPiece>)info.GetValue("Neighbors", typeof(HashSet<JigsawPiece>));
			this.Visible = true;
			this.State = JigsawPieceState.None;
			// 重新填充路径。
			this.originalPath = this.shape.GetGeometryGroup(factory);
			// 重新计算转换矩阵。
			CalculateMatrix();
			UpdatePath();
		}
		/// <summary>
		/// 获取原始轮廓路径。
		/// </summary>
		public GeometryGroup OriginalPath { get { return this.originalPath; } }
		/// <summary>
		/// 获取当前轮廓路径。
		/// </summary>
		public Geometry Path { get { return this.path; } }
		/// <summary>
		/// 获取用于坐标变换的矩阵。
		/// </summary>
		public Matrix3x2 TransformMatrix { get { return this.matrix; } }
		/// <summary>
		/// 获取拼图碎片的外边框。
		/// </summary>
		public RectangleF Bounds { get { return this.bounds; } }
		/// <summary>
		/// 获取拼图碎片的类型。
		/// </summary>
		public JigsawPieceType PieceType { get { return this.pieceType; } }
		/// <summary>
		/// 获取或设置拼图碎片所处的状态。
		/// </summary>
		public JigsawPieceState State { get; set; }
		/// <summary>
		/// 获取或设置当前拼图碎片的可见性。
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// 获取或设置当前拼图碎片是否被冻结。
		/// </summary>
		public bool Frozen { get; set; }
		/// <summary>
		/// 获取相邻的拼图碎片。
		/// </summary>
		public HashSet<JigsawPiece> Neighbors
		{
			get { return this.neighbors; }
		}
		/// <summary>
		/// 获取或设置拼图碎片的位置偏移量。
		/// </summary>
		public Vector2 Offset
		{
			get { return this.offset; }
			set
			{
				this.SetOffset(value.X, value.Y);
				this.UpdatePath();
			}
		}
		/// <summary>
		/// 获取或设置拼图碎片的旋转弧度。
		/// </summary>
		public int Rotate
		{
			get { return this.rotate; }
			set
			{
				this.rotate = value;
				this.rotateRadian = (float)(value * Math.PI / 180);
				this.CalculateMatrix();
				this.UpdatePath();
			}
		}
		/// <summary>
		/// 获取或设置拼图碎片的缩放比例。
		/// </summary>
		public float Scale
		{
			get { return this.scale; }
			set
			{
				float rate = value / this.scale;
				this.scale = value;
				this.SetOffset(this.offset.X * rate, this.offset.Y * rate);
				this.CalculateMatrix();
				this.UpdatePath();
			}
		}
		/// <summary>
		/// 获取当前拼图碎片的权重，越大的拼图碎片权重越大。
		/// </summary>
		public float Weight { get { return this.shape.Weight; } }
		/// <summary>
		/// 获取或设置当前拼图碎片链表中的前一位置的拼图碎片。
		/// </summary>
		public JigsawPiece Prev { get; set; }
		/// <summary>
		/// 获取或设置当前拼图碎片链表中的后一位置的拼图碎片。
		/// </summary>
		public JigsawPiece Next { get; set; }
		/// <summary>
		/// 设置当前的拼图碎片的偏移量。
		/// </summary>
		/// <param name="x">拼图碎片的 X 偏移量。</param>
		/// <param name="y">拼图碎片的 Y 偏移量。</param>
		private void SetOffset(float x, float y)
		{
			this.matrix.M31 += x - this.offset.X;
			this.matrix.M32 += y - this.offset.Y;
			this.offset.X = x;
			this.offset.Y = y;
		}
		/// <summary>
		/// 将指定的拼图原始坐标转换为拼图坐标。
		/// </summary>
		/// <param name="point">要转换的点。</param>
		/// <returns>转换得到的结果。</returns>
		private Vector2 PointToJigsaw(Vector2 point)
		{
			return Matrix3x2.TransformPoint(this.TransformMatrix, point);
		}
		/// <summary>
		/// 计算转换矩阵。
		/// </summary>
		private void CalculateMatrix()
		{
			matrix = new Matrix3x2(1, 0, 0, 1, -shape.Center.X, -shape.Center.Y);
			if (rotate != 0)
			{
				matrix *= Matrix3x2.Rotation(this.rotateRadian);
			}
			if (this.scale != 1)
			{
				matrix *= Matrix3x2.Scaling(scale);
			}
			matrix *= Matrix3x2.Translation(offset.X + scale * shape.Center.X, offset.Y + scale * shape.Center.Y);
		}
		/// <summary>
		/// 更新当前轮廓路径。
		/// </summary>
		private void UpdatePath()
		{
			if (this.path != null)
			{
				this.path.Dispose();
			}
			this.path = new TransformedGeometry(factory, this.originalPath, this.matrix);
			this.bounds = this.path.GetBounds();
		}
		/// <summary>
		/// 判断特定点是否在拼图碎片中。
		/// </summary>
		/// <param name="pos">要判断的点。</param>
		/// <returns>点是否在拼图碎片中。</returns>
		public bool TestHit(Vector2 pos)
		{
			if (pos.X >= bounds.X && pos.Y >= bounds.Y && pos.X <= bounds.Right && pos.Y <= bounds.Bottom)
			{
				return this.path.FillContainsPoint(pos);
			}
			return false;
		}
		/// <summary>
		/// 判断当前拼图碎片与指定矩形是否相交。
		/// </summary>
		/// <param name="rect">要判断的矩形。</param>
		/// <returns>是否相交。</returns>
		public bool TestHit(RectangleF rect)
		{
			if (this.bounds.Right < rect.X)
			{
				return false;
			}
			if (this.bounds.X > rect.Right)
			{
				return false;
			}
			if (this.bounds.Bottom < rect.Y)
			{
				return false;
			}
			if (this.bounds.Y > rect.Bottom)
			{
				return false;
			}
			RectangleGeometry rectGeom = new RectangleGeometry(this.factory, rect);
			return this.path.Compare(rectGeom) != GeometryRelation.Disjoint;
		}
		/// <summary>
		/// 判断指定的拼图碎片是否可以与当前碎片合并。
		/// </summary>
		/// <param name="piece">要判断合并的拼图碎片。</param>
		/// <param name="radius">允许进行吸附的半径。</param>
		/// <returns>是否能合并。</returns>
		public bool CanMerge(JigsawPiece piece, float radius)
		{
			if (piece == this)
			{
				return true;
			}
			if (piece.rotate != this.rotate)
			{
				return false;
			}
			// 在拼图坐标系下比较 (0, 0) 点的距离。
			Vector2 point = new Vector2();
			return radius * radius >= Vector2.DistanceSquared(this.PointToJigsaw(point), piece.PointToJigsaw(point));
		}
		/// <summary>
		/// 将指定的拼图碎片与当前的拼图碎片合并。不检查两个拼图碎片是否可以被合并。
		/// </summary>
		/// <param name="piece">要合并的拼图碎片。</param>
		public void Merge(JigsawPiece piece)
		{
			// 更新相邻拼图碎片信息。
			foreach (JigsawPiece p in piece.neighbors)
			{
				p.neighbors.Remove(piece);
				p.neighbors.Add(this);
			}
			this.neighbors.UnionWith(piece.neighbors);
			this.neighbors.Remove(this);
			// 更新形状。
			float sum = this.shape.Weight + piece.shape.Weight;
			this.offset = new Vector2((this.offset.X * this.shape.Weight + piece.offset.X * piece.shape.Weight) / sum,
				(offset.Y * this.shape.Weight + piece.offset.Y * piece.shape.Weight) / sum);
			this.shape.Merge(piece.shape);
			// 更新路径。
			GeometryGroup newGroup = SharpDXUtility.Merge(this.originalPath, piece.originalPath);
			this.originalPath.Dispose();
			this.originalPath = newGroup;
			this.CalculateMatrix();
			this.UpdatePath();
			if (piece.PieceType == JigsawPieceType.Border)
			{
				this.pieceType = JigsawPieceType.Border;
			}
			piece.Dispose();
		}
		/// <summary>
		/// 返回拼图碎片中包含的形状的颜色（黑/白）。
		/// </summary>
		/// <returns>表示颜色的数组。</returns>
		public bool[] GetColors()
		{
			return this.shape.GetColors();
		}

		#region IDisposable 成员

		/// <summary>
		/// 回收当前对象使用的资源。
		/// </summary>
		public void Dispose()
		{
			this.originalPath.Dispose();
			if (this.path != null)
			{
				this.path.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		#endregion

		#region ISerializable 成员

		/// <summary>
		/// 使用将目标对象序列化所需的数据填充 <see cref="System.Runtime.Serialization.SerializationInfo"/>。
		/// </summary>
		/// <param name="info">要填充数据的 <see cref="System.Runtime.Serialization.SerializationInfo"/>。</param>
		/// <param name="context">此序列化的目标。</param>
		/// <exception cref="System.Security.SecurityException">调用方没有所要求的权限。</exception>
		/// <exception cref="System.ArgumentNullException">info 参数为 <c>null</c>。</exception>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw CommonExceptions.ArgumentNull("info");
			}
			info.AddValue("Shape", this.shape);
			info.AddValue("Type", this.pieceType);
			info.AddValue("Offset", this.offset);
			info.AddValue("Rotate", this.rotate);
			info.AddValue("Scale", this.scale);
			info.AddValue("Neighbors", this.neighbors);
			info.AddValue("Next", this.Next);
			info.AddValue("Prev", this.Prev);
			info.AddValue("Frozen", this.Frozen);
		}

		#endregion

	}
}
