using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000DF RID: 223
	[ComVisible(true)]
	[Serializable]
	public class NotImplementedException : SystemException
	{
		// Token: 0x06000C27 RID: 3111 RVA: 0x00024242 File Offset: 0x00023242
		public NotImplementedException() : base(Environment.GetResourceString("Arg_NotImplementedException"))
		{
			base.SetErrorCode(-2147467263);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0002425F File Offset: 0x0002325F
		public NotImplementedException(string message) : base(message)
		{
			base.SetErrorCode(-2147467263);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00024273 File Offset: 0x00023273
		public NotImplementedException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2147467263);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00024288 File Offset: 0x00023288
		protected NotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
