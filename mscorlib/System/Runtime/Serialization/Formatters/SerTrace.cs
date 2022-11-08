using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007BA RID: 1978
	internal static class SerTrace
	{
		// Token: 0x0600467A RID: 18042 RVA: 0x000F07AD File Offset: 0x000EF7AD
		[Conditional("_LOGGING")]
		internal static void InfoLog(params object[] messages)
		{
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x000F07AF File Offset: 0x000EF7AF
		[Conditional("SER_LOGGING")]
		internal static void Log(params object[] messages)
		{
			if (!(messages[0] is string))
			{
				messages[0] = messages[0].GetType().Name + " ";
				return;
			}
			messages[0] = messages[0] + " ";
		}
	}
}
