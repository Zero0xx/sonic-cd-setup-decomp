using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000AA RID: 170
	[ComVisible(true)]
	[Serializable]
	public class DivideByZeroException : ArithmeticException
	{
		// Token: 0x06000A23 RID: 2595 RVA: 0x0001F408 File Offset: 0x0001E408
		public DivideByZeroException() : base(Environment.GetResourceString("Arg_DivideByZero"))
		{
			base.SetErrorCode(-2147352558);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001F425 File Offset: 0x0001E425
		public DivideByZeroException(string message) : base(message)
		{
			base.SetErrorCode(-2147352558);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0001F439 File Offset: 0x0001E439
		public DivideByZeroException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147352558);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0001F44E File Offset: 0x0001E44E
		protected DivideByZeroException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
