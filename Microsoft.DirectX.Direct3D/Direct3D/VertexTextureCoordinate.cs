using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200002E RID: 46
	public sealed class VertexTextureCoordinate
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00056D80 File Offset: 0x00056180
		private VertexTextureCoordinate()
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00056D98 File Offset: 0x00056198
		public static VertexFormats Size1(int coordIndex)
		{
			return (VertexFormats)(3 << coordIndex * 2 + 16);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00056DB4 File Offset: 0x000561B4
		public static VertexFormats Size2()
		{
			return VertexFormats.Texture0;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00056DC8 File Offset: 0x000561C8
		public static VertexFormats Size3(int coordIndex)
		{
			return (VertexFormats)(1 << coordIndex * 2 + 16);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00056DE4 File Offset: 0x000561E4
		public static VertexFormats Size4(int coordIndex)
		{
			return (VertexFormats)(2 << coordIndex * 2 + 16);
		}
	}
}
