using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020005BF RID: 1471
	[DefaultProperty("Value")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[DefaultBindingProperty("Value")]
	[SRDescription("DescriptionProgressBar")]
	public class ProgressBar : Control
	{
		// Token: 0x06004C88 RID: 19592 RVA: 0x00119E70 File Offset: 0x00118E70
		public ProgressBar()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.UseTextForAccessibility, false);
			this.ForeColor = this.defaultForeColor;
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06004C89 RID: 19593 RVA: 0x00119EC0 File Offset: 0x00118EC0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "msctls_progress32";
				if (this.Style == ProgressBarStyle.Continuous)
				{
					createParams.Style |= 1;
				}
				else if (this.Style == ProgressBarStyle.Marquee && !base.DesignMode)
				{
					createParams.Style |= 8;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 4194304;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06004C8A RID: 19594 RVA: 0x00119F4D File Offset: 0x00118F4D
		// (set) Token: 0x06004C8B RID: 19595 RVA: 0x00119F55 File Offset: 0x00118F55
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

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06004C8C RID: 19596 RVA: 0x00119F5E File Offset: 0x00118F5E
		// (set) Token: 0x06004C8D RID: 19597 RVA: 0x00119F66 File Offset: 0x00118F66
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

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x06004C8E RID: 19598 RVA: 0x00119F6F File Offset: 0x00118F6F
		// (set) Token: 0x06004C8F RID: 19599 RVA: 0x00119F78 File Offset: 0x00118F78
		[DefaultValue(ProgressBarStyle.Blocks)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[SRDescription("ProgressBarStyleDescr")]
		[SRCategory("CatBehavior")]
		public ProgressBarStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (this.style != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ProgressBarStyle));
					}
					this.style = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
					if (this.style == ProgressBarStyle.Marquee)
					{
						this.StartMarquee();
					}
				}
			}
		}

		// Token: 0x140002B8 RID: 696
		// (add) Token: 0x06004C90 RID: 19600 RVA: 0x00119FD8 File Offset: 0x00118FD8
		// (remove) Token: 0x06004C91 RID: 19601 RVA: 0x00119FE1 File Offset: 0x00118FE1
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

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x06004C92 RID: 19602 RVA: 0x00119FEA File Offset: 0x00118FEA
		// (set) Token: 0x06004C93 RID: 19603 RVA: 0x00119FF2 File Offset: 0x00118FF2
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

		// Token: 0x140002B9 RID: 697
		// (add) Token: 0x06004C94 RID: 19604 RVA: 0x00119FFB File Offset: 0x00118FFB
		// (remove) Token: 0x06004C95 RID: 19605 RVA: 0x0011A004 File Offset: 0x00119004
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

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x0011A00D File Offset: 0x0011900D
		// (set) Token: 0x06004C97 RID: 19607 RVA: 0x0011A015 File Offset: 0x00119015
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		// Token: 0x140002BA RID: 698
		// (add) Token: 0x06004C98 RID: 19608 RVA: 0x0011A01E File Offset: 0x0011901E
		// (remove) Token: 0x06004C99 RID: 19609 RVA: 0x0011A027 File Offset: 0x00119027
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x06004C9A RID: 19610 RVA: 0x0011A030 File Offset: 0x00119030
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x06004C9B RID: 19611 RVA: 0x0011A033 File Offset: 0x00119033
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 23);
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x06004C9C RID: 19612 RVA: 0x0011A03E File Offset: 0x0011903E
		// (set) Token: 0x06004C9D RID: 19613 RVA: 0x0011A046 File Offset: 0x00119046
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

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x0011A04F File Offset: 0x0011904F
		// (set) Token: 0x06004C9F RID: 19615 RVA: 0x0011A057 File Offset: 0x00119057
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

		// Token: 0x140002BB RID: 699
		// (add) Token: 0x06004CA0 RID: 19616 RVA: 0x0011A060 File Offset: 0x00119060
		// (remove) Token: 0x06004CA1 RID: 19617 RVA: 0x0011A069 File Offset: 0x00119069
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

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x0011A072 File Offset: 0x00119072
		// (set) Token: 0x06004CA3 RID: 19619 RVA: 0x0011A07A File Offset: 0x0011907A
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

		// Token: 0x140002BC RID: 700
		// (add) Token: 0x06004CA4 RID: 19620 RVA: 0x0011A083 File Offset: 0x00119083
		// (remove) Token: 0x06004CA5 RID: 19621 RVA: 0x0011A08C File Offset: 0x0011908C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x06004CA6 RID: 19622 RVA: 0x0011A095 File Offset: 0x00119095
		// (set) Token: 0x06004CA7 RID: 19623 RVA: 0x0011A09D File Offset: 0x0011909D
		[SRDescription("ProgressBarMarqueeAnimationSpeed")]
		[DefaultValue(100)]
		[SRCategory("CatBehavior")]
		public int MarqueeAnimationSpeed
		{
			get
			{
				return this.marqueeSpeed;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MarqueeAnimationSpeed must be non-negative");
				}
				this.marqueeSpeed = value;
				if (!base.DesignMode)
				{
					this.StartMarquee();
				}
			}
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x0011A0C4 File Offset: 0x001190C4
		private void StartMarquee()
		{
			if (base.IsHandleCreated && this.style == ProgressBarStyle.Marquee)
			{
				if (this.marqueeSpeed == 0)
				{
					base.SendMessage(1034, 0, this.marqueeSpeed);
					return;
				}
				base.SendMessage(1034, 1, this.marqueeSpeed);
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x0011A111 File Offset: 0x00119111
		// (set) Token: 0x06004CAA RID: 19626 RVA: 0x0011A11C File Offset: 0x0011911C
		[DefaultValue(100)]
		[SRDescription("ProgressBarMaximumDescr")]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (this.maximum != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Maximum", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"Maximum",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.minimum > value)
					{
						this.minimum = value;
					}
					this.maximum = value;
					if (this.value > this.maximum)
					{
						this.value = this.maximum;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(1030, this.minimum, this.maximum);
						this.UpdatePos();
					}
				}
			}
		}

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x06004CAB RID: 19627 RVA: 0x0011A1D5 File Offset: 0x001191D5
		// (set) Token: 0x06004CAC RID: 19628 RVA: 0x0011A1E0 File Offset: 0x001191E0
		[SRDescription("ProgressBarMinimumDescr")]
		[DefaultValue(0)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				if (this.minimum != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Minimum", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"Minimum",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.maximum < value)
					{
						this.maximum = value;
					}
					this.minimum = value;
					if (this.value < this.minimum)
					{
						this.value = this.minimum;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(1030, this.minimum, this.maximum);
						this.UpdatePos();
					}
				}
			}
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x0011A299 File Offset: 0x00119299
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 8193, 0, ColorTranslator.ToWin32(this.BackColor));
			}
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x0011A2CD File Offset: 0x001192CD
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1033, 0, ColorTranslator.ToWin32(this.ForeColor));
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06004CAF RID: 19631 RVA: 0x0011A301 File Offset: 0x00119301
		// (set) Token: 0x06004CB0 RID: 19632 RVA: 0x0011A309 File Offset: 0x00119309
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x140002BD RID: 701
		// (add) Token: 0x06004CB1 RID: 19633 RVA: 0x0011A312 File Offset: 0x00119312
		// (remove) Token: 0x06004CB2 RID: 19634 RVA: 0x0011A31B File Offset: 0x0011931B
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x0011A324 File Offset: 0x00119324
		// (set) Token: 0x06004CB4 RID: 19636 RVA: 0x0011A32C File Offset: 0x0011932C
		[SRDescription("ControlRightToLeftLayoutDescr")]
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x140002BE RID: 702
		// (add) Token: 0x06004CB5 RID: 19637 RVA: 0x0011A380 File Offset: 0x00119380
		// (remove) Token: 0x06004CB6 RID: 19638 RVA: 0x0011A399 File Offset: 0x00119399
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Combine(this.onRightToLeftLayoutChanged, value);
			}
			remove
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Remove(this.onRightToLeftLayoutChanged, value);
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x0011A3B2 File Offset: 0x001193B2
		// (set) Token: 0x06004CB8 RID: 19640 RVA: 0x0011A3BA File Offset: 0x001193BA
		[DefaultValue(10)]
		[SRDescription("ProgressBarStepDescr")]
		[SRCategory("CatBehavior")]
		public int Step
		{
			get
			{
				return this.step;
			}
			set
			{
				this.step = value;
				if (base.IsHandleCreated)
				{
					base.SendMessage(1028, this.step, 0);
				}
			}
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x0011A3DE File Offset: 0x001193DE
		// (set) Token: 0x06004CBA RID: 19642 RVA: 0x0011A3E6 File Offset: 0x001193E6
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

		// Token: 0x140002BF RID: 703
		// (add) Token: 0x06004CBB RID: 19643 RVA: 0x0011A3EF File Offset: 0x001193EF
		// (remove) Token: 0x06004CBC RID: 19644 RVA: 0x0011A3F8 File Offset: 0x001193F8
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

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06004CBD RID: 19645 RVA: 0x0011A401 File Offset: 0x00119401
		// (set) Token: 0x06004CBE RID: 19646 RVA: 0x0011A409 File Offset: 0x00119409
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

		// Token: 0x140002C0 RID: 704
		// (add) Token: 0x06004CBF RID: 19647 RVA: 0x0011A412 File Offset: 0x00119412
		// (remove) Token: 0x06004CC0 RID: 19648 RVA: 0x0011A41B File Offset: 0x0011941B
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

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x0011A424 File Offset: 0x00119424
		// (set) Token: 0x06004CC2 RID: 19650 RVA: 0x0011A42C File Offset: 0x0011942C
		[SRDescription("ProgressBarValueDescr")]
		[DefaultValue(0)]
		[Bindable(true)]
		[SRCategory("CatBehavior")]
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					if (value < this.minimum || value > this.maximum)
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'minimum'",
							"'maximum'"
						}));
					}
					this.value = value;
					this.UpdatePos();
				}
			}
		}

		// Token: 0x140002C1 RID: 705
		// (add) Token: 0x06004CC3 RID: 19651 RVA: 0x0011A4A5 File Offset: 0x001194A5
		// (remove) Token: 0x06004CC4 RID: 19652 RVA: 0x0011A4AE File Offset: 0x001194AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler DoubleClick
		{
			add
			{
				base.DoubleClick += value;
			}
			remove
			{
				base.DoubleClick -= value;
			}
		}

		// Token: 0x140002C2 RID: 706
		// (add) Token: 0x06004CC5 RID: 19653 RVA: 0x0011A4B7 File Offset: 0x001194B7
		// (remove) Token: 0x06004CC6 RID: 19654 RVA: 0x0011A4C0 File Offset: 0x001194C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.MouseDoubleClick += value;
			}
			remove
			{
				base.MouseDoubleClick -= value;
			}
		}

		// Token: 0x140002C3 RID: 707
		// (add) Token: 0x06004CC7 RID: 19655 RVA: 0x0011A4C9 File Offset: 0x001194C9
		// (remove) Token: 0x06004CC8 RID: 19656 RVA: 0x0011A4D2 File Offset: 0x001194D2
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

		// Token: 0x140002C4 RID: 708
		// (add) Token: 0x06004CC9 RID: 19657 RVA: 0x0011A4DB File Offset: 0x001194DB
		// (remove) Token: 0x06004CCA RID: 19658 RVA: 0x0011A4E4 File Offset: 0x001194E4
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

		// Token: 0x140002C5 RID: 709
		// (add) Token: 0x06004CCB RID: 19659 RVA: 0x0011A4ED File Offset: 0x001194ED
		// (remove) Token: 0x06004CCC RID: 19660 RVA: 0x0011A4F6 File Offset: 0x001194F6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x140002C6 RID: 710
		// (add) Token: 0x06004CCD RID: 19661 RVA: 0x0011A4FF File Offset: 0x001194FF
		// (remove) Token: 0x06004CCE RID: 19662 RVA: 0x0011A508 File Offset: 0x00119508
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140002C7 RID: 711
		// (add) Token: 0x06004CCF RID: 19663 RVA: 0x0011A511 File Offset: 0x00119511
		// (remove) Token: 0x06004CD0 RID: 19664 RVA: 0x0011A51A File Offset: 0x0011951A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140002C8 RID: 712
		// (add) Token: 0x06004CD1 RID: 19665 RVA: 0x0011A523 File Offset: 0x00119523
		// (remove) Token: 0x06004CD2 RID: 19666 RVA: 0x0011A52C File Offset: 0x0011952C
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

		// Token: 0x06004CD3 RID: 19667 RVA: 0x0011A538 File Offset: 0x00119538
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 32
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0011A588 File Offset: 0x00119588
		public void Increment(int value)
		{
			if (this.Style == ProgressBarStyle.Marquee)
			{
				throw new InvalidOperationException(SR.GetString("ProgressBarIncrementMarqueeException"));
			}
			this.value += value;
			if (this.value < this.minimum)
			{
				this.value = this.minimum;
			}
			if (this.value > this.maximum)
			{
				this.value = this.maximum;
			}
			this.UpdatePos();
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x0011A5F8 File Offset: 0x001195F8
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(1030, this.minimum, this.maximum);
			base.SendMessage(1028, this.step, 0);
			base.SendMessage(1026, this.value, 0);
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 8193, 0, ColorTranslator.ToWin32(this.BackColor));
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1033, 0, ColorTranslator.ToWin32(this.ForeColor));
			this.StartMarquee();
			SystemEvents.UserPreferenceChanged += this.UserPreferenceChangedHandler;
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x0011A6A7 File Offset: 0x001196A7
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.UserPreferenceChangedHandler;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x0011A6C1 File Offset: 0x001196C1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				base.RecreateHandle();
			}
			if (this.onRightToLeftLayoutChanged != null)
			{
				this.onRightToLeftLayoutChanged(this, e);
			}
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x0011A6F0 File Offset: 0x001196F0
		public void PerformStep()
		{
			if (this.Style == ProgressBarStyle.Marquee)
			{
				throw new InvalidOperationException(SR.GetString("ProgressBarPerformStepMarqueeException"));
			}
			this.Increment(this.step);
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x0011A717 File Offset: 0x00119717
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetForeColor()
		{
			this.ForeColor = this.defaultForeColor;
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x0011A725 File Offset: 0x00119725
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal override bool ShouldSerializeForeColor()
		{
			return this.ForeColor != this.defaultForeColor;
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x0011A738 File Offset: 0x00119738
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				", Minimum: ",
				this.Minimum.ToString(CultureInfo.CurrentCulture),
				", Maximum: ",
				this.Maximum.ToString(CultureInfo.CurrentCulture),
				", Value: ",
				this.Value.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x0011A7B8 File Offset: 0x001197B8
		private void UpdatePos()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(1026, this.value, 0);
			}
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x0011A7D8 File Offset: 0x001197D8
		private void UserPreferenceChangedHandler(object o, UserPreferenceChangedEventArgs e)
		{
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1033, 0, ColorTranslator.ToWin32(this.ForeColor));
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 8193, 0, ColorTranslator.ToWin32(this.BackColor));
			}
		}

		// Token: 0x0400322C RID: 12844
		private int minimum;

		// Token: 0x0400322D RID: 12845
		private int maximum = 100;

		// Token: 0x0400322E RID: 12846
		private int step = 10;

		// Token: 0x0400322F RID: 12847
		private int value;

		// Token: 0x04003230 RID: 12848
		private int marqueeSpeed = 100;

		// Token: 0x04003231 RID: 12849
		private Color defaultForeColor = SystemColors.Highlight;

		// Token: 0x04003232 RID: 12850
		private ProgressBarStyle style;

		// Token: 0x04003233 RID: 12851
		private EventHandler onRightToLeftLayoutChanged;

		// Token: 0x04003234 RID: 12852
		private bool rightToLeftLayout;
	}
}
