using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003E0 RID: 992
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct DTFIUserOverrideValues
	{
		// Token: 0x0400137B RID: 4987
		internal string shortDatePattern;

		// Token: 0x0400137C RID: 4988
		internal string longDatePattern;

		// Token: 0x0400137D RID: 4989
		internal string yearMonthPattern;

		// Token: 0x0400137E RID: 4990
		internal string amDesignator;

		// Token: 0x0400137F RID: 4991
		internal string pmDesignator;

		// Token: 0x04001380 RID: 4992
		internal string longTimePattern;

		// Token: 0x04001381 RID: 4993
		internal int firstDayOfWeek;

		// Token: 0x04001382 RID: 4994
		internal int padding1;

		// Token: 0x04001383 RID: 4995
		internal int calendarWeekRule;

		// Token: 0x04001384 RID: 4996
		internal int padding2;
	}
}
