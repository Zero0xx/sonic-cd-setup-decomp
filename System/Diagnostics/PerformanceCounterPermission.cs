using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x0200076C RID: 1900
	[Serializable]
	public sealed class PerformanceCounterPermission : ResourcePermissionBase
	{
		// Token: 0x06003A79 RID: 14969 RVA: 0x000F900F File Offset: 0x000F800F
		public PerformanceCounterPermission()
		{
			this.SetNames();
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x000F901D File Offset: 0x000F801D
		public PerformanceCounterPermission(PermissionState state) : base(state)
		{
			this.SetNames();
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x000F902C File Offset: 0x000F802C
		public PerformanceCounterPermission(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName)
		{
			this.SetNames();
			this.AddPermissionAccess(new PerformanceCounterPermissionEntry(permissionAccess, machineName, categoryName));
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x000F9048 File Offset: 0x000F8048
		public PerformanceCounterPermission(PerformanceCounterPermissionEntry[] permissionAccessEntries)
		{
			if (permissionAccessEntries == null)
			{
				throw new ArgumentNullException("permissionAccessEntries");
			}
			this.SetNames();
			for (int i = 0; i < permissionAccessEntries.Length; i++)
			{
				this.AddPermissionAccess(permissionAccessEntries[i]);
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06003A7D RID: 14973 RVA: 0x000F9086 File Offset: 0x000F8086
		public PerformanceCounterPermissionEntryCollection PermissionEntries
		{
			get
			{
				if (this.innerCollection == null)
				{
					this.innerCollection = new PerformanceCounterPermissionEntryCollection(this, base.GetPermissionEntries());
				}
				return this.innerCollection;
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x000F90A8 File Offset: 0x000F80A8
		internal void AddPermissionAccess(PerformanceCounterPermissionEntry entry)
		{
			base.AddPermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x000F90B6 File Offset: 0x000F80B6
		internal new void Clear()
		{
			base.Clear();
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x000F90BE File Offset: 0x000F80BE
		internal void RemovePermissionAccess(PerformanceCounterPermissionEntry entry)
		{
			base.RemovePermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x000F90CC File Offset: 0x000F80CC
		private void SetNames()
		{
			base.PermissionAccessType = typeof(PerformanceCounterPermissionAccess);
			base.TagNames = new string[]
			{
				"Machine",
				"Category"
			};
		}

		// Token: 0x0400333D RID: 13117
		private PerformanceCounterPermissionEntryCollection innerCollection;
	}
}
