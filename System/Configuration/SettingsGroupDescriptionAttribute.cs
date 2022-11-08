using System;

namespace System.Configuration
{
	// Token: 0x02000707 RID: 1799
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SettingsGroupDescriptionAttribute : Attribute
	{
		// Token: 0x0600373A RID: 14138 RVA: 0x000EAD38 File Offset: 0x000E9D38
		public SettingsGroupDescriptionAttribute(string description)
		{
			this._desc = description;
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x000EAD47 File Offset: 0x000E9D47
		public string Description
		{
			get
			{
				return this._desc;
			}
		}

		// Token: 0x040031AE RID: 12718
		private readonly string _desc;
	}
}
