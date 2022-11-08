using System;

namespace System.Net
{
	// Token: 0x0200037C RID: 892
	public class Authorization
	{
		// Token: 0x06001BDE RID: 7134 RVA: 0x0006939D File Offset: 0x0006839D
		public Authorization(string token)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_Complete = true;
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x000693B8 File Offset: 0x000683B8
		public Authorization(string token, bool finished)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_Complete = finished;
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000693D3 File Offset: 0x000683D3
		public Authorization(string token, bool finished, string connectionGroupId) : this(token, finished, connectionGroupId, false)
		{
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000693DF File Offset: 0x000683DF
		internal Authorization(string token, bool finished, string connectionGroupId, bool mutualAuth)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_ConnectionGroupId = ValidationHelper.MakeStringNull(connectionGroupId);
			this.m_Complete = finished;
			this.m_MutualAuth = mutualAuth;
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x0006940E File Offset: 0x0006840E
		public string Message
		{
			get
			{
				return this.m_Message;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x00069416 File Offset: 0x00068416
		public string ConnectionGroupId
		{
			get
			{
				return this.m_ConnectionGroupId;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x0006941E File Offset: 0x0006841E
		public bool Complete
		{
			get
			{
				return this.m_Complete;
			}
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x00069426 File Offset: 0x00068426
		internal void SetComplete(bool complete)
		{
			this.m_Complete = complete;
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x0006942F File Offset: 0x0006842F
		// (set) Token: 0x06001BE7 RID: 7143 RVA: 0x00069438 File Offset: 0x00068438
		public string[] ProtectionRealm
		{
			get
			{
				return this.m_ProtectionRealm;
			}
			set
			{
				string[] protectionRealm = ValidationHelper.MakeEmptyArrayNull(value);
				this.m_ProtectionRealm = protectionRealm;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001BE8 RID: 7144 RVA: 0x00069453 File Offset: 0x00068453
		// (set) Token: 0x06001BE9 RID: 7145 RVA: 0x00069465 File Offset: 0x00068465
		public bool MutuallyAuthenticated
		{
			get
			{
				return this.Complete && this.m_MutualAuth;
			}
			set
			{
				this.m_MutualAuth = value;
			}
		}

		// Token: 0x04001C8A RID: 7306
		private string m_Message;

		// Token: 0x04001C8B RID: 7307
		private bool m_Complete;

		// Token: 0x04001C8C RID: 7308
		private string[] m_ProtectionRealm;

		// Token: 0x04001C8D RID: 7309
		private string m_ConnectionGroupId;

		// Token: 0x04001C8E RID: 7310
		private bool m_MutualAuth;
	}
}
