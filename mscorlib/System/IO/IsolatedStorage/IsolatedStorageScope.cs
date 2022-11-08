using System;
using System.Runtime.InteropServices;

namespace System.IO.IsolatedStorage
{
	// Token: 0x020007AA RID: 1962
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum IsolatedStorageScope
	{
		// Token: 0x040022A7 RID: 8871
		None = 0,
		// Token: 0x040022A8 RID: 8872
		User = 1,
		// Token: 0x040022A9 RID: 8873
		Domain = 2,
		// Token: 0x040022AA RID: 8874
		Assembly = 4,
		// Token: 0x040022AB RID: 8875
		Roaming = 8,
		// Token: 0x040022AC RID: 8876
		Machine = 16,
		// Token: 0x040022AD RID: 8877
		Application = 32
	}
}
