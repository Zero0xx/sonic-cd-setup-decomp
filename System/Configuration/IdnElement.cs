using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x02000672 RID: 1650
	public sealed class IdnElement : ConfigurationElement
	{
		// Token: 0x060032F0 RID: 13040 RVA: 0x000D7A2C File Offset: 0x000D6A2C
		public IdnElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x060032F1 RID: 13041 RVA: 0x000D7A82 File Offset: 0x000D6A82
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x060032F2 RID: 13042 RVA: 0x000D7A8A File Offset: 0x000D6A8A
		// (set) Token: 0x060032F3 RID: 13043 RVA: 0x000D7A9D File Offset: 0x000D6A9D
		[ConfigurationProperty("enabled", DefaultValue = UriIdnScope.None)]
		public UriIdnScope Enabled
		{
			get
			{
				return (UriIdnScope)base[this.enabled];
			}
			set
			{
				base[this.enabled] = value;
			}
		}

		// Token: 0x04002F75 RID: 12149
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F76 RID: 12150
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(UriIdnScope), UriIdnScope.None, new IdnElement.UriIdnScopeTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x02000673 RID: 1651
		private class UriIdnScopeTypeConverter : TypeConverter
		{
			// Token: 0x060032F4 RID: 13044 RVA: 0x000D7AB1 File Offset: 0x000D6AB1
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x060032F5 RID: 13045 RVA: 0x000D7ACC File Offset: 0x000D6ACC
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text != null)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					string a;
					if ((a = text) != null)
					{
						if (a == "all")
						{
							return UriIdnScope.All;
						}
						if (a == "none")
						{
							return UriIdnScope.None;
						}
						if (a == "allexceptintranet")
						{
							return UriIdnScope.AllExceptIntranet;
						}
					}
				}
				return base.ConvertFrom(context, culture, value);
			}
		}
	}
}
