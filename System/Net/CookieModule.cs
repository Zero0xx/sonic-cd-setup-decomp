using System;

namespace System.Net
{
	// Token: 0x020004D1 RID: 1233
	internal static class CookieModule
	{
		// Token: 0x0600265E RID: 9822 RVA: 0x0009C0CC File Offset: 0x0009B0CC
		internal static void OnSendingHeaders(HttpWebRequest httpWebRequest)
		{
			try
			{
				if (httpWebRequest.CookieContainer != null)
				{
					httpWebRequest.Headers.RemoveInternal("Cookie");
					string text;
					string cookieHeader = httpWebRequest.CookieContainer.GetCookieHeader(httpWebRequest.Address, out text);
					if (cookieHeader.Length > 0)
					{
						httpWebRequest.Headers["Cookie"] = cookieHeader;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0009C138 File Offset: 0x0009B138
		internal static void OnReceivedHeaders(HttpWebRequest httpWebRequest)
		{
			try
			{
				if (httpWebRequest.CookieContainer != null)
				{
					HttpWebResponse httpResponse = httpWebRequest._HttpResponse;
					if (httpResponse != null)
					{
						CookieCollection cookieCollection = null;
						try
						{
							string setCookie = httpResponse.Headers.SetCookie;
							if (setCookie != null && setCookie.Length > 0)
							{
								cookieCollection = httpWebRequest.CookieContainer.CookieCutter(httpResponse.ResponseUri, "Set-Cookie", setCookie, false);
							}
						}
						catch
						{
						}
						try
						{
							string setCookie2 = httpResponse.Headers.SetCookie2;
							if (setCookie2 != null && setCookie2.Length > 0)
							{
								CookieCollection cookieCollection2 = httpWebRequest.CookieContainer.CookieCutter(httpResponse.ResponseUri, "Set-Cookie2", setCookie2, false);
								if (cookieCollection != null && cookieCollection.Count != 0)
								{
									cookieCollection.Add(cookieCollection2);
								}
								else
								{
									cookieCollection = cookieCollection2;
								}
							}
						}
						catch
						{
						}
						if (cookieCollection != null)
						{
							httpResponse.Cookies = cookieCollection;
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
