using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x02000419 RID: 1049
	internal class HttpProtocolUtils
	{
		// Token: 0x060020D2 RID: 8402 RVA: 0x00081165 File Offset: 0x00080165
		private HttpProtocolUtils()
		{
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00081170 File Offset: 0x00080170
		internal static DateTime string2date(string S)
		{
			DateTime result;
			if (HttpDateParse.ParseHttpDate(S, out result))
			{
				return result;
			}
			throw new ProtocolViolationException(SR.GetString("net_baddate"));
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00081198 File Offset: 0x00080198
		internal static string date2string(DateTime D)
		{
			DateTimeFormatInfo provider = new DateTimeFormatInfo();
			return D.ToUniversalTime().ToString("R", provider);
		}
	}
}
