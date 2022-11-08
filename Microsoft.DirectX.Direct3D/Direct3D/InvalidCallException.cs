using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public class InvalidCallException : GraphicsException
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00056C8C File Offset: 0x0005608C
		protected InvalidCallException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00056C70 File Offset: 0x00056070
		public InvalidCallException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00056C54 File Offset: 0x00056054
		public InvalidCallException(string message) : base(message)
		{
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00056C3C File Offset: 0x0005603C
		public InvalidCallException()
		{
		}
	}
}
