using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004BF RID: 1215
	[ComVisible(true)]
	[Serializable]
	public sealed class GacMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x0600308E RID: 12430 RVA: 0x000A6810 File Offset: 0x000A5810
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000A6828 File Offset: 0x000A5828
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
				if (obj is GacInstalled)
				{
					usedEvidence = obj;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000A6863 File Offset: 0x000A5863
		public IMembershipCondition Copy()
		{
			return new GacMembershipCondition();
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000A686A File Offset: 0x000A586A
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000A6873 File Offset: 0x000A5873
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000A6880 File Offset: 0x000A5880
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x000A68C0 File Offset: 0x000A58C0
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

		// Token: 0x06003095 RID: 12437 RVA: 0x000A68F4 File Offset: 0x000A58F4
		public override bool Equals(object o)
		{
			return o is GacMembershipCondition;
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000A690E File Offset: 0x000A590E
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000A6911 File Offset: 0x000A5911
		public override string ToString()
		{
			return Environment.GetResourceString("GAC_ToString");
		}
	}
}
