using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E9 RID: 1257
	[ComVisible(true)]
	[Obsolete("The IDispatchImplAttribute is deprecated.", false)]
	[Serializable]
	public enum IDispatchImplType
	{
		// Token: 0x04001903 RID: 6403
		SystemDefinedImpl,
		// Token: 0x04001904 RID: 6404
		InternalImpl,
		// Token: 0x04001905 RID: 6405
		CompatibleImpl
	}
}
