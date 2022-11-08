using System;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x02000493 RID: 1171
	[ComVisible(true)]
	[Serializable]
	public sealed class AllMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002E66 RID: 11878 RVA: 0x0009CAF4 File Offset: 0x0009BAF4
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x0009CB0C File Offset: 0x0009BB0C
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			return true;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x0009CB12 File Offset: 0x0009BB12
		public IMembershipCondition Copy()
		{
			return new AllMembershipCondition();
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x0009CB19 File Offset: 0x0009BB19
		public override string ToString()
		{
			return Environment.GetResourceString("All_ToString");
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x0009CB25 File Offset: 0x0009BB25
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x0009CB2E File Offset: 0x0009BB2E
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x0009CB38 File Offset: 0x0009BB38
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.AllMembershipCondition");
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x0009CB72 File Offset: 0x0009BB72
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

		// Token: 0x06002E6E RID: 11886 RVA: 0x0009CBA4 File Offset: 0x0009BBA4
		public override bool Equals(object o)
		{
			return o is AllMembershipCondition;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x0009CBAF File Offset: 0x0009BBAF
		public override int GetHashCode()
		{
			return typeof(AllMembershipCondition).GetHashCode();
		}
	}
}
