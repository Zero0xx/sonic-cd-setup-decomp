using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000E8 RID: 232
	public enum RenderStates
	{
		// Token: 0x04000F86 RID: 3974
		BlendOperationAlpha = 209,
		// Token: 0x04000F87 RID: 3975
		DestinationBlendAlpha = 208,
		// Token: 0x04000F88 RID: 3976
		SourceBlendAlpha = 207,
		// Token: 0x04000F89 RID: 3977
		SeparateAlphaBlendEnable = 206,
		// Token: 0x04000F8A RID: 3978
		Wrap15 = 205,
		// Token: 0x04000F8B RID: 3979
		Wrap14 = 204,
		// Token: 0x04000F8C RID: 3980
		Wrap13 = 203,
		// Token: 0x04000F8D RID: 3981
		Wrap12 = 202,
		// Token: 0x04000F8E RID: 3982
		Wrap11 = 201,
		// Token: 0x04000F8F RID: 3983
		Wrap10 = 200,
		// Token: 0x04000F90 RID: 3984
		Wrap9 = 199,
		// Token: 0x04000F91 RID: 3985
		DepthBias = 195,
		// Token: 0x04000F92 RID: 3986
		SrgbWriteEnable = 194,
		// Token: 0x04000F93 RID: 3987
		ColorWriteEnable3 = 192,
		// Token: 0x04000F94 RID: 3988
		ColorWriteEnable2 = 191,
		// Token: 0x04000F95 RID: 3989
		ColorWriteEnable1 = 190,
		// Token: 0x04000F96 RID: 3990
		CounterClockwiseStencilFunction = 189,
		// Token: 0x04000F97 RID: 3991
		CounterClockwiseStencilPass = 188,
		// Token: 0x04000F98 RID: 3992
		CounterClockwiseStencilZBufferFail = 187,
		// Token: 0x04000F99 RID: 3993
		CounterClockwiseStencilFail = 186,
		// Token: 0x04000F9A RID: 3994
		TwoSidedStencilMode = 185,
		// Token: 0x04000F9B RID: 3995
		EnableAdaptiveTessellation = 184,
		// Token: 0x04000F9C RID: 3996
		AdaptiveTessellateW = 183,
		// Token: 0x04000F9D RID: 3997
		AdaptiveTessellateZ = 182,
		// Token: 0x04000F9E RID: 3998
		AdaptiveTessellateY = 181,
		// Token: 0x04000F9F RID: 3999
		AdaptiveTessellateX = 180,
		// Token: 0x04000FA0 RID: 4000
		MaxTessellationLevel = 179,
		// Token: 0x04000FA1 RID: 4001
		MinTessellationLevel = 178,
		// Token: 0x04000FA2 RID: 4002
		AntialiasedLineEnable = 176,
		// Token: 0x04000FA3 RID: 4003
		SlopeScaleDepthBias = 175,
		// Token: 0x04000FA4 RID: 4004
		ScissorTestEnable = 174,
		// Token: 0x04000FA5 RID: 4005
		NormalDegree = 173,
		// Token: 0x04000FA6 RID: 4006
		PositionDegree = 172,
		// Token: 0x04000FA7 RID: 4007
		TweenFactor = 170,
		// Token: 0x04000FA8 RID: 4008
		IndexedVertexBlendEnable = 167,
		// Token: 0x04000FA9 RID: 4009
		PointSizeMax = 166,
		// Token: 0x04000FAA RID: 4010
		DebugMonitorToken = 165,
		// Token: 0x04000FAB RID: 4011
		PatchEdgeStyle = 163,
		// Token: 0x04000FAC RID: 4012
		MultisampleMask = 162,
		// Token: 0x04000FAD RID: 4013
		MultisampleAntiAlias = 161,
		// Token: 0x04000FAE RID: 4014
		PointScaleC = 160,
		// Token: 0x04000FAF RID: 4015
		PointScaleB = 159,
		// Token: 0x04000FB0 RID: 4016
		PointScaleA = 158,
		// Token: 0x04000FB1 RID: 4017
		PointScaleEnable = 157,
		// Token: 0x04000FB2 RID: 4018
		PointSpriteEnable = 156,
		// Token: 0x04000FB3 RID: 4019
		PointSizeMin = 155,
		// Token: 0x04000FB4 RID: 4020
		ClipPlaneEnable = 152,
		// Token: 0x04000FB5 RID: 4021
		EmissiveMaterialSource = 148,
		// Token: 0x04000FB6 RID: 4022
		AmbientMaterialSource = 147,
		// Token: 0x04000FB7 RID: 4023
		SpecularMaterialSource = 146,
		// Token: 0x04000FB8 RID: 4024
		DiffuseMaterialSource = 145,
		// Token: 0x04000FB9 RID: 4025
		NormalizeNormals = 143,
		// Token: 0x04000FBA RID: 4026
		LocalViewer = 142,
		// Token: 0x04000FBB RID: 4027
		ColorVertex = 141,
		// Token: 0x04000FBC RID: 4028
		FogVertexMode = 140,
		// Token: 0x04000FBD RID: 4029
		Lighting = 137,
		// Token: 0x04000FBE RID: 4030
		Clipping = 136,
		// Token: 0x04000FBF RID: 4031
		FogColor = 34,
		// Token: 0x04000FC0 RID: 4032
		FillMode = 8,
		// Token: 0x04000FC1 RID: 4033
		ZEnable = 7,
		// Token: 0x04000FC2 RID: 4034
		Wrap8 = 198,
		// Token: 0x04000FC3 RID: 4035
		BlendFactor = 193,
		// Token: 0x04000FC4 RID: 4036
		BlendOperation = 171,
		// Token: 0x04000FC5 RID: 4037
		ColorWriteEnable = 168,
		// Token: 0x04000FC6 RID: 4038
		VertexBlend = 151,
		// Token: 0x04000FC7 RID: 4039
		Wrap7 = 135,
		// Token: 0x04000FC8 RID: 4040
		Wrap6 = 134,
		// Token: 0x04000FC9 RID: 4041
		Wrap5 = 133,
		// Token: 0x04000FCA RID: 4042
		Wrap4 = 132,
		// Token: 0x04000FCB RID: 4043
		Wrap3 = 131,
		// Token: 0x04000FCC RID: 4044
		Wrap2 = 130,
		// Token: 0x04000FCD RID: 4045
		Wrap1 = 129,
		// Token: 0x04000FCE RID: 4046
		Wrap0 = 128,
		// Token: 0x04000FCF RID: 4047
		TextureFactor = 60,
		// Token: 0x04000FD0 RID: 4048
		StencilWriteMask = 59,
		// Token: 0x04000FD1 RID: 4049
		StencilMask = 58,
		// Token: 0x04000FD2 RID: 4050
		ReferenceStencil = 57,
		// Token: 0x04000FD3 RID: 4051
		StencilFunction = 56,
		// Token: 0x04000FD4 RID: 4052
		StencilPass = 55,
		// Token: 0x04000FD5 RID: 4053
		StencilZBufferFail = 54,
		// Token: 0x04000FD6 RID: 4054
		StencilFail = 53,
		// Token: 0x04000FD7 RID: 4055
		StencilEnable = 52,
		// Token: 0x04000FD8 RID: 4056
		RangeFogEnable = 48,
		// Token: 0x04000FD9 RID: 4057
		FogDensity = 38,
		// Token: 0x04000FDA RID: 4058
		FogEnd = 37,
		// Token: 0x04000FDB RID: 4059
		FogStart = 36,
		// Token: 0x04000FDC RID: 4060
		FogTableMode = 35,
		// Token: 0x04000FDD RID: 4061
		SpecularEnable = 29,
		// Token: 0x04000FDE RID: 4062
		FogEnable = 28,
		// Token: 0x04000FDF RID: 4063
		AlphaBlendEnable = 27,
		// Token: 0x04000FE0 RID: 4064
		DitherEnable = 26,
		// Token: 0x04000FE1 RID: 4065
		AlphaFunction = 25,
		// Token: 0x04000FE2 RID: 4066
		ReferenceAlpha = 24,
		// Token: 0x04000FE3 RID: 4067
		ZBufferFunction = 23,
		// Token: 0x04000FE4 RID: 4068
		CullMode = 22,
		// Token: 0x04000FE5 RID: 4069
		DestinationBlend = 20,
		// Token: 0x04000FE6 RID: 4070
		SourceBlend = 19,
		// Token: 0x04000FE7 RID: 4071
		LastPixel = 16,
		// Token: 0x04000FE8 RID: 4072
		AlphaTestEnable = 15,
		// Token: 0x04000FE9 RID: 4073
		ZBufferWriteEnable = 14,
		// Token: 0x04000FEA RID: 4074
		ShadeMode = 9,
		// Token: 0x04000FEB RID: 4075
		PointSize = 154,
		// Token: 0x04000FEC RID: 4076
		Ambient = 139
	}
}
