using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000098 RID: 152
	[ComVisible(true)]
	[Obsolete("ContextMarshalException is obsolete.")]
	[Serializable]
	public class ContextMarshalException : SystemException
	{
		// Token: 0x06000807 RID: 2055 RVA: 0x0001A376 File Offset: 0x00019376
		public ContextMarshalException() : base(Environment.GetResourceString("Arg_ContextMarshalException"))
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001A393 File Offset: 0x00019393
		public ContextMarshalException(string message) : base(message)
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001A3A7 File Offset: 0x000193A7
		public ContextMarshalException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001A3BC File Offset: 0x000193BC
		protected ContextMarshalException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
