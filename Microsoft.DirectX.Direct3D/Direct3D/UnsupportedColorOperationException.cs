using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class UnsupportedColorOperationException : GraphicsException
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000564F4 File Offset: 0x000558F4
		protected UnsupportedColorOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000564D8 File Offset: 0x000558D8
		public UnsupportedColorOperationException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000564BC File Offset: 0x000558BC
		public UnsupportedColorOperationException(string message) : base(message)
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000564A4 File Offset: 0x000558A4
		public UnsupportedColorOperationException()
		{
		}
	}
}
