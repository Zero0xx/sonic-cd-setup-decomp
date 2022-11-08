using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008FA RID: 2298
	public sealed class MutexAccessRule : AccessRule
	{
		// Token: 0x0600534B RID: 21323 RVA: 0x0012C217 File Offset: 0x0012B217
		public MutexAccessRule(IdentityReference identity, MutexRights eventRights, AccessControlType type) : this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x0012C225 File Offset: 0x0012B225
		public MutexAccessRule(string identity, MutexRights eventRights, AccessControlType type) : this(new NTAccount(identity), (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x0012C238 File Offset: 0x0012B238
		internal MutexAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x0600534E RID: 21326 RVA: 0x0012C249 File Offset: 0x0012B249
		public MutexRights MutexRights
		{
			get
			{
				return (MutexRights)base.AccessMask;
			}
		}
	}
}
