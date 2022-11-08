using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x02000496 RID: 1174
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationDirectoryMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002E7F RID: 11903 RVA: 0x0009CD8C File Offset: 0x0009BD8C
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x0009CDA4 File Offset: 0x0009BDA4
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
				ApplicationDirectory applicationDirectory = obj as ApplicationDirectory;
				if (applicationDirectory != null)
				{
					IEnumerator hostEnumerator2 = evidence.GetHostEnumerator();
					while (hostEnumerator2.MoveNext())
					{
						object obj2 = hostEnumerator2.Current;
						Url url = obj2 as Url;
						if (url != null)
						{
							string text = applicationDirectory.Directory;
							if (text != null && text.Length > 1)
							{
								if (text[text.Length - 1] == '/')
								{
									text += "*";
								}
								else
								{
									text += "/*";
								}
								URLString operand = new URLString(text);
								if (url.GetURLString().IsSubsetOf(operand))
								{
									usedEvidence = applicationDirectory;
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x0009CE68 File Offset: 0x0009BE68
		public IMembershipCondition Copy()
		{
			return new ApplicationDirectoryMembershipCondition();
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x0009CE6F File Offset: 0x0009BE6F
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x0009CE78 File Offset: 0x0009BE78
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x0009CE84 File Offset: 0x0009BE84
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.ApplicationDirectoryMembershipCondition");
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x0009CEBE File Offset: 0x0009BEBE
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
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x0009CEF0 File Offset: 0x0009BEF0
		public override bool Equals(object o)
		{
			return o is ApplicationDirectoryMembershipCondition;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x0009CEFB File Offset: 0x0009BEFB
		public override int GetHashCode()
		{
			return typeof(ApplicationDirectoryMembershipCondition).GetHashCode();
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x0009CF0C File Offset: 0x0009BF0C
		public override string ToString()
		{
			return Environment.GetResourceString("ApplicationDirectory_ToString");
		}
	}
}
