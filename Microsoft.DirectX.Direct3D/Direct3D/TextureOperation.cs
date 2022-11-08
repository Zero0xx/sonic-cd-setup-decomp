using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000067 RID: 103
	public enum TextureOperation
	{
		// Token: 0x04000DF8 RID: 3576
		Lerp = 26,
		// Token: 0x04000DF9 RID: 3577
		MultiplyAdd = 25,
		// Token: 0x04000DFA RID: 3578
		DotProduct3 = 24,
		// Token: 0x04000DFB RID: 3579
		BumpEnvironmentMapLuminance = 23,
		// Token: 0x04000DFC RID: 3580
		BumpEnvironmentMap = 22,
		// Token: 0x04000DFD RID: 3581
		ModulateInvColorAddAlpha = 21,
		// Token: 0x04000DFE RID: 3582
		ModulateInvAlphaAddColor = 20,
		// Token: 0x04000DFF RID: 3583
		ModulateColorAddAlpha = 19,
		// Token: 0x04000E00 RID: 3584
		ModulateAlphaAddColor = 18,
		// Token: 0x04000E01 RID: 3585
		PreModulate = 17,
		// Token: 0x04000E02 RID: 3586
		BlendCurrentAlpha = 16,
		// Token: 0x04000E03 RID: 3587
		BlendTextureAlphaPM = 15,
		// Token: 0x04000E04 RID: 3588
		BlendFactorAlpha = 14,
		// Token: 0x04000E05 RID: 3589
		BlendTextureAlpha = 13,
		// Token: 0x04000E06 RID: 3590
		BlendDiffuseAlpha = 12,
		// Token: 0x04000E07 RID: 3591
		AddSmooth = 11,
		// Token: 0x04000E08 RID: 3592
		Subtract = 10,
		// Token: 0x04000E09 RID: 3593
		AddSigned2X = 9,
		// Token: 0x04000E0A RID: 3594
		AddSigned = 8,
		// Token: 0x04000E0B RID: 3595
		Add = 7,
		// Token: 0x04000E0C RID: 3596
		Modulate4X = 6,
		// Token: 0x04000E0D RID: 3597
		Modulate2X = 5,
		// Token: 0x04000E0E RID: 3598
		Modulate = 4,
		// Token: 0x04000E0F RID: 3599
		SelectArg2 = 3,
		// Token: 0x04000E10 RID: 3600
		SelectArg1 = 2,
		// Token: 0x04000E11 RID: 3601
		Disable = 1
	}
}
