using System;
using System.Security.Principal;

namespace System.Security.Permissions
{
	// Token: 0x0200064E RID: 1614
	[Serializable]
	internal class IDRole
	{
		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06003A25 RID: 14885 RVA: 0x000C3048 File Offset: 0x000C2048
		internal SecurityIdentifier Sid
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_role))
				{
					return null;
				}
				if (this.m_sid == null)
				{
					NTAccount identity = new NTAccount(this.m_role);
					IdentityReferenceCollection identityReferenceCollection = NTAccount.Translate(new IdentityReferenceCollection(1)
					{
						identity
					}, typeof(SecurityIdentifier), false);
					this.m_sid = (identityReferenceCollection[0] as SecurityIdentifier);
				}
				return this.m_sid;
			}
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x000C30B8 File Offset: 0x000C20B8
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("Identity");
			if (this.m_authenticated)
			{
				securityElement.AddAttribute("Authenticated", "true");
			}
			if (this.m_id != null)
			{
				securityElement.AddAttribute("ID", SecurityElement.Escape(this.m_id));
			}
			if (this.m_role != null)
			{
				securityElement.AddAttribute("Role", SecurityElement.Escape(this.m_role));
			}
			return securityElement;
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x000C3128 File Offset: 0x000C2128
		internal void FromXml(SecurityElement e)
		{
			string text = e.Attribute("Authenticated");
			if (text != null)
			{
				this.m_authenticated = (string.Compare(text, "true", StringComparison.OrdinalIgnoreCase) == 0);
			}
			else
			{
				this.m_authenticated = false;
			}
			string text2 = e.Attribute("ID");
			if (text2 != null)
			{
				this.m_id = text2;
			}
			else
			{
				this.m_id = null;
			}
			string text3 = e.Attribute("Role");
			if (text3 != null)
			{
				this.m_role = text3;
				return;
			}
			this.m_role = null;
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x000C319F File Offset: 0x000C219F
		public override int GetHashCode()
		{
			return (this.m_authenticated ? 0 : 101) + ((this.m_id == null) ? 0 : this.m_id.GetHashCode()) + ((this.m_role == null) ? 0 : this.m_role.GetHashCode());
		}

		// Token: 0x04001E2E RID: 7726
		internal bool m_authenticated;

		// Token: 0x04001E2F RID: 7727
		internal string m_id;

		// Token: 0x04001E30 RID: 7728
		internal string m_role;

		// Token: 0x04001E31 RID: 7729
		[NonSerialized]
		private SecurityIdentifier m_sid;
	}
}
