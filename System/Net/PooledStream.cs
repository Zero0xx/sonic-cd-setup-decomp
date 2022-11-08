using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020004B7 RID: 1207
	internal class PooledStream : Stream
	{
		// Token: 0x0600254E RID: 9550 RVA: 0x00094E3F File Offset: 0x00093E3F
		internal PooledStream(object owner)
		{
			this.m_Owner = new WeakReference(owner);
			this.m_PooledCount = -1;
			this.m_Initalizing = true;
			this.m_NetworkStream = new NetworkStream();
			this.m_CreateTime = DateTime.UtcNow;
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x00094E77 File Offset: 0x00093E77
		internal PooledStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
		{
			this.m_ConnectionPool = connectionPool;
			this.m_Lifetime = lifetime;
			this.m_CheckLifetime = checkLifetime;
			this.m_Initalizing = true;
			this.m_NetworkStream = new NetworkStream();
			this.m_CreateTime = DateTime.UtcNow;
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002550 RID: 9552 RVA: 0x00094EB1 File Offset: 0x00093EB1
		internal bool JustConnected
		{
			get
			{
				if (this.m_JustConnected)
				{
					this.m_JustConnected = false;
					return true;
				}
				return false;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x00094EC5 File Offset: 0x00093EC5
		internal IPAddress ServerAddress
		{
			get
			{
				return this.m_ServerAddress;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06002552 RID: 9554 RVA: 0x00094ECD File Offset: 0x00093ECD
		internal bool IsInitalizing
		{
			get
			{
				return this.m_Initalizing;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x00094ED8 File Offset: 0x00093ED8
		// (set) Token: 0x06002554 RID: 9556 RVA: 0x00094F21 File Offset: 0x00093F21
		internal bool CanBePooled
		{
			get
			{
				if (this.m_Initalizing)
				{
					return true;
				}
				if (!this.m_NetworkStream.Connected)
				{
					return false;
				}
				WeakReference owner = this.m_Owner;
				return !this.m_ConnectionIsDoomed && (owner == null || !owner.IsAlive);
			}
			set
			{
				this.m_ConnectionIsDoomed |= !value;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x00094F34 File Offset: 0x00093F34
		internal bool IsEmancipated
		{
			get
			{
				WeakReference owner = this.m_Owner;
				return 0 >= this.m_PooledCount && (owner == null || !owner.IsAlive);
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x00094F68 File Offset: 0x00093F68
		// (set) Token: 0x06002557 RID: 9559 RVA: 0x00094F90 File Offset: 0x00093F90
		internal object Owner
		{
			get
			{
				WeakReference owner = this.m_Owner;
				if (owner != null && owner.IsAlive)
				{
					return owner.Target;
				}
				return null;
			}
			set
			{
				lock (this)
				{
					if (this.m_Owner != null)
					{
						this.m_Owner.Target = value;
					}
				}
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002558 RID: 9560 RVA: 0x00094FD4 File Offset: 0x00093FD4
		internal ConnectionPool Pool
		{
			get
			{
				return this.m_ConnectionPool;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002559 RID: 9561 RVA: 0x00094FDC File Offset: 0x00093FDC
		internal virtual ServicePoint ServicePoint
		{
			get
			{
				return this.Pool.ServicePoint;
			}
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x00094FE9 File Offset: 0x00093FE9
		internal bool Activate(object owningObject, GeneralAsyncDelegate asyncCallback)
		{
			return this.Activate(owningObject, asyncCallback != null, -1, asyncCallback);
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x00094FFC File Offset: 0x00093FFC
		protected bool Activate(object owningObject, bool async, int timeout, GeneralAsyncDelegate asyncCallback)
		{
			bool result;
			try
			{
				if (this.m_Initalizing)
				{
					IPAddress serverAddress = null;
					this.m_AsyncCallback = asyncCallback;
					Socket connection = this.ServicePoint.GetConnection(this, owningObject, async, out serverAddress, ref this.m_AbortSocket, ref this.m_AbortSocket6, timeout);
					if (connection != null)
					{
						this.m_NetworkStream.InitNetworkStream(connection, FileAccess.ReadWrite);
						this.m_ServerAddress = serverAddress;
						this.m_Initalizing = false;
						this.m_JustConnected = true;
						this.m_AbortSocket = null;
						this.m_AbortSocket6 = null;
						result = true;
					}
					else
					{
						result = false;
					}
				}
				else
				{
					if (async && asyncCallback != null)
					{
						asyncCallback(owningObject, this);
					}
					result = true;
				}
			}
			catch
			{
				this.m_Initalizing = false;
				throw;
			}
			return result;
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000950A4 File Offset: 0x000940A4
		internal void Deactivate()
		{
			this.m_AsyncCallback = null;
			if (!this.m_ConnectionIsDoomed && this.m_CheckLifetime)
			{
				this.CheckLifetime();
			}
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000950C4 File Offset: 0x000940C4
		internal virtual void ConnectionCallback(object owningObject, Exception e, Socket socket, IPAddress address)
		{
			object state = null;
			if (e != null)
			{
				this.m_Initalizing = false;
				state = e;
			}
			else
			{
				try
				{
					this.m_NetworkStream.InitNetworkStream(socket, FileAccess.ReadWrite);
					state = this;
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					state = ex;
				}
				catch
				{
					throw;
				}
				this.m_ServerAddress = address;
				this.m_Initalizing = false;
				this.m_JustConnected = true;
			}
			if (this.m_AsyncCallback != null)
			{
				this.m_AsyncCallback(owningObject, state);
			}
			this.m_AbortSocket = null;
			this.m_AbortSocket6 = null;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x0009515C File Offset: 0x0009415C
		protected void CheckLifetime()
		{
			bool flag = !this.m_ConnectionIsDoomed;
			if (flag)
			{
				TimeSpan t = DateTime.UtcNow.Subtract(this.m_CreateTime);
				this.m_ConnectionIsDoomed = (0 < TimeSpan.Compare(this.m_Lifetime, t));
			}
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000951A0 File Offset: 0x000941A0
		internal void UpdateLifetime()
		{
			int connectionLeaseTimeout = this.ServicePoint.ConnectionLeaseTimeout;
			TimeSpan maxValue;
			if (connectionLeaseTimeout == -1)
			{
				maxValue = TimeSpan.MaxValue;
				this.m_CheckLifetime = false;
			}
			else
			{
				maxValue = new TimeSpan(0, 0, 0, 0, connectionLeaseTimeout);
				this.m_CheckLifetime = true;
			}
			if (maxValue != this.m_Lifetime)
			{
				this.m_Lifetime = maxValue;
			}
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000951F4 File Offset: 0x000941F4
		internal void Destroy()
		{
			this.m_Owner = null;
			this.m_ConnectionIsDoomed = true;
			this.Close(0);
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x0009520C File Offset: 0x0009420C
		internal void PrePush(object expectedOwner)
		{
			lock (this)
			{
				if (expectedOwner == null)
				{
					if (this.m_Owner != null && this.m_Owner.Target != null)
					{
						throw new InternalException();
					}
				}
				else if (this.m_Owner == null || this.m_Owner.Target != expectedOwner)
				{
					throw new InternalException();
				}
				this.m_PooledCount++;
				if (1 != this.m_PooledCount)
				{
					throw new InternalException();
				}
				if (this.m_Owner != null)
				{
					this.m_Owner.Target = null;
				}
			}
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000952A8 File Offset: 0x000942A8
		internal void PostPop(object newOwner)
		{
			lock (this)
			{
				if (this.m_Owner == null)
				{
					this.m_Owner = new WeakReference(newOwner);
				}
				else
				{
					if (this.m_Owner.Target != null)
					{
						throw new InternalException();
					}
					this.m_Owner.Target = newOwner;
				}
				this.m_PooledCount--;
				if (this.Pool != null)
				{
					if (this.m_PooledCount != 0)
					{
						throw new InternalException();
					}
				}
				else if (-1 != this.m_PooledCount)
				{
					throw new InternalException();
				}
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002563 RID: 9571 RVA: 0x00095340 File Offset: 0x00094340
		protected bool UsingSecureStream
		{
			get
			{
				return this.m_NetworkStream is TlsStream;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002564 RID: 9572 RVA: 0x00095350 File Offset: 0x00094350
		// (set) Token: 0x06002565 RID: 9573 RVA: 0x00095358 File Offset: 0x00094358
		internal NetworkStream NetworkStream
		{
			get
			{
				return this.m_NetworkStream;
			}
			set
			{
				this.m_Initalizing = false;
				this.m_NetworkStream = value;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002566 RID: 9574 RVA: 0x00095368 File Offset: 0x00094368
		protected Socket Socket
		{
			get
			{
				return this.m_NetworkStream.InternalSocket;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002567 RID: 9575 RVA: 0x00095375 File Offset: 0x00094375
		public override bool CanRead
		{
			get
			{
				return this.m_NetworkStream.CanRead;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002568 RID: 9576 RVA: 0x00095382 File Offset: 0x00094382
		public override bool CanSeek
		{
			get
			{
				return this.m_NetworkStream.CanSeek;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002569 RID: 9577 RVA: 0x0009538F File Offset: 0x0009438F
		public override bool CanWrite
		{
			get
			{
				return this.m_NetworkStream.CanWrite;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600256A RID: 9578 RVA: 0x0009539C File Offset: 0x0009439C
		public override bool CanTimeout
		{
			get
			{
				return this.m_NetworkStream.CanTimeout;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600256B RID: 9579 RVA: 0x000953A9 File Offset: 0x000943A9
		// (set) Token: 0x0600256C RID: 9580 RVA: 0x000953B6 File Offset: 0x000943B6
		public override int ReadTimeout
		{
			get
			{
				return this.m_NetworkStream.ReadTimeout;
			}
			set
			{
				this.m_NetworkStream.ReadTimeout = value;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600256D RID: 9581 RVA: 0x000953C4 File Offset: 0x000943C4
		// (set) Token: 0x0600256E RID: 9582 RVA: 0x000953D1 File Offset: 0x000943D1
		public override int WriteTimeout
		{
			get
			{
				return this.m_NetworkStream.WriteTimeout;
			}
			set
			{
				this.m_NetworkStream.WriteTimeout = value;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x000953DF File Offset: 0x000943DF
		public override long Length
		{
			get
			{
				return this.m_NetworkStream.Length;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002570 RID: 9584 RVA: 0x000953EC File Offset: 0x000943EC
		// (set) Token: 0x06002571 RID: 9585 RVA: 0x000953F9 File Offset: 0x000943F9
		public override long Position
		{
			get
			{
				return this.m_NetworkStream.Position;
			}
			set
			{
				this.m_NetworkStream.Position = value;
			}
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x00095407 File Offset: 0x00094407
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.m_NetworkStream.Seek(offset, origin);
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x00095418 File Offset: 0x00094418
		public override int Read(byte[] buffer, int offset, int size)
		{
			return this.m_NetworkStream.Read(buffer, offset, size);
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x00095435 File Offset: 0x00094435
		public override void Write(byte[] buffer, int offset, int size)
		{
			this.m_NetworkStream.Write(buffer, offset, size);
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x00095445 File Offset: 0x00094445
		internal void MultipleWrite(BufferOffsetSize[] buffers)
		{
			this.m_NetworkStream.MultipleWrite(buffers);
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00095454 File Offset: 0x00094454
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.CloseSocket();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00095484 File Offset: 0x00094484
		internal void CloseSocket()
		{
			Socket abortSocket = this.m_AbortSocket;
			Socket abortSocket2 = this.m_AbortSocket6;
			this.m_NetworkStream.Close();
			if (abortSocket != null)
			{
				abortSocket.Close();
			}
			if (abortSocket2 != null)
			{
				abortSocket2.Close();
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000954BC File Offset: 0x000944BC
		public void Close(int timeout)
		{
			Socket abortSocket = this.m_AbortSocket;
			Socket abortSocket2 = this.m_AbortSocket6;
			this.m_NetworkStream.Close(timeout);
			if (abortSocket != null)
			{
				abortSocket.Close(timeout);
			}
			if (abortSocket2 != null)
			{
				abortSocket2.Close(timeout);
			}
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000954F7 File Offset: 0x000944F7
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginRead(buffer, offset, size, callback, state);
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x0009550B File Offset: 0x0009450B
		internal virtual IAsyncResult UnsafeBeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.UnsafeBeginRead(buffer, offset, size, callback, state);
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x0009551F File Offset: 0x0009451F
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.m_NetworkStream.EndRead(asyncResult);
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x0009552D File Offset: 0x0009452D
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginWrite(buffer, offset, size, callback, state);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00095541 File Offset: 0x00094541
		internal virtual IAsyncResult UnsafeBeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.UnsafeBeginWrite(buffer, offset, size, callback, state);
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00095555 File Offset: 0x00094555
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_NetworkStream.EndWrite(asyncResult);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00095563 File Offset: 0x00094563
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		internal IAsyncResult BeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginMultipleWrite(buffers, callback, state);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00095573 File Offset: 0x00094573
		internal void EndMultipleWrite(IAsyncResult asyncResult)
		{
			this.m_NetworkStream.EndMultipleWrite(asyncResult);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00095581 File Offset: 0x00094581
		public override void Flush()
		{
			this.m_NetworkStream.Flush();
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x0009558E File Offset: 0x0009458E
		public override void SetLength(long value)
		{
			this.m_NetworkStream.SetLength(value);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x0009559C File Offset: 0x0009459C
		internal void SetSocketTimeoutOption(SocketShutdown mode, int timeout, bool silent)
		{
			this.m_NetworkStream.SetSocketTimeoutOption(mode, timeout, silent);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000955AC File Offset: 0x000945AC
		internal bool Poll(int microSeconds, SelectMode mode)
		{
			return this.m_NetworkStream.Poll(microSeconds, mode);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000955BB File Offset: 0x000945BB
		internal bool PollRead()
		{
			return this.m_NetworkStream.PollRead();
		}

		// Token: 0x04002515 RID: 9493
		private bool m_CheckLifetime;

		// Token: 0x04002516 RID: 9494
		private TimeSpan m_Lifetime;

		// Token: 0x04002517 RID: 9495
		private DateTime m_CreateTime;

		// Token: 0x04002518 RID: 9496
		private bool m_ConnectionIsDoomed;

		// Token: 0x04002519 RID: 9497
		private ConnectionPool m_ConnectionPool;

		// Token: 0x0400251A RID: 9498
		private WeakReference m_Owner;

		// Token: 0x0400251B RID: 9499
		private int m_PooledCount;

		// Token: 0x0400251C RID: 9500
		private bool m_Initalizing;

		// Token: 0x0400251D RID: 9501
		private IPAddress m_ServerAddress;

		// Token: 0x0400251E RID: 9502
		private NetworkStream m_NetworkStream;

		// Token: 0x0400251F RID: 9503
		private Socket m_AbortSocket;

		// Token: 0x04002520 RID: 9504
		private Socket m_AbortSocket6;

		// Token: 0x04002521 RID: 9505
		private bool m_JustConnected;

		// Token: 0x04002522 RID: 9506
		private GeneralAsyncDelegate m_AsyncCallback;
	}
}
