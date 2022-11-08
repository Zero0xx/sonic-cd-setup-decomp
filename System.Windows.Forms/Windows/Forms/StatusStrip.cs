using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000633 RID: 1587
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionStatusStrip")]
	public class StatusStrip : ToolStrip
	{
		// Token: 0x06005305 RID: 21253 RVA: 0x0012FA58 File Offset: 0x0012EA58
		public StatusStrip()
		{
			base.SuspendLayout();
			this.CanOverflow = false;
			this.LayoutStyle = ToolStripLayoutStyle.Table;
			base.RenderMode = ToolStripRenderMode.System;
			this.GripStyle = ToolStripGripStyle.Hidden;
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			this.Stretch = true;
			this.state[StatusStrip.stateSizingGrip] = true;
			base.ResumeLayout(true);
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x06005306 RID: 21254 RVA: 0x0012FAC1 File Offset: 0x0012EAC1
		// (set) Token: 0x06005307 RID: 21255 RVA: 0x0012FAC9 File Offset: 0x0012EAC9
		[Browsable(false)]
		[SRDescription("ToolStripCanOverflowDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		public new bool CanOverflow
		{
			get
			{
				return base.CanOverflow;
			}
			set
			{
				base.CanOverflow = value;
			}
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x06005308 RID: 21256 RVA: 0x0012FAD2 File Offset: 0x0012EAD2
		protected override bool DefaultShowItemToolTips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x06005309 RID: 21257 RVA: 0x0012FAD5 File Offset: 0x0012EAD5
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 22);
			}
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x0600530A RID: 21258 RVA: 0x0012FAE4 File Offset: 0x0012EAE4
		protected override Padding DefaultPadding
		{
			get
			{
				if (base.Orientation != Orientation.Horizontal)
				{
					return new Padding(1, 3, 1, this.DefaultSize.Height);
				}
				if (this.RightToLeft == RightToLeft.No)
				{
					return new Padding(1, 0, 14, 0);
				}
				return new Padding(14, 0, 1, 0);
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x0600530B RID: 21259 RVA: 0x0012FB2D File Offset: 0x0012EB2D
		protected override DockStyle DefaultDock
		{
			get
			{
				return DockStyle.Bottom;
			}
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x0600530C RID: 21260 RVA: 0x0012FB30 File Offset: 0x0012EB30
		// (set) Token: 0x0600530D RID: 21261 RVA: 0x0012FB38 File Offset: 0x0012EB38
		[DefaultValue(DockStyle.Bottom)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x0600530E RID: 21262 RVA: 0x0012FB41 File Offset: 0x0012EB41
		// (set) Token: 0x0600530F RID: 21263 RVA: 0x0012FB49 File Offset: 0x0012EB49
		[DefaultValue(ToolStripGripStyle.Hidden)]
		public new ToolStripGripStyle GripStyle
		{
			get
			{
				return base.GripStyle;
			}
			set
			{
				base.GripStyle = value;
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06005310 RID: 21264 RVA: 0x0012FB52 File Offset: 0x0012EB52
		// (set) Token: 0x06005311 RID: 21265 RVA: 0x0012FB5A File Offset: 0x0012EB5A
		[DefaultValue(ToolStripLayoutStyle.Table)]
		public new ToolStripLayoutStyle LayoutStyle
		{
			get
			{
				return base.LayoutStyle;
			}
			set
			{
				base.LayoutStyle = value;
			}
		}

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06005312 RID: 21266 RVA: 0x0012FB63 File Offset: 0x0012EB63
		// (set) Token: 0x06005313 RID: 21267 RVA: 0x0012FB6B File Offset: 0x0012EB6B
		[Browsable(false)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x14000315 RID: 789
		// (add) Token: 0x06005314 RID: 21268 RVA: 0x0012FB74 File Offset: 0x0012EB74
		// (remove) Token: 0x06005315 RID: 21269 RVA: 0x0012FB7D File Offset: 0x0012EB7D
		[Browsable(false)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06005316 RID: 21270 RVA: 0x0012FB86 File Offset: 0x0012EB86
		private Control RTLGrip
		{
			get
			{
				if (this.rtlLayoutGrip == null)
				{
					this.rtlLayoutGrip = new StatusStrip.RightToLeftLayoutGrip();
				}
				return this.rtlLayoutGrip;
			}
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06005317 RID: 21271 RVA: 0x0012FBA1 File Offset: 0x0012EBA1
		// (set) Token: 0x06005318 RID: 21272 RVA: 0x0012FBA9 File Offset: 0x0012EBA9
		[DefaultValue(false)]
		[SRDescription("ToolStripShowItemToolTipsDescr")]
		[SRCategory("CatBehavior")]
		public new bool ShowItemToolTips
		{
			get
			{
				return base.ShowItemToolTips;
			}
			set
			{
				base.ShowItemToolTips = value;
			}
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x06005319 RID: 21273 RVA: 0x0012FBB4 File Offset: 0x0012EBB4
		private bool ShowSizingGrip
		{
			get
			{
				if (this.SizingGrip && base.IsHandleCreated)
				{
					if (base.DesignMode)
					{
						return true;
					}
					HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this);
					if (rootHWnd.Handle != IntPtr.Zero)
					{
						return !UnsafeNativeMethods.IsZoomed(rootHWnd);
					}
				}
				return false;
			}
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x0600531A RID: 21274 RVA: 0x0012FC00 File Offset: 0x0012EC00
		// (set) Token: 0x0600531B RID: 21275 RVA: 0x0012FC12 File Offset: 0x0012EC12
		[SRDescription("StatusStripSizingGripDescr")]
		[DefaultValue(true)]
		[SRCategory("CatAppearance")]
		public bool SizingGrip
		{
			get
			{
				return this.state[StatusStrip.stateSizingGrip];
			}
			set
			{
				if (value != this.state[StatusStrip.stateSizingGrip])
				{
					this.state[StatusStrip.stateSizingGrip] = value;
					this.EnsureRightToLeftGrip();
					base.Invalidate(true);
				}
			}
		}

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x0600531C RID: 21276 RVA: 0x0012FC48 File Offset: 0x0012EC48
		[Browsable(false)]
		public Rectangle SizeGripBounds
		{
			get
			{
				if (!this.SizingGrip)
				{
					return Rectangle.Empty;
				}
				Size size = base.Size;
				int num = Math.Min(this.DefaultSize.Height, size.Height);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					return new Rectangle(0, size.Height - num, 12, num);
				}
				return new Rectangle(size.Width - 12, size.Height - num, 12, num);
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x0600531D RID: 21277 RVA: 0x0012FCBC File Offset: 0x0012ECBC
		// (set) Token: 0x0600531E RID: 21278 RVA: 0x0012FCC4 File Offset: 0x0012ECC4
		[DefaultValue(true)]
		[SRDescription("ToolStripStretchDescr")]
		[SRCategory("CatLayout")]
		public new bool Stretch
		{
			get
			{
				return base.Stretch;
			}
			set
			{
				base.Stretch = value;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x0600531F RID: 21279 RVA: 0x0012FCCD File Offset: 0x0012ECCD
		private TableLayoutSettings TableLayoutSettings
		{
			get
			{
				return base.LayoutSettings as TableLayoutSettings;
			}
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x0012FCDA File Offset: 0x0012ECDA
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new StatusStrip.StatusStripAccessibleObject(this);
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x0012FCE2 File Offset: 0x0012ECE2
		protected internal override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
		{
			return new ToolStripStatusLabel(text, image, onClick);
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x0012FCEC File Offset: 0x0012ECEC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.rtlLayoutGrip != null)
			{
				this.rtlLayoutGrip.Dispose();
				this.rtlLayoutGrip = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x0012FD14 File Offset: 0x0012ED14
		private void EnsureRightToLeftGrip()
		{
			if (this.SizingGrip && this.RightToLeft == RightToLeft.Yes)
			{
				this.RTLGrip.Bounds = this.SizeGripBounds;
				if (!base.Controls.Contains(this.RTLGrip))
				{
					WindowsFormsUtils.ReadOnlyControlCollection readOnlyControlCollection = base.Controls as WindowsFormsUtils.ReadOnlyControlCollection;
					if (readOnlyControlCollection != null)
					{
						readOnlyControlCollection.AddInternal(this.RTLGrip);
						return;
					}
				}
			}
			else if (this.rtlLayoutGrip != null && base.Controls.Contains(this.rtlLayoutGrip))
			{
				WindowsFormsUtils.ReadOnlyControlCollection readOnlyControlCollection2 = base.Controls as WindowsFormsUtils.ReadOnlyControlCollection;
				if (readOnlyControlCollection2 != null)
				{
					readOnlyControlCollection2.RemoveInternal(this.rtlLayoutGrip);
				}
				this.rtlLayoutGrip.Dispose();
				this.rtlLayoutGrip = null;
			}
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x0012FDBC File Offset: 0x0012EDBC
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (this.LayoutStyle != ToolStripLayoutStyle.Table)
			{
				return base.GetPreferredSizeCore(proposedSize);
			}
			if (proposedSize.Width == 1)
			{
				proposedSize.Width = int.MaxValue;
			}
			if (proposedSize.Height == 1)
			{
				proposedSize.Height = int.MaxValue;
			}
			if (base.Orientation == Orientation.Horizontal)
			{
				return ToolStrip.GetPreferredSizeHorizontal(this, proposedSize) + this.Padding.Size;
			}
			return ToolStrip.GetPreferredSizeVertical(this, proposedSize) + this.Padding.Size;
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x0012FE43 File Offset: 0x0012EE43
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			if (this.ShowSizingGrip)
			{
				base.Renderer.DrawStatusStripSizingGrip(new ToolStripRenderEventArgs(e.Graphics, this));
			}
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x0012FE6C File Offset: 0x0012EE6C
		protected override void OnLayout(LayoutEventArgs levent)
		{
			this.state[StatusStrip.stateCalledSpringTableLayout] = false;
			bool flag = false;
			ToolStripItem toolStripItem = levent.AffectedComponent as ToolStripItem;
			int count = this.DisplayedItems.Count;
			if (toolStripItem != null)
			{
				flag = this.DisplayedItems.Contains(toolStripItem);
			}
			if (this.LayoutStyle == ToolStripLayoutStyle.Table)
			{
				this.OnSpringTableLayoutCore();
			}
			base.OnLayout(levent);
			if ((count != this.DisplayedItems.Count || (toolStripItem != null && flag != this.DisplayedItems.Contains(toolStripItem))) && this.LayoutStyle == ToolStripLayoutStyle.Table)
			{
				this.OnSpringTableLayoutCore();
				base.OnLayout(levent);
			}
			this.EnsureRightToLeftGrip();
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x0012FF08 File Offset: 0x0012EF08
		protected override void SetDisplayedItems()
		{
			if (this.state[StatusStrip.stateCalledSpringTableLayout])
			{
				if (base.Orientation == Orientation.Horizontal)
				{
					RightToLeft rightToLeft = this.RightToLeft;
				}
				Point location = this.DisplayRectangle.Location;
				location.X += base.ClientSize.Width + 1;
				location.Y += base.ClientSize.Height + 1;
				bool flag = false;
				Rectangle rectangle = Rectangle.Empty;
				ToolStripItem toolStripItem = null;
				for (int i = 0; i < this.Items.Count; i++)
				{
					ToolStripItem toolStripItem2 = this.Items[i];
					if (flag || ((IArrangedElement)toolStripItem2).ParticipatesInLayout)
					{
						if (flag || (this.SizingGrip && toolStripItem2.Bounds.IntersectsWith(this.SizeGripBounds)))
						{
							base.SetItemLocation(toolStripItem2, location);
							toolStripItem2.SetPlacement(ToolStripItemPlacement.None);
						}
					}
					else if (toolStripItem != null && rectangle.IntersectsWith(toolStripItem2.Bounds))
					{
						base.SetItemLocation(toolStripItem2, location);
						toolStripItem2.SetPlacement(ToolStripItemPlacement.None);
					}
					else if (toolStripItem2.Bounds.Width == 1)
					{
						ToolStripStatusLabel toolStripStatusLabel = toolStripItem2 as ToolStripStatusLabel;
						if (toolStripStatusLabel != null && toolStripStatusLabel.Spring)
						{
							base.SetItemLocation(toolStripItem2, location);
							toolStripItem2.SetPlacement(ToolStripItemPlacement.None);
						}
					}
					if (toolStripItem2.Bounds.Location != location)
					{
						toolStripItem = toolStripItem2;
						rectangle = toolStripItem.Bounds;
					}
					else if (((IArrangedElement)toolStripItem2).ParticipatesInLayout)
					{
						flag = true;
					}
				}
			}
			base.SetDisplayedItems();
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x0013009D File Offset: 0x0012F09D
		internal override void ResetRenderMode()
		{
			base.RenderMode = ToolStripRenderMode.System;
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x001300A6 File Offset: 0x0012F0A6
		internal override bool ShouldSerializeRenderMode()
		{
			return base.RenderMode != ToolStripRenderMode.System && base.RenderMode != ToolStripRenderMode.Custom;
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x001300C0 File Offset: 0x0012F0C0
		protected virtual void OnSpringTableLayoutCore()
		{
			if (this.LayoutStyle == ToolStripLayoutStyle.Table)
			{
				this.state[StatusStrip.stateCalledSpringTableLayout] = true;
				base.SuspendLayout();
				if (this.lastOrientation != base.Orientation)
				{
					TableLayoutSettings tableLayoutSettings = this.TableLayoutSettings;
					tableLayoutSettings.RowCount = 0;
					tableLayoutSettings.ColumnCount = 0;
					tableLayoutSettings.ColumnStyles.Clear();
					tableLayoutSettings.RowStyles.Clear();
				}
				this.lastOrientation = base.Orientation;
				if (base.Orientation == Orientation.Horizontal)
				{
					this.TableLayoutSettings.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
					int count = this.TableLayoutSettings.ColumnStyles.Count;
					for (int i = 0; i < this.DisplayedItems.Count; i++)
					{
						if (i >= count)
						{
							this.TableLayoutSettings.ColumnStyles.Add(new ColumnStyle());
						}
						ToolStripStatusLabel toolStripStatusLabel = this.DisplayedItems[i] as ToolStripStatusLabel;
						bool flag = toolStripStatusLabel != null && toolStripStatusLabel.Spring;
						this.DisplayedItems[i].Anchor = (flag ? (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right) : (AnchorStyles.Top | AnchorStyles.Bottom));
						ColumnStyle columnStyle = this.TableLayoutSettings.ColumnStyles[i];
						columnStyle.Width = 100f;
						columnStyle.SizeType = (flag ? SizeType.Percent : SizeType.AutoSize);
					}
					if (this.TableLayoutSettings.RowStyles.Count > 1 || this.TableLayoutSettings.RowStyles.Count == 0)
					{
						this.TableLayoutSettings.RowStyles.Clear();
						this.TableLayoutSettings.RowStyles.Add(new RowStyle());
					}
					this.TableLayoutSettings.RowCount = 1;
					this.TableLayoutSettings.RowStyles[0].SizeType = SizeType.Absolute;
					this.TableLayoutSettings.RowStyles[0].Height = (float)Math.Max(0, this.DisplayRectangle.Height);
					this.TableLayoutSettings.ColumnCount = this.DisplayedItems.Count + 1;
					for (int j = this.DisplayedItems.Count; j < this.TableLayoutSettings.ColumnStyles.Count; j++)
					{
						this.TableLayoutSettings.ColumnStyles[j].SizeType = SizeType.AutoSize;
					}
				}
				else
				{
					this.TableLayoutSettings.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
					int count2 = this.TableLayoutSettings.RowStyles.Count;
					for (int k = 0; k < this.DisplayedItems.Count; k++)
					{
						if (k >= count2)
						{
							this.TableLayoutSettings.RowStyles.Add(new RowStyle());
						}
						ToolStripStatusLabel toolStripStatusLabel2 = this.DisplayedItems[k] as ToolStripStatusLabel;
						bool flag2 = toolStripStatusLabel2 != null && toolStripStatusLabel2.Spring;
						this.DisplayedItems[k].Anchor = (flag2 ? (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right) : (AnchorStyles.Left | AnchorStyles.Right));
						RowStyle rowStyle = this.TableLayoutSettings.RowStyles[k];
						rowStyle.Height = 100f;
						rowStyle.SizeType = (flag2 ? SizeType.Percent : SizeType.AutoSize);
					}
					this.TableLayoutSettings.ColumnCount = 1;
					if (this.TableLayoutSettings.ColumnStyles.Count > 1 || this.TableLayoutSettings.ColumnStyles.Count == 0)
					{
						this.TableLayoutSettings.ColumnStyles.Clear();
						this.TableLayoutSettings.ColumnStyles.Add(new ColumnStyle());
					}
					this.TableLayoutSettings.ColumnCount = 1;
					this.TableLayoutSettings.ColumnStyles[0].SizeType = SizeType.Absolute;
					this.TableLayoutSettings.ColumnStyles[0].Width = (float)Math.Max(0, this.DisplayRectangle.Width);
					this.TableLayoutSettings.RowCount = this.DisplayedItems.Count + 1;
					for (int l = this.DisplayedItems.Count; l < this.TableLayoutSettings.RowStyles.Count; l++)
					{
						this.TableLayoutSettings.RowStyles[l].SizeType = SizeType.AutoSize;
					}
				}
				base.ResumeLayout(false);
			}
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x001304C4 File Offset: 0x0012F4C4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 132 && this.SizingGrip)
			{
				Rectangle sizeGripBounds = this.SizeGripBounds;
				int x = NativeMethods.Util.LOWORD(m.LParam);
				int y = NativeMethods.Util.HIWORD(m.LParam);
				if (sizeGripBounds.Contains(base.PointToClient(new Point(x, y))))
				{
					HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this);
					if (rootHWnd.Handle != IntPtr.Zero && !UnsafeNativeMethods.IsZoomed(rootHWnd))
					{
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						UnsafeNativeMethods.GetClientRect(rootHWnd, ref rect);
						NativeMethods.POINT point;
						if (this.RightToLeft == RightToLeft.Yes)
						{
							point = new NativeMethods.POINT(this.SizeGripBounds.Left, this.SizeGripBounds.Bottom);
						}
						else
						{
							point = new NativeMethods.POINT(this.SizeGripBounds.Right, this.SizeGripBounds.Bottom);
						}
						UnsafeNativeMethods.MapWindowPoints(new HandleRef(this, base.Handle), rootHWnd, point, 1);
						int num = Math.Abs(rect.bottom - point.y);
						int num2 = Math.Abs(rect.right - point.x);
						if (this.RightToLeft != RightToLeft.Yes && num2 + num < 2)
						{
							m.Result = (IntPtr)17;
							return;
						}
					}
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x04003663 RID: 13923
		private const AnchorStyles AllAnchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

		// Token: 0x04003664 RID: 13924
		private const AnchorStyles HorizontalAnchor = AnchorStyles.Left | AnchorStyles.Right;

		// Token: 0x04003665 RID: 13925
		private const AnchorStyles VerticalAnchor = AnchorStyles.Top | AnchorStyles.Bottom;

		// Token: 0x04003666 RID: 13926
		private const int gripWidth = 12;

		// Token: 0x04003667 RID: 13927
		private BitVector32 state = default(BitVector32);

		// Token: 0x04003668 RID: 13928
		private static readonly int stateSizingGrip = BitVector32.CreateMask();

		// Token: 0x04003669 RID: 13929
		private static readonly int stateCalledSpringTableLayout = BitVector32.CreateMask(StatusStrip.stateSizingGrip);

		// Token: 0x0400366A RID: 13930
		private StatusStrip.RightToLeftLayoutGrip rtlLayoutGrip;

		// Token: 0x0400366B RID: 13931
		private Orientation lastOrientation;

		// Token: 0x02000634 RID: 1588
		private class RightToLeftLayoutGrip : Control
		{
			// Token: 0x0600532D RID: 21293 RVA: 0x00130635 File Offset: 0x0012F635
			public RightToLeftLayoutGrip()
			{
				base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
				this.BackColor = Color.Transparent;
			}

			// Token: 0x170010EA RID: 4330
			// (get) Token: 0x0600532E RID: 21294 RVA: 0x00130654 File Offset: 0x0012F654
			protected override CreateParams CreateParams
			{
				[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.ExStyle |= 4194304;
					return createParams;
				}
			}

			// Token: 0x0600532F RID: 21295 RVA: 0x0013067C File Offset: 0x0012F67C
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 132)
				{
					int x = NativeMethods.Util.LOWORD(m.LParam);
					int y = NativeMethods.Util.HIWORD(m.LParam);
					if (base.ClientRectangle.Contains(base.PointToClient(new Point(x, y))))
					{
						m.Result = (IntPtr)16;
						return;
					}
				}
				base.WndProc(ref m);
			}
		}

		// Token: 0x02000635 RID: 1589
		[ComVisible(true)]
		internal class StatusStripAccessibleObject : ToolStrip.ToolStripAccessibleObject
		{
			// Token: 0x06005330 RID: 21296 RVA: 0x001306E0 File Offset: 0x0012F6E0
			public StatusStripAccessibleObject(StatusStrip owner) : base(owner)
			{
			}

			// Token: 0x170010EB RID: 4331
			// (get) Token: 0x06005331 RID: 21297 RVA: 0x001306EC File Offset: 0x0012F6EC
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.StatusBar;
				}
			}
		}
	}
}
