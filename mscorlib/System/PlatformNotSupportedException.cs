using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000ED RID: 237
	[ComVisible(true)]
	[Serializable]
	public class PlatformNotSupportedException : NotSupportedException
	{
		// Token: 0x06000C87 RID: 3207 RVA: 0x00025891 File Offset: 0x00024891
		public PlatformNotSupportedException() : base(Environment.GetResourceString("Arg_PlatformNotSupported"))
		{
			base.SetErrorCode(-2146233031);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000258AE File Offset: 0x000248AE
		public PlatformNotSupportedException(string message) : base(message)
		{
			base.SetErrorCode(-2146233031);
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x000258C2 File Offset: 0x000248C2
		public PlatformNotSupportedException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233031);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x000258D7 File Offset: 0x000248D7
		protected PlatformNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
