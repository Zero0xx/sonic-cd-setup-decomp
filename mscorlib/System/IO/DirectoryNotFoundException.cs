using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x020005AF RID: 1455
	[ComVisible(true)]
	[Serializable]
	public class DirectoryNotFoundException : IOException
	{
		// Token: 0x06003596 RID: 13718 RVA: 0x000B2A91 File Offset: 0x000B1A91
		public DirectoryNotFoundException() : base(Environment.GetResourceString("Arg_DirectoryNotFoundException"))
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000B2AAE File Offset: 0x000B1AAE
		public DirectoryNotFoundException(string message) : base(message)
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000B2AC2 File Offset: 0x000B1AC2
		public DirectoryNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024893);
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000B2AD7 File Offset: 0x000B1AD7
		protected DirectoryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
