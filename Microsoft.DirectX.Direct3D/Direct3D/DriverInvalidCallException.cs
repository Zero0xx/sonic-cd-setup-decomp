using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200001B RID: 27
	[Serializable]
	public class DriverInvalidCallException : GraphicsException
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00056CF8 File Offset: 0x000560F8
		protected DriverInvalidCallException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00056CDC File Offset: 0x000560DC
		public DriverInvalidCallException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00056CC0 File Offset: 0x000560C0
		public DriverInvalidCallException(string message) : base(message)
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00056CA8 File Offset: 0x000560A8
		public DriverInvalidCallException()
		{
		}
	}
}
