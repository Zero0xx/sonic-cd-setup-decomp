using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;

namespace System.Security.Permissions
{
	// Token: 0x0200063D RID: 1597
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class FileIOPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003975 RID: 14709 RVA: 0x000C1FDB File Offset: 0x000C0FDB
		public FileIOPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06003976 RID: 14710 RVA: 0x000C1FE4 File Offset: 0x000C0FE4
		// (set) Token: 0x06003977 RID: 14711 RVA: 0x000C1FEC File Offset: 0x000C0FEC
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

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003978 RID: 14712 RVA: 0x000C1FF5 File Offset: 0x000C0FF5
		// (set) Token: 0x06003979 RID: 14713 RVA: 0x000C1FFD File Offset: 0x000C0FFD
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

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x0600397A RID: 14714 RVA: 0x000C2006 File Offset: 0x000C1006
		// (set) Token: 0x0600397B RID: 14715 RVA: 0x000C200E File Offset: 0x000C100E
		public string Append
		{
			get
			{
				return this.m_append;
			}
			set
			{
				this.m_append = value;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x0600397C RID: 14716 RVA: 0x000C2017 File Offset: 0x000C1017
		// (set) Token: 0x0600397D RID: 14717 RVA: 0x000C201F File Offset: 0x000C101F
		public string PathDiscovery
		{
			get
			{
				return this.m_pathDiscovery;
			}
			set
			{
				this.m_pathDiscovery = value;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x0600397E RID: 14718 RVA: 0x000C2028 File Offset: 0x000C1028
		// (set) Token: 0x0600397F RID: 14719 RVA: 0x000C2030 File Offset: 0x000C1030
		public string ViewAccessControl
		{
			get
			{
				return this.m_viewAccess;
			}
			set
			{
				this.m_viewAccess = value;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06003980 RID: 14720 RVA: 0x000C2039 File Offset: 0x000C1039
		// (set) Token: 0x06003981 RID: 14721 RVA: 0x000C2041 File Offset: 0x000C1041
		public string ChangeAccessControl
		{
			get
			{
				return this.m_changeAccess;
			}
			set
			{
				this.m_changeAccess = value;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003983 RID: 14723 RVA: 0x000C2068 File Offset: 0x000C1068
		// (set) Token: 0x06003982 RID: 14722 RVA: 0x000C204A File Offset: 0x000C104A
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
				this.m_append = value;
				this.m_pathDiscovery = value;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06003984 RID: 14724 RVA: 0x000C2079 File Offset: 0x000C1079
		// (set) Token: 0x06003985 RID: 14725 RVA: 0x000C208A File Offset: 0x000C108A
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
				this.m_append = value;
				this.m_pathDiscovery = value;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06003986 RID: 14726 RVA: 0x000C20A8 File Offset: 0x000C10A8
		// (set) Token: 0x06003987 RID: 14727 RVA: 0x000C20B0 File Offset: 0x000C10B0
		public FileIOPermissionAccess AllFiles
		{
			get
			{
				return this.m_allFiles;
			}
			set
			{
				this.m_allFiles = value;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06003988 RID: 14728 RVA: 0x000C20B9 File Offset: 0x000C10B9
		// (set) Token: 0x06003989 RID: 14729 RVA: 0x000C20C1 File Offset: 0x000C10C1
		public FileIOPermissionAccess AllLocalFiles
		{
			get
			{
				return this.m_allLocalFiles;
			}
			set
			{
				this.m_allLocalFiles = value;
			}
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x000C20CC File Offset: 0x000C10CC
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new FileIOPermission(PermissionState.Unrestricted);
			}
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.None);
			if (this.m_read != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.Read, this.m_read);
			}
			if (this.m_write != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.Write, this.m_write);
			}
			if (this.m_append != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.Append, this.m_append);
			}
			if (this.m_pathDiscovery != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.PathDiscovery, this.m_pathDiscovery);
			}
			if (this.m_viewAccess != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.NoAccess, AccessControlActions.View, new string[]
				{
					this.m_viewAccess
				}, false);
			}
			if (this.m_changeAccess != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.NoAccess, AccessControlActions.Change, new string[]
				{
					this.m_changeAccess
				}, false);
			}
			fileIOPermission.AllFiles = this.m_allFiles;
			fileIOPermission.AllLocalFiles = this.m_allLocalFiles;
			return fileIOPermission;
		}

		// Token: 0x04001DFB RID: 7675
		private string m_read;

		// Token: 0x04001DFC RID: 7676
		private string m_write;

		// Token: 0x04001DFD RID: 7677
		private string m_append;

		// Token: 0x04001DFE RID: 7678
		private string m_pathDiscovery;

		// Token: 0x04001DFF RID: 7679
		private string m_viewAccess;

		// Token: 0x04001E00 RID: 7680
		private string m_changeAccess;

		// Token: 0x04001E01 RID: 7681
		[OptionalField(VersionAdded = 2)]
		private FileIOPermissionAccess m_allLocalFiles;

		// Token: 0x04001E02 RID: 7682
		[OptionalField(VersionAdded = 2)]
		private FileIOPermissionAccess m_allFiles;
	}
}
