using System;
using System.Collections;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x0200066E RID: 1646
	internal sealed class WebRequestModulesSectionInternal
	{
		// Token: 0x060032DF RID: 13023 RVA: 0x000D76C8 File Offset: 0x000D66C8
		internal WebRequestModulesSectionInternal(WebRequestModulesSection section)
		{
			if (section.WebRequestModules.Count > 0)
			{
				this.webRequestModules = new ArrayList(section.WebRequestModules.Count);
				foreach (object obj in section.WebRequestModules)
				{
					WebRequestModuleElement webRequestModuleElement = (WebRequestModuleElement)obj;
					try
					{
						this.webRequestModules.Add(new WebRequestPrefixElement(webRequestModuleElement.Prefix, webRequestModuleElement.Type));
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_webrequestmodules"), ex);
					}
					catch
					{
						throw new ConfigurationErrorsException(ConfigurationStrings.WebRequestModulesSectionPath, new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x060032E0 RID: 13024 RVA: 0x000D77B8 File Offset: 0x000D67B8
		internal static object ClassSyncObject
		{
			get
			{
				if (WebRequestModulesSectionInternal.classSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref WebRequestModulesSectionInternal.classSyncObject, value, null);
				}
				return WebRequestModulesSectionInternal.classSyncObject;
			}
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000D77E4 File Offset: 0x000D67E4
		internal static WebRequestModulesSectionInternal GetSection()
		{
			WebRequestModulesSectionInternal result;
			lock (WebRequestModulesSectionInternal.ClassSyncObject)
			{
				WebRequestModulesSection webRequestModulesSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.WebRequestModulesSectionPath) as WebRequestModulesSection;
				if (webRequestModulesSection == null)
				{
					result = null;
				}
				else
				{
					result = new WebRequestModulesSectionInternal(webRequestModulesSection);
				}
			}
			return result;
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x060032E2 RID: 13026 RVA: 0x000D7838 File Offset: 0x000D6838
		internal ArrayList WebRequestModules
		{
			get
			{
				ArrayList arrayList = this.webRequestModules;
				if (arrayList == null)
				{
					arrayList = new ArrayList(0);
				}
				return arrayList;
			}
		}

		// Token: 0x04002F6B RID: 12139
		private static object classSyncObject;

		// Token: 0x04002F6C RID: 12140
		private ArrayList webRequestModules;
	}
}
