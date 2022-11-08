using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000641 RID: 1601
	public sealed class AuthenticationModulesSection : ConfigurationSection
	{
		// Token: 0x06003198 RID: 12696 RVA: 0x000D4422 File Offset: 0x000D3422
		public AuthenticationModulesSection()
		{
			this.properties.Add(this.authenticationModules);
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000D4460 File Offset: 0x000D3460
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.UnmanagedPermission.Demand();
			}
			catch (Exception inner)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[]
				{
					"authenticationModules"
				}), inner);
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x000D44BC File Offset: 0x000D34BC
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public AuthenticationModuleElementCollection AuthenticationModules
		{
			get
			{
				return (AuthenticationModuleElementCollection)base[this.authenticationModules];
			}
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000D44D0 File Offset: 0x000D34D0
		protected override void InitializeDefault()
		{
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(NegotiateClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(KerberosClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(NtlmClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(DigestClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(BasicClient).AssemblyQualifiedName));
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x0600319C RID: 12700 RVA: 0x000D4578 File Offset: 0x000D3578
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002E8E RID: 11918
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002E8F RID: 11919
		private readonly ConfigurationProperty authenticationModules = new ConfigurationProperty(null, typeof(AuthenticationModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
