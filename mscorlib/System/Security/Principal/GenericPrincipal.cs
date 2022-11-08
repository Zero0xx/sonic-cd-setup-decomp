using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004C7 RID: 1223
	[ComVisible(true)]
	[Serializable]
	public class GenericPrincipal : IPrincipal
	{
		// Token: 0x060030EB RID: 12523 RVA: 0x000A7BA0 File Offset: 0x000A6BA0
		public GenericPrincipal(IIdentity identity, string[] roles)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.m_identity = identity;
			if (roles != null)
			{
				this.m_roles = new string[roles.Length];
				for (int i = 0; i < roles.Length; i++)
				{
					this.m_roles[i] = roles[i];
				}
				return;
			}
			this.m_roles = null;
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x000A7BFA File Offset: 0x000A6BFA
		public virtual IIdentity Identity
		{
			get
			{
				return this.m_identity;
			}
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x000A7C04 File Offset: 0x000A6C04
		public virtual bool IsInRole(string role)
		{
			if (role == null || this.m_roles == null)
			{
				return false;
			}
			for (int i = 0; i < this.m_roles.Length; i++)
			{
				if (this.m_roles[i] != null && string.Compare(this.m_roles[i], role, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400187F RID: 6271
		private IIdentity m_identity;

		// Token: 0x04001880 RID: 6272
		private string[] m_roles;
	}
}
