using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000086 RID: 134
	[ComVisible(true)]
	[Serializable]
	public class TypeUnloadedException : SystemException
	{
		// Token: 0x0600076A RID: 1898 RVA: 0x00018268 File Offset: 0x00017268
		public TypeUnloadedException() : base(Environment.GetResourceString("Arg_TypeUnloadedException"))
		{
			base.SetErrorCode(-2146234349);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00018285 File Offset: 0x00017285
		public TypeUnloadedException(string message) : base(message)
		{
			base.SetErrorCode(-2146234349);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00018299 File Offset: 0x00017299
		public TypeUnloadedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146234349);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000182AE File Offset: 0x000172AE
		protected TypeUnloadedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
