using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class NotAvailableException : GraphicsException
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00056B48 File Offset: 0x00055F48
		protected NotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00056B2C File Offset: 0x00055F2C
		public NotAvailableException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00056B10 File Offset: 0x00055F10
		public NotAvailableException(string message) : base(message)
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00056AF8 File Offset: 0x00055EF8
		public NotAvailableException()
		{
		}
	}
}
