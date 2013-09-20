using System;
using System.Collections.Generic;
using Cyjb.Projects.JigsawGame.Jigsaw;
using SharpDX;
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
		/// 原始的图片数据。
		/// </summary>
		private byte[] originalImageData;
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
		{ }

		#endregion // IDisposable 成员

		/// <summary>
		/// 获取拼图渲染器的类型。
		/// </summary>
		public abstract JigsawRendererType RendererType { get; }
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
		/// 将图像加载到指定的渲染目标上。
		/// </summary>
		/// <param name="target">要加载图像的渲染目标。</param>
		/// <returns>加载得到的图像。</returns>
		protected Bitmap LoadImage(RenderTarget target)
		{
			return this.deviceManager.WicFactory.LoadBitmapFromBytes(this.originalImageData, target);
		}
		/// <summary>
		/// 准备渲染拼图碎片。
		/// </summary>
		/// <param name="imageData">拼图使用的图片数据。</param>
		/// <param name="pieces">所有拼图碎片的集合。</param>
		/// <param name="rotatable">拼图碎片是否可以旋转。</param>
		public virtual void PrepareRender(byte[] imageData, IEnumerable<JigsawPiece> pieces, bool rotatable)
		{
			ExceptionHelper.CheckArgumentNull(imageData, "image");
			this.originalImageData = imageData;
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
		/// <summary>
		/// 获取拼图碎片渲染后的矩形范围。
		/// </summary>
		/// <param name="pieces">拼图碎片。</param>
		/// <returns>拼图碎片渲染后的矩形范围。</returns>
		public virtual RectangleF GetRenderRect(JigsawPiece piece)
		{
			return piece.Bounds;
		}
	}
}
