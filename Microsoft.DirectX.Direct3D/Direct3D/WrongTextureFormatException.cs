using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class WrongTextureFormatException : GraphicsException
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00056488 File Offset: 0x00055888
		protected WrongTextureFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0005646C File Offset: 0x0005586C
		public WrongTextureFormatException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00056450 File Offset: 0x00055850
		public WrongTextureFormatException(string message) : base(message)
		{
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00056438 File Offset: 0x00055838
		public WrongTextureFormatException()
		{
		}
	}
}
