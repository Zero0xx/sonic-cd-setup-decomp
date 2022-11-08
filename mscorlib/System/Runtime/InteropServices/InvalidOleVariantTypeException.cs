using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000512 RID: 1298
	[ComVisible(true)]
	[Serializable]
	public class InvalidOleVariantTypeException : SystemException
	{
		// Token: 0x060031E3 RID: 12771 RVA: 0x000AA28B File Offset: 0x000A928B
		public InvalidOleVariantTypeException() : base(Environment.GetResourceString("Arg_InvalidOleVariantTypeException"))
		{
			base.SetErrorCode(-2146233039);
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000AA2A8 File Offset: 0x000A92A8
		public InvalidOleVariantTypeException(string message) : base(message)
		{
			base.SetErrorCode(-2146233039);
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000AA2BC File Offset: 0x000A92BC
		public InvalidOleVariantTypeException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233039);
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000AA2D1 File Offset: 0x000A92D1
		protected InvalidOleVariantTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
