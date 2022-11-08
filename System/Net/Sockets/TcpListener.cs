using System;
using System.Security.Permissions;

namespace System.Net.Sockets
{
	// Token: 0x020005C8 RID: 1480
	public class TcpListener
	{
		// Token: 0x06002E53 RID: 11859 RVA: 0x000CC4D4 File Offset: 0x000CB4D4
		public TcpListener(IPEndPoint localEP)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpListener", localEP);
			}
			if (localEP == null)
			{
				throw new ArgumentNullException("localEP");
			}
			this.m_ServerSocketEP = localEP;
			this.m_ServerSocket = new Socket(this.m_ServerSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpListener", null);
			}
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000CC544 File Offset: 0x000CB544
		public TcpListener(IPAddress localaddr, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpListener", localaddr);
			}
			if (localaddr == null)
			{
				throw new ArgumentNullException("localaddr");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_ServerSocketEP = new IPEndPoint(localaddr, port);
			this.m_ServerSocket = new Socket(this.m_ServerSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpListener", null);
			}
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000CC5D0 File Offset: 0x000CB5D0
		[Obsolete("This method has been deprecated. Please use TcpListener(IPAddress localaddr, int port) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public TcpListener(int port)
		{
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_ServerSocketEP = new IPEndPoint(IPAddress.Any, port);
			this.m_ServerSocket = new Socket(this.m_ServerSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x000CC61F File Offset: 0x000CB61F
		public Socket Server
		{
			get
			{
				return this.m_ServerSocket;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06002E57 RID: 11863 RVA: 0x000CC627 File Offset: 0x000CB627
		protected bool Active
		{
			get
			{
				return this.m_Active;
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06002E58 RID: 11864 RVA: 0x000CC62F File Offset: 0x000CB62F
		public EndPoint LocalEndpoint
		{
			get
			{
				if (!this.m_Active)
				{
					return this.m_ServerSocketEP;
				}
				return this.m_ServerSocket.LocalEndPoint;
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06002E59 RID: 11865 RVA: 0x000CC64B File Offset: 0x000CB64B
		// (set) Token: 0x06002E5A RID: 11866 RVA: 0x000CC658 File Offset: 0x000CB658
		public bool ExclusiveAddressUse
		{
			get
			{
				return this.m_ServerSocket.ExclusiveAddressUse;
			}
			set
			{
				if (this.m_Active)
				{
					throw new InvalidOperationException(SR.GetString("net_tcplistener_mustbestopped"));
				}
				this.m_ServerSocket.ExclusiveAddressUse = value;
				this.m_ExclusiveAddressUse = value;
			}
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x000CC685 File Offset: 0x000CB685
		public void Start()
		{
			this.Start(int.MaxValue);
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000CC694 File Offset: 0x000CB694
		public void Start(int backlog)
		{
			if (backlog > 2147483647 || backlog < 0)
			{
				throw new ArgumentOutOfRangeException("backlog");
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Start", null);
			}
			if (this.m_ServerSocket == null)
			{
				throw new InvalidOperationException(SR.GetString("net_InvalidSocketHandle"));
			}
			if (this.m_Active)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Sockets, this, "Start", null);
				}
				return;
			}
			this.m_ServerSocket.Bind(this.m_ServerSocketEP);
			this.m_ServerSocket.Listen(backlog);
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Start", null);
			}
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000CC748 File Offset: 0x000CB748
		public void Stop()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Stop", null);
			}
			if (this.m_ServerSocket != null)
			{
				this.m_ServerSocket.Close();
				this.m_ServerSocket = null;
			}
			this.m_Active = false;
			this.m_ServerSocket = new Socket(this.m_ServerSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (this.m_ExclusiveAddressUse)
			{
				this.m_ServerSocket.ExclusiveAddressUse = true;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Stop", null);
			}
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000CC7D2 File Offset: 0x000CB7D2
		public bool Pending()
		{
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			return this.m_ServerSocket.Poll(0, SelectMode.SelectRead);
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000CC7FC File Offset: 0x000CB7FC
		public Socket AcceptSocket()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "AcceptSocket", null);
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			Socket socket = this.m_ServerSocket.Accept();
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "AcceptSocket", socket);
			}
			return socket;
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000CC860 File Offset: 0x000CB860
		public TcpClient AcceptTcpClient()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "AcceptTcpClient", null);
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			Socket acceptedSocket = this.m_ServerSocket.Accept();
			TcpClient tcpClient = new TcpClient(acceptedSocket);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "AcceptTcpClient", tcpClient);
			}
			return tcpClient;
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000CC8CC File Offset: 0x000CB8CC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginAcceptSocket", null);
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			IAsyncResult result = this.m_ServerSocket.BeginAccept(callback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginAcceptSocket", null);
			}
			return result;
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000CC930 File Offset: 0x000CB930
		public Socket EndAcceptSocket(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "EndAcceptSocket", null);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			Socket socket = (lazyAsyncResult == null) ? null : (lazyAsyncResult.AsyncObject as Socket);
			if (socket == null)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			Socket socket2 = socket.EndAccept(asyncResult);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "EndAcceptSocket", socket2);
			}
			return socket2;
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x000CC9B8 File Offset: 0x000CB9B8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginAcceptTcpClient(AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginAcceptTcpClient", null);
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_stopped"));
			}
			IAsyncResult result = this.m_ServerSocket.BeginAccept(callback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginAcceptTcpClient", null);
			}
			return result;
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x000CCA1C File Offset: 0x000CBA1C
		public TcpClient EndAcceptTcpClient(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "EndAcceptTcpClient", null);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			Socket socket = (lazyAsyncResult == null) ? null : (lazyAsyncResult.AsyncObject as Socket);
			if (socket == null)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			Socket socket2 = socket.EndAccept(asyncResult);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "EndAcceptTcpClient", socket2);
			}
			return new TcpClient(socket2);
		}

		// Token: 0x04002C1F RID: 11295
		private IPEndPoint m_ServerSocketEP;

		// Token: 0x04002C20 RID: 11296
		private Socket m_ServerSocket;

		// Token: 0x04002C21 RID: 11297
		private bool m_Active;

		// Token: 0x04002C22 RID: 11298
		private bool m_ExclusiveAddressUse;
	}
}
