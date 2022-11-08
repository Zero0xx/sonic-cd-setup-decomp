using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008FB RID: 2299
	public sealed class MutexAuditRule : AuditRule
	{
		// Token: 0x0600534F RID: 21327 RVA: 0x0012C251 File Offset: 0x0012B251
		public MutexAuditRule(IdentityReference identity, MutexRights eventRights, AuditFlags flags) : this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x0012C25F File Offset: 0x0012B25F
		internal MutexAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005351 RID: 21329 RVA: 0x0012C270 File Offset: 0x0012B270
		public MutexRights MutexRights
		{
			get
			{
				return (MutexRights)base.AccessMask;
			}
		}
	}
}
