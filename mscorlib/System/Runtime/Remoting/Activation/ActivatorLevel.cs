using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006DE RID: 1758
	[ComVisible(true)]
	[Serializable]
	public enum ActivatorLevel
	{
		// Token: 0x0400200F RID: 8207
		Construction = 4,
		// Token: 0x04002010 RID: 8208
		Context = 8,
		// Token: 0x04002011 RID: 8209
		AppDomain = 12,
		// Token: 0x04002012 RID: 8210
		Process = 16,
		// Token: 0x04002013 RID: 8211
		Machine = 20
	}
}
