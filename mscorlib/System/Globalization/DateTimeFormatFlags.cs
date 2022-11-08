using System;

namespace System.Globalization
{
	// Token: 0x020003A8 RID: 936
	[Flags]
	internal enum DateTimeFormatFlags
	{
		// Token: 0x04001074 RID: 4212
		None = 0,
		// Token: 0x04001075 RID: 4213
		UseGenitiveMonth = 1,
		// Token: 0x04001076 RID: 4214
		UseLeapYearMonth = 2,
		// Token: 0x04001077 RID: 4215
		UseSpacesInMonthNames = 4,
		// Token: 0x04001078 RID: 4216
		UseHebrewRule = 8,
		// Token: 0x04001079 RID: 4217
		UseSpacesInDayNames = 16,
		// Token: 0x0400107A RID: 4218
		UseDigitPrefixInTokens = 32,
		// Token: 0x0400107B RID: 4219
		NotInitialized = -1
	}
}
