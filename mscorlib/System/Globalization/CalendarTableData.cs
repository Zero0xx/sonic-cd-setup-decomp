using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003D9 RID: 985
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct CalendarTableData
	{
		// Token: 0x04001313 RID: 4883
		internal const int sizeofDataFields = 72;

		// Token: 0x04001314 RID: 4884
		internal ushort iCalendar;

		// Token: 0x04001315 RID: 4885
		internal ushort iTwoDigitYearMax;

		// Token: 0x04001316 RID: 4886
		internal uint saShortDate;

		// Token: 0x04001317 RID: 4887
		internal uint saYearMonth;

		// Token: 0x04001318 RID: 4888
		internal uint saLongDate;

		// Token: 0x04001319 RID: 4889
		internal uint saEraNames;

		// Token: 0x0400131A RID: 4890
		internal uint waaEraRanges;

		// Token: 0x0400131B RID: 4891
		internal uint saDayNames;

		// Token: 0x0400131C RID: 4892
		internal uint saAbbrevDayNames;

		// Token: 0x0400131D RID: 4893
		internal uint saMonthNames;

		// Token: 0x0400131E RID: 4894
		internal uint saAbbrevMonthNames;

		// Token: 0x0400131F RID: 4895
		internal ushort iCurrentEra;

		// Token: 0x04001320 RID: 4896
		internal ushort iFormatFlags;

		// Token: 0x04001321 RID: 4897
		internal uint sName;

		// Token: 0x04001322 RID: 4898
		internal uint sMonthDay;

		// Token: 0x04001323 RID: 4899
		internal uint saAbbrevEraNames;

		// Token: 0x04001324 RID: 4900
		internal uint saAbbrevEnglishEraNames;

		// Token: 0x04001325 RID: 4901
		internal uint saLeapYearMonthNames;

		// Token: 0x04001326 RID: 4902
		internal uint saSuperShortDayNames;

		// Token: 0x04001327 RID: 4903
		internal ushort _padding1;

		// Token: 0x04001328 RID: 4904
		internal ushort _padding2;
	}
}
