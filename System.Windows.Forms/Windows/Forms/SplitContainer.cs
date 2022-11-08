using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000615 RID: 1557
	[Designer("System.Windows.Forms.Design.SplitContainerDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("SplitterMoved")]
	[Docking(DockingBehavior.AutoDock)]
	[SRDescription("DescriptionSplitContainer")]
	[ComVisible(true)]
	public class SplitContainer : ContainerControl
	{
		// Token: 0x06005107 RID: 20743 RVA: 0x00128EB4 File Offset: 0x00127EB4
		public SplitContainer()
		{
			this.panel1 = new SplitterPanel(this);
			this.panel2 = new SplitterPanel(this);
			this.splitterRect = default(Rectangle);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			((WindowsFormsUtils.TypedControlCollection)this.Controls).AddInternal(this.panel1);
			((WindowsFormsUtils.TypedControlCollection)this.Controls).AddInternal(this.panel2);
			this.UpdateSplitter();
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x06005108 RID: 20744 RVA: 0x00128F83 File Offset: 0x00127F83
		// (set) Token: 0x06005109 RID: 20745 RVA: 0x00128F86 File Offset: 0x00127F86
		[SRDescription("FormAutoScrollDescr")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[DefaultValue(false)]
		[Browsable(false)]
		public override bool AutoScroll
		{
			get
			{
				return false;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x0600510A RID: 20746 RVA: 0x00128F8F File Offset: 0x00127F8F
		// (set) Token: 0x0600510B RID: 20747 RVA: 0x00128F97 File Offset: 0x00127F97
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(typeof(Point), "0, 0")]
		public override Point AutoScrollOffset
		{
			get
			{
				return base.AutoScrollOffset;
			}
			set
			{
				base.AutoScrollOffset = value;
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x0600510C RID: 20748 RVA: 0x00128FA0 File Offset: 0x00127FA0
		// (set) Token: 0x0600510D RID: 20749 RVA: 0x00128FA8 File Offset: 0x00127FA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x0600510E RID: 20750 RVA: 0x00128FB1 File Offset: 0x00127FB1
		// (set) Token: 0x0600510F RID: 20751 RVA: 0x00128FB9 File Offset: 0x00127FB9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06005110 RID: 20752 RVA: 0x00128FC2 File Offset: 0x00127FC2
		// (set) Token: 0x06005111 RID: 20753 RVA: 0x00128FCA File Offset: 0x00127FCA
		[Browsable(false)]
		[SRCategory("CatLayout")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormAutoScrollPositionDescr")]
		public new Point AutoScrollPosition
		{
			get
			{
				return base.AutoScrollPosition;
			}
			set
			{
				base.AutoScrollPosition = value;
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06005112 RID: 20754 RVA: 0x00128FD3 File Offset: 0x00127FD3
		// (set) Token: 0x06005113 RID: 20755 RVA: 0x00128FDB File Offset: 0x00127FDB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		// Token: 0x140002F0 RID: 752
		// (add) Token: 0x06005114 RID: 20756 RVA: 0x00128FE4 File Offset: 0x00127FE4
		// (remove) Token: 0x06005115 RID: 20757 RVA: 0x00128FED File Offset: 0x00127FED
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06005116 RID: 20758 RVA: 0x00128FF6 File Offset: 0x00127FF6
		// (set) Token: 0x06005117 RID: 20759 RVA: 0x00128FFE File Offset: 0x00127FFE
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06005118 RID: 20760 RVA: 0x00129007 File Offset: 0x00128007
		// (set) Token: 0x06005119 RID: 20761 RVA: 0x0012900F File Offset: 0x0012800F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x0600511A RID: 20762 RVA: 0x00129018 File Offset: 0x00128018
		// (set) Token: 0x0600511B RID: 20763 RVA: 0x00129020 File Offset: 0x00128020
		[SRDescription("ContainerControlBindingContextDescr")]
		[Browsable(false)]
		public override BindingContext BindingContext
		{
			get
			{
				return base.BindingContextInternal;
			}
			set
			{
				base.BindingContextInternal = value;
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x0600511C RID: 20764 RVA: 0x00129029 File Offset: 0x00128029
		// (set) Token: 0x0600511D RID: 20765 RVA: 0x00129034 File Offset: 0x00128034
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.None)]
		[DispId(-504)]
		[SRDescription("SplitterBorderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.Invalidate();
					this.SetInnerMostBorder(this);
					if (this.ParentInternal != null && this.ParentInternal is SplitterPanel)
					{
						SplitContainer owner = ((SplitterPanel)this.ParentInternal).Owner;
						owner.SetInnerMostBorder(owner);
					}
				}
				switch (this.BorderStyle)
				{
				case BorderStyle.None:
					this.BORDERSIZE = 0;
					return;
				case BorderStyle.FixedSingle:
					this.BORDERSIZE = 1;
					return;
				case BorderStyle.Fixed3D:
					this.BORDERSIZE = 4;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x0600511E RID: 20766 RVA: 0x001290E2 File Offset: 0x001280E2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control.ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		// Token: 0x140002F1 RID: 753
		// (add) Token: 0x0600511F RID: 20767 RVA: 0x001290EA File Offset: 0x001280EA
		// (remove) Token: 0x06005120 RID: 20768 RVA: 0x001290F3 File Offset: 0x001280F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event ControlEventHandler ControlAdded
		{
			add
			{
				base.ControlAdded += value;
			}
			remove
			{
				base.ControlAdded -= value;
			}
		}

		// Token: 0x140002F2 RID: 754
		// (add) Token: 0x06005121 RID: 20769 RVA: 0x001290FC File Offset: 0x001280FC
		// (remove) Token: 0x06005122 RID: 20770 RVA: 0x00129105 File Offset: 0x00128105
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event ControlEventHandler ControlRemoved
		{
			add
			{
				base.ControlRemoved += value;
			}
			remove
			{
				base.ControlRemoved -= value;
			}
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06005123 RID: 20771 RVA: 0x0012910E File Offset: 0x0012810E
		// (set) Token: 0x06005124 RID: 20772 RVA: 0x00129118 File Offset: 0x00128118
		public new DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
				if (this.ParentInternal != null && this.ParentInternal is SplitterPanel)
				{
					SplitContainer owner = ((SplitterPanel)this.ParentInternal).Owner;
					owner.SetInnerMostBorder(owner);
				}
				this.ResizeSplitContainer();
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x06005125 RID: 20773 RVA: 0x0012915F File Offset: 0x0012815F
		protected override Size DefaultSize
		{
			get
			{
				return new Size(150, 100);
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06005126 RID: 20774 RVA: 0x0012916D File Offset: 0x0012816D
		// (set) Token: 0x06005127 RID: 20775 RVA: 0x00129178 File Offset: 0x00128178
		[SRCategory("CatLayout")]
		[SRDescription("SplitContainerFixedPanelDescr")]
		[DefaultValue(FixedPanel.None)]
		public FixedPanel FixedPanel
		{
			get
			{
				return this.fixedPanel;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FixedPanel));
				}
				if (this.fixedPanel != value)
				{
					this.fixedPanel = value;
					FixedPanel fixedPanel = this.fixedPanel;
					if (fixedPanel == FixedPanel.Panel2)
					{
						if (this.Orientation == Orientation.Vertical)
						{
							this.panelSize = base.Width - this.SplitterDistanceInternal - this.SplitterWidthInternal;
							return;
						}
						this.panelSize = base.Height - this.SplitterDistanceInternal - this.SplitterWidthInternal;
						return;
					}
					else
					{
						this.panelSize = this.SplitterDistanceInternal;
					}
				}
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06005128 RID: 20776 RVA: 0x00129211 File Offset: 0x00128211
		// (set) Token: 0x06005129 RID: 20777 RVA: 0x00129219 File Offset: 0x00128219
		[SRCategory("CatLayout")]
		[SRDescription("SplitContainerIsSplitterFixedDescr")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool IsSplitterFixed
		{
			get
			{
				return this.splitterFixed;
			}
			set
			{
				this.splitterFixed = value;
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x0600512A RID: 20778 RVA: 0x00129224 File Offset: 0x00128224
		private bool IsSplitterMovable
		{
			get
			{
				if (this.Orientation == Orientation.Vertical)
				{
					return base.Width >= this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize;
				}
				return base.Height >= this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize;
			}
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x0600512B RID: 20779 RVA: 0x00129279 File Offset: 0x00128279
		internal override bool IsContainerControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x0600512C RID: 20780 RVA: 0x0012927C File Offset: 0x0012827C
		// (set) Token: 0x0600512D RID: 20781 RVA: 0x00129284 File Offset: 0x00128284
		[DefaultValue(Orientation.Vertical)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("SplitContainerOrientationDescr")]
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Orientation));
				}
				if (this.orientation != value)
				{
					this.orientation = value;
					this.splitDistance = 0;
					this.SplitterDistance = this.SplitterDistanceInternal;
					this.UpdateSplitter();
				}
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x001292E0 File Offset: 0x001282E0
		// (set) Token: 0x0600512F RID: 20783 RVA: 0x001292E8 File Offset: 0x001282E8
		private Cursor OverrideCursor
		{
			get
			{
				return this.overrideCursor;
			}
			set
			{
				if (this.overrideCursor != value)
				{
					this.overrideCursor = value;
					if (base.IsHandleCreated)
					{
						NativeMethods.POINT point = new NativeMethods.POINT();
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						UnsafeNativeMethods.GetCursorPos(point);
						UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
						if ((rect.left <= point.x && point.x < rect.right && rect.top <= point.y && point.y < rect.bottom) || UnsafeNativeMethods.GetCapture() == base.Handle)
						{
							base.SendMessage(32, base.Handle, 1);
						}
					}
				}
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06005130 RID: 20784 RVA: 0x0012939F File Offset: 0x0012839F
		private bool CollapsedMode
		{
			get
			{
				return this.Panel1Collapsed || this.Panel2Collapsed;
			}
		}

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06005131 RID: 20785 RVA: 0x001293B1 File Offset: 0x001283B1
		[SRCategory("CatAppearance")]
		[Localizable(false)]
		[SRDescription("SplitContainerPanel1Descr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SplitterPanel Panel1
		{
			get
			{
				return this.panel1;
			}
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x001293B9 File Offset: 0x001283B9
		private void CollapsePanel(SplitterPanel p, bool collapsing)
		{
			p.Collapsed = collapsing;
			if (collapsing)
			{
				p.Visible = false;
			}
			else
			{
				p.Visible = true;
			}
			this.UpdateSplitter();
		}

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06005133 RID: 20787 RVA: 0x001293DB File Offset: 0x001283DB
		// (set) Token: 0x06005134 RID: 20788 RVA: 0x001293E3 File Offset: 0x001283E3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x140002F3 RID: 755
		// (add) Token: 0x06005135 RID: 20789 RVA: 0x001293EC File Offset: 0x001283EC
		// (remove) Token: 0x06005136 RID: 20790 RVA: 0x001293F5 File Offset: 0x001283F5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06005137 RID: 20791 RVA: 0x001293FE File Offset: 0x001283FE
		// (set) Token: 0x06005138 RID: 20792 RVA: 0x0012940B File Offset: 0x0012840B
		[DefaultValue(false)]
		[SRDescription("SplitContainerPanel1CollapsedDescr")]
		[SRCategory("CatLayout")]
		public bool Panel1Collapsed
		{
			get
			{
				return this.panel1.Collapsed;
			}
			set
			{
				if (value != this.panel1.Collapsed)
				{
					if (value && this.panel2.Collapsed)
					{
						this.CollapsePanel(this.panel2, false);
					}
					this.CollapsePanel(this.panel1, value);
				}
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06005139 RID: 20793 RVA: 0x00129445 File Offset: 0x00128445
		// (set) Token: 0x0600513A RID: 20794 RVA: 0x00129452 File Offset: 0x00128452
		[SRDescription("SplitContainerPanel2CollapsedDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		public bool Panel2Collapsed
		{
			get
			{
				return this.panel2.Collapsed;
			}
			set
			{
				if (value != this.panel2.Collapsed)
				{
					if (value && this.panel1.Collapsed)
					{
						this.CollapsePanel(this.panel1, false);
					}
					this.CollapsePanel(this.panel2, value);
				}
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x0600513B RID: 20795 RVA: 0x0012948C File Offset: 0x0012848C
		// (set) Token: 0x0600513C RID: 20796 RVA: 0x00129494 File Offset: 0x00128494
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("SplitContainerPanel1MinSizeDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(25)]
		[Localizable(true)]
		public int Panel1MinSize
		{
			get
			{
				return this.panel1MinSize;
			}
			set
			{
				if (value != this.Panel1MinSize)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Panel1MinSize", SR.GetString("InvalidLowBoundArgument", new object[]
						{
							"Panel1MinSize",
							value.ToString(CultureInfo.CurrentCulture),
							"0"
						}));
					}
					if (this.Orientation == Orientation.Vertical)
					{
						if (base.DesignMode && base.Width != this.DefaultSize.Width && value + this.Panel2MinSize + this.SplitterWidth > base.Width)
						{
							throw new ArgumentOutOfRangeException("Panel1MinSize", SR.GetString("InvalidArgument", new object[]
							{
								"Panel1MinSize",
								value.ToString(CultureInfo.CurrentCulture)
							}));
						}
					}
					else if (this.Orientation == Orientation.Horizontal && base.DesignMode && base.Height != this.DefaultSize.Height && value + this.Panel2MinSize + this.SplitterWidth > base.Height)
					{
						throw new ArgumentOutOfRangeException("Panel1MinSize", SR.GetString("InvalidArgument", new object[]
						{
							"Panel1MinSize",
							value.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.panel1MinSize = value;
					if (value > this.SplitterDistanceInternal)
					{
						this.SplitterDistanceInternal = value;
					}
				}
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x0600513D RID: 20797 RVA: 0x001295F4 File Offset: 0x001285F4
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatAppearance")]
		[SRDescription("SplitContainerPanel2Descr")]
		public SplitterPanel Panel2
		{
			get
			{
				return this.panel2;
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x001295FC File Offset: 0x001285FC
		// (set) Token: 0x0600513F RID: 20799 RVA: 0x00129604 File Offset: 0x00128604
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("SplitContainerPanel2MinSizeDescr")]
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(25)]
		public int Panel2MinSize
		{
			get
			{
				return this.panel2MinSize;
			}
			set
			{
				if (value != this.Panel2MinSize)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Panel2MinSize", SR.GetString("InvalidLowBoundArgument", new object[]
						{
							"Panel2MinSize",
							value.ToString(CultureInfo.CurrentCulture),
							"0"
						}));
					}
					if (this.Orientation == Orientation.Vertical)
					{
						if (base.DesignMode && base.Width != this.DefaultSize.Width && value + this.Panel1MinSize + this.SplitterWidth > base.Width)
						{
							throw new ArgumentOutOfRangeException("Panel2MinSize", SR.GetString("InvalidArgument", new object[]
							{
								"Panel2MinSize",
								value.ToString(CultureInfo.CurrentCulture)
							}));
						}
					}
					else if (this.Orientation == Orientation.Horizontal && base.DesignMode && base.Height != this.DefaultSize.Height && value + this.Panel1MinSize + this.SplitterWidth > base.Height)
					{
						throw new ArgumentOutOfRangeException("Panel2MinSize", SR.GetString("InvalidArgument", new object[]
						{
							"Panel2MinSize",
							value.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.panel2MinSize = value;
					if (value > this.Panel2.Width)
					{
						this.SplitterDistanceInternal = this.Panel2.Width + this.SplitterWidthInternal;
					}
				}
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06005140 RID: 20800 RVA: 0x0012977A File Offset: 0x0012877A
		// (set) Token: 0x06005141 RID: 20801 RVA: 0x00129784 File Offset: 0x00128784
		[Localizable(true)]
		[SRDescription("SplitContainerSplitterDistanceDescr")]
		[DefaultValue(50)]
		[SRCategory("CatLayout")]
		[SettingsBindable(true)]
		public int SplitterDistance
		{
			get
			{
				return this.splitDistance;
			}
			set
			{
				if (value != this.SplitterDistance)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("SplitterDistance", SR.GetString("InvalidLowBoundArgument", new object[]
						{
							"SplitterDistance",
							value.ToString(CultureInfo.CurrentCulture),
							"0"
						}));
					}
					try
					{
						this.setSplitterDistance = true;
						if (this.Orientation == Orientation.Vertical)
						{
							if (value < this.Panel1MinSize)
							{
								value = this.Panel1MinSize;
							}
							if (value + this.SplitterWidthInternal > base.Width - this.Panel2MinSize)
							{
								value = base.Width - this.Panel2MinSize - this.SplitterWidthInternal;
							}
							if (value < 0)
							{
								throw new InvalidOperationException(SR.GetString("SplitterDistanceNotAllowed"));
							}
							this.splitDistance = value;
							this.splitterDistance = value;
							this.panel1.WidthInternal = this.SplitterDistance;
						}
						else
						{
							if (value < this.Panel1MinSize)
							{
								value = this.Panel1MinSize;
							}
							if (value + this.SplitterWidthInternal > base.Height - this.Panel2MinSize)
							{
								value = base.Height - this.Panel2MinSize - this.SplitterWidthInternal;
							}
							if (value < 0)
							{
								throw new InvalidOperationException(SR.GetString("SplitterDistanceNotAllowed"));
							}
							this.splitDistance = value;
							this.splitterDistance = value;
							this.panel1.HeightInternal = this.SplitterDistance;
						}
						switch (this.fixedPanel)
						{
						case FixedPanel.Panel1:
							this.panelSize = this.SplitterDistance;
							break;
						case FixedPanel.Panel2:
							if (this.Orientation == Orientation.Vertical)
							{
								this.panelSize = base.Width - this.SplitterDistance - this.SplitterWidthInternal;
							}
							else
							{
								this.panelSize = base.Height - this.SplitterDistance - this.SplitterWidthInternal;
							}
							break;
						}
						this.UpdateSplitter();
					}
					finally
					{
						this.setSplitterDistance = false;
					}
					this.OnSplitterMoved(new SplitterEventArgs(this.SplitterRectangle.X + this.SplitterRectangle.Width / 2, this.SplitterRectangle.Y + this.SplitterRectangle.Height / 2, this.SplitterRectangle.X, this.SplitterRectangle.Y));
				}
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06005142 RID: 20802 RVA: 0x001299CC File Offset: 0x001289CC
		// (set) Token: 0x06005143 RID: 20803 RVA: 0x001299D4 File Offset: 0x001289D4
		private int SplitterDistanceInternal
		{
			get
			{
				return this.splitterDistance;
			}
			set
			{
				this.SplitterDistance = value;
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06005144 RID: 20804 RVA: 0x001299DD File Offset: 0x001289DD
		// (set) Token: 0x06005145 RID: 20805 RVA: 0x001299E8 File Offset: 0x001289E8
		[SRDescription("SplitContainerSplitterIncrementDescr")]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		[DefaultValue(1)]
		public int SplitterIncrement
		{
			get
			{
				return this.splitterInc;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("SplitterIncrement", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"SplitterIncrement",
						value.ToString(CultureInfo.CurrentCulture),
						"1"
					}));
				}
				this.splitterInc = value;
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06005146 RID: 20806 RVA: 0x00129A3C File Offset: 0x00128A3C
		[SRDescription("SplitContainerSplitterRectangleDescr")]
		[Browsable(false)]
		[SRCategory("CatLayout")]
		public Rectangle SplitterRectangle
		{
			get
			{
				Rectangle result = this.splitterRect;
				result.X = this.splitterRect.X - base.Left;
				result.Y = this.splitterRect.Y - base.Top;
				return result;
			}
		}

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x06005147 RID: 20807 RVA: 0x00129A83 File Offset: 0x00128A83
		// (set) Token: 0x06005148 RID: 20808 RVA: 0x00129A8C File Offset: 0x00128A8C
		[SRCategory("CatLayout")]
		[SRDescription("SplitContainerSplitterWidthDescr")]
		[Localizable(true)]
		[DefaultValue(4)]
		public int SplitterWidth
		{
			get
			{
				return this.splitterWidth;
			}
			set
			{
				if (value != this.SplitterWidth)
				{
					if (value < 1)
					{
						throw new ArgumentOutOfRangeException("SplitterWidth", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"SplitterWidth",
							value.ToString(CultureInfo.CurrentCulture),
							"1"
						}));
					}
					if (this.Orientation == Orientation.Vertical)
					{
						if (base.DesignMode && value + this.Panel1MinSize + this.Panel2MinSize > base.Width)
						{
							throw new ArgumentOutOfRangeException("SplitterWidth", SR.GetString("InvalidArgument", new object[]
							{
								"SplitterWidth",
								value.ToString(CultureInfo.CurrentCulture)
							}));
						}
					}
					else if (this.Orientation == Orientation.Horizontal && base.DesignMode && value + this.Panel1MinSize + this.Panel2MinSize > base.Height)
					{
						throw new ArgumentOutOfRangeException("SplitterWidth", SR.GetString("InvalidArgument", new object[]
						{
							"SplitterWidth",
							value.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.splitterWidth = value;
					this.UpdateSplitter();
				}
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06005149 RID: 20809 RVA: 0x00129BAF File Offset: 0x00128BAF
		private int SplitterWidthInternal
		{
			get
			{
				if (!this.CollapsedMode)
				{
					return this.splitterWidth;
				}
				return 0;
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x0600514A RID: 20810 RVA: 0x00129BC1 File Offset: 0x00128BC1
		// (set) Token: 0x0600514B RID: 20811 RVA: 0x00129BC9 File Offset: 0x00128BC9
		[SRDescription("ControlTabStopDescr")]
		[DispId(-516)]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public new bool TabStop
		{
			get
			{
				return this.tabStop;
			}
			set
			{
				if (this.TabStop != value)
				{
					this.tabStop = value;
					this.OnTabStopChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x0600514C RID: 20812 RVA: 0x00129BE6 File Offset: 0x00128BE6
		// (set) Token: 0x0600514D RID: 20813 RVA: 0x00129BEE File Offset: 0x00128BEE
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		[Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x140002F4 RID: 756
		// (add) Token: 0x0600514E RID: 20814 RVA: 0x00129BF7 File Offset: 0x00128BF7
		// (remove) Token: 0x0600514F RID: 20815 RVA: 0x00129C00 File Offset: 0x00128C00
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		// Token: 0x140002F5 RID: 757
		// (add) Token: 0x06005150 RID: 20816 RVA: 0x00129C09 File Offset: 0x00128C09
		// (remove) Token: 0x06005151 RID: 20817 RVA: 0x00129C12 File Offset: 0x00128C12
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		// Token: 0x140002F6 RID: 758
		// (add) Token: 0x06005152 RID: 20818 RVA: 0x00129C1B File Offset: 0x00128C1B
		// (remove) Token: 0x06005153 RID: 20819 RVA: 0x00129C2E File Offset: 0x00128C2E
		[SRDescription("SplitterSplitterMovingDescr")]
		[SRCategory("CatBehavior")]
		public event SplitterCancelEventHandler SplitterMoving
		{
			add
			{
				base.Events.AddHandler(SplitContainer.EVENT_MOVING, value);
			}
			remove
			{
				base.Events.RemoveHandler(SplitContainer.EVENT_MOVING, value);
			}
		}

		// Token: 0x140002F7 RID: 759
		// (add) Token: 0x06005154 RID: 20820 RVA: 0x00129C41 File Offset: 0x00128C41
		// (remove) Token: 0x06005155 RID: 20821 RVA: 0x00129C54 File Offset: 0x00128C54
		[SRCategory("CatBehavior")]
		[SRDescription("SplitterSplitterMovedDescr")]
		public event SplitterEventHandler SplitterMoved
		{
			add
			{
				base.Events.AddHandler(SplitContainer.EVENT_MOVED, value);
			}
			remove
			{
				base.Events.RemoveHandler(SplitContainer.EVENT_MOVED, value);
			}
		}

		// Token: 0x140002F8 RID: 760
		// (add) Token: 0x06005156 RID: 20822 RVA: 0x00129C67 File Offset: 0x00128C67
		// (remove) Token: 0x06005157 RID: 20823 RVA: 0x00129C70 File Offset: 0x00128C70
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x00129C79 File Offset: 0x00128C79
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x00129C88 File Offset: 0x00128C88
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.IsSplitterMovable && !this.IsSplitterFixed)
			{
				if (e.KeyData == Keys.Escape && this.splitBegin)
				{
					this.splitBegin = false;
					this.splitBreak = true;
					return;
				}
				if (e.KeyData == Keys.Right || e.KeyData == Keys.Down || e.KeyData == Keys.Left || (e.KeyData == Keys.Up && this.splitterFocused))
				{
					if (this.splitBegin)
					{
						this.splitMove = true;
					}
					if (e.KeyData == Keys.Left || (e.KeyData == Keys.Up && this.splitterFocused))
					{
						this.splitterDistance -= this.SplitterIncrement;
						this.splitterDistance = ((this.splitterDistance < this.Panel1MinSize) ? (this.splitterDistance + this.SplitterIncrement) : Math.Max(this.splitterDistance, this.BORDERSIZE));
					}
					if (e.KeyData == Keys.Right || (e.KeyData == Keys.Down && this.splitterFocused))
					{
						this.splitterDistance += this.SplitterIncrement;
						if (this.Orientation == Orientation.Vertical)
						{
							this.splitterDistance = ((this.splitterDistance + this.SplitterWidth > base.Width - this.Panel2MinSize - this.BORDERSIZE) ? (this.splitterDistance - this.SplitterIncrement) : this.splitterDistance);
						}
						else
						{
							this.splitterDistance = ((this.splitterDistance + this.SplitterWidth > base.Height - this.Panel2MinSize - this.BORDERSIZE) ? (this.splitterDistance - this.SplitterIncrement) : this.splitterDistance);
						}
					}
					if (!this.splitBegin)
					{
						this.splitBegin = true;
					}
					if (this.splitBegin && !this.splitMove)
					{
						this.initialSplitterDistance = this.SplitterDistanceInternal;
						this.DrawSplitBar(1);
						return;
					}
					this.DrawSplitBar(2);
					Rectangle rectangle = this.CalcSplitLine(this.splitterDistance, 0);
					int x = rectangle.X;
					int y = rectangle.Y;
					SplitterCancelEventArgs splitterCancelEventArgs = new SplitterCancelEventArgs(base.Left + this.SplitterRectangle.X + this.SplitterRectangle.Width / 2, base.Top + this.SplitterRectangle.Y + this.SplitterRectangle.Height / 2, x, y);
					this.OnSplitterMoving(splitterCancelEventArgs);
					if (splitterCancelEventArgs.Cancel)
					{
						this.SplitEnd(false);
					}
				}
			}
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x00129F00 File Offset: 0x00128F00
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (this.splitBegin && this.IsSplitterMovable && (e.KeyData == Keys.Right || e.KeyData == Keys.Down || e.KeyData == Keys.Left || (e.KeyData == Keys.Up && this.splitterFocused)))
			{
				this.DrawSplitBar(3);
				this.ApplySplitterDistance();
				this.splitBegin = false;
				this.splitMove = false;
			}
			if (this.splitBreak)
			{
				this.splitBreak = false;
				this.SplitEnd(false);
			}
			using (Graphics graphics = base.CreateGraphicsInternal())
			{
				if (this.BackgroundImage == null)
				{
					graphics.FillRectangle(new SolidBrush(this.BackColor), this.SplitterRectangle);
				}
				this.DrawFocus(graphics, this.SplitterRectangle);
			}
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x00129FD4 File Offset: 0x00128FD4
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.SetInnerMostBorder(this);
			if (this.IsSplitterMovable && !this.setSplitterDistance)
			{
				this.ResizeSplitContainer();
			}
			base.OnLayout(e);
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x00129FFA File Offset: 0x00128FFA
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			base.Invalidate();
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x0012A00C File Offset: 0x0012900C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!this.IsSplitterFixed && this.IsSplitterMovable)
			{
				if (this.Cursor == this.DefaultCursor && this.SplitterRectangle.Contains(e.Location))
				{
					if (this.Orientation == Orientation.Vertical)
					{
						this.OverrideCursor = Cursors.VSplit;
					}
					else
					{
						this.OverrideCursor = Cursors.HSplit;
					}
				}
				else
				{
					this.OverrideCursor = null;
				}
				if (this.splitterClick)
				{
					int num = e.X;
					int num2 = e.Y;
					this.splitterDrag = true;
					this.SplitMove(num, num2);
					if (this.Orientation == Orientation.Vertical)
					{
						num = Math.Max(Math.Min(num, base.Width - this.Panel2MinSize), this.Panel1MinSize);
						num2 = Math.Max(num2, 0);
					}
					else
					{
						num2 = Math.Max(Math.Min(num2, base.Height - this.Panel2MinSize), this.Panel1MinSize);
						num = Math.Max(num, 0);
					}
					Rectangle rectangle = this.CalcSplitLine(this.GetSplitterDistance(e.X, e.Y), 0);
					int x = rectangle.X;
					int y = rectangle.Y;
					SplitterCancelEventArgs splitterCancelEventArgs = new SplitterCancelEventArgs(num, num2, x, y);
					this.OnSplitterMoving(splitterCancelEventArgs);
					if (splitterCancelEventArgs.Cancel)
					{
						this.SplitEnd(false);
					}
				}
			}
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x0012A15A File Offset: 0x0012915A
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!base.Enabled)
			{
				return;
			}
			this.OverrideCursor = null;
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x0012A174 File Offset: 0x00129174
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.IsSplitterMovable && this.SplitterRectangle.Contains(e.Location))
			{
				if (!base.Enabled)
				{
					return;
				}
				if (e.Button == MouseButtons.Left && e.Clicks == 1 && !this.IsSplitterFixed)
				{
					this.splitterFocused = true;
					IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
					if (containerControlInternal != null)
					{
						ContainerControl containerControl = containerControlInternal as ContainerControl;
						if (containerControl == null)
						{
							containerControlInternal.ActiveControl = this;
						}
						else
						{
							containerControl.SetActiveControlInternal(this);
						}
					}
					base.SetActiveControlInternal(null);
					this.nextActiveControl = this.panel2;
					this.SplitBegin(e.X, e.Y);
					this.splitterClick = true;
				}
			}
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x0012A230 File Offset: 0x00129230
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!base.Enabled)
			{
				return;
			}
			if (!this.IsSplitterFixed && this.IsSplitterMovable && this.splitterClick)
			{
				base.CaptureInternal = false;
				if (this.splitterDrag)
				{
					this.CalcSplitLine(this.GetSplitterDistance(e.X, e.Y), 0);
					this.SplitEnd(true);
				}
				else
				{
					this.SplitEnd(false);
				}
				this.splitterClick = false;
				this.splitterDrag = false;
			}
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x0012A2AC File Offset: 0x001292AC
		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			this.SetSplitterRect(this.Orientation == Orientation.Vertical);
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x0012A2C4 File Offset: 0x001292C4
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this.Focused)
			{
				this.DrawFocus(e.Graphics, this.SplitterRectangle);
			}
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x0012A2E8 File Offset: 0x001292E8
		public void OnSplitterMoving(SplitterCancelEventArgs e)
		{
			SplitterCancelEventHandler splitterCancelEventHandler = (SplitterCancelEventHandler)base.Events[SplitContainer.EVENT_MOVING];
			if (splitterCancelEventHandler != null)
			{
				splitterCancelEventHandler(this, e);
			}
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x0012A318 File Offset: 0x00129318
		public void OnSplitterMoved(SplitterEventArgs e)
		{
			SplitterEventHandler splitterEventHandler = (SplitterEventHandler)base.Events[SplitContainer.EVENT_MOVED];
			if (splitterEventHandler != null)
			{
				splitterEventHandler(this, e);
			}
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x0012A346 File Offset: 0x00129346
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			this.panel1.RightToLeft = this.RightToLeft;
			this.panel2.RightToLeft = this.RightToLeft;
			this.UpdateSplitter();
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x0012A378 File Offset: 0x00129378
		private void ApplySplitterDistance()
		{
			using (new LayoutTransaction(this, this, "SplitterDistance", false))
			{
				this.SplitterDistanceInternal = this.splitterDistance;
			}
			if (this.BackColor == Color.Transparent)
			{
				base.Invalidate();
			}
			if (this.Orientation != Orientation.Vertical)
			{
				this.splitterRect.Y = base.Location.Y + this.SplitterDistanceInternal;
				return;
			}
			if (this.RightToLeft == RightToLeft.No)
			{
				this.splitterRect.X = base.Location.X + this.SplitterDistanceInternal;
				return;
			}
			this.splitterRect.X = base.Right - this.SplitterDistanceInternal - this.SplitterWidthInternal;
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x0012A448 File Offset: 0x00129448
		private Rectangle CalcSplitLine(int splitSize, int minWeight)
		{
			Rectangle result = default(Rectangle);
			switch (this.Orientation)
			{
			case Orientation.Horizontal:
				result.Width = base.Width;
				result.Height = this.SplitterWidthInternal;
				if (result.Width < minWeight)
				{
					result.Width = minWeight;
				}
				result.Y = this.panel1.Location.Y + splitSize;
				break;
			case Orientation.Vertical:
				result.Width = this.SplitterWidthInternal;
				result.Height = base.Height;
				if (result.Width < minWeight)
				{
					result.Width = minWeight;
				}
				if (this.RightToLeft == RightToLeft.No)
				{
					result.X = this.panel1.Location.X + splitSize;
				}
				else
				{
					result.X = base.Width - splitSize - this.SplitterWidthInternal;
				}
				break;
			}
			return result;
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x0012A52C File Offset: 0x0012952C
		private void DrawSplitBar(int mode)
		{
			if (mode != 1 && this.lastDrawSplit != -1)
			{
				this.DrawSplitHelper(this.lastDrawSplit);
				this.lastDrawSplit = -1;
			}
			else if (mode != 1 && this.lastDrawSplit == -1)
			{
				return;
			}
			if (mode == 3)
			{
				if (this.lastDrawSplit != -1)
				{
					this.DrawSplitHelper(this.lastDrawSplit);
				}
				this.lastDrawSplit = -1;
				return;
			}
			if (this.splitMove || this.splitBegin)
			{
				this.DrawSplitHelper(this.splitterDistance);
				this.lastDrawSplit = this.splitterDistance;
				return;
			}
			this.DrawSplitHelper(this.splitterDistance);
			this.lastDrawSplit = this.splitterDistance;
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x0012A5CB File Offset: 0x001295CB
		private void DrawFocus(Graphics g, Rectangle r)
		{
			r.Inflate(-1, -1);
			ControlPaint.DrawFocusRectangle(g, r, this.ForeColor, this.BackColor);
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x0012A5EC File Offset: 0x001295EC
		private void DrawSplitHelper(int splitSize)
		{
			Rectangle rectangle = this.CalcSplitLine(splitSize, 3);
			IntPtr handle = base.Handle;
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(this, handle), NativeMethods.NullHandleRef, 1026);
			IntPtr handle2 = ControlPaint.CreateHalftoneHBRUSH();
			IntPtr handle3 = SafeNativeMethods.SelectObject(new HandleRef(this, dcex), new HandleRef(null, handle2));
			SafeNativeMethods.PatBlt(new HandleRef(this, dcex), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 5898313);
			SafeNativeMethods.SelectObject(new HandleRef(this, dcex), new HandleRef(null, handle3));
			SafeNativeMethods.DeleteObject(new HandleRef(null, handle2));
			UnsafeNativeMethods.ReleaseDC(new HandleRef(this, handle), new HandleRef(null, dcex));
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x0012A6A0 File Offset: 0x001296A0
		private int GetSplitterDistance(int x, int y)
		{
			int num;
			if (this.Orientation == Orientation.Vertical)
			{
				num = x - this.anchor.X;
			}
			else
			{
				num = y - this.anchor.Y;
			}
			int val = 0;
			switch (this.Orientation)
			{
			case Orientation.Horizontal:
				val = Math.Max(this.panel1.Height + num, this.BORDERSIZE);
				break;
			case Orientation.Vertical:
				if (this.RightToLeft == RightToLeft.No)
				{
					val = Math.Max(this.panel1.Width + num, this.BORDERSIZE);
				}
				else
				{
					val = Math.Max(this.panel1.Width - num, this.BORDERSIZE);
				}
				break;
			}
			if (this.Orientation == Orientation.Vertical)
			{
				return Math.Max(Math.Min(val, base.Width - this.Panel2MinSize), this.Panel1MinSize);
			}
			return Math.Max(Math.Min(val, base.Height - this.Panel2MinSize), this.Panel1MinSize);
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x0012A78C File Offset: 0x0012978C
		private bool ProcessArrowKey(bool forward)
		{
			Control control = this;
			if (base.ActiveControl != null)
			{
				control = base.ActiveControl.ParentInternal;
			}
			return control.SelectNextControl(base.ActiveControl, forward, false, false, true);
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x0012A7C0 File Offset: 0x001297C0
		private void RepaintSplitterRect()
		{
			if (base.IsHandleCreated)
			{
				Graphics graphics = base.CreateGraphicsInternal();
				if (this.BackgroundImage != null)
				{
					using (TextureBrush textureBrush = new TextureBrush(this.BackgroundImage, WrapMode.Tile))
					{
						graphics.FillRectangle(textureBrush, base.ClientRectangle);
						goto IL_54;
					}
				}
				graphics.FillRectangle(new SolidBrush(this.BackColor), this.splitterRect);
				IL_54:
				graphics.Dispose();
			}
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x0012A838 File Offset: 0x00129838
		private void SetSplitterRect(bool vertical)
		{
			if (vertical)
			{
				this.splitterRect.X = ((this.RightToLeft == RightToLeft.Yes) ? (base.Width - this.splitterDistance - this.SplitterWidthInternal) : (base.Location.X + this.splitterDistance));
				this.splitterRect.Y = base.Location.Y;
				this.splitterRect.Width = this.SplitterWidthInternal;
				this.splitterRect.Height = base.Height;
				return;
			}
			this.splitterRect.X = base.Location.X;
			this.splitterRect.Y = base.Location.Y + this.SplitterDistanceInternal;
			this.splitterRect.Width = base.Width;
			this.splitterRect.Height = this.SplitterWidthInternal;
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x0012A920 File Offset: 0x00129920
		private void ResizeSplitContainer()
		{
			if (this.splitContainerScaling)
			{
				return;
			}
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			if (base.Width == 0)
			{
				this.panel1.Size = new Size(0, this.panel1.Height);
				this.panel2.Size = new Size(0, this.panel2.Height);
			}
			else if (base.Height == 0)
			{
				this.panel1.Size = new Size(this.panel1.Width, 0);
				this.panel2.Size = new Size(this.panel2.Width, 0);
			}
			else
			{
				if (this.Orientation == Orientation.Vertical)
				{
					if (!this.CollapsedMode)
					{
						if (this.FixedPanel == FixedPanel.Panel1)
						{
							this.panel1.Size = new Size(this.panelSize, base.Height);
							this.panel2.Size = new Size(Math.Max(base.Width - this.panelSize - this.SplitterWidthInternal, this.Panel2MinSize), base.Height);
						}
						if (this.FixedPanel == FixedPanel.Panel2)
						{
							this.panel2.Size = new Size(this.panelSize, base.Height);
							this.splitterDistance = Math.Max(base.Width - this.panelSize - this.SplitterWidthInternal, this.Panel1MinSize);
							this.panel1.WidthInternal = this.splitterDistance;
							this.panel1.HeightInternal = base.Height;
						}
						if (this.FixedPanel == FixedPanel.None)
						{
							if (this.ratioWidth != 0.0)
							{
								this.splitterDistance = Math.Max((int)Math.Floor((double)base.Width / this.ratioWidth), this.Panel1MinSize);
							}
							this.panel1.WidthInternal = this.splitterDistance;
							this.panel1.HeightInternal = base.Height;
							this.panel2.Size = new Size(Math.Max(base.Width - this.splitterDistance - this.SplitterWidthInternal, this.Panel2MinSize), base.Height);
						}
						if (this.RightToLeft == RightToLeft.No)
						{
							this.panel2.Location = new Point(this.panel1.WidthInternal + this.SplitterWidthInternal, 0);
						}
						else
						{
							this.panel1.Location = new Point(base.Width - this.panel1.WidthInternal, 0);
						}
						this.RepaintSplitterRect();
						this.SetSplitterRect(true);
					}
					else if (this.Panel1Collapsed)
					{
						this.panel2.Size = base.Size;
						this.panel2.Location = new Point(0, 0);
					}
					else if (this.Panel2Collapsed)
					{
						this.panel1.Size = base.Size;
						this.panel1.Location = new Point(0, 0);
					}
				}
				else if (this.Orientation == Orientation.Horizontal)
				{
					if (!this.CollapsedMode)
					{
						if (this.FixedPanel == FixedPanel.Panel1)
						{
							this.panel1.Size = new Size(base.Width, this.panelSize);
							int num = this.panelSize + this.SplitterWidthInternal;
							this.panel2.Size = new Size(base.Width, Math.Max(base.Height - num, this.Panel2MinSize));
							this.panel2.Location = new Point(0, num);
						}
						if (this.FixedPanel == FixedPanel.Panel2)
						{
							this.panel2.Size = new Size(base.Width, this.panelSize);
							this.splitterDistance = Math.Max(base.Height - this.Panel2.Height - this.SplitterWidthInternal, this.Panel1MinSize);
							this.panel1.HeightInternal = this.splitterDistance;
							this.panel1.WidthInternal = base.Width;
							int y = this.splitterDistance + this.SplitterWidthInternal;
							this.panel2.Location = new Point(0, y);
						}
						if (this.FixedPanel == FixedPanel.None)
						{
							if (this.ratioHeight != 0.0)
							{
								this.splitterDistance = Math.Max((int)Math.Floor((double)base.Height / this.ratioHeight), this.Panel1MinSize);
							}
							this.panel1.HeightInternal = this.splitterDistance;
							this.panel1.WidthInternal = base.Width;
							int num2 = this.splitterDistance + this.SplitterWidthInternal;
							this.panel2.Size = new Size(base.Width, Math.Max(base.Height - num2, this.Panel2MinSize));
							this.panel2.Location = new Point(0, num2);
						}
						this.RepaintSplitterRect();
						this.SetSplitterRect(false);
					}
					else if (this.Panel1Collapsed)
					{
						this.panel2.Size = base.Size;
						this.panel2.Location = new Point(0, 0);
					}
					else if (this.Panel2Collapsed)
					{
						this.panel1.Size = base.Size;
						this.panel1.Location = new Point(0, 0);
					}
				}
				try
				{
					this.resizeCalled = true;
					this.ApplySplitterDistance();
				}
				finally
				{
					this.resizeCalled = false;
				}
			}
			this.panel1.ResumeLayout();
			this.panel2.ResumeLayout();
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x0012AE78 File Offset: 0x00129E78
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			try
			{
				this.splitContainerScaling = true;
				base.ScaleControl(factor, specified);
				float num;
				if (this.orientation == Orientation.Vertical)
				{
					num = factor.Width;
				}
				else
				{
					num = factor.Height;
				}
				this.SplitterWidth = (int)Math.Round((double)((float)this.SplitterWidth * num));
			}
			finally
			{
				this.splitContainerScaling = false;
			}
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x0012AEE0 File Offset: 0x00129EE0
		protected override void Select(bool directed, bool forward)
		{
			if (this.selectNextControl)
			{
				return;
			}
			if (this.Panel1.Controls.Count > 0 || this.Panel2.Controls.Count > 0 || this.tabStop)
			{
				this.SelectNextControlInContainer(this, forward, true, true, false);
				return;
			}
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				try
				{
					this.selectNextControl = true;
					parentInternal.SelectNextControl(this, forward, true, true, true);
				}
				finally
				{
					this.selectNextControl = false;
				}
			}
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x0012AF68 File Offset: 0x00129F68
		private bool SelectNextControlInContainer(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			if (!base.Contains(ctl) || (!nested && ctl.ParentInternal != this))
			{
				ctl = null;
			}
			SplitterPanel splitterPanel = null;
			for (;;)
			{
				ctl = base.GetNextControl(ctl, forward);
				SplitterPanel splitterPanel2 = ctl as SplitterPanel;
				if (splitterPanel2 != null && splitterPanel2.Visible)
				{
					if (splitterPanel != null)
					{
						goto IL_8B;
					}
					splitterPanel = splitterPanel2;
				}
				if (!forward && splitterPanel != null && ctl.ParentInternal != splitterPanel)
				{
					break;
				}
				if (ctl == null)
				{
					goto IL_8B;
				}
				if (ctl.CanSelect && ctl.TabStop)
				{
					goto Block_11;
				}
				if (ctl == null)
				{
					goto IL_8B;
				}
			}
			ctl = splitterPanel;
			goto IL_8B;
			Block_11:
			if (ctl is SplitContainer)
			{
				((SplitContainer)ctl).Select(forward, forward);
			}
			else
			{
				SplitContainer.SelectNextActiveControl(ctl, forward, tabStopOnly, nested, wrap);
			}
			return true;
			IL_8B:
			if (ctl != null && this.TabStop)
			{
				this.splitterFocused = true;
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					ContainerControl containerControl = containerControlInternal as ContainerControl;
					if (containerControl == null)
					{
						containerControlInternal.ActiveControl = this;
					}
					else
					{
						IntSecurity.ModifyFocus.Demand();
						containerControl.SetActiveControlInternal(this);
					}
				}
				base.SetActiveControlInternal(null);
				this.nextActiveControl = ctl;
				return true;
			}
			if (!this.SelectNextControlInPanel(ctl, forward, tabStopOnly, nested, wrap))
			{
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					try
					{
						this.selectNextControl = true;
						parentInternal.SelectNextControl(this, forward, true, true, true);
					}
					finally
					{
						this.selectNextControl = false;
					}
				}
			}
			return false;
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x0012B0A4 File Offset: 0x0012A0A4
		private bool SelectNextControlInPanel(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			if (!base.Contains(ctl) || (!nested && ctl.ParentInternal != this))
			{
				ctl = null;
			}
			for (;;)
			{
				ctl = base.GetNextControl(ctl, forward);
				if (ctl == null || (ctl is SplitterPanel && ctl.Visible))
				{
					goto IL_71;
				}
				if (ctl.CanSelect && (!tabStopOnly || ctl.TabStop))
				{
					break;
				}
				if (ctl == null)
				{
					goto IL_71;
				}
			}
			if (ctl is SplitContainer)
			{
				((SplitContainer)ctl).Select(forward, forward);
			}
			else
			{
				SplitContainer.SelectNextActiveControl(ctl, forward, tabStopOnly, nested, wrap);
			}
			return true;
			IL_71:
			if (ctl == null || (ctl is SplitterPanel && !ctl.Visible))
			{
				this.callBaseVersion = true;
			}
			else
			{
				ctl = base.GetNextControl(ctl, forward);
				if (forward)
				{
					this.nextActiveControl = this.panel2;
				}
				else if (ctl == null || !ctl.ParentInternal.Visible)
				{
					this.callBaseVersion = true;
				}
				else
				{
					this.nextActiveControl = this.panel2;
				}
			}
			return false;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x0012B180 File Offset: 0x0012A180
		private static void SelectNextActiveControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			ContainerControl containerControl = ctl as ContainerControl;
			if (containerControl != null)
			{
				bool flag = true;
				if (containerControl.ParentInternal != null)
				{
					IContainerControl containerControlInternal = containerControl.ParentInternal.GetContainerControlInternal();
					if (containerControlInternal != null)
					{
						containerControlInternal.ActiveControl = containerControl;
						flag = (containerControlInternal.ActiveControl == containerControl);
					}
				}
				if (flag)
				{
					ctl.SelectNextControl(null, forward, tabStopOnly, nested, wrap);
					return;
				}
			}
			else
			{
				ctl.Select();
			}
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x0012B1D8 File Offset: 0x0012A1D8
		private void SetInnerMostBorder(SplitContainer sc)
		{
			foreach (object obj in sc.Controls)
			{
				Control control = (Control)obj;
				bool flag = false;
				if (control is SplitterPanel)
				{
					foreach (object obj2 in control.Controls)
					{
						Control control2 = (Control)obj2;
						SplitContainer splitContainer = control2 as SplitContainer;
						if (splitContainer != null && splitContainer.Dock == DockStyle.Fill)
						{
							if (splitContainer.BorderStyle != this.BorderStyle)
							{
								break;
							}
							((SplitterPanel)control).BorderStyle = BorderStyle.None;
							this.SetInnerMostBorder(splitContainer);
							flag = true;
						}
					}
					if (!flag)
					{
						((SplitterPanel)control).BorderStyle = this.BorderStyle;
					}
				}
			}
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0012B2D8 File Offset: 0x0012A2D8
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None && this.Orientation == Orientation.Horizontal && height < this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize)
			{
				height = this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize;
			}
			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None && this.Orientation == Orientation.Vertical && width < this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize)
			{
				width = this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize;
			}
			base.SetBoundsCore(x, y, width, height, specified);
			this.SetSplitterRect(this.Orientation == Orientation.Vertical);
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x0012B37C File Offset: 0x0012A37C
		private void SplitBegin(int x, int y)
		{
			this.anchor = new Point(x, y);
			this.splitterDistance = this.GetSplitterDistance(x, y);
			this.initialSplitterDistance = this.splitterDistance;
			this.initialSplitterRectangle = this.SplitterRectangle;
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				if (this.splitContainerMessageFilter == null)
				{
					this.splitContainerMessageFilter = new SplitContainer.SplitContainerMessageFilter(this);
				}
				Application.AddMessageFilter(this.splitContainerMessageFilter);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			base.CaptureInternal = true;
			this.DrawSplitBar(1);
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x0012B40C File Offset: 0x0012A40C
		private void SplitMove(int x, int y)
		{
			int num = this.GetSplitterDistance(x, y);
			int num2 = num - this.initialSplitterDistance;
			int num3 = num2 % this.SplitterIncrement;
			if (this.splitterDistance != num)
			{
				if (this.Orientation == Orientation.Vertical)
				{
					if (num + this.SplitterWidthInternal <= base.Width - this.Panel2MinSize - this.BORDERSIZE)
					{
						this.splitterDistance = num - num3;
					}
				}
				else if (num + this.SplitterWidthInternal <= base.Height - this.Panel2MinSize - this.BORDERSIZE)
				{
					this.splitterDistance = num - num3;
				}
			}
			this.DrawSplitBar(2);
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x0012B4A0 File Offset: 0x0012A4A0
		private void SplitEnd(bool accept)
		{
			this.DrawSplitBar(3);
			if (this.splitContainerMessageFilter != null)
			{
				Application.RemoveMessageFilter(this.splitContainerMessageFilter);
				this.splitContainerMessageFilter = null;
			}
			if (accept)
			{
				this.ApplySplitterDistance();
			}
			else if (this.splitterDistance != this.initialSplitterDistance)
			{
				this.splitterClick = false;
				this.splitterDistance = (this.SplitterDistanceInternal = this.initialSplitterDistance);
			}
			this.anchor = Point.Empty;
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x0012B510 File Offset: 0x0012A510
		private void UpdateSplitter()
		{
			if (this.splitContainerScaling)
			{
				return;
			}
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			if (this.Orientation == Orientation.Vertical)
			{
				bool flag = this.RightToLeft == RightToLeft.Yes;
				if (!this.CollapsedMode)
				{
					this.panel1.HeightInternal = base.Height;
					this.panel1.WidthInternal = this.splitterDistance;
					this.panel2.Size = new Size(base.Width - this.splitterDistance - this.SplitterWidthInternal, base.Height);
					if (!flag)
					{
						this.panel1.Location = new Point(0, 0);
						this.panel2.Location = new Point(this.splitterDistance + this.SplitterWidthInternal, 0);
					}
					else
					{
						this.panel1.Location = new Point(base.Width - this.splitterDistance, 0);
						this.panel2.Location = new Point(0, 0);
					}
					this.RepaintSplitterRect();
					this.SetSplitterRect(true);
					if (!this.resizeCalled)
					{
						this.ratioWidth = (((double)base.Width / (double)this.panel1.Width > 0.0) ? ((double)base.Width / (double)this.panel1.Width) : this.ratioWidth);
					}
				}
				else
				{
					if (this.Panel1Collapsed)
					{
						this.panel2.Size = base.Size;
						this.panel2.Location = new Point(0, 0);
					}
					else if (this.Panel2Collapsed)
					{
						this.panel1.Size = base.Size;
						this.panel1.Location = new Point(0, 0);
					}
					if (!this.resizeCalled)
					{
						this.ratioWidth = (((double)base.Width / (double)this.splitterDistance > 0.0) ? ((double)base.Width / (double)this.splitterDistance) : this.ratioWidth);
					}
				}
			}
			else if (!this.CollapsedMode)
			{
				this.panel1.Location = new Point(0, 0);
				this.panel1.WidthInternal = base.Width;
				this.panel1.HeightInternal = this.SplitterDistanceInternal;
				int num = this.splitterDistance + this.SplitterWidthInternal;
				this.panel2.Size = new Size(base.Width, base.Height - num);
				this.panel2.Location = new Point(0, num);
				this.RepaintSplitterRect();
				this.SetSplitterRect(false);
				if (!this.resizeCalled)
				{
					this.ratioHeight = (((double)base.Height / (double)this.panel1.Height > 0.0) ? ((double)base.Height / (double)this.panel1.Height) : this.ratioHeight);
				}
			}
			else
			{
				if (this.Panel1Collapsed)
				{
					this.panel2.Size = base.Size;
					this.panel2.Location = new Point(0, 0);
				}
				else if (this.Panel2Collapsed)
				{
					this.panel1.Size = base.Size;
					this.panel1.Location = new Point(0, 0);
				}
				if (!this.resizeCalled)
				{
					this.ratioHeight = (((double)base.Height / (double)this.splitterDistance > 0.0) ? ((double)base.Height / (double)this.splitterDistance) : this.ratioHeight);
				}
			}
			this.panel1.ResumeLayout();
			this.panel2.ResumeLayout();
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x0012B894 File Offset: 0x0012A894
		private void WmSetCursor(ref Message m)
		{
			if (!(m.WParam == base.InternalHandle) || ((int)m.LParam & 65535) != 1)
			{
				this.DefWndProc(ref m);
				return;
			}
			if (this.OverrideCursor != null)
			{
				Cursor.CurrentInternal = this.OverrideCursor;
				return;
			}
			Cursor.CurrentInternal = this.Cursor;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x0012B8F5 File Offset: 0x0012A8F5
		internal override void AfterControlRemoved(Control control, Control oldParent)
		{
			base.AfterControlRemoved(control, oldParent);
			if (control is SplitContainer && control.Dock == DockStyle.Fill)
			{
				this.SetInnerMostBorder(this);
			}
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x0012B918 File Offset: 0x0012A918
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				Keys keys2 = keys;
				if (keys2 != Keys.Tab)
				{
					switch (keys2)
					{
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
						if (this.splitterFocused)
						{
							return false;
						}
						if (this.ProcessArrowKey(keys == Keys.Right || keys == Keys.Down))
						{
							return true;
						}
						break;
					}
				}
				else if (this.ProcessTabKey((keyData & Keys.Shift) == Keys.None))
				{
					return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x0012B994 File Offset: 0x0012A994
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessTabKey(bool forward)
		{
			if (!this.tabStop || this.IsSplitterFixed)
			{
				return base.ProcessTabKey(forward);
			}
			if (this.nextActiveControl != null)
			{
				base.SetActiveControlInternal(this.nextActiveControl);
				this.nextActiveControl = null;
			}
			if (this.SelectNextControlInPanel(base.ActiveControl, forward, true, true, true))
			{
				this.nextActiveControl = null;
				this.splitterFocused = false;
				return true;
			}
			if (this.callBaseVersion)
			{
				this.callBaseVersion = false;
				return base.ProcessTabKey(forward);
			}
			this.splitterFocused = true;
			IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
			if (containerControlInternal != null)
			{
				ContainerControl containerControl = containerControlInternal as ContainerControl;
				if (containerControl == null)
				{
					containerControlInternal.ActiveControl = this;
				}
				else
				{
					containerControl.SetActiveControlInternal(this);
				}
			}
			base.SetActiveControlInternal(null);
			return true;
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x0012BA45 File Offset: 0x0012AA45
		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			base.OnMouseCaptureChanged(e);
			if (this.splitContainerMessageFilter != null)
			{
				Application.RemoveMessageFilter(this.splitContainerMessageFilter);
				this.splitContainerMessageFilter = null;
			}
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x0012BA68 File Offset: 0x0012AA68
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			switch (msg2)
			{
			case 7:
				this.splitterFocused = true;
				base.WndProc(ref msg);
				return;
			case 8:
				this.splitterFocused = false;
				base.WndProc(ref msg);
				return;
			default:
				if (msg2 == 32)
				{
					this.WmSetCursor(ref msg);
					return;
				}
				base.WndProc(ref msg);
				return;
			}
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x0012BABE File Offset: 0x0012AABE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new SplitContainer.SplitContainerTypedControlCollection(this, typeof(SplitterPanel), true);
		}

		// Token: 0x040035CD RID: 13773
		private const int DRAW_START = 1;

		// Token: 0x040035CE RID: 13774
		private const int DRAW_MOVE = 2;

		// Token: 0x040035CF RID: 13775
		private const int DRAW_END = 3;

		// Token: 0x040035D0 RID: 13776
		private const int rightBorder = 5;

		// Token: 0x040035D1 RID: 13777
		private const int leftBorder = 2;

		// Token: 0x040035D2 RID: 13778
		private int BORDERSIZE;

		// Token: 0x040035D3 RID: 13779
		private Orientation orientation = Orientation.Vertical;

		// Token: 0x040035D4 RID: 13780
		private SplitterPanel panel1;

		// Token: 0x040035D5 RID: 13781
		private SplitterPanel panel2;

		// Token: 0x040035D6 RID: 13782
		private BorderStyle borderStyle;

		// Token: 0x040035D7 RID: 13783
		private FixedPanel fixedPanel;

		// Token: 0x040035D8 RID: 13784
		private int panel1MinSize = 25;

		// Token: 0x040035D9 RID: 13785
		private int panel2MinSize = 25;

		// Token: 0x040035DA RID: 13786
		private bool tabStop = true;

		// Token: 0x040035DB RID: 13787
		private int panelSize;

		// Token: 0x040035DC RID: 13788
		private Rectangle splitterRect;

		// Token: 0x040035DD RID: 13789
		private int splitterInc = 1;

		// Token: 0x040035DE RID: 13790
		private bool splitterFixed;

		// Token: 0x040035DF RID: 13791
		private int splitterDistance = 50;

		// Token: 0x040035E0 RID: 13792
		private int splitterWidth = 4;

		// Token: 0x040035E1 RID: 13793
		private int splitDistance = 50;

		// Token: 0x040035E2 RID: 13794
		private int lastDrawSplit = 1;

		// Token: 0x040035E3 RID: 13795
		private int initialSplitterDistance;

		// Token: 0x040035E4 RID: 13796
		private Rectangle initialSplitterRectangle;

		// Token: 0x040035E5 RID: 13797
		private Point anchor = Point.Empty;

		// Token: 0x040035E6 RID: 13798
		private bool splitBegin;

		// Token: 0x040035E7 RID: 13799
		private bool splitMove;

		// Token: 0x040035E8 RID: 13800
		private bool splitBreak;

		// Token: 0x040035E9 RID: 13801
		private Cursor overrideCursor;

		// Token: 0x040035EA RID: 13802
		private Control nextActiveControl;

		// Token: 0x040035EB RID: 13803
		private bool callBaseVersion;

		// Token: 0x040035EC RID: 13804
		private bool splitterFocused;

		// Token: 0x040035ED RID: 13805
		private bool splitterClick;

		// Token: 0x040035EE RID: 13806
		private bool splitterDrag;

		// Token: 0x040035EF RID: 13807
		private double ratioWidth;

		// Token: 0x040035F0 RID: 13808
		private double ratioHeight;

		// Token: 0x040035F1 RID: 13809
		private bool resizeCalled;

		// Token: 0x040035F2 RID: 13810
		private bool splitContainerScaling;

		// Token: 0x040035F3 RID: 13811
		private bool setSplitterDistance;

		// Token: 0x040035F4 RID: 13812
		private static readonly object EVENT_MOVING = new object();

		// Token: 0x040035F5 RID: 13813
		private static readonly object EVENT_MOVED = new object();

		// Token: 0x040035F6 RID: 13814
		private SplitContainer.SplitContainerMessageFilter splitContainerMessageFilter;

		// Token: 0x040035F7 RID: 13815
		private bool selectNextControl;

		// Token: 0x02000616 RID: 1558
		private class SplitContainerMessageFilter : IMessageFilter
		{
			// Token: 0x06005183 RID: 20867 RVA: 0x0012BAE7 File Offset: 0x0012AAE7
			public SplitContainerMessageFilter(SplitContainer splitContainer)
			{
				this.owner = splitContainer;
			}

			// Token: 0x06005184 RID: 20868 RVA: 0x0012BAF8 File Offset: 0x0012AAF8
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			bool IMessageFilter.PreFilterMessage(ref Message m)
			{
				if (m.Msg >= 256 && m.Msg <= 264)
				{
					if ((m.Msg == 256 && (int)m.WParam == 27) || m.Msg == 260)
					{
						this.owner.splitBegin = false;
						this.owner.SplitEnd(false);
						this.owner.splitterClick = false;
						this.owner.splitterDrag = false;
					}
					return true;
				}
				return false;
			}

			// Token: 0x040035F8 RID: 13816
			private SplitContainer owner;
		}

		// Token: 0x0200061D RID: 1565
		internal class SplitContainerTypedControlCollection : WindowsFormsUtils.TypedControlCollection
		{
			// Token: 0x060051AE RID: 20910 RVA: 0x0012C4E9 File Offset: 0x0012B4E9
			public SplitContainerTypedControlCollection(Control c, Type type, bool isReadOnly) : base(c, type, isReadOnly)
			{
				this.owner = (c as SplitContainer);
			}

			// Token: 0x060051AF RID: 20911 RVA: 0x0012C500 File Offset: 0x0012B500
			public override void Remove(Control value)
			{
				if (value is SplitterPanel && !this.owner.DesignMode && this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				base.Remove(value);
			}

			// Token: 0x060051B0 RID: 20912 RVA: 0x0012C536 File Offset: 0x0012B536
			internal override void SetChildIndexInternal(Control child, int newIndex)
			{
				if (child is SplitterPanel)
				{
					if (this.owner.DesignMode)
					{
						return;
					}
					if (this.IsReadOnly)
					{
						throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
					}
				}
				base.SetChildIndexInternal(child, newIndex);
			}

			// Token: 0x04003609 RID: 13833
			private SplitContainer owner;
		}
	}
}
