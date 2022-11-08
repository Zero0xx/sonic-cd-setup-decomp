using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000340 RID: 832
	[ComVisible(true)]
	[Serializable]
	public class TargetException : ApplicationException
	{
		// Token: 0x06001FCC RID: 8140 RVA: 0x0004FEBF File Offset: 0x0004EEBF
		public TargetException()
		{
			base.SetErrorCode(-2146232829);
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0004FED2 File Offset: 0x0004EED2
		public TargetException(string message) : base(message)
		{
			base.SetErrorCode(-2146232829);
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0004FEE6 File Offset: 0x0004EEE6
		public TargetException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146232829);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0004FEFB File Offset: 0x0004EEFB
		protected TargetException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
