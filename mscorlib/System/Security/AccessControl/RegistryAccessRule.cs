using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000903 RID: 2307
	public sealed class RegistryAccessRule : AccessRule
	{
		// Token: 0x06005397 RID: 21399 RVA: 0x0012DB91 File Offset: 0x0012CB91
		public RegistryAccessRule(IdentityReference identity, RegistryRights registryRights, AccessControlType type) : this(identity, (int)registryRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x0012DB9F File Offset: 0x0012CB9F
		public RegistryAccessRule(string identity, RegistryRights registryRights, AccessControlType type) : this(new NTAccount(identity), (int)registryRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06005399 RID: 21401 RVA: 0x0012DBB2 File Offset: 0x0012CBB2
		public RegistryAccessRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(identity, (int)registryRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x0012DBC2 File Offset: 0x0012CBC2
		public RegistryAccessRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(new NTAccount(identity), (int)registryRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x0012DBD7 File Offset: 0x0012CBD7
		internal RegistryAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x0600539C RID: 21404 RVA: 0x0012DBE8 File Offset: 0x0012CBE8
		public RegistryRights RegistryRights
		{
			get
			{
				return (RegistryRights)base.AccessMask;
			}
		}
	}
}
