using System;

namespace System.Net.Cache
{
	// Token: 0x02000568 RID: 1384
	internal class RequestCacheBinding
	{
		// Token: 0x06002A81 RID: 10881 RVA: 0x000B4C58 File Offset: 0x000B3C58
		internal RequestCacheBinding(RequestCache requestCache, RequestCacheValidator cacheValidator, RequestCachePolicy policy)
		{
			this.m_RequestCache = requestCache;
			this.m_CacheValidator = cacheValidator;
			this.m_Policy = policy;
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x000B4C75 File Offset: 0x000B3C75
		internal RequestCache Cache
		{
			get
			{
				return this.m_RequestCache;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002A83 RID: 10883 RVA: 0x000B4C7D File Offset: 0x000B3C7D
		internal RequestCacheValidator Validator
		{
			get
			{
				return this.m_CacheValidator;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x000B4C85 File Offset: 0x000B3C85
		internal RequestCachePolicy Policy
		{
			get
			{
				return this.m_Policy;
			}
		}

		// Token: 0x04002903 RID: 10499
		private RequestCache m_RequestCache;

		// Token: 0x04002904 RID: 10500
		private RequestCacheValidator m_CacheValidator;

		// Token: 0x04002905 RID: 10501
		private RequestCachePolicy m_Policy;
	}
}
