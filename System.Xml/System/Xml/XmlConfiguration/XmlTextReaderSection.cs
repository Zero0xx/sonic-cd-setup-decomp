using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Xml.XmlConfiguration
{
	// Token: 0x02000077 RID: 119
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class XmlTextReaderSection : ConfigurationSection
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00015BD8 File Offset: 0x00014BD8
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x00015BEA File Offset: 0x00014BEA
		[ConfigurationProperty("limitCharactersFromEntities", DefaultValue = "true")]
		internal string LimitCharactersFromEntitiesString
		{
			get
			{
				return (string)base["limitCharactersFromEntities"];
			}
			set
			{
				base["limitCharactersFromEntities"] = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00015BF8 File Offset: 0x00014BF8
		private bool _LimitCharactersFromEntities
		{
			get
			{
				string limitCharactersFromEntitiesString = this.LimitCharactersFromEntitiesString;
				bool result = true;
				XmlConvert.TryToBoolean(limitCharactersFromEntitiesString, out result);
				return result;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00015C18 File Offset: 0x00014C18
		internal static bool LimitCharactersFromEntities
		{
			get
			{
				XmlTextReaderSection xmlTextReaderSection = ConfigurationManager.GetSection(XmlConfigurationString.XmlTextReaderSectionPath) as XmlTextReaderSection;
				return xmlTextReaderSection == null || xmlTextReaderSection._LimitCharactersFromEntities;
			}
		}
	}
}
