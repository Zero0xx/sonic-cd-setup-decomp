using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200065E RID: 1630
	public sealed class ServicePointManagerElement : ConfigurationElement
	{
		// Token: 0x0600326F RID: 12911 RVA: 0x000D661C File Offset: 0x000D561C
		public ServicePointManagerElement()
		{
			this.properties.Add(this.checkCertificateName);
			this.properties.Add(this.checkCertificateRevocationList);
			this.properties.Add(this.dnsRefreshTimeout);
			this.properties.Add(this.enableDnsRoundRobin);
			this.properties.Add(this.expect100Continue);
			this.properties.Add(this.useNagleAlgorithm);
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000D6774 File Offset: 0x000D5774
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation[] array = new PropertyInformation[]
			{
				base.ElementInformation.Properties["checkCertificateName"],
				base.ElementInformation.Properties["checkCertificateRevocationList"]
			};
			foreach (PropertyInformation propertyInformation in array)
			{
				if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere)
				{
					try
					{
						ExceptionHelper.UnmanagedPermission.Demand();
					}
					catch (Exception inner)
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_property_permission", new object[]
						{
							propertyInformation.Name
						}), inner);
					}
				}
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06003271 RID: 12913 RVA: 0x000D6830 File Offset: 0x000D5830
		// (set) Token: 0x06003272 RID: 12914 RVA: 0x000D6843 File Offset: 0x000D5843
		[ConfigurationProperty("checkCertificateName", DefaultValue = true)]
		public bool CheckCertificateName
		{
			get
			{
				return (bool)base[this.checkCertificateName];
			}
			set
			{
				base[this.checkCertificateName] = value;
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x000D6857 File Offset: 0x000D5857
		// (set) Token: 0x06003274 RID: 12916 RVA: 0x000D686A File Offset: 0x000D586A
		[ConfigurationProperty("checkCertificateRevocationList", DefaultValue = false)]
		public bool CheckCertificateRevocationList
		{
			get
			{
				return (bool)base[this.checkCertificateRevocationList];
			}
			set
			{
				base[this.checkCertificateRevocationList] = value;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x000D687E File Offset: 0x000D587E
		// (set) Token: 0x06003276 RID: 12918 RVA: 0x000D6891 File Offset: 0x000D5891
		[ConfigurationProperty("dnsRefreshTimeout", DefaultValue = 120000)]
		public int DnsRefreshTimeout
		{
			get
			{
				return (int)base[this.dnsRefreshTimeout];
			}
			set
			{
				base[this.dnsRefreshTimeout] = value;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06003277 RID: 12919 RVA: 0x000D68A5 File Offset: 0x000D58A5
		// (set) Token: 0x06003278 RID: 12920 RVA: 0x000D68B8 File Offset: 0x000D58B8
		[ConfigurationProperty("enableDnsRoundRobin", DefaultValue = false)]
		public bool EnableDnsRoundRobin
		{
			get
			{
				return (bool)base[this.enableDnsRoundRobin];
			}
			set
			{
				base[this.enableDnsRoundRobin] = value;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06003279 RID: 12921 RVA: 0x000D68CC File Offset: 0x000D58CC
		// (set) Token: 0x0600327A RID: 12922 RVA: 0x000D68DF File Offset: 0x000D58DF
		[ConfigurationProperty("expect100Continue", DefaultValue = true)]
		public bool Expect100Continue
		{
			get
			{
				return (bool)base[this.expect100Continue];
			}
			set
			{
				base[this.expect100Continue] = value;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x0600327B RID: 12923 RVA: 0x000D68F3 File Offset: 0x000D58F3
		// (set) Token: 0x0600327C RID: 12924 RVA: 0x000D6906 File Offset: 0x000D5906
		[ConfigurationProperty("useNagleAlgorithm", DefaultValue = true)]
		public bool UseNagleAlgorithm
		{
			get
			{
				return (bool)base[this.useNagleAlgorithm];
			}
			set
			{
				base[this.useNagleAlgorithm] = value;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x0600327D RID: 12925 RVA: 0x000D691A File Offset: 0x000D591A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F3D RID: 12093
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F3E RID: 12094
		private readonly ConfigurationProperty checkCertificateName = new ConfigurationProperty("checkCertificateName", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002F3F RID: 12095
		private readonly ConfigurationProperty checkCertificateRevocationList = new ConfigurationProperty("checkCertificateRevocationList", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F40 RID: 12096
		private readonly ConfigurationProperty dnsRefreshTimeout = new ConfigurationProperty("dnsRefreshTimeout", typeof(int), 120000, null, new TimeoutValidator(true), ConfigurationPropertyOptions.None);

		// Token: 0x04002F41 RID: 12097
		private readonly ConfigurationProperty enableDnsRoundRobin = new ConfigurationProperty("enableDnsRoundRobin", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F42 RID: 12098
		private readonly ConfigurationProperty expect100Continue = new ConfigurationProperty("expect100Continue", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002F43 RID: 12099
		private readonly ConfigurationProperty useNagleAlgorithm = new ConfigurationProperty("useNagleAlgorithm", typeof(bool), true, ConfigurationPropertyOptions.None);
	}
}
