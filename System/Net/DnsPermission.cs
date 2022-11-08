using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003A6 RID: 934
	[Serializable]
	public sealed class DnsPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06001D39 RID: 7481 RVA: 0x0006FCF1 File Offset: 0x0006ECF1
		public DnsPermission(PermissionState state)
		{
			this.m_noRestriction = (state == PermissionState.Unrestricted);
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x0006FD03 File Offset: 0x0006ED03
		internal DnsPermission(bool free)
		{
			this.m_noRestriction = free;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x0006FD12 File Offset: 0x0006ED12
		public bool IsUnrestricted()
		{
			return this.m_noRestriction;
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x0006FD1A File Offset: 0x0006ED1A
		public override IPermission Copy()
		{
			return new DnsPermission(this.m_noRestriction);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x0006FD28 File Offset: 0x0006ED28
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return new DnsPermission(this.m_noRestriction || dnsPermission.m_noRestriction);
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x0006FD74 File Offset: 0x0006ED74
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.m_noRestriction && dnsPermission.m_noRestriction)
			{
				return new DnsPermission(true);
			}
			return null;
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x0006FDC0 File Offset: 0x0006EDC0
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return !this.m_noRestriction;
			}
			DnsPermission dnsPermission = target as DnsPermission;
			if (dnsPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			return !this.m_noRestriction || dnsPermission.m_noRestriction;
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x0006FE0C File Offset: 0x0006EE0C
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("net_no_classname"), "securityElement");
			}
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(SR.GetString("net_no_classname"), "securityElement");
			}
			if (text.IndexOf(base.GetType().FullName) < 0)
			{
				throw new ArgumentException(SR.GetString("net_no_typename"), "securityElement");
			}
			string text2 = securityElement.Attribute("Unrestricted");
			this.m_noRestriction = (text2 != null && 0 == string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x0006FEC4 File Offset: 0x0006EEC4
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.m_noRestriction)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x04001D72 RID: 7538
		private bool m_noRestriction;
	}
}
