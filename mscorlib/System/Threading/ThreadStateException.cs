using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200017C RID: 380
	[ComVisible(true)]
	[Serializable]
	public class ThreadStateException : SystemException
	{
		// Token: 0x060013FE RID: 5118 RVA: 0x00035ED8 File Offset: 0x00034ED8
		public ThreadStateException() : base(Environment.GetResourceString("Arg_ThreadStateException"))
		{
			base.SetErrorCode(-2146233056);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00035EF5 File Offset: 0x00034EF5
		public ThreadStateException(string message) : base(message)
		{
			base.SetErrorCode(-2146233056);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x00035F09 File Offset: 0x00034F09
		public ThreadStateException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233056);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00035F1E File Offset: 0x00034F1E
		protected ThreadStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
