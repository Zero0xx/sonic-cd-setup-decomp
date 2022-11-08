using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000904 RID: 2308
	public sealed class RegistryAuditRule : AuditRule
	{
		// Token: 0x0600539D RID: 21405 RVA: 0x0012DBF0 File Offset: 0x0012CBF0
		public RegistryAuditRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(identity, (int)registryRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x0012DC00 File Offset: 0x0012CC00
		public RegistryAuditRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(new NTAccount(identity), (int)registryRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x0012DC15 File Offset: 0x0012CC15
		internal RegistryAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x060053A0 RID: 21408 RVA: 0x0012DC26 File Offset: 0x0012CC26
		public RegistryRights RegistryRights
		{
			get
			{
				return (RegistryRights)base.AccessMask;
			}
		}
	}
}
