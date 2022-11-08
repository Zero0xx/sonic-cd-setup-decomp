using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003A1 RID: 929
	internal class DataGridViewUtilities
	{
		// Token: 0x06003896 RID: 14486 RVA: 0x000CED38 File Offset: 0x000CDD38
		internal static ContentAlignment ComputeDrawingContentAlignmentForCellStyleAlignment(DataGridViewContentAlignment alignment)
		{
			if (alignment <= DataGridViewContentAlignment.MiddleCenter)
			{
				switch (alignment)
				{
				case DataGridViewContentAlignment.TopLeft:
					return ContentAlignment.TopLeft;
				case DataGridViewContentAlignment.TopCenter:
					return ContentAlignment.TopCenter;
				case (DataGridViewContentAlignment)3:
					break;
				case DataGridViewContentAlignment.TopRight:
					return ContentAlignment.TopRight;
				default:
					if (alignment == DataGridViewContentAlignment.MiddleLeft)
					{
						return ContentAlignment.MiddleLeft;
					}
					if (alignment == DataGridViewContentAlignment.MiddleCenter)
					{
						return ContentAlignment.MiddleCenter;
					}
					break;
				}
			}
			else if (alignment <= DataGridViewContentAlignment.BottomLeft)
			{
				if (alignment == DataGridViewContentAlignment.MiddleRight)
				{
					return ContentAlignment.MiddleRight;
				}
				if (alignment == DataGridViewContentAlignment.BottomLeft)
				{
					return ContentAlignment.BottomLeft;
				}
			}
			else
			{
				if (alignment == DataGridViewContentAlignment.BottomCenter)
				{
					return ContentAlignment.BottomCenter;
				}
				if (alignment == DataGridViewContentAlignment.BottomRight)
				{
					return ContentAlignment.BottomRight;
				}
			}
			return ContentAlignment.MiddleCenter;
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x000CEDBC File Offset: 0x000CDDBC
		internal static TextFormatFlags ComputeTextFormatFlagsForCellStyleAlignment(bool rightToLeft, DataGridViewContentAlignment alignment, DataGridViewTriState wrapMode)
		{
			TextFormatFlags textFormatFlags;
			if (alignment <= DataGridViewContentAlignment.MiddleCenter)
			{
				switch (alignment)
				{
				case DataGridViewContentAlignment.TopLeft:
					textFormatFlags = TextFormatFlags.Default;
					if (rightToLeft)
					{
						textFormatFlags |= TextFormatFlags.Right;
						goto IL_C0;
					}
					textFormatFlags = textFormatFlags;
					goto IL_C0;
				case DataGridViewContentAlignment.TopCenter:
					textFormatFlags = TextFormatFlags.HorizontalCenter;
					goto IL_C0;
				case (DataGridViewContentAlignment)3:
					break;
				case DataGridViewContentAlignment.TopRight:
					textFormatFlags = TextFormatFlags.Default;
					if (rightToLeft)
					{
						textFormatFlags = textFormatFlags;
						goto IL_C0;
					}
					textFormatFlags |= TextFormatFlags.Right;
					goto IL_C0;
				default:
					if (alignment != DataGridViewContentAlignment.MiddleLeft)
					{
						if (alignment == DataGridViewContentAlignment.MiddleCenter)
						{
							textFormatFlags = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
							goto IL_C0;
						}
					}
					else
					{
						textFormatFlags = TextFormatFlags.VerticalCenter;
						if (rightToLeft)
						{
							textFormatFlags |= TextFormatFlags.Right;
							goto IL_C0;
						}
						textFormatFlags = textFormatFlags;
						goto IL_C0;
					}
					break;
				}
			}
			else if (alignment <= DataGridViewContentAlignment.BottomLeft)
			{
				if (alignment != DataGridViewContentAlignment.MiddleRight)
				{
					if (alignment == DataGridViewContentAlignment.BottomLeft)
					{
						textFormatFlags = TextFormatFlags.Bottom;
						if (rightToLeft)
						{
							textFormatFlags |= TextFormatFlags.Right;
							goto IL_C0;
						}
						textFormatFlags = textFormatFlags;
						goto IL_C0;
					}
				}
				else
				{
					textFormatFlags = TextFormatFlags.VerticalCenter;
					if (rightToLeft)
					{
						textFormatFlags = textFormatFlags;
						goto IL_C0;
					}
					textFormatFlags |= TextFormatFlags.Right;
					goto IL_C0;
				}
			}
			else
			{
				if (alignment == DataGridViewContentAlignment.BottomCenter)
				{
					textFormatFlags = (TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
					goto IL_C0;
				}
				if (alignment == DataGridViewContentAlignment.BottomRight)
				{
					textFormatFlags = TextFormatFlags.Bottom;
					if (rightToLeft)
					{
						textFormatFlags = textFormatFlags;
						goto IL_C0;
					}
					textFormatFlags |= TextFormatFlags.Right;
					goto IL_C0;
				}
			}
			textFormatFlags = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
			IL_C0:
			if (wrapMode == DataGridViewTriState.False)
			{
				textFormatFlags |= TextFormatFlags.SingleLine;
			}
			else
			{
				textFormatFlags |= TextFormatFlags.WordBreak;
			}
			textFormatFlags |= TextFormatFlags.NoPrefix;
			textFormatFlags |= TextFormatFlags.PreserveGraphicsClipping;
			if (rightToLeft)
			{
				textFormatFlags |= TextFormatFlags.RightToLeft;
			}
			return textFormatFlags;
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x000CEEB8 File Offset: 0x000CDEB8
		internal static Size GetPreferredRowHeaderSize(Graphics graphics, string val, DataGridViewCellStyle cellStyle, int borderAndPaddingWidths, int borderAndPaddingHeights, bool showRowErrors, bool showGlyph, Size constraintSize, TextFormatFlags flags)
		{
			switch (DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize))
			{
			case DataGridViewFreeDimension.Height:
			{
				int val2 = 1;
				int num = 1;
				int num2 = constraintSize.Width - borderAndPaddingWidths;
				if (!string.IsNullOrEmpty(val))
				{
					if (showGlyph && num2 >= 18)
					{
						val2 = 15;
						num2 -= 18;
					}
					if (showRowErrors && num2 >= 18)
					{
						val2 = 15;
						num2 -= 18;
					}
					if (num2 > 9)
					{
						num2 -= 9;
						if (cellStyle.WrapMode == DataGridViewTriState.True)
						{
							num = DataGridViewCell.MeasureTextHeight(graphics, val, cellStyle.Font, num2, flags);
						}
						else
						{
							num = DataGridViewCell.MeasureTextSize(graphics, val, cellStyle.Font, flags).Height;
						}
						num += 2;
					}
				}
				else if ((showGlyph || showRowErrors) && num2 >= 18)
				{
					val2 = 15;
				}
				return new Size(0, Math.Max(val2, num) + borderAndPaddingHeights);
			}
			case DataGridViewFreeDimension.Width:
			{
				int num3 = 0;
				int num4 = constraintSize.Height - borderAndPaddingHeights;
				if (!string.IsNullOrEmpty(val))
				{
					int num5 = num4 - 2;
					if (num5 > 0)
					{
						if (cellStyle.WrapMode == DataGridViewTriState.True)
						{
							num3 = DataGridViewCell.MeasureTextWidth(graphics, val, cellStyle.Font, num5, flags);
						}
						else
						{
							num3 = DataGridViewCell.MeasureTextSize(graphics, val, cellStyle.Font, flags).Width;
						}
						num3 += 9;
					}
				}
				if (num4 >= 15)
				{
					if (showGlyph)
					{
						num3 += 18;
					}
					if (showRowErrors)
					{
						num3 += 18;
					}
				}
				num3 = Math.Max(num3, 1);
				num3 += borderAndPaddingWidths;
				return new Size(num3, 0);
			}
			default:
			{
				Size result;
				if (!string.IsNullOrEmpty(val))
				{
					if (cellStyle.WrapMode == DataGridViewTriState.True)
					{
						result = DataGridViewCell.MeasureTextPreferredSize(graphics, val, cellStyle.Font, 5f, flags);
					}
					else
					{
						result = DataGridViewCell.MeasureTextSize(graphics, val, cellStyle.Font, flags);
					}
					result.Width += 9;
					result.Height += 2;
				}
				else
				{
					result = new Size(0, 1);
				}
				if (showGlyph)
				{
					result.Width += 18;
				}
				if (showRowErrors)
				{
					result.Width += 18;
				}
				if (showGlyph || showRowErrors)
				{
					result.Height = Math.Max(result.Height, 15);
				}
				result.Width += borderAndPaddingWidths;
				result.Height += borderAndPaddingHeights;
				return result;
			}
			}
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x000CF0E7 File Offset: 0x000CE0E7
		internal static Rectangle GetTextBounds(Rectangle cellBounds, string text, TextFormatFlags flags, DataGridViewCellStyle cellStyle)
		{
			return DataGridViewUtilities.GetTextBounds(cellBounds, text, flags, cellStyle, cellStyle.Font);
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x000CF0F8 File Offset: 0x000CE0F8
		internal static Rectangle GetTextBounds(Rectangle cellBounds, string text, TextFormatFlags flags, DataGridViewCellStyle cellStyle, Font font)
		{
			if ((flags & TextFormatFlags.SingleLine) != TextFormatFlags.Default && TextRenderer.MeasureText(text, font, new Size(2147483647, 2147483647), flags).Width > cellBounds.Width)
			{
				flags |= TextFormatFlags.EndEllipsis;
			}
			Size size = new Size(cellBounds.Width, cellBounds.Height);
			Size size2 = TextRenderer.MeasureText(text, font, size, flags);
			if (size2.Width > size.Width)
			{
				size2.Width = size.Width;
			}
			if (size2.Height > size.Height)
			{
				size2.Height = size.Height;
			}
			if (size2 == size)
			{
				return cellBounds;
			}
			return new Rectangle(DataGridViewUtilities.GetTextLocation(cellBounds, size2, flags, cellStyle), size2);
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x000CF1B4 File Offset: 0x000CE1B4
		internal static Point GetTextLocation(Rectangle cellBounds, Size sizeText, TextFormatFlags flags, DataGridViewCellStyle cellStyle)
		{
			Point result = new Point(0, 0);
			DataGridViewContentAlignment dataGridViewContentAlignment = cellStyle.Alignment;
			if ((flags & TextFormatFlags.RightToLeft) != TextFormatFlags.Default)
			{
				DataGridViewContentAlignment dataGridViewContentAlignment2 = dataGridViewContentAlignment;
				if (dataGridViewContentAlignment2 <= DataGridViewContentAlignment.MiddleLeft)
				{
					if (dataGridViewContentAlignment2 != DataGridViewContentAlignment.TopLeft)
					{
						if (dataGridViewContentAlignment2 != DataGridViewContentAlignment.TopRight)
						{
							if (dataGridViewContentAlignment2 == DataGridViewContentAlignment.MiddleLeft)
							{
								dataGridViewContentAlignment = DataGridViewContentAlignment.MiddleRight;
							}
						}
						else
						{
							dataGridViewContentAlignment = DataGridViewContentAlignment.TopLeft;
						}
					}
					else
					{
						dataGridViewContentAlignment = DataGridViewContentAlignment.TopRight;
					}
				}
				else if (dataGridViewContentAlignment2 != DataGridViewContentAlignment.MiddleRight)
				{
					if (dataGridViewContentAlignment2 != DataGridViewContentAlignment.BottomLeft)
					{
						if (dataGridViewContentAlignment2 == DataGridViewContentAlignment.BottomRight)
						{
							dataGridViewContentAlignment = DataGridViewContentAlignment.BottomLeft;
						}
					}
					else
					{
						dataGridViewContentAlignment = DataGridViewContentAlignment.BottomRight;
					}
				}
				else
				{
					dataGridViewContentAlignment = DataGridViewContentAlignment.MiddleLeft;
				}
			}
			DataGridViewContentAlignment dataGridViewContentAlignment3 = dataGridViewContentAlignment;
			if (dataGridViewContentAlignment3 <= DataGridViewContentAlignment.MiddleCenter)
			{
				switch (dataGridViewContentAlignment3)
				{
				case DataGridViewContentAlignment.TopLeft:
					result.X = cellBounds.X;
					result.Y = cellBounds.Y;
					break;
				case DataGridViewContentAlignment.TopCenter:
					result.X = cellBounds.X + (cellBounds.Width - sizeText.Width) / 2;
					result.Y = cellBounds.Y;
					break;
				case (DataGridViewContentAlignment)3:
					break;
				case DataGridViewContentAlignment.TopRight:
					result.X = cellBounds.Right - sizeText.Width;
					result.Y = cellBounds.Y;
					break;
				default:
					if (dataGridViewContentAlignment3 != DataGridViewContentAlignment.MiddleLeft)
					{
						if (dataGridViewContentAlignment3 == DataGridViewContentAlignment.MiddleCenter)
						{
							result.X = cellBounds.X + (cellBounds.Width - sizeText.Width) / 2;
							result.Y = cellBounds.Y + (cellBounds.Height - sizeText.Height) / 2;
						}
					}
					else
					{
						result.X = cellBounds.X;
						result.Y = cellBounds.Y + (cellBounds.Height - sizeText.Height) / 2;
					}
					break;
				}
			}
			else if (dataGridViewContentAlignment3 <= DataGridViewContentAlignment.BottomLeft)
			{
				if (dataGridViewContentAlignment3 != DataGridViewContentAlignment.MiddleRight)
				{
					if (dataGridViewContentAlignment3 == DataGridViewContentAlignment.BottomLeft)
					{
						result.X = cellBounds.X;
						result.Y = cellBounds.Bottom - sizeText.Height;
					}
				}
				else
				{
					result.X = cellBounds.Right - sizeText.Width;
					result.Y = cellBounds.Y + (cellBounds.Height - sizeText.Height) / 2;
				}
			}
			else if (dataGridViewContentAlignment3 != DataGridViewContentAlignment.BottomCenter)
			{
				if (dataGridViewContentAlignment3 == DataGridViewContentAlignment.BottomRight)
				{
					result.X = cellBounds.Right - sizeText.Width;
					result.Y = cellBounds.Bottom - sizeText.Height;
				}
			}
			else
			{
				result.X = cellBounds.X + (cellBounds.Width - sizeText.Width) / 2;
				result.Y = cellBounds.Bottom - sizeText.Height;
			}
			return result;
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x000CF451 File Offset: 0x000CE451
		internal static bool ValidTextFormatFlags(TextFormatFlags flags)
		{
			return (flags & ~(TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis | TextFormatFlags.ExpandTabs | TextFormatFlags.ExternalLeading | TextFormatFlags.HidePrefix | TextFormatFlags.HorizontalCenter | TextFormatFlags.Internal | TextFormatFlags.ModifyString | TextFormatFlags.NoClipping | TextFormatFlags.NoPrefix | TextFormatFlags.NoFullWidthCharacterBreak | TextFormatFlags.PathEllipsis | TextFormatFlags.PrefixOnly | TextFormatFlags.Right | TextFormatFlags.RightToLeft | TextFormatFlags.SingleLine | TextFormatFlags.TextBoxControl | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform)) == TextFormatFlags.Default;
		}

		// Token: 0x04001C83 RID: 7299
		private const byte DATAGRIDVIEWROWHEADERCELL_iconMarginWidth = 3;

		// Token: 0x04001C84 RID: 7300
		private const byte DATAGRIDVIEWROWHEADERCELL_iconMarginHeight = 2;

		// Token: 0x04001C85 RID: 7301
		private const byte DATAGRIDVIEWROWHEADERCELL_contentMarginWidth = 3;

		// Token: 0x04001C86 RID: 7302
		private const byte DATAGRIDVIEWROWHEADERCELL_contentMarginHeight = 3;

		// Token: 0x04001C87 RID: 7303
		private const byte DATAGRIDVIEWROWHEADERCELL_iconsWidth = 12;

		// Token: 0x04001C88 RID: 7304
		private const byte DATAGRIDVIEWROWHEADERCELL_iconsHeight = 11;

		// Token: 0x04001C89 RID: 7305
		private const byte DATAGRIDVIEWROWHEADERCELL_horizontalTextMarginLeft = 1;

		// Token: 0x04001C8A RID: 7306
		private const byte DATAGRIDVIEWROWHEADERCELL_horizontalTextMarginRight = 2;

		// Token: 0x04001C8B RID: 7307
		private const byte DATAGRIDVIEWROWHEADERCELL_verticalTextMargin = 1;
	}
}
