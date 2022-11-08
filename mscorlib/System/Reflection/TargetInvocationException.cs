using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000341 RID: 833
	[ComVisible(true)]
	[Serializable]
	public sealed class TargetInvocationException : ApplicationException
	{
		// Token: 0x06001FD0 RID: 8144 RVA: 0x0004FF05 File Offset: 0x0004EF05
		private TargetInvocationException() : base(Environment.GetResourceString("Arg_TargetInvocationException"))
		{
			base.SetErrorCode(-2146232828);
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0004FF22 File Offset: 0x0004EF22
		private TargetInvocationException(string message) : base(message)
		{
			base.SetErrorCode(-2146232828);
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0004FF36 File Offset: 0x0004EF36
		public TargetInvocationException(Exception inner) : base(Environment.GetResourceString("Arg_TargetInvocationException"), inner)
		{
			base.SetErrorCode(-2146232828);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0004FF54 File Offset: 0x0004EF54
		public TargetInvocationException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146232828);
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0004FF69 File Offset: 0x0004EF69
		internal TargetInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
