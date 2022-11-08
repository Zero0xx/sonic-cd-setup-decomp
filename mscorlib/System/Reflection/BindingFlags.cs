using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F6 RID: 758
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum BindingFlags
	{
		// Token: 0x04000AF6 RID: 2806
		Default = 0,
		// Token: 0x04000AF7 RID: 2807
		IgnoreCase = 1,
		// Token: 0x04000AF8 RID: 2808
		DeclaredOnly = 2,
		// Token: 0x04000AF9 RID: 2809
		Instance = 4,
		// Token: 0x04000AFA RID: 2810
		Static = 8,
		// Token: 0x04000AFB RID: 2811
		Public = 16,
		// Token: 0x04000AFC RID: 2812
		NonPublic = 32,
		// Token: 0x04000AFD RID: 2813
		FlattenHierarchy = 64,
		// Token: 0x04000AFE RID: 2814
		InvokeMethod = 256,
		// Token: 0x04000AFF RID: 2815
		CreateInstance = 512,
		// Token: 0x04000B00 RID: 2816
		GetField = 1024,
		// Token: 0x04000B01 RID: 2817
		SetField = 2048,
		// Token: 0x04000B02 RID: 2818
		GetProperty = 4096,
		// Token: 0x04000B03 RID: 2819
		SetProperty = 8192,
		// Token: 0x04000B04 RID: 2820
		PutDispProperty = 16384,
		// Token: 0x04000B05 RID: 2821
		PutRefDispProperty = 32768,
		// Token: 0x04000B06 RID: 2822
		ExactBinding = 65536,
		// Token: 0x04000B07 RID: 2823
		SuppressChangeType = 131072,
		// Token: 0x04000B08 RID: 2824
		OptionalParamBinding = 262144,
		// Token: 0x04000B09 RID: 2825
		IgnoreReturn = 16777216
	}
}
