using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200016E RID: 366
	[ComVisible(true)]
	[Serializable]
	public class ThreadInterruptedException : SystemException
	{
		// Token: 0x060013A5 RID: 5029 RVA: 0x00035505 File Offset: 0x00034505
		public ThreadInterruptedException() : base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.ThreadInterrupted))
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0003551E File Offset: 0x0003451E
		public ThreadInterruptedException(string message) : base(message)
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00035532 File Offset: 0x00034532
		public ThreadInterruptedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00035547 File Offset: 0x00034547
		protected ThreadInterruptedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
