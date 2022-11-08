using System;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x02000646 RID: 1606
	internal static class CommonConfigurationStrings
	{
		// Token: 0x060031BE RID: 12734 RVA: 0x000D4964 File Offset: 0x000D3964
		private static string GetSectionPath(string sectionName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				sectionName
			});
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x000D498C File Offset: 0x000D398C
		private static string GetSectionPath(string sectionName, string subSectionName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[]
			{
				sectionName,
				subSectionName
			});
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x060031C0 RID: 12736 RVA: 0x000D49B8 File Offset: 0x000D39B8
		internal static string UriSectionPath
		{
			get
			{
				return CommonConfigurationStrings.GetSectionPath("uri");
			}
		}

		// Token: 0x04002EDA RID: 11994
		internal const string UriSectionName = "uri";

		// Token: 0x04002EDB RID: 11995
		internal const string IriParsing = "iriParsing";

		// Token: 0x04002EDC RID: 11996
		internal const string Idn = "idn";

		// Token: 0x04002EDD RID: 11997
		internal const string Enabled = "enabled";
	}
}
