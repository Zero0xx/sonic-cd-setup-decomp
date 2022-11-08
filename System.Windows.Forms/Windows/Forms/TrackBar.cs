using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006F4 RID: 1780
	[DefaultProperty("Value")]
	[DefaultBindingProperty("Value")]
	[DefaultEvent("Scroll")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.TrackBarDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionTrackBar")]
	public class TrackBar : Control, ISupportInitialize
	{
		// Token: 0x06005E4C RID: 24140 RVA: 0x00157194 File Offset: 0x00156194
		public TrackBar()
		{
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
			this.requestedDim = this.PreferredDimension;
		}

		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x06005E4D RID: 24141 RVA: 0x001571F2 File Offset: 0x001561F2
		// (set) Token: 0x06005E4E RID: 24142 RVA: 0x001571FC File Offset: 0x001561FC
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TrackBarAutoSizeDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return this.autoSize;
			}
			set
			{
				if (this.autoSize != value)
				{
					this.autoSize = value;
					if (this.orientation == Orientation.Horizontal)
					{
						base.SetStyle(ControlStyles.FixedHeight, this.autoSize);
						base.SetStyle(ControlStyles.FixedWidth, false);
					}
					else
					{
						base.SetStyle(ControlStyles.FixedWidth, this.autoSize);
						base.SetStyle(ControlStyles.FixedHeight, false);
					}
					this.AdjustSize();
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400038D RID: 909
		// (add) Token: 0x06005E4F RID: 24143 RVA: 0x00157262 File Offset: 0x00156262
		// (remove) Token: 0x06005E50 RID: 24144 RVA: 0x0015726B File Offset: 0x0015626B
		[SRCategory("CatPropertyChanged")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
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

		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x06005E51 RID: 24145 RVA: 0x00157274 File Offset: 0x00156274
		// (set) Token: 0x06005E52 RID: 24146 RVA: 0x0015727C File Offset: 0x0015627C
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

		// Token: 0x1400038E RID: 910
		// (add) Token: 0x06005E53 RID: 24147 RVA: 0x00157285 File Offset: 0x00156285
		// (remove) Token: 0x06005E54 RID: 24148 RVA: 0x0015728E File Offset: 0x0015628E
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

		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x06005E55 RID: 24149 RVA: 0x00157297 File Offset: 0x00156297
		// (set) Token: 0x06005E56 RID: 24150 RVA: 0x0015729F File Offset: 0x0015629F
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

		// Token: 0x1400038F RID: 911
		// (add) Token: 0x06005E57 RID: 24151 RVA: 0x001572A8 File Offset: 0x001562A8
		// (remove) Token: 0x06005E58 RID: 24152 RVA: 0x001572B1 File Offset: 0x001562B1
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

		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06005E59 RID: 24153 RVA: 0x001572BC File Offset: 0x001562BC
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "msctls_trackbar32";
				switch (this.tickStyle)
				{
				case TickStyle.None:
					createParams.Style |= 16;
					break;
				case TickStyle.TopLeft:
					createParams.Style |= 5;
					break;
				case TickStyle.BottomRight:
					createParams.Style |= 1;
					break;
				case TickStyle.Both:
					createParams.Style |= 9;
					break;
				}
				if (this.orientation == Orientation.Vertical)
				{
					createParams.Style |= 2;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 5242880;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x06005E5A RID: 24154 RVA: 0x00157387 File Offset: 0x00156387
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x06005E5B RID: 24155 RVA: 0x0015738A File Offset: 0x0015638A
		protected override Size DefaultSize
		{
			get
			{
				return new Size(104, this.PreferredDimension);
			}
		}

		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x06005E5C RID: 24156 RVA: 0x00157399 File Offset: 0x00156399
		// (set) Token: 0x06005E5D RID: 24157 RVA: 0x001573A1 File Offset: 0x001563A1
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

		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x06005E5E RID: 24158 RVA: 0x001573AA File Offset: 0x001563AA
		// (set) Token: 0x06005E5F RID: 24159 RVA: 0x001573B2 File Offset: 0x001563B2
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

		// Token: 0x14000390 RID: 912
		// (add) Token: 0x06005E60 RID: 24160 RVA: 0x001573BB File Offset: 0x001563BB
		// (remove) Token: 0x06005E61 RID: 24161 RVA: 0x001573C4 File Offset: 0x001563C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x06005E62 RID: 24162 RVA: 0x001573CD File Offset: 0x001563CD
		// (set) Token: 0x06005E63 RID: 24163 RVA: 0x001573D4 File Offset: 0x001563D4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return SystemColors.WindowText;
			}
			set
			{
			}
		}

		// Token: 0x14000391 RID: 913
		// (add) Token: 0x06005E64 RID: 24164 RVA: 0x001573D6 File Offset: 0x001563D6
		// (remove) Token: 0x06005E65 RID: 24165 RVA: 0x001573DF File Offset: 0x001563DF
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

		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x06005E66 RID: 24166 RVA: 0x001573E8 File Offset: 0x001563E8
		// (set) Token: 0x06005E67 RID: 24167 RVA: 0x001573F0 File Offset: 0x001563F0
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

		// Token: 0x14000392 RID: 914
		// (add) Token: 0x06005E68 RID: 24168 RVA: 0x001573F9 File Offset: 0x001563F9
		// (remove) Token: 0x06005E69 RID: 24169 RVA: 0x00157402 File Offset: 0x00156402
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

		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x06005E6A RID: 24170 RVA: 0x0015740B File Offset: 0x0015640B
		// (set) Token: 0x06005E6B RID: 24171 RVA: 0x00157414 File Offset: 0x00156414
		[DefaultValue(5)]
		[SRDescription("TrackBarLargeChangeDescr")]
		[SRCategory("CatBehavior")]
		public int LargeChange
		{
			get
			{
				return this.largeChange;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("LargeChange", SR.GetString("TrackBarLargeChangeError", new object[]
					{
						value
					}));
				}
				if (this.largeChange != value)
				{
					this.largeChange = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(1045, 0, value);
					}
				}
			}
		}

		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x06005E6C RID: 24172 RVA: 0x00157471 File Offset: 0x00156471
		// (set) Token: 0x06005E6D RID: 24173 RVA: 0x00157479 File Offset: 0x00156479
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(10)]
		[SRDescription("TrackBarMaximumDescr")]
		[SRCategory("CatBehavior")]
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
					if (value < this.minimum)
					{
						this.minimum = value;
					}
					this.SetRange(this.minimum, value);
				}
			}
		}

		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x06005E6E RID: 24174 RVA: 0x001574A1 File Offset: 0x001564A1
		// (set) Token: 0x06005E6F RID: 24175 RVA: 0x001574A9 File Offset: 0x001564A9
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("TrackBarMinimumDescr")]
		[SRCategory("CatBehavior")]
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
					if (value > this.maximum)
					{
						this.maximum = value;
					}
					this.SetRange(value, this.maximum);
				}
			}
		}

		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x06005E70 RID: 24176 RVA: 0x001574D1 File Offset: 0x001564D1
		// (set) Token: 0x06005E71 RID: 24177 RVA: 0x001574DC File Offset: 0x001564DC
		[DefaultValue(Orientation.Horizontal)]
		[Localizable(true)]
		[SRDescription("TrackBarOrientationDescr")]
		[SRCategory("CatAppearance")]
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
					if (this.orientation == Orientation.Horizontal)
					{
						base.SetStyle(ControlStyles.FixedHeight, this.autoSize);
						base.SetStyle(ControlStyles.FixedWidth, false);
						base.Width = this.requestedDim;
					}
					else
					{
						base.SetStyle(ControlStyles.FixedHeight, false);
						base.SetStyle(ControlStyles.FixedWidth, this.autoSize);
						base.Height = this.requestedDim;
					}
					if (base.IsHandleCreated)
					{
						Rectangle bounds = base.Bounds;
						base.RecreateHandle();
						base.SetBounds(bounds.X, bounds.Y, bounds.Height, bounds.Width, BoundsSpecified.All);
						this.AdjustSize();
					}
				}
			}
		}

		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x06005E72 RID: 24178 RVA: 0x001575B1 File Offset: 0x001565B1
		// (set) Token: 0x06005E73 RID: 24179 RVA: 0x001575B9 File Offset: 0x001565B9
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000393 RID: 915
		// (add) Token: 0x06005E74 RID: 24180 RVA: 0x001575C2 File Offset: 0x001565C2
		// (remove) Token: 0x06005E75 RID: 24181 RVA: 0x001575CB File Offset: 0x001565CB
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

		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x06005E76 RID: 24182 RVA: 0x001575D4 File Offset: 0x001565D4
		private int PreferredDimension
		{
			get
			{
				int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(3);
				return systemMetrics * 8 / 3;
			}
		}

		// Token: 0x06005E77 RID: 24183 RVA: 0x001575ED File Offset: 0x001565ED
		private void RedrawControl()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(1032, 1, this.maximum);
				base.Invalidate();
			}
		}

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x06005E78 RID: 24184 RVA: 0x00157610 File Offset: 0x00156610
		// (set) Token: 0x06005E79 RID: 24185 RVA: 0x00157618 File Offset: 0x00156618
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		[DefaultValue(false)]
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

		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x06005E7A RID: 24186 RVA: 0x0015766C File Offset: 0x0015666C
		// (set) Token: 0x06005E7B RID: 24187 RVA: 0x00157674 File Offset: 0x00156674
		[SRCategory("CatAppearance")]
		[SRDescription("TrackBarSmallChangeDescr")]
		[DefaultValue(1)]
		public int SmallChange
		{
			get
			{
				return this.smallChange;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SmallChange", SR.GetString("TrackBarSmallChangeError", new object[]
					{
						value
					}));
				}
				if (this.smallChange != value)
				{
					this.smallChange = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(1047, 0, value);
					}
				}
			}
		}

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06005E7C RID: 24188 RVA: 0x001576D1 File Offset: 0x001566D1
		// (set) Token: 0x06005E7D RID: 24189 RVA: 0x001576D9 File Offset: 0x001566D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000394 RID: 916
		// (add) Token: 0x06005E7E RID: 24190 RVA: 0x001576E2 File Offset: 0x001566E2
		// (remove) Token: 0x06005E7F RID: 24191 RVA: 0x001576EB File Offset: 0x001566EB
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

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06005E80 RID: 24192 RVA: 0x001576F4 File Offset: 0x001566F4
		// (set) Token: 0x06005E81 RID: 24193 RVA: 0x001576FC File Offset: 0x001566FC
		[DefaultValue(TickStyle.BottomRight)]
		[SRDescription("TrackBarTickStyleDescr")]
		[SRCategory("CatAppearance")]
		public TickStyle TickStyle
		{
			get
			{
				return this.tickStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TickStyle));
				}
				if (this.tickStyle != value)
				{
					this.tickStyle = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x06005E82 RID: 24194 RVA: 0x0015773A File Offset: 0x0015673A
		// (set) Token: 0x06005E83 RID: 24195 RVA: 0x00157742 File Offset: 0x00156742
		[SRDescription("TrackBarTickFrequencyDescr")]
		[DefaultValue(1)]
		[SRCategory("CatAppearance")]
		public int TickFrequency
		{
			get
			{
				return this.tickFrequency;
			}
			set
			{
				if (this.tickFrequency != value)
				{
					this.tickFrequency = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(1044, value, 0);
						base.Invalidate();
					}
				}
			}
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x06005E84 RID: 24196 RVA: 0x00157770 File Offset: 0x00156770
		// (set) Token: 0x06005E85 RID: 24197 RVA: 0x00157780 File Offset: 0x00156780
		[Bindable(true)]
		[DefaultValue(0)]
		[SRDescription("TrackBarValueDescr")]
		[SRCategory("CatBehavior")]
		public int Value
		{
			get
			{
				this.GetTrackBarValue();
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					if (!this.initializing && (value < this.minimum || value > this.maximum))
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'Minimum'",
							"'Maximum'"
						}));
					}
					this.value = value;
					this.SetTrackBarPosition();
					this.OnValueChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000395 RID: 917
		// (add) Token: 0x06005E86 RID: 24198 RVA: 0x0015780C File Offset: 0x0015680C
		// (remove) Token: 0x06005E87 RID: 24199 RVA: 0x00157815 File Offset: 0x00156815
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		// Token: 0x14000396 RID: 918
		// (add) Token: 0x06005E88 RID: 24200 RVA: 0x0015781E File Offset: 0x0015681E
		// (remove) Token: 0x06005E89 RID: 24201 RVA: 0x00157827 File Offset: 0x00156827
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000397 RID: 919
		// (add) Token: 0x06005E8A RID: 24202 RVA: 0x00157830 File Offset: 0x00156830
		// (remove) Token: 0x06005E8B RID: 24203 RVA: 0x00157839 File Offset: 0x00156839
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		// Token: 0x14000398 RID: 920
		// (add) Token: 0x06005E8C RID: 24204 RVA: 0x00157842 File Offset: 0x00156842
		// (remove) Token: 0x06005E8D RID: 24205 RVA: 0x0015784B File Offset: 0x0015684B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000399 RID: 921
		// (add) Token: 0x06005E8E RID: 24206 RVA: 0x00157854 File Offset: 0x00156854
		// (remove) Token: 0x06005E8F RID: 24207 RVA: 0x00157867 File Offset: 0x00156867
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(TrackBar.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TrackBar.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
		}

		// Token: 0x1400039A RID: 922
		// (add) Token: 0x06005E90 RID: 24208 RVA: 0x0015787A File Offset: 0x0015687A
		// (remove) Token: 0x06005E91 RID: 24209 RVA: 0x0015788D File Offset: 0x0015688D
		[SRCategory("CatBehavior")]
		[SRDescription("TrackBarOnScrollDescr")]
		public event EventHandler Scroll
		{
			add
			{
				base.Events.AddHandler(TrackBar.EVENT_SCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(TrackBar.EVENT_SCROLL, value);
			}
		}

		// Token: 0x1400039B RID: 923
		// (add) Token: 0x06005E92 RID: 24210 RVA: 0x001578A0 File Offset: 0x001568A0
		// (remove) Token: 0x06005E93 RID: 24211 RVA: 0x001578A9 File Offset: 0x001568A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x1400039C RID: 924
		// (add) Token: 0x06005E94 RID: 24212 RVA: 0x001578B2 File Offset: 0x001568B2
		// (remove) Token: 0x06005E95 RID: 24213 RVA: 0x001578C5 File Offset: 0x001568C5
		[SRDescription("valueChangedEventDescr")]
		[SRCategory("CatAction")]
		public event EventHandler ValueChanged
		{
			add
			{
				base.Events.AddHandler(TrackBar.EVENT_VALUECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TrackBar.EVENT_VALUECHANGED, value);
			}
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x001578D8 File Offset: 0x001568D8
		private void AdjustSize()
		{
			if (base.IsHandleCreated)
			{
				int num = this.requestedDim;
				try
				{
					if (this.orientation == Orientation.Horizontal)
					{
						base.Height = (this.autoSize ? this.PreferredDimension : num);
					}
					else
					{
						base.Width = (this.autoSize ? this.PreferredDimension : num);
					}
				}
				finally
				{
					this.requestedDim = num;
				}
			}
		}

		// Token: 0x06005E97 RID: 24215 RVA: 0x00157948 File Offset: 0x00156948
		public void BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x00157951 File Offset: 0x00156951
		private void ConstrainValue()
		{
			if (this.initializing)
			{
				return;
			}
			if (this.Value < this.minimum)
			{
				this.Value = this.minimum;
			}
			if (this.Value > this.maximum)
			{
				this.Value = this.maximum;
			}
		}

		// Token: 0x06005E99 RID: 24217 RVA: 0x00157990 File Offset: 0x00156990
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

		// Token: 0x06005E9A RID: 24218 RVA: 0x001579E0 File Offset: 0x001569E0
		public void EndInit()
		{
			this.initializing = false;
			this.ConstrainValue();
		}

		// Token: 0x06005E9B RID: 24219 RVA: 0x001579F0 File Offset: 0x001569F0
		private void GetTrackBarValue()
		{
			if (base.IsHandleCreated)
			{
				this.value = (int)base.SendMessage(1024, 0, 0);
				if (this.orientation == Orientation.Vertical)
				{
					this.value = this.Minimum + this.Maximum - this.value;
				}
				if (this.orientation == Orientation.Horizontal && this.RightToLeft == RightToLeft.Yes && !base.IsMirrored)
				{
					this.value = this.Minimum + this.Maximum - this.value;
				}
			}
		}

		// Token: 0x06005E9C RID: 24220 RVA: 0x00157A74 File Offset: 0x00156A74
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			switch (keyData & Keys.KeyCode)
			{
			case Keys.Prior:
			case Keys.Next:
			case Keys.End:
			case Keys.Home:
				return true;
			default:
				return base.IsInputKey(keyData);
			}
		}

		// Token: 0x06005E9D RID: 24221 RVA: 0x00157AC0 File Offset: 0x00156AC0
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(1031, 0, this.minimum);
			base.SendMessage(1032, 0, this.maximum);
			base.SendMessage(1044, this.tickFrequency, 0);
			base.SendMessage(1045, 0, this.largeChange);
			base.SendMessage(1047, 0, this.smallChange);
			this.SetTrackBarPosition();
			this.AdjustSize();
		}

		// Token: 0x06005E9E RID: 24222 RVA: 0x00157B40 File Offset: 0x00156B40
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
			EventHandler eventHandler = base.Events[TrackBar.EVENT_RIGHTTOLEFTLAYOUTCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005E9F RID: 24223 RVA: 0x00157B88 File Offset: 0x00156B88
		protected virtual void OnScroll(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TrackBar.EVENT_SCROLL];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005EA0 RID: 24224 RVA: 0x00157BB8 File Offset: 0x00156BB8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			HandledMouseEventArgs handledMouseEventArgs = e as HandledMouseEventArgs;
			if (handledMouseEventArgs != null)
			{
				if (handledMouseEventArgs.Handled)
				{
					return;
				}
				handledMouseEventArgs.Handled = true;
			}
			if ((Control.ModifierKeys & (Keys.Shift | Keys.Alt)) != Keys.None || Control.MouseButtons != MouseButtons.None)
			{
				return;
			}
			int mouseWheelScrollLines = SystemInformation.MouseWheelScrollLines;
			if (mouseWheelScrollLines == 0)
			{
				return;
			}
			this.cumulativeWheelData += e.Delta;
			float num = (float)this.cumulativeWheelData / 120f;
			if (mouseWheelScrollLines == -1)
			{
				mouseWheelScrollLines = this.TickFrequency;
			}
			int num2 = (int)((float)mouseWheelScrollLines * num);
			if (num2 != 0)
			{
				if (num2 > 0)
				{
					int num3 = num2;
					this.Value = Math.Min(num3 + this.Value, this.Maximum);
					this.cumulativeWheelData -= (int)((float)num2 * (120f / (float)mouseWheelScrollLines));
				}
				else
				{
					int num3 = -num2;
					this.Value = Math.Max(this.Value - num3, this.Minimum);
					this.cumulativeWheelData -= (int)((float)num2 * (120f / (float)mouseWheelScrollLines));
				}
			}
			if (e.Delta != this.Value)
			{
				this.OnScroll(EventArgs.Empty);
				this.OnValueChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06005EA1 RID: 24225 RVA: 0x00157CD4 File Offset: 0x00156CD4
		protected virtual void OnValueChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TrackBar.EVENT_VALUECHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005EA2 RID: 24226 RVA: 0x00157D02 File Offset: 0x00156D02
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.RedrawControl();
		}

		// Token: 0x06005EA3 RID: 24227 RVA: 0x00157D11 File Offset: 0x00156D11
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			this.RedrawControl();
		}

		// Token: 0x06005EA4 RID: 24228 RVA: 0x00157D20 File Offset: 0x00156D20
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			this.requestedDim = ((this.orientation == Orientation.Horizontal) ? height : width);
			if (this.autoSize)
			{
				if (this.orientation == Orientation.Horizontal)
				{
					if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
					{
						height = this.PreferredDimension;
					}
				}
				else if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
				{
					width = this.PreferredDimension;
				}
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06005EA5 RID: 24229 RVA: 0x00157D7C File Offset: 0x00156D7C
		public void SetRange(int minValue, int maxValue)
		{
			if (this.minimum != minValue || this.maximum != maxValue)
			{
				if (minValue > maxValue)
				{
					maxValue = minValue;
				}
				this.minimum = minValue;
				this.maximum = maxValue;
				if (base.IsHandleCreated)
				{
					base.SendMessage(1031, 0, this.minimum);
					base.SendMessage(1032, 1, this.maximum);
					base.Invalidate();
				}
				if (this.value < this.minimum)
				{
					this.value = this.minimum;
				}
				if (this.value > this.maximum)
				{
					this.value = this.maximum;
				}
				this.SetTrackBarPosition();
			}
		}

		// Token: 0x06005EA6 RID: 24230 RVA: 0x00157E24 File Offset: 0x00156E24
		private void SetTrackBarPosition()
		{
			if (base.IsHandleCreated)
			{
				int lparam = this.value;
				if (this.orientation == Orientation.Vertical)
				{
					lparam = this.Minimum + this.Maximum - this.value;
				}
				if (this.orientation == Orientation.Horizontal && this.RightToLeft == RightToLeft.Yes && !base.IsMirrored)
				{
					lparam = this.Minimum + this.Maximum - this.value;
				}
				base.SendMessage(1029, 1, lparam);
			}
		}

		// Token: 0x06005EA7 RID: 24231 RVA: 0x00157E9C File Offset: 0x00156E9C
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

		// Token: 0x06005EA8 RID: 24232 RVA: 0x00157F1C File Offset: 0x00156F1C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
			case 8468:
			case 8469:
				switch (NativeMethods.Util.LOWORD(m.WParam))
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 5:
				case 6:
				case 7:
				case 8:
					if (this.value != this.Value)
					{
						this.OnScroll(EventArgs.Empty);
						this.OnValueChanged(EventArgs.Empty);
						return;
					}
					break;
				case 4:
					break;
				default:
					return;
				}
				break;
			default:
				base.WndProc(ref m);
				break;
			}
		}

		// Token: 0x04003993 RID: 14739
		private static readonly object EVENT_SCROLL = new object();

		// Token: 0x04003994 RID: 14740
		private static readonly object EVENT_VALUECHANGED = new object();

		// Token: 0x04003995 RID: 14741
		private static readonly object EVENT_RIGHTTOLEFTLAYOUTCHANGED = new object();

		// Token: 0x04003996 RID: 14742
		private bool autoSize = true;

		// Token: 0x04003997 RID: 14743
		private int largeChange = 5;

		// Token: 0x04003998 RID: 14744
		private int maximum = 10;

		// Token: 0x04003999 RID: 14745
		private int minimum;

		// Token: 0x0400399A RID: 14746
		private Orientation orientation;

		// Token: 0x0400399B RID: 14747
		private int value;

		// Token: 0x0400399C RID: 14748
		private int smallChange = 1;

		// Token: 0x0400399D RID: 14749
		private int tickFrequency = 1;

		// Token: 0x0400399E RID: 14750
		private TickStyle tickStyle = TickStyle.BottomRight;

		// Token: 0x0400399F RID: 14751
		private int requestedDim;

		// Token: 0x040039A0 RID: 14752
		private int cumulativeWheelData;

		// Token: 0x040039A1 RID: 14753
		private bool initializing;

		// Token: 0x040039A2 RID: 14754
		private bool rightToLeftLayout;
	}
}
