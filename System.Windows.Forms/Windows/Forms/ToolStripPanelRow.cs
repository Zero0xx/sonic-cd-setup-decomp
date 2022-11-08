using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006CB RID: 1739
	[ToolboxItem(false)]
	public class ToolStripPanelRow : Component, IArrangedElement, IComponent, IDisposable
	{
		// Token: 0x06005B70 RID: 23408 RVA: 0x0014A882 File Offset: 0x00149882
		public ToolStripPanelRow(ToolStripPanel parent) : this(parent, true)
		{
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x0014A88C File Offset: 0x0014988C
		internal ToolStripPanelRow(ToolStripPanel parent, bool visible)
		{
			this.parent = parent;
			this.state[ToolStripPanelRow.stateVisible] = visible;
			this.state[ToolStripPanelRow.stateDisposing | ToolStripPanelRow.stateLocked | ToolStripPanelRow.stateInitialized] = false;
			using (new LayoutTransaction(parent, this, null))
			{
				this.Margin = this.DefaultMargin;
				CommonProperties.SetAutoSize(this, true);
			}
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x06005B72 RID: 23410 RVA: 0x0014A930 File Offset: 0x00149930
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x06005B73 RID: 23411 RVA: 0x0014A938 File Offset: 0x00149938
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlControlsDescr")]
		public Control[] Controls
		{
			get
			{
				Control[] array = new Control[this.ControlsInternal.Count];
				this.ControlsInternal.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x06005B74 RID: 23412 RVA: 0x0014A964 File Offset: 0x00149964
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("ControlControlsDescr")]
		internal ToolStripPanelRow.ToolStripPanelRowControlCollection ControlsInternal
		{
			get
			{
				ToolStripPanelRow.ToolStripPanelRowControlCollection toolStripPanelRowControlCollection = (ToolStripPanelRow.ToolStripPanelRowControlCollection)this.Properties.GetObject(ToolStripPanelRow.PropControlsCollection);
				if (toolStripPanelRowControlCollection == null)
				{
					toolStripPanelRowControlCollection = this.CreateControlsInstance();
					this.Properties.SetObject(ToolStripPanelRow.PropControlsCollection, toolStripPanelRowControlCollection);
				}
				return toolStripPanelRowControlCollection;
			}
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x06005B75 RID: 23413 RVA: 0x0014A9A3 File Offset: 0x001499A3
		internal ArrangedElementCollection Cells
		{
			get
			{
				return this.ControlsInternal.Cells;
			}
		}

		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x06005B76 RID: 23414 RVA: 0x0014A9B0 File Offset: 0x001499B0
		// (set) Token: 0x06005B77 RID: 23415 RVA: 0x0014A9C2 File Offset: 0x001499C2
		internal bool CachedBoundsMode
		{
			get
			{
				return this.state[ToolStripPanelRow.stateCachedBoundsMode];
			}
			set
			{
				this.state[ToolStripPanelRow.stateCachedBoundsMode] = value;
			}
		}

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x06005B78 RID: 23416 RVA: 0x0014A9D5 File Offset: 0x001499D5
		private ToolStripPanelRow.ToolStripPanelRowManager RowManager
		{
			get
			{
				if (this.rowManager == null)
				{
					this.rowManager = ((this.Orientation == Orientation.Horizontal) ? new ToolStripPanelRow.HorizontalRowManager(this) : new ToolStripPanelRow.VerticalRowManager(this));
					this.Initialized = true;
				}
				return this.rowManager;
			}
		}

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x06005B79 RID: 23417 RVA: 0x0014AA08 File Offset: 0x00149A08
		protected virtual Padding DefaultMargin
		{
			get
			{
				ToolStripPanelCell nextVisibleCell = this.RowManager.GetNextVisibleCell(0, true);
				if (nextVisibleCell != null && nextVisibleCell.DraggedControl != null && nextVisibleCell.DraggedControl.Stretch)
				{
					Padding rowMargin = this.ToolStripPanel.RowMargin;
					if (this.Orientation == Orientation.Horizontal)
					{
						rowMargin.Left = 0;
						rowMargin.Right = 0;
					}
					else
					{
						rowMargin.Top = 0;
						rowMargin.Bottom = 0;
					}
					return rowMargin;
				}
				return this.ToolStripPanel.RowMargin;
			}
		}

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x06005B7A RID: 23418 RVA: 0x0014AA7E File Offset: 0x00149A7E
		protected virtual Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06005B7B RID: 23419 RVA: 0x0014AA85 File Offset: 0x00149A85
		public Rectangle DisplayRectangle
		{
			get
			{
				return this.RowManager.DisplayRectangle;
			}
		}

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x06005B7C RID: 23420 RVA: 0x0014AA92 File Offset: 0x00149A92
		public LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x06005B7D RID: 23421 RVA: 0x0014AA99 File Offset: 0x00149A99
		internal bool Locked
		{
			get
			{
				return this.state[ToolStripPanelRow.stateLocked];
			}
		}

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x06005B7E RID: 23422 RVA: 0x0014AAAB File Offset: 0x00149AAB
		// (set) Token: 0x06005B7F RID: 23423 RVA: 0x0014AABD File Offset: 0x00149ABD
		private bool Initialized
		{
			get
			{
				return this.state[ToolStripPanelRow.stateInitialized];
			}
			set
			{
				this.state[ToolStripPanelRow.stateInitialized] = value;
			}
		}

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x06005B80 RID: 23424 RVA: 0x0014AAD0 File Offset: 0x00149AD0
		// (set) Token: 0x06005B81 RID: 23425 RVA: 0x0014AAD8 File Offset: 0x00149AD8
		public Padding Margin
		{
			get
			{
				return CommonProperties.GetMargin(this);
			}
			set
			{
				if (this.Margin != value)
				{
					CommonProperties.SetMargin(this, value);
				}
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06005B82 RID: 23426 RVA: 0x0014AAEF File Offset: 0x00149AEF
		// (set) Token: 0x06005B83 RID: 23427 RVA: 0x0014AAFD File Offset: 0x00149AFD
		public virtual Padding Padding
		{
			get
			{
				return CommonProperties.GetPadding(this, this.DefaultPadding);
			}
			set
			{
				if (this.Padding != value)
				{
					CommonProperties.SetPadding(this, value);
				}
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06005B84 RID: 23428 RVA: 0x0014AB14 File Offset: 0x00149B14
		internal Control ParentInternal
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06005B85 RID: 23429 RVA: 0x0014AB1C File Offset: 0x00149B1C
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x0014AB24 File Offset: 0x00149B24
		public ToolStripPanel ToolStripPanel
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06005B87 RID: 23431 RVA: 0x0014AB2C File Offset: 0x00149B2C
		internal bool Visible
		{
			get
			{
				return this.state[ToolStripPanelRow.stateVisible];
			}
		}

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x0014AB3E File Offset: 0x00149B3E
		public Orientation Orientation
		{
			get
			{
				return this.ToolStripPanel.Orientation;
			}
		}

		// Token: 0x06005B89 RID: 23433 RVA: 0x0014AB4B File Offset: 0x00149B4B
		public bool CanMove(ToolStrip toolStripToDrag)
		{
			return !this.ToolStripPanel.Locked && !this.Locked && this.RowManager.CanMove(toolStripToDrag);
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x0014AB70 File Offset: 0x00149B70
		private ToolStripPanelRow.ToolStripPanelRowControlCollection CreateControlsInstance()
		{
			return new ToolStripPanelRow.ToolStripPanelRowControlCollection(this);
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x0014AB78 File Offset: 0x00149B78
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.state[ToolStripPanelRow.stateDisposing] = true;
					this.ControlsInternal.Clear();
				}
			}
			finally
			{
				this.state[ToolStripPanelRow.stateDisposing] = false;
				base.Dispose(disposing);
			}
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x0014ABD0 File Offset: 0x00149BD0
		protected internal virtual void OnControlAdded(Control control, int index)
		{
			ISupportToolStripPanel supportToolStripPanel = control as ISupportToolStripPanel;
			if (supportToolStripPanel != null)
			{
				supportToolStripPanel.ToolStripPanelRow = this;
			}
			this.RowManager.OnControlAdded(control, index);
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x0014ABFB File Offset: 0x00149BFB
		protected internal virtual void OnOrientationChanged()
		{
			this.rowManager = null;
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x0014AC04 File Offset: 0x00149C04
		protected void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			((IArrangedElement)this).PerformLayout(this, PropertyNames.Size);
			this.RowManager.OnBoundsChanged(oldBounds, newBounds);
		}

		// Token: 0x06005B8F RID: 23439 RVA: 0x0014AC20 File Offset: 0x00149C20
		protected internal virtual void OnControlRemoved(Control control, int index)
		{
			if (!this.state[ToolStripPanelRow.stateDisposing])
			{
				this.SuspendLayout();
				this.RowManager.OnControlRemoved(control, index);
				ISupportToolStripPanel supportToolStripPanel = control as ISupportToolStripPanel;
				if (supportToolStripPanel != null && supportToolStripPanel.ToolStripPanelRow == this)
				{
					supportToolStripPanel.ToolStripPanelRow = null;
				}
				this.ResumeLayout(true);
				if (this.ControlsInternal.Count <= 0)
				{
					this.ToolStripPanel.RowsInternal.Remove(this);
					base.Dispose();
				}
			}
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x0014AC98 File Offset: 0x00149C98
		internal Size GetMinimumSize(ToolStrip toolStrip)
		{
			if (toolStrip.MinimumSize == Size.Empty)
			{
				return new Size(50, 50);
			}
			return toolStrip.MinimumSize;
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x0014ACBC File Offset: 0x00149CBC
		private void ApplyCachedBounds()
		{
			for (int i = 0; i < this.Cells.Count; i++)
			{
				IArrangedElement arrangedElement = this.Cells[i];
				if (arrangedElement.ParticipatesInLayout)
				{
					ToolStripPanelCell toolStripPanelCell = arrangedElement as ToolStripPanelCell;
					arrangedElement.SetBounds(toolStripPanelCell.CachedBounds, BoundsSpecified.None);
				}
			}
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x0014AD08 File Offset: 0x00149D08
		protected virtual void OnLayout(LayoutEventArgs e)
		{
			if (this.Initialized && !this.state[ToolStripPanelRow.stateInLayout])
			{
				this.state[ToolStripPanelRow.stateInLayout] = true;
				try
				{
					this.Margin = this.DefaultMargin;
					this.CachedBoundsMode = true;
					try
					{
						this.LayoutEngine.Layout(this, e);
					}
					finally
					{
						this.CachedBoundsMode = false;
					}
					if (this.RowManager.GetNextVisibleCell(this.Cells.Count - 1, false) == null)
					{
						this.ApplyCachedBounds();
					}
					else if (this.Orientation == Orientation.Horizontal)
					{
						this.OnLayoutHorizontalPostFix();
					}
					else
					{
						this.OnLayoutVerticalPostFix();
					}
				}
				finally
				{
					this.state[ToolStripPanelRow.stateInLayout] = false;
				}
			}
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x0014ADDC File Offset: 0x00149DDC
		private void OnLayoutHorizontalPostFix()
		{
			ToolStripPanelCell nextVisibleCell = this.RowManager.GetNextVisibleCell(this.Cells.Count - 1, false);
			if (nextVisibleCell == null)
			{
				this.ApplyCachedBounds();
				return;
			}
			int num = nextVisibleCell.CachedBounds.Right - this.RowManager.DisplayRectangle.Right;
			if (num <= 0)
			{
				this.ApplyCachedBounds();
				return;
			}
			int[] array = new int[this.Cells.Count];
			for (int i = 0; i < this.Cells.Count; i++)
			{
				ToolStripPanelCell toolStripPanelCell = this.Cells[i] as ToolStripPanelCell;
				array[i] = toolStripPanelCell.Margin.Left;
			}
			num -= this.RowManager.FreeSpaceFromRow(num);
			for (int j = 0; j < this.Cells.Count; j++)
			{
				ToolStripPanelCell toolStripPanelCell2 = this.Cells[j] as ToolStripPanelCell;
				Rectangle cachedBounds = toolStripPanelCell2.CachedBounds;
				cachedBounds.X -= Math.Max(0, array[j] - toolStripPanelCell2.Margin.Left);
				toolStripPanelCell2.CachedBounds = cachedBounds;
			}
			if (num <= 0)
			{
				this.ApplyCachedBounds();
				return;
			}
			int[] array2 = null;
			for (int k = this.Cells.Count - 1; k >= 0; k--)
			{
				ToolStripPanelCell toolStripPanelCell3 = this.Cells[k] as ToolStripPanelCell;
				if (toolStripPanelCell3.Visible)
				{
					Size minimumSize = this.GetMinimumSize(toolStripPanelCell3.Control as ToolStrip);
					Rectangle cachedBounds2 = toolStripPanelCell3.CachedBounds;
					if (cachedBounds2.Width > minimumSize.Width)
					{
						num -= cachedBounds2.Width - minimumSize.Width;
						cachedBounds2.Width = ((num < 0) ? (minimumSize.Width + -num) : minimumSize.Width);
						for (int l = k + 1; l < this.Cells.Count; l++)
						{
							if (array2 == null)
							{
								array2 = new int[this.Cells.Count];
							}
							array2[l] += Math.Max(0, toolStripPanelCell3.CachedBounds.Width - cachedBounds2.Width);
						}
						toolStripPanelCell3.CachedBounds = cachedBounds2;
					}
				}
				if (num <= 0)
				{
					break;
				}
			}
			if (array2 != null)
			{
				for (int m = 0; m < this.Cells.Count; m++)
				{
					ToolStripPanelCell toolStripPanelCell4 = this.Cells[m] as ToolStripPanelCell;
					Rectangle cachedBounds3 = toolStripPanelCell4.CachedBounds;
					cachedBounds3.X -= array2[m];
					toolStripPanelCell4.CachedBounds = cachedBounds3;
				}
			}
			this.ApplyCachedBounds();
		}

		// Token: 0x06005B94 RID: 23444 RVA: 0x0014B084 File Offset: 0x0014A084
		private void OnLayoutVerticalPostFix()
		{
			ToolStripPanelCell nextVisibleCell = this.RowManager.GetNextVisibleCell(this.Cells.Count - 1, false);
			int num = nextVisibleCell.CachedBounds.Bottom - this.RowManager.DisplayRectangle.Bottom;
			if (num <= 0)
			{
				this.ApplyCachedBounds();
				return;
			}
			int[] array = new int[this.Cells.Count];
			for (int i = 0; i < this.Cells.Count; i++)
			{
				ToolStripPanelCell toolStripPanelCell = this.Cells[i] as ToolStripPanelCell;
				array[i] = toolStripPanelCell.Margin.Top;
			}
			num -= this.RowManager.FreeSpaceFromRow(num);
			for (int j = 0; j < this.Cells.Count; j++)
			{
				ToolStripPanelCell toolStripPanelCell2 = this.Cells[j] as ToolStripPanelCell;
				Rectangle cachedBounds = toolStripPanelCell2.CachedBounds;
				cachedBounds.X = Math.Max(0, cachedBounds.X - array[j] - toolStripPanelCell2.Margin.Top);
				toolStripPanelCell2.CachedBounds = cachedBounds;
			}
			if (num <= 0)
			{
				this.ApplyCachedBounds();
				return;
			}
			int[] array2 = null;
			for (int k = this.Cells.Count - 1; k >= 0; k--)
			{
				ToolStripPanelCell toolStripPanelCell3 = this.Cells[k] as ToolStripPanelCell;
				if (toolStripPanelCell3.Visible)
				{
					Size minimumSize = this.GetMinimumSize(toolStripPanelCell3.Control as ToolStrip);
					Rectangle cachedBounds2 = toolStripPanelCell3.CachedBounds;
					if (cachedBounds2.Height > minimumSize.Height)
					{
						num -= cachedBounds2.Height - minimumSize.Height;
						cachedBounds2.Height = ((num < 0) ? (minimumSize.Height + -num) : minimumSize.Height);
						for (int l = k + 1; l < this.Cells.Count; l++)
						{
							if (array2 == null)
							{
								array2 = new int[this.Cells.Count];
							}
							array2[l] += Math.Max(0, toolStripPanelCell3.CachedBounds.Height - cachedBounds2.Height);
						}
						toolStripPanelCell3.CachedBounds = cachedBounds2;
					}
				}
				if (num <= 0)
				{
					break;
				}
			}
			if (array2 != null)
			{
				for (int m = 0; m < this.Cells.Count; m++)
				{
					ToolStripPanelCell toolStripPanelCell4 = this.Cells[m] as ToolStripPanelCell;
					Rectangle cachedBounds3 = toolStripPanelCell4.CachedBounds;
					cachedBounds3.Y -= array2[m];
					toolStripPanelCell4.CachedBounds = cachedBounds3;
				}
			}
			this.ApplyCachedBounds();
		}

		// Token: 0x06005B95 RID: 23445 RVA: 0x0014B324 File Offset: 0x0014A324
		private void SetBounds(Rectangle bounds)
		{
			if (bounds != this.bounds)
			{
				Rectangle oldBounds = this.bounds;
				this.bounds = bounds;
				this.OnBoundsChanged(oldBounds, bounds);
			}
		}

		// Token: 0x06005B96 RID: 23446 RVA: 0x0014B355 File Offset: 0x0014A355
		private void SuspendLayout()
		{
			this.suspendCount++;
		}

		// Token: 0x06005B97 RID: 23447 RVA: 0x0014B365 File Offset: 0x0014A365
		private void ResumeLayout(bool performLayout)
		{
			this.suspendCount--;
			if (performLayout)
			{
				((IArrangedElement)this).PerformLayout(this, null);
			}
		}

		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06005B98 RID: 23448 RVA: 0x0014B380 File Offset: 0x0014A380
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.Cells;
			}
		}

		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x06005B99 RID: 23449 RVA: 0x0014B388 File Offset: 0x0014A388
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				return this.ToolStripPanel;
			}
		}

		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x06005B9A RID: 23450 RVA: 0x0014B390 File Offset: 0x0014A390
		Rectangle IArrangedElement.DisplayRectangle
		{
			get
			{
				return this.Bounds;
			}
		}

		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06005B9B RID: 23451 RVA: 0x0014B3A5 File Offset: 0x0014A3A5
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return this.Visible;
			}
		}

		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06005B9C RID: 23452 RVA: 0x0014B3AD File Offset: 0x0014A3AD
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return this.Properties;
			}
		}

		// Token: 0x06005B9D RID: 23453 RVA: 0x0014B3B8 File Offset: 0x0014A3B8
		Size IArrangedElement.GetPreferredSize(Size constrainingSize)
		{
			Size result = this.LayoutEngine.GetPreferredSize(this, constrainingSize - this.Padding.Size) + this.Padding.Size;
			if (this.Orientation == Orientation.Horizontal && this.ParentInternal != null)
			{
				result.Width = this.DisplayRectangle.Width;
			}
			else
			{
				result.Height = this.DisplayRectangle.Height;
			}
			return result;
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x0014B437 File Offset: 0x0014A437
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBounds(bounds);
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x0014B440 File Offset: 0x0014A440
		void IArrangedElement.PerformLayout(IArrangedElement container, string propertyName)
		{
			if (this.suspendCount <= 0)
			{
				this.OnLayout(new LayoutEventArgs(container, propertyName));
			}
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06005BA0 RID: 23456 RVA: 0x0014B458 File Offset: 0x0014A458
		internal Rectangle DragBounds
		{
			get
			{
				return this.RowManager.DragBounds;
			}
		}

		// Token: 0x06005BA1 RID: 23457 RVA: 0x0014B465 File Offset: 0x0014A465
		internal void MoveControl(ToolStrip movingControl, Point startClientLocation, Point endClientLocation)
		{
			this.RowManager.MoveControl(movingControl, startClientLocation, endClientLocation);
		}

		// Token: 0x06005BA2 RID: 23458 RVA: 0x0014B475 File Offset: 0x0014A475
		internal void JoinRow(ToolStrip toolStripToDrag, Point locationToDrag)
		{
			this.RowManager.JoinRow(toolStripToDrag, locationToDrag);
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x0014B484 File Offset: 0x0014A484
		internal void LeaveRow(ToolStrip toolStripToDrag)
		{
			this.RowManager.LeaveRow(toolStripToDrag);
			if (this.ControlsInternal.Count == 0)
			{
				this.ToolStripPanel.RowsInternal.Remove(this);
				base.Dispose();
			}
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x0014B4B6 File Offset: 0x0014A4B6
		[Conditional("DEBUG")]
		private void PrintPlacements(int index)
		{
		}

		// Token: 0x040038E9 RID: 14569
		private const int MINALLOWEDWIDTH = 50;

		// Token: 0x040038EA RID: 14570
		private const int DragInflateSize = 4;

		// Token: 0x040038EB RID: 14571
		private Rectangle bounds = Rectangle.Empty;

		// Token: 0x040038EC RID: 14572
		private ToolStripPanel parent;

		// Token: 0x040038ED RID: 14573
		private BitVector32 state = default(BitVector32);

		// Token: 0x040038EE RID: 14574
		private PropertyStore propertyStore = new PropertyStore();

		// Token: 0x040038EF RID: 14575
		private int suspendCount;

		// Token: 0x040038F0 RID: 14576
		private ToolStripPanelRow.ToolStripPanelRowManager rowManager;

		// Token: 0x040038F1 RID: 14577
		private static readonly int stateVisible = BitVector32.CreateMask();

		// Token: 0x040038F2 RID: 14578
		private static readonly int stateDisposing = BitVector32.CreateMask(ToolStripPanelRow.stateVisible);

		// Token: 0x040038F3 RID: 14579
		private static readonly int stateLocked = BitVector32.CreateMask(ToolStripPanelRow.stateDisposing);

		// Token: 0x040038F4 RID: 14580
		private static readonly int stateInitialized = BitVector32.CreateMask(ToolStripPanelRow.stateLocked);

		// Token: 0x040038F5 RID: 14581
		private static readonly int stateCachedBoundsMode = BitVector32.CreateMask(ToolStripPanelRow.stateInitialized);

		// Token: 0x040038F6 RID: 14582
		private static readonly int stateInLayout = BitVector32.CreateMask(ToolStripPanelRow.stateCachedBoundsMode);

		// Token: 0x040038F7 RID: 14583
		private static readonly int PropControlsCollection = PropertyStore.CreateKey();

		// Token: 0x040038F8 RID: 14584
		internal static TraceSwitch ToolStripPanelRowCreationDebug;

		// Token: 0x040038F9 RID: 14585
		internal static readonly TraceSwitch ToolStripPanelMouseDebug;

		// Token: 0x020006CC RID: 1740
		private abstract class ToolStripPanelRowManager
		{
			// Token: 0x06005BA6 RID: 23462 RVA: 0x0014B524 File Offset: 0x0014A524
			public ToolStripPanelRowManager(ToolStripPanelRow owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005BA7 RID: 23463 RVA: 0x0014B534 File Offset: 0x0014A534
			public virtual bool CanMove(ToolStrip toolStripToDrag)
			{
				if (toolStripToDrag != null && ((ISupportToolStripPanel)toolStripToDrag).Stretch)
				{
					return false;
				}
				foreach (object obj in this.Row.ControlsInternal)
				{
					Control control = (Control)obj;
					ISupportToolStripPanel supportToolStripPanel = control as ISupportToolStripPanel;
					if (supportToolStripPanel != null && supportToolStripPanel.Stretch)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x17001331 RID: 4913
			// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x0014B5B8 File Offset: 0x0014A5B8
			public virtual Rectangle DragBounds
			{
				get
				{
					return Rectangle.Empty;
				}
			}

			// Token: 0x17001332 RID: 4914
			// (get) Token: 0x06005BA9 RID: 23465 RVA: 0x0014B5BF File Offset: 0x0014A5BF
			public virtual Rectangle DisplayRectangle
			{
				get
				{
					return Rectangle.Empty;
				}
			}

			// Token: 0x17001333 RID: 4915
			// (get) Token: 0x06005BAA RID: 23466 RVA: 0x0014B5C6 File Offset: 0x0014A5C6
			public ToolStripPanel ToolStripPanel
			{
				get
				{
					return this.owner.ToolStripPanel;
				}
			}

			// Token: 0x17001334 RID: 4916
			// (get) Token: 0x06005BAB RID: 23467 RVA: 0x0014B5D3 File Offset: 0x0014A5D3
			public ToolStripPanelRow Row
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x17001335 RID: 4917
			// (get) Token: 0x06005BAC RID: 23468 RVA: 0x0014B5DB File Offset: 0x0014A5DB
			public FlowLayoutSettings FlowLayoutSettings
			{
				get
				{
					if (this.flowLayoutSettings == null)
					{
						this.flowLayoutSettings = new FlowLayoutSettings(this.owner);
					}
					return this.flowLayoutSettings;
				}
			}

			// Token: 0x06005BAD RID: 23469 RVA: 0x0014B5FC File Offset: 0x0014A5FC
			protected internal virtual int FreeSpaceFromRow(int spaceToFree)
			{
				return 0;
			}

			// Token: 0x06005BAE RID: 23470 RVA: 0x0014B600 File Offset: 0x0014A600
			protected virtual int Grow(int index, int growBy)
			{
				int result = 0;
				if (index >= 0 && index < this.Row.ControlsInternal.Count - 1)
				{
					ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)this.Row.Cells[index];
					if (toolStripPanelCell.Visible)
					{
						result = toolStripPanelCell.Grow(growBy);
					}
				}
				return result;
			}

			// Token: 0x06005BAF RID: 23471 RVA: 0x0014B650 File Offset: 0x0014A650
			public ToolStripPanelCell GetNextVisibleCell(int index, bool forward)
			{
				if (forward)
				{
					for (int i = index; i < this.Row.Cells.Count; i++)
					{
						ToolStripPanelCell toolStripPanelCell = this.Row.Cells[i] as ToolStripPanelCell;
						if ((toolStripPanelCell.Visible || (this.owner.parent.Visible && toolStripPanelCell.ControlInDesignMode)) && toolStripPanelCell.ToolStripPanelRow == this.owner)
						{
							return toolStripPanelCell;
						}
					}
				}
				else
				{
					for (int j = index; j >= 0; j--)
					{
						ToolStripPanelCell toolStripPanelCell2 = this.Row.Cells[j] as ToolStripPanelCell;
						if ((toolStripPanelCell2.Visible || (this.owner.parent.Visible && toolStripPanelCell2.ControlInDesignMode)) && toolStripPanelCell2.ToolStripPanelRow == this.owner)
						{
							return toolStripPanelCell2;
						}
					}
				}
				return null;
			}

			// Token: 0x06005BB0 RID: 23472 RVA: 0x0014B71C File Offset: 0x0014A71C
			protected virtual int GrowControlsAfter(int index, int growBy)
			{
				if (growBy < 0)
				{
					return 0;
				}
				int num = growBy;
				for (int i = index + 1; i < this.Row.ControlsInternal.Count; i++)
				{
					int num2 = this.Grow(i, num);
					if (num2 >= 0)
					{
						num -= num2;
						if (num <= 0)
						{
							return growBy;
						}
					}
				}
				return growBy - num;
			}

			// Token: 0x06005BB1 RID: 23473 RVA: 0x0014B768 File Offset: 0x0014A768
			protected virtual int GrowControlsBefore(int index, int growBy)
			{
				if (growBy < 0)
				{
					return 0;
				}
				int num = growBy;
				for (int i = index - 1; i >= 0; i--)
				{
					num -= this.Grow(i, num);
					if (num <= 0)
					{
						return growBy;
					}
				}
				return growBy - num;
			}

			// Token: 0x06005BB2 RID: 23474 RVA: 0x0014B79F File Offset: 0x0014A79F
			public virtual void MoveControl(ToolStrip movingControl, Point startClientLocation, Point endClientLocation)
			{
			}

			// Token: 0x06005BB3 RID: 23475 RVA: 0x0014B7A1 File Offset: 0x0014A7A1
			public virtual void LeaveRow(ToolStrip toolStripToDrag)
			{
			}

			// Token: 0x06005BB4 RID: 23476 RVA: 0x0014B7A3 File Offset: 0x0014A7A3
			public virtual void JoinRow(ToolStrip toolStripToDrag, Point locationToDrag)
			{
			}

			// Token: 0x06005BB5 RID: 23477 RVA: 0x0014B7A5 File Offset: 0x0014A7A5
			protected internal virtual void OnControlAdded(Control c, int index)
			{
			}

			// Token: 0x06005BB6 RID: 23478 RVA: 0x0014B7A7 File Offset: 0x0014A7A7
			protected internal virtual void OnControlRemoved(Control c, int index)
			{
			}

			// Token: 0x06005BB7 RID: 23479 RVA: 0x0014B7A9 File Offset: 0x0014A7A9
			protected internal virtual void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
			{
			}

			// Token: 0x040038FA RID: 14586
			private FlowLayoutSettings flowLayoutSettings;

			// Token: 0x040038FB RID: 14587
			private ToolStripPanelRow owner;
		}

		// Token: 0x020006CD RID: 1741
		private class HorizontalRowManager : ToolStripPanelRow.ToolStripPanelRowManager
		{
			// Token: 0x06005BB8 RID: 23480 RVA: 0x0014B7AB File Offset: 0x0014A7AB
			public HorizontalRowManager(ToolStripPanelRow owner) : base(owner)
			{
				owner.SuspendLayout();
				base.FlowLayoutSettings.WrapContents = false;
				base.FlowLayoutSettings.FlowDirection = FlowDirection.LeftToRight;
				owner.ResumeLayout(false);
			}

			// Token: 0x17001336 RID: 4918
			// (get) Token: 0x06005BB9 RID: 23481 RVA: 0x0014B7DC File Offset: 0x0014A7DC
			public override Rectangle DisplayRectangle
			{
				get
				{
					Rectangle displayRectangle = ((IArrangedElement)base.Row).DisplayRectangle;
					if (base.ToolStripPanel != null)
					{
						Rectangle displayRectangle2 = base.ToolStripPanel.DisplayRectangle;
						if ((!base.ToolStripPanel.Visible || LayoutUtils.IsZeroWidthOrHeight(displayRectangle2)) && base.ToolStripPanel.ParentInternal != null)
						{
							displayRectangle.Width = base.ToolStripPanel.ParentInternal.DisplayRectangle.Width - (base.ToolStripPanel.Margin.Horizontal + base.ToolStripPanel.Padding.Horizontal) - base.Row.Margin.Horizontal;
						}
						else
						{
							displayRectangle.Width = displayRectangle2.Width - base.Row.Margin.Horizontal;
						}
					}
					return displayRectangle;
				}
			}

			// Token: 0x17001337 RID: 4919
			// (get) Token: 0x06005BBA RID: 23482 RVA: 0x0014B8B4 File Offset: 0x0014A8B4
			public override Rectangle DragBounds
			{
				get
				{
					Rectangle bounds = base.Row.Bounds;
					int num = base.ToolStripPanel.RowsInternal.IndexOf(base.Row);
					if (num > 0)
					{
						Rectangle bounds2 = base.ToolStripPanel.RowsInternal[num - 1].Bounds;
						int num2 = bounds2.Y + bounds2.Height - (bounds2.Height >> 2);
						bounds.Height += bounds.Y - num2;
						bounds.Y = num2;
					}
					if (num < base.ToolStripPanel.RowsInternal.Count - 1)
					{
						Rectangle bounds3 = base.ToolStripPanel.RowsInternal[num + 1].Bounds;
						bounds.Height += (bounds3.Height >> 2) + base.Row.Margin.Bottom + base.ToolStripPanel.RowsInternal[num + 1].Margin.Top;
					}
					bounds.Width += base.Row.Margin.Horizontal + base.ToolStripPanel.Padding.Horizontal + 5;
					bounds.X -= base.Row.Margin.Left + base.ToolStripPanel.Padding.Left + 4;
					return bounds;
				}
			}

			// Token: 0x06005BBB RID: 23483 RVA: 0x0014BA2C File Offset: 0x0014AA2C
			public override bool CanMove(ToolStrip toolStripToDrag)
			{
				if (base.CanMove(toolStripToDrag))
				{
					Size sz = Size.Empty;
					for (int i = 0; i < base.Row.ControlsInternal.Count; i++)
					{
						sz += base.Row.GetMinimumSize(base.Row.ControlsInternal[i] as ToolStrip);
					}
					return (sz + base.Row.GetMinimumSize(toolStripToDrag)).Width < this.DisplayRectangle.Width;
				}
				return false;
			}

			// Token: 0x06005BBC RID: 23484 RVA: 0x0014BAB8 File Offset: 0x0014AAB8
			protected internal override int FreeSpaceFromRow(int spaceToFree)
			{
				int num = spaceToFree;
				if (spaceToFree > 0)
				{
					ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
					if (nextVisibleCell == null)
					{
						return 0;
					}
					Padding margin = nextVisibleCell.Margin;
					if (margin.Left >= spaceToFree)
					{
						margin.Left -= spaceToFree;
						margin.Right = 0;
						spaceToFree = 0;
					}
					else
					{
						spaceToFree -= nextVisibleCell.Margin.Left;
						margin.Left = 0;
						margin.Right = 0;
					}
					nextVisibleCell.Margin = margin;
					spaceToFree -= this.MoveLeft(base.Row.Cells.Count - 1, spaceToFree);
					if (spaceToFree > 0)
					{
						spaceToFree -= nextVisibleCell.Shrink(spaceToFree);
					}
				}
				return num - Math.Max(0, spaceToFree);
			}

			// Token: 0x06005BBD RID: 23485 RVA: 0x0014BB78 File Offset: 0x0014AB78
			public override void MoveControl(ToolStrip movingControl, Point clientStartLocation, Point clientEndLocation)
			{
				if (base.Row.Locked)
				{
					return;
				}
				if (!this.DragBounds.Contains(clientEndLocation))
				{
					base.MoveControl(movingControl, clientStartLocation, clientEndLocation);
					return;
				}
				int index = base.Row.ControlsInternal.IndexOf(movingControl);
				int num = clientEndLocation.X - clientStartLocation.X;
				if (num < 0)
				{
					this.MoveLeft(index, num * -1);
					return;
				}
				this.MoveRight(index, num);
			}

			// Token: 0x06005BBE RID: 23486 RVA: 0x0014BBEC File Offset: 0x0014ABEC
			private int MoveLeft(int index, int spaceToFree)
			{
				int num = 0;
				base.Row.SuspendLayout();
				try
				{
					if (spaceToFree == 0 || index < 0)
					{
						return 0;
					}
					for (int i = index; i >= 0; i--)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[i];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int num2 = spaceToFree - num;
							Padding margin = toolStripPanelCell.Margin;
							if (margin.Horizontal >= num2)
							{
								num += num2;
								margin.Left -= num2;
								margin.Right = 0;
								toolStripPanelCell.Margin = margin;
							}
							else
							{
								num += toolStripPanelCell.Margin.Horizontal;
								margin.Left = 0;
								margin.Right = 0;
								toolStripPanelCell.Margin = margin;
							}
							if (num >= spaceToFree)
							{
								if (index + 1 < base.Row.Cells.Count)
								{
									toolStripPanelCell = base.GetNextVisibleCell(index + 1, true);
									if (toolStripPanelCell != null)
									{
										margin = toolStripPanelCell.Margin;
										margin.Left += spaceToFree;
										toolStripPanelCell.Margin = margin;
									}
								}
								return spaceToFree;
							}
						}
					}
				}
				finally
				{
					base.Row.ResumeLayout(true);
				}
				return num;
			}

			// Token: 0x06005BBF RID: 23487 RVA: 0x0014BD24 File Offset: 0x0014AD24
			private int MoveRight(int index, int spaceToFree)
			{
				int num = 0;
				base.Row.SuspendLayout();
				try
				{
					if (spaceToFree == 0 || index < 0 || index >= base.Row.ControlsInternal.Count)
					{
						return 0;
					}
					int i = index + 1;
					while (i < base.Row.Cells.Count)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[i];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int num2 = spaceToFree - num;
							Padding margin = toolStripPanelCell.Margin;
							if (margin.Horizontal >= num2)
							{
								num += num2;
								margin.Left -= num2;
								margin.Right = 0;
								toolStripPanelCell.Margin = margin;
								break;
							}
							num += toolStripPanelCell.Margin.Horizontal;
							margin.Left = 0;
							margin.Right = 0;
							toolStripPanelCell.Margin = margin;
							break;
						}
						else
						{
							i++;
						}
					}
					if (base.Row.Cells.Count > 0 && spaceToFree > num)
					{
						ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
						if (nextVisibleCell != null)
						{
							num += this.DisplayRectangle.Right - nextVisibleCell.Bounds.Right;
						}
						else
						{
							num += this.DisplayRectangle.Width;
						}
					}
					if (spaceToFree <= num)
					{
						ToolStripPanelCell toolStripPanelCell = base.GetNextVisibleCell(index, true);
						if (toolStripPanelCell == null)
						{
							toolStripPanelCell = (base.Row.Cells[index] as ToolStripPanelCell);
						}
						if (toolStripPanelCell != null)
						{
							Padding margin = toolStripPanelCell.Margin;
							margin.Left += spaceToFree;
							toolStripPanelCell.Margin = margin;
						}
						return spaceToFree;
					}
					for (int j = index + 1; j < base.Row.Cells.Count; j++)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[j];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int shrinkBy = spaceToFree - num;
							num += toolStripPanelCell.Shrink(shrinkBy);
							if (spaceToFree >= num)
							{
								base.Row.ResumeLayout(true);
								return spaceToFree;
							}
						}
					}
					if (base.Row.Cells.Count == 1)
					{
						ToolStripPanelCell toolStripPanelCell = base.GetNextVisibleCell(index, true);
						if (toolStripPanelCell != null)
						{
							Padding margin = toolStripPanelCell.Margin;
							margin.Left += num;
							toolStripPanelCell.Margin = margin;
						}
					}
				}
				finally
				{
					base.Row.ResumeLayout(true);
				}
				return num;
			}

			// Token: 0x06005BC0 RID: 23488 RVA: 0x0014BFA8 File Offset: 0x0014AFA8
			public override void LeaveRow(ToolStrip toolStripToDrag)
			{
				base.Row.SuspendLayout();
				int num = base.Row.ControlsInternal.IndexOf(toolStripToDrag);
				if (num >= 0)
				{
					if (num < base.Row.ControlsInternal.Count - 1)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[num];
						if (toolStripPanelCell.Visible)
						{
							int num2 = toolStripPanelCell.Margin.Horizontal + toolStripPanelCell.Bounds.Width;
							ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(num + 1, true);
							if (nextVisibleCell != null)
							{
								Padding margin = nextVisibleCell.Margin;
								margin.Left += num2;
								nextVisibleCell.Margin = margin;
							}
						}
					}
					((IList)base.Row.Cells).RemoveAt(num);
				}
				base.Row.ResumeLayout(true);
			}

			// Token: 0x06005BC1 RID: 23489 RVA: 0x0014C077 File Offset: 0x0014B077
			protected internal override void OnControlAdded(Control control, int index)
			{
			}

			// Token: 0x06005BC2 RID: 23490 RVA: 0x0014C079 File Offset: 0x0014B079
			protected internal override void OnControlRemoved(Control control, int index)
			{
			}

			// Token: 0x06005BC3 RID: 23491 RVA: 0x0014C07C File Offset: 0x0014B07C
			public override void JoinRow(ToolStrip toolStripToDrag, Point locationToDrag)
			{
				if (!base.Row.ControlsInternal.Contains(toolStripToDrag))
				{
					base.Row.SuspendLayout();
					try
					{
						if (base.Row.ControlsInternal.Count > 0)
						{
							int i;
							for (i = 0; i < base.Row.Cells.Count; i++)
							{
								ToolStripPanelCell toolStripPanelCell = base.Row.Cells[i] as ToolStripPanelCell;
								if ((toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode) && (base.Row.Cells[i].Bounds.Contains(locationToDrag) || base.Row.Cells[i].Bounds.X >= locationToDrag.X))
								{
									break;
								}
							}
							Control control = base.Row.ControlsInternal[i];
							if (i < base.Row.ControlsInternal.Count)
							{
								base.Row.ControlsInternal.Insert(i, toolStripToDrag);
							}
							else
							{
								base.Row.ControlsInternal.Add(toolStripToDrag);
							}
							int num = toolStripToDrag.AutoSize ? toolStripToDrag.PreferredSize.Width : toolStripToDrag.Width;
							int num2 = num;
							if (i == 0)
							{
								num2 += locationToDrag.X;
							}
							int num3 = 0;
							if (i < base.Row.ControlsInternal.Count - 1)
							{
								ToolStripPanelCell toolStripPanelCell2 = (ToolStripPanelCell)base.Row.Cells[i + 1];
								Padding margin = toolStripPanelCell2.Margin;
								if (margin.Left > num2)
								{
									margin.Left -= num2;
									toolStripPanelCell2.Margin = margin;
									num3 = num2;
								}
								else
								{
									num3 = this.MoveRight(i + 1, num2 - num3);
									if (num3 > 0)
									{
										margin = toolStripPanelCell2.Margin;
										margin.Left = Math.Max(0, margin.Left - num3);
										toolStripPanelCell2.Margin = margin;
									}
								}
							}
							else
							{
								ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 2, false);
								ToolStripPanelCell nextVisibleCell2 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
								if (nextVisibleCell != null && nextVisibleCell2 != null)
								{
									Padding margin2 = nextVisibleCell2.Margin;
									margin2.Left = Math.Max(0, locationToDrag.X - nextVisibleCell.Bounds.Right);
									nextVisibleCell2.Margin = margin2;
									num3 = num2;
								}
							}
							if (num3 < num2 && i > 0)
							{
								num3 = this.MoveLeft(i - 1, num2 - num3);
							}
							if (i == 0 && num3 - num > 0)
							{
								ToolStripPanelCell toolStripPanelCell3 = base.Row.Cells[i] as ToolStripPanelCell;
								Padding margin3 = toolStripPanelCell3.Margin;
								margin3.Left = num3 - num;
								toolStripPanelCell3.Margin = margin3;
							}
						}
						else
						{
							base.Row.ControlsInternal.Add(toolStripToDrag);
							if (base.Row.Cells.Count > 0 || toolStripToDrag.IsInDesignMode)
							{
								ToolStripPanelCell toolStripPanelCell4 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
								if (toolStripPanelCell4 == null && toolStripToDrag.IsInDesignMode)
								{
									toolStripPanelCell4 = (ToolStripPanelCell)base.Row.Cells[base.Row.Cells.Count - 1];
								}
								if (toolStripPanelCell4 != null)
								{
									Padding margin4 = toolStripPanelCell4.Margin;
									margin4.Left = Math.Max(0, locationToDrag.X - base.Row.Margin.Left);
									toolStripPanelCell4.Margin = margin4;
								}
							}
						}
					}
					finally
					{
						base.Row.ResumeLayout(true);
					}
				}
			}

			// Token: 0x06005BC4 RID: 23492 RVA: 0x0014C438 File Offset: 0x0014B438
			protected internal override void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
			{
				base.OnBoundsChanged(oldBounds, newBounds);
			}

			// Token: 0x040038FC RID: 14588
			private const int DRAG_BOUNDS_INFLATE = 4;
		}

		// Token: 0x020006CE RID: 1742
		private class VerticalRowManager : ToolStripPanelRow.ToolStripPanelRowManager
		{
			// Token: 0x06005BC5 RID: 23493 RVA: 0x0014C442 File Offset: 0x0014B442
			public VerticalRowManager(ToolStripPanelRow owner) : base(owner)
			{
				owner.SuspendLayout();
				base.FlowLayoutSettings.WrapContents = false;
				base.FlowLayoutSettings.FlowDirection = FlowDirection.TopDown;
				owner.ResumeLayout(false);
			}

			// Token: 0x17001338 RID: 4920
			// (get) Token: 0x06005BC6 RID: 23494 RVA: 0x0014C470 File Offset: 0x0014B470
			public override Rectangle DisplayRectangle
			{
				get
				{
					Rectangle displayRectangle = ((IArrangedElement)base.Row).DisplayRectangle;
					if (base.ToolStripPanel != null)
					{
						Rectangle displayRectangle2 = base.ToolStripPanel.DisplayRectangle;
						if ((!base.ToolStripPanel.Visible || LayoutUtils.IsZeroWidthOrHeight(displayRectangle2)) && base.ToolStripPanel.ParentInternal != null)
						{
							displayRectangle.Height = base.ToolStripPanel.ParentInternal.DisplayRectangle.Height - (base.ToolStripPanel.Margin.Vertical + base.ToolStripPanel.Padding.Vertical) - base.Row.Margin.Vertical;
						}
						else
						{
							displayRectangle.Height = displayRectangle2.Height - base.Row.Margin.Vertical;
						}
					}
					return displayRectangle;
				}
			}

			// Token: 0x17001339 RID: 4921
			// (get) Token: 0x06005BC7 RID: 23495 RVA: 0x0014C548 File Offset: 0x0014B548
			public override Rectangle DragBounds
			{
				get
				{
					Rectangle bounds = base.Row.Bounds;
					int num = base.ToolStripPanel.RowsInternal.IndexOf(base.Row);
					if (num > 0)
					{
						Rectangle bounds2 = base.ToolStripPanel.RowsInternal[num - 1].Bounds;
						int num2 = bounds2.X + bounds2.Width - (bounds2.Width >> 2);
						bounds.Width += bounds.X - num2;
						bounds.X = num2;
					}
					if (num < base.ToolStripPanel.RowsInternal.Count - 1)
					{
						Rectangle bounds3 = base.ToolStripPanel.RowsInternal[num + 1].Bounds;
						bounds.Width += (bounds3.Width >> 2) + base.Row.Margin.Right + base.ToolStripPanel.RowsInternal[num + 1].Margin.Left;
					}
					bounds.Height += base.Row.Margin.Vertical + base.ToolStripPanel.Padding.Vertical + 5;
					bounds.Y -= base.Row.Margin.Top + base.ToolStripPanel.Padding.Top + 4;
					return bounds;
				}
			}

			// Token: 0x06005BC8 RID: 23496 RVA: 0x0014C6C0 File Offset: 0x0014B6C0
			public override bool CanMove(ToolStrip toolStripToDrag)
			{
				if (base.CanMove(toolStripToDrag))
				{
					Size sz = Size.Empty;
					for (int i = 0; i < base.Row.ControlsInternal.Count; i++)
					{
						sz += base.Row.GetMinimumSize(base.Row.ControlsInternal[i] as ToolStrip);
					}
					return (sz + base.Row.GetMinimumSize(toolStripToDrag)).Height < this.DisplayRectangle.Height;
				}
				return false;
			}

			// Token: 0x06005BC9 RID: 23497 RVA: 0x0014C74C File Offset: 0x0014B74C
			protected internal override int FreeSpaceFromRow(int spaceToFree)
			{
				int num = spaceToFree;
				if (spaceToFree > 0)
				{
					ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
					if (nextVisibleCell == null)
					{
						return 0;
					}
					Padding margin = nextVisibleCell.Margin;
					if (margin.Top >= spaceToFree)
					{
						margin.Top -= spaceToFree;
						margin.Bottom = 0;
						spaceToFree = 0;
					}
					else
					{
						spaceToFree -= nextVisibleCell.Margin.Top;
						margin.Top = 0;
						margin.Bottom = 0;
					}
					nextVisibleCell.Margin = margin;
					spaceToFree -= this.MoveUp(base.Row.Cells.Count - 1, spaceToFree);
					if (spaceToFree > 0)
					{
						spaceToFree -= nextVisibleCell.Shrink(spaceToFree);
					}
				}
				return num - Math.Max(0, spaceToFree);
			}

			// Token: 0x06005BCA RID: 23498 RVA: 0x0014C80C File Offset: 0x0014B80C
			public override void MoveControl(ToolStrip movingControl, Point clientStartLocation, Point clientEndLocation)
			{
				if (base.Row.Locked)
				{
					return;
				}
				if (!this.DragBounds.Contains(clientEndLocation))
				{
					base.MoveControl(movingControl, clientStartLocation, clientEndLocation);
					return;
				}
				int index = base.Row.ControlsInternal.IndexOf(movingControl);
				int num = clientEndLocation.Y - clientStartLocation.Y;
				if (num < 0)
				{
					this.MoveUp(index, num * -1);
					return;
				}
				this.MoveDown(index, num);
			}

			// Token: 0x06005BCB RID: 23499 RVA: 0x0014C880 File Offset: 0x0014B880
			private int MoveUp(int index, int spaceToFree)
			{
				int num = 0;
				base.Row.SuspendLayout();
				try
				{
					if (spaceToFree == 0 || index < 0)
					{
						return 0;
					}
					for (int i = index; i >= 0; i--)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[i];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int num2 = spaceToFree - num;
							Padding margin = toolStripPanelCell.Margin;
							if (margin.Vertical >= num2)
							{
								num += num2;
								margin.Top -= num2;
								margin.Bottom = 0;
								toolStripPanelCell.Margin = margin;
							}
							else
							{
								num += toolStripPanelCell.Margin.Vertical;
								margin.Top = 0;
								margin.Bottom = 0;
								toolStripPanelCell.Margin = margin;
							}
							if (num >= spaceToFree)
							{
								if (index + 1 < base.Row.Cells.Count)
								{
									toolStripPanelCell = base.GetNextVisibleCell(index + 1, true);
									if (toolStripPanelCell != null)
									{
										margin = toolStripPanelCell.Margin;
										margin.Top += spaceToFree;
										toolStripPanelCell.Margin = margin;
									}
								}
								return spaceToFree;
							}
						}
					}
				}
				finally
				{
					base.Row.ResumeLayout(true);
				}
				return num;
			}

			// Token: 0x06005BCC RID: 23500 RVA: 0x0014C9B8 File Offset: 0x0014B9B8
			private int MoveDown(int index, int spaceToFree)
			{
				int num = 0;
				base.Row.SuspendLayout();
				try
				{
					if (spaceToFree == 0 || index < 0 || index >= base.Row.ControlsInternal.Count)
					{
						return 0;
					}
					int i = index + 1;
					while (i < base.Row.Cells.Count)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[i];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int num2 = spaceToFree - num;
							Padding margin = toolStripPanelCell.Margin;
							if (margin.Vertical >= num2)
							{
								num += num2;
								margin.Top -= num2;
								margin.Bottom = 0;
								toolStripPanelCell.Margin = margin;
								break;
							}
							num += toolStripPanelCell.Margin.Vertical;
							margin.Top = 0;
							margin.Bottom = 0;
							toolStripPanelCell.Margin = margin;
							break;
						}
						else
						{
							i++;
						}
					}
					if (base.Row.Cells.Count > 0 && spaceToFree > num)
					{
						ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
						if (nextVisibleCell != null)
						{
							num += this.DisplayRectangle.Bottom - nextVisibleCell.Bounds.Bottom;
						}
						else
						{
							num += this.DisplayRectangle.Height;
						}
					}
					if (spaceToFree <= num)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[index];
						Padding margin = toolStripPanelCell.Margin;
						margin.Top += spaceToFree;
						toolStripPanelCell.Margin = margin;
						return spaceToFree;
					}
					for (int j = index + 1; j < base.Row.Cells.Count; j++)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[j];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int shrinkBy = spaceToFree - num;
							num += toolStripPanelCell.Shrink(shrinkBy);
							if (spaceToFree >= num)
							{
								base.Row.ResumeLayout(true);
								return spaceToFree;
							}
						}
					}
					if (base.Row.Cells.Count == 1)
					{
						ToolStripPanelCell toolStripPanelCell = base.GetNextVisibleCell(index, true);
						if (toolStripPanelCell != null)
						{
							Padding margin = toolStripPanelCell.Margin;
							margin.Top += num;
							toolStripPanelCell.Margin = margin;
						}
					}
				}
				finally
				{
					base.Row.ResumeLayout(true);
				}
				return spaceToFree - num;
			}

			// Token: 0x06005BCD RID: 23501 RVA: 0x0014CC34 File Offset: 0x0014BC34
			protected internal override void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
			{
				base.OnBoundsChanged(oldBounds, newBounds);
				if (base.Row.Cells.Count > 0)
				{
					ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
					int num = (nextVisibleCell != null) ? (nextVisibleCell.Bounds.Bottom - newBounds.Height) : 0;
					if (num > 0)
					{
						ToolStripPanelCell nextVisibleCell2 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
						Padding margin = nextVisibleCell2.Margin;
						if (margin.Top >= num)
						{
							margin.Top -= num;
							margin.Bottom = 0;
							nextVisibleCell2.Margin = margin;
							num = 0;
						}
						else
						{
							num -= nextVisibleCell2.Margin.Top;
							margin.Top = 0;
							margin.Bottom = 0;
							nextVisibleCell2.Margin = margin;
						}
						num -= nextVisibleCell2.Shrink(num);
						this.MoveUp(base.Row.Cells.Count - 1, num);
					}
				}
			}

			// Token: 0x06005BCE RID: 23502 RVA: 0x0014CD38 File Offset: 0x0014BD38
			protected internal override void OnControlRemoved(Control c, int index)
			{
			}

			// Token: 0x06005BCF RID: 23503 RVA: 0x0014CD3A File Offset: 0x0014BD3A
			protected internal override void OnControlAdded(Control control, int index)
			{
			}

			// Token: 0x06005BD0 RID: 23504 RVA: 0x0014CD3C File Offset: 0x0014BD3C
			public override void JoinRow(ToolStrip toolStripToDrag, Point locationToDrag)
			{
				if (!base.Row.ControlsInternal.Contains(toolStripToDrag))
				{
					base.Row.SuspendLayout();
					try
					{
						if (base.Row.ControlsInternal.Count > 0)
						{
							int i;
							for (i = 0; i < base.Row.Cells.Count; i++)
							{
								ToolStripPanelCell toolStripPanelCell = base.Row.Cells[i] as ToolStripPanelCell;
								if ((toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode) && (toolStripPanelCell.Bounds.Contains(locationToDrag) || toolStripPanelCell.Bounds.Y >= locationToDrag.Y))
								{
									break;
								}
							}
							Control control = base.Row.ControlsInternal[i];
							if (i < base.Row.ControlsInternal.Count)
							{
								base.Row.ControlsInternal.Insert(i, toolStripToDrag);
							}
							else
							{
								base.Row.ControlsInternal.Add(toolStripToDrag);
							}
							int num = toolStripToDrag.AutoSize ? toolStripToDrag.PreferredSize.Height : toolStripToDrag.Height;
							int num2 = num;
							if (i == 0)
							{
								num2 += locationToDrag.Y;
							}
							int num3 = 0;
							if (i < base.Row.ControlsInternal.Count - 1)
							{
								ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(i + 1, true);
								if (nextVisibleCell != null)
								{
									Padding margin = nextVisibleCell.Margin;
									if (margin.Top > num2)
									{
										margin.Top -= num2;
										nextVisibleCell.Margin = margin;
										num3 = num2;
									}
									else
									{
										num3 = this.MoveDown(i + 1, num2 - num3);
										if (num3 > 0)
										{
											margin = nextVisibleCell.Margin;
											margin.Top -= num3;
											nextVisibleCell.Margin = margin;
										}
									}
								}
							}
							else
							{
								ToolStripPanelCell nextVisibleCell2 = base.GetNextVisibleCell(base.Row.Cells.Count - 2, false);
								ToolStripPanelCell nextVisibleCell3 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
								if (nextVisibleCell2 != null && nextVisibleCell3 != null)
								{
									Padding margin2 = nextVisibleCell3.Margin;
									margin2.Top = Math.Max(0, locationToDrag.Y - nextVisibleCell2.Bounds.Bottom);
									nextVisibleCell3.Margin = margin2;
									num3 = num2;
								}
							}
							if (num3 < num2 && i > 0)
							{
								num3 = this.MoveUp(i - 1, num2 - num3);
							}
							if (i == 0 && num3 - num > 0)
							{
								ToolStripPanelCell toolStripPanelCell2 = base.Row.Cells[i] as ToolStripPanelCell;
								Padding margin3 = toolStripPanelCell2.Margin;
								margin3.Top = num3 - num;
								toolStripPanelCell2.Margin = margin3;
							}
						}
						else
						{
							base.Row.ControlsInternal.Add(toolStripToDrag);
							if (base.Row.Cells.Count > 0)
							{
								ToolStripPanelCell nextVisibleCell4 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
								if (nextVisibleCell4 != null)
								{
									Padding margin4 = nextVisibleCell4.Margin;
									margin4.Top = Math.Max(0, locationToDrag.Y - base.Row.Margin.Top);
									nextVisibleCell4.Margin = margin4;
								}
							}
						}
					}
					finally
					{
						base.Row.ResumeLayout(true);
					}
				}
			}

			// Token: 0x06005BD1 RID: 23505 RVA: 0x0014D084 File Offset: 0x0014C084
			public override void LeaveRow(ToolStrip toolStripToDrag)
			{
				base.Row.SuspendLayout();
				int num = base.Row.ControlsInternal.IndexOf(toolStripToDrag);
				if (num >= 0)
				{
					if (num < base.Row.ControlsInternal.Count - 1)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[num];
						if (toolStripPanelCell.Visible)
						{
							int num2 = toolStripPanelCell.Margin.Vertical + toolStripPanelCell.Bounds.Height;
							ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(num + 1, true);
							if (nextVisibleCell != null)
							{
								Padding margin = nextVisibleCell.Margin;
								margin.Top += num2;
								nextVisibleCell.Margin = margin;
							}
						}
					}
					((IList)base.Row.Cells).RemoveAt(num);
				}
				base.Row.ResumeLayout(true);
			}

			// Token: 0x040038FD RID: 14589
			private const int DRAG_BOUNDS_INFLATE = 4;
		}

		// Token: 0x020006CF RID: 1743
		internal class ToolStripPanelRowControlCollection : ArrangedElementCollection, IList, ICollection, IEnumerable
		{
			// Token: 0x06005BD2 RID: 23506 RVA: 0x0014D153 File Offset: 0x0014C153
			public ToolStripPanelRowControlCollection(ToolStripPanelRow owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005BD3 RID: 23507 RVA: 0x0014D162 File Offset: 0x0014C162
			public ToolStripPanelRowControlCollection(ToolStripPanelRow owner, Control[] value)
			{
				this.owner = owner;
				this.AddRange(value);
			}

			// Token: 0x1700133A RID: 4922
			public virtual Control this[int index]
			{
				get
				{
					return this.GetControl(index);
				}
			}

			// Token: 0x1700133B RID: 4923
			// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x0014D181 File Offset: 0x0014C181
			public ArrangedElementCollection Cells
			{
				get
				{
					if (this.cellCollection == null)
					{
						this.cellCollection = new ArrangedElementCollection(base.InnerList);
					}
					return this.cellCollection;
				}
			}

			// Token: 0x1700133C RID: 4924
			// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x0014D1A2 File Offset: 0x0014C1A2
			public ToolStripPanel ToolStripPanel
			{
				get
				{
					return this.owner.ToolStripPanel;
				}
			}

			// Token: 0x06005BD7 RID: 23511 RVA: 0x0014D1B0 File Offset: 0x0014C1B0
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int Add(Control value)
			{
				ISupportToolStripPanel supportToolStripPanel = value as ISupportToolStripPanel;
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (supportToolStripPanel == null)
				{
					throw new NotSupportedException(SR.GetString("TypedControlCollectionShouldBeOfType", new object[]
					{
						typeof(ToolStrip).Name
					}));
				}
				int num = base.InnerList.Add(supportToolStripPanel.ToolStripPanelCell);
				this.OnAdd(supportToolStripPanel, num);
				return num;
			}

			// Token: 0x06005BD8 RID: 23512 RVA: 0x0014D21C File Offset: 0x0014C21C
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void AddRange(Control[] value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ToolStripPanel toolStripPanel = this.ToolStripPanel;
				if (toolStripPanel != null)
				{
					toolStripPanel.SuspendLayout();
				}
				try
				{
					for (int i = 0; i < value.Length; i++)
					{
						this.Add(value[i]);
					}
				}
				finally
				{
					if (toolStripPanel != null)
					{
						toolStripPanel.ResumeLayout();
					}
				}
			}

			// Token: 0x06005BD9 RID: 23513 RVA: 0x0014D27C File Offset: 0x0014C27C
			public bool Contains(Control value)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.GetControl(i) == value)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06005BDA RID: 23514 RVA: 0x0014D2A8 File Offset: 0x0014C2A8
			public virtual void Clear()
			{
				if (this.owner != null)
				{
					this.ToolStripPanel.SuspendLayout();
				}
				try
				{
					while (this.Count != 0)
					{
						this.RemoveAt(this.Count - 1);
					}
				}
				finally
				{
					if (this.owner != null)
					{
						this.ToolStripPanel.ResumeLayout();
					}
				}
			}

			// Token: 0x06005BDB RID: 23515 RVA: 0x0014D308 File Offset: 0x0014C308
			public override IEnumerator GetEnumerator()
			{
				return new ToolStripPanelRow.ToolStripPanelRowControlCollection.ToolStripPanelCellToControlEnumerator(base.InnerList);
			}

			// Token: 0x06005BDC RID: 23516 RVA: 0x0014D318 File Offset: 0x0014C318
			private Control GetControl(int index)
			{
				Control result = null;
				if (index < this.Count && index >= 0)
				{
					ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.InnerList[index];
					result = ((toolStripPanelCell != null) ? toolStripPanelCell.Control : null);
				}
				return result;
			}

			// Token: 0x06005BDD RID: 23517 RVA: 0x0014D358 File Offset: 0x0014C358
			private int IndexOfControl(Control c)
			{
				for (int i = 0; i < this.Count; i++)
				{
					ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.InnerList[i];
					if (toolStripPanelCell.Control == c)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06005BDE RID: 23518 RVA: 0x0014D394 File Offset: 0x0014C394
			void IList.Clear()
			{
				this.Clear();
			}

			// Token: 0x1700133D RID: 4925
			// (get) Token: 0x06005BDF RID: 23519 RVA: 0x0014D39C File Offset: 0x0014C39C
			bool IList.IsFixedSize
			{
				get
				{
					return base.InnerList.IsFixedSize;
				}
			}

			// Token: 0x06005BE0 RID: 23520 RVA: 0x0014D3A9 File Offset: 0x0014C3A9
			bool IList.Contains(object value)
			{
				return base.InnerList.Contains(value);
			}

			// Token: 0x1700133E RID: 4926
			// (get) Token: 0x06005BE1 RID: 23521 RVA: 0x0014D3B7 File Offset: 0x0014C3B7
			bool IList.IsReadOnly
			{
				get
				{
					return base.InnerList.IsReadOnly;
				}
			}

			// Token: 0x06005BE2 RID: 23522 RVA: 0x0014D3C4 File Offset: 0x0014C3C4
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			// Token: 0x06005BE3 RID: 23523 RVA: 0x0014D3CD File Offset: 0x0014C3CD
			void IList.Remove(object value)
			{
				this.Remove(value as Control);
			}

			// Token: 0x06005BE4 RID: 23524 RVA: 0x0014D3DB File Offset: 0x0014C3DB
			int IList.Add(object value)
			{
				return this.Add(value as Control);
			}

			// Token: 0x06005BE5 RID: 23525 RVA: 0x0014D3E9 File Offset: 0x0014C3E9
			int IList.IndexOf(object value)
			{
				return this.IndexOf(value as Control);
			}

			// Token: 0x06005BE6 RID: 23526 RVA: 0x0014D3F7 File Offset: 0x0014C3F7
			void IList.Insert(int index, object value)
			{
				this.Insert(index, value as Control);
			}

			// Token: 0x06005BE7 RID: 23527 RVA: 0x0014D408 File Offset: 0x0014C408
			public int IndexOf(Control value)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.GetControl(i) == value)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06005BE8 RID: 23528 RVA: 0x0014D434 File Offset: 0x0014C434
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void Insert(int index, Control value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ISupportToolStripPanel supportToolStripPanel = value as ISupportToolStripPanel;
				if (supportToolStripPanel == null)
				{
					throw new NotSupportedException(SR.GetString("TypedControlCollectionShouldBeOfType", new object[]
					{
						typeof(ToolStrip).Name
					}));
				}
				base.InnerList.Insert(index, supportToolStripPanel.ToolStripPanelCell);
				this.OnAdd(supportToolStripPanel, index);
			}

			// Token: 0x06005BE9 RID: 23529 RVA: 0x0014D4A0 File Offset: 0x0014C4A0
			private void OnAfterRemove(Control control, int index)
			{
				if (this.owner != null)
				{
					using (new LayoutTransaction(this.ToolStripPanel, control, PropertyNames.Parent))
					{
						this.owner.ToolStripPanel.Controls.Remove(control);
						this.owner.OnControlRemoved(control, index);
					}
				}
			}

			// Token: 0x06005BEA RID: 23530 RVA: 0x0014D508 File Offset: 0x0014C508
			private void OnAdd(ISupportToolStripPanel controlToBeDragged, int index)
			{
				if (this.owner != null)
				{
					LayoutTransaction layoutTransaction = null;
					if (this.ToolStripPanel != null && this.ToolStripPanel.ParentInternal != null)
					{
						layoutTransaction = new LayoutTransaction(this.ToolStripPanel, this.ToolStripPanel.ParentInternal, PropertyNames.Parent);
					}
					try
					{
						if (controlToBeDragged != null)
						{
							controlToBeDragged.ToolStripPanelRow = this.owner;
							Control control = controlToBeDragged as Control;
							if (control != null)
							{
								control.ParentInternal = this.owner.ToolStripPanel;
								this.owner.OnControlAdded(control, index);
							}
						}
					}
					finally
					{
						if (layoutTransaction != null)
						{
							layoutTransaction.Dispose();
						}
					}
				}
			}

			// Token: 0x06005BEB RID: 23531 RVA: 0x0014D5A4 File Offset: 0x0014C5A4
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void Remove(Control value)
			{
				int index = this.IndexOfControl(value);
				this.RemoveAt(index);
			}

			// Token: 0x06005BEC RID: 23532 RVA: 0x0014D5C0 File Offset: 0x0014C5C0
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void RemoveAt(int index)
			{
				if (index >= 0 && index < this.Count)
				{
					Control control = this.GetControl(index);
					object obj = base.InnerList[index];
					base.InnerList.RemoveAt(index);
					this.OnAfterRemove(control, index);
				}
			}

			// Token: 0x06005BED RID: 23533 RVA: 0x0014D604 File Offset: 0x0014C604
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void CopyTo(Control[] array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index >= array.Length || base.InnerList.Count > array.Length - index)
				{
					throw new ArgumentException(SR.GetString("ToolStripPanelRowControlCollectionIncorrectIndexLength"));
				}
				for (int i = 0; i < base.InnerList.Count; i++)
				{
					array[index++] = this.GetControl(i);
				}
			}

			// Token: 0x040038FE RID: 14590
			private ToolStripPanelRow owner;

			// Token: 0x040038FF RID: 14591
			private ArrangedElementCollection cellCollection;

			// Token: 0x020006D0 RID: 1744
			private class ToolStripPanelCellToControlEnumerator : IEnumerator, ICloneable
			{
				// Token: 0x06005BEE RID: 23534 RVA: 0x0014D67B File Offset: 0x0014C67B
				internal ToolStripPanelCellToControlEnumerator(ArrayList list)
				{
					this.arrayListEnumerator = ((IEnumerable)list).GetEnumerator();
				}

				// Token: 0x1700133F RID: 4927
				// (get) Token: 0x06005BEF RID: 23535 RVA: 0x0014D690 File Offset: 0x0014C690
				public virtual object Current
				{
					get
					{
						ToolStripPanelCell toolStripPanelCell = this.arrayListEnumerator.Current as ToolStripPanelCell;
						if (toolStripPanelCell != null)
						{
							return toolStripPanelCell.Control;
						}
						return null;
					}
				}

				// Token: 0x06005BF0 RID: 23536 RVA: 0x0014D6B9 File Offset: 0x0014C6B9
				public object Clone()
				{
					return base.MemberwiseClone();
				}

				// Token: 0x06005BF1 RID: 23537 RVA: 0x0014D6C1 File Offset: 0x0014C6C1
				public virtual bool MoveNext()
				{
					return this.arrayListEnumerator.MoveNext();
				}

				// Token: 0x06005BF2 RID: 23538 RVA: 0x0014D6CE File Offset: 0x0014C6CE
				public virtual void Reset()
				{
					this.arrayListEnumerator.Reset();
				}

				// Token: 0x04003900 RID: 14592
				private IEnumerator arrayListEnumerator;
			}
		}
	}
}
