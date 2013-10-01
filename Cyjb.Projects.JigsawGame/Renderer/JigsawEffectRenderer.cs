using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cyjb.Projects.JigsawGame.Jigsaw;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;

namespace Cyjb.Projects.JigsawGame.Renderer
{
	/// <summary>
	/// 具有特效的拼图渲染器。
	/// </summary>
	public sealed class JigsawEffectRenderer : JigsawRenderer
	{
		/// <summary>
		/// 阴影周围的边距。
		/// </summary>
		private const int ShadowPadding = 20;
		/// <summary>
		/// 阴影特效。
		/// </summary>
		private Shadow shadowEffect;
		/// <summary>
		/// 斜角特效的基础特效。
		/// </summary>
		private Shadow bevelBaseEffect;
		/// <summary>
		/// 点光源光照特效。
		/// </summary>
		private PointSpecular pointSpecularEffect;
		/// <summary>
		/// 组合得到斜角光照特效。
		/// </summary>
		private Composite bevelLightEffect;
		/// <summary>
		/// 最终的斜角特效。
		/// </summary>
		private ArithmeticComposite bevelEffect;
		/// <summary>
		/// 绘制拼图碎片的笔刷。
		/// </summary>
		private BitmapBrush brush;
		/// <summary>
		/// 绘制拼图碎片的阴影的笔刷。
		/// </summary>
		private BitmapBrush shadowBrush;
		/// <summary>
		/// 被选择的拼图碎片的笔刷颜色，是 20% 透明度的粉色。
		/// </summary>
		private Color selectedColor = new Color(0x307F7FFF);
		/// <summary>
		/// 绘制被选择的拼图碎片的笔刷。
		/// </summary>
		private SolidColorBrush selectedBrush;
		/// <summary>
		/// 拼图的图片，分别对应四种不同的旋转角度的图片。
		/// </summary>
		private Bitmap[] images = new Bitmap[4];
		/// <summary>
		/// 黑色的笔刷。
		/// </summary>
		private SolidColorBrush blackBrush;
		/// <summary>
		/// 拼图图片的大小。
		/// </summary>
		private Size2 imageSize;
		/// <summary>
		/// 斜角的高度。
		/// </summary>
		private float bevelHeight = 2f;
		/// <summary>
		/// 斜角的宽度。
		/// </summary>
		private float bevelWidth = 2f;
		/// <summary>
		/// 阴影的尺寸。
		/// </summary>
		private float shadowSize = 2f;
		/// <summary>
		/// 阴影的偏移量。
		/// </summary>
		private Vector2 shadowOffset = new Vector2(5, 5);
		/// <summary>
		/// 拼图碎片的阴影缓存。
		/// </summary>
		private Dictionary<Geometry, Tuple<Bitmap, Vector2>> shadowCache =
			new Dictionary<Geometry, Tuple<Bitmap, Vector2>>();
		/// <summary>
		/// 使用指定的设备管理器初始化 <see cref="JigsawEffectRenderer"/> 类的新实例。
		/// </summary>
		/// <param name="deviceManager">设备管理器。</param>
		public JigsawEffectRenderer(DeviceManager deviceManager)
			: base(deviceManager)
		{
			InitEffects();
			InitBrushes();
		}

		#region 初始化

		/// <summary>
		/// 初始化笔刷。
		/// </summary>
		private void InitBrushes()
		{
			this.brush = new BitmapBrush(this.RenderTarget, null);
			this.shadowBrush = new BitmapBrush(this.RenderTarget, null);
			this.selectedBrush = new SolidColorBrush(this.RenderTarget, selectedColor);
			this.blackBrush = new SolidColorBrush(this.DeviceContext, Color.Black);
		}
		/// <summary>
		/// 初始化 Direct2D 特效。
		/// </summary>
		private void InitEffects()
		{
			// 创建 Direct2D 特效。
			// 阴影特效。
			shadowEffect = new Shadow(this.DeviceContext);
			shadowEffect.BlurStandardDeviation = shadowSize;
			shadowEffect.Color = Color.Black;
			// 斜角特效的基础特效。
			bevelBaseEffect = new Shadow(this.DeviceContext);
			bevelBaseEffect.BlurStandardDeviation = bevelWidth;
			bevelBaseEffect.Color = Color.Black;
			// 点光源光照特效。
			pointSpecularEffect = new PointSpecular(this.DeviceContext);
			pointSpecularEffect.SetInputEffect(0, bevelBaseEffect);
			pointSpecularEffect.SurfaceScale = bevelHeight;
			// 组合得到斜角光照特效。
			bevelLightEffect = new Composite(this.DeviceContext);
			bevelLightEffect.SetInputEffect(0, bevelBaseEffect);
			bevelLightEffect.SetInputEffect(1, pointSpecularEffect);
			// 最终的斜角特效。
			bevelEffect = new ArithmeticComposite(this.DeviceContext);
			bevelEffect.SetInputEffect(1, bevelLightEffect);
			bevelEffect.Coefficients = new Vector4(1.0f, 0.5f, 0.0f, 0.0f);
		}

		#endregion // 初始化

		#region IDisposable 成员

		/// <summary>
		/// 释放对象占用的资源。
		/// </summary>
		/// <param name="disposing">是否释放托管资源。</param>
		protected override void Dispose(bool disposing)
		{
			this.brush.Dispose();
			this.shadowBrush.Dispose();
			this.selectedBrush.Dispose();
			this.blackBrush.Dispose();
			this.shadowEffect.Dispose();
			this.bevelBaseEffect.Dispose();
			this.pointSpecularEffect.Dispose();
			this.bevelLightEffect.Dispose();
			this.bevelEffect.Dispose();
			base.Dispose(true);
		}

		#endregion // IDisposable 成员

		#region 渲染器属性

		/// <summary>
		/// 获取拼图渲染器的类型。
		/// </summary>
		public override JigsawRendererType RendererType { get { return JigsawRendererType.Effect; } }
		/// <summary>
		/// 获取或设置斜角的高度。
		/// </summary>
		public float BevelHeight
		{
			get { return this.bevelHeight; }
			set
			{
				this.bevelHeight = value;
				this.pointSpecularEffect.SurfaceScale = value;
			}
		}
		/// <summary>
		/// 获取或设置斜角的宽度。
		/// </summary>
		public float BevelWidth
		{
			get { return this.bevelWidth; }
			set
			{
				this.bevelWidth = value;
				this.bevelBaseEffect.BlurStandardDeviation = value;
			}
		}
		/// <summary>
		/// 获取或设置阴影的尺寸。
		/// </summary>
		public float ShadowSize
		{
			get { return this.shadowSize; }
			set
			{
				this.shadowSize = value;
				this.shadowEffect.BlurStandardDeviation = value;
			}
		}
		/// <summary>
		/// 获取或设置阴影的偏移量。
		/// </summary>
		public Vector2 ShadowOffset
		{
			get { return this.shadowOffset; }
			set { this.shadowOffset = value; }
		}
		/// <summary>
		/// 获取或设置被选择的拼图碎片的笔刷。
		/// </summary>
		public Color SelectedColor
		{
			get { return this.selectedColor; }
			set
			{
				this.selectedColor = value;
				selectedBrush.Color = value;
			}
		}

		#endregion // 渲染器属性

		/// <summary>
		/// 准备渲染拼图碎片。
		/// </summary>
		/// <param name="imageData">拼图使用的图片数据。</param>
		/// <param name="pieces">所有拼图碎片的集合。</param>
		/// <param name="rotatable">拼图碎片是否可以旋转。</param>
		/// <param name="ct">取消任务的通知。</param>
		public override void PrepareRender(byte[] imageData, JigsawPieceCollection pieces, bool rotatable,
			CancellationToken ct)
		{
			base.PrepareRender(imageData, pieces, rotatable, ct);
			ct.ThrowIfCancellationRequested();
			imageSize = new Size2((int)this.Image.Size.Width, (int)this.Image.Size.Height);
			using (Brush imageBrush = new BitmapBrush(this.DeviceContext, this.Image))
			{
				using (Bitmap1 source = DeviceManager.CreateBitmap(imageSize))
				{
					ct.ThrowIfCancellationRequested();
					// 设置特效的输入。
					bevelBaseEffect.SetInput(0, source, true);
					bevelEffect.SetInput(0, source, true);
					// 分别按黑色与白色创建拼图碎片特效。
					List<Geometry> blacks = new List<Geometry>();
					List<Geometry> whites = new List<Geometry>();
					foreach (JigsawPiece piece in pieces)
					{
						Geometry[] geoms = piece.OriginalPath.GetSourceGeometry();
						// 划分黑色与白色，他们分别是不相邻的。
						bool[] colors = piece.GetColors();
						for (int i = 0; i < colors.Length; i++)
						{
							if (colors[i])
							{
								blacks.Add(geoms[i]);
							}
							else
							{
								whites.Add(geoms[i]);
							}
						}
					}
					ct.ThrowIfCancellationRequested();
					pointSpecularEffect.LightPosition = new Vector3(0, 0, 0);
					this.images[0] = this.CreateImage(imageBrush, source, blacks, whites, ct);
					if (rotatable)
					{
						// 生成不同旋转角度的图像，使用不同的光照情况。
						pointSpecularEffect.LightPosition = new Vector3(0, imageSize.Height, 0);
						this.images[1] = this.CreateImage(imageBrush, source, blacks, whites, ct);
						pointSpecularEffect.LightPosition = new Vector3(imageSize.Width, imageSize.Height, 0);
						this.images[2] = this.CreateImage(imageBrush, source, blacks, whites, ct);
						pointSpecularEffect.LightPosition = new Vector3(imageSize.Width, 0, 0);
						this.images[3] = this.CreateImage(imageBrush, source, blacks, whites, ct);
					}
				}
			}
		}
		/// <summary>
		/// 清除渲染使用的资源。
		/// </summary>
		public override void ClearResources()
		{
			base.ClearResources();
			foreach (Tuple<Bitmap, Vector2> pair in shadowCache.Values)
			{
				pair.Item1.Dispose();
			}
			shadowCache.Clear();
			for (int i = 0; i < images.Length; i++)
			{
				if (images[i] != null)
				{
					images[i].Dispose();
				}
			}
		}

		#region 生成 Direct2D 特效

		/// <summary>
		/// 创建具有指定角度凸起效果的图像。
		/// </summary>
		/// <param name="imageBrush">原始的拼图图像笔刷。</param>
		/// <param name="source">用于生成图像的源位图。</param>
		/// <param name="blacks">黑色图形集合。</param>
		/// <param name="whites">白色图形集合。</param>
		/// <param name="ct">取消任务的通知。</param>
		/// <returns>具有指定角度凸起效果的图像。</returns>
		private Bitmap CreateImage(Brush imageBrush, Bitmap1 source,
			IList<Geometry> blacks, IList<Geometry> whites, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();
			Bitmap1 target = null;
			try
			{
				target = DeviceManager.CreateBitmap(imageSize);
				// 一次性绘制所有不相邻的拼图碎片。
				ct.ThrowIfCancellationRequested();
				this.CreateImage(imageBrush, source, target, blacks, ct);
				ct.ThrowIfCancellationRequested();
				this.CreateImage(imageBrush, source, target, whites, ct);
				return target;
			}
			catch
			{
				if (target != null)
				{
					target.Dispose();
				}
				throw;
			}
		}
		/// <summary>
		/// 从指定的形状创建具有凸起效果的图像。
		/// </summary>
		/// <param name="imageBrush">原始的拼图图像笔刷。</param>
		/// <param name="source">源位图。</param>
		/// <param name="target">目标位图。</param>
		/// <param name="geoms">形状列表。</param>
		/// <param name="ct">取消任务的通知。</param>
		private void CreateImage(Brush imageBrush, Bitmap1 source, Bitmap1 target,
			IList<Geometry> geoms, CancellationToken ct)
		{
			this.DeviceContext.Target = source;
			this.DeviceContext.BeginDraw();
			this.DeviceContext.Clear(Color.Transparent);
			int cnt = geoms.Count;
			for (int i = 0; i < cnt; i++)
			{
				if (ct.IsCancellationRequested)
				{
					break;
				}
				this.DeviceContext.FillGeometry(geoms[i], imageBrush);
			}
			this.DeviceContext.EndDraw();
			// 将添加特效后的拼图碎片绘制到位图上。
			ct.ThrowIfCancellationRequested();
			this.DeviceContext.Target = target;
			this.DeviceContext.BeginDraw();
			this.DeviceContext.DrawImage(bevelEffect);
			this.DeviceContext.EndDraw();
		}
		/// <summary>
		/// 返回指定拼图碎片的阴影位图。
		/// </summary>
		/// <param name="piece">要获取阴影位图的拼图碎片。</param>
		/// <returns>指定拼图碎片的阴影位图。</returns>
		private Tuple<Bitmap, Vector2> GetShadow(JigsawPiece piece)
		{
			Tuple<Bitmap, Vector2> tuple;
			if (!shadowCache.TryGetValue(piece.OriginalPath, out tuple))
			{
				// 移除已被释放的拼图碎片。
				Geometry disposedGeom = null;
				if ((disposedGeom = shadowCache.Keys.FirstOrDefault(g => g.IsDisposed)) != null)
				{
					shadowCache.Remove(disposedGeom);
				}
				RectangleF bounds = piece.OriginalPath.GetBounds();
				Size2 size = new Size2((int)bounds.Width + ShadowPadding * 2, (int)bounds.Height + ShadowPadding * 2);
				using (Bitmap1 source = DeviceManager.CreateBitmap(size))
				{
					Bitmap1 target = DeviceManager.CreateBitmap(size);
					// 设置特效的输入。
					shadowEffect.SetInput(0, source, true);
					// 阴影的偏移。
					Vector2 offset = new Vector2(bounds.Left - ShadowPadding, bounds.Top - ShadowPadding);
					using (TransformedGeometry tGeom = new TransformedGeometry(this.DeviceContext.Factory,
						piece.OriginalPath, Matrix3x2.Translation(-offset.X, -offset.Y)))
					{
						// 将拼图碎片绘制到位图上。
						this.DeviceContext.Target = source;
						this.DeviceContext.BeginDraw();
						this.DeviceContext.Clear(Color.Transparent);
						this.DeviceContext.FillGeometry(tGeom, blackBrush);
						this.DeviceContext.EndDraw();

						// 将添加特效后的拼图碎片绘制到位图上。
						this.DeviceContext.Target = target;
						this.DeviceContext.BeginDraw();
						this.DeviceContext.DrawImage(shadowEffect);
						this.DeviceContext.EndDraw();

						tuple = new Tuple<Bitmap, Vector2>(target, offset);
						shadowCache.Add(piece.OriginalPath, tuple);
					}
				}
			}
			return tuple;
		}

		#endregion // 生成 Direct2D 特效

		/// <summary>
		/// 渲染拼图碎片。
		/// </summary>
		protected override void Render()
		{
			int idx = CurrentPieces.Count - 1;
			bool drawShadow = true;
			for (; idx >= 0; idx--)
			{
				JigsawPiece piece = CurrentPieces[idx];
				if (!piece.Visible)
				{
					continue;
				}
				if ((piece.State & JigsawPieceState.Draging) == JigsawPieceState.Draging && drawShadow)
				{
					// 之后的拼图碎片都是拖动状态，绘制阴影。
					drawShadow = false;
					for (int i = idx; i >= 0; i--)
					{
						Tuple<Bitmap, Vector2> shadow = GetShadow(CurrentPieces[i]);
						brush.Bitmap = shadow.Item1;
						brush.Transform = Matrix3x2.Translation(shadow.Item2) *
							CurrentPieces[i].TransformMatrix * Matrix3x2.Translation(shadowOffset);
						RectangleF bounds = CurrentPieces[i].Bounds;
						this.RenderTarget.FillRectangle(new RectangleF(bounds.X - ShadowPadding, bounds.Y - ShadowPadding,
							bounds.Width + ShadowPadding * 2, bounds.Height + ShadowPadding * 2), brush);
					}
				}
				this.brush.Bitmap = images[piece.Rotate / 90];
				brush.Transform = piece.TransformMatrix;
				this.RenderTarget.FillGeometry(piece.Path, brush);
				// 叠加被选择的颜色，仅当没有被拖动时才叠加。
				if ((piece.State & JigsawPieceState.Selected) == JigsawPieceState.Selected &&
					(piece.State & JigsawPieceState.Draging) == JigsawPieceState.None)
				{
					this.RenderTarget.FillGeometry(piece.Path, selectedBrush);
				}
			}
		}
	}
}
