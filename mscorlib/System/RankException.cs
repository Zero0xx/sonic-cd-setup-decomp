using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000EF RID: 239
	[ComVisible(true)]
	[Serializable]
	public class RankException : SystemException
	{
		// Token: 0x06000C95 RID: 3221 RVA: 0x00025BB5 File Offset: 0x00024BB5
		public RankException() : base(Environment.GetResourceString("Arg_RankException"))
		{
			base.SetErrorCode(-2146233065);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00025BD2 File Offset: 0x00024BD2
		public RankException(string message) : base(message)
		{
			base.SetErrorCode(-2146233065);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00025BE6 File Offset: 0x00024BE6
		public RankException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233065);
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00025BFB File Offset: 0x00024BFB
		protected RankException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
