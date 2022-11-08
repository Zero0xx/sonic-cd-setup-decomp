using System;
using System.Configuration;
using System.Net.Mail;

namespace System.Net.Configuration
{
	// Token: 0x02000662 RID: 1634
	public sealed class SmtpNetworkElement : ConfigurationElement
	{
		// Token: 0x06003290 RID: 12944 RVA: 0x000D6BEC File Offset: 0x000D5BEC
		public SmtpNetworkElement()
		{
			this.properties.Add(this.defaultCredentials);
			this.properties.Add(this.host);
			this.properties.Add(this.clientDomain);
			this.properties.Add(this.password);
			this.properties.Add(this.port);
			this.properties.Add(this.userName);
			this.properties.Add(this.targetName);
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x000D6D5C File Offset: 0x000D5D5C
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation propertyInformation = base.ElementInformation.Properties["port"];
			if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere && (int)propertyInformation.Value != (int)propertyInformation.DefaultValue)
			{
				try
				{
					new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
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

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06003292 RID: 12946 RVA: 0x000D6DF0 File Offset: 0x000D5DF0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06003293 RID: 12947 RVA: 0x000D6DF8 File Offset: 0x000D5DF8
		// (set) Token: 0x06003294 RID: 12948 RVA: 0x000D6E0B File Offset: 0x000D5E0B
		[ConfigurationProperty("defaultCredentials", DefaultValue = false)]
		public bool DefaultCredentials
		{
			get
			{
				return (bool)base[this.defaultCredentials];
			}
			set
			{
				base[this.defaultCredentials] = value;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06003295 RID: 12949 RVA: 0x000D6E1F File Offset: 0x000D5E1F
		// (set) Token: 0x06003296 RID: 12950 RVA: 0x000D6E32 File Offset: 0x000D5E32
		[ConfigurationProperty("host")]
		public string Host
		{
			get
			{
				return (string)base[this.host];
			}
			set
			{
				base[this.host] = value;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x000D6E41 File Offset: 0x000D5E41
		// (set) Token: 0x06003298 RID: 12952 RVA: 0x000D6E54 File Offset: 0x000D5E54
		[ConfigurationProperty("clientDomain")]
		public string ClientDomain
		{
			get
			{
				return (string)base[this.clientDomain];
			}
			set
			{
				base[this.clientDomain] = value;
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x000D6E63 File Offset: 0x000D5E63
		// (set) Token: 0x0600329A RID: 12954 RVA: 0x000D6E76 File Offset: 0x000D5E76
		[ConfigurationProperty("targetName")]
		public string TargetName
		{
			get
			{
				return (string)base[this.targetName];
			}
			set
			{
				base[this.targetName] = value;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600329B RID: 12955 RVA: 0x000D6E85 File Offset: 0x000D5E85
		// (set) Token: 0x0600329C RID: 12956 RVA: 0x000D6E98 File Offset: 0x000D5E98
		[ConfigurationProperty("password")]
		public string Password
		{
			get
			{
				return (string)base[this.password];
			}
			set
			{
				base[this.password] = value;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600329D RID: 12957 RVA: 0x000D6EA7 File Offset: 0x000D5EA7
		// (set) Token: 0x0600329E RID: 12958 RVA: 0x000D6EBA File Offset: 0x000D5EBA
		[ConfigurationProperty("port", DefaultValue = 25)]
		public int Port
		{
			get
			{
				return (int)base[this.port];
			}
			set
			{
				base[this.port] = value;
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600329F RID: 12959 RVA: 0x000D6ECE File Offset: 0x000D5ECE
		// (set) Token: 0x060032A0 RID: 12960 RVA: 0x000D6EE1 File Offset: 0x000D5EE1
		[ConfigurationProperty("userName")]
		public string UserName
		{
			get
			{
				return (string)base[this.userName];
			}
			set
			{
				base[this.userName] = value;
			}
		}

		// Token: 0x04002F4E RID: 12110
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F4F RID: 12111
		private readonly ConfigurationProperty defaultCredentials = new ConfigurationProperty("defaultCredentials", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F50 RID: 12112
		private readonly ConfigurationProperty host = new ConfigurationProperty("host", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F51 RID: 12113
		private readonly ConfigurationProperty clientDomain = new ConfigurationProperty("clientDomain", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F52 RID: 12114
		private readonly ConfigurationProperty password = new ConfigurationProperty("password", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F53 RID: 12115
		private readonly ConfigurationProperty port = new ConfigurationProperty("port", typeof(int), 25, null, new IntegerValidator(1, 65535), ConfigurationPropertyOptions.None);

		// Token: 0x04002F54 RID: 12116
		private readonly ConfigurationProperty userName = new ConfigurationProperty("userName", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F55 RID: 12117
		private readonly ConfigurationProperty targetName = new ConfigurationProperty("targetName", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}
