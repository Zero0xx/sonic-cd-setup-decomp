using System;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public class InvalidDeviceException : GraphicsException
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00056C20 File Offset: 0x00056020
		protected InvalidDeviceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00056C04 File Offset: 0x00056004
		public InvalidDeviceException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00056BE8 File Offset: 0x00055FE8
		public InvalidDeviceException(string message) : base(message)
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00056BD0 File Offset: 0x00055FD0
		public InvalidDeviceException()
		{
		}
	}
}
