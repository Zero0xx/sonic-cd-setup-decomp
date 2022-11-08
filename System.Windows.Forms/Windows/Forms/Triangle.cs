using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x0200070B RID: 1803
	internal static class Triangle
	{
		// Token: 0x06006041 RID: 24641 RVA: 0x0015EF1C File Offset: 0x0015DF1C
		public static void Paint(Graphics g, Rectangle bounds, TriangleDirection dir, Brush backBr, Pen backPen1, Pen backPen2, Pen backPen3, bool opaque)
		{
			Point[] array = Triangle.BuildTrianglePoints(dir, bounds);
			if (opaque)
			{
				g.FillPolygon(backBr, array);
			}
			g.DrawLine(backPen1, array[0], array[1]);
			g.DrawLine(backPen2, array[1], array[2]);
			g.DrawLine(backPen3, array[2], array[0]);
		}

		// Token: 0x06006042 RID: 24642 RVA: 0x0015EFA0 File Offset: 0x0015DFA0
		private static Point[] BuildTrianglePoints(TriangleDirection dir, Rectangle bounds)
		{
			Point[] array = new Point[3];
			int num = (int)((double)bounds.Width * 0.8);
			if (num % 2 == 1)
			{
				num++;
			}
			int num2 = (int)Math.Ceiling((double)(num / 2) * 2.5);
			int num3 = (int)((double)bounds.Height * 0.8);
			if (num3 % 2 == 0)
			{
				num3++;
			}
			int num4 = (int)Math.Ceiling((double)(num3 / 2) * 2.5);
			switch (dir)
			{
			case TriangleDirection.Up:
				array[0] = new Point(0, num2);
				array[1] = new Point(num, num2);
				array[2] = new Point(num / 2, 0);
				break;
			case TriangleDirection.Down:
				array[0] = new Point(0, 0);
				array[1] = new Point(num, 0);
				array[2] = new Point(num / 2, num2);
				break;
			case TriangleDirection.Left:
				array[0] = new Point(num3, 0);
				array[1] = new Point(num3, num4);
				array[2] = new Point(0, num4 / 2);
				break;
			case TriangleDirection.Right:
				array[0] = new Point(0, 0);
				array[1] = new Point(0, num4);
				array[2] = new Point(num3, num4 / 2);
				break;
			}
			switch (dir)
			{
			case TriangleDirection.Up:
			case TriangleDirection.Down:
				Triangle.OffsetPoints(array, bounds.X + (bounds.Width - num2) / 2, bounds.Y + (bounds.Height - num) / 2);
				break;
			case TriangleDirection.Left:
			case TriangleDirection.Right:
				Triangle.OffsetPoints(array, bounds.X + (bounds.Width - num3) / 2, bounds.Y + (bounds.Height - num4) / 2);
				break;
			}
			return array;
		}

		// Token: 0x06006043 RID: 24643 RVA: 0x0015F1AC File Offset: 0x0015E1AC
		private static void OffsetPoints(Point[] points, int xOffset, int yOffset)
		{
			for (int i = 0; i < points.Length; i++)
			{
				int num = i;
				points[num].X = points[num].X + xOffset;
				int num2 = i;
				points[num2].Y = points[num2].Y + yOffset;
			}
		}

		// Token: 0x04003A30 RID: 14896
		private const double TRI_HEIGHT_RATIO = 2.5;

		// Token: 0x04003A31 RID: 14897
		private const double TRI_WIDTH_RATIO = 0.8;
	}
}
