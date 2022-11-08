using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008F1 RID: 2289
	public sealed class EventWaitHandleAuditRule : AuditRule
	{
		// Token: 0x06005307 RID: 21255 RVA: 0x0012B9AE File Offset: 0x0012A9AE
		public EventWaitHandleAuditRule(IdentityReference identity, EventWaitHandleRights eventRights, AuditFlags flags) : this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x0012B9BC File Offset: 0x0012A9BC
		internal EventWaitHandleAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005309 RID: 21257 RVA: 0x0012B9CD File Offset: 0x0012A9CD
		public EventWaitHandleRights EventWaitHandleRights
		{
			get
			{
				return (EventWaitHandleRights)base.AccessMask;
			}
		}
	}
}
