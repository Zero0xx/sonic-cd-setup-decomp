using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200037B RID: 891
	[ComVisible(true)]
	[Serializable]
	public class SerializationException : SystemException
	{
		// Token: 0x060022F1 RID: 8945 RVA: 0x0005845E File Offset: 0x0005745E
		public SerializationException() : base(SerializationException._nullMessage)
		{
			base.SetErrorCode(-2146233076);
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x00058476 File Offset: 0x00057476
		public SerializationException(string message) : base(message)
		{
			base.SetErrorCode(-2146233076);
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0005848A File Offset: 0x0005748A
		public SerializationException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233076);
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0005849F File Offset: 0x0005749F
		protected SerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04000EAE RID: 3758
		private static string _nullMessage = Environment.GetResourceString("Arg_SerializationException");
	}
}
