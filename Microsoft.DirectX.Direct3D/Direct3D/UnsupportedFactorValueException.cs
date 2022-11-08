using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200000E RID: 14
	[Serializable]
	public class UnsupportedFactorValueException : GraphicsException
	{
		// Token: 0x0600003B RID: 59 RVA: 0x0005677C File Offset: 0x00055B7C
		protected UnsupportedFactorValueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00056760 File Offset: 0x00055B60
		public UnsupportedFactorValueException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00056744 File Offset: 0x00055B44
		public UnsupportedFactorValueException(string message) : base(message)
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0005672C File Offset: 0x00055B2C
		public UnsupportedFactorValueException()
		{
		}
	}
}
