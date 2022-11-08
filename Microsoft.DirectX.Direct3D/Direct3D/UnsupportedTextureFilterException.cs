using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class UnsupportedTextureFilterException : GraphicsException
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00056854 File Offset: 0x00055C54
		protected UnsupportedTextureFilterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00056838 File Offset: 0x00055C38
		public UnsupportedTextureFilterException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0005681C File Offset: 0x00055C1C
		public UnsupportedTextureFilterException(string message) : base(message)
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00056804 File Offset: 0x00055C04
		public UnsupportedTextureFilterException()
		{
		}
	}
}
