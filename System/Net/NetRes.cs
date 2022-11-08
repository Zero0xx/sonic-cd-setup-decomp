using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020004F2 RID: 1266
	internal class NetRes
	{
		// Token: 0x06002796 RID: 10134 RVA: 0x000A2F2E File Offset: 0x000A1F2E
		private NetRes()
		{
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000A2F38 File Offset: 0x000A1F38
		public static string GetWebStatusString(string Res, WebExceptionStatus Status)
		{
			string @string = SR.GetString(WebExceptionMapping.GetWebStatusString(Status));
			string string2 = SR.GetString(Res);
			return string.Format(CultureInfo.CurrentCulture, string2, new object[]
			{
				@string
			});
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000A2F6F File Offset: 0x000A1F6F
		public static string GetWebStatusString(WebExceptionStatus Status)
		{
			return SR.GetString(WebExceptionMapping.GetWebStatusString(Status));
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000A2F7C File Offset: 0x000A1F7C
		public static string GetWebStatusCodeString(HttpStatusCode statusCode, string statusDescription)
		{
			string str = "(";
			int num = (int)statusCode;
			string text = str + num.ToString(NumberFormatInfo.InvariantInfo) + ")";
			string text2 = null;
			try
			{
				text2 = SR.GetString("net_httpstatuscode_" + statusCode.ToString(), null);
			}
			catch
			{
			}
			if (text2 != null && text2.Length > 0)
			{
				text = text + " " + text2;
			}
			else if (statusDescription != null && statusDescription.Length > 0)
			{
				text = text + " " + statusDescription;
			}
			return text;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000A3010 File Offset: 0x000A2010
		public static string GetWebStatusCodeString(FtpStatusCode statusCode, string statusDescription)
		{
			string str = "(";
			int num = (int)statusCode;
			string text = str + num.ToString(NumberFormatInfo.InvariantInfo) + ")";
			string text2 = null;
			try
			{
				text2 = SR.GetString("net_ftpstatuscode_" + statusCode.ToString(), null);
			}
			catch
			{
			}
			if (text2 != null && text2.Length > 0)
			{
				text = text + " " + text2;
			}
			else if (statusDescription != null && statusDescription.Length > 0)
			{
				text = text + " " + statusDescription;
			}
			return text;
		}
	}
}
