using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000745 RID: 1861
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum SoapOption
	{
		// Token: 0x04002162 RID: 8546
		None = 0,
		// Token: 0x04002163 RID: 8547
		AlwaysIncludeTypes = 1,
		// Token: 0x04002164 RID: 8548
		XsdString = 2,
		// Token: 0x04002165 RID: 8549
		EmbedAll = 4,
		// Token: 0x04002166 RID: 8550
		Option1 = 8,
		// Token: 0x04002167 RID: 8551
		Option2 = 16
	}
}
