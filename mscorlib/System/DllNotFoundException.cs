using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000B0 RID: 176
	[ComVisible(true)]
	[Serializable]
	public class DllNotFoundException : TypeLoadException
	{
		// Token: 0x06000A66 RID: 2662 RVA: 0x0001FBD1 File Offset: 0x0001EBD1
		public DllNotFoundException() : base(Environment.GetResourceString("Arg_DllNotFoundException"))
		{
			base.SetErrorCode(-2146233052);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0001FBEE File Offset: 0x0001EBEE
		public DllNotFoundException(string message) : base(message)
		{
			base.SetErrorCode(-2146233052);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0001FC02 File Offset: 0x0001EC02
		public DllNotFoundException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233052);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0001FC17 File Offset: 0x0001EC17
		protected DllNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
