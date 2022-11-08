using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization
{
	// Token: 0x020003B6 RID: 950
	[ComVisible(true)]
	[Serializable]
	public class GregorianCalendar : Calendar
	{
		// Token: 0x060025C7 RID: 9671 RVA: 0x00069550 File Offset: 0x00068550
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_type < GregorianCalendarTypes.Localized || this.m_type > GregorianCalendarTypes.TransliteratedFrench)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_MemberOutOfRange"), new object[]
				{
					"type",
					"GregorianCalendar"
				}));
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060025C8 RID: 9672 RVA: 0x000695A2 File Offset: 0x000685A2
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060025C9 RID: 9673 RVA: 0x000695A9 File Offset: 0x000685A9
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060025CA RID: 9674 RVA: 0x000695B0 File Offset: 0x000685B0
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000695B3 File Offset: 0x000685B3
		internal static Calendar GetDefaultInstance()
		{
			if (GregorianCalendar.m_defaultInstance == null)
			{
				GregorianCalendar.m_defaultInstance = new GregorianCalendar();
			}
			return GregorianCalendar.m_defaultInstance;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000695CB File Offset: 0x000685CB
		public GregorianCalendar() : this(GregorianCalendarTypes.Localized)
		{
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000695D4 File Offset: 0x000685D4
		public GregorianCalendar(GregorianCalendarTypes type)
		{
			if (type < GregorianCalendarTypes.Localized || type > GregorianCalendarTypes.TransliteratedFrench)
			{
				throw new ArgumentOutOfRangeException("type", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					GregorianCalendarTypes.Localized,
					GregorianCalendarTypes.TransliteratedFrench
				}));
			}
			this.m_type = type;
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060025CE RID: 9678 RVA: 0x00069631 File Offset: 0x00068631
		// (set) Token: 0x060025CF RID: 9679 RVA: 0x0006963C File Offset: 0x0006863C
		public virtual GregorianCalendarTypes CalendarType
		{
			get
			{
				return this.m_type;
			}
			set
			{
				base.VerifyWritable();
				switch (value)
				{
				case GregorianCalendarTypes.Localized:
				case GregorianCalendarTypes.USEnglish:
					break;
				default:
					switch (value)
					{
					case GregorianCalendarTypes.MiddleEastFrench:
					case GregorianCalendarTypes.Arabic:
					case GregorianCalendarTypes.TransliteratedEnglish:
					case GregorianCalendarTypes.TransliteratedFrench:
						break;
					default:
						throw new ArgumentOutOfRangeException("m_type", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
					}
					break;
				}
				this.m_type = value;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060025D0 RID: 9680 RVA: 0x00069698 File Offset: 0x00068698
		internal override int ID
		{
			get
			{
				return (int)this.m_type;
			}
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000696A0 File Offset: 0x000686A0
		internal virtual int GetDatePart(long ticks, int part)
		{
			int i = (int)(ticks / 864000000000L);
			int num = i / 146097;
			i -= num * 146097;
			int num2 = i / 36524;
			if (num2 == 4)
			{
				num2 = 3;
			}
			i -= num2 * 36524;
			int num3 = i / 1461;
			i -= num3 * 1461;
			int num4 = i / 365;
			if (num4 == 4)
			{
				num4 = 3;
			}
			if (part == 0)
			{
				return num * 400 + num2 * 100 + num3 * 4 + num4 + 1;
			}
			i -= num4 * 365;
			if (part == 1)
			{
				return i + 1;
			}
			int[] array = (num4 == 3 && (num3 != 24 || num2 == 3)) ? GregorianCalendar.DaysToMonth366 : GregorianCalendar.DaysToMonth365;
			int num5 = i >> 6;
			while (i >= array[num5])
			{
				num5++;
			}
			if (part == 2)
			{
				return num5;
			}
			return i - array[num5 - 1] + 1;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00069784 File Offset: 0x00068784
		internal static long GetAbsoluteDate(int year, int month, int day)
		{
			if (year >= 1 && year <= 9999 && month >= 1 && month <= 12)
			{
				int[] array = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendar.DaysToMonth366 : GregorianCalendar.DaysToMonth365;
				if (day >= 1 && day <= array[month] - array[month - 1])
				{
					int num = year - 1;
					int num2 = num * 365 + num / 4 - num / 100 + num / 400 + array[month - 1] + day - 1;
					return (long)num2;
				}
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00069811 File Offset: 0x00068811
		internal virtual long DateToTicks(int year, int month, int day)
		{
			return GregorianCalendar.GetAbsoluteDate(year, month, day) * 864000000000L;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00069828 File Offset: 0x00068828
		public override DateTime AddMonths(DateTime time, int months)
		{
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					-120000,
					120000
				}));
			}
			int num = this.GetDatePart(time.Ticks, 0);
			int num2 = this.GetDatePart(time.Ticks, 2);
			int num3 = this.GetDatePart(time.Ticks, 3);
			int num4 = num2 - 1 + months;
			if (num4 >= 0)
			{
				num2 = num4 % 12 + 1;
				num += num4 / 12;
			}
			else
			{
				num2 = 12 + (num4 + 1) % 12;
				num += (num4 - 11) / 12;
			}
			int[] array = (num % 4 == 0 && (num % 100 != 0 || num % 400 == 0)) ? GregorianCalendar.DaysToMonth366 : GregorianCalendar.DaysToMonth365;
			int num5 = array[num2] - array[num2 - 1];
			if (num3 > num5)
			{
				num3 = num5;
			}
			long ticks = this.DateToTicks(num, num2, num3) + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00069953 File Offset: 0x00068953
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00069960 File Offset: 0x00068960
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00069970 File Offset: 0x00068970
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x0006998C File Offset: 0x0006898C
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			if (firstDayOfWeek < DayOfWeek.Sunday || firstDayOfWeek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException("firstDayOfWeek", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					DayOfWeek.Sunday,
					DayOfWeek.Saturday
				}));
			}
			switch (rule)
			{
			case CalendarWeekRule.FirstDay:
				return base.GetFirstDayWeekOfYear(time, (int)firstDayOfWeek);
			case CalendarWeekRule.FirstFullWeek:
				return GregorianCalendar.InternalGetWeekOfYearFullDays(this, time, (int)firstDayOfWeek, 7, 365);
			case CalendarWeekRule.FirstFourDayWeek:
				return GregorianCalendar.InternalGetWeekOfYearFullDays(this, time, (int)firstDayOfWeek, 4, 365);
			default:
				throw new ArgumentOutOfRangeException("rule", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					CalendarWeekRule.FirstDay,
					CalendarWeekRule.FirstFourDayWeek
				}));
			}
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x00069A50 File Offset: 0x00068A50
		internal static int InternalGetWeekOfYearFullDays(Calendar cal, DateTime time, int firstDayOfWeek, int fullDays, int daysOfMinYearMinusOne)
		{
			int num = cal.GetDayOfYear(time) - 1;
			int num2 = cal.GetDayOfWeek(time) - (DayOfWeek)(num % 7);
			int num3 = (firstDayOfWeek - num2 + 14) % 7;
			if (num3 != 0 && num3 >= fullDays)
			{
				num3 -= 7;
			}
			int num4 = num - num3;
			if (num4 >= 0)
			{
				return num4 / 7 + 1;
			}
			int year = cal.GetYear(time);
			if (year <= cal.GetYear(cal.MinSupportedDateTime))
			{
				num = daysOfMinYearMinusOne;
			}
			else
			{
				num = cal.GetDaysInYear(year - 1);
			}
			num2 -= num % 7;
			num3 = (firstDayOfWeek - num2 + 14) % 7;
			if (num3 != 0 && num3 >= fullDays)
			{
				num3 -= 7;
			}
			num4 = num - num3;
			return num4 / 7 + 1;
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00069AE1 File Offset: 0x00068AE1
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00069AF4 File Offset: 0x00068AF4
		public override int GetDaysInMonth(int year, int month, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9999
				}));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
			int[] array = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendar.DaysToMonth366 : GregorianCalendar.DaysToMonth365;
			return array[month] - array[month - 1];
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00069BB4 File Offset: 0x00068BB4
		public override int GetDaysInYear(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9999
				}));
			}
			if (year % 4 != 0 || (year % 100 == 0 && year % 400 != 0))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x00069C45 File Offset: 0x00068C45
		public override int GetEra(DateTime time)
		{
			return 1;
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060025DE RID: 9694 RVA: 0x00069C48 File Offset: 0x00068C48
		public override int[] Eras
		{
			get
			{
				return new int[]
				{
					1
				};
			}
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x00069C61 File Offset: 0x00068C61
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00069C74 File Offset: 0x00068C74
		public override int GetMonthsInYear(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year >= 1 && year <= 9999)
			{
				return 12;
			}
			throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
			{
				1,
				9999
			}));
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x00069CE8 File Offset: 0x00068CE8
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00069CF8 File Offset: 0x00068CF8
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9999
				}));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					12
				}));
			}
			if (day < 1 || day > this.GetDaysInMonth(year, month))
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					this.GetDaysInMonth(year, month)
				}));
			}
			return this.IsLeapYear(year) && (month == 2 && day == 29);
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x00069E18 File Offset: 0x00068E18
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9999
				}));
			}
			return 0;
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00069E8C File Offset: 0x00068E8C
		public override bool IsLeapMonth(int year, int month, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9999
				}));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					12
				}));
			}
			return false;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x00069F44 File Offset: 0x00068F44
		public override bool IsLeapYear(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year >= 1 && year <= 9999)
			{
				return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
			}
			throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
			{
				1,
				9999
			}));
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x00069FCF File Offset: 0x00068FCF
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			if (era == 0 || era == 1)
			{
				return new DateTime(year, month, day, hour, minute, second, millisecond);
			}
			throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x00069FFF File Offset: 0x00068FFF
		internal override bool TryToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era, out DateTime result)
		{
			if (era == 0 || era == 1)
			{
				return DateTime.TryCreate(year, month, day, hour, minute, second, millisecond, out result);
			}
			result = DateTime.MinValue;
			return false;
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x0006A02A File Offset: 0x0006902A
		// (set) Token: 0x060025E9 RID: 9705 RVA: 0x0006A054 File Offset: 0x00069054
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 2029);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > 9999)
				{
					throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						99,
						9999
					}));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x0006A0BC File Offset: 0x000690BC
		public override int ToFourDigitYear(int year)
		{
			if (year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9999
				}));
			}
			return base.ToFourDigitYear(year);
		}

		// Token: 0x0400112C RID: 4396
		public const int ADEra = 1;

		// Token: 0x0400112D RID: 4397
		internal const int DatePartYear = 0;

		// Token: 0x0400112E RID: 4398
		internal const int DatePartDayOfYear = 1;

		// Token: 0x0400112F RID: 4399
		internal const int DatePartMonth = 2;

		// Token: 0x04001130 RID: 4400
		internal const int DatePartDay = 3;

		// Token: 0x04001131 RID: 4401
		internal const int MaxYear = 9999;

		// Token: 0x04001132 RID: 4402
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 2029;

		// Token: 0x04001133 RID: 4403
		internal GregorianCalendarTypes m_type;

		// Token: 0x04001134 RID: 4404
		internal static readonly int[] DaysToMonth365 = new int[]
		{
			0,
			31,
			59,
			90,
			120,
			151,
			181,
			212,
			243,
			273,
			304,
			334,
			365
		};

		// Token: 0x04001135 RID: 4405
		internal static readonly int[] DaysToMonth366 = new int[]
		{
			0,
			31,
			60,
			91,
			121,
			152,
			182,
			213,
			244,
			274,
			305,
			335,
			366
		};

		// Token: 0x04001136 RID: 4406
		internal static Calendar m_defaultInstance = null;
	}
}
