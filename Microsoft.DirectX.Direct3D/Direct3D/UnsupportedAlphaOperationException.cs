using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class UnsupportedAlphaOperationException : GraphicsException
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000565CC File Offset: 0x000559CC
		protected UnsupportedAlphaOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000565B0 File Offset: 0x000559B0
		public UnsupportedAlphaOperationException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00056594 File Offset: 0x00055994
		public UnsupportedAlphaOperationException(string message) : base(message)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0005657C File Offset: 0x0005597C
		public UnsupportedAlphaOperationException()
		{
		}
	}
}
