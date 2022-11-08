using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x020005B3 RID: 1459
	[ComVisible(true)]
	[Serializable]
	public class EndOfStreamException : IOException
	{
		// Token: 0x060035AD RID: 13741 RVA: 0x000B2FA5 File Offset: 0x000B1FA5
		public EndOfStreamException() : base(Environment.GetResourceString("Arg_EndOfStreamException"))
		{
			base.SetErrorCode(-2147024858);
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x000B2FC2 File Offset: 0x000B1FC2
		public EndOfStreamException(string message) : base(message)
		{
			base.SetErrorCode(-2147024858);
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x000B2FD6 File Offset: 0x000B1FD6
		public EndOfStreamException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024858);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x000B2FEB File Offset: 0x000B1FEB
		protected EndOfStreamException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
