using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x0200072B RID: 1835
	[Serializable]
	public class InternalBufferOverflowException : SystemException
	{
		// Token: 0x06003813 RID: 14355 RVA: 0x000ECEC8 File Offset: 0x000EBEC8
		public InternalBufferOverflowException()
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x000ECEDB File Offset: 0x000EBEDB
		public InternalBufferOverflowException(string message) : base(message)
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x000ECEEF File Offset: 0x000EBEEF
		public InternalBufferOverflowException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x000ECF04 File Offset: 0x000EBF04
		protected InternalBufferOverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
