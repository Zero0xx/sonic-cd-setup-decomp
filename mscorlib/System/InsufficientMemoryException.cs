using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000C4 RID: 196
	[Serializable]
	public sealed class InsufficientMemoryException : OutOfMemoryException
	{
		// Token: 0x06000B05 RID: 2821 RVA: 0x0002259F File Offset: 0x0002159F
		public InsufficientMemoryException() : base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.OutOfMemory))
		{
			base.SetErrorCode(-2146233027);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x000225B8 File Offset: 0x000215B8
		public InsufficientMemoryException(string message) : base(message)
		{
			base.SetErrorCode(-2146233027);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x000225CC File Offset: 0x000215CC
		public InsufficientMemoryException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233027);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x000225E1 File Offset: 0x000215E1
		private InsufficientMemoryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
