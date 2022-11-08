using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class UnsupportedAlphaArgumentException : GraphicsException
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00056638 File Offset: 0x00055A38
		protected UnsupportedAlphaArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0005661C File Offset: 0x00055A1C
		public UnsupportedAlphaArgumentException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00056600 File Offset: 0x00055A00
		public UnsupportedAlphaArgumentException(string message) : base(message)
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000565E8 File Offset: 0x000559E8
		public UnsupportedAlphaArgumentException()
		{
		}
	}
}
