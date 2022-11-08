using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
	// Token: 0x020000A0 RID: 160
	[Serializable]
	internal class CurrentSystemTimeZone : TimeZone
	{
		// Token: 0x0600095B RID: 2395 RVA: 0x0001C752 File Offset: 0x0001B752
		internal CurrentSystemTimeZone()
		{
			this.m_ticksOffset = (long)CurrentSystemTimeZone.nativeGetTimeZoneMinuteOffset() * 600000000L;
			this.m_standardName = null;
			this.m_daylightName = null;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0001C786 File Offset: 0x0001B786
		public override string StandardName
		{
			get
			{
				if (this.m_standardName == null)
				{
					this.m_standardName = CurrentSystemTimeZone.nativeGetStandardName();
				}
				return this.m_standardName;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0001C7A1 File Offset: 0x0001B7A1
		public override string DaylightName
		{
			get
			{
				if (this.m_daylightName == null)
				{
					this.m_daylightName = CurrentSystemTimeZone.nativeGetDaylightName();
					if (this.m_daylightName == null)
					{
						this.m_daylightName = this.StandardName;
					}
				}
				return this.m_daylightName;
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0001C7D0 File Offset: 0x0001B7D0
		internal long GetUtcOffsetFromUniversalTime(DateTime time, ref bool isAmbiguousLocalDst)
		{
			TimeSpan timeSpan = new TimeSpan(this.m_ticksOffset);
			DaylightTime daylightChanges = this.GetDaylightChanges(time.Year);
			isAmbiguousLocalDst = false;
			if (daylightChanges == null || daylightChanges.Delta.Ticks == 0L)
			{
				return timeSpan.Ticks;
			}
			DateTime dateTime = daylightChanges.Start - timeSpan;
			DateTime dateTime2 = daylightChanges.End - timeSpan - daylightChanges.Delta;
			DateTime t;
			DateTime t2;
			if (daylightChanges.Delta.Ticks > 0L)
			{
				t = dateTime2 - daylightChanges.Delta;
				t2 = dateTime2;
			}
			else
			{
				t = dateTime;
				t2 = dateTime - daylightChanges.Delta;
			}
			bool flag;
			if (dateTime > dateTime2)
			{
				flag = (time < dateTime2 || time >= dateTime);
			}
			else
			{
				flag = (time >= dateTime && time < dateTime2);
			}
			if (flag)
			{
				timeSpan += daylightChanges.Delta;
				if (time >= t && time < t2)
				{
					isAmbiguousLocalDst = true;
				}
			}
			return timeSpan.Ticks;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001C8DC File Offset: 0x0001B8DC
		public override DateTime ToLocalTime(DateTime time)
		{
			if (time.Kind == DateTimeKind.Local)
			{
				return time;
			}
			bool isAmbiguousDst = false;
			long utcOffsetFromUniversalTime = this.GetUtcOffsetFromUniversalTime(time, ref isAmbiguousDst);
			long num = time.Ticks + utcOffsetFromUniversalTime;
			if (num > 3155378975999999999L)
			{
				return new DateTime(3155378975999999999L, DateTimeKind.Local);
			}
			if (num < 0L)
			{
				return new DateTime(0L, DateTimeKind.Local);
			}
			return new DateTime(num, DateTimeKind.Local, isAmbiguousDst);
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0001C940 File Offset: 0x0001B940
		private static object InternalSyncObject
		{
			get
			{
				if (CurrentSystemTimeZone.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref CurrentSystemTimeZone.s_InternalSyncObject, value, null);
				}
				return CurrentSystemTimeZone.s_InternalSyncObject;
			}
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001C96C File Offset: 0x0001B96C
		public override DaylightTime GetDaylightChanges(int year)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					9999
				}));
			}
			object key = year;
			if (!this.m_CachedDaylightChanges.Contains(key))
			{
				lock (CurrentSystemTimeZone.InternalSyncObject)
				{
					if (!this.m_CachedDaylightChanges.Contains(key))
					{
						short[] array = CurrentSystemTimeZone.nativeGetDaylightChanges();
						if (array == null)
						{
							this.m_CachedDaylightChanges.Add(key, new DaylightTime(DateTime.MinValue, DateTime.MinValue, TimeSpan.Zero));
						}
						else
						{
							DateTime dayOfWeek = CurrentSystemTimeZone.GetDayOfWeek(year, array[0] != 0, (int)array[1], (int)array[2], (int)array[3], (int)array[4], (int)array[5], (int)array[6], (int)array[7]);
							DateTime dayOfWeek2 = CurrentSystemTimeZone.GetDayOfWeek(year, array[8] != 0, (int)array[9], (int)array[10], (int)array[11], (int)array[12], (int)array[13], (int)array[14], (int)array[15]);
							TimeSpan delta = new TimeSpan((long)array[16] * 600000000L);
							DaylightTime value = new DaylightTime(dayOfWeek, dayOfWeek2, delta);
							this.m_CachedDaylightChanges.Add(key, value);
						}
					}
				}
			}
			return (DaylightTime)this.m_CachedDaylightChanges[key];
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0001CAD4 File Offset: 0x0001BAD4
		public override TimeSpan GetUtcOffset(DateTime time)
		{
			if (time.Kind == DateTimeKind.Utc)
			{
				return TimeSpan.Zero;
			}
			return new TimeSpan(TimeZone.CalculateUtcOffset(time, this.GetDaylightChanges(time.Year)).Ticks + this.m_ticksOffset);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0001CB18 File Offset: 0x0001BB18
		private static DateTime GetDayOfWeek(int year, bool fixedDate, int month, int targetDayOfWeek, int numberOfSunday, int hour, int minute, int second, int millisecond)
		{
			DateTime result;
			if (fixedDate)
			{
				int num = DateTime.DaysInMonth(year, month);
				result = new DateTime(year, month, (num < numberOfSunday) ? num : numberOfSunday, hour, minute, second, millisecond, DateTimeKind.Local);
			}
			else if (numberOfSunday <= 4)
			{
				result = new DateTime(year, month, 1, hour, minute, second, millisecond, DateTimeKind.Local);
				int dayOfWeek = (int)result.DayOfWeek;
				int num2 = targetDayOfWeek - dayOfWeek;
				if (num2 < 0)
				{
					num2 += 7;
				}
				num2 += 7 * (numberOfSunday - 1);
				if (num2 > 0)
				{
					result = result.AddDays((double)num2);
				}
			}
			else
			{
				Calendar defaultInstance = GregorianCalendar.GetDefaultInstance();
				result = new DateTime(year, month, defaultInstance.GetDaysInMonth(year, month), hour, minute, second, millisecond, DateTimeKind.Local);
				int dayOfWeek2 = (int)result.DayOfWeek;
				int num3 = dayOfWeek2 - targetDayOfWeek;
				if (num3 < 0)
				{
					num3 += 7;
				}
				if (num3 > 0)
				{
					result = result.AddDays((double)(-(double)num3));
				}
			}
			return result;
		}

		// Token: 0x06000964 RID: 2404
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeGetTimeZoneMinuteOffset();

		// Token: 0x06000965 RID: 2405
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetDaylightName();

		// Token: 0x06000966 RID: 2406
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetStandardName();

		// Token: 0x06000967 RID: 2407
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern short[] nativeGetDaylightChanges();

		// Token: 0x0400038E RID: 910
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x0400038F RID: 911
		private const long TicksPerSecond = 10000000L;

		// Token: 0x04000390 RID: 912
		private const long TicksPerMinute = 600000000L;

		// Token: 0x04000391 RID: 913
		private Hashtable m_CachedDaylightChanges = new Hashtable();

		// Token: 0x04000392 RID: 914
		private long m_ticksOffset;

		// Token: 0x04000393 RID: 915
		private string m_standardName;

		// Token: 0x04000394 RID: 916
		private string m_daylightName;

		// Token: 0x04000395 RID: 917
		private static object s_InternalSyncObject;
	}
}
