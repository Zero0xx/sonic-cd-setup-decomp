using System;

namespace System.Configuration
{
	// Token: 0x02000709 RID: 1801
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsManageabilityAttribute : Attribute
	{
		// Token: 0x0600373E RID: 14142 RVA: 0x000EAD66 File Offset: 0x000E9D66
		public SettingsManageabilityAttribute(SettingsManageability manageability)
		{
			this._manageability = manageability;
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x0600373F RID: 14143 RVA: 0x000EAD75 File Offset: 0x000E9D75
		public SettingsManageability Manageability
		{
			get
			{
				return this._manageability;
			}
		}

		// Token: 0x040031B0 RID: 12720
		private readonly SettingsManageability _manageability;
	}
}
