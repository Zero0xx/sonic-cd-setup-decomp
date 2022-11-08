using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200017E RID: 382
	[Serializable]
	public sealed class ThreadStartException : SystemException
	{
		// Token: 0x06001403 RID: 5123 RVA: 0x00035F30 File Offset: 0x00034F30
		private ThreadStartException() : base(Environment.GetResourceString("Arg_ThreadStartException"))
		{
			base.SetErrorCode(-2146233051);
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00035F4D File Offset: 0x00034F4D
		private ThreadStartException(Exception reason) : base(Environment.GetResourceString("Arg_ThreadStartException"), reason)
		{
			base.SetErrorCode(-2146233051);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00035F6B File Offset: 0x00034F6B
		internal ThreadStartException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
