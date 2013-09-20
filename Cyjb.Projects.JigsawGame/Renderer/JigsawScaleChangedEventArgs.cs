using System;

namespace Cyjb.Projects.JigsawGame.Renderer
{
	/// <summary>
	/// 拼图缩放比例被改变的事件参数。
	/// </summary>
	public sealed class JigsawScaleChangedEventArgs : EventArgs
	{
		/// <summary>
		/// 初始化 <see cref="JigsawScaleChangedEventArgs"/> 类的新实例。
		/// </summary>
		public JigsawScaleChangedEventArgs()
		{ }
		/// <summary>
		/// 使用指定的缩放比例初始化 <see cref="JigsawScaleChangedEventArgs"/> 类的新实例。
		/// </summary>
		/// <param name="scale">拼图的缩放比例。</param>
		public JigsawScaleChangedEventArgs(float scale)
		{
			this.Scale = scale;
		}
		/// <summary>
		/// 获取或设置拼图的缩放比例。
		/// </summary>
		public float Scale { get; set; }
	}
}
