using System;

namespace System.Configuration
{
	// Token: 0x020006F9 RID: 1785
	public interface IPersistComponentSettings
	{
		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06003703 RID: 14083
		// (set) Token: 0x06003704 RID: 14084
		bool SaveSettings { get; set; }

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06003705 RID: 14085
		// (set) Token: 0x06003706 RID: 14086
		string SettingsKey { get; set; }

		// Token: 0x06003707 RID: 14087
		void LoadComponentSettings();

		// Token: 0x06003708 RID: 14088
		void SaveComponentSettings();

		// Token: 0x06003709 RID: 14089
		void ResetComponentSettings();
	}
}
