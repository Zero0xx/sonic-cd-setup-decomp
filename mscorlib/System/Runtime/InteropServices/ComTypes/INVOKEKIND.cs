using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000592 RID: 1426
	[Flags]
	[Serializable]
	public enum INVOKEKIND
	{
		// Token: 0x04001BA5 RID: 7077
		INVOKE_FUNC = 1,
		// Token: 0x04001BA6 RID: 7078
		INVOKE_PROPERTYGET = 2,
		// Token: 0x04001BA7 RID: 7079
		INVOKE_PROPERTYPUT = 4,
		// Token: 0x04001BA8 RID: 7080
		INVOKE_PROPERTYPUTREF = 8
	}
}
