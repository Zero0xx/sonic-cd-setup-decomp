using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000074 RID: 116
	[Flags]
	public enum PresentFlag
	{
		// Token: 0x04000E71 RID: 3697
		Video = 16,
		// Token: 0x04000E72 RID: 3698
		DeviceClip = 4,
		// Token: 0x04000E73 RID: 3699
		DiscardDepthStencil = 2,
		// Token: 0x04000E74 RID: 3700
		LockableBackBuffer = 1,
		// Token: 0x04000E75 RID: 3701
		None = 0
	}
}
