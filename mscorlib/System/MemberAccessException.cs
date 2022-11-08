using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000044 RID: 68
	[ComVisible(true)]
	[Serializable]
	public class MemberAccessException : SystemException
	{
		// Token: 0x060003EA RID: 1002 RVA: 0x00010116 File Offset: 0x0000F116
		public MemberAccessException() : base(Environment.GetResourceString("Arg_AccessException"))
		{
			base.SetErrorCode(-2146233062);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00010133 File Offset: 0x0000F133
		public MemberAccessException(string message) : base(message)
		{
			base.SetErrorCode(-2146233062);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00010147 File Offset: 0x0000F147
		public MemberAccessException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233062);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001015C File Offset: 0x0000F15C
		protected MemberAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
