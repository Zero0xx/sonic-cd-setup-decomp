using System;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000691 RID: 1681
	internal class ToolStripGrip : ToolStripButton
	{
		// Token: 0x060058E8 RID: 22760 RVA: 0x001436C2 File Offset: 0x001426C2
		internal ToolStripGrip()
		{
			this.gripThickness = (ToolStripManager.VisualStylesEnabled ? 5 : 3);
			base.SupportsItemClick = false;
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x060058E9 RID: 22761 RVA: 0x001436F8 File Offset: 0x001426F8
		protected internal override Padding DefaultMargin
		{
			get
			{
				return new Padding(2);
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x060058EA RID: 22762 RVA: 0x00143700 File Offset: 0x00142700
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x060058EB RID: 22763 RVA: 0x00143703 File Offset: 0x00142703
		internal int GripThickness
		{
			get
			{
				return this.gripThickness;
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x060058EC RID: 22764 RVA: 0x0014370B File Offset: 0x0014270B
		// (set) Token: 0x060058ED RID: 22765 RVA: 0x00143720 File Offset: 0x00142720
		internal bool MovingToolStrip
		{
			get
			{
				return this.ToolStripPanelRow != null && this.movingToolStrip;
			}
			set
			{
				if (this.movingToolStrip != value && base.ParentInternal != null)
				{
					if (value && base.ParentInternal.ToolStripPanelRow == null)
					{
						return;
					}
					this.movingToolStrip = value;
					this.lastEndLocation = ToolStrip.InvalidMouseEnter;
					if (this.movingToolStrip)
					{
						((ISupportToolStripPanel)base.ParentInternal).BeginDrag();
						return;
					}
					((ISupportToolStripPanel)base.ParentInternal).EndDrag();
				}
			}
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x060058EE RID: 22766 RVA: 0x00143780 File Offset: 0x00142780
		private ToolStripPanelRow ToolStripPanelRow
		{
			get
			{
				if (base.ParentInternal != null)
				{
					return ((ISupportToolStripPanel)base.ParentInternal).ToolStripPanelRow;
				}
				return null;
			}
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x00143797 File Offset: 0x00142797
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripGrip.ToolStripGripAccessibleObject(this);
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x001437A0 File Offset: 0x001427A0
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size empty = Size.Empty;
			if (base.ParentInternal != null)
			{
				if (base.ParentInternal.LayoutStyle == ToolStripLayoutStyle.VerticalStackWithOverflow)
				{
					empty = new Size(base.ParentInternal.Width, this.gripThickness);
				}
				else
				{
					empty = new Size(this.gripThickness, base.ParentInternal.Height);
				}
			}
			if (empty.Width > constrainingSize.Width)
			{
				empty.Width = constrainingSize.Width;
			}
			if (empty.Height > constrainingSize.Height)
			{
				empty.Height = constrainingSize.Height;
			}
			return empty;
		}

		// Token: 0x060058F1 RID: 22769 RVA: 0x00143838 File Offset: 0x00142838
		private bool LeftMouseButtonIsDown()
		{
			return Control.MouseButtons == MouseButtons.Left && Control.ModifierKeys == Keys.None;
		}

		// Token: 0x060058F2 RID: 22770 RVA: 0x00143850 File Offset: 0x00142850
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.ParentInternal != null)
			{
				base.ParentInternal.OnPaintGrip(e);
			}
		}

		// Token: 0x060058F3 RID: 22771 RVA: 0x00143866 File Offset: 0x00142866
		protected override void OnMouseDown(MouseEventArgs mea)
		{
			this.startLocation = base.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
			base.OnMouseDown(mea);
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x00143890 File Offset: 0x00142890
		protected override void OnMouseMove(MouseEventArgs mea)
		{
			bool flag = this.LeftMouseButtonIsDown();
			if (!this.MovingToolStrip && flag)
			{
				Point point = base.TranslatePoint(mea.Location, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
				int num = point.X - this.startLocation.X;
				num = ((num < 0) ? (num * -1) : num);
				if (ToolStripGrip.DragSize == LayoutUtils.MaxSize)
				{
					ToolStripGrip.DragSize = SystemInformation.DragSize;
				}
				if (num >= ToolStripGrip.DragSize.Width)
				{
					this.MovingToolStrip = true;
				}
				else
				{
					int num2 = point.Y - this.startLocation.Y;
					num2 = ((num2 < 0) ? (num2 * -1) : num2);
					if (num2 >= ToolStripGrip.DragSize.Height)
					{
						this.MovingToolStrip = true;
					}
				}
			}
			if (this.MovingToolStrip)
			{
				if (flag)
				{
					Point point2 = base.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
					if (point2 != this.lastEndLocation)
					{
						this.ToolStripPanelRow.ToolStripPanel.MoveControl(base.ParentInternal, point2);
						this.lastEndLocation = point2;
					}
					this.startLocation = point2;
				}
				else
				{
					this.MovingToolStrip = false;
				}
			}
			base.OnMouseMove(mea);
		}

		// Token: 0x060058F5 RID: 22773 RVA: 0x001439B4 File Offset: 0x001429B4
		protected override void OnMouseEnter(EventArgs e)
		{
			if (base.ParentInternal != null && this.ToolStripPanelRow != null && !base.ParentInternal.IsInDesignMode)
			{
				this.oldCursor = base.ParentInternal.Cursor;
				ToolStripGrip.SetCursor(base.ParentInternal, Cursors.SizeAll);
			}
			else
			{
				this.oldCursor = null;
			}
			base.OnMouseEnter(e);
		}

		// Token: 0x060058F6 RID: 22774 RVA: 0x00143A10 File Offset: 0x00142A10
		protected override void OnMouseLeave(EventArgs e)
		{
			if (this.oldCursor != null && !base.ParentInternal.IsInDesignMode)
			{
				ToolStripGrip.SetCursor(base.ParentInternal, this.oldCursor);
			}
			if (!this.MovingToolStrip && this.LeftMouseButtonIsDown())
			{
				this.MovingToolStrip = true;
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x060058F7 RID: 22775 RVA: 0x00143A68 File Offset: 0x00142A68
		protected override void OnMouseUp(MouseEventArgs mea)
		{
			if (this.MovingToolStrip)
			{
				Point screenLocation = base.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
				this.ToolStripPanelRow.ToolStripPanel.MoveControl(base.ParentInternal, screenLocation);
			}
			if (!base.ParentInternal.IsInDesignMode)
			{
				ToolStripGrip.SetCursor(base.ParentInternal, this.oldCursor);
			}
			ToolStripPanel.ClearDragFeedback();
			this.MovingToolStrip = false;
			base.OnMouseUp(mea);
		}

		// Token: 0x060058F8 RID: 22776 RVA: 0x00143ADF File Offset: 0x00142ADF
		private static void SetCursor(Control control, Cursor cursor)
		{
			IntSecurity.ModifyCursor.Assert();
			control.Cursor = cursor;
		}

		// Token: 0x04003825 RID: 14373
		private Cursor oldCursor;

		// Token: 0x04003826 RID: 14374
		private int gripThickness;

		// Token: 0x04003827 RID: 14375
		private Point startLocation = Point.Empty;

		// Token: 0x04003828 RID: 14376
		private bool movingToolStrip;

		// Token: 0x04003829 RID: 14377
		private Point lastEndLocation = ToolStrip.InvalidMouseEnter;

		// Token: 0x0400382A RID: 14378
		private static Size DragSize = LayoutUtils.MaxSize;

		// Token: 0x02000692 RID: 1682
		internal class ToolStripGripAccessibleObject : ToolStripButton.ToolStripButtonAccessibleObject
		{
			// Token: 0x060058FA RID: 22778 RVA: 0x00143AFE File Offset: 0x00142AFE
			public ToolStripGripAccessibleObject(ToolStripGrip owner) : base(owner)
			{
			}

			// Token: 0x17001267 RID: 4711
			// (get) Token: 0x060058FB RID: 22779 RVA: 0x00143B08 File Offset: 0x00142B08
			// (set) Token: 0x060058FC RID: 22780 RVA: 0x00143B49 File Offset: 0x00142B49
			public override string Name
			{
				get
				{
					string accessibleName = base.Owner.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					if (string.IsNullOrEmpty(this.stockName))
					{
						this.stockName = SR.GetString("ToolStripGripAccessibleName");
					}
					return this.stockName;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x17001268 RID: 4712
			// (get) Token: 0x060058FD RID: 22781 RVA: 0x00143B54 File Offset: 0x00142B54
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Grip;
				}
			}

			// Token: 0x0400382B RID: 14379
			private string stockName;
		}
	}
}
