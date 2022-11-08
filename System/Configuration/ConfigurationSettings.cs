using System;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x020006EB RID: 1771
	public sealed class ConfigurationSettings
	{
		// Token: 0x060036BA RID: 14010 RVA: 0x000E96F9 File Offset: 0x000E86F9
		private ConfigurationSettings()
		{
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x060036BB RID: 14011 RVA: 0x000E9701 File Offset: 0x000E8701
		[Obsolete("This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.AppSettings")]
		public static NameValueCollection AppSettings
		{
			get
			{
				return ConfigurationManager.AppSettings;
			}
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000E9708 File Offset: 0x000E8708
		[Obsolete("This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.GetSection")]
		public static object GetConfig(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
