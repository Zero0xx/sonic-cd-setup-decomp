using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000D5 RID: 213
	[ComVisible(true)]
	[Serializable]
	public class MethodAccessException : MemberAccessException
	{
		// Token: 0x06000BFC RID: 3068 RVA: 0x00023C9D File Offset: 0x00022C9D
		public MethodAccessException() : base(Environment.GetResourceString("Arg_MethodAccessException"))
		{
			base.SetErrorCode(-2146233072);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00023CBA File Offset: 0x00022CBA
		public MethodAccessException(string message) : base(message)
		{
			base.SetErrorCode(-2146233072);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00023CCE File Offset: 0x00022CCE
		public MethodAccessException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233072);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00023CE3 File Offset: 0x00022CE3
		protected MethodAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
