using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x020005B2 RID: 1458
	[ComVisible(true)]
	[Serializable]
	public class DriveNotFoundException : IOException
	{
		// Token: 0x060035A9 RID: 13737 RVA: 0x000B2F55 File Offset: 0x000B1F55
		public DriveNotFoundException() : base(Environment.GetResourceString("Arg_DriveNotFoundException"))
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000B2F72 File Offset: 0x000B1F72
		public DriveNotFoundException(string message) : base(message)
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x000B2F86 File Offset: 0x000B1F86
		public DriveNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x000B2F9B File Offset: 0x000B1F9B
		protected DriveNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
