using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000664 RID: 1636
	public sealed class SmtpSpecifiedPickupDirectoryElement : ConfigurationElement
	{
		// Token: 0x060032A7 RID: 12967 RVA: 0x000D6FA1 File Offset: 0x000D5FA1
		public SmtpSpecifiedPickupDirectoryElement()
		{
			this.properties.Add(this.pickupDirectoryLocation);
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000D6FE1 File Offset: 0x000D5FE1
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060032A9 RID: 12969 RVA: 0x000D6FE9 File Offset: 0x000D5FE9
		// (set) Token: 0x060032AA RID: 12970 RVA: 0x000D6FFC File Offset: 0x000D5FFC
		[ConfigurationProperty("pickupDirectoryLocation")]
		public string PickupDirectoryLocation
		{
			get
			{
				return (string)base[this.pickupDirectoryLocation];
			}
			set
			{
				base[this.pickupDirectoryLocation] = value;
			}
		}

		// Token: 0x04002F5B RID: 12123
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F5C RID: 12124
		private readonly ConfigurationProperty pickupDirectoryLocation = new ConfigurationProperty("pickupDirectoryLocation", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}
