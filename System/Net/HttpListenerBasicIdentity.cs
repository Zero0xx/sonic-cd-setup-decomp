using System;
using System.Security.Principal;

namespace System.Net
{
	// Token: 0x020003C4 RID: 964
	public class HttpListenerBasicIdentity : GenericIdentity
	{
		// Token: 0x06001E56 RID: 7766 RVA: 0x00074411 File Offset: 0x00073411
		public HttpListenerBasicIdentity(string username, string password) : base(username, "Basic")
		{
			this.m_Password = password;
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x00074426 File Offset: 0x00073426
		public virtual string Password
		{
			get
			{
				return this.m_Password;
			}
		}

		// Token: 0x04001E42 RID: 7746
		private string m_Password;
	}
}
