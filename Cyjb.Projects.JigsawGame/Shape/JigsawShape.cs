using System.Collections.Generic;
using SharpDX;

namespace Cyjb.Projects.JigsawGame.Shape
{
	/// <summary>
	/// 拼图形状的抽象基类。
	/// </summary>
	public class JigsawShape
	{
		/// <summary>
		/// 创建指定类型的拼图形状。
		/// </summary>
		/// <param name="type">拼图形状的类型。</param>
		/// <returns>指定类型的拼图形状。</returns>
		public static JigsawShape CreateShape(JigsawShapeType type)
		{
			switch (type)
			{
				case JigsawShapeType.Square:
					return new JigsawShape();
				case JigsawShapeType.StandardCirle:
					return new JigsawStandardCircleShape();
				case JigsawShapeType.Standard:
					return new JigsawStandardShape();
				case JigsawShapeType.StandardSmooth:
					return new JigsawStandardSmoothShape();
			}
			return null;
		}
		/// <summary>
		/// 拼图分割时的随机化程度(0.0 - 1.0)。
		/// </summary>
		private float randomization = 0.3f;
		/// <summary>
		/// 拼图分割的水平片数。
		/// </summary>
		private int horizontalDimension = 10;
		/// <summary>
		/// 拼图分割的垂直片数。
		/// </summary>
		private int verticalDimension = 10;
		/// <summary>
		/// 拼图的尺寸。
		/// </summary>
		private Size2F size = new Size2F(100, 100);
		/// <summary>
		/// 拼图形状集合。
		/// </summary>
		private List<Path> paths = new List<Path>();
		/// <summary>
		/// 生成边时需要的随机数个数。
		/// </summary>
		private int randomCount;
		/// <summary>
		/// 创建一个新的拼图划分器。
		/// </summary>
		private JigsawShape()
			: this(0)
		{ }
		/// <summary>
		/// 创建一个新的拼图划分器。
		/// </summary>
		/// <param name="randomCnt">生成边时需要的随机数个数。</param>
		protected JigsawShape(int randomCnt)
		{
			this.randomCount = randomCnt;
		}
		/// <summary>
		/// 获取或设置拼图分割时的随机化程度(0.0 - 1.0)。
		/// </summary>
		public float Randomization
		{
			get { return this.randomization; }
			set
			{
				if (value < 0f || value > 1f)
				{
					throw CommonExceptions.ArgumentOutOfRange("value", value, 0, 1);
				}
				else
				{
					this.randomization = value;
				}
			}
		}
		/// <summary>
		/// 获取或设置拼图分割的水平片数。
		/// </summary>
		public int HorizontalDimension
		{
			get { return this.horizontalDimension; }
			set
			{
				if (value <= 0)
				{
					throw CommonExceptions.ArgumentOutOfRange("value", value);
				}
				this.horizontalDimension = value;
			}
		}
		/// <summary>
		/// 获取或设置拼图分割的垂直片数。
		/// </summary>
		public int VerticalDimension
		{
			get { return this.verticalDimension; }
			set
			{
				if (value <= 0)
				{
					throw CommonExceptions.ArgumentOutOfRange("value", value);
				}
				this.verticalDimension = value;
			}
		}
		/// <summary>
		/// 获取或设置拼图的尺寸。
		/// </summary>
		public Size2F Size
		{
			get { return this.size; }
			set
			{
				if (value.Height <= 0f || value.Width <= 0f)
				{
					throw CommonExceptions.ArgumentOutOfRange("value", value);
				}
				this.size = value;
			}
		}
		/// <summary>
		/// 获取拼图形状集合。
		/// </summary>
		public IList<Path> Paths
		{
			get { return this.paths; }
		}
		/// <summary>
		/// 生成拼图形状。
		/// </summary>
		public void GererateJigsawShape()
		{
			// 拼图尺寸。
			float width = Size.Width;
			float height = Size.Height;
			// 拼图碎片的尺寸。
			float pWidth = width / horizontalDimension;
			float pHeight = height / verticalDimension;
			// 拼图碎片的随机化尺寸。
			float rWidth = pWidth * randomization / 3;
			float rHeight = pHeight * randomization / 3;
			float x, y;
			// 最后一行节点。
			Vector2[] corners = new Vector2[horizontalDimension + 1];
			Vector2 lastPoint = new Vector2();
			// 最后一行的边凹凸性。
			bool[] borders = new bool[horizontalDimension + 1];
			bool lastBorder = false;
			// 最后一行的随机数。
			float[][] values = new float[horizontalDimension + 1][];
			float[] lastValue = null;
			for (int i = 0; i <= this.verticalDimension; i++)
			{
				for (int j = 0; j <= this.horizontalDimension; j++)
				{
					y = pHeight * i;
					if (i > 0 && i < this.verticalDimension && rHeight != 0f)
					{
						y += (float)((RandomExt.NextDouble() * 2 - 1) * rHeight);
					}
					x = pWidth * j;
					if (j > 0 && j < horizontalDimension && rWidth != 0f)
					{
						x += (float)((RandomExt.NextDouble() * 2 - 1) * rWidth);
					}
					Vector2 currentPoint = new Vector2(x, y);
					if (i == 0)
					{
						corners[j] = currentPoint;
					}
					else if (j == 0)
					{
						lastPoint = currentPoint;
					}
					else
					{
						// 将拼图碎片放置在国际象棋盘上，每片会分别对应黑色和白色。
						bool isBlack = (i + j) % 2 == 0;
						Path path = new Path(corners[j], isBlack);
						// 逆时针添加边。
						// 顶边。
						if (i == 1)
						{
							path.AddLine(corners[j - 1]);
						}
						else
						{
							AddBorder(path, corners[j], corners[j - 1], !borders[j], values[j]);
						}
						// 左边。
						if (j == 1)
						{
							path.AddLine(lastPoint);
						}
						else
						{
							AddBorder(path, corners[j - 1], lastPoint, !lastBorder, lastValue);
						}
						// 底边。
						if (i == verticalDimension)
						{
							path.AddLine(currentPoint);
						}
						else
						{
							borders[j] = RandomExt.NextBoolean();
							values[j] = GenerateRandom();
							AddBorder(path, lastPoint, currentPoint, borders[j], values[j]);
						}
						// 右边。
						if (j == horizontalDimension)
						{
							path.AddLine(corners[j]);
						}
						else
						{
							lastBorder = RandomExt.NextBoolean();
							lastValue = GenerateRandom();
							AddBorder(path, currentPoint, corners[j], lastBorder, lastValue);
						}
						this.paths.Add(path);
						// 计算形状的重心。
						Vector2 c1 = SharpDXUtility.GetCenter(corners[j - 1], corners[j], lastPoint);
						Vector2 c2 = SharpDXUtility.GetCenter(corners[j], lastPoint, currentPoint);
						float w1 = SharpDXUtility.Area(corners[j - 1], corners[j], lastPoint);
						float w2 = SharpDXUtility.Area(corners[j], lastPoint, currentPoint);
						path.Weight = w1 + w2;
						path.Center = new Vector2((c1.X * w1 + c2.X * w2) / path.Weight, (c1.Y * w1 + c2.Y * w2) / path.Weight);
						corners[j - 1] = lastPoint;
						lastPoint = currentPoint;
						if (j == this.horizontalDimension)
						{
							corners[j] = currentPoint;
						}
					}
				}
			}
		}
		/// <summary>
		/// 生成指定个数的随机数。
		/// </summary>
		/// <returns>生成的随机数数组。</returns>
		private float[] GenerateRandom()
		{
			if (randomCount == 0)
			{
				return null;
			}
			float[] randoms = new float[randomCount];
			for (int i = 0; i < randomCount; i++)
			{
				randoms[i] = RandomExt.NextSingle();
			}
			return randoms;
		}
		/// <summary>
		/// 向拼图碎片的路径中添加一条边，路径的当前节点总是在起始点。
		/// </summary>
		/// <param name="path">路径。</param>
		/// <param name="startPoint">边的起始点。</param>
		/// <param name="endPoint">边的结束点。</param>
		/// <param name="randoms">该边的凹凸性。</param>
		/// <param name="border">与该条边相关的一组随机数，范围都是 [0, 1)。</param>
		protected virtual void AddBorder(Path path, Vector2 startPoint, Vector2 endPoint,
			bool border, float[] randoms)
		{
			path.AddLine(endPoint);
		}
	}
}
