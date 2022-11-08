using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Xml.XmlConfiguration
{
	// Token: 0x02000076 RID: 118
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class XmlReaderSection : ConfigurationSection
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00015B55 File Offset: 0x00014B55
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x00015B67 File Offset: 0x00014B67
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00015B78 File Offset: 0x00014B78
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00015B98 File Offset: 0x00014B98
		internal static bool ProhibitDefaultUrlResolver
		{
			get
			{
				XmlReaderSection xmlReaderSection = ConfigurationManager.GetSection(XmlConfigurationString.XmlReaderSectionPath) as XmlReaderSection;
				return xmlReaderSection != null && xmlReaderSection._ProhibitDefaultResolver;
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00015BC0 File Offset: 0x00014BC0
		internal static XmlResolver CreateDefaultResolver()
		{
			if (XmlReaderSection.ProhibitDefaultUrlResolver)
			{
				return null;
			}
			return new XmlUrlResolver();
		}
	}
}
