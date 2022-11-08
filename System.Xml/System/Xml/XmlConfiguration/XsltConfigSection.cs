using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Xml.XmlConfiguration
{
	// Token: 0x02000078 RID: 120
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class XsltConfigSection : ConfigurationSection
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00015C48 File Offset: 0x00014C48
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00015C5A File Offset: 0x00014C5A
		[ConfigurationProperty("prohibitDefaultResolver", DefaultValue = "false")]
		internal string ProhibitDefaultResolverString
		{
			get
			{
				return (string)base["prohibitDefaultResolver"];
			}
			set
			{
				base["prohibitDefaultResolver"] = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00015C68 File Offset: 0x00014C68
		private bool _ProhibitDefaultResolver
		{
			get
			{
				string prohibitDefaultResolverString = this.ProhibitDefaultResolverString;
				bool result;
				XmlConvert.TryToBoolean(prohibitDefaultResolverString, out result);
				return result;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00015C88 File Offset: 0x00014C88
		private static bool s_ProhibitDefaultUrlResolver
		{
			get
			{
				XsltConfigSection xsltConfigSection = ConfigurationManager.GetSection(XmlConfigurationString.XsltSectionPath) as XsltConfigSection;
				return xsltConfigSection != null && xsltConfigSection._ProhibitDefaultResolver;
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00015CB0 File Offset: 0x00014CB0
		internal static XmlResolver CreateDefaultResolver()
		{
			if (XsltConfigSection.s_ProhibitDefaultUrlResolver)
			{
				return XmlNullResolver.Singleton;
			}
			return new XmlUrlResolver();
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00015CC4 File Offset: 0x00014CC4
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x00015CD6 File Offset: 0x00014CD6
		[ConfigurationProperty("limitXPathComplexity", DefaultValue = "true")]
		internal string LimitXPathComplexityString
		{
			get
			{
				return (string)base["limitXPathComplexity"];
			}
			set
			{
				base["limitXPathComplexity"] = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00015CE4 File Offset: 0x00014CE4
		private bool _LimitXPathComplexity
		{
			get
			{
				string limitXPathComplexityString = this.LimitXPathComplexityString;
				bool result = true;
				XmlConvert.TryToBoolean(limitXPathComplexityString, out result);
				return result;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00015D04 File Offset: 0x00014D04
		public static bool LimitXPathComplexity
		{
			get
			{
				XsltConfigSection xsltConfigSection = ConfigurationManager.GetSection(XmlConfigurationString.XsltSectionPath) as XsltConfigSection;
				return xsltConfigSection == null || xsltConfigSection._LimitXPathComplexity;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00015D2C File Offset: 0x00014D2C
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x00015D3E File Offset: 0x00014D3E
		[ConfigurationProperty("enableMemberAccessForXslCompiledTransform", DefaultValue = "False")]
		internal string EnableMemberAccessForXslCompiledTransformString
		{
			get
			{
				return (string)base["enableMemberAccessForXslCompiledTransform"];
			}
			set
			{
				base["enableMemberAccessForXslCompiledTransform"] = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00015D4C File Offset: 0x00014D4C
		private bool _EnableMemberAccessForXslCompiledTransform
		{
			get
			{
				string enableMemberAccessForXslCompiledTransformString = this.EnableMemberAccessForXslCompiledTransformString;
				bool result = false;
				XmlConvert.TryToBoolean(enableMemberAccessForXslCompiledTransformString, out result);
				return result;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00015D6C File Offset: 0x00014D6C
		internal static bool EnableMemberAccessForXslCompiledTransform
		{
			get
			{
				XsltConfigSection xsltConfigSection = ConfigurationManager.GetSection(XmlConfigurationString.XsltSectionPath) as XsltConfigSection;
				return xsltConfigSection != null && xsltConfigSection._EnableMemberAccessForXslCompiledTransform;
			}
		}
	}
}
