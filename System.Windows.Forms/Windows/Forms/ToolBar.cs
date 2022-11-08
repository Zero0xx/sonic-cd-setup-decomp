using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000666 RID: 1638
	[DefaultEvent("ButtonClick")]
	[DefaultProperty("Buttons")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ToolBarDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ToolBar : Control
	{
		// Token: 0x060055D3 RID: 21971 RVA: 0x0013846C File Offset: 0x0013746C
		public ToolBar()
		{
			this.toolBarState = new BitVector32(31);
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.FixedHeight, this.AutoSize);
			base.SetStyle(ControlStyles.FixedWidth, false);
			this.TabStop = false;
			this.Dock = DockStyle.Top;
			this.buttonsCollection = new ToolBar.ToolBarButtonCollection(this);
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x060055D4 RID: 21972 RVA: 0x001384F4 File Offset: 0x001374F4
		// (set) Token: 0x060055D5 RID: 21973 RVA: 0x001384FC File Offset: 0x001374FC
		[Localizable(true)]
		[SRDescription("ToolBarAppearanceDescr")]
		[DefaultValue(ToolBarAppearance.Normal)]
		[SRCategory("CatBehavior")]
		public ToolBarAppearance Appearance
		{
			get
			{
				return this.appearance;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolBarAppearance));
				}
				if (value != this.appearance)
				{
					this.appearance = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x060055D6 RID: 21974 RVA: 0x0013853A File Offset: 0x0013753A
		// (set) Token: 0x060055D7 RID: 21975 RVA: 0x0013854C File Offset: 0x0013754C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(true)]
		[Localizable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRCategory("CatBehavior")]
		[SRDescription("ToolBarAutoSizeDescr")]
		[Browsable(true)]
		public override bool AutoSize
		{
			get
			{
				return this.toolBarState[16];
			}
			set
			{
				if (this.AutoSize != value)
				{
					this.toolBarState[16] = value;
					if (this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
					{
						base.SetStyle(ControlStyles.FixedWidth, this.AutoSize);
						base.SetStyle(ControlStyles.FixedHeight, false);
					}
					else
					{
						base.SetStyle(ControlStyles.FixedHeight, this.AutoSize);
						base.SetStyle(ControlStyles.FixedWidth, false);
					}
					this.AdjustSize(this.Dock);
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400032E RID: 814
		// (add) Token: 0x060055D8 RID: 21976 RVA: 0x001385C9 File Offset: 0x001375C9
		// (remove) Token: 0x060055D9 RID: 21977 RVA: 0x001385D2 File Offset: 0x001375D2
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[Browsable(true)]
		[SRCategory("CatPropertyChanged")]
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

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x060055DA RID: 21978 RVA: 0x001385DB File Offset: 0x001375DB
		// (set) Token: 0x060055DB RID: 21979 RVA: 0x001385E3 File Offset: 0x001375E3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x1400032F RID: 815
		// (add) Token: 0x060055DC RID: 21980 RVA: 0x001385EC File Offset: 0x001375EC
		// (remove) Token: 0x060055DD RID: 21981 RVA: 0x001385F5 File Offset: 0x001375F5
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				base.BackColorChanged += value;
			}
			remove
			{
				base.BackColorChanged -= value;
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x060055DE RID: 21982 RVA: 0x001385FE File Offset: 0x001375FE
		// (set) Token: 0x060055DF RID: 21983 RVA: 0x00138606 File Offset: 0x00137606
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000330 RID: 816
		// (add) Token: 0x060055E0 RID: 21984 RVA: 0x0013860F File Offset: 0x0013760F
		// (remove) Token: 0x060055E1 RID: 21985 RVA: 0x00138618 File Offset: 0x00137618
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x060055E2 RID: 21986 RVA: 0x00138621 File Offset: 0x00137621
		// (set) Token: 0x060055E3 RID: 21987 RVA: 0x00138629 File Offset: 0x00137629
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

		// Token: 0x14000331 RID: 817
		// (add) Token: 0x060055E4 RID: 21988 RVA: 0x00138632 File Offset: 0x00137632
		// (remove) Token: 0x060055E5 RID: 21989 RVA: 0x0013863B File Offset: 0x0013763B
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

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x060055E6 RID: 21990 RVA: 0x00138644 File Offset: 0x00137644
		// (set) Token: 0x060055E7 RID: 21991 RVA: 0x0013864C File Offset: 0x0013764C
		[DispId(-504)]
		[SRDescription("ToolBarBorderStyleDescr")]
		[DefaultValue(BorderStyle.None)]
		[SRCategory("CatAppearance")]
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
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x060055E8 RID: 21992 RVA: 0x0013868A File Offset: 0x0013768A
		[MergableProperty(false)]
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ToolBarButtonsDescr")]
		public ToolBar.ToolBarButtonCollection Buttons
		{
			get
			{
				return this.buttonsCollection;
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x060055E9 RID: 21993 RVA: 0x00138694 File Offset: 0x00137694
		// (set) Token: 0x060055EA RID: 21994 RVA: 0x00138714 File Offset: 0x00137714
		[RefreshProperties(RefreshProperties.All)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ToolBarButtonSizeDescr")]
		public Size ButtonSize
		{
			get
			{
				if (!this.buttonSize.IsEmpty)
				{
					return this.buttonSize;
				}
				if (base.IsHandleCreated && this.buttons != null && this.buttonCount > 0)
				{
					int num = (int)base.SendMessage(1082, 0, 0);
					if (num > 0)
					{
						return new Size(NativeMethods.Util.LOWORD(num), NativeMethods.Util.HIWORD(num));
					}
				}
				if (this.TextAlign == ToolBarTextAlign.Underneath)
				{
					return new Size(39, 36);
				}
				return new Size(23, 22);
			}
			set
			{
				if (value.Width < 0 || value.Height < 0)
				{
					throw new ArgumentOutOfRangeException("ButtonSize", SR.GetString("InvalidArgument", new object[]
					{
						"ButtonSize",
						value.ToString()
					}));
				}
				if (this.buttonSize != value)
				{
					this.buttonSize = value;
					this.maxWidth = -1;
					base.RecreateHandle();
					this.AdjustSize(this.Dock);
				}
			}
		}

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x060055EB RID: 21995 RVA: 0x00138798 File Offset: 0x00137798
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "ToolbarWindow32";
				createParams.Style |= 12;
				if (!this.Divider)
				{
					createParams.Style |= 64;
				}
				if (this.Wrappable)
				{
					createParams.Style |= 512;
				}
				if (this.ShowToolTips && !base.DesignMode)
				{
					createParams.Style |= 256;
				}
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
				switch (this.appearance)
				{
				case ToolBarAppearance.Flat:
					createParams.Style |= 2048;
					break;
				}
				switch (this.textAlign)
				{
				case ToolBarTextAlign.Right:
					createParams.Style |= 4096;
					break;
				}
				return createParams;
			}
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x060055EC RID: 21996 RVA: 0x001388CF File Offset: 0x001378CF
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x060055ED RID: 21997 RVA: 0x001388D2 File Offset: 0x001378D2
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 22);
			}
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x060055EE RID: 21998 RVA: 0x001388DD File Offset: 0x001378DD
		// (set) Token: 0x060055EF RID: 21999 RVA: 0x001388EB File Offset: 0x001378EB
		[SRCategory("CatAppearance")]
		[SRDescription("ToolBarDividerDescr")]
		[DefaultValue(true)]
		public bool Divider
		{
			get
			{
				return this.toolBarState[4];
			}
			set
			{
				if (this.Divider != value)
				{
					this.toolBarState[4] = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x060055F0 RID: 22000 RVA: 0x00138909 File Offset: 0x00137909
		// (set) Token: 0x060055F1 RID: 22001 RVA: 0x00138914 File Offset: 0x00137914
		[DefaultValue(DockStyle.Top)]
		[Localizable(true)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DockStyle));
				}
				if (this.Dock != value)
				{
					if (value == DockStyle.Left || value == DockStyle.Right)
					{
						base.SetStyle(ControlStyles.FixedWidth, this.AutoSize);
						base.SetStyle(ControlStyles.FixedHeight, false);
					}
					else
					{
						base.SetStyle(ControlStyles.FixedHeight, this.AutoSize);
						base.SetStyle(ControlStyles.FixedWidth, false);
					}
					this.AdjustSize(value);
					base.Dock = value;
				}
			}
		}

		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x060055F2 RID: 22002 RVA: 0x00138996 File Offset: 0x00137996
		// (set) Token: 0x060055F3 RID: 22003 RVA: 0x0013899E File Offset: 0x0013799E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x060055F4 RID: 22004 RVA: 0x001389A7 File Offset: 0x001379A7
		// (set) Token: 0x060055F5 RID: 22005 RVA: 0x001389B5 File Offset: 0x001379B5
		[SRCategory("CatAppearance")]
		[SRDescription("ToolBarDropDownArrowsDescr")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool DropDownArrows
		{
			get
			{
				return this.toolBarState[2];
			}
			set
			{
				if (this.DropDownArrows != value)
				{
					this.toolBarState[2] = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x060055F6 RID: 22006 RVA: 0x001389D3 File Offset: 0x001379D3
		// (set) Token: 0x060055F7 RID: 22007 RVA: 0x001389DB File Offset: 0x001379DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000332 RID: 818
		// (add) Token: 0x060055F8 RID: 22008 RVA: 0x001389E4 File Offset: 0x001379E4
		// (remove) Token: 0x060055F9 RID: 22009 RVA: 0x001389ED File Offset: 0x001379ED
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

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x060055FA RID: 22010 RVA: 0x001389F6 File Offset: 0x001379F6
		// (set) Token: 0x060055FB RID: 22011 RVA: 0x00138A00 File Offset: 0x00137A00
		[SRDescription("ToolBarImageListDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (value != this.imageList)
				{
					EventHandler value2 = new EventHandler(this.ImageListRecreateHandle);
					EventHandler value3 = new EventHandler(this.DetachImageList);
					if (this.imageList != null)
					{
						this.imageList.Disposed -= value3;
						this.imageList.RecreateHandle -= value2;
					}
					this.imageList = value;
					if (value != null)
					{
						value.Disposed += value3;
						value.RecreateHandle += value2;
					}
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x060055FC RID: 22012 RVA: 0x00138A76 File Offset: 0x00137A76
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ToolBarImageSizeDescr")]
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Size ImageSize
		{
			get
			{
				if (this.imageList != null)
				{
					return this.imageList.ImageSize;
				}
				return new Size(0, 0);
			}
		}

		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x060055FD RID: 22013 RVA: 0x00138A93 File Offset: 0x00137A93
		// (set) Token: 0x060055FE RID: 22014 RVA: 0x00138A9B File Offset: 0x00137A9B
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

		// Token: 0x14000333 RID: 819
		// (add) Token: 0x060055FF RID: 22015 RVA: 0x00138AA4 File Offset: 0x00137AA4
		// (remove) Token: 0x06005600 RID: 22016 RVA: 0x00138AAD File Offset: 0x00137AAD
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

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x06005601 RID: 22017 RVA: 0x00138AB8 File Offset: 0x00137AB8
		internal int PreferredHeight
		{
			get
			{
				int num;
				if (this.buttons == null || this.buttonCount == 0 || !base.IsHandleCreated)
				{
					num = this.ButtonSize.Height;
				}
				else
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					int num2 = 0;
					while (num2 < this.buttons.Length && (this.buttons[num2] == null || !this.buttons[num2].Visible))
					{
						num2++;
					}
					if (num2 == this.buttons.Length)
					{
						num2 = 0;
					}
					base.SendMessage(1075, num2, ref rect);
					num = rect.bottom - rect.top;
				}
				if (this.Wrappable && base.IsHandleCreated)
				{
					num *= (int)base.SendMessage(1064, 0, 0);
				}
				num = ((num > 0) ? num : 1);
				switch (this.borderStyle)
				{
				case BorderStyle.FixedSingle:
					num += SystemInformation.BorderSize.Height;
					break;
				case BorderStyle.Fixed3D:
					num += SystemInformation.Border3DSize.Height;
					break;
				}
				if (this.Divider)
				{
					num += 2;
				}
				return num + 4;
			}
		}

		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x06005602 RID: 22018 RVA: 0x00138BD0 File Offset: 0x00137BD0
		internal int PreferredWidth
		{
			get
			{
				if (this.maxWidth == -1)
				{
					if (!base.IsHandleCreated || this.buttons == null)
					{
						this.maxWidth = this.ButtonSize.Width;
					}
					else
					{
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						for (int i = 0; i < this.buttonCount; i++)
						{
							base.SendMessage(1075, 0, ref rect);
							if (rect.right - rect.left > this.maxWidth)
							{
								this.maxWidth = rect.right - rect.left;
							}
						}
					}
				}
				int num = this.maxWidth;
				if (this.borderStyle != BorderStyle.None)
				{
					num += SystemInformation.BorderSize.Height * 4 + 3;
				}
				return num;
			}
		}

		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x06005603 RID: 22019 RVA: 0x00138C85 File Offset: 0x00137C85
		// (set) Token: 0x06005604 RID: 22020 RVA: 0x00138C8D File Offset: 0x00137C8D
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		// Token: 0x14000334 RID: 820
		// (add) Token: 0x06005605 RID: 22021 RVA: 0x00138C96 File Offset: 0x00137C96
		// (remove) Token: 0x06005606 RID: 22022 RVA: 0x00138C9F File Offset: 0x00137C9F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				base.RightToLeftChanged += value;
			}
			remove
			{
				base.RightToLeftChanged -= value;
			}
		}

		// Token: 0x06005607 RID: 22023 RVA: 0x00138CA8 File Offset: 0x00137CA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			this.currentScaleDX = dx;
			this.currentScaleDY = dy;
			base.ScaleCore(dx, dy);
			this.UpdateButtons();
		}

		// Token: 0x06005608 RID: 22024 RVA: 0x00138CC6 File Offset: 0x00137CC6
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			this.currentScaleDX = factor.Width;
			this.currentScaleDY = factor.Height;
			base.ScaleControl(factor, specified);
		}

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x06005609 RID: 22025 RVA: 0x00138CEA File Offset: 0x00137CEA
		// (set) Token: 0x0600560A RID: 22026 RVA: 0x00138CF8 File Offset: 0x00137CF8
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ToolBarShowToolTipsDescr")]
		[DefaultValue(false)]
		public bool ShowToolTips
		{
			get
			{
				return this.toolBarState[8];
			}
			set
			{
				if (this.ShowToolTips != value)
				{
					this.toolBarState[8] = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x0600560B RID: 22027 RVA: 0x00138D16 File Offset: 0x00137D16
		// (set) Token: 0x0600560C RID: 22028 RVA: 0x00138D1E File Offset: 0x00137D1E
		[DefaultValue(false)]
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

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x0600560D RID: 22029 RVA: 0x00138D27 File Offset: 0x00137D27
		// (set) Token: 0x0600560E RID: 22030 RVA: 0x00138D2F File Offset: 0x00137D2F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
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

		// Token: 0x14000335 RID: 821
		// (add) Token: 0x0600560F RID: 22031 RVA: 0x00138D38 File Offset: 0x00137D38
		// (remove) Token: 0x06005610 RID: 22032 RVA: 0x00138D41 File Offset: 0x00137D41
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

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x06005611 RID: 22033 RVA: 0x00138D4A File Offset: 0x00137D4A
		// (set) Token: 0x06005612 RID: 22034 RVA: 0x00138D52 File Offset: 0x00137D52
		[SRDescription("ToolBarTextAlignDescr")]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(ToolBarTextAlign.Underneath)]
		public ToolBarTextAlign TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolBarTextAlign));
				}
				if (this.textAlign == value)
				{
					return;
				}
				this.textAlign = value;
				base.RecreateHandle();
			}
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x06005613 RID: 22035 RVA: 0x00138D91 File Offset: 0x00137D91
		// (set) Token: 0x06005614 RID: 22036 RVA: 0x00138D9F File Offset: 0x00137D9F
		[SRDescription("ToolBarWrappableDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		public bool Wrappable
		{
			get
			{
				return this.toolBarState[1];
			}
			set
			{
				if (this.Wrappable != value)
				{
					this.toolBarState[1] = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x14000336 RID: 822
		// (add) Token: 0x06005615 RID: 22037 RVA: 0x00138DBD File Offset: 0x00137DBD
		// (remove) Token: 0x06005616 RID: 22038 RVA: 0x00138DD6 File Offset: 0x00137DD6
		[SRCategory("CatBehavior")]
		[SRDescription("ToolBarButtonClickDescr")]
		public event ToolBarButtonClickEventHandler ButtonClick
		{
			add
			{
				this.onButtonClick = (ToolBarButtonClickEventHandler)Delegate.Combine(this.onButtonClick, value);
			}
			remove
			{
				this.onButtonClick = (ToolBarButtonClickEventHandler)Delegate.Remove(this.onButtonClick, value);
			}
		}

		// Token: 0x14000337 RID: 823
		// (add) Token: 0x06005617 RID: 22039 RVA: 0x00138DEF File Offset: 0x00137DEF
		// (remove) Token: 0x06005618 RID: 22040 RVA: 0x00138E08 File Offset: 0x00137E08
		[SRCategory("CatBehavior")]
		[SRDescription("ToolBarButtonDropDownDescr")]
		public event ToolBarButtonClickEventHandler ButtonDropDown
		{
			add
			{
				this.onButtonDropDown = (ToolBarButtonClickEventHandler)Delegate.Combine(this.onButtonDropDown, value);
			}
			remove
			{
				this.onButtonDropDown = (ToolBarButtonClickEventHandler)Delegate.Remove(this.onButtonDropDown, value);
			}
		}

		// Token: 0x14000338 RID: 824
		// (add) Token: 0x06005619 RID: 22041 RVA: 0x00138E21 File Offset: 0x00137E21
		// (remove) Token: 0x0600561A RID: 22042 RVA: 0x00138E2A File Offset: 0x00137E2A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		// Token: 0x0600561B RID: 22043 RVA: 0x00138E34 File Offset: 0x00137E34
		private void AdjustSize(DockStyle dock)
		{
			int num = this.requestedSize;
			try
			{
				if (dock == DockStyle.Left || dock == DockStyle.Right)
				{
					base.Width = (this.AutoSize ? this.PreferredWidth : num);
				}
				else
				{
					base.Height = (this.AutoSize ? this.PreferredHeight : num);
				}
			}
			finally
			{
				this.requestedSize = num;
			}
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x00138E9C File Offset: 0x00137E9C
		internal void BeginUpdate()
		{
			base.BeginUpdateInternal();
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x00138EA4 File Offset: 0x00137EA4
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 4
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x00138EF4 File Offset: 0x00137EF4
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		// Token: 0x0600561F RID: 22047 RVA: 0x00138F00 File Offset: 0x00137F00
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this)
				{
					bool state = base.GetState(4096);
					try
					{
						base.SetState(4096, true);
						if (this.imageList != null)
						{
							this.imageList.Disposed -= this.DetachImageList;
							this.imageList = null;
						}
						if (this.buttonsCollection != null)
						{
							ToolBarButton[] array = new ToolBarButton[this.buttonsCollection.Count];
							((ICollection)this.buttonsCollection).CopyTo(array, 0);
							this.buttonsCollection.Clear();
							foreach (ToolBarButton toolBarButton in array)
							{
								toolBarButton.Dispose();
							}
						}
					}
					finally
					{
						base.SetState(4096, state);
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x00138FE8 File Offset: 0x00137FE8
		internal void EndUpdate()
		{
			base.EndUpdateInternal();
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x00138FF4 File Offset: 0x00137FF4
		private void ForceButtonWidths()
		{
			if (this.buttons != null && this.buttonSize.IsEmpty && base.IsHandleCreated)
			{
				this.maxWidth = -1;
				for (int i = 0; i < this.buttonCount; i++)
				{
					NativeMethods.TBBUTTONINFO tbbuttoninfo = default(NativeMethods.TBBUTTONINFO);
					tbbuttoninfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.TBBUTTONINFO));
					tbbuttoninfo.cx = this.buttons[i].Width;
					if ((int)tbbuttoninfo.cx > this.maxWidth)
					{
						this.maxWidth = (int)tbbuttoninfo.cx;
					}
					tbbuttoninfo.dwMask = 64;
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TB_SETBUTTONINFO, i, ref tbbuttoninfo);
				}
			}
		}

		// Token: 0x06005622 RID: 22050 RVA: 0x001390B0 File Offset: 0x001380B0
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x06005623 RID: 22051 RVA: 0x001390C0 File Offset: 0x001380C0
		private void Insert(int index, ToolBarButton button)
		{
			button.parent = this;
			if (this.buttons == null)
			{
				this.buttons = new ToolBarButton[4];
			}
			else if (this.buttons.Length == this.buttonCount)
			{
				ToolBarButton[] destinationArray = new ToolBarButton[this.buttonCount + 4];
				Array.Copy(this.buttons, 0, destinationArray, 0, this.buttonCount);
				this.buttons = destinationArray;
			}
			if (index < this.buttonCount)
			{
				Array.Copy(this.buttons, index, this.buttons, index + 1, this.buttonCount - index);
			}
			this.buttons[index] = button;
			this.buttonCount++;
		}

		// Token: 0x06005624 RID: 22052 RVA: 0x00139160 File Offset: 0x00138160
		private void InsertButton(int index, ToolBarButton value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (index < 0 || (this.buttons != null && index > this.buttonCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.Insert(index, value);
			if (base.IsHandleCreated)
			{
				NativeMethods.TBBUTTON tbbutton = value.GetTBBUTTON(index);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TB_INSERTBUTTON, index, ref tbbutton);
			}
			this.UpdateButtons();
		}

		// Token: 0x06005625 RID: 22053 RVA: 0x001391FC File Offset: 0x001381FC
		private int InternalAddButton(ToolBarButton button)
		{
			if (button == null)
			{
				throw new ArgumentNullException("button");
			}
			int num = this.buttonCount;
			this.Insert(num, button);
			return num;
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x00139228 File Offset: 0x00138228
		internal void InternalSetButton(int index, ToolBarButton value, bool recreate, bool updateText)
		{
			this.buttons[index].parent = null;
			this.buttons[index].stringIndex = (IntPtr)(-1);
			this.buttons[index] = value;
			this.buttons[index].parent = this;
			if (base.IsHandleCreated)
			{
				NativeMethods.TBBUTTONINFO tbbuttoninfo = value.GetTBBUTTONINFO(updateText, index);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TB_SETBUTTONINFO, index, ref tbbuttoninfo);
				if (tbbuttoninfo.pszText != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(tbbuttoninfo.pszText);
				}
				if (recreate)
				{
					this.UpdateButtons();
					return;
				}
				base.SendMessage(1057, 0, 0);
				this.ForceButtonWidths();
				base.Invalidate();
			}
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x001392DC File Offset: 0x001382DC
		protected virtual void OnButtonClick(ToolBarButtonClickEventArgs e)
		{
			if (this.onButtonClick != null)
			{
				this.onButtonClick(this, e);
			}
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x001392F3 File Offset: 0x001382F3
		protected virtual void OnButtonDropDown(ToolBarButtonClickEventArgs e)
		{
			if (this.onButtonDropDown != null)
			{
				this.onButtonDropDown(this, e);
			}
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x0013930C File Offset: 0x0013830C
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(1054, Marshal.SizeOf(typeof(NativeMethods.TBBUTTON)), 0);
			if (this.DropDownArrows)
			{
				base.SendMessage(1108, 0, 1);
			}
			if (this.imageList != null)
			{
				base.SendMessage(1072, 0, this.imageList.Handle);
			}
			this.RealizeButtons();
			this.BeginUpdate();
			try
			{
				Size size = base.Size;
				base.Size = new Size(size.Width + 1, size.Height);
				base.Size = size;
			}
			finally
			{
				this.EndUpdate();
			}
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x001393C0 File Offset: 0x001383C0
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.Wrappable)
			{
				this.AdjustSize(this.Dock);
			}
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x001393DD File Offset: 0x001383DD
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (base.IsHandleCreated)
			{
				if (!this.buttonSize.IsEmpty)
				{
					this.SendToolbarButtonSizeMessage();
					return;
				}
				this.AdjustSize(this.Dock);
				this.ForceButtonWidths();
			}
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x00139414 File Offset: 0x00138414
		private void RealizeButtons()
		{
			if (this.buttons != null)
			{
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					this.BeginUpdate();
					for (int i = 0; i < this.buttonCount; i++)
					{
						if (this.buttons[i].Text.Length > 0)
						{
							string lparam = this.buttons[i].Text + '\0'.ToString();
							this.buttons[i].stringIndex = base.SendMessage(NativeMethods.TB_ADDSTRING, 0, lparam);
						}
						else
						{
							this.buttons[i].stringIndex = (IntPtr)(-1);
						}
					}
					int num = Marshal.SizeOf(typeof(NativeMethods.TBBUTTON));
					int num2 = this.buttonCount;
					intPtr = Marshal.AllocHGlobal(checked(num * num2));
					for (int j = 0; j < num2; j++)
					{
						NativeMethods.TBBUTTON tbbutton = this.buttons[j].GetTBBUTTON(j);
						Marshal.StructureToPtr(tbbutton, (IntPtr)(checked((long)intPtr + unchecked((long)(checked(num * j))))), true);
						this.buttons[j].parent = this;
					}
					base.SendMessage(NativeMethods.TB_ADDBUTTONS, num2, intPtr);
					base.SendMessage(1057, 0, 0);
					if (!this.buttonSize.IsEmpty)
					{
						this.SendToolbarButtonSizeMessage();
					}
					else
					{
						this.ForceButtonWidths();
					}
					this.AdjustSize(this.Dock);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
					this.EndUpdate();
				}
			}
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x0013958C File Offset: 0x0013858C
		private void RemoveAt(int index)
		{
			this.buttons[index].parent = null;
			this.buttons[index].stringIndex = (IntPtr)(-1);
			this.buttonCount--;
			if (index < this.buttonCount)
			{
				Array.Copy(this.buttons, index + 1, this.buttons, index, this.buttonCount - index);
			}
			this.buttons[this.buttonCount] = null;
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x001395FC File Offset: 0x001385FC
		private void ResetButtonSize()
		{
			this.buttonSize = Size.Empty;
			base.RecreateHandle();
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x0013960F File Offset: 0x0013860F
		private void SendToolbarButtonSizeMessage()
		{
			base.SendMessage(1055, 0, NativeMethods.Util.MAKELPARAM((int)((float)this.buttonSize.Width * this.currentScaleDX), (int)((float)this.buttonSize.Height * this.currentScaleDY)));
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x0013964C File Offset: 0x0013864C
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			int num = height;
			int num2 = width;
			base.SetBoundsCore(x, y, width, height, specified);
			Rectangle bounds = base.Bounds;
			if (this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
			{
				if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
				{
					this.requestedSize = width;
				}
				if (this.AutoSize)
				{
					width = this.PreferredWidth;
				}
				if (width != num2 && this.Dock == DockStyle.Right)
				{
					int num3 = num2 - width;
					x += num3;
				}
			}
			else
			{
				if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
				{
					this.requestedSize = height;
				}
				if (this.AutoSize)
				{
					height = this.PreferredHeight;
				}
				if (height != num && this.Dock == DockStyle.Bottom)
				{
					int num4 = num - height;
					y += num4;
				}
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06005631 RID: 22065 RVA: 0x001396FC File Offset: 0x001386FC
		private bool ShouldSerializeButtonSize()
		{
			return !this.buttonSize.IsEmpty;
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x0013970C File Offset: 0x0013870C
		internal void SetToolTip(ToolTip toolTip)
		{
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1060, new HandleRef(toolTip, toolTip.Handle), 0);
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x00139734 File Offset: 0x00138734
		public override string ToString()
		{
			string text = base.ToString();
			text = text + ", Buttons.Count: " + this.buttonCount.ToString(CultureInfo.CurrentCulture);
			if (this.buttonCount > 0)
			{
				text = text + ", Buttons[0]: " + this.buttons[0].ToString();
			}
			return text;
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x00139787 File Offset: 0x00138787
		internal void UpdateButtons()
		{
			if (base.IsHandleCreated)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x00139798 File Offset: 0x00138798
		private void WmNotifyDropDown(ref Message m)
		{
			NativeMethods.NMTOOLBAR nmtoolbar = (NativeMethods.NMTOOLBAR)m.GetLParam(typeof(NativeMethods.NMTOOLBAR));
			ToolBarButton toolBarButton = this.buttons[nmtoolbar.iItem];
			if (toolBarButton == null)
			{
				throw new InvalidOperationException(SR.GetString("ToolBarButtonNotFound"));
			}
			this.OnButtonDropDown(new ToolBarButtonClickEventArgs(toolBarButton));
			Menu dropDownMenu = toolBarButton.DropDownMenu;
			if (dropDownMenu != null)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				NativeMethods.TPMPARAMS tpmparams = new NativeMethods.TPMPARAMS();
				base.SendMessage(1075, nmtoolbar.iItem, ref rect);
				if (dropDownMenu.GetType().IsAssignableFrom(typeof(ContextMenu)))
				{
					((ContextMenu)dropDownMenu).Show(this, new Point(rect.left, rect.bottom));
					return;
				}
				Menu mainMenu = dropDownMenu.GetMainMenu();
				if (mainMenu != null)
				{
					mainMenu.ProcessInitMenuPopup(dropDownMenu.Handle);
				}
				UnsafeNativeMethods.MapWindowPoints(new HandleRef(nmtoolbar.hdr, nmtoolbar.hdr.hwndFrom), NativeMethods.NullHandleRef, ref rect, 2);
				tpmparams.rcExclude_left = rect.left;
				tpmparams.rcExclude_top = rect.top;
				tpmparams.rcExclude_right = rect.right;
				tpmparams.rcExclude_bottom = rect.bottom;
				SafeNativeMethods.TrackPopupMenuEx(new HandleRef(dropDownMenu, dropDownMenu.Handle), 64, rect.left, rect.bottom, new HandleRef(this, base.Handle), tpmparams);
			}
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x00139900 File Offset: 0x00138900
		private void WmNotifyNeedText(ref Message m)
		{
			NativeMethods.TOOLTIPTEXT tooltiptext = (NativeMethods.TOOLTIPTEXT)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXT));
			int num = (int)tooltiptext.hdr.idFrom;
			ToolBarButton toolBarButton = this.buttons[num];
			if (toolBarButton != null && toolBarButton.ToolTipText != null)
			{
				tooltiptext.lpszText = toolBarButton.ToolTipText;
			}
			else
			{
				tooltiptext.lpszText = null;
			}
			tooltiptext.hinst = IntPtr.Zero;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				tooltiptext.uFlags |= 4;
			}
			Marshal.StructureToPtr(tooltiptext, m.LParam, false);
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x0013998C File Offset: 0x0013898C
		private void WmNotifyNeedTextA(ref Message m)
		{
			NativeMethods.TOOLTIPTEXTA tooltiptexta = (NativeMethods.TOOLTIPTEXTA)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXTA));
			int num = (int)tooltiptexta.hdr.idFrom;
			ToolBarButton toolBarButton = this.buttons[num];
			if (toolBarButton != null && toolBarButton.ToolTipText != null)
			{
				tooltiptexta.lpszText = toolBarButton.ToolTipText;
			}
			else
			{
				tooltiptexta.lpszText = null;
			}
			tooltiptexta.hinst = IntPtr.Zero;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				tooltiptexta.uFlags |= 4;
			}
			Marshal.StructureToPtr(tooltiptexta, m.LParam, false);
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x00139A18 File Offset: 0x00138A18
		private void WmNotifyHotItemChange(ref Message m)
		{
			NativeMethods.NMTBHOTITEM nmtbhotitem = (NativeMethods.NMTBHOTITEM)m.GetLParam(typeof(NativeMethods.NMTBHOTITEM));
			if (16 == (nmtbhotitem.dwFlags & 16))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (32 == (nmtbhotitem.dwFlags & 32))
			{
				this.hotItem = -1;
				return;
			}
			if (1 == (nmtbhotitem.dwFlags & 1))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (2 == (nmtbhotitem.dwFlags & 2))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (4 == (nmtbhotitem.dwFlags & 4))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (8 == (nmtbhotitem.dwFlags & 8))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (64 == (nmtbhotitem.dwFlags & 64))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (128 == (nmtbhotitem.dwFlags & 128))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (256 == (nmtbhotitem.dwFlags & 256))
			{
				this.hotItem = nmtbhotitem.idNew;
			}
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x00139B34 File Offset: 0x00138B34
		private void WmReflectCommand(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.WParam);
			ToolBarButton toolBarButton = this.buttons[num];
			if (toolBarButton != null)
			{
				ToolBarButtonClickEventArgs e = new ToolBarButtonClickEventArgs(toolBarButton);
				this.OnButtonClick(e);
			}
			base.WndProc(ref m);
			base.ResetMouseEventArgs();
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x00139B74 File Offset: 0x00138B74
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 78 && msg != 8270)
			{
				if (msg == 8465)
				{
					this.WmReflectCommand(ref m);
				}
			}
			else
			{
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
				int code = nmhdr.code;
				if (code <= -710)
				{
					if (code != -713)
					{
						if (code == -710)
						{
							this.WmNotifyDropDown(ref m);
						}
					}
					else
					{
						this.WmNotifyHotItemChange(ref m);
					}
				}
				else if (code != -706)
				{
					if (code != -530)
					{
						switch (code)
						{
						case -521:
						{
							NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
							UnsafeNativeMethods.GetWindowPlacement(new HandleRef(null, nmhdr.hwndFrom), ref windowplacement);
							if (windowplacement.rcNormalPosition_left == 0 && windowplacement.rcNormalPosition_top == 0 && this.hotItem != -1)
							{
								int num = 0;
								for (int i = 0; i <= this.hotItem; i++)
								{
									num += this.buttonsCollection[i].GetButtonWidth();
								}
								int num2 = windowplacement.rcNormalPosition_right - windowplacement.rcNormalPosition_left;
								int num3 = windowplacement.rcNormalPosition_bottom - windowplacement.rcNormalPosition_top;
								int x = base.Location.X + num + 1;
								int y = base.Location.Y + this.ButtonSize.Height / 2;
								NativeMethods.POINT point = new NativeMethods.POINT(x, y);
								UnsafeNativeMethods.ClientToScreen(new HandleRef(this, base.Handle), point);
								if (point.y < SystemInformation.WorkingArea.Y)
								{
									point.y += this.ButtonSize.Height / 2 + 1;
								}
								if (point.y + num3 > SystemInformation.WorkingArea.Height)
								{
									point.y -= this.ButtonSize.Height / 2 + num3 + 1;
								}
								if (point.x + num2 > SystemInformation.WorkingArea.Right)
								{
									point.x -= this.ButtonSize.Width + num2 + 2;
								}
								SafeNativeMethods.SetWindowPos(new HandleRef(null, nmhdr.hwndFrom), NativeMethods.NullHandleRef, point.x, point.y, 0, 0, 21);
								m.Result = (IntPtr)1;
								return;
							}
							break;
						}
						case -520:
							this.WmNotifyNeedTextA(ref m);
							m.Result = (IntPtr)1;
							return;
						}
					}
					else if (Marshal.SystemDefaultCharSize == 2)
					{
						this.WmNotifyNeedText(ref m);
						m.Result = (IntPtr)1;
						return;
					}
				}
				else
				{
					m.Result = (IntPtr)1;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x04003738 RID: 14136
		internal const int DDARROW_WIDTH = 15;

		// Token: 0x04003739 RID: 14137
		private const int TOOLBARSTATE_wrappable = 1;

		// Token: 0x0400373A RID: 14138
		private const int TOOLBARSTATE_dropDownArrows = 2;

		// Token: 0x0400373B RID: 14139
		private const int TOOLBARSTATE_divider = 4;

		// Token: 0x0400373C RID: 14140
		private const int TOOLBARSTATE_showToolTips = 8;

		// Token: 0x0400373D RID: 14141
		private const int TOOLBARSTATE_autoSize = 16;

		// Token: 0x0400373E RID: 14142
		private ToolBar.ToolBarButtonCollection buttonsCollection;

		// Token: 0x0400373F RID: 14143
		internal Size buttonSize = Size.Empty;

		// Token: 0x04003740 RID: 14144
		private int requestedSize;

		// Token: 0x04003741 RID: 14145
		private ToolBarAppearance appearance;

		// Token: 0x04003742 RID: 14146
		private BorderStyle borderStyle;

		// Token: 0x04003743 RID: 14147
		private ToolBarButton[] buttons;

		// Token: 0x04003744 RID: 14148
		private int buttonCount;

		// Token: 0x04003745 RID: 14149
		private ToolBarTextAlign textAlign;

		// Token: 0x04003746 RID: 14150
		private ImageList imageList;

		// Token: 0x04003747 RID: 14151
		private int maxWidth = -1;

		// Token: 0x04003748 RID: 14152
		private int hotItem = -1;

		// Token: 0x04003749 RID: 14153
		private float currentScaleDX = 1f;

		// Token: 0x0400374A RID: 14154
		private float currentScaleDY = 1f;

		// Token: 0x0400374B RID: 14155
		private BitVector32 toolBarState;

		// Token: 0x0400374C RID: 14156
		private ToolBarButtonClickEventHandler onButtonClick;

		// Token: 0x0400374D RID: 14157
		private ToolBarButtonClickEventHandler onButtonDropDown;

		// Token: 0x02000667 RID: 1639
		public class ToolBarButtonCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x0600563B RID: 22075 RVA: 0x00139E46 File Offset: 0x00138E46
			public ToolBarButtonCollection(ToolBar owner)
			{
				this.owner = owner;
			}

			// Token: 0x170011E4 RID: 4580
			public virtual ToolBarButton this[int index]
			{
				get
				{
					if (index < 0 || (this.owner.buttons != null && index >= this.owner.buttonCount))
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.buttons[index];
				}
				set
				{
					if (index < 0 || (this.owner.buttons != null && index >= this.owner.buttonCount))
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}
					this.owner.InternalSetButton(index, value, true, true);
				}
			}

			// Token: 0x170011E5 RID: 4581
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is ToolBarButton)
					{
						this[index] = (ToolBarButton)value;
						return;
					}
					throw new ArgumentException(SR.GetString("ToolBarBadToolBarButton"), "value");
				}
			}

			// Token: 0x170011E6 RID: 4582
			public virtual ToolBarButton this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			// Token: 0x170011E7 RID: 4583
			// (get) Token: 0x06005641 RID: 22081 RVA: 0x00139FB1 File Offset: 0x00138FB1
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.buttonCount;
				}
			}

			// Token: 0x170011E8 RID: 4584
			// (get) Token: 0x06005642 RID: 22082 RVA: 0x00139FBE File Offset: 0x00138FBE
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170011E9 RID: 4585
			// (get) Token: 0x06005643 RID: 22083 RVA: 0x00139FC1 File Offset: 0x00138FC1
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170011EA RID: 4586
			// (get) Token: 0x06005644 RID: 22084 RVA: 0x00139FC4 File Offset: 0x00138FC4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170011EB RID: 4587
			// (get) Token: 0x06005645 RID: 22085 RVA: 0x00139FC7 File Offset: 0x00138FC7
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06005646 RID: 22086 RVA: 0x00139FCC File Offset: 0x00138FCC
			public int Add(ToolBarButton button)
			{
				int result = this.owner.InternalAddButton(button);
				if (!this.suspendUpdate)
				{
					this.owner.UpdateButtons();
				}
				return result;
			}

			// Token: 0x06005647 RID: 22087 RVA: 0x00139FFC File Offset: 0x00138FFC
			public int Add(string text)
			{
				ToolBarButton button = new ToolBarButton(text);
				return this.Add(button);
			}

			// Token: 0x06005648 RID: 22088 RVA: 0x0013A017 File Offset: 0x00139017
			int IList.Add(object button)
			{
				if (button is ToolBarButton)
				{
					return this.Add((ToolBarButton)button);
				}
				throw new ArgumentException(SR.GetString("ToolBarBadToolBarButton"), "button");
			}

			// Token: 0x06005649 RID: 22089 RVA: 0x0013A044 File Offset: 0x00139044
			public void AddRange(ToolBarButton[] buttons)
			{
				if (buttons == null)
				{
					throw new ArgumentNullException("buttons");
				}
				try
				{
					this.suspendUpdate = true;
					foreach (ToolBarButton button in buttons)
					{
						this.Add(button);
					}
				}
				finally
				{
					this.suspendUpdate = false;
					this.owner.UpdateButtons();
				}
			}

			// Token: 0x0600564A RID: 22090 RVA: 0x0013A0A8 File Offset: 0x001390A8
			public void Clear()
			{
				if (this.owner.buttons == null)
				{
					return;
				}
				for (int i = this.owner.buttonCount; i > 0; i--)
				{
					if (this.owner.IsHandleCreated)
					{
						this.owner.SendMessage(1046, i - 1, 0);
					}
					this.owner.RemoveAt(i - 1);
				}
				this.owner.buttons = null;
				this.owner.buttonCount = 0;
				if (!this.owner.Disposing)
				{
					this.owner.UpdateButtons();
				}
			}

			// Token: 0x0600564B RID: 22091 RVA: 0x0013A139 File Offset: 0x00139139
			public bool Contains(ToolBarButton button)
			{
				return this.IndexOf(button) != -1;
			}

			// Token: 0x0600564C RID: 22092 RVA: 0x0013A148 File Offset: 0x00139148
			bool IList.Contains(object button)
			{
				return button is ToolBarButton && this.Contains((ToolBarButton)button);
			}

			// Token: 0x0600564D RID: 22093 RVA: 0x0013A160 File Offset: 0x00139160
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x0600564E RID: 22094 RVA: 0x0013A16F File Offset: 0x0013916F
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.owner.buttonCount > 0)
				{
					Array.Copy(this.owner.buttons, 0, dest, index, this.owner.buttonCount);
				}
			}

			// Token: 0x0600564F RID: 22095 RVA: 0x0013A1A0 File Offset: 0x001391A0
			public int IndexOf(ToolBarButton button)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == button)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06005650 RID: 22096 RVA: 0x0013A1CB File Offset: 0x001391CB
			int IList.IndexOf(object button)
			{
				if (button is ToolBarButton)
				{
					return this.IndexOf((ToolBarButton)button);
				}
				return -1;
			}

			// Token: 0x06005651 RID: 22097 RVA: 0x0013A1E4 File Offset: 0x001391E4
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x06005652 RID: 22098 RVA: 0x0013A261 File Offset: 0x00139261
			public void Insert(int index, ToolBarButton button)
			{
				this.owner.InsertButton(index, button);
			}

			// Token: 0x06005653 RID: 22099 RVA: 0x0013A270 File Offset: 0x00139270
			void IList.Insert(int index, object button)
			{
				if (button is ToolBarButton)
				{
					this.Insert(index, (ToolBarButton)button);
					return;
				}
				throw new ArgumentException(SR.GetString("ToolBarBadToolBarButton"), "button");
			}

			// Token: 0x06005654 RID: 22100 RVA: 0x0013A29C File Offset: 0x0013929C
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x06005655 RID: 22101 RVA: 0x0013A2B0 File Offset: 0x001392B0
			public void RemoveAt(int index)
			{
				int num = (this.owner.buttons == null) ? 0 : this.owner.buttonCount;
				if (index < 0 || index >= num)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.IsHandleCreated)
				{
					this.owner.SendMessage(1046, index, 0);
				}
				this.owner.RemoveAt(index);
				this.owner.UpdateButtons();
			}

			// Token: 0x06005656 RID: 22102 RVA: 0x0013A34C File Offset: 0x0013934C
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			// Token: 0x06005657 RID: 22103 RVA: 0x0013A374 File Offset: 0x00139374
			public void Remove(ToolBarButton button)
			{
				int num = this.IndexOf(button);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06005658 RID: 22104 RVA: 0x0013A394 File Offset: 0x00139394
			void IList.Remove(object button)
			{
				if (button is ToolBarButton)
				{
					this.Remove((ToolBarButton)button);
				}
			}

			// Token: 0x06005659 RID: 22105 RVA: 0x0013A3AA File Offset: 0x001393AA
			public IEnumerator GetEnumerator()
			{
				return new WindowsFormsUtils.ArraySubsetEnumerator(this.owner.buttons, this.owner.buttonCount);
			}

			// Token: 0x0400374E RID: 14158
			private ToolBar owner;

			// Token: 0x0400374F RID: 14159
			private bool suspendUpdate;

			// Token: 0x04003750 RID: 14160
			private int lastAccessedIndex = -1;
		}
	}
}
