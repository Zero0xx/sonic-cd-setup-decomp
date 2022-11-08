using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003C7 RID: 967
	[ComVisible(true)]
	[Serializable]
	public class TaiwanCalendar : Calendar
	{
		// Token: 0x0600273F RID: 10047 RVA: 0x000756F7 File Offset: 0x000746F7
		internal static Calendar GetDefaultInstance()
		{
			if (TaiwanCalendar.m_defaultInstance == null)
			{
				TaiwanCalendar.m_defaultInstance = new TaiwanCalendar();
			}
			return TaiwanCalendar.m_defaultInstance;
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002740 RID: 10048 RVA: 0x0007570F File Offset: 0x0007470F
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return TaiwanCalendar.calendarMinValue;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x00075716 File Offset: 0x00074716
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002742 RID: 10050 RVA: 0x0007571D File Offset: 0x0007471D
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x00075720 File Offset: 0x00074720
		public TaiwanCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, TaiwanCalendar.m_EraInfo);
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002744 RID: 10052 RVA: 0x00075739 File Offset: 0x00074739
		internal override int ID
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x0007573C File Offset: 0x0007473C
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x0007574B File Offset: 0x0007474B
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x0007575A File Offset: 0x0007475A
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x0007576A File Offset: 0x0007476A
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x00075779 File Offset: 0x00074779
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x00075787 File Offset: 0x00074787
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x00075795 File Offset: 0x00074795
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000757A3 File Offset: 0x000747A3
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000757B2 File Offset: 0x000747B2
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000757C2 File Offset: 0x000747C2
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000757D0 File Offset: 0x000747D0
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000757DE File Offset: 0x000747DE
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x000757EC File Offset: 0x000747EC
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000757FE File Offset: 0x000747FE
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x0007580D File Offset: 0x0007480D
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x0007581C File Offset: 0x0007481C
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x0007582C File Offset: 0x0007482C
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002756 RID: 10070 RVA: 0x00075851 File Offset: 0x00074851
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002757 RID: 10071 RVA: 0x0007585E File Offset: 0x0007485E
		// (set) Token: 0x06002758 RID: 10072 RVA: 0x00075884 File Offset: 0x00074884
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 99);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > this.helper.MaxYear)
				{
					throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						99,
						this.helper.MaxYear
					}));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000758F8 File Offset: 0x000748F8
		public override int ToFourDigitYear(int year)
		{
			if (year <= 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (year > this.helper.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					this.helper.MaxYear
				}));
			}
			return year;
		}

		// Token: 0x040011DF RID: 4575
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 99;

		// Token: 0x040011E0 RID: 4576
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(4);

		// Token: 0x040011E1 RID: 4577
		internal static Calendar m_defaultInstance;

		// Token: 0x040011E2 RID: 4578
		internal GregorianCalendarHelper helper;

		// Token: 0x040011E3 RID: 4579
		internal static readonly DateTime calendarMinValue = new DateTime(1912, 1, 1);
	}
}
