using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003BE RID: 958
	[ComVisible(true)]
	[Serializable]
	public class JapaneseCalendar : Calendar
	{
		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x0006E940 File Offset: 0x0006D940
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return JapaneseCalendar.calendarMinValue;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002676 RID: 9846 RVA: 0x0006E947 File Offset: 0x0006D947
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002677 RID: 9847 RVA: 0x0006E94E File Offset: 0x0006D94E
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x0006E951 File Offset: 0x0006D951
		internal static Calendar GetDefaultInstance()
		{
			if (JapaneseCalendar.m_defaultInstance == null)
			{
				JapaneseCalendar.m_defaultInstance = new JapaneseCalendar();
			}
			return JapaneseCalendar.m_defaultInstance;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0006E969 File Offset: 0x0006D969
		public JapaneseCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, JapaneseCalendar.m_EraInfo);
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600267A RID: 9850 RVA: 0x0006E982 File Offset: 0x0006D982
		internal override int ID
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0006E985 File Offset: 0x0006D985
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0006E994 File Offset: 0x0006D994
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x0006E9A3 File Offset: 0x0006D9A3
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x0006E9B3 File Offset: 0x0006D9B3
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0006E9C2 File Offset: 0x0006D9C2
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0006E9D0 File Offset: 0x0006D9D0
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0006E9DE File Offset: 0x0006D9DE
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0006E9EC File Offset: 0x0006D9EC
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x0006E9FB File Offset: 0x0006D9FB
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0006EA0B File Offset: 0x0006DA0B
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0006EA19 File Offset: 0x0006DA19
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0006EA27 File Offset: 0x0006DA27
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0006EA35 File Offset: 0x0006DA35
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x0006EA47 File Offset: 0x0006DA47
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x0006EA56 File Offset: 0x0006DA56
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0006EA65 File Offset: 0x0006DA65
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x0006EA78 File Offset: 0x0006DA78
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x0006EAA0 File Offset: 0x0006DAA0
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

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x0006EB18 File Offset: 0x0006DB18
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x0006EB25 File Offset: 0x0006DB25
		internal override bool IsValidYear(int year, int era)
		{
			return this.helper.IsValidYear(year, era);
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600268F RID: 9871 RVA: 0x0006EB34 File Offset: 0x0006DB34
		// (set) Token: 0x06002690 RID: 9872 RVA: 0x0006EB58 File Offset: 0x0006DB58
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

		// Token: 0x04001195 RID: 4501
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 99;

		// Token: 0x04001196 RID: 4502
		internal static readonly DateTime calendarMinValue = new DateTime(1868, 9, 8);

		// Token: 0x04001197 RID: 4503
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(3);

		// Token: 0x04001198 RID: 4504
		internal static Calendar m_defaultInstance;

		// Token: 0x04001199 RID: 4505
		internal GregorianCalendarHelper helper;
	}
}
