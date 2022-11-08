using System;
using System.Runtime.Serialization;

namespace System.Security.Authentication
{
	// Token: 0x02000588 RID: 1416
	[Serializable]
	public class InvalidCredentialException : AuthenticationException
	{
		// Token: 0x06002B89 RID: 11145 RVA: 0x000BC8F1 File Offset: 0x000BB8F1
		public InvalidCredentialException()
		{
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x000BC8F9 File Offset: 0x000BB8F9
		protected InvalidCredentialException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000BC903 File Offset: 0x000BB903
		public InvalidCredentialException(string message) : base(message)
		{
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000BC90C File Offset: 0x000BB90C
		public InvalidCredentialException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
