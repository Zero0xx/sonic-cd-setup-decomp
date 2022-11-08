using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000DA RID: 218
	[ComVisible(true)]
	[Serializable]
	public sealed class MulticastNotSupportedException : SystemException
	{
		// Token: 0x06000C17 RID: 3095 RVA: 0x000240AE File Offset: 0x000230AE
		public MulticastNotSupportedException() : base(Environment.GetResourceString("Arg_MulticastNotSupportedException"))
		{
			base.SetErrorCode(-2146233068);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x000240CB File Offset: 0x000230CB
		public MulticastNotSupportedException(string message) : base(message)
		{
			base.SetErrorCode(-2146233068);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000240DF File Offset: 0x000230DF
		public MulticastNotSupportedException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233068);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000240F4 File Offset: 0x000230F4
		internal MulticastNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
