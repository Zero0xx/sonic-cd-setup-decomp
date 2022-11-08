using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class DeviceLostException : GraphicsException
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00056A70 File Offset: 0x00055E70
		protected DeviceLostException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00056A54 File Offset: 0x00055E54
		public DeviceLostException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00056A38 File Offset: 0x00055E38
		public DeviceLostException(string message) : base(message)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00056A20 File Offset: 0x00055E20
		public DeviceLostException()
		{
		}
	}
}
