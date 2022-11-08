using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200033C RID: 828
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum PropertyAttributes
	{
		// Token: 0x04000DB1 RID: 3505
		None = 0,
		// Token: 0x04000DB2 RID: 3506
		SpecialName = 512,
		// Token: 0x04000DB3 RID: 3507
		ReservedMask = 62464,
		// Token: 0x04000DB4 RID: 3508
		RTSpecialName = 1024,
		// Token: 0x04000DB5 RID: 3509
		HasDefault = 4096,
		// Token: 0x04000DB6 RID: 3510
		Reserved2 = 8192,
		// Token: 0x04000DB7 RID: 3511
		Reserved3 = 16384,
		// Token: 0x04000DB8 RID: 3512
		Reserved4 = 32768
	}
}
