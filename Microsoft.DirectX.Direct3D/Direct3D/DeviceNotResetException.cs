using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public class DeviceNotResetException : GraphicsException
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00056ADC File Offset: 0x00055EDC
		protected DeviceNotResetException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00056AC0 File Offset: 0x00055EC0
		public DeviceNotResetException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00056AA4 File Offset: 0x00055EA4
		public DeviceNotResetException(string message) : base(message)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00056A8C File Offset: 0x00055E8C
		public DeviceNotResetException()
		{
		}
	}
}
