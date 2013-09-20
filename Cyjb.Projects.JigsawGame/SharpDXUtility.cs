using System.Diagnostics.CodeAnalysis;
using System.IO;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.IO;
using SharpDX.Win32;
using DXGI = SharpDX.DXGI;
using WIC = SharpDX.WIC;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 包含 Sharp2D 的实用方法。
	/// </summary>
	public static class SharpDXUtility
	{

		#region 几何操作

		/// <summary>
		/// 获取给定点集合中心（坐标平均值）。
		/// </summary>
		/// <param name="args">要计算的点集合。</param>
		/// <returns>点集合的中心。</returns>
		public static Vector2 GetCenter(params Vector2[] args)
		{
			float x = 0, y = 0;
			for (int i = 0; i < args.Length; i++)
			{
				x += args[i].X;
				y += args[i].Y;
			}
			return new Vector2(x / args.Length, y / args.Length);
		}
		/// <summary>
		/// 计算三角形的面积（的二倍）。
		/// </summary>
		/// <param name="a">三角形的第一个顶点坐标。</param>
		/// <param name="b">三角形的第二个顶点坐标。</</param>
		/// <param name="c">三角形的第三个顶点坐标。</</param>
		/// <returns></returns>
		public static float Area(Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float area = (p1.X - p2.X) * (p1.Y - p3.Y) - (p1.X - p3.X) * (p1.Y - p2.Y);
			return area < 0 ? -area : area;
		}
		/// <summary>
		/// 将给定的几何图形组合并为一个组。
		/// </summary>
		/// <param name="geometies">要合并集合图形组。</param>
		/// <returns>合并得到的集合图形组。</returns>
		public static GeometryGroup Merge(params GeometryGroup[] geometies)
		{
			int[] idx = new int[geometies.Length];
			for (int i = 0; i < geometies.Length; i++)
			{
				if (i == 0)
				{
					idx[i] = geometies[i].SourceGeometryCount;
				}
				else
				{
					idx[i] = idx[i - 1] + geometies[i].SourceGeometryCount;
				}
			}
			Geometry[] geoms = new Geometry[idx[idx.Length - 1]];
			for (int i = 0; i < geometies.Length; i++)
			{
				if (i == 0)
				{
					geometies[i].GetSourceGeometry().CopyTo(geoms, 0);
				}
				else
				{
					geometies[i].GetSourceGeometry().CopyTo(geoms, idx[i - 1]);
				}
			}
			return new GeometryGroup(geometies[0].Factory, FillMode.Winding, geoms);
		}

		#endregion // 几何操作

		#region 位图操作

		/// <summary>
		/// Direct2D 的像素格式。
		/// </summary>
		public static readonly PixelFormat D2D1PixelFormat =
			new PixelFormat(DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);
		/// <summary>
		/// 位图的属性。
		/// </summary>
		public static readonly BitmapProperties BitmapProps = new BitmapProperties(D2D1PixelFormat, 96, 96);
		/// <summary>
		/// 位图的属性。
		/// </summary>
		public static readonly BitmapProperties1 BitmapProps1 = new BitmapProperties1(D2D1PixelFormat, 96, 96,
			BitmapOptions.Target);
		/// <summary>
		/// 从给定的文件中加载 Direct2D 位图。
		/// </summary>
		/// <param name="factory">Windows 图片组件工厂。</param>
		/// <param name="fileName">要加载位图的文件。</param>
		/// <param name="renderTarget">创建位图的渲染目标。</param>
		/// <returns>得到的 Direct2D 位图。</returns>
		public static Bitmap LoadBitmapFromFile(this WIC.ImagingFactory factory, string fileName, RenderTarget renderTarget)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open))
			{
				return LoadBitmapFromStream(factory, stream, renderTarget);
			}
		}
		/// <summary>
		/// 从给定的 Byte 数组中加载 Direct2D 位图。
		/// </summary>
		/// <param name="factory">Windows 图片组件工厂。</param>
		/// <param name="data">要加载位图的数据。</param>
		/// <param name="renderTarget">创建位图的渲染目标。</param>
		/// <returns>得到的 Direct2D 位图。</returns>
		public static Bitmap LoadBitmapFromBytes(this WIC.ImagingFactory factory, byte[] data, RenderTarget renderTarget)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				return LoadBitmapFromStream(factory, stream, renderTarget);
			}
		}
		/// <summary>
		/// 从给定的流中加载 Direct2D 位图。
		/// </summary>
		/// <param name="factory">Windows 图片组件工厂。</param>
		/// <param name="stream">要加载位图的流。</param>
		/// <param name="renderTarget">创建位图的渲染目标。</param>
		/// <returns>得到的 Direct2D 位图。</returns>
		public static Bitmap LoadBitmapFromStream(this WIC.ImagingFactory factory, Stream stream, RenderTarget renderTarget)
		{
			using (WIC.BitmapDecoder decoder = new WIC.BitmapDecoder(factory, stream, WIC.DecodeOptions.CacheOnLoad))
			{
				using (WIC.FormatConverter formatConverter = new WIC.FormatConverter(factory))
				{
					formatConverter.Initialize(decoder.GetFrame(0), WIC.PixelFormat.Format32bppPBGRA);
					return Bitmap.FromWicBitmap(renderTarget, formatConverter);
				}
			}
		}
		/// <summary>
		/// 将 Direct2D 位图保存到文件中。
		/// </summary>
		/// <param name="factory">Windows 图片组件工厂。</param>
		/// <param name="image">要保存的位图。</param>
		/// <param name="device">Direct2D 设备。</param>
		/// <param name="fileName">要保存的文件名。</param>
		public static void SaveBitmapToFile(this WIC.ImagingFactory2 factory, Bitmap image, Device device, string fileName)
		{
			using (WIC.WICStream stream = new WIC.WICStream(factory, fileName, NativeFileAccess.Write))
			{
				SaveBitmapToStream(factory, image, device, stream);
			}
		}
		/// <summary>
		/// 将 Direct2D 位图保存到指定的流中。
		/// </summary>
		/// <param name="factory">Windows 图片组件工厂。</param>
		/// <param name="image">要保存的位图。</param>
		/// <param name="device">Direct2D 设备。</param>
		/// <param name="fileName">要保存到的流。</param>
		public static void SaveBitmapToStream(this WIC.ImagingFactory2 factory, Bitmap image, Device device, IStream stream)
		{
			using (WIC.BitmapEncoder encoder = new WIC.PngBitmapEncoder(factory))
			{
				encoder.Initialize(stream);
				using (WIC.BitmapFrameEncode bitmapFrameEncode = new WIC.BitmapFrameEncode(encoder))
				{
					bitmapFrameEncode.Initialize();
					int width = image.PixelSize.Width;
					int height = image.PixelSize.Height;
					bitmapFrameEncode.SetSize(width, height);
					var wicPixelFormat = WIC.PixelFormat.Format32bppPRGBA;
					bitmapFrameEncode.SetPixelFormat(ref wicPixelFormat);
					using (WIC.ImageEncoder imageEncoder = new WIC.ImageEncoder(factory, device))
					{
						imageEncoder.WriteFrame(image, bitmapFrameEncode,
							new WIC.ImageParameters(D2D1PixelFormat, 96, 96, 0, 0, width, height));
						bitmapFrameEncode.Commit();
						encoder.Commit();
					}
				}
			}
		}
		/// <summary>
		/// 复制指定的位图。
		/// </summary>
		/// <param name="factory">Windows 图片组件工厂。</param>
		/// <param name="source">要赋值的位图。</param>
		/// <param name="device">Direct2D 设备。</param>
		/// <param name="renderTarget">要复制到的目标渲染目标。</param>
		/// <returns>复制得到的位图。</returns>
		[SuppressMessage("Microsoft.Usage", "CA2202:不要多次释放对象")]
		public static Bitmap CopyBitmap(this WIC.ImagingFactory2 factory, Bitmap source, Device device, RenderTarget renderTarget)
		{
			int width = source.PixelSize.Width;
			int height = source.PixelSize.Height;
			using (MemoryStream memStream = new MemoryStream())
			{
				using (WIC.WICStream stream = new WIC.WICStream(factory, memStream))
				{
					SaveBitmapToStream(factory, source, device, stream);
				}
				memStream.Seek(0, SeekOrigin.Begin);
				return LoadBitmapFromStream(factory, memStream, renderTarget);
			}
		}

		#endregion // 位图操作

		#region 颜色操作

		/// <summary>
		/// 将指定的 <see cref="System.Drawing.Color"/> 对象转换为等价的 
		/// <see cref="SharpDX.Color4"/> 对象。
		/// </summary>
		/// <param name="color">要转换的 <see cref="System.Drawing.Color"/> 对象。</param>
		/// <returns>转换得到的 <see cref="SharpDX.Color4"/> 对象。</returns>
		public static Color4 ToColor4(this System.Drawing.Color color)
		{
			uint argb = (uint)color.ToArgb();
			// Color4 的 R, G, B, A 是从小端到大端排列。
			uint rgba = (argb & 0xFF00FF00) | ((argb >> 16) & 0x000000FF) | ((argb << 16) & 0x00FF0000);
			return new Color4(rgba);
		}
		/// <summary>
		/// 将指定的 <see cref="SharpDX.Color4"/> 对象转换为等价的 <see cref="System.Drawing.Color"/> 对象。
		/// </summary>
		/// <param name="color">要转换的 <see cref="SharpDX.Color4"/> 对象。</param>
		/// <returns>转换得到的 <see cref="System.Drawing.Color"/> 对象。</returns>
		public static System.Drawing.Color ToColor(this Color4 color)
		{
			uint rgba = (uint)color.ToRgba();
			// Color4 的 R, G, B, A 是从小端到大端排列。
			uint argb = (rgba & 0xFF00FF00) | ((rgba >> 16) & 0x000000FF) | ((rgba << 16) & 0x00FF0000);
			return System.Drawing.Color.FromArgb((int)argb);
		}

		#endregion // 颜色操作

	}
}
