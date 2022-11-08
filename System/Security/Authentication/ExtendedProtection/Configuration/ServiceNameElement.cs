using System;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x0200034F RID: 847
	public sealed class ServiceNameElement : ConfigurationElement
	{
		// Token: 0x06001A92 RID: 6802 RVA: 0x0005CAF4 File Offset: 0x0005BAF4
		public ServiceNameElement()
		{
			this.properties.Add(this.name);
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x0005CB34 File Offset: 0x0005BB34
		// (set) Token: 0x06001A94 RID: 6804 RVA: 0x0005CB47 File Offset: 0x0005BB47
		[ConfigurationProperty("name")]
		public string Name
		{
			get
			{
				return (string)base[this.name];
			}
			set
			{
				base[this.name] = value;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x0005CB56 File Offset: 0x0005BB56
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x0005CB5E File Offset: 0x0005BB5E
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x04001B4F RID: 6991
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001B50 RID: 6992
		private readonly ConfigurationProperty name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
	}
}
