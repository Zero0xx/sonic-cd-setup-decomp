using System;
using System.Globalization;

namespace System
{
	// Token: 0x020003A4 RID: 932
	internal struct ParsingInfo
	{
		// Token: 0x0600251C RID: 9500 RVA: 0x00065586 File Offset: 0x00064586
		internal void Init()
		{
			this.dayOfWeek = -1;
			this.timeMark = DateTimeParse.TM.NotSet;
		}

		// Token: 0x0400103A RID: 4154
		internal Calendar calendar;

		// Token: 0x0400103B RID: 4155
		internal int dayOfWeek;

		// Token: 0x0400103C RID: 4156
		internal DateTimeParse.TM timeMark;

		// Token: 0x0400103D RID: 4157
		internal bool fUseHour12;

		// Token: 0x0400103E RID: 4158
		internal bool fUseTwoDigitYear;

		// Token: 0x0400103F RID: 4159
		internal bool fAllowInnerWhite;

		// Token: 0x04001040 RID: 4160
		internal bool fAllowTrailingWhite;

		// Token: 0x04001041 RID: 4161
		internal bool fCustomNumberParser;

		// Token: 0x04001042 RID: 4162
		internal DateTimeParse.MatchNumberDelegate parseNumberDelegate;
	}
}
