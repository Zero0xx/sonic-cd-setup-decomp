using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct DateTimeOffset : IComparable, IFormattable, ISerializable, IDeserializationCallback, IComparable<DateTimeOffset>, IEquatable<DateTimeOffset>
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000D4B4 File Offset: 0x0000C4B4
		public DateTimeOffset(long ticks, TimeSpan offset)
		{
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			DateTime dateTime = new DateTime(ticks);
			this.m_dateTime = DateTimeOffset.ValidateDate(dateTime, offset);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000D4E4 File Offset: 0x0000C4E4
		public DateTimeOffset(DateTime dateTime)
		{
			TimeSpan utcOffset;
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
			}
			else
			{
				utcOffset = new TimeSpan(0L);
			}
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(utcOffset);
			this.m_dateTime = DateTimeOffset.ValidateDate(dateTime, utcOffset);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000D52C File Offset: 0x0000C52C
		public DateTimeOffset(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == DateTimeKind.Local)
			{
				if (offset != TimeZone.CurrentTimeZone.GetUtcOffset(dateTime))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_OffsetLocalMismatch"), "offset");
				}
			}
			else if (dateTime.Kind == DateTimeKind.Utc && offset != TimeSpan.Zero)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OffsetUtcMismatch"), "offset");
			}
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this.m_dateTime = DateTimeOffset.ValidateDate(dateTime, offset);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000D5B0 File Offset: 0x0000C5B0
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, TimeSpan offset)
		{
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			this.m_dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second), offset);
			if (num == 60 && !DateTime.IsValidTimeWithLeapSeconds(this.m_dateTime.Year, this.m_dateTime.Month, this.m_dateTime.Day, this.m_dateTime.Hour, this.m_dateTime.Minute, 60, DateTimeKind.Utc))
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000D650 File Offset: 0x0000C650
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, TimeSpan offset)
		{
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			this.m_dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond), offset);
			if (num == 60 && !DateTime.IsValidTimeWithLeapSeconds(this.m_dateTime.Year, this.m_dateTime.Month, this.m_dateTime.Day, this.m_dateTime.Hour, this.m_dateTime.Minute, 60, DateTimeKind.Utc))
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000D6F4 File Offset: 0x0000C6F4
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, TimeSpan offset)
		{
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			this.m_dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond, calendar), offset);
			if (num == 60 && !DateTime.IsValidTimeWithLeapSeconds(this.m_dateTime.Year, this.m_dateTime.Month, this.m_dateTime.Day, this.m_dateTime.Hour, this.m_dateTime.Minute, 60, DateTimeKind.Utc))
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000D798 File Offset: 0x0000C798
		public static DateTimeOffset Now
		{
			get
			{
				return new DateTimeOffset(DateTime.Now);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000D7A4 File Offset: 0x0000C7A4
		public static DateTimeOffset UtcNow
		{
			get
			{
				return new DateTimeOffset(DateTime.UtcNow);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000D7B0 File Offset: 0x0000C7B0
		public DateTime DateTime
		{
			get
			{
				return this.ClockDateTime;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000D7B8 File Offset: 0x0000C7B8
		public DateTime UtcDateTime
		{
			get
			{
				return DateTime.SpecifyKind(this.m_dateTime, DateTimeKind.Utc);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000D7C8 File Offset: 0x0000C7C8
		public DateTime LocalDateTime
		{
			get
			{
				return this.UtcDateTime.ToLocalTime();
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000D7E4 File Offset: 0x0000C7E4
		public DateTimeOffset ToOffset(TimeSpan offset)
		{
			return new DateTimeOffset((this.m_dateTime + offset).Ticks, offset);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000D80C File Offset: 0x0000C80C
		private DateTime ClockDateTime
		{
			get
			{
				return new DateTime((this.m_dateTime + this.Offset).Ticks, DateTimeKind.Unspecified);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000D838 File Offset: 0x0000C838
		public DateTime Date
		{
			get
			{
				return this.ClockDateTime.Date;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000D854 File Offset: 0x0000C854
		public int Day
		{
			get
			{
				return this.ClockDateTime.Day;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000D870 File Offset: 0x0000C870
		public DayOfWeek DayOfWeek
		{
			get
			{
				return this.ClockDateTime.DayOfWeek;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000D88C File Offset: 0x0000C88C
		public int DayOfYear
		{
			get
			{
				return this.ClockDateTime.DayOfYear;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000D8A8 File Offset: 0x0000C8A8
		public int Hour
		{
			get
			{
				return this.ClockDateTime.Hour;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000D8C4 File Offset: 0x0000C8C4
		public int Millisecond
		{
			get
			{
				return this.ClockDateTime.Millisecond;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000D8E0 File Offset: 0x0000C8E0
		public int Minute
		{
			get
			{
				return this.ClockDateTime.Minute;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000D8FC File Offset: 0x0000C8FC
		public int Month
		{
			get
			{
				return this.ClockDateTime.Month;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000D917 File Offset: 0x0000C917
		public TimeSpan Offset
		{
			get
			{
				return new TimeSpan(0, (int)this.m_offsetMinutes, 0);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000D928 File Offset: 0x0000C928
		public int Second
		{
			get
			{
				return this.ClockDateTime.Second;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000D944 File Offset: 0x0000C944
		public long Ticks
		{
			get
			{
				return this.ClockDateTime.Ticks;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000D960 File Offset: 0x0000C960
		public long UtcTicks
		{
			get
			{
				return this.UtcDateTime.Ticks;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000D97C File Offset: 0x0000C97C
		public TimeSpan TimeOfDay
		{
			get
			{
				return this.ClockDateTime.TimeOfDay;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000D998 File Offset: 0x0000C998
		public int Year
		{
			get
			{
				return this.ClockDateTime.Year;
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000D9B4 File Offset: 0x0000C9B4
		public DateTimeOffset Add(TimeSpan timeSpan)
		{
			return new DateTimeOffset(this.ClockDateTime.Add(timeSpan), this.Offset);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000D9DC File Offset: 0x0000C9DC
		public DateTimeOffset AddDays(double days)
		{
			return new DateTimeOffset(this.ClockDateTime.AddDays(days), this.Offset);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000DA04 File Offset: 0x0000CA04
		public DateTimeOffset AddHours(double hours)
		{
			return new DateTimeOffset(this.ClockDateTime.AddHours(hours), this.Offset);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000DA2C File Offset: 0x0000CA2C
		public DateTimeOffset AddMilliseconds(double milliseconds)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMilliseconds(milliseconds), this.Offset);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000DA54 File Offset: 0x0000CA54
		public DateTimeOffset AddMinutes(double minutes)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMinutes(minutes), this.Offset);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000DA7C File Offset: 0x0000CA7C
		public DateTimeOffset AddMonths(int months)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMonths(months), this.Offset);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000DAA4 File Offset: 0x0000CAA4
		public DateTimeOffset AddSeconds(double seconds)
		{
			return new DateTimeOffset(this.ClockDateTime.AddSeconds(seconds), this.Offset);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000DACC File Offset: 0x0000CACC
		public DateTimeOffset AddTicks(long ticks)
		{
			return new DateTimeOffset(this.ClockDateTime.AddTicks(ticks), this.Offset);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000DAF4 File Offset: 0x0000CAF4
		public DateTimeOffset AddYears(int years)
		{
			return new DateTimeOffset(this.ClockDateTime.AddYears(years), this.Offset);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000DB1B File Offset: 0x0000CB1B
		public static int Compare(DateTimeOffset first, DateTimeOffset second)
		{
			return DateTime.Compare(first.UtcDateTime, second.UtcDateTime);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000DB30 File Offset: 0x0000CB30
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is DateTimeOffset))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDateTimeOffset"));
			}
			DateTime utcDateTime = ((DateTimeOffset)obj).UtcDateTime;
			DateTime utcDateTime2 = this.UtcDateTime;
			if (utcDateTime2 > utcDateTime)
			{
				return 1;
			}
			if (utcDateTime2 < utcDateTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000DB88 File Offset: 0x0000CB88
		public int CompareTo(DateTimeOffset other)
		{
			DateTime utcDateTime = other.UtcDateTime;
			DateTime utcDateTime2 = this.UtcDateTime;
			if (utcDateTime2 > utcDateTime)
			{
				return 1;
			}
			if (utcDateTime2 < utcDateTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000DBBC File Offset: 0x0000CBBC
		public override bool Equals(object obj)
		{
			return obj is DateTimeOffset && this.UtcDateTime.Equals(((DateTimeOffset)obj).UtcDateTime);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000DBF0 File Offset: 0x0000CBF0
		public bool Equals(DateTimeOffset other)
		{
			return this.UtcDateTime.Equals(other.UtcDateTime);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000DC14 File Offset: 0x0000CC14
		public bool EqualsExact(DateTimeOffset other)
		{
			return this.ClockDateTime == other.ClockDateTime && this.Offset == other.Offset && this.ClockDateTime.Kind == other.ClockDateTime.Kind;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000DC6A File Offset: 0x0000CC6A
		public static bool Equals(DateTimeOffset first, DateTimeOffset second)
		{
			return DateTime.Equals(first.UtcDateTime, second.UtcDateTime);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000DC7F File Offset: 0x0000CC7F
		public static DateTimeOffset FromFileTime(long fileTime)
		{
			return new DateTimeOffset(DateTime.FromFileTime(fileTime));
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000DC8C File Offset: 0x0000CC8C
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			try
			{
				this.m_offsetMinutes = DateTimeOffset.ValidateOffset(this.Offset);
				this.m_dateTime = DateTimeOffset.ValidateDate(this.ClockDateTime, this.Offset);
			}
			catch (ArgumentException innerException)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidData"), innerException);
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000DCE8 File Offset: 0x0000CCE8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("DateTime", this.m_dateTime);
			info.AddValue("OffsetMinutes", this.m_offsetMinutes);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000DD1C File Offset: 0x0000CD1C
		private DateTimeOffset(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_dateTime = (DateTime)info.GetValue("DateTime", typeof(DateTime));
			this.m_offsetMinutes = (short)info.GetValue("OffsetMinutes", typeof(short));
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000DD78 File Offset: 0x0000CD78
		public override int GetHashCode()
		{
			return this.UtcDateTime.GetHashCode();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000DD9C File Offset: 0x0000CD9C
		public static DateTimeOffset Parse(string input)
		{
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.Parse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out offset).Ticks, offset);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000DDC5 File Offset: 0x0000CDC5
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider)
		{
			return DateTimeOffset.Parse(input, formatProvider, DateTimeStyles.None);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000DDD0 File Offset: 0x0000CDD0
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.Parse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000DE07 File Offset: 0x0000CE07
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider)
		{
			return DateTimeOffset.ParseExact(input, format, formatProvider, DateTimeStyles.None);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000DE14 File Offset: 0x0000CE14
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.ParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000DE4C File Offset: 0x0000CE4C
		public static DateTimeOffset ParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.ParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000DE84 File Offset: 0x0000CE84
		public TimeSpan Subtract(DateTimeOffset value)
		{
			return this.UtcDateTime.Subtract(value.UtcDateTime);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000DEA8 File Offset: 0x0000CEA8
		public DateTimeOffset Subtract(TimeSpan value)
		{
			return new DateTimeOffset(this.ClockDateTime.Subtract(value), this.Offset);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000DED0 File Offset: 0x0000CED0
		public long ToFileTime()
		{
			return this.UtcDateTime.ToFileTime();
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000DEEC File Offset: 0x0000CEEC
		public DateTimeOffset ToLocalTime()
		{
			return new DateTimeOffset(this.UtcDateTime.ToLocalTime());
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000DF0C File Offset: 0x0000CF0C
		public override string ToString()
		{
			return DateTimeFormat.Format(this.ClockDateTime, null, DateTimeFormatInfo.CurrentInfo, this.Offset);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000DF25 File Offset: 0x0000CF25
		public string ToString(string format)
		{
			return DateTimeFormat.Format(this.ClockDateTime, format, DateTimeFormatInfo.CurrentInfo, this.Offset);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000DF3E File Offset: 0x0000CF3E
		public string ToString(IFormatProvider formatProvider)
		{
			return DateTimeFormat.Format(this.ClockDateTime, null, DateTimeFormatInfo.GetInstance(formatProvider), this.Offset);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000DF58 File Offset: 0x0000CF58
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return DateTimeFormat.Format(this.ClockDateTime, format, DateTimeFormatInfo.GetInstance(formatProvider), this.Offset);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000DF72 File Offset: 0x0000CF72
		public DateTimeOffset ToUniversalTime()
		{
			return new DateTimeOffset(this.UtcDateTime);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000DF80 File Offset: 0x0000CF80
		public static bool TryParse(string input, out DateTimeOffset result)
		{
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000DFB4 File Offset: 0x0000CFB4
		public static bool TryParse(string input, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000DFF4 File Offset: 0x0000CFF4
		public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000E038 File Offset: 0x0000D038
		public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000E07C File Offset: 0x0000D07C
		private static short ValidateOffset(TimeSpan offset)
		{
			long ticks = offset.Ticks;
			if (ticks % 600000000L != 0L)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OffsetPrecision"), "offset");
			}
			if (ticks < -504000000000L || ticks > 504000000000L)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("Argument_OffsetOutOfRange"));
			}
			return (short)(offset.Ticks / 600000000L);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000E0F0 File Offset: 0x0000D0F0
		private static DateTime ValidateDate(DateTime dateTime, TimeSpan offset)
		{
			long num = dateTime.Ticks - offset.Ticks;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("Argument_UTCOutOfRange"));
			}
			return new DateTime(num, DateTimeKind.Unspecified);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000E13C File Offset: 0x0000D13C
		private static DateTimeStyles ValidateStyles(DateTimeStyles style, string parameterName)
		{
			if ((style & ~(DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal | DateTimeStyles.AssumeUniversal | DateTimeStyles.RoundtripKind)) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDateTimeStyles"), parameterName);
			}
			if ((style & DateTimeStyles.AssumeLocal) != DateTimeStyles.None && (style & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ConflictingDateTimeStyles"), parameterName);
			}
			if ((style & DateTimeStyles.NoCurrentDateDefault) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DateTimeOffsetInvalidDateTimeStyles"), parameterName);
			}
			style &= ~DateTimeStyles.RoundtripKind;
			style &= ~DateTimeStyles.AssumeLocal;
			return style;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000E1A6 File Offset: 0x0000D1A6
		public static implicit operator DateTimeOffset(DateTime dateTime)
		{
			return new DateTimeOffset(dateTime);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000E1AE File Offset: 0x0000D1AE
		public static DateTimeOffset operator +(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
		{
			return new DateTimeOffset(dateTimeTz.ClockDateTime + timeSpan, dateTimeTz.Offset);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000E1C9 File Offset: 0x0000D1C9
		public static DateTimeOffset operator -(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
		{
			return new DateTimeOffset(dateTimeTz.ClockDateTime - timeSpan, dateTimeTz.Offset);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000E1E4 File Offset: 0x0000D1E4
		public static TimeSpan operator -(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime - right.UtcDateTime;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000E1F9 File Offset: 0x0000D1F9
		public static bool operator ==(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime == right.UtcDateTime;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000E20E File Offset: 0x0000D20E
		public static bool operator !=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime != right.UtcDateTime;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000E223 File Offset: 0x0000D223
		public static bool operator <(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime < right.UtcDateTime;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000E238 File Offset: 0x0000D238
		public static bool operator <=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime <= right.UtcDateTime;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000E24D File Offset: 0x0000D24D
		public static bool operator >(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime > right.UtcDateTime;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000E262 File Offset: 0x0000D262
		public static bool operator >=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime >= right.UtcDateTime;
		}

		// Token: 0x04000103 RID: 259
		internal const long MaxOffset = 504000000000L;

		// Token: 0x04000104 RID: 260
		internal const long MinOffset = -504000000000L;

		// Token: 0x04000105 RID: 261
		public static readonly DateTimeOffset MinValue = new DateTimeOffset(0L, TimeSpan.Zero);

		// Token: 0x04000106 RID: 262
		public static readonly DateTimeOffset MaxValue = new DateTimeOffset(3155378975999999999L, TimeSpan.Zero);

		// Token: 0x04000107 RID: 263
		private DateTime m_dateTime;

		// Token: 0x04000108 RID: 264
		private short m_offsetMinutes;
	}
}
