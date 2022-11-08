using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200064D RID: 1613
	public sealed class HttpWebRequestElement : ConfigurationElement
	{
		// Token: 0x060031EC RID: 12780 RVA: 0x000D5338 File Offset: 0x000D4338
		public HttpWebRequestElement()
		{
			this.properties.Add(this.maximumResponseHeadersLength);
			this.properties.Add(this.maximumErrorResponseLength);
			this.properties.Add(this.maximumUnauthorizedUploadLength);
			this.properties.Add(this.useUnsafeHeaderParsing);
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x000D5420 File Offset: 0x000D4420
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation[] array = new PropertyInformation[]
			{
				base.ElementInformation.Properties["maximumResponseHeadersLength"],
				base.ElementInformation.Properties["maximumErrorResponseLength"]
			};
			foreach (PropertyInformation propertyInformation in array)
			{
				if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere)
				{
					try
					{
						ExceptionHelper.WebPermissionUnrestricted.Demand();
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

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x000D54DC File Offset: 0x000D44DC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x000D54E4 File Offset: 0x000D44E4
		// (set) Token: 0x060031F0 RID: 12784 RVA: 0x000D54F7 File Offset: 0x000D44F7
		[ConfigurationProperty("maximumUnauthorizedUploadLength", DefaultValue = -1)]
		public int MaximumUnauthorizedUploadLength
		{
			get
			{
				return (int)base[this.maximumUnauthorizedUploadLength];
			}
			set
			{
				base[this.maximumUnauthorizedUploadLength] = value;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x000D550B File Offset: 0x000D450B
		// (set) Token: 0x060031F2 RID: 12786 RVA: 0x000D551E File Offset: 0x000D451E
		[ConfigurationProperty("maximumErrorResponseLength", DefaultValue = 64)]
		public int MaximumErrorResponseLength
		{
			get
			{
				return (int)base[this.maximumErrorResponseLength];
			}
			set
			{
				base[this.maximumErrorResponseLength] = value;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060031F3 RID: 12787 RVA: 0x000D5532 File Offset: 0x000D4532
		// (set) Token: 0x060031F4 RID: 12788 RVA: 0x000D5545 File Offset: 0x000D4545
		[ConfigurationProperty("maximumResponseHeadersLength", DefaultValue = 64)]
		public int MaximumResponseHeadersLength
		{
			get
			{
				return (int)base[this.maximumResponseHeadersLength];
			}
			set
			{
				base[this.maximumResponseHeadersLength] = value;
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x000D5559 File Offset: 0x000D4559
		// (set) Token: 0x060031F6 RID: 12790 RVA: 0x000D556C File Offset: 0x000D456C
		[ConfigurationProperty("useUnsafeHeaderParsing", DefaultValue = false)]
		public bool UseUnsafeHeaderParsing
		{
			get
			{
				return (bool)base[this.useUnsafeHeaderParsing];
			}
			set
			{
				base[this.useUnsafeHeaderParsing] = value;
			}
		}

		// Token: 0x04002EED RID: 12013
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EEE RID: 12014
		private readonly ConfigurationProperty maximumResponseHeadersLength = new ConfigurationProperty("maximumResponseHeadersLength", typeof(int), 64, ConfigurationPropertyOptions.None);

		// Token: 0x04002EEF RID: 12015
		private readonly ConfigurationProperty maximumErrorResponseLength = new ConfigurationProperty("maximumErrorResponseLength", typeof(int), 64, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF0 RID: 12016
		private readonly ConfigurationProperty maximumUnauthorizedUploadLength = new ConfigurationProperty("maximumUnauthorizedUploadLength", typeof(int), -1, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF1 RID: 12017
		private readonly ConfigurationProperty useUnsafeHeaderParsing = new ConfigurationProperty("useUnsafeHeaderParsing", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
