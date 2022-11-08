using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000533 RID: 1331
	[ComVisible(true)]
	[Serializable]
	public class SafeArrayTypeMismatchException : SystemException
	{
		// Token: 0x0600331A RID: 13082 RVA: 0x000AD358 File Offset: 0x000AC358
		public SafeArrayTypeMismatchException() : base(Environment.GetResourceString("Arg_SafeArrayTypeMismatchException"))
		{
			base.SetErrorCode(-2146233037);
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000AD375 File Offset: 0x000AC375
		public SafeArrayTypeMismatchException(string message) : base(message)
		{
			base.SetErrorCode(-2146233037);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000AD389 File Offset: 0x000AC389
		public SafeArrayTypeMismatchException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233037);
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000AD39E File Offset: 0x000AC39E
		protected SafeArrayTypeMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
