using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public class ConflictingTextureFilterException : GraphicsException
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00056710 File Offset: 0x00055B10
		protected ConflictingTextureFilterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000566F4 File Offset: 0x00055AF4
		public ConflictingTextureFilterException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000566D8 File Offset: 0x00055AD8
		public ConflictingTextureFilterException(string message) : base(message)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000566C0 File Offset: 0x00055AC0
		public ConflictingTextureFilterException()
		{
		}
	}
}
