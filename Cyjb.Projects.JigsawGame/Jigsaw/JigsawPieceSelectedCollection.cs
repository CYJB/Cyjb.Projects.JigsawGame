using System.Collections;
using System.Collections.Generic;

namespace Cyjb.Projects.JigsawGame.Jigsaw
{
	/// <summary>
	/// 表示被选择的拼图碎片集合。
	/// </summary>
	public sealed class JigsawPieceSelectedCollection : IEnumerable<JigsawPiece>
	{
		/// <summary>
		/// 拼图碎片集合。
		/// </summary>
		private HashSet<JigsawPiece> items = new HashSet<JigsawPiece>();
		/// <summary>
		/// 是否选择了单个拼图碎片。
		/// </summary>
		private bool singleSelected = false;
		/// <summary>
		/// 获取是否选择了单个拼图碎片。
		/// </summary>
		public bool SingleSelected { get { return singleSelected; } }
		/// <summary>
		/// 将指定的拼图碎片添加到被选择的集合中。
		/// </summary>
		/// <param name="piece">要添加的拼图碎片。</param>
		public void Add(JigsawPiece piece)
		{
			piece.State |= JigsawPieceState.Selected;
			items.Add(piece);
			singleSelected = true;
		}
		/// <summary>
		/// 将指定的拼图碎片集合添加到被选择的集合中。
		/// </summary>
		/// <param name="pieces">要添加的拼图碎片集合。</param>
		public void AddRange(IEnumerable<JigsawPiece> pieces)
		{
			foreach (JigsawPiece piece in pieces)
			{
				if (!piece.Frozen)
				{
					piece.State |= JigsawPieceState.Selected;
					items.Add(piece);
				}
			}
			singleSelected = false;
		}
		/// <summary>
		/// 清除所有被选择的拼图碎片。
		/// </summary>
		public void Clear()
		{
			foreach (JigsawPiece piece in this.items)
			{
				piece.State &= ~JigsawPieceState.Selected;
			}
			this.items.Clear();
			singleSelected = false;
		}
		/// <summary>
		/// 返回当前集合中是否包含给定的拼图碎片。
		/// </summary>
		/// <param name="piece">要测试的拼图碎片。</param>
		/// <returns>如果指定的拼图碎片包含在集合中，则为 <c>true</c>；
		/// 否则为 <c>false</c>。</returns>
		public bool Contains(JigsawPiece piece)
		{
			return this.items.Contains(piece);
		}
		/// <summary>
		/// 修改当前的集合，以仅包含该对象和指定集合中存在的元素。
		/// </summary>
		/// <param name="other">要与当前的集合进行比较的集合。</param>
		public void IntersectWith(IEnumerable<JigsawPiece> other)
		{
			this.items.IntersectWith(other);
		}

		#region IEnumerable<JigsawPiece> 成员

		/// <summary>
		/// 返回一个循环访问集合的枚举器。
		/// </summary>
		/// <returns>可用于循环访问集合的 <see cref="System.Collections.Generic.IEnumerator&lt;T&gt;"/>。</returns>
		public IEnumerator<JigsawPiece> GetEnumerator()
		{
			return this.items.GetEnumerator();
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

	}
}
