using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000756 RID: 1878
	[Serializable]
	public class EventLogPermissionEntry
	{
		// Token: 0x06003980 RID: 14720 RVA: 0x000F41BC File Offset: 0x000F31BC
		public EventLogPermissionEntry(EventLogPermissionAccess permissionAccess, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"MachineName",
					machineName
				}));
			}
			this.permissionAccess = permissionAccess;
			this.machineName = machineName;
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x000F4209 File Offset: 0x000F3209
		internal EventLogPermissionEntry(ResourcePermissionBaseEntry baseEntry)
		{
			this.permissionAccess = (EventLogPermissionAccess)baseEntry.PermissionAccess;
			this.machineName = baseEntry.PermissionAccessPath[0];
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06003982 RID: 14722 RVA: 0x000F422B File Offset: 0x000F322B
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06003983 RID: 14723 RVA: 0x000F4233 File Offset: 0x000F3233
		public EventLogPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x000F423C File Offset: 0x000F323C
		internal ResourcePermissionBaseEntry GetBaseEntry()
		{
			return new ResourcePermissionBaseEntry((int)this.PermissionAccess, new string[]
			{
				this.MachineName
			});
		}

		// Token: 0x040032BB RID: 12987
		private string machineName;

		// Token: 0x040032BC RID: 12988
		private EventLogPermissionAccess permissionAccess;
	}
}
