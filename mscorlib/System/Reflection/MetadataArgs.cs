using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000324 RID: 804
	[Serializable]
	internal static class MetadataArgs
	{
		// Token: 0x04000D0F RID: 3343
		public static MetadataArgs.SkipAddresses Skip = default(MetadataArgs.SkipAddresses);

		// Token: 0x02000325 RID: 805
		[ComVisible(true)]
		[Serializable]
		[StructLayout(LayoutKind.Auto)]
		public struct SkipAddresses
		{
			// Token: 0x04000D10 RID: 3344
			public string String;

			// Token: 0x04000D11 RID: 3345
			public int[] Int32Array;

			// Token: 0x04000D12 RID: 3346
			public byte[] ByteArray;

			// Token: 0x04000D13 RID: 3347
			public MetadataFieldOffset[] MetadataFieldOffsetArray;

			// Token: 0x04000D14 RID: 3348
			public int Int32;

			// Token: 0x04000D15 RID: 3349
			public TypeAttributes TypeAttributes;

			// Token: 0x04000D16 RID: 3350
			public MethodAttributes MethodAttributes;

			// Token: 0x04000D17 RID: 3351
			public PropertyAttributes PropertyAttributes;

			// Token: 0x04000D18 RID: 3352
			public MethodImplAttributes MethodImplAttributes;

			// Token: 0x04000D19 RID: 3353
			public ParameterAttributes ParameterAttributes;

			// Token: 0x04000D1A RID: 3354
			public FieldAttributes FieldAttributes;

			// Token: 0x04000D1B RID: 3355
			public EventAttributes EventAttributes;

			// Token: 0x04000D1C RID: 3356
			public MetadataColumnType MetadataColumnType;

			// Token: 0x04000D1D RID: 3357
			public PInvokeAttributes PInvokeAttributes;

			// Token: 0x04000D1E RID: 3358
			public MethodSemanticsAttributes MethodSemanticsAttributes;

			// Token: 0x04000D1F RID: 3359
			public DeclSecurityAttributes DeclSecurityAttributes;

			// Token: 0x04000D20 RID: 3360
			public CorElementType CorElementType;

			// Token: 0x04000D21 RID: 3361
			public ConstArray ConstArray;

			// Token: 0x04000D22 RID: 3362
			public Guid Guid;
		}
	}
}
