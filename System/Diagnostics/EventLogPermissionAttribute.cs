using System;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000755 RID: 1877
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public class EventLogPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600397A RID: 14714 RVA: 0x000F4123 File Offset: 0x000F3123
		public EventLogPermissionAttribute(SecurityAction action) : base(action)
		{
			this.machineName = ".";
			this.permissionAccess = EventLogPermissionAccess.Write;
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x000F413F File Offset: 0x000F313F
		// (set) Token: 0x0600397C RID: 14716 RVA: 0x000F4148 File Offset: 0x000F3148
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

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x0600397D RID: 14717 RVA: 0x000F4188 File Offset: 0x000F3188
		// (set) Token: 0x0600397E RID: 14718 RVA: 0x000F4190 File Offset: 0x000F3190
		public EventLogPermissionAccess PermissionAccess
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

		// Token: 0x0600397F RID: 14719 RVA: 0x000F4199 File Offset: 0x000F3199
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new EventLogPermission(PermissionState.Unrestricted);
			}
			return new EventLogPermission(this.PermissionAccess, this.MachineName);
		}

		// Token: 0x040032B9 RID: 12985
		private string machineName;

		// Token: 0x040032BA RID: 12986
		private EventLogPermissionAccess permissionAccess;
	}
}
