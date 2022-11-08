﻿using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct DateTime : IComparable, IFormattable, IConvertible, ISerializable, IComparable<DateTime>, IEquatable<DateTime>
	{
		// Token: 0x0600029B RID: 667 RVA: 0x0000B695 File Offset: 0x0000A695
		public DateTime(long ticks)
		{
			if (ticks < 0L || ticks > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("ticks", Environment.GetResourceString("ArgumentOutOfRange_DateTimeBadTicks"));
			}
			this.dateData = (ulong)ticks;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000B6C4 File Offset: 0x0000A6C4
		private DateTime(ulong dateData)
		{
			this.dateData = dateData;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000B6D0 File Offset: 0x0000A6D0
		public DateTime(long ticks, DateTimeKind kind)
		{
			if (ticks < 0L || ticks > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("ticks", Environment.GetResourceString("ArgumentOutOfRange_DateTimeBadTicks"));
			}
			if (kind < DateTimeKind.Unspecified || kind > DateTimeKind.Local)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDateTimeKind"), "kind");
			}
			this.dateData = (ulong)(ticks | (long)kind << 62);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000B730 File Offset: 0x0000A730
		internal DateTime(long ticks, DateTimeKind kind, bool isAmbiguousDst)
		{
			if (ticks < 0L || ticks > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("ticks", Environment.GetResourceString("ArgumentOutOfRange_DateTimeBadTicks"));
			}
			this.dateData = (ulong)(ticks | (isAmbiguousDst ? -4611686018427387904L : long.MinValue));
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000B782 File Offset: 0x0000A782
		public DateTime(int year, int month, int day)
		{
			this.dateData = (ulong)DateTime.DateToTicks(year, month, day);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000B792 File Offset: 0x0000A792
		public DateTime(int year, int month, int day, Calendar calendar)
		{
			this = new DateTime(year, month, day, 0, 0, 0, calendar);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000B7A2 File Offset: 0x0000A7A2
		public DateTime(int year, int month, int day, int hour, int minute, int second)
		{
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem && DateTime.IsValidTimeWithLeapSeconds(year, month, day, hour, minute, second, DateTimeKind.Unspecified))
			{
				second = 59;
			}
			this.dateData = (ulong)(DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second));
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000B7E0 File Offset: 0x0000A7E0
		public DateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
		{
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem && DateTime.IsValidTimeWithLeapSeconds(year, month, day, hour, minute, second, kind))
			{
				second = 59;
			}
			long num = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
			if (kind < DateTimeKind.Unspecified || kind > DateTimeKind.Local)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDateTimeKind"), "kind");
			}
			this.dateData = (ulong)(num | (long)kind << 62);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B854 File Offset: 0x0000A854
		public DateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			this.dateData = (ulong)calendar.ToDateTime(year, month, day, hour, minute, second, 0).Ticks;
			if (num == 60)
			{
				DateTime dateTime = new DateTime(this.dateData);
				if (!DateTime.IsValidTimeWithLeapSeconds(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 60, DateTimeKind.Unspecified))
				{
					throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
				}
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000B8F4 File Offset: 0x0000A8F4
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem && DateTime.IsValidTimeWithLeapSeconds(year, month, day, hour, minute, second, DateTimeKind.Unspecified))
			{
				second = 59;
			}
			long num = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					0,
					999
				}));
			}
			num += (long)millisecond * 10000L;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DateTimeRange"));
			}
			this.dateData = (ulong)num;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000B9B8 File Offset: 0x0000A9B8
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
		{
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem && DateTime.IsValidTimeWithLeapSeconds(year, month, day, hour, minute, second, kind))
			{
				second = 59;
			}
			long num = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					0,
					999
				}));
			}
			num += (long)millisecond * 10000L;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DateTimeRange"));
			}
			if (kind < DateTimeKind.Unspecified || kind > DateTimeKind.Local)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDateTimeKind"), "kind");
			}
			this.dateData = (ulong)(num | (long)kind << 62);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000BAA4 File Offset: 0x0000AAA4
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			long num2 = calendar.ToDateTime(year, month, day, hour, minute, second, 0).Ticks;
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					0,
					999
				}));
			}
			num2 += (long)millisecond * 10000L;
			if (num2 < 0L || num2 > 3155378975999999999L)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DateTimeRange"));
			}
			this.dateData = (ulong)num2;
			if (num == 60)
			{
				DateTime dateTime = new DateTime(this.dateData);
				if (!DateTime.IsValidTimeWithLeapSeconds(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 60, DateTimeKind.Unspecified))
				{
					throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
				}
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000BBC4 File Offset: 0x0000ABC4
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			long num2 = calendar.ToDateTime(year, month, day, hour, minute, second, 0).Ticks;
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					0,
					999
				}));
			}
			num2 += (long)millisecond * 10000L;
			if (num2 < 0L || num2 > 3155378975999999999L)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DateTimeRange"));
			}
			if (kind < DateTimeKind.Unspecified || kind > DateTimeKind.Local)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDateTimeKind"), "kind");
			}
			this.dateData = (ulong)(num2 | (long)kind << 62);
			if (num == 60)
			{
				DateTime dateTime = new DateTime(this.dateData);
				if (!DateTime.IsValidTimeWithLeapSeconds(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 60, kind))
				{
					throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
				}
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000BD0C File Offset: 0x0000AD0C
		private DateTime(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			bool flag = false;
			bool flag2 = false;
			long num = 0L;
			ulong num2 = 0UL;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				if ((name = enumerator.Name) != null)
				{
					if (!(name == "ticks"))
					{
						if (name == "dateData")
						{
							num2 = Convert.ToUInt64(enumerator.Value, CultureInfo.InvariantCulture);
							flag2 = true;
						}
					}
					else
					{
						num = Convert.ToInt64(enumerator.Value, CultureInfo.InvariantCulture);
						flag = true;
					}
				}
			}
			if (flag2)
			{
				this.dateData = num2;
			}
			else
			{
				if (!flag)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_MissingDateTimeData"));
				}
				this.dateData = (ulong)num;
			}
			long internalTicks = this.InternalTicks;
			if (internalTicks < 0L || internalTicks > 3155378975999999999L)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_DateTimeTicksOutOfRange"));
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000BDEB File Offset: 0x0000ADEB
		private long InternalTicks
		{
			get
			{
				return (long)(this.dateData & 4611686018427387903UL);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000BDFD File Offset: 0x0000ADFD
		private ulong InternalKind
		{
			get
			{
				return this.dateData & 13835058055282163712UL;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000BE0F File Offset: 0x0000AE0F
		public DateTime Add(TimeSpan value)
		{
			return this.AddTicks(value._ticks);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000BE20 File Offset: 0x0000AE20
		private DateTime Add(double value, int scale)
		{
			long num = (long)(value * (double)scale + ((value >= 0.0) ? 0.5 : -0.5));
			if (num <= -315537897600000L || num >= 315537897600000L)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_AddValue"));
			}
			return this.AddTicks(num * 10000L);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000BE8F File Offset: 0x0000AE8F
		public DateTime AddDays(double value)
		{
			return this.Add(value, 86400000);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000BE9D File Offset: 0x0000AE9D
		public DateTime AddHours(double value)
		{
			return this.Add(value, 3600000);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000BEAB File Offset: 0x0000AEAB
		public DateTime AddMilliseconds(double value)
		{
			return this.Add(value, 1);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000BEB5 File Offset: 0x0000AEB5
		public DateTime AddMinutes(double value)
		{
			return this.Add(value, 60000);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000BEC4 File Offset: 0x0000AEC4
		public DateTime AddMonths(int months)
		{
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", Environment.GetResourceString("ArgumentOutOfRange_DateTimeBadMonths"));
			}
			int num = this.GetDatePart(0);
			int num2 = this.GetDatePart(2);
			int num3 = this.GetDatePart(3);
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
			if (num < 1 || num > 9999)
			{
				throw new ArgumentOutOfRangeException("months", Environment.GetResourceString("ArgumentOutOfRange_DateArithmetic"));
			}
			int num5 = DateTime.DaysInMonth(num, num2);
			if (num3 > num5)
			{
				num3 = num5;
			}
			return new DateTime((ulong)(DateTime.DateToTicks(num, num2, num3) + this.InternalTicks % 864000000000L | (long)this.InternalKind));
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000BF93 File Offset: 0x0000AF93
		public DateTime AddSeconds(double value)
		{
			return this.Add(value, 1000);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000BFA4 File Offset: 0x0000AFA4
		public DateTime AddTicks(long value)
		{
			long internalTicks = this.InternalTicks;
			if (value > 3155378975999999999L - internalTicks || value < -internalTicks)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_DateArithmetic"));
			}
			return new DateTime((ulong)(internalTicks + value | (long)this.InternalKind));
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000BFEF File Offset: 0x0000AFEF
		public DateTime AddYears(int value)
		{
			if (value < -10000 || value > 10000)
			{
				throw new ArgumentOutOfRangeException("years", Environment.GetResourceString("ArgumentOutOfRange_DateTimeBadYears"));
			}
			return this.AddMonths(value * 12);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000C020 File Offset: 0x0000B020
		public static int Compare(DateTime t1, DateTime t2)
		{
			long internalTicks = t1.InternalTicks;
			long internalTicks2 = t2.InternalTicks;
			if (internalTicks > internalTicks2)
			{
				return 1;
			}
			if (internalTicks < internalTicks2)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000C04C File Offset: 0x0000B04C
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is DateTime))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDateTime"));
			}
			long internalTicks = ((DateTime)value).InternalTicks;
			long internalTicks2 = this.InternalTicks;
			if (internalTicks2 > internalTicks)
			{
				return 1;
			}
			if (internalTicks2 < internalTicks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000C09C File Offset: 0x0000B09C
		public int CompareTo(DateTime value)
		{
			long internalTicks = value.InternalTicks;
			long internalTicks2 = this.InternalTicks;
			if (internalTicks2 > internalTicks)
			{
				return 1;
			}
			if (internalTicks2 < internalTicks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000C0C8 File Offset: 0x0000B0C8
		private static long DateToTicks(int year, int month, int day)
		{
			if (year >= 1 && year <= 9999 && month >= 1 && month <= 12)
			{
				int[] array = DateTime.IsLeapYear(year) ? DateTime.DaysToMonth366 : DateTime.DaysToMonth365;
				if (day >= 1 && day <= array[month] - array[month - 1])
				{
					int num = year - 1;
					int num2 = num * 365 + num / 4 - num / 100 + num / 400 + array[month - 1] + day - 1;
					return (long)num2 * 864000000000L;
				}
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000C153 File Offset: 0x0000B153
		private static long TimeToTicks(int hour, int minute, int second)
		{
			if (hour >= 0 && hour < 24 && minute >= 0 && minute < 60 && second >= 0 && second < 60)
			{
				return TimeSpan.TimeToTicks(hour, minute, second);
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000C18C File Offset: 0x0000B18C
		public static int DaysInMonth(int year, int month)
		{
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
			int[] array = DateTime.IsLeapYear(year) ? DateTime.DaysToMonth366 : DateTime.DaysToMonth365;
			return array[month] - array[month - 1];
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000C1D8 File Offset: 0x0000B1D8
		internal static long DoubleDateToTicks(double value)
		{
			if (value >= 2958466.0 || value <= -657435.0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_OleAutDateInvalid"));
			}
			long num = (long)(value * 86400000.0 + ((value >= 0.0) ? 0.5 : -0.5));
			if (num < 0L)
			{
				num -= num % 86400000L * 2L;
			}
			num += 59926435200000L;
			if (num < 0L || num >= 315537897600000L)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_OleAutDateScale"));
			}
			return num * 10000L;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000C284 File Offset: 0x0000B284
		public override bool Equals(object value)
		{
			return value is DateTime && this.InternalTicks == ((DateTime)value).InternalTicks;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000C2B1 File Offset: 0x0000B2B1
		public bool Equals(DateTime value)
		{
			return this.InternalTicks == value.InternalTicks;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000C2C2 File Offset: 0x0000B2C2
		public static bool Equals(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks == t2.InternalTicks;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000C2D4 File Offset: 0x0000B2D4
		public static DateTime FromBinary(long dateData)
		{
			if ((dateData & -9223372036854775808L) == 0L)
			{
				return DateTime.FromBinaryRaw(dateData);
			}
			long num = dateData & 4611686018427387903L;
			if (num > 4611685154427387904L)
			{
				num -= 4611686018427387904L;
			}
			bool isAmbiguousDst = false;
			long num2;
			if (num < 0L)
			{
				num2 = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.MinValue).Ticks;
			}
			else if (num > 3155378975999999999L)
			{
				num2 = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.MaxValue).Ticks;
			}
			else
			{
				CurrentSystemTimeZone currentSystemTimeZone = (CurrentSystemTimeZone)TimeZone.CurrentTimeZone;
				num2 = currentSystemTimeZone.GetUtcOffsetFromUniversalTime(new DateTime(num), ref isAmbiguousDst);
			}
			num += num2;
			if (num < 0L)
			{
				num += 864000000000L;
			}
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DateTimeBadBinaryData"), "dateData");
			}
			return new DateTime(num, DateTimeKind.Local, isAmbiguousDst);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000C3C4 File Offset: 0x0000B3C4
		internal static DateTime FromBinaryRaw(long dateData)
		{
			long num = dateData & 4611686018427387903L;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DateTimeBadBinaryData"), "dateData");
			}
			return new DateTime((ulong)dateData);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000C40C File Offset: 0x0000B40C
		public static DateTime FromFileTime(long fileTime)
		{
			return DateTime.FromFileTimeUtc(fileTime).ToLocalTime();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000C428 File Offset: 0x0000B428
		public static DateTime FromFileTimeUtc(long fileTime)
		{
			if (fileTime < 0L || fileTime > 2650467743999999999L)
			{
				throw new ArgumentOutOfRangeException("fileTime", Environment.GetResourceString("ArgumentOutOfRange_FileTimeInvalid"));
			}
			if (DateTime.s_isLeapSecondsSupportedSystem)
			{
				return DateTime.InternalFromFileTime(fileTime);
			}
			long ticks = fileTime + 504911232000000000L;
			return new DateTime(ticks, DateTimeKind.Utc);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000C47C File Offset: 0x0000B47C
		public static DateTime FromOADate(double d)
		{
			return new DateTime(DateTime.DoubleDateToTicks(d), DateTimeKind.Unspecified);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000C48A File Offset: 0x0000B48A
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("ticks", this.InternalTicks);
			info.AddValue("dateData", this.dateData);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C4BC File Offset: 0x0000B4BC
		public bool IsDaylightSavingTime()
		{
			return TimeZone.CurrentTimeZone.IsDaylightSavingTime(this);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C4CE File Offset: 0x0000B4CE
		public static DateTime SpecifyKind(DateTime value, DateTimeKind kind)
		{
			return new DateTime(value.InternalTicks, kind);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C4E0 File Offset: 0x0000B4E0
		public long ToBinary()
		{
			if (this.Kind == DateTimeKind.Local)
			{
				TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(this);
				long ticks = this.Ticks;
				long num = ticks - utcOffset.Ticks;
				if (num < 0L)
				{
					num = 4611686018427387904L + num;
				}
				return num | long.MinValue;
			}
			return (long)this.dateData;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C53B File Offset: 0x0000B53B
		internal long ToBinaryRaw()
		{
			return (long)this.dateData;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000C544 File Offset: 0x0000B544
		public DateTime Date
		{
			get
			{
				long internalTicks = this.InternalTicks;
				return new DateTime((ulong)(internalTicks - internalTicks % 864000000000L | (long)this.InternalKind));
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000C574 File Offset: 0x0000B574
		private int GetDatePart(int part)
		{
			long internalTicks = this.InternalTicks;
			int i = (int)(internalTicks / 864000000000L);
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
			int[] array = (num4 == 3 && (num3 != 24 || num2 == 3)) ? DateTime.DaysToMonth366 : DateTime.DaysToMonth365;
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000C661 File Offset: 0x0000B661
		public int Day
		{
			get
			{
				return this.GetDatePart(3);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000C66A File Offset: 0x0000B66A
		public DayOfWeek DayOfWeek
		{
			get
			{
				return (DayOfWeek)((this.InternalTicks / 864000000000L + 1L) % 7L);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000C683 File Offset: 0x0000B683
		public int DayOfYear
		{
			get
			{
				return this.GetDatePart(1);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000C68C File Offset: 0x0000B68C
		public override int GetHashCode()
		{
			long internalTicks = this.InternalTicks;
			return (int)internalTicks ^ (int)(internalTicks >> 32);
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000C6A8 File Offset: 0x0000B6A8
		public int Hour
		{
			get
			{
				return (int)(this.InternalTicks / 36000000000L % 24L);
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000C6BF File Offset: 0x0000B6BF
		internal bool IsAmbiguousDaylightSavingTime()
		{
			return this.InternalKind == 13835058055282163712UL;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000C6D4 File Offset: 0x0000B6D4
		public DateTimeKind Kind
		{
			get
			{
				ulong internalKind = this.InternalKind;
				if (internalKind == 0UL)
				{
					return DateTimeKind.Unspecified;
				}
				if (internalKind != 4611686018427387904UL)
				{
					return DateTimeKind.Local;
				}
				return DateTimeKind.Utc;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000C700 File Offset: 0x0000B700
		public int Millisecond
		{
			get
			{
				return (int)(this.InternalTicks / 10000L % 1000L);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000C717 File Offset: 0x0000B717
		public int Minute
		{
			get
			{
				return (int)(this.InternalTicks / 600000000L % 60L);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000C72B File Offset: 0x0000B72B
		public int Month
		{
			get
			{
				return this.GetDatePart(2);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000C734 File Offset: 0x0000B734
		public static DateTime Now
		{
			get
			{
				return DateTime.UtcNow.ToLocalTime();
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000C750 File Offset: 0x0000B750
		public static DateTime UtcNow
		{
			get
			{
				long num;
				if (DateTime.s_isLeapSecondsSupportedSystem)
				{
					DateTime.FullSystemTime fullSystemTime = default(DateTime.FullSystemTime);
					DateTime.GetSystemTimeWithLeapSecondsHandling(ref fullSystemTime);
					num = DateTime.DateToTicks((int)fullSystemTime.wYear, (int)fullSystemTime.wMonth, (int)fullSystemTime.wDay);
					num += DateTime.TimeToTicks((int)fullSystemTime.wHour, (int)fullSystemTime.wMinute, (int)fullSystemTime.wSecond);
					num += (long)((ulong)fullSystemTime.wMillisecond * 10000UL);
					num += fullSystemTime.hundredNanoSecond;
					return new DateTime((ulong)(num | 4611686018427387904L));
				}
				num = DateTime.GetSystemTimeAsFileTime();
				return new DateTime((ulong)(num + 504911232000000000L | 4611686018427387904L));
			}
		}

		// Token: 0x060002D7 RID: 727
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern long GetSystemTimeAsFileTime();

		// Token: 0x060002D8 RID: 728
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool ValidateSystemTime(ref DateTime.FullSystemTime time, bool localTime);

		// Token: 0x060002D9 RID: 729
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetSystemTimeWithLeapSecondsHandling(ref DateTime.FullSystemTime time);

		// Token: 0x060002DA RID: 730
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsLeapSecondsSupportedSystem();

		// Token: 0x060002DB RID: 731
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SystemFileTimeToSystemTime(long fileTime, ref DateTime.FullSystemTime time);

		// Token: 0x060002DC RID: 732
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SystemTimeToSystemFileTime(ref DateTime.FullSystemTime time, ref long fileTime);

		// Token: 0x060002DD RID: 733 RVA: 0x0000C7FC File Offset: 0x0000B7FC
		internal static DateTime InternalFromFileTime(long fileTime)
		{
			DateTime.FullSystemTime fullSystemTime = default(DateTime.FullSystemTime);
			if (DateTime.SystemFileTimeToSystemTime(fileTime, ref fullSystemTime))
			{
				fullSystemTime.hundredNanoSecond = fileTime % 10000L;
				long num = DateTime.DateToTicks((int)fullSystemTime.wYear, (int)fullSystemTime.wMonth, (int)fullSystemTime.wDay);
				num += DateTime.TimeToTicks((int)fullSystemTime.wHour, (int)fullSystemTime.wMinute, (int)fullSystemTime.wSecond);
				num += (long)((ulong)fullSystemTime.wMillisecond * 10000UL);
				num += fullSystemTime.hundredNanoSecond;
				return new DateTime((ulong)(num | 4611686018427387904L));
			}
			throw new ArgumentOutOfRangeException("fileTime", Environment.GetResourceString("ArgumentOutOfRange_DateTimeBadTicks"));
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000C8A4 File Offset: 0x0000B8A4
		internal static long InternalToFileTime(long ticks)
		{
			long num = 0L;
			DateTime.FullSystemTime fullSystemTime = new DateTime.FullSystemTime(ticks);
			if (DateTime.SystemTimeToSystemFileTime(ref fullSystemTime, ref num))
			{
				return num + ticks % 10000L;
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_FileTimeInvalid"));
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000C8E8 File Offset: 0x0000B8E8
		internal static bool IsValidTimeWithLeapSeconds(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
		{
			DateTime dateTime = new DateTime(year, month, day);
			DateTime.FullSystemTime fullSystemTime = new DateTime.FullSystemTime(year, month, dateTime.DayOfWeek, day, hour, minute, second);
			switch (kind)
			{
			case DateTimeKind.Utc:
				return DateTime.ValidateSystemTime(ref fullSystemTime, false);
			case DateTimeKind.Local:
				return DateTime.ValidateSystemTime(ref fullSystemTime, true);
			default:
				return DateTime.ValidateSystemTime(ref fullSystemTime, true) || DateTime.ValidateSystemTime(ref fullSystemTime, false);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000C955 File Offset: 0x0000B955
		public int Second
		{
			get
			{
				return (int)(this.InternalTicks / 10000000L % 60L);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000C969 File Offset: 0x0000B969
		public long Ticks
		{
			get
			{
				return this.InternalTicks;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000C971 File Offset: 0x0000B971
		public TimeSpan TimeOfDay
		{
			get
			{
				return new TimeSpan(this.InternalTicks % 864000000000L);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000C988 File Offset: 0x0000B988
		public static DateTime Today
		{
			get
			{
				return DateTime.Now.Date;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000C9A2 File Offset: 0x0000B9A2
		public int Year
		{
			get
			{
				return this.GetDatePart(0);
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000C9AB File Offset: 0x0000B9AB
		public static bool IsLeapYear(int year)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_Year"));
			}
			return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000C9E7 File Offset: 0x0000B9E7
		public static DateTime Parse(string s)
		{
			return DateTimeParse.Parse(s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000C9F5 File Offset: 0x0000B9F5
		public static DateTime Parse(string s, IFormatProvider provider)
		{
			return DateTimeParse.Parse(s, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000CA04 File Offset: 0x0000BA04
		public static DateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles)
		{
			DateTimeFormatInfo.ValidateStyles(styles, "styles");
			return DateTimeParse.Parse(s, DateTimeFormatInfo.GetInstance(provider), styles);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000CA1E File Offset: 0x0000BA1E
		public static DateTime ParseExact(string s, string format, IFormatProvider provider)
		{
			return DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000CA2E File Offset: 0x0000BA2E
		public static DateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			return DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000CA49 File Offset: 0x0000BA49
		public static DateTime ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			return DateTimeParse.ParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000CA64 File Offset: 0x0000BA64
		public TimeSpan Subtract(DateTime value)
		{
			return new TimeSpan(this.InternalTicks - value.InternalTicks);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000CA7C File Offset: 0x0000BA7C
		public DateTime Subtract(TimeSpan value)
		{
			long internalTicks = this.InternalTicks;
			long ticks = value._ticks;
			if (internalTicks < ticks || internalTicks - 3155378975999999999L > ticks)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_DateArithmetic"));
			}
			return new DateTime((ulong)(internalTicks - ticks | (long)this.InternalKind));
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000CAD0 File Offset: 0x0000BAD0
		private static double TicksToOADate(long value)
		{
			if (value == 0L)
			{
				return 0.0;
			}
			if (value < 864000000000L)
			{
				value += 599264352000000000L;
			}
			if (value < 31241376000000000L)
			{
				throw new OverflowException(Environment.GetResourceString("Arg_OleAutDateInvalid"));
			}
			long num = (value - 599264352000000000L) / 10000L;
			if (num < 0L)
			{
				long num2 = num % 86400000L;
				if (num2 != 0L)
				{
					num -= (86400000L + num2) * 2L;
				}
			}
			return (double)num / 86400000.0;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000CB61 File Offset: 0x0000BB61
		public double ToOADate()
		{
			return DateTime.TicksToOADate(this.InternalTicks);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000CB70 File Offset: 0x0000BB70
		public long ToFileTime()
		{
			return this.ToUniversalTime().ToFileTimeUtc();
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000CB8C File Offset: 0x0000BB8C
		public long ToFileTimeUtc()
		{
			long num = ((this.InternalKind & 9223372036854775808UL) != 0UL) ? this.ToUniversalTime().InternalTicks : this.InternalTicks;
			if (DateTime.s_isLeapSecondsSupportedSystem)
			{
				return DateTime.InternalToFileTime(num);
			}
			num -= 504911232000000000L;
			if (num < 0L)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_FileTimeInvalid"));
			}
			return num;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000CBF5 File Offset: 0x0000BBF5
		public DateTime ToLocalTime()
		{
			return TimeZone.CurrentTimeZone.ToLocalTime(this);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000CC07 File Offset: 0x0000BC07
		public string ToLongDateString()
		{
			return DateTimeFormat.Format(this, "D", DateTimeFormatInfo.CurrentInfo);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000CC1E File Offset: 0x0000BC1E
		public string ToLongTimeString()
		{
			return DateTimeFormat.Format(this, "T", DateTimeFormatInfo.CurrentInfo);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000CC35 File Offset: 0x0000BC35
		public string ToShortDateString()
		{
			return DateTimeFormat.Format(this, "d", DateTimeFormatInfo.CurrentInfo);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000CC4C File Offset: 0x0000BC4C
		public string ToShortTimeString()
		{
			return DateTimeFormat.Format(this, "t", DateTimeFormatInfo.CurrentInfo);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000CC63 File Offset: 0x0000BC63
		public override string ToString()
		{
			return DateTimeFormat.Format(this, null, DateTimeFormatInfo.CurrentInfo);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000CC76 File Offset: 0x0000BC76
		public string ToString(string format)
		{
			return DateTimeFormat.Format(this, format, DateTimeFormatInfo.CurrentInfo);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000CC89 File Offset: 0x0000BC89
		public string ToString(IFormatProvider provider)
		{
			return DateTimeFormat.Format(this, null, DateTimeFormatInfo.GetInstance(provider));
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000CC9D File Offset: 0x0000BC9D
		public string ToString(string format, IFormatProvider provider)
		{
			return DateTimeFormat.Format(this, format, DateTimeFormatInfo.GetInstance(provider));
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000CCB1 File Offset: 0x0000BCB1
		public DateTime ToUniversalTime()
		{
			return TimeZone.CurrentTimeZone.ToUniversalTime(this);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000CCC3 File Offset: 0x0000BCC3
		public static bool TryParse(string s, out DateTime result)
		{
			return DateTimeParse.TryParse(s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out result);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000CCD2 File Offset: 0x0000BCD2
		public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(styles, "styles");
			return DateTimeParse.TryParse(s, DateTimeFormatInfo.GetInstance(provider), styles, out result);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000CCED File Offset: 0x0000BCED
		public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			return DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, out result);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000CD0A File Offset: 0x0000BD0A
		public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			return DateTimeParse.TryParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style, out result);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000CD28 File Offset: 0x0000BD28
		public static DateTime operator +(DateTime d, TimeSpan t)
		{
			long internalTicks = d.InternalTicks;
			long ticks = t._ticks;
			if (ticks > 3155378975999999999L - internalTicks || ticks < -internalTicks)
			{
				throw new ArgumentOutOfRangeException("t", Environment.GetResourceString("Overflow_DateArithmetic"));
			}
			return new DateTime((ulong)(internalTicks + ticks | (long)d.InternalKind));
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000CD80 File Offset: 0x0000BD80
		public static DateTime operator -(DateTime d, TimeSpan t)
		{
			long internalTicks = d.InternalTicks;
			long ticks = t._ticks;
			if (internalTicks < ticks || internalTicks - 3155378975999999999L > ticks)
			{
				throw new ArgumentOutOfRangeException("t", Environment.GetResourceString("Overflow_DateArithmetic"));
			}
			return new DateTime((ulong)(internalTicks - ticks | (long)d.InternalKind));
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000CDD4 File Offset: 0x0000BDD4
		public static TimeSpan operator -(DateTime d1, DateTime d2)
		{
			return new TimeSpan(d1.InternalTicks - d2.InternalTicks);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000CDEA File Offset: 0x0000BDEA
		public static bool operator ==(DateTime d1, DateTime d2)
		{
			return d1.InternalTicks == d2.InternalTicks;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000CDFC File Offset: 0x0000BDFC
		public static bool operator !=(DateTime d1, DateTime d2)
		{
			return d1.InternalTicks != d2.InternalTicks;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000CE11 File Offset: 0x0000BE11
		public static bool operator <(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks < t2.InternalTicks;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000CE23 File Offset: 0x0000BE23
		public static bool operator <=(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks <= t2.InternalTicks;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000CE38 File Offset: 0x0000BE38
		public static bool operator >(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks > t2.InternalTicks;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000CE4A File Offset: 0x0000BE4A
		public static bool operator >=(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks >= t2.InternalTicks;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000CE5F File Offset: 0x0000BE5F
		public string[] GetDateTimeFormats()
		{
			return this.GetDateTimeFormats(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000CE6C File Offset: 0x0000BE6C
		public string[] GetDateTimeFormats(IFormatProvider provider)
		{
			return DateTimeFormat.GetAllDateTimes(this, DateTimeFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000CE7F File Offset: 0x0000BE7F
		public string[] GetDateTimeFormats(char format)
		{
			return this.GetDateTimeFormats(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000CE8D File Offset: 0x0000BE8D
		public string[] GetDateTimeFormats(char format, IFormatProvider provider)
		{
			return DateTimeFormat.GetAllDateTimes(this, format, DateTimeFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000CEA1 File Offset: 0x0000BEA1
		public TypeCode GetTypeCode()
		{
			return TypeCode.DateTime;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000CEA8 File Offset: 0x0000BEA8
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Boolean"
			}));
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000CEE8 File Offset: 0x0000BEE8
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Char"
			}));
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000CF28 File Offset: 0x0000BF28
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"SByte"
			}));
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000CF68 File Offset: 0x0000BF68
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Byte"
			}));
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000CFA8 File Offset: 0x0000BFA8
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Int16"
			}));
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000CFE8 File Offset: 0x0000BFE8
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"UInt16"
			}));
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000D028 File Offset: 0x0000C028
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Int32"
			}));
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000D068 File Offset: 0x0000C068
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"UInt32"
			}));
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000D0A8 File Offset: 0x0000C0A8
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Int64"
			}));
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000D0E8 File Offset: 0x0000C0E8
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"UInt64"
			}));
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000D128 File Offset: 0x0000C128
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Single"
			}));
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000D168 File Offset: 0x0000C168
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Double"
			}));
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000D1A8 File Offset: 0x0000C1A8
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"DateTime",
				"Decimal"
			}));
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000D1E6 File Offset: 0x0000C1E6
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000D1EE File Offset: 0x0000C1EE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000D204 File Offset: 0x0000C204
		internal static bool TryCreate(int year, int month, int day, int hour, int minute, int second, int millisecond, out DateTime result)
		{
			result = DateTime.MinValue;
			if (year < 1 || year > 9999 || month < 1 || month > 12)
			{
				return false;
			}
			int[] array = DateTime.IsLeapYear(year) ? DateTime.DaysToMonth366 : DateTime.DaysToMonth365;
			if (day < 1 || day > array[month] - array[month - 1])
			{
				return false;
			}
			if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60 || second < 0 || second > 60)
			{
				return false;
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				return false;
			}
			if (second == 60)
			{
				if (!DateTime.s_isLeapSecondsSupportedSystem || !DateTime.IsValidTimeWithLeapSeconds(year, month, day, hour, minute, second, DateTimeKind.Unspecified))
				{
					return false;
				}
				second = 59;
			}
			long num = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
			num += (long)millisecond * 10000L;
			if (num < 0L || num > 3155378975999999999L)
			{
				return false;
			}
			result = new DateTime(num, DateTimeKind.Unspecified);
			return true;
		}

		// Token: 0x040000C9 RID: 201
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x040000CA RID: 202
		private const long TicksPerSecond = 10000000L;

		// Token: 0x040000CB RID: 203
		private const long TicksPerMinute = 600000000L;

		// Token: 0x040000CC RID: 204
		private const long TicksPerHour = 36000000000L;

		// Token: 0x040000CD RID: 205
		private const long TicksPerDay = 864000000000L;

		// Token: 0x040000CE RID: 206
		private const int MillisPerSecond = 1000;

		// Token: 0x040000CF RID: 207
		private const int MillisPerMinute = 60000;

		// Token: 0x040000D0 RID: 208
		private const int MillisPerHour = 3600000;

		// Token: 0x040000D1 RID: 209
		private const int MillisPerDay = 86400000;

		// Token: 0x040000D2 RID: 210
		private const int DaysPerYear = 365;

		// Token: 0x040000D3 RID: 211
		private const int DaysPer4Years = 1461;

		// Token: 0x040000D4 RID: 212
		private const int DaysPer100Years = 36524;

		// Token: 0x040000D5 RID: 213
		private const int DaysPer400Years = 146097;

		// Token: 0x040000D6 RID: 214
		private const int DaysTo1601 = 584388;

		// Token: 0x040000D7 RID: 215
		private const int DaysTo1899 = 693593;

		// Token: 0x040000D8 RID: 216
		private const int DaysTo10000 = 3652059;

		// Token: 0x040000D9 RID: 217
		internal const long MinTicks = 0L;

		// Token: 0x040000DA RID: 218
		internal const long MaxTicks = 3155378975999999999L;

		// Token: 0x040000DB RID: 219
		private const long MaxMillis = 315537897600000L;

		// Token: 0x040000DC RID: 220
		private const long FileTimeOffset = 504911232000000000L;

		// Token: 0x040000DD RID: 221
		private const long DoubleDateOffset = 599264352000000000L;

		// Token: 0x040000DE RID: 222
		private const long OADateMinAsTicks = 31241376000000000L;

		// Token: 0x040000DF RID: 223
		private const double OADateMinAsDouble = -657435.0;

		// Token: 0x040000E0 RID: 224
		private const double OADateMaxAsDouble = 2958466.0;

		// Token: 0x040000E1 RID: 225
		private const int DatePartYear = 0;

		// Token: 0x040000E2 RID: 226
		private const int DatePartDayOfYear = 1;

		// Token: 0x040000E3 RID: 227
		private const int DatePartMonth = 2;

		// Token: 0x040000E4 RID: 228
		private const int DatePartDay = 3;

		// Token: 0x040000E5 RID: 229
		private const ulong TicksMask = 4611686018427387903UL;

		// Token: 0x040000E6 RID: 230
		private const ulong FlagsMask = 13835058055282163712UL;

		// Token: 0x040000E7 RID: 231
		private const ulong LocalMask = 9223372036854775808UL;

		// Token: 0x040000E8 RID: 232
		private const long TicksCeiling = 4611686018427387904L;

		// Token: 0x040000E9 RID: 233
		private const ulong KindUnspecified = 0UL;

		// Token: 0x040000EA RID: 234
		private const ulong KindUtc = 4611686018427387904UL;

		// Token: 0x040000EB RID: 235
		private const ulong KindLocal = 9223372036854775808UL;

		// Token: 0x040000EC RID: 236
		private const ulong KindLocalAmbiguousDst = 13835058055282163712UL;

		// Token: 0x040000ED RID: 237
		private const int KindShift = 62;

		// Token: 0x040000EE RID: 238
		private const string TicksField = "ticks";

		// Token: 0x040000EF RID: 239
		private const string DateDataField = "dateData";

		// Token: 0x040000F0 RID: 240
		internal static readonly bool s_isLeapSecondsSupportedSystem = DateTime.IsLeapSecondsSupportedSystem();

		// Token: 0x040000F1 RID: 241
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

		// Token: 0x040000F2 RID: 242
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

		// Token: 0x040000F3 RID: 243
		public static readonly DateTime MinValue = new DateTime(0L, DateTimeKind.Unspecified);

		// Token: 0x040000F4 RID: 244
		public static readonly DateTime MaxValue = new DateTime(3155378975999999999L, DateTimeKind.Unspecified);

		// Token: 0x040000F5 RID: 245
		private ulong dateData;

		// Token: 0x02000033 RID: 51
		internal struct FullSystemTime
		{
			// Token: 0x0600031F RID: 799 RVA: 0x0000D3CC File Offset: 0x0000C3CC
			internal FullSystemTime(int year, int month, DayOfWeek dayOfWeek, int day, int hour, int minute, int second)
			{
				this.wYear = (ushort)year;
				this.wMonth = (ushort)month;
				this.wDayOfWeek = (ushort)dayOfWeek;
				this.wDay = (ushort)day;
				this.wHour = (ushort)hour;
				this.wMinute = (ushort)minute;
				this.wSecond = (ushort)second;
				this.wMillisecond = 0;
				this.hundredNanoSecond = 0L;
			}

			// Token: 0x06000320 RID: 800 RVA: 0x0000D424 File Offset: 0x0000C424
			internal FullSystemTime(long ticks)
			{
				DateTime dateTime = new DateTime(ticks);
				this.wYear = (ushort)dateTime.Year;
				this.wMonth = (ushort)dateTime.Month;
				this.wDayOfWeek = (ushort)dateTime.DayOfWeek;
				this.wDay = (ushort)dateTime.Day;
				this.wHour = (ushort)dateTime.Hour;
				this.wMinute = (ushort)dateTime.Minute;
				this.wSecond = (ushort)dateTime.Second;
				this.wMillisecond = (ushort)dateTime.Millisecond;
				this.hundredNanoSecond = 0L;
			}

			// Token: 0x040000F6 RID: 246
			internal ushort wYear;

			// Token: 0x040000F7 RID: 247
			internal ushort wMonth;

			// Token: 0x040000F8 RID: 248
			internal ushort wDayOfWeek;

			// Token: 0x040000F9 RID: 249
			internal ushort wDay;

			// Token: 0x040000FA RID: 250
			internal ushort wHour;

			// Token: 0x040000FB RID: 251
			internal ushort wMinute;

			// Token: 0x040000FC RID: 252
			internal ushort wSecond;

			// Token: 0x040000FD RID: 253
			internal ushort wMillisecond;

			// Token: 0x040000FE RID: 254
			internal long hundredNanoSecond;
		}
	}
}
