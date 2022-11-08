using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace System.Net.Configuration
{
	// Token: 0x02000669 RID: 1641
	public sealed class WebRequestModuleElement : ConfigurationElement
	{
		// Token: 0x060032BC RID: 12988 RVA: 0x000D72C8 File Offset: 0x000D62C8
		public WebRequestModuleElement()
		{
			this.properties.Add(this.prefix);
			this.properties.Add(this.type);
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x000D7346 File Offset: 0x000D6346
		public WebRequestModuleElement(string prefix, string type) : this()
		{
			this.Prefix = prefix;
			base[this.type] = new WebRequestModuleElement.TypeAndName(type);
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x000D7367 File Offset: 0x000D6367
		public WebRequestModuleElement(string prefix, Type type) : this()
		{
			this.Prefix = prefix;
			this.Type = type;
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x000D737D File Offset: 0x000D637D
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x060032C0 RID: 12992 RVA: 0x000D7385 File Offset: 0x000D6385
		// (set) Token: 0x060032C1 RID: 12993 RVA: 0x000D7398 File Offset: 0x000D6398
		[ConfigurationProperty("prefix", IsRequired = true, IsKey = true)]
		public string Prefix
		{
			get
			{
				return (string)base[this.prefix];
			}
			set
			{
				base[this.prefix] = value;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x060032C2 RID: 12994 RVA: 0x000D73A8 File Offset: 0x000D63A8
		// (set) Token: 0x060032C3 RID: 12995 RVA: 0x000D73D2 File Offset: 0x000D63D2
		[TypeConverter(typeof(WebRequestModuleElement.TypeTypeConverter))]
		[ConfigurationProperty("type")]
		public Type Type
		{
			get
			{
				WebRequestModuleElement.TypeAndName typeAndName = (WebRequestModuleElement.TypeAndName)base[this.type];
				if (typeAndName != null)
				{
					return typeAndName.type;
				}
				return null;
			}
			set
			{
				base[this.type] = new WebRequestModuleElement.TypeAndName(value);
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x060032C4 RID: 12996 RVA: 0x000D73E6 File Offset: 0x000D63E6
		internal string Key
		{
			get
			{
				return this.Prefix;
			}
		}

		// Token: 0x04002F64 RID: 12132
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F65 RID: 12133
		private readonly ConfigurationProperty prefix = new ConfigurationProperty("prefix", typeof(string), null, ConfigurationPropertyOptions.IsKey);

		// Token: 0x04002F66 RID: 12134
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(WebRequestModuleElement.TypeAndName), null, new WebRequestModuleElement.TypeTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x0200066A RID: 1642
		private class TypeAndName
		{
			// Token: 0x060032C5 RID: 12997 RVA: 0x000D73EE File Offset: 0x000D63EE
			public TypeAndName(string name)
			{
				this.type = Type.GetType(name, true, true);
				this.name = name;
			}

			// Token: 0x060032C6 RID: 12998 RVA: 0x000D740B File Offset: 0x000D640B
			public TypeAndName(Type type)
			{
				this.type = type;
			}

			// Token: 0x060032C7 RID: 12999 RVA: 0x000D741A File Offset: 0x000D641A
			public override int GetHashCode()
			{
				return this.type.GetHashCode();
			}

			// Token: 0x060032C8 RID: 13000 RVA: 0x000D7427 File Offset: 0x000D6427
			public override bool Equals(object comparand)
			{
				return this.type.Equals(((WebRequestModuleElement.TypeAndName)comparand).type);
			}

			// Token: 0x04002F67 RID: 12135
			public readonly Type type;

			// Token: 0x04002F68 RID: 12136
			public readonly string name;
		}

		// Token: 0x0200066B RID: 1643
		private class TypeTypeConverter : TypeConverter
		{
			// Token: 0x060032C9 RID: 13001 RVA: 0x000D743F File Offset: 0x000D643F
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x060032CA RID: 13002 RVA: 0x000D7458 File Offset: 0x000D6458
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string)
				{
					return new WebRequestModuleElement.TypeAndName((string)value);
				}
				return base.ConvertFrom(context, culture, value);
			}

			// Token: 0x060032CB RID: 13003 RVA: 0x000D7478 File Offset: 0x000D6478
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType != typeof(string))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				WebRequestModuleElement.TypeAndName typeAndName = (WebRequestModuleElement.TypeAndName)value;
				if (typeAndName.name != null)
				{
					return typeAndName.name;
				}
				return typeAndName.type.AssemblyQualifiedName;
			}
		}
	}
}
