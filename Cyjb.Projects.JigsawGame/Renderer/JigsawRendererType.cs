using System.ComponentModel;

namespace Cyjb.Projects.JigsawGame.Renderer
{
	/// <summary>
	/// 拼图渲染器的类型。
	/// </summary>
	public enum JigsawRendererType
	{
		/// <summary>
		/// 简单渲染器。
		/// </summary>
		[Description("简单渲染器")]
		Simple,
		/// <summary>
		/// 特效渲染器。
		/// </summary>
		[Description("特效渲染器")]
		Effect
	}
}
