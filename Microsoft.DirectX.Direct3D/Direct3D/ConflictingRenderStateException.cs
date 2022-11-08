using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class ConflictingRenderStateException : GraphicsException
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000567E8 File Offset: 0x00055BE8
		protected ConflictingRenderStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000567CC File Offset: 0x00055BCC
		public ConflictingRenderStateException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000567B0 File Offset: 0x00055BB0
		public ConflictingRenderStateException(string message) : base(message)
		{
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00056798 File Offset: 0x00055B98
		public ConflictingRenderStateException()
		{
		}
	}
}
