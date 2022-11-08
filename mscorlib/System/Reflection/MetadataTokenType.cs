using System;

namespace System.Reflection
{
	// Token: 0x0200031C RID: 796
	[Serializable]
	internal enum MetadataTokenType
	{
		// Token: 0x04000BE8 RID: 3048
		Module,
		// Token: 0x04000BE9 RID: 3049
		TypeRef = 16777216,
		// Token: 0x04000BEA RID: 3050
		TypeDef = 33554432,
		// Token: 0x04000BEB RID: 3051
		FieldDef = 67108864,
		// Token: 0x04000BEC RID: 3052
		MethodDef = 100663296,
		// Token: 0x04000BED RID: 3053
		ParamDef = 134217728,
		// Token: 0x04000BEE RID: 3054
		InterfaceImpl = 150994944,
		// Token: 0x04000BEF RID: 3055
		MemberRef = 167772160,
		// Token: 0x04000BF0 RID: 3056
		CustomAttribute = 201326592,
		// Token: 0x04000BF1 RID: 3057
		Permission = 234881024,
		// Token: 0x04000BF2 RID: 3058
		Signature = 285212672,
		// Token: 0x04000BF3 RID: 3059
		Event = 335544320,
		// Token: 0x04000BF4 RID: 3060
		Property = 385875968,
		// Token: 0x04000BF5 RID: 3061
		ModuleRef = 436207616,
		// Token: 0x04000BF6 RID: 3062
		TypeSpec = 452984832,
		// Token: 0x04000BF7 RID: 3063
		Assembly = 536870912,
		// Token: 0x04000BF8 RID: 3064
		AssemblyRef = 587202560,
		// Token: 0x04000BF9 RID: 3065
		File = 637534208,
		// Token: 0x04000BFA RID: 3066
		ExportedType = 654311424,
		// Token: 0x04000BFB RID: 3067
		ManifestResource = 671088640,
		// Token: 0x04000BFC RID: 3068
		GenericPar = 704643072,
		// Token: 0x04000BFD RID: 3069
		MethodSpec = 721420288,
		// Token: 0x04000BFE RID: 3070
		String = 1879048192,
		// Token: 0x04000BFF RID: 3071
		Name = 1895825408,
		// Token: 0x04000C00 RID: 3072
		BaseType = 1912602624,
		// Token: 0x04000C01 RID: 3073
		Invalid = 2147483647
	}
}
