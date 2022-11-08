using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000038 RID: 56
	[ComVisible(true)]
	[Serializable]
	public class OutOfMemoryException : SystemException
	{
		// Token: 0x06000377 RID: 887 RVA: 0x0000E2F2 File Offset: 0x0000D2F2
		public OutOfMemoryException() : base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.OutOfMemory))
		{
			base.SetErrorCode(-2147024882);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000E30B File Offset: 0x0000D30B
		public OutOfMemoryException(string message) : base(message)
		{
			base.SetErrorCode(-2147024882);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000E31F File Offset: 0x0000D31F
		public OutOfMemoryException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024882);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000E334 File Offset: 0x0000D334
		protected OutOfMemoryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
