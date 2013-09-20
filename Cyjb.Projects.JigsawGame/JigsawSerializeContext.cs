using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 拼图反序列化的上下文。
	/// </summary>
	public sealed class JigsawSerializeContext
	{
		/// <summary>
		/// 设备管理器。
		/// </summary>
		private DeviceManager manager;
		/// <summary>
		/// 使用指定的设备管理器初始化 <see cref="JigsawSerializeContext"/> 类的新实例。
		/// </summary>
		/// <param name="manager">设备管理器。</param>
		public JigsawSerializeContext(DeviceManager manager)
		{
			this.manager = manager;
		}
		/// <summary>
		/// 获取 Direct2D 的工厂。
		/// </summary>
		public Factory Factory { get { return manager.D2DFactory; } }
		/// <summary>
		/// 获取 Direct2D 的设备上下文。
		/// </summary>
		public DeviceContext DeviceContext { get { return manager.D2DContext; } }
	}
}
