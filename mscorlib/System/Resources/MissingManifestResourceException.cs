using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Resources
{
	// Token: 0x02000432 RID: 1074
	[ComVisible(true)]
	[Serializable]
	public class MissingManifestResourceException : SystemException
	{
		// Token: 0x06002BD0 RID: 11216 RVA: 0x000929F4 File Offset: 0x000919F4
		public MissingManifestResourceException() : base(Environment.GetResourceString("Arg_MissingManifestResourceException"))
		{
			base.SetErrorCode(-2146233038);
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x00092A11 File Offset: 0x00091A11
		public MissingManifestResourceException(string message) : base(message)
		{
			base.SetErrorCode(-2146233038);
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x00092A25 File Offset: 0x00091A25
		public MissingManifestResourceException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233038);
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x00092A3A File Offset: 0x00091A3A
		protected MissingManifestResourceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
