using System;

namespace System.ComponentModel
{
	// Token: 0x0200011F RID: 287
	public enum MaskedTextResultHint
	{
		// Token: 0x040009EE RID: 2542
		Unknown,
		// Token: 0x040009EF RID: 2543
		CharacterEscaped,
		// Token: 0x040009F0 RID: 2544
		NoEffect,
		// Token: 0x040009F1 RID: 2545
		SideEffect,
		// Token: 0x040009F2 RID: 2546
		Success,
		// Token: 0x040009F3 RID: 2547
		AsciiCharacterExpected = -1,
		// Token: 0x040009F4 RID: 2548
		AlphanumericCharacterExpected = -2,
		// Token: 0x040009F5 RID: 2549
		DigitExpected = -3,
		// Token: 0x040009F6 RID: 2550
		LetterExpected = -4,
		// Token: 0x040009F7 RID: 2551
		SignedDigitExpected = -5,
		// Token: 0x040009F8 RID: 2552
		InvalidInput = -51,
		// Token: 0x040009F9 RID: 2553
		PromptCharNotAllowed = -52,
		// Token: 0x040009FA RID: 2554
		UnavailableEditPosition = -53,
		// Token: 0x040009FB RID: 2555
		NonEditPosition = -54,
		// Token: 0x040009FC RID: 2556
		PositionOutOfRange = -55
	}
}
