using System;

namespace System.Configuration
{
	// Token: 0x02000706 RID: 1798
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SettingsDescriptionAttribute : Attribute
	{
		// Token: 0x06003738 RID: 14136 RVA: 0x000EAD21 File Offset: 0x000E9D21
		public SettingsDescriptionAttribute(string description)
		{
			this._desc = description;
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06003739 RID: 14137 RVA: 0x000EAD30 File Offset: 0x000E9D30
		public string Description
		{
			get
			{
				return this._desc;
			}
		}

		// Token: 0x040031AD RID: 12717
		private readonly string _desc;
	}
}
