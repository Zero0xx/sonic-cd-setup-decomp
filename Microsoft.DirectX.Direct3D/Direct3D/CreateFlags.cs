using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000030 RID: 48
	[Flags]
	public enum CreateFlags
	{
		// Token: 0x04000D34 RID: 3380
		NoWindowChanges = 2048,
		// Token: 0x04000D35 RID: 3381
		DisableDriverManagementEx = 1024,
		// Token: 0x04000D36 RID: 3382
		AdapterGroupDevice = 512,
		// Token: 0x04000D37 RID: 3383
		DisableDriverManagement = 256,
		// Token: 0x04000D38 RID: 3384
		MixedVertexProcessing = 128,
		// Token: 0x04000D39 RID: 3385
		HardwareVertexProcessing = 64,
		// Token: 0x04000D3A RID: 3386
		SoftwareVertexProcessing = 32,
		// Token: 0x04000D3B RID: 3387
		PureDevice = 16,
		// Token: 0x04000D3C RID: 3388
		MultiThreaded = 4,
		// Token: 0x04000D3D RID: 3389
		FpuPreserve = 2
	}
}
