using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x02000387 RID: 903
	[ComVisible(true)]
	[Serializable]
	public abstract class Calendar : ICloneable
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x00058E32 File Offset: 0x00057E32
		[ComVisible(false)]
		public virtual DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002321 RID: 8993 RVA: 0x00058E39 File Offset: 0x00057E39
		[ComVisible(false)]
		public virtual DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06002323 RID: 8995 RVA: 0x00058E56 File Offset: 0x00057E56
		internal virtual int ID
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x00058E59 File Offset: 0x00057E59
		internal virtual int BaseCalendarID
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x00058E61 File Offset: 0x00057E61
		[ComVisible(false)]
		public virtual CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.Unknown;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002326 RID: 8998 RVA: 0x00058E64 File Offset: 0x00057E64
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x00058E6C File Offset: 0x00057E6C
		[ComVisible(false)]
		public virtual object Clone()
		{
			object obj = base.MemberwiseClone();
			((Calendar)obj).SetReadOnlyState(false);
			return obj;
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x00058E90 File Offset: 0x00057E90
		[ComVisible(false)]
		public static Calendar ReadOnly(Calendar calendar)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			if (calendar.IsReadOnly)
			{
				return calendar;
			}
			Calendar calendar2 = (Calendar)calendar.MemberwiseClone();
			calendar2.SetReadOnlyState(true);
			return calendar2;
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x00058EC9 File Offset: 0x00057EC9
		internal void VerifyWritable()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x00058EE3 File Offset: 0x00057EE3
		internal void SetReadOnlyState(bool readOnly)
		{
			this.m_isReadOnly = readOnly;
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600232B RID: 9003 RVA: 0x00058EEC File Offset: 0x00057EEC
		internal virtual int CurrentEraValue
		{
			get
			{
				if (this.m_currentEraValue == -1)
				{
					this.m_currentEraValue = CalendarTable.Default.ICURRENTERA(this.BaseCalendarID);
				}
				return this.m_currentEraValue;
			}
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x00058F14 File Offset: 0x00057F14
		internal static void CheckAddResult(long ticks, DateTime minValue, DateTime maxValue)
		{
			if (ticks < minValue.Ticks || ticks > maxValue.Ticks)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Argument_ResultCalendarRange"), new object[]
				{
					minValue,
					maxValue
				}));
			}
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x00058F6C File Offset: 0x00057F6C
		internal DateTime Add(DateTime time, double value, int scale)
		{
			long num = (long)(value * (double)scale + ((value >= 0.0) ? 0.5 : -0.5));
			if (num <= -315537897600000L || num >= 315537897600000L)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_AddValue"));
			}
			long ticks = time.Ticks + num * 10000L;
			Calendar.CheckAddResult(ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x00058FF6 File Offset: 0x00057FF6
		public virtual DateTime AddMilliseconds(DateTime time, double milliseconds)
		{
			return this.Add(time, milliseconds, 1);
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x00059001 File Offset: 0x00058001
		public virtual DateTime AddDays(DateTime time, int days)
		{
			return this.Add(time, (double)days, 86400000);
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x00059011 File Offset: 0x00058011
		public virtual DateTime AddHours(DateTime time, int hours)
		{
			return this.Add(time, (double)hours, 3600000);
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x00059021 File Offset: 0x00058021
		public virtual DateTime AddMinutes(DateTime time, int minutes)
		{
			return this.Add(time, (double)minutes, 60000);
		}

		// Token: 0x06002332 RID: 9010
		public abstract DateTime AddMonths(DateTime time, int months);

		// Token: 0x06002333 RID: 9011 RVA: 0x00059031 File Offset: 0x00058031
		public virtual DateTime AddSeconds(DateTime time, int seconds)
		{
			return this.Add(time, (double)seconds, 1000);
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x00059041 File Offset: 0x00058041
		public virtual DateTime AddWeeks(DateTime time, int weeks)
		{
			return this.AddDays(time, weeks * 7);
		}

		// Token: 0x06002335 RID: 9013
		public abstract DateTime AddYears(DateTime time, int years);

		// Token: 0x06002336 RID: 9014
		public abstract int GetDayOfMonth(DateTime time);

		// Token: 0x06002337 RID: 9015
		public abstract DayOfWeek GetDayOfWeek(DateTime time);

		// Token: 0x06002338 RID: 9016
		public abstract int GetDayOfYear(DateTime time);

		// Token: 0x06002339 RID: 9017 RVA: 0x0005904D File Offset: 0x0005804D
		public virtual int GetDaysInMonth(int year, int month)
		{
			return this.GetDaysInMonth(year, month, 0);
		}

		// Token: 0x0600233A RID: 9018
		public abstract int GetDaysInMonth(int year, int month, int era);

		// Token: 0x0600233B RID: 9019 RVA: 0x00059058 File Offset: 0x00058058
		public virtual int GetDaysInYear(int year)
		{
			return this.GetDaysInYear(year, 0);
		}

		// Token: 0x0600233C RID: 9020
		public abstract int GetDaysInYear(int year, int era);

		// Token: 0x0600233D RID: 9021
		public abstract int GetEra(DateTime time);

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600233E RID: 9022
		public abstract int[] Eras { get; }

		// Token: 0x0600233F RID: 9023 RVA: 0x00059062 File Offset: 0x00058062
		public virtual int GetHour(DateTime time)
		{
			return (int)(time.Ticks / 36000000000L % 24L);
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0005907A File Offset: 0x0005807A
		public virtual double GetMilliseconds(DateTime time)
		{
			return (double)(time.Ticks / 10000L % 1000L);
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x00059092 File Offset: 0x00058092
		public virtual int GetMinute(DateTime time)
		{
			return (int)(time.Ticks / 600000000L % 60L);
		}

		// Token: 0x06002342 RID: 9026
		public abstract int GetMonth(DateTime time);

		// Token: 0x06002343 RID: 9027 RVA: 0x000590A7 File Offset: 0x000580A7
		public virtual int GetMonthsInYear(int year)
		{
			return this.GetMonthsInYear(year, 0);
		}

		// Token: 0x06002344 RID: 9028
		public abstract int GetMonthsInYear(int year, int era);

		// Token: 0x06002345 RID: 9029 RVA: 0x000590B1 File Offset: 0x000580B1
		public virtual int GetSecond(DateTime time)
		{
			return (int)(time.Ticks / 10000000L % 60L);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000590C8 File Offset: 0x000580C8
		internal int GetFirstDayWeekOfYear(DateTime time, int firstDayOfWeek)
		{
			int num = this.GetDayOfYear(time) - 1;
			int num2 = this.GetDayOfWeek(time) - (DayOfWeek)(num % 7);
			int num3 = (num2 - firstDayOfWeek + 14) % 7;
			return (num + num3) / 7 + 1;
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000590FC File Offset: 0x000580FC
		internal int GetWeekOfYearFullDays(DateTime time, CalendarWeekRule rule, int firstDayOfWeek, int fullDays)
		{
			int num = this.GetDayOfYear(time) - 1;
			int num2 = this.GetDayOfWeek(time) - (DayOfWeek)(num % 7);
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
			return this.GetWeekOfYearFullDays(time.AddDays((double)(-(double)(num + 1))), rule, firstDayOfWeek, fullDays);
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x00059158 File Offset: 0x00058158
		public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
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
				return this.GetFirstDayWeekOfYear(time, (int)firstDayOfWeek);
			case CalendarWeekRule.FirstFullWeek:
				return this.GetWeekOfYearFullDays(time, rule, (int)firstDayOfWeek, 7);
			case CalendarWeekRule.FirstFourDayWeek:
				return this.GetWeekOfYearFullDays(time, rule, (int)firstDayOfWeek, 4);
			default:
				throw new ArgumentOutOfRangeException("rule", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					CalendarWeekRule.FirstDay,
					CalendarWeekRule.FirstFourDayWeek
				}));
			}
		}

		// Token: 0x06002349 RID: 9033
		public abstract int GetYear(DateTime time);

		// Token: 0x0600234A RID: 9034 RVA: 0x00059213 File Offset: 0x00058213
		public virtual bool IsLeapDay(int year, int month, int day)
		{
			return this.IsLeapDay(year, month, day, 0);
		}

		// Token: 0x0600234B RID: 9035
		public abstract bool IsLeapDay(int year, int month, int day, int era);

		// Token: 0x0600234C RID: 9036 RVA: 0x0005921F File Offset: 0x0005821F
		public virtual bool IsLeapMonth(int year, int month)
		{
			return this.IsLeapMonth(year, month, 0);
		}

		// Token: 0x0600234D RID: 9037
		public abstract bool IsLeapMonth(int year, int month, int era);

		// Token: 0x0600234E RID: 9038 RVA: 0x0005922A File Offset: 0x0005822A
		[ComVisible(false)]
		public virtual int GetLeapMonth(int year)
		{
			return this.GetLeapMonth(year, 0);
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x00059234 File Offset: 0x00058234
		[ComVisible(false)]
		public virtual int GetLeapMonth(int year, int era)
		{
			if (!this.IsLeapYear(year, era))
			{
				return 0;
			}
			int monthsInYear = this.GetMonthsInYear(year, era);
			for (int i = 1; i <= monthsInYear; i++)
			{
				if (this.IsLeapMonth(year, i, era))
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x00059270 File Offset: 0x00058270
		public virtual bool IsLeapYear(int year)
		{
			return this.IsLeapYear(year, 0);
		}

		// Token: 0x06002351 RID: 9041
		public abstract bool IsLeapYear(int year, int era);

		// Token: 0x06002352 RID: 9042 RVA: 0x0005927C File Offset: 0x0005827C
		public virtual DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			return this.ToDateTime(year, month, day, hour, minute, second, millisecond, 0);
		}

		// Token: 0x06002353 RID: 9043
		public abstract DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era);

		// Token: 0x06002354 RID: 9044 RVA: 0x0005929C File Offset: 0x0005829C
		internal virtual bool TryToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era, out DateTime result)
		{
			result = DateTime.MinValue;
			bool result2;
			try
			{
				result = this.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
				result2 = true;
			}
			catch (ArgumentException)
			{
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000592EC File Offset: 0x000582EC
		internal virtual bool IsValidYear(int year, int era)
		{
			return year >= this.GetYear(this.MinSupportedDateTime) && year <= this.GetYear(this.MaxSupportedDateTime);
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x00059311 File Offset: 0x00058311
		internal virtual bool IsValidMonth(int year, int month, int era)
		{
			return this.IsValidYear(year, era) && month >= 1 && month <= this.GetMonthsInYear(year, era);
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x00059331 File Offset: 0x00058331
		internal virtual bool IsValidDay(int year, int month, int day, int era)
		{
			return this.IsValidMonth(year, month, era) && day >= 1 && day <= this.GetDaysInMonth(year, month, era);
		}

		// Token: 0x06002358 RID: 9048
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeGetTwoDigitYearMax(int calID);

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x00059355 File Offset: 0x00058355
		// (set) Token: 0x0600235A RID: 9050 RVA: 0x0005935D File Offset: 0x0005835D
		public virtual int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				this.VerifyWritable();
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0005936C File Offset: 0x0005836C
		public virtual int ToFourDigitYear(int year)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (year < 100)
			{
				return (this.TwoDigitYearMax / 100 - ((year > this.TwoDigitYearMax % 100) ? 1 : 0)) * 100 + year;
			}
			return year;
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x000593B8 File Offset: 0x000583B8
		internal static long TimeToTicks(int hour, int minute, int second, int millisecond)
		{
			if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60 || second < 0 || second >= 60)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					0,
					999
				}));
			}
			return TimeSpan.TimeToTicks(hour, minute, second) + (long)millisecond * 10000L;
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0005944C File Offset: 0x0005844C
		internal static int GetSystemTwoDigitYearSetting(int CalID, int defaultYearValue)
		{
			int num = Calendar.nativeGetTwoDigitYearMax(CalID);
			if (num < 0)
			{
				RegistryKey registryKey = null;
				try
				{
					registryKey = Registry.CurrentUser.InternalOpenSubKey("Control Panel\\International\\Calendars\\TwoDigitYearMax", false);
				}
				catch (ObjectDisposedException)
				{
				}
				catch (ArgumentException)
				{
				}
				if (registryKey != null)
				{
					try
					{
						object obj = registryKey.InternalGetValue(CalID.ToString(CultureInfo.InvariantCulture), null, false, false);
						if (obj != null)
						{
							try
							{
								num = int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
							}
							catch (ArgumentException)
							{
							}
							catch (FormatException)
							{
							}
							catch (OverflowException)
							{
							}
						}
					}
					finally
					{
						registryKey.Close();
					}
				}
				if (num < 0)
				{
					num = defaultYearValue;
				}
			}
			return num;
		}

		// Token: 0x04000EE0 RID: 3808
		internal const long TicksPerMillisecond = 10000L;

		// Token: 0x04000EE1 RID: 3809
		internal const long TicksPerSecond = 10000000L;

		// Token: 0x04000EE2 RID: 3810
		internal const long TicksPerMinute = 600000000L;

		// Token: 0x04000EE3 RID: 3811
		internal const long TicksPerHour = 36000000000L;

		// Token: 0x04000EE4 RID: 3812
		internal const long TicksPerDay = 864000000000L;

		// Token: 0x04000EE5 RID: 3813
		internal const int MillisPerSecond = 1000;

		// Token: 0x04000EE6 RID: 3814
		internal const int MillisPerMinute = 60000;

		// Token: 0x04000EE7 RID: 3815
		internal const int MillisPerHour = 3600000;

		// Token: 0x04000EE8 RID: 3816
		internal const int MillisPerDay = 86400000;

		// Token: 0x04000EE9 RID: 3817
		internal const int DaysPerYear = 365;

		// Token: 0x04000EEA RID: 3818
		internal const int DaysPer4Years = 1461;

		// Token: 0x04000EEB RID: 3819
		internal const int DaysPer100Years = 36524;

		// Token: 0x04000EEC RID: 3820
		internal const int DaysPer400Years = 146097;

		// Token: 0x04000EED RID: 3821
		internal const int DaysTo10000 = 3652059;

		// Token: 0x04000EEE RID: 3822
		internal const long MaxMillis = 315537897600000L;

		// Token: 0x04000EEF RID: 3823
		internal const int CAL_GREGORIAN = 1;

		// Token: 0x04000EF0 RID: 3824
		internal const int CAL_GREGORIAN_US = 2;

		// Token: 0x04000EF1 RID: 3825
		internal const int CAL_JAPAN = 3;

		// Token: 0x04000EF2 RID: 3826
		internal const int CAL_TAIWAN = 4;

		// Token: 0x04000EF3 RID: 3827
		internal const int CAL_KOREA = 5;

		// Token: 0x04000EF4 RID: 3828
		internal const int CAL_HIJRI = 6;

		// Token: 0x04000EF5 RID: 3829
		internal const int CAL_THAI = 7;

		// Token: 0x04000EF6 RID: 3830
		internal const int CAL_HEBREW = 8;

		// Token: 0x04000EF7 RID: 3831
		internal const int CAL_GREGORIAN_ME_FRENCH = 9;

		// Token: 0x04000EF8 RID: 3832
		internal const int CAL_GREGORIAN_ARABIC = 10;

		// Token: 0x04000EF9 RID: 3833
		internal const int CAL_GREGORIAN_XLIT_ENGLISH = 11;

		// Token: 0x04000EFA RID: 3834
		internal const int CAL_GREGORIAN_XLIT_FRENCH = 12;

		// Token: 0x04000EFB RID: 3835
		internal const int CAL_JULIAN = 13;

		// Token: 0x04000EFC RID: 3836
		internal const int CAL_JAPANESELUNISOLAR = 14;

		// Token: 0x04000EFD RID: 3837
		internal const int CAL_CHINESELUNISOLAR = 15;

		// Token: 0x04000EFE RID: 3838
		internal const int CAL_SAKA = 16;

		// Token: 0x04000EFF RID: 3839
		internal const int CAL_LUNAR_ETO_CHN = 17;

		// Token: 0x04000F00 RID: 3840
		internal const int CAL_LUNAR_ETO_KOR = 18;

		// Token: 0x04000F01 RID: 3841
		internal const int CAL_LUNAR_ETO_ROKUYOU = 19;

		// Token: 0x04000F02 RID: 3842
		internal const int CAL_KOREANLUNISOLAR = 20;

		// Token: 0x04000F03 RID: 3843
		internal const int CAL_TAIWANLUNISOLAR = 21;

		// Token: 0x04000F04 RID: 3844
		internal const int CAL_PERSIAN = 22;

		// Token: 0x04000F05 RID: 3845
		internal const int CAL_UMALQURA = 23;

		// Token: 0x04000F06 RID: 3846
		public const int CurrentEra = 0;

		// Token: 0x04000F07 RID: 3847
		private const string TwoDigitYearMaxSubKey = "Control Panel\\International\\Calendars\\TwoDigitYearMax";

		// Token: 0x04000F08 RID: 3848
		internal int m_currentEraValue = -1;

		// Token: 0x04000F09 RID: 3849
		[OptionalField(VersionAdded = 2)]
		private bool m_isReadOnly;

		// Token: 0x04000F0A RID: 3850
		internal int twoDigitYearMax = -1;
	}
}
