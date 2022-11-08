using System;

namespace System.Globalization
{
	// Token: 0x020003B8 RID: 952
	[Serializable]
	internal class EraInfo
	{
		// Token: 0x060025EC RID: 9708 RVA: 0x0006A1BA File Offset: 0x000691BA
		internal EraInfo(int era, long ticks, int yearOffset, int minEraYear, int maxEraYear)
		{
			this.era = era;
			this.ticks = ticks;
			this.yearOffset = yearOffset;
			this.minEraYear = minEraYear;
			this.maxEraYear = maxEraYear;
		}

		// Token: 0x0400113E RID: 4414
		internal int era;

		// Token: 0x0400113F RID: 4415
		internal long ticks;

		// Token: 0x04001140 RID: 4416
		internal int yearOffset;

		// Token: 0x04001141 RID: 4417
		internal int minEraYear;

		// Token: 0x04001142 RID: 4418
		internal int maxEraYear;
	}
}
