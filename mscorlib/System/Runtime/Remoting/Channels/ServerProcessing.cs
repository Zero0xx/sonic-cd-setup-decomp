using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006EA RID: 1770
	[ComVisible(true)]
	[Serializable]
	public enum ServerProcessing
	{
		// Token: 0x04002015 RID: 8213
		Complete,
		// Token: 0x04002016 RID: 8214
		OneWay,
		// Token: 0x04002017 RID: 8215
		Async
	}
}
