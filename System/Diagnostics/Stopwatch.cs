using System;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000797 RID: 1943
	public class Stopwatch
	{
		// Token: 0x06003BEF RID: 15343 RVA: 0x001007A4 File Offset: 0x000FF7A4
		static Stopwatch()
		{
			if (!SafeNativeMethods.QueryPerformanceFrequency(out Stopwatch.Frequency))
			{
				Stopwatch.IsHighResolution = false;
				Stopwatch.Frequency = 10000000L;
				Stopwatch.tickFrequency = 1.0;
				return;
			}
			Stopwatch.IsHighResolution = true;
			Stopwatch.tickFrequency = 10000000.0;
			Stopwatch.tickFrequency /= (double)Stopwatch.Frequency;
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x00100804 File Offset: 0x000FF804
		public Stopwatch()
		{
			this.Reset();
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x00100812 File Offset: 0x000FF812
		public void Start()
		{
			if (!this.isRunning)
			{
				this.startTimeStamp = Stopwatch.GetTimestamp();
				this.isRunning = true;
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x00100830 File Offset: 0x000FF830
		public static Stopwatch StartNew()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0010084C File Offset: 0x000FF84C
		public void Stop()
		{
			if (this.isRunning)
			{
				long timestamp = Stopwatch.GetTimestamp();
				long num = timestamp - this.startTimeStamp;
				this.elapsed += num;
				this.isRunning = false;
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x00100885 File Offset: 0x000FF885
		public void Reset()
		{
			this.elapsed = 0L;
			this.isRunning = false;
			this.startTimeStamp = 0L;
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003BF5 RID: 15349 RVA: 0x0010089E File Offset: 0x000FF89E
		public bool IsRunning
		{
			get
			{
				return this.isRunning;
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003BF6 RID: 15350 RVA: 0x001008A6 File Offset: 0x000FF8A6
		public TimeSpan Elapsed
		{
			get
			{
				return new TimeSpan(this.GetElapsedDateTimeTicks());
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003BF7 RID: 15351 RVA: 0x001008B3 File Offset: 0x000FF8B3
		public long ElapsedMilliseconds
		{
			get
			{
				return this.GetElapsedDateTimeTicks() / 10000L;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x001008C2 File Offset: 0x000FF8C2
		public long ElapsedTicks
		{
			get
			{
				return this.GetRawElapsedTicks();
			}
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x001008CC File Offset: 0x000FF8CC
		public static long GetTimestamp()
		{
			if (Stopwatch.IsHighResolution)
			{
				long result = 0L;
				SafeNativeMethods.QueryPerformanceCounter(out result);
				return result;
			}
			return DateTime.UtcNow.Ticks;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x001008FC File Offset: 0x000FF8FC
		private long GetRawElapsedTicks()
		{
			long num = this.elapsed;
			if (this.isRunning)
			{
				long timestamp = Stopwatch.GetTimestamp();
				long num2 = timestamp - this.startTimeStamp;
				num += num2;
			}
			return num;
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x0010092C File Offset: 0x000FF92C
		private long GetElapsedDateTimeTicks()
		{
			long rawElapsedTicks = this.GetRawElapsedTicks();
			if (Stopwatch.IsHighResolution)
			{
				double num = (double)rawElapsedTicks;
				num *= Stopwatch.tickFrequency;
				return (long)num;
			}
			return rawElapsedTicks;
		}

		// Token: 0x0400348C RID: 13452
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x0400348D RID: 13453
		private const long TicksPerSecond = 10000000L;

		// Token: 0x0400348E RID: 13454
		private long elapsed;

		// Token: 0x0400348F RID: 13455
		private long startTimeStamp;

		// Token: 0x04003490 RID: 13456
		private bool isRunning;

		// Token: 0x04003491 RID: 13457
		public static readonly long Frequency;

		// Token: 0x04003492 RID: 13458
		public static readonly bool IsHighResolution;

		// Token: 0x04003493 RID: 13459
		private static readonly double tickFrequency;
	}
}
