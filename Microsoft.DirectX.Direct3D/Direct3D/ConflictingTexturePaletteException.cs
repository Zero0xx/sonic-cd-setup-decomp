using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000011 RID: 17
	[Serializable]
	public class ConflictingTexturePaletteException : GraphicsException
	{
		// Token: 0x06000047 RID: 71 RVA: 0x000568C0 File Offset: 0x00055CC0
		protected ConflictingTexturePaletteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000568A4 File Offset: 0x00055CA4
		public ConflictingTexturePaletteException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00056888 File Offset: 0x00055C88
		public ConflictingTexturePaletteException(string message) : base(message)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00056870 File Offset: 0x00055C70
		public ConflictingTexturePaletteException()
		{
		}
	}
}
