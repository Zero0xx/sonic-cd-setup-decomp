using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D5 RID: 2005
	internal static class BinaryUtil
	{
		// Token: 0x06004725 RID: 18213 RVA: 0x000F3A0D File Offset: 0x000F2A0D
		[Conditional("_LOGGING")]
		public static void NVTraceI(string name, string value)
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x000F3A1A File Offset: 0x000F2A1A
		[Conditional("_LOGGING")]
		public static void NVTraceI(string name, object value)
		{
			BCLDebug.CheckEnabled("BINARY");
		}
	}
}
