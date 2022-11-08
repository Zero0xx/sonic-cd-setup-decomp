using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000AC RID: 172
	[ComVisible(true)]
	[Serializable]
	public class DuplicateWaitObjectException : ArgumentException
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0001F893 File Offset: 0x0001E893
		private static string DuplicateWaitObjectMessage
		{
			get
			{
				if (DuplicateWaitObjectException._duplicateWaitObjectMessage == null)
				{
					DuplicateWaitObjectException._duplicateWaitObjectMessage = Environment.GetResourceString("Arg_DuplicateWaitObjectException");
				}
				return DuplicateWaitObjectException._duplicateWaitObjectMessage;
			}
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0001F8B0 File Offset: 0x0001E8B0
		public DuplicateWaitObjectException() : base(DuplicateWaitObjectException.DuplicateWaitObjectMessage)
		{
			base.SetErrorCode(-2146233047);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0001F8C8 File Offset: 0x0001E8C8
		public DuplicateWaitObjectException(string parameterName) : base(DuplicateWaitObjectException.DuplicateWaitObjectMessage, parameterName)
		{
			base.SetErrorCode(-2146233047);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0001F8E1 File Offset: 0x0001E8E1
		public DuplicateWaitObjectException(string parameterName, string message) : base(message, parameterName)
		{
			base.SetErrorCode(-2146233047);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0001F8F6 File Offset: 0x0001E8F6
		public DuplicateWaitObjectException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233047);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0001F90B File Offset: 0x0001E90B
		protected DuplicateWaitObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040003C1 RID: 961
		private static string _duplicateWaitObjectMessage;
	}
}
