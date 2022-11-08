using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200002D RID: 45
	[Flags]
	public enum VertexFormats
	{
		// Token: 0x04000D13 RID: 3347
		PositionNormal = 18,
		// Token: 0x04000D14 RID: 3348
		LastBetaD3DColor = 32768,
		// Token: 0x04000D15 RID: 3349
		LastBetaUByte4 = 4096,
		// Token: 0x04000D16 RID: 3350
		Texture8 = 2048,
		// Token: 0x04000D17 RID: 3351
		Texture7 = 1792,
		// Token: 0x04000D18 RID: 3352
		Texture6 = 1536,
		// Token: 0x04000D19 RID: 3353
		Texture5 = 1280,
		// Token: 0x04000D1A RID: 3354
		Texture4 = 1024,
		// Token: 0x04000D1B RID: 3355
		Texture3 = 768,
		// Token: 0x04000D1C RID: 3356
		Texture2 = 512,
		// Token: 0x04000D1D RID: 3357
		Texture1 = 256,
		// Token: 0x04000D1E RID: 3358
		Texture0 = 0,
		// Token: 0x04000D1F RID: 3359
		TextureCountShift = 8,
		// Token: 0x04000D20 RID: 3360
		TextureCountMask = 3840,
		// Token: 0x04000D21 RID: 3361
		Specular = 128,
		// Token: 0x04000D22 RID: 3362
		Diffuse = 64,
		// Token: 0x04000D23 RID: 3363
		PointSize = 32,
		// Token: 0x04000D24 RID: 3364
		Normal = 16,
		// Token: 0x04000D25 RID: 3365
		PositionW = 16386,
		// Token: 0x04000D26 RID: 3366
		PositionBlend5 = 14,
		// Token: 0x04000D27 RID: 3367
		PositionBlend4 = 12,
		// Token: 0x04000D28 RID: 3368
		PositionBlend3 = 10,
		// Token: 0x04000D29 RID: 3369
		PositionBlend2 = 8,
		// Token: 0x04000D2A RID: 3370
		PositionBlend1 = 6,
		// Token: 0x04000D2B RID: 3371
		Transformed = 4,
		// Token: 0x04000D2C RID: 3372
		Position = 2,
		// Token: 0x04000D2D RID: 3373
		PositionMask = 16398,
		// Token: 0x04000D2E RID: 3374
		None = 0
	}
}
