using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000E9 RID: 233
	[ComVisible(true)]
	[Serializable]
	public class OverflowException : ArithmeticException
	{
		// Token: 0x06000C7A RID: 3194 RVA: 0x000257E2 File Offset: 0x000247E2
		public OverflowException() : base(Environment.GetResourceString("Arg_OverflowException"))
		{
			base.SetErrorCode(-2146233066);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x000257FF File Offset: 0x000247FF
		public OverflowException(string message) : base(message)
		{
			base.SetErrorCode(-2146233066);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00025813 File Offset: 0x00024813
		public OverflowException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233066);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00025828 File Offset: 0x00024828
		protected OverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
