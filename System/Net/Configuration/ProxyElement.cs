using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000656 RID: 1622
	public sealed class ProxyElement : ConfigurationElement
	{
		// Token: 0x06003225 RID: 12837 RVA: 0x000D5A94 File Offset: 0x000D4A94
		public ProxyElement()
		{
			this.properties.Add(this.autoDetect);
			this.properties.Add(this.scriptLocation);
			this.properties.Add(this.bypassonlocal);
			this.properties.Add(this.proxyaddress);
			this.properties.Add(this.usesystemdefault);
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06003226 RID: 12838 RVA: 0x000D5BE0 File Offset: 0x000D4BE0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x000D5BE8 File Offset: 0x000D4BE8
		// (set) Token: 0x06003228 RID: 12840 RVA: 0x000D5BFB File Offset: 0x000D4BFB
		[ConfigurationProperty("autoDetect", DefaultValue = ProxyElement.AutoDetectValues.Unspecified)]
		public ProxyElement.AutoDetectValues AutoDetect
		{
			get
			{
				return (ProxyElement.AutoDetectValues)base[this.autoDetect];
			}
			set
			{
				base[this.autoDetect] = value;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x000D5C0F File Offset: 0x000D4C0F
		// (set) Token: 0x0600322A RID: 12842 RVA: 0x000D5C22 File Offset: 0x000D4C22
		[ConfigurationProperty("scriptLocation")]
		public Uri ScriptLocation
		{
			get
			{
				return (Uri)base[this.scriptLocation];
			}
			set
			{
				base[this.scriptLocation] = value;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x000D5C31 File Offset: 0x000D4C31
		// (set) Token: 0x0600322C RID: 12844 RVA: 0x000D5C44 File Offset: 0x000D4C44
		[ConfigurationProperty("bypassonlocal", DefaultValue = ProxyElement.BypassOnLocalValues.Unspecified)]
		public ProxyElement.BypassOnLocalValues BypassOnLocal
		{
			get
			{
				return (ProxyElement.BypassOnLocalValues)base[this.bypassonlocal];
			}
			set
			{
				base[this.bypassonlocal] = value;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x000D5C58 File Offset: 0x000D4C58
		// (set) Token: 0x0600322E RID: 12846 RVA: 0x000D5C6B File Offset: 0x000D4C6B
		[ConfigurationProperty("proxyaddress")]
		public Uri ProxyAddress
		{
			get
			{
				return (Uri)base[this.proxyaddress];
			}
			set
			{
				base[this.proxyaddress] = value;
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x000D5C7A File Offset: 0x000D4C7A
		// (set) Token: 0x06003230 RID: 12848 RVA: 0x000D5C8D File Offset: 0x000D4C8D
		[ConfigurationProperty("usesystemdefault", DefaultValue = ProxyElement.UseSystemDefaultValues.Unspecified)]
		public ProxyElement.UseSystemDefaultValues UseSystemDefault
		{
			get
			{
				return (ProxyElement.UseSystemDefaultValues)base[this.usesystemdefault];
			}
			set
			{
				base[this.usesystemdefault] = value;
			}
		}

		// Token: 0x04002F02 RID: 12034
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F03 RID: 12035
		private readonly ConfigurationProperty autoDetect = new ConfigurationProperty("autoDetect", typeof(ProxyElement.AutoDetectValues), ProxyElement.AutoDetectValues.Unspecified, new EnumConverter(typeof(ProxyElement.AutoDetectValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F04 RID: 12036
		private readonly ConfigurationProperty scriptLocation = new ConfigurationProperty("scriptLocation", typeof(Uri), null, new UriTypeConverter(UriKind.Absolute), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F05 RID: 12037
		private readonly ConfigurationProperty bypassonlocal = new ConfigurationProperty("bypassonlocal", typeof(ProxyElement.BypassOnLocalValues), ProxyElement.BypassOnLocalValues.Unspecified, new EnumConverter(typeof(ProxyElement.BypassOnLocalValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F06 RID: 12038
		private readonly ConfigurationProperty proxyaddress = new ConfigurationProperty("proxyaddress", typeof(Uri), null, new UriTypeConverter(UriKind.Absolute), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F07 RID: 12039
		private readonly ConfigurationProperty usesystemdefault = new ConfigurationProperty("usesystemdefault", typeof(ProxyElement.UseSystemDefaultValues), ProxyElement.UseSystemDefaultValues.Unspecified, new EnumConverter(typeof(ProxyElement.UseSystemDefaultValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x02000657 RID: 1623
		public enum BypassOnLocalValues
		{
			// Token: 0x04002F09 RID: 12041
			Unspecified = -1,
			// Token: 0x04002F0A RID: 12042
			False,
			// Token: 0x04002F0B RID: 12043
			True
		}

		// Token: 0x02000658 RID: 1624
		public enum UseSystemDefaultValues
		{
			// Token: 0x04002F0D RID: 12045
			Unspecified = -1,
			// Token: 0x04002F0E RID: 12046
			False,
			// Token: 0x04002F0F RID: 12047
			True
		}

		// Token: 0x02000659 RID: 1625
		public enum AutoDetectValues
		{
			// Token: 0x04002F11 RID: 12049
			Unspecified = -1,
			// Token: 0x04002F12 RID: 12050
			False,
			// Token: 0x04002F13 RID: 12051
			True
		}
	}
}
