using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000E8 RID: 232
	[ComVisible(true)]
	[Serializable]
	public class OperationCanceledException : SystemException
	{
		// Token: 0x06000C76 RID: 3190 RVA: 0x00025792 File Offset: 0x00024792
		public OperationCanceledException() : base(Environment.GetResourceString("OperationCanceled"))
		{
			base.SetErrorCode(-2146233029);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000257AF File Offset: 0x000247AF
		public OperationCanceledException(string message) : base(message)
		{
			base.SetErrorCode(-2146233029);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x000257C3 File Offset: 0x000247C3
		public OperationCanceledException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233029);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x000257D8 File Offset: 0x000247D8
		protected OperationCanceledException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
