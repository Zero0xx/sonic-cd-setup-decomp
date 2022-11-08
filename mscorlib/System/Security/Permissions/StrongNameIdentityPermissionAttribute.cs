using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000645 RID: 1605
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class StrongNameIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039E7 RID: 14823 RVA: 0x000C28D8 File Offset: 0x000C18D8
		public StrongNameIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x000C28E1 File Offset: 0x000C18E1
		// (set) Token: 0x060039E9 RID: 14825 RVA: 0x000C28E9 File Offset: 0x000C18E9
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x000C28F2 File Offset: 0x000C18F2
		// (set) Token: 0x060039EB RID: 14827 RVA: 0x000C28FA File Offset: 0x000C18FA
		public string Version
		{
			get
			{
				return this.m_version;
			}
			set
			{
				this.m_version = value;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x000C2903 File Offset: 0x000C1903
		// (set) Token: 0x060039ED RID: 14829 RVA: 0x000C290B File Offset: 0x000C190B
		public string PublicKey
		{
			get
			{
				return this.m_blob;
			}
			set
			{
				this.m_blob = value;
			}
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x000C2914 File Offset: 0x000C1914
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new StrongNameIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.m_blob == null && this.m_name == null && this.m_version == null)
			{
				return new StrongNameIdentityPermission(PermissionState.None);
			}
			if (this.m_blob == null)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentNull_Key"));
			}
			StrongNamePublicKeyBlob blob = new StrongNamePublicKeyBlob(this.m_blob);
			if (this.m_version == null || this.m_version.Equals(string.Empty))
			{
				return new StrongNameIdentityPermission(blob, this.m_name, null);
			}
			return new StrongNameIdentityPermission(blob, this.m_name, new Version(this.m_version));
		}

		// Token: 0x04001E16 RID: 7702
		private string m_name;

		// Token: 0x04001E17 RID: 7703
		private string m_version;

		// Token: 0x04001E18 RID: 7704
		private string m_blob;
	}
}
