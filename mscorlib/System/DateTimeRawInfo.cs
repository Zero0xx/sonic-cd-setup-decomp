using System;

namespace System
{
	// Token: 0x020003A0 RID: 928
	internal struct DateTimeRawInfo
	{
		// Token: 0x06002515 RID: 9493 RVA: 0x00065498 File Offset: 0x00064498
		internal unsafe void Init(int* numberBuffer)
		{
			this.month = -1;
			this.year = -1;
			this.dayOfWeek = -1;
			this.era = -1;
			this.timeMark = DateTimeParse.TM.NotSet;
			this.fraction = -1.0;
			this.num = numberBuffer;
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x000654D4 File Offset: 0x000644D4
		internal unsafe void AddNumber(int value)
		{
			this.num[(IntPtr)(this.numCount++) * 4] = value;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000654FE File Offset: 0x000644FE
		internal unsafe int GetNumber(int index)
		{
			return this.num[index];
		}

		// Token: 0x0400100B RID: 4107
		private unsafe int* num;

		// Token: 0x0400100C RID: 4108
		internal int numCount;

		// Token: 0x0400100D RID: 4109
		internal int month;

		// Token: 0x0400100E RID: 4110
		internal int year;

		// Token: 0x0400100F RID: 4111
		internal int dayOfWeek;

		// Token: 0x04001010 RID: 4112
		internal int era;

		// Token: 0x04001011 RID: 4113
		internal DateTimeParse.TM timeMark;

		// Token: 0x04001012 RID: 4114
		internal double fraction;

		// Token: 0x04001013 RID: 4115
		internal bool timeZone;
	}
}
