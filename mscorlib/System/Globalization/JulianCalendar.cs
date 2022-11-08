using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003C1 RID: 961
	[ComVisible(true)]
	[Serializable]
	public class JulianCalendar : Calendar
	{
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x0006FB59 File Offset: 0x0006EB59
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060026C5 RID: 9925 RVA: 0x0006FB60 File Offset: 0x0006EB60
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x0006FB67 File Offset: 0x0006EB67
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x0006FB6A File Offset: 0x0006EB6A
		public JulianCalendar()
		{
			this.twoDigitYearMax = 2029;
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x0006FB88 File Offset: 0x0006EB88
		internal override int ID
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0006FB8C File Offset: 0x0006EB8C
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != JulianCalendar.JulianEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x0006FBB0 File Offset: 0x0006EBB0
		internal void CheckYearEraRange(int year, int era)
		{
			this.CheckEraRange(era);
			if (year <= 0 || year > this.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					this.MaxYear
				}));
			}
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0006FC0F File Offset: 0x0006EC0F
		internal void CheckMonthRange(int month)
		{
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0006FC30 File Offset: 0x0006EC30
		internal void CheckDayRange(int year, int month, int day)
		{
			if (year == 1 && month == 1 && day < 3)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
			}
			int[] array = (year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			int num = array[month] - array[month - 1];
			if (day < 1 || day > num)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					num
				}));
			}
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x0006FCBC File Offset: 0x0006ECBC
		internal int GetDatePart(long ticks, int part)
		{
			long num = ticks + 1728000000000L;
			int i = (int)(num / 864000000000L);
			int num2 = i / 1461;
			i -= num2 * 1461;
			int num3 = i / 365;
			if (num3 == 4)
			{
				num3 = 3;
			}
			if (part == 0)
			{
				return num2 * 4 + num3 + 1;
			}
			i -= num3 * 365;
			if (part == 1)
			{
				return i + 1;
			}
			int[] array = (num3 == 3) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			int num4 = i >> 6;
			while (i >= array[num4])
			{
				num4++;
			}
			if (part == 2)
			{
				return num4;
			}
			return i - array[num4 - 1] + 1;
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x0006FD60 File Offset: 0x0006ED60
		internal long DateToTicks(int year, int month, int day)
		{
			int[] array = (year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			int num = year - 1;
			int num2 = num * 365 + num / 4 + array[month - 1] + day - 1;
			return (long)(num2 - 2) * 864000000000L;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x0006FDA8 File Offset: 0x0006EDA8
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
			int[] array = (num % 4 == 0 && (num % 100 != 0 || num % 400 == 0)) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			int num5 = array[num2] - array[num2 - 1];
			if (num3 > num5)
			{
				num3 = num5;
			}
			long ticks = this.DateToTicks(num, num2, num3) + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x0006FED3 File Offset: 0x0006EED3
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x0006FEE0 File Offset: 0x0006EEE0
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x0006FEF0 File Offset: 0x0006EEF0
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x0006FF09 File Offset: 0x0006EF09
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x0006FF1C File Offset: 0x0006EF1C
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearEraRange(year, era);
			this.CheckMonthRange(month);
			int[] array = (year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			return array[month] - array[month - 1];
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x0006FF53 File Offset: 0x0006EF53
		public override int GetDaysInYear(int year, int era)
		{
			if (!this.IsLeapYear(year, era))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x0006FF6A File Offset: 0x0006EF6A
		public override int GetEra(DateTime time)
		{
			return JulianCalendar.JulianEra;
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x0006FF71 File Offset: 0x0006EF71
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x0006FF84 File Offset: 0x0006EF84
		public override int[] Eras
		{
			get
			{
				return new int[]
				{
					JulianCalendar.JulianEra
				};
			}
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x0006FFA1 File Offset: 0x0006EFA1
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return 12;
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x0006FFAD File Offset: 0x0006EFAD
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x0006FFBD File Offset: 0x0006EFBD
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			this.CheckMonthRange(month);
			if (this.IsLeapYear(year, era))
			{
				this.CheckDayRange(year, month, day);
				return month == 2 && day == 29;
			}
			this.CheckDayRange(year, month, day);
			return false;
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x0006FFF0 File Offset: 0x0006EFF0
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return 0;
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x0006FFFB File Offset: 0x0006EFFB
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearEraRange(year, era);
			this.CheckMonthRange(month);
			return false;
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x0007000D File Offset: 0x0006F00D
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return year % 4 == 0;
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x00070020 File Offset: 0x0006F020
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			this.CheckYearEraRange(year, era);
			this.CheckMonthRange(month);
			this.CheckDayRange(year, month, day);
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					0,
					999
				}));
			}
			if (hour >= 0 && hour < 24 && minute >= 0 && minute < 60 && second >= 0 && second < 60)
			{
				return new DateTime(this.DateToTicks(year, month, day) + new TimeSpan(0, hour, minute, second, millisecond).Ticks);
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060026E0 RID: 9952 RVA: 0x000700E8 File Offset: 0x0006F0E8
		// (set) Token: 0x060026E1 RID: 9953 RVA: 0x000700F0 File Offset: 0x0006F0F0
		public override int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > this.MaxYear)
				{
					throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						99,
						this.MaxYear
					}));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x00070158 File Offset: 0x0006F158
		public override int ToFourDigitYear(int year)
		{
			if (year > this.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[]
				{
					1,
					this.MaxYear
				}));
			}
			return base.ToFourDigitYear(year);
		}

		// Token: 0x040011B9 RID: 4537
		private const int DatePartYear = 0;

		// Token: 0x040011BA RID: 4538
		private const int DatePartDayOfYear = 1;

		// Token: 0x040011BB RID: 4539
		private const int DatePartMonth = 2;

		// Token: 0x040011BC RID: 4540
		private const int DatePartDay = 3;

		// Token: 0x040011BD RID: 4541
		private const int JulianDaysPerYear = 365;

		// Token: 0x040011BE RID: 4542
		private const int JulianDaysPer4Years = 1461;

		// Token: 0x040011BF RID: 4543
		public static readonly int JulianEra = 1;

		// Token: 0x040011C0 RID: 4544
		private static readonly int[] DaysToMonth365 = new int[]
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

		// Token: 0x040011C1 RID: 4545
		private static readonly int[] DaysToMonth366 = new int[]
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

		// Token: 0x040011C2 RID: 4546
		internal int MaxYear = 9999;
	}
}
