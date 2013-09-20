using System;
using System.Diagnostics.CodeAnalysis;
using SharpDX.Direct2D1;
using D3D = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using WIC = SharpDX.WIC;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 设备的管理器。
	/// </summary>
	public sealed class DeviceManager : IDisposable
	{
		/// <summary>
		/// Direct3D 设备。
		/// </summary>
		private D3D.Device d3DDevice;
		/// <summary>
		/// Direct3D 11.1 设备。
		/// </summary>
		private D3D.Device1 d3DDevice1;
		/// <summary>
		/// DXGI 设备。
		/// </summary>
		private DXGI.Device dxgiDevice;
		/// <summary>
		/// Direct2D 设备。
		/// </summary>
		private Device d2DDevice;
		/// <summary>
		/// Direct2D 的设备上下文。
		/// </summary>
		private DeviceContext d2DContext;
		/// <summary>
		/// Direct2D 的工厂。
		/// </summary>
		private Factory d2DFactory;
		/// <summary>
		/// Windows 图片组件的工厂。
		/// </summary>
		private WIC.ImagingFactory wicFactory;
		/// <summary>
		/// Windows 图片组件的工厂。
		/// </summary>
		private WIC.ImagingFactory2 wicFactory2;
		/// <summary>
		/// 初始化 <see cref="DeviceManager"/> 类的新实例。
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public DeviceManager()
		{
			try
			{
				// 创建 D3D 设备。
				this.d3DDevice = new D3D.Device(SharpDX.Direct3D.DriverType.Hardware, D3D.DeviceCreationFlags.BgraSupport);
				this.d3DDevice1 = d3DDevice.QueryInterface<D3D.Device1>();
				this.dxgiDevice = d3DDevice1.QueryInterface<DXGI.Device>();
				// 创建 Direct2D 设备和工厂。
				this.d2DDevice = new Device(dxgiDevice);
				this.d2DContext = new DeviceContext(d2DDevice, DeviceContextOptions.None);
				this.d2DFactory = this.d2DContext.Factory;
				this.wicFactory = this.wicFactory2 = new WIC.ImagingFactory2();
			}
			catch (Exception)
			{
				this.ClearResources();
				try
				{
					// 创建 D3D 设备异常，则尝试 Direct2D 单线程工厂。
					this.d2DFactory = new Factory(FactoryType.SingleThreaded);
					this.wicFactory = new WIC.ImagingFactory();
				}
				catch (Exception)
				{
					this.ClearResources();
				}
			}
		}
		/// <summary>
		/// 获取 Direct2D 的设备上下文。
		/// </summary>
		public DeviceContext D2DContext
		{
			get { return this.d2DContext; }
		}
		/// <summary>
		/// 获取 Direct2D 的工厂。
		/// </summary>
		public Factory D2DFactory
		{
			get { return this.d2DFactory; }
		}
		/// <summary>
		/// 获取或设置 Direct2D 的渲染目标。
		/// </summary>
		public RenderTarget RenderTarget { get; set; }
		/// <summary>
		/// 获取 Windows 图片组件的工厂。
		/// </summary>
		public WIC.ImagingFactory WicFactory
		{
			get { return this.wicFactory; }
		}
		/// <summary>
		/// 获取 Windows 图片组件的工厂。
		/// </summary>
		public WIC.ImagingFactory2 WicFactory2
		{
			get { return this.wicFactory2; }
		}
		/// <summary>
		/// 获取是否支持 DirectX 3D 设备。
		/// </summary>
		public bool SupportD3D
		{
			get { return this.d2DContext != null; }
		}
		/// <summary>
		/// 获取是否支持 Direct2D 设备。
		/// </summary>
		public bool SupportD2D
		{
			get { return this.d2DFactory != null; }
		}

		#region IDisposable 成员

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		public void Dispose()
		{
			ClearResources();
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// 清理所有正在使用的资源，并设置为 <c>null</c>。
		/// </summary>
		private void ClearResources()
		{
			if (this.d3DDevice != null)
			{
				this.d3DDevice.Dispose();
				this.d3DDevice = null;
			}
			if (this.d3DDevice1 != null)
			{
				this.d3DDevice1.Dispose();
				this.d3DDevice1 = null;
			}
			if (this.dxgiDevice != null)
			{
				this.dxgiDevice.Dispose();
				this.dxgiDevice = null;
			}
			if (this.d2DDevice != null)
			{
				this.d2DDevice.Dispose();
				this.d2DDevice = null;
			}
			if (this.d2DContext != null)
			{
				this.d2DContext.Dispose();
				this.d2DContext = null;
			}
			if (this.d2DFactory != null)
			{
				this.d2DFactory.Dispose();
				this.d2DFactory = null;
			}
			if (this.wicFactory != null)
			{
				this.wicFactory.Dispose();
				this.wicFactory = null;
			}
			if (this.wicFactory2 != null)
			{
				this.wicFactory2.Dispose();
				this.wicFactory2 = null;
			}
		}

		#endregion // IDisposable 成员

	}
}
