using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003CF RID: 975
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum NumberStyles
	{
		// Token: 0x04001243 RID: 4675
		None = 0,
		// Token: 0x04001244 RID: 4676
		AllowLeadingWhite = 1,
		// Token: 0x04001245 RID: 4677
		AllowTrailingWhite = 2,
		// Token: 0x04001246 RID: 4678
		AllowLeadingSign = 4,
		// Token: 0x04001247 RID: 4679
		AllowTrailingSign = 8,
		// Token: 0x04001248 RID: 4680
		AllowParentheses = 16,
		// Token: 0x04001249 RID: 4681
		AllowDecimalPoint = 32,
		// Token: 0x0400124A RID: 4682
		AllowThousands = 64,
		// Token: 0x0400124B RID: 4683
		AllowExponent = 128,
		// Token: 0x0400124C RID: 4684
		AllowCurrencySymbol = 256,
		// Token: 0x0400124D RID: 4685
		AllowHexSpecifier = 512,
		// Token: 0x0400124E RID: 4686
		Integer = 7,
		// Token: 0x0400124F RID: 4687
		HexNumber = 515,
		// Token: 0x04001250 RID: 4688
		Number = 111,
		// Token: 0x04001251 RID: 4689
		Float = 167,
		// Token: 0x04001252 RID: 4690
		Currency = 383,
		// Token: 0x04001253 RID: 4691
		Any = 511
	}
}
