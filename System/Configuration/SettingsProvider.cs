using System;
using System.Configuration.Provider;

namespace System.Configuration
{
	// Token: 0x020006FB RID: 1787
	public abstract class SettingsProvider : ProviderBase
	{
		// Token: 0x0600370B RID: 14091
		public abstract SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection);

		// Token: 0x0600370C RID: 14092
		public abstract void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection);

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x0600370D RID: 14093
		// (set) Token: 0x0600370E RID: 14094
		public abstract string ApplicationName { get; set; }
	}
}
