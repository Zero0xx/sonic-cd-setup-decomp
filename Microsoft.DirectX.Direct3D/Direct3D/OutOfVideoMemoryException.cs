using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public class OutOfVideoMemoryException : GraphicsException
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00056BB4 File Offset: 0x00055FB4
		protected OutOfVideoMemoryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00056B98 File Offset: 0x00055F98
		public OutOfVideoMemoryException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00056B7C File Offset: 0x00055F7C
		public OutOfVideoMemoryException(string message) : base(message)
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00056B64 File Offset: 0x00055F64
		public OutOfVideoMemoryException()
		{
		}
	}
}
