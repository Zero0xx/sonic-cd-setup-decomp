using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003C2 RID: 962
	[ComVisible(true)]
	[Serializable]
	public class KoreanCalendar : Calendar
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060026E4 RID: 9956 RVA: 0x0007025A File Offset: 0x0006F25A
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060026E5 RID: 9957 RVA: 0x00070261 File Offset: 0x0006F261
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060026E6 RID: 9958 RVA: 0x00070268 File Offset: 0x0006F268
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x0007026B File Offset: 0x0006F26B
		public KoreanCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, KoreanCalendar.m_EraInfo);
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x00070284 File Offset: 0x0006F284
		internal override int ID
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x00070287 File Offset: 0x0006F287
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x00070296 File Offset: 0x0006F296
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000702A5 File Offset: 0x0006F2A5
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000702B5 File Offset: 0x0006F2B5
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000702C4 File Offset: 0x0006F2C4
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000702D2 File Offset: 0x0006F2D2
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x000702E0 File Offset: 0x0006F2E0
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000702EE File Offset: 0x0006F2EE
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x000702FD File Offset: 0x0006F2FD
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x0007030D File Offset: 0x0006F30D
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x0007031B File Offset: 0x0006F31B
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x00070329 File Offset: 0x0006F329
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x00070337 File Offset: 0x0006F337
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x00070349 File Offset: 0x0006F349
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x00070358 File Offset: 0x0006F358
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x00070367 File Offset: 0x0006F367
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x00070378 File Offset: 0x0006F378
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060026FA RID: 9978 RVA: 0x0007039D File Offset: 0x0006F39D
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x000703AA File Offset: 0x0006F3AA
		// (set) Token: 0x060026FC RID: 9980 RVA: 0x000703D4 File Offset: 0x0006F3D4
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 4362);
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

		// Token: 0x060026FD RID: 9981 RVA: 0x00070445 File Offset: 0x0006F445
		public override int ToFourDigitYear(int year)
		{
			return this.helper.ToFourDigitYear(year, this.TwoDigitYearMax);
		}

		// Token: 0x040011C3 RID: 4547
		public const int KoreanEra = 1;

		// Token: 0x040011C4 RID: 4548
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 4362;

		// Token: 0x040011C5 RID: 4549
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(5);

		// Token: 0x040011C6 RID: 4550
		internal GregorianCalendarHelper helper;
	}
}
