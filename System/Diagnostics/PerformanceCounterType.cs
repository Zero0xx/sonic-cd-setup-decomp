using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x02000771 RID: 1905
	[TypeConverter(typeof(AlphabeticalEnumConverter))]
	public enum PerformanceCounterType
	{
		// Token: 0x0400334D RID: 13133
		NumberOfItems32 = 65536,
		// Token: 0x0400334E RID: 13134
		NumberOfItems64 = 65792,
		// Token: 0x0400334F RID: 13135
		NumberOfItemsHEX32 = 0,
		// Token: 0x04003350 RID: 13136
		NumberOfItemsHEX64 = 256,
		// Token: 0x04003351 RID: 13137
		RateOfCountsPerSecond32 = 272696320,
		// Token: 0x04003352 RID: 13138
		RateOfCountsPerSecond64 = 272696576,
		// Token: 0x04003353 RID: 13139
		CountPerTimeInterval32 = 4523008,
		// Token: 0x04003354 RID: 13140
		CountPerTimeInterval64 = 4523264,
		// Token: 0x04003355 RID: 13141
		RawFraction = 537003008,
		// Token: 0x04003356 RID: 13142
		RawBase = 1073939459,
		// Token: 0x04003357 RID: 13143
		AverageTimer32 = 805438464,
		// Token: 0x04003358 RID: 13144
		AverageBase = 1073939458,
		// Token: 0x04003359 RID: 13145
		AverageCount64 = 1073874176,
		// Token: 0x0400335A RID: 13146
		SampleFraction = 549585920,
		// Token: 0x0400335B RID: 13147
		SampleCounter = 4260864,
		// Token: 0x0400335C RID: 13148
		SampleBase = 1073939457,
		// Token: 0x0400335D RID: 13149
		CounterTimer = 541132032,
		// Token: 0x0400335E RID: 13150
		CounterTimerInverse = 557909248,
		// Token: 0x0400335F RID: 13151
		Timer100Ns = 542180608,
		// Token: 0x04003360 RID: 13152
		Timer100NsInverse = 558957824,
		// Token: 0x04003361 RID: 13153
		ElapsedTime = 807666944,
		// Token: 0x04003362 RID: 13154
		CounterMultiTimer = 574686464,
		// Token: 0x04003363 RID: 13155
		CounterMultiTimerInverse = 591463680,
		// Token: 0x04003364 RID: 13156
		CounterMultiTimer100Ns = 575735040,
		// Token: 0x04003365 RID: 13157
		CounterMultiTimer100NsInverse = 592512256,
		// Token: 0x04003366 RID: 13158
		CounterMultiBase = 1107494144,
		// Token: 0x04003367 RID: 13159
		CounterDelta32 = 4195328,
		// Token: 0x04003368 RID: 13160
		CounterDelta64 = 4195584
	}
}
