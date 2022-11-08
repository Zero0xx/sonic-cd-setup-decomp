using System;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x020003E9 RID: 1001
	internal class InternalException : SystemException
	{
		// Token: 0x0600206C RID: 8300 RVA: 0x0007FBA7 File Offset: 0x0007EBA7
		internal InternalException()
		{
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0007FBAF File Offset: 0x0007EBAF
		internal InternalException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
