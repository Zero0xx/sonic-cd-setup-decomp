using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200016D RID: 365
	[ComVisible(true)]
	[Serializable]
	public sealed class ThreadAbortException : SystemException
	{
		// Token: 0x060013A2 RID: 5026 RVA: 0x000354D6 File Offset: 0x000344D6
		private ThreadAbortException() : base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.ThreadAbort))
		{
			base.SetErrorCode(-2146233040);
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x000354EF File Offset: 0x000344EF
		internal ThreadAbortException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060013A4 RID: 5028 RVA: 0x000354F9 File Offset: 0x000344F9
		public object ExceptionState
		{
			get
			{
				return Thread.CurrentThread.AbortReason;
			}
		}
	}
}
