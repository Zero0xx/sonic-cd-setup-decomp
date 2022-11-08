using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000532 RID: 1330
	[ComVisible(true)]
	[Serializable]
	public class SafeArrayRankMismatchException : SystemException
	{
		// Token: 0x06003316 RID: 13078 RVA: 0x000AD308 File Offset: 0x000AC308
		public SafeArrayRankMismatchException() : base(Environment.GetResourceString("Arg_SafeArrayRankMismatchException"))
		{
			base.SetErrorCode(-2146233032);
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000AD325 File Offset: 0x000AC325
		public SafeArrayRankMismatchException(string message) : base(message)
		{
			base.SetErrorCode(-2146233032);
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000AD339 File Offset: 0x000AC339
		public SafeArrayRankMismatchException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233032);
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x000AD34E File Offset: 0x000AC34E
		protected SafeArrayRankMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
