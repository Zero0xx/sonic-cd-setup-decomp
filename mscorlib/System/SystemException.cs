using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000037 RID: 55
	[ComVisible(true)]
	[Serializable]
	public class SystemException : Exception
	{
		// Token: 0x06000373 RID: 883 RVA: 0x0000E2A2 File Offset: 0x0000D2A2
		public SystemException() : base(Environment.GetResourceString("Arg_SystemException"))
		{
			base.SetErrorCode(-2146233087);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000E2BF File Offset: 0x0000D2BF
		public SystemException(string message) : base(message)
		{
			base.SetErrorCode(-2146233087);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000E2D3 File Offset: 0x0000D2D3
		public SystemException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233087);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000E2E8 File Offset: 0x0000D2E8
		protected SystemException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
