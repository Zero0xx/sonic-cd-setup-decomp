using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200004A RID: 74
	[ComVisible(true)]
	[Serializable]
	public class ApplicationException : Exception
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x000109A3 File Offset: 0x0000F9A3
		public ApplicationException() : base(Environment.GetResourceString("Arg_ApplicationException"))
		{
			base.SetErrorCode(-2146232832);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000109C0 File Offset: 0x0000F9C0
		public ApplicationException(string message) : base(message)
		{
			base.SetErrorCode(-2146232832);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000109D4 File Offset: 0x0000F9D4
		public ApplicationException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146232832);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000109E9 File Offset: 0x0000F9E9
		protected ApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
