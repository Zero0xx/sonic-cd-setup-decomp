using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000530 RID: 1328
	[Flags]
	public enum RegistrationConnectionType
	{
		// Token: 0x04001A30 RID: 6704
		SingleUse = 0,
		// Token: 0x04001A31 RID: 6705
		MultipleUse = 1,
		// Token: 0x04001A32 RID: 6706
		MultiSeparate = 2,
		// Token: 0x04001A33 RID: 6707
		Suspended = 4,
		// Token: 0x04001A34 RID: 6708
		Surrogate = 8
	}
}
