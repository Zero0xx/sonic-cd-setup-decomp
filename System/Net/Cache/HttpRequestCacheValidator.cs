using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200055B RID: 1371
	internal class HttpRequestCacheValidator : RequestCacheValidator
	{
		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x000AF03D File Offset: 0x000AE03D
		// (set) Token: 0x060029C6 RID: 10694 RVA: 0x000AF045 File Offset: 0x000AE045
		internal HttpStatusCode CacheStatusCode
		{
			get
			{
				return this.m_StatusCode;
			}
			set
			{
				this.m_StatusCode = value;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x000AF04E File Offset: 0x000AE04E
		// (set) Token: 0x060029C8 RID: 10696 RVA: 0x000AF056 File Offset: 0x000AE056
		internal string CacheStatusDescription
		{
			get
			{
				return this.m_StatusDescription;
			}
			set
			{
				this.m_StatusDescription = value;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x000AF05F File Offset: 0x000AE05F
		// (set) Token: 0x060029CA RID: 10698 RVA: 0x000AF067 File Offset: 0x000AE067
		internal Version CacheHttpVersion
		{
			get
			{
				return this.m_HttpVersion;
			}
			set
			{
				this.m_HttpVersion = value;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x000AF070 File Offset: 0x000AE070
		// (set) Token: 0x060029CC RID: 10700 RVA: 0x000AF078 File Offset: 0x000AE078
		internal WebHeaderCollection CacheHeaders
		{
			get
			{
				return this.m_Headers;
			}
			set
			{
				this.m_Headers = value;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060029CD RID: 10701 RVA: 0x000AF084 File Offset: 0x000AE084
		internal new HttpRequestCachePolicy Policy
		{
			get
			{
				if (this.m_HttpPolicy != null)
				{
					return this.m_HttpPolicy;
				}
				this.m_HttpPolicy = (base.Policy as HttpRequestCachePolicy);
				if (this.m_HttpPolicy != null)
				{
					return this.m_HttpPolicy;
				}
				this.m_HttpPolicy = new HttpRequestCachePolicy((HttpRequestCacheLevel)base.Policy.Level);
				return this.m_HttpPolicy;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x000AF0DC File Offset: 0x000AE0DC
		// (set) Token: 0x060029CF RID: 10703 RVA: 0x000AF0E4 File Offset: 0x000AE0E4
		internal NameValueCollection SystemMeta
		{
			get
			{
				return this.m_SystemMeta;
			}
			set
			{
				this.m_SystemMeta = value;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x000AF0ED File Offset: 0x000AE0ED
		// (set) Token: 0x060029D1 RID: 10705 RVA: 0x000AF0FA File Offset: 0x000AE0FA
		internal HttpMethod RequestMethod
		{
			get
			{
				return this.m_RequestVars.Method;
			}
			set
			{
				this.m_RequestVars.Method = value;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x000AF108 File Offset: 0x000AE108
		// (set) Token: 0x060029D3 RID: 10707 RVA: 0x000AF115 File Offset: 0x000AE115
		internal bool RequestRangeCache
		{
			get
			{
				return this.m_RequestVars.IsCacheRange;
			}
			set
			{
				this.m_RequestVars.IsCacheRange = value;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x000AF123 File Offset: 0x000AE123
		// (set) Token: 0x060029D5 RID: 10709 RVA: 0x000AF130 File Offset: 0x000AE130
		internal bool RequestRangeUser
		{
			get
			{
				return this.m_RequestVars.IsUserRange;
			}
			set
			{
				this.m_RequestVars.IsUserRange = value;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060029D6 RID: 10710 RVA: 0x000AF13E File Offset: 0x000AE13E
		// (set) Token: 0x060029D7 RID: 10711 RVA: 0x000AF14B File Offset: 0x000AE14B
		internal string RequestIfHeader1
		{
			get
			{
				return this.m_RequestVars.IfHeader1;
			}
			set
			{
				this.m_RequestVars.IfHeader1 = value;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060029D8 RID: 10712 RVA: 0x000AF159 File Offset: 0x000AE159
		// (set) Token: 0x060029D9 RID: 10713 RVA: 0x000AF166 File Offset: 0x000AE166
		internal string RequestValidator1
		{
			get
			{
				return this.m_RequestVars.Validator1;
			}
			set
			{
				this.m_RequestVars.Validator1 = value;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060029DA RID: 10714 RVA: 0x000AF174 File Offset: 0x000AE174
		// (set) Token: 0x060029DB RID: 10715 RVA: 0x000AF181 File Offset: 0x000AE181
		internal string RequestIfHeader2
		{
			get
			{
				return this.m_RequestVars.IfHeader2;
			}
			set
			{
				this.m_RequestVars.IfHeader2 = value;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x000AF18F File Offset: 0x000AE18F
		// (set) Token: 0x060029DD RID: 10717 RVA: 0x000AF19C File Offset: 0x000AE19C
		internal string RequestValidator2
		{
			get
			{
				return this.m_RequestVars.Validator2;
			}
			set
			{
				this.m_RequestVars.Validator2 = value;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x000AF1AA File Offset: 0x000AE1AA
		// (set) Token: 0x060029DF RID: 10719 RVA: 0x000AF1B2 File Offset: 0x000AE1B2
		internal bool CacheDontUpdateHeaders
		{
			get
			{
				return this.m_DontUpdateHeaders;
			}
			set
			{
				this.m_DontUpdateHeaders = value;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x000AF1BB File Offset: 0x000AE1BB
		// (set) Token: 0x060029E1 RID: 10721 RVA: 0x000AF1C8 File Offset: 0x000AE1C8
		internal DateTime CacheDate
		{
			get
			{
				return this.m_CacheVars.Date;
			}
			set
			{
				this.m_CacheVars.Date = value;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060029E2 RID: 10722 RVA: 0x000AF1D6 File Offset: 0x000AE1D6
		// (set) Token: 0x060029E3 RID: 10723 RVA: 0x000AF1E3 File Offset: 0x000AE1E3
		internal DateTime CacheExpires
		{
			get
			{
				return this.m_CacheVars.Expires;
			}
			set
			{
				this.m_CacheVars.Expires = value;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x000AF1F1 File Offset: 0x000AE1F1
		// (set) Token: 0x060029E5 RID: 10725 RVA: 0x000AF1FE File Offset: 0x000AE1FE
		internal DateTime CacheLastModified
		{
			get
			{
				return this.m_CacheVars.LastModified;
			}
			set
			{
				this.m_CacheVars.LastModified = value;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060029E6 RID: 10726 RVA: 0x000AF20C File Offset: 0x000AE20C
		// (set) Token: 0x060029E7 RID: 10727 RVA: 0x000AF219 File Offset: 0x000AE219
		internal long CacheEntityLength
		{
			get
			{
				return this.m_CacheVars.EntityLength;
			}
			set
			{
				this.m_CacheVars.EntityLength = value;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x000AF227 File Offset: 0x000AE227
		// (set) Token: 0x060029E9 RID: 10729 RVA: 0x000AF234 File Offset: 0x000AE234
		internal TimeSpan CacheAge
		{
			get
			{
				return this.m_CacheVars.Age;
			}
			set
			{
				this.m_CacheVars.Age = value;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060029EA RID: 10730 RVA: 0x000AF242 File Offset: 0x000AE242
		// (set) Token: 0x060029EB RID: 10731 RVA: 0x000AF24F File Offset: 0x000AE24F
		internal TimeSpan CacheMaxAge
		{
			get
			{
				return this.m_CacheVars.MaxAge;
			}
			set
			{
				this.m_CacheVars.MaxAge = value;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060029EC RID: 10732 RVA: 0x000AF25D File Offset: 0x000AE25D
		// (set) Token: 0x060029ED RID: 10733 RVA: 0x000AF265 File Offset: 0x000AE265
		internal bool HeuristicExpiration
		{
			get
			{
				return this.m_HeuristicExpiration;
			}
			set
			{
				this.m_HeuristicExpiration = value;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x000AF26E File Offset: 0x000AE26E
		// (set) Token: 0x060029EF RID: 10735 RVA: 0x000AF27B File Offset: 0x000AE27B
		internal ResponseCacheControl CacheCacheControl
		{
			get
			{
				return this.m_CacheVars.CacheControl;
			}
			set
			{
				this.m_CacheVars.CacheControl = value;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060029F0 RID: 10736 RVA: 0x000AF289 File Offset: 0x000AE289
		// (set) Token: 0x060029F1 RID: 10737 RVA: 0x000AF296 File Offset: 0x000AE296
		internal DateTime ResponseDate
		{
			get
			{
				return this.m_ResponseVars.Date;
			}
			set
			{
				this.m_ResponseVars.Date = value;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x000AF2A4 File Offset: 0x000AE2A4
		// (set) Token: 0x060029F3 RID: 10739 RVA: 0x000AF2B1 File Offset: 0x000AE2B1
		internal DateTime ResponseExpires
		{
			get
			{
				return this.m_ResponseVars.Expires;
			}
			set
			{
				this.m_ResponseVars.Expires = value;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060029F4 RID: 10740 RVA: 0x000AF2BF File Offset: 0x000AE2BF
		// (set) Token: 0x060029F5 RID: 10741 RVA: 0x000AF2CC File Offset: 0x000AE2CC
		internal DateTime ResponseLastModified
		{
			get
			{
				return this.m_ResponseVars.LastModified;
			}
			set
			{
				this.m_ResponseVars.LastModified = value;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060029F6 RID: 10742 RVA: 0x000AF2DA File Offset: 0x000AE2DA
		// (set) Token: 0x060029F7 RID: 10743 RVA: 0x000AF2E7 File Offset: 0x000AE2E7
		internal long ResponseEntityLength
		{
			get
			{
				return this.m_ResponseVars.EntityLength;
			}
			set
			{
				this.m_ResponseVars.EntityLength = value;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060029F8 RID: 10744 RVA: 0x000AF2F5 File Offset: 0x000AE2F5
		// (set) Token: 0x060029F9 RID: 10745 RVA: 0x000AF302 File Offset: 0x000AE302
		internal long ResponseRangeStart
		{
			get
			{
				return this.m_ResponseVars.RangeStart;
			}
			set
			{
				this.m_ResponseVars.RangeStart = value;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060029FA RID: 10746 RVA: 0x000AF310 File Offset: 0x000AE310
		// (set) Token: 0x060029FB RID: 10747 RVA: 0x000AF31D File Offset: 0x000AE31D
		internal long ResponseRangeEnd
		{
			get
			{
				return this.m_ResponseVars.RangeEnd;
			}
			set
			{
				this.m_ResponseVars.RangeEnd = value;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060029FC RID: 10748 RVA: 0x000AF32B File Offset: 0x000AE32B
		// (set) Token: 0x060029FD RID: 10749 RVA: 0x000AF338 File Offset: 0x000AE338
		internal TimeSpan ResponseAge
		{
			get
			{
				return this.m_ResponseVars.Age;
			}
			set
			{
				this.m_ResponseVars.Age = value;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060029FE RID: 10750 RVA: 0x000AF346 File Offset: 0x000AE346
		// (set) Token: 0x060029FF RID: 10751 RVA: 0x000AF353 File Offset: 0x000AE353
		internal ResponseCacheControl ResponseCacheControl
		{
			get
			{
				return this.m_ResponseVars.CacheControl;
			}
			set
			{
				this.m_ResponseVars.CacheControl = value;
			}
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000AF364 File Offset: 0x000AE364
		private void ZeroPrivateVars()
		{
			this.m_RequestVars = default(HttpRequestCacheValidator.RequestVars);
			this.m_HttpPolicy = null;
			this.m_StatusCode = (HttpStatusCode)0;
			this.m_StatusDescription = null;
			this.m_HttpVersion = null;
			this.m_Headers = null;
			this.m_SystemMeta = null;
			this.m_DontUpdateHeaders = false;
			this.m_HeuristicExpiration = false;
			this.m_CacheVars = default(HttpRequestCacheValidator.Vars);
			this.m_CacheVars.Initialize();
			this.m_ResponseVars = default(HttpRequestCacheValidator.Vars);
			this.m_ResponseVars.Initialize();
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000AF3E3 File Offset: 0x000AE3E3
		internal override RequestCacheValidator CreateValidator()
		{
			return new HttpRequestCacheValidator(base.StrictCacheErrors, base.UnspecifiedMaxAge);
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000AF3F6 File Offset: 0x000AE3F6
		internal HttpRequestCacheValidator(bool strictCacheErrors, TimeSpan unspecifiedMaxAge) : base(strictCacheErrors, unspecifiedMaxAge)
		{
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000AF400 File Offset: 0x000AE400
		protected internal override CacheValidationStatus ValidateRequest()
		{
			this.ZeroPrivateVars();
			string text = base.Request.Method.ToUpper(CultureInfo.InvariantCulture);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_request_method", new object[]
				{
					text
				}));
			}
			string key;
			switch (key = text)
			{
			case "GET":
				this.RequestMethod = HttpMethod.Get;
				goto IL_149;
			case "POST":
				this.RequestMethod = HttpMethod.Post;
				goto IL_149;
			case "HEAD":
				this.RequestMethod = HttpMethod.Head;
				goto IL_149;
			case "PUT":
				this.RequestMethod = HttpMethod.Put;
				goto IL_149;
			case "DELETE":
				this.RequestMethod = HttpMethod.Delete;
				goto IL_149;
			case "OPTIONS":
				this.RequestMethod = HttpMethod.Options;
				goto IL_149;
			case "TRACE":
				this.RequestMethod = HttpMethod.Trace;
				goto IL_149;
			case "CONNECT":
				this.RequestMethod = HttpMethod.Connect;
				goto IL_149;
			}
			this.RequestMethod = HttpMethod.Other;
			IL_149:
			return Rfc2616.OnValidateRequest(this);
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000AF55C File Offset: 0x000AE55C
		protected internal override CacheFreshnessStatus ValidateFreshness()
		{
			string text = this.ParseStatusLine();
			if (Logging.On)
			{
				if (this.CacheStatusCode == (HttpStatusCode)0)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_http_status_parse_failure", new object[]
					{
						(text == null) ? "null" : text
					}));
				}
				else
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_http_status_line", new object[]
					{
						(this.CacheHttpVersion != null) ? this.CacheHttpVersion.ToString() : "null",
						(int)this.CacheStatusCode,
						this.CacheStatusDescription
					}));
				}
			}
			this.CreateCacheHeaders(this.CacheStatusCode != (HttpStatusCode)0);
			this.CreateSystemMeta();
			this.FetchHeaderValues(true);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_cache_control", new object[]
				{
					this.CacheCacheControl.ToString()
				}));
			}
			return Rfc2616.OnValidateFreshness(this);
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000AF65C File Offset: 0x000AE65C
		protected internal override CacheValidationStatus ValidateCache()
		{
			if (this.Policy.Level != HttpRequestCacheLevel.Revalidate && base.Policy.Level >= RequestCacheLevel.Reload)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_validator_invalid_for_policy", new object[]
					{
						this.Policy.ToString()
					}));
				}
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (base.CacheStream == Stream.Null || this.CacheStatusCode == (HttpStatusCode)0 || this.CacheStatusCode == HttpStatusCode.NotModified)
			{
				if (this.Policy.Level == HttpRequestCacheLevel.CacheOnly)
				{
					this.FailRequest(WebExceptionStatus.CacheEntryNotFound);
				}
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (this.RequestMethod == HttpMethod.Head)
			{
				base.CacheStream.Close();
				base.CacheStream = new SyncMemoryStream(new byte[0]);
			}
			this.RemoveWarnings_1xx();
			base.CacheStreamOffset = 0L;
			base.CacheStreamLength = base.CacheEntry.StreamSize;
			CacheValidationStatus cacheValidationStatus = Rfc2616.OnValidateCache(this);
			if (cacheValidationStatus != CacheValidationStatus.ReturnCachedResponse && this.Policy.Level == HttpRequestCacheLevel.CacheOnly)
			{
				this.FailRequest(WebExceptionStatus.CacheEntryNotFound);
			}
			if (cacheValidationStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				if (base.CacheFreshnessStatus == CacheFreshnessStatus.Stale)
				{
					this.CacheHeaders.Add("Warning", "110 Response is stale");
				}
				if (base.Policy.Level == RequestCacheLevel.CacheOnly)
				{
					this.CacheHeaders.Add("Warning", "112 Disconnected operation");
				}
				if (this.HeuristicExpiration && (int)this.CacheAge.TotalSeconds >= 86400)
				{
					this.CacheHeaders.Add("Warning", "113 Heuristic expiration");
				}
			}
			if (cacheValidationStatus == CacheValidationStatus.DoNotTakeFromCache)
			{
				this.CacheStatusCode = (HttpStatusCode)0;
			}
			else if (cacheValidationStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				this.CacheHeaders["Age"] = ((int)this.CacheAge.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000AF810 File Offset: 0x000AE810
		protected internal override CacheValidationStatus RevalidateCache()
		{
			if (this.Policy.Level != HttpRequestCacheLevel.Revalidate && base.Policy.Level >= RequestCacheLevel.Reload)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_validator_invalid_for_policy", new object[]
					{
						this.Policy.ToString()
					}));
				}
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (base.CacheStream == Stream.Null || this.CacheStatusCode == (HttpStatusCode)0 || this.CacheStatusCode == HttpStatusCode.NotModified)
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			CacheValidationStatus cacheValidationStatus = CacheValidationStatus.DoNotTakeFromCache;
			HttpWebResponse httpWebResponse = base.Response as HttpWebResponse;
			if (httpWebResponse == null)
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (httpWebResponse.StatusCode >= HttpStatusCode.InternalServerError)
			{
				if (Rfc2616.Common.ValidateCacheOn5XXResponse(this) == CacheValidationStatus.ReturnCachedResponse)
				{
					if (base.CacheFreshnessStatus == CacheFreshnessStatus.Stale)
					{
						this.CacheHeaders.Add("Warning", "110 Response is stale");
					}
					if (this.HeuristicExpiration && (int)this.CacheAge.TotalSeconds >= 86400)
					{
						this.CacheHeaders.Add("Warning", "113 Heuristic expiration");
					}
				}
			}
			else if (base.ResponseCount > 1)
			{
				cacheValidationStatus = CacheValidationStatus.DoNotTakeFromCache;
			}
			else
			{
				this.CacheAge = TimeSpan.Zero;
				cacheValidationStatus = Rfc2616.Common.ValidateCacheAfterResponse(this, httpWebResponse);
			}
			if (cacheValidationStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				this.CacheHeaders["Age"] = ((int)this.CacheAge.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000AF960 File Offset: 0x000AE960
		protected internal override CacheValidationStatus ValidateResponse()
		{
			if (this.Policy.Level != HttpRequestCacheLevel.CacheOrNextCacheOnly && this.Policy.Level != HttpRequestCacheLevel.Default && this.Policy.Level != HttpRequestCacheLevel.Revalidate)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_response_valid_based_on_policy", new object[]
					{
						this.Policy.ToString()
					}));
				}
				return CacheValidationStatus.Continue;
			}
			HttpWebResponse httpWebResponse = base.Response as HttpWebResponse;
			if (httpWebResponse == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_null_response_failure"));
				}
				return CacheValidationStatus.Continue;
			}
			this.FetchHeaderValues(false);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, string.Concat(new object[]
				{
					"StatusCode=",
					((int)httpWebResponse.StatusCode).ToString(CultureInfo.InvariantCulture),
					' ',
					httpWebResponse.StatusCode.ToString(),
					(httpWebResponse.StatusCode == HttpStatusCode.PartialContent) ? (", Content-Range: " + httpWebResponse.Headers["Content-Range"]) : string.Empty
				}));
			}
			return Rfc2616.OnValidateResponse(this);
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000AFA94 File Offset: 0x000AEA94
		protected internal override CacheValidationStatus UpdateCache()
		{
			if (this.Policy.Level == HttpRequestCacheLevel.NoCacheNoStore)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_removed_existing_based_on_policy", new object[]
					{
						this.Policy.ToString()
					}));
				}
				return CacheValidationStatus.RemoveFromCache;
			}
			if (this.Policy.Level == HttpRequestCacheLevel.CacheOnly)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_not_updated_based_on_policy", new object[]
					{
						this.Policy.ToString()
					}));
				}
				return CacheValidationStatus.DoNotUpdateCache;
			}
			if (this.CacheHeaders == null)
			{
				this.CacheHeaders = new WebHeaderCollection();
			}
			if (this.SystemMeta == null)
			{
				this.SystemMeta = new NameValueCollection(1, CaseInsensitiveAscii.StaticInstance);
			}
			if (this.ResponseCacheControl == null)
			{
				this.FetchHeaderValues(false);
			}
			CacheValidationStatus cacheValidationStatus = Rfc2616.OnUpdateCache(this);
			if (cacheValidationStatus == CacheValidationStatus.UpdateResponseInformation || cacheValidationStatus == CacheValidationStatus.CacheResponse)
			{
				this.FinallyUpdateCacheEntry();
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000AFB78 File Offset: 0x000AEB78
		private void FinallyUpdateCacheEntry()
		{
			base.CacheEntry.EntryMetadata = null;
			base.CacheEntry.SystemMetadata = null;
			if (this.CacheHeaders == null)
			{
				return;
			}
			base.CacheEntry.EntryMetadata = new StringCollection();
			base.CacheEntry.SystemMetadata = new StringCollection();
			if (this.CacheHttpVersion == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_invalid_http_version"));
				}
				this.CacheHttpVersion = new Version(1, 0);
			}
			StringBuilder stringBuilder = new StringBuilder(this.CacheStatusDescription.Length + 20);
			stringBuilder.Append("HTTP/");
			stringBuilder.Append(this.CacheHttpVersion.ToString(2));
			stringBuilder.Append(' ');
			stringBuilder.Append(((int)this.CacheStatusCode).ToString(NumberFormatInfo.InvariantInfo));
			stringBuilder.Append(' ');
			stringBuilder.Append(this.CacheStatusDescription);
			base.CacheEntry.EntryMetadata.Add(stringBuilder.ToString());
			HttpRequestCacheValidator.UpdateStringCollection(base.CacheEntry.EntryMetadata, this.CacheHeaders, false);
			if (this.SystemMeta != null)
			{
				HttpRequestCacheValidator.UpdateStringCollection(base.CacheEntry.SystemMetadata, this.SystemMeta, true);
			}
			if (this.ResponseExpires != DateTime.MinValue)
			{
				base.CacheEntry.ExpiresUtc = this.ResponseExpires;
			}
			if (this.ResponseLastModified != DateTime.MinValue)
			{
				base.CacheEntry.LastModifiedUtc = this.ResponseLastModified;
			}
			if (this.Policy.Level == HttpRequestCacheLevel.Default)
			{
				base.CacheEntry.MaxStale = this.Policy.MaxStale;
			}
			base.CacheEntry.LastSynchronizedUtc = DateTime.UtcNow;
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000AFD30 File Offset: 0x000AED30
		private static void UpdateStringCollection(StringCollection result, NameValueCollection cc, bool winInetCompat)
		{
			for (int i = 0; i < cc.Count; i++)
			{
				StringBuilder stringBuilder = new StringBuilder(40);
				string key = cc.GetKey(i);
				stringBuilder.Append(key).Append(':');
				string[] values = cc.GetValues(i);
				if (values.Length != 0)
				{
					if (winInetCompat)
					{
						stringBuilder.Append(values[0]);
					}
					else
					{
						stringBuilder.Append(' ').Append(values[0]);
					}
				}
				for (int j = 1; j < values.Length; j++)
				{
					stringBuilder.Append(key).Append(", ").Append(values[j]);
				}
				result.Add(stringBuilder.ToString());
			}
			result.Add(string.Empty);
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000AFDE8 File Offset: 0x000AEDE8
		private string ParseStatusLine()
		{
			this.CacheStatusCode = (HttpStatusCode)0;
			if (base.CacheEntry.EntryMetadata == null || base.CacheEntry.EntryMetadata.Count == 0)
			{
				return null;
			}
			string text = base.CacheEntry.EntryMetadata[0];
			if (text == null)
			{
				return null;
			}
			int num = 0;
			char c = '\0';
			while (++num < text.Length && (c = text[num]) != '/')
			{
			}
			if (num == text.Length)
			{
				return text;
			}
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			while (++num < text.Length && (c = text[num]) >= '0' && c <= '9')
			{
				num2 = ((num2 < 0) ? 0 : (num2 * 10)) + (int)(c - '0');
			}
			if (num2 < 0 || c != '.')
			{
				return text;
			}
			while (++num < text.Length && (c = text[num]) >= '0' && c <= '9')
			{
				num3 = ((num3 < 0) ? 0 : (num3 * 10)) + (int)(c - '0');
			}
			if (num3 < 0 || (c != ' ' && c != '\t'))
			{
				return text;
			}
			while (++num < text.Length && ((c = text[num]) == ' ' || c == '\t'))
			{
			}
			if (num >= text.Length)
			{
				return text;
			}
			while (c >= '0' && c <= '9')
			{
				num4 = ((num4 < 0) ? 0 : (num4 * 10)) + (int)(c - '0');
				if (++num == text.Length)
				{
					break;
				}
				c = text[num];
			}
			if (num4 < 0 || (num <= text.Length && c != ' ' && c != '\t'))
			{
				return text;
			}
			while (num < text.Length && (text[num] == ' ' || text[num] == '\t'))
			{
				num++;
			}
			this.CacheStatusDescription = text.Substring(num);
			this.CacheHttpVersion = new Version(num2, num3);
			this.CacheStatusCode = (HttpStatusCode)num4;
			return text;
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000AFFA4 File Offset: 0x000AEFA4
		private void CreateCacheHeaders(bool ignoreFirstString)
		{
			if (this.CacheHeaders == null)
			{
				this.CacheHeaders = new WebHeaderCollection();
			}
			if (base.CacheEntry.EntryMetadata == null || base.CacheEntry.EntryMetadata.Count == 0)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_no_http_response_header"));
				}
				return;
			}
			string text = this.ParseNameValues(this.CacheHeaders, base.CacheEntry.EntryMetadata, ignoreFirstString ? 1 : 0);
			if (text != null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_http_header_parse_error", new object[]
					{
						text
					}));
				}
				this.CacheHeaders.Clear();
			}
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000B0054 File Offset: 0x000AF054
		private void CreateSystemMeta()
		{
			if (this.SystemMeta == null)
			{
				this.SystemMeta = new NameValueCollection((base.CacheEntry.EntryMetadata == null || base.CacheEntry.EntryMetadata.Count == 0) ? 2 : base.CacheEntry.EntryMetadata.Count, CaseInsensitiveAscii.StaticInstance);
			}
			if (base.CacheEntry.EntryMetadata == null || base.CacheEntry.EntryMetadata.Count == 0)
			{
				return;
			}
			string text = this.ParseNameValues(this.SystemMeta, base.CacheEntry.SystemMetadata, 0);
			if (text != null && Logging.On)
			{
				Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_metadata_name_value_parse_error", new object[]
				{
					text
				}));
			}
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000B0110 File Offset: 0x000AF110
		private string ParseNameValues(NameValueCollection cc, StringCollection sc, int start)
		{
			WebHeaderCollection webHeaderCollection = cc as WebHeaderCollection;
			string text = null;
			if (sc != null)
			{
				for (int i = start; i < sc.Count; i++)
				{
					string text2 = sc[i];
					if (text2 == null || text2.Length == 0)
					{
						return null;
					}
					if (text2[0] == ' ' || text2[0] == '\t')
					{
						if (text == null)
						{
							return text2;
						}
						if (webHeaderCollection != null)
						{
							webHeaderCollection.AddInternal(text, text2);
						}
						else
						{
							cc.Add(text, text2);
						}
					}
					int num = text2.IndexOf(':');
					if (num < 0)
					{
						return text2;
					}
					text = text2.Substring(0, num);
					while (++num < text2.Length)
					{
						if (text2[num] != ' ' && text2[num] != '\t')
						{
							break;
						}
					}
					try
					{
						if (webHeaderCollection != null)
						{
							webHeaderCollection.AddInternal(text, text2.Substring(num));
						}
						else
						{
							cc.Add(text, text2.Substring(num));
						}
					}
					catch (Exception ex)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
						return text2;
					}
				}
			}
			return null;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000B0228 File Offset: 0x000AF228
		private void FetchHeaderValues(bool forCache)
		{
			WebHeaderCollection webHeaderCollection = forCache ? this.CacheHeaders : base.Response.Headers;
			this.FetchCacheControl(webHeaderCollection.CacheControl, forCache);
			string text = webHeaderCollection.Date;
			DateTime dateTime = DateTime.MinValue;
			if (text != null && HttpDateParse.ParseHttpDate(text, out dateTime))
			{
				dateTime = dateTime.ToUniversalTime();
			}
			if (forCache)
			{
				this.CacheDate = dateTime;
			}
			else
			{
				this.ResponseDate = dateTime;
			}
			text = webHeaderCollection.Expires;
			dateTime = DateTime.MinValue;
			if (text != null && HttpDateParse.ParseHttpDate(text, out dateTime))
			{
				dateTime = dateTime.ToUniversalTime();
			}
			if (forCache)
			{
				this.CacheExpires = dateTime;
			}
			else
			{
				this.ResponseExpires = dateTime;
			}
			text = webHeaderCollection.LastModified;
			dateTime = DateTime.MinValue;
			if (text != null && HttpDateParse.ParseHttpDate(text, out dateTime))
			{
				dateTime = dateTime.ToUniversalTime();
			}
			if (forCache)
			{
				this.CacheLastModified = dateTime;
			}
			else
			{
				this.ResponseLastModified = dateTime;
			}
			long num = -1L;
			long responseRangeStart = -1L;
			long responseRangeEnd = -1L;
			HttpWebResponse httpWebResponse = base.Response as HttpWebResponse;
			if ((forCache ? this.CacheStatusCode : httpWebResponse.StatusCode) != HttpStatusCode.PartialContent)
			{
				text = webHeaderCollection.ContentLength;
				if (text != null && text.Length != 0)
				{
					int num2 = 0;
					char c = text[0];
					while (num2 < text.Length && c == ' ')
					{
						c = text[++num2];
					}
					if (num2 != text.Length && c >= '0' && c <= '9')
					{
						num = (long)(c - '0');
						while (++num2 < text.Length && (c = text[num2]) >= '0')
						{
							if (c > '9')
							{
								break;
							}
							num = num * 10L + (long)(c - '0');
						}
					}
				}
			}
			else
			{
				text = webHeaderCollection["Content-Range"];
				if (text == null || !Rfc2616.Common.GetBytesRange(text, ref responseRangeStart, ref responseRangeEnd, ref num, false))
				{
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_content_range_error", new object[]
						{
							(text == null) ? "<null>" : text
						}));
					}
					responseRangeEnd = (responseRangeStart = (num = -1L));
				}
				else if (forCache && num == base.CacheEntry.StreamSize)
				{
					responseRangeStart = -1L;
					responseRangeEnd = -1L;
					this.CacheStatusCode = HttpStatusCode.OK;
					this.CacheStatusDescription = "OK";
				}
			}
			if (forCache)
			{
				this.CacheEntityLength = num;
				this.ResponseRangeStart = responseRangeStart;
				this.ResponseRangeEnd = responseRangeEnd;
			}
			else
			{
				this.ResponseEntityLength = num;
				this.ResponseRangeStart = responseRangeStart;
				this.ResponseRangeEnd = responseRangeEnd;
			}
			TimeSpan timeSpan = TimeSpan.MinValue;
			text = webHeaderCollection["Age"];
			if (text != null)
			{
				int i = 0;
				int num3 = 0;
				while (i < text.Length)
				{
					if (text[i++] != ' ')
					{
						break;
					}
				}
				while (i < text.Length && text[i] >= '0' && text[i] <= '9')
				{
					num3 = num3 * 10 + (int)(text[i++] - '0');
				}
				timeSpan = TimeSpan.FromSeconds((double)num3);
			}
			if (forCache)
			{
				this.CacheAge = timeSpan;
				return;
			}
			this.ResponseAge = timeSpan;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000B0528 File Offset: 0x000AF528
		private unsafe void FetchCacheControl(string s, bool forCache)
		{
			ResponseCacheControl responseCacheControl = new ResponseCacheControl();
			if (forCache)
			{
				this.CacheCacheControl = responseCacheControl;
			}
			else
			{
				this.ResponseCacheControl = responseCacheControl;
			}
			if (s != null && s.Length != 0)
			{
				fixed (char* ptr = s)
				{
					int length = s.Length;
					for (int i = 0; i < length - 4; i++)
					{
						if (ptr[i] < ' ' || ptr[i] >= '\u007f')
						{
							if (Logging.On)
							{
								Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_cache_control_error", new object[]
								{
									s
								}));
							}
							return;
						}
						if (ptr[i] != ' ' && ptr[i] != ',')
						{
							if (IntPtr.Size == 4)
							{
								long* ptr2 = (long*)(ptr + i);
								long num = *ptr2 | 9007336695791648L;
								if (num <= 30399718399213680L)
								{
									if (num <= 27303540895318131L)
									{
										if (num != 12666889354412141L)
										{
											if (num == 27303540895318131L)
											{
												if (i + 8 > length)
												{
													return;
												}
												if ((ptr2[1] | 2097184L) == 28429415035764856L)
												{
													i += 8;
													while (i < length && ptr[i] == ' ')
													{
														i++;
													}
													if (i == length || ptr[(IntPtr)(i++) * 2] != '=')
													{
														return;
													}
													while (i < length && ptr[i] == ' ')
													{
														i++;
													}
													if (i == length)
													{
														return;
													}
													responseCacheControl.SMaxAge = 0;
													while (i < length && ptr[i] >= '0' && ptr[i] <= '9')
													{
														responseCacheControl.SMaxAge = responseCacheControl.SMaxAge * 10 + (int)(ptr[(IntPtr)(i++) * 2] - '0');
													}
													i--;
												}
											}
										}
										else
										{
											if (i + 7 > length)
											{
												return;
											}
											if ((*(int*)(ptr2 + 1) | 2097184) == 6750305 && (ptr[i + 6] | ' ') == 'e')
											{
												i += 7;
												while (i < length && ptr[i] == ' ')
												{
													i++;
												}
												if (i == length || ptr[(IntPtr)(i++) * 2] != '=')
												{
													return;
												}
												while (i < length && ptr[i] == ' ')
												{
													i++;
												}
												if (i == length)
												{
													return;
												}
												responseCacheControl.MaxAge = 0;
												while (i < length && ptr[i] >= '0' && ptr[i] <= '9')
												{
													responseCacheControl.MaxAge = responseCacheControl.MaxAge * 10 + (int)(ptr[(IntPtr)(i++) * 2] - '0');
												}
												i--;
											}
										}
									}
									else if (num != 27866215975157870L)
									{
										if (num == 30399718399213680L)
										{
											if (i + 6 > length)
											{
												return;
											}
											if ((*(int*)(ptr2 + 1) | 2097184) == 6488169)
											{
												responseCacheControl.Public = true;
												i += 5;
											}
										}
									}
									else
									{
										if (i + 8 > length)
										{
											return;
										}
										if ((ptr2[1] | 2097184L) == 28429419330863201L)
										{
											responseCacheControl.NoCache = true;
											i += 7;
											while (i < length && ptr[i] == ' ')
											{
												i++;
											}
											if (i >= length || ptr[i] != '=')
											{
												i--;
											}
											else
											{
												while (i < length && ptr[(IntPtr)(++i) * 2] == ' ')
												{
												}
												if (i >= length || ptr[i] != '"')
												{
													i--;
												}
												else
												{
													ArrayList arrayList = new ArrayList();
													i++;
													while (i < length && ptr[i] != '"')
													{
														while (i < length && ptr[i] == ' ')
														{
															i++;
														}
														int num2 = i;
														while (i < length && ptr[i] != ' ' && ptr[i] != ',' && ptr[i] != '"')
														{
															i++;
														}
														if (num2 != i)
														{
															arrayList.Add(s.Substring(num2, i - num2));
														}
														while (i < length && ptr[i] != ',' && ptr[i] != '"')
														{
															i++;
														}
													}
													if (arrayList.Count != 0)
													{
														responseCacheControl.NoCacheHeaders = (string[])arrayList.ToArray(typeof(string));
													}
												}
											}
										}
									}
								}
								else if (num <= 32651591227342957L)
								{
									if (num != 32369815602528366L)
									{
										if (num == 32651591227342957L)
										{
											if (i + 15 <= length && (ptr2[1] | 9007336695791648L) == 33214481051025453L && (ptr2[2] | 9007336695791648L) == 28147948649709665L && (*(int*)(ptr2 + 3) | 2097184) == 7602273 && (ptr[i + 14] | ' ') == 'e')
											{
												responseCacheControl.MustRevalidate = true;
												i += 14;
											}
										}
									}
									else
									{
										if (i + 8 > length)
										{
											return;
										}
										if ((ptr2[1] | 2097184L) == 28429462281322612L)
										{
											responseCacheControl.NoStore = true;
											i += 7;
										}
									}
								}
								else if (num != 33214498230894704L)
								{
									if (num == 33777473954119792L && i + 16 <= length && (ptr2[1] | 9007336695791648L) == 28429462276997241L && (ptr2[2] | 9007336695791648L) == 29555336417443958L && (ptr2[3] | 9007336695791648L) == 28429470870339684L)
									{
										responseCacheControl.ProxyRevalidate = true;
										i += 15;
									}
								}
								else
								{
									if (i + 7 > length)
									{
										return;
									}
									if ((*(int*)(ptr2 + 1) | 2097184) == 7602273 && (ptr[i + 6] | ' ') == 'e')
									{
										responseCacheControl.Private = true;
										i += 6;
										while (i < length && ptr[i] == ' ')
										{
											i++;
										}
										if (i >= length || ptr[i] != '=')
										{
											i--;
										}
										else
										{
											while (i < length && ptr[(IntPtr)(++i) * 2] == ' ')
											{
											}
											if (i >= length || ptr[i] != '"')
											{
												i--;
											}
											else
											{
												ArrayList arrayList2 = new ArrayList();
												i++;
												while (i < length && ptr[i] != '"')
												{
													while (i < length && ptr[i] == ' ')
													{
														i++;
													}
													int num3 = i;
													while (i < length && ptr[i] != ' ' && ptr[i] != ',' && ptr[i] != '"')
													{
														i++;
													}
													if (num3 != i)
													{
														arrayList2.Add(s.Substring(num3, i - num3));
													}
													while (i < length && ptr[i] != ',' && ptr[i] != '"')
													{
														i++;
													}
												}
												if (arrayList2.Count != 0)
												{
													responseCacheControl.PrivateHeaders = (string[])arrayList2.ToArray(typeof(string));
												}
											}
										}
									}
								}
							}
							else if (Rfc2616.Common.UnsafeAsciiLettersNoCaseEqual(ptr, i, length, "proxy-revalidate"))
							{
								responseCacheControl.ProxyRevalidate = true;
								i += 15;
							}
							else if (Rfc2616.Common.UnsafeAsciiLettersNoCaseEqual(ptr, i, length, "public"))
							{
								responseCacheControl.Public = true;
								i += 5;
							}
							else if (Rfc2616.Common.UnsafeAsciiLettersNoCaseEqual(ptr, i, length, "private"))
							{
								responseCacheControl.Private = true;
								i += 6;
								while (i < length && ptr[i] == ' ')
								{
									i++;
								}
								if (i >= length || ptr[i] != '=')
								{
									i--;
									break;
								}
								while (i < length && ptr[(IntPtr)(++i) * 2] == ' ')
								{
								}
								if (i >= length || ptr[i] != '"')
								{
									i--;
									break;
								}
								ArrayList arrayList3 = new ArrayList();
								i++;
								while (i < length && ptr[i] != '"')
								{
									while (i < length && ptr[i] == ' ')
									{
										i++;
									}
									int num4 = i;
									while (i < length && ptr[i] != ' ' && ptr[i] != ',' && ptr[i] != '"')
									{
										i++;
									}
									if (num4 != i)
									{
										arrayList3.Add(s.Substring(num4, i - num4));
									}
									while (i < length && ptr[i] != ',' && ptr[i] != '"')
									{
										i++;
									}
								}
								if (arrayList3.Count != 0)
								{
									responseCacheControl.PrivateHeaders = (string[])arrayList3.ToArray(typeof(string));
								}
							}
							else if (Rfc2616.Common.UnsafeAsciiLettersNoCaseEqual(ptr, i, length, "no-cache"))
							{
								responseCacheControl.NoCache = true;
								i += 7;
								while (i < length && ptr[i] == ' ')
								{
									i++;
								}
								if (i >= length || ptr[i] != '=')
								{
									i--;
									break;
								}
								while (i < length && ptr[(IntPtr)(++i) * 2] == ' ')
								{
								}
								if (i >= length || ptr[i] != '"')
								{
									i--;
									break;
								}
								ArrayList arrayList4 = new ArrayList();
								i++;
								while (i < length && ptr[i] != '"')
								{
									while (i < length && ptr[i] == ' ')
									{
										i++;
									}
									int num5 = i;
									while (i < length && ptr[i] != ' ' && ptr[i] != ',' && ptr[i] != '"')
									{
										i++;
									}
									if (num5 != i)
									{
										arrayList4.Add(s.Substring(num5, i - num5));
									}
									while (i < length && ptr[i] != ',' && ptr[i] != '"')
									{
										i++;
									}
								}
								if (arrayList4.Count != 0)
								{
									responseCacheControl.NoCacheHeaders = (string[])arrayList4.ToArray(typeof(string));
								}
							}
							else if (Rfc2616.Common.UnsafeAsciiLettersNoCaseEqual(ptr, i, length, "no-store"))
							{
								responseCacheControl.NoStore = true;
								i += 7;
							}
							else if (Rfc2616.Common.UnsafeAsciiLettersNoCaseEqual(ptr, i, length, "must-revalidate"))
							{
								responseCacheControl.MustRevalidate = true;
								i += 14;
							}
							else if (Rfc2616.Common.UnsafeAsciiLettersNoCaseEqual(ptr, i, length, "max-age"))
							{
								i += 7;
								while (i < length && ptr[i] == ' ')
								{
									i++;
								}
								if (i == length || ptr[(IntPtr)(i++) * 2] != '=')
								{
									return;
								}
								while (i < length && ptr[i] == ' ')
								{
									i++;
								}
								if (i == length)
								{
									return;
								}
								responseCacheControl.MaxAge = 0;
								while (i < length && ptr[i] >= '0' && ptr[i] <= '9')
								{
									responseCacheControl.MaxAge = responseCacheControl.MaxAge * 10 + (int)(ptr[(IntPtr)(i++) * 2] - '0');
								}
								i--;
							}
							else if (Rfc2616.Common.UnsafeAsciiLettersNoCaseEqual(ptr, i, length, "smax-age"))
							{
								i += 8;
								while (i < length && ptr[i] == ' ')
								{
									i++;
								}
								if (i == length || ptr[(IntPtr)(i++) * 2] != '=')
								{
									return;
								}
								while (i < length && ptr[i] == ' ')
								{
									i++;
								}
								if (i == length)
								{
									return;
								}
								responseCacheControl.SMaxAge = 0;
								while (i < length && ptr[i] >= '0' && ptr[i] <= '9')
								{
									responseCacheControl.SMaxAge = responseCacheControl.SMaxAge * 10 + (int)(ptr[(IntPtr)(i++) * 2] - '0');
								}
								i--;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000B1098 File Offset: 0x000B0098
		private void RemoveWarnings_1xx()
		{
			string[] values = this.CacheHeaders.GetValues("Warning");
			if (values == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			HttpRequestCacheValidator.ParseHeaderValues(values, HttpRequestCacheValidator.ParseWarningsCallback, arrayList);
			this.CacheHeaders.Remove("Warning");
			for (int i = 0; i < arrayList.Count; i++)
			{
				this.CacheHeaders.Add("Warning", (string)arrayList[i]);
			}
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000B1109 File Offset: 0x000B0109
		private static void ParseWarningsCallbackMethod(string s, int start, int end, IList list)
		{
			if (end >= start && s[start] != '1')
			{
				HttpRequestCacheValidator.ParseValuesCallbackMethod(s, start, end, list);
			}
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000B1123 File Offset: 0x000B0123
		private static void ParseValuesCallbackMethod(string s, int start, int end, IList list)
		{
			while (end >= start && s[end] == ' ')
			{
				end--;
			}
			if (end >= start)
			{
				list.Add(s.Substring(start, end - start + 1));
			}
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000B1154 File Offset: 0x000B0154
		internal static void ParseHeaderValues(string[] values, HttpRequestCacheValidator.ParseCallback calback, IList list)
		{
			if (values == null)
			{
				return;
			}
			foreach (string text in values)
			{
				int j = 0;
				int num = 0;
				while (j < text.Length)
				{
					while (num < text.Length && text[num] == ' ')
					{
						num++;
					}
					if (num != text.Length)
					{
						j = num;
						for (;;)
						{
							if (j >= text.Length || text[j] == ',' || text[j] == '"')
							{
								if (j == text.Length)
								{
									goto Block_6;
								}
								if (text[j] != '"')
								{
									break;
								}
								while (++j < text.Length && text[j] != '"')
								{
								}
								if (j == text.Length)
								{
									goto Block_8;
								}
							}
							else
							{
								j++;
							}
						}
						calback(text, num, j - 1, list);
						while (++j < text.Length && text[j] == ' ')
						{
						}
						if (j < text.Length)
						{
							num = j;
							continue;
						}
						break;
						Block_6:
						calback(text, num, j - 1, list);
						break;
						Block_8:
						calback(text, num, j - 1, list);
						break;
					}
					break;
				}
			}
		}

		// Token: 0x0400288C RID: 10380
		internal const string Warning_110 = "110 Response is stale";

		// Token: 0x0400288D RID: 10381
		internal const string Warning_111 = "111 Revalidation failed";

		// Token: 0x0400288E RID: 10382
		internal const string Warning_112 = "112 Disconnected operation";

		// Token: 0x0400288F RID: 10383
		internal const string Warning_113 = "113 Heuristic expiration";

		// Token: 0x04002890 RID: 10384
		private const long LO = 9007336695791648L;

		// Token: 0x04002891 RID: 10385
		private const int LOI = 2097184;

		// Token: 0x04002892 RID: 10386
		private const long _prox = 33777473954119792L;

		// Token: 0x04002893 RID: 10387
		private const long _y_re = 28429462276997241L;

		// Token: 0x04002894 RID: 10388
		private const long _vali = 29555336417443958L;

		// Token: 0x04002895 RID: 10389
		private const long _date = 28429470870339684L;

		// Token: 0x04002896 RID: 10390
		private const long _publ = 30399718399213680L;

		// Token: 0x04002897 RID: 10391
		private const int _ic = 6488169;

		// Token: 0x04002898 RID: 10392
		private const long _priv = 33214498230894704L;

		// Token: 0x04002899 RID: 10393
		private const int _at = 7602273;

		// Token: 0x0400289A RID: 10394
		private const long _no_c = 27866215975157870L;

		// Token: 0x0400289B RID: 10395
		private const long _ache = 28429419330863201L;

		// Token: 0x0400289C RID: 10396
		private const long _no_s = 32369815602528366L;

		// Token: 0x0400289D RID: 10397
		private const long _tore = 28429462281322612L;

		// Token: 0x0400289E RID: 10398
		private const long _must = 32651591227342957L;

		// Token: 0x0400289F RID: 10399
		private const long __rev = 33214481051025453L;

		// Token: 0x040028A0 RID: 10400
		private const long _alid = 28147948649709665L;

		// Token: 0x040028A1 RID: 10401
		private const long _max_ = 12666889354412141L;

		// Token: 0x040028A2 RID: 10402
		private const int _ag = 6750305;

		// Token: 0x040028A3 RID: 10403
		private const long _s_ma = 27303540895318131L;

		// Token: 0x040028A4 RID: 10404
		private const long _xage = 28429415035764856L;

		// Token: 0x040028A5 RID: 10405
		private HttpRequestCachePolicy m_HttpPolicy;

		// Token: 0x040028A6 RID: 10406
		private HttpStatusCode m_StatusCode;

		// Token: 0x040028A7 RID: 10407
		private string m_StatusDescription;

		// Token: 0x040028A8 RID: 10408
		private Version m_HttpVersion;

		// Token: 0x040028A9 RID: 10409
		private WebHeaderCollection m_Headers;

		// Token: 0x040028AA RID: 10410
		private NameValueCollection m_SystemMeta;

		// Token: 0x040028AB RID: 10411
		private bool m_DontUpdateHeaders;

		// Token: 0x040028AC RID: 10412
		private bool m_HeuristicExpiration;

		// Token: 0x040028AD RID: 10413
		private HttpRequestCacheValidator.Vars m_CacheVars;

		// Token: 0x040028AE RID: 10414
		private HttpRequestCacheValidator.Vars m_ResponseVars;

		// Token: 0x040028AF RID: 10415
		private HttpRequestCacheValidator.RequestVars m_RequestVars;

		// Token: 0x040028B0 RID: 10416
		private static readonly HttpRequestCacheValidator.ParseCallback ParseWarningsCallback = new HttpRequestCacheValidator.ParseCallback(HttpRequestCacheValidator.ParseWarningsCallbackMethod);

		// Token: 0x040028B1 RID: 10417
		internal static readonly HttpRequestCacheValidator.ParseCallback ParseValuesCallback = new HttpRequestCacheValidator.ParseCallback(HttpRequestCacheValidator.ParseValuesCallbackMethod);

		// Token: 0x0200055C RID: 1372
		private struct RequestVars
		{
			// Token: 0x040028B2 RID: 10418
			internal HttpMethod Method;

			// Token: 0x040028B3 RID: 10419
			internal bool IsCacheRange;

			// Token: 0x040028B4 RID: 10420
			internal bool IsUserRange;

			// Token: 0x040028B5 RID: 10421
			internal string IfHeader1;

			// Token: 0x040028B6 RID: 10422
			internal string Validator1;

			// Token: 0x040028B7 RID: 10423
			internal string IfHeader2;

			// Token: 0x040028B8 RID: 10424
			internal string Validator2;
		}

		// Token: 0x0200055D RID: 1373
		private struct Vars
		{
			// Token: 0x06002A16 RID: 10774 RVA: 0x000B1284 File Offset: 0x000B0284
			internal void Initialize()
			{
				this.EntityLength = (this.RangeStart = (this.RangeEnd = -1L));
				this.Date = DateTime.MinValue;
				this.Expires = DateTime.MinValue;
				this.LastModified = DateTime.MinValue;
				this.Age = TimeSpan.MinValue;
				this.MaxAge = TimeSpan.MinValue;
			}

			// Token: 0x040028B9 RID: 10425
			internal DateTime Date;

			// Token: 0x040028BA RID: 10426
			internal DateTime Expires;

			// Token: 0x040028BB RID: 10427
			internal DateTime LastModified;

			// Token: 0x040028BC RID: 10428
			internal long EntityLength;

			// Token: 0x040028BD RID: 10429
			internal TimeSpan Age;

			// Token: 0x040028BE RID: 10430
			internal TimeSpan MaxAge;

			// Token: 0x040028BF RID: 10431
			internal ResponseCacheControl CacheControl;

			// Token: 0x040028C0 RID: 10432
			internal long RangeStart;

			// Token: 0x040028C1 RID: 10433
			internal long RangeEnd;
		}

		// Token: 0x0200055E RID: 1374
		// (Invoke) Token: 0x06002A18 RID: 10776
		internal delegate void ParseCallback(string s, int start, int end, IList list);
	}
}
