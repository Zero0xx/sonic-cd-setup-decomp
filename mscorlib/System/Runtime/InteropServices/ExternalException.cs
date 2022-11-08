using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050B RID: 1291
	[ComVisible(true)]
	[Serializable]
	public class ExternalException : SystemException
	{
		// Token: 0x060031A8 RID: 12712 RVA: 0x000A9970 File Offset: 0x000A8970
		public ExternalException() : base(Environment.GetResourceString("Arg_ExternalException"))
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000A998D File Offset: 0x000A898D
		public ExternalException(string message) : base(message)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000A99A1 File Offset: 0x000A89A1
		public ExternalException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000A99B6 File Offset: 0x000A89B6
		public ExternalException(string message, int errorCode) : base(message)
		{
			base.SetErrorCode(errorCode);
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000A99C6 File Offset: 0x000A89C6
		protected ExternalException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x060031AD RID: 12717 RVA: 0x000A99D0 File Offset: 0x000A89D0
		public virtual int ErrorCode
		{
			get
			{
				return base.HResult;
			}
		}
	}
}
