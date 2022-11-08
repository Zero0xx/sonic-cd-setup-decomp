using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public class MoreDataException : GraphicsException
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00056A04 File Offset: 0x00055E04
		protected MoreDataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000569E8 File Offset: 0x00055DE8
		public MoreDataException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000569CC File Offset: 0x00055DCC
		public MoreDataException(string message) : base(message)
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000569B4 File Offset: 0x00055DB4
		public MoreDataException()
		{
		}
	}
}
