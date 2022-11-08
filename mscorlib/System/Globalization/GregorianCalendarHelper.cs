using System;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x020003B9 RID: 953
	[Serializable]
	internal class GregorianCalendarHelper
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x0006A1E7 File Offset: 0x000691E7
		internal int MaxYear
		{
			get
			{
				return this.m_maxYear;
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x0006A1F0 File Offset: 0x000691F0
		internal static EraInfo[] InitEraInfo(int calID)
		{
			int[][] array = CalendarTable.Default.SERARANGES(calID);
			EraInfo[] array2 = new EraInfo[array.Length];
			int num = 9999;
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = new EraInfo(array[i][0], new DateTime(array[i][1], array[i][2], array[i][3]).Ticks, array[i][4], array[i][5], num - array[i][4]);
				num = array[i][1];
			}
			return array2;
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x0006A268 File Offset: 0x00069268
		internal GregorianCalendarHelper(Calendar cal, EraInfo[] eraInfo)
		{
			this.m_Cal = cal;
			this.m_EraInfo = eraInfo;
			this.m_minDate = this.m_Cal.MinSupportedDateTime;
			this.m_maxYear = this.m_EraInfo[0].maxEraYear;
			this.m_minYear = this.m_EraInfo[0].minEraYear;
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x0006A2CB File Offset: 0x000692CB
		private static bool EnforceJapaneseEraYearRanges
		{
			get
			{
				if (GregorianCalendarHelper.s_enforceJapaneseEraYearRanges == 0)
				{
					GregorianCalendarHelper.InitializeJapaneseCalendarConfigSwitches();
				}
				return GregorianCalendarHelper.s_enforceJapaneseEraYearRanges == 1;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060025F1 RID: 9713 RVA: 0x0006A2E1 File Offset: 0x000692E1
		internal static bool FormatJapaneseFirstYearAsANumber
		{
			get
			{
				if (GregorianCalendarHelper.s_formatJapaneseFirstYearAsANumber == 0)
				{
					GregorianCalendarHelper.InitializeJapaneseCalendarConfigSwitches();
				}
				return GregorianCalendarHelper.s_formatJapaneseFirstYearAsANumber == 1;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x0006A2F7 File Offset: 0x000692F7
		internal static bool EnforceLegacyJapaneseDateParsing
		{
			get
			{
				if (GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing == 0)
				{
					GregorianCalendarHelper.InitializeJapaneseCalendarConfigSwitches();
				}
				return GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing == 1;
			}
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x0006A310 File Offset: 0x00069310
		private static void InitializeJapaneseCalendarConfigSwitches()
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.InternalOpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AppContext", false))
				{
					if (registryKey == null)
					{
						GregorianCalendarHelper.s_enforceJapaneseEraYearRanges = 2;
						GregorianCalendarHelper.s_formatJapaneseFirstYearAsANumber = 2;
						GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing = 2;
					}
					else
					{
						string text = registryKey.InternalGetValue("Switch.System.Globalization.EnforceJapaneseEraYearRanges", null, false, false) as string;
						GregorianCalendarHelper.s_enforceJapaneseEraYearRanges = ((text == null) ? 2 : ((text == "1" || text.Equals("true", StringComparison.OrdinalIgnoreCase)) ? 1 : 2));
						text = (registryKey.InternalGetValue("Switch.System.Globalization.FormatJapaneseFirstYearAsANumber", null, false, false) as string);
						GregorianCalendarHelper.s_formatJapaneseFirstYearAsANumber = ((text == null) ? 2 : ((text == "1" || text.Equals("true", StringComparison.OrdinalIgnoreCase)) ? 1 : 2));
						text = (registryKey.InternalGetValue("Switch.System.Globalization.EnforceLegacyJapaneseDateParsing", null, false, false) as string);
						GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing = ((text == null) ? 2 : ((text == "1" || text.Equals("true", StringComparison.OrdinalIgnoreCase)) ? 1 : 2));
					}
				}
			}
			catch
			{
				if (GregorianCalendarHelper.s_enforceJapaneseEraYearRanges == 0)
				{
					GregorianCalendarHelper.s_enforceJapaneseEraYearRanges = 2;
				}
				if (GregorianCalendarHelper.s_enforceJapaneseEraYearRanges == 0)
				{
					GregorianCalendarHelper.s_enforceJapaneseEraYearRanges = 2;
				}
				if (GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing == 0)
				{
					GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing = 2;
				}
			}
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x0006A458 File Offset: 0x00069458
		private int GetYearOffset(int year, int era, bool throwOnError)
		{
			if (year < 0)
			{
				if (throwOnError)
				{
					throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				return -1;
			}
			else
			{
				if (era == 0)
				{
					era = this.m_Cal.CurrentEraValue;
				}
				int i = 0;
				while (i < this.m_EraInfo.Length)
				{
					if (era == this.m_EraInfo[i].era)
					{
						if (year >= this.m_EraInfo[i].minEraYear)
						{
							if (year <= this.m_EraInfo[i].maxEraYear)
							{
								return this.m_EraInfo[i].yearOffset;
							}
							if (!GregorianCalendarHelper.EnforceJapaneseEraYearRanges)
							{
								int num = year - this.m_EraInfo[i].maxEraYear;
								for (int j = i - 1; j >= 0; j--)
								{
									if (num <= this.m_EraInfo[j].maxEraYear)
									{
										return this.m_EraInfo[i].yearOffset;
									}
									num -= this.m_EraInfo[j].maxEraYear;
								}
							}
						}
						if (throwOnError)
						{
							throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
							{
								this.m_EraInfo[i].minEraYear,
								this.m_EraInfo[i].maxEraYear
							}));
						}
						break;
					}
					else
					{
						i++;
					}
				}
				if (throwOnError)
				{
					throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
				}
				return -1;
			}
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0006A5AD File Offset: 0x000695AD
		internal int GetGregorianYear(int year, int era)
		{
			return this.GetYearOffset(year, era, true) + year;
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0006A5BA File Offset: 0x000695BA
		internal bool IsValidYear(int year, int era)
		{
			return this.GetYearOffset(year, era, false) >= 0;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0006A5CC File Offset: 0x000695CC
		internal virtual int GetDatePart(long ticks, int part)
		{
			this.CheckTicksRange(ticks);
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
			int[] array = (num4 == 3 && (num3 != 24 || num2 == 3)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365;
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

		// Token: 0x060025F8 RID: 9720 RVA: 0x0006A6B8 File Offset: 0x000696B8
		internal static long GetAbsoluteDate(int year, int month, int day)
		{
			if (year >= 1 && year <= 9999 && month >= 1 && month <= 12)
			{
				int[] array = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365;
				if (day >= 1 && day <= array[month] - array[month - 1])
				{
					int num = year - 1;
					int num2 = num * 365 + num / 4 - num / 100 + num / 400 + array[month - 1] + day - 1;
					return (long)num2;
				}
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x0006A745 File Offset: 0x00069745
		internal static long DateToTicks(int year, int month, int day)
		{
			return GregorianCalendarHelper.GetAbsoluteDate(year, month, day) * 864000000000L;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x0006A75C File Offset: 0x0006975C
		internal static long TimeToTicks(int hour, int minute, int second, int millisecond)
		{
			if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60 || second < 0 || second >= 60)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					0,
					999
				}));
			}
			return TimeSpan.TimeToTicks(hour, minute, second) + (long)millisecond * 10000L;
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x0006A7F0 File Offset: 0x000697F0
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < this.m_Cal.MinSupportedDateTime.Ticks || ticks > this.m_Cal.MaxSupportedDateTime.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					this.m_Cal.MinSupportedDateTime,
					this.m_Cal.MaxSupportedDateTime
				}));
			}
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x0006A878 File Offset: 0x00069878
		public DateTime AddMonths(DateTime time, int months)
		{
			this.CheckTicksRange(time.Ticks);
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
			int[] array = (num % 4 == 0 && (num % 100 != 0 || num % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365;
			int num5 = array[num2] - array[num2 - 1];
			if (num3 > num5)
			{
				num3 = num5;
			}
			long ticks = GregorianCalendarHelper.DateToTicks(num, num2, num3) + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(ticks, this.m_Cal.MinSupportedDateTime, this.m_Cal.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x0006A9B9 File Offset: 0x000699B9
		public DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x0006A9C6 File Offset: 0x000699C6
		public int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x0006A9D6 File Offset: 0x000699D6
		public DayOfWeek GetDayOfWeek(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return (DayOfWeek)((time.Ticks / 864000000000L + 1L) % 7L);
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x0006A9FD File Offset: 0x000699FD
		public int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x0006AA10 File Offset: 0x00069A10
		public int GetDaysInMonth(int year, int month, int era)
		{
			year = this.GetGregorianYear(year, era);
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
			int[] array = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365;
			return array[month] - array[month - 1];
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x0006AA6F File Offset: 0x00069A6F
		public int GetDaysInYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			if (year % 4 != 0 || (year % 100 == 0 && year % 400 != 0))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x0006AA9C File Offset: 0x00069A9C
		public int GetEra(DateTime time)
		{
			long ticks = time.Ticks;
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return this.m_EraInfo[i].era;
				}
			}
			throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_Era"));
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x0006AAF4 File Offset: 0x00069AF4
		public int[] Eras
		{
			get
			{
				if (this.m_eras == null)
				{
					this.m_eras = new int[this.m_EraInfo.Length];
					for (int i = 0; i < this.m_EraInfo.Length; i++)
					{
						this.m_eras[i] = this.m_EraInfo[i].era;
					}
				}
				return (int[])this.m_eras.Clone();
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x0006AB54 File Offset: 0x00069B54
		public int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x0006AB64 File Offset: 0x00069B64
		public int GetMonthsInYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return 12;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x0006AB74 File Offset: 0x00069B74
		public int GetYear(DateTime time)
		{
			long ticks = time.Ticks;
			int datePart = this.GetDatePart(ticks, 0);
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return datePart - this.m_EraInfo[i].yearOffset;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_NoEra"));
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0006ABD4 File Offset: 0x00069BD4
		public int GetYear(int year, DateTime time)
		{
			long ticks = time.Ticks;
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return year - this.m_EraInfo[i].yearOffset;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_NoEra"));
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0006AC2C File Offset: 0x00069C2C
		public bool IsLeapDay(int year, int month, int day, int era)
		{
			if (day < 1 || day > this.GetDaysInMonth(year, month, era))
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					this.GetDaysInMonth(year, month, era)
				}));
			}
			return this.IsLeapYear(year, era) && (month == 2 && day == 29);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0006ACA5 File Offset: 0x00069CA5
		public int GetLeapMonth(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return 0;
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x0006ACB4 File Offset: 0x00069CB4
		public bool IsLeapMonth(int year, int month, int era)
		{
			year = this.GetGregorianYear(year, era);
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

		// Token: 0x0600260C RID: 9740 RVA: 0x0006AD0F File Offset: 0x00069D0F
		public bool IsLeapYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0006AD34 File Offset: 0x00069D34
		public DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			year = this.GetGregorianYear(year, era);
			long ticks = GregorianCalendarHelper.DateToTicks(year, month, day) + GregorianCalendarHelper.TimeToTicks(hour, minute, second, millisecond);
			this.CheckTicksRange(ticks);
			return new DateTime(ticks);
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x0006AD70 File Offset: 0x00069D70
		public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			this.CheckTicksRange(time.Ticks);
			return GregorianCalendar.GetDefaultInstance().GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x0006AD8C File Offset: 0x00069D8C
		public int ToFourDigitYear(int year, int twoDigitYearMax)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (year < 100)
			{
				int num = year % 100;
				return (twoDigitYearMax / 100 - ((num > twoDigitYearMax % 100) ? 1 : 0)) * 100 + num;
			}
			if (year < this.m_minYear || year > this.m_maxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					this.m_minYear,
					this.m_maxYear
				}));
			}
			return year;
		}

		// Token: 0x04001143 RID: 4419
		internal const long TicksPerMillisecond = 10000L;

		// Token: 0x04001144 RID: 4420
		internal const long TicksPerSecond = 10000000L;

		// Token: 0x04001145 RID: 4421
		internal const long TicksPerMinute = 600000000L;

		// Token: 0x04001146 RID: 4422
		internal const long TicksPerHour = 36000000000L;

		// Token: 0x04001147 RID: 4423
		internal const long TicksPerDay = 864000000000L;

		// Token: 0x04001148 RID: 4424
		internal const int MillisPerSecond = 1000;

		// Token: 0x04001149 RID: 4425
		internal const int MillisPerMinute = 60000;

		// Token: 0x0400114A RID: 4426
		internal const int MillisPerHour = 3600000;

		// Token: 0x0400114B RID: 4427
		internal const int MillisPerDay = 86400000;

		// Token: 0x0400114C RID: 4428
		internal const int DaysPerYear = 365;

		// Token: 0x0400114D RID: 4429
		internal const int DaysPer4Years = 1461;

		// Token: 0x0400114E RID: 4430
		internal const int DaysPer100Years = 36524;

		// Token: 0x0400114F RID: 4431
		internal const int DaysPer400Years = 146097;

		// Token: 0x04001150 RID: 4432
		internal const int DaysTo10000 = 3652059;

		// Token: 0x04001151 RID: 4433
		internal const long MaxMillis = 315537897600000L;

		// Token: 0x04001152 RID: 4434
		internal const int DatePartYear = 0;

		// Token: 0x04001153 RID: 4435
		internal const int DatePartDayOfYear = 1;

		// Token: 0x04001154 RID: 4436
		internal const int DatePartMonth = 2;

		// Token: 0x04001155 RID: 4437
		internal const int DatePartDay = 3;

		// Token: 0x04001156 RID: 4438
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

		// Token: 0x04001157 RID: 4439
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

		// Token: 0x04001158 RID: 4440
		internal int m_maxYear = 9999;

		// Token: 0x04001159 RID: 4441
		internal int m_minYear;

		// Token: 0x0400115A RID: 4442
		internal Calendar m_Cal;

		// Token: 0x0400115B RID: 4443
		internal EraInfo[] m_EraInfo;

		// Token: 0x0400115C RID: 4444
		internal int[] m_eras;

		// Token: 0x0400115D RID: 4445
		internal DateTime m_minDate;

		// Token: 0x0400115E RID: 4446
		private static int s_enforceJapaneseEraYearRanges = 0;

		// Token: 0x0400115F RID: 4447
		private static int s_formatJapaneseFirstYearAsANumber = 0;

		// Token: 0x04001160 RID: 4448
		private static int s_enforceLegacyJapaneseDateParsing = 0;
	}
}
