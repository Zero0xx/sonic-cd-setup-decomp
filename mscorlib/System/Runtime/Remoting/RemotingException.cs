using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x02000764 RID: 1892
	[ComVisible(true)]
	[Serializable]
	public class RemotingException : SystemException
	{
		// Token: 0x0600433E RID: 17214 RVA: 0x000E5B72 File Offset: 0x000E4B72
		public RemotingException() : base(RemotingException._nullMessage)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x000E5B8A File Offset: 0x000E4B8A
		public RemotingException(string message) : base(message)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x000E5B9E File Offset: 0x000E4B9E
		public RemotingException(string message, Exception InnerException) : base(message, InnerException)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x000E5BB3 File Offset: 0x000E4BB3
		protected RemotingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040021D4 RID: 8660
		private static string _nullMessage = Environment.GetResourceString("Remoting_Default");
	}
}
