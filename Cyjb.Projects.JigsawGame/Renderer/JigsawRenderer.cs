using System;
using System.Collections.Generic;
using System.Threading;
using Cyjb.Projects.JigsawGame.Jigsaw;
using SharpDX.Direct2D1;

namespace Cyjb.Projects.JigsawGame.Renderer
{
	/// <summary>
	/// 拼图的渲染器。
	/// </summary>
	public abstract class JigsawRenderer : IDisposable
	{
		/// <summary>
		/// 创建指定类型的拼图渲染器。
		/// </summary>
		/// <param name="rendererType">拼图渲染器的类型。</param>
		/// <param name="deviceManager">设备管理器。</param>
		public static JigsawRenderer CreateRenderer(JigsawRendererType rendererType, DeviceManager deviceManager)
		{
			switch (rendererType)
			{
				case JigsawRendererType.Effect:
					return new JigsawEffectRenderer(deviceManager);
				default:
					return new JigsawSimpleRenderer(deviceManager);
			}
		}
		/// <summary>
		/// 设备管理器。
		/// </summary>
		private DeviceManager deviceManager;
		/// <summary>
		/// 需要渲染的拼图碎片列表。
		/// </summary>
		private List<JigsawPiece> currentPieces = new List<JigsawPiece>();
		/// <summary>
		/// 使用指定的设备管理器初始化 <see cref="JigsawRenderer"/> 类的新实例。
		/// </summary>
		/// <param name="deviceManager">设备管理器。</param>
		protected JigsawRenderer(DeviceManager deviceManager)
		{
			this.deviceManager = deviceManager;
		}

		#region IDisposable 成员

		/// <summary>
		/// 释放对象占用的资源。
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// 释放对象占用的资源。
		/// </summary>
		/// <param name="disposing">是否释放托管资源。</param>
		protected virtual void Dispose(bool disposing)
		{
			ClearResources();
		}

		#endregion // IDisposable 成员

		/// <summary>
		/// 获取拼图渲染器的类型。
		/// </summary>
		public abstract JigsawRendererType RendererType { get; }
		/// <summary>
		/// 获取拼图图片数据。
		/// </summary>
		protected Bitmap Image { get; private set; }
		/// <summary>
		/// 获取 Direct2D 渲染目标。
		/// </summary>
		protected RenderTarget RenderTarget
		{
			get { return this.deviceManager.RenderTarget; }
		}
		/// <summary>
		/// 获取 Direct2D 设备上下文。
		/// </summary>
		protected DeviceContext DeviceContext
		{
			get { return this.deviceManager.D2DContext; }
		}
		/// <summary>
		/// 获取设备管理器。
		/// </summary>
		protected DeviceManager DeviceManager
		{
			get { return this.deviceManager; }
		}
		/// <summary>
		/// 获取需要渲染的拼图碎片列表，以拼图被传入的顺序排列。
		/// </summary>
		protected IList<JigsawPiece> CurrentPieces
		{
			get { return this.currentPieces; }
		}
		/// <summary>
		/// 准备渲染拼图碎片。
		/// </summary>
		/// <param name="imageData">拼图使用的图片数据。</param>
		/// <param name="pieces">所有拼图碎片的集合。</param>
		/// <param name="rotatable">拼图碎片是否可以旋转。</param>
		/// <param name="ct">取消任务的通知。</param>
		public virtual void PrepareRender(byte[] imageData, JigsawPieceCollection pieces, bool rotatable,
			CancellationToken ct)
		{
			ExceptionHelper.CheckArgumentNull(imageData, "image");
			ClearResources();
			this.Image = this.deviceManager.LoadBitmapFromBytes(imageData);
		}
		/// <summary>
		/// 清除渲染使用的资源。
		/// </summary>
		public virtual void ClearResources()
		{
			if (this.Image != null)
			{
				this.Image.Dispose();
				this.Image = null;
			}
		}
		/// <summary>
		/// 渲染指定的拼图碎片。
		/// </summary>
		/// <param name="pieces">要绘制的拼图碎片集合。</param>
		public void Render(IEnumerable<JigsawPiece> pieces)
		{
			this.currentPieces.Clear();
			this.currentPieces.AddRange(pieces);
			Render();
		}
		/// <summary>
		/// 渲染拼图碎片。
		/// </summary>
		protected abstract void Render();
	}
}
