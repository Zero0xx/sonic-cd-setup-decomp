using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006C6 RID: 1734
	internal class ToolStripPanelCell : ArrangedElement
	{
		// Token: 0x06005B43 RID: 23363 RVA: 0x0014A098 File Offset: 0x00149098
		public ToolStripPanelCell(Control control) : this(null, control)
		{
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x0014A0A4 File Offset: 0x001490A4
		public ToolStripPanelCell(ToolStripPanelRow parent, Control control)
		{
			this.ToolStripPanelRow = parent;
			this._wrappedToolStrip = (control as ToolStrip);
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this._wrappedToolStrip == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("TypedControlCollectionShouldBeOfType", new object[]
				{
					typeof(ToolStrip).Name
				}), new object[0]), control.GetType().Name);
			}
			CommonProperties.SetAutoSize(this, true);
			this._wrappedToolStrip.LocationChanging += this.OnToolStripLocationChanging;
			this._wrappedToolStrip.VisibleChanged += this.OnToolStripVisibleChanged;
		}

		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06005B45 RID: 23365 RVA: 0x0014A170 File Offset: 0x00149170
		// (set) Token: 0x06005B46 RID: 23366 RVA: 0x0014A178 File Offset: 0x00149178
		public Rectangle CachedBounds
		{
			get
			{
				return this.cachedBounds;
			}
			set
			{
				this.cachedBounds = value;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06005B47 RID: 23367 RVA: 0x0014A181 File Offset: 0x00149181
		public Control Control
		{
			get
			{
				return this._wrappedToolStrip;
			}
		}

		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x06005B48 RID: 23368 RVA: 0x0014A189 File Offset: 0x00149189
		public bool ControlInDesignMode
		{
			get
			{
				return this._wrappedToolStrip != null && this._wrappedToolStrip.IsInDesignMode;
			}
		}

		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x06005B49 RID: 23369 RVA: 0x0014A1A0 File Offset: 0x001491A0
		public IArrangedElement InnerElement
		{
			get
			{
				return this._wrappedToolStrip;
			}
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x06005B4A RID: 23370 RVA: 0x0014A1A8 File Offset: 0x001491A8
		public ISupportToolStripPanel DraggedControl
		{
			get
			{
				return this._wrappedToolStrip;
			}
		}

		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x06005B4B RID: 23371 RVA: 0x0014A1B0 File Offset: 0x001491B0
		// (set) Token: 0x06005B4C RID: 23372 RVA: 0x0014A1B8 File Offset: 0x001491B8
		public ToolStripPanelRow ToolStripPanelRow
		{
			get
			{
				return this.parent;
			}
			set
			{
				if (this.parent != value)
				{
					if (this.parent != null)
					{
						((IList)this.parent.Cells).Remove(this);
					}
					this.parent = value;
					base.Margin = Padding.Empty;
				}
			}
		}

		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x06005B4D RID: 23373 RVA: 0x0014A1EE File Offset: 0x001491EE
		// (set) Token: 0x06005B4E RID: 23374 RVA: 0x0014A21D File Offset: 0x0014921D
		public override bool Visible
		{
			get
			{
				return this.Control != null && this.Control.ParentInternal == this.ToolStripPanelRow.ToolStripPanel && this.InnerElement.ParticipatesInLayout;
			}
			set
			{
				this.Control.Visible = value;
			}
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x06005B4F RID: 23375 RVA: 0x0014A22B File Offset: 0x0014922B
		public Size MaximumSize
		{
			get
			{
				return this.maxSize;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06005B50 RID: 23376 RVA: 0x0014A233 File Offset: 0x00149233
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return DefaultLayout.Instance;
			}
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x0014A23A File Offset: 0x0014923A
		protected override IArrangedElement GetContainer()
		{
			return this.parent;
		}

		// Token: 0x06005B52 RID: 23378 RVA: 0x0014A242 File Offset: 0x00149242
		public int Grow(int growBy)
		{
			if (this.ToolStripPanelRow.Orientation == Orientation.Vertical)
			{
				return this.GrowVertical(growBy);
			}
			return this.GrowHorizontal(growBy);
		}

		// Token: 0x06005B53 RID: 23379 RVA: 0x0014A264 File Offset: 0x00149264
		private int GrowVertical(int growBy)
		{
			if (this.MaximumSize.Height >= this.Control.PreferredSize.Height)
			{
				return 0;
			}
			if (this.MaximumSize.Height + growBy >= this.Control.PreferredSize.Height)
			{
				int result = this.Control.PreferredSize.Height - this.MaximumSize.Height;
				this.maxSize = LayoutUtils.MaxSize;
				return result;
			}
			if (this.MaximumSize.Height + growBy < this.Control.PreferredSize.Height)
			{
				this.maxSize.Height = this.maxSize.Height + growBy;
				return growBy;
			}
			return 0;
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x0014A32C File Offset: 0x0014932C
		private int GrowHorizontal(int growBy)
		{
			if (this.MaximumSize.Width >= this.Control.PreferredSize.Width)
			{
				return 0;
			}
			if (this.MaximumSize.Width + growBy >= this.Control.PreferredSize.Width)
			{
				int result = this.Control.PreferredSize.Width - this.MaximumSize.Width;
				this.maxSize = LayoutUtils.MaxSize;
				return result;
			}
			if (this.MaximumSize.Width + growBy < this.Control.PreferredSize.Width)
			{
				this.maxSize.Width = this.maxSize.Width + growBy;
				return growBy;
			}
			return 0;
		}

		// Token: 0x06005B55 RID: 23381 RVA: 0x0014A3F4 File Offset: 0x001493F4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._wrappedToolStrip != null)
					{
						this._wrappedToolStrip.LocationChanging -= this.OnToolStripLocationChanging;
						this._wrappedToolStrip.VisibleChanged -= this.OnToolStripVisibleChanged;
					}
					this._wrappedToolStrip = null;
					if (this.parent != null)
					{
						((IList)this.parent.Cells).Remove(this);
					}
					this.parent = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06005B56 RID: 23382 RVA: 0x0014A47C File Offset: 0x0014947C
		protected override ArrangedElementCollection GetChildren()
		{
			return ArrangedElementCollection.Empty;
		}

		// Token: 0x06005B57 RID: 23383 RVA: 0x0014A484 File Offset: 0x00149484
		public override Size GetPreferredSize(Size constrainingSize)
		{
			ISupportToolStripPanel draggedControl = this.DraggedControl;
			Size result = Size.Empty;
			if (draggedControl.Stretch)
			{
				if (this.ToolStripPanelRow.Orientation == Orientation.Horizontal)
				{
					constrainingSize.Width = this.ToolStripPanelRow.Bounds.Width;
					result = this._wrappedToolStrip.GetPreferredSize(constrainingSize);
					result.Width = constrainingSize.Width;
				}
				else
				{
					constrainingSize.Height = this.ToolStripPanelRow.Bounds.Height;
					result = this._wrappedToolStrip.GetPreferredSize(constrainingSize);
					result.Height = constrainingSize.Height;
				}
			}
			else
			{
				result = ((!this._wrappedToolStrip.AutoSize) ? this._wrappedToolStrip.Size : this._wrappedToolStrip.GetPreferredSize(constrainingSize));
			}
			return result;
		}

		// Token: 0x06005B58 RID: 23384 RVA: 0x0014A54C File Offset: 0x0014954C
		protected override void SetBoundsCore(Rectangle bounds, BoundsSpecified specified)
		{
			this.currentlySizing = true;
			this.CachedBounds = bounds;
			try
			{
				if (this.DraggedControl.IsCurrentlyDragging)
				{
					if (this.ToolStripPanelRow.Cells[this.ToolStripPanelRow.Cells.Count - 1] == this)
					{
						Rectangle displayRectangle = this.ToolStripPanelRow.DisplayRectangle;
						if (this.ToolStripPanelRow.Orientation == Orientation.Horizontal)
						{
							int num = bounds.Right - displayRectangle.Right;
							if (num > 0 && bounds.Width > num)
							{
								bounds.Width -= num;
							}
						}
						else
						{
							int num2 = bounds.Bottom - displayRectangle.Bottom;
							if (num2 > 0 && bounds.Height > num2)
							{
								bounds.Height -= num2;
							}
						}
					}
					base.SetBoundsCore(bounds, specified);
					this.InnerElement.SetBounds(bounds, specified);
				}
				else if (!this.ToolStripPanelRow.CachedBoundsMode)
				{
					base.SetBoundsCore(bounds, specified);
					this.InnerElement.SetBounds(bounds, specified);
				}
			}
			finally
			{
				this.currentlySizing = false;
			}
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x0014A664 File Offset: 0x00149664
		public int Shrink(int shrinkBy)
		{
			if (this.ToolStripPanelRow.Orientation == Orientation.Vertical)
			{
				return this.ShrinkVertical(shrinkBy);
			}
			return this.ShrinkHorizontal(shrinkBy);
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x0014A683 File Offset: 0x00149683
		private int ShrinkHorizontal(int shrinkBy)
		{
			return 0;
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x0014A686 File Offset: 0x00149686
		private int ShrinkVertical(int shrinkBy)
		{
			return 0;
		}

		// Token: 0x06005B5C RID: 23388 RVA: 0x0014A68C File Offset: 0x0014968C
		private void OnToolStripLocationChanging(object sender, ToolStripLocationCancelEventArgs e)
		{
			if (this.ToolStripPanelRow == null)
			{
				return;
			}
			if (!this.currentlySizing && !this.currentlyDragging)
			{
				try
				{
					this.currentlyDragging = true;
					Point newLocation = e.NewLocation;
					if (this.ToolStripPanelRow != null && this.ToolStripPanelRow.Bounds == Rectangle.Empty)
					{
						this.ToolStripPanelRow.ToolStripPanel.PerformUpdate(true);
					}
					if (this._wrappedToolStrip != null)
					{
						this.ToolStripPanelRow.ToolStripPanel.Join(this._wrappedToolStrip, newLocation);
					}
				}
				finally
				{
					this.currentlyDragging = false;
					e.Cancel = true;
				}
			}
		}

		// Token: 0x06005B5D RID: 23389 RVA: 0x0014A730 File Offset: 0x00149730
		private void OnToolStripVisibleChanged(object sender, EventArgs e)
		{
			if (this._wrappedToolStrip != null && !this._wrappedToolStrip.IsInDesignMode && !this._wrappedToolStrip.IsCurrentlyDragging && !this._wrappedToolStrip.IsDisposed && !this._wrappedToolStrip.Disposing)
			{
				if (!this.Control.Visible)
				{
					this.restoreOnVisibleChanged = (this.ToolStripPanelRow != null && ((IList)this.ToolStripPanelRow.Cells).Contains(this));
					return;
				}
				if (this.restoreOnVisibleChanged)
				{
					try
					{
						if (this.ToolStripPanelRow != null && ((IList)this.ToolStripPanelRow.Cells).Contains(this))
						{
							this.ToolStripPanelRow.ToolStripPanel.Join(this._wrappedToolStrip, this._wrappedToolStrip.Location);
						}
					}
					finally
					{
						this.restoreOnVisibleChanged = false;
					}
				}
			}
		}

		// Token: 0x040038DC RID: 14556
		private ToolStrip _wrappedToolStrip;

		// Token: 0x040038DD RID: 14557
		private ToolStripPanelRow parent;

		// Token: 0x040038DE RID: 14558
		private Size maxSize = LayoutUtils.MaxSize;

		// Token: 0x040038DF RID: 14559
		private bool currentlySizing;

		// Token: 0x040038E0 RID: 14560
		private bool currentlyDragging;

		// Token: 0x040038E1 RID: 14561
		private bool restoreOnVisibleChanged;

		// Token: 0x040038E2 RID: 14562
		private Rectangle cachedBounds = Rectangle.Empty;
	}
}
