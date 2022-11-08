using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020002F5 RID: 757
	[ComVisible(true)]
	[Serializable]
	public class CustomAttributeFormatException : FormatException
	{
		// Token: 0x06001D54 RID: 7508 RVA: 0x0004A5A0 File Offset: 0x000495A0
		public CustomAttributeFormatException() : base(Environment.GetResourceString("Arg_CustomAttributeFormatException"))
		{
			base.SetErrorCode(-2146232827);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x0004A5BD File Offset: 0x000495BD
		public CustomAttributeFormatException(string message) : base(message)
		{
			base.SetErrorCode(-2146232827);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x0004A5D1 File Offset: 0x000495D1
		public CustomAttributeFormatException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146232827);
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x0004A5E6 File Offset: 0x000495E6
		protected CustomAttributeFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
