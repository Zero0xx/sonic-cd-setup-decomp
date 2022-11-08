using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000CA RID: 202
	[ComVisible(true)]
	[Serializable]
	public class InvalidCastException : SystemException
	{
		// Token: 0x06000B82 RID: 2946 RVA: 0x00023095 File Offset: 0x00022095
		public InvalidCastException() : base(Environment.GetResourceString("Arg_InvalidCastException"))
		{
			base.SetErrorCode(-2147467262);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000230B2 File Offset: 0x000220B2
		public InvalidCastException(string message) : base(message)
		{
			base.SetErrorCode(-2147467262);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000230C6 File Offset: 0x000220C6
		public InvalidCastException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147467262);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000230DB File Offset: 0x000220DB
		protected InvalidCastException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x000230E5 File Offset: 0x000220E5
		public InvalidCastException(string message, int errorCode) : base(message)
		{
			base.SetErrorCode(errorCode);
		}
	}
}
