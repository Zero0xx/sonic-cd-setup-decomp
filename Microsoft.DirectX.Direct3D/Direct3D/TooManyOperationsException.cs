using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class TooManyOperationsException : GraphicsException
	{
		// Token: 0x06000033 RID: 51 RVA: 0x000566A4 File Offset: 0x00055AA4
		protected TooManyOperationsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00056688 File Offset: 0x00055A88
		public TooManyOperationsException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0005666C File Offset: 0x00055A6C
		public TooManyOperationsException(string message) : base(message)
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00056654 File Offset: 0x00055A54
		public TooManyOperationsException()
		{
		}
	}
}
