using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200003B RID: 59
	[ComVisible(true)]
	[Serializable]
	public sealed class ExecutionEngineException : SystemException
	{
		// Token: 0x06000383 RID: 899 RVA: 0x0000E3DE File Offset: 0x0000D3DE
		public ExecutionEngineException() : base(Environment.GetResourceString("Arg_ExecutionEngineException"))
		{
			base.SetErrorCode(-2146233082);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000E3FB File Offset: 0x0000D3FB
		public ExecutionEngineException(string message) : base(message)
		{
			base.SetErrorCode(-2146233082);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000E40F File Offset: 0x0000D40F
		public ExecutionEngineException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233082);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000E424 File Offset: 0x0000D424
		internal ExecutionEngineException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
