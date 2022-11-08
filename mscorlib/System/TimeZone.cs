using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	// Token: 0x0200009F RID: 159
	[ComVisible(true)]
	[Serializable]
	public abstract class TimeZone
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0001C4E0 File Offset: 0x0001B4E0
		private static object InternalSyncObject
		{
			get
			{
				if (TimeZone.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref TimeZone.s_InternalSyncObject, value, null);
				}
				return TimeZone.s_InternalSyncObject;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0001C514 File Offset: 0x0001B514
		public static TimeZone CurrentTimeZone
		{
			get
			{
				TimeZone timeZone = TimeZone.currentTimeZone;
				if (timeZone == null)
				{
					lock (TimeZone.InternalSyncObject)
					{
						if (TimeZone.currentTimeZone == null)
						{
							TimeZone.currentTimeZone = new CurrentSystemTimeZone();
						}
						timeZone = TimeZone.currentTimeZone;
					}
				}
				return timeZone;
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001C568 File Offset: 0x0001B568
		internal static void ResetTimeZone()
		{
			if (TimeZone.currentTimeZone != null)
			{
				lock (TimeZone.InternalSyncObject)
				{
					TimeZone.currentTimeZone = null;
				}
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000952 RID: 2386
		public abstract string StandardName { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000953 RID: 2387
		public abstract string DaylightName { get; }

		// Token: 0x06000954 RID: 2388
		public abstract TimeSpan GetUtcOffset(DateTime time);

		// Token: 0x06000955 RID: 2389 RVA: 0x0001C5A8 File Offset: 0x0001B5A8
		public virtual DateTime ToUniversalTime(DateTime time)
		{
			if (time.Kind == DateTimeKind.Utc)
			{
				return time;
			}
			long num = time.Ticks - this.GetUtcOffset(time).Ticks;
			if (num > 3155378975999999999L)
			{
				return new DateTime(3155378975999999999L, DateTimeKind.Utc);
			}
			if (num < 0L)
			{
				return new DateTime(0L, DateTimeKind.Utc);
			}
			return new DateTime(num, DateTimeKind.Utc);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0001C60C File Offset: 0x0001B60C
		public virtual DateTime ToLocalTime(DateTime time)
		{
			if (time.Kind == DateTimeKind.Local)
			{
				return time;
			}
			bool isAmbiguousDst = false;
			long utcOffsetFromUniversalTime = ((CurrentSystemTimeZone)TimeZone.CurrentTimeZone).GetUtcOffsetFromUniversalTime(time, ref isAmbiguousDst);
			return new DateTime(time.Ticks + utcOffsetFromUniversalTime, DateTimeKind.Local, isAmbiguousDst);
		}

		// Token: 0x06000957 RID: 2391
		public abstract DaylightTime GetDaylightChanges(int year);

		// Token: 0x06000958 RID: 2392 RVA: 0x0001C64A File Offset: 0x0001B64A
		public virtual bool IsDaylightSavingTime(DateTime time)
		{
			return TimeZone.IsDaylightSavingTime(time, this.GetDaylightChanges(time.Year));
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0001C65F File Offset: 0x0001B65F
		public static bool IsDaylightSavingTime(DateTime time, DaylightTime daylightTimes)
		{
			return TimeZone.CalculateUtcOffset(time, daylightTimes) != TimeSpan.Zero;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0001C674 File Offset: 0x0001B674
		internal static TimeSpan CalculateUtcOffset(DateTime time, DaylightTime daylightTimes)
		{
			if (daylightTimes == null)
			{
				return TimeSpan.Zero;
			}
			DateTimeKind kind = time.Kind;
			if (kind == DateTimeKind.Utc)
			{
				return TimeSpan.Zero;
			}
			DateTime dateTime = daylightTimes.Start + daylightTimes.Delta;
			DateTime end = daylightTimes.End;
			DateTime t;
			DateTime t2;
			if (daylightTimes.Delta.Ticks > 0L)
			{
				t = end - daylightTimes.Delta;
				t2 = end;
			}
			else
			{
				t = dateTime;
				t2 = dateTime - daylightTimes.Delta;
			}
			bool flag = false;
			if (dateTime > end)
			{
				if (time >= dateTime || time < end)
				{
					flag = true;
				}
			}
			else if (time >= dateTime && time < end)
			{
				flag = true;
			}
			if (flag && time >= t && time < t2)
			{
				flag = time.IsAmbiguousDaylightSavingTime();
			}
			if (flag)
			{
				return daylightTimes.Delta;
			}
			return TimeSpan.Zero;
		}

		// Token: 0x0400038C RID: 908
		private static TimeZone currentTimeZone;

		// Token: 0x0400038D RID: 909
		private static object s_InternalSyncObject;
	}
}
