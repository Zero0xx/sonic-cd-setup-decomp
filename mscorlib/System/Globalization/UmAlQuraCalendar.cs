using System;

namespace System.Globalization
{
	// Token: 0x020003D0 RID: 976
	[Serializable]
	public class UmAlQuraCalendar : Calendar
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x00077E92 File Offset: 0x00076E92
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return UmAlQuraCalendar.minDate;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x00077E99 File Offset: 0x00076E99
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return UmAlQuraCalendar.maxDate;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x00077EA0 File Offset: 0x00076EA0
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.LunarCalendar;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x00077EAB File Offset: 0x00076EAB
		internal override int BaseCalendarID
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x00077EAE File Offset: 0x00076EAE
		internal override int ID
		{
			get
			{
				return 23;
			}
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x00077EB4 File Offset: 0x00076EB4
		private void ConvertHijriToGregorian(int HijriYear, int HijriMonth, int HijriDay, ref int yg, ref int mg, ref int dg)
		{
			int num = HijriDay - 1;
			int num2 = HijriYear - 1318;
			DateTime dateTime = UmAlQuraCalendar.HijriYearInfo[num2].GregorianDate;
			int num3 = UmAlQuraCalendar.HijriYearInfo[num2].HijriMonthsLengthFlags;
			for (int i = 1; i < HijriMonth; i++)
			{
				num += 29 + (num3 & 1);
				num3 >>= 1;
			}
			dateTime = dateTime.AddDays((double)num);
			yg = dateTime.Year;
			mg = dateTime.Month;
			dg = dateTime.Day;
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x00077F34 File Offset: 0x00076F34
		private long GetAbsoluteDateUmAlQura(int year, int month, int day)
		{
			int year2 = 0;
			int month2 = 0;
			int day2 = 0;
			this.ConvertHijriToGregorian(year, month, day, ref year2, ref month2, ref day2);
			return GregorianCalendar.GetAbsoluteDate(year2, month2, day2);
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x00077F60 File Offset: 0x00076F60
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < UmAlQuraCalendar.minDate.Ticks || ticks > UmAlQuraCalendar.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					UmAlQuraCalendar.minDate,
					UmAlQuraCalendar.maxDate
				}));
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x00077FC8 File Offset: 0x00076FC8
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x00077FE8 File Offset: 0x00076FE8
		internal void CheckYearRange(int year, int era)
		{
			this.CheckEraRange(era);
			if (year < 1318 || year > 1450)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1318,
					1450
				}));
			}
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x0007804D File Offset: 0x0007704D
		internal void CheckYearMonthRange(int year, int month, int era)
		{
			this.CheckYearRange(year, era);
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x00078078 File Offset: 0x00077078
		private void ConvertGregorianToHijri(DateTime time, ref int HijriYear, ref int HijriMonth, ref int HijriDay)
		{
			int num = (int)((time.Ticks - UmAlQuraCalendar.minDate.Ticks) / 864000000000L) / 355;
			while (time.CompareTo(UmAlQuraCalendar.HijriYearInfo[++num].GregorianDate) > 0)
			{
			}
			if (time.CompareTo(UmAlQuraCalendar.HijriYearInfo[num].GregorianDate) != 0)
			{
				num--;
			}
			TimeSpan timeSpan = time.Subtract(UmAlQuraCalendar.HijriYearInfo[num].GregorianDate);
			int num2 = num + 1318;
			int num3 = 1;
			int num4 = 1;
			double num5 = timeSpan.TotalDays;
			int num6 = UmAlQuraCalendar.HijriYearInfo[num].HijriMonthsLengthFlags;
			int num7 = 29 + (num6 & 1);
			while (num5 >= (double)num7)
			{
				num5 -= (double)num7;
				num6 >>= 1;
				num7 = 29 + (num6 & 1);
				num3++;
			}
			num4 += (int)num5;
			HijriDay = num4;
			HijriMonth = num3;
			HijriYear = num2;
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x00078168 File Offset: 0x00077168
		internal virtual int GetDatePart(DateTime time, int part)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			long ticks = time.Ticks;
			this.CheckTicksRange(ticks);
			this.ConvertGregorianToHijri(time, ref num, ref num2, ref num3);
			if (part == 0)
			{
				return num;
			}
			if (part == 2)
			{
				return num2;
			}
			if (part == 3)
			{
				return num3;
			}
			if (part == 1)
			{
				return (int)(this.GetAbsoluteDateUmAlQura(num, num2, num3) - this.GetAbsoluteDateUmAlQura(num, 1, 1) + 1L);
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DateTimeParsing"));
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000781D4 File Offset: 0x000771D4
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
			int num = this.GetDatePart(time, 0);
			int num2 = this.GetDatePart(time, 2);
			int num3 = this.GetDatePart(time, 3);
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
			if (num3 > 29)
			{
				int daysInMonth = this.GetDaysInMonth(num, num2);
				if (num3 > daysInMonth)
				{
					num3 = daysInMonth;
				}
			}
			this.CheckYearRange(num, 1);
			DateTime result = new DateTime(this.GetAbsoluteDateUmAlQura(num, num2, num3) * 864000000000L + time.Ticks % 864000000000L);
			Calendar.CheckAddResult(result.Ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return result;
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000782E4 File Offset: 0x000772E4
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000782F1 File Offset: 0x000772F1
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time, 3);
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000782FB File Offset: 0x000772FB
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x00078314 File Offset: 0x00077314
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time, 1);
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x0007831E File Offset: 0x0007731E
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			if ((UmAlQuraCalendar.HijriYearInfo[year - 1318].HijriMonthsLengthFlags & 1 << month - 1) == 0)
			{
				return 29;
			}
			return 30;
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x00078350 File Offset: 0x00077350
		internal int RealGetDaysInYear(int year)
		{
			int num = 0;
			int num2 = UmAlQuraCalendar.HijriYearInfo[year - 1318].HijriMonthsLengthFlags;
			for (int i = 1; i <= 12; i++)
			{
				num += 29 + (num2 & 1);
				num2 >>= 1;
			}
			return num;
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x00078391 File Offset: 0x00077391
		public override int GetDaysInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return this.RealGetDaysInYear(year);
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000783A2 File Offset: 0x000773A2
		public override int GetEra(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return 1;
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x000783B4 File Offset: 0x000773B4
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

		// Token: 0x0600282A RID: 10282 RVA: 0x000783CD File Offset: 0x000773CD
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time, 2);
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000783D7 File Offset: 0x000773D7
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 12;
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000783E3 File Offset: 0x000773E3
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time, 0);
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000783F0 File Offset: 0x000773F0
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			if (day >= 1 && day <= 29)
			{
				this.CheckYearMonthRange(year, month, era);
				return false;
			}
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[]
				{
					daysInMonth,
					month
				}));
			}
			return false;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x0007845F File Offset: 0x0007745F
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 0;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x0007846A File Offset: 0x0007746A
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			return false;
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x00078476 File Offset: 0x00077476
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return this.RealGetDaysInYear(year) == 355;
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x00078494 File Offset: 0x00077494
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			if (day >= 1 && day <= 29)
			{
				this.CheckYearMonthRange(year, month, era);
			}
			else
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
			}
			long absoluteDateUmAlQura = this.GetAbsoluteDateUmAlQura(year, month, day);
			if (absoluteDateUmAlQura >= 0L)
			{
				return new DateTime(absoluteDateUmAlQura * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x00078540 File Offset: 0x00077540
		// (set) Token: 0x06002833 RID: 10291 RVA: 0x00078568 File Offset: 0x00077568
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 1451);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value != 99 && (value < 1318 || value > 1450))
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						1318,
						1450
					}));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000785D8 File Offset: 0x000775D8
		public override int ToFourDigitYear(int year)
		{
			if (year < 100)
			{
				return base.ToFourDigitYear(year);
			}
			if (year < 1318 || year > 1450)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1318,
					1450
				}));
			}
			return year;
		}

		// Token: 0x04001254 RID: 4692
		internal const int MinCalendarYear = 1318;

		// Token: 0x04001255 RID: 4693
		internal const int MaxCalendarYear = 1450;

		// Token: 0x04001256 RID: 4694
		public const int UmAlQuraEra = 1;

		// Token: 0x04001257 RID: 4695
		internal const int DateCycle = 30;

		// Token: 0x04001258 RID: 4696
		internal const int DatePartYear = 0;

		// Token: 0x04001259 RID: 4697
		internal const int DatePartDayOfYear = 1;

		// Token: 0x0400125A RID: 4698
		internal const int DatePartMonth = 2;

		// Token: 0x0400125B RID: 4699
		internal const int DatePartDay = 3;

		// Token: 0x0400125C RID: 4700
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 1451;

		// Token: 0x0400125D RID: 4701
		private static readonly UmAlQuraCalendar.DateMapping[] HijriYearInfo = new UmAlQuraCalendar.DateMapping[]
		{
			new UmAlQuraCalendar.DateMapping(746, 1900, 4, 30),
			new UmAlQuraCalendar.DateMapping(1769, 1901, 4, 19),
			new UmAlQuraCalendar.DateMapping(3794, 1902, 4, 9),
			new UmAlQuraCalendar.DateMapping(3748, 1903, 3, 30),
			new UmAlQuraCalendar.DateMapping(3402, 1904, 3, 18),
			new UmAlQuraCalendar.DateMapping(2710, 1905, 3, 7),
			new UmAlQuraCalendar.DateMapping(1334, 1906, 2, 24),
			new UmAlQuraCalendar.DateMapping(2741, 1907, 2, 13),
			new UmAlQuraCalendar.DateMapping(3498, 1908, 2, 3),
			new UmAlQuraCalendar.DateMapping(2980, 1909, 1, 23),
			new UmAlQuraCalendar.DateMapping(2889, 1910, 1, 12),
			new UmAlQuraCalendar.DateMapping(2707, 1911, 1, 1),
			new UmAlQuraCalendar.DateMapping(1323, 1911, 12, 21),
			new UmAlQuraCalendar.DateMapping(2647, 1912, 12, 9),
			new UmAlQuraCalendar.DateMapping(1206, 1913, 11, 29),
			new UmAlQuraCalendar.DateMapping(2741, 1914, 11, 18),
			new UmAlQuraCalendar.DateMapping(1450, 1915, 11, 8),
			new UmAlQuraCalendar.DateMapping(3413, 1916, 10, 27),
			new UmAlQuraCalendar.DateMapping(3370, 1917, 10, 17),
			new UmAlQuraCalendar.DateMapping(2646, 1918, 10, 6),
			new UmAlQuraCalendar.DateMapping(1198, 1919, 9, 25),
			new UmAlQuraCalendar.DateMapping(2397, 1920, 9, 13),
			new UmAlQuraCalendar.DateMapping(748, 1921, 9, 3),
			new UmAlQuraCalendar.DateMapping(1749, 1922, 8, 23),
			new UmAlQuraCalendar.DateMapping(1706, 1923, 8, 13),
			new UmAlQuraCalendar.DateMapping(1365, 1924, 8, 1),
			new UmAlQuraCalendar.DateMapping(1195, 1925, 7, 21),
			new UmAlQuraCalendar.DateMapping(2395, 1926, 7, 10),
			new UmAlQuraCalendar.DateMapping(698, 1927, 6, 30),
			new UmAlQuraCalendar.DateMapping(1397, 1928, 6, 18),
			new UmAlQuraCalendar.DateMapping(2994, 1929, 6, 8),
			new UmAlQuraCalendar.DateMapping(1892, 1930, 5, 29),
			new UmAlQuraCalendar.DateMapping(1865, 1931, 5, 18),
			new UmAlQuraCalendar.DateMapping(1621, 1932, 5, 6),
			new UmAlQuraCalendar.DateMapping(683, 1933, 4, 25),
			new UmAlQuraCalendar.DateMapping(1371, 1934, 4, 14),
			new UmAlQuraCalendar.DateMapping(2778, 1935, 4, 4),
			new UmAlQuraCalendar.DateMapping(1748, 1936, 3, 24),
			new UmAlQuraCalendar.DateMapping(3785, 1937, 3, 13),
			new UmAlQuraCalendar.DateMapping(3474, 1938, 3, 3),
			new UmAlQuraCalendar.DateMapping(3365, 1939, 2, 20),
			new UmAlQuraCalendar.DateMapping(2637, 1940, 2, 9),
			new UmAlQuraCalendar.DateMapping(685, 1941, 1, 28),
			new UmAlQuraCalendar.DateMapping(1389, 1942, 1, 17),
			new UmAlQuraCalendar.DateMapping(2922, 1943, 1, 7),
			new UmAlQuraCalendar.DateMapping(2898, 1943, 12, 28),
			new UmAlQuraCalendar.DateMapping(2725, 1944, 12, 16),
			new UmAlQuraCalendar.DateMapping(2635, 1945, 12, 5),
			new UmAlQuraCalendar.DateMapping(1175, 1946, 11, 24),
			new UmAlQuraCalendar.DateMapping(2359, 1947, 11, 13),
			new UmAlQuraCalendar.DateMapping(694, 1948, 11, 2),
			new UmAlQuraCalendar.DateMapping(1397, 1949, 10, 22),
			new UmAlQuraCalendar.DateMapping(3434, 1950, 10, 12),
			new UmAlQuraCalendar.DateMapping(3410, 1951, 10, 2),
			new UmAlQuraCalendar.DateMapping(2710, 1952, 9, 20),
			new UmAlQuraCalendar.DateMapping(2349, 1953, 9, 9),
			new UmAlQuraCalendar.DateMapping(605, 1954, 8, 29),
			new UmAlQuraCalendar.DateMapping(1245, 1955, 8, 18),
			new UmAlQuraCalendar.DateMapping(2778, 1956, 8, 7),
			new UmAlQuraCalendar.DateMapping(1492, 1957, 7, 28),
			new UmAlQuraCalendar.DateMapping(3497, 1958, 7, 17),
			new UmAlQuraCalendar.DateMapping(3410, 1959, 7, 7),
			new UmAlQuraCalendar.DateMapping(2730, 1960, 6, 25),
			new UmAlQuraCalendar.DateMapping(1238, 1961, 6, 14),
			new UmAlQuraCalendar.DateMapping(2486, 1962, 6, 3),
			new UmAlQuraCalendar.DateMapping(884, 1963, 5, 24),
			new UmAlQuraCalendar.DateMapping(1897, 1964, 5, 12),
			new UmAlQuraCalendar.DateMapping(1874, 1965, 5, 2),
			new UmAlQuraCalendar.DateMapping(1701, 1966, 4, 21),
			new UmAlQuraCalendar.DateMapping(1355, 1967, 4, 10),
			new UmAlQuraCalendar.DateMapping(2731, 1968, 3, 29),
			new UmAlQuraCalendar.DateMapping(1370, 1969, 3, 19),
			new UmAlQuraCalendar.DateMapping(2773, 1970, 3, 8),
			new UmAlQuraCalendar.DateMapping(3538, 1971, 2, 26),
			new UmAlQuraCalendar.DateMapping(3492, 1972, 2, 16),
			new UmAlQuraCalendar.DateMapping(3401, 1973, 2, 4),
			new UmAlQuraCalendar.DateMapping(2709, 1974, 1, 24),
			new UmAlQuraCalendar.DateMapping(1325, 1975, 1, 13),
			new UmAlQuraCalendar.DateMapping(2653, 1976, 1, 2),
			new UmAlQuraCalendar.DateMapping(1370, 1976, 12, 22),
			new UmAlQuraCalendar.DateMapping(2773, 1977, 12, 11),
			new UmAlQuraCalendar.DateMapping(1706, 1978, 12, 1),
			new UmAlQuraCalendar.DateMapping(1685, 1979, 11, 20),
			new UmAlQuraCalendar.DateMapping(1323, 1980, 11, 8),
			new UmAlQuraCalendar.DateMapping(2647, 1981, 10, 28),
			new UmAlQuraCalendar.DateMapping(1198, 1982, 10, 18),
			new UmAlQuraCalendar.DateMapping(2422, 1983, 10, 7),
			new UmAlQuraCalendar.DateMapping(1388, 1984, 9, 26),
			new UmAlQuraCalendar.DateMapping(2901, 1985, 9, 15),
			new UmAlQuraCalendar.DateMapping(2730, 1986, 9, 5),
			new UmAlQuraCalendar.DateMapping(2645, 1987, 8, 25),
			new UmAlQuraCalendar.DateMapping(1197, 1988, 8, 13),
			new UmAlQuraCalendar.DateMapping(2397, 1989, 8, 2),
			new UmAlQuraCalendar.DateMapping(730, 1990, 7, 23),
			new UmAlQuraCalendar.DateMapping(1497, 1991, 7, 12),
			new UmAlQuraCalendar.DateMapping(3506, 1992, 7, 1),
			new UmAlQuraCalendar.DateMapping(2980, 1993, 6, 21),
			new UmAlQuraCalendar.DateMapping(2890, 1994, 6, 10),
			new UmAlQuraCalendar.DateMapping(2645, 1995, 5, 30),
			new UmAlQuraCalendar.DateMapping(693, 1996, 5, 18),
			new UmAlQuraCalendar.DateMapping(1397, 1997, 5, 7),
			new UmAlQuraCalendar.DateMapping(2922, 1998, 4, 27),
			new UmAlQuraCalendar.DateMapping(3026, 1999, 4, 17),
			new UmAlQuraCalendar.DateMapping(3012, 2000, 4, 6),
			new UmAlQuraCalendar.DateMapping(2953, 2001, 3, 26),
			new UmAlQuraCalendar.DateMapping(2709, 2002, 3, 15),
			new UmAlQuraCalendar.DateMapping(1325, 2003, 3, 4),
			new UmAlQuraCalendar.DateMapping(1453, 2004, 2, 21),
			new UmAlQuraCalendar.DateMapping(2922, 2005, 2, 10),
			new UmAlQuraCalendar.DateMapping(1748, 2006, 1, 31),
			new UmAlQuraCalendar.DateMapping(3529, 2007, 1, 20),
			new UmAlQuraCalendar.DateMapping(3474, 2008, 1, 10),
			new UmAlQuraCalendar.DateMapping(2726, 2008, 12, 29),
			new UmAlQuraCalendar.DateMapping(2390, 2009, 12, 18),
			new UmAlQuraCalendar.DateMapping(686, 2010, 12, 7),
			new UmAlQuraCalendar.DateMapping(1389, 2011, 11, 26),
			new UmAlQuraCalendar.DateMapping(874, 2012, 11, 15),
			new UmAlQuraCalendar.DateMapping(2901, 2013, 11, 4),
			new UmAlQuraCalendar.DateMapping(2730, 2014, 10, 25),
			new UmAlQuraCalendar.DateMapping(2381, 2015, 10, 14),
			new UmAlQuraCalendar.DateMapping(1181, 2016, 10, 2),
			new UmAlQuraCalendar.DateMapping(2397, 2017, 9, 21),
			new UmAlQuraCalendar.DateMapping(698, 2018, 9, 11),
			new UmAlQuraCalendar.DateMapping(1461, 2019, 8, 31),
			new UmAlQuraCalendar.DateMapping(1450, 2020, 8, 20),
			new UmAlQuraCalendar.DateMapping(3413, 2021, 8, 9),
			new UmAlQuraCalendar.DateMapping(2714, 2022, 7, 30),
			new UmAlQuraCalendar.DateMapping(2350, 2023, 7, 19),
			new UmAlQuraCalendar.DateMapping(622, 2024, 7, 7),
			new UmAlQuraCalendar.DateMapping(1373, 2025, 6, 26),
			new UmAlQuraCalendar.DateMapping(2778, 2026, 6, 16),
			new UmAlQuraCalendar.DateMapping(1748, 2027, 6, 6),
			new UmAlQuraCalendar.DateMapping(1701, 2028, 5, 25),
			new UmAlQuraCalendar.DateMapping(0, 2029, 5, 14)
		};

		// Token: 0x0400125E RID: 4702
		internal static short[] gmonth = new short[]
		{
			31,
			31,
			28,
			31,
			30,
			31,
			30,
			31,
			31,
			30,
			31,
			30,
			31,
			31
		};

		// Token: 0x0400125F RID: 4703
		internal static DateTime minDate = new DateTime(1900, 4, 30);

		// Token: 0x04001260 RID: 4704
		internal static DateTime maxDate = new DateTime(new DateTime(2029, 5, 13, 23, 59, 59, 999).Ticks + 9999L);

		// Token: 0x020003D1 RID: 977
		internal struct DateMapping
		{
			// Token: 0x06002836 RID: 10294 RVA: 0x00079723 File Offset: 0x00078723
			internal DateMapping(int MonthsLengthFlags, int GYear, int GMonth, int GDay)
			{
				this.HijriMonthsLengthFlags = MonthsLengthFlags;
				this.GregorianDate = new DateTime(GYear, GMonth, GDay);
			}

			// Token: 0x04001261 RID: 4705
			internal int HijriMonthsLengthFlags;

			// Token: 0x04001262 RID: 4706
			internal DateTime GregorianDate;
		}
	}
}
