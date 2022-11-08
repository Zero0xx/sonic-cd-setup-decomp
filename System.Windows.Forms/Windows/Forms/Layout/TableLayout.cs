using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Layout
{
	// Token: 0x02000790 RID: 1936
	internal class TableLayout : LayoutEngine
	{
		// Token: 0x06006599 RID: 26009 RVA: 0x001744E9 File Offset: 0x001734E9
		internal static TableLayoutSettings CreateSettings(IArrangedElement owner)
		{
			return new TableLayoutSettings(owner);
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x001744F4 File Offset: 0x001734F4
		internal override void ProcessSuspendedLayoutEventArgs(IArrangedElement container, LayoutEventArgs args)
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			foreach (string objB in TableLayout._propertiesWhichInvalidateCache)
			{
				if (object.ReferenceEquals(args.AffectedProperty, objB))
				{
					TableLayout.ClearCachedAssignments(containerInfo);
					return;
				}
			}
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x00174538 File Offset: 0x00173538
		internal override bool LayoutCore(IArrangedElement container, LayoutEventArgs args)
		{
			this.ProcessSuspendedLayoutEventArgs(container, args);
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			this.EnsureRowAndColumnAssignments(container, containerInfo, false);
			int cellBorderWidth = containerInfo.CellBorderWidth;
			Size size = container.DisplayRectangle.Size - new Size(cellBorderWidth, cellBorderWidth);
			size.Width = Math.Max(size.Width, 1);
			size.Height = Math.Max(size.Height, 1);
			Size usedSpace = this.ApplyStyles(containerInfo, size, false);
			this.ExpandLastElement(containerInfo, usedSpace, size);
			RectangleF displayRectF = container.DisplayRectangle;
			displayRectF.Inflate(-((float)cellBorderWidth / 2f), (float)(-(float)cellBorderWidth) / 2f);
			this.SetElementBounds(containerInfo, displayRectF);
			CommonProperties.SetLayoutBounds(containerInfo.Container, new Size(this.SumStrips(containerInfo.Columns, 0, containerInfo.Columns.Length), this.SumStrips(containerInfo.Rows, 0, containerInfo.Rows.Length)));
			return CommonProperties.GetAutoSize(container);
		}

		// Token: 0x0600659C RID: 26012 RVA: 0x0017462C File Offset: 0x0017362C
		internal override Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			bool flag = false;
			float num = -1f;
			Size size = containerInfo.GetCachedPreferredSize(proposedConstraints, out flag);
			if (flag)
			{
				return size;
			}
			TableLayout.ContainerInfo containerInfo2 = new TableLayout.ContainerInfo(containerInfo);
			int cellBorderWidth = containerInfo.CellBorderWidth;
			if (containerInfo.MaxColumns == 1 && containerInfo.ColumnStyles.Count > 0 && containerInfo.ColumnStyles[0].SizeType == SizeType.Absolute)
			{
				Size size2 = container.DisplayRectangle.Size - new Size(cellBorderWidth * 2, cellBorderWidth * 2);
				size2.Width = Math.Max(size2.Width, 1);
				size2.Height = Math.Max(size2.Height, 1);
				num = containerInfo.ColumnStyles[0].Size;
				containerInfo.ColumnStyles[0].SetSize(Math.Max(num, (float)Math.Min(proposedConstraints.Width, size2.Width)));
			}
			this.EnsureRowAndColumnAssignments(container, containerInfo2, true);
			Size sz = new Size(cellBorderWidth, cellBorderWidth);
			proposedConstraints -= sz;
			proposedConstraints.Width = Math.Max(proposedConstraints.Width, 1);
			proposedConstraints.Height = Math.Max(proposedConstraints.Height, 1);
			if (containerInfo2.Columns != null && containerInfo.Columns != null && containerInfo2.Columns.Length != containerInfo.Columns.Length)
			{
				TableLayout.ClearCachedAssignments(containerInfo);
			}
			if (containerInfo2.Rows != null && containerInfo.Rows != null && containerInfo2.Rows.Length != containerInfo.Rows.Length)
			{
				TableLayout.ClearCachedAssignments(containerInfo);
			}
			size = this.ApplyStyles(containerInfo2, proposedConstraints, true);
			if (num >= 0f)
			{
				containerInfo.ColumnStyles[0].SetSize(num);
			}
			return size + sz;
		}

		// Token: 0x0600659D RID: 26013 RVA: 0x001747EB File Offset: 0x001737EB
		private void EnsureRowAndColumnAssignments(IArrangedElement container, TableLayout.ContainerInfo containerInfo, bool doNotCache)
		{
			if (!TableLayout.HasCachedAssignments(containerInfo) || doNotCache)
			{
				this.AssignRowsAndColumns(containerInfo);
			}
		}

		// Token: 0x0600659E RID: 26014 RVA: 0x00174800 File Offset: 0x00173800
		private void ExpandLastElement(TableLayout.ContainerInfo containerInfo, Size usedSpace, Size totalSpace)
		{
			TableLayout.Strip[] rows = containerInfo.Rows;
			TableLayout.Strip[] columns = containerInfo.Columns;
			if (columns.Length != 0 && totalSpace.Width > usedSpace.Width)
			{
				TableLayout.Strip[] array = columns;
				int num = columns.Length - 1;
				array[num].MinSize = array[num].MinSize + (totalSpace.Width - usedSpace.Width);
			}
			if (rows.Length != 0 && totalSpace.Height > usedSpace.Height)
			{
				TableLayout.Strip[] array2 = rows;
				int num2 = rows.Length - 1;
				array2[num2].MinSize = array2[num2].MinSize + (totalSpace.Height - usedSpace.Height);
			}
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x00174894 File Offset: 0x00173894
		private void AssignRowsAndColumns(TableLayout.ContainerInfo containerInfo)
		{
			int num = containerInfo.MaxColumns;
			int num2 = containerInfo.MaxRows;
			TableLayout.LayoutInfo[] childrenInfo = containerInfo.ChildrenInfo;
			int minRowsAndColumns = containerInfo.MinRowsAndColumns;
			int minColumns = containerInfo.MinColumns;
			int minRows = containerInfo.MinRows;
			TableLayoutPanelGrowStyle growStyle = containerInfo.GrowStyle;
			if (growStyle == TableLayoutPanelGrowStyle.FixedSize)
			{
				if (containerInfo.MinRowsAndColumns > num * num2)
				{
					throw new ArgumentException(SR.GetString("TableLayoutPanelFullDesc"));
				}
				if (minColumns > num || minRows > num2)
				{
					throw new ArgumentException(SR.GetString("TableLayoutPanelSpanDesc"));
				}
				num2 = Math.Max(1, num2);
				num = Math.Max(1, num);
			}
			else if (growStyle == TableLayoutPanelGrowStyle.AddRows)
			{
				num2 = 0;
			}
			else
			{
				num = 0;
			}
			if (num > 0)
			{
				this.xAssignRowsAndColumns(containerInfo, childrenInfo, num, (num2 == 0) ? int.MaxValue : num2, growStyle);
				return;
			}
			if (num2 > 0)
			{
				int num3 = Math.Max((int)Math.Ceiling((double)((float)minRowsAndColumns / (float)num2)), minColumns);
				num3 = Math.Max(num3, 1);
				while (!this.xAssignRowsAndColumns(containerInfo, childrenInfo, num3, num2, growStyle))
				{
					num3++;
				}
				return;
			}
			this.xAssignRowsAndColumns(containerInfo, childrenInfo, Math.Max(minColumns, 1), int.MaxValue, growStyle);
		}

		// Token: 0x060065A0 RID: 26016 RVA: 0x0017499C File Offset: 0x0017399C
		private bool xAssignRowsAndColumns(TableLayout.ContainerInfo containerInfo, TableLayout.LayoutInfo[] childrenInfo, int maxColumns, int maxRows, TableLayoutPanelGrowStyle growStyle)
		{
			int num = 0;
			int num2 = 0;
			TableLayout.ReservationGrid reservationGrid = new TableLayout.ReservationGrid();
			int num3 = 0;
			int num4 = 0;
			int num5 = -1;
			int num6 = -1;
			TableLayout.LayoutInfo[] fixedChildrenInfo = containerInfo.FixedChildrenInfo;
			TableLayout.LayoutInfo nextLayoutInfo = TableLayout.GetNextLayoutInfo(fixedChildrenInfo, ref num5, true);
			TableLayout.LayoutInfo nextLayoutInfo2 = TableLayout.GetNextLayoutInfo(childrenInfo, ref num6, false);
			while (nextLayoutInfo != null || nextLayoutInfo2 != null)
			{
				int num7 = num4;
				if (nextLayoutInfo2 != null)
				{
					nextLayoutInfo2.RowStart = num3;
					nextLayoutInfo2.ColumnStart = num4;
					this.AdvanceUntilFits(maxColumns, reservationGrid, nextLayoutInfo2, out num7);
					if (nextLayoutInfo2.RowStart >= maxRows)
					{
						return false;
					}
				}
				int num8;
				if (nextLayoutInfo2 != null && (nextLayoutInfo == null || (!this.IsCursorPastInsertionPoint(nextLayoutInfo, nextLayoutInfo2.RowStart, num7) && !this.IsOverlappingWithReservationGrid(nextLayoutInfo, reservationGrid, num3))))
				{
					for (int i = 0; i < nextLayoutInfo2.RowStart - num3; i++)
					{
						reservationGrid.AdvanceRow();
					}
					num3 = nextLayoutInfo2.RowStart;
					num8 = Math.Min(num3 + nextLayoutInfo2.RowSpan, maxRows);
					reservationGrid.ReserveAll(nextLayoutInfo2, num8, num7);
					nextLayoutInfo2 = TableLayout.GetNextLayoutInfo(childrenInfo, ref num6, false);
				}
				else
				{
					if (num4 >= maxColumns)
					{
						num4 = 0;
						num3++;
						reservationGrid.AdvanceRow();
					}
					nextLayoutInfo.RowStart = Math.Min(nextLayoutInfo.RowPosition, maxRows - 1);
					nextLayoutInfo.ColumnStart = Math.Min(nextLayoutInfo.ColumnPosition, maxColumns - 1);
					if (num3 > nextLayoutInfo.RowStart)
					{
						nextLayoutInfo.ColumnStart = num4;
					}
					else if (num3 == nextLayoutInfo.RowStart)
					{
						nextLayoutInfo.ColumnStart = Math.Max(nextLayoutInfo.ColumnStart, num4);
					}
					nextLayoutInfo.RowStart = Math.Max(nextLayoutInfo.RowStart, num3);
					int j;
					for (j = 0; j < nextLayoutInfo.RowStart - num3; j++)
					{
						reservationGrid.AdvanceRow();
					}
					this.AdvanceUntilFits(maxColumns, reservationGrid, nextLayoutInfo, out num7);
					if (nextLayoutInfo.RowStart >= maxRows)
					{
						return false;
					}
					while (j < nextLayoutInfo.RowStart - num3)
					{
						reservationGrid.AdvanceRow();
						j++;
					}
					num3 = nextLayoutInfo.RowStart;
					num7 = Math.Min(nextLayoutInfo.ColumnStart + nextLayoutInfo.ColumnSpan, maxColumns);
					num8 = Math.Min(nextLayoutInfo.RowStart + nextLayoutInfo.RowSpan, maxRows);
					reservationGrid.ReserveAll(nextLayoutInfo, num8, num7);
					nextLayoutInfo = TableLayout.GetNextLayoutInfo(fixedChildrenInfo, ref num5, true);
				}
				num4 = num7;
				num2 = ((num2 == int.MaxValue) ? num8 : Math.Max(num2, num8));
				num = ((num == int.MaxValue) ? num7 : Math.Max(num, num7));
			}
			if (growStyle == TableLayoutPanelGrowStyle.FixedSize)
			{
				num = maxColumns;
				num2 = maxRows;
			}
			else if (growStyle == TableLayoutPanelGrowStyle.AddRows)
			{
				num = maxColumns;
				num2 = Math.Max(containerInfo.MaxRows, num2);
			}
			else
			{
				num2 = ((maxRows == int.MaxValue) ? num2 : maxRows);
				num = Math.Max(containerInfo.MaxColumns, num);
			}
			if (containerInfo.Rows == null || containerInfo.Rows.Length != num2)
			{
				containerInfo.Rows = new TableLayout.Strip[num2];
			}
			if (containerInfo.Columns == null || containerInfo.Columns.Length != num)
			{
				containerInfo.Columns = new TableLayout.Strip[num];
			}
			containerInfo.Valid = true;
			return true;
		}

		// Token: 0x060065A1 RID: 26017 RVA: 0x00174C74 File Offset: 0x00173C74
		private static TableLayout.LayoutInfo GetNextLayoutInfo(TableLayout.LayoutInfo[] layoutInfo, ref int index, bool absolutelyPositioned)
		{
			for (int i = ++index; i < layoutInfo.Length; i++)
			{
				if (absolutelyPositioned == layoutInfo[i].IsAbsolutelyPositioned)
				{
					index = i;
					return layoutInfo[i];
				}
			}
			index = layoutInfo.Length;
			return null;
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x00174CAF File Offset: 0x00173CAF
		private bool IsCursorPastInsertionPoint(TableLayout.LayoutInfo fixedLayoutInfo, int insertionRow, int insertionCol)
		{
			return fixedLayoutInfo.RowPosition < insertionRow || (fixedLayoutInfo.RowPosition == insertionRow && fixedLayoutInfo.ColumnPosition < insertionCol);
		}

		// Token: 0x060065A3 RID: 26019 RVA: 0x00174CD4 File Offset: 0x00173CD4
		private bool IsOverlappingWithReservationGrid(TableLayout.LayoutInfo fixedLayoutInfo, TableLayout.ReservationGrid reservationGrid, int currentRow)
		{
			if (fixedLayoutInfo.RowPosition < currentRow)
			{
				return true;
			}
			for (int i = fixedLayoutInfo.RowPosition - currentRow; i < fixedLayoutInfo.RowPosition - currentRow + fixedLayoutInfo.RowSpan; i++)
			{
				for (int j = fixedLayoutInfo.ColumnPosition; j < fixedLayoutInfo.ColumnPosition + fixedLayoutInfo.ColumnSpan; j++)
				{
					if (reservationGrid.IsReserved(j, i))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060065A4 RID: 26020 RVA: 0x00174D38 File Offset: 0x00173D38
		private void AdvanceUntilFits(int maxColumns, TableLayout.ReservationGrid reservationGrid, TableLayout.LayoutInfo layoutInfo, out int colStop)
		{
			int rowStart = layoutInfo.RowStart;
			do
			{
				this.GetColStartAndStop(maxColumns, reservationGrid, layoutInfo, out colStop);
			}
			while (this.ScanRowForOverlap(maxColumns, reservationGrid, layoutInfo, colStop, layoutInfo.RowStart - rowStart));
		}

		// Token: 0x060065A5 RID: 26021 RVA: 0x00174D70 File Offset: 0x00173D70
		private void GetColStartAndStop(int maxColumns, TableLayout.ReservationGrid reservationGrid, TableLayout.LayoutInfo layoutInfo, out int colStop)
		{
			colStop = layoutInfo.ColumnStart + layoutInfo.ColumnSpan;
			if (colStop > maxColumns)
			{
				if (layoutInfo.ColumnStart != 0)
				{
					layoutInfo.ColumnStart = 0;
					layoutInfo.RowStart++;
				}
				colStop = Math.Min(layoutInfo.ColumnSpan, maxColumns);
			}
		}

		// Token: 0x060065A6 RID: 26022 RVA: 0x00174DC0 File Offset: 0x00173DC0
		private bool ScanRowForOverlap(int maxColumns, TableLayout.ReservationGrid reservationGrid, TableLayout.LayoutInfo layoutInfo, int stopCol, int rowOffset)
		{
			for (int i = layoutInfo.ColumnStart; i < stopCol; i++)
			{
				if (reservationGrid.IsReserved(i, rowOffset))
				{
					layoutInfo.ColumnStart = i + 1;
					while (layoutInfo.ColumnStart < maxColumns && reservationGrid.IsReserved(layoutInfo.ColumnStart, rowOffset))
					{
						layoutInfo.ColumnStart++;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060065A7 RID: 26023 RVA: 0x00174E20 File Offset: 0x00173E20
		private Size ApplyStyles(TableLayout.ContainerInfo containerInfo, Size proposedConstraints, bool measureOnly)
		{
			Size empty = Size.Empty;
			this.InitializeStrips(containerInfo.Columns, containerInfo.ColumnStyles);
			this.InitializeStrips(containerInfo.Rows, containerInfo.RowStyles);
			containerInfo.ChildHasColumnSpan = false;
			containerInfo.ChildHasRowSpan = false;
			foreach (TableLayout.LayoutInfo layoutInfo in containerInfo.ChildrenInfo)
			{
				containerInfo.Columns[layoutInfo.ColumnStart].IsStart = true;
				containerInfo.Rows[layoutInfo.RowStart].IsStart = true;
				if (layoutInfo.ColumnSpan > 1)
				{
					containerInfo.ChildHasColumnSpan = true;
				}
				if (layoutInfo.RowSpan > 1)
				{
					containerInfo.ChildHasRowSpan = true;
				}
			}
			empty.Width = this.InflateColumns(containerInfo, proposedConstraints, measureOnly);
			int expandLastElementWidth = Math.Max(0, proposedConstraints.Width - empty.Width);
			empty.Height = this.InflateRows(containerInfo, proposedConstraints, expandLastElementWidth, measureOnly);
			return empty;
		}

		// Token: 0x060065A8 RID: 26024 RVA: 0x00174F0C File Offset: 0x00173F0C
		private void InitializeStrips(TableLayout.Strip[] strips, IList styles)
		{
			for (int i = 0; i < strips.Length; i++)
			{
				TableLayoutStyle tableLayoutStyle = (i < styles.Count) ? ((TableLayoutStyle)styles[i]) : null;
				TableLayout.Strip strip = strips[i];
				if (tableLayoutStyle != null && tableLayoutStyle.SizeType == SizeType.Absolute)
				{
					strip.MinSize = (int)Math.Round((double)((TableLayoutStyle)styles[i]).Size);
					strip.MaxSize = strip.MinSize;
				}
				else
				{
					strip.MinSize = 0;
					strip.MaxSize = 0;
				}
				strip.IsStart = false;
				strips[i] = strip;
			}
		}

		// Token: 0x060065A9 RID: 26025 RVA: 0x00174FB4 File Offset: 0x00173FB4
		private int InflateColumns(TableLayout.ContainerInfo containerInfo, Size proposedConstraints, bool measureOnly)
		{
			bool flag = measureOnly;
			TableLayout.LayoutInfo[] childrenInfo = containerInfo.ChildrenInfo;
			if (containerInfo.ChildHasColumnSpan)
			{
				Array.Sort(childrenInfo, TableLayout.ColumnSpanComparer.GetInstance);
			}
			if (flag && proposedConstraints.Width < 32767)
			{
				TableLayoutPanel tableLayoutPanel = containerInfo.Container as TableLayoutPanel;
				if (tableLayoutPanel != null && tableLayoutPanel.ParentInternal != null && tableLayoutPanel.ParentInternal.LayoutEngine == DefaultLayout.Instance)
				{
					if (tableLayoutPanel.Dock == DockStyle.Top || tableLayoutPanel.Dock == DockStyle.Bottom || tableLayoutPanel.Dock == DockStyle.Fill)
					{
						flag = false;
					}
					if ((tableLayoutPanel.Anchor & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right))
					{
						flag = false;
					}
				}
			}
			foreach (TableLayout.LayoutInfo layoutInfo in childrenInfo)
			{
				IArrangedElement element = layoutInfo.Element;
				int columnSpan = layoutInfo.ColumnSpan;
				if (columnSpan > 1 || !this.IsAbsolutelySized(layoutInfo.ColumnStart, containerInfo.ColumnStyles))
				{
					int num;
					int num2;
					if (columnSpan == 1 && layoutInfo.RowSpan == 1 && this.IsAbsolutelySized(layoutInfo.RowStart, containerInfo.RowStyles))
					{
						int height = (int)containerInfo.RowStyles[layoutInfo.RowStart].Size;
						num = this.GetElementSize(element, new Size(0, height)).Width;
						num2 = num;
					}
					else
					{
						num = this.GetElementSize(element, new Size(1, 0)).Width;
						num2 = this.GetElementSize(element, Size.Empty).Width;
					}
					Padding margin = CommonProperties.GetMargin(element);
					num += margin.Horizontal;
					num2 += margin.Horizontal;
					int stop = Math.Min(layoutInfo.ColumnStart + layoutInfo.ColumnSpan, containerInfo.Columns.Length);
					this.DistributeSize(containerInfo.ColumnStyles, containerInfo.Columns, layoutInfo.ColumnStart, stop, num, num2, containerInfo.CellBorderWidth);
				}
			}
			int num3 = this.DistributeStyles(containerInfo.CellBorderWidth, containerInfo.ColumnStyles, containerInfo.Columns, proposedConstraints.Width, flag);
			if (flag && num3 > proposedConstraints.Width && proposedConstraints.Width > 1)
			{
				TableLayout.Strip[] columns = containerInfo.Columns;
				float num4 = 0f;
				int num5 = 0;
				TableLayoutStyleCollection columnStyles = containerInfo.ColumnStyles;
				for (int j = 0; j < columns.Length; j++)
				{
					TableLayout.Strip strip = columns[j];
					if (j < columnStyles.Count)
					{
						TableLayoutStyle tableLayoutStyle = columnStyles[j];
						if (tableLayoutStyle.SizeType == SizeType.Percent)
						{
							num4 += tableLayoutStyle.Size;
							num5 += strip.MinSize;
						}
					}
				}
				int val = num3 - proposedConstraints.Width;
				int num6 = Math.Min(val, num5);
				for (int k = 0; k < columns.Length; k++)
				{
					if (k < columnStyles.Count)
					{
						TableLayoutStyle tableLayoutStyle2 = columnStyles[k];
						if (tableLayoutStyle2.SizeType == SizeType.Percent)
						{
							float num7 = tableLayoutStyle2.Size / num4;
							TableLayout.Strip[] array2 = columns;
							int num8 = k;
							array2[num8].MinSize = array2[num8].MinSize - (int)(num7 * (float)num6);
						}
					}
				}
				return num3 - num6;
			}
			return num3;
		}

		// Token: 0x060065AA RID: 26026 RVA: 0x001752BC File Offset: 0x001742BC
		private int InflateRows(TableLayout.ContainerInfo containerInfo, Size proposedConstraints, int expandLastElementWidth, bool measureOnly)
		{
			bool flag = measureOnly;
			TableLayout.LayoutInfo[] childrenInfo = containerInfo.ChildrenInfo;
			if (containerInfo.ChildHasRowSpan)
			{
				Array.Sort(childrenInfo, TableLayout.RowSpanComparer.GetInstance);
			}
			bool hasMultiplePercentColumns = containerInfo.HasMultiplePercentColumns;
			if (flag && proposedConstraints.Height < 32767)
			{
				TableLayoutPanel tableLayoutPanel = containerInfo.Container as TableLayoutPanel;
				if (tableLayoutPanel != null && tableLayoutPanel.ParentInternal != null && tableLayoutPanel.ParentInternal.LayoutEngine == DefaultLayout.Instance)
				{
					if (tableLayoutPanel.Dock == DockStyle.Left || tableLayoutPanel.Dock == DockStyle.Right || tableLayoutPanel.Dock == DockStyle.Fill)
					{
						flag = false;
					}
					if ((tableLayoutPanel.Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom))
					{
						flag = false;
					}
				}
			}
			foreach (TableLayout.LayoutInfo layoutInfo in childrenInfo)
			{
				IArrangedElement element = layoutInfo.Element;
				int rowSpan = layoutInfo.RowSpan;
				if (rowSpan > 1 || !this.IsAbsolutelySized(layoutInfo.RowStart, containerInfo.RowStyles))
				{
					int num = this.SumStrips(containerInfo.Columns, layoutInfo.ColumnStart, layoutInfo.ColumnSpan);
					if (!flag && layoutInfo.ColumnStart + layoutInfo.ColumnSpan >= containerInfo.MaxColumns && !hasMultiplePercentColumns)
					{
						num += expandLastElementWidth;
					}
					Padding margin = CommonProperties.GetMargin(element);
					int num2 = this.GetElementSize(element, new Size(num - margin.Horizontal, 0)).Height + margin.Vertical;
					int max = num2;
					int stop = Math.Min(layoutInfo.RowStart + layoutInfo.RowSpan, containerInfo.Rows.Length);
					this.DistributeSize(containerInfo.RowStyles, containerInfo.Rows, layoutInfo.RowStart, stop, num2, max, containerInfo.CellBorderWidth);
				}
			}
			return this.DistributeStyles(containerInfo.CellBorderWidth, containerInfo.RowStyles, containerInfo.Rows, proposedConstraints.Height, flag);
		}

		// Token: 0x060065AB RID: 26027 RVA: 0x00175480 File Offset: 0x00174480
		private Size GetElementSize(IArrangedElement element, Size proposedConstraints)
		{
			if (CommonProperties.GetAutoSize(element))
			{
				return element.GetPreferredSize(proposedConstraints);
			}
			return CommonProperties.GetSpecifiedBounds(element).Size;
		}

		// Token: 0x060065AC RID: 26028 RVA: 0x001754AC File Offset: 0x001744AC
		internal int SumStrips(TableLayout.Strip[] strips, int start, int span)
		{
			int num = 0;
			for (int i = start; i < Math.Min(start + span, strips.Length); i++)
			{
				TableLayout.Strip strip = strips[i];
				num += strip.MinSize;
			}
			return num;
		}

		// Token: 0x060065AD RID: 26029 RVA: 0x001754E9 File Offset: 0x001744E9
		private void DistributeSize(IList styles, TableLayout.Strip[] strips, int start, int stop, int min, int max, int cellBorderWidth)
		{
			this.xDistributeSize(styles, strips, start, stop, min, TableLayout.MinSizeProxy.GetInstance, cellBorderWidth);
			this.xDistributeSize(styles, strips, start, stop, max, TableLayout.MaxSizeProxy.GetInstance, cellBorderWidth);
		}

		// Token: 0x060065AE RID: 26030 RVA: 0x00175514 File Offset: 0x00174514
		private void xDistributeSize(IList styles, TableLayout.Strip[] strips, int start, int stop, int desiredLength, TableLayout.SizeProxy sizeProxy, int cellBorderWidth)
		{
			int num = 0;
			int num2 = 0;
			desiredLength -= cellBorderWidth * (stop - start - 1);
			desiredLength = Math.Max(0, desiredLength);
			for (int i = start; i < stop; i++)
			{
				sizeProxy.Strip = strips[i];
				if (!this.IsAbsolutelySized(i, styles) && sizeProxy.Size == 0)
				{
					num2++;
				}
				num += sizeProxy.Size;
			}
			int num3 = desiredLength - num;
			if (num3 <= 0)
			{
				return;
			}
			if (num2 == 0)
			{
				int num4 = stop - 1;
				while (num4 >= start && (num4 >= styles.Count || ((TableLayoutStyle)styles[num4]).SizeType != SizeType.Percent))
				{
					num4--;
				}
				if (num4 != start - 1)
				{
					stop = num4 + 1;
				}
				for (int j = stop - 1; j >= start; j--)
				{
					if (!this.IsAbsolutelySized(j, styles))
					{
						sizeProxy.Strip = strips[j];
						if (j != strips.Length - 1 && !strips[j + 1].IsStart && !this.IsAbsolutelySized(j + 1, styles))
						{
							sizeProxy.Strip = strips[j + 1];
							int num5 = Math.Min(sizeProxy.Size, num3);
							sizeProxy.Size -= num5;
							strips[j + 1] = sizeProxy.Strip;
							sizeProxy.Strip = strips[j];
						}
						sizeProxy.Size += num3;
						strips[j] = sizeProxy.Strip;
						return;
					}
				}
				return;
			}
			int num6 = num3 / num2;
			int num7 = 0;
			for (int k = start; k < stop; k++)
			{
				sizeProxy.Strip = strips[k];
				if (!this.IsAbsolutelySized(k, styles) && sizeProxy.Size == 0)
				{
					num7++;
					if (num7 == num2)
					{
						num6 = num3 - num6 * (num2 - 1);
					}
					sizeProxy.Size += num6;
					strips[k] = sizeProxy.Strip;
				}
			}
		}

		// Token: 0x060065AF RID: 26031 RVA: 0x0017572D File Offset: 0x0017472D
		private bool IsAbsolutelySized(int index, IList styles)
		{
			return index < styles.Count && ((TableLayoutStyle)styles[index]).SizeType == SizeType.Absolute;
		}

		// Token: 0x060065B0 RID: 26032 RVA: 0x00175750 File Offset: 0x00174750
		private int DistributeStyles(int cellBorderWidth, IList styles, TableLayout.Strip[] strips, int maxSize, bool dontHonorConstraint)
		{
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			bool flag = false;
			for (int i = 0; i < strips.Length; i++)
			{
				TableLayout.Strip strip = strips[i];
				if (i < styles.Count)
				{
					TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)styles[i];
					switch (tableLayoutStyle.SizeType)
					{
					case SizeType.Absolute:
						num5 += (float)strip.MinSize;
						break;
					case SizeType.Percent:
						num3 += tableLayoutStyle.Size;
						num4 += (float)strip.MinSize;
						break;
					default:
						num5 += (float)strip.MinSize;
						flag = true;
						break;
					}
				}
				else
				{
					flag = true;
				}
				strip.MaxSize += cellBorderWidth;
				strip.MinSize += cellBorderWidth;
				strips[i] = strip;
				num += strip.MinSize;
			}
			int num6 = maxSize - num;
			if (num3 > 0f)
			{
				if (!dontHonorConstraint)
				{
					if (num4 > (float)maxSize - num5)
					{
						num4 = Math.Max(0f, (float)maxSize - num5);
					}
					if (num6 > 0)
					{
						num4 += (float)num6;
					}
					else if (num6 < 0)
					{
						num4 = (float)maxSize - num5 - (float)(strips.Length * cellBorderWidth);
					}
					for (int j = 0; j < strips.Length; j++)
					{
						TableLayout.Strip strip2 = strips[j];
						SizeType sizeType = (j < styles.Count) ? ((TableLayoutStyle)styles[j]).SizeType : SizeType.AutoSize;
						if (sizeType == SizeType.Percent)
						{
							TableLayoutStyle tableLayoutStyle2 = (TableLayoutStyle)styles[j];
							int num7 = (int)(tableLayoutStyle2.Size * num4 / num3);
							num -= strip2.MinSize;
							num += num7 + cellBorderWidth;
							strip2.MinSize = num7 + cellBorderWidth;
							strips[j] = strip2;
						}
					}
				}
				else
				{
					int num8 = 0;
					for (int k = 0; k < strips.Length; k++)
					{
						TableLayout.Strip strip3 = strips[k];
						SizeType sizeType2 = (k < styles.Count) ? ((TableLayoutStyle)styles[k]).SizeType : SizeType.AutoSize;
						if (sizeType2 == SizeType.Percent)
						{
							TableLayoutStyle tableLayoutStyle3 = (TableLayoutStyle)styles[k];
							int val = (int)Math.Round((double)((float)strip3.MinSize * num3 / tableLayoutStyle3.Size));
							num8 = Math.Max(num8, val);
							num -= strip3.MinSize;
						}
					}
					num += num8;
				}
			}
			num6 = maxSize - num;
			if (flag && num6 > 0)
			{
				if ((float)num6 < num2)
				{
					float num9 = (float)num6 / num2;
				}
				num6 -= (int)Math.Ceiling((double)num2);
				for (int l = 0; l < strips.Length; l++)
				{
					TableLayout.Strip strip4 = strips[l];
					if (l >= styles.Count || ((TableLayoutStyle)styles[l]).SizeType == SizeType.AutoSize)
					{
						int num10 = Math.Min(strip4.MaxSize - strip4.MinSize, num6);
						if (num10 > 0)
						{
							num += num10;
							num6 -= num10;
							strip4.MinSize += num10;
							strips[l] = strip4;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060065B1 RID: 26033 RVA: 0x00175A8C File Offset: 0x00174A8C
		private void SetElementBounds(TableLayout.ContainerInfo containerInfo, RectangleF displayRectF)
		{
			int cellBorderWidth = containerInfo.CellBorderWidth;
			float num = displayRectF.Y;
			int i = 0;
			int j = 0;
			bool flag = false;
			Rectangle.Truncate(displayRectF);
			if (containerInfo.Container is Control)
			{
				Control control = containerInfo.Container as Control;
				flag = (control.RightToLeft == RightToLeft.Yes);
			}
			TableLayout.LayoutInfo[] childrenInfo = containerInfo.ChildrenInfo;
			float num2 = flag ? displayRectF.Right : displayRectF.X;
			Array.Sort(childrenInfo, TableLayout.PostAssignedPositionComparer.GetInstance);
			foreach (TableLayout.LayoutInfo layoutInfo in childrenInfo)
			{
				IArrangedElement element = layoutInfo.Element;
				if (j != layoutInfo.RowStart)
				{
					while (j < layoutInfo.RowStart)
					{
						num += (float)containerInfo.Rows[j].MinSize;
						j++;
					}
					num2 = (flag ? displayRectF.Right : displayRectF.X);
					i = 0;
				}
				while (i < layoutInfo.ColumnStart)
				{
					if (flag)
					{
						num2 -= (float)containerInfo.Columns[i].MinSize;
					}
					else
					{
						num2 += (float)containerInfo.Columns[i].MinSize;
					}
					i++;
				}
				int num3 = i + layoutInfo.ColumnSpan;
				int num4 = 0;
				while (i < num3 && i < containerInfo.Columns.Length)
				{
					num4 += containerInfo.Columns[i].MinSize;
					i++;
				}
				if (flag)
				{
					num2 -= (float)num4;
				}
				int num5 = j + layoutInfo.RowSpan;
				int num6 = 0;
				int num7 = j;
				while (num7 < num5 && num7 < containerInfo.Rows.Length)
				{
					num6 += containerInfo.Rows[num7].MinSize;
					num7++;
				}
				Rectangle rectangle = new Rectangle((int)(num2 + (float)cellBorderWidth / 2f), (int)(num + (float)cellBorderWidth / 2f), num4 - cellBorderWidth, num6 - cellBorderWidth);
				Padding margin = CommonProperties.GetMargin(element);
				if (flag)
				{
					int right = margin.Right;
					margin.Right = margin.Left;
					margin.Left = right;
				}
				rectangle = LayoutUtils.DeflateRect(rectangle, margin);
				rectangle.Width = Math.Max(rectangle.Width, 1);
				rectangle.Height = Math.Max(rectangle.Height, 1);
				AnchorStyles unifiedAnchor = LayoutUtils.GetUnifiedAnchor(element);
				Rectangle bounds = LayoutUtils.AlignAndStretch(this.GetElementSize(element, rectangle.Size), rectangle, unifiedAnchor);
				bounds.Width = Math.Min(rectangle.Width, bounds.Width);
				bounds.Height = Math.Min(rectangle.Height, bounds.Height);
				if (flag)
				{
					bounds.X = rectangle.X + (rectangle.Right - bounds.Right);
				}
				element.SetBounds(bounds, BoundsSpecified.None);
				if (!flag)
				{
					num2 += (float)num4;
				}
			}
		}

		// Token: 0x060065B2 RID: 26034 RVA: 0x00175D58 File Offset: 0x00174D58
		internal IArrangedElement GetControlFromPosition(IArrangedElement container, int column, int row)
		{
			if (row < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"RowPosition",
					row.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (column < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"ColumnPosition",
					column.ToString(CultureInfo.CurrentCulture)
				}));
			}
			ArrangedElementCollection children = container.Children;
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			if (children == null || children.Count == 0)
			{
				return null;
			}
			if (!containerInfo.Valid)
			{
				this.EnsureRowAndColumnAssignments(container, containerInfo, true);
			}
			for (int i = 0; i < children.Count; i++)
			{
				TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(children[i]);
				if (layoutInfo.ColumnStart <= column && layoutInfo.ColumnStart + layoutInfo.ColumnSpan - 1 >= column && layoutInfo.RowStart <= row && layoutInfo.RowStart + layoutInfo.RowSpan - 1 >= row)
				{
					return layoutInfo.Element;
				}
			}
			return null;
		}

		// Token: 0x060065B3 RID: 26035 RVA: 0x00175E5C File Offset: 0x00174E5C
		internal TableLayoutPanelCellPosition GetPositionFromControl(IArrangedElement container, IArrangedElement child)
		{
			if (container == null || child == null)
			{
				return new TableLayoutPanelCellPosition(-1, -1);
			}
			ArrangedElementCollection children = container.Children;
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			if (children == null || children.Count == 0)
			{
				return new TableLayoutPanelCellPosition(-1, -1);
			}
			if (!containerInfo.Valid)
			{
				this.EnsureRowAndColumnAssignments(container, containerInfo, true);
			}
			TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(child);
			return new TableLayoutPanelCellPosition(layoutInfo.ColumnStart, layoutInfo.RowStart);
		}

		// Token: 0x060065B4 RID: 26036 RVA: 0x00175EC4 File Offset: 0x00174EC4
		internal static TableLayout.LayoutInfo GetLayoutInfo(IArrangedElement element)
		{
			TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)element.Properties.GetObject(TableLayout._layoutInfoProperty);
			if (layoutInfo == null)
			{
				layoutInfo = new TableLayout.LayoutInfo(element);
				TableLayout.SetLayoutInfo(element, layoutInfo);
			}
			return layoutInfo;
		}

		// Token: 0x060065B5 RID: 26037 RVA: 0x00175EF9 File Offset: 0x00174EF9
		internal static void SetLayoutInfo(IArrangedElement element, TableLayout.LayoutInfo value)
		{
			element.Properties.SetObject(TableLayout._layoutInfoProperty, value);
		}

		// Token: 0x060065B6 RID: 26038 RVA: 0x00175F0C File Offset: 0x00174F0C
		internal static bool HasCachedAssignments(TableLayout.ContainerInfo containerInfo)
		{
			return containerInfo.Valid;
		}

		// Token: 0x060065B7 RID: 26039 RVA: 0x00175F14 File Offset: 0x00174F14
		internal static void ClearCachedAssignments(TableLayout.ContainerInfo containerInfo)
		{
			containerInfo.Valid = false;
		}

		// Token: 0x060065B8 RID: 26040 RVA: 0x00175F20 File Offset: 0x00174F20
		internal static TableLayout.ContainerInfo GetContainerInfo(IArrangedElement container)
		{
			TableLayout.ContainerInfo containerInfo = (TableLayout.ContainerInfo)container.Properties.GetObject(TableLayout._containerInfoProperty);
			if (containerInfo == null)
			{
				containerInfo = new TableLayout.ContainerInfo(container);
				container.Properties.SetObject(TableLayout._containerInfoProperty, containerInfo);
			}
			return containerInfo;
		}

		// Token: 0x060065B9 RID: 26041 RVA: 0x00175F5F File Offset: 0x00174F5F
		[Conditional("DEBUG_LAYOUT")]
		private void Debug_VerifyAssignmentsAreCurrent(IArrangedElement container, TableLayout.ContainerInfo containerInfo)
		{
		}

		// Token: 0x060065BA RID: 26042 RVA: 0x00175F64 File Offset: 0x00174F64
		[Conditional("DEBUG_LAYOUT")]
		private void Debug_VerifyNoOverlapping(IArrangedElement container)
		{
			ArrayList arrayList = new ArrayList(container.Children.Count);
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(container);
			TableLayout.Strip[] rows = containerInfo.Rows;
			TableLayout.Strip[] columns = containerInfo.Columns;
			foreach (object obj in container.Children)
			{
				IArrangedElement arrangedElement = (IArrangedElement)obj;
				if (arrangedElement.ParticipatesInLayout)
				{
					arrayList.Add(TableLayout.GetLayoutInfo(arrangedElement));
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)arrayList[i];
				Rectangle bounds = layoutInfo.Element.Bounds;
				new Rectangle(layoutInfo.ColumnStart, layoutInfo.RowStart, layoutInfo.ColumnSpan, layoutInfo.RowSpan);
				for (int j = i + 1; j < arrayList.Count; j++)
				{
					TableLayout.LayoutInfo layoutInfo2 = (TableLayout.LayoutInfo)arrayList[j];
					Rectangle bounds2 = layoutInfo2.Element.Bounds;
					new Rectangle(layoutInfo2.ColumnStart, layoutInfo2.RowStart, layoutInfo2.ColumnSpan, layoutInfo2.RowSpan);
					if (LayoutUtils.IsIntersectHorizontally(bounds, bounds2))
					{
						for (int k = layoutInfo.ColumnStart; k < layoutInfo.ColumnStart + layoutInfo.ColumnSpan; k++)
						{
						}
						for (int k = layoutInfo2.ColumnStart; k < layoutInfo2.ColumnStart + layoutInfo2.ColumnSpan; k++)
						{
						}
					}
					if (LayoutUtils.IsIntersectVertically(bounds, bounds2))
					{
						for (int l = layoutInfo.RowStart; l < layoutInfo.RowStart + layoutInfo.RowSpan; l++)
						{
						}
						for (int l = layoutInfo2.RowStart; l < layoutInfo2.RowStart + layoutInfo2.RowSpan; l++)
						{
						}
					}
				}
			}
		}

		// Token: 0x04003C97 RID: 15511
		internal static readonly TableLayout Instance = new TableLayout();

		// Token: 0x04003C98 RID: 15512
		private static readonly int _containerInfoProperty = PropertyStore.CreateKey();

		// Token: 0x04003C99 RID: 15513
		private static readonly int _layoutInfoProperty = PropertyStore.CreateKey();

		// Token: 0x04003C9A RID: 15514
		private static string[] _propertiesWhichInvalidateCache = new string[]
		{
			null,
			PropertyNames.ChildIndex,
			PropertyNames.Parent,
			PropertyNames.Visible,
			PropertyNames.Items,
			PropertyNames.Rows,
			PropertyNames.Columns,
			PropertyNames.RowStyles,
			PropertyNames.ColumnStyles
		};

		// Token: 0x02000791 RID: 1937
		internal sealed class LayoutInfo
		{
			// Token: 0x060065BD RID: 26045 RVA: 0x001761D1 File Offset: 0x001751D1
			public LayoutInfo(IArrangedElement element)
			{
				this._element = element;
			}

			// Token: 0x17001547 RID: 5447
			// (get) Token: 0x060065BE RID: 26046 RVA: 0x0017620A File Offset: 0x0017520A
			internal bool IsAbsolutelyPositioned
			{
				get
				{
					return this._rowPos >= 0 && this._colPos >= 0;
				}
			}

			// Token: 0x17001548 RID: 5448
			// (get) Token: 0x060065BF RID: 26047 RVA: 0x00176223 File Offset: 0x00175223
			internal IArrangedElement Element
			{
				get
				{
					return this._element;
				}
			}

			// Token: 0x17001549 RID: 5449
			// (get) Token: 0x060065C0 RID: 26048 RVA: 0x0017622B File Offset: 0x0017522B
			// (set) Token: 0x060065C1 RID: 26049 RVA: 0x00176233 File Offset: 0x00175233
			internal int RowPosition
			{
				get
				{
					return this._rowPos;
				}
				set
				{
					this._rowPos = value;
				}
			}

			// Token: 0x1700154A RID: 5450
			// (get) Token: 0x060065C2 RID: 26050 RVA: 0x0017623C File Offset: 0x0017523C
			// (set) Token: 0x060065C3 RID: 26051 RVA: 0x00176244 File Offset: 0x00175244
			internal int ColumnPosition
			{
				get
				{
					return this._colPos;
				}
				set
				{
					this._colPos = value;
				}
			}

			// Token: 0x1700154B RID: 5451
			// (get) Token: 0x060065C4 RID: 26052 RVA: 0x0017624D File Offset: 0x0017524D
			// (set) Token: 0x060065C5 RID: 26053 RVA: 0x00176255 File Offset: 0x00175255
			internal int RowStart
			{
				get
				{
					return this._rowStart;
				}
				set
				{
					this._rowStart = value;
				}
			}

			// Token: 0x1700154C RID: 5452
			// (get) Token: 0x060065C6 RID: 26054 RVA: 0x0017625E File Offset: 0x0017525E
			// (set) Token: 0x060065C7 RID: 26055 RVA: 0x00176266 File Offset: 0x00175266
			internal int ColumnStart
			{
				get
				{
					return this._columnStart;
				}
				set
				{
					this._columnStart = value;
				}
			}

			// Token: 0x1700154D RID: 5453
			// (get) Token: 0x060065C8 RID: 26056 RVA: 0x0017626F File Offset: 0x0017526F
			// (set) Token: 0x060065C9 RID: 26057 RVA: 0x00176277 File Offset: 0x00175277
			internal int ColumnSpan
			{
				get
				{
					return this._columnSpan;
				}
				set
				{
					this._columnSpan = value;
				}
			}

			// Token: 0x1700154E RID: 5454
			// (get) Token: 0x060065CA RID: 26058 RVA: 0x00176280 File Offset: 0x00175280
			// (set) Token: 0x060065CB RID: 26059 RVA: 0x00176288 File Offset: 0x00175288
			internal int RowSpan
			{
				get
				{
					return this._rowSpan;
				}
				set
				{
					this._rowSpan = value;
				}
			}

			// Token: 0x04003C9B RID: 15515
			private int _rowStart = -1;

			// Token: 0x04003C9C RID: 15516
			private int _columnStart = -1;

			// Token: 0x04003C9D RID: 15517
			private int _columnSpan = 1;

			// Token: 0x04003C9E RID: 15518
			private int _rowSpan = 1;

			// Token: 0x04003C9F RID: 15519
			private int _rowPos = -1;

			// Token: 0x04003CA0 RID: 15520
			private int _colPos = -1;

			// Token: 0x04003CA1 RID: 15521
			private IArrangedElement _element;
		}

		// Token: 0x02000792 RID: 1938
		internal sealed class ContainerInfo
		{
			// Token: 0x060065CC RID: 26060 RVA: 0x00176291 File Offset: 0x00175291
			public ContainerInfo(IArrangedElement container)
			{
				this._container = container;
				this._growStyle = TableLayoutPanelGrowStyle.AddRows;
			}

			// Token: 0x060065CD RID: 26061 RVA: 0x001762CC File Offset: 0x001752CC
			public ContainerInfo(TableLayout.ContainerInfo containerInfo)
			{
				this._cellBorderWidth = containerInfo.CellBorderWidth;
				this._maxRows = containerInfo.MaxRows;
				this._maxColumns = containerInfo.MaxColumns;
				this._growStyle = containerInfo.GrowStyle;
				this._container = containerInfo.Container;
				this._rowStyles = containerInfo.RowStyles;
				this._colStyles = containerInfo.ColumnStyles;
			}

			// Token: 0x1700154F RID: 5455
			// (get) Token: 0x060065CE RID: 26062 RVA: 0x00176355 File Offset: 0x00175355
			public IArrangedElement Container
			{
				get
				{
					return this._container;
				}
			}

			// Token: 0x17001550 RID: 5456
			// (get) Token: 0x060065CF RID: 26063 RVA: 0x0017635D File Offset: 0x0017535D
			// (set) Token: 0x060065D0 RID: 26064 RVA: 0x00176365 File Offset: 0x00175365
			public int CellBorderWidth
			{
				get
				{
					return this._cellBorderWidth;
				}
				set
				{
					this._cellBorderWidth = value;
				}
			}

			// Token: 0x17001551 RID: 5457
			// (get) Token: 0x060065D1 RID: 26065 RVA: 0x0017636E File Offset: 0x0017536E
			// (set) Token: 0x060065D2 RID: 26066 RVA: 0x00176376 File Offset: 0x00175376
			public TableLayout.Strip[] Columns
			{
				get
				{
					return this._cols;
				}
				set
				{
					this._cols = value;
				}
			}

			// Token: 0x17001552 RID: 5458
			// (get) Token: 0x060065D3 RID: 26067 RVA: 0x0017637F File Offset: 0x0017537F
			// (set) Token: 0x060065D4 RID: 26068 RVA: 0x00176387 File Offset: 0x00175387
			public TableLayout.Strip[] Rows
			{
				get
				{
					return this._rows;
				}
				set
				{
					this._rows = value;
				}
			}

			// Token: 0x17001553 RID: 5459
			// (get) Token: 0x060065D5 RID: 26069 RVA: 0x00176390 File Offset: 0x00175390
			// (set) Token: 0x060065D6 RID: 26070 RVA: 0x00176398 File Offset: 0x00175398
			public int MaxRows
			{
				get
				{
					return this._maxRows;
				}
				set
				{
					if (this._maxRows != value)
					{
						this._maxRows = value;
						this.Valid = false;
					}
				}
			}

			// Token: 0x17001554 RID: 5460
			// (get) Token: 0x060065D7 RID: 26071 RVA: 0x001763B1 File Offset: 0x001753B1
			// (set) Token: 0x060065D8 RID: 26072 RVA: 0x001763B9 File Offset: 0x001753B9
			public int MaxColumns
			{
				get
				{
					return this._maxColumns;
				}
				set
				{
					if (this._maxColumns != value)
					{
						this._maxColumns = value;
						this.Valid = false;
					}
				}
			}

			// Token: 0x17001555 RID: 5461
			// (get) Token: 0x060065D9 RID: 26073 RVA: 0x001763D2 File Offset: 0x001753D2
			public int MinRowsAndColumns
			{
				get
				{
					return this._minRowsAndColumns;
				}
			}

			// Token: 0x17001556 RID: 5462
			// (get) Token: 0x060065DA RID: 26074 RVA: 0x001763DA File Offset: 0x001753DA
			public int MinColumns
			{
				get
				{
					return this._minColumns;
				}
			}

			// Token: 0x17001557 RID: 5463
			// (get) Token: 0x060065DB RID: 26075 RVA: 0x001763E2 File Offset: 0x001753E2
			public int MinRows
			{
				get
				{
					return this._minRows;
				}
			}

			// Token: 0x17001558 RID: 5464
			// (get) Token: 0x060065DC RID: 26076 RVA: 0x001763EA File Offset: 0x001753EA
			// (set) Token: 0x060065DD RID: 26077 RVA: 0x001763F2 File Offset: 0x001753F2
			public TableLayoutPanelGrowStyle GrowStyle
			{
				get
				{
					return this._growStyle;
				}
				set
				{
					if (this._growStyle != value)
					{
						this._growStyle = value;
						this.Valid = false;
					}
				}
			}

			// Token: 0x17001559 RID: 5465
			// (get) Token: 0x060065DE RID: 26078 RVA: 0x0017640B File Offset: 0x0017540B
			// (set) Token: 0x060065DF RID: 26079 RVA: 0x0017642C File Offset: 0x0017542C
			public TableLayoutRowStyleCollection RowStyles
			{
				get
				{
					if (this._rowStyles == null)
					{
						this._rowStyles = new TableLayoutRowStyleCollection(this._container);
					}
					return this._rowStyles;
				}
				set
				{
					this._rowStyles = value;
					if (this._rowStyles != null)
					{
						this._rowStyles.EnsureOwnership(this._container);
					}
				}
			}

			// Token: 0x1700155A RID: 5466
			// (get) Token: 0x060065E0 RID: 26080 RVA: 0x0017644E File Offset: 0x0017544E
			// (set) Token: 0x060065E1 RID: 26081 RVA: 0x0017646F File Offset: 0x0017546F
			public TableLayoutColumnStyleCollection ColumnStyles
			{
				get
				{
					if (this._colStyles == null)
					{
						this._colStyles = new TableLayoutColumnStyleCollection(this._container);
					}
					return this._colStyles;
				}
				set
				{
					this._colStyles = value;
					if (this._colStyles != null)
					{
						this._colStyles.EnsureOwnership(this._container);
					}
				}
			}

			// Token: 0x1700155B RID: 5467
			// (get) Token: 0x060065E2 RID: 26082 RVA: 0x00176494 File Offset: 0x00175494
			public TableLayout.LayoutInfo[] ChildrenInfo
			{
				get
				{
					if (!this._state[TableLayout.ContainerInfo.stateChildInfoValid])
					{
						this._countFixedChildren = 0;
						this._minRowsAndColumns = 0;
						this._minColumns = 0;
						this._minRows = 0;
						ArrangedElementCollection children = this.Container.Children;
						TableLayout.LayoutInfo[] array = new TableLayout.LayoutInfo[children.Count];
						int num = 0;
						int num2 = 0;
						for (int i = 0; i < children.Count; i++)
						{
							IArrangedElement arrangedElement = children[i];
							if (!arrangedElement.ParticipatesInLayout)
							{
								num++;
							}
							else
							{
								TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(arrangedElement);
								if (layoutInfo.IsAbsolutelyPositioned)
								{
									this._countFixedChildren++;
								}
								array[num2++] = layoutInfo;
								this._minRowsAndColumns += layoutInfo.RowSpan * layoutInfo.ColumnSpan;
								if (layoutInfo.IsAbsolutelyPositioned)
								{
									this._minColumns = Math.Max(this._minColumns, layoutInfo.ColumnPosition + layoutInfo.ColumnSpan);
									this._minRows = Math.Max(this._minRows, layoutInfo.RowPosition + layoutInfo.RowSpan);
								}
							}
						}
						if (num > 0)
						{
							TableLayout.LayoutInfo[] array2 = new TableLayout.LayoutInfo[array.Length - num];
							Array.Copy(array, array2, array2.Length);
							this._childInfo = array2;
						}
						else
						{
							this._childInfo = array;
						}
						this._state[TableLayout.ContainerInfo.stateChildInfoValid] = true;
					}
					if (this._childInfo != null)
					{
						return this._childInfo;
					}
					return new TableLayout.LayoutInfo[0];
				}
			}

			// Token: 0x1700155C RID: 5468
			// (get) Token: 0x060065E3 RID: 26083 RVA: 0x00176606 File Offset: 0x00175606
			public bool ChildInfoValid
			{
				get
				{
					return this._state[TableLayout.ContainerInfo.stateChildInfoValid];
				}
			}

			// Token: 0x1700155D RID: 5469
			// (get) Token: 0x060065E4 RID: 26084 RVA: 0x00176618 File Offset: 0x00175618
			public TableLayout.LayoutInfo[] FixedChildrenInfo
			{
				get
				{
					TableLayout.LayoutInfo[] array = new TableLayout.LayoutInfo[this._countFixedChildren];
					if (this.HasChildWithAbsolutePositioning)
					{
						int num = 0;
						for (int i = 0; i < this._childInfo.Length; i++)
						{
							if (this._childInfo[i].IsAbsolutelyPositioned)
							{
								array[num++] = this._childInfo[i];
							}
						}
						Array.Sort(array, TableLayout.PreAssignedPositionComparer.GetInstance);
					}
					return array;
				}
			}

			// Token: 0x1700155E RID: 5470
			// (get) Token: 0x060065E5 RID: 26085 RVA: 0x00176678 File Offset: 0x00175678
			// (set) Token: 0x060065E6 RID: 26086 RVA: 0x0017668A File Offset: 0x0017568A
			public bool Valid
			{
				get
				{
					return this._state[TableLayout.ContainerInfo.stateValid];
				}
				set
				{
					this._state[TableLayout.ContainerInfo.stateValid] = value;
					if (!this._state[TableLayout.ContainerInfo.stateValid])
					{
						this._state[TableLayout.ContainerInfo.stateChildInfoValid] = false;
					}
				}
			}

			// Token: 0x1700155F RID: 5471
			// (get) Token: 0x060065E7 RID: 26087 RVA: 0x001766C0 File Offset: 0x001756C0
			public bool HasChildWithAbsolutePositioning
			{
				get
				{
					return this._countFixedChildren > 0;
				}
			}

			// Token: 0x17001560 RID: 5472
			// (get) Token: 0x060065E8 RID: 26088 RVA: 0x001766CC File Offset: 0x001756CC
			public bool HasMultiplePercentColumns
			{
				get
				{
					if (this._colStyles != null)
					{
						bool flag = false;
						foreach (object obj in ((IEnumerable)this._colStyles))
						{
							ColumnStyle columnStyle = (ColumnStyle)obj;
							if (columnStyle.SizeType == SizeType.Percent)
							{
								if (flag)
								{
									return true;
								}
								flag = true;
							}
						}
						return false;
					}
					return false;
				}
			}

			// Token: 0x17001561 RID: 5473
			// (get) Token: 0x060065E9 RID: 26089 RVA: 0x00176740 File Offset: 0x00175740
			// (set) Token: 0x060065EA RID: 26090 RVA: 0x00176752 File Offset: 0x00175752
			public bool ChildHasColumnSpan
			{
				get
				{
					return this._state[TableLayout.ContainerInfo.stateChildHasColumnSpan];
				}
				set
				{
					this._state[TableLayout.ContainerInfo.stateChildHasColumnSpan] = value;
				}
			}

			// Token: 0x17001562 RID: 5474
			// (get) Token: 0x060065EB RID: 26091 RVA: 0x00176765 File Offset: 0x00175765
			// (set) Token: 0x060065EC RID: 26092 RVA: 0x00176777 File Offset: 0x00175777
			public bool ChildHasRowSpan
			{
				get
				{
					return this._state[TableLayout.ContainerInfo.stateChildHasRowSpan];
				}
				set
				{
					this._state[TableLayout.ContainerInfo.stateChildHasRowSpan] = value;
				}
			}

			// Token: 0x060065ED RID: 26093 RVA: 0x0017678C File Offset: 0x0017578C
			public Size GetCachedPreferredSize(Size proposedContstraints, out bool isValid)
			{
				isValid = false;
				if (proposedContstraints.Height == 0 || proposedContstraints.Width == 0)
				{
					Size result = CommonProperties.xGetPreferredSizeCache(this.Container);
					if (!result.IsEmpty)
					{
						isValid = true;
						return result;
					}
				}
				return Size.Empty;
			}

			// Token: 0x04003CA2 RID: 15522
			private static TableLayout.Strip[] emptyStrip = new TableLayout.Strip[0];

			// Token: 0x04003CA3 RID: 15523
			private static readonly int stateValid = BitVector32.CreateMask();

			// Token: 0x04003CA4 RID: 15524
			private static readonly int stateChildInfoValid = BitVector32.CreateMask(TableLayout.ContainerInfo.stateValid);

			// Token: 0x04003CA5 RID: 15525
			private static readonly int stateChildHasColumnSpan = BitVector32.CreateMask(TableLayout.ContainerInfo.stateChildInfoValid);

			// Token: 0x04003CA6 RID: 15526
			private static readonly int stateChildHasRowSpan = BitVector32.CreateMask(TableLayout.ContainerInfo.stateChildHasColumnSpan);

			// Token: 0x04003CA7 RID: 15527
			private int _cellBorderWidth;

			// Token: 0x04003CA8 RID: 15528
			private TableLayout.Strip[] _cols = TableLayout.ContainerInfo.emptyStrip;

			// Token: 0x04003CA9 RID: 15529
			private TableLayout.Strip[] _rows = TableLayout.ContainerInfo.emptyStrip;

			// Token: 0x04003CAA RID: 15530
			private int _maxRows;

			// Token: 0x04003CAB RID: 15531
			private int _maxColumns;

			// Token: 0x04003CAC RID: 15532
			private TableLayoutRowStyleCollection _rowStyles;

			// Token: 0x04003CAD RID: 15533
			private TableLayoutColumnStyleCollection _colStyles;

			// Token: 0x04003CAE RID: 15534
			private TableLayoutPanelGrowStyle _growStyle;

			// Token: 0x04003CAF RID: 15535
			private IArrangedElement _container;

			// Token: 0x04003CB0 RID: 15536
			private TableLayout.LayoutInfo[] _childInfo;

			// Token: 0x04003CB1 RID: 15537
			private int _countFixedChildren;

			// Token: 0x04003CB2 RID: 15538
			private int _minRowsAndColumns;

			// Token: 0x04003CB3 RID: 15539
			private int _minColumns;

			// Token: 0x04003CB4 RID: 15540
			private int _minRows;

			// Token: 0x04003CB5 RID: 15541
			private BitVector32 _state = default(BitVector32);
		}

		// Token: 0x02000793 RID: 1939
		private abstract class SizeProxy
		{
			// Token: 0x17001563 RID: 5475
			// (get) Token: 0x060065EF RID: 26095 RVA: 0x0017681F File Offset: 0x0017581F
			// (set) Token: 0x060065F0 RID: 26096 RVA: 0x00176827 File Offset: 0x00175827
			public TableLayout.Strip Strip
			{
				get
				{
					return this.strip;
				}
				set
				{
					this.strip = value;
				}
			}

			// Token: 0x17001564 RID: 5476
			// (get) Token: 0x060065F1 RID: 26097
			// (set) Token: 0x060065F2 RID: 26098
			public abstract int Size { get; set; }

			// Token: 0x04003CB6 RID: 15542
			protected TableLayout.Strip strip;
		}

		// Token: 0x02000794 RID: 1940
		private class MinSizeProxy : TableLayout.SizeProxy
		{
			// Token: 0x17001565 RID: 5477
			// (get) Token: 0x060065F4 RID: 26100 RVA: 0x00176838 File Offset: 0x00175838
			// (set) Token: 0x060065F5 RID: 26101 RVA: 0x00176845 File Offset: 0x00175845
			public override int Size
			{
				get
				{
					return this.strip.MinSize;
				}
				set
				{
					this.strip.MinSize = value;
				}
			}

			// Token: 0x17001566 RID: 5478
			// (get) Token: 0x060065F6 RID: 26102 RVA: 0x00176853 File Offset: 0x00175853
			public static TableLayout.MinSizeProxy GetInstance
			{
				get
				{
					return TableLayout.MinSizeProxy.instance;
				}
			}

			// Token: 0x04003CB7 RID: 15543
			private static readonly TableLayout.MinSizeProxy instance = new TableLayout.MinSizeProxy();
		}

		// Token: 0x02000795 RID: 1941
		private class MaxSizeProxy : TableLayout.SizeProxy
		{
			// Token: 0x17001567 RID: 5479
			// (get) Token: 0x060065F9 RID: 26105 RVA: 0x0017686E File Offset: 0x0017586E
			// (set) Token: 0x060065FA RID: 26106 RVA: 0x0017687B File Offset: 0x0017587B
			public override int Size
			{
				get
				{
					return this.strip.MaxSize;
				}
				set
				{
					this.strip.MaxSize = value;
				}
			}

			// Token: 0x17001568 RID: 5480
			// (get) Token: 0x060065FB RID: 26107 RVA: 0x00176889 File Offset: 0x00175889
			public static TableLayout.MaxSizeProxy GetInstance
			{
				get
				{
					return TableLayout.MaxSizeProxy.instance;
				}
			}

			// Token: 0x04003CB8 RID: 15544
			private static readonly TableLayout.MaxSizeProxy instance = new TableLayout.MaxSizeProxy();
		}

		// Token: 0x02000796 RID: 1942
		private abstract class SpanComparer : IComparer
		{
			// Token: 0x060065FE RID: 26110
			public abstract int GetSpan(TableLayout.LayoutInfo layoutInfo);

			// Token: 0x060065FF RID: 26111 RVA: 0x001768A4 File Offset: 0x001758A4
			public int Compare(object x, object y)
			{
				TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)x;
				TableLayout.LayoutInfo layoutInfo2 = (TableLayout.LayoutInfo)y;
				return this.GetSpan(layoutInfo) - this.GetSpan(layoutInfo2);
			}
		}

		// Token: 0x02000797 RID: 1943
		private class RowSpanComparer : TableLayout.SpanComparer
		{
			// Token: 0x06006601 RID: 26113 RVA: 0x001768D6 File Offset: 0x001758D6
			public override int GetSpan(TableLayout.LayoutInfo layoutInfo)
			{
				return layoutInfo.RowSpan;
			}

			// Token: 0x17001569 RID: 5481
			// (get) Token: 0x06006602 RID: 26114 RVA: 0x001768DE File Offset: 0x001758DE
			public static TableLayout.RowSpanComparer GetInstance
			{
				get
				{
					return TableLayout.RowSpanComparer.instance;
				}
			}

			// Token: 0x04003CB9 RID: 15545
			private static readonly TableLayout.RowSpanComparer instance = new TableLayout.RowSpanComparer();
		}

		// Token: 0x02000798 RID: 1944
		private class ColumnSpanComparer : TableLayout.SpanComparer
		{
			// Token: 0x06006605 RID: 26117 RVA: 0x001768F9 File Offset: 0x001758F9
			public override int GetSpan(TableLayout.LayoutInfo layoutInfo)
			{
				return layoutInfo.ColumnSpan;
			}

			// Token: 0x1700156A RID: 5482
			// (get) Token: 0x06006606 RID: 26118 RVA: 0x00176901 File Offset: 0x00175901
			public static TableLayout.ColumnSpanComparer GetInstance
			{
				get
				{
					return TableLayout.ColumnSpanComparer.instance;
				}
			}

			// Token: 0x04003CBA RID: 15546
			private static readonly TableLayout.ColumnSpanComparer instance = new TableLayout.ColumnSpanComparer();
		}

		// Token: 0x02000799 RID: 1945
		private class PostAssignedPositionComparer : IComparer
		{
			// Token: 0x1700156B RID: 5483
			// (get) Token: 0x06006609 RID: 26121 RVA: 0x0017691C File Offset: 0x0017591C
			public static TableLayout.PostAssignedPositionComparer GetInstance
			{
				get
				{
					return TableLayout.PostAssignedPositionComparer.instance;
				}
			}

			// Token: 0x0600660A RID: 26122 RVA: 0x00176924 File Offset: 0x00175924
			public int Compare(object x, object y)
			{
				TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)x;
				TableLayout.LayoutInfo layoutInfo2 = (TableLayout.LayoutInfo)y;
				if (layoutInfo.RowStart < layoutInfo2.RowStart)
				{
					return -1;
				}
				if (layoutInfo.RowStart > layoutInfo2.RowStart)
				{
					return 1;
				}
				if (layoutInfo.ColumnStart < layoutInfo2.ColumnStart)
				{
					return -1;
				}
				if (layoutInfo.ColumnStart > layoutInfo2.ColumnStart)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04003CBB RID: 15547
			private static readonly TableLayout.PostAssignedPositionComparer instance = new TableLayout.PostAssignedPositionComparer();
		}

		// Token: 0x0200079A RID: 1946
		private class PreAssignedPositionComparer : IComparer
		{
			// Token: 0x1700156C RID: 5484
			// (get) Token: 0x0600660D RID: 26125 RVA: 0x00176994 File Offset: 0x00175994
			public static TableLayout.PreAssignedPositionComparer GetInstance
			{
				get
				{
					return TableLayout.PreAssignedPositionComparer.instance;
				}
			}

			// Token: 0x0600660E RID: 26126 RVA: 0x0017699C File Offset: 0x0017599C
			public int Compare(object x, object y)
			{
				TableLayout.LayoutInfo layoutInfo = (TableLayout.LayoutInfo)x;
				TableLayout.LayoutInfo layoutInfo2 = (TableLayout.LayoutInfo)y;
				if (layoutInfo.RowPosition < layoutInfo2.RowPosition)
				{
					return -1;
				}
				if (layoutInfo.RowPosition > layoutInfo2.RowPosition)
				{
					return 1;
				}
				if (layoutInfo.ColumnPosition < layoutInfo2.ColumnPosition)
				{
					return -1;
				}
				if (layoutInfo.ColumnPosition > layoutInfo2.ColumnPosition)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04003CBC RID: 15548
			private static readonly TableLayout.PreAssignedPositionComparer instance = new TableLayout.PreAssignedPositionComparer();
		}

		// Token: 0x0200079B RID: 1947
		private sealed class ReservationGrid
		{
			// Token: 0x06006611 RID: 26129 RVA: 0x00176A0C File Offset: 0x00175A0C
			public bool IsReserved(int column, int rowOffset)
			{
				return rowOffset < this._rows.Count && column < ((BitArray)this._rows[rowOffset]).Length && ((BitArray)this._rows[rowOffset])[column];
			}

			// Token: 0x06006612 RID: 26130 RVA: 0x00176A5C File Offset: 0x00175A5C
			public void Reserve(int column, int rowOffset)
			{
				while (rowOffset >= this._rows.Count)
				{
					this._rows.Add(new BitArray(this._numColumns));
				}
				if (column >= ((BitArray)this._rows[rowOffset]).Length)
				{
					((BitArray)this._rows[rowOffset]).Length = column + 1;
					if (column >= this._numColumns)
					{
						this._numColumns = column + 1;
					}
				}
				((BitArray)this._rows[rowOffset])[column] = true;
			}

			// Token: 0x06006613 RID: 26131 RVA: 0x00176AEC File Offset: 0x00175AEC
			public void ReserveAll(TableLayout.LayoutInfo layoutInfo, int rowStop, int colStop)
			{
				for (int i = 1; i < rowStop - layoutInfo.RowStart; i++)
				{
					for (int j = layoutInfo.ColumnStart; j < colStop; j++)
					{
						this.Reserve(j, i);
					}
				}
			}

			// Token: 0x06006614 RID: 26132 RVA: 0x00176B25 File Offset: 0x00175B25
			public void AdvanceRow()
			{
				if (this._rows.Count > 0)
				{
					this._rows.RemoveAt(0);
				}
			}

			// Token: 0x04003CBD RID: 15549
			private int _numColumns = 1;

			// Token: 0x04003CBE RID: 15550
			private ArrayList _rows = new ArrayList();
		}

		// Token: 0x0200079C RID: 1948
		internal struct Strip
		{
			// Token: 0x1700156D RID: 5485
			// (get) Token: 0x06006616 RID: 26134 RVA: 0x00176B5B File Offset: 0x00175B5B
			// (set) Token: 0x06006617 RID: 26135 RVA: 0x00176B63 File Offset: 0x00175B63
			public int MinSize
			{
				get
				{
					return this._minSize;
				}
				set
				{
					this._minSize = value;
				}
			}

			// Token: 0x1700156E RID: 5486
			// (get) Token: 0x06006618 RID: 26136 RVA: 0x00176B6C File Offset: 0x00175B6C
			// (set) Token: 0x06006619 RID: 26137 RVA: 0x00176B74 File Offset: 0x00175B74
			public int MaxSize
			{
				get
				{
					return this._maxSize;
				}
				set
				{
					this._maxSize = value;
				}
			}

			// Token: 0x1700156F RID: 5487
			// (get) Token: 0x0600661A RID: 26138 RVA: 0x00176B7D File Offset: 0x00175B7D
			// (set) Token: 0x0600661B RID: 26139 RVA: 0x00176B85 File Offset: 0x00175B85
			public bool IsStart
			{
				get
				{
					return this._isStart;
				}
				set
				{
					this._isStart = value;
				}
			}

			// Token: 0x04003CBF RID: 15551
			private int _maxSize;

			// Token: 0x04003CC0 RID: 15552
			private int _minSize;

			// Token: 0x04003CC1 RID: 15553
			private bool _isStart;
		}
	}
}
