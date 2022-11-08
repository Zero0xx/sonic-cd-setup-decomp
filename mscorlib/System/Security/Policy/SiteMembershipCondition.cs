using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004B6 RID: 1206
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06003000 RID: 12288 RVA: 0x000A49C4 File Offset: 0x000A39C4
		internal SiteMembershipCondition()
		{
			this.m_site = null;
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000A49D3 File Offset: 0x000A39D3
		public SiteMembershipCondition(string site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			this.m_site = new SiteString(site);
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x000A4A11 File Offset: 0x000A3A11
		// (set) Token: 0x06003002 RID: 12290 RVA: 0x000A49F5 File Offset: 0x000A39F5
		public string Site
		{
			get
			{
				if (this.m_site == null && this.m_element != null)
				{
					this.ParseSite();
				}
				if (this.m_site != null)
				{
					return this.m_site.ToString();
				}
				return "";
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_site = new SiteString(value);
			}
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000A4A44 File Offset: 0x000A3A44
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000A4A5C File Offset: 0x000A3A5C
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				Site site = obj as Site;
				if (site != null)
				{
					if (this.m_site == null && this.m_element != null)
					{
						this.ParseSite();
					}
					if (site.GetSiteString().IsSubsetOf(this.m_site))
					{
						usedEvidence = site;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x000A4AC0 File Offset: 0x000A3AC0
		public IMembershipCondition Copy()
		{
			if (this.m_site == null && this.m_element != null)
			{
				this.ParseSite();
			}
			return new SiteMembershipCondition(this.m_site.ToString());
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000A4AE8 File Offset: 0x000A3AE8
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000A4AF1 File Offset: 0x000A3AF1
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000A4AFC File Offset: 0x000A3AFC
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_site == null && this.m_element != null)
			{
				this.ParseSite();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.SiteMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_site != null)
			{
				securityElement.AddAttribute("Site", this.m_site.ToString());
			}
			return securityElement;
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x000A4B6C File Offset: 0x000A3B6C
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Tag.Equals("IMembershipCondition"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MembershipConditionElement"));
			}
			lock (this)
			{
				this.m_site = null;
				this.m_element = e;
			}
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x000A4BD8 File Offset: 0x000A3BD8
		private void ParseSite()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("Site");
					if (text == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_SiteCannotBeNull"));
					}
					this.m_site = new SiteString(text);
					this.m_element = null;
				}
			}
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x000A4C48 File Offset: 0x000A3C48
		public override bool Equals(object o)
		{
			SiteMembershipCondition siteMembershipCondition = o as SiteMembershipCondition;
			if (siteMembershipCondition != null)
			{
				if (this.m_site == null && this.m_element != null)
				{
					this.ParseSite();
				}
				if (siteMembershipCondition.m_site == null && siteMembershipCondition.m_element != null)
				{
					siteMembershipCondition.ParseSite();
				}
				if (object.Equals(this.m_site, siteMembershipCondition.m_site))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x000A4CA1 File Offset: 0x000A3CA1
		public override int GetHashCode()
		{
			if (this.m_site == null && this.m_element != null)
			{
				this.ParseSite();
			}
			if (this.m_site != null)
			{
				return this.m_site.GetHashCode();
			}
			return typeof(SiteMembershipCondition).GetHashCode();
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x000A4CDC File Offset: 0x000A3CDC
		public override string ToString()
		{
			if (this.m_site == null && this.m_element != null)
			{
				this.ParseSite();
			}
			if (this.m_site != null)
			{
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Site_ToStringArg"), new object[]
				{
					this.m_site
				});
			}
			return Environment.GetResourceString("Site_ToString");
		}

		// Token: 0x04001859 RID: 6233
		private SiteString m_site;

		// Token: 0x0400185A RID: 6234
		private SecurityElement m_element;
	}
}
