using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000CB RID: 203
	[ComVisible(true)]
	[Serializable]
	public class InvalidOperationException : SystemException
	{
		// Token: 0x06000B87 RID: 2951 RVA: 0x000230F5 File Offset: 0x000220F5
		public InvalidOperationException() : base(Environment.GetResourceString("Arg_InvalidOperationException"))
		{
			base.SetErrorCode(-2146233079);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00023112 File Offset: 0x00022112
		public InvalidOperationException(string message) : base(message)
		{
			base.SetErrorCode(-2146233079);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00023126 File Offset: 0x00022126
		public InvalidOperationException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233079);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002313B File Offset: 0x0002213B
		protected InvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
