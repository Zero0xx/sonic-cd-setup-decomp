using System;
using System.Globalization;

namespace System.Net.Cache
{
	// Token: 0x0200056D RID: 1389
	public class HttpRequestCachePolicy : RequestCachePolicy
	{
		// Token: 0x06002A89 RID: 10889 RVA: 0x000B4CDC File Offset: 0x000B3CDC
		public HttpRequestCachePolicy() : this(HttpRequestCacheLevel.Default)
		{
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000B4CE8 File Offset: 0x000B3CE8
		public HttpRequestCachePolicy(HttpRequestCacheLevel level) : base(HttpRequestCachePolicy.MapLevel(level))
		{
			this.m_Level = level;
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000B4D34 File Offset: 0x000B3D34
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan ageOrFreshOrStale) : this(HttpRequestCacheLevel.Default)
		{
			switch (cacheAgeControl)
			{
			case HttpCacheAgeControl.MinFresh:
				this.m_MinFresh = ageOrFreshOrStale;
				return;
			case HttpCacheAgeControl.MaxAge:
				this.m_MaxAge = ageOrFreshOrStale;
				return;
			case HttpCacheAgeControl.MaxStale:
				this.m_MaxStale = ageOrFreshOrStale;
				return;
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
			{
				"HttpCacheAgeControl"
			}), "cacheAgeControl");
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000B4DA0 File Offset: 0x000B3DA0
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale) : this(HttpRequestCacheLevel.Default)
		{
			switch (cacheAgeControl)
			{
			case HttpCacheAgeControl.MinFresh:
				this.m_MinFresh = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxAge:
				this.m_MaxAge = maxAge;
				return;
			case HttpCacheAgeControl.MaxAgeAndMinFresh:
				this.m_MaxAge = maxAge;
				this.m_MinFresh = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxStale:
				this.m_MaxStale = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxAgeAndMaxStale:
				this.m_MaxAge = maxAge;
				this.m_MaxStale = freshOrStale;
				return;
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
			{
				"HttpCacheAgeControl"
			}), "cacheAgeControl");
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000B4E32 File Offset: 0x000B3E32
		public HttpRequestCachePolicy(DateTime cacheSyncDate) : this(HttpRequestCacheLevel.Default)
		{
			this.m_LastSyncDateUtc = cacheSyncDate.ToUniversalTime();
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000B4E48 File Offset: 0x000B3E48
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale, DateTime cacheSyncDate) : this(cacheAgeControl, maxAge, freshOrStale)
		{
			this.m_LastSyncDateUtc = cacheSyncDate.ToUniversalTime();
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002A8F RID: 10895 RVA: 0x000B4E60 File Offset: 0x000B3E60
		public new HttpRequestCacheLevel Level
		{
			get
			{
				return this.m_Level;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000B4E68 File Offset: 0x000B3E68
		public DateTime CacheSyncDate
		{
			get
			{
				if (this.m_LastSyncDateUtc == DateTime.MinValue || this.m_LastSyncDateUtc == DateTime.MaxValue)
				{
					return this.m_LastSyncDateUtc;
				}
				return this.m_LastSyncDateUtc.ToLocalTime();
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002A91 RID: 10897 RVA: 0x000B4EA0 File Offset: 0x000B3EA0
		internal DateTime InternalCacheSyncDateUtc
		{
			get
			{
				return this.m_LastSyncDateUtc;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06002A92 RID: 10898 RVA: 0x000B4EA8 File Offset: 0x000B3EA8
		public TimeSpan MaxAge
		{
			get
			{
				return this.m_MaxAge;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002A93 RID: 10899 RVA: 0x000B4EB0 File Offset: 0x000B3EB0
		public TimeSpan MinFresh
		{
			get
			{
				return this.m_MinFresh;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002A94 RID: 10900 RVA: 0x000B4EB8 File Offset: 0x000B3EB8
		public TimeSpan MaxStale
		{
			get
			{
				return this.m_MaxStale;
			}
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000B4EC0 File Offset: 0x000B3EC0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Level:",
				this.m_Level.ToString(),
				(this.m_MaxAge == TimeSpan.MaxValue) ? string.Empty : (" MaxAge:" + this.m_MaxAge.ToString()),
				(this.m_MinFresh == TimeSpan.MinValue) ? string.Empty : (" MinFresh:" + this.m_MinFresh.ToString()),
				(this.m_MaxStale == TimeSpan.MinValue) ? string.Empty : (" MaxStale:" + this.m_MaxStale.ToString()),
				(this.CacheSyncDate == DateTime.MinValue) ? string.Empty : (" CacheSyncDate:" + this.CacheSyncDate.ToString(CultureInfo.CurrentCulture))
			});
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000B4FD3 File Offset: 0x000B3FD3
		private static RequestCacheLevel MapLevel(HttpRequestCacheLevel level)
		{
			if (level <= HttpRequestCacheLevel.NoCacheNoStore)
			{
				return (RequestCacheLevel)level;
			}
			if (level == HttpRequestCacheLevel.CacheOrNextCacheOnly)
			{
				return RequestCacheLevel.CacheOnly;
			}
			if (level == HttpRequestCacheLevel.Refresh)
			{
				return RequestCacheLevel.Reload;
			}
			throw new ArgumentOutOfRangeException("level");
		}

		// Token: 0x04002920 RID: 10528
		internal static readonly HttpRequestCachePolicy BypassCache = new HttpRequestCachePolicy(HttpRequestCacheLevel.BypassCache);

		// Token: 0x04002921 RID: 10529
		private HttpRequestCacheLevel m_Level;

		// Token: 0x04002922 RID: 10530
		private DateTime m_LastSyncDateUtc = DateTime.MinValue;

		// Token: 0x04002923 RID: 10531
		private TimeSpan m_MaxAge = TimeSpan.MaxValue;

		// Token: 0x04002924 RID: 10532
		private TimeSpan m_MinFresh = TimeSpan.MinValue;

		// Token: 0x04002925 RID: 10533
		private TimeSpan m_MaxStale = TimeSpan.MinValue;
	}
}
