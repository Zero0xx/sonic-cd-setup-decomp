using System;
using System.Net.Configuration;

namespace System.Configuration.Internal
{
	// Token: 0x0200071C RID: 1820
	internal sealed class ConfigurationManagerHelper : IConfigurationManagerHelper
	{
		// Token: 0x060037A0 RID: 14240 RVA: 0x000EBBF6 File Offset: 0x000EABF6
		private ConfigurationManagerHelper()
		{
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000EBBFE File Offset: 0x000EABFE
		void IConfigurationManagerHelper.EnsureNetConfigLoaded()
		{
			SettingsSection.EnsureConfigLoaded();
		}
	}
}
