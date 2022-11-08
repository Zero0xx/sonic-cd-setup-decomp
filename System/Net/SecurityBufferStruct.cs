using System;

namespace System.Net
{
	// Token: 0x02000406 RID: 1030
	internal struct SecurityBufferStruct
	{
		// Token: 0x04002087 RID: 8327
		public int count;

		// Token: 0x04002088 RID: 8328
		public BufferType type;

		// Token: 0x04002089 RID: 8329
		public IntPtr token;

		// Token: 0x0400208A RID: 8330
		public static readonly int Size = sizeof(SecurityBufferStruct);
	}
}
