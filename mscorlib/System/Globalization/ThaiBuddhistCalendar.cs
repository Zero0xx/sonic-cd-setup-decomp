using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003CD RID: 973
	[ComVisible(true)]
	[Serializable]
	public class ThaiBuddhistCalendar : Calendar
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060027AD RID: 10157 RVA: 0x00076F2B File Offset: 0x00075F2B
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x00076F32 File Offset: 0x00075F32
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060027AF RID: 10159 RVA: 0x00076F39 File Offset: 0x00075F39
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x00076F49 File Offset: 0x00075F49
		public ThaiBuddhistCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, ThaiBuddhistCalendar.m_EraInfo);
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060027B2 RID: 10162 RVA: 0x00076F62 File Offset: 0x00075F62
		internal override int ID
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x00076F65 File Offset: 0x00075F65
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x00076F74 File Offset: 0x00075F74
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x00076F83 File Offset: 0x00075F83
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x00076F93 File Offset: 0x00075F93
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x00076FA2 File Offset: 0x00075FA2
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x00076FB0 File Offset: 0x00075FB0
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x00076FBE File Offset: 0x00075FBE
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x00076FCC File Offset: 0x00075FCC
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x00076FDB File Offset: 0x00075FDB
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x00076FEB File Offset: 0x00075FEB
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x00076FF9 File Offset: 0x00075FF9
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x00077007 File Offset: 0x00076007
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x00077015 File Offset: 0x00076015
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x00077027 File Offset: 0x00076027
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x00077036 File Offset: 0x00076036
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x00077045 File Offset: 0x00076045
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x00077058 File Offset: 0x00076058
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060027C4 RID: 10180 RVA: 0x0007707D File Offset: 0x0007607D
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060027C5 RID: 10181 RVA: 0x0007708A File Offset: 0x0007608A
		// (set) Token: 0x060027C6 RID: 10182 RVA: 0x000770B4 File Offset: 0x000760B4
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 2572);
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

		// Token: 0x060027C7 RID: 10183 RVA: 0x00077125 File Offset: 0x00076125
		public override int ToFourDigitYear(int year)
		{
			return this.helper.ToFourDigitYear(year, this.TwoDigitYearMax);
		}

		// Token: 0x0400121B RID: 4635
		public const int ThaiBuddhistEra = 1;

		// Token: 0x0400121C RID: 4636
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 2572;

		// Token: 0x0400121D RID: 4637
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(7);

		// Token: 0x0400121E RID: 4638
		internal GregorianCalendarHelper helper;
	}
}
