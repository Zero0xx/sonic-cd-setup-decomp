using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200063B RID: 1595
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class EnvironmentPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003967 RID: 14695 RVA: 0x000C1EC0 File Offset: 0x000C0EC0
		public EnvironmentPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003968 RID: 14696 RVA: 0x000C1EC9 File Offset: 0x000C0EC9
		// (set) Token: 0x06003969 RID: 14697 RVA: 0x000C1ED1 File Offset: 0x000C0ED1
		public string Read
		{
			get
			{
				return this.m_read;
			}
			set
			{
				this.m_read = value;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x000C1EDA File Offset: 0x000C0EDA
		// (set) Token: 0x0600396B RID: 14699 RVA: 0x000C1EE2 File Offset: 0x000C0EE2
		public string Write
		{
			get
			{
				return this.m_write;
			}
			set
			{
				this.m_write = value;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600396C RID: 14700 RVA: 0x000C1EEB File Offset: 0x000C0EEB
		// (set) Token: 0x0600396D RID: 14701 RVA: 0x000C1EFC File Offset: 0x000C0EFC
		public string All
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
			}
			set
			{
				this.m_write = value;
				this.m_read = value;
			}
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x000C1F0C File Offset: 0x000C0F0C
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new EnvironmentPermission(PermissionState.Unrestricted);
			}
			EnvironmentPermission environmentPermission = new EnvironmentPermission(PermissionState.None);
			if (this.m_read != null)
			{
				environmentPermission.SetPathList(EnvironmentPermissionAccess.Read, this.m_read);
			}
			if (this.m_write != null)
			{
				environmentPermission.SetPathList(EnvironmentPermissionAccess.Write, this.m_write);
			}
			return environmentPermission;
		}

		// Token: 0x04001DF8 RID: 7672
		private string m_read;

		// Token: 0x04001DF9 RID: 7673
		private string m_write;
	}
}
