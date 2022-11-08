using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public class DriverInternalErrorException : GraphicsException
	{
		// Token: 0x0600004B RID: 75 RVA: 0x0005692C File Offset: 0x00055D2C
		protected DriverInternalErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00056910 File Offset: 0x00055D10
		public DriverInternalErrorException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000568F4 File Offset: 0x00055CF4
		public DriverInternalErrorException(string message) : base(message)
		{
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000568DC File Offset: 0x00055CDC
		public DriverInternalErrorException()
		{
		}
	}
}
