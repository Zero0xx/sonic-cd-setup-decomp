using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003A5 RID: 933
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class DnsPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06001D37 RID: 7479 RVA: 0x0006FCD1 File Offset: 0x0006ECD1
		public DnsPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x0006FCDA File Offset: 0x0006ECDA
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new DnsPermission(PermissionState.Unrestricted);
			}
			return new DnsPermission(PermissionState.None);
		}
	}
}
