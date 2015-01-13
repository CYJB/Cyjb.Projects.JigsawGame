using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Cyjb.Projects.JigsawGame.Shape;
using SharpDX;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Jigsaw
{
	/// <summary>
	/// 表示拼图碎片的集合。
	/// </summary>
	[Serializable]
	public sealed class JigsawPieceCollection : IEnumerable<JigsawPiece>, IDisposable, ISerializable
	{
		/// <summary>
		/// 允许拼图碎片可见的部分。
		/// </summary>
		private const int visibleMargin = 20;
		/// <summary>
		/// 拼图的缩放因子。
		/// </summary>
		private float scale = 1;
		/// <summary>
		/// 位于所有拼图碎片之前的拼图碎片。
		/// </summary>
		private JigsawPiece front;
		/// <summary>
		/// 位于所有拼图碎片之后的拼图碎片。
		/// </summary>
		private JigsawPiece back;
		/// <summary>
		/// 拼图碎片集合。
		/// </summary>
		private HashSet<JigsawPiece> items = new HashSet<JigsawPiece>();
		/// <summary>
		/// 使用指定的拼图形状初始化 <see cref="JigsawPieceCollection"/> 类的新实例。
		/// </summary>
		/// <param name="shape">拼图形状。</param>
		public JigsawPieceCollection(Factory factory, JigsawShape shape)
		{
			CommonExceptions.CheckArgumentNull(factory, "factory");
			CommonExceptions.CheckArgumentNull(shape, "shape");
			InitializePieces(factory, shape);
		}
		/// <summary>
		/// 用指定的序列化信息和上下文初始化 <see cref="JigsawPieceCollection"/> 类的新实例。
		/// </summary>
		/// <param name="info"><see cref="System.Runtime.Serialization.SerializationInfo"/> 对象，
		/// 包含序列化 <see cref="JigsawPieceCollection"/> 所需的信息。</param>
		/// <param name="context"><see cref="System.Runtime.Serialization.StreamingContext"/> 对象，
		/// 该对象包含与 <see cref="JigsawPieceCollection"/> 相关联的序列化流的源和目标。</param>
		/// <exception cref="System.ArgumentNullException">info 参数为 <c>null</c>。</exception>
		private JigsawPieceCollection(SerializationInfo info, StreamingContext context)
		{
			CommonExceptions.CheckArgumentNull(info, "info");
			this.scale = info.GetSingle("Scale");
			this.front = (JigsawPiece)info.GetValue("Front", typeof(JigsawPiece));
			this.back = (JigsawPiece)info.GetValue("Back", typeof(JigsawPiece));
			this.items = (HashSet<JigsawPiece>)info.GetValue("Items", typeof(HashSet<JigsawPiece>));
		}
		/// <summary>
		/// 初始化拼图碎片列表。
		/// </summary>
		/// <param name="factory">Direct2D 工厂。</param>
		/// <param name="shape">拼图形状。</param>
		private void InitializePieces(Factory factory, JigsawShape shape)
		{
			shape.GererateJigsawShape();
			JigsawPiece[] pieces = new JigsawPiece[shape.HorizontalDimension];
			JigsawPiece lastPiece = null;
			int idx = 0;
			for (int i = 0; i < shape.VerticalDimension; i++)
			{
				for (int j = 0; j < shape.HorizontalDimension; j++, idx++)
				{
					if (i == 0)
					{
						JigsawPiece currentPiece = new JigsawPiece(factory, shape.Paths[idx], JigsawPieceType.Border);
						items.Add(currentPiece);
						if (j == 0)
						{
							front = lastPiece = pieces[j] = currentPiece;
						}
						else
						{
							pieces[j] = currentPiece;
							currentPiece.Neighbors.Add(lastPiece);
							lastPiece.Neighbors.Add(currentPiece);
							currentPiece.Prev = lastPiece;
							lastPiece.Next = currentPiece;
							lastPiece = currentPiece;
						}
					}
					else
					{
						JigsawPieceType type = JigsawPieceType.Normal;
						if (j == 0 || i == shape.VerticalDimension - 1 || j == shape.HorizontalDimension - 1)
						{
							type = JigsawPieceType.Border;
						}
						JigsawPiece currentPiece = new JigsawPiece(factory, shape.Paths[idx], type);
						items.Add(currentPiece);
						if (j != 0)
						{
							currentPiece.Neighbors.Add(lastPiece);
							lastPiece.Neighbors.Add(currentPiece);
						}
						currentPiece.Prev = lastPiece;
						lastPiece.Next = currentPiece;
						currentPiece.Neighbors.Add(pieces[j]);
						pieces[j].Neighbors.Add(currentPiece);
						lastPiece = pieces[j] = currentPiece;
					}
				}
			}
			back = lastPiece;
		}

		#region IDisposable 成员

		/// <summary>
		/// 释放对象的资源。
		/// </summary>
		public void Dispose()
		{
			foreach (JigsawPiece p in items)
			{
				p.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		#endregion // IDisposable 成员

		/// <summary>
		/// 获取或设置拼图的缩放因子。
		/// </summary>
		public float Scale
		{
			get { return this.scale; }
			set
			{
				if (this.scale != value)
				{
					this.scale = value;
					foreach (JigsawPiece piece in this)
					{
						piece.Scale = value;
					}
				}
			}
		}
		/// <summary>
		/// 获取当前拼图碎片的个数。
		/// </summary>
		public int Count { get { return this.items.Count; } }
		/// <summary>
		/// 从当前集合中移除指定的拼图碎片。
		/// </summary>
		/// <param name="piece">要移除的拼图碎片。</param>
		private void Remove(JigsawPiece piece)
		{
			this.items.Remove(piece);
			this.RemoveFromList(piece);
			piece.Dispose();
		}

		#region 拼图碎片位置

		/// <summary>
		/// 将拼图碎片在给定范围内随机分布。
		/// </summary>
		/// <param name="rect">碎片的分布矩形。</param>
		/// <param name="rotatable">是否允许旋转拼图碎片。</param>
		public void SpreadPieces(RectangleF rect, bool rotatable)
		{
			foreach (JigsawPiece p in this)
			{
				if (rotatable)
				{
					p.Rotate = RandomExt.Next(4) * 90;
				}
				RectangleF bounds = p.Bounds;
				float x = RandomExt.NextSingle() * (rect.Width - bounds.Width) + rect.X - bounds.X + p.Offset.X;
				float y = RandomExt.NextSingle() * (rect.Height - bounds.Height) + rect.Y - bounds.Y + p.Offset.Y;
				p.Offset = new Vector2(x, y);
			}
		}
		/// <summary>
		/// 判断特定位置是否在某片拼图碎片中，并返回最上面的拼图碎片。
		/// </summary>
		/// <param name="point">要判断的位置。</param>
		/// <returns>给定位置所在的拼图碎片。</returns>
		public JigsawPiece GetPiece(Vector2 point)
		{
			foreach (JigsawPiece piece in this)
			{
				if (piece.Visible && piece.TestHit(point))
				{
					return piece;
				}
			}
			return null;
		}
		/// <summary>
		/// 获取与指定矩形相交的拼图碎片。
		/// </summary>
		/// <param name="rect">要判断的矩形。</param>
		/// <returns>与矩形相交的拼图碎片。</returns>
		public IEnumerable<JigsawPiece> GetPiece(RectangleF rect)
		{
			foreach (JigsawPiece piece in this)
			{
				if (piece.Visible && piece.TestHit(rect))
				{
					yield return piece;
				}
			}
		}
		/// <summary>
		/// 将指定的拼图碎片的收集到特定矩形范围内，只要有部分可见就允许。
		/// </summary>
		/// <param name="rect">矩形范围。</param>
		/// <param name="pieces">要收集的拼图碎片集合。</param>
		/// <returns>是否有拼图碎片的位置发生了改变。</returns>
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		public bool CollectInRectangle(RectangleF rect, IEnumerable<JigsawPiece> pieces)
		{
			bool changed = false;
			foreach (JigsawPiece piece in pieces)
			{
				float tx = 0, ty = 0;
				if (piece.Bounds.Left > rect.Right - visibleMargin)
				{
					tx = rect.Right - visibleMargin - piece.Bounds.Left;
				}
				else if (piece.Bounds.Right < rect.Left + visibleMargin)
				{
					tx = rect.Left + visibleMargin - piece.Bounds.Right;
				}
				if (piece.Bounds.Top > rect.Bottom - visibleMargin)
				{
					ty = rect.Bottom - visibleMargin - piece.Bounds.Top;
				}
				else if (piece.Bounds.Bottom < rect.Top + visibleMargin)
				{
					ty = rect.Top + visibleMargin - piece.Bounds.Bottom;
				}
				if (tx != 0 || ty != 0)
				{
					piece.Offset = new Vector2(piece.Offset.X + tx, piece.Offset.Y + ty);
					changed = true;
				}
			}
			return changed;
		}
		/// <summary>
		/// 将拼图碎片的收集到特定矩形范围内。
		/// </summary>
		/// <param name="rect">矩形范围。</param>
		/// <returns>是否有拼图碎片的位置发生了改变。</returns>
		public bool CollectInRectangle(RectangleF rect)
		{
			return CollectInRectangle(rect, this);
		}
		/// <summary>
		/// 重置所有拼图碎片为初始状态。
		/// </summary>
		public void Reset()
		{
			foreach (JigsawPiece piece in items)
			{
				piece.Rotate = 0;
				piece.Frozen = true;
				piece.Offset = new Vector2(0, 0);
				piece.State = JigsawPieceState.None;
			}
		}

		#endregion // 拼图碎片位置

		#region 合并拼图碎片

		/// <summary>
		/// 将指定的拼图碎片与其它可以合并的拼图碎片合并到一起。
		/// </summary>
		/// <param name="piece">要合并的拼图碎片。</param>
		/// <param name="radius">允许进行吸附的半径。</param>
		/// <returns>最后合并得到的拼图碎片。</returns>
		public JigsawPiece Merge(JigsawPiece piece, float radius)
		{
			JigsawPiece[] mergedPieces = GetCanMerge(piece, radius).ToArray();
			// 总是与权重更大的拼图碎片合并。
			for (int i = 0; i < mergedPieces.Length; i++)
			{
				if (mergedPieces[i] == piece)
				{
					continue;
				}
				JigsawPiece tmp = mergedPieces[i];
				if (tmp.Frozen || tmp.Weight > piece.Weight)
				{
					tmp = piece;
					piece = mergedPieces[i];
				}
				piece.Merge(tmp);
				this.Remove(tmp);
			}
			return piece;
		}
		/// <summary>
		/// 获取所有可以与指定拼图碎片合并的拼图碎片。
		/// </summary>
		/// <param name="piece">要判断的拼图碎片。</param>
		/// <param name="radius">允许进行吸附的半径。</param>
		/// <returns>可以合并的拼图碎片。</returns>
		private IEnumerable<JigsawPiece> GetCanMerge(JigsawPiece piece, float radius)
		{
			foreach (JigsawPiece p in piece.Neighbors)
			{
				if (p.Visible && piece.CanMerge(p, radius))
				{
					yield return p;
				}
			}
		}

		#endregion // 合并拼图碎片

		#region 拼图碎片 Z 顺序

		/// <summary>
		/// 将指定的拼图碎片提升到其他拼图碎片的前面。
		/// </summary>
		/// <param name="piece">要提升的拼图碎片。</param>
		public void BringToFront(JigsawPiece piece)
		{
			if (piece.Prev != null)
			{
				Debug.Assert(front != piece);
				RemoveFromList(piece);
				Debug.Assert(front != null);
				Debug.Assert(back != null);
				Debug.Assert(piece.Prev == null);
				Debug.Assert(piece.Next == null);
				piece.Next = front;
				front.Prev = piece;
				front = piece;
			}
		}
		/// <summary>
		/// 将指定的拼图碎片提升到其他拼图碎片的前面。
		/// </summary>
		/// <param name="pieces">要提升的拼图碎片。</param>
		public void BringToFront(IEnumerable<JigsawPiece> pieces)
		{
			JigsawPiece tFront = null, tBack = null;
			foreach (JigsawPiece piece in pieces)
			{
				RemoveFromList(piece);
				if (tFront == null)
				{
					tFront = tBack = piece;
				}
				else
				{
					tBack.Next = piece;
					piece.Prev = tBack;
					tBack = piece;
				}
			}
			if (tFront != null)
			{
				Debug.Assert(tBack != null);
				tBack.Next = front;
				if (front == null)
				{
					back = tBack;
				}
				else
				{
					front.Prev = tBack;
				}
				front = tFront;
			}
		}
		/// <summary>
		/// 将指定的拼图碎片下降到其他拼图碎片的后面。
		/// </summary>
		/// <param name="piece">要下降的拼图碎片。</param>
		public void BringToBack(JigsawPiece piece)
		{
			if (piece.Next != null)
			{
				Debug.Assert(back != piece);
				RemoveFromList(piece);
				Debug.Assert(front != null);
				Debug.Assert(back != null);
				Debug.Assert(piece.Prev == null);
				Debug.Assert(piece.Next == null);
				piece.Prev = back;
				back.Next = piece;
				back = piece;
			}
		}
		/// <summary>
		/// 将指定的拼图碎片下降到其他拼图碎片的后面。
		/// </summary>
		/// <param name="pieces">要下降的拼图碎片。</param>
		public void BringToBack(IEnumerable<JigsawPiece> pieces)
		{
			JigsawPiece tFront = null, tBack = null;
			foreach (JigsawPiece piece in pieces)
			{
				RemoveFromList(piece);
				if (tFront == null)
				{
					tFront = tBack = piece;
				}
				else
				{
					tFront.Prev = piece;
					piece.Next = tFront;
					tFront = piece;
				}
			}
			if (tFront != null)
			{
				Debug.Assert(tBack != null);
				tFront.Prev = back;
				if (back == null)
				{
					front = tFront;
				}
				else
				{
					back.Next = tFront;
				}
				back = tBack;
			}
		}
		/// <summary>
		/// 将指定的拼图碎片从链表中摘除。
		/// </summary>
		/// <param name="item">要摘除的拼图碎片。</param>
		private void RemoveFromList(JigsawPiece item)
		{
			if (item.Prev == null)
			{
				Debug.Assert(front == item);
				front = item.Next;
				if (front != null)
				{
					front.Prev = null;
				}
			}
			else
			{
				item.Prev.Next = item.Next;
			}
			if (item.Next == null)
			{
				Debug.Assert(back == item);
				back = item.Prev;
				if (back != null)
				{
					back.Next = null;
				}
			}
			else
			{
				item.Next.Prev = item.Prev;
			}
			item.Prev = item.Next = null;
		}

		#endregion // 拼图碎片 Z 顺序

		#region IEnumerable<JigsawPiece> 成员

		/// <summary>
		/// 返回一个循环访问集合的枚举器。
		/// </summary>
		/// <returns>可用于循环访问集合的 <see cref="System.Collections.Generic.IEnumerator&lt;T&gt;"/>。</returns>
		public IEnumerator<JigsawPiece> GetEnumerator()
		{
			JigsawPiece p = front;
			while (p != null)
			{
				yield return p;
				p = p.Next;
			}
		}

		#endregion // IEnumerable<JigsawPiece> 成员

		#region IEnumerable 成员

		/// <summary>
		/// 返回一个循环访问集合的枚举器。
		/// </summary>
		/// <returns>可用于循环访问集合的 <see cref="System.Collections.IEnumerator"/>。</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion // IEnumerable 成员

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
			CommonExceptions.CheckArgumentNull(info, "info");
			info.AddValue("Scale", this.scale);
			info.AddValue("Front", this.front);
			info.AddValue("Back", this.back);
			info.AddValue("Items", this.items);
		}

		#endregion

	}
}
