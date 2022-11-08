using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200005E RID: 94
	[Flags]
	public enum Usage
	{
		// Token: 0x04000DA9 RID: 3497
		QueryWrapAndMip = 2097152,
		// Token: 0x04000DAA RID: 3498
		QueryVertexTexture = 1048576,
		// Token: 0x04000DAB RID: 3499
		QueryPostPixelShaderBlending = 524288,
		// Token: 0x04000DAC RID: 3500
		QuerySrgbWrite = 262144,
		// Token: 0x04000DAD RID: 3501
		QueryFilter = 131072,
		// Token: 0x04000DAE RID: 3502
		QuerySrgbRead = 65536,
		// Token: 0x04000DAF RID: 3503
		QueryLegacyBumpMap = 32768,
		// Token: 0x04000DB0 RID: 3504
		QueryDisplacementMap = 16384,
		// Token: 0x04000DB1 RID: 3505
		AutoGenerateMipMap = 1024,
		// Token: 0x04000DB2 RID: 3506
		Dynamic = 512,
		// Token: 0x04000DB3 RID: 3507
		NPatches = 256,
		// Token: 0x04000DB4 RID: 3508
		RTPatches = 128,
		// Token: 0x04000DB5 RID: 3509
		Points = 64,
		// Token: 0x04000DB6 RID: 3510
		DoNotClip = 32,
		// Token: 0x04000DB7 RID: 3511
		SoftwareProcessing = 16,
		// Token: 0x04000DB8 RID: 3512
		WriteOnly = 8,
		// Token: 0x04000DB9 RID: 3513
		DepthStencil = 2,
		// Token: 0x04000DBA RID: 3514
		RenderTarget = 1,
		// Token: 0x04000DBB RID: 3515
		None = 0
	}
}
