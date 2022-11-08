using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004BD RID: 1213
	[ComVisible(true)]
	[Serializable]
	public sealed class ZoneMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06003072 RID: 12402 RVA: 0x000A63EE File Offset: 0x000A53EE
		internal ZoneMembershipCondition()
		{
			this.m_zone = SecurityZone.NoZone;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000A63FD File Offset: 0x000A53FD
		public ZoneMembershipCondition(SecurityZone zone)
		{
			ZoneMembershipCondition.VerifyZone(zone);
			this.SecurityZone = zone;
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x000A6421 File Offset: 0x000A5421
		// (set) Token: 0x06003074 RID: 12404 RVA: 0x000A6412 File Offset: 0x000A5412
		public SecurityZone SecurityZone
		{
			get
			{
				if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
				{
					this.ParseZone();
				}
				return this.m_zone;
			}
			set
			{
				ZoneMembershipCondition.VerifyZone(value);
				this.m_zone = value;
			}
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x000A6440 File Offset: 0x000A5440
		private static void VerifyZone(SecurityZone zone)
		{
			if (zone < SecurityZone.MyComputer || zone > SecurityZone.Untrusted)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalZone"));
			}
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000A645C File Offset: 0x000A545C
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000A6474 File Offset: 0x000A5474
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
				Zone zone = obj as Zone;
				if (zone != null)
				{
					if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
					{
						this.ParseZone();
					}
					if (zone.SecurityZone == this.m_zone)
					{
						usedEvidence = zone;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x000A64D4 File Offset: 0x000A54D4
		public IMembershipCondition Copy()
		{
			if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
			{
				this.ParseZone();
			}
			return new ZoneMembershipCondition(this.m_zone);
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x000A64F8 File Offset: 0x000A54F8
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000A6501 File Offset: 0x000A5501
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x000A650C File Offset: 0x000A550C
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
			{
				this.ParseZone();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.ZoneMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_zone != SecurityZone.NoZone)
			{
				securityElement.AddAttribute("Zone", Enum.GetName(typeof(SecurityZone), this.m_zone));
			}
			return securityElement;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000A658C File Offset: 0x000A558C
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
				this.m_zone = SecurityZone.NoZone;
				this.m_element = e;
			}
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000A65F8 File Offset: 0x000A55F8
		private void ParseZone()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("Zone");
					this.m_zone = SecurityZone.NoZone;
					if (text == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ZoneCannotBeNull"));
					}
					this.m_zone = (SecurityZone)Enum.Parse(typeof(SecurityZone), text);
					ZoneMembershipCondition.VerifyZone(this.m_zone);
					this.m_element = null;
				}
			}
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000A668C File Offset: 0x000A568C
		public override bool Equals(object o)
		{
			ZoneMembershipCondition zoneMembershipCondition = o as ZoneMembershipCondition;
			if (zoneMembershipCondition != null)
			{
				if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
				{
					this.ParseZone();
				}
				if (zoneMembershipCondition.m_zone == SecurityZone.NoZone && zoneMembershipCondition.m_element != null)
				{
					zoneMembershipCondition.ParseZone();
				}
				if (this.m_zone == zoneMembershipCondition.m_zone)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x000A66E2 File Offset: 0x000A56E2
		public override int GetHashCode()
		{
			if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
			{
				this.ParseZone();
			}
			return (int)this.m_zone;
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x000A6704 File Offset: 0x000A5704
		public override string ToString()
		{
			if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
			{
				this.ParseZone();
			}
			return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Zone_ToString"), new object[]
			{
				ZoneMembershipCondition.s_names[(int)this.m_zone]
			});
		}

		// Token: 0x0400186D RID: 6253
		private static readonly string[] s_names = new string[]
		{
			"MyComputer",
			"Intranet",
			"Trusted",
			"Internet",
			"Untrusted"
		};

		// Token: 0x0400186E RID: 6254
		private SecurityZone m_zone;

		// Token: 0x0400186F RID: 6255
		private SecurityElement m_element;
	}
}
