using System;
using System.Runtime.Serialization;

namespace System.Security.Authentication
{
	// Token: 0x02000587 RID: 1415
	[Serializable]
	public class AuthenticationException : SystemException
	{
		// Token: 0x06002B85 RID: 11141 RVA: 0x000BC8CC File Offset: 0x000BB8CC
		public AuthenticationException()
		{
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x000BC8D4 File Offset: 0x000BB8D4
		protected AuthenticationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000BC8DE File Offset: 0x000BB8DE
		public AuthenticationException(string message) : base(message)
		{
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000BC8E7 File Offset: 0x000BB8E7
		public AuthenticationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
