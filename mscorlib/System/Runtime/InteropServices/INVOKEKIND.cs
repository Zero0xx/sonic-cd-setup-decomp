using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200055E RID: 1374
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.INVOKEKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum INVOKEKIND
	{
		// Token: 0x04001AD4 RID: 6868
		INVOKE_FUNC = 1,
		// Token: 0x04001AD5 RID: 6869
		INVOKE_PROPERTYGET,
		// Token: 0x04001AD6 RID: 6870
		INVOKE_PROPERTYPUT = 4,
		// Token: 0x04001AD7 RID: 6871
		INVOKE_PROPERTYPUTREF = 8
	}
}
