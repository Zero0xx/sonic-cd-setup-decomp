using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200030D RID: 781
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum EventAttributes
	{
		// Token: 0x04000B4A RID: 2890
		None = 0,
		// Token: 0x04000B4B RID: 2891
		SpecialName = 512,
		// Token: 0x04000B4C RID: 2892
		ReservedMask = 1024,
		// Token: 0x04000B4D RID: 2893
		RTSpecialName = 1024
	}
}
