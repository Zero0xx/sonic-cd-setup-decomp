using System;
using System.Configuration;
using System.Net.Cache;
using System.Threading;
using Microsoft.Win32;

namespace System.Net.Configuration
{
	// Token: 0x0200065B RID: 1627
	internal sealed class RequestCachingSectionInternal
	{
		// Token: 0x0600323F RID: 12863 RVA: 0x000D5F40 File Offset: 0x000D4F40
		private RequestCachingSectionInternal()
		{
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x000D5F48 File Offset: 0x000D4F48
		internal RequestCachingSectionInternal(RequestCachingSection section)
		{
			if (!section.DisableAllCaching)
			{
				this.defaultCachePolicy = new RequestCachePolicy(section.DefaultPolicyLevel);
				this.isPrivateCache = section.IsPrivateCache;
				this.unspecifiedMaximumAge = section.UnspecifiedMaximumAge;
			}
			else
			{
				this.disableAllCaching = true;
			}
			this.httpRequestCacheValidator = new HttpRequestCacheValidator(false, this.UnspecifiedMaximumAge);
			this.ftpRequestCacheValidator = new FtpRequestCacheValidator(false, this.UnspecifiedMaximumAge);
			this.defaultCache = new WinInetCache(this.IsPrivateCache, true, true);
			if (section.DisableAllCaching)
			{
				return;
			}
			HttpCachePolicyElement httpCachePolicyElement = section.DefaultHttpCachePolicy;
			if (httpCachePolicyElement.WasReadFromConfig)
			{
				if (httpCachePolicyElement.PolicyLevel == HttpRequestCacheLevel.Default)
				{
					HttpCacheAgeControl cacheAgeControl = (httpCachePolicyElement.MinimumFresh != TimeSpan.MinValue) ? HttpCacheAgeControl.MaxAgeAndMinFresh : HttpCacheAgeControl.MaxAgeAndMaxStale;
					this.defaultHttpCachePolicy = new HttpRequestCachePolicy(cacheAgeControl, httpCachePolicyElement.MaximumAge, (httpCachePolicyElement.MinimumFresh != TimeSpan.MinValue) ? httpCachePolicyElement.MinimumFresh : httpCachePolicyElement.MaximumStale);
				}
				else
				{
					this.defaultHttpCachePolicy = new HttpRequestCachePolicy(httpCachePolicyElement.PolicyLevel);
				}
			}
			FtpCachePolicyElement ftpCachePolicyElement = section.DefaultFtpCachePolicy;
			if (ftpCachePolicyElement.WasReadFromConfig)
			{
				this.defaultFtpCachePolicy = new RequestCachePolicy(ftpCachePolicyElement.PolicyLevel);
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x000D6068 File Offset: 0x000D5068
		internal static object ClassSyncObject
		{
			get
			{
				if (RequestCachingSectionInternal.classSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref RequestCachingSectionInternal.classSyncObject, value, null);
				}
				return RequestCachingSectionInternal.classSyncObject;
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06003242 RID: 12866 RVA: 0x000D6094 File Offset: 0x000D5094
		internal bool DisableAllCaching
		{
			get
			{
				return this.disableAllCaching;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x000D609C File Offset: 0x000D509C
		internal RequestCache DefaultCache
		{
			get
			{
				return this.defaultCache;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06003244 RID: 12868 RVA: 0x000D60A4 File Offset: 0x000D50A4
		internal RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				return this.defaultCachePolicy;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x000D60AC File Offset: 0x000D50AC
		internal bool IsPrivateCache
		{
			get
			{
				return this.isPrivateCache;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06003246 RID: 12870 RVA: 0x000D60B4 File Offset: 0x000D50B4
		internal TimeSpan UnspecifiedMaximumAge
		{
			get
			{
				return this.unspecifiedMaximumAge;
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x000D60BC File Offset: 0x000D50BC
		internal HttpRequestCachePolicy DefaultHttpCachePolicy
		{
			get
			{
				return this.defaultHttpCachePolicy;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06003248 RID: 12872 RVA: 0x000D60C4 File Offset: 0x000D50C4
		internal RequestCachePolicy DefaultFtpCachePolicy
		{
			get
			{
				return this.defaultFtpCachePolicy;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06003249 RID: 12873 RVA: 0x000D60CC File Offset: 0x000D50CC
		internal HttpRequestCacheValidator DefaultHttpValidator
		{
			get
			{
				return this.httpRequestCacheValidator;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x0600324A RID: 12874 RVA: 0x000D60D4 File Offset: 0x000D50D4
		internal FtpRequestCacheValidator DefaultFtpValidator
		{
			get
			{
				return this.ftpRequestCacheValidator;
			}
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x000D60DC File Offset: 0x000D50DC
		internal static RequestCachingSectionInternal GetSection()
		{
			RequestCachingSectionInternal result;
			lock (RequestCachingSectionInternal.ClassSyncObject)
			{
				RequestCachingSection requestCachingSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.RequestCachingSectionPath) as RequestCachingSection;
				if (requestCachingSection == null)
				{
					result = null;
				}
				else
				{
					try
					{
						result = new RequestCachingSectionInternal(requestCachingSection);
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_requestcaching"), ex);
					}
					catch
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_requestcaching"), new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}
			return result;
		}

		// Token: 0x04002F1B RID: 12059
		private static object classSyncObject;

		// Token: 0x04002F1C RID: 12060
		private RequestCache defaultCache;

		// Token: 0x04002F1D RID: 12061
		private HttpRequestCachePolicy defaultHttpCachePolicy;

		// Token: 0x04002F1E RID: 12062
		private RequestCachePolicy defaultFtpCachePolicy;

		// Token: 0x04002F1F RID: 12063
		private RequestCachePolicy defaultCachePolicy;

		// Token: 0x04002F20 RID: 12064
		private bool disableAllCaching;

		// Token: 0x04002F21 RID: 12065
		private HttpRequestCacheValidator httpRequestCacheValidator;

		// Token: 0x04002F22 RID: 12066
		private FtpRequestCacheValidator ftpRequestCacheValidator;

		// Token: 0x04002F23 RID: 12067
		private bool isPrivateCache;

		// Token: 0x04002F24 RID: 12068
		private TimeSpan unspecifiedMaximumAge;
	}
}
