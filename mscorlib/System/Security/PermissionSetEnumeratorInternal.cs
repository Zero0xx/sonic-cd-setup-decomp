using System;
using System.Security.Util;

namespace System.Security
{
	// Token: 0x02000678 RID: 1656
	internal struct PermissionSetEnumeratorInternal
	{
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06003BE1 RID: 15329 RVA: 0x000CC41E File Offset: 0x000CB41E
		public object Current
		{
			get
			{
				return this.enm.Current;
			}
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x000CC42B File Offset: 0x000CB42B
		internal PermissionSetEnumeratorInternal(PermissionSet permSet)
		{
			this.m_permSet = permSet;
			this.enm = new TokenBasedSetEnumerator(permSet.m_permSet);
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x000CC445 File Offset: 0x000CB445
		public int GetCurrentIndex()
		{
			return this.enm.Index;
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x000CC452 File Offset: 0x000CB452
		public void Reset()
		{
			this.enm.Reset();
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x000CC460 File Offset: 0x000CB460
		public bool MoveNext()
		{
			while (this.enm.MoveNext())
			{
				object current = this.enm.Current;
				IPermission permission = current as IPermission;
				if (permission != null)
				{
					this.enm.Current = permission;
					return true;
				}
				SecurityElement securityElement = current as SecurityElement;
				if (securityElement != null)
				{
					permission = this.m_permSet.CreatePermission(securityElement, this.enm.Index);
					if (permission != null)
					{
						this.enm.Current = permission;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04001EDE RID: 7902
		private PermissionSet m_permSet;

		// Token: 0x04001EDF RID: 7903
		private TokenBasedSetEnumerator enm;
	}
}
