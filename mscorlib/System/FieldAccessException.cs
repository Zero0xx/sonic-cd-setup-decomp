using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000B9 RID: 185
	[ComVisible(true)]
	[Serializable]
	public class FieldAccessException : MemberAccessException
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x00020D7C File Offset: 0x0001FD7C
		public FieldAccessException() : base(Environment.GetResourceString("Arg_FieldAccessException"))
		{
			base.SetErrorCode(-2146233081);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00020D99 File Offset: 0x0001FD99
		public FieldAccessException(string message) : base(message)
		{
			base.SetErrorCode(-2146233081);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00020DAD File Offset: 0x0001FDAD
		public FieldAccessException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233081);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00020DC2 File Offset: 0x0001FDC2
		protected FieldAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
