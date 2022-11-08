using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public class NotFoundException : GraphicsException
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00056998 File Offset: 0x00055D98
		protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0005697C File Offset: 0x00055D7C
		public NotFoundException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00056960 File Offset: 0x00055D60
		public NotFoundException(string message) : base(message)
		{
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00056948 File Offset: 0x00055D48
		public NotFoundException()
		{
		}
	}
}
