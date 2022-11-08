using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x0200076F RID: 1903
	[Serializable]
	public class PerformanceCounterPermissionEntry
	{
		// Token: 0x06003A8A RID: 14986 RVA: 0x000F91D0 File Offset: 0x000F81D0
		public PerformanceCounterPermissionEntry(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if ((permissionAccess & (PerformanceCounterPermissionAccess)(-8)) != PerformanceCounterPermissionAccess.None)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"permissionAccess",
					permissionAccess
				}));
			}
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
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
			this.categoryName = categoryName;
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x000F926F File Offset: 0x000F826F
		internal PerformanceCounterPermissionEntry(ResourcePermissionBaseEntry baseEntry)
		{
			this.permissionAccess = (PerformanceCounterPermissionAccess)baseEntry.PermissionAccess;
			this.machineName = baseEntry.PermissionAccessPath[0];
			this.categoryName = baseEntry.PermissionAccessPath[1];
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06003A8C RID: 14988 RVA: 0x000F929F File Offset: 0x000F829F
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06003A8D RID: 14989 RVA: 0x000F92A7 File Offset: 0x000F82A7
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06003A8E RID: 14990 RVA: 0x000F92AF File Offset: 0x000F82AF
		public PerformanceCounterPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x000F92B8 File Offset: 0x000F82B8
		internal ResourcePermissionBaseEntry GetBaseEntry()
		{
			return new ResourcePermissionBaseEntry((int)this.PermissionAccess, new string[]
			{
				this.MachineName,
				this.CategoryName
			});
		}

		// Token: 0x04003348 RID: 13128
		private string categoryName;

		// Token: 0x04003349 RID: 13129
		private string machineName;

		// Token: 0x0400334A RID: 13130
		private PerformanceCounterPermissionAccess permissionAccess;
	}
}
