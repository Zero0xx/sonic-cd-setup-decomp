using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200061E RID: 1566
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.SplitterDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("SplitterMoved")]
	[DefaultProperty("Dock")]
	[SRDescription("DescriptionSplitter")]
	public class Splitter : Control
	{
		// Token: 0x060051B1 RID: 20913 RVA: 0x0012C570 File Offset: 0x0012B570
		public Splitter()
		{
			base.SetStyle(ControlStyles.Selectable, false);
			this.TabStop = false;
			this.minSize = 25;
			this.minExtra = 25;
			this.Dock = DockStyle.Left;
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x060051B2 RID: 20914 RVA: 0x0012C5DD File Offset: 0x0012B5DD
		// (set) Token: 0x060051B3 RID: 20915 RVA: 0x0012C5E0 File Offset: 0x0012B5E0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(AnchorStyles.None)]
		public override AnchorStyles Anchor
		{
			get
			{
				return AnchorStyles.None;
			}
			set
			{
			}
		}

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x060051B4 RID: 20916 RVA: 0x0012C5E2 File Offset: 0x0012B5E2
		// (set) Token: 0x060051B5 RID: 20917 RVA: 0x0012C5EA File Offset: 0x0012B5EA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x060051B6 RID: 20918 RVA: 0x0012C5F3 File Offset: 0x0012B5F3
		protected override Size DefaultSize
		{
			get
			{
				return new Size(3, 3);
			}
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x060051B7 RID: 20919 RVA: 0x0012C5FC File Offset: 0x0012B5FC
		protected override Cursor DefaultCursor
		{
			get
			{
				switch (this.Dock)
				{
				case DockStyle.Top:
				case DockStyle.Bottom:
					return Cursors.HSplit;
				case DockStyle.Left:
				case DockStyle.Right:
					return Cursors.VSplit;
				default:
					return base.DefaultCursor;
				}
			}
		}

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x060051B8 RID: 20920 RVA: 0x0012C63C File Offset: 0x0012B63C
		// (set) Token: 0x060051B9 RID: 20921 RVA: 0x0012C644 File Offset: 0x0012B644
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x140002F9 RID: 761
		// (add) Token: 0x060051BA RID: 20922 RVA: 0x0012C64D File Offset: 0x0012B64D
		// (remove) Token: 0x060051BB RID: 20923 RVA: 0x0012C656 File Offset: 0x0012B656
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x060051BC RID: 20924 RVA: 0x0012C65F File Offset: 0x0012B65F
		// (set) Token: 0x060051BD RID: 20925 RVA: 0x0012C667 File Offset: 0x0012B667
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x140002FA RID: 762
		// (add) Token: 0x060051BE RID: 20926 RVA: 0x0012C670 File Offset: 0x0012B670
		// (remove) Token: 0x060051BF RID: 20927 RVA: 0x0012C679 File Offset: 0x0012B679
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x060051C0 RID: 20928 RVA: 0x0012C682 File Offset: 0x0012B682
		// (set) Token: 0x060051C1 RID: 20929 RVA: 0x0012C68A File Offset: 0x0012B68A
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

		// Token: 0x140002FB RID: 763
		// (add) Token: 0x060051C2 RID: 20930 RVA: 0x0012C693 File Offset: 0x0012B693
		// (remove) Token: 0x060051C3 RID: 20931 RVA: 0x0012C69C File Offset: 0x0012B69C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x060051C4 RID: 20932 RVA: 0x0012C6A5 File Offset: 0x0012B6A5
		// (set) Token: 0x060051C5 RID: 20933 RVA: 0x0012C6AD File Offset: 0x0012B6AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		// Token: 0x140002FC RID: 764
		// (add) Token: 0x060051C6 RID: 20934 RVA: 0x0012C6B6 File Offset: 0x0012B6B6
		// (remove) Token: 0x060051C7 RID: 20935 RVA: 0x0012C6BF File Offset: 0x0012B6BF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler FontChanged
		{
			add
			{
				base.FontChanged += value;
			}
			remove
			{
				base.FontChanged -= value;
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x060051C8 RID: 20936 RVA: 0x0012C6C8 File Offset: 0x0012B6C8
		// (set) Token: 0x060051C9 RID: 20937 RVA: 0x0012C6D0 File Offset: 0x0012B6D0
		[DispId(-504)]
		[SRCategory("CatAppearance")]
		[SRDescription("SplitterBorderStyleDescr")]
		[DefaultValue(BorderStyle.None)]
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
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x060051CA RID: 20938 RVA: 0x0012C710 File Offset: 0x0012B710
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
				switch (this.borderStyle)
				{
				case BorderStyle.FixedSingle:
					createParams.Style |= 8388608;
					break;
				case BorderStyle.Fixed3D:
					createParams.ExStyle |= 512;
					break;
				}
				return createParams;
			}
		}

		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x060051CB RID: 20939 RVA: 0x0012C788 File Offset: 0x0012B788
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x060051CC RID: 20940 RVA: 0x0012C78B File Offset: 0x0012B78B
		// (set) Token: 0x060051CD RID: 20941 RVA: 0x0012C794 File Offset: 0x0012B794
		[DefaultValue(DockStyle.Left)]
		[Localizable(true)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				if (value != DockStyle.Top && value != DockStyle.Bottom && value != DockStyle.Left && value != DockStyle.Right)
				{
					throw new ArgumentException(SR.GetString("SplitterInvalidDockEnum"));
				}
				int num = this.splitterThickness;
				base.Dock = value;
				switch (this.Dock)
				{
				case DockStyle.Top:
				case DockStyle.Bottom:
					if (this.splitterThickness != -1)
					{
						base.Height = num;
						return;
					}
					break;
				case DockStyle.Left:
				case DockStyle.Right:
					if (this.splitterThickness != -1)
					{
						base.Width = num;
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x060051CE RID: 20942 RVA: 0x0012C810 File Offset: 0x0012B810
		private bool Horizontal
		{
			get
			{
				DockStyle dock = this.Dock;
				return dock == DockStyle.Left || dock == DockStyle.Right;
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x060051CF RID: 20943 RVA: 0x0012C82E File Offset: 0x0012B82E
		// (set) Token: 0x060051D0 RID: 20944 RVA: 0x0012C836 File Offset: 0x0012B836
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		// Token: 0x140002FD RID: 765
		// (add) Token: 0x060051D1 RID: 20945 RVA: 0x0012C83F File Offset: 0x0012B83F
		// (remove) Token: 0x060051D2 RID: 20946 RVA: 0x0012C848 File Offset: 0x0012B848
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x060051D3 RID: 20947 RVA: 0x0012C851 File Offset: 0x0012B851
		// (set) Token: 0x060051D4 RID: 20948 RVA: 0x0012C859 File Offset: 0x0012B859
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(25)]
		[SRDescription("SplitterMinExtraDescr")]
		public int MinExtra
		{
			get
			{
				return this.minExtra;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.minExtra = value;
			}
		}

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x060051D5 RID: 20949 RVA: 0x0012C869 File Offset: 0x0012B869
		// (set) Token: 0x060051D6 RID: 20950 RVA: 0x0012C871 File Offset: 0x0012B871
		[SRDescription("SplitterMinSizeDescr")]
		[DefaultValue(25)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		public int MinSize
		{
			get
			{
				return this.minSize;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.minSize = value;
			}
		}

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x060051D7 RID: 20951 RVA: 0x0012C881 File Offset: 0x0012B881
		// (set) Token: 0x060051D8 RID: 20952 RVA: 0x0012C8A0 File Offset: 0x0012B8A0
		[Browsable(false)]
		[SRDescription("SplitterSplitPositionDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		public int SplitPosition
		{
			get
			{
				if (this.splitSize == -1)
				{
					this.splitSize = this.CalcSplitSize();
				}
				return this.splitSize;
			}
			set
			{
				Splitter.SplitData splitData = this.CalcSplitBounds();
				if (value > this.maxSize)
				{
					value = this.maxSize;
				}
				if (value < this.minSize)
				{
					value = this.minSize;
				}
				this.splitSize = value;
				this.DrawSplitBar(3);
				if (splitData.target == null)
				{
					this.splitSize = -1;
					return;
				}
				Rectangle bounds = splitData.target.Bounds;
				switch (this.Dock)
				{
				case DockStyle.Top:
					bounds.Height = value;
					break;
				case DockStyle.Bottom:
					bounds.Y += bounds.Height - this.splitSize;
					bounds.Height = value;
					break;
				case DockStyle.Left:
					bounds.Width = value;
					break;
				case DockStyle.Right:
					bounds.X += bounds.Width - this.splitSize;
					bounds.Width = value;
					break;
				}
				splitData.target.Bounds = bounds;
				Application.DoEvents();
				this.OnSplitterMoved(new SplitterEventArgs(base.Left, base.Top, base.Left + bounds.Width / 2, base.Top + bounds.Height / 2));
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x0012C9C7 File Offset: 0x0012B9C7
		// (set) Token: 0x060051DA RID: 20954 RVA: 0x0012C9CF File Offset: 0x0012B9CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140002FE RID: 766
		// (add) Token: 0x060051DB RID: 20955 RVA: 0x0012C9D8 File Offset: 0x0012B9D8
		// (remove) Token: 0x060051DC RID: 20956 RVA: 0x0012C9E1 File Offset: 0x0012B9E1
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x060051DD RID: 20957 RVA: 0x0012C9EA File Offset: 0x0012B9EA
		// (set) Token: 0x060051DE RID: 20958 RVA: 0x0012C9F2 File Offset: 0x0012B9F2
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x140002FF RID: 767
		// (add) Token: 0x060051DF RID: 20959 RVA: 0x0012C9FB File Offset: 0x0012B9FB
		// (remove) Token: 0x060051E0 RID: 20960 RVA: 0x0012CA04 File Offset: 0x0012BA04
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000300 RID: 768
		// (add) Token: 0x060051E1 RID: 20961 RVA: 0x0012CA0D File Offset: 0x0012BA0D
		// (remove) Token: 0x060051E2 RID: 20962 RVA: 0x0012CA16 File Offset: 0x0012BA16
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Enter
		{
			add
			{
				base.Enter += value;
			}
			remove
			{
				base.Enter -= value;
			}
		}

		// Token: 0x14000301 RID: 769
		// (add) Token: 0x060051E3 RID: 20963 RVA: 0x0012CA1F File Offset: 0x0012BA1F
		// (remove) Token: 0x060051E4 RID: 20964 RVA: 0x0012CA28 File Offset: 0x0012BA28
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		// Token: 0x14000302 RID: 770
		// (add) Token: 0x060051E5 RID: 20965 RVA: 0x0012CA31 File Offset: 0x0012BA31
		// (remove) Token: 0x060051E6 RID: 20966 RVA: 0x0012CA3A File Offset: 0x0012BA3A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		// Token: 0x14000303 RID: 771
		// (add) Token: 0x060051E7 RID: 20967 RVA: 0x0012CA43 File Offset: 0x0012BA43
		// (remove) Token: 0x060051E8 RID: 20968 RVA: 0x0012CA4C File Offset: 0x0012BA4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		// Token: 0x14000304 RID: 772
		// (add) Token: 0x060051E9 RID: 20969 RVA: 0x0012CA55 File Offset: 0x0012BA55
		// (remove) Token: 0x060051EA RID: 20970 RVA: 0x0012CA5E File Offset: 0x0012BA5E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Leave
		{
			add
			{
				base.Leave += value;
			}
			remove
			{
				base.Leave -= value;
			}
		}

		// Token: 0x14000305 RID: 773
		// (add) Token: 0x060051EB RID: 20971 RVA: 0x0012CA67 File Offset: 0x0012BA67
		// (remove) Token: 0x060051EC RID: 20972 RVA: 0x0012CA7A File Offset: 0x0012BA7A
		[SRDescription("SplitterSplitterMovingDescr")]
		[SRCategory("CatBehavior")]
		public event SplitterEventHandler SplitterMoving
		{
			add
			{
				base.Events.AddHandler(Splitter.EVENT_MOVING, value);
			}
			remove
			{
				base.Events.RemoveHandler(Splitter.EVENT_MOVING, value);
			}
		}

		// Token: 0x14000306 RID: 774
		// (add) Token: 0x060051ED RID: 20973 RVA: 0x0012CA8D File Offset: 0x0012BA8D
		// (remove) Token: 0x060051EE RID: 20974 RVA: 0x0012CAA0 File Offset: 0x0012BAA0
		[SRDescription("SplitterSplitterMovedDescr")]
		[SRCategory("CatBehavior")]
		public event SplitterEventHandler SplitterMoved
		{
			add
			{
				base.Events.AddHandler(Splitter.EVENT_MOVED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Splitter.EVENT_MOVED, value);
			}
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x0012CAB4 File Offset: 0x0012BAB4
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
			if (mode != 3)
			{
				this.DrawSplitHelper(this.splitSize);
				this.lastDrawSplit = this.splitSize;
				return;
			}
			if (this.lastDrawSplit != -1)
			{
				this.DrawSplitHelper(this.lastDrawSplit);
			}
			this.lastDrawSplit = -1;
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x0012CB2C File Offset: 0x0012BB2C
		private Rectangle CalcSplitLine(int splitSize, int minWeight)
		{
			Rectangle bounds = base.Bounds;
			Rectangle bounds2 = this.splitTarget.Bounds;
			switch (this.Dock)
			{
			case DockStyle.Top:
				if (bounds.Height < minWeight)
				{
					bounds.Height = minWeight;
				}
				bounds.Y = bounds2.Y + splitSize;
				break;
			case DockStyle.Bottom:
				if (bounds.Height < minWeight)
				{
					bounds.Height = minWeight;
				}
				bounds.Y = bounds2.Y + bounds2.Height - splitSize - bounds.Height;
				break;
			case DockStyle.Left:
				if (bounds.Width < minWeight)
				{
					bounds.Width = minWeight;
				}
				bounds.X = bounds2.X + splitSize;
				break;
			case DockStyle.Right:
				if (bounds.Width < minWeight)
				{
					bounds.Width = minWeight;
				}
				bounds.X = bounds2.X + bounds2.Width - splitSize - bounds.Width;
				break;
			}
			return bounds;
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x0012CC24 File Offset: 0x0012BC24
		private int CalcSplitSize()
		{
			Control control = this.FindTarget();
			if (control == null)
			{
				return -1;
			}
			Rectangle bounds = control.Bounds;
			switch (this.Dock)
			{
			case DockStyle.Top:
			case DockStyle.Bottom:
				return bounds.Height;
			case DockStyle.Left:
			case DockStyle.Right:
				return bounds.Width;
			default:
				return -1;
			}
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x0012CC78 File Offset: 0x0012BC78
		private Splitter.SplitData CalcSplitBounds()
		{
			Splitter.SplitData splitData = new Splitter.SplitData();
			Control control = this.FindTarget();
			splitData.target = control;
			if (control != null)
			{
				switch (control.Dock)
				{
				case DockStyle.Top:
				case DockStyle.Bottom:
					this.initTargetSize = control.Bounds.Height;
					break;
				case DockStyle.Left:
				case DockStyle.Right:
					this.initTargetSize = control.Bounds.Width;
					break;
				}
				Control parentInternal = this.ParentInternal;
				Control.ControlCollection controls = parentInternal.Controls;
				int count = controls.Count;
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < count; i++)
				{
					Control control2 = controls[i];
					if (control2 != control)
					{
						switch (control2.Dock)
						{
						case DockStyle.Top:
						case DockStyle.Bottom:
							num2 += control2.Height;
							break;
						case DockStyle.Left:
						case DockStyle.Right:
							num += control2.Width;
							break;
						}
					}
				}
				Size clientSize = parentInternal.ClientSize;
				if (this.Horizontal)
				{
					this.maxSize = clientSize.Width - num - this.minExtra;
				}
				else
				{
					this.maxSize = clientSize.Height - num2 - this.minExtra;
				}
				splitData.dockWidth = num;
				splitData.dockHeight = num2;
			}
			return splitData;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x0012CDBC File Offset: 0x0012BDBC
		private void DrawSplitHelper(int splitSize)
		{
			if (this.splitTarget == null)
			{
				return;
			}
			Rectangle rectangle = this.CalcSplitLine(splitSize, 3);
			IntPtr handle = this.ParentInternal.Handle;
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(this.ParentInternal, handle), NativeMethods.NullHandleRef, 1026);
			IntPtr handle2 = ControlPaint.CreateHalftoneHBRUSH();
			IntPtr handle3 = SafeNativeMethods.SelectObject(new HandleRef(this.ParentInternal, dcex), new HandleRef(null, handle2));
			SafeNativeMethods.PatBlt(new HandleRef(this.ParentInternal, dcex), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 5898313);
			SafeNativeMethods.SelectObject(new HandleRef(this.ParentInternal, dcex), new HandleRef(null, handle3));
			SafeNativeMethods.DeleteObject(new HandleRef(null, handle2));
			UnsafeNativeMethods.ReleaseDC(new HandleRef(this.ParentInternal, handle), new HandleRef(null, dcex));
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x0012CE98 File Offset: 0x0012BE98
		private Control FindTarget()
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal == null)
			{
				return null;
			}
			Control.ControlCollection controls = parentInternal.Controls;
			int count = controls.Count;
			DockStyle dock = this.Dock;
			for (int i = 0; i < count; i++)
			{
				Control control = controls[i];
				if (control != this)
				{
					switch (dock)
					{
					case DockStyle.Top:
						if (control.Bottom == base.Top)
						{
							return control;
						}
						break;
					case DockStyle.Bottom:
						if (control.Top == base.Bottom)
						{
							return control;
						}
						break;
					case DockStyle.Left:
						if (control.Right == base.Left)
						{
							return control;
						}
						break;
					case DockStyle.Right:
						if (control.Left == base.Right)
						{
							return control;
						}
						break;
					}
				}
			}
			return null;
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x0012CF4C File Offset: 0x0012BF4C
		private int GetSplitSize(int x, int y)
		{
			int num;
			if (this.Horizontal)
			{
				num = x - this.anchor.X;
			}
			else
			{
				num = y - this.anchor.Y;
			}
			int val = 0;
			switch (this.Dock)
			{
			case DockStyle.Top:
				val = this.splitTarget.Height + num;
				break;
			case DockStyle.Bottom:
				val = this.splitTarget.Height - num;
				break;
			case DockStyle.Left:
				val = this.splitTarget.Width + num;
				break;
			case DockStyle.Right:
				val = this.splitTarget.Width - num;
				break;
			}
			return Math.Max(Math.Min(val, this.maxSize), this.minSize);
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x0012CFF7 File Offset: 0x0012BFF7
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.splitTarget != null && e.KeyCode == Keys.Escape)
			{
				this.SplitEnd(false);
			}
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x0012D019 File Offset: 0x0012C019
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && e.Clicks == 1)
			{
				this.SplitBegin(e.X, e.Y);
			}
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x0012D04C File Offset: 0x0012C04C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.splitTarget != null)
			{
				int x = e.X + base.Left;
				int y = e.Y + base.Top;
				Rectangle rectangle = this.CalcSplitLine(this.GetSplitSize(e.X, e.Y), 0);
				int x2 = rectangle.X;
				int y2 = rectangle.Y;
				this.OnSplitterMoving(new SplitterEventArgs(x, y, x2, y2));
			}
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x0012D0C0 File Offset: 0x0012C0C0
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this.splitTarget != null)
			{
				int x = e.X;
				int left = base.Left;
				int y = e.Y;
				int top = base.Top;
				Rectangle rectangle = this.CalcSplitLine(this.GetSplitSize(e.X, e.Y), 0);
				int x2 = rectangle.X;
				int y2 = rectangle.Y;
				this.SplitEnd(true);
			}
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x0012D12C File Offset: 0x0012C12C
		protected virtual void OnSplitterMoving(SplitterEventArgs sevent)
		{
			SplitterEventHandler splitterEventHandler = (SplitterEventHandler)base.Events[Splitter.EVENT_MOVING];
			if (splitterEventHandler != null)
			{
				splitterEventHandler(this, sevent);
			}
			if (this.splitTarget != null)
			{
				this.SplitMove(sevent.SplitX, sevent.SplitY);
			}
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x0012D174 File Offset: 0x0012C174
		protected virtual void OnSplitterMoved(SplitterEventArgs sevent)
		{
			SplitterEventHandler splitterEventHandler = (SplitterEventHandler)base.Events[Splitter.EVENT_MOVED];
			if (splitterEventHandler != null)
			{
				splitterEventHandler(this, sevent);
			}
			if (this.splitTarget != null)
			{
				this.SplitMove(sevent.SplitX, sevent.SplitY);
			}
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x0012D1BC File Offset: 0x0012C1BC
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.Horizontal)
			{
				if (width < 1)
				{
					width = 3;
				}
				this.splitterThickness = width;
			}
			else
			{
				if (height < 1)
				{
					height = 3;
				}
				this.splitterThickness = height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x0012D1F4 File Offset: 0x0012C1F4
		private void SplitBegin(int x, int y)
		{
			Splitter.SplitData splitData = this.CalcSplitBounds();
			if (splitData.target != null && this.minSize < this.maxSize)
			{
				this.anchor = new Point(x, y);
				this.splitTarget = splitData.target;
				this.splitSize = this.GetSplitSize(x, y);
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					if (this.splitterMessageFilter != null)
					{
						this.splitterMessageFilter = new Splitter.SplitterMessageFilter(this);
					}
					Application.AddMessageFilter(this.splitterMessageFilter);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				base.CaptureInternal = true;
				this.DrawSplitBar(1);
			}
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x0012D294 File Offset: 0x0012C294
		private void SplitEnd(bool accept)
		{
			this.DrawSplitBar(3);
			this.splitTarget = null;
			base.CaptureInternal = false;
			if (this.splitterMessageFilter != null)
			{
				Application.RemoveMessageFilter(this.splitterMessageFilter);
				this.splitterMessageFilter = null;
			}
			if (accept)
			{
				this.ApplySplitPosition();
			}
			else if (this.splitSize != this.initTargetSize)
			{
				this.SplitPosition = this.initTargetSize;
			}
			this.anchor = Point.Empty;
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x0012D300 File Offset: 0x0012C300
		private void ApplySplitPosition()
		{
			this.SplitPosition = this.splitSize;
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x0012D310 File Offset: 0x0012C310
		private void SplitMove(int x, int y)
		{
			int num = this.GetSplitSize(x - base.Left + this.anchor.X, y - base.Top + this.anchor.Y);
			if (this.splitSize != num)
			{
				this.splitSize = num;
				this.DrawSplitBar(2);
			}
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x0012D364 File Offset: 0x0012C364
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				", MinExtra: ",
				this.MinExtra.ToString(CultureInfo.CurrentCulture),
				", MinSize: ",
				this.MinSize.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x0400360A RID: 13834
		private const int DRAW_START = 1;

		// Token: 0x0400360B RID: 13835
		private const int DRAW_MOVE = 2;

		// Token: 0x0400360C RID: 13836
		private const int DRAW_END = 3;

		// Token: 0x0400360D RID: 13837
		private const int defaultWidth = 3;

		// Token: 0x0400360E RID: 13838
		private BorderStyle borderStyle;

		// Token: 0x0400360F RID: 13839
		private int minSize = 25;

		// Token: 0x04003610 RID: 13840
		private int minExtra = 25;

		// Token: 0x04003611 RID: 13841
		private Point anchor = Point.Empty;

		// Token: 0x04003612 RID: 13842
		private Control splitTarget;

		// Token: 0x04003613 RID: 13843
		private int splitSize = -1;

		// Token: 0x04003614 RID: 13844
		private int splitterThickness = 3;

		// Token: 0x04003615 RID: 13845
		private int initTargetSize;

		// Token: 0x04003616 RID: 13846
		private int lastDrawSplit = -1;

		// Token: 0x04003617 RID: 13847
		private int maxSize;

		// Token: 0x04003618 RID: 13848
		private static readonly object EVENT_MOVING = new object();

		// Token: 0x04003619 RID: 13849
		private static readonly object EVENT_MOVED = new object();

		// Token: 0x0400361A RID: 13850
		private Splitter.SplitterMessageFilter splitterMessageFilter;

		// Token: 0x0200061F RID: 1567
		private class SplitData
		{
			// Token: 0x0400361B RID: 13851
			public int dockWidth = -1;

			// Token: 0x0400361C RID: 13852
			public int dockHeight = -1;

			// Token: 0x0400361D RID: 13853
			internal Control target;
		}

		// Token: 0x02000620 RID: 1568
		private class SplitterMessageFilter : IMessageFilter
		{
			// Token: 0x06005204 RID: 20996 RVA: 0x0012D3F1 File Offset: 0x0012C3F1
			public SplitterMessageFilter(Splitter splitter)
			{
				this.owner = splitter;
			}

			// Token: 0x06005205 RID: 20997 RVA: 0x0012D400 File Offset: 0x0012C400
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public bool PreFilterMessage(ref Message m)
			{
				if (m.Msg >= 256 && m.Msg <= 264)
				{
					if (m.Msg == 256 && (int)((long)m.WParam) == 27)
					{
						this.owner.SplitEnd(false);
					}
					return true;
				}
				return false;
			}

			// Token: 0x0400361E RID: 13854
			private Splitter owner;
		}
	}
}
