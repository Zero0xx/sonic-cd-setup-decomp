using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000625 RID: 1573
	[Designer("System.Windows.Forms.Design.SplitterPanelDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[ToolboxItem(false)]
	[Docking(DockingBehavior.Never)]
	public sealed class SplitterPanel : Panel
	{
		// Token: 0x0600521C RID: 21020 RVA: 0x0012D502 File Offset: 0x0012C502
		public SplitterPanel(SplitContainer owner)
		{
			this.owner = owner;
			base.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x0600521D RID: 21021 RVA: 0x0012D51A File Offset: 0x0012C51A
		// (set) Token: 0x0600521E RID: 21022 RVA: 0x0012D522 File Offset: 0x0012C522
		internal bool Collapsed
		{
			get
			{
				return this.collapsed;
			}
			set
			{
				this.collapsed = value;
			}
		}

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x0600521F RID: 21023 RVA: 0x0012D52B File Offset: 0x0012C52B
		// (set) Token: 0x06005220 RID: 21024 RVA: 0x0012D533 File Offset: 0x0012C533
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new bool AutoSize
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

		// Token: 0x14000307 RID: 775
		// (add) Token: 0x06005221 RID: 21025 RVA: 0x0012D53C File Offset: 0x0012C53C
		// (remove) Token: 0x06005222 RID: 21026 RVA: 0x0012D545 File Offset: 0x0012C545
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

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x0012D54E File Offset: 0x0012C54E
		// (set) Token: 0x06005224 RID: 21028 RVA: 0x0012D551 File Offset: 0x0012C551
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[Localizable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override AutoSizeMode AutoSizeMode
		{
			get
			{
				return AutoSizeMode.GrowOnly;
			}
			set
			{
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06005225 RID: 21029 RVA: 0x0012D553 File Offset: 0x0012C553
		// (set) Token: 0x06005226 RID: 21030 RVA: 0x0012D55B File Offset: 0x0012C55B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				base.Anchor = value;
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06005227 RID: 21031 RVA: 0x0012D564 File Offset: 0x0012C564
		// (set) Token: 0x06005228 RID: 21032 RVA: 0x0012D56C File Offset: 0x0012C56C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06005229 RID: 21033 RVA: 0x0012D575 File Offset: 0x0012C575
		// (set) Token: 0x0600522A RID: 21034 RVA: 0x0012D57D File Offset: 0x0012C57D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new DockStyle Dock
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

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x0012D586 File Offset: 0x0012C586
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x0600522C RID: 21036 RVA: 0x0012D58E File Offset: 0x0012C58E
		// (set) Token: 0x0600522D RID: 21037 RVA: 0x0012D5A0 File Offset: 0x0012C5A0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[SRDescription("ControlHeightDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new int Height
		{
			get
			{
				if (this.Collapsed)
				{
					return 0;
				}
				return base.Height;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("SplitContainerPanelHeight"));
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x0600522E RID: 21038 RVA: 0x0012D5B1 File Offset: 0x0012C5B1
		// (set) Token: 0x0600522F RID: 21039 RVA: 0x0012D5B9 File Offset: 0x0012C5B9
		internal int HeightInternal
		{
			get
			{
				return base.Height;
			}
			set
			{
				base.Height = value;
			}
		}

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06005230 RID: 21040 RVA: 0x0012D5C2 File Offset: 0x0012C5C2
		// (set) Token: 0x06005231 RID: 21041 RVA: 0x0012D5CA File Offset: 0x0012C5CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
			}
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x0012D5D3 File Offset: 0x0012C5D3
		protected override Padding DefaultMargin
		{
			get
			{
				return new Padding(0, 0, 0, 0);
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x0012D5DE File Offset: 0x0012C5DE
		// (set) Token: 0x06005234 RID: 21044 RVA: 0x0012D5E6 File Offset: 0x0012C5E6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size MinimumSize
		{
			get
			{
				return base.MinimumSize;
			}
			set
			{
				base.MinimumSize = value;
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06005235 RID: 21045 RVA: 0x0012D5EF File Offset: 0x0012C5EF
		// (set) Token: 0x06005236 RID: 21046 RVA: 0x0012D5F7 File Offset: 0x0012C5F7
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size MaximumSize
		{
			get
			{
				return base.MaximumSize;
			}
			set
			{
				base.MaximumSize = value;
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06005237 RID: 21047 RVA: 0x0012D600 File Offset: 0x0012C600
		// (set) Token: 0x06005238 RID: 21048 RVA: 0x0012D608 File Offset: 0x0012C608
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x0012D611 File Offset: 0x0012C611
		internal SplitContainer Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x0600523A RID: 21050 RVA: 0x0012D619 File Offset: 0x0012C619
		// (set) Token: 0x0600523B RID: 21051 RVA: 0x0012D621 File Offset: 0x0012C621
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Control Parent
		{
			get
			{
				return base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x0600523C RID: 21052 RVA: 0x0012D62A File Offset: 0x0012C62A
		// (set) Token: 0x0600523D RID: 21053 RVA: 0x0012D640 File Offset: 0x0012C640
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size Size
		{
			get
			{
				if (this.Collapsed)
				{
					return Size.Empty;
				}
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x0600523E RID: 21054 RVA: 0x0012D649 File Offset: 0x0012C649
		// (set) Token: 0x0600523F RID: 21055 RVA: 0x0012D651 File Offset: 0x0012C651
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x06005240 RID: 21056 RVA: 0x0012D65A File Offset: 0x0012C65A
		// (set) Token: 0x06005241 RID: 21057 RVA: 0x0012D662 File Offset: 0x0012C662
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x06005242 RID: 21058 RVA: 0x0012D66B File Offset: 0x0012C66B
		// (set) Token: 0x06005243 RID: 21059 RVA: 0x0012D673 File Offset: 0x0012C673
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x06005244 RID: 21060 RVA: 0x0012D67C File Offset: 0x0012C67C
		// (set) Token: 0x06005245 RID: 21061 RVA: 0x0012D68E File Offset: 0x0012C68E
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("ControlWidthDescr")]
		public new int Width
		{
			get
			{
				if (this.Collapsed)
				{
					return 0;
				}
				return base.Width;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("SplitContainerPanelWidth"));
			}
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x06005246 RID: 21062 RVA: 0x0012D69F File Offset: 0x0012C69F
		// (set) Token: 0x06005247 RID: 21063 RVA: 0x0012D6A7 File Offset: 0x0012C6A7
		internal int WidthInternal
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
			}
		}

		// Token: 0x14000308 RID: 776
		// (add) Token: 0x06005248 RID: 21064 RVA: 0x0012D6B0 File Offset: 0x0012C6B0
		// (remove) Token: 0x06005249 RID: 21065 RVA: 0x0012D6B9 File Offset: 0x0012C6B9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler VisibleChanged
		{
			add
			{
				base.VisibleChanged += value;
			}
			remove
			{
				base.VisibleChanged -= value;
			}
		}

		// Token: 0x14000309 RID: 777
		// (add) Token: 0x0600524A RID: 21066 RVA: 0x0012D6C2 File Offset: 0x0012C6C2
		// (remove) Token: 0x0600524B RID: 21067 RVA: 0x0012D6CB File Offset: 0x0012C6CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler DockChanged
		{
			add
			{
				base.DockChanged += value;
			}
			remove
			{
				base.DockChanged -= value;
			}
		}

		// Token: 0x1400030A RID: 778
		// (add) Token: 0x0600524C RID: 21068 RVA: 0x0012D6D4 File Offset: 0x0012C6D4
		// (remove) Token: 0x0600524D RID: 21069 RVA: 0x0012D6DD File Offset: 0x0012C6DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler LocationChanged
		{
			add
			{
				base.LocationChanged += value;
			}
			remove
			{
				base.LocationChanged -= value;
			}
		}

		// Token: 0x1400030B RID: 779
		// (add) Token: 0x0600524E RID: 21070 RVA: 0x0012D6E6 File Offset: 0x0012C6E6
		// (remove) Token: 0x0600524F RID: 21071 RVA: 0x0012D6EF File Offset: 0x0012C6EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler TabIndexChanged
		{
			add
			{
				base.TabIndexChanged += value;
			}
			remove
			{
				base.TabIndexChanged -= value;
			}
		}

		// Token: 0x1400030C RID: 780
		// (add) Token: 0x06005250 RID: 21072 RVA: 0x0012D6F8 File Offset: 0x0012C6F8
		// (remove) Token: 0x06005251 RID: 21073 RVA: 0x0012D701 File Offset: 0x0012C701
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x04003627 RID: 13863
		private SplitContainer owner;

		// Token: 0x04003628 RID: 13864
		private bool collapsed;
	}
}
