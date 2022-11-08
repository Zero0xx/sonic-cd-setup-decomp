using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000AF RID: 175
	[ComVisible(true)]
	[Serializable]
	public class EntryPointNotFoundException : TypeLoadException
	{
		// Token: 0x06000A62 RID: 2658 RVA: 0x0001FB81 File Offset: 0x0001EB81
		public EntryPointNotFoundException() : base(Environment.GetResourceString("Arg_EntryPointNotFoundException"))
		{
			base.SetErrorCode(-2146233053);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0001FB9E File Offset: 0x0001EB9E
		public EntryPointNotFoundException(string message) : base(message)
		{
			base.SetErrorCode(-2146233053);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0001FBB2 File Offset: 0x0001EBB2
		public EntryPointNotFoundException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233053);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0001FBC7 File Offset: 0x0001EBC7
		protected EntryPointNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
