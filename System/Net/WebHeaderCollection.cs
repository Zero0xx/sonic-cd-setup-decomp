using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Net
{
	// Token: 0x020004A1 RID: 1185
	[ComVisible(true)]
	[Serializable]
	public class WebHeaderCollection : NameValueCollection, ISerializable
	{
		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x0008D1BF File Offset: 0x0008C1BF
		internal string ContentLength
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[1]);
				}
				return this.m_CommonHeaders[1];
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x0008D1DF File Offset: 0x0008C1DF
		internal string CacheControl
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[2]);
				}
				return this.m_CommonHeaders[2];
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x0008D1FF File Offset: 0x0008C1FF
		internal string ContentType
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[3]);
				}
				return this.m_CommonHeaders[3];
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x0008D21F File Offset: 0x0008C21F
		internal string Date
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[4]);
				}
				return this.m_CommonHeaders[4];
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x0008D23F File Offset: 0x0008C23F
		internal string Expires
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[5]);
				}
				return this.m_CommonHeaders[5];
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x0008D25F File Offset: 0x0008C25F
		internal string ETag
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[6]);
				}
				return this.m_CommonHeaders[6];
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x0008D27F File Offset: 0x0008C27F
		internal string LastModified
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[7]);
				}
				return this.m_CommonHeaders[7];
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x0008D29F File Offset: 0x0008C29F
		internal string Location
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[8]);
				}
				return this.m_CommonHeaders[8];
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x0008D2BF File Offset: 0x0008C2BF
		internal string ProxyAuthenticate
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[9]);
				}
				return this.m_CommonHeaders[9];
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x0008D2E1 File Offset: 0x0008C2E1
		internal string SetCookie2
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[11]);
				}
				return this.m_CommonHeaders[11];
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x0008D303 File Offset: 0x0008C303
		internal string SetCookie
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[12]);
				}
				return this.m_CommonHeaders[12];
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x0008D325 File Offset: 0x0008C325
		internal string Server
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[13]);
				}
				return this.m_CommonHeaders[13];
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0008D347 File Offset: 0x0008C347
		internal string Via
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[14]);
				}
				return this.m_CommonHeaders[14];
			}
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x0008D36C File Offset: 0x0008C36C
		private void NormalizeCommonHeaders()
		{
			if (this.m_CommonHeaders == null)
			{
				return;
			}
			for (int i = 0; i < this.m_CommonHeaders.Length; i++)
			{
				if (this.m_CommonHeaders[i] != null)
				{
					this.InnerCollection.Add(WebHeaderCollection.s_CommonHeaderNames[i], this.m_CommonHeaders[i]);
				}
			}
			this.m_CommonHeaders = null;
			this.m_NumCommonHeaders = 0;
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x0008D3C7 File Offset: 0x0008C3C7
		private NameValueCollection InnerCollection
		{
			get
			{
				if (this.m_InnerCollection == null)
				{
					this.m_InnerCollection = new NameValueCollection(16, CaseInsensitiveAscii.StaticInstance);
				}
				return this.m_InnerCollection;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x0008D3E9 File Offset: 0x0008C3E9
		private bool AllowHttpRequestHeader
		{
			get
			{
				if (this.m_Type == WebHeaderCollectionType.Unknown)
				{
					this.m_Type = WebHeaderCollectionType.WebRequest;
				}
				return this.m_Type == WebHeaderCollectionType.WebRequest || this.m_Type == WebHeaderCollectionType.HttpWebRequest || this.m_Type == WebHeaderCollectionType.HttpListenerRequest;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x0008D417 File Offset: 0x0008C417
		internal bool AllowHttpResponseHeader
		{
			get
			{
				if (this.m_Type == WebHeaderCollectionType.Unknown)
				{
					this.m_Type = WebHeaderCollectionType.WebResponse;
				}
				return this.m_Type == WebHeaderCollectionType.WebResponse || this.m_Type == WebHeaderCollectionType.HttpWebResponse || this.m_Type == WebHeaderCollectionType.HttpListenerResponse;
			}
		}

		// Token: 0x17000785 RID: 1925
		public string this[HttpRequestHeader header]
		{
			get
			{
				if (!this.AllowHttpRequestHeader)
				{
					throw new InvalidOperationException(SR.GetString("net_headers_req"));
				}
				return base[UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header)];
			}
			set
			{
				if (!this.AllowHttpRequestHeader)
				{
					throw new InvalidOperationException(SR.GetString("net_headers_req"));
				}
				base[UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header)] = value;
			}
		}

		// Token: 0x17000786 RID: 1926
		public string this[HttpResponseHeader header]
		{
			get
			{
				if (!this.AllowHttpResponseHeader)
				{
					throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
				}
				if (this.m_CommonHeaders != null)
				{
					if (header == HttpResponseHeader.ProxyAuthenticate)
					{
						return this.m_CommonHeaders[9];
					}
					if (header == HttpResponseHeader.WwwAuthenticate)
					{
						return this.m_CommonHeaders[15];
					}
				}
				return base[UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header)];
			}
			set
			{
				if (!this.AllowHttpResponseHeader)
				{
					throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
				}
				if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
					{
						ushort.MaxValue
					}));
				}
				base[UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header)] = value;
			}
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0008D560 File Offset: 0x0008C560
		public void Add(HttpRequestHeader header, string value)
		{
			if (!this.AllowHttpRequestHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_req"));
			}
			this.Add(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0008D588 File Offset: 0x0008C588
		public void Add(HttpResponseHeader header, string value)
		{
			if (!this.AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
			}
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
				{
					ushort.MaxValue
				}));
			}
			this.Add(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x0008D5F8 File Offset: 0x0008C5F8
		public void Set(HttpRequestHeader header, string value)
		{
			if (!this.AllowHttpRequestHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_req"));
			}
			this.Set(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x0008D620 File Offset: 0x0008C620
		public void Set(HttpResponseHeader header, string value)
		{
			if (!this.AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
			}
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
				{
					ushort.MaxValue
				}));
			}
			this.Set(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0008D690 File Offset: 0x0008C690
		internal void SetInternal(HttpResponseHeader header, string value)
		{
			if (!this.AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
			}
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
				{
					ushort.MaxValue
				}));
			}
			this.SetInternal(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x0008D700 File Offset: 0x0008C700
		public void Remove(HttpRequestHeader header)
		{
			if (!this.AllowHttpRequestHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_req"));
			}
			this.Remove(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header));
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0008D726 File Offset: 0x0008C726
		public void Remove(HttpResponseHeader header)
		{
			if (!this.AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
			}
			this.Remove(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header));
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0008D74C File Offset: 0x0008C74C
		protected void AddWithoutValidate(string headerName, string headerValue)
		{
			headerName = WebHeaderCollection.CheckBadChars(headerName, false);
			headerValue = WebHeaderCollection.CheckBadChars(headerValue, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && headerValue != null && headerValue.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
				{
					ushort.MaxValue
				}));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(headerName, headerValue);
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x0008D7C4 File Offset: 0x0008C7C4
		internal void SetAddVerified(string name, string value)
		{
			if (WebHeaderCollection.HInfo[name].AllowMultiValues)
			{
				this.NormalizeCommonHeaders();
				base.InvalidateCachedArrays();
				this.InnerCollection.Add(name, value);
				return;
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Set(name, value);
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0008D816 File Offset: 0x0008C816
		internal void AddInternal(string name, string value)
		{
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(name, value);
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x0008D831 File Offset: 0x0008C831
		internal void ChangeInternal(string name, string value)
		{
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Set(name, value);
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x0008D84C File Offset: 0x0008C84C
		internal void RemoveInternal(string name)
		{
			this.NormalizeCommonHeaders();
			if (this.m_InnerCollection != null)
			{
				base.InvalidateCachedArrays();
				this.m_InnerCollection.Remove(name);
			}
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0008D86E File Offset: 0x0008C86E
		internal void CheckUpdate(string name, string value)
		{
			value = WebHeaderCollection.CheckBadChars(value, true);
			this.ChangeInternal(name, value);
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0008D881 File Offset: 0x0008C881
		private void AddInternalNotCommon(string name, string value)
		{
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(name, value);
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0008D898 File Offset: 0x0008C898
		internal static string CheckBadChars(string name, bool isHeaderValue)
		{
			if (name != null && name.Length != 0)
			{
				if (isHeaderValue)
				{
					name = name.Trim(WebHeaderCollection.HttpTrimCharacters);
					int num = 0;
					for (int i = 0; i < name.Length; i++)
					{
						char c = 'ÿ' & name[i];
						switch (num)
						{
						case 0:
							if (c == '\r')
							{
								num = 1;
							}
							else if (c == '\n')
							{
								num = 2;
							}
							else if (c == '\u007f' || (c < ' ' && c != '\t'))
							{
								throw new ArgumentException(SR.GetString("net_WebHeaderInvalidControlChars"), "value");
							}
							break;
						case 1:
							if (c != '\n')
							{
								throw new ArgumentException(SR.GetString("net_WebHeaderInvalidCRLFChars"), "value");
							}
							num = 2;
							break;
						case 2:
							if (c != ' ' && c != '\t')
							{
								throw new ArgumentException(SR.GetString("net_WebHeaderInvalidCRLFChars"), "value");
							}
							num = 0;
							break;
						}
					}
					if (num != 0)
					{
						throw new ArgumentException(SR.GetString("net_WebHeaderInvalidCRLFChars"), "value");
					}
				}
				else
				{
					if (name.IndexOfAny(ValidationHelper.InvalidParamChars) != -1)
					{
						throw new ArgumentException(SR.GetString("net_WebHeaderInvalidHeaderChars"), "name");
					}
					if (WebHeaderCollection.ContainsNonAsciiChars(name))
					{
						throw new ArgumentException(SR.GetString("net_WebHeaderInvalidNonAsciiChars"), "name");
					}
				}
				return name;
			}
			if (!isHeaderValue)
			{
				throw (name == null) ? new ArgumentNullException("name") : new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"name"
				}), "name");
			}
			return string.Empty;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0008DA10 File Offset: 0x0008CA10
		internal static bool IsValidToken(string token)
		{
			return token.Length > 0 && token.IndexOfAny(ValidationHelper.InvalidParamChars) == -1 && !WebHeaderCollection.ContainsNonAsciiChars(token);
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0008DA34 File Offset: 0x0008CA34
		internal static bool ContainsNonAsciiChars(string token)
		{
			for (int i = 0; i < token.Length; i++)
			{
				if (token[i] < ' ' || token[i] > '~')
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0008DA6C File Offset: 0x0008CA6C
		internal void ThrowOnRestrictedHeader(string headerName)
		{
			if (this.m_Type == WebHeaderCollectionType.HttpWebRequest)
			{
				if (WebHeaderCollection.HInfo[headerName].IsRequestRestricted)
				{
					throw new ArgumentException((!object.Equals(headerName, "Host")) ? SR.GetString("net_headerrestrict") : SR.GetString("net_headerrestrict_resp", new object[]
					{
						"Host"
					}), "name");
				}
			}
			else if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && WebHeaderCollection.HInfo[headerName].IsResponseRestricted)
			{
				throw new ArgumentException(SR.GetString("net_headerrestrict_resp", new object[]
				{
					headerName
				}), "name");
			}
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0008DB10 File Offset: 0x0008CB10
		public override void Add(string name, string value)
		{
			name = WebHeaderCollection.CheckBadChars(name, false);
			this.ThrowOnRestrictedHeader(name);
			value = WebHeaderCollection.CheckBadChars(value, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
				{
					ushort.MaxValue
				}));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(name, value);
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0008DB90 File Offset: 0x0008CB90
		public void Add(string header)
		{
			if (ValidationHelper.IsBlankString(header))
			{
				throw new ArgumentNullException("header");
			}
			int num = header.IndexOf(':');
			if (num < 0)
			{
				throw new ArgumentException(SR.GetString("net_WebHeaderMissingColon"), "header");
			}
			string text = header.Substring(0, num);
			string text2 = header.Substring(num + 1);
			text = WebHeaderCollection.CheckBadChars(text, false);
			this.ThrowOnRestrictedHeader(text);
			text2 = WebHeaderCollection.CheckBadChars(text2, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && text2 != null && text2.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
				{
					ushort.MaxValue
				}));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(text, text2);
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0008DC54 File Offset: 0x0008CC54
		public override void Set(string name, string value)
		{
			if (ValidationHelper.IsBlankString(name))
			{
				throw new ArgumentNullException("name");
			}
			name = WebHeaderCollection.CheckBadChars(name, false);
			this.ThrowOnRestrictedHeader(name);
			value = WebHeaderCollection.CheckBadChars(value, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
				{
					ushort.MaxValue
				}));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Set(name, value);
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0008DCE4 File Offset: 0x0008CCE4
		internal void SetInternal(string name, string value)
		{
			if (ValidationHelper.IsBlankString(name))
			{
				throw new ArgumentNullException("name");
			}
			name = WebHeaderCollection.CheckBadChars(name, false);
			value = WebHeaderCollection.CheckBadChars(value, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[]
				{
					ushort.MaxValue
				}));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Set(name, value);
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0008DD70 File Offset: 0x0008CD70
		public override void Remove(string name)
		{
			if (ValidationHelper.IsBlankString(name))
			{
				throw new ArgumentNullException("name");
			}
			this.ThrowOnRestrictedHeader(name);
			name = WebHeaderCollection.CheckBadChars(name, false);
			this.NormalizeCommonHeaders();
			if (this.m_InnerCollection != null)
			{
				base.InvalidateCachedArrays();
				this.m_InnerCollection.Remove(name);
			}
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0008DDC0 File Offset: 0x0008CDC0
		public override string[] GetValues(string header)
		{
			this.NormalizeCommonHeaders();
			HeaderInfo headerInfo = WebHeaderCollection.HInfo[header];
			string[] values = this.InnerCollection.GetValues(header);
			if (headerInfo == null || values == null || !headerInfo.AllowMultiValues)
			{
				return values;
			}
			ArrayList arrayList = null;
			for (int i = 0; i < values.Length; i++)
			{
				string[] array = headerInfo.Parser(values[i]);
				if (arrayList == null)
				{
					if (array.Length > 1)
					{
						arrayList = new ArrayList(values);
						arrayList.RemoveRange(i, values.Length - i);
						arrayList.AddRange(array);
					}
				}
				else
				{
					arrayList.AddRange(array);
				}
			}
			if (arrayList != null)
			{
				string[] array2 = new string[arrayList.Count];
				arrayList.CopyTo(array2);
				return array2;
			}
			return values;
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0008DE6C File Offset: 0x0008CE6C
		public override string ToString()
		{
			return WebHeaderCollection.GetAsString(this, false, false);
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x0008DE83 File Offset: 0x0008CE83
		internal string ToString(bool forTrace)
		{
			return WebHeaderCollection.GetAsString(this, false, true);
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x0008DE90 File Offset: 0x0008CE90
		internal static string GetAsString(NameValueCollection cc, bool winInetCompat, bool forTrace)
		{
			if (cc == null || cc.Count == 0)
			{
				return "\r\n";
			}
			StringBuilder stringBuilder = new StringBuilder(30 * cc.Count);
			string text = cc[string.Empty];
			if (text != null)
			{
				stringBuilder.Append(text).Append("\r\n");
			}
			for (int i = 0; i < cc.Count; i++)
			{
				string key = cc.GetKey(i);
				string value = cc.Get(i);
				if (!ValidationHelper.IsBlankString(key))
				{
					stringBuilder.Append(key);
					if (winInetCompat)
					{
						stringBuilder.Append(':');
					}
					else
					{
						stringBuilder.Append(": ");
					}
					stringBuilder.Append(value).Append("\r\n");
				}
			}
			if (!forTrace)
			{
				stringBuilder.Append("\r\n");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0008DF54 File Offset: 0x0008CF54
		public byte[] ToByteArray()
		{
			string myString = this.ToString();
			return WebHeaderCollection.HeaderEncoding.GetBytes(myString);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0008DF70 File Offset: 0x0008CF70
		public static bool IsRestricted(string headerName)
		{
			return WebHeaderCollection.IsRestricted(headerName, false);
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0008DF79 File Offset: 0x0008CF79
		public static bool IsRestricted(string headerName, bool response)
		{
			if (!response)
			{
				return WebHeaderCollection.HInfo[WebHeaderCollection.CheckBadChars(headerName, false)].IsRequestRestricted;
			}
			return WebHeaderCollection.HInfo[WebHeaderCollection.CheckBadChars(headerName, false)].IsResponseRestricted;
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0008DFAB File Offset: 0x0008CFAB
		public WebHeaderCollection() : base(DBNull.Value)
		{
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0008DFB8 File Offset: 0x0008CFB8
		internal WebHeaderCollection(WebHeaderCollectionType type) : base(DBNull.Value)
		{
			this.m_Type = type;
			if (type == WebHeaderCollectionType.HttpWebResponse)
			{
				this.m_CommonHeaders = new string[WebHeaderCollection.s_CommonHeaderNames.Length - 1];
			}
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x0008DFE4 File Offset: 0x0008CFE4
		internal WebHeaderCollection(NameValueCollection cc) : base(DBNull.Value)
		{
			this.m_InnerCollection = new NameValueCollection(cc.Count + 2, CaseInsensitiveAscii.StaticInstance);
			int count = cc.Count;
			for (int i = 0; i < count; i++)
			{
				string key = cc.GetKey(i);
				string[] values = cc.GetValues(i);
				if (values != null)
				{
					for (int j = 0; j < values.Length; j++)
					{
						this.InnerCollection.Add(key, values[j]);
					}
				}
				else
				{
					this.InnerCollection.Add(key, null);
				}
			}
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x0008E06C File Offset: 0x0008D06C
		protected WebHeaderCollection(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(DBNull.Value)
		{
			int @int = serializationInfo.GetInt32("Count");
			this.m_InnerCollection = new NameValueCollection(@int + 2, CaseInsensitiveAscii.StaticInstance);
			for (int i = 0; i < @int; i++)
			{
				string @string = serializationInfo.GetString(i.ToString(NumberFormatInfo.InvariantInfo));
				string string2 = serializationInfo.GetString((i + @int).ToString(NumberFormatInfo.InvariantInfo));
				this.InnerCollection.Add(@string, string2);
			}
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0008E0E7 File Offset: 0x0008D0E7
		public override void OnDeserialization(object sender)
		{
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x0008E0EC File Offset: 0x0008D0EC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.NormalizeCommonHeaders();
			serializationInfo.AddValue("Count", this.Count);
			for (int i = 0; i < this.Count; i++)
			{
				serializationInfo.AddValue(i.ToString(NumberFormatInfo.InvariantInfo), this.GetKey(i));
				serializationInfo.AddValue((i + this.Count).ToString(NumberFormatInfo.InvariantInfo), this.Get(i));
			}
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0008E15C File Offset: 0x0008D15C
		internal unsafe DataParseStatus ParseHeaders(byte[] buffer, int size, ref int unparsed, ref int totalResponseHeadersLength, int maximumResponseHeadersLength, ref WebParseError parseError)
		{
			DataParseStatus result;
			try
			{
				fixed (byte* ptr = buffer)
				{
					if (buffer.Length < size)
					{
						result = DataParseStatus.NeedMoreData;
					}
					else
					{
						int num = -1;
						int num2 = -1;
						int num3 = -1;
						int i = unparsed;
						int num4 = totalResponseHeadersLength;
						WebParseErrorCode code = WebParseErrorCode.Generic;
						for (;;)
						{
							string text = string.Empty;
							string text2 = string.Empty;
							bool flag = false;
							string text3 = null;
							if (this.Count == 0)
							{
								while (i < size)
								{
									char c = (char)ptr[i];
									if (c != ' ' && c != '\t')
									{
										break;
									}
									i++;
									if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
									{
										goto Block_7;
									}
								}
								if (i == size)
								{
									goto Block_8;
								}
							}
							int num5 = i;
							while (i < size)
							{
								char c = (char)ptr[i];
								if (c != ':' && c != '\n')
								{
									if (c > ' ')
									{
										num = i;
									}
									i++;
									if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
									{
										goto Block_13;
									}
								}
								else
								{
									if (c != ':')
									{
										break;
									}
									i++;
									if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
									{
										goto Block_16;
									}
									break;
								}
							}
							if (i == size)
							{
								goto Block_17;
							}
							int num6;
							for (;;)
							{
								num6 = ((this.Count == 0 && num < 0) ? 1 : 0);
								char c;
								while (i < size && num6 < 2)
								{
									c = (char)ptr[i];
									if (c > ' ')
									{
										break;
									}
									if (c == '\n')
									{
										num6++;
										if (num6 == 1)
										{
											if (i + 1 == size)
											{
												goto Block_22;
											}
											flag = (ptr[i + 1] == 32 || ptr[i + 1] == 9);
										}
									}
									i++;
									if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
									{
										goto Block_25;
									}
								}
								if (num6 != 2 && (num6 != 1 || flag))
								{
									if (i == size)
									{
										goto Block_29;
									}
									num2 = i;
									while (i < size)
									{
										c = (char)ptr[i];
										if (c == '\n')
										{
											break;
										}
										if (c > ' ')
										{
											num3 = i;
										}
										i++;
										if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
										{
											goto Block_33;
										}
									}
									if (i == size)
									{
										goto Block_34;
									}
									num6 = 0;
									while (i < size && num6 < 2)
									{
										c = (char)ptr[i];
										if (c != '\r' && c != '\n')
										{
											break;
										}
										if (c == '\n')
										{
											num6++;
										}
										i++;
										if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
										{
											goto Block_38;
										}
									}
									if (i == size && num6 < 2)
									{
										goto Block_41;
									}
								}
								if (num2 >= 0 && num2 > num && num3 >= num2)
								{
									text2 = WebHeaderCollection.HeaderEncoding.GetString(ptr + num2, num3 - num2 + 1);
								}
								text3 = ((text3 == null) ? text2 : (text3 + " " + text2));
								if (i >= size || num6 != 1)
								{
									break;
								}
								c = (char)ptr[i];
								if (c != ' ' && c != '\t')
								{
									break;
								}
								i++;
								if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
								{
									goto Block_50;
								}
							}
							if (num5 >= 0 && num >= num5)
							{
								text = WebHeaderCollection.HeaderEncoding.GetString(ptr + num5, num - num5 + 1);
							}
							if (text.Length > 0)
							{
								this.AddInternal(text, text3);
							}
							totalResponseHeadersLength = num4;
							unparsed = i;
							if (num6 == 2)
							{
								goto Block_54;
							}
						}
						Block_7:
						DataParseStatus dataParseStatus = DataParseStatus.DataTooBig;
						goto IL_316;
						Block_8:
						dataParseStatus = DataParseStatus.NeedMoreData;
						goto IL_316;
						Block_13:
						dataParseStatus = DataParseStatus.DataTooBig;
						goto IL_316;
						Block_16:
						dataParseStatus = DataParseStatus.DataTooBig;
						goto IL_316;
						Block_17:
						dataParseStatus = DataParseStatus.NeedMoreData;
						goto IL_316;
						Block_22:
						dataParseStatus = DataParseStatus.NeedMoreData;
						goto IL_316;
						Block_25:
						dataParseStatus = DataParseStatus.DataTooBig;
						goto IL_316;
						Block_29:
						dataParseStatus = DataParseStatus.NeedMoreData;
						goto IL_316;
						Block_33:
						dataParseStatus = DataParseStatus.DataTooBig;
						goto IL_316;
						Block_34:
						dataParseStatus = DataParseStatus.NeedMoreData;
						goto IL_316;
						Block_38:
						dataParseStatus = DataParseStatus.DataTooBig;
						goto IL_316;
						Block_41:
						dataParseStatus = DataParseStatus.NeedMoreData;
						goto IL_316;
						Block_50:
						dataParseStatus = DataParseStatus.DataTooBig;
						goto IL_316;
						Block_54:
						dataParseStatus = DataParseStatus.Done;
						IL_316:
						if (dataParseStatus == DataParseStatus.Invalid)
						{
							parseError.Section = WebParseErrorSection.ResponseHeader;
							parseError.Code = code;
						}
						result = dataParseStatus;
					}
				}
			}
			finally
			{
				byte* ptr = null;
			}
			return result;
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0008E4C0 File Offset: 0x0008D4C0
		internal unsafe DataParseStatus ParseHeadersStrict(byte[] buffer, int size, ref int unparsed, ref int totalResponseHeadersLength, int maximumResponseHeadersLength, ref WebParseError parseError)
		{
			WebParseErrorCode code = WebParseErrorCode.Generic;
			DataParseStatus dataParseStatus = DataParseStatus.Invalid;
			int num = unparsed;
			int num2 = (maximumResponseHeadersLength <= 0) ? int.MaxValue : (maximumResponseHeadersLength - totalResponseHeadersLength + num);
			DataParseStatus dataParseStatus2 = DataParseStatus.DataTooBig;
			if (size < num2)
			{
				num2 = size;
				dataParseStatus2 = DataParseStatus.NeedMoreData;
			}
			if (num >= num2)
			{
				dataParseStatus = dataParseStatus2;
			}
			else
			{
				try
				{
					fixed (byte* ptr = buffer)
					{
						while (ptr[num] != 13)
						{
							int num3 = num;
							while (num < num2 && ((ptr[num] > 127) ? WebHeaderCollection.RfcChar.High : WebHeaderCollection.RfcCharMap[(int)ptr[num]]) == WebHeaderCollection.RfcChar.Reg)
							{
								num++;
							}
							if (num == num2)
							{
								dataParseStatus = dataParseStatus2;
								goto IL_42C;
							}
							if (num == num3)
							{
								dataParseStatus = DataParseStatus.Invalid;
								code = WebParseErrorCode.InvalidHeaderName;
								goto IL_42C;
							}
							int num4 = num - 1;
							int num5 = 0;
							WebHeaderCollection.RfcChar rfcChar;
							while (num < num2 && (rfcChar = ((ptr[num] > 127) ? WebHeaderCollection.RfcChar.High : WebHeaderCollection.RfcCharMap[(int)ptr[num]])) != WebHeaderCollection.RfcChar.Colon)
							{
								switch (rfcChar)
								{
								case WebHeaderCollection.RfcChar.CR:
									if (num5 != 0)
									{
										goto IL_122;
									}
									num5 = 1;
									break;
								case WebHeaderCollection.RfcChar.LF:
									if (num5 != 1)
									{
										goto IL_122;
									}
									num5 = 2;
									break;
								case WebHeaderCollection.RfcChar.WS:
									if (num5 == 1)
									{
										goto IL_122;
									}
									num5 = 0;
									break;
								default:
									goto IL_122;
								}
								num++;
								continue;
								IL_122:
								dataParseStatus = DataParseStatus.Invalid;
								code = WebParseErrorCode.CrLfError;
								goto IL_42C;
							}
							if (num == num2)
							{
								dataParseStatus = dataParseStatus2;
								goto IL_42C;
							}
							if (num5 != 0)
							{
								dataParseStatus = DataParseStatus.Invalid;
								code = WebParseErrorCode.IncompleteHeaderLine;
								goto IL_42C;
							}
							if (++num == num2)
							{
								dataParseStatus = dataParseStatus2;
								goto IL_42C;
							}
							int num6 = -1;
							int num7 = -1;
							StringBuilder stringBuilder = null;
							while (num < num2 && ((rfcChar = ((ptr[num] > 127) ? WebHeaderCollection.RfcChar.High : WebHeaderCollection.RfcCharMap[(int)ptr[num]])) == WebHeaderCollection.RfcChar.WS || num5 != 2))
							{
								switch (rfcChar)
								{
								case WebHeaderCollection.RfcChar.High:
								case WebHeaderCollection.RfcChar.Reg:
								case WebHeaderCollection.RfcChar.Colon:
								case WebHeaderCollection.RfcChar.Delim:
									if (num5 == 1)
									{
										goto IL_24A;
									}
									if (num5 == 3)
									{
										num5 = 0;
										if (num6 != -1)
										{
											string @string = WebHeaderCollection.HeaderEncoding.GetString(ptr + num6, num7 - num6 + 1);
											if (stringBuilder == null)
											{
												stringBuilder = new StringBuilder(@string, @string.Length * 5);
											}
											else
											{
												stringBuilder.Append(" ");
												stringBuilder.Append(@string);
											}
										}
										num6 = -1;
									}
									if (num6 == -1)
									{
										num6 = num;
									}
									num7 = num;
									break;
								case WebHeaderCollection.RfcChar.Ctl:
									goto IL_24A;
								case WebHeaderCollection.RfcChar.CR:
									if (num5 != 0)
									{
										goto IL_24A;
									}
									num5 = 1;
									break;
								case WebHeaderCollection.RfcChar.LF:
									if (num5 != 1)
									{
										goto IL_24A;
									}
									num5 = 2;
									break;
								case WebHeaderCollection.RfcChar.WS:
									if (num5 == 1)
									{
										goto IL_24A;
									}
									if (num5 == 2)
									{
										num5 = 3;
									}
									break;
								default:
									goto IL_24A;
								}
								num++;
								continue;
								IL_24A:
								dataParseStatus = DataParseStatus.Invalid;
								code = WebParseErrorCode.CrLfError;
								goto IL_42C;
							}
							if (num == num2)
							{
								dataParseStatus = dataParseStatus2;
								goto IL_42C;
							}
							string text = (num6 == -1) ? "" : WebHeaderCollection.HeaderEncoding.GetString(ptr + num6, num7 - num6 + 1);
							if (stringBuilder != null)
							{
								if (text.Length != 0)
								{
									stringBuilder.Append(" ");
									stringBuilder.Append(text);
								}
								text = stringBuilder.ToString();
							}
							string text2 = null;
							int num8 = num4 - num3 + 1;
							if (this.m_CommonHeaders != null)
							{
								int num9 = (int)WebHeaderCollection.s_CommonHeaderHints[(int)(ptr[num3] & 31)];
								if (num9 >= 0)
								{
									string text3;
									for (;;)
									{
										text3 = WebHeaderCollection.s_CommonHeaderNames[num9++];
										if (text3.Length < num8 || CaseInsensitiveAscii.AsciiToLower[(int)ptr[num3]] != CaseInsensitiveAscii.AsciiToLower[(int)text3[0]])
										{
											goto IL_3F8;
										}
										if (text3.Length <= num8)
										{
											byte* ptr2 = ptr + num3 + 1;
											int num10 = 1;
											while (num10 < text3.Length && ((char)(*(ptr2++)) == text3[num10] || CaseInsensitiveAscii.AsciiToLower[(int)(*(ptr2 - 1))] == CaseInsensitiveAscii.AsciiToLower[(int)text3[num10]]))
											{
												num10++;
											}
											if (num10 == text3.Length)
											{
												break;
											}
										}
									}
									this.m_NumCommonHeaders++;
									num9--;
									if (this.m_CommonHeaders[num9] == null)
									{
										this.m_CommonHeaders[num9] = text;
									}
									else
									{
										this.NormalizeCommonHeaders();
										this.AddInternalNotCommon(text3, text);
									}
									text2 = text3;
								}
							}
							IL_3F8:
							if (text2 == null)
							{
								text2 = WebHeaderCollection.HeaderEncoding.GetString(ptr + num3, num8);
								this.AddInternalNotCommon(text2, text);
							}
							totalResponseHeadersLength += num - unparsed;
							unparsed = num;
						}
						if (++num == num2)
						{
							dataParseStatus = dataParseStatus2;
						}
						else if (ptr[num++] == 10)
						{
							totalResponseHeadersLength += num - unparsed;
							unparsed = num;
							dataParseStatus = DataParseStatus.Done;
						}
						else
						{
							dataParseStatus = DataParseStatus.Invalid;
							code = WebParseErrorCode.CrLfError;
						}
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			IL_42C:
			if (dataParseStatus == DataParseStatus.Invalid)
			{
				parseError.Section = WebParseErrorSection.ResponseHeader;
				parseError.Code = code;
			}
			return dataParseStatus;
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0008E92C File Offset: 0x0008D92C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0008E938 File Offset: 0x0008D938
		public override string Get(string name)
		{
			if (this.m_CommonHeaders != null && name != null && name.Length > 0 && name[0] < 'Ā')
			{
				int num = (int)WebHeaderCollection.s_CommonHeaderHints[(int)(name[0] & '\u001f')];
				if (num >= 0)
				{
					for (;;)
					{
						string text = WebHeaderCollection.s_CommonHeaderNames[num++];
						if (text.Length < name.Length || CaseInsensitiveAscii.AsciiToLower[(int)name[0]] != CaseInsensitiveAscii.AsciiToLower[(int)text[0]])
						{
							goto IL_EF;
						}
						if (text.Length <= name.Length)
						{
							int num2 = 1;
							while (num2 < text.Length && (name[num2] == text[num2] || (name[num2] <= 'ÿ' && CaseInsensitiveAscii.AsciiToLower[(int)name[num2]] == CaseInsensitiveAscii.AsciiToLower[(int)text[num2]])))
							{
								num2++;
							}
							if (num2 == text.Length)
							{
								break;
							}
						}
					}
					return this.m_CommonHeaders[num - 1];
				}
			}
			IL_EF:
			if (this.m_InnerCollection == null)
			{
				return null;
			}
			return this.m_InnerCollection.Get(name);
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0008EA4A File Offset: 0x0008DA4A
		public override IEnumerator GetEnumerator()
		{
			this.NormalizeCommonHeaders();
			return new NameObjectCollectionBase.NameObjectKeysEnumerator(this.InnerCollection);
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x0008EA5D File Offset: 0x0008DA5D
		public override int Count
		{
			get
			{
				return ((this.m_InnerCollection == null) ? 0 : this.m_InnerCollection.Count) + this.m_NumCommonHeaders;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x0008EA7C File Offset: 0x0008DA7C
		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				this.NormalizeCommonHeaders();
				return this.InnerCollection.Keys;
			}
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0008EA8F File Offset: 0x0008DA8F
		internal override bool InternalHasKeys()
		{
			this.NormalizeCommonHeaders();
			return this.m_InnerCollection != null && this.m_InnerCollection.HasKeys();
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x0008EAAC File Offset: 0x0008DAAC
		public override string Get(int index)
		{
			this.NormalizeCommonHeaders();
			return this.InnerCollection.Get(index);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0008EAC0 File Offset: 0x0008DAC0
		public override string[] GetValues(int index)
		{
			this.NormalizeCommonHeaders();
			return this.InnerCollection.GetValues(index);
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0008EAD4 File Offset: 0x0008DAD4
		public override string GetKey(int index)
		{
			this.NormalizeCommonHeaders();
			return this.InnerCollection.GetKey(index);
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x0008EAE8 File Offset: 0x0008DAE8
		public override string[] AllKeys
		{
			get
			{
				this.NormalizeCommonHeaders();
				return this.InnerCollection.AllKeys;
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0008EAFB File Offset: 0x0008DAFB
		public override void Clear()
		{
			this.m_CommonHeaders = null;
			this.m_NumCommonHeaders = 0;
			base.InvalidateCachedArrays();
			if (this.m_InnerCollection != null)
			{
				this.m_InnerCollection.Clear();
			}
		}

		// Token: 0x0400248C RID: 9356
		private const int ApproxAveHeaderLineSize = 30;

		// Token: 0x0400248D RID: 9357
		private const int ApproxHighAvgNumHeaders = 16;

		// Token: 0x0400248E RID: 9358
		private const int c_AcceptRanges = 0;

		// Token: 0x0400248F RID: 9359
		private const int c_ContentLength = 1;

		// Token: 0x04002490 RID: 9360
		private const int c_CacheControl = 2;

		// Token: 0x04002491 RID: 9361
		private const int c_ContentType = 3;

		// Token: 0x04002492 RID: 9362
		private const int c_Date = 4;

		// Token: 0x04002493 RID: 9363
		private const int c_Expires = 5;

		// Token: 0x04002494 RID: 9364
		private const int c_ETag = 6;

		// Token: 0x04002495 RID: 9365
		private const int c_LastModified = 7;

		// Token: 0x04002496 RID: 9366
		private const int c_Location = 8;

		// Token: 0x04002497 RID: 9367
		private const int c_ProxyAuthenticate = 9;

		// Token: 0x04002498 RID: 9368
		private const int c_P3P = 10;

		// Token: 0x04002499 RID: 9369
		private const int c_SetCookie2 = 11;

		// Token: 0x0400249A RID: 9370
		private const int c_SetCookie = 12;

		// Token: 0x0400249B RID: 9371
		private const int c_Server = 13;

		// Token: 0x0400249C RID: 9372
		private const int c_Via = 14;

		// Token: 0x0400249D RID: 9373
		private const int c_WwwAuthenticate = 15;

		// Token: 0x0400249E RID: 9374
		private const int c_XAspNetVersion = 16;

		// Token: 0x0400249F RID: 9375
		private const int c_XPoweredBy = 17;

		// Token: 0x040024A0 RID: 9376
		private static readonly HeaderInfoTable HInfo = new HeaderInfoTable();

		// Token: 0x040024A1 RID: 9377
		private string[] m_CommonHeaders;

		// Token: 0x040024A2 RID: 9378
		private int m_NumCommonHeaders;

		// Token: 0x040024A3 RID: 9379
		private static readonly string[] s_CommonHeaderNames = new string[]
		{
			"Accept-Ranges",
			"Content-Length",
			"Cache-Control",
			"Content-Type",
			"Date",
			"Expires",
			"ETag",
			"Last-Modified",
			"Location",
			"Proxy-Authenticate",
			"P3P",
			"Set-Cookie2",
			"Set-Cookie",
			"Server",
			"Via",
			"WWW-Authenticate",
			"X-AspNet-Version",
			"X-Powered-By",
			"["
		};

		// Token: 0x040024A4 RID: 9380
		private static readonly sbyte[] s_CommonHeaderHints = new sbyte[]
		{
			-1,
			0,
			-1,
			1,
			4,
			5,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			7,
			-1,
			-1,
			-1,
			9,
			-1,
			-1,
			11,
			-1,
			-1,
			14,
			15,
			16,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1
		};

		// Token: 0x040024A5 RID: 9381
		private NameValueCollection m_InnerCollection;

		// Token: 0x040024A6 RID: 9382
		private WebHeaderCollectionType m_Type;

		// Token: 0x040024A7 RID: 9383
		private static readonly char[] HttpTrimCharacters = new char[]
		{
			'\t',
			'\n',
			'\v',
			'\f',
			'\r',
			' '
		};

		// Token: 0x040024A8 RID: 9384
		private static WebHeaderCollection.RfcChar[] RfcCharMap = new WebHeaderCollection.RfcChar[]
		{
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.WS,
			WebHeaderCollection.RfcChar.LF,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.CR,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.WS,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Colon,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Ctl
		};

		// Token: 0x020004A2 RID: 1186
		internal static class HeaderEncoding
		{
			// Token: 0x06002459 RID: 9305 RVA: 0x0008EED0 File Offset: 0x0008DED0
			internal unsafe static string GetString(byte[] bytes, int byteIndex, int byteCount)
			{
				fixed (byte* ptr = bytes)
				{
					return WebHeaderCollection.HeaderEncoding.GetString(ptr + byteIndex, byteCount);
				}
			}

			// Token: 0x0600245A RID: 9306 RVA: 0x0008EF04 File Offset: 0x0008DF04
			internal unsafe static string GetString(byte* pBytes, int byteCount)
			{
				if (byteCount < 1)
				{
					return "";
				}
				string text = new string('\0', byteCount);
				fixed (char* ptr = text)
				{
					char* ptr2 = ptr;
					while (byteCount >= 8)
					{
						*ptr2 = (char)(*pBytes);
						ptr2[1] = (char)pBytes[1];
						ptr2[2] = (char)pBytes[2];
						ptr2[3] = (char)pBytes[3];
						ptr2[4] = (char)pBytes[4];
						ptr2[5] = (char)pBytes[5];
						ptr2[6] = (char)pBytes[6];
						ptr2[7] = (char)pBytes[7];
						ptr2 += 8;
						pBytes += 8;
						byteCount -= 8;
					}
					for (int i = 0; i < byteCount; i++)
					{
						ptr2[i] = (char)pBytes[i];
					}
				}
				return text;
			}

			// Token: 0x0600245B RID: 9307 RVA: 0x0008EFB4 File Offset: 0x0008DFB4
			internal static int GetByteCount(string myString)
			{
				return myString.Length;
			}

			// Token: 0x0600245C RID: 9308 RVA: 0x0008EFBC File Offset: 0x0008DFBC
			internal unsafe static void GetBytes(string myString, int charIndex, int charCount, byte[] bytes, int byteIndex)
			{
				if (myString.Length == 0)
				{
					return;
				}
				fixed (byte* ptr = bytes)
				{
					byte* ptr2 = ptr + byteIndex;
					int num = charIndex + charCount;
					while (charIndex < num)
					{
						*(ptr2++) = (byte)myString[charIndex++];
					}
				}
			}

			// Token: 0x0600245D RID: 9309 RVA: 0x0008F010 File Offset: 0x0008E010
			internal static byte[] GetBytes(string myString)
			{
				byte[] array = new byte[myString.Length];
				if (myString.Length != 0)
				{
					WebHeaderCollection.HeaderEncoding.GetBytes(myString, 0, myString.Length, array, 0);
				}
				return array;
			}
		}

		// Token: 0x020004A3 RID: 1187
		private enum RfcChar : byte
		{
			// Token: 0x040024AA RID: 9386
			High,
			// Token: 0x040024AB RID: 9387
			Reg,
			// Token: 0x040024AC RID: 9388
			Ctl,
			// Token: 0x040024AD RID: 9389
			CR,
			// Token: 0x040024AE RID: 9390
			LF,
			// Token: 0x040024AF RID: 9391
			WS,
			// Token: 0x040024B0 RID: 9392
			Colon,
			// Token: 0x040024B1 RID: 9393
			Delim
		}
	}
}
