using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200065B RID: 1627
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class GacIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003AB2 RID: 15026 RVA: 0x000C6407 File Offset: 0x000C5407
		public GacIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x000C6410 File Offset: 0x000C5410
		public override IPermission CreatePermission()
		{
			return new GacIdentityPermission();
		}
	}
}
