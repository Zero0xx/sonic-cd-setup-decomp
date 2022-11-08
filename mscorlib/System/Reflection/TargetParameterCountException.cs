using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000342 RID: 834
	[ComVisible(true)]
	[Serializable]
	public sealed class TargetParameterCountException : ApplicationException
	{
		// Token: 0x06001FD5 RID: 8149 RVA: 0x0004FF73 File Offset: 0x0004EF73
		public TargetParameterCountException() : base(Environment.GetResourceString("Arg_TargetParameterCountException"))
		{
			base.SetErrorCode(-2147352562);
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x0004FF90 File Offset: 0x0004EF90
		public TargetParameterCountException(string message) : base(message)
		{
			base.SetErrorCode(-2147352562);
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0004FFA4 File Offset: 0x0004EFA4
		public TargetParameterCountException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2147352562);
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0004FFB9 File Offset: 0x0004EFB9
		internal TargetParameterCountException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
