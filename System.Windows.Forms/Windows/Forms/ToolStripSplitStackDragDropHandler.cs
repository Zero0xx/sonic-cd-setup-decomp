using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000671 RID: 1649
	internal sealed class ToolStripSplitStackDragDropHandler : IDropTarget, ISupportOleDropSource
	{
		// Token: 0x06005697 RID: 22167 RVA: 0x0013B15A File Offset: 0x0013A15A
		public ToolStripSplitStackDragDropHandler(ToolStrip owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.owner = owner;
		}

		// Token: 0x06005698 RID: 22168 RVA: 0x0013B178 File Offset: 0x0013A178
		public void OnDragEnter(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				e.Effect = DragDropEffects.Move;
				this.ShowItemDropPoint(this.owner.PointToClient(new Point(e.X, e.Y)));
			}
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x0013B1C6 File Offset: 0x0013A1C6
		public void OnDragLeave(EventArgs e)
		{
			this.owner.ClearInsertionMark();
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x0013B1D4 File Offset: 0x0013A1D4
		public void OnDragDrop(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				ToolStripItem droppedItem = (ToolStripItem)e.Data.GetData(typeof(ToolStripItem));
				this.OnDropItem(droppedItem, this.owner.PointToClient(new Point(e.X, e.Y)));
			}
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x0013B238 File Offset: 0x0013A238
		public void OnDragOver(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				if (this.ShowItemDropPoint(this.owner.PointToClient(new Point(e.X, e.Y))))
				{
					e.Effect = DragDropEffects.Move;
					return;
				}
				if (this.owner != null)
				{
					this.owner.ClearInsertionMark();
				}
				e.Effect = DragDropEffects.None;
			}
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x0013B2A2 File Offset: 0x0013A2A2
		public void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x0013B2A4 File Offset: 0x0013A2A4
		public void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
		}

		// Token: 0x0600569E RID: 22174 RVA: 0x0013B2A8 File Offset: 0x0013A2A8
		private void OnDropItem(ToolStripItem droppedItem, Point ownerClientAreaRelativeDropPoint)
		{
			Point empty = Point.Empty;
			int itemInsertionIndex = this.GetItemInsertionIndex(ownerClientAreaRelativeDropPoint);
			if (itemInsertionIndex < 0)
			{
				if (itemInsertionIndex == -1 && this.owner.Items.Count == 0)
				{
					this.owner.Items.Add(droppedItem);
					this.owner.ClearInsertionMark();
				}
				return;
			}
			ToolStripItem toolStripItem = this.owner.Items[itemInsertionIndex];
			if (toolStripItem == droppedItem)
			{
				this.owner.ClearInsertionMark();
				return;
			}
			ToolStripSplitStackDragDropHandler.RelativeLocation relativeLocation = this.ComparePositions(toolStripItem.Bounds, ownerClientAreaRelativeDropPoint);
			droppedItem.Alignment = toolStripItem.Alignment;
			int num = Math.Max(0, itemInsertionIndex);
			if (relativeLocation == ToolStripSplitStackDragDropHandler.RelativeLocation.Above)
			{
				num = ((toolStripItem.Alignment == ToolStripItemAlignment.Left) ? num : (num + 1));
			}
			else if (relativeLocation == ToolStripSplitStackDragDropHandler.RelativeLocation.Below)
			{
				num = ((toolStripItem.Alignment == ToolStripItemAlignment.Left) ? num : (num - 1));
			}
			else if ((toolStripItem.Alignment == ToolStripItemAlignment.Left && relativeLocation == ToolStripSplitStackDragDropHandler.RelativeLocation.Left) || (toolStripItem.Alignment == ToolStripItemAlignment.Right && relativeLocation == ToolStripSplitStackDragDropHandler.RelativeLocation.Right))
			{
				num = Math.Max(0, (this.owner.RightToLeft == RightToLeft.Yes) ? (num + 1) : num);
			}
			else
			{
				num = Math.Max(0, (this.owner.RightToLeft == RightToLeft.No) ? (num + 1) : num);
			}
			if (this.owner.Items.IndexOf(droppedItem) < num)
			{
				num--;
			}
			this.owner.Items.MoveItem(Math.Max(0, num), droppedItem);
			this.owner.ClearInsertionMark();
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x0013B3F8 File Offset: 0x0013A3F8
		private bool ShowItemDropPoint(Point ownerClientAreaRelativeDropPoint)
		{
			int itemInsertionIndex = this.GetItemInsertionIndex(ownerClientAreaRelativeDropPoint);
			if (itemInsertionIndex >= 0)
			{
				ToolStripItem toolStripItem = this.owner.Items[itemInsertionIndex];
				ToolStripSplitStackDragDropHandler.RelativeLocation relativeLocation = this.ComparePositions(toolStripItem.Bounds, ownerClientAreaRelativeDropPoint);
				Rectangle empty = Rectangle.Empty;
				switch (relativeLocation)
				{
				case ToolStripSplitStackDragDropHandler.RelativeLocation.Above:
					empty = new Rectangle(this.owner.Margin.Left, toolStripItem.Bounds.Top, this.owner.Width - this.owner.Margin.Horizontal - 1, 6);
					break;
				case ToolStripSplitStackDragDropHandler.RelativeLocation.Below:
					empty = new Rectangle(this.owner.Margin.Left, toolStripItem.Bounds.Bottom, this.owner.Width - this.owner.Margin.Horizontal - 1, 6);
					break;
				case ToolStripSplitStackDragDropHandler.RelativeLocation.Right:
					empty = new Rectangle(toolStripItem.Bounds.Right, this.owner.Margin.Top, 6, this.owner.Height - this.owner.Margin.Vertical - 1);
					break;
				case ToolStripSplitStackDragDropHandler.RelativeLocation.Left:
					empty = new Rectangle(toolStripItem.Bounds.Left, this.owner.Margin.Top, 6, this.owner.Height - this.owner.Margin.Vertical - 1);
					break;
				}
				this.owner.PaintInsertionMark(empty);
				return true;
			}
			if (this.owner.Items.Count == 0)
			{
				Rectangle displayRectangle = this.owner.DisplayRectangle;
				displayRectangle.Width = 6;
				this.owner.PaintInsertionMark(displayRectangle);
				return true;
			}
			return false;
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x0013B5E0 File Offset: 0x0013A5E0
		private int GetItemInsertionIndex(Point ownerClientAreaRelativeDropPoint)
		{
			for (int i = 0; i < this.owner.DisplayedItems.Count; i++)
			{
				Rectangle bounds = this.owner.DisplayedItems[i].Bounds;
				bounds.Inflate(this.owner.DisplayedItems[i].Margin.Size);
				if (bounds.Contains(ownerClientAreaRelativeDropPoint))
				{
					return this.owner.Items.IndexOf(this.owner.DisplayedItems[i]);
				}
			}
			if (this.owner.DisplayedItems.Count > 0)
			{
				int j = 0;
				while (j < this.owner.DisplayedItems.Count)
				{
					if (this.owner.DisplayedItems[j].Alignment == ToolStripItemAlignment.Right)
					{
						if (j > 0)
						{
							return this.owner.Items.IndexOf(this.owner.DisplayedItems[j - 1]);
						}
						return this.owner.Items.IndexOf(this.owner.DisplayedItems[j]);
					}
					else
					{
						j++;
					}
				}
				return this.owner.Items.IndexOf(this.owner.DisplayedItems[this.owner.DisplayedItems.Count - 1]);
			}
			return -1;
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x0013B738 File Offset: 0x0013A738
		private ToolStripSplitStackDragDropHandler.RelativeLocation ComparePositions(Rectangle orig, Point check)
		{
			if (this.owner.Orientation == Orientation.Horizontal)
			{
				int num = orig.Width / 2;
				if (orig.Left + num >= check.X)
				{
					return ToolStripSplitStackDragDropHandler.RelativeLocation.Left;
				}
				if (orig.Right - num <= check.X)
				{
					return ToolStripSplitStackDragDropHandler.RelativeLocation.Right;
				}
			}
			if (this.owner.Orientation == Orientation.Vertical)
			{
				int num2 = orig.Height / 2;
				return (check.Y <= orig.Top + num2) ? ToolStripSplitStackDragDropHandler.RelativeLocation.Above : ToolStripSplitStackDragDropHandler.RelativeLocation.Below;
			}
			return ToolStripSplitStackDragDropHandler.RelativeLocation.Left;
		}

		// Token: 0x04003772 RID: 14194
		private ToolStrip owner;

		// Token: 0x02000672 RID: 1650
		private enum RelativeLocation
		{
			// Token: 0x04003774 RID: 14196
			Above,
			// Token: 0x04003775 RID: 14197
			Below,
			// Token: 0x04003776 RID: 14198
			Right,
			// Token: 0x04003777 RID: 14199
			Left
		}
	}
}
