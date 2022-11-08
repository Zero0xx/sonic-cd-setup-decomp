using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x02000766 RID: 1894
	[ComVisible(true)]
	[Serializable]
	public class RemotingTimeoutException : RemotingException
	{
		// Token: 0x06004348 RID: 17224 RVA: 0x000E5C2A File Offset: 0x000E4C2A
		public RemotingTimeoutException() : base(RemotingTimeoutException._nullMessage)
		{
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x000E5C37 File Offset: 0x000E4C37
		public RemotingTimeoutException(string message) : base(message)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x0600434A RID: 17226 RVA: 0x000E5C4B File Offset: 0x000E4C4B
		public RemotingTimeoutException(string message, Exception InnerException) : base(message, InnerException)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x0600434B RID: 17227 RVA: 0x000E5C60 File Offset: 0x000E4C60
		internal RemotingTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040021D6 RID: 8662
		private static string _nullMessage = Environment.GetResourceString("Remoting_Default");
	}
}
