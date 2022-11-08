using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006E5 RID: 1765
	internal class ToolStripSplitStackLayout : LayoutEngine
	{
		// Token: 0x06005D31 RID: 23857 RVA: 0x001523E5 File Offset: 0x001513E5
		internal ToolStripSplitStackLayout(ToolStrip owner)
		{
			this.toolStrip = owner;
		}

		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06005D32 RID: 23858 RVA: 0x001523FF File Offset: 0x001513FF
		// (set) Token: 0x06005D33 RID: 23859 RVA: 0x00152407 File Offset: 0x00151407
		protected int BackwardsWalkingIndex
		{
			get
			{
				return this.backwardsWalkingIndex;
			}
			set
			{
				this.backwardsWalkingIndex = value;
			}
		}

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06005D34 RID: 23860 RVA: 0x00152410 File Offset: 0x00151410
		// (set) Token: 0x06005D35 RID: 23861 RVA: 0x00152418 File Offset: 0x00151418
		protected int ForwardsWalkingIndex
		{
			get
			{
				return this.forwardsWalkingIndex;
			}
			set
			{
				this.forwardsWalkingIndex = value;
			}
		}

		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x06005D36 RID: 23862 RVA: 0x00152424 File Offset: 0x00151424
		private Size OverflowButtonSize
		{
			get
			{
				ToolStrip toolStrip = this.ToolStrip;
				if (!toolStrip.CanOverflow)
				{
					return Size.Empty;
				}
				Size sz = toolStrip.OverflowButton.AutoSize ? toolStrip.OverflowButton.GetPreferredSize(this.displayRectangle.Size) : toolStrip.OverflowButton.Size;
				return sz + toolStrip.OverflowButton.Margin.Size;
			}
		}

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06005D37 RID: 23863 RVA: 0x00152490 File Offset: 0x00151490
		// (set) Token: 0x06005D38 RID: 23864 RVA: 0x00152498 File Offset: 0x00151498
		private int OverflowSpace
		{
			get
			{
				return this.overflowSpace;
			}
			set
			{
				this.overflowSpace = value;
			}
		}

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06005D39 RID: 23865 RVA: 0x001524A1 File Offset: 0x001514A1
		// (set) Token: 0x06005D3A RID: 23866 RVA: 0x001524A9 File Offset: 0x001514A9
		private bool OverflowRequired
		{
			get
			{
				return this.overflowRequired;
			}
			set
			{
				this.overflowRequired = value;
			}
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06005D3B RID: 23867 RVA: 0x001524B2 File Offset: 0x001514B2
		public ToolStrip ToolStrip
		{
			get
			{
				return this.toolStrip;
			}
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x001524BC File Offset: 0x001514BC
		private void CalculatePlacementsHorizontal()
		{
			this.ResetItemPlacements();
			ToolStrip toolStrip = this.ToolStrip;
			int num = 0;
			if (this.ToolStrip.CanOverflow)
			{
				this.ForwardsWalkingIndex = 0;
				while (this.ForwardsWalkingIndex < toolStrip.Items.Count)
				{
					ToolStripItem toolStripItem = toolStrip.Items[this.ForwardsWalkingIndex];
					if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
					{
						if (toolStripItem.Overflow == ToolStripItemOverflow.Always)
						{
							this.OverflowRequired = true;
						}
						if (toolStripItem.Overflow != ToolStripItemOverflow.Always && toolStripItem.Placement == ToolStripItemPlacement.None)
						{
							num += (toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(this.displayRectangle.Size) : toolStripItem.Size).Width + toolStripItem.Margin.Horizontal;
							int num2 = this.OverflowRequired ? this.OverflowButtonSize.Width : 0;
							if (num > this.displayRectangle.Width - num2)
							{
								int num3 = this.SendNextItemToOverflow(num + num2 - this.displayRectangle.Width, true);
								num -= num3;
							}
						}
					}
					this.ForwardsWalkingIndex++;
				}
			}
			this.PlaceItems();
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x001525EC File Offset: 0x001515EC
		private void CalculatePlacementsVertical()
		{
			this.ResetItemPlacements();
			ToolStrip toolStrip = this.ToolStrip;
			int num = 0;
			if (this.ToolStrip.CanOverflow)
			{
				this.ForwardsWalkingIndex = 0;
				while (this.ForwardsWalkingIndex < this.ToolStrip.Items.Count)
				{
					ToolStripItem toolStripItem = toolStrip.Items[this.ForwardsWalkingIndex];
					if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
					{
						if (toolStripItem.Overflow == ToolStripItemOverflow.Always)
						{
							this.OverflowRequired = true;
						}
						if (toolStripItem.Overflow != ToolStripItemOverflow.Always && toolStripItem.Placement == ToolStripItemPlacement.None)
						{
							Size size = toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(this.displayRectangle.Size) : toolStripItem.Size;
							int num2 = this.OverflowRequired ? this.OverflowButtonSize.Height : 0;
							num += size.Height + toolStripItem.Margin.Vertical;
							if (num > this.displayRectangle.Height - num2)
							{
								int num3 = this.SendNextItemToOverflow(num - this.displayRectangle.Height, false);
								num -= num3;
							}
						}
					}
					this.ForwardsWalkingIndex++;
				}
			}
			this.PlaceItems();
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x0015271C File Offset: 0x0015171C
		internal override Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
		{
			if (!(container is ToolStrip))
			{
				throw new NotSupportedException(SR.GetString("ToolStripSplitStackLayoutContainerMustBeAToolStrip"));
			}
			if (this.toolStrip.LayoutStyle == ToolStripLayoutStyle.HorizontalStackWithOverflow)
			{
				return ToolStrip.GetPreferredSizeHorizontal(container, proposedConstraints);
			}
			return ToolStrip.GetPreferredSizeVertical(container, proposedConstraints);
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x00152753 File Offset: 0x00151753
		private void InvalidateLayout()
		{
			this.forwardsWalkingIndex = 0;
			this.backwardsWalkingIndex = -1;
			this.overflowSpace = 0;
			this.overflowRequired = false;
			this.displayRectangle = Rectangle.Empty;
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x0015277C File Offset: 0x0015177C
		internal override bool LayoutCore(IArrangedElement container, LayoutEventArgs layoutEventArgs)
		{
			if (!(container is ToolStrip))
			{
				throw new NotSupportedException(SR.GetString("ToolStripSplitStackLayoutContainerMustBeAToolStrip"));
			}
			this.InvalidateLayout();
			this.displayRectangle = this.toolStrip.DisplayRectangle;
			this.noMansLand = this.displayRectangle.Location;
			this.noMansLand.X = this.noMansLand.X + (this.toolStrip.ClientSize.Width + 1);
			this.noMansLand.Y = this.noMansLand.Y + (this.toolStrip.ClientSize.Height + 1);
			if (this.toolStrip.LayoutStyle == ToolStripLayoutStyle.HorizontalStackWithOverflow)
			{
				this.LayoutHorizontal();
			}
			else
			{
				this.LayoutVertical();
			}
			return CommonProperties.GetAutoSize(container);
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x0015283C File Offset: 0x0015183C
		private bool LayoutHorizontal()
		{
			ToolStrip toolStrip = this.ToolStrip;
			Rectangle clientRectangle = toolStrip.ClientRectangle;
			int num = this.displayRectangle.Right;
			int num2 = this.displayRectangle.Left;
			bool result = false;
			Size itemSize = Size.Empty;
			Rectangle rectangle = Rectangle.Empty;
			Rectangle rectangle2 = Rectangle.Empty;
			this.CalculatePlacementsHorizontal();
			bool flag = toolStrip.CanOverflow && (this.OverflowRequired || this.OverflowSpace >= this.OverflowButtonSize.Width);
			toolStrip.OverflowButton.Visible = flag;
			if (flag)
			{
				if (toolStrip.RightToLeft == RightToLeft.No)
				{
					num = clientRectangle.Right;
				}
				else
				{
					num2 = clientRectangle.Left;
				}
			}
			int i = -1;
			while (i < toolStrip.Items.Count)
			{
				ToolStripItem toolStripItem;
				if (i == -1)
				{
					if (flag)
					{
						toolStripItem = toolStrip.OverflowButton;
						toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
						itemSize = this.OverflowButtonSize;
						goto IL_11F;
					}
					toolStripItem = toolStrip.OverflowButton;
					toolStripItem.SetPlacement(ToolStripItemPlacement.None);
				}
				else
				{
					toolStripItem = toolStrip.Items[i];
					if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
					{
						itemSize = (toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(Size.Empty) : toolStripItem.Size);
						goto IL_11F;
					}
				}
				IL_356:
				i++;
				continue;
				IL_11F:
				if (!flag && toolStripItem.Overflow == ToolStripItemOverflow.AsNeeded && toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
				}
				if (toolStripItem != null && toolStripItem.Placement == ToolStripItemPlacement.Main)
				{
					int num3 = this.displayRectangle.Left;
					int num4 = this.displayRectangle.Top;
					Padding margin = toolStripItem.Margin;
					if ((toolStripItem.Alignment == ToolStripItemAlignment.Right && toolStrip.RightToLeft == RightToLeft.No) || (toolStripItem.Alignment == ToolStripItemAlignment.Left && toolStrip.RightToLeft == RightToLeft.Yes))
					{
						num3 = num - (margin.Right + itemSize.Width);
						num4 += margin.Top;
						num = num3 - margin.Left;
						rectangle2 = ((rectangle2 == Rectangle.Empty) ? new Rectangle(num3, num4, itemSize.Width, itemSize.Height) : Rectangle.Union(rectangle2, new Rectangle(num3, num4, itemSize.Width, itemSize.Height)));
					}
					else
					{
						num3 = num2 + margin.Left;
						num4 += margin.Top;
						num2 = num3 + itemSize.Width + margin.Right;
						rectangle = ((rectangle == Rectangle.Empty) ? new Rectangle(num3, num4, itemSize.Width, itemSize.Height) : Rectangle.Union(rectangle, new Rectangle(num3, num4, itemSize.Width, itemSize.Height)));
					}
					toolStripItem.ParentInternal = this.ToolStrip;
					Point itemLocation = new Point(num3, num4);
					if (!clientRectangle.Contains(num3, num4))
					{
						toolStripItem.SetPlacement(ToolStripItemPlacement.None);
					}
					else if (rectangle2.Width > 0 && rectangle.Width > 0 && rectangle2.IntersectsWith(rectangle))
					{
						itemLocation = this.noMansLand;
						toolStripItem.SetPlacement(ToolStripItemPlacement.None);
					}
					if (toolStripItem.AutoSize)
					{
						itemSize.Height = Math.Max(this.displayRectangle.Height - margin.Vertical, 0);
					}
					else
					{
						itemLocation.Y = LayoutUtils.VAlign(toolStripItem.Size, this.displayRectangle, AnchorStyles.None).Y;
					}
					this.SetItemLocation(toolStripItem, itemLocation, itemSize);
					goto IL_356;
				}
				toolStripItem.ParentInternal = ((toolStripItem.Placement == ToolStripItemPlacement.Overflow) ? toolStrip.OverflowButton.DropDown : null);
				goto IL_356;
			}
			return result;
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x00152BBC File Offset: 0x00151BBC
		private bool LayoutVertical()
		{
			ToolStrip toolStrip = this.ToolStrip;
			Rectangle clientRectangle = toolStrip.ClientRectangle;
			int num = this.displayRectangle.Bottom;
			int num2 = this.displayRectangle.Top;
			bool result = false;
			Size itemSize = Size.Empty;
			Rectangle rectangle = Rectangle.Empty;
			Rectangle rectangle2 = Rectangle.Empty;
			Size size = this.displayRectangle.Size;
			DockStyle dock = toolStrip.Dock;
			if (toolStrip.AutoSize && ((!toolStrip.IsInToolStripPanel && dock == DockStyle.Left) || dock == DockStyle.Right))
			{
				size = ToolStrip.GetPreferredSizeVertical(toolStrip, Size.Empty) - toolStrip.Padding.Size;
			}
			this.CalculatePlacementsVertical();
			bool flag = toolStrip.CanOverflow && (this.OverflowRequired || this.OverflowSpace >= this.OverflowButtonSize.Height);
			toolStrip.OverflowButton.Visible = flag;
			int i = -1;
			while (i < this.ToolStrip.Items.Count)
			{
				ToolStripItem toolStripItem;
				if (i == -1)
				{
					if (flag)
					{
						toolStripItem = toolStrip.OverflowButton;
						toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
						itemSize = this.OverflowButtonSize;
						goto IL_153;
					}
					toolStripItem = toolStrip.OverflowButton;
					toolStripItem.SetPlacement(ToolStripItemPlacement.None);
				}
				else
				{
					toolStripItem = toolStrip.Items[i];
					if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
					{
						itemSize = (toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(Size.Empty) : toolStripItem.Size);
						goto IL_153;
					}
				}
				IL_36E:
				i++;
				continue;
				IL_153:
				if (!flag && toolStripItem.Overflow == ToolStripItemOverflow.AsNeeded && toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
				}
				if (toolStripItem != null && toolStripItem.Placement == ToolStripItemPlacement.Main)
				{
					Padding margin = toolStripItem.Margin;
					int x = this.displayRectangle.Left + margin.Left;
					int num3 = this.displayRectangle.Top;
					switch (toolStripItem.Alignment)
					{
					case ToolStripItemAlignment.Left:
						goto IL_232;
					case ToolStripItemAlignment.Right:
						num3 = num - (margin.Bottom + itemSize.Height);
						num = num3 - margin.Top;
						rectangle2 = ((rectangle2 == Rectangle.Empty) ? new Rectangle(x, num3, itemSize.Width, itemSize.Height) : Rectangle.Union(rectangle2, new Rectangle(x, num3, itemSize.Width, itemSize.Height)));
						break;
					default:
						goto IL_232;
					}
					IL_297:
					toolStripItem.ParentInternal = this.ToolStrip;
					Point itemLocation = new Point(x, num3);
					if (!clientRectangle.Contains(x, num3))
					{
						toolStripItem.SetPlacement(ToolStripItemPlacement.None);
					}
					else if (rectangle2.Width > 0 && rectangle.Width > 0 && rectangle2.IntersectsWith(rectangle))
					{
						itemLocation = this.noMansLand;
						toolStripItem.SetPlacement(ToolStripItemPlacement.None);
					}
					if (toolStripItem.AutoSize)
					{
						itemSize.Width = Math.Max(size.Width - margin.Horizontal - 1, 0);
					}
					else
					{
						itemLocation.X = LayoutUtils.HAlign(toolStripItem.Size, this.displayRectangle, AnchorStyles.None).X;
					}
					this.SetItemLocation(toolStripItem, itemLocation, itemSize);
					goto IL_36E;
					IL_232:
					num3 = num2 + margin.Top;
					num2 = num3 + itemSize.Height + margin.Bottom;
					rectangle = ((rectangle == Rectangle.Empty) ? new Rectangle(x, num3, itemSize.Width, itemSize.Height) : Rectangle.Union(rectangle, new Rectangle(x, num3, itemSize.Width, itemSize.Height)));
					goto IL_297;
				}
				toolStripItem.ParentInternal = ((toolStripItem.Placement == ToolStripItemPlacement.Overflow) ? toolStrip.OverflowButton.DropDown : null);
				goto IL_36E;
			}
			return result;
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x00152F58 File Offset: 0x00151F58
		private void SetItemLocation(ToolStripItem item, Point itemLocation, Size itemSize)
		{
			if (item.Placement == ToolStripItemPlacement.Main && !(item is ToolStripOverflowButton))
			{
				bool flag = this.ToolStrip.LayoutStyle == ToolStripLayoutStyle.HorizontalStackWithOverflow;
				Rectangle rectangle = new Rectangle(itemLocation, itemSize);
				if (flag)
				{
					if (rectangle.Right > this.displayRectangle.Right || rectangle.Left < this.displayRectangle.Left)
					{
						itemLocation = this.noMansLand;
						item.SetPlacement(ToolStripItemPlacement.None);
					}
				}
				else if (rectangle.Bottom > this.displayRectangle.Bottom || rectangle.Top < this.displayRectangle.Top)
				{
					itemLocation = this.noMansLand;
					item.SetPlacement(ToolStripItemPlacement.None);
				}
			}
			item.SetBounds(new Rectangle(itemLocation, itemSize));
		}

		// Token: 0x06005D44 RID: 23876 RVA: 0x00153014 File Offset: 0x00152014
		private void PlaceItems()
		{
			ToolStrip toolStrip = this.ToolStrip;
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				ToolStripItem toolStripItem = toolStrip.Items[i];
				if (toolStripItem.Placement == ToolStripItemPlacement.None)
				{
					if (toolStripItem.Overflow != ToolStripItemOverflow.Always)
					{
						toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
					}
					else
					{
						toolStripItem.SetPlacement(ToolStripItemPlacement.Overflow);
					}
				}
			}
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x00153070 File Offset: 0x00152070
		private void ResetItemPlacements()
		{
			ToolStrip toolStrip = this.ToolStrip;
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				if (toolStrip.Items[i].Placement == ToolStripItemPlacement.Overflow)
				{
					toolStrip.Items[i].ParentInternal = null;
				}
				toolStrip.Items[i].SetPlacement(ToolStripItemPlacement.None);
			}
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x001530D4 File Offset: 0x001520D4
		private int SendNextItemToOverflow(int spaceNeeded, bool horizontal)
		{
			int num = 0;
			int num2 = this.BackwardsWalkingIndex;
			this.BackwardsWalkingIndex = ((num2 == -1) ? (this.ToolStrip.Items.Count - 1) : (num2 - 1));
			while (this.BackwardsWalkingIndex >= 0)
			{
				ToolStripItem toolStripItem = this.ToolStrip.Items[this.BackwardsWalkingIndex];
				if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
				{
					Padding margin = toolStripItem.Margin;
					if (toolStripItem.Overflow == ToolStripItemOverflow.AsNeeded && toolStripItem.Placement != ToolStripItemPlacement.Overflow)
					{
						Size size = toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(this.displayRectangle.Size) : toolStripItem.Size;
						if (this.BackwardsWalkingIndex <= this.ForwardsWalkingIndex)
						{
							num += (horizontal ? (size.Width + margin.Horizontal) : (size.Height + margin.Vertical));
						}
						toolStripItem.SetPlacement(ToolStripItemPlacement.Overflow);
						if (!this.OverflowRequired)
						{
							spaceNeeded += (horizontal ? this.OverflowButtonSize.Width : this.OverflowButtonSize.Height);
							this.OverflowRequired = true;
						}
						this.OverflowSpace += (horizontal ? (size.Width + margin.Horizontal) : (size.Height + margin.Vertical));
					}
					if (num > spaceNeeded)
					{
						break;
					}
				}
				this.BackwardsWalkingIndex--;
			}
			return num;
		}

		// Token: 0x04003940 RID: 14656
		private int backwardsWalkingIndex;

		// Token: 0x04003941 RID: 14657
		private int forwardsWalkingIndex;

		// Token: 0x04003942 RID: 14658
		private ToolStrip toolStrip;

		// Token: 0x04003943 RID: 14659
		private int overflowSpace;

		// Token: 0x04003944 RID: 14660
		private bool overflowRequired;

		// Token: 0x04003945 RID: 14661
		private Point noMansLand;

		// Token: 0x04003946 RID: 14662
		private Rectangle displayRectangle = Rectangle.Empty;

		// Token: 0x04003947 RID: 14663
		internal static readonly TraceSwitch DebugLayoutTraceSwitch;
	}
}
