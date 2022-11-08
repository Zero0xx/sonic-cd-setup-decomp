using System;

namespace System.Net
{
	// Token: 0x0200049F RID: 1183
	internal static class WebExceptionMapping
	{
		// Token: 0x06002410 RID: 9232 RVA: 0x0008D164 File Offset: 0x0008C164
		internal static string GetWebStatusString(WebExceptionStatus status)
		{
			if (status >= (WebExceptionStatus)WebExceptionMapping.s_Mapping.Length || status < WebExceptionStatus.Success)
			{
				throw new InternalException();
			}
			string text = WebExceptionMapping.s_Mapping[(int)status];
			if (text == null)
			{
				text = "net_webstatus_" + status.ToString();
				WebExceptionMapping.s_Mapping[(int)status] = text;
			}
			return text;
		}

		// Token: 0x0400247F RID: 9343
		private static readonly string[] s_Mapping = new string[21];
	}
}
