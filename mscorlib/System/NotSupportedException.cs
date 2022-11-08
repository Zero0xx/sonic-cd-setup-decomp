using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000E0 RID: 224
	[ComVisible(true)]
	[Serializable]
	public class NotSupportedException : SystemException
	{
		// Token: 0x06000C2B RID: 3115 RVA: 0x00024292 File Offset: 0x00023292
		public NotSupportedException() : base(Environment.GetResourceString("Arg_NotSupportedException"))
		{
			base.SetErrorCode(-2146233067);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000242AF File Offset: 0x000232AF
		public NotSupportedException(string message) : base(message)
		{
			base.SetErrorCode(-2146233067);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000242C3 File Offset: 0x000232C3
		public NotSupportedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233067);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x000242D8 File Offset: 0x000232D8
		protected NotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
