using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000517 RID: 1303
	[ComVisible(true)]
	[Serializable]
	public class MarshalDirectiveException : SystemException
	{
		// Token: 0x060032AC RID: 12972 RVA: 0x000AB47C File Offset: 0x000AA47C
		public MarshalDirectiveException() : base(Environment.GetResourceString("Arg_MarshalDirectiveException"))
		{
			base.SetErrorCode(-2146233035);
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x000AB499 File Offset: 0x000AA499
		public MarshalDirectiveException(string message) : base(message)
		{
			base.SetErrorCode(-2146233035);
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000AB4AD File Offset: 0x000AA4AD
		public MarshalDirectiveException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233035);
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000AB4C2 File Offset: 0x000AA4C2
		protected MarshalDirectiveException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
