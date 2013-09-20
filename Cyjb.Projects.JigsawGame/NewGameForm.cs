using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Cyjb.Projects.JigsawGame.Shape;

namespace Cyjb.Projects.JigsawGame
{
	/// <summary>
	/// 新建游戏的窗体。
	/// </summary>
	public partial class NewGameForm : Form
	{
		/// <summary>
		/// 游戏图像。
		/// </summary>
		private Image image;
		/// <summary>
		/// 可分割的最小碎片数。
		/// </summary>
		private int minPieceCount;
		/// <summary>
		/// 可分割的最大碎片数。
		/// </summary>
		private int maxPieceCount;
		/// <summary>
		/// 拼图横向分割的块数。
		/// </summary>
		private int horizontalDimension;
		/// <summary>
		/// 拼图纵向分割的块数。
		/// </summary>
		private int verticalDimension;
		/// <summary>
		/// 图片网格的起始 X 位置。
		/// </summary>
		private float netX;
		/// <summary>
		/// 图片网格的起始 Y 位置。
		/// </summary>
		private float netY;
		/// <summary>
		/// 图片网格的宽度。
		/// </summary>
		private float netW;
		/// <summary>
		/// 图片网格的高度。
		/// </summary>
		private float netH;
		/// <summary>
		/// 绘制网格的画笔。
		/// </summary>
		private Pen pen = new Pen(Color.Red);
		/// <summary>
		/// 拼图的所有合理划分。
		/// </summary>
		private IList<Partition> partitions = new List<Partition>();
		/// <summary>
		/// 拼图信息。
		/// </summary>
		private JigsawInfo info = new JigsawInfo();
		/// <summary>
		/// 拼图形状。
		/// </summary>
		private JigsawShape shape;
		/// <summary>
		/// 构造函数。
		/// </summary>
		public NewGameForm()
		{
			InitializeComponent();
			picture.BackColor = JigsawSetting.Default.BackgroundColor;
			combShape.DisplayMember = "Text";
			combShape.ValueMember = "Value";
			combShape.DataSource = EnumExt.GetTextValues<JigsawShapeType>();
			combDifficulty.SelectedIndex = JigsawSetting.Default.Difficulty;
			combShape.SelectedIndex = JigsawSetting.Default.Shape;
			cbxRotate.Checked = JigsawSetting.Default.Rotatable;
			cbxAnchor.Checked = JigsawSetting.Default.Anchor;
			tbarRand.Value = (int)(JigsawSetting.Default.Randomization * 100);
			tbarRand_Scroll(tbarRand, null);
		}
		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 <c>true</c>；否则为 <c>false</c>。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.components != null)
				{
					this.components.Dispose();
				}
				this.pen.Dispose();
				if (this.image != null)
				{
					this.image.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		/// <summary>
		/// 获取拼图信息。
		/// </summary>
		public JigsawInfo JigsawInfo { get { return this.info; } }
		/// <summary>
		/// 获取拼图形状。
		/// </summary>
		public JigsawShape JigsawShape { get { return this.shape; } }
		/// <summary>
		/// 浏览图片。
		/// </summary>
		private void btnPic_Click(object sender, EventArgs e)
		{
			if (imageOpenDialog.ShowDialog() == DialogResult.OK)
			{
				image = Image.FromFile(imageOpenDialog.FileName);
				lblPic.Text = string.Concat("选择图片：", imageOpenDialog.FileName,
					MultiplyFormat(image.Width, image.Height));
				// 图片缩放后的大小
				if (image.Width * picture.Height >= image.Height * picture.Width)
				{
					netW = picture.Width - 1;
					netH = (float)picture.Width * image.Height / image.Width - 1;
					netX = 0;
					netY = ((float)picture.Height - netH) / 2;
				}
				else
				{
					netW = (float)picture.Height * image.Width / image.Height - 1;
					netH = picture.Height - 1;
					netX = ((float)picture.Width - netW) / 2;
					netY = 0;
				}
				PartitionImage();
				tbarDifficulty.Enabled = tbarRand.Enabled = true;
				cbxRotate.Enabled = cbxAnchor.Enabled = true;
				combDifficulty.Enabled = combShape.Enabled = true;
				btnOK.Enabled = true;
			}
		}
		/// <summary>
		/// 划分游戏图片。
		/// </summary>
		private void PartitionImage()
		{
			// 图片的合理划分列表。
			int[] tmpPartition = new int[maxPieceCount - minPieceCount + 1];
			// 合理的划分，纵横比例在图片纵横比例上下 1/3 之间，即拼图碎片的比例不会超过 4:3。
			float minRatio = (float)image.Height * 3 / (image.Width * 4);
			float maxRatio = (float)image.Height * 4 / (image.Width * 3);
			for (int hDim = 2; ; hDim++)
			{
				// 寻找相应的纵向分割块数。
				int minVDim = (int)(hDim * minRatio + 1);
				int maxVDim = (int)(hDim * maxRatio);
				int startVDim = minPieceCount / hDim;
				int endVDim = maxPieceCount / hDim;
				if (minVDim > endVDim)
				{
					break;
				}
				if (endVDim < maxVDim)
				{
					maxVDim = endVDim;
				}
				if (startVDim > minVDim)
				{
					minVDim = startVDim;
				}
				for (int vDim = minVDim; vDim <= maxVDim; vDim++)
				{
					int pieceCount = hDim * vDim;
					if (pieceCount < minPieceCount)
					{
						continue;
					}
					if (pieceCount > maxPieceCount)
					{
						break;
					}
					int oldHDim = tmpPartition[pieceCount - minPieceCount];
					if (oldHDim > 0)
					{
						// 对于相同总块数的不同划分，总是选择最终划分最接近正方形的。
						float ratio = ((float)image.Width * pieceCount) / image.Height;
						float oldRatio = oldHDim * oldHDim - ratio;
						if (oldRatio < 0)
						{
							oldRatio = -oldRatio;
						}
						ratio = hDim * hDim - ratio;
						if (ratio < 0)
						{
							ratio = -ratio;
						}
						if (ratio < oldRatio)
						{
							tmpPartition[pieceCount - minPieceCount] = hDim;
						}
					}
					else
					{
						tmpPartition[pieceCount - minPieceCount] = hDim;
					}
				}
			}
			partitions.Clear();
			for (int i = minPieceCount; i <= maxPieceCount; i++)
			{
				if (tmpPartition[i - minPieceCount] > 0)
				{
					partitions.Add(new Partition(i, tmpPartition[i - minPieceCount]));
				}
			}
			tbarDifficulty.Maximum = partitions.Count - 1;
			tbarDifficulty_Scroll(this, null);
		}
		/// <summary>
		/// 更新游戏难度信息。
		/// </summary>
		private void tbarDifficulty_Scroll(object sender, EventArgs e)
		{
			Partition partition = partitions[tbarDifficulty.Value];
			this.horizontalDimension = partition.HorizontalDimension;
			this.verticalDimension = partition.VerticalDimension;
			lblPieces.Text = string.Concat("碎片数目：",
				partition.PieceCount.ToString(CultureInfo.CurrentCulture),
				MultiplyFormat(horizontalDimension, verticalDimension));
			picture.Invalidate();
		}
		/// <summary>
		/// 图片框大小被改变的事件。
		/// </summary>
		private void picture_Resize(object sender, EventArgs e)
		{
			if (image == null)
			{
				return;
			}
			// 图片缩放后的大小
			if (image.Width * picture.Height >= image.Height * picture.Width)
			{
				netW = picture.Width - 1;
				netH = (float)picture.Width * image.Height / image.Width - 1;
				netX = 0;
				netY = ((float)picture.Height - netH) / 2;
			}
			else
			{
				netW = (float)picture.Height * image.Width / image.Height - 1;
				netH = picture.Height - 1;
				netX = ((float)picture.Width - netW) / 2;
				netY = 0;
			}
			picture.Invalidate();
		}
		/// <summary>
		/// 在图像上绘制网格。
		/// </summary>
		private void picture_Paint(object sender, PaintEventArgs e)
		{
			if (image == null)
			{
				return;
			}
			e.Graphics.DrawImage(image, netX, netY, netW, netH);
			float start = netY, end = netY + netH;
			float pieceW = netW / horizontalDimension;
			for (int i = 0; i <= horizontalDimension; i++)
			{
				e.Graphics.DrawLine(pen, netX + i * pieceW, start, netX + i * pieceW, end);
			}
			start = netX;
			end = netX + netW;
			float pieceH = netH / verticalDimension;
			for (int i = 0; i <= verticalDimension; i++)
			{
				e.Graphics.DrawLine(pen, start, netY + i * pieceH, end, netY + i * pieceH);
			}
		}
		/// <summary>
		/// 难度被改变的事件。
		/// </summary>
		private void combDifficulty_SelectedIndexChanged(object sender, EventArgs e)
		{
			JigsawSetting.Default.Difficulty = combDifficulty.SelectedIndex;
			switch (combDifficulty.SelectedIndex)
			{
				case 0:
					minPieceCount = 2;
					maxPieceCount = 200;
					break;
				case 1:
					minPieceCount = 201;
					maxPieceCount = 400;
					break;
				case 2:
					minPieceCount = 401;
					maxPieceCount = 800;
					break;
				case 3:
					minPieceCount = 801;
					maxPieceCount = 1200;
					break;
			}
			tbarDifficulty.Value = 0;
			if (image != null)
			{
				PartitionImage();
			}
		}
		/// <summary>
		/// 随机程度被改变的事件。
		/// </summary>
		private void tbarRand_Scroll(object sender, EventArgs e)
		{
			lblRandInfo.Text = tbarRand.Value.ToString(CultureInfo.CurrentCulture) + "%";
		}
		/// <summary>
		/// 确定按钮按下的事件。
		/// </summary>
		private void btnOK_Click(object sender, EventArgs e)
		{
			JigsawSetting.Default.Shape = combShape.SelectedIndex;
			JigsawSetting.Default.Anchor = info.AnchorToBackground = cbxAnchor.Checked;
			info.AnchorRadius = 5f;
			using (MemoryStream stream = new MemoryStream())
			{
				image.Save(stream, ImageFormat.Png);
				stream.Seek(0, SeekOrigin.Begin);
				info.ImageData = stream.ToArray();
			}
			info.Scale = 1f;
			info.PieceSum = horizontalDimension * verticalDimension;
			JigsawSetting.Default.Rotatable = info.Rotatable = cbxRotate.Checked;
			shape = JigsawShape.CreateShape((JigsawShapeType)combShape.SelectedValue);
			shape.HorizontalDimension = horizontalDimension;
			shape.VerticalDimension = verticalDimension;
			JigsawSetting.Default.Randomization = shape.Randomization = (float)tbarRand.Value / 100;
			shape.Size = new SharpDX.Size2F(image.Width, image.Height);
			this.DialogResult = DialogResult.OK;
		}
		/// <summary>
		/// 将指定数字格式化为相乘的格式。
		/// </summary>
		/// <param name="left">左操作数。</param>
		/// <param name="right">右操作数。</param>
		/// <returns>格式化的结果。</returns>
		private static string MultiplyFormat(int left, int right)
		{
			return string.Concat(" (", left.ToString(CultureInfo.CurrentCulture), "×",
				right.ToString(CultureInfo.CurrentCulture), ")");
		}
		/// <summary>
		/// 表示拼图的一个合理划分。
		/// </summary>
		private struct Partition
		{
			/// <summary>
			/// 拼图被分割的总块数。
			/// </summary>
			public int PieceCount;
			/// <summary>
			/// 拼图横向分割的块数。
			/// </summary>
			public int HorizontalDimension;
			/// <summary>
			/// 拼图纵向分割的块数。
			/// </summary>
			public int VerticalDimension;
			public Partition(int count, int hDim)
			{
				this.PieceCount = count;
				this.HorizontalDimension = hDim;
				this.VerticalDimension = count / hDim;
			}
		}
	}
}
