using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.IO
{
	// Token: 0x0200059E RID: 1438
	[ComVisible(true)]
	[Serializable]
	public abstract class Stream : MarshalByRefObject, IDisposable
	{
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003473 RID: 13427
		public abstract bool CanRead { get; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003474 RID: 13428
		public abstract bool CanSeek { get; }

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06003475 RID: 13429 RVA: 0x000ADD12 File Offset: 0x000ACD12
		[ComVisible(false)]
		public virtual bool CanTimeout
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003476 RID: 13430
		public abstract bool CanWrite { get; }

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003477 RID: 13431
		public abstract long Length { get; }

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003478 RID: 13432
		// (set) Token: 0x06003479 RID: 13433
		public abstract long Position { get; set; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x0600347A RID: 13434 RVA: 0x000ADD15 File Offset: 0x000ACD15
		// (set) Token: 0x0600347B RID: 13435 RVA: 0x000ADD26 File Offset: 0x000ACD26
		[ComVisible(false)]
		public virtual int ReadTimeout
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TimeoutsNotSupported"));
			}
			set
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TimeoutsNotSupported"));
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x0600347C RID: 13436 RVA: 0x000ADD37 File Offset: 0x000ACD37
		// (set) Token: 0x0600347D RID: 13437 RVA: 0x000ADD48 File Offset: 0x000ACD48
		[ComVisible(false)]
		public virtual int WriteTimeout
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TimeoutsNotSupported"));
			}
			set
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TimeoutsNotSupported"));
			}
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x000ADD59 File Offset: 0x000ACD59
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x000ADD68 File Offset: 0x000ACD68
		public void Dispose()
		{
			this.Close();
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x000ADD70 File Offset: 0x000ACD70
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this._asyncActiveEvent != null)
			{
				this._CloseAsyncActiveEvent(Interlocked.Decrement(ref this._asyncActiveCount));
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x000ADD8E File Offset: 0x000ACD8E
		private void _CloseAsyncActiveEvent(int asyncActiveCount)
		{
			if (this._asyncActiveEvent != null && asyncActiveCount == 0)
			{
				this._asyncActiveEvent.Close();
				this._asyncActiveEvent = null;
			}
		}

		// Token: 0x06003482 RID: 13442
		public abstract void Flush();

		// Token: 0x06003483 RID: 13443 RVA: 0x000ADDAD File Offset: 0x000ACDAD
		[Obsolete("CreateWaitHandle will be removed eventually.  Please use \"new ManualResetEvent(false)\" instead.")]
		protected virtual WaitHandle CreateWaitHandle()
		{
			return new ManualResetEvent(false);
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x000ADDB8 File Offset: 0x000ACDB8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanRead)
			{
				__Error.ReadNotSupported();
			}
			Interlocked.Increment(ref this._asyncActiveCount);
			Stream.ReadDelegate readDelegate = new Stream.ReadDelegate(this.Read);
			if (this._asyncActiveEvent == null)
			{
				lock (this)
				{
					if (this._asyncActiveEvent == null)
					{
						this._asyncActiveEvent = new AutoResetEvent(true);
					}
				}
			}
			this._asyncActiveEvent.WaitOne();
			this._readDelegate = readDelegate;
			return readDelegate.BeginInvoke(buffer, offset, count, callback, state);
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000ADE4C File Offset: 0x000ACE4C
		public virtual int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (this._readDelegate == null)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_WrongAsyncResultOrEndReadCalledMultiple"));
			}
			int result = -1;
			try
			{
				result = this._readDelegate.EndInvoke(asyncResult);
			}
			finally
			{
				this._readDelegate = null;
				this._asyncActiveEvent.Set();
				this._CloseAsyncActiveEvent(Interlocked.Decrement(ref this._asyncActiveCount));
			}
			return result;
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x000ADEC8 File Offset: 0x000ACEC8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanWrite)
			{
				__Error.WriteNotSupported();
			}
			Interlocked.Increment(ref this._asyncActiveCount);
			Stream.WriteDelegate writeDelegate = new Stream.WriteDelegate(this.Write);
			if (this._asyncActiveEvent == null)
			{
				lock (this)
				{
					if (this._asyncActiveEvent == null)
					{
						this._asyncActiveEvent = new AutoResetEvent(true);
					}
				}
			}
			this._asyncActiveEvent.WaitOne();
			this._writeDelegate = writeDelegate;
			return writeDelegate.BeginInvoke(buffer, offset, count, callback, state);
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x000ADF5C File Offset: 0x000ACF5C
		public virtual void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (this._writeDelegate == null)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_WrongAsyncResultOrEndWriteCalledMultiple"));
			}
			try
			{
				this._writeDelegate.EndInvoke(asyncResult);
			}
			finally
			{
				this._writeDelegate = null;
				this._asyncActiveEvent.Set();
				this._CloseAsyncActiveEvent(Interlocked.Decrement(ref this._asyncActiveCount));
			}
		}

		// Token: 0x06003488 RID: 13448
		public abstract long Seek(long offset, SeekOrigin origin);

		// Token: 0x06003489 RID: 13449
		public abstract void SetLength(long value);

		// Token: 0x0600348A RID: 13450
		public abstract int Read([In] [Out] byte[] buffer, int offset, int count);

		// Token: 0x0600348B RID: 13451 RVA: 0x000ADFD4 File Offset: 0x000ACFD4
		public virtual int ReadByte()
		{
			byte[] array = new byte[1];
			if (this.Read(array, 0, 1) == 0)
			{
				return -1;
			}
			return (int)array[0];
		}

		// Token: 0x0600348C RID: 13452
		public abstract void Write(byte[] buffer, int offset, int count);

		// Token: 0x0600348D RID: 13453 RVA: 0x000ADFFC File Offset: 0x000ACFFC
		public virtual void WriteByte(byte value)
		{
			this.Write(new byte[]
			{
				value
			}, 0, 1);
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000AE01D File Offset: 0x000AD01D
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Stream Synchronized(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (stream is Stream.SyncStream)
			{
				return stream;
			}
			return new Stream.SyncStream(stream);
		}

		// Token: 0x04001BE0 RID: 7136
		public static readonly Stream Null = new Stream.NullStream();

		// Token: 0x04001BE1 RID: 7137
		[NonSerialized]
		private Stream.ReadDelegate _readDelegate;

		// Token: 0x04001BE2 RID: 7138
		[NonSerialized]
		private Stream.WriteDelegate _writeDelegate;

		// Token: 0x04001BE3 RID: 7139
		[NonSerialized]
		private AutoResetEvent _asyncActiveEvent;

		// Token: 0x04001BE4 RID: 7140
		[NonSerialized]
		private int _asyncActiveCount = 1;

		// Token: 0x0200059F RID: 1439
		// (Invoke) Token: 0x06003492 RID: 13458
		private delegate int ReadDelegate([In] [Out] byte[] bytes, int index, int offset);

		// Token: 0x020005A0 RID: 1440
		// (Invoke) Token: 0x06003496 RID: 13462
		private delegate void WriteDelegate(byte[] bytes, int index, int offset);

		// Token: 0x020005A1 RID: 1441
		[Serializable]
		private sealed class NullStream : Stream
		{
			// Token: 0x06003499 RID: 13465 RVA: 0x000AE058 File Offset: 0x000AD058
			internal NullStream()
			{
			}

			// Token: 0x170008EF RID: 2287
			// (get) Token: 0x0600349A RID: 13466 RVA: 0x000AE060 File Offset: 0x000AD060
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008F0 RID: 2288
			// (get) Token: 0x0600349B RID: 13467 RVA: 0x000AE063 File Offset: 0x000AD063
			public override bool CanWrite
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008F1 RID: 2289
			// (get) Token: 0x0600349C RID: 13468 RVA: 0x000AE066 File Offset: 0x000AD066
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008F2 RID: 2290
			// (get) Token: 0x0600349D RID: 13469 RVA: 0x000AE069 File Offset: 0x000AD069
			public override long Length
			{
				get
				{
					return 0L;
				}
			}

			// Token: 0x170008F3 RID: 2291
			// (get) Token: 0x0600349E RID: 13470 RVA: 0x000AE06D File Offset: 0x000AD06D
			// (set) Token: 0x0600349F RID: 13471 RVA: 0x000AE071 File Offset: 0x000AD071
			public override long Position
			{
				get
				{
					return 0L;
				}
				set
				{
				}
			}

			// Token: 0x060034A0 RID: 13472 RVA: 0x000AE073 File Offset: 0x000AD073
			public override void Flush()
			{
			}

			// Token: 0x060034A1 RID: 13473 RVA: 0x000AE075 File Offset: 0x000AD075
			public override int Read([In] [Out] byte[] buffer, int offset, int count)
			{
				return 0;
			}

			// Token: 0x060034A2 RID: 13474 RVA: 0x000AE078 File Offset: 0x000AD078
			public override int ReadByte()
			{
				return -1;
			}

			// Token: 0x060034A3 RID: 13475 RVA: 0x000AE07B File Offset: 0x000AD07B
			public override void Write(byte[] buffer, int offset, int count)
			{
			}

			// Token: 0x060034A4 RID: 13476 RVA: 0x000AE07D File Offset: 0x000AD07D
			public override void WriteByte(byte value)
			{
			}

			// Token: 0x060034A5 RID: 13477 RVA: 0x000AE07F File Offset: 0x000AD07F
			public override long Seek(long offset, SeekOrigin origin)
			{
				return 0L;
			}

			// Token: 0x060034A6 RID: 13478 RVA: 0x000AE083 File Offset: 0x000AD083
			public override void SetLength(long length)
			{
			}
		}

		// Token: 0x020005A2 RID: 1442
		[Serializable]
		internal sealed class SyncStream : Stream, IDisposable
		{
			// Token: 0x060034A7 RID: 13479 RVA: 0x000AE085 File Offset: 0x000AD085
			internal SyncStream(Stream stream)
			{
				if (stream == null)
				{
					throw new ArgumentNullException("stream");
				}
				this._stream = stream;
			}

			// Token: 0x170008F4 RID: 2292
			// (get) Token: 0x060034A8 RID: 13480 RVA: 0x000AE0A2 File Offset: 0x000AD0A2
			public override bool CanRead
			{
				get
				{
					return this._stream.CanRead;
				}
			}

			// Token: 0x170008F5 RID: 2293
			// (get) Token: 0x060034A9 RID: 13481 RVA: 0x000AE0AF File Offset: 0x000AD0AF
			public override bool CanWrite
			{
				get
				{
					return this._stream.CanWrite;
				}
			}

			// Token: 0x170008F6 RID: 2294
			// (get) Token: 0x060034AA RID: 13482 RVA: 0x000AE0BC File Offset: 0x000AD0BC
			public override bool CanSeek
			{
				get
				{
					return this._stream.CanSeek;
				}
			}

			// Token: 0x170008F7 RID: 2295
			// (get) Token: 0x060034AB RID: 13483 RVA: 0x000AE0C9 File Offset: 0x000AD0C9
			[ComVisible(false)]
			public override bool CanTimeout
			{
				get
				{
					return this._stream.CanTimeout;
				}
			}

			// Token: 0x170008F8 RID: 2296
			// (get) Token: 0x060034AC RID: 13484 RVA: 0x000AE0D8 File Offset: 0x000AD0D8
			public override long Length
			{
				get
				{
					long length;
					lock (this._stream)
					{
						length = this._stream.Length;
					}
					return length;
				}
			}

			// Token: 0x170008F9 RID: 2297
			// (get) Token: 0x060034AD RID: 13485 RVA: 0x000AE118 File Offset: 0x000AD118
			// (set) Token: 0x060034AE RID: 13486 RVA: 0x000AE158 File Offset: 0x000AD158
			public override long Position
			{
				get
				{
					long position;
					lock (this._stream)
					{
						position = this._stream.Position;
					}
					return position;
				}
				set
				{
					lock (this._stream)
					{
						this._stream.Position = value;
					}
				}
			}

			// Token: 0x170008FA RID: 2298
			// (get) Token: 0x060034AF RID: 13487 RVA: 0x000AE198 File Offset: 0x000AD198
			// (set) Token: 0x060034B0 RID: 13488 RVA: 0x000AE1A5 File Offset: 0x000AD1A5
			[ComVisible(false)]
			public override int ReadTimeout
			{
				get
				{
					return this._stream.ReadTimeout;
				}
				set
				{
					this._stream.ReadTimeout = value;
				}
			}

			// Token: 0x170008FB RID: 2299
			// (get) Token: 0x060034B1 RID: 13489 RVA: 0x000AE1B3 File Offset: 0x000AD1B3
			// (set) Token: 0x060034B2 RID: 13490 RVA: 0x000AE1C0 File Offset: 0x000AD1C0
			[ComVisible(false)]
			public override int WriteTimeout
			{
				get
				{
					return this._stream.WriteTimeout;
				}
				set
				{
					this._stream.WriteTimeout = value;
				}
			}

			// Token: 0x060034B3 RID: 13491 RVA: 0x000AE1D0 File Offset: 0x000AD1D0
			public override void Close()
			{
				lock (this._stream)
				{
					try
					{
						this._stream.Close();
					}
					finally
					{
						base.Dispose(true);
					}
				}
			}

			// Token: 0x060034B4 RID: 13492 RVA: 0x000AE224 File Offset: 0x000AD224
			protected override void Dispose(bool disposing)
			{
				lock (this._stream)
				{
					try
					{
						if (disposing)
						{
							((IDisposable)this._stream).Dispose();
						}
					}
					finally
					{
						base.Dispose(disposing);
					}
				}
			}

			// Token: 0x060034B5 RID: 13493 RVA: 0x000AE27C File Offset: 0x000AD27C
			public override void Flush()
			{
				lock (this._stream)
				{
					this._stream.Flush();
				}
			}

			// Token: 0x060034B6 RID: 13494 RVA: 0x000AE2BC File Offset: 0x000AD2BC
			public override int Read([In] [Out] byte[] bytes, int offset, int count)
			{
				int result;
				lock (this._stream)
				{
					result = this._stream.Read(bytes, offset, count);
				}
				return result;
			}

			// Token: 0x060034B7 RID: 13495 RVA: 0x000AE300 File Offset: 0x000AD300
			public override int ReadByte()
			{
				int result;
				lock (this._stream)
				{
					result = this._stream.ReadByte();
				}
				return result;
			}

			// Token: 0x060034B8 RID: 13496 RVA: 0x000AE340 File Offset: 0x000AD340
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				IAsyncResult result;
				lock (this._stream)
				{
					result = this._stream.BeginRead(buffer, offset, count, callback, state);
				}
				return result;
			}

			// Token: 0x060034B9 RID: 13497 RVA: 0x000AE388 File Offset: 0x000AD388
			public override int EndRead(IAsyncResult asyncResult)
			{
				int result;
				lock (this._stream)
				{
					result = this._stream.EndRead(asyncResult);
				}
				return result;
			}

			// Token: 0x060034BA RID: 13498 RVA: 0x000AE3CC File Offset: 0x000AD3CC
			public override long Seek(long offset, SeekOrigin origin)
			{
				long result;
				lock (this._stream)
				{
					result = this._stream.Seek(offset, origin);
				}
				return result;
			}

			// Token: 0x060034BB RID: 13499 RVA: 0x000AE410 File Offset: 0x000AD410
			public override void SetLength(long length)
			{
				lock (this._stream)
				{
					this._stream.SetLength(length);
				}
			}

			// Token: 0x060034BC RID: 13500 RVA: 0x000AE450 File Offset: 0x000AD450
			public override void Write(byte[] bytes, int offset, int count)
			{
				lock (this._stream)
				{
					this._stream.Write(bytes, offset, count);
				}
			}

			// Token: 0x060034BD RID: 13501 RVA: 0x000AE494 File Offset: 0x000AD494
			public override void WriteByte(byte b)
			{
				lock (this._stream)
				{
					this._stream.WriteByte(b);
				}
			}

			// Token: 0x060034BE RID: 13502 RVA: 0x000AE4D4 File Offset: 0x000AD4D4
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				IAsyncResult result;
				lock (this._stream)
				{
					result = this._stream.BeginWrite(buffer, offset, count, callback, state);
				}
				return result;
			}

			// Token: 0x060034BF RID: 13503 RVA: 0x000AE51C File Offset: 0x000AD51C
			public override void EndWrite(IAsyncResult asyncResult)
			{
				lock (this._stream)
				{
					this._stream.EndWrite(asyncResult);
				}
			}

			// Token: 0x04001BE5 RID: 7141
			private Stream _stream;
		}
	}
}
