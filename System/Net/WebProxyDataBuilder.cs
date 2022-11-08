using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net
{
	// Token: 0x02000508 RID: 1288
	internal abstract class WebProxyDataBuilder
	{
		// Token: 0x060027F5 RID: 10229 RVA: 0x000A4CD8 File Offset: 0x000A3CD8
		public WebProxyData Build()
		{
			this.m_Result = new WebProxyData();
			this.BuildInternal();
			return this.m_Result;
		}

		// Token: 0x060027F6 RID: 10230
		protected abstract void BuildInternal();

		// Token: 0x060027F7 RID: 10231 RVA: 0x000A4CF4 File Offset: 0x000A3CF4
		protected void SetProxyAndBypassList(string addressString, string bypassListString)
		{
			Uri uri = null;
			Hashtable hashtable = null;
			if (addressString != null)
			{
				uri = WebProxyDataBuilder.ParseProxyUri(addressString, true);
				if (uri == null)
				{
					hashtable = WebProxyDataBuilder.ParseProtocolProxies(addressString);
				}
				if ((uri != null || hashtable != null) && bypassListString != null)
				{
					bool bypassOnLocal = false;
					this.m_Result.bypassList = WebProxyDataBuilder.ParseBypassList(bypassListString, out bypassOnLocal);
					this.m_Result.bypassOnLocal = bypassOnLocal;
				}
			}
			if (hashtable != null)
			{
				uri = (hashtable["http"] as Uri);
			}
			this.m_Result.proxyAddress = uri;
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x000A4D70 File Offset: 0x000A3D70
		protected void SetAutoProxyUrl(string autoConfigUrl)
		{
			if (!string.IsNullOrEmpty(autoConfigUrl))
			{
				Uri scriptLocation = null;
				if (Uri.TryCreate(autoConfigUrl, UriKind.Absolute, out scriptLocation))
				{
					this.m_Result.scriptLocation = scriptLocation;
				}
			}
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000A4D9E File Offset: 0x000A3D9E
		protected void SetAutoDetectSettings(bool value)
		{
			this.m_Result.automaticallyDetectSettings = value;
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000A4DAC File Offset: 0x000A3DAC
		private static Uri ParseProxyUri(string proxyString, bool validate)
		{
			if (validate)
			{
				if (proxyString.Length == 0)
				{
					return null;
				}
				if (proxyString.IndexOf('=') != -1)
				{
					return null;
				}
			}
			if (proxyString.IndexOf("://") == -1)
			{
				proxyString = "http://" + proxyString;
			}
			try
			{
				return new Uri(proxyString);
			}
			catch (UriFormatException ex)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, ex.Message);
				}
			}
			return null;
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000A4E24 File Offset: 0x000A3E24
		private static Hashtable ParseProtocolProxies(string proxyListString)
		{
			if (proxyListString.Length == 0)
			{
				return null;
			}
			string[] array = proxyListString.Split(WebProxyDataBuilder.s_AddressListSplitChars);
			bool flag = true;
			string key = null;
			Hashtable hashtable = new Hashtable(CaseInsensitiveAscii.StaticInstance);
			foreach (string text in array)
			{
				string text2 = text.Trim().ToLower(CultureInfo.InvariantCulture);
				if (flag)
				{
					key = text2;
				}
				else
				{
					hashtable[key] = WebProxyDataBuilder.ParseProxyUri(text2, false);
				}
				flag = !flag;
			}
			if (hashtable.Count == 0)
			{
				return null;
			}
			return hashtable;
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000A4EB0 File Offset: 0x000A3EB0
		private static string BypassStringEscape(string rawString)
		{
			Regex regex = new Regex("^(?<scheme>.*://)?(?<host>[^:]*)(?<port>:[0-9]{1,5})?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
			Match match = regex.Match(rawString);
			string text;
			string text2;
			string text3;
			if (match.Success)
			{
				text = match.Groups["scheme"].Value;
				text2 = match.Groups["host"].Value;
				text3 = match.Groups["port"].Value;
			}
			else
			{
				text = string.Empty;
				text2 = rawString;
				text3 = string.Empty;
			}
			text = WebProxyDataBuilder.ConvertRegexReservedChars(text);
			text2 = WebProxyDataBuilder.ConvertRegexReservedChars(text2);
			text3 = WebProxyDataBuilder.ConvertRegexReservedChars(text3);
			if (text == string.Empty)
			{
				text = "(?:.*://)?";
			}
			if (text3 == string.Empty)
			{
				text3 = "(?::[0-9]{1,5})?";
			}
			return string.Concat(new string[]
			{
				"^",
				text,
				text2,
				text3,
				"$"
			});
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000A4FA4 File Offset: 0x000A3FA4
		private static string ConvertRegexReservedChars(string rawString)
		{
			if (rawString.Length == 0)
			{
				return rawString;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in rawString)
			{
				if ("#$()+.?[\\^{|".IndexOf(c) != -1)
				{
					stringBuilder.Append('\\');
				}
				else if (c == '*')
				{
					stringBuilder.Append('.');
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000A5014 File Offset: 0x000A4014
		private static ArrayList ParseBypassList(string bypassListString, out bool bypassOnLocal)
		{
			string[] array = bypassListString.Split(WebProxyDataBuilder.s_BypassListDelimiter);
			bypassOnLocal = false;
			if (array.Length == 0)
			{
				return null;
			}
			ArrayList arrayList = null;
			foreach (string text in array)
			{
				if (text != null)
				{
					string text2 = text.Trim();
					if (text2.Length > 0)
					{
						if (string.Compare(text2, "<local>", StringComparison.OrdinalIgnoreCase) == 0)
						{
							bypassOnLocal = true;
						}
						else
						{
							text2 = WebProxyDataBuilder.BypassStringEscape(text2);
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							if (!arrayList.Contains(text2))
							{
								arrayList.Add(text2);
							}
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x04002745 RID: 10053
		private const string regexReserved = "#$()+.?[\\^{|";

		// Token: 0x04002746 RID: 10054
		private static readonly char[] s_AddressListSplitChars = new char[]
		{
			';',
			'='
		};

		// Token: 0x04002747 RID: 10055
		private static readonly char[] s_BypassListDelimiter = new char[]
		{
			';'
		};

		// Token: 0x04002748 RID: 10056
		private WebProxyData m_Result;
	}
}
