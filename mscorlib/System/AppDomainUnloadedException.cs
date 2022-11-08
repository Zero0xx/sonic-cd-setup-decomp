using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000063 RID: 99
	[ComVisible(true)]
	[Serializable]
	public class AppDomainUnloadedException : SystemException
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x00014DA1 File Offset: 0x00013DA1
		public AppDomainUnloadedException() : base(Environment.GetResourceString("Arg_AppDomainUnloadedException"))
		{
			base.SetErrorCode(-2146234348);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00014DBE File Offset: 0x00013DBE
		public AppDomainUnloadedException(string message) : base(message)
		{
			base.SetErrorCode(-2146234348);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00014DD2 File Offset: 0x00013DD2
		public AppDomainUnloadedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146234348);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00014DE7 File Offset: 0x00013DE7
		protected AppDomainUnloadedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
