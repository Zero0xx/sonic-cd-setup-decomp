using System;

namespace System.Globalization
{
	// Token: 0x020003C0 RID: 960
	[Serializable]
	public class PersianCalendar : Calendar
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060026A2 RID: 9890 RVA: 0x0006F2FC File Offset: 0x0006E2FC
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return PersianCalendar.minDate;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x060026A3 RID: 9891 RVA: 0x0006F303 File Offset: 0x0006E303
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return PersianCalendar.maxDate;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x060026A4 RID: 9892 RVA: 0x0006F30A File Offset: 0x0006E30A
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060026A6 RID: 9894 RVA: 0x0006F315 File Offset: 0x0006E315
		internal override int BaseCalendarID
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060026A7 RID: 9895 RVA: 0x0006F318 File Offset: 0x0006E318
		internal override int ID
		{
			get
			{
				return 22;
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0006F31C File Offset: 0x0006E31C
		private long GetAbsoluteDatePersian(int year, int month, int day)
		{
			if (year >= 1 && year <= 9378 && month >= 1 && month <= 12)
			{
				return this.DaysUpToPersianYear(year) + (long)PersianCalendar.DaysToMonth[month - 1] + (long)day - 1L;
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0006F35C File Offset: 0x0006E35C
		private long DaysUpToPersianYear(int PersianYear)
		{
			int num = (PersianYear - 1) / 33;
			int i = (PersianYear - 1) % 33;
			long num2 = (long)num * 12053L + 226894L;
			while (i > 0)
			{
				num2 += 365L;
				if (this.IsLeapYear(i, 0))
				{
					num2 += 1L;
				}
				i--;
			}
			return num2;
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x0006F3AC File Offset: 0x0006E3AC
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < PersianCalendar.minDate.Ticks || ticks > PersianCalendar.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					PersianCalendar.minDate,
					PersianCalendar.maxDate
				}));
			}
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x0006F414 File Offset: 0x0006E414
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != PersianCalendar.PersianEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x0006F438 File Offset: 0x0006E438
		internal void CheckYearRange(int year, int era)
		{
			this.CheckEraRange(era);
			if (year < 1 || year > 9378)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9378
				}));
			}
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x0006F498 File Offset: 0x0006E498
		internal void CheckYearMonthRange(int year, int month, int era)
		{
			this.CheckYearRange(year, era);
			if (year == 9378 && month > 10)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					10
				}));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x0006F514 File Offset: 0x0006E514
		internal int GetDatePart(long ticks, int part)
		{
			this.CheckTicksRange(ticks);
			long num = ticks / 864000000000L + 1L;
			int num2 = (int)((num - 226894L) * 33L / 12053L) + 1;
			long num3 = this.DaysUpToPersianYear(num2);
			long num4 = (long)this.GetDaysInYear(num2, 0);
			if (num < num3)
			{
				num3 -= num4;
				num2--;
			}
			else if (num == num3)
			{
				num2--;
				num3 -= (long)this.GetDaysInYear(num2, 0);
			}
			else if (num > num3 + num4)
			{
				num3 += num4;
				num2++;
			}
			if (part == 0)
			{
				return num2;
			}
			num -= num3;
			if (part == 1)
			{
				return (int)num;
			}
			int num5 = 0;
			while (num5 < 12 && num > (long)PersianCalendar.DaysToMonth[num5])
			{
				num5++;
			}
			if (part == 2)
			{
				return num5;
			}
			int result = (int)(num - (long)PersianCalendar.DaysToMonth[num5 - 1]);
			if (part == 3)
			{
				return result;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DateTimeParsing"));
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x0006F5F4 File Offset: 0x0006E5F4
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
			int daysInMonth = this.GetDaysInMonth(num, num2);
			if (num3 > daysInMonth)
			{
				num3 = daysInMonth;
			}
			long ticks = this.GetAbsoluteDatePersian(num, num2, num3) * 864000000000L + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x0006F704 File Offset: 0x0006E704
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x0006F711 File Offset: 0x0006E711
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0006F721 File Offset: 0x0006E721
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x0006F73A File Offset: 0x0006E73A
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0006F74A File Offset: 0x0006E74A
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			if (month == 10 && year == 9378)
			{
				return 10;
			}
			if (month == 12)
			{
				if (!this.IsLeapYear(year, 0))
				{
					return 29;
				}
				return 30;
			}
			else
			{
				if (month <= 6)
				{
					return 31;
				}
				return 30;
			}
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x0006F783 File Offset: 0x0006E783
		public override int GetDaysInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			if (year == 9378)
			{
				return PersianCalendar.DaysToMonth[9] + 10;
			}
			if (!this.IsLeapYear(year, 0))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x0006F7B6 File Offset: 0x0006E7B6
		public override int GetEra(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return PersianCalendar.PersianEra;
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060026B7 RID: 9911 RVA: 0x0006F7CC File Offset: 0x0006E7CC
		public override int[] Eras
		{
			get
			{
				return new int[]
				{
					PersianCalendar.PersianEra
				};
			}
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0006F7E9 File Offset: 0x0006E7E9
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0006F7F9 File Offset: 0x0006E7F9
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			if (year == 9378)
			{
				return 10;
			}
			return 12;
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x0006F810 File Offset: 0x0006E810
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x0006F820 File Offset: 0x0006E820
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[]
				{
					daysInMonth,
					month
				}));
			}
			return this.IsLeapYear(year, era) && month == 12 && day == 30;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0006F890 File Offset: 0x0006E890
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 0;
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0006F89B File Offset: 0x0006E89B
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			return false;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0006F8A7 File Offset: 0x0006E8A7
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return PersianCalendar.LeapYears33[year % 33] == 1;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0006F8C0 File Offset: 0x0006E8C0
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[]
				{
					daysInMonth,
					month
				}));
			}
			long absoluteDatePersian = this.GetAbsoluteDatePersian(year, month, day);
			if (absoluteDatePersian >= 0L)
			{
				return new DateTime(absoluteDatePersian * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x0006F957 File Offset: 0x0006E957
		// (set) Token: 0x060026C1 RID: 9921 RVA: 0x0006F980 File Offset: 0x0006E980
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 1410);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > 9378)
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						99,
						9378
					}));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x0006F9E8 File Offset: 0x0006E9E8
		public override int ToFourDigitYear(int year)
		{
			if (year < 100)
			{
				return base.ToFourDigitYear(year);
			}
			if (year > 9378)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9378
				}));
			}
			return year;
		}

		// Token: 0x040011A8 RID: 4520
		internal const int DateCycle = 33;

		// Token: 0x040011A9 RID: 4521
		internal const int DatePartYear = 0;

		// Token: 0x040011AA RID: 4522
		internal const int DatePartDayOfYear = 1;

		// Token: 0x040011AB RID: 4523
		internal const int DatePartMonth = 2;

		// Token: 0x040011AC RID: 4524
		internal const int DatePartDay = 3;

		// Token: 0x040011AD RID: 4525
		internal const int LeapYearsPerCycle = 8;

		// Token: 0x040011AE RID: 4526
		internal const long GregorianOffset = 226894L;

		// Token: 0x040011AF RID: 4527
		internal const long DaysPerCycle = 12053L;

		// Token: 0x040011B0 RID: 4528
		internal const int MaxCalendarYear = 9378;

		// Token: 0x040011B1 RID: 4529
		internal const int MaxCalendarMonth = 10;

		// Token: 0x040011B2 RID: 4530
		internal const int MaxCalendarDay = 10;

		// Token: 0x040011B3 RID: 4531
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 1410;

		// Token: 0x040011B4 RID: 4532
		public static readonly int PersianEra = 1;

		// Token: 0x040011B5 RID: 4533
		internal static int[] DaysToMonth = new int[]
		{
			0,
			31,
			62,
			93,
			124,
			155,
			186,
			216,
			246,
			276,
			306,
			336
		};

		// Token: 0x040011B6 RID: 4534
		internal static int[] LeapYears33 = new int[]
		{
			0,
			1,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			1,
			0,
			0
		};

		// Token: 0x040011B7 RID: 4535
		internal static DateTime minDate = new DateTime(622, 3, 21);

		// Token: 0x040011B8 RID: 4536
		internal static DateTime maxDate = DateTime.MaxValue;
	}
}
