using System;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x0200055A RID: 1370
	internal abstract class RequestCacheValidator
	{
		// Token: 0x060029A6 RID: 10662
		internal abstract RequestCacheValidator CreateValidator();

		// Token: 0x060029A7 RID: 10663 RVA: 0x000AEE7D File Offset: 0x000ADE7D
		protected RequestCacheValidator(bool strictCacheErrors, TimeSpan unspecifiedMaxAge)
		{
			this._StrictCacheErrors = strictCacheErrors;
			this._UnspecifiedMaxAge = unspecifiedMaxAge;
			this._ValidationStatus = CacheValidationStatus.DoNotUseCache;
			this._CacheFreshnessStatus = CacheFreshnessStatus.Undefined;
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060029A8 RID: 10664 RVA: 0x000AEEA1 File Offset: 0x000ADEA1
		internal bool StrictCacheErrors
		{
			get
			{
				return this._StrictCacheErrors;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060029A9 RID: 10665 RVA: 0x000AEEA9 File Offset: 0x000ADEA9
		internal TimeSpan UnspecifiedMaxAge
		{
			get
			{
				return this._UnspecifiedMaxAge;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x000AEEB1 File Offset: 0x000ADEB1
		protected internal Uri Uri
		{
			get
			{
				return this._Uri;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x000AEEB9 File Offset: 0x000ADEB9
		protected internal WebRequest Request
		{
			get
			{
				return this._Request;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x000AEEC1 File Offset: 0x000ADEC1
		protected internal WebResponse Response
		{
			get
			{
				return this._Response;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060029AD RID: 10669 RVA: 0x000AEEC9 File Offset: 0x000ADEC9
		protected internal RequestCachePolicy Policy
		{
			get
			{
				return this._Policy;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x000AEED1 File Offset: 0x000ADED1
		protected internal int ResponseCount
		{
			get
			{
				return this._ResponseCount;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060029AF RID: 10671 RVA: 0x000AEED9 File Offset: 0x000ADED9
		protected internal CacheValidationStatus ValidationStatus
		{
			get
			{
				return this._ValidationStatus;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x000AEEE1 File Offset: 0x000ADEE1
		protected internal CacheFreshnessStatus CacheFreshnessStatus
		{
			get
			{
				return this._CacheFreshnessStatus;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x000AEEE9 File Offset: 0x000ADEE9
		protected internal RequestCacheEntry CacheEntry
		{
			get
			{
				return this._CacheEntry;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x000AEEF1 File Offset: 0x000ADEF1
		// (set) Token: 0x060029B3 RID: 10675 RVA: 0x000AEEF9 File Offset: 0x000ADEF9
		protected internal Stream CacheStream
		{
			get
			{
				return this._CacheStream;
			}
			set
			{
				this._CacheStream = value;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000AEF02 File Offset: 0x000ADF02
		// (set) Token: 0x060029B5 RID: 10677 RVA: 0x000AEF0A File Offset: 0x000ADF0A
		protected internal long CacheStreamOffset
		{
			get
			{
				return this._CacheStreamOffset;
			}
			set
			{
				this._CacheStreamOffset = value;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x000AEF13 File Offset: 0x000ADF13
		// (set) Token: 0x060029B7 RID: 10679 RVA: 0x000AEF1B File Offset: 0x000ADF1B
		protected internal long CacheStreamLength
		{
			get
			{
				return this._CacheStreamLength;
			}
			set
			{
				this._CacheStreamLength = value;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x000AEF24 File Offset: 0x000ADF24
		protected internal string CacheKey
		{
			get
			{
				return this._CacheKey;
			}
		}

		// Token: 0x060029B9 RID: 10681
		protected internal abstract CacheValidationStatus ValidateRequest();

		// Token: 0x060029BA RID: 10682
		protected internal abstract CacheFreshnessStatus ValidateFreshness();

		// Token: 0x060029BB RID: 10683
		protected internal abstract CacheValidationStatus ValidateCache();

		// Token: 0x060029BC RID: 10684
		protected internal abstract CacheValidationStatus ValidateResponse();

		// Token: 0x060029BD RID: 10685
		protected internal abstract CacheValidationStatus RevalidateCache();

		// Token: 0x060029BE RID: 10686
		protected internal abstract CacheValidationStatus UpdateCache();

		// Token: 0x060029BF RID: 10687 RVA: 0x000AEF2C File Offset: 0x000ADF2C
		protected internal virtual void FailRequest(WebExceptionStatus webStatus)
		{
			if (Logging.On)
			{
				Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_failing_request_with_exception", new object[]
				{
					webStatus.ToString()
				}));
			}
			if (webStatus == WebExceptionStatus.CacheEntryNotFound)
			{
				throw ExceptionHelper.CacheEntryNotFoundException;
			}
			if (webStatus == WebExceptionStatus.RequestProhibitedByCachePolicy)
			{
				throw ExceptionHelper.RequestProhibitedByCachePolicyException;
			}
			throw new WebException(NetRes.GetWebStatusString("net_requestaborted", webStatus), webStatus);
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x000AEF94 File Offset: 0x000ADF94
		internal void FetchRequest(Uri uri, WebRequest request)
		{
			this._Request = request;
			this._Policy = request.CachePolicy;
			this._Response = null;
			this._ResponseCount = 0;
			this._ValidationStatus = CacheValidationStatus.DoNotUseCache;
			this._CacheFreshnessStatus = CacheFreshnessStatus.Undefined;
			this._CacheStream = null;
			this._CacheStreamOffset = 0L;
			this._CacheStreamLength = 0L;
			if (!uri.Equals(this._Uri))
			{
				this._CacheKey = uri.GetParts(UriComponents.AbsoluteUri, UriFormat.Unescaped);
			}
			this._Uri = uri;
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000AF00B File Offset: 0x000AE00B
		internal void FetchCacheEntry(RequestCacheEntry fetchEntry)
		{
			this._CacheEntry = fetchEntry;
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000AF014 File Offset: 0x000AE014
		internal void FetchResponse(WebResponse fetchResponse)
		{
			this._ResponseCount++;
			this._Response = fetchResponse;
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x000AF02B File Offset: 0x000AE02B
		internal void SetFreshnessStatus(CacheFreshnessStatus status)
		{
			this._CacheFreshnessStatus = status;
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000AF034 File Offset: 0x000AE034
		internal void SetValidationStatus(CacheValidationStatus status)
		{
			this._ValidationStatus = status;
		}

		// Token: 0x0400287E RID: 10366
		internal WebRequest _Request;

		// Token: 0x0400287F RID: 10367
		internal WebResponse _Response;

		// Token: 0x04002880 RID: 10368
		internal Stream _CacheStream;

		// Token: 0x04002881 RID: 10369
		private RequestCachePolicy _Policy;

		// Token: 0x04002882 RID: 10370
		private Uri _Uri;

		// Token: 0x04002883 RID: 10371
		private string _CacheKey;

		// Token: 0x04002884 RID: 10372
		private RequestCacheEntry _CacheEntry;

		// Token: 0x04002885 RID: 10373
		private int _ResponseCount;

		// Token: 0x04002886 RID: 10374
		private CacheValidationStatus _ValidationStatus;

		// Token: 0x04002887 RID: 10375
		private CacheFreshnessStatus _CacheFreshnessStatus;

		// Token: 0x04002888 RID: 10376
		private long _CacheStreamOffset;

		// Token: 0x04002889 RID: 10377
		private long _CacheStreamLength;

		// Token: 0x0400288A RID: 10378
		private bool _StrictCacheErrors;

		// Token: 0x0400288B RID: 10379
		private TimeSpan _UnspecifiedMaxAge;
	}
}
