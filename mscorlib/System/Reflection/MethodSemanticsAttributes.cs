using System;

namespace System.Reflection
{
	// Token: 0x0200031A RID: 794
	[Flags]
	[Serializable]
	internal enum MethodSemanticsAttributes
	{
		// Token: 0x04000BCE RID: 3022
		Setter = 1,
		// Token: 0x04000BCF RID: 3023
		Getter = 2,
		// Token: 0x04000BD0 RID: 3024
		Other = 4,
		// Token: 0x04000BD1 RID: 3025
		AddOn = 8,
		// Token: 0x04000BD2 RID: 3026
		RemoveOn = 16,
		// Token: 0x04000BD3 RID: 3027
		Fire = 32
	}
}
