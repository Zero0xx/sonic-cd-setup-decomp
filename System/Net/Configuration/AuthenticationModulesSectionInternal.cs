using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000642 RID: 1602
	internal sealed class AuthenticationModulesSectionInternal
	{
		// Token: 0x0600319D RID: 12701 RVA: 0x000D4580 File Offset: 0x000D3580
		internal AuthenticationModulesSectionInternal(AuthenticationModulesSection section)
		{
			if (section.AuthenticationModules.Count > 0)
			{
				this.authenticationModules = new List<Type>(section.AuthenticationModules.Count);
				foreach (object obj in section.AuthenticationModules)
				{
					AuthenticationModuleElement authenticationModuleElement = (AuthenticationModuleElement)obj;
					Type type = null;
					try
					{
						type = Type.GetType(authenticationModuleElement.Type, true, true);
						if (!typeof(IAuthenticationModule).IsAssignableFrom(type))
						{
							throw new InvalidCastException(SR.GetString("net_invalid_cast", new object[]
							{
								type.FullName,
								"IAuthenticationModule"
							}));
						}
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_authenticationmodules"), ex);
					}
					catch
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_authenticationmodules"), new Exception(SR.GetString("net_nonClsCompliantException")));
					}
					this.authenticationModules.Add(type);
				}
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x0600319E RID: 12702 RVA: 0x000D46BC File Offset: 0x000D36BC
		internal List<Type> AuthenticationModules
		{
			get
			{
				List<Type> list = this.authenticationModules;
				if (list == null)
				{
					list = new List<Type>(0);
				}
				return list;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x0600319F RID: 12703 RVA: 0x000D46DC File Offset: 0x000D36DC
		internal static object ClassSyncObject
		{
			get
			{
				if (AuthenticationModulesSectionInternal.classSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref AuthenticationModulesSectionInternal.classSyncObject, value, null);
				}
				return AuthenticationModulesSectionInternal.classSyncObject;
			}
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000D4708 File Offset: 0x000D3708
		internal static AuthenticationModulesSectionInternal GetSection()
		{
			AuthenticationModulesSectionInternal result;
			lock (AuthenticationModulesSectionInternal.ClassSyncObject)
			{
				AuthenticationModulesSection authenticationModulesSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.AuthenticationModulesSectionPath) as AuthenticationModulesSection;
				if (authenticationModulesSection == null)
				{
					result = null;
				}
				else
				{
					result = new AuthenticationModulesSectionInternal(authenticationModulesSection);
				}
			}
			return result;
		}

		// Token: 0x04002E90 RID: 11920
		private List<Type> authenticationModules;

		// Token: 0x04002E91 RID: 11921
		private static object classSyncObject;
	}
}
