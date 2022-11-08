using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x020005C1 RID: 1473
	[ComVisible(true)]
	[Serializable]
	public class PathTooLongException : IOException
	{
		// Token: 0x060036A9 RID: 13993 RVA: 0x000B8DA4 File Offset: 0x000B7DA4
		public PathTooLongException() : base(Environment.GetResourceString("IO.PathTooLong"))
		{
			base.SetErrorCode(-2147024690);
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000B8DC1 File Offset: 0x000B7DC1
		public PathTooLongException(string message) : base(message)
		{
			base.SetErrorCode(-2147024690);
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000B8DD5 File Offset: 0x000B7DD5
		public PathTooLongException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024690);
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000B8DEA File Offset: 0x000B7DEA
		protected PathTooLongException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
