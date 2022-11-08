using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000311 RID: 785
	[ComVisible(true)]
	[Serializable]
	public class InvalidFilterCriteriaException : ApplicationException
	{
		// Token: 0x06001EA1 RID: 7841 RVA: 0x0004D582 File Offset: 0x0004C582
		public InvalidFilterCriteriaException() : base(Environment.GetResourceString("Arg_InvalidFilterCriteriaException"))
		{
			base.SetErrorCode(-2146232831);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0004D59F File Offset: 0x0004C59F
		public InvalidFilterCriteriaException(string message) : base(message)
		{
			base.SetErrorCode(-2146232831);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0004D5B3 File Offset: 0x0004C5B3
		public InvalidFilterCriteriaException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146232831);
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0004D5C8 File Offset: 0x0004C5C8
		protected InvalidFilterCriteriaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
