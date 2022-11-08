using System;
using System.Collections;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000393 RID: 915
	[Serializable]
	public class CookieContainer
	{
		// Token: 0x06001C8B RID: 7307 RVA: 0x0006C2B0 File Offset: 0x0006B2B0
		public CookieContainer()
		{
			string domainName = IPGlobalProperties.InternalGetIPGlobalProperties().DomainName;
			if (domainName != null && domainName.Length > 1)
			{
				this.m_fqdnMyDomain = '.' + domainName;
			}
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x0006C321 File Offset: 0x0006B321
		public CookieContainer(int capacity) : this()
		{
			if (capacity <= 0)
			{
				throw new ArgumentException(SR.GetString("net_toosmall"), "Capacity");
			}
			this.m_maxCookies = capacity;
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0006C34C File Offset: 0x0006B34C
		public CookieContainer(int capacity, int perDomainCapacity, int maxCookieSize) : this(capacity)
		{
			if (perDomainCapacity != 2147483647 && (perDomainCapacity <= 0 || perDomainCapacity > capacity))
			{
				throw new ArgumentOutOfRangeException("perDomainCapacity", SR.GetString("net_cookie_capacity_range", new object[]
				{
					"PerDomainCapacity",
					0,
					capacity
				}));
			}
			this.m_maxCookiesPerDomain = perDomainCapacity;
			if (maxCookieSize <= 0)
			{
				throw new ArgumentException(SR.GetString("net_toosmall"), "MaxCookieSize");
			}
			this.m_maxCookieSize = maxCookieSize;
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001C8E RID: 7310 RVA: 0x0006C3CE File Offset: 0x0006B3CE
		// (set) Token: 0x06001C8F RID: 7311 RVA: 0x0006C3D8 File Offset: 0x0006B3D8
		public int Capacity
		{
			get
			{
				return this.m_maxCookies;
			}
			set
			{
				if (value <= 0 || (value < this.m_maxCookiesPerDomain && this.m_maxCookiesPerDomain != 2147483647))
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("net_cookie_capacity_range", new object[]
					{
						"Capacity",
						0,
						this.m_maxCookiesPerDomain
					}));
				}
				if (value < this.m_maxCookies)
				{
					this.m_maxCookies = value;
					this.AgeCookies(null);
				}
				this.m_maxCookies = value;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001C90 RID: 7312 RVA: 0x0006C45A File Offset: 0x0006B45A
		public int Count
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x0006C462 File Offset: 0x0006B462
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x0006C46A File Offset: 0x0006B46A
		public int MaxCookieSize
		{
			get
			{
				return this.m_maxCookieSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_maxCookieSize = value;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x0006C482 File Offset: 0x0006B482
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x0006C48C File Offset: 0x0006B48C
		public int PerDomainCapacity
		{
			get
			{
				return this.m_maxCookiesPerDomain;
			}
			set
			{
				if (value <= 0 || (value > this.m_maxCookies && value != 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value < this.m_maxCookiesPerDomain)
				{
					this.m_maxCookiesPerDomain = value;
					this.AgeCookies(null);
				}
				this.m_maxCookiesPerDomain = value;
			}
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x0006C4D8 File Offset: 0x0006B4D8
		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (cookie.Domain.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall"), "cookie.Domain");
			}
			Cookie cookie2 = new Cookie(cookie.Name, cookie.Value);
			cookie2.Version = cookie.Version;
			string text = (cookie.Secure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp) + Uri.SchemeDelimiter;
			if (cookie.Domain[0] == '.')
			{
				text += "0";
				cookie2.Domain = cookie.Domain;
			}
			text += cookie.Domain;
			if (cookie.PortList != null)
			{
				cookie2.Port = cookie.Port;
				text = text + ":" + cookie.PortList[0];
			}
			cookie2.Path = ((cookie.Path.Length == 0) ? "/" : cookie.Path);
			text += cookie.Path;
			Uri uri;
			if (!Uri.TryCreate(text, UriKind.Absolute, out uri))
			{
				throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
				{
					"Domain",
					cookie.Domain
				}));
			}
			cookie2.VerifySetDefaults(CookieVariant.Unknown, uri, this.IsLocal(uri.Host), this.m_fqdnMyDomain, true, true);
			this.Add(cookie2, true);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x0006C63C File Offset: 0x0006B63C
		private void AddRemoveDomain(string key, PathList value)
		{
			lock (this)
			{
				if (value == null)
				{
					this.m_domainTable.Remove(key);
				}
				else
				{
					this.m_domainTable[key] = value;
				}
			}
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x0006C688 File Offset: 0x0006B688
		internal void Add(Cookie cookie, bool throwOnError)
		{
			if (cookie.Value.Length <= this.m_maxCookieSize)
			{
				try
				{
					PathList pathList = (PathList)this.m_domainTable[cookie.DomainKey];
					if (pathList == null)
					{
						pathList = new PathList();
						this.AddRemoveDomain(cookie.DomainKey, pathList);
					}
					int cookiesCount = pathList.GetCookiesCount();
					CookieCollection cookieCollection = (CookieCollection)pathList[cookie.Path];
					if (cookieCollection == null)
					{
						cookieCollection = new CookieCollection();
						pathList[cookie.Path] = cookieCollection;
					}
					if (cookie.Expired)
					{
						lock (cookieCollection)
						{
							int num = cookieCollection.IndexOf(cookie);
							if (num != -1)
							{
								cookieCollection.RemoveAt(num);
								this.m_count--;
							}
							goto IL_142;
						}
					}
					if (cookiesCount < this.m_maxCookiesPerDomain || this.AgeCookies(cookie.DomainKey))
					{
						if (this.m_count < this.m_maxCookies || this.AgeCookies(null))
						{
							lock (cookieCollection)
							{
								this.m_count += cookieCollection.InternalAdd(cookie, true);
							}
						}
					}
					IL_142:;
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					if (throwOnError)
					{
						throw new CookieException(SR.GetString("net_container_add_cookie"), ex);
					}
				}
				catch
				{
					if (throwOnError)
					{
						throw new CookieException(SR.GetString("net_container_add_cookie"), new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
				return;
			}
			if (throwOnError)
			{
				throw new CookieException(SR.GetString("net_cookie_size", new object[]
				{
					cookie.ToString(),
					this.m_maxCookieSize
				}));
			}
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x0006C868 File Offset: 0x0006B868
		private bool AgeCookies(string domain)
		{
			if (this.m_maxCookies == 0 || this.m_maxCookiesPerDomain == 0)
			{
				this.m_domainTable = new Hashtable();
				this.m_count = 0;
				return false;
			}
			int num = 0;
			DateTime dateTime = DateTime.MaxValue;
			CookieCollection cookieCollection = null;
			int num2 = 0;
			int num3 = 0;
			float num4 = 1f;
			if (this.m_count > this.m_maxCookies)
			{
				num4 = (float)this.m_maxCookies / (float)this.m_count;
			}
			foreach (object obj in this.m_domainTable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				PathList pathList;
				if (domain == null)
				{
					string text = (string)dictionaryEntry.Key;
					pathList = (PathList)dictionaryEntry.Value;
				}
				else
				{
					pathList = (PathList)this.m_domainTable[domain];
				}
				num2 = 0;
				foreach (object obj2 in pathList.Values)
				{
					CookieCollection cookieCollection2 = (CookieCollection)obj2;
					num3 = this.ExpireCollection(cookieCollection2);
					num += num3;
					this.m_count -= num3;
					num2 += cookieCollection2.Count;
					DateTime dateTime2;
					if (cookieCollection2.Count > 0 && (dateTime2 = cookieCollection2.TimeStamp(CookieCollection.Stamp.Check)) < dateTime)
					{
						cookieCollection = cookieCollection2;
						dateTime = dateTime2;
					}
				}
				int num5 = Math.Min((int)((float)num2 * num4), Math.Min(this.m_maxCookiesPerDomain, this.m_maxCookies) - 1);
				if (num2 > num5)
				{
					Array array = Array.CreateInstance(typeof(CookieCollection), pathList.Count);
					Array array2 = Array.CreateInstance(typeof(DateTime), pathList.Count);
					foreach (object obj3 in pathList.Values)
					{
						CookieCollection cookieCollection3 = (CookieCollection)obj3;
						array2.SetValue(cookieCollection3.TimeStamp(CookieCollection.Stamp.Check), num3);
						array.SetValue(cookieCollection3, num3);
						num3++;
					}
					Array.Sort(array2, array);
					num3 = 0;
					for (int i = 0; i < pathList.Count; i++)
					{
						CookieCollection cookieCollection4 = (CookieCollection)array.GetValue(i);
						lock (cookieCollection4)
						{
							while (num2 > num5 && cookieCollection4.Count > 0)
							{
								cookieCollection4.RemoveAt(0);
								num2--;
								this.m_count--;
								num++;
							}
						}
						if (num2 <= num5)
						{
							break;
						}
					}
					if (num2 > num5 && domain != null)
					{
						return false;
					}
				}
				if (domain != null)
				{
					return true;
				}
			}
			if (num != 0)
			{
				return true;
			}
			if (dateTime == DateTime.MaxValue)
			{
				return false;
			}
			lock (cookieCollection)
			{
				while (this.m_count >= this.m_maxCookies && cookieCollection.Count > 0)
				{
					cookieCollection.RemoveAt(0);
					this.m_count--;
				}
			}
			return true;
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x0006CC04 File Offset: 0x0006BC04
		private int ExpireCollection(CookieCollection cc)
		{
			int count = cc.Count;
			int i = count - 1;
			DateTime now = DateTime.Now;
			lock (cc)
			{
				while (i >= 0)
				{
					Cookie cookie = cc[i];
					if (cookie.Expires <= now && cookie.Expires != DateTime.MinValue)
					{
						cc.RemoveAt(i);
					}
					i--;
				}
			}
			return count - cc.Count;
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x0006CC88 File Offset: 0x0006BC88
		public void Add(CookieCollection cookies)
		{
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				this.Add(cookie);
			}
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x0006CCEC File Offset: 0x0006BCEC
		internal bool IsLocal(string host)
		{
			int num = host.IndexOf('.');
			if (num == -1)
			{
				return true;
			}
			if (host == "127.0.0.1")
			{
				return true;
			}
			if (string.Compare(this.m_fqdnMyDomain, 0, host, num, this.m_fqdnMyDomain.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			string[] array = host.Split(new char[]
			{
				'.'
			});
			if (array != null && array.Length == 4 && array[0] == "127")
			{
				int i = 1;
				while (i < 4)
				{
					switch (array[i].Length)
					{
					case 1:
						break;
					case 2:
						goto IL_B0;
					case 3:
						if (array[i][2] >= '0' && array[i][2] <= '9')
						{
							goto IL_B0;
						}
						goto IL_EC;
					default:
						goto IL_EC;
					}
					IL_CA:
					if (array[i][0] >= '0' && array[i][0] <= '9')
					{
						i++;
						continue;
					}
					break;
					IL_B0:
					if (array[i][1] >= '0' && array[i][1] <= '9')
					{
						goto IL_CA;
					}
					break;
				}
				IL_EC:
				if (i == 4)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x0006CDEC File Offset: 0x0006BDEC
		public void Add(Uri uri, Cookie cookie)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			cookie.VerifySetDefaults(CookieVariant.Unknown, uri, this.IsLocal(uri.Host), this.m_fqdnMyDomain, true, true);
			this.Add(cookie, true);
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x0006CE40 File Offset: 0x0006BE40
		public void Add(Uri uri, CookieCollection cookies)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			bool isLocalDomain = this.IsLocal(uri.Host);
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				cookie.VerifySetDefaults(CookieVariant.Unknown, uri, isLocalDomain, this.m_fqdnMyDomain, true, true);
				this.Add(cookie, true);
			}
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0006CED8 File Offset: 0x0006BED8
		internal CookieCollection CookieCutter(Uri uri, string headerName, string setCookieHeader, bool isThrow)
		{
			CookieCollection cookieCollection = new CookieCollection();
			CookieVariant variant = CookieVariant.Unknown;
			if (headerName == null)
			{
				variant = CookieVariant.Rfc2109;
			}
			else
			{
				for (int i = 0; i < CookieContainer.HeaderInfo.Length; i++)
				{
					if (string.Compare(headerName, CookieContainer.HeaderInfo[i].Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						variant = CookieContainer.HeaderInfo[i].Variant;
					}
				}
			}
			bool isLocalDomain = this.IsLocal(uri.Host);
			try
			{
				CookieParser cookieParser = new CookieParser(setCookieHeader);
				for (;;)
				{
					Cookie cookie = cookieParser.Get();
					if (cookie == null)
					{
						goto IL_B0;
					}
					if (ValidationHelper.IsBlankString(cookie.Name))
					{
						if (isThrow)
						{
							break;
						}
					}
					else if (cookie.VerifySetDefaults(variant, uri, isLocalDomain, this.m_fqdnMyDomain, true, isThrow))
					{
						cookieCollection.InternalAdd(cookie, true);
					}
				}
				throw new CookieException(SR.GetString("net_cookie_format"));
				IL_B0:;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_parse_header", new object[]
					{
						uri.AbsoluteUri
					}), ex);
				}
			}
			catch
			{
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_parse_header", new object[]
					{
						uri.AbsoluteUri
					}), new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
			foreach (object obj in cookieCollection)
			{
				Cookie cookie2 = (Cookie)obj;
				this.Add(cookie2, isThrow);
			}
			return cookieCollection;
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x0006D08C File Offset: 0x0006C08C
		public CookieCollection GetCookies(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			return this.InternalGetCookies(uri);
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0006D0AC File Offset: 0x0006C0AC
		internal CookieCollection InternalGetCookies(Uri uri)
		{
			bool isSecure = uri.Scheme == Uri.UriSchemeHttps;
			int port = uri.Port;
			CookieCollection cookieCollection = new CookieCollection();
			ArrayList arrayList = new ArrayList();
			int num = 0;
			string host = uri.Host;
			int num2 = host.IndexOf('.');
			if (num2 == -1)
			{
				arrayList.Add(host);
				if (this.m_fqdnMyDomain != null && this.m_fqdnMyDomain.Length != 0)
				{
					arrayList.Add(host + this.m_fqdnMyDomain);
					arrayList.Add(this.m_fqdnMyDomain);
					num = 3;
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				arrayList.Add(host);
				arrayList.Add(host.Substring(num2));
				num = 2;
				if (host.Length > 2)
				{
					int num3 = host.LastIndexOf('.', host.Length - 2);
					if (num3 > 0)
					{
						num3 = host.LastIndexOf('.', num3 - 1);
					}
					if (num3 != -1)
					{
						while (num2 < num3 && (num2 = host.IndexOf('.', num2 + 1)) != -1)
						{
							arrayList.Add(host.Substring(num2));
						}
					}
				}
			}
			foreach (object obj in arrayList)
			{
				string key = (string)obj;
				bool flag = false;
				bool flag2 = false;
				PathList pathList = (PathList)this.m_domainTable[key];
				num--;
				if (pathList != null)
				{
					foreach (object obj2 in pathList)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
						string text = (string)dictionaryEntry.Key;
						if (uri.AbsolutePath.StartsWith(CookieParser.CheckQuoted(text)))
						{
							flag = true;
							CookieCollection cookieCollection2 = (CookieCollection)dictionaryEntry.Value;
							cookieCollection2.TimeStamp(CookieCollection.Stamp.Set);
							this.MergeUpdateCollections(cookieCollection, cookieCollection2, port, isSecure, num < 0);
							if (text == "/")
							{
								flag2 = true;
							}
						}
						else if (flag)
						{
							break;
						}
					}
					if (!flag2)
					{
						CookieCollection cookieCollection3 = (CookieCollection)pathList["/"];
						if (cookieCollection3 != null)
						{
							cookieCollection3.TimeStamp(CookieCollection.Stamp.Set);
							this.MergeUpdateCollections(cookieCollection, cookieCollection3, port, isSecure, num < 0);
						}
					}
					if (pathList.Count == 0)
					{
						this.AddRemoveDomain(key, null);
					}
				}
			}
			return cookieCollection;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0006D340 File Offset: 0x0006C340
		private void MergeUpdateCollections(CookieCollection destination, CookieCollection source, int port, bool isSecure, bool isPlainOnly)
		{
			lock (source)
			{
				for (int i = 0; i < source.Count; i++)
				{
					bool flag = false;
					Cookie cookie = source[i];
					if (cookie.Expired)
					{
						source.RemoveAt(i);
						this.m_count--;
						i--;
					}
					else
					{
						if (!isPlainOnly || cookie.Variant == CookieVariant.Plain)
						{
							if (cookie.PortList != null)
							{
								foreach (int num in cookie.PortList)
								{
									if (num == port)
									{
										flag = true;
										break;
									}
								}
							}
							else
							{
								flag = true;
							}
						}
						if (cookie.Secure && !isSecure)
						{
							flag = false;
						}
						if (flag)
						{
							destination.InternalAdd(cookie, false);
						}
					}
				}
			}
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0006D410 File Offset: 0x0006C410
		public string GetCookieHeader(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			string text;
			return this.GetCookieHeader(uri, out text);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0006D43C File Offset: 0x0006C43C
		internal string GetCookieHeader(Uri uri, out string optCookie2)
		{
			CookieCollection cookieCollection = this.InternalGetCookies(uri);
			string text = string.Empty;
			string str = string.Empty;
			foreach (object obj in cookieCollection)
			{
				Cookie cookie = (Cookie)obj;
				text = text + str + cookie.ToString();
				str = "; ";
			}
			optCookie2 = (cookieCollection.IsOtherVersionSeen ? ("$Version=" + 1.ToString(NumberFormatInfo.InvariantInfo)) : string.Empty);
			return text;
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0006D4E4 File Offset: 0x0006C4E4
		public void SetCookies(Uri uri, string cookieHeader)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookieHeader == null)
			{
				throw new ArgumentNullException("cookieHeader");
			}
			this.CookieCutter(uri, null, cookieHeader, true);
		}

		// Token: 0x04001D29 RID: 7465
		public const int DefaultCookieLimit = 300;

		// Token: 0x04001D2A RID: 7466
		public const int DefaultPerDomainCookieLimit = 20;

		// Token: 0x04001D2B RID: 7467
		public const int DefaultCookieLengthLimit = 4096;

		// Token: 0x04001D2C RID: 7468
		private static readonly HeaderVariantInfo[] HeaderInfo = new HeaderVariantInfo[]
		{
			new HeaderVariantInfo("Set-Cookie", CookieVariant.Rfc2109),
			new HeaderVariantInfo("Set-Cookie2", CookieVariant.Rfc2965)
		};

		// Token: 0x04001D2D RID: 7469
		private Hashtable m_domainTable = new Hashtable();

		// Token: 0x04001D2E RID: 7470
		private int m_maxCookieSize = 4096;

		// Token: 0x04001D2F RID: 7471
		private int m_maxCookies = 300;

		// Token: 0x04001D30 RID: 7472
		private int m_maxCookiesPerDomain = 20;

		// Token: 0x04001D31 RID: 7473
		private int m_count;

		// Token: 0x04001D32 RID: 7474
		private string m_fqdnMyDomain = string.Empty;
	}
}
