using System;
using System.Collections;
using System.Drawing;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007AC RID: 1964
	internal class CategoryGridEntry : GridEntry
	{
		// Token: 0x06006822 RID: 26658 RVA: 0x0017D494 File Offset: 0x0017C494
		public CategoryGridEntry(PropertyGrid ownerGrid, GridEntry peParent, string name, GridEntry[] childGridEntries) : base(ownerGrid, peParent)
		{
			this.name = name;
			if (CategoryGridEntry.categoryStates == null)
			{
				CategoryGridEntry.categoryStates = new Hashtable();
			}
			lock (CategoryGridEntry.categoryStates)
			{
				if (!CategoryGridEntry.categoryStates.ContainsKey(name))
				{
					CategoryGridEntry.categoryStates.Add(name, true);
				}
			}
			this.IsExpandable = true;
			for (int i = 0; i < childGridEntries.Length; i++)
			{
				childGridEntries[i].ParentGridEntry = this;
			}
			base.ChildCollection = new GridEntryCollection(this, childGridEntries);
			lock (CategoryGridEntry.categoryStates)
			{
				this.InternalExpanded = (bool)CategoryGridEntry.categoryStates[name];
			}
			this.SetFlag(64, true);
		}

		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x06006823 RID: 26659 RVA: 0x0017D574 File Offset: 0x0017C574
		internal override bool HasValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x0017D577 File Offset: 0x0017C577
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.backBrush != null)
				{
					this.backBrush.Dispose();
					this.backBrush = null;
				}
				if (base.ChildCollection != null)
				{
					base.ChildCollection = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x0017D5AC File Offset: 0x0017C5AC
		public override void DisposeChildren()
		{
		}

		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x06006826 RID: 26662 RVA: 0x0017D5AE File Offset: 0x0017C5AE
		public override int PropertyDepth
		{
			get
			{
				return base.PropertyDepth - 1;
			}
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x0017D5B8 File Offset: 0x0017C5B8
		protected override Brush GetBackgroundBrush(Graphics g)
		{
			return this.GridEntryHost.GetLineBrush(g);
		}

		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x06006828 RID: 26664 RVA: 0x0017D5C6 File Offset: 0x0017C5C6
		protected override Color LabelTextColor
		{
			get
			{
				return this.ownerGrid.CategoryForeColor;
			}
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06006829 RID: 26665 RVA: 0x0017D5D3 File Offset: 0x0017C5D3
		public override bool Expandable
		{
			get
			{
				return !this.GetFlagSet(524288);
			}
		}

		// Token: 0x1700161F RID: 5663
		// (set) Token: 0x0600682A RID: 26666 RVA: 0x0017D5E4 File Offset: 0x0017C5E4
		internal override bool InternalExpanded
		{
			set
			{
				base.InternalExpanded = value;
				lock (CategoryGridEntry.categoryStates)
				{
					CategoryGridEntry.categoryStates[this.name] = value;
				}
			}
		}

		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x0600682B RID: 26667 RVA: 0x0017D634 File Offset: 0x0017C634
		public override GridItemType GridItemType
		{
			get
			{
				return GridItemType.Category;
			}
		}

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x0600682C RID: 26668 RVA: 0x0017D637 File Offset: 0x0017C637
		public override string HelpKeyword
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x0600682D RID: 26669 RVA: 0x0017D63A File Offset: 0x0017C63A
		public override string PropertyLabel
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x0600682E RID: 26670 RVA: 0x0017D644 File Offset: 0x0017C644
		internal override int PropertyLabelIndent
		{
			get
			{
				PropertyGridView gridEntryHost = this.GridEntryHost;
				return 1 + gridEntryHost.GetOutlineIconSize() + 5 + base.PropertyDepth * gridEntryHost.GetDefaultOutlineIndent();
			}
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x0017D670 File Offset: 0x0017C670
		public override string GetPropertyTextValue(object o)
		{
			return "";
		}

		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06006830 RID: 26672 RVA: 0x0017D677 File Offset: 0x0017C677
		public override Type PropertyType
		{
			get
			{
				return typeof(void);
			}
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x0017D683 File Offset: 0x0017C683
		public override object GetChildValueOwner(GridEntry childEntry)
		{
			return this.ParentGridEntry.GetChildValueOwner(childEntry);
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x0017D691 File Offset: 0x0017C691
		protected override bool CreateChildren(bool diffOldChildren)
		{
			return true;
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x0017D694 File Offset: 0x0017C694
		public override string GetTestingInfo()
		{
			string str = "object = (";
			str += base.FullLabel;
			return str + "), Category = (" + this.PropertyLabel + ")";
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x0017D6CC File Offset: 0x0017C6CC
		public override void PaintLabel(Graphics g, Rectangle rect, Rectangle clipRect, bool selected, bool paintFullLabel)
		{
			base.PaintLabel(g, rect, clipRect, false, true);
			if (selected && this.hasFocus)
			{
				bool boldFont = (this.Flags & 64) != 0;
				Font font = base.GetFont(boldFont);
				int labelTextWidth = base.GetLabelTextWidth(this.PropertyLabel, g, font);
				int x = this.PropertyLabelIndent - 2;
				Rectangle rectangle = new Rectangle(x, rect.Y, labelTextWidth + 3, rect.Height - 1);
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
			if (this.parentPE.GetChildIndex(this) > 0)
			{
				g.DrawLine(SystemPens.Control, rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Y - 1);
			}
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x0017D784 File Offset: 0x0017C784
		public override void PaintOutline(Graphics g, Rectangle r)
		{
			if (this.Expandable)
			{
				bool expanded = this.Expanded;
				Rectangle rectangle = base.OutlineRect;
				rectangle = Rectangle.Intersect(r, rectangle);
				if (rectangle.IsEmpty)
				{
					return;
				}
				Color color = this.GridEntryHost.GetLineColor();
				Brush brush = new SolidBrush(g.GetNearestColor(color));
				bool flag = true;
				color = this.GridEntryHost.GetTextColor();
				Pen pen = new Pen(g.GetNearestColor(color));
				bool flag2 = true;
				g.FillRectangle(brush, rectangle);
				g.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				int num = 2;
				g.DrawLine(SystemPens.WindowText, rectangle.X + num, rectangle.Y + rectangle.Height / 2, rectangle.X + rectangle.Width - num - 1, rectangle.Y + rectangle.Height / 2);
				if (!expanded)
				{
					g.DrawLine(SystemPens.WindowText, rectangle.X + rectangle.Width / 2, rectangle.Y + num, rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height - num - 1);
				}
				if (flag2)
				{
					pen.Dispose();
				}
				if (flag)
				{
					brush.Dispose();
				}
			}
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x0017D8E0 File Offset: 0x0017C8E0
		public override void PaintValue(object val, Graphics g, Rectangle rect, Rectangle clipRect, GridEntry.PaintValueFlags paintFlags)
		{
			base.PaintValue(val, g, rect, clipRect, paintFlags & ~GridEntry.PaintValueFlags.DrawSelected);
			if (this.parentPE.GetChildIndex(this) > 0)
			{
				g.DrawLine(SystemPens.Control, rect.X - 2, rect.Y - 1, rect.Width + 1, rect.Y - 1);
			}
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x0017D93B File Offset: 0x0017C93B
		internal override bool NotifyChildValue(GridEntry pe, int type)
		{
			return this.parentPE.NotifyChildValue(pe, type);
		}

		// Token: 0x04003D5D RID: 15709
		internal string name;

		// Token: 0x04003D5E RID: 15710
		private Brush backBrush;

		// Token: 0x04003D5F RID: 15711
		private static Hashtable categoryStates;
	}
}
