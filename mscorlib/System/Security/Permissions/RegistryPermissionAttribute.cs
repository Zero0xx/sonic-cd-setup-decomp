using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Permissions
{
	// Token: 0x02000641 RID: 1601
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class RegistryPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039AD RID: 14765 RVA: 0x000C2405 File Offset: 0x000C1405
		public RegistryPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x000C240E File Offset: 0x000C140E
		// (set) Token: 0x060039AF RID: 14767 RVA: 0x000C2416 File Offset: 0x000C1416
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

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060039B0 RID: 14768 RVA: 0x000C241F File Offset: 0x000C141F
		// (set) Token: 0x060039B1 RID: 14769 RVA: 0x000C2427 File Offset: 0x000C1427
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

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060039B2 RID: 14770 RVA: 0x000C2430 File Offset: 0x000C1430
		// (set) Token: 0x060039B3 RID: 14771 RVA: 0x000C2438 File Offset: 0x000C1438
		public string Create
		{
			get
			{
				return this.m_create;
			}
			set
			{
				this.m_create = value;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x000C2441 File Offset: 0x000C1441
		// (set) Token: 0x060039B5 RID: 14773 RVA: 0x000C2449 File Offset: 0x000C1449
		public string ViewAccessControl
		{
			get
			{
				return this.m_viewAcl;
			}
			set
			{
				this.m_viewAcl = value;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060039B6 RID: 14774 RVA: 0x000C2452 File Offset: 0x000C1452
		// (set) Token: 0x060039B7 RID: 14775 RVA: 0x000C245A File Offset: 0x000C145A
		public string ChangeAccessControl
		{
			get
			{
				return this.m_changeAcl;
			}
			set
			{
				this.m_changeAcl = value;
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x000C2463 File Offset: 0x000C1463
		// (set) Token: 0x060039B9 RID: 14777 RVA: 0x000C2474 File Offset: 0x000C1474
		public string ViewAndModify
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
			}
			set
			{
				this.m_read = value;
				this.m_write = value;
				this.m_create = value;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060039BA RID: 14778 RVA: 0x000C248B File Offset: 0x000C148B
		// (set) Token: 0x060039BB RID: 14779 RVA: 0x000C249C File Offset: 0x000C149C
		[Obsolete("Please use the ViewAndModify property instead.")]
		public string All
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
			}
			set
			{
				this.m_read = value;
				this.m_write = value;
				this.m_create = value;
			}
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x000C24B4 File Offset: 0x000C14B4
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new RegistryPermission(PermissionState.Unrestricted);
			}
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.None);
			if (this.m_read != null)
			{
				registryPermission.SetPathList(RegistryPermissionAccess.Read, this.m_read);
			}
			if (this.m_write != null)
			{
				registryPermission.SetPathList(RegistryPermissionAccess.Write, this.m_write);
			}
			if (this.m_create != null)
			{
				registryPermission.SetPathList(RegistryPermissionAccess.Create, this.m_create);
			}
			if (this.m_viewAcl != null)
			{
				registryPermission.SetPathList(AccessControlActions.View, this.m_viewAcl);
			}
			if (this.m_changeAcl != null)
			{
				registryPermission.SetPathList(AccessControlActions.Change, this.m_changeAcl);
			}
			return registryPermission;
		}

		// Token: 0x04001E0D RID: 7693
		private string m_read;

		// Token: 0x04001E0E RID: 7694
		private string m_write;

		// Token: 0x04001E0F RID: 7695
		private string m_create;

		// Token: 0x04001E10 RID: 7696
		private string m_viewAcl;

		// Token: 0x04001E11 RID: 7697
		private string m_changeAcl;
	}
}
