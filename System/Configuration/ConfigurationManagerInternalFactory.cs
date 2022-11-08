using System;
using System.Configuration.Internal;

namespace System.Configuration
{
	// Token: 0x020006EA RID: 1770
	internal static class ConfigurationManagerInternalFactory
	{
		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x060036B9 RID: 14009 RVA: 0x000E96D7 File Offset: 0x000E86D7
		internal static IConfigurationManagerInternal Instance
		{
			get
			{
				if (ConfigurationManagerInternalFactory.s_instance == null)
				{
					ConfigurationManagerInternalFactory.s_instance = (IConfigurationManagerInternal)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.Internal.ConfigurationManagerInternal, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				}
				return ConfigurationManagerInternalFactory.s_instance;
			}
		}

		// Token: 0x04003190 RID: 12688
		private const string ConfigurationManagerInternalTypeString = "System.Configuration.Internal.ConfigurationManagerInternal, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		// Token: 0x04003191 RID: 12689
		private static IConfigurationManagerInternal s_instance;
	}
}
