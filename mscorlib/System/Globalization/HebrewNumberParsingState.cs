using System;

namespace System.Globalization
{
	// Token: 0x020003E3 RID: 995
	internal enum HebrewNumberParsingState
	{
		// Token: 0x040013C9 RID: 5065
		InvalidHebrewNumber,
		// Token: 0x040013CA RID: 5066
		NotHebrewDigit,
		// Token: 0x040013CB RID: 5067
		FoundEndOfHebrewNumber,
		// Token: 0x040013CC RID: 5068
		ContinueParsing
	}
}
