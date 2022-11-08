using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	public class WasStillDrawingException : GraphicsException
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00056D64 File Offset: 0x00056164
		protected WasStillDrawingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00056D48 File Offset: 0x00056148
		public WasStillDrawingException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00056D2C File Offset: 0x0005612C
		public WasStillDrawingException(string message) : base(message)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00056D14 File Offset: 0x00056114
		public WasStillDrawingException()
		{
		}
	}
}
