using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008F0 RID: 2288
	public sealed class EventWaitHandleAccessRule : AccessRule
	{
		// Token: 0x06005303 RID: 21251 RVA: 0x0012B974 File Offset: 0x0012A974
		public EventWaitHandleAccessRule(IdentityReference identity, EventWaitHandleRights eventRights, AccessControlType type) : this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x0012B982 File Offset: 0x0012A982
		public EventWaitHandleAccessRule(string identity, EventWaitHandleRights eventRights, AccessControlType type) : this(new NTAccount(identity), (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x0012B995 File Offset: 0x0012A995
		internal EventWaitHandleAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06005306 RID: 21254 RVA: 0x0012B9A6 File Offset: 0x0012A9A6
		public EventWaitHandleRights EventWaitHandleRights
		{
			get
			{
				return (EventWaitHandleRights)base.AccessMask;
			}
		}
	}
}
