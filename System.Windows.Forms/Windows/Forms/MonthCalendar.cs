using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020004C6 RID: 1222
	[ComVisible(true)]
	[DefaultProperty("SelectionRange")]
	[DefaultBindingProperty("SelectionRange")]
	[Designer("System.Windows.Forms.Design.MonthCalendarDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionMonthCalendar")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("DateChanged")]
	public class MonthCalendar : Control
	{
		// Token: 0x060048C2 RID: 18626 RVA: 0x00109664 File Offset: 0x00108664
		public MonthCalendar()
		{
			this.selectionStart = this.todayDate;
			this.selectionEnd = this.todayDate;
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.TabStop = true;
			if (MonthCalendar.restrictUnmanagedCode == null)
			{
				bool flag = false;
				try
				{
					IntSecurity.UnmanagedCode.Demand();
					MonthCalendar.restrictUnmanagedCode = new bool?(false);
				}
				catch
				{
					flag = true;
				}
				if (flag)
				{
					new RegistryPermission(PermissionState.Unrestricted).Assert();
					try
					{
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework");
						if (registryKey != null)
						{
							object value = registryKey.GetValue("AllowWindowsFormsReentrantDestroy");
							if (value != null && value is int && (int)value == 1)
							{
								MonthCalendar.restrictUnmanagedCode = new bool?(false);
							}
							else
							{
								MonthCalendar.restrictUnmanagedCode = new bool?(true);
							}
						}
						else
						{
							MonthCalendar.restrictUnmanagedCode = new bool?(true);
						}
					}
					catch
					{
						MonthCalendar.restrictUnmanagedCode = new bool?(true);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x060048C3 RID: 18627 RVA: 0x0010982C File Offset: 0x0010882C
		// (set) Token: 0x060048C4 RID: 18628 RVA: 0x00109880 File Offset: 0x00108880
		[Localizable(true)]
		[SRDescription("MonthCalendarAnnuallyBoldedDatesDescr")]
		public DateTime[] AnnuallyBoldedDates
		{
			get
			{
				DateTime[] array = new DateTime[this.annualArrayOfDates.Count];
				for (int i = 0; i < this.annualArrayOfDates.Count; i++)
				{
					array[i] = (DateTime)this.annualArrayOfDates[i];
				}
				return array;
			}
			set
			{
				this.annualArrayOfDates.Clear();
				for (int i = 0; i < 12; i++)
				{
					this.monthsOfYear[i] = 0;
				}
				if (value != null && value.Length > 0)
				{
					for (int j = 0; j < value.Length; j++)
					{
						this.annualArrayOfDates.Add(value[j]);
					}
					for (int k = 0; k < value.Length; k++)
					{
						this.monthsOfYear[value[k].Month - 1] |= 1 << value[k].Day - 1;
					}
				}
				base.RecreateHandle();
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x060048C5 RID: 18629 RVA: 0x0010992E File Offset: 0x0010892E
		// (set) Token: 0x060048C6 RID: 18630 RVA: 0x00109944 File Offset: 0x00108944
		[SRDescription("MonthCalendarMonthBackColorDescr")]
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x060048C7 RID: 18631 RVA: 0x0010994D File Offset: 0x0010894D
		// (set) Token: 0x060048C8 RID: 18632 RVA: 0x00109955 File Offset: 0x00108955
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

		// Token: 0x1400028E RID: 654
		// (add) Token: 0x060048C9 RID: 18633 RVA: 0x0010995E File Offset: 0x0010895E
		// (remove) Token: 0x060048CA RID: 18634 RVA: 0x00109967 File Offset: 0x00108967
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

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x060048CB RID: 18635 RVA: 0x00109970 File Offset: 0x00108970
		// (set) Token: 0x060048CC RID: 18636 RVA: 0x00109978 File Offset: 0x00108978
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

		// Token: 0x1400028F RID: 655
		// (add) Token: 0x060048CD RID: 18637 RVA: 0x00109981 File Offset: 0x00108981
		// (remove) Token: 0x060048CE RID: 18638 RVA: 0x0010998A File Offset: 0x0010898A
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

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x060048CF RID: 18639 RVA: 0x00109994 File Offset: 0x00108994
		// (set) Token: 0x060048D0 RID: 18640 RVA: 0x001099E8 File Offset: 0x001089E8
		[Localizable(true)]
		public DateTime[] BoldedDates
		{
			get
			{
				DateTime[] array = new DateTime[this.arrayOfDates.Count];
				for (int i = 0; i < this.arrayOfDates.Count; i++)
				{
					array[i] = (DateTime)this.arrayOfDates[i];
				}
				return array;
			}
			set
			{
				this.arrayOfDates.Clear();
				if (value != null && value.Length > 0)
				{
					for (int i = 0; i < value.Length; i++)
					{
						this.arrayOfDates.Add(value[i]);
					}
				}
				base.RecreateHandle();
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x00109A3A File Offset: 0x00108A3A
		// (set) Token: 0x060048D2 RID: 18642 RVA: 0x00109A42 File Offset: 0x00108A42
		[SRCategory("CatAppearance")]
		[SRDescription("MonthCalendarDimensionsDescr")]
		[Localizable(true)]
		public Size CalendarDimensions
		{
			get
			{
				return this.dimensions;
			}
			set
			{
				if (!this.dimensions.Equals(value))
				{
					this.SetCalendarDimensions(value.Width, value.Height);
				}
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x060048D3 RID: 18643 RVA: 0x00109A74 File Offset: 0x00108A74
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysMonthCal32";
				createParams.Style |= 3;
				if (!this.showToday)
				{
					createParams.Style |= 16;
				}
				if (!this.showTodayCircle)
				{
					createParams.Style |= 8;
				}
				if (this.showWeekNumbers)
				{
					createParams.Style |= 4;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 4194304;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x060048D4 RID: 18644 RVA: 0x00109B1A File Offset: 0x00108B1A
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x060048D5 RID: 18645 RVA: 0x00109B1D File Offset: 0x00108B1D
		protected override Padding DefaultMargin
		{
			get
			{
				return new Padding(9);
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x060048D6 RID: 18646 RVA: 0x00109B26 File Offset: 0x00108B26
		protected override Size DefaultSize
		{
			get
			{
				return this.GetMinReqRect();
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x060048D7 RID: 18647 RVA: 0x00109B2E File Offset: 0x00108B2E
		// (set) Token: 0x060048D8 RID: 18648 RVA: 0x00109B36 File Offset: 0x00108B36
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

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x060048D9 RID: 18649 RVA: 0x00109B3F File Offset: 0x00108B3F
		// (set) Token: 0x060048DA RID: 18650 RVA: 0x00109B48 File Offset: 0x00108B48
		[Localizable(true)]
		[DefaultValue(Day.Default)]
		[SRCategory("CatBehavior")]
		[SRDescription("MonthCalendarFirstDayOfWeekDescr")]
		public Day FirstDayOfWeek
		{
			get
			{
				return this.firstDayOfWeek;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("FirstDayOfWeek", (int)value, typeof(Day));
				}
				if (value != this.firstDayOfWeek)
				{
					this.firstDayOfWeek = value;
					if (base.IsHandleCreated)
					{
						if (value == Day.Default)
						{
							base.RecreateHandle();
							return;
						}
						base.SendMessage(4111, 0, (int)value);
					}
				}
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x060048DB RID: 18651 RVA: 0x00109BAC File Offset: 0x00108BAC
		// (set) Token: 0x060048DC RID: 18652 RVA: 0x00109BC2 File Offset: 0x00108BC2
		[SRDescription("MonthCalendarForeColorDescr")]
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x060048DD RID: 18653 RVA: 0x00109BCB File Offset: 0x00108BCB
		// (set) Token: 0x060048DE RID: 18654 RVA: 0x00109BD3 File Offset: 0x00108BD3
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

		// Token: 0x14000290 RID: 656
		// (add) Token: 0x060048DF RID: 18655 RVA: 0x00109BDC File Offset: 0x00108BDC
		// (remove) Token: 0x060048E0 RID: 18656 RVA: 0x00109BE5 File Offset: 0x00108BE5
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

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x060048E1 RID: 18657 RVA: 0x00109BEE File Offset: 0x00108BEE
		// (set) Token: 0x060048E2 RID: 18658 RVA: 0x00109BFC File Offset: 0x00108BFC
		[SRCategory("CatBehavior")]
		[SRDescription("MonthCalendarMaxDateDescr")]
		public DateTime MaxDate
		{
			get
			{
				return DateTimePicker.EffectiveMaxDate(this.maxDate);
			}
			set
			{
				if (value != this.maxDate)
				{
					if (value < DateTimePicker.EffectiveMinDate(this.minDate))
					{
						throw new ArgumentOutOfRangeException("MaxDate", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"MaxDate",
							MonthCalendar.FormatDate(value),
							"MinDate"
						}));
					}
					this.maxDate = value;
					this.SetRange();
				}
			}
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x060048E3 RID: 18659 RVA: 0x00109C6D File Offset: 0x00108C6D
		// (set) Token: 0x060048E4 RID: 18660 RVA: 0x00109C78 File Offset: 0x00108C78
		[DefaultValue(7)]
		[SRDescription("MonthCalendarMaxSelectionCountDescr")]
		[SRCategory("CatBehavior")]
		public int MaxSelectionCount
		{
			get
			{
				return this.maxSelectionCount;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("MaxSelectionCount", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"MaxSelectionCount",
						value.ToString("D", CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value != this.maxSelectionCount)
				{
					if (base.IsHandleCreated && (int)base.SendMessage(4100, value, 0) == 0)
					{
						throw new ArgumentException(SR.GetString("MonthCalendarMaxSelCount", new object[]
						{
							value.ToString("D", CultureInfo.CurrentCulture)
						}), "MaxSelectionCount");
					}
					this.maxSelectionCount = value;
				}
			}
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x060048E5 RID: 18661 RVA: 0x00109D30 File Offset: 0x00108D30
		// (set) Token: 0x060048E6 RID: 18662 RVA: 0x00109D40 File Offset: 0x00108D40
		[SRDescription("MonthCalendarMinDateDescr")]
		[SRCategory("CatBehavior")]
		public DateTime MinDate
		{
			get
			{
				return DateTimePicker.EffectiveMinDate(this.minDate);
			}
			set
			{
				if (value != this.minDate)
				{
					if (value > DateTimePicker.EffectiveMaxDate(this.maxDate))
					{
						throw new ArgumentOutOfRangeException("MinDate", SR.GetString("InvalidHighBoundArgument", new object[]
						{
							"MinDate",
							MonthCalendar.FormatDate(value),
							"MaxDate"
						}));
					}
					if (value < DateTimePicker.MinimumDateTime)
					{
						throw new ArgumentOutOfRangeException("MinDate", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"MinDate",
							MonthCalendar.FormatDate(value),
							MonthCalendar.FormatDate(DateTimePicker.MinimumDateTime)
						}));
					}
					this.minDate = value;
					this.SetRange();
				}
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x060048E7 RID: 18663 RVA: 0x00109DFC File Offset: 0x00108DFC
		// (set) Token: 0x060048E8 RID: 18664 RVA: 0x00109E50 File Offset: 0x00108E50
		[Localizable(true)]
		[SRDescription("MonthCalendarMonthlyBoldedDatesDescr")]
		public DateTime[] MonthlyBoldedDates
		{
			get
			{
				DateTime[] array = new DateTime[this.monthlyArrayOfDates.Count];
				for (int i = 0; i < this.monthlyArrayOfDates.Count; i++)
				{
					array[i] = (DateTime)this.monthlyArrayOfDates[i];
				}
				return array;
			}
			set
			{
				this.monthlyArrayOfDates.Clear();
				this.datesToBoldMonthly = 0;
				if (value != null && value.Length > 0)
				{
					for (int i = 0; i < value.Length; i++)
					{
						this.monthlyArrayOfDates.Add(value[i]);
					}
					for (int j = 0; j < value.Length; j++)
					{
						this.datesToBoldMonthly |= 1 << value[j].Day - 1;
					}
				}
				base.RecreateHandle();
			}
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x060048E9 RID: 18665 RVA: 0x00109ED8 File Offset: 0x00108ED8
		private DateTime Now
		{
			get
			{
				return DateTime.Now.Date;
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x060048EA RID: 18666 RVA: 0x00109EF2 File Offset: 0x00108EF2
		// (set) Token: 0x060048EB RID: 18667 RVA: 0x00109EFA File Offset: 0x00108EFA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000291 RID: 657
		// (add) Token: 0x060048EC RID: 18668 RVA: 0x00109F03 File Offset: 0x00108F03
		// (remove) Token: 0x060048ED RID: 18669 RVA: 0x00109F0C File Offset: 0x00108F0C
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

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x060048EE RID: 18670 RVA: 0x00109F15 File Offset: 0x00108F15
		// (set) Token: 0x060048EF RID: 18671 RVA: 0x00109F20 File Offset: 0x00108F20
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
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

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x060048F0 RID: 18672 RVA: 0x00109F74 File Offset: 0x00108F74
		// (set) Token: 0x060048F1 RID: 18673 RVA: 0x00109F7C File Offset: 0x00108F7C
		[SRCategory("CatBehavior")]
		[SRDescription("MonthCalendarScrollChangeDescr")]
		[DefaultValue(0)]
		public int ScrollChange
		{
			get
			{
				return this.scrollChange;
			}
			set
			{
				if (this.scrollChange != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("ScrollChange", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"ScrollChange",
							value.ToString("D", CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value > 20000)
					{
						throw new ArgumentOutOfRangeException("ScrollChange", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"ScrollChange",
							value.ToString("D", CultureInfo.CurrentCulture),
							20000.ToString("D", CultureInfo.CurrentCulture)
						}));
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(4116, value, 0);
					}
					this.scrollChange = value;
				}
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x060048F2 RID: 18674 RVA: 0x0010A05B File Offset: 0x0010905B
		// (set) Token: 0x060048F3 RID: 18675 RVA: 0x0010A064 File Offset: 0x00109064
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("MonthCalendarSelectionEndDescr")]
		[SRCategory("CatBehavior")]
		public DateTime SelectionEnd
		{
			get
			{
				return this.selectionEnd;
			}
			set
			{
				if (this.selectionEnd != value)
				{
					if (value < this.MinDate)
					{
						throw new ArgumentOutOfRangeException("SelectionEnd", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"SelectionEnd",
							MonthCalendar.FormatDate(value),
							"MinDate"
						}));
					}
					if (value > this.MaxDate)
					{
						throw new ArgumentOutOfRangeException("SelectionEnd", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"SelectionEnd",
							MonthCalendar.FormatDate(value),
							"MaxDate"
						}));
					}
					if (this.selectionStart > value)
					{
						this.selectionStart = value;
					}
					if ((value - this.selectionStart).Days >= this.maxSelectionCount)
					{
						this.selectionStart = value.AddDays((double)(1 - this.maxSelectionCount));
					}
					this.SetSelRange(this.selectionStart, value);
				}
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x060048F4 RID: 18676 RVA: 0x0010A15E File Offset: 0x0010915E
		// (set) Token: 0x060048F5 RID: 18677 RVA: 0x0010A168 File Offset: 0x00109168
		[Browsable(false)]
		[SRDescription("MonthCalendarSelectionStartDescr")]
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DateTime SelectionStart
		{
			get
			{
				return this.selectionStart;
			}
			set
			{
				if (this.selectionStart != value)
				{
					if (value < this.minDate)
					{
						throw new ArgumentOutOfRangeException("SelectionStart", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"SelectionStart",
							MonthCalendar.FormatDate(value),
							"MinDate"
						}));
					}
					if (value > this.maxDate)
					{
						throw new ArgumentOutOfRangeException("SelectionStart", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"SelectionStart",
							MonthCalendar.FormatDate(value),
							"MaxDate"
						}));
					}
					if (this.selectionEnd < value)
					{
						this.selectionEnd = value;
					}
					if ((this.selectionEnd - value).Days >= this.maxSelectionCount)
					{
						this.selectionEnd = value.AddDays((double)(this.maxSelectionCount - 1));
					}
					this.SetSelRange(value, this.selectionEnd);
				}
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x060048F6 RID: 18678 RVA: 0x0010A262 File Offset: 0x00109262
		// (set) Token: 0x060048F7 RID: 18679 RVA: 0x0010A275 File Offset: 0x00109275
		[Bindable(true)]
		[SRDescription("MonthCalendarSelectionRangeDescr")]
		[SRCategory("CatBehavior")]
		public SelectionRange SelectionRange
		{
			get
			{
				return new SelectionRange(this.SelectionStart, this.SelectionEnd);
			}
			set
			{
				this.SetSelectionRange(value.Start, value.End);
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x060048F8 RID: 18680 RVA: 0x0010A289 File Offset: 0x00109289
		// (set) Token: 0x060048F9 RID: 18681 RVA: 0x0010A291 File Offset: 0x00109291
		[SRDescription("MonthCalendarShowTodayDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool ShowToday
		{
			get
			{
				return this.showToday;
			}
			set
			{
				if (this.showToday != value)
				{
					this.showToday = value;
					base.UpdateStyles();
					this.AdjustSize();
				}
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x060048FA RID: 18682 RVA: 0x0010A2AF File Offset: 0x001092AF
		// (set) Token: 0x060048FB RID: 18683 RVA: 0x0010A2B7 File Offset: 0x001092B7
		[SRDescription("MonthCalendarShowTodayCircleDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool ShowTodayCircle
		{
			get
			{
				return this.showTodayCircle;
			}
			set
			{
				if (this.showTodayCircle != value)
				{
					this.showTodayCircle = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x060048FC RID: 18684 RVA: 0x0010A2CF File Offset: 0x001092CF
		// (set) Token: 0x060048FD RID: 18685 RVA: 0x0010A2D7 File Offset: 0x001092D7
		[Localizable(true)]
		[SRDescription("MonthCalendarShowWeekNumbersDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ShowWeekNumbers
		{
			get
			{
				return this.showWeekNumbers;
			}
			set
			{
				if (this.showWeekNumbers != value)
				{
					this.showWeekNumbers = value;
					base.UpdateStyles();
					this.AdjustSize();
				}
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x060048FE RID: 18686 RVA: 0x0010A2F8 File Offset: 0x001092F8
		[SRCategory("CatAppearance")]
		[SRDescription("MonthCalendarSingleMonthSizeDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Size SingleMonthSize
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				if (!base.IsHandleCreated)
				{
					return MonthCalendar.DefaultSingleMonthSize;
				}
				if ((int)base.SendMessage(4105, 0, ref rect) == 0)
				{
					throw new InvalidOperationException(SR.GetString("InvalidSingleMonthSize"));
				}
				return new Size(rect.right, rect.bottom);
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060048FF RID: 18687 RVA: 0x0010A353 File Offset: 0x00109353
		// (set) Token: 0x06004900 RID: 18688 RVA: 0x0010A35B File Offset: 0x0010935B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		public new Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06004901 RID: 18689 RVA: 0x0010A364 File Offset: 0x00109364
		// (set) Token: 0x06004902 RID: 18690 RVA: 0x0010A36C File Offset: 0x0010936C
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

		// Token: 0x14000292 RID: 658
		// (add) Token: 0x06004903 RID: 18691 RVA: 0x0010A375 File Offset: 0x00109375
		// (remove) Token: 0x06004904 RID: 18692 RVA: 0x0010A37E File Offset: 0x0010937E
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

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06004905 RID: 18693 RVA: 0x0010A388 File Offset: 0x00109388
		// (set) Token: 0x06004906 RID: 18694 RVA: 0x0010A3F0 File Offset: 0x001093F0
		[SRDescription("MonthCalendarTodayDateDescr")]
		[SRCategory("CatBehavior")]
		public DateTime TodayDate
		{
			get
			{
				if (this.todayDateSet)
				{
					return this.todayDate;
				}
				if (base.IsHandleCreated)
				{
					NativeMethods.SYSTEMTIME systemtime = new NativeMethods.SYSTEMTIME();
					(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4109, 0, systemtime);
					return DateTimePicker.SysTimeToDateTime(systemtime).Date;
				}
				return this.Now.Date;
			}
			set
			{
				if (!this.todayDateSet || DateTime.Compare(value, this.todayDate) != 0)
				{
					if (DateTime.Compare(value, this.maxDate) > 0)
					{
						throw new ArgumentOutOfRangeException("TodayDate", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"TodayDate",
							MonthCalendar.FormatDate(value),
							MonthCalendar.FormatDate(this.maxDate)
						}));
					}
					if (DateTime.Compare(value, this.minDate) < 0)
					{
						throw new ArgumentOutOfRangeException("TodayDate", SR.GetString("InvalidLowBoundArgument", new object[]
						{
							"TodayDate",
							MonthCalendar.FormatDate(value),
							MonthCalendar.FormatDate(this.minDate)
						}));
					}
					this.todayDate = value.Date;
					this.todayDateSet = true;
					this.UpdateTodayDate();
				}
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06004907 RID: 18695 RVA: 0x0010A4C6 File Offset: 0x001094C6
		[SRDescription("MonthCalendarTodayDateSetDescr")]
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool TodayDateSet
		{
			get
			{
				return this.todayDateSet;
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06004908 RID: 18696 RVA: 0x0010A4CE File Offset: 0x001094CE
		// (set) Token: 0x06004909 RID: 18697 RVA: 0x0010A4D8 File Offset: 0x001094D8
		[SRDescription("MonthCalendarTitleBackColorDescr")]
		[SRCategory("CatAppearance")]
		public Color TitleBackColor
		{
			get
			{
				return this.titleBackColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[]
					{
						"value"
					}));
				}
				this.titleBackColor = value;
				this.SetControlColor(2, value);
			}
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x0600490A RID: 18698 RVA: 0x0010A51D File Offset: 0x0010951D
		// (set) Token: 0x0600490B RID: 18699 RVA: 0x0010A528 File Offset: 0x00109528
		[SRCategory("CatAppearance")]
		[SRDescription("MonthCalendarTitleForeColorDescr")]
		public Color TitleForeColor
		{
			get
			{
				return this.titleForeColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[]
					{
						"value"
					}));
				}
				this.titleForeColor = value;
				this.SetControlColor(3, value);
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x0600490C RID: 18700 RVA: 0x0010A56D File Offset: 0x0010956D
		// (set) Token: 0x0600490D RID: 18701 RVA: 0x0010A578 File Offset: 0x00109578
		[SRCategory("CatAppearance")]
		[SRDescription("MonthCalendarTrailingForeColorDescr")]
		public Color TrailingForeColor
		{
			get
			{
				return this.trailingForeColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[]
					{
						"value"
					}));
				}
				this.trailingForeColor = value;
				this.SetControlColor(5, value);
			}
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x0010A5C0 File Offset: 0x001095C0
		public void AddAnnuallyBoldedDate(DateTime date)
		{
			this.annualArrayOfDates.Add(date);
			this.monthsOfYear[date.Month - 1] |= 1 << date.Day - 1;
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x0010A60D File Offset: 0x0010960D
		public void AddBoldedDate(DateTime date)
		{
			if (!this.arrayOfDates.Contains(date))
			{
				this.arrayOfDates.Add(date);
			}
		}

		// Token: 0x06004910 RID: 18704 RVA: 0x0010A634 File Offset: 0x00109634
		public void AddMonthlyBoldedDate(DateTime date)
		{
			this.monthlyArrayOfDates.Add(date);
			this.datesToBoldMonthly |= 1 << date.Day - 1;
		}

		// Token: 0x14000293 RID: 659
		// (add) Token: 0x06004911 RID: 18705 RVA: 0x0010A663 File Offset: 0x00109663
		// (remove) Token: 0x06004912 RID: 18706 RVA: 0x0010A66C File Offset: 0x0010966C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000294 RID: 660
		// (add) Token: 0x06004913 RID: 18707 RVA: 0x0010A675 File Offset: 0x00109675
		// (remove) Token: 0x06004914 RID: 18708 RVA: 0x0010A68E File Offset: 0x0010968E
		[SRCategory("CatAction")]
		[SRDescription("MonthCalendarOnDateChangedDescr")]
		public event DateRangeEventHandler DateChanged
		{
			add
			{
				this.onDateChanged = (DateRangeEventHandler)Delegate.Combine(this.onDateChanged, value);
			}
			remove
			{
				this.onDateChanged = (DateRangeEventHandler)Delegate.Remove(this.onDateChanged, value);
			}
		}

		// Token: 0x14000295 RID: 661
		// (add) Token: 0x06004915 RID: 18709 RVA: 0x0010A6A7 File Offset: 0x001096A7
		// (remove) Token: 0x06004916 RID: 18710 RVA: 0x0010A6C0 File Offset: 0x001096C0
		[SRDescription("MonthCalendarOnDateSelectedDescr")]
		[SRCategory("CatAction")]
		public event DateRangeEventHandler DateSelected
		{
			add
			{
				this.onDateSelected = (DateRangeEventHandler)Delegate.Combine(this.onDateSelected, value);
			}
			remove
			{
				this.onDateSelected = (DateRangeEventHandler)Delegate.Remove(this.onDateSelected, value);
			}
		}

		// Token: 0x14000296 RID: 662
		// (add) Token: 0x06004917 RID: 18711 RVA: 0x0010A6D9 File Offset: 0x001096D9
		// (remove) Token: 0x06004918 RID: 18712 RVA: 0x0010A6E2 File Offset: 0x001096E2
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

		// Token: 0x14000297 RID: 663
		// (add) Token: 0x06004919 RID: 18713 RVA: 0x0010A6EB File Offset: 0x001096EB
		// (remove) Token: 0x0600491A RID: 18714 RVA: 0x0010A6F4 File Offset: 0x001096F4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000298 RID: 664
		// (add) Token: 0x0600491B RID: 18715 RVA: 0x0010A6FD File Offset: 0x001096FD
		// (remove) Token: 0x0600491C RID: 18716 RVA: 0x0010A706 File Offset: 0x00109706
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

		// Token: 0x14000299 RID: 665
		// (add) Token: 0x0600491D RID: 18717 RVA: 0x0010A70F File Offset: 0x0010970F
		// (remove) Token: 0x0600491E RID: 18718 RVA: 0x0010A718 File Offset: 0x00109718
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

		// Token: 0x1400029A RID: 666
		// (add) Token: 0x0600491F RID: 18719 RVA: 0x0010A721 File Offset: 0x00109721
		// (remove) Token: 0x06004920 RID: 18720 RVA: 0x0010A73A File Offset: 0x0010973A
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

		// Token: 0x06004921 RID: 18721 RVA: 0x0010A754 File Offset: 0x00109754
		private void AdjustSize()
		{
			Size minReqRect = this.GetMinReqRect();
			this.Size = minReqRect;
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x0010A770 File Offset: 0x00109770
		private void BoldDates(DateBoldEventArgs e)
		{
			int size = e.Size;
			e.DaysToBold = new int[size];
			SelectionRange displayRange = this.GetDisplayRange(false);
			int num = displayRange.Start.Month;
			int year = displayRange.Start.Year;
			int count = this.arrayOfDates.Count;
			for (int i = 0; i < count; i++)
			{
				DateTime t = (DateTime)this.arrayOfDates[i];
				if (DateTime.Compare(t, displayRange.Start) >= 0 && DateTime.Compare(t, displayRange.End) <= 0)
				{
					int month = t.Month;
					int year2 = t.Year;
					int num2 = (year2 == year) ? (month - num) : (month + year2 * 12 - year * 12 - num);
					e.DaysToBold[num2] |= 1 << t.Day - 1;
				}
			}
			num--;
			int j = 0;
			while (j < size)
			{
				e.DaysToBold[j] |= (this.monthsOfYear[num % 12] | this.datesToBoldMonthly);
				j++;
				num++;
			}
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x0010A8A8 File Offset: 0x001098A8
		private bool CompareDayAndMonth(DateTime t1, DateTime t2)
		{
			return t1.Day == t2.Day && t1.Month == t2.Month;
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x0010A8CC File Offset: 0x001098CC
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 256
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x0010A920 File Offset: 0x00109920
		protected override void Dispose(bool disposing)
		{
			if (this.mdsBuffer != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.mdsBuffer);
				this.mdsBuffer = IntPtr.Zero;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x0010A951 File Offset: 0x00109951
		private static string FormatDate(DateTime value)
		{
			return value.ToString("d", CultureInfo.CurrentCulture);
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x0010A964 File Offset: 0x00109964
		public SelectionRange GetDisplayRange(bool visible)
		{
			if (visible)
			{
				return this.GetMonthRange(0);
			}
			return this.GetMonthRange(1);
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x0010A978 File Offset: 0x00109978
		private MonthCalendar.HitArea GetHitArea(int hit)
		{
			if (hit <= 196608)
			{
				switch (hit)
				{
				case 65536:
					return MonthCalendar.HitArea.TitleBackground;
				case 65537:
					return MonthCalendar.HitArea.TitleMonth;
				case 65538:
					return MonthCalendar.HitArea.TitleYear;
				default:
					switch (hit)
					{
					case 131072:
						return MonthCalendar.HitArea.CalendarBackground;
					case 131073:
						return MonthCalendar.HitArea.Date;
					case 131074:
						return MonthCalendar.HitArea.DayOfWeek;
					case 131075:
						return MonthCalendar.HitArea.WeekNumbers;
					default:
						if (hit == 196608)
						{
							return MonthCalendar.HitArea.TodayLink;
						}
						break;
					}
					break;
				}
			}
			else if (hit <= 16908289)
			{
				if (hit == 16842755)
				{
					return MonthCalendar.HitArea.NextMonthButton;
				}
				if (hit == 16908289)
				{
					return MonthCalendar.HitArea.NextMonthDate;
				}
			}
			else
			{
				if (hit == 33619971)
				{
					return MonthCalendar.HitArea.PrevMonthButton;
				}
				if (hit == 33685505)
				{
					return MonthCalendar.HitArea.PrevMonthDate;
				}
			}
			return MonthCalendar.HitArea.Nowhere;
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x0010AA16 File Offset: 0x00109A16
		private Size GetMinReqRect()
		{
			return this.GetMinReqRect(0, false, false);
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x0010AA24 File Offset: 0x00109A24
		private Size GetMinReqRect(int newDimensionLength, bool updateRows, bool updateCols)
		{
			Size singleMonthSize = this.SingleMonthSize;
			Size textExtent;
			using (WindowsFont windowsFont = WindowsFont.FromFont(this.Font))
			{
				textExtent = WindowsGraphicsCacheManager.MeasurementGraphics.GetTextExtent(DateTime.Now.ToShortDateString(), windowsFont);
			}
			int num = textExtent.Height + 4;
			int num2 = singleMonthSize.Height;
			if (this.ShowToday)
			{
				num2 -= num;
			}
			if (updateRows)
			{
				int num3 = (newDimensionLength - num + 6) / (num2 + 6);
				this.dimensions.Height = ((num3 < 1) ? 1 : num3);
			}
			if (updateCols)
			{
				int num4 = (newDimensionLength - 2) / singleMonthSize.Width;
				this.dimensions.Width = ((num4 < 1) ? 1 : num4);
			}
			singleMonthSize.Width = (singleMonthSize.Width + 6) * this.dimensions.Width - 6;
			singleMonthSize.Height = (num2 + 6) * this.dimensions.Height - 6 + num;
			if (base.IsHandleCreated)
			{
				int num5 = (int)base.SendMessage(4117, 0, 0);
				if (num5 > singleMonthSize.Width)
				{
					singleMonthSize.Width = num5;
				}
			}
			singleMonthSize.Width += 2;
			singleMonthSize.Height += 2;
			return singleMonthSize;
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x0010AB6C File Offset: 0x00109B6C
		private SelectionRange GetMonthRange(int flag)
		{
			NativeMethods.SYSTEMTIMEARRAY systemtimearray = new NativeMethods.SYSTEMTIMEARRAY();
			SelectionRange selectionRange = new SelectionRange();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4103, flag, systemtimearray);
			NativeMethods.SYSTEMTIME systemtime = new NativeMethods.SYSTEMTIME();
			systemtime.wYear = systemtimearray.wYear1;
			systemtime.wMonth = systemtimearray.wMonth1;
			systemtime.wDayOfWeek = systemtimearray.wDayOfWeek1;
			systemtime.wDay = systemtimearray.wDay1;
			selectionRange.Start = DateTimePicker.SysTimeToDateTime(systemtime);
			systemtime.wYear = systemtimearray.wYear2;
			systemtime.wMonth = systemtimearray.wMonth2;
			systemtime.wDayOfWeek = systemtimearray.wDayOfWeek2;
			systemtime.wDay = systemtimearray.wDay2;
			selectionRange.End = DateTimePicker.SysTimeToDateTime(systemtime);
			return selectionRange;
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x0010AC20 File Offset: 0x00109C20
		private int GetPreferredHeight(int height, bool updateRows)
		{
			return this.GetMinReqRect(height, updateRows, false).Height;
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x0010AC40 File Offset: 0x00109C40
		private int GetPreferredWidth(int width, bool updateCols)
		{
			return this.GetMinReqRect(width, false, updateCols).Width;
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x0010AC60 File Offset: 0x00109C60
		public MonthCalendar.HitTestInfo HitTest(int x, int y)
		{
			NativeMethods.MCHITTESTINFO mchittestinfo = new NativeMethods.MCHITTESTINFO();
			mchittestinfo.pt_x = x;
			mchittestinfo.pt_y = y;
			mchittestinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.MCHITTESTINFO));
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4110, 0, mchittestinfo);
			MonthCalendar.HitArea hitArea = this.GetHitArea(mchittestinfo.uHit);
			if (MonthCalendar.HitTestInfo.HitAreaHasValidDateTime(hitArea))
			{
				NativeMethods.SYSTEMTIME systemtime = new NativeMethods.SYSTEMTIME();
				systemtime.wYear = mchittestinfo.st_wYear;
				systemtime.wMonth = mchittestinfo.st_wMonth;
				systemtime.wDayOfWeek = mchittestinfo.st_wDayOfWeek;
				systemtime.wDay = mchittestinfo.st_wDay;
				systemtime.wHour = mchittestinfo.st_wHour;
				systemtime.wMinute = mchittestinfo.st_wMinute;
				systemtime.wSecond = mchittestinfo.st_wSecond;
				systemtime.wMilliseconds = mchittestinfo.st_wMilliseconds;
				return new MonthCalendar.HitTestInfo(new Point(mchittestinfo.pt_x, mchittestinfo.pt_y), hitArea, DateTimePicker.SysTimeToDateTime(systemtime));
			}
			return new MonthCalendar.HitTestInfo(new Point(mchittestinfo.pt_x, mchittestinfo.pt_y), hitArea);
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x0010AD62 File Offset: 0x00109D62
		public MonthCalendar.HitTestInfo HitTest(Point point)
		{
			return this.HitTest(point.X, point.Y);
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x0010AD78 File Offset: 0x00109D78
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

		// Token: 0x06004931 RID: 18737 RVA: 0x0010ADC4 File Offset: 0x00109DC4
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.SetSelRange(this.selectionStart, this.selectionEnd);
			if (this.maxSelectionCount != 7)
			{
				base.SendMessage(4100, this.maxSelectionCount, 0);
			}
			this.AdjustSize();
			if (this.todayDateSet)
			{
				NativeMethods.SYSTEMTIME lParam = DateTimePicker.DateTimeToSysTime(this.todayDate);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4108, 0, lParam);
			}
			this.SetControlColor(1, this.ForeColor);
			this.SetControlColor(4, this.BackColor);
			this.SetControlColor(2, this.titleBackColor);
			this.SetControlColor(3, this.titleForeColor);
			this.SetControlColor(5, this.trailingForeColor);
			int lparam;
			if (this.firstDayOfWeek == Day.Default)
			{
				lparam = 4108;
			}
			else
			{
				lparam = (int)this.firstDayOfWeek;
			}
			base.SendMessage(4111, 0, lparam);
			this.SetRange();
			if (this.scrollChange != 0)
			{
				base.SendMessage(4116, this.scrollChange, 0);
			}
			SystemEvents.UserPreferenceChanged += this.MarshaledUserPreferenceChanged;
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x0010AED2 File Offset: 0x00109ED2
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.MarshaledUserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x0010AEEC File Offset: 0x00109EEC
		protected virtual void OnDateChanged(DateRangeEventArgs drevent)
		{
			if (this.onDateChanged != null)
			{
				this.onDateChanged(this, drevent);
			}
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x0010AF03 File Offset: 0x00109F03
		protected virtual void OnDateSelected(DateRangeEventArgs drevent)
		{
			if (this.onDateSelected != null)
			{
				this.onDateSelected(this, drevent);
			}
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x0010AF1A File Offset: 0x00109F1A
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AdjustSize();
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x0010AF29 File Offset: 0x00109F29
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.SetControlColor(1, this.ForeColor);
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x0010AF3F File Offset: 0x00109F3F
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.SetControlColor(4, this.BackColor);
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x0010AF55 File Offset: 0x00109F55
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

		// Token: 0x06004939 RID: 18745 RVA: 0x0010AF84 File Offset: 0x00109F84
		public void RemoveAllAnnuallyBoldedDates()
		{
			this.annualArrayOfDates.Clear();
			for (int i = 0; i < 12; i++)
			{
				this.monthsOfYear[i] = 0;
			}
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x0010AFB2 File Offset: 0x00109FB2
		public void RemoveAllBoldedDates()
		{
			this.arrayOfDates.Clear();
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x0010AFBF File Offset: 0x00109FBF
		public void RemoveAllMonthlyBoldedDates()
		{
			this.monthlyArrayOfDates.Clear();
			this.datesToBoldMonthly = 0;
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x0010AFD4 File Offset: 0x00109FD4
		public void RemoveAnnuallyBoldedDate(DateTime date)
		{
			int num = this.annualArrayOfDates.Count;
			int i;
			for (i = 0; i < num; i++)
			{
				if (this.CompareDayAndMonth((DateTime)this.annualArrayOfDates[i], date))
				{
					this.annualArrayOfDates.RemoveAt(i);
					break;
				}
			}
			num--;
			for (int j = i; j < num; j++)
			{
				if (this.CompareDayAndMonth((DateTime)this.annualArrayOfDates[j], date))
				{
					return;
				}
			}
			this.monthsOfYear[date.Month - 1] &= ~(1 << date.Day - 1);
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x0010B07C File Offset: 0x0010A07C
		public void RemoveBoldedDate(DateTime date)
		{
			int count = this.arrayOfDates.Count;
			for (int i = 0; i < count; i++)
			{
				if (DateTime.Compare(((DateTime)this.arrayOfDates[i]).Date, date.Date) == 0)
				{
					this.arrayOfDates.RemoveAt(i);
					base.Invalidate();
					return;
				}
			}
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x0010B0DC File Offset: 0x0010A0DC
		public void RemoveMonthlyBoldedDate(DateTime date)
		{
			int num = this.monthlyArrayOfDates.Count;
			int i;
			for (i = 0; i < num; i++)
			{
				if (this.CompareDayAndMonth((DateTime)this.monthlyArrayOfDates[i], date))
				{
					this.monthlyArrayOfDates.RemoveAt(i);
					break;
				}
			}
			num--;
			for (int j = i; j < num; j++)
			{
				if (this.CompareDayAndMonth((DateTime)this.monthlyArrayOfDates[j], date))
				{
					return;
				}
			}
			this.datesToBoldMonthly &= ~(1 << date.Day - 1);
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x0010B170 File Offset: 0x0010A170
		private void ResetAnnuallyBoldedDates()
		{
			this.annualArrayOfDates.Clear();
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x0010B17D File Offset: 0x0010A17D
		private void ResetBoldedDates()
		{
			this.arrayOfDates.Clear();
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x0010B18A File Offset: 0x0010A18A
		private void ResetCalendarDimensions()
		{
			this.CalendarDimensions = new Size(1, 1);
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x0010B199 File Offset: 0x0010A199
		private void ResetMaxDate()
		{
			this.MaxDate = DateTime.MaxValue;
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x0010B1A6 File Offset: 0x0010A1A6
		private void ResetMinDate()
		{
			this.MinDate = DateTime.MinValue;
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x0010B1B3 File Offset: 0x0010A1B3
		private void ResetMonthlyBoldedDates()
		{
			this.monthlyArrayOfDates.Clear();
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x0010B1C0 File Offset: 0x0010A1C0
		private void ResetSelectionRange()
		{
			this.SetSelectionRange(this.Now, this.Now);
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x0010B1D4 File Offset: 0x0010A1D4
		private void ResetTrailingForeColor()
		{
			this.TrailingForeColor = MonthCalendar.DEFAULT_TRAILING_FORE_COLOR;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x0010B1E1 File Offset: 0x0010A1E1
		private void ResetTitleForeColor()
		{
			this.TitleForeColor = MonthCalendar.DEFAULT_TITLE_FORE_COLOR;
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x0010B1EE File Offset: 0x0010A1EE
		private void ResetTitleBackColor()
		{
			this.TitleBackColor = MonthCalendar.DEFAULT_TITLE_BACK_COLOR;
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x0010B1FB File Offset: 0x0010A1FB
		private void ResetTodayDate()
		{
			this.todayDateSet = false;
			this.UpdateTodayDate();
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x0010B20C File Offset: 0x0010A20C
		private IntPtr RequestBuffer(int reqSize)
		{
			int num = 4;
			if (reqSize * num > this.mdsBufferSize)
			{
				if (this.mdsBuffer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.mdsBuffer);
					this.mdsBuffer = IntPtr.Zero;
				}
				float num2 = (float)(reqSize - 1) / 12f;
				int num3 = (int)(num2 + 1f) * 12;
				this.mdsBufferSize = num3 * num;
				this.mdsBuffer = Marshal.AllocHGlobal(this.mdsBufferSize);
				return this.mdsBuffer;
			}
			return this.mdsBuffer;
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x0010B28C File Offset: 0x0010A28C
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			Rectangle bounds = base.Bounds;
			Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
			if (width != bounds.Width)
			{
				if (width > maxWindowTrackSize.Width)
				{
					width = maxWindowTrackSize.Width;
				}
				width = this.GetPreferredWidth(width, true);
			}
			if (height != bounds.Height)
			{
				if (height > maxWindowTrackSize.Height)
				{
					height = maxWindowTrackSize.Height;
				}
				height = this.GetPreferredHeight(height, true);
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x0010B304 File Offset: 0x0010A304
		private void SetControlColor(int colorIndex, Color value)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4106, colorIndex, ColorTranslator.ToWin32(value));
			}
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x0010B321 File Offset: 0x0010A321
		private void SetRange()
		{
			this.SetRange(DateTimePicker.EffectiveMinDate(this.minDate), DateTimePicker.EffectiveMaxDate(this.maxDate));
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x0010B340 File Offset: 0x0010A340
		private void SetRange(DateTime minDate, DateTime maxDate)
		{
			if (this.selectionStart < minDate)
			{
				this.selectionStart = minDate;
			}
			if (this.selectionStart > maxDate)
			{
				this.selectionStart = maxDate;
			}
			if (this.selectionEnd < minDate)
			{
				this.selectionEnd = minDate;
			}
			if (this.selectionEnd > maxDate)
			{
				this.selectionEnd = maxDate;
			}
			this.SetSelRange(this.selectionStart, this.selectionEnd);
			if (base.IsHandleCreated)
			{
				int num = 0;
				NativeMethods.SYSTEMTIMEARRAY systemtimearray = new NativeMethods.SYSTEMTIMEARRAY();
				num |= 3;
				NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(minDate);
				systemtimearray.wYear1 = systemtime.wYear;
				systemtimearray.wMonth1 = systemtime.wMonth;
				systemtimearray.wDayOfWeek1 = systemtime.wDayOfWeek;
				systemtimearray.wDay1 = systemtime.wDay;
				systemtime = DateTimePicker.DateTimeToSysTime(maxDate);
				systemtimearray.wYear2 = systemtime.wYear;
				systemtimearray.wMonth2 = systemtime.wMonth;
				systemtimearray.wDayOfWeek2 = systemtime.wDayOfWeek;
				systemtimearray.wDay2 = systemtime.wDay;
				if ((int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, num, systemtimearray) == 0)
				{
					throw new InvalidOperationException(SR.GetString("MonthCalendarRange", new object[]
					{
						minDate.ToShortDateString(),
						maxDate.ToShortDateString()
					}));
				}
			}
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x0010B484 File Offset: 0x0010A484
		public void SetCalendarDimensions(int x, int y)
		{
			if (x < 1)
			{
				throw new ArgumentOutOfRangeException("x", SR.GetString("MonthCalendarInvalidDimensions", new object[]
				{
					x.ToString("D", CultureInfo.CurrentCulture),
					y.ToString("D", CultureInfo.CurrentCulture)
				}));
			}
			if (y < 1)
			{
				throw new ArgumentOutOfRangeException("y", SR.GetString("MonthCalendarInvalidDimensions", new object[]
				{
					x.ToString("D", CultureInfo.CurrentCulture),
					y.ToString("D", CultureInfo.CurrentCulture)
				}));
			}
			while (x * y > 12)
			{
				if (x > y)
				{
					x--;
				}
				else
				{
					y--;
				}
			}
			if (this.dimensions.Width != x || this.dimensions.Height != y)
			{
				this.dimensions.Width = x;
				this.dimensions.Height = y;
				this.AdjustSize();
			}
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x0010B574 File Offset: 0x0010A574
		public void SetDate(DateTime date)
		{
			if (date.Ticks < this.minDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"date",
					MonthCalendar.FormatDate(date),
					"MinDate"
				}));
			}
			if (date.Ticks > this.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"date",
					MonthCalendar.FormatDate(date),
					"MaxDate"
				}));
			}
			this.SetSelectionRange(date, date);
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x0010B620 File Offset: 0x0010A620
		public void SetSelectionRange(DateTime date1, DateTime date2)
		{
			if (date1.Ticks < this.minDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date1", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"SelectionStart",
					MonthCalendar.FormatDate(date1),
					"MinDate"
				}));
			}
			if (date1.Ticks > this.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date1", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"SelectionEnd",
					MonthCalendar.FormatDate(date1),
					"MaxDate"
				}));
			}
			if (date2.Ticks < this.minDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date2", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"SelectionStart",
					MonthCalendar.FormatDate(date2),
					"MinDate"
				}));
			}
			if (date2.Ticks > this.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("date2", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"SelectionEnd",
					MonthCalendar.FormatDate(date2),
					"MaxDate"
				}));
			}
			if (date1 > date2)
			{
				date2 = date1;
			}
			if ((date2 - date1).Days >= this.maxSelectionCount)
			{
				if (date1.Ticks == this.selectionStart.Ticks)
				{
					date1 = date2.AddDays((double)(1 - this.maxSelectionCount));
				}
				else
				{
					date2 = date1.AddDays((double)(this.maxSelectionCount - 1));
				}
			}
			this.SetSelRange(date1, date2);
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x0010B7BC File Offset: 0x0010A7BC
		private void SetSelRange(DateTime lower, DateTime upper)
		{
			bool flag = false;
			if (this.selectionStart != lower || this.selectionEnd != upper)
			{
				flag = true;
				this.selectionStart = lower;
				this.selectionEnd = upper;
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.SYSTEMTIMEARRAY systemtimearray = new NativeMethods.SYSTEMTIMEARRAY();
				NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(lower);
				systemtimearray.wYear1 = systemtime.wYear;
				systemtimearray.wMonth1 = systemtime.wMonth;
				systemtimearray.wDayOfWeek1 = systemtime.wDayOfWeek;
				systemtimearray.wDay1 = systemtime.wDay;
				systemtime = DateTimePicker.DateTimeToSysTime(upper);
				systemtimearray.wYear2 = systemtime.wYear;
				systemtimearray.wMonth2 = systemtime.wMonth;
				systemtimearray.wDayOfWeek2 = systemtime.wDayOfWeek;
				systemtimearray.wDay2 = systemtime.wDay;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4102, 0, systemtimearray);
			}
			if (flag)
			{
				this.OnDateChanged(new DateRangeEventArgs(lower, upper));
			}
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x0010B89F File Offset: 0x0010A89F
		private bool ShouldSerializeAnnuallyBoldedDates()
		{
			return this.annualArrayOfDates.Count > 0;
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x0010B8AF File Offset: 0x0010A8AF
		private bool ShouldSerializeBoldedDates()
		{
			return this.arrayOfDates.Count > 0;
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x0010B8BF File Offset: 0x0010A8BF
		private bool ShouldSerializeCalendarDimensions()
		{
			return !this.dimensions.Equals(new Size(1, 1));
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x0010B8E4 File Offset: 0x0010A8E4
		private bool ShouldSerializeTrailingForeColor()
		{
			return !this.TrailingForeColor.Equals(MonthCalendar.DEFAULT_TRAILING_FORE_COLOR);
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x0010B914 File Offset: 0x0010A914
		private bool ShouldSerializeTitleForeColor()
		{
			return !this.TitleForeColor.Equals(MonthCalendar.DEFAULT_TITLE_FORE_COLOR);
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x0010B944 File Offset: 0x0010A944
		private bool ShouldSerializeTitleBackColor()
		{
			return !this.TitleBackColor.Equals(MonthCalendar.DEFAULT_TITLE_BACK_COLOR);
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x0010B972 File Offset: 0x0010A972
		private bool ShouldSerializeMonthlyBoldedDates()
		{
			return this.monthlyArrayOfDates.Count > 0;
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x0010B982 File Offset: 0x0010A982
		private bool ShouldSerializeMaxDate()
		{
			return this.maxDate != DateTimePicker.MaximumDateTime && this.maxDate != DateTime.MaxValue;
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x0010B9A8 File Offset: 0x0010A9A8
		private bool ShouldSerializeMinDate()
		{
			return this.minDate != DateTimePicker.MinimumDateTime && this.minDate != DateTime.MinValue;
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x0010B9CE File Offset: 0x0010A9CE
		private bool ShouldSerializeSelectionRange()
		{
			return !DateTime.Equals(this.selectionEnd, this.selectionStart);
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x0010B9E4 File Offset: 0x0010A9E4
		private bool ShouldSerializeTodayDate()
		{
			return this.todayDateSet;
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x0010B9EC File Offset: 0x0010A9EC
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", " + this.SelectionRange.ToString();
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x0010BA16 File Offset: 0x0010AA16
		public void UpdateBoldedDates()
		{
			base.RecreateHandle();
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x0010BA20 File Offset: 0x0010AA20
		private void UpdateTodayDate()
		{
			if (base.IsHandleCreated)
			{
				NativeMethods.SYSTEMTIME lParam = null;
				if (this.todayDateSet)
				{
					lParam = DateTimePicker.DateTimeToSysTime(this.todayDate);
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4108, 0, lParam);
			}
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x0010BA64 File Offset: 0x0010AA64
		private void MarshaledUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			try
			{
				base.BeginInvoke(new UserPreferenceChangedEventHandler(this.UserPreferenceChanged), new object[]
				{
					sender,
					pref
				});
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x0010BAAC File Offset: 0x0010AAAC
		private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			if (pref.Category == UserPreferenceCategory.Locale)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x0010BAC0 File Offset: 0x0010AAC0
		private void WmDateChanged(ref Message m)
		{
			NativeMethods.NMSELCHANGE nmselchange = (NativeMethods.NMSELCHANGE)m.GetLParam(typeof(NativeMethods.NMSELCHANGE));
			DateTime start = this.selectionStart = DateTimePicker.SysTimeToDateTime(nmselchange.stSelStart);
			DateTime end = this.selectionEnd = DateTimePicker.SysTimeToDateTime(nmselchange.stSelEnd);
			if (start.Ticks < this.minDate.Ticks || end.Ticks < this.minDate.Ticks)
			{
				this.SetSelRange(this.minDate, this.minDate);
			}
			else if (start.Ticks > this.maxDate.Ticks || end.Ticks > this.maxDate.Ticks)
			{
				this.SetSelRange(this.maxDate, this.maxDate);
			}
			this.OnDateChanged(new DateRangeEventArgs(start, end));
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x0010BB94 File Offset: 0x0010AB94
		private void WmDateBold(ref Message m)
		{
			NativeMethods.NMDAYSTATE nmdaystate = (NativeMethods.NMDAYSTATE)m.GetLParam(typeof(NativeMethods.NMDAYSTATE));
			DateTime start = DateTimePicker.SysTimeToDateTime(nmdaystate.stStart);
			DateBoldEventArgs dateBoldEventArgs = new DateBoldEventArgs(start, nmdaystate.cDayState);
			this.BoldDates(dateBoldEventArgs);
			this.mdsBuffer = this.RequestBuffer(dateBoldEventArgs.Size);
			Marshal.Copy(dateBoldEventArgs.DaysToBold, 0, this.mdsBuffer, dateBoldEventArgs.Size);
			nmdaystate.prgDayState = this.mdsBuffer;
			Marshal.StructureToPtr(nmdaystate, m.LParam, false);
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x0010BC1C File Offset: 0x0010AC1C
		private void WmDateSelected(ref Message m)
		{
			NativeMethods.NMSELCHANGE nmselchange = (NativeMethods.NMSELCHANGE)m.GetLParam(typeof(NativeMethods.NMSELCHANGE));
			DateTime start = this.selectionStart = DateTimePicker.SysTimeToDateTime(nmselchange.stSelStart);
			DateTime end = this.selectionEnd = DateTimePicker.SysTimeToDateTime(nmselchange.stSelEnd);
			if (start.Ticks < this.minDate.Ticks || end.Ticks < this.minDate.Ticks)
			{
				this.SetSelRange(this.minDate, this.minDate);
			}
			else if (start.Ticks > this.maxDate.Ticks || end.Ticks > this.maxDate.Ticks)
			{
				this.SetSelRange(this.maxDate, this.maxDate);
			}
			this.OnDateSelected(new DateRangeEventArgs(start, end));
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x0010BCEE File Offset: 0x0010ACEE
		private void WmGetDlgCode(ref Message m)
		{
			m.Result = (IntPtr)1;
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x0010BCFC File Offset: 0x0010ACFC
		private void WmReflectCommand(ref Message m)
		{
			if (m.HWnd == base.Handle)
			{
				switch (((NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR))).code)
				{
				case -749:
					this.WmDateChanged(ref m);
					return;
				case -748:
					break;
				case -747:
					this.WmDateBold(ref m);
					break;
				case -746:
					this.WmDateSelected(ref m);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x0010BD70 File Offset: 0x0010AD70
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 135)
			{
				if (msg != 2)
				{
					if (msg == 135)
					{
						this.WmGetDlgCode(ref m);
						return;
					}
				}
				else
				{
					if (MonthCalendar.restrictUnmanagedCode == true && this.nativeWndProcCount > 0)
					{
						throw new InvalidOperationException();
					}
					base.WndProc(ref m);
					return;
				}
			}
			else if (msg != 513)
			{
				if (msg == 8270)
				{
					this.WmReflectCommand(ref m);
					base.WndProc(ref m);
					return;
				}
			}
			else
			{
				this.FocusInternal();
				if (!base.ValidationCancelled)
				{
					base.WndProc(ref m);
					return;
				}
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x0010BE14 File Offset: 0x0010AE14
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void DefWndProc(ref Message m)
		{
			if (MonthCalendar.restrictUnmanagedCode == true)
			{
				this.nativeWndProcCount++;
				try
				{
					base.DefWndProc(ref m);
				}
				finally
				{
					this.nativeWndProcCount--;
				}
				return;
			}
			base.DefWndProc(ref m);
		}

		// Token: 0x04002273 RID: 8819
		private const long DAYS_TO_1601 = 548229L;

		// Token: 0x04002274 RID: 8820
		private const long DAYS_TO_10000 = 3615900L;

		// Token: 0x04002275 RID: 8821
		private const int MINIMUM_ALLOC_SIZE = 12;

		// Token: 0x04002276 RID: 8822
		private const int MONTHS_IN_YEAR = 12;

		// Token: 0x04002277 RID: 8823
		private const int INSERT_WIDTH_SIZE = 6;

		// Token: 0x04002278 RID: 8824
		private const int INSERT_HEIGHT_SIZE = 6;

		// Token: 0x04002279 RID: 8825
		private const Day DEFAULT_FIRST_DAY_OF_WEEK = Day.Default;

		// Token: 0x0400227A RID: 8826
		private const int DEFAULT_MAX_SELECTION_COUNT = 7;

		// Token: 0x0400227B RID: 8827
		private const int DEFAULT_SCROLL_CHANGE = 0;

		// Token: 0x0400227C RID: 8828
		private const int UNIQUE_DATE = 0;

		// Token: 0x0400227D RID: 8829
		private const int ANNUAL_DATE = 1;

		// Token: 0x0400227E RID: 8830
		private const int MONTHLY_DATE = 2;

		// Token: 0x0400227F RID: 8831
		private const int MaxScrollChange = 20000;

		// Token: 0x04002280 RID: 8832
		private const int ExtraPadding = 2;

		// Token: 0x04002281 RID: 8833
		private static readonly Color DEFAULT_TITLE_BACK_COLOR = SystemColors.ActiveCaption;

		// Token: 0x04002282 RID: 8834
		private static readonly Color DEFAULT_TITLE_FORE_COLOR = SystemColors.ActiveCaptionText;

		// Token: 0x04002283 RID: 8835
		private static readonly Color DEFAULT_TRAILING_FORE_COLOR = SystemColors.GrayText;

		// Token: 0x04002284 RID: 8836
		private static readonly Size DefaultSingleMonthSize = new Size(176, 153);

		// Token: 0x04002285 RID: 8837
		private IntPtr mdsBuffer = IntPtr.Zero;

		// Token: 0x04002286 RID: 8838
		private int mdsBufferSize;

		// Token: 0x04002287 RID: 8839
		private Color titleBackColor = MonthCalendar.DEFAULT_TITLE_BACK_COLOR;

		// Token: 0x04002288 RID: 8840
		private Color titleForeColor = MonthCalendar.DEFAULT_TITLE_FORE_COLOR;

		// Token: 0x04002289 RID: 8841
		private Color trailingForeColor = MonthCalendar.DEFAULT_TRAILING_FORE_COLOR;

		// Token: 0x0400228A RID: 8842
		private bool showToday = true;

		// Token: 0x0400228B RID: 8843
		private bool showTodayCircle = true;

		// Token: 0x0400228C RID: 8844
		private bool showWeekNumbers;

		// Token: 0x0400228D RID: 8845
		private bool rightToLeftLayout;

		// Token: 0x0400228E RID: 8846
		private Size dimensions = new Size(1, 1);

		// Token: 0x0400228F RID: 8847
		private int maxSelectionCount = 7;

		// Token: 0x04002290 RID: 8848
		private DateTime maxDate = DateTime.MaxValue;

		// Token: 0x04002291 RID: 8849
		private DateTime minDate = DateTime.MinValue;

		// Token: 0x04002292 RID: 8850
		private int scrollChange;

		// Token: 0x04002293 RID: 8851
		private bool todayDateSet;

		// Token: 0x04002294 RID: 8852
		private DateTime todayDate = DateTime.Now.Date;

		// Token: 0x04002295 RID: 8853
		private DateTime selectionStart;

		// Token: 0x04002296 RID: 8854
		private DateTime selectionEnd;

		// Token: 0x04002297 RID: 8855
		private Day firstDayOfWeek = Day.Default;

		// Token: 0x04002298 RID: 8856
		private int[] monthsOfYear = new int[12];

		// Token: 0x04002299 RID: 8857
		private int datesToBoldMonthly;

		// Token: 0x0400229A RID: 8858
		private ArrayList arrayOfDates = new ArrayList();

		// Token: 0x0400229B RID: 8859
		private ArrayList annualArrayOfDates = new ArrayList();

		// Token: 0x0400229C RID: 8860
		private ArrayList monthlyArrayOfDates = new ArrayList();

		// Token: 0x0400229D RID: 8861
		private DateRangeEventHandler onDateChanged;

		// Token: 0x0400229E RID: 8862
		private DateRangeEventHandler onDateSelected;

		// Token: 0x0400229F RID: 8863
		private EventHandler onRightToLeftLayoutChanged;

		// Token: 0x040022A0 RID: 8864
		private int nativeWndProcCount;

		// Token: 0x040022A1 RID: 8865
		private static bool? restrictUnmanagedCode;

		// Token: 0x020004C7 RID: 1223
		public sealed class HitTestInfo
		{
			// Token: 0x0600496B RID: 18795 RVA: 0x0010BEB0 File Offset: 0x0010AEB0
			internal HitTestInfo(Point pt, MonthCalendar.HitArea area, DateTime time)
			{
				this.point = pt;
				this.hitArea = area;
				this.time = time;
			}

			// Token: 0x0600496C RID: 18796 RVA: 0x0010BECD File Offset: 0x0010AECD
			internal HitTestInfo(Point pt, MonthCalendar.HitArea area)
			{
				this.point = pt;
				this.hitArea = area;
			}

			// Token: 0x17000EA9 RID: 3753
			// (get) Token: 0x0600496D RID: 18797 RVA: 0x0010BEE3 File Offset: 0x0010AEE3
			public Point Point
			{
				get
				{
					return this.point;
				}
			}

			// Token: 0x17000EAA RID: 3754
			// (get) Token: 0x0600496E RID: 18798 RVA: 0x0010BEEB File Offset: 0x0010AEEB
			public MonthCalendar.HitArea HitArea
			{
				get
				{
					return this.hitArea;
				}
			}

			// Token: 0x17000EAB RID: 3755
			// (get) Token: 0x0600496F RID: 18799 RVA: 0x0010BEF3 File Offset: 0x0010AEF3
			public DateTime Time
			{
				get
				{
					return this.time;
				}
			}

			// Token: 0x06004970 RID: 18800 RVA: 0x0010BEFC File Offset: 0x0010AEFC
			internal static bool HitAreaHasValidDateTime(MonthCalendar.HitArea hitArea)
			{
				return hitArea == MonthCalendar.HitArea.Date || hitArea == MonthCalendar.HitArea.WeekNumbers;
			}

			// Token: 0x040022A2 RID: 8866
			private readonly Point point;

			// Token: 0x040022A3 RID: 8867
			private readonly MonthCalendar.HitArea hitArea;

			// Token: 0x040022A4 RID: 8868
			private readonly DateTime time;
		}

		// Token: 0x020004C8 RID: 1224
		public enum HitArea
		{
			// Token: 0x040022A6 RID: 8870
			Nowhere,
			// Token: 0x040022A7 RID: 8871
			TitleBackground,
			// Token: 0x040022A8 RID: 8872
			TitleMonth,
			// Token: 0x040022A9 RID: 8873
			TitleYear,
			// Token: 0x040022AA RID: 8874
			NextMonthButton,
			// Token: 0x040022AB RID: 8875
			PrevMonthButton,
			// Token: 0x040022AC RID: 8876
			CalendarBackground,
			// Token: 0x040022AD RID: 8877
			Date,
			// Token: 0x040022AE RID: 8878
			NextMonthDate,
			// Token: 0x040022AF RID: 8879
			PrevMonthDate,
			// Token: 0x040022B0 RID: 8880
			DayOfWeek,
			// Token: 0x040022B1 RID: 8881
			WeekNumbers,
			// Token: 0x040022B2 RID: 8882
			TodayLink
		}
	}
}
