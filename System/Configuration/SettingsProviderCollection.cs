using System;
using System.Configuration.Provider;

namespace System.Configuration
{
	// Token: 0x02000718 RID: 1816
	public class SettingsProviderCollection : ProviderCollection
	{
		// Token: 0x0600379A RID: 14234 RVA: 0x000EBADC File Offset: 0x000EAADC
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is SettingsProvider))
			{
				throw new ArgumentException(SR.GetString("Config_provider_must_implement_type", new object[]
				{
					typeof(SettingsProvider).ToString()
				}), "provider");
			}
			base.Add(provider);
		}

		// Token: 0x17000CE6 RID: 3302
		public SettingsProvider this[string name]
		{
			get
			{
				return (SettingsProvider)base[name];
			}
		}
	}
}
