using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200007B RID: 123
	[ComVisible(true)]
	[Serializable]
	public class CannotUnloadAppDomainException : SystemException
	{
		// Token: 0x060006F0 RID: 1776 RVA: 0x00016C76 File Offset: 0x00015C76
		public CannotUnloadAppDomainException() : base(Environment.GetResourceString("Arg_CannotUnloadAppDomainException"))
		{
			base.SetErrorCode(-2146234347);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00016C93 File Offset: 0x00015C93
		public CannotUnloadAppDomainException(string message) : base(message)
		{
			base.SetErrorCode(-2146234347);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00016CA7 File Offset: 0x00015CA7
		public CannotUnloadAppDomainException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146234347);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00016CBC File Offset: 0x00015CBC
		protected CannotUnloadAppDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
