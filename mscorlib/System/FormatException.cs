using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000BB RID: 187
	[ComVisible(true)]
	[Serializable]
	public class FormatException : SystemException
	{
		// Token: 0x06000AB4 RID: 2740 RVA: 0x00020DD4 File Offset: 0x0001FDD4
		public FormatException() : base(Environment.GetResourceString("Arg_FormatException"))
		{
			base.SetErrorCode(-2146233033);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00020DF1 File Offset: 0x0001FDF1
		public FormatException(string message) : base(message)
		{
			base.SetErrorCode(-2146233033);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00020E05 File Offset: 0x0001FE05
		public FormatException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233033);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00020E1A File Offset: 0x0001FE1A
		protected FormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
