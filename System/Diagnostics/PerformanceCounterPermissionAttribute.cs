using System;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x0200076E RID: 1902
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public class PerformanceCounterPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003A82 RID: 14978 RVA: 0x000F9107 File Offset: 0x000F8107
		public PerformanceCounterPermissionAttribute(SecurityAction action) : base(action)
		{
			this.categoryName = "*";
			this.machineName = ".";
			this.permissionAccess = PerformanceCounterPermissionAccess.Write;
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06003A83 RID: 14979 RVA: 0x000F912D File Offset: 0x000F812D
		// (set) Token: 0x06003A84 RID: 14980 RVA: 0x000F9135 File Offset: 0x000F8135
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.categoryName = value;
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06003A85 RID: 14981 RVA: 0x000F914C File Offset: 0x000F814C
		// (set) Token: 0x06003A86 RID: 14982 RVA: 0x000F9154 File Offset: 0x000F8154
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[]
					{
						"MachineName",
						value
					}));
				}
				this.machineName = value;
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x000F9194 File Offset: 0x000F8194
		// (set) Token: 0x06003A88 RID: 14984 RVA: 0x000F919C File Offset: 0x000F819C
		public PerformanceCounterPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
			set
			{
				this.permissionAccess = value;
			}
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x000F91A5 File Offset: 0x000F81A5
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new PerformanceCounterPermission(PermissionState.Unrestricted);
			}
			return new PerformanceCounterPermission(this.PermissionAccess, this.MachineName, this.CategoryName);
		}

		// Token: 0x04003345 RID: 13125
		private string categoryName;

		// Token: 0x04003346 RID: 13126
		private string machineName;

		// Token: 0x04003347 RID: 13127
		private PerformanceCounterPermissionAccess permissionAccess;
	}
}
