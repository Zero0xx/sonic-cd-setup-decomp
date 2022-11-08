using System;
using System.Security.Permissions;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x020005C7 RID: 1479
	public class TcpClient : IDisposable
	{
		// Token: 0x06002E2B RID: 11819 RVA: 0x000CBA5C File Offset: 0x000CAA5C
		public TcpClient(IPEndPoint localEP)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", localEP);
			}
			if (localEP == null)
			{
				throw new ArgumentNullException("localEP");
			}
			this.m_Family = localEP.AddressFamily;
			this.initialize();
			this.Client.Bind(localEP);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", "");
			}
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000CBAD6 File Offset: 0x000CAAD6
		public TcpClient() : this(AddressFamily.InterNetwork)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", null);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", null);
			}
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000CBB10 File Offset: 0x000CAB10
		public TcpClient(AddressFamily family)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", family);
			}
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_family", new object[]
				{
					"TCP"
				}), "family");
			}
			this.m_Family = family;
			this.initialize();
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", null);
			}
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x000CBB9C File Offset: 0x000CAB9C
		public TcpClient(string hostname, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", hostname);
			}
			if (hostname == null)
			{
				throw new ArgumentNullException("hostname");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			try
			{
				this.Connect(hostname, port);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (this.m_ClientSocket != null)
				{
					this.m_ClientSocket.Close();
				}
				throw ex;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", null);
			}
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000CBC54 File Offset: 0x000CAC54
		internal TcpClient(Socket acceptedSocket)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "TcpClient", acceptedSocket);
			}
			this.Client = acceptedSocket;
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "TcpClient", null);
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06002E30 RID: 11824 RVA: 0x000CBCAC File Offset: 0x000CACAC
		// (set) Token: 0x06002E31 RID: 11825 RVA: 0x000CBCB4 File Offset: 0x000CACB4
		public Socket Client
		{
			get
			{
				return this.m_ClientSocket;
			}
			set
			{
				this.m_ClientSocket = value;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06002E32 RID: 11826 RVA: 0x000CBCBD File Offset: 0x000CACBD
		// (set) Token: 0x06002E33 RID: 11827 RVA: 0x000CBCC5 File Offset: 0x000CACC5
		protected bool Active
		{
			get
			{
				return this.m_Active;
			}
			set
			{
				this.m_Active = value;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06002E34 RID: 11828 RVA: 0x000CBCCE File Offset: 0x000CACCE
		public int Available
		{
			get
			{
				return this.m_ClientSocket.Available;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06002E35 RID: 11829 RVA: 0x000CBCDB File Offset: 0x000CACDB
		public bool Connected
		{
			get
			{
				return this.m_ClientSocket.Connected;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06002E36 RID: 11830 RVA: 0x000CBCE8 File Offset: 0x000CACE8
		// (set) Token: 0x06002E37 RID: 11831 RVA: 0x000CBCF5 File Offset: 0x000CACF5
		public bool ExclusiveAddressUse
		{
			get
			{
				return this.m_ClientSocket.ExclusiveAddressUse;
			}
			set
			{
				this.m_ClientSocket.ExclusiveAddressUse = value;
			}
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000CBD04 File Offset: 0x000CAD04
		public void Connect(string hostname, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", hostname);
			}
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (hostname == null)
			{
				throw new ArgumentNullException("hostname");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (this.m_Active)
			{
				throw new SocketException(SocketError.IsConnected);
			}
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
			Exception ex = null;
			Socket socket = null;
			Socket socket2 = null;
			try
			{
				if (this.m_ClientSocket == null)
				{
					if (Socket.SupportsIPv4)
					{
						socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					}
					if (Socket.OSSupportsIPv6)
					{
						socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
					}
				}
				foreach (IPAddress ipaddress in hostAddresses)
				{
					try
					{
						if (this.m_ClientSocket == null)
						{
							if (ipaddress.AddressFamily == AddressFamily.InterNetwork && socket2 != null)
							{
								socket2.Connect(ipaddress, port);
								this.m_ClientSocket = socket2;
								if (socket != null)
								{
									socket.Close();
								}
							}
							else if (socket != null)
							{
								socket.Connect(ipaddress, port);
								this.m_ClientSocket = socket;
								if (socket2 != null)
								{
									socket2.Close();
								}
							}
							this.m_Family = ipaddress.AddressFamily;
							this.m_Active = true;
							break;
						}
						if (ipaddress.AddressFamily == this.m_Family)
						{
							this.Connect(new IPEndPoint(ipaddress, port));
							this.m_Active = true;
							break;
						}
					}
					catch (Exception ex2)
					{
						if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
						{
							throw;
						}
						ex = ex2;
					}
				}
			}
			catch (Exception ex3)
			{
				if (ex3 is ThreadAbortException || ex3 is StackOverflowException || ex3 is OutOfMemoryException)
				{
					throw;
				}
				ex = ex3;
			}
			finally
			{
				if (!this.m_Active)
				{
					if (socket != null)
					{
						socket.Close();
					}
					if (socket2 != null)
					{
						socket2.Close();
					}
					if (ex != null)
					{
						throw ex;
					}
					throw new SocketException(SocketError.NotConnected);
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x000CBF34 File Offset: 0x000CAF34
		public void Connect(IPAddress address, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", address);
			}
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			IPEndPoint remoteEP = new IPEndPoint(address, port);
			this.Connect(remoteEP);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000CBFBC File Offset: 0x000CAFBC
		public void Connect(IPEndPoint remoteEP)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", remoteEP);
			}
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			this.Client.Connect(remoteEP);
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x000CC034 File Offset: 0x000CB034
		public void Connect(IPAddress[] ipAddresses, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", ipAddresses);
			}
			this.Client.Connect(ipAddresses, port);
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x000CC088 File Offset: 0x000CB088
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", host);
			}
			IAsyncResult result = this.Client.BeginConnect(host, port, requestCallback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", null);
			}
			return result;
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000CC0D8 File Offset: 0x000CB0D8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", address);
			}
			IAsyncResult result = this.Client.BeginConnect(address, port, requestCallback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", null);
			}
			return result;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000CC128 File Offset: 0x000CB128
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", addresses);
			}
			IAsyncResult result = this.Client.BeginConnect(addresses, port, requestCallback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", null);
			}
			return result;
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000CC178 File Offset: 0x000CB178
		public void EndConnect(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "EndConnect", asyncResult);
			}
			this.Client.EndConnect(asyncResult);
			this.m_Active = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "EndConnect", null);
			}
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000CC1C8 File Offset: 0x000CB1C8
		public NetworkStream GetStream()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "GetStream", "");
			}
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!this.Client.Connected)
			{
				throw new InvalidOperationException(SR.GetString("net_notconnected"));
			}
			if (this.m_DataStream == null)
			{
				this.m_DataStream = new NetworkStream(this.Client, true);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "GetStream", this.m_DataStream);
			}
			return this.m_DataStream;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000CC264 File Offset: 0x000CB264
		public void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Close", "");
			}
			((IDisposable)this).Dispose();
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Close", "");
			}
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000CC2A4 File Offset: 0x000CB2A4
		protected virtual void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Sockets, this, "Dispose", "");
			}
			if (this.m_CleanedUp)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Sockets, this, "Dispose", "");
				}
				return;
			}
			if (disposing)
			{
				IDisposable dataStream = this.m_DataStream;
				if (dataStream != null)
				{
					dataStream.Dispose();
				}
				else
				{
					Socket client = this.Client;
					if (client != null)
					{
						try
						{
							client.InternalShutdown(SocketShutdown.Both);
						}
						finally
						{
							client.Close();
							this.Client = null;
						}
					}
				}
				GC.SuppressFinalize(this);
			}
			this.m_CleanedUp = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.Sockets, this, "Dispose", "");
			}
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000CC364 File Offset: 0x000CB364
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000CC370 File Offset: 0x000CB370
		~TcpClient()
		{
			this.Dispose(false);
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06002E45 RID: 11845 RVA: 0x000CC3A0 File Offset: 0x000CB3A0
		// (set) Token: 0x06002E46 RID: 11846 RVA: 0x000CC3B2 File Offset: 0x000CB3B2
		public int ReceiveBufferSize
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06002E47 RID: 11847 RVA: 0x000CC3CA File Offset: 0x000CB3CA
		// (set) Token: 0x06002E48 RID: 11848 RVA: 0x000CC3DC File Offset: 0x000CB3DC
		public int SendBufferSize
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06002E49 RID: 11849 RVA: 0x000CC3F4 File Offset: 0x000CB3F4
		// (set) Token: 0x06002E4A RID: 11850 RVA: 0x000CC406 File Offset: 0x000CB406
		public int ReceiveTimeout
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06002E4B RID: 11851 RVA: 0x000CC41E File Offset: 0x000CB41E
		// (set) Token: 0x06002E4C RID: 11852 RVA: 0x000CC430 File Offset: 0x000CB430
		public int SendTimeout
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06002E4D RID: 11853 RVA: 0x000CC448 File Offset: 0x000CB448
		// (set) Token: 0x06002E4E RID: 11854 RVA: 0x000CC464 File Offset: 0x000CB464
		public LingerOption LingerState
		{
			get
			{
				return (LingerOption)this.Client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06002E4F RID: 11855 RVA: 0x000CC47C File Offset: 0x000CB47C
		// (set) Token: 0x06002E50 RID: 11856 RVA: 0x000CC48B File Offset: 0x000CB48B
		public bool NoDelay
		{
			get
			{
				return this.numericOption(SocketOptionLevel.Tcp, SocketOptionName.Debug) != 0;
			}
			set
			{
				this.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
			}
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000CC4A1 File Offset: 0x000CB4A1
		private void initialize()
		{
			this.Client = new Socket(this.m_Family, SocketType.Stream, ProtocolType.Tcp);
			this.m_Active = false;
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000CC4BD File Offset: 0x000CB4BD
		private int numericOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
		{
			return (int)this.Client.GetSocketOption(optionLevel, optionName);
		}

		// Token: 0x04002C1A RID: 11290
		private Socket m_ClientSocket;

		// Token: 0x04002C1B RID: 11291
		private bool m_Active;

		// Token: 0x04002C1C RID: 11292
		private NetworkStream m_DataStream;

		// Token: 0x04002C1D RID: 11293
		private AddressFamily m_Family = AddressFamily.InterNetwork;

		// Token: 0x04002C1E RID: 11294
		private bool m_CleanedUp;
	}
}
