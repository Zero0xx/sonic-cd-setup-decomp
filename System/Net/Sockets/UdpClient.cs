using System;
using System.Security.Permissions;

namespace System.Net.Sockets
{
	// Token: 0x020005CA RID: 1482
	public class UdpClient : IDisposable
	{
		// Token: 0x06002E65 RID: 11877 RVA: 0x000CCAA6 File Offset: 0x000CBAA6
		public UdpClient() : this(AddressFamily.InterNetwork)
		{
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000CCAB0 File Offset: 0x000CBAB0
		public UdpClient(AddressFamily family)
		{
			this.m_Buffer = new byte[65536];
			this.m_Family = AddressFamily.InterNetwork;
			base..ctor();
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_family", new object[]
				{
					"UDP"
				}), "family");
			}
			this.m_Family = family;
			this.createClientSocket();
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000CCB15 File Offset: 0x000CBB15
		public UdpClient(int port) : this(port, AddressFamily.InterNetwork)
		{
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000CCB20 File Offset: 0x000CBB20
		public UdpClient(int port, AddressFamily family)
		{
			this.m_Buffer = new byte[65536];
			this.m_Family = AddressFamily.InterNetwork;
			base..ctor();
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_family"), "family");
			}
			this.m_Family = family;
			IPEndPoint localEP;
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				localEP = new IPEndPoint(IPAddress.Any, port);
			}
			else
			{
				localEP = new IPEndPoint(IPAddress.IPv6Any, port);
			}
			this.createClientSocket();
			this.Client.Bind(localEP);
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000CCBB8 File Offset: 0x000CBBB8
		public UdpClient(IPEndPoint localEP)
		{
			this.m_Buffer = new byte[65536];
			this.m_Family = AddressFamily.InterNetwork;
			base..ctor();
			if (localEP == null)
			{
				throw new ArgumentNullException("localEP");
			}
			this.m_Family = localEP.AddressFamily;
			this.createClientSocket();
			this.Client.Bind(localEP);
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000CCC10 File Offset: 0x000CBC10
		public UdpClient(string hostname, int port)
		{
			this.m_Buffer = new byte[65536];
			this.m_Family = AddressFamily.InterNetwork;
			base..ctor();
			if (hostname == null)
			{
				throw new ArgumentNullException("hostname");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.Connect(hostname, port);
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06002E6B RID: 11883 RVA: 0x000CCC63 File Offset: 0x000CBC63
		// (set) Token: 0x06002E6C RID: 11884 RVA: 0x000CCC6B File Offset: 0x000CBC6B
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

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06002E6D RID: 11885 RVA: 0x000CCC74 File Offset: 0x000CBC74
		// (set) Token: 0x06002E6E RID: 11886 RVA: 0x000CCC7C File Offset: 0x000CBC7C
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

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06002E6F RID: 11887 RVA: 0x000CCC85 File Offset: 0x000CBC85
		public int Available
		{
			get
			{
				return this.m_ClientSocket.Available;
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06002E70 RID: 11888 RVA: 0x000CCC92 File Offset: 0x000CBC92
		// (set) Token: 0x06002E71 RID: 11889 RVA: 0x000CCC9F File Offset: 0x000CBC9F
		public short Ttl
		{
			get
			{
				return this.m_ClientSocket.Ttl;
			}
			set
			{
				this.m_ClientSocket.Ttl = value;
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06002E72 RID: 11890 RVA: 0x000CCCAD File Offset: 0x000CBCAD
		// (set) Token: 0x06002E73 RID: 11891 RVA: 0x000CCCBA File Offset: 0x000CBCBA
		public bool DontFragment
		{
			get
			{
				return this.m_ClientSocket.DontFragment;
			}
			set
			{
				this.m_ClientSocket.DontFragment = value;
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06002E74 RID: 11892 RVA: 0x000CCCC8 File Offset: 0x000CBCC8
		// (set) Token: 0x06002E75 RID: 11893 RVA: 0x000CCCD5 File Offset: 0x000CBCD5
		public bool MulticastLoopback
		{
			get
			{
				return this.m_ClientSocket.MulticastLoopback;
			}
			set
			{
				this.m_ClientSocket.MulticastLoopback = value;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06002E76 RID: 11894 RVA: 0x000CCCE3 File Offset: 0x000CBCE3
		// (set) Token: 0x06002E77 RID: 11895 RVA: 0x000CCCF0 File Offset: 0x000CBCF0
		public bool EnableBroadcast
		{
			get
			{
				return this.m_ClientSocket.EnableBroadcast;
			}
			set
			{
				this.m_ClientSocket.EnableBroadcast = value;
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06002E78 RID: 11896 RVA: 0x000CCCFE File Offset: 0x000CBCFE
		// (set) Token: 0x06002E79 RID: 11897 RVA: 0x000CCD0B File Offset: 0x000CBD0B
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

		// Token: 0x06002E7A RID: 11898 RVA: 0x000CCD19 File Offset: 0x000CBD19
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x000CCD24 File Offset: 0x000CBD24
		private void FreeResources()
		{
			if (this.m_CleanedUp)
			{
				return;
			}
			Socket client = this.Client;
			if (client != null)
			{
				client.InternalShutdown(SocketShutdown.Both);
				client.Close();
				this.Client = null;
			}
			this.m_CleanedUp = true;
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000CCD5F File Offset: 0x000CBD5F
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000CCD68 File Offset: 0x000CBD68
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.FreeResources();
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000CCD7C File Offset: 0x000CBD7C
		public void Connect(string hostname, int port)
		{
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
						socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					}
					if (Socket.OSSupportsIPv6)
					{
						socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
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
						if (NclUtilities.IsFatal(ex2))
						{
							throw;
						}
						ex = ex2;
					}
				}
			}
			catch (Exception ex3)
			{
				if (NclUtilities.IsFatal(ex3))
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
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000CCF48 File Offset: 0x000CBF48
		public void Connect(IPAddress addr, int port)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (addr == null)
			{
				throw new ArgumentNullException("addr");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			IPEndPoint endPoint = new IPEndPoint(addr, port);
			this.Connect(endPoint);
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000CCFA0 File Offset: 0x000CBFA0
		public void Connect(IPEndPoint endPoint)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			this.CheckForBroadcast(endPoint.Address);
			this.Client.Connect(endPoint);
			this.m_Active = true;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000CCFF3 File Offset: 0x000CBFF3
		private void CheckForBroadcast(IPAddress ipAddress)
		{
			if (this.Client != null && !this.m_IsBroadcast && ipAddress.IsBroadcast)
			{
				this.m_IsBroadcast = true;
				this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
			}
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000CD028 File Offset: 0x000CC028
		public int Send(byte[] dgram, int bytes, IPEndPoint endPoint)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (dgram == null)
			{
				throw new ArgumentNullException("dgram");
			}
			if (this.m_Active && endPoint != null)
			{
				throw new InvalidOperationException(SR.GetString("net_udpconnected"));
			}
			if (endPoint == null)
			{
				return this.Client.Send(dgram, 0, bytes, SocketFlags.None);
			}
			this.CheckForBroadcast(endPoint.Address);
			return this.Client.SendTo(dgram, 0, bytes, SocketFlags.None, endPoint);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000CD0A8 File Offset: 0x000CC0A8
		public int Send(byte[] dgram, int bytes, string hostname, int port)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (dgram == null)
			{
				throw new ArgumentNullException("dgram");
			}
			if (this.m_Active && (hostname != null || port != 0))
			{
				throw new InvalidOperationException(SR.GetString("net_udpconnected"));
			}
			if (hostname == null || port == 0)
			{
				return this.Client.Send(dgram, 0, bytes, SocketFlags.None);
			}
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
			int num = 0;
			while (num < hostAddresses.Length && hostAddresses[num].AddressFamily != this.m_Family)
			{
				num++;
			}
			if (hostAddresses.Length == 0 || num == hostAddresses.Length)
			{
				throw new ArgumentException(SR.GetString("net_invalidAddressList"), "hostname");
			}
			this.CheckForBroadcast(hostAddresses[num]);
			IPEndPoint remoteEP = new IPEndPoint(hostAddresses[num], port);
			return this.Client.SendTo(dgram, 0, bytes, SocketFlags.None, remoteEP);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000CD17C File Offset: 0x000CC17C
		public int Send(byte[] dgram, int bytes)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (dgram == null)
			{
				throw new ArgumentNullException("dgram");
			}
			if (!this.m_Active)
			{
				throw new InvalidOperationException(SR.GetString("net_notconnected"));
			}
			return this.Client.Send(dgram, 0, bytes, SocketFlags.None);
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000CD1D8 File Offset: 0x000CC1D8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] datagram, int bytes, IPEndPoint endPoint, AsyncCallback requestCallback, object state)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (datagram == null)
			{
				throw new ArgumentNullException("datagram");
			}
			if (bytes > datagram.Length)
			{
				throw new ArgumentOutOfRangeException("bytes");
			}
			if (this.m_Active && endPoint != null)
			{
				throw new InvalidOperationException(SR.GetString("net_udpconnected"));
			}
			if (endPoint == null)
			{
				return this.Client.BeginSend(datagram, 0, bytes, SocketFlags.None, requestCallback, state);
			}
			this.CheckForBroadcast(endPoint.Address);
			return this.Client.BeginSendTo(datagram, 0, bytes, SocketFlags.None, endPoint, requestCallback, state);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000CD270 File Offset: 0x000CC270
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] datagram, int bytes, string hostname, int port, AsyncCallback requestCallback, object state)
		{
			if (this.m_Active && (hostname != null || port != 0))
			{
				throw new InvalidOperationException(SR.GetString("net_udpconnected"));
			}
			IPEndPoint endPoint = null;
			if (hostname != null && port != 0)
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
				int num = 0;
				while (num < hostAddresses.Length && hostAddresses[num].AddressFamily != this.m_Family)
				{
					num++;
				}
				if (hostAddresses.Length == 0 || num == hostAddresses.Length)
				{
					throw new ArgumentException(SR.GetString("net_invalidAddressList"), "hostname");
				}
				this.CheckForBroadcast(hostAddresses[num]);
				endPoint = new IPEndPoint(hostAddresses[num], port);
			}
			return this.BeginSend(datagram, bytes, endPoint, requestCallback, state);
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000CD30B File Offset: 0x000CC30B
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] datagram, int bytes, AsyncCallback requestCallback, object state)
		{
			return this.BeginSend(datagram, bytes, null, requestCallback, state);
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000CD319 File Offset: 0x000CC319
		public int EndSend(IAsyncResult asyncResult)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this.m_Active)
			{
				return this.Client.EndSend(asyncResult);
			}
			return this.Client.EndSendTo(asyncResult);
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000CD358 File Offset: 0x000CC358
		public byte[] Receive(ref IPEndPoint remoteEP)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			EndPoint endPoint;
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				endPoint = IPEndPoint.Any;
			}
			else
			{
				endPoint = IPEndPoint.IPv6Any;
			}
			int num = this.Client.ReceiveFrom(this.m_Buffer, 65536, SocketFlags.None, ref endPoint);
			remoteEP = (IPEndPoint)endPoint;
			if (num < 65536)
			{
				byte[] array = new byte[num];
				Buffer.BlockCopy(this.m_Buffer, 0, array, 0, num);
				return array;
			}
			return this.m_Buffer;
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000CD3E0 File Offset: 0x000CC3E0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceive(AsyncCallback requestCallback, object state)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			EndPoint endPoint;
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				endPoint = IPEndPoint.Any;
			}
			else
			{
				endPoint = IPEndPoint.IPv6Any;
			}
			return this.Client.BeginReceiveFrom(this.m_Buffer, 0, 65536, SocketFlags.None, ref endPoint, requestCallback, state);
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000CD43C File Offset: 0x000CC43C
		public byte[] EndReceive(IAsyncResult asyncResult, ref IPEndPoint remoteEP)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			EndPoint endPoint;
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				endPoint = IPEndPoint.Any;
			}
			else
			{
				endPoint = IPEndPoint.IPv6Any;
			}
			int num = this.Client.EndReceiveFrom(asyncResult, ref endPoint);
			remoteEP = (IPEndPoint)endPoint;
			if (num < 65536)
			{
				byte[] array = new byte[num];
				Buffer.BlockCopy(this.m_Buffer, 0, array, 0, num);
				return array;
			}
			return this.m_Buffer;
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000CD4B8 File Offset: 0x000CC4B8
		public void JoinMulticastGroup(IPAddress multicastAddr)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (multicastAddr.AddressFamily != this.m_Family)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_multicast_family", new object[]
				{
					"UDP"
				}), "multicastAddr");
			}
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				MulticastOption optionValue = new MulticastOption(multicastAddr);
				this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, optionValue);
				return;
			}
			IPv6MulticastOption optionValue2 = new IPv6MulticastOption(multicastAddr);
			this.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, optionValue2);
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000CD558 File Offset: 0x000CC558
		public void JoinMulticastGroup(IPAddress multicastAddr, IPAddress localAddress)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this.m_Family != AddressFamily.InterNetwork)
			{
				throw new SocketException(SocketError.OperationNotSupported);
			}
			MulticastOption optionValue = new MulticastOption(multicastAddr, localAddress);
			this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, optionValue);
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000CD5AC File Offset: 0x000CC5AC
		public void JoinMulticastGroup(int ifindex, IPAddress multicastAddr)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (ifindex < 0)
			{
				throw new ArgumentException(SR.GetString("net_value_cannot_be_negative"), "ifindex");
			}
			if (this.m_Family != AddressFamily.InterNetworkV6)
			{
				throw new SocketException(SocketError.OperationNotSupported);
			}
			IPv6MulticastOption optionValue = new IPv6MulticastOption(multicastAddr, (long)ifindex);
			this.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, optionValue);
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x000CD628 File Offset: 0x000CC628
		public void JoinMulticastGroup(IPAddress multicastAddr, int timeToLive)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (!ValidationHelper.ValidateRange(timeToLive, 0, 255))
			{
				throw new ArgumentOutOfRangeException("timeToLive");
			}
			this.JoinMulticastGroup(multicastAddr);
			this.Client.SetSocketOption((this.m_Family == AddressFamily.InterNetwork) ? SocketOptionLevel.IP : SocketOptionLevel.IPv6, SocketOptionName.MulticastTimeToLive, timeToLive);
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x000CD698 File Offset: 0x000CC698
		public void DropMulticastGroup(IPAddress multicastAddr)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (multicastAddr.AddressFamily != this.m_Family)
			{
				throw new ArgumentException(SR.GetString("net_protocol_invalid_multicast_family", new object[]
				{
					"UDP"
				}), "multicastAddr");
			}
			if (this.m_Family == AddressFamily.InterNetwork)
			{
				MulticastOption optionValue = new MulticastOption(multicastAddr);
				this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, optionValue);
				return;
			}
			IPv6MulticastOption optionValue2 = new IPv6MulticastOption(multicastAddr);
			this.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, optionValue2);
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x000CD738 File Offset: 0x000CC738
		public void DropMulticastGroup(IPAddress multicastAddr, int ifindex)
		{
			if (this.m_CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}
			if (ifindex < 0)
			{
				throw new ArgumentException(SR.GetString("net_value_cannot_be_negative"), "ifindex");
			}
			if (this.m_Family != AddressFamily.InterNetworkV6)
			{
				throw new SocketException(SocketError.OperationNotSupported);
			}
			IPv6MulticastOption optionValue = new IPv6MulticastOption(multicastAddr, (long)ifindex);
			this.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, optionValue);
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x000CD7B3 File Offset: 0x000CC7B3
		private void createClientSocket()
		{
			this.Client = new Socket(this.m_Family, SocketType.Dgram, ProtocolType.Udp);
		}

		// Token: 0x04002C2A RID: 11306
		private const int MaxUDPSize = 65536;

		// Token: 0x04002C2B RID: 11307
		private Socket m_ClientSocket;

		// Token: 0x04002C2C RID: 11308
		private bool m_Active;

		// Token: 0x04002C2D RID: 11309
		private byte[] m_Buffer;

		// Token: 0x04002C2E RID: 11310
		private AddressFamily m_Family;

		// Token: 0x04002C2F RID: 11311
		private bool m_CleanedUp;

		// Token: 0x04002C30 RID: 11312
		private bool m_IsBroadcast;
	}
}
