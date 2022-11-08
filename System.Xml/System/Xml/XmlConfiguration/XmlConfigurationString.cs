using System;
using System.Globalization;

namespace System.Xml.XmlConfiguration
{
	// Token: 0x02000075 RID: 117
	internal static class XmlConfigurationString
	{
		// Token: 0x040005FD RID: 1533
		internal const string XmlReaderSectionName = "xmlReader";

		// Token: 0x040005FE RID: 1534
		internal const string XmlTextReaderSectionName = "xmlTextReader";

		// Token: 0x040005FF RID: 1535
		internal const string XsltSectionName = "xslt";

		// Token: 0x04000600 RID: 1536
		internal const string ProhibitDefaultResolverName = "prohibitDefaultResolver";

		// Token: 0x04000601 RID: 1537
		internal const string LimitCharactersFromEntitiesName = "limitCharactersFromEntities";

		// Token: 0x04000602 RID: 1538
		internal const string LimitXPathComplexityName = "limitXPathComplexity";

		// Token: 0x04000603 RID: 1539
		internal const string EnableMemberAccessForXslCompiledTransformName = "enableMemberAccessForXslCompiledTransform";

		// Token: 0x04000604 RID: 1540
		internal const string XmlConfigurationSectionName = "system.xml";

		// Token: 0x04000605 RID: 1541
		internal static string XmlReaderSectionPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[]
		{
			"system.xml",
			"xmlReader"
		});

		// Token: 0x04000606 RID: 1542
		internal static string XmlTextReaderSectionPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[]
		{
			"system.xml",
			"xmlTextReader"
		});

		// Token: 0x04000607 RID: 1543
		internal static string XsltSectionPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[]
		{
			"system.xml",
			"xslt"
		});
	}
}
