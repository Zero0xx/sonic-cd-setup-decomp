using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050D RID: 1293
	[ComVisible(true)]
	[Serializable]
	public enum GCHandleType
	{
		// Token: 0x040019BD RID: 6589
		Weak,
		// Token: 0x040019BE RID: 6590
		WeakTrackResurrection,
		// Token: 0x040019BF RID: 6591
		Normal,
		// Token: 0x040019C0 RID: 6592
		Pinned
	}
}
