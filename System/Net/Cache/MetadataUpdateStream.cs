using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200057C RID: 1404
	internal class MetadataUpdateStream : Stream, ICloseEx
	{
		// Token: 0x06002ADD RID: 10973 RVA: 0x000B6A28 File Offset: 0x000B5A28
		internal MetadataUpdateStream(Stream parentStream, RequestCache cache, string key, DateTime expiresGMT, DateTime lastModifiedGMT, DateTime lastSynchronizedGMT, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, bool isStrictCacheErrors)
		{
			if (parentStream == null)
			{
				throw new ArgumentNullException("parentStream");
			}
			this.m_ParentStream = parentStream;
			this.m_Cache = cache;
			this.m_Key = key;
			this.m_Expires = expiresGMT;
			this.m_LastModified = lastModifiedGMT;
			this.m_LastSynchronized = lastSynchronizedGMT;
			this.m_MaxStale = maxStale;
			this.m_EntryMetadata = entryMetadata;
			this.m_SystemMetadata = systemMetadata;
			this.m_IsStrictCacheErrors = isStrictCacheErrors;
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000B6A96 File Offset: 0x000B5A96
		private MetadataUpdateStream(Stream parentStream, RequestCache cache, string key, bool isStrictCacheErrors)
		{
			if (parentStream == null)
			{
				throw new ArgumentNullException("parentStream");
			}
			this.m_ParentStream = parentStream;
			this.m_Cache = cache;
			this.m_Key = key;
			this.m_CacheDestroy = true;
			this.m_IsStrictCacheErrors = isStrictCacheErrors;
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000B6AD0 File Offset: 0x000B5AD0
		public override bool CanRead
		{
			get
			{
				return this.m_ParentStream.CanRead;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002AE0 RID: 10976 RVA: 0x000B6ADD File Offset: 0x000B5ADD
		public override bool CanSeek
		{
			get
			{
				return this.m_ParentStream.CanSeek;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x000B6AEA File Offset: 0x000B5AEA
		public override bool CanWrite
		{
			get
			{
				return this.m_ParentStream.CanWrite;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x000B6AF7 File Offset: 0x000B5AF7
		public override long Length
		{
			get
			{
				return this.m_ParentStream.Length;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x000B6B04 File Offset: 0x000B5B04
		// (set) Token: 0x06002AE4 RID: 10980 RVA: 0x000B6B11 File Offset: 0x000B5B11
		public override long Position
		{
			get
			{
				return this.m_ParentStream.Position;
			}
			set
			{
				this.m_ParentStream.Position = value;
			}
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000B6B1F File Offset: 0x000B5B1F
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.m_ParentStream.Seek(offset, origin);
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000B6B2E File Offset: 0x000B5B2E
		public override void SetLength(long value)
		{
			this.m_ParentStream.SetLength(value);
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x000B6B3C File Offset: 0x000B5B3C
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.m_ParentStream.Write(buffer, offset, count);
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000B6B4C File Offset: 0x000B5B4C
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.m_ParentStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000B6B60 File Offset: 0x000B5B60
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_ParentStream.EndWrite(asyncResult);
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000B6B6E File Offset: 0x000B5B6E
		public override void Flush()
		{
			this.m_ParentStream.Flush();
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000B6B7B File Offset: 0x000B5B7B
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.m_ParentStream.Read(buffer, offset, count);
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x000B6B8B File Offset: 0x000B5B8B
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.m_ParentStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000B6B9F File Offset: 0x000B5B9F
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.m_ParentStream.EndRead(asyncResult);
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x000B6BAD File Offset: 0x000B5BAD
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x000B6BBD File Offset: 0x000B5BBD
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.Dispose(true, closeState);
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000B6BC7 File Offset: 0x000B5BC7
		public override bool CanTimeout
		{
			get
			{
				return this.m_ParentStream.CanTimeout;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x000B6BD4 File Offset: 0x000B5BD4
		// (set) Token: 0x06002AF2 RID: 10994 RVA: 0x000B6BE1 File Offset: 0x000B5BE1
		public override int ReadTimeout
		{
			get
			{
				return this.m_ParentStream.ReadTimeout;
			}
			set
			{
				this.m_ParentStream.ReadTimeout = value;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x000B6BEF File Offset: 0x000B5BEF
		// (set) Token: 0x06002AF4 RID: 10996 RVA: 0x000B6BFC File Offset: 0x000B5BFC
		public override int WriteTimeout
		{
			get
			{
				return this.m_ParentStream.WriteTimeout;
			}
			set
			{
				this.m_ParentStream.WriteTimeout = value;
			}
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x000B6C0C File Offset: 0x000B5C0C
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			if (Interlocked.Increment(ref this._Disposed) == 1)
			{
				ICloseEx closeEx = this.m_ParentStream as ICloseEx;
				if (closeEx != null)
				{
					closeEx.CloseEx(closeState);
				}
				else
				{
					this.m_ParentStream.Close();
				}
				if (this.m_CacheDestroy)
				{
					if (this.m_IsStrictCacheErrors)
					{
						this.m_Cache.Remove(this.m_Key);
					}
					else
					{
						this.m_Cache.TryRemove(this.m_Key);
					}
				}
				else if (this.m_IsStrictCacheErrors)
				{
					this.m_Cache.Update(this.m_Key, this.m_Expires, this.m_LastModified, this.m_LastSynchronized, this.m_MaxStale, this.m_EntryMetadata, this.m_SystemMetadata);
				}
				else
				{
					this.m_Cache.TryUpdate(this.m_Key, this.m_Expires, this.m_LastModified, this.m_LastSynchronized, this.m_MaxStale, this.m_EntryMetadata, this.m_SystemMetadata);
				}
				if (!disposing)
				{
					this.m_Cache = null;
					this.m_Key = null;
					this.m_EntryMetadata = null;
					this.m_SystemMetadata = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400298D RID: 10637
		private Stream m_ParentStream;

		// Token: 0x0400298E RID: 10638
		private RequestCache m_Cache;

		// Token: 0x0400298F RID: 10639
		private string m_Key;

		// Token: 0x04002990 RID: 10640
		private DateTime m_Expires;

		// Token: 0x04002991 RID: 10641
		private DateTime m_LastModified;

		// Token: 0x04002992 RID: 10642
		private DateTime m_LastSynchronized;

		// Token: 0x04002993 RID: 10643
		private TimeSpan m_MaxStale;

		// Token: 0x04002994 RID: 10644
		private StringCollection m_EntryMetadata;

		// Token: 0x04002995 RID: 10645
		private StringCollection m_SystemMetadata;

		// Token: 0x04002996 RID: 10646
		private bool m_CacheDestroy;

		// Token: 0x04002997 RID: 10647
		private bool m_IsStrictCacheErrors;

		// Token: 0x04002998 RID: 10648
		private int _Disposed;
	}
}
