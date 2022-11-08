using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200012A RID: 298
	[ComVisible(true)]
	[Serializable]
	public class TimeoutException : SystemException
	{
		// Token: 0x06001079 RID: 4217 RVA: 0x0002E2C0 File Offset: 0x0002D2C0
		public TimeoutException() : base(Environment.GetResourceString("Arg_TimeoutException"))
		{
			base.SetErrorCode(-2146233083);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0002E2DD File Offset: 0x0002D2DD
		public TimeoutException(string message) : base(message)
		{
			base.SetErrorCode(-2146233083);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0002E2F1 File Offset: 0x0002D2F1
		public TimeoutException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233083);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0002E306 File Offset: 0x0002D306
		protected TimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
