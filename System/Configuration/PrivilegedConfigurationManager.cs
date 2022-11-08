using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020007A2 RID: 1954
	[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
	internal static class PrivilegedConfigurationManager
	{
		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003C15 RID: 15381 RVA: 0x00100E64 File Offset: 0x000FFE64
		internal static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return ConfigurationManager.ConnectionStrings;
			}
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x00100E6B File Offset: 0x000FFE6B
		internal static object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
