using System;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008E7 RID: 2279
	public sealed class CryptoKeySecurity : NativeObjectSecurity
	{
		// Token: 0x060052F0 RID: 21232 RVA: 0x0012B838 File Offset: 0x0012A838
		public CryptoKeySecurity() : base(false, ResourceType.FileObject)
		{
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x0012B842 File Offset: 0x0012A842
		public CryptoKeySecurity(CommonSecurityDescriptor securityDescriptor) : base(ResourceType.FileObject, securityDescriptor)
		{
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x0012B84C File Offset: 0x0012A84C
		public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new CryptoKeyAccessRule(identityReference, CryptoKeyAccessRule.RightsFromAccessMask(accessMask), type);
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x0012B85C File Offset: 0x0012A85C
		public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new CryptoKeyAuditRule(identityReference, CryptoKeyAuditRule.RightsFromAccessMask(accessMask), flags);
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x0012B86C File Offset: 0x0012A86C
		public void AddAccessRule(CryptoKeyAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x0012B875 File Offset: 0x0012A875
		public void SetAccessRule(CryptoKeyAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x0012B87E File Offset: 0x0012A87E
		public void ResetAccessRule(CryptoKeyAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x0012B887 File Offset: 0x0012A887
		public bool RemoveAccessRule(CryptoKeyAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x0012B890 File Offset: 0x0012A890
		public void RemoveAccessRuleAll(CryptoKeyAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x0012B899 File Offset: 0x0012A899
		public void RemoveAccessRuleSpecific(CryptoKeyAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x0012B8A2 File Offset: 0x0012A8A2
		public void AddAuditRule(CryptoKeyAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x0012B8AB File Offset: 0x0012A8AB
		public void SetAuditRule(CryptoKeyAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x0012B8B4 File Offset: 0x0012A8B4
		public bool RemoveAuditRule(CryptoKeyAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x0012B8BD File Offset: 0x0012A8BD
		public void RemoveAuditRuleAll(CryptoKeyAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x0012B8C6 File Offset: 0x0012A8C6
		public void RemoveAuditRuleSpecific(CryptoKeyAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x060052FF RID: 21247 RVA: 0x0012B8CF File Offset: 0x0012A8CF
		public override Type AccessRightType
		{
			get
			{
				return typeof(CryptoKeyRights);
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06005300 RID: 21248 RVA: 0x0012B8DB File Offset: 0x0012A8DB
		public override Type AccessRuleType
		{
			get
			{
				return typeof(CryptoKeyAccessRule);
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06005301 RID: 21249 RVA: 0x0012B8E7 File Offset: 0x0012A8E7
		public override Type AuditRuleType
		{
			get
			{
				return typeof(CryptoKeyAuditRule);
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06005302 RID: 21250 RVA: 0x0012B8F4 File Offset: 0x0012A8F4
		internal AccessControlSections ChangedAccessControlSections
		{
			get
			{
				AccessControlSections accessControlSections = AccessControlSections.None;
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						base.ReadLock();
						flag = true;
					}
					if (base.AccessRulesModified)
					{
						accessControlSections |= AccessControlSections.Access;
					}
					if (base.AuditRulesModified)
					{
						accessControlSections |= AccessControlSections.Audit;
					}
					if (base.GroupModified)
					{
						accessControlSections |= AccessControlSections.Group;
					}
					if (base.OwnerModified)
					{
						accessControlSections |= AccessControlSections.Owner;
					}
				}
				finally
				{
					if (flag)
					{
						base.ReadUnlock();
					}
				}
				return accessControlSections;
			}
		}

		// Token: 0x04002AC1 RID: 10945
		private const ResourceType s_ResourceType = ResourceType.FileObject;
	}
}
