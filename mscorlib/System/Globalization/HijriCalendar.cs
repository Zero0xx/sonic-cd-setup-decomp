using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x020003BC RID: 956
	[ComVisible(true)]
	[Serializable]
	public class HijriCalendar : Calendar
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002637 RID: 9783 RVA: 0x0006CFF8 File Offset: 0x0006BFF8
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return HijriCalendar.calendarMinValue;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x0006CFFF File Offset: 0x0006BFFF
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return HijriCalendar.calendarMaxValue;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x0006D006 File Offset: 0x0006C006
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.LunarCalendar;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x0600263B RID: 9787 RVA: 0x0006D01C File Offset: 0x0006C01C
		internal override int ID
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x0006D01F File Offset: 0x0006C01F
		private long GetAbsoluteDateHijri(int y, int m, int d)
		{
			return this.DaysUpToHijriYear(y) + (long)HijriCalendar.HijriMonthDays[m - 1] + (long)d - 1L - (long)this.HijriAdjustment;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x0006D044 File Offset: 0x0006C044
		private long DaysUpToHijriYear(int HijriYear)
		{
			int num = (HijriYear - 1) / 30 * 30;
			int i = HijriYear - num - 1;
			long num2 = (long)num * 10631L / 30L + 227013L;
			while (i > 0)
			{
				num2 += (long)(354 + (this.IsLeapYear(i, 0) ? 1 : 0));
				i--;
			}
			return num2;
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600263E RID: 9790 RVA: 0x0006D099 File Offset: 0x0006C099
		// (set) Token: 0x0600263F RID: 9791 RVA: 0x0006D0BC File Offset: 0x0006C0BC
		public int HijriAdjustment
		{
			get
			{
				if (this.m_HijriAdvance == -2147483648)
				{
					this.m_HijriAdvance = this.GetAdvanceHijriDate();
				}
				return this.m_HijriAdvance;
			}
			set
			{
				base.VerifyWritable();
				if (value < -2 || value > 2)
				{
					throw new ArgumentOutOfRangeException("HijriAdjustment", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[]
					{
						-2,
						2
					}));
				}
				this.m_HijriAdvance = value;
			}
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x0006D11C File Offset: 0x0006C11C
		private int GetAdvanceHijriDate()
		{
			int result = 0;
			RegistryKey registryKey = null;
			try
			{
				registryKey = Registry.CurrentUser.InternalOpenSubKey(HijriCalendar.m_InternationalRegKey, false);
			}
			catch (ObjectDisposedException)
			{
				return 0;
			}
			catch (ArgumentException)
			{
				return 0;
			}
			if (registryKey != null)
			{
				try
				{
					object obj = registryKey.InternalGetValue(HijriCalendar.m_HijriAdvanceRegKeyEntry, null, false, false);
					if (obj == null)
					{
						return 0;
					}
					string text = obj.ToString();
					if (string.Compare(text, 0, HijriCalendar.m_HijriAdvanceRegKeyEntry, 0, HijriCalendar.m_HijriAdvanceRegKeyEntry.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						if (text.Length == HijriCalendar.m_HijriAdvanceRegKeyEntry.Length)
						{
							result = -1;
						}
						else
						{
							text = text.Substring(HijriCalendar.m_HijriAdvanceRegKeyEntry.Length);
							try
							{
								int num = int.Parse(text.ToString(), CultureInfo.InvariantCulture);
								if (num >= -2 && num <= 2)
								{
									result = num;
								}
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
				}
				finally
				{
					registryKey.Close();
				}
			}
			return result;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0006D23C File Offset: 0x0006C23C
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < HijriCalendar.calendarMinValue.Ticks || ticks > HijriCalendar.calendarMaxValue.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					HijriCalendar.calendarMinValue,
					HijriCalendar.calendarMaxValue
				}));
			}
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x0006D2AA File Offset: 0x0006C2AA
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != HijriCalendar.HijriEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x0006D2CC File Offset: 0x0006C2CC
		internal void CheckYearRange(int year, int era)
		{
			this.CheckEraRange(era);
			if (year < 1 || year > 9666)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9666
				}));
			}
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x0006D32C File Offset: 0x0006C32C
		internal void CheckYearMonthRange(int year, int month, int era)
		{
			this.CheckYearRange(year, era);
			if (year == 9666 && month > 4)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					4
				}));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x0006D3A4 File Offset: 0x0006C3A4
		internal virtual int GetDatePart(long ticks, int part)
		{
			this.CheckTicksRange(ticks);
			long num = ticks / 864000000000L + 1L;
			num += (long)this.HijriAdjustment;
			int num2 = (int)((num - 227013L) * 30L / 10631L) + 1;
			long num3 = this.DaysUpToHijriYear(num2);
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
			int num5 = 1;
			num -= num3;
			if (part == 1)
			{
				return (int)num;
			}
			while (num5 <= 12 && num > (long)HijriCalendar.HijriMonthDays[num5 - 1])
			{
				num5++;
			}
			num5--;
			if (part == 2)
			{
				return num5;
			}
			int result = (int)(num - (long)HijriCalendar.HijriMonthDays[num5 - 1]);
			if (part == 3)
			{
				return result;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DateTimeParsing"));
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x0006D490 File Offset: 0x0006C490
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
			long ticks = this.GetAbsoluteDateHijri(num, num2, num3) * 864000000000L + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x0006D5A0 File Offset: 0x0006C5A0
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x0006D5AD File Offset: 0x0006C5AD
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x0006D5BD File Offset: 0x0006C5BD
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x0006D5D6 File Offset: 0x0006C5D6
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x0006D5E6 File Offset: 0x0006C5E6
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
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
				if (month % 2 != 1)
				{
					return 29;
				}
				return 30;
			}
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x0006D611 File Offset: 0x0006C611
		public override int GetDaysInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			if (!this.IsLeapYear(year, 0))
			{
				return 354;
			}
			return 355;
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x0006D630 File Offset: 0x0006C630
		public override int GetEra(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return HijriCalendar.HijriEra;
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600264E RID: 9806 RVA: 0x0006D644 File Offset: 0x0006C644
		public override int[] Eras
		{
			get
			{
				return new int[]
				{
					HijriCalendar.HijriEra
				};
			}
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x0006D661 File Offset: 0x0006C661
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x0006D671 File Offset: 0x0006C671
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 12;
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x0006D67D File Offset: 0x0006C67D
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x0006D690 File Offset: 0x0006C690
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

		// Token: 0x06002653 RID: 9811 RVA: 0x0006D700 File Offset: 0x0006C700
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 0;
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x0006D70B File Offset: 0x0006C70B
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			return false;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x0006D717 File Offset: 0x0006C717
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return (year * 11 + 14) % 30 < 11;
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x0006D730 File Offset: 0x0006C730
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
			long absoluteDateHijri = this.GetAbsoluteDateHijri(year, month, day);
			if (absoluteDateHijri >= 0L)
			{
				return new DateTime(absoluteDateHijri * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x0006D7C7 File Offset: 0x0006C7C7
		// (set) Token: 0x06002658 RID: 9816 RVA: 0x0006D7F0 File Offset: 0x0006C7F0
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
				if (value < 99 || value > 9666)
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						99,
						9666
					}));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x0006D858 File Offset: 0x0006C858
		public override int ToFourDigitYear(int year)
		{
			if (year < 100)
			{
				return base.ToFourDigitYear(year);
			}
			if (year > 9666)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9666
				}));
			}
			return year;
		}

		// Token: 0x04001175 RID: 4469
		internal const int DatePartYear = 0;

		// Token: 0x04001176 RID: 4470
		internal const int DatePartDayOfYear = 1;

		// Token: 0x04001177 RID: 4471
		internal const int DatePartMonth = 2;

		// Token: 0x04001178 RID: 4472
		internal const int DatePartDay = 3;

		// Token: 0x04001179 RID: 4473
		internal const int MinAdvancedHijri = -2;

		// Token: 0x0400117A RID: 4474
		internal const int MaxAdvancedHijri = 2;

		// Token: 0x0400117B RID: 4475
		internal const int MaxCalendarYear = 9666;

		// Token: 0x0400117C RID: 4476
		internal const int MaxCalendarMonth = 4;

		// Token: 0x0400117D RID: 4477
		internal const int MaxCalendarDay = 3;

		// Token: 0x0400117E RID: 4478
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 1451;

		// Token: 0x0400117F RID: 4479
		public static readonly int HijriEra = 1;

		// Token: 0x04001180 RID: 4480
		internal static readonly int[] HijriMonthDays = new int[]
		{
			0,
			30,
			59,
			89,
			118,
			148,
			177,
			207,
			236,
			266,
			295,
			325,
			355
		};

		// Token: 0x04001181 RID: 4481
		private static string m_InternationalRegKey = "Control Panel\\International";

		// Token: 0x04001182 RID: 4482
		private static string m_HijriAdvanceRegKeyEntry = "AddHijriDate";

		// Token: 0x04001183 RID: 4483
		private int m_HijriAdvance = int.MinValue;

		// Token: 0x04001184 RID: 4484
		internal static readonly DateTime calendarMinValue = new DateTime(622, 7, 18);

		// Token: 0x04001185 RID: 4485
		internal static readonly DateTime calendarMaxValue = DateTime.MaxValue;
	}
}
