using System;
using System.Collections;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x0200078A RID: 1930
	internal class LayoutUtils
	{
		// Token: 0x0600655D RID: 25949 RVA: 0x00173388 File Offset: 0x00172388
		public static Size OldGetLargestStringSizeInCollection(Font font, ICollection objects)
		{
			Size empty = Size.Empty;
			if (objects != null)
			{
				foreach (object obj in objects)
				{
					Size size = TextRenderer.MeasureText(obj.ToString(), font, new Size(32767, 32767), TextFormatFlags.SingleLine);
					empty.Width = Math.Max(empty.Width, size.Width);
					empty.Height = Math.Max(empty.Height, size.Height);
				}
			}
			return empty;
		}

		// Token: 0x0600655E RID: 25950 RVA: 0x00173434 File Offset: 0x00172434
		public static int ContentAlignmentToIndex(ContentAlignment alignment)
		{
			int num = (int)LayoutUtils.xContentAlignmentToIndex((int)(alignment & (ContentAlignment)15));
			int num2 = (int)LayoutUtils.xContentAlignmentToIndex((int)(alignment >> 4 & (ContentAlignment)15));
			int num3 = (int)LayoutUtils.xContentAlignmentToIndex((int)(alignment >> 8 & (ContentAlignment)15));
			int num4 = ((num2 != 0) ? 4 : 0) | ((num3 != 0) ? 8 : 0) | num | num2 | num3;
			return num4 - 1;
		}

		// Token: 0x0600655F RID: 25951 RVA: 0x00173480 File Offset: 0x00172480
		private static byte xContentAlignmentToIndex(int threeBitFlag)
		{
			return (threeBitFlag == 4) ? 3 : ((byte)threeBitFlag);
		}

		// Token: 0x06006560 RID: 25952 RVA: 0x00173498 File Offset: 0x00172498
		public static Size ConvertZeroToUnbounded(Size size)
		{
			if (size.Width == 0)
			{
				size.Width = int.MaxValue;
			}
			if (size.Height == 0)
			{
				size.Height = int.MaxValue;
			}
			return size;
		}

		// Token: 0x06006561 RID: 25953 RVA: 0x001734C8 File Offset: 0x001724C8
		public static Padding ClampNegativePaddingToZero(Padding padding)
		{
			if (padding.All < 0)
			{
				padding.Left = Math.Max(0, padding.Left);
				padding.Top = Math.Max(0, padding.Top);
				padding.Right = Math.Max(0, padding.Right);
				padding.Bottom = Math.Max(0, padding.Bottom);
			}
			return padding;
		}

		// Token: 0x06006562 RID: 25954 RVA: 0x00173530 File Offset: 0x00172530
		private static AnchorStyles GetOppositeAnchor(AnchorStyles anchor)
		{
			AnchorStyles anchorStyles = AnchorStyles.None;
			if (anchor == AnchorStyles.None)
			{
				return anchorStyles;
			}
			for (int i = 1; i <= 8; i <<= 1)
			{
				switch (anchor & (AnchorStyles)i)
				{
				case AnchorStyles.Top:
					anchorStyles |= AnchorStyles.Bottom;
					break;
				case AnchorStyles.Bottom:
					anchorStyles |= AnchorStyles.Top;
					break;
				case AnchorStyles.Left:
					anchorStyles |= AnchorStyles.Right;
					break;
				case AnchorStyles.Right:
					anchorStyles |= AnchorStyles.Left;
					break;
				}
			}
			return anchorStyles;
		}

		// Token: 0x06006563 RID: 25955 RVA: 0x00173597 File Offset: 0x00172597
		public static TextImageRelation GetOppositeTextImageRelation(TextImageRelation relation)
		{
			return (TextImageRelation)LayoutUtils.GetOppositeAnchor((AnchorStyles)relation);
		}

		// Token: 0x06006564 RID: 25956 RVA: 0x0017359F File Offset: 0x0017259F
		public static Size UnionSizes(Size a, Size b)
		{
			return new Size(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
		}

		// Token: 0x06006565 RID: 25957 RVA: 0x001735CC File Offset: 0x001725CC
		public static Size IntersectSizes(Size a, Size b)
		{
			return new Size(Math.Min(a.Width, b.Width), Math.Min(a.Height, b.Height));
		}

		// Token: 0x06006566 RID: 25958 RVA: 0x001735FC File Offset: 0x001725FC
		public static bool IsIntersectHorizontally(Rectangle rect1, Rectangle rect2)
		{
			return rect1.IntersectsWith(rect2) && ((rect1.X <= rect2.X && rect1.X + rect1.Width >= rect2.X + rect2.Width) || (rect2.X <= rect1.X && rect2.X + rect2.Width >= rect1.X + rect1.Width));
		}

		// Token: 0x06006567 RID: 25959 RVA: 0x0017367C File Offset: 0x0017267C
		public static bool IsIntersectVertically(Rectangle rect1, Rectangle rect2)
		{
			return rect1.IntersectsWith(rect2) && ((rect1.Y <= rect2.Y && rect1.Y + rect1.Width >= rect2.Y + rect2.Width) || (rect2.Y <= rect1.Y && rect2.Y + rect2.Width >= rect1.Y + rect1.Width));
		}

		// Token: 0x06006568 RID: 25960 RVA: 0x001736FC File Offset: 0x001726FC
		internal static AnchorStyles GetUnifiedAnchor(IArrangedElement element)
		{
			DockStyle dock = DefaultLayout.GetDock(element);
			if (dock != DockStyle.None)
			{
				return LayoutUtils.dockingToAnchor[(int)dock];
			}
			return DefaultLayout.GetAnchor(element);
		}

		// Token: 0x06006569 RID: 25961 RVA: 0x00173721 File Offset: 0x00172721
		public static Rectangle AlignAndStretch(Size fitThis, Rectangle withinThis, AnchorStyles anchorStyles)
		{
			return LayoutUtils.Align(LayoutUtils.Stretch(fitThis, withinThis.Size, anchorStyles), withinThis, anchorStyles);
		}

		// Token: 0x0600656A RID: 25962 RVA: 0x00173738 File Offset: 0x00172738
		public static Rectangle Align(Size alignThis, Rectangle withinThis, AnchorStyles anchorStyles)
		{
			return LayoutUtils.VAlign(alignThis, LayoutUtils.HAlign(alignThis, withinThis, anchorStyles), anchorStyles);
		}

		// Token: 0x0600656B RID: 25963 RVA: 0x00173749 File Offset: 0x00172749
		public static Rectangle Align(Size alignThis, Rectangle withinThis, ContentAlignment align)
		{
			return LayoutUtils.VAlign(alignThis, LayoutUtils.HAlign(alignThis, withinThis, align), align);
		}

		// Token: 0x0600656C RID: 25964 RVA: 0x0017375C File Offset: 0x0017275C
		public static Rectangle HAlign(Size alignThis, Rectangle withinThis, AnchorStyles anchorStyles)
		{
			if ((anchorStyles & AnchorStyles.Right) != AnchorStyles.None)
			{
				withinThis.X += withinThis.Width - alignThis.Width;
			}
			else if (anchorStyles == AnchorStyles.None || (anchorStyles & (AnchorStyles.Left | AnchorStyles.Right)) == AnchorStyles.None)
			{
				withinThis.X += (withinThis.Width - alignThis.Width) / 2;
			}
			withinThis.Width = alignThis.Width;
			return withinThis;
		}

		// Token: 0x0600656D RID: 25965 RVA: 0x001737C4 File Offset: 0x001727C4
		private static Rectangle HAlign(Size alignThis, Rectangle withinThis, ContentAlignment align)
		{
			if ((align & (ContentAlignment)1092) != (ContentAlignment)0)
			{
				withinThis.X += withinThis.Width - alignThis.Width;
			}
			else if ((align & (ContentAlignment)546) != (ContentAlignment)0)
			{
				withinThis.X += (withinThis.Width - alignThis.Width) / 2;
			}
			withinThis.Width = alignThis.Width;
			return withinThis;
		}

		// Token: 0x0600656E RID: 25966 RVA: 0x00173830 File Offset: 0x00172830
		public static Rectangle VAlign(Size alignThis, Rectangle withinThis, AnchorStyles anchorStyles)
		{
			if ((anchorStyles & AnchorStyles.Bottom) != AnchorStyles.None)
			{
				withinThis.Y += withinThis.Height - alignThis.Height;
			}
			else if (anchorStyles == AnchorStyles.None || (anchorStyles & (AnchorStyles.Top | AnchorStyles.Bottom)) == AnchorStyles.None)
			{
				withinThis.Y += (withinThis.Height - alignThis.Height) / 2;
			}
			withinThis.Height = alignThis.Height;
			return withinThis;
		}

		// Token: 0x0600656F RID: 25967 RVA: 0x00173898 File Offset: 0x00172898
		public static Rectangle VAlign(Size alignThis, Rectangle withinThis, ContentAlignment align)
		{
			if ((align & (ContentAlignment)1792) != (ContentAlignment)0)
			{
				withinThis.Y += withinThis.Height - alignThis.Height;
			}
			else if ((align & (ContentAlignment)112) != (ContentAlignment)0)
			{
				withinThis.Y += (withinThis.Height - alignThis.Height) / 2;
			}
			withinThis.Height = alignThis.Height;
			return withinThis;
		}

		// Token: 0x06006570 RID: 25968 RVA: 0x00173904 File Offset: 0x00172904
		public static Size Stretch(Size stretchThis, Size withinThis, AnchorStyles anchorStyles)
		{
			Size result = new Size(((anchorStyles & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right)) ? withinThis.Width : stretchThis.Width, ((anchorStyles & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom)) ? withinThis.Height : stretchThis.Height);
			if (result.Width > withinThis.Width)
			{
				result.Width = withinThis.Width;
			}
			if (result.Height > withinThis.Height)
			{
				result.Height = withinThis.Height;
			}
			return result;
		}

		// Token: 0x06006571 RID: 25969 RVA: 0x00173984 File Offset: 0x00172984
		public static Rectangle InflateRect(Rectangle rect, Padding padding)
		{
			rect.X -= padding.Left;
			rect.Y -= padding.Top;
			rect.Width += padding.Horizontal;
			rect.Height += padding.Vertical;
			return rect;
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x001739E8 File Offset: 0x001729E8
		public static Rectangle DeflateRect(Rectangle rect, Padding padding)
		{
			rect.X += padding.Left;
			rect.Y += padding.Top;
			rect.Width -= padding.Horizontal;
			rect.Height -= padding.Vertical;
			return rect;
		}

		// Token: 0x06006573 RID: 25971 RVA: 0x00173A4A File Offset: 0x00172A4A
		public static Size AddAlignedRegion(Size textSize, Size imageSize, TextImageRelation relation)
		{
			return LayoutUtils.AddAlignedRegionCore(textSize, imageSize, LayoutUtils.IsVerticalRelation(relation));
		}

		// Token: 0x06006574 RID: 25972 RVA: 0x00173A5C File Offset: 0x00172A5C
		public static Size AddAlignedRegionCore(Size currentSize, Size contentSize, bool vertical)
		{
			if (vertical)
			{
				currentSize.Width = Math.Max(currentSize.Width, contentSize.Width);
				currentSize.Height += contentSize.Height;
			}
			else
			{
				currentSize.Width += contentSize.Width;
				currentSize.Height = Math.Max(currentSize.Height, contentSize.Height);
			}
			return currentSize;
		}

		// Token: 0x06006575 RID: 25973 RVA: 0x00173AD0 File Offset: 0x00172AD0
		public static Padding FlipPadding(Padding padding)
		{
			if (padding.All != -1)
			{
				return padding;
			}
			int num = padding.Top;
			padding.Top = padding.Left;
			padding.Left = num;
			num = padding.Bottom;
			padding.Bottom = padding.Right;
			padding.Right = num;
			return padding;
		}

		// Token: 0x06006576 RID: 25974 RVA: 0x00173B28 File Offset: 0x00172B28
		public static Point FlipPoint(Point point)
		{
			int x = point.X;
			point.X = point.Y;
			point.Y = x;
			return point;
		}

		// Token: 0x06006577 RID: 25975 RVA: 0x00173B54 File Offset: 0x00172B54
		public static Rectangle FlipRectangle(Rectangle rect)
		{
			rect.Location = LayoutUtils.FlipPoint(rect.Location);
			rect.Size = LayoutUtils.FlipSize(rect.Size);
			return rect;
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x00173B7D File Offset: 0x00172B7D
		public static Rectangle FlipRectangleIf(bool condition, Rectangle rect)
		{
			if (!condition)
			{
				return rect;
			}
			return LayoutUtils.FlipRectangle(rect);
		}

		// Token: 0x06006579 RID: 25977 RVA: 0x00173B8C File Offset: 0x00172B8C
		public static Size FlipSize(Size size)
		{
			int width = size.Width;
			size.Width = size.Height;
			size.Height = width;
			return size;
		}

		// Token: 0x0600657A RID: 25978 RVA: 0x00173BB8 File Offset: 0x00172BB8
		public static Size FlipSizeIf(bool condition, Size size)
		{
			if (!condition)
			{
				return size;
			}
			return LayoutUtils.FlipSize(size);
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x00173BC5 File Offset: 0x00172BC5
		public static bool IsHorizontalAlignment(ContentAlignment align)
		{
			return !LayoutUtils.IsVerticalAlignment(align);
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x00173BD0 File Offset: 0x00172BD0
		public static bool IsHorizontalRelation(TextImageRelation relation)
		{
			return (relation & (TextImageRelation)12) != TextImageRelation.Overlay;
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x00173BDC File Offset: 0x00172BDC
		public static bool IsVerticalAlignment(ContentAlignment align)
		{
			return (align & (ContentAlignment)514) != (ContentAlignment)0;
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x00173BEB File Offset: 0x00172BEB
		public static bool IsVerticalRelation(TextImageRelation relation)
		{
			return (relation & (TextImageRelation)3) != TextImageRelation.Overlay;
		}

		// Token: 0x0600657F RID: 25983 RVA: 0x00173BF6 File Offset: 0x00172BF6
		public static bool IsZeroWidthOrHeight(Rectangle rectangle)
		{
			return rectangle.Width == 0 || rectangle.Height == 0;
		}

		// Token: 0x06006580 RID: 25984 RVA: 0x00173C0D File Offset: 0x00172C0D
		public static bool IsZeroWidthOrHeight(Size size)
		{
			return size.Width == 0 || size.Height == 0;
		}

		// Token: 0x06006581 RID: 25985 RVA: 0x00173C24 File Offset: 0x00172C24
		public static bool AreWidthAndHeightLarger(Size size1, Size size2)
		{
			return size1.Width >= size2.Width && size1.Height >= size2.Height;
		}

		// Token: 0x06006582 RID: 25986 RVA: 0x00173C4C File Offset: 0x00172C4C
		public static void SplitRegion(Rectangle bounds, Size specifiedContent, AnchorStyles region1Align, out Rectangle region1, out Rectangle region2)
		{
			region1 = (region2 = bounds);
			switch (region1Align)
			{
			case AnchorStyles.Top:
				region1.Height = specifiedContent.Height;
				region2.Y += specifiedContent.Height;
				region2.Height -= specifiedContent.Height;
				return;
			case AnchorStyles.Bottom:
				region1.Y += bounds.Height - specifiedContent.Height;
				region1.Height = specifiedContent.Height;
				region2.Height -= specifiedContent.Height;
				break;
			case AnchorStyles.Top | AnchorStyles.Bottom:
				break;
			case AnchorStyles.Left:
				region1.Width = specifiedContent.Width;
				region2.X += specifiedContent.Width;
				region2.Width -= specifiedContent.Width;
				return;
			default:
				if (region1Align != AnchorStyles.Right)
				{
					return;
				}
				region1.X += bounds.Width - specifiedContent.Width;
				region1.Width = specifiedContent.Width;
				region2.Width -= specifiedContent.Width;
				return;
			}
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x00173D78 File Offset: 0x00172D78
		public static void ExpandRegionsToFillBounds(Rectangle bounds, AnchorStyles region1Align, ref Rectangle region1, ref Rectangle region2)
		{
			switch (region1Align)
			{
			case AnchorStyles.Top:
				region1 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region1, AnchorStyles.Bottom);
				region2 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region2, AnchorStyles.Top);
				return;
			case AnchorStyles.Bottom:
				region1 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region1, AnchorStyles.Top);
				region2 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region2, AnchorStyles.Bottom);
				break;
			case AnchorStyles.Top | AnchorStyles.Bottom:
				break;
			case AnchorStyles.Left:
				region1 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region1, AnchorStyles.Right);
				region2 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region2, AnchorStyles.Left);
				return;
			default:
				if (region1Align != AnchorStyles.Right)
				{
					return;
				}
				region1 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region1, AnchorStyles.Left);
				region2 = LayoutUtils.SubstituteSpecifiedBounds(bounds, region2, AnchorStyles.Right);
				return;
			}
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x00173E3F File Offset: 0x00172E3F
		public static Size SubAlignedRegion(Size currentSize, Size contentSize, TextImageRelation relation)
		{
			return LayoutUtils.SubAlignedRegionCore(currentSize, contentSize, LayoutUtils.IsVerticalRelation(relation));
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x00173E4E File Offset: 0x00172E4E
		public static Size SubAlignedRegionCore(Size currentSize, Size contentSize, bool vertical)
		{
			if (vertical)
			{
				currentSize.Height -= contentSize.Height;
			}
			else
			{
				currentSize.Width -= contentSize.Width;
			}
			return currentSize;
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x00173E80 File Offset: 0x00172E80
		private static Rectangle SubstituteSpecifiedBounds(Rectangle originalBounds, Rectangle substitutionBounds, AnchorStyles specified)
		{
			int left = ((specified & AnchorStyles.Left) != AnchorStyles.None) ? substitutionBounds.Left : originalBounds.Left;
			int top = ((specified & AnchorStyles.Top) != AnchorStyles.None) ? substitutionBounds.Top : originalBounds.Top;
			int right = ((specified & AnchorStyles.Right) != AnchorStyles.None) ? substitutionBounds.Right : originalBounds.Right;
			int bottom = ((specified & AnchorStyles.Bottom) != AnchorStyles.None) ? substitutionBounds.Bottom : originalBounds.Bottom;
			return Rectangle.FromLTRB(left, top, right, bottom);
		}

		// Token: 0x06006587 RID: 25991 RVA: 0x00173EEE File Offset: 0x00172EEE
		public static Rectangle RTLTranslate(Rectangle bounds, Rectangle withinBounds)
		{
			bounds.X = withinBounds.Width - bounds.Right;
			return bounds;
		}

		// Token: 0x04003C3A RID: 15418
		public const ContentAlignment AnyTop = (ContentAlignment)7;

		// Token: 0x04003C3B RID: 15419
		public const ContentAlignment AnyBottom = (ContentAlignment)1792;

		// Token: 0x04003C3C RID: 15420
		public const ContentAlignment AnyLeft = (ContentAlignment)273;

		// Token: 0x04003C3D RID: 15421
		public const ContentAlignment AnyRight = (ContentAlignment)1092;

		// Token: 0x04003C3E RID: 15422
		public const ContentAlignment AnyCenter = (ContentAlignment)546;

		// Token: 0x04003C3F RID: 15423
		public const ContentAlignment AnyMiddle = (ContentAlignment)112;

		// Token: 0x04003C40 RID: 15424
		public const AnchorStyles HorizontalAnchorStyles = AnchorStyles.Left | AnchorStyles.Right;

		// Token: 0x04003C41 RID: 15425
		public const AnchorStyles VerticalAnchorStyles = AnchorStyles.Top | AnchorStyles.Bottom;

		// Token: 0x04003C42 RID: 15426
		public static readonly Size MaxSize = new Size(int.MaxValue, int.MaxValue);

		// Token: 0x04003C43 RID: 15427
		public static readonly Size InvalidSize = new Size(int.MinValue, int.MinValue);

		// Token: 0x04003C44 RID: 15428
		public static readonly Rectangle MaxRectangle = new Rectangle(0, 0, int.MaxValue, int.MaxValue);

		// Token: 0x04003C45 RID: 15429
		private static readonly AnchorStyles[] dockingToAnchor = new AnchorStyles[]
		{
			AnchorStyles.Top | AnchorStyles.Left,
			AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
			AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
			AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
			AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right,
			AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
		};

		// Token: 0x04003C46 RID: 15430
		public static readonly string TestString = "j^";

		// Token: 0x0200078B RID: 1931
		public sealed class MeasureTextCache
		{
			// Token: 0x0600658A RID: 25994 RVA: 0x00173F8E File Offset: 0x00172F8E
			public void InvalidateCache()
			{
				this.unconstrainedPreferredSize = LayoutUtils.InvalidSize;
				this.sizeCacheList = null;
			}

			// Token: 0x0600658B RID: 25995 RVA: 0x00173FA4 File Offset: 0x00172FA4
			public Size GetTextSize(string text, Font font, Size proposedConstraints, TextFormatFlags flags)
			{
				if (!this.TextRequiresWordBreak(text, font, proposedConstraints, flags))
				{
					return this.unconstrainedPreferredSize;
				}
				if (this.sizeCacheList == null)
				{
					this.sizeCacheList = new LayoutUtils.MeasureTextCache.PreferredSizeCache[6];
				}
				LayoutUtils.MeasureTextCache.PreferredSizeCache[] array = this.sizeCacheList;
				int i = 0;
				while (i < array.Length)
				{
					LayoutUtils.MeasureTextCache.PreferredSizeCache preferredSizeCache = array[i];
					if (!(preferredSizeCache.ConstrainingSize == proposedConstraints))
					{
						Size constrainingSize = preferredSizeCache.ConstrainingSize;
						if (constrainingSize.Width == proposedConstraints.Width)
						{
							Size preferredSize = preferredSizeCache.PreferredSize;
							if (preferredSize.Height <= proposedConstraints.Height)
							{
								return preferredSizeCache.PreferredSize;
							}
						}
						i++;
						continue;
					}
					return preferredSizeCache.PreferredSize;
				}
				Size size = TextRenderer.MeasureText(text, font, proposedConstraints, flags);
				this.nextCacheEntry = (this.nextCacheEntry + 1) % 6;
				this.sizeCacheList[this.nextCacheEntry] = new LayoutUtils.MeasureTextCache.PreferredSizeCache(proposedConstraints, size);
				return size;
			}

			// Token: 0x0600658C RID: 25996 RVA: 0x00174093 File Offset: 0x00173093
			private Size GetUnconstrainedSize(string text, Font font, TextFormatFlags flags)
			{
				if (this.unconstrainedPreferredSize == LayoutUtils.InvalidSize)
				{
					flags &= ~TextFormatFlags.WordBreak;
					this.unconstrainedPreferredSize = TextRenderer.MeasureText(text, font, LayoutUtils.MaxSize, flags);
				}
				return this.unconstrainedPreferredSize;
			}

			// Token: 0x0600658D RID: 25997 RVA: 0x001740C8 File Offset: 0x001730C8
			public bool TextRequiresWordBreak(string text, Font font, Size size, TextFormatFlags flags)
			{
				return this.GetUnconstrainedSize(text, font, flags).Width > size.Width;
			}

			// Token: 0x04003C47 RID: 15431
			private const int MaxCacheSize = 6;

			// Token: 0x04003C48 RID: 15432
			private Size unconstrainedPreferredSize = LayoutUtils.InvalidSize;

			// Token: 0x04003C49 RID: 15433
			private int nextCacheEntry = -1;

			// Token: 0x04003C4A RID: 15434
			private LayoutUtils.MeasureTextCache.PreferredSizeCache[] sizeCacheList;

			// Token: 0x0200078C RID: 1932
			private struct PreferredSizeCache
			{
				// Token: 0x0600658F RID: 25999 RVA: 0x0017410A File Offset: 0x0017310A
				public PreferredSizeCache(Size constrainingSize, Size preferredSize)
				{
					this.ConstrainingSize = constrainingSize;
					this.PreferredSize = preferredSize;
				}

				// Token: 0x04003C4B RID: 15435
				public Size ConstrainingSize;

				// Token: 0x04003C4C RID: 15436
				public Size PreferredSize;
			}
		}
	}
}
