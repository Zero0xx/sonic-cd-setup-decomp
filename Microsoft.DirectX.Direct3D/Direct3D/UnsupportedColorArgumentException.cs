using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class UnsupportedColorArgumentException : GraphicsException
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00056560 File Offset: 0x00055960
		protected UnsupportedColorArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00056544 File Offset: 0x00055944
		public UnsupportedColorArgumentException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00056528 File Offset: 0x00055928
		public UnsupportedColorArgumentException(string message) : base(message)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00056510 File Offset: 0x00055910
		public UnsupportedColorArgumentException()
		{
		}
	}
}
