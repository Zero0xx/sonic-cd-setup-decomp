using System;

namespace System.Runtime.InteropServices.TCEAdapterGen
{
	// Token: 0x020008C6 RID: 2246
	internal static class NameSpaceExtractor
	{
		// Token: 0x060051CC RID: 20940 RVA: 0x00126348 File Offset: 0x00125348
		public static string ExtractNameSpace(string FullyQualifiedTypeName)
		{
			int num = FullyQualifiedTypeName.LastIndexOf(NameSpaceExtractor.NameSpaceSeperator);
			if (num == -1)
			{
				return "";
			}
			return FullyQualifiedTypeName.Substring(0, num);
		}

		// Token: 0x04002A35 RID: 10805
		private static char NameSpaceSeperator = '.';
	}
}
