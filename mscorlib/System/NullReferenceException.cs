using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000E1 RID: 225
	[ComVisible(true)]
	[Serializable]
	public class NullReferenceException : SystemException
	{
		// Token: 0x06000C2F RID: 3119 RVA: 0x000242E2 File Offset: 0x000232E2
		public NullReferenceException() : base(Environment.GetResourceString("Arg_NullReferenceException"))
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x000242FF File Offset: 0x000232FF
		public NullReferenceException(string message) : base(message)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00024313 File Offset: 0x00023313
		public NullReferenceException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00024328 File Offset: 0x00023328
		protected NullReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
