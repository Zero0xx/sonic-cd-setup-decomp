using System;

namespace System.Security.Permissions
{
	// Token: 0x0200073C RID: 1852
	[Serializable]
	public class ResourcePermissionBaseEntry
	{
		// Token: 0x0600387A RID: 14458 RVA: 0x000EEA1B File Offset: 0x000EDA1B
		public ResourcePermissionBaseEntry()
		{
			this.permissionAccess = 0;
			this.accessPath = new string[0];
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x000EEA36 File Offset: 0x000EDA36
		public ResourcePermissionBaseEntry(int permissionAccess, string[] permissionAccessPath)
		{
			if (permissionAccessPath == null)
			{
				throw new ArgumentNullException("permissionAccessPath");
			}
			this.permissionAccess = permissionAccess;
			this.accessPath = permissionAccessPath;
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x0600387C RID: 14460 RVA: 0x000EEA5A File Offset: 0x000EDA5A
		public int PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x0600387D RID: 14461 RVA: 0x000EEA62 File Offset: 0x000EDA62
		public string[] PermissionAccessPath
		{
			get
			{
				return this.accessPath;
			}
		}

		// Token: 0x04003243 RID: 12867
		private string[] accessPath;

		// Token: 0x04003244 RID: 12868
		private int permissionAccess;
	}
}
