using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x02000765 RID: 1893
	[ComVisible(true)]
	[Serializable]
	public class ServerException : SystemException
	{
		// Token: 0x06004343 RID: 17219 RVA: 0x000E5BCE File Offset: 0x000E4BCE
		public ServerException() : base(ServerException._nullMessage)
		{
			base.SetErrorCode(-2146233074);
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x000E5BE6 File Offset: 0x000E4BE6
		public ServerException(string message) : base(message)
		{
			base.SetErrorCode(-2146233074);
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x000E5BFA File Offset: 0x000E4BFA
		public ServerException(string message, Exception InnerException) : base(message, InnerException)
		{
			base.SetErrorCode(-2146233074);
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x000E5C0F File Offset: 0x000E4C0F
		internal ServerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040021D5 RID: 8661
		private static string _nullMessage = Environment.GetResourceString("Remoting_Default");
	}
}
