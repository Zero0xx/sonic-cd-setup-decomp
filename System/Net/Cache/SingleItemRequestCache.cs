using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Win32;

namespace System.Net.Cache
{
	// Token: 0x02000582 RID: 1410
	internal class SingleItemRequestCache : WinInetCache
	{
		// Token: 0x06002B3E RID: 11070 RVA: 0x000BBD45 File Offset: 0x000BAD45
		internal SingleItemRequestCache(bool useWinInet) : base(true, true, false)
		{
			this._UseWinInet = useWinInet;
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000BBD58 File Offset: 0x000BAD58
		internal override Stream Retrieve(string key, out RequestCacheEntry cacheEntry)
		{
			Stream result;
			if (!this.TryRetrieve(key, out cacheEntry, out result))
			{
				FileNotFoundException ex = new FileNotFoundException(null, key);
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
				{
					ex.Message
				}), ex);
			}
			return result;
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000BBD9C File Offset: 0x000BAD9C
		internal override Stream Store(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			Stream result;
			if (!this.TryStore(key, contentLength, expiresUtc, lastModifiedUtc, maxStale, entryMetadata, systemMetadata, out result))
			{
				FileNotFoundException ex = new FileNotFoundException(null, key);
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
				{
					ex.Message
				}), ex);
			}
			return result;
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000BBDEC File Offset: 0x000BADEC
		internal override void Remove(string key)
		{
			if (!this.TryRemove(key))
			{
				FileNotFoundException ex = new FileNotFoundException(null, key);
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
				{
					ex.Message
				}), ex);
			}
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000BBE2C File Offset: 0x000BAE2C
		internal override void Update(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			if (!this.TryUpdate(key, expiresUtc, lastModifiedUtc, lastSynchronizedUtc, maxStale, entryMetadata, systemMetadata))
			{
				FileNotFoundException ex = new FileNotFoundException(null, key);
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
				{
					ex.Message
				}), ex);
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000BBE78 File Offset: 0x000BAE78
		internal override bool TryRetrieve(string key, out RequestCacheEntry cacheEntry, out Stream readStream)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			SingleItemRequestCache.FrozenCacheEntry frozenCacheEntry = this._Entry;
			cacheEntry = null;
			readStream = null;
			if (frozenCacheEntry == null || frozenCacheEntry.Key != key)
			{
				RequestCacheEntry entry;
				Stream stream;
				if (!this._UseWinInet || !base.TryRetrieve(key, out entry, out stream))
				{
					return false;
				}
				frozenCacheEntry = new SingleItemRequestCache.FrozenCacheEntry(key, entry, stream);
				stream.Close();
				this._Entry = frozenCacheEntry;
			}
			cacheEntry = SingleItemRequestCache.FrozenCacheEntry.Create(frozenCacheEntry);
			readStream = new SingleItemRequestCache.ReadOnlyStream(frozenCacheEntry.StreamBytes);
			return true;
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000BBEF4 File Offset: 0x000BAEF4
		internal override bool TryStore(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, out Stream writeStream)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			RequestCacheEntry requestCacheEntry = new RequestCacheEntry();
			requestCacheEntry.IsPrivateEntry = base.IsPrivateCache;
			requestCacheEntry.StreamSize = contentLength;
			requestCacheEntry.ExpiresUtc = expiresUtc;
			requestCacheEntry.LastModifiedUtc = lastModifiedUtc;
			requestCacheEntry.LastAccessedUtc = DateTime.UtcNow;
			requestCacheEntry.LastSynchronizedUtc = DateTime.UtcNow;
			requestCacheEntry.MaxStale = maxStale;
			requestCacheEntry.HitCount = 0;
			requestCacheEntry.UsageCount = 0;
			requestCacheEntry.IsPartialEntry = false;
			requestCacheEntry.EntryMetadata = entryMetadata;
			requestCacheEntry.SystemMetadata = systemMetadata;
			writeStream = null;
			Stream realWriteStream = null;
			if (this._UseWinInet)
			{
				base.TryStore(key, contentLength, expiresUtc, lastModifiedUtc, maxStale, entryMetadata, systemMetadata, out realWriteStream);
			}
			writeStream = new SingleItemRequestCache.WriteOnlyStream(key, this, requestCacheEntry, realWriteStream);
			return true;
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x000BBFAC File Offset: 0x000BAFAC
		private void Commit(string key, RequestCacheEntry tempEntry, byte[] allBytes)
		{
			SingleItemRequestCache.FrozenCacheEntry entry = new SingleItemRequestCache.FrozenCacheEntry(key, tempEntry, allBytes);
			this._Entry = entry;
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000BBFCC File Offset: 0x000BAFCC
		internal override bool TryRemove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this._UseWinInet)
			{
				base.TryRemove(key);
			}
			SingleItemRequestCache.FrozenCacheEntry entry = this._Entry;
			if (entry != null && entry.Key == key)
			{
				this._Entry = null;
			}
			return true;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000BC018 File Offset: 0x000BB018
		internal override bool TryUpdate(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			SingleItemRequestCache.FrozenCacheEntry frozenCacheEntry = SingleItemRequestCache.FrozenCacheEntry.Create(this._Entry);
			if (frozenCacheEntry == null || frozenCacheEntry.Key != key)
			{
				return true;
			}
			frozenCacheEntry.ExpiresUtc = expiresUtc;
			frozenCacheEntry.LastModifiedUtc = lastModifiedUtc;
			frozenCacheEntry.LastSynchronizedUtc = lastSynchronizedUtc;
			frozenCacheEntry.MaxStale = maxStale;
			frozenCacheEntry.EntryMetadata = entryMetadata;
			frozenCacheEntry.SystemMetadata = systemMetadata;
			this._Entry = frozenCacheEntry;
			return true;
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x000BC088 File Offset: 0x000BB088
		internal override void UnlockEntry(Stream stream)
		{
		}

		// Token: 0x040029AB RID: 10667
		private bool _UseWinInet;

		// Token: 0x040029AC RID: 10668
		private SingleItemRequestCache.FrozenCacheEntry _Entry;

		// Token: 0x02000583 RID: 1411
		private sealed class FrozenCacheEntry : RequestCacheEntry
		{
			// Token: 0x06002B49 RID: 11081 RVA: 0x000BC08A File Offset: 0x000BB08A
			public FrozenCacheEntry(string key, RequestCacheEntry entry, Stream stream) : this(key, entry, SingleItemRequestCache.FrozenCacheEntry.GetBytes(stream))
			{
			}

			// Token: 0x06002B4A RID: 11082 RVA: 0x000BC09C File Offset: 0x000BB09C
			public FrozenCacheEntry(string key, RequestCacheEntry entry, byte[] streamBytes)
			{
				this._Key = key;
				this._StreamBytes = streamBytes;
				base.IsPrivateEntry = entry.IsPrivateEntry;
				base.StreamSize = entry.StreamSize;
				base.ExpiresUtc = entry.ExpiresUtc;
				base.HitCount = entry.HitCount;
				base.LastAccessedUtc = entry.LastAccessedUtc;
				entry.LastModifiedUtc = entry.LastModifiedUtc;
				base.LastSynchronizedUtc = entry.LastSynchronizedUtc;
				base.MaxStale = entry.MaxStale;
				base.UsageCount = entry.UsageCount;
				base.IsPartialEntry = entry.IsPartialEntry;
				base.EntryMetadata = entry.EntryMetadata;
				base.SystemMetadata = entry.SystemMetadata;
			}

			// Token: 0x06002B4B RID: 11083 RVA: 0x000BC150 File Offset: 0x000BB150
			private static byte[] GetBytes(Stream stream)
			{
				bool flag = false;
				byte[] array;
				if (stream.CanSeek)
				{
					array = new byte[stream.Length];
				}
				else
				{
					flag = true;
					array = new byte[8192];
				}
				int num = 0;
				for (;;)
				{
					int num2 = stream.Read(array, num, array.Length - num);
					if (num2 == 0)
					{
						break;
					}
					if ((num += num2) == array.Length && flag)
					{
						byte[] array2 = new byte[array.Length + 8192];
						Buffer.BlockCopy(array, 0, array2, 0, num);
						array = array2;
					}
				}
				if (flag)
				{
					byte[] array3 = new byte[num];
					Buffer.BlockCopy(array, 0, array3, 0, num);
					array = array3;
				}
				return array;
			}

			// Token: 0x06002B4C RID: 11084 RVA: 0x000BC1DD File Offset: 0x000BB1DD
			public static SingleItemRequestCache.FrozenCacheEntry Create(SingleItemRequestCache.FrozenCacheEntry clonedObject)
			{
				if (clonedObject != null)
				{
					return (SingleItemRequestCache.FrozenCacheEntry)clonedObject.MemberwiseClone();
				}
				return null;
			}

			// Token: 0x170008F8 RID: 2296
			// (get) Token: 0x06002B4D RID: 11085 RVA: 0x000BC1EF File Offset: 0x000BB1EF
			public byte[] StreamBytes
			{
				get
				{
					return this._StreamBytes;
				}
			}

			// Token: 0x170008F9 RID: 2297
			// (get) Token: 0x06002B4E RID: 11086 RVA: 0x000BC1F7 File Offset: 0x000BB1F7
			public string Key
			{
				get
				{
					return this._Key;
				}
			}

			// Token: 0x040029AD RID: 10669
			private byte[] _StreamBytes;

			// Token: 0x040029AE RID: 10670
			private string _Key;
		}

		// Token: 0x02000584 RID: 1412
		internal class ReadOnlyStream : Stream
		{
			// Token: 0x06002B4F RID: 11087 RVA: 0x000BC200 File Offset: 0x000BB200
			internal ReadOnlyStream(byte[] bytes)
			{
				this._Bytes = bytes;
				this._Offset = 0;
				this._Disposed = false;
				this._ReadTimeout = (this._WriteTimeout = -1);
			}

			// Token: 0x170008FA RID: 2298
			// (get) Token: 0x06002B50 RID: 11088 RVA: 0x000BC238 File Offset: 0x000BB238
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008FB RID: 2299
			// (get) Token: 0x06002B51 RID: 11089 RVA: 0x000BC23B File Offset: 0x000BB23B
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008FC RID: 2300
			// (get) Token: 0x06002B52 RID: 11090 RVA: 0x000BC23E File Offset: 0x000BB23E
			public override bool CanTimeout
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008FD RID: 2301
			// (get) Token: 0x06002B53 RID: 11091 RVA: 0x000BC241 File Offset: 0x000BB241
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170008FE RID: 2302
			// (get) Token: 0x06002B54 RID: 11092 RVA: 0x000BC244 File Offset: 0x000BB244
			public override long Length
			{
				get
				{
					return (long)this._Bytes.Length;
				}
			}

			// Token: 0x170008FF RID: 2303
			// (get) Token: 0x06002B55 RID: 11093 RVA: 0x000BC24F File Offset: 0x000BB24F
			// (set) Token: 0x06002B56 RID: 11094 RVA: 0x000BC258 File Offset: 0x000BB258
			public override long Position
			{
				get
				{
					return (long)this._Offset;
				}
				set
				{
					if (value < 0L || value > (long)this._Bytes.Length)
					{
						throw new ArgumentOutOfRangeException("value");
					}
					this._Offset = (int)value;
				}
			}

			// Token: 0x17000900 RID: 2304
			// (get) Token: 0x06002B57 RID: 11095 RVA: 0x000BC27E File Offset: 0x000BB27E
			// (set) Token: 0x06002B58 RID: 11096 RVA: 0x000BC286 File Offset: 0x000BB286
			public override int ReadTimeout
			{
				get
				{
					return this._ReadTimeout;
				}
				set
				{
					if (value <= 0 && value != -1)
					{
						throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
					}
					this._ReadTimeout = value;
				}
			}

			// Token: 0x17000901 RID: 2305
			// (get) Token: 0x06002B59 RID: 11097 RVA: 0x000BC2A7 File Offset: 0x000BB2A7
			// (set) Token: 0x06002B5A RID: 11098 RVA: 0x000BC2AF File Offset: 0x000BB2AF
			public override int WriteTimeout
			{
				get
				{
					return this._WriteTimeout;
				}
				set
				{
					if (value <= 0 && value != -1)
					{
						throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
					}
					this._WriteTimeout = value;
				}
			}

			// Token: 0x06002B5B RID: 11099 RVA: 0x000BC2D0 File Offset: 0x000BB2D0
			public override void Flush()
			{
			}

			// Token: 0x06002B5C RID: 11100 RVA: 0x000BC2D4 File Offset: 0x000BB2D4
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				int num = this.Read(buffer, offset, count);
				LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(null, state, callback);
				lazyAsyncResult.InvokeCallback(num);
				return lazyAsyncResult;
			}

			// Token: 0x06002B5D RID: 11101 RVA: 0x000BC304 File Offset: 0x000BB304
			public override int EndRead(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
				if (lazyAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
					{
						"EndRead"
					}));
				}
				lazyAsyncResult.EndCalled = true;
				return (int)lazyAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x06002B5E RID: 11102 RVA: 0x000BC360 File Offset: 0x000BB360
			public override int Read(byte[] buffer, int offset, int count)
			{
				if (this._Disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (offset < 0 || offset > buffer.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (count < 0 || count > buffer.Length - offset)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (this._Offset == this._Bytes.Length)
				{
					return 0;
				}
				int num = this._Offset;
				count = Math.Min(count, this._Bytes.Length - num);
				System.Buffer.BlockCopy(this._Bytes, num, buffer, offset, count);
				num += count;
				this._Offset = num;
				return count;
			}

			// Token: 0x06002B5F RID: 11103 RVA: 0x000BC406 File Offset: 0x000BB406
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
			}

			// Token: 0x06002B60 RID: 11104 RVA: 0x000BC417 File Offset: 0x000BB417
			public override void EndWrite(IAsyncResult asyncResult)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
			}

			// Token: 0x06002B61 RID: 11105 RVA: 0x000BC428 File Offset: 0x000BB428
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
			}

			// Token: 0x06002B62 RID: 11106 RVA: 0x000BC43C File Offset: 0x000BB43C
			public override long Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
				case SeekOrigin.Begin:
					this.Position = offset;
					return offset;
				case SeekOrigin.Current:
					return this.Position += offset;
				case SeekOrigin.End:
					return this.Position = (long)this._Bytes.Length - offset;
				default:
					throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
					{
						"SeekOrigin"
					}), "origin");
				}
			}

			// Token: 0x06002B63 RID: 11107 RVA: 0x000BC4B8 File Offset: 0x000BB4B8
			public override void SetLength(long length)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
			}

			// Token: 0x06002B64 RID: 11108 RVA: 0x000BC4CC File Offset: 0x000BB4CC
			protected override void Dispose(bool disposing)
			{
				try
				{
					this._Disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}

			// Token: 0x17000902 RID: 2306
			// (get) Token: 0x06002B65 RID: 11109 RVA: 0x000BC4FC File Offset: 0x000BB4FC
			internal byte[] Buffer
			{
				get
				{
					return this._Bytes;
				}
			}

			// Token: 0x040029AF RID: 10671
			private byte[] _Bytes;

			// Token: 0x040029B0 RID: 10672
			private int _Offset;

			// Token: 0x040029B1 RID: 10673
			private bool _Disposed;

			// Token: 0x040029B2 RID: 10674
			private int _ReadTimeout;

			// Token: 0x040029B3 RID: 10675
			private int _WriteTimeout;
		}

		// Token: 0x02000585 RID: 1413
		private class WriteOnlyStream : Stream
		{
			// Token: 0x06002B66 RID: 11110 RVA: 0x000BC504 File Offset: 0x000BB504
			public WriteOnlyStream(string key, SingleItemRequestCache cache, RequestCacheEntry cacheEntry, Stream realWriteStream)
			{
				this._Key = key;
				this._Cache = cache;
				this._TempEntry = cacheEntry;
				this._RealStream = realWriteStream;
				this._Buffers = new ArrayList();
			}

			// Token: 0x17000903 RID: 2307
			// (get) Token: 0x06002B67 RID: 11111 RVA: 0x000BC534 File Offset: 0x000BB534
			public override bool CanRead
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000904 RID: 2308
			// (get) Token: 0x06002B68 RID: 11112 RVA: 0x000BC537 File Offset: 0x000BB537
			public override bool CanSeek
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000905 RID: 2309
			// (get) Token: 0x06002B69 RID: 11113 RVA: 0x000BC53A File Offset: 0x000BB53A
			public override bool CanTimeout
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000906 RID: 2310
			// (get) Token: 0x06002B6A RID: 11114 RVA: 0x000BC53D File Offset: 0x000BB53D
			public override bool CanWrite
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000907 RID: 2311
			// (get) Token: 0x06002B6B RID: 11115 RVA: 0x000BC540 File Offset: 0x000BB540
			public override long Length
			{
				get
				{
					throw new NotSupportedException(SR.GetString("net_writeonlystream"));
				}
			}

			// Token: 0x17000908 RID: 2312
			// (get) Token: 0x06002B6C RID: 11116 RVA: 0x000BC551 File Offset: 0x000BB551
			// (set) Token: 0x06002B6D RID: 11117 RVA: 0x000BC562 File Offset: 0x000BB562
			public override long Position
			{
				get
				{
					throw new NotSupportedException(SR.GetString("net_writeonlystream"));
				}
				set
				{
					throw new NotSupportedException(SR.GetString("net_writeonlystream"));
				}
			}

			// Token: 0x17000909 RID: 2313
			// (get) Token: 0x06002B6E RID: 11118 RVA: 0x000BC573 File Offset: 0x000BB573
			// (set) Token: 0x06002B6F RID: 11119 RVA: 0x000BC57B File Offset: 0x000BB57B
			public override int ReadTimeout
			{
				get
				{
					return this._ReadTimeout;
				}
				set
				{
					if (value <= 0 && value != -1)
					{
						throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
					}
					this._ReadTimeout = value;
				}
			}

			// Token: 0x1700090A RID: 2314
			// (get) Token: 0x06002B70 RID: 11120 RVA: 0x000BC59C File Offset: 0x000BB59C
			// (set) Token: 0x06002B71 RID: 11121 RVA: 0x000BC5A4 File Offset: 0x000BB5A4
			public override int WriteTimeout
			{
				get
				{
					return this._WriteTimeout;
				}
				set
				{
					if (value <= 0 && value != -1)
					{
						throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
					}
					this._WriteTimeout = value;
				}
			}

			// Token: 0x06002B72 RID: 11122 RVA: 0x000BC5C5 File Offset: 0x000BB5C5
			public override void Flush()
			{
			}

			// Token: 0x06002B73 RID: 11123 RVA: 0x000BC5C7 File Offset: 0x000BB5C7
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06002B74 RID: 11124 RVA: 0x000BC5D8 File Offset: 0x000BB5D8
			public override int EndRead(IAsyncResult asyncResult)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06002B75 RID: 11125 RVA: 0x000BC5E9 File Offset: 0x000BB5E9
			public override int Read(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06002B76 RID: 11126 RVA: 0x000BC5FA File Offset: 0x000BB5FA
			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06002B77 RID: 11127 RVA: 0x000BC60B File Offset: 0x000BB60B
			public override void SetLength(long length)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
			}

			// Token: 0x06002B78 RID: 11128 RVA: 0x000BC61C File Offset: 0x000BB61C
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				this.Write(buffer, offset, count);
				LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(null, state, callback);
				lazyAsyncResult.InvokeCallback(null);
				return lazyAsyncResult;
			}

			// Token: 0x06002B79 RID: 11129 RVA: 0x000BC648 File Offset: 0x000BB648
			public override void EndWrite(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
				if (lazyAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
					{
						"EndWrite"
					}));
				}
				lazyAsyncResult.EndCalled = true;
				lazyAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x06002B7A RID: 11130 RVA: 0x000BC6A0 File Offset: 0x000BB6A0
			public override void Write(byte[] buffer, int offset, int count)
			{
				if (this._Disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (offset < 0 || offset > buffer.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (count < 0 || count > buffer.Length - offset)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (this._RealStream != null)
				{
					try
					{
						this._RealStream.Write(buffer, offset, count);
					}
					catch
					{
						this._RealStream.Close();
						this._RealStream = null;
					}
				}
				byte[] array = new byte[count];
				Buffer.BlockCopy(buffer, offset, array, 0, count);
				this._Buffers.Add(array);
				this._TotalSize += (long)count;
			}

			// Token: 0x06002B7B RID: 11131 RVA: 0x000BC76C File Offset: 0x000BB76C
			protected override void Dispose(bool disposing)
			{
				this._Disposed = true;
				base.Dispose(disposing);
				if (disposing)
				{
					if (this._RealStream != null)
					{
						try
						{
							this._RealStream.Close();
						}
						catch
						{
						}
					}
					byte[] array = new byte[this._TotalSize];
					int num = 0;
					for (int i = 0; i < this._Buffers.Count; i++)
					{
						byte[] array2 = (byte[])this._Buffers[i];
						Buffer.BlockCopy(array2, 0, array, num, array2.Length);
						num += array2.Length;
					}
					this._Cache.Commit(this._Key, this._TempEntry, array);
				}
			}

			// Token: 0x040029B4 RID: 10676
			private string _Key;

			// Token: 0x040029B5 RID: 10677
			private SingleItemRequestCache _Cache;

			// Token: 0x040029B6 RID: 10678
			private RequestCacheEntry _TempEntry;

			// Token: 0x040029B7 RID: 10679
			private Stream _RealStream;

			// Token: 0x040029B8 RID: 10680
			private long _TotalSize;

			// Token: 0x040029B9 RID: 10681
			private ArrayList _Buffers;

			// Token: 0x040029BA RID: 10682
			private bool _Disposed;

			// Token: 0x040029BB RID: 10683
			private int _ReadTimeout;

			// Token: 0x040029BC RID: 10684
			private int _WriteTimeout;
		}
	}
}
