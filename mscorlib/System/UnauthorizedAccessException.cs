using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000135 RID: 309
	[ComVisible(true)]
	[Serializable]
	public class UnauthorizedAccessException : SystemException
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x0002F810 File Offset: 0x0002E810
		public UnauthorizedAccessException() : base(Environment.GetResourceString("Arg_UnauthorizedAccessException"))
		{
			base.SetErrorCode(-2147024891);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0002F82D File Offset: 0x0002E82D
		public UnauthorizedAccessException(string message) : base(message)
		{
			base.SetErrorCode(-2147024891);
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0002F841 File Offset: 0x0002E841
		public UnauthorizedAccessException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2147024891);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0002F856 File Offset: 0x0002E856
		protected UnauthorizedAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
