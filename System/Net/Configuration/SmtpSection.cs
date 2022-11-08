using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Net.Mail;

namespace System.Net.Configuration
{
	// Token: 0x0200065F RID: 1631
	public sealed class SmtpSection : ConfigurationSection
	{
		// Token: 0x0600327E RID: 12926 RVA: 0x000D6924 File Offset: 0x000D5924
		public SmtpSection()
		{
			this.properties.Add(this.deliveryMethod);
			this.properties.Add(this.from);
			this.properties.Add(this.network);
			this.properties.Add(this.specifiedPickupDirectory);
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x0600327F RID: 12927 RVA: 0x000D6A01 File Offset: 0x000D5A01
		// (set) Token: 0x06003280 RID: 12928 RVA: 0x000D6A14 File Offset: 0x000D5A14
		[ConfigurationProperty("deliveryMethod", DefaultValue = SmtpDeliveryMethod.Network)]
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return (SmtpDeliveryMethod)base[this.deliveryMethod];
			}
			set
			{
				base[this.deliveryMethod] = value;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x000D6A28 File Offset: 0x000D5A28
		// (set) Token: 0x06003282 RID: 12930 RVA: 0x000D6A3B File Offset: 0x000D5A3B
		[ConfigurationProperty("from")]
		public string From
		{
			get
			{
				return (string)base[this.from];
			}
			set
			{
				base[this.from] = value;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06003283 RID: 12931 RVA: 0x000D6A4A File Offset: 0x000D5A4A
		[ConfigurationProperty("network")]
		public SmtpNetworkElement Network
		{
			get
			{
				return (SmtpNetworkElement)base[this.network];
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06003284 RID: 12932 RVA: 0x000D6A5D File Offset: 0x000D5A5D
		[ConfigurationProperty("specifiedPickupDirectory")]
		public SmtpSpecifiedPickupDirectoryElement SpecifiedPickupDirectory
		{
			get
			{
				return (SmtpSpecifiedPickupDirectoryElement)base[this.specifiedPickupDirectory];
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06003285 RID: 12933 RVA: 0x000D6A70 File Offset: 0x000D5A70
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F44 RID: 12100
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F45 RID: 12101
		private readonly ConfigurationProperty from = new ConfigurationProperty("from", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F46 RID: 12102
		private readonly ConfigurationProperty network = new ConfigurationProperty("network", typeof(SmtpNetworkElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F47 RID: 12103
		private readonly ConfigurationProperty specifiedPickupDirectory = new ConfigurationProperty("specifiedPickupDirectory", typeof(SmtpSpecifiedPickupDirectoryElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F48 RID: 12104
		private readonly ConfigurationProperty deliveryMethod = new ConfigurationProperty("deliveryMethod", typeof(SmtpDeliveryMethod), SmtpDeliveryMethod.Network, new SmtpSection.SmtpDeliveryMethodTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x02000660 RID: 1632
		private class SmtpDeliveryMethodTypeConverter : TypeConverter
		{
			// Token: 0x06003286 RID: 12934 RVA: 0x000D6A78 File Offset: 0x000D5A78
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06003287 RID: 12935 RVA: 0x000D6A94 File Offset: 0x000D5A94
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text != null)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					string a;
					if ((a = text) != null)
					{
						if (a == "network")
						{
							return SmtpDeliveryMethod.Network;
						}
						if (a == "specifiedpickupdirectory")
						{
							return SmtpDeliveryMethod.SpecifiedPickupDirectory;
						}
						if (a == "pickupdirectoryfromiis")
						{
							return SmtpDeliveryMethod.PickupDirectoryFromIis;
						}
					}
				}
				return base.ConvertFrom(context, culture, value);
			}
		}
	}
}
