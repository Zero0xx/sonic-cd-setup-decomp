using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000524 RID: 1316
	[ComVisible(true)]
	[Serializable]
	public class InvalidComObjectException : SystemException
	{
		// Token: 0x060032E3 RID: 13027 RVA: 0x000ABBFF File Offset: 0x000AABFF
		public InvalidComObjectException() : base(Environment.GetResourceString("Arg_InvalidComObjectException"))
		{
			base.SetErrorCode(-2146233049);
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x000ABC1C File Offset: 0x000AAC1C
		public InvalidComObjectException(string message) : base(message)
		{
			base.SetErrorCode(-2146233049);
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x000ABC30 File Offset: 0x000AAC30
		public InvalidComObjectException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233049);
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x000ABC45 File Offset: 0x000AAC45
		protected InvalidComObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
