using System;

namespace System.Configuration
{
	// Token: 0x020006FA RID: 1786
	public interface ISettingsProviderService
	{
		// Token: 0x0600370A RID: 14090
		SettingsProvider GetSettingsProvider(SettingsProperty property);
	}
}
