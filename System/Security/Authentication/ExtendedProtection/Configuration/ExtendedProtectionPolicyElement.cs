using System;
using System.Collections.Generic;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x0200034D RID: 845
	public sealed class ExtendedProtectionPolicyElement : ConfigurationElement
	{
		// Token: 0x06001A7C RID: 6780 RVA: 0x0005C868 File Offset: 0x0005B868
		public ExtendedProtectionPolicyElement()
		{
			this.properties.Add(this.policyEnforcement);
			this.properties.Add(this.protectionScenario);
			this.properties.Add(this.customServiceNames);
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x0005C91B File Offset: 0x0005B91B
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001A7E RID: 6782 RVA: 0x0005C923 File Offset: 0x0005B923
		// (set) Token: 0x06001A7F RID: 6783 RVA: 0x0005C936 File Offset: 0x0005B936
		[ConfigurationProperty("policyEnforcement")]
		public PolicyEnforcement PolicyEnforcement
		{
			get
			{
				return (PolicyEnforcement)base[this.policyEnforcement];
			}
			set
			{
				base[this.policyEnforcement] = value;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x0005C94A File Offset: 0x0005B94A
		// (set) Token: 0x06001A81 RID: 6785 RVA: 0x0005C95D File Offset: 0x0005B95D
		[ConfigurationProperty("protectionScenario", DefaultValue = ProtectionScenario.TransportSelected)]
		public ProtectionScenario ProtectionScenario
		{
			get
			{
				return (ProtectionScenario)base[this.protectionScenario];
			}
			set
			{
				base[this.protectionScenario] = value;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001A82 RID: 6786 RVA: 0x0005C971 File Offset: 0x0005B971
		[ConfigurationProperty("customServiceNames")]
		public ServiceNameElementCollection CustomServiceNames
		{
			get
			{
				return (ServiceNameElementCollection)base[this.customServiceNames];
			}
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0005C984 File Offset: 0x0005B984
		public ExtendedProtectionPolicy BuildPolicy()
		{
			if (this.PolicyEnforcement == PolicyEnforcement.Never)
			{
				return new ExtendedProtectionPolicy(PolicyEnforcement.Never);
			}
			ServiceNameCollection serviceNameCollection = null;
			ServiceNameElementCollection serviceNameElementCollection = this.CustomServiceNames;
			if (serviceNameElementCollection != null && serviceNameElementCollection.Count > 0)
			{
				List<string> list = new List<string>(serviceNameElementCollection.Count);
				foreach (object obj in serviceNameElementCollection)
				{
					ServiceNameElement serviceNameElement = (ServiceNameElement)obj;
					list.Add(serviceNameElement.Name);
				}
				serviceNameCollection = new ServiceNameCollection(list);
			}
			return new ExtendedProtectionPolicy(this.PolicyEnforcement, this.ProtectionScenario, serviceNameCollection);
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001A84 RID: 6788 RVA: 0x0005CA30 File Offset: 0x0005BA30
		private static PolicyEnforcement DefaultPolicyEnforcement
		{
			get
			{
				return PolicyEnforcement.Never;
			}
		}

		// Token: 0x04001B4B RID: 6987
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001B4C RID: 6988
		private readonly ConfigurationProperty policyEnforcement = new ConfigurationProperty("policyEnforcement", typeof(PolicyEnforcement), ExtendedProtectionPolicyElement.DefaultPolicyEnforcement, ConfigurationPropertyOptions.None);

		// Token: 0x04001B4D RID: 6989
		private readonly ConfigurationProperty protectionScenario = new ConfigurationProperty("protectionScenario", typeof(ProtectionScenario), ProtectionScenario.TransportSelected, ConfigurationPropertyOptions.None);

		// Token: 0x04001B4E RID: 6990
		private readonly ConfigurationProperty customServiceNames = new ConfigurationProperty("customServiceNames", typeof(ServiceNameElementCollection), null, ConfigurationPropertyOptions.None);
	}
}
