using System;

namespace System.Diagnostics
{
	// Token: 0x02000742 RID: 1858
	public struct CounterSample
	{
		// Token: 0x060038A8 RID: 14504 RVA: 0x000EF19D File Offset: 0x000EE19D
		public CounterSample(long rawValue, long baseValue, long counterFrequency, long systemFrequency, long timeStamp, long timeStamp100nSec, PerformanceCounterType counterType)
		{
			this.rawValue = rawValue;
			this.baseValue = baseValue;
			this.timeStamp = timeStamp;
			this.counterFrequency = counterFrequency;
			this.counterType = counterType;
			this.timeStamp100nSec = timeStamp100nSec;
			this.systemFrequency = systemFrequency;
			this.counterTimeStamp = 0L;
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x000EF1DC File Offset: 0x000EE1DC
		public CounterSample(long rawValue, long baseValue, long counterFrequency, long systemFrequency, long timeStamp, long timeStamp100nSec, PerformanceCounterType counterType, long counterTimeStamp)
		{
			this.rawValue = rawValue;
			this.baseValue = baseValue;
			this.timeStamp = timeStamp;
			this.counterFrequency = counterFrequency;
			this.counterType = counterType;
			this.timeStamp100nSec = timeStamp100nSec;
			this.systemFrequency = systemFrequency;
			this.counterTimeStamp = counterTimeStamp;
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x060038AA RID: 14506 RVA: 0x000EF21B File Offset: 0x000EE21B
		public long RawValue
		{
			get
			{
				return this.rawValue;
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x060038AB RID: 14507 RVA: 0x000EF223 File Offset: 0x000EE223
		internal ulong UnsignedRawValue
		{
			get
			{
				return (ulong)this.rawValue;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x060038AC RID: 14508 RVA: 0x000EF22B File Offset: 0x000EE22B
		public long BaseValue
		{
			get
			{
				return this.baseValue;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x060038AD RID: 14509 RVA: 0x000EF233 File Offset: 0x000EE233
		public long SystemFrequency
		{
			get
			{
				return this.systemFrequency;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x060038AE RID: 14510 RVA: 0x000EF23B File Offset: 0x000EE23B
		public long CounterFrequency
		{
			get
			{
				return this.counterFrequency;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x060038AF RID: 14511 RVA: 0x000EF243 File Offset: 0x000EE243
		public long CounterTimeStamp
		{
			get
			{
				return this.counterTimeStamp;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x060038B0 RID: 14512 RVA: 0x000EF24B File Offset: 0x000EE24B
		public long TimeStamp
		{
			get
			{
				return this.timeStamp;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x060038B1 RID: 14513 RVA: 0x000EF253 File Offset: 0x000EE253
		public long TimeStamp100nSec
		{
			get
			{
				return this.timeStamp100nSec;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x060038B2 RID: 14514 RVA: 0x000EF25B File Offset: 0x000EE25B
		public PerformanceCounterType CounterType
		{
			get
			{
				return this.counterType;
			}
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x000EF263 File Offset: 0x000EE263
		public static float Calculate(CounterSample counterSample)
		{
			return CounterSampleCalculator.ComputeCounterValue(counterSample);
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x000EF26B File Offset: 0x000EE26B
		public static float Calculate(CounterSample counterSample, CounterSample nextCounterSample)
		{
			return CounterSampleCalculator.ComputeCounterValue(counterSample, nextCounterSample);
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x000EF274 File Offset: 0x000EE274
		public override bool Equals(object o)
		{
			return o is CounterSample && this.Equals((CounterSample)o);
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x000EF28C File Offset: 0x000EE28C
		public bool Equals(CounterSample sample)
		{
			return this.rawValue == sample.rawValue && this.baseValue == sample.baseValue && this.timeStamp == sample.timeStamp && this.counterFrequency == sample.counterFrequency && this.counterType == sample.counterType && this.timeStamp100nSec == sample.timeStamp100nSec && this.systemFrequency == sample.systemFrequency && this.counterTimeStamp == sample.counterTimeStamp;
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x000EF313 File Offset: 0x000EE313
		public override int GetHashCode()
		{
			return this.rawValue.GetHashCode();
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x000EF320 File Offset: 0x000EE320
		public static bool operator ==(CounterSample a, CounterSample b)
		{
			return a.Equals(b);
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x000EF32A File Offset: 0x000EE32A
		public static bool operator !=(CounterSample a, CounterSample b)
		{
			return !a.Equals(b);
		}

		// Token: 0x04003257 RID: 12887
		private long rawValue;

		// Token: 0x04003258 RID: 12888
		private long baseValue;

		// Token: 0x04003259 RID: 12889
		private long timeStamp;

		// Token: 0x0400325A RID: 12890
		private long counterFrequency;

		// Token: 0x0400325B RID: 12891
		private PerformanceCounterType counterType;

		// Token: 0x0400325C RID: 12892
		private long timeStamp100nSec;

		// Token: 0x0400325D RID: 12893
		private long systemFrequency;

		// Token: 0x0400325E RID: 12894
		private long counterTimeStamp;

		// Token: 0x0400325F RID: 12895
		public static CounterSample Empty = new CounterSample(0L, 0L, 0L, 0L, 0L, 0L, PerformanceCounterType.NumberOfItems32);
	}
}
