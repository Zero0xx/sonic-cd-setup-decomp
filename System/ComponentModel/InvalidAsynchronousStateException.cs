using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000FE RID: 254
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class InvalidAsynchronousStateException : ArgumentException
	{
		// Token: 0x0600081F RID: 2079 RVA: 0x0001C188 File Offset: 0x0001B188
		public InvalidAsynchronousStateException() : this(null)
		{
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001C191 File Offset: 0x0001B191
		public InvalidAsynchronousStateException(string message) : base(message)
		{
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001C19A File Offset: 0x0001B19A
		public InvalidAsynchronousStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001C1A4 File Offset: 0x0001B1A4
		protected InvalidAsynchronousStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
