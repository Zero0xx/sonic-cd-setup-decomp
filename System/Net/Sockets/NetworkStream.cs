using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x02000557 RID: 1367
	public class NetworkStream : Stream
	{
		// Token: 0x06002959 RID: 10585 RVA: 0x000ACFC5 File Offset: 0x000ABFC5
		internal NetworkStream()
		{
			this.m_OwnsSocket = true;
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000ACFE9 File Offset: 0x000ABFE9
		public NetworkStream(Socket socket)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this.InitNetworkStream(socket, FileAccess.ReadWrite);
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000AD01C File Offset: 0x000AC01C
		public NetworkStream(Socket socket, bool ownsSocket)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this.InitNetworkStream(socket, FileAccess.ReadWrite);
			this.m_OwnsSocket = ownsSocket;
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000AD058 File Offset: 0x000AC058
		internal NetworkStream(NetworkStream networkStream, bool ownsSocket)
		{
			Socket socket = networkStream.Socket;
			if (socket == null)
			{
				throw new ArgumentNullException("networkStream");
			}
			this.InitNetworkStream(socket, FileAccess.ReadWrite);
			this.m_OwnsSocket = ownsSocket;
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000AD0A4 File Offset: 0x000AC0A4
		public NetworkStream(Socket socket, FileAccess access)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this.InitNetworkStream(socket, access);
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000AD0D7 File Offset: 0x000AC0D7
		public NetworkStream(Socket socket, FileAccess access, bool ownsSocket)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this.InitNetworkStream(socket, access);
			this.m_OwnsSocket = ownsSocket;
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x0600295F RID: 10591 RVA: 0x000AD111 File Offset: 0x000AC111
		protected Socket Socket
		{
			get
			{
				return this.m_StreamSocket;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002960 RID: 10592 RVA: 0x000AD11C File Offset: 0x000AC11C
		internal Socket InternalSocket
		{
			get
			{
				Socket streamSocket = this.m_StreamSocket;
				if (this.m_CleanedUp || streamSocket == null)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				return streamSocket;
			}
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000AD14D File Offset: 0x000AC14D
		internal void ConvertToNotSocketOwner()
		{
			this.m_OwnsSocket = false;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002962 RID: 10594 RVA: 0x000AD15C File Offset: 0x000AC15C
		// (set) Token: 0x06002963 RID: 10595 RVA: 0x000AD164 File Offset: 0x000AC164
		protected bool Readable
		{
			get
			{
				return this.m_Readable;
			}
			set
			{
				this.m_Readable = value;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002964 RID: 10596 RVA: 0x000AD16D File Offset: 0x000AC16D
		// (set) Token: 0x06002965 RID: 10597 RVA: 0x000AD175 File Offset: 0x000AC175
		protected bool Writeable
		{
			get
			{
				return this.m_Writeable;
			}
			set
			{
				this.m_Writeable = value;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002966 RID: 10598 RVA: 0x000AD17E File Offset: 0x000AC17E
		public override bool CanRead
		{
			get
			{
				return this.m_Readable;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002967 RID: 10599 RVA: 0x000AD186 File Offset: 0x000AC186
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002968 RID: 10600 RVA: 0x000AD189 File Offset: 0x000AC189
		public override bool CanWrite
		{
			get
			{
				return this.m_Writeable;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002969 RID: 10601 RVA: 0x000AD191 File Offset: 0x000AC191
		public override bool CanTimeout
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x0600296A RID: 10602 RVA: 0x000AD194 File Offset: 0x000AC194
		// (set) Token: 0x0600296B RID: 10603 RVA: 0x000AD1C2 File Offset: 0x000AC1C2
		public override int ReadTimeout
		{
			get
			{
				int num = (int)this.m_StreamSocket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
				if (num == 0)
				{
					return -1;
				}
				return num;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this.SetSocketTimeoutOption(SocketShutdown.Receive, value, false);
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x0600296C RID: 10604 RVA: 0x000AD1E8 File Offset: 0x000AC1E8
		// (set) Token: 0x0600296D RID: 10605 RVA: 0x000AD216 File Offset: 0x000AC216
		public override int WriteTimeout
		{
			get
			{
				int num = (int)this.m_StreamSocket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
				if (num == 0)
				{
					return -1;
				}
				return num;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this.SetSocketTimeoutOption(SocketShutdown.Send, value, false);
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600296E RID: 10606 RVA: 0x000AD23C File Offset: 0x000AC23C
		public virtual bool DataAvailable
		{
			get
			{
				if (this.m_CleanedUp)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				Socket streamSocket = this.m_StreamSocket;
				if (streamSocket == null)
				{
					throw new IOException(SR.GetString("net_io_readfailure", new object[]
					{
						SR.GetString("net_io_connectionclosed")
					}));
				}
				return streamSocket.Available != 0;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x000AD29D File Offset: 0x000AC29D
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002970 RID: 10608 RVA: 0x000AD2AE File Offset: 0x000AC2AE
		// (set) Token: 0x06002971 RID: 10609 RVA: 0x000AD2BF File Offset: 0x000AC2BF
		public override long Position
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x000AD2D0 File Offset: 0x000AC2D0
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000AD2E4 File Offset: 0x000AC2E4
		internal void InitNetworkStream(Socket socket, FileAccess Access)
		{
			if (!socket.Blocking)
			{
				throw new IOException(SR.GetString("net_sockets_blocking"));
			}
			if (!socket.Connected)
			{
				throw new IOException(SR.GetString("net_notconnected"));
			}
			if (socket.SocketType != SocketType.Stream)
			{
				throw new IOException(SR.GetString("net_notstream"));
			}
			this.m_StreamSocket = socket;
			switch (Access)
			{
			case FileAccess.Read:
				this.m_Readable = true;
				return;
			case FileAccess.Write:
				this.m_Writeable = true;
				return;
			}
			this.m_Readable = true;
			this.m_Writeable = true;
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000AD378 File Offset: 0x000AC378
		internal bool PollRead()
		{
			if (this.m_CleanedUp)
			{
				return false;
			}
			Socket streamSocket = this.m_StreamSocket;
			return streamSocket != null && streamSocket.Poll(0, SelectMode.SelectRead);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000AD3A4 File Offset: 0x000AC3A4
		internal bool Poll(int microSeconds, SelectMode mode)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			return streamSocket.Poll(microSeconds, mode);
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000AD404 File Offset: 0x000AC404
		public override int Read([In] [Out] byte[] buffer, int offset, int size)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (!this.CanRead)
			{
				throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			int result;
			try
			{
				int num = streamSocket.Receive(buffer, offset, size, SocketFlags.None);
				result = num;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					ex.Message
				}), ex);
			}
			return result;
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000AD510 File Offset: 0x000AC510
		public override void Write(byte[] buffer, int offset, int size)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (!this.CanWrite)
			{
				throw new InvalidOperationException(SR.GetString("net_readonlystream"));
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			try
			{
				streamSocket.Send(buffer, offset, size, SocketFlags.None);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000AD650 File Offset: 0x000AC650
		public void Close(int timeout)
		{
			if (timeout < -1)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			this.m_CloseTimeout = timeout;
			this.Close();
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x000AD670 File Offset: 0x000AC670
		protected override void Dispose(bool disposing)
		{
			if (!this.m_CleanedUp && disposing && this.m_StreamSocket != null)
			{
				this.m_Readable = false;
				this.m_Writeable = false;
				if (this.m_OwnsSocket)
				{
					Socket streamSocket = this.m_StreamSocket;
					if (streamSocket != null)
					{
						streamSocket.InternalShutdown(SocketShutdown.Both);
						streamSocket.Close(this.m_CloseTimeout);
					}
				}
			}
			this.m_CleanedUp = true;
			base.Dispose(disposing);
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000AD6D4 File Offset: 0x000AC6D4
		~NetworkStream()
		{
			this.Dispose(false);
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x0600297B RID: 10619 RVA: 0x000AD704 File Offset: 0x000AC704
		internal bool Connected
		{
			get
			{
				Socket streamSocket = this.m_StreamSocket;
				return !this.m_CleanedUp && streamSocket != null && streamSocket.Connected;
			}
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000AD730 File Offset: 0x000AC730
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (!this.CanRead)
			{
				throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			IAsyncResult result;
			try
			{
				IAsyncResult asyncResult = streamSocket.BeginReceive(buffer, offset, size, SocketFlags.None, callback, state);
				result = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return result;
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000AD880 File Offset: 0x000AC880
		internal virtual IAsyncResult UnsafeBeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!this.CanRead)
			{
				throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			IAsyncResult result;
			try
			{
				IAsyncResult asyncResult = streamSocket.UnsafeBeginReceive(buffer, offset, size, SocketFlags.None, callback, state);
				result = asyncResult;
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return result;
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x000AD984 File Offset: 0x000AC984
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			int result;
			try
			{
				int num = streamSocket.EndReceive(asyncResult);
				result = num;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return result;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000ADA88 File Offset: 0x000ACA88
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (!this.CanWrite)
			{
				throw new InvalidOperationException(SR.GetString("net_readonlystream"));
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			IAsyncResult result;
			try
			{
				IAsyncResult asyncResult = streamSocket.BeginSend(buffer, offset, size, SocketFlags.None, callback, state);
				result = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return result;
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000ADBD8 File Offset: 0x000ACBD8
		internal virtual IAsyncResult UnsafeBeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!this.CanWrite)
			{
				throw new InvalidOperationException(SR.GetString("net_readonlystream"));
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			IAsyncResult result;
			try
			{
				IAsyncResult asyncResult = streamSocket.UnsafeBeginSend(buffer, offset, size, SocketFlags.None, callback, state);
				result = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return result;
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000ADCEC File Offset: 0x000ACCEC
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			try
			{
				streamSocket.EndSend(asyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000ADDE8 File Offset: 0x000ACDE8
		internal virtual void MultipleWrite(BufferOffsetSize[] buffers)
		{
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			try
			{
				buffers = this.ConcatenateBuffersOnWin9x(buffers);
				streamSocket.MultipleSend(buffers, SocketFlags.None);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000ADED4 File Offset: 0x000ACED4
		internal virtual IAsyncResult BeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			IAsyncResult result;
			try
			{
				buffers = this.ConcatenateBuffersOnWin9x(buffers);
				IAsyncResult asyncResult = streamSocket.BeginMultipleSend(buffers, SocketFlags.None, callback, state);
				result = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return result;
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000ADFCC File Offset: 0x000ACFCC
		internal virtual IAsyncResult UnsafeBeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			IAsyncResult result;
			try
			{
				buffers = this.ConcatenateBuffersOnWin9x(buffers);
				IAsyncResult asyncResult = streamSocket.UnsafeBeginMultipleSend(buffers, SocketFlags.None, callback, state);
				result = asyncResult;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return result;
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000AE0C4 File Offset: 0x000AD0C4
		internal virtual void EndMultipleWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					SR.GetString("net_io_connectionclosed")
				}));
			}
			try
			{
				streamSocket.EndMultipleSend(asyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					ex.Message
				}), ex);
			}
			catch
			{
				throw new IOException(SR.GetString("net_io_writefailure", new object[]
				{
					string.Empty
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000AE1A4 File Offset: 0x000AD1A4
		private BufferOffsetSize[] ConcatenateBuffersOnWin9x(BufferOffsetSize[] buffers)
		{
			if (ComNetOS.IsWin9x && buffers.Length > 16)
			{
				BufferOffsetSize[] array = new BufferOffsetSize[16];
				for (int i = 0; i < 16; i++)
				{
					array[i] = buffers[i];
				}
				int num = 0;
				for (int i = 15; i < buffers.Length; i++)
				{
					num += buffers[i].Size;
				}
				if (num > 0)
				{
					array[15] = new BufferOffsetSize(new byte[num], 0, num, false);
					num = 0;
					for (int i = 15; i < buffers.Length; i++)
					{
						Buffer.BlockCopy(buffers[i].Buffer, buffers[i].Offset, array[15].Buffer, num, buffers[i].Size);
						num += buffers[i].Size;
					}
				}
				buffers = array;
			}
			return buffers;
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000AE259 File Offset: 0x000AD259
		public override void Flush()
		{
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x000AE25B File Offset: 0x000AD25B
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000AE26C File Offset: 0x000AD26C
		internal void SetSocketTimeoutOption(SocketShutdown mode, int timeout, bool silent)
		{
			if (timeout < 0)
			{
				timeout = 0;
			}
			Socket streamSocket = this.m_StreamSocket;
			if (streamSocket == null)
			{
				return;
			}
			if ((mode == SocketShutdown.Send || mode == SocketShutdown.Both) && timeout != this.m_CurrentWriteTimeout)
			{
				streamSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, timeout, silent);
				this.m_CurrentWriteTimeout = timeout;
			}
			if ((mode == SocketShutdown.Receive || mode == SocketShutdown.Both) && timeout != this.m_CurrentReadTimeout)
			{
				streamSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout, silent);
				this.m_CurrentReadTimeout = timeout;
			}
		}

		// Token: 0x04002867 RID: 10343
		private Socket m_StreamSocket;

		// Token: 0x04002868 RID: 10344
		private bool m_Readable;

		// Token: 0x04002869 RID: 10345
		private bool m_Writeable;

		// Token: 0x0400286A RID: 10346
		private bool m_OwnsSocket;

		// Token: 0x0400286B RID: 10347
		private int m_CloseTimeout = -1;

		// Token: 0x0400286C RID: 10348
		private bool m_CleanedUp;

		// Token: 0x0400286D RID: 10349
		private int m_CurrentReadTimeout = -1;

		// Token: 0x0400286E RID: 10350
		private int m_CurrentWriteTimeout = -1;
	}
}
