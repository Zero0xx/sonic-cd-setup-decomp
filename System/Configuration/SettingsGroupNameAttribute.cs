using System;

namespace System.Configuration
{
	// Token: 0x02000708 RID: 1800
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SettingsGroupNameAttribute : Attribute
	{
		// Token: 0x0600373C RID: 14140 RVA: 0x000EAD4F File Offset: 0x000E9D4F
		public SettingsGroupNameAttribute(string groupName)
		{
			this._groupName = groupName;
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x000EAD5E File Offset: 0x000E9D5E
		public string GroupName
		{
			get
			{
				return this._groupName;
			}
		}

		// Token: 0x040031AF RID: 12719
		private readonly string _groupName;
	}
}
