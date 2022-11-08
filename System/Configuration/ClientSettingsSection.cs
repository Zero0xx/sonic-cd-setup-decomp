using System;

namespace System.Configuration
{
	// Token: 0x0200071F RID: 1823
	public sealed class ClientSettingsSection : ConfigurationSection
	{
		// Token: 0x060037A4 RID: 14244 RVA: 0x000EBC15 File Offset: 0x000EAC15
		static ClientSettingsSection()
		{
			ClientSettingsSection._properties = new ConfigurationPropertyCollection();
			ClientSettingsSection._properties.Add(ClientSettingsSection._propSettings);
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x060037A6 RID: 14246 RVA: 0x000EBC4F File Offset: 0x000EAC4F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ClientSettingsSection._properties;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x060037A7 RID: 14247 RVA: 0x000EBC56 File Offset: 0x000EAC56
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public SettingElementCollection Settings
		{
			get
			{
				return (SettingElementCollection)base[ClientSettingsSection._propSettings];
			}
		}

		// Token: 0x040031D3 RID: 12755
		private static ConfigurationPropertyCollection _properties;

		// Token: 0x040031D4 RID: 12756
		private static readonly ConfigurationProperty _propSettings = new ConfigurationProperty(null, typeof(SettingElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
