using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000753 RID: 1875
	[Serializable]
	public sealed class EventLogPermission : ResourcePermissionBase
	{
		// Token: 0x06003971 RID: 14705 RVA: 0x000F4033 File Offset: 0x000F3033
		public EventLogPermission()
		{
			this.SetNames();
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x000F4041 File Offset: 0x000F3041
		public EventLogPermission(PermissionState state) : base(state)
		{
			this.SetNames();
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x000F4050 File Offset: 0x000F3050
		public EventLogPermission(EventLogPermissionAccess permissionAccess, string machineName)
		{
			this.SetNames();
			this.AddPermissionAccess(new EventLogPermissionEntry(permissionAccess, machineName));
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x000F406C File Offset: 0x000F306C
		public EventLogPermission(EventLogPermissionEntry[] permissionAccessEntries)
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

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003975 RID: 14709 RVA: 0x000F40AA File Offset: 0x000F30AA
		public EventLogPermissionEntryCollection PermissionEntries
		{
			get
			{
				if (this.innerCollection == null)
				{
					this.innerCollection = new EventLogPermissionEntryCollection(this, base.GetPermissionEntries());
				}
				return this.innerCollection;
			}
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x000F40CC File Offset: 0x000F30CC
		internal void AddPermissionAccess(EventLogPermissionEntry entry)
		{
			base.AddPermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x000F40DA File Offset: 0x000F30DA
		internal new void Clear()
		{
			base.Clear();
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x000F40E2 File Offset: 0x000F30E2
		internal void RemovePermissionAccess(EventLogPermissionEntry entry)
		{
			base.RemovePermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x000F40F0 File Offset: 0x000F30F0
		private void SetNames()
		{
			base.PermissionAccessType = typeof(EventLogPermissionAccess);
			base.TagNames = new string[]
			{
				"Machine"
			};
		}

		// Token: 0x040032B1 RID: 12977
		private EventLogPermissionEntryCollection innerCollection;
	}
}
