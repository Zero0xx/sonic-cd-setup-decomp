using System;

namespace System.Reflection
{
	// Token: 0x02000304 RID: 772
	[Serializable]
	internal enum CustomAttributeEncoding
	{
		// Token: 0x04000B1E RID: 2846
		Undefined,
		// Token: 0x04000B1F RID: 2847
		Boolean = 2,
		// Token: 0x04000B20 RID: 2848
		Char,
		// Token: 0x04000B21 RID: 2849
		SByte,
		// Token: 0x04000B22 RID: 2850
		Byte,
		// Token: 0x04000B23 RID: 2851
		Int16,
		// Token: 0x04000B24 RID: 2852
		UInt16,
		// Token: 0x04000B25 RID: 2853
		Int32,
		// Token: 0x04000B26 RID: 2854
		UInt32,
		// Token: 0x04000B27 RID: 2855
		Int64,
		// Token: 0x04000B28 RID: 2856
		UInt64,
		// Token: 0x04000B29 RID: 2857
		Float,
		// Token: 0x04000B2A RID: 2858
		Double,
		// Token: 0x04000B2B RID: 2859
		String,
		// Token: 0x04000B2C RID: 2860
		Array = 29,
		// Token: 0x04000B2D RID: 2861
		Type = 80,
		// Token: 0x04000B2E RID: 2862
		Object,
		// Token: 0x04000B2F RID: 2863
		Field = 83,
		// Token: 0x04000B30 RID: 2864
		Property,
		// Token: 0x04000B31 RID: 2865
		Enum
	}
}
