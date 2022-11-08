using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x02000562 RID: 1378
	internal abstract class RequestCache
	{
		// Token: 0x06002A2C RID: 10796 RVA: 0x000B23FB File Offset: 0x000B13FB
		protected RequestCache(bool isPrivateCache, bool canWrite)
		{
			this._IsPrivateCache = isPrivateCache;
			this._CanWrite = canWrite;
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002A2D RID: 10797 RVA: 0x000B2411 File Offset: 0x000B1411
		internal bool IsPrivateCache
		{
			get
			{
				return this._IsPrivateCache;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002A2E RID: 10798 RVA: 0x000B2419 File Offset: 0x000B1419
		internal bool CanWrite
		{
			get
			{
				return this._CanWrite;
			}
		}

		// Token: 0x06002A2F RID: 10799
		internal abstract Stream Retrieve(string key, out RequestCacheEntry cacheEntry);

		// Token: 0x06002A30 RID: 10800
		internal abstract Stream Store(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06002A31 RID: 10801
		internal abstract void Remove(string key);

		// Token: 0x06002A32 RID: 10802
		internal abstract void Update(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06002A33 RID: 10803
		internal abstract bool TryRetrieve(string key, out RequestCacheEntry cacheEntry, out Stream readStream);

		// Token: 0x06002A34 RID: 10804
		internal abstract bool TryStore(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, out Stream writeStream);

		// Token: 0x06002A35 RID: 10805
		internal abstract bool TryRemove(string key);

		// Token: 0x06002A36 RID: 10806
		internal abstract bool TryUpdate(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06002A37 RID: 10807
		internal abstract void UnlockEntry(Stream retrieveStream);

		// Token: 0x040028D8 RID: 10456
		internal static readonly char[] LineSplits = new char[]
		{
			'\r',
			'\n'
		};

		// Token: 0x040028D9 RID: 10457
		private bool _IsPrivateCache;

		// Token: 0x040028DA RID: 10458
		private bool _CanWrite;
	}
}
