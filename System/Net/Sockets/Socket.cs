using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x020005B1 RID: 1457
	public class Socket : IDisposable
	{
		// Token: 0x06002CDC RID: 11484 RVA: 0x000C1C4C File Offset: 0x000C0C4C
		public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
		{
			Socket.s_LoggingEnabled = Logging.On;
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Socket", addressFamily);
			}
			Socket.InitializeSockets();
			this.m_Handle = SafeCloseSocket.CreateWSASocket(addressFamily, socketType, protocolType);
			if (this.m_Handle.IsInvalid)
			{
				throw new SocketException();
			}
			this.addressFamily = addressFamily;
			this.socketType = socketType;
			this.protocolType = protocolType;
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Socket", null);
			}
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000C1CF0 File Offset: 0x000C0CF0
		public unsafe Socket(SocketInformation socketInformation)
		{
			Socket.s_LoggingEnabled = Logging.On;
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Socket", this.addressFamily);
			}
			ExceptionHelper.UnrestrictedSocketPermission.Demand();
			Socket.InitializeSockets();
			if (socketInformation.ProtocolInformation == null || socketInformation.ProtocolInformation.Length < Socket.protocolInformationSize)
			{
				throw new ArgumentException(SR.GetString("net_sockets_invalid_socketinformation"), "socketInformation.ProtocolInformation");
			}
			fixed (byte* protocolInformation = socketInformation.ProtocolInformation)
			{
				this.m_Handle = SafeCloseSocket.CreateWSASocket(protocolInformation);
				UnsafeNclNativeMethods.OSSOCK.WSAPROTOCOL_INFO wsaprotocol_INFO = (UnsafeNclNativeMethods.OSSOCK.WSAPROTOCOL_INFO)Marshal.PtrToStructure((IntPtr)((void*)protocolInformation), typeof(UnsafeNclNativeMethods.OSSOCK.WSAPROTOCOL_INFO));
				this.addressFamily = wsaprotocol_INFO.iAddressFamily;
				this.socketType = (SocketType)wsaprotocol_INFO.iSocketType;
				this.protocolType = (ProtocolType)wsaprotocol_INFO.iProtocol;
			}
			if (this.m_Handle.IsInvalid)
			{
				SocketException ex = new SocketException();
				if (ex.ErrorCode == 10022)
				{
					throw new ArgumentException(SR.GetString("net_sockets_invalid_socketinformation"), "socketInformation");
				}
				throw ex;
			}
			else
			{
				if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
				{
					throw new NotSupportedException(SR.GetString("net_invalidversion"));
				}
				this.m_IsConnected = socketInformation.IsConnected;
				this.willBlock = !socketInformation.IsNonBlocking;
				this.InternalSetBlocking(this.willBlock);
				this.isListening = socketInformation.IsListening;
				this.UseOnlyOverlappedIO = socketInformation.UseOnlyOverlappedIO;
				EndPoint endPoint = null;
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					endPoint = IPEndPoint.Any;
				}
				else if (this.addressFamily == AddressFamily.InterNetworkV6)
				{
					endPoint = IPEndPoint.IPv6Any;
				}
				SocketAddress socketAddress = endPoint.Serialize();
				SocketError socketError;
				try
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.getsockname(this.m_Handle, socketAddress.m_Buffer, ref socketAddress.m_Size);
				}
				catch (ObjectDisposedException)
				{
					socketError = SocketError.NotSocket;
				}
				if (socketError == SocketError.Success)
				{
					try
					{
						this.m_RightEndPoint = endPoint.Create(socketAddress);
					}
					catch
					{
					}
				}
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exit(Logging.Sockets, this, "Socket", null);
				}
				return;
			}
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000C1F30 File Offset: 0x000C0F30
		private Socket(SafeCloseSocket fd)
		{
			Socket.s_LoggingEnabled = Logging.On;
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Socket", null);
			}
			Socket.InitializeSockets();
			if (fd == null || fd.IsInvalid)
			{
				throw new ArgumentException(SR.GetString("net_InvalidSocketHandle"));
			}
			this.m_Handle = fd;
			this.addressFamily = AddressFamily.Unknown;
			this.socketType = SocketType.Unknown;
			this.protocolType = ProtocolType.Unknown;
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Socket", null);
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06002CDF RID: 11487 RVA: 0x000C1FCE File Offset: 0x000C0FCE
		public static bool SupportsIPv4
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_SupportsIPv4;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06002CE0 RID: 11488 RVA: 0x000C1FDA File Offset: 0x000C0FDA
		[Obsolete("SupportsIPv6 is obsoleted for this type, please use OSSupportsIPv6 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static bool SupportsIPv6
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_SupportsIPv6;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06002CE1 RID: 11489 RVA: 0x000C1FE6 File Offset: 0x000C0FE6
		internal static bool LegacySupportsIPv6
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_SupportsIPv6;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x000C1FF2 File Offset: 0x000C0FF2
		public static bool OSSupportsIPv6
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_OSSupportsIPv6;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002CE3 RID: 11491 RVA: 0x000C2000 File Offset: 0x000C1000
		public int Available
		{
			get
			{
				if (this.CleanedUp)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				int result = 0;
				SocketError socketError = UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.m_Handle, 1074030207, ref result);
				if (socketError == SocketError.SocketError)
				{
					SocketException ex = new SocketException();
					this.UpdateStatusAfterSocketError(ex);
					if (Socket.s_LoggingEnabled)
					{
						Logging.Exception(Logging.Sockets, this, "Available", ex);
					}
					throw ex;
				}
				return result;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06002CE4 RID: 11492 RVA: 0x000C2068 File Offset: 0x000C1068
		public EndPoint LocalEndPoint
		{
			get
			{
				if (this.CleanedUp)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.m_NonBlockingConnectInProgress && this.Poll(0, SelectMode.SelectWrite))
				{
					this.m_IsConnected = true;
					this.m_RightEndPoint = this.m_NonBlockingConnectRightEndPoint;
					this.m_NonBlockingConnectInProgress = false;
				}
				if (this.m_RightEndPoint == null)
				{
					return null;
				}
				SocketAddress socketAddress = this.m_RightEndPoint.Serialize();
				SocketError socketError = UnsafeNclNativeMethods.OSSOCK.getsockname(this.m_Handle, socketAddress.m_Buffer, ref socketAddress.m_Size);
				if (socketError != SocketError.Success)
				{
					SocketException ex = new SocketException();
					this.UpdateStatusAfterSocketError(ex);
					if (Socket.s_LoggingEnabled)
					{
						Logging.Exception(Logging.Sockets, this, "LocalEndPoint", ex);
					}
					throw ex;
				}
				return this.m_RightEndPoint.Create(socketAddress);
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06002CE5 RID: 11493 RVA: 0x000C2120 File Offset: 0x000C1120
		public EndPoint RemoteEndPoint
		{
			get
			{
				if (this.CleanedUp)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.m_RemoteEndPoint == null)
				{
					if (this.m_NonBlockingConnectInProgress && this.Poll(0, SelectMode.SelectWrite))
					{
						this.m_IsConnected = true;
						this.m_RightEndPoint = this.m_NonBlockingConnectRightEndPoint;
						this.m_NonBlockingConnectInProgress = false;
					}
					if (this.m_RightEndPoint == null)
					{
						return null;
					}
					SocketAddress socketAddress = this.m_RightEndPoint.Serialize();
					SocketError socketError = UnsafeNclNativeMethods.OSSOCK.getpeername(this.m_Handle, socketAddress.m_Buffer, ref socketAddress.m_Size);
					if (socketError != SocketError.Success)
					{
						SocketException ex = new SocketException();
						this.UpdateStatusAfterSocketError(ex);
						if (Socket.s_LoggingEnabled)
						{
							Logging.Exception(Logging.Sockets, this, "RemoteEndPoint", ex);
						}
						throw ex;
					}
					try
					{
						this.m_RemoteEndPoint = this.m_RightEndPoint.Create(socketAddress);
					}
					catch
					{
					}
				}
				return this.m_RemoteEndPoint;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06002CE6 RID: 11494 RVA: 0x000C2204 File Offset: 0x000C1204
		public IntPtr Handle
		{
			get
			{
				ExceptionHelper.UnmanagedPermission.Demand();
				return this.m_Handle.DangerousGetHandle();
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06002CE7 RID: 11495 RVA: 0x000C221B File Offset: 0x000C121B
		internal SafeCloseSocket SafeHandle
		{
			get
			{
				return this.m_Handle;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x000C2223 File Offset: 0x000C1223
		// (set) Token: 0x06002CE9 RID: 11497 RVA: 0x000C222C File Offset: 0x000C122C
		public bool Blocking
		{
			get
			{
				return this.willBlock;
			}
			set
			{
				if (this.CleanedUp)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				bool flag;
				SocketError socketError = this.InternalSetBlocking(value, out flag);
				if (socketError != SocketError.Success)
				{
					SocketException ex = new SocketException(socketError);
					this.UpdateStatusAfterSocketError(ex);
					if (Socket.s_LoggingEnabled)
					{
						Logging.Exception(Logging.Sockets, this, "Blocking", ex);
					}
					throw ex;
				}
				this.willBlock = flag;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06002CEA RID: 11498 RVA: 0x000C228E File Offset: 0x000C128E
		// (set) Token: 0x06002CEB RID: 11499 RVA: 0x000C2296 File Offset: 0x000C1296
		public bool UseOnlyOverlappedIO
		{
			get
			{
				return this.useOverlappedIO;
			}
			set
			{
				if (this.m_BoundToThreadPool)
				{
					throw new InvalidOperationException(SR.GetString("net_io_completionportwasbound"));
				}
				this.useOverlappedIO = value;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002CEC RID: 11500 RVA: 0x000C22B7 File Offset: 0x000C12B7
		public bool Connected
		{
			get
			{
				if (this.m_NonBlockingConnectInProgress && this.Poll(0, SelectMode.SelectWrite))
				{
					this.m_IsConnected = true;
					this.m_RightEndPoint = this.m_NonBlockingConnectRightEndPoint;
					this.m_NonBlockingConnectInProgress = false;
				}
				return this.m_IsConnected;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06002CED RID: 11501 RVA: 0x000C22EB File Offset: 0x000C12EB
		public AddressFamily AddressFamily
		{
			get
			{
				return this.addressFamily;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06002CEE RID: 11502 RVA: 0x000C22F3 File Offset: 0x000C12F3
		public SocketType SocketType
		{
			get
			{
				return this.socketType;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06002CEF RID: 11503 RVA: 0x000C22FB File Offset: 0x000C12FB
		public ProtocolType ProtocolType
		{
			get
			{
				return this.protocolType;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x000C2303 File Offset: 0x000C1303
		public bool IsBound
		{
			get
			{
				return this.m_RightEndPoint != null;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06002CF1 RID: 11505 RVA: 0x000C2311 File Offset: 0x000C1311
		// (set) Token: 0x06002CF2 RID: 11506 RVA: 0x000C232A File Offset: 0x000C132A
		public bool ExclusiveAddressUse
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse) != 0;
			}
			set
			{
				if (this.IsBound)
				{
					throw new InvalidOperationException(SR.GetString("net_sockets_mustnotbebound"));
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, value ? 1 : 0);
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x000C2358 File Offset: 0x000C1358
		// (set) Token: 0x06002CF4 RID: 11508 RVA: 0x000C236F File Offset: 0x000C136F
		public int ReceiveBufferSize
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x000C2391 File Offset: 0x000C1391
		// (set) Token: 0x06002CF6 RID: 11510 RVA: 0x000C23A8 File Offset: 0x000C13A8
		public int SendBufferSize
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x000C23CA File Offset: 0x000C13CA
		// (set) Token: 0x06002CF8 RID: 11512 RVA: 0x000C23E1 File Offset: 0x000C13E1
		public int ReceiveTimeout
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value == -1)
				{
					value = 0;
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x000C240A File Offset: 0x000C140A
		// (set) Token: 0x06002CFA RID: 11514 RVA: 0x000C2421 File Offset: 0x000C1421
		public int SendTimeout
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value == -1)
				{
					value = 0;
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06002CFB RID: 11515 RVA: 0x000C244A File Offset: 0x000C144A
		// (set) Token: 0x06002CFC RID: 11516 RVA: 0x000C2461 File Offset: 0x000C1461
		public LingerOption LingerState
		{
			get
			{
				return (LingerOption)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
			}
			set
			{
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06002CFD RID: 11517 RVA: 0x000C2474 File Offset: 0x000C1474
		// (set) Token: 0x06002CFE RID: 11518 RVA: 0x000C2488 File Offset: 0x000C1488
		public bool NoDelay
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug) != 0;
			}
			set
			{
				this.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x000C249C File Offset: 0x000C149C
		// (set) Token: 0x06002D00 RID: 11520 RVA: 0x000C24EC File Offset: 0x000C14EC
		public short Ttl
		{
			get
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					return (short)((int)this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress));
				}
				if (this.addressFamily == AddressFamily.InterNetworkV6)
				{
					return (short)((int)this.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.ReuseAddress));
				}
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			set
			{
				if (value < -1 || (value == -1 && this.addressFamily != AddressFamily.InterNetworkV6))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, (int)value);
					return;
				}
				if (this.addressFamily == AddressFamily.InterNetworkV6)
				{
					this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.ReuseAddress, (int)value);
					return;
				}
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x000C254D File Offset: 0x000C154D
		// (set) Token: 0x06002D02 RID: 11522 RVA: 0x000C257B File Offset: 0x000C157B
		public bool DontFragment
		{
			get
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					return (int)this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment) != 0;
				}
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			set
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment, value ? 1 : 0);
					return;
				}
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06002D03 RID: 11523 RVA: 0x000C25A8 File Offset: 0x000C15A8
		// (set) Token: 0x06002D04 RID: 11524 RVA: 0x000C2600 File Offset: 0x000C1600
		public bool MulticastLoopback
		{
			get
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					return (int)this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback) != 0;
				}
				if (this.addressFamily == AddressFamily.InterNetworkV6)
				{
					return (int)this.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback) != 0;
				}
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			set
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, value ? 1 : 0);
					return;
				}
				if (this.addressFamily == AddressFamily.InterNetworkV6)
				{
					this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback, value ? 1 : 0);
					return;
				}
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06002D05 RID: 11525 RVA: 0x000C2652 File Offset: 0x000C1652
		// (set) Token: 0x06002D06 RID: 11526 RVA: 0x000C266B File Offset: 0x000C166B
		public bool EnableBroadcast
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast) != 0;
			}
			set
			{
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, value ? 1 : 0);
			}
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000C2684 File Offset: 0x000C1684
		public void Bind(EndPoint localEP)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Bind", localEP);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (localEP == null)
			{
				throw new ArgumentNullException("localEP");
			}
			EndPoint endPoint = localEP;
			IPEndPoint ipendPoint = localEP as IPEndPoint;
			if (ipendPoint != null)
			{
				ipendPoint = ipendPoint.Snapshot();
				endPoint = ipendPoint;
				SocketPermission socketPermission = new SocketPermission(NetworkAccess.Accept, this.Transport, ipendPoint.Address.ToString(), ipendPoint.Port);
				socketPermission.Demand();
			}
			else
			{
				ExceptionHelper.UnmanagedPermission.Demand();
			}
			SocketAddress socketAddress = endPoint.Serialize();
			this.DoBind(endPoint, socketAddress);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Bind", "");
			}
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000C2744 File Offset: 0x000C1744
		internal void InternalBind(EndPoint localEP)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "InternalBind", localEP);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			EndPoint endPointSnapshot = localEP;
			SocketAddress socketAddress = this.SnapshotAndSerialize(ref endPointSnapshot);
			this.DoBind(endPointSnapshot, socketAddress);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "InternalBind", "");
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000C27B4 File Offset: 0x000C17B4
		private void DoBind(EndPoint endPointSnapshot, SocketAddress socketAddress)
		{
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.bind(this.m_Handle, socketAddress.m_Buffer, socketAddress.m_Size);
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "DoBind", ex);
				}
				throw ex;
			}
			if (this.m_RightEndPoint == null)
			{
				this.m_RightEndPoint = endPointSnapshot;
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000C2814 File Offset: 0x000C1814
		public void Connect(EndPoint remoteEP)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (this.m_IsDisconnected)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_disconnectedConnect"));
			}
			if (this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
			}
			this.ValidateBlockingMode();
			EndPoint endPoint = remoteEP;
			SocketAddress socketAddress = this.CheckCacheRemote(ref endPoint, true);
			if (!this.Blocking)
			{
				this.m_NonBlockingConnectRightEndPoint = endPoint;
				this.m_NonBlockingConnectInProgress = true;
			}
			this.DoConnect(endPoint, socketAddress);
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000C28A8 File Offset: 0x000C18A8
		public void Connect(IPAddress address, int port)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", address);
			}
			if (this.CleanedUp)
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
			if (this.addressFamily != address.AddressFamily)
			{
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			IPEndPoint remoteEP = new IPEndPoint(address, port);
			this.Connect(remoteEP);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000C294C File Offset: 0x000C194C
		public void Connect(string host, int port)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", host);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
			this.Connect(hostAddresses, port);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000C29F8 File Offset: 0x000C19F8
		public void Connect(IPAddress[] addresses, int port)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", addresses);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (addresses.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_sockets_invalid_ipaddress_length"), "addresses");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			Exception ex = null;
			foreach (IPAddress ipaddress in addresses)
			{
				if (ipaddress.AddressFamily == this.addressFamily)
				{
					try
					{
						this.Connect(new IPEndPoint(ipaddress, port));
						ex = null;
						break;
					}
					catch (Exception ex2)
					{
						if (NclUtilities.IsFatal(ex2))
						{
							throw;
						}
						ex = ex2;
					}
					catch
					{
						ex = new Exception(SR.GetString("net_nonClsCompliantException"));
					}
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			if (!this.Connected)
			{
				throw new ArgumentException(SR.GetString("net_invalidAddressList"), "addresses");
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", null);
			}
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000C2B48 File Offset: 0x000C1B48
		public void Close()
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Close", null);
			}
			((IDisposable)this).Dispose();
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Close", null);
			}
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000C2B80 File Offset: 0x000C1B80
		public void Close(int timeout)
		{
			if (timeout < -1)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			this.m_CloseTimeout = timeout;
			((IDisposable)this).Dispose();
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000C2BA0 File Offset: 0x000C1BA0
		public void Listen(int backlog)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Listen", backlog);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.listen(this.m_Handle, backlog);
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Listen", ex);
				}
				throw ex;
			}
			this.isListening = true;
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Listen", "");
			}
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000C2C40 File Offset: 0x000C1C40
		public Socket Accept()
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Accept", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this.m_RightEndPoint == null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
			}
			if (!this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustlisten"));
			}
			if (this.m_IsDisconnected)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_disconnectedAccept"));
			}
			this.ValidateBlockingMode();
			SocketAddress socketAddress = this.m_RightEndPoint.Serialize();
			SafeCloseSocket safeCloseSocket = SafeCloseSocket.Accept(this.m_Handle, socketAddress.m_Buffer, ref socketAddress.m_Size);
			if (safeCloseSocket.IsInvalid)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Accept", ex);
				}
				throw ex;
			}
			Socket socket = this.CreateAcceptSocket(safeCloseSocket, this.m_RightEndPoint.Create(socketAddress), false);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Accept", socket);
			}
			return socket;
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000C2D51 File Offset: 0x000C1D51
		public int Send(byte[] buffer, int size, SocketFlags socketFlags)
		{
			return this.Send(buffer, 0, size, socketFlags);
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000C2D5D File Offset: 0x000C1D5D
		public int Send(byte[] buffer, SocketFlags socketFlags)
		{
			return this.Send(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags);
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000C2D71 File Offset: 0x000C1D71
		public int Send(byte[] buffer)
		{
			return this.Send(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None);
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000C2D85 File Offset: 0x000C1D85
		public int Send(IList<ArraySegment<byte>> buffers)
		{
			return this.Send(buffers, SocketFlags.None);
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000C2D90 File Offset: 0x000C1D90
		public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			SocketError socketError;
			int result = this.Send(buffers, socketFlags, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000C2DB4 File Offset: 0x000C1DB4
		public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Send", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (buffers.Count == 0)
			{
				throw new ArgumentException(SR.GetString("net_sockets_zerolist", new object[]
				{
					"buffers"
				}), "buffers");
			}
			this.ValidateBlockingMode();
			errorCode = SocketError.Success;
			int count = buffers.Count;
			WSABuffer[] array = new WSABuffer[count];
			GCHandle[] array2 = null;
			int num;
			try
			{
				array2 = new GCHandle[count];
				for (int i = 0; i < count; i++)
				{
					ArraySegment<byte> segment = buffers[i];
					ValidationHelper.ValidateSegment(segment);
					array2[i] = GCHandle.Alloc(segment.Array, GCHandleType.Pinned);
					array[i].Length = segment.Count;
					array[i].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(segment.Array, segment.Offset);
				}
				errorCode = UnsafeNclNativeMethods.OSSOCK.WSASend_Blocking(this.m_Handle.DangerousGetHandle(), array, count, out num, socketFlags, SafeNativeOverlapped.Zero, IntPtr.Zero);
				if (errorCode == SocketError.SocketError)
				{
					errorCode = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				if (array2 != null)
				{
					for (int j = 0; j < array2.Length; j++)
					{
						if (array2[j].IsAllocated)
						{
							array2[j].Free();
						}
					}
				}
			}
			if (errorCode != SocketError.Success)
			{
				this.UpdateStatusAfterSocketError(errorCode);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Send", new SocketException(errorCode));
					Logging.Exit(Logging.Sockets, this, "Send", 0);
				}
				return 0;
			}
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesSent(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsSent();
				}
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Send", num);
			}
			return num;
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000C2FB4 File Offset: 0x000C1FB4
		public void SendFile(string fileName)
		{
			if (!ComNetOS.IsWinNt)
			{
				this.DownLevelSendFile(fileName);
				return;
			}
			this.SendFile(fileName, null, null, TransmitFileOptions.UseDefaultWorkerThread);
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000C2FD0 File Offset: 0x000C1FD0
		public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "SendFile", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsWinNt)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
			if (!this.Connected)
			{
				throw new NotSupportedException(SR.GetString("net_notconnected"));
			}
			this.ValidateBlockingMode();
			TransmitFileOverlappedAsyncResult transmitFileOverlappedAsyncResult = new TransmitFileOverlappedAsyncResult(this);
			FileStream fileStream = null;
			if (fileName != null && fileName.Length > 0)
			{
				fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			SafeHandle safeHandle = null;
			if (fileStream != null)
			{
				ExceptionHelper.UnmanagedPermission.Assert();
				try
				{
					safeHandle = fileStream.SafeFileHandle;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			SocketError socketError = SocketError.Success;
			try
			{
				transmitFileOverlappedAsyncResult.SetUnmanagedStructures(preBuffer, postBuffer, fileStream, TransmitFileOptions.UseDefaultWorkerThread, true);
				if ((safeHandle != null) ? (!UnsafeNclNativeMethods.OSSOCK.TransmitFile_Blocking(this.m_Handle.DangerousGetHandle(), safeHandle, 0, 0, SafeNativeOverlapped.Zero, transmitFileOverlappedAsyncResult.TransmitFileBuffers, flags)) : (!UnsafeNclNativeMethods.OSSOCK.TransmitFile_Blocking2(this.m_Handle.DangerousGetHandle(), IntPtr.Zero, 0, 0, SafeNativeOverlapped.Zero, transmitFileOverlappedAsyncResult.TransmitFileBuffers, flags)))
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				transmitFileOverlappedAsyncResult.SyncReleaseUnmanagedStructures();
			}
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "SendFile", ex);
				}
				throw ex;
			}
			if ((transmitFileOverlappedAsyncResult.Flags & (TransmitFileOptions.Disconnect | TransmitFileOptions.ReuseSocket)) != TransmitFileOptions.UseDefaultWorkerThread)
			{
				this.SetToDisconnected();
				this.m_RemoteEndPoint = null;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "SendFile", socketError);
			}
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000C3170 File Offset: 0x000C2170
		public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
		{
			SocketError socketError;
			int result = this.Send(buffer, offset, size, socketFlags, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000C3198 File Offset: 0x000C2198
		public unsafe int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Send", "");
			}
			if (this.CleanedUp)
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
			errorCode = SocketError.Success;
			this.ValidateBlockingMode();
			int num;
			if (buffer.Length == 0)
			{
				num = UnsafeNclNativeMethods.OSSOCK.send(this.m_Handle.DangerousGetHandle(), null, 0, socketFlags);
			}
			else
			{
				fixed (byte* ptr = buffer)
				{
					num = UnsafeNclNativeMethods.OSSOCK.send(this.m_Handle.DangerousGetHandle(), ptr + offset, size, socketFlags);
				}
			}
			if (num == -1)
			{
				errorCode = (SocketError)Marshal.GetLastWin32Error();
				this.UpdateStatusAfterSocketError(errorCode);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Send", new SocketException(errorCode));
					Logging.Exit(Logging.Sockets, this, "Send", 0);
				}
				return 0;
			}
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesSent(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsSent();
				}
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Dump(Logging.Sockets, this, "Send", buffer, offset, size);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Send", num);
			}
			return num;
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000C3310 File Offset: 0x000C2310
		public unsafe int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "SendTo", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			this.ValidateBlockingMode();
			EndPoint rightEndPoint = remoteEP;
			SocketAddress socketAddress = this.CheckCacheRemote(ref rightEndPoint, false);
			int num;
			if (buffer.Length == 0)
			{
				num = UnsafeNclNativeMethods.OSSOCK.sendto(this.m_Handle.DangerousGetHandle(), null, 0, socketFlags, socketAddress.m_Buffer, socketAddress.m_Size);
			}
			else
			{
				fixed (byte* ptr = buffer)
				{
					num = UnsafeNclNativeMethods.OSSOCK.sendto(this.m_Handle.DangerousGetHandle(), ptr + offset, size, socketFlags, socketAddress.m_Buffer, socketAddress.m_Size);
				}
			}
			if (num == -1)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "SendTo", ex);
				}
				throw ex;
			}
			if (this.m_RightEndPoint == null)
			{
				this.m_RightEndPoint = rightEndPoint;
			}
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesSent(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsSent();
				}
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Dump(Logging.Sockets, this, "SendTo", buffer, offset, size);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "SendTo", num);
			}
			return num;
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000C34AD File Offset: 0x000C24AD
		public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP)
		{
			return this.SendTo(buffer, 0, size, socketFlags, remoteEP);
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000C34BB File Offset: 0x000C24BB
		public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP)
		{
			return this.SendTo(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags, remoteEP);
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000C34D0 File Offset: 0x000C24D0
		public int SendTo(byte[] buffer, EndPoint remoteEP)
		{
			return this.SendTo(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None, remoteEP);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000C34E5 File Offset: 0x000C24E5
		public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
		{
			return this.Receive(buffer, 0, size, socketFlags);
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000C34F1 File Offset: 0x000C24F1
		public int Receive(byte[] buffer, SocketFlags socketFlags)
		{
			return this.Receive(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags);
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000C3505 File Offset: 0x000C2505
		public int Receive(byte[] buffer)
		{
			return this.Receive(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None);
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x000C351C File Offset: 0x000C251C
		public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
		{
			SocketError socketError;
			int result = this.Receive(buffer, offset, size, socketFlags, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000C3544 File Offset: 0x000C2544
		public unsafe int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Receive", "");
			}
			if (this.CleanedUp)
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
			this.ValidateBlockingMode();
			errorCode = SocketError.Success;
			int num;
			if (buffer.Length == 0)
			{
				num = UnsafeNclNativeMethods.OSSOCK.recv(this.m_Handle.DangerousGetHandle(), null, 0, socketFlags);
			}
			else
			{
				fixed (byte* ptr = buffer)
				{
					num = UnsafeNclNativeMethods.OSSOCK.recv(this.m_Handle.DangerousGetHandle(), ptr + offset, size, socketFlags);
				}
			}
			if (num == -1)
			{
				errorCode = (SocketError)Marshal.GetLastWin32Error();
				this.UpdateStatusAfterSocketError(errorCode);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Receive", new SocketException(errorCode));
					Logging.Exit(Logging.Sockets, this, "Receive", 0);
				}
				return 0;
			}
			if (Socket.s_PerfCountersEnabled)
			{
				bool flag = (socketFlags & SocketFlags.Peek) != SocketFlags.None;
				if (num > 0 && !flag)
				{
					NetworkingPerfCounters.AddBytesReceived(num);
					if (this.Transport == TransportType.Udp)
					{
						NetworkingPerfCounters.IncrementDatagramsReceived();
					}
				}
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Dump(Logging.Sockets, this, "Receive", buffer, offset, num);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Receive", num);
			}
			return num;
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000C36CA File Offset: 0x000C26CA
		public int Receive(IList<ArraySegment<byte>> buffers)
		{
			return this.Receive(buffers, SocketFlags.None);
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000C36D4 File Offset: 0x000C26D4
		public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			SocketError socketError;
			int result = this.Receive(buffers, socketFlags, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000C36F8 File Offset: 0x000C26F8
		public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Receive", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (buffers.Count == 0)
			{
				throw new ArgumentException(SR.GetString("net_sockets_zerolist", new object[]
				{
					"buffers"
				}), "buffers");
			}
			this.ValidateBlockingMode();
			int count = buffers.Count;
			WSABuffer[] array = new WSABuffer[count];
			GCHandle[] array2 = null;
			errorCode = SocketError.Success;
			int num;
			try
			{
				array2 = new GCHandle[count];
				for (int i = 0; i < count; i++)
				{
					ArraySegment<byte> segment = buffers[i];
					ValidationHelper.ValidateSegment(segment);
					array2[i] = GCHandle.Alloc(segment.Array, GCHandleType.Pinned);
					array[i].Length = segment.Count;
					array[i].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(segment.Array, segment.Offset);
				}
				errorCode = UnsafeNclNativeMethods.OSSOCK.WSARecv_Blocking(this.m_Handle.DangerousGetHandle(), array, count, out num, ref socketFlags, SafeNativeOverlapped.Zero, IntPtr.Zero);
				if (errorCode == SocketError.SocketError)
				{
					errorCode = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				if (array2 != null)
				{
					for (int j = 0; j < array2.Length; j++)
					{
						if (array2[j].IsAllocated)
						{
							array2[j].Free();
						}
					}
				}
			}
			if (errorCode != SocketError.Success)
			{
				this.UpdateStatusAfterSocketError(errorCode);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Receive", new SocketException(errorCode));
					Logging.Exit(Logging.Sockets, this, "Receive", 0);
				}
				return 0;
			}
			if (Socket.s_PerfCountersEnabled)
			{
				bool flag = (socketFlags & SocketFlags.Peek) != SocketFlags.None;
				if (num > 0 && !flag)
				{
					NetworkingPerfCounters.AddBytesReceived(num);
					if (this.Transport == TransportType.Udp)
					{
						NetworkingPerfCounters.IncrementDatagramsReceived();
					}
				}
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Receive", num);
			}
			return num;
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000C3908 File Offset: 0x000C2908
		public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "ReceiveMessageFrom", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (this.m_RightEndPoint == null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
			}
			this.ValidateBlockingMode();
			EndPoint endPoint = remoteEP;
			SocketAddress socketAddress = this.CheckCacheRemote(ref endPoint, false);
			ReceiveMessageOverlappedAsyncResult receiveMessageOverlappedAsyncResult = new ReceiveMessageOverlappedAsyncResult(this, null, null);
			receiveMessageOverlappedAsyncResult.SetUnmanagedStructures(buffer, offset, size, socketAddress, socketFlags);
			SocketAddress socketAddress2 = endPoint.Serialize();
			int result = 0;
			SocketError socketError = SocketError.Success;
			if (this.addressFamily == AddressFamily.InterNetwork)
			{
				this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
			}
			else if (this.addressFamily == AddressFamily.InterNetworkV6)
			{
				this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.PacketInformation, true);
			}
			try
			{
				if (this.WSARecvMsg_Blocking(this.m_Handle.DangerousGetHandle(), Marshal.UnsafeAddrOfPinnedArrayElement(receiveMessageOverlappedAsyncResult.m_MessageBuffer, 0), out result, IntPtr.Zero, IntPtr.Zero) == SocketError.SocketError)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				receiveMessageOverlappedAsyncResult.SyncReleaseUnmanagedStructures();
			}
			if (socketError != SocketError.Success && socketError != SocketError.MessageSize)
			{
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "ReceiveMessageFrom", ex);
				}
				throw ex;
			}
			if (!socketAddress2.Equals(receiveMessageOverlappedAsyncResult.m_SocketAddress))
			{
				try
				{
					remoteEP = endPoint.Create(receiveMessageOverlappedAsyncResult.m_SocketAddress);
				}
				catch
				{
				}
				if (this.m_RightEndPoint == null)
				{
					this.m_RightEndPoint = endPoint;
				}
			}
			socketFlags = receiveMessageOverlappedAsyncResult.m_flags;
			ipPacketInformation = receiveMessageOverlappedAsyncResult.m_IPPacketInformation;
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "ReceiveMessageFrom", socketError);
			}
			return result;
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x000C3B24 File Offset: 0x000C2B24
		public unsafe int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "ReceiveFrom", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (this.m_RightEndPoint == null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
			}
			this.ValidateBlockingMode();
			EndPoint endPoint = remoteEP;
			SocketAddress socketAddress = this.CheckCacheRemote(ref endPoint, false);
			SocketAddress socketAddress2 = endPoint.Serialize();
			int num;
			if (buffer.Length == 0)
			{
				num = UnsafeNclNativeMethods.OSSOCK.recvfrom(this.m_Handle.DangerousGetHandle(), null, 0, socketFlags, socketAddress.m_Buffer, ref socketAddress.m_Size);
			}
			else
			{
				fixed (byte* ptr = buffer)
				{
					num = UnsafeNclNativeMethods.OSSOCK.recvfrom(this.m_Handle.DangerousGetHandle(), ptr + offset, size, socketFlags, socketAddress.m_Buffer, ref socketAddress.m_Size);
				}
			}
			SocketException ex = null;
			if (num == -1)
			{
				ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "ReceiveFrom", ex);
				}
				if (ex.ErrorCode != 10040)
				{
					throw ex;
				}
			}
			if (!socketAddress2.Equals(socketAddress))
			{
				try
				{
					remoteEP = endPoint.Create(socketAddress);
				}
				catch
				{
				}
				if (this.m_RightEndPoint == null)
				{
					this.m_RightEndPoint = endPoint;
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesReceived(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsReceived();
				}
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Dump(Logging.Sockets, this, "ReceiveFrom", buffer, offset, size);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "ReceiveFrom", num);
			}
			return num;
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000C3D28 File Offset: 0x000C2D28
		public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
		{
			return this.ReceiveFrom(buffer, 0, size, socketFlags, ref remoteEP);
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x000C3D36 File Offset: 0x000C2D36
		public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP)
		{
			return this.ReceiveFrom(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags, ref remoteEP);
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x000C3D4B File Offset: 0x000C2D4B
		public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP)
		{
			return this.ReceiveFrom(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None, ref remoteEP);
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000C3D60 File Offset: 0x000C2D60
		public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (ioControlCode == -2147195266)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_useblocking"));
			}
			ExceptionHelper.UnmanagedPermission.Demand();
			int result = 0;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(this.m_Handle.DangerousGetHandle(), ioControlCode, optionInValue, (optionInValue != null) ? optionInValue.Length : 0, optionOutValue, (optionOutValue != null) ? optionOutValue.Length : 0, out result, SafeNativeOverlapped.Zero, IntPtr.Zero);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "IOControl", ex);
				}
				throw ex;
			}
			return result;
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000C3E08 File Offset: 0x000C2E08
		public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
		{
			return this.IOControl((int)ioControlCode, optionInValue, optionOutValue);
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000C3E14 File Offset: 0x000C2E14
		internal int IOControl(IOControlCode ioControlCode, IntPtr optionInValue, int inValueSize, IntPtr optionOutValue, int outValueSize)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if ((int)ioControlCode == -2147195266)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_useblocking"));
			}
			int result = 0;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking_Internal(this.m_Handle.DangerousGetHandle(), (uint)ioControlCode, optionInValue, inValueSize, optionOutValue, outValueSize, out result, SafeNativeOverlapped.Zero, IntPtr.Zero);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "IOControl", ex);
				}
				throw ex;
			}
			return result;
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000C3EA6 File Offset: 0x000C2EA6
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			this.CheckSetOptionPermissions(optionLevel, optionName);
			this.SetSocketOption(optionLevel, optionName, optionValue, false);
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000C3ED4 File Offset: 0x000C2ED4
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			this.CheckSetOptionPermissions(optionLevel, optionName);
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, optionLevel, optionName, optionValue, (optionValue != null) ? optionValue.Length : 0);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "SetSocketOption", ex);
				}
				throw ex;
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000C3F45 File Offset: 0x000C2F45
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
		{
			this.SetSocketOption(optionLevel, optionName, optionValue ? 1 : 0);
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000C3F58 File Offset: 0x000C2F58
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (optionValue == null)
			{
				throw new ArgumentNullException("optionValue");
			}
			this.CheckSetOptionPermissions(optionLevel, optionName);
			if (optionLevel == SocketOptionLevel.Socket && optionName == SocketOptionName.Linger)
			{
				LingerOption lingerOption = optionValue as LingerOption;
				if (lingerOption == null)
				{
					throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue", new object[]
					{
						"LingerOption"
					}), "optionValue");
				}
				if (lingerOption.LingerTime < 0 || lingerOption.LingerTime > 65535)
				{
					throw new ArgumentException(SR.GetString("ArgumentOutOfRange_Bounds_Lower_Upper", new object[]
					{
						0,
						65535
					}), "optionValue.LingerTime");
				}
				this.setLingerOption(lingerOption);
				return;
			}
			else if (optionLevel == SocketOptionLevel.IP && (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership))
			{
				MulticastOption multicastOption = optionValue as MulticastOption;
				if (multicastOption == null)
				{
					throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue", new object[]
					{
						"MulticastOption"
					}), "optionValue");
				}
				this.setMulticastOption(optionName, multicastOption);
				return;
			}
			else
			{
				if (optionLevel != SocketOptionLevel.IPv6 || (optionName != SocketOptionName.AddMembership && optionName != SocketOptionName.DropMembership))
				{
					throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue_all"), "optionValue");
				}
				IPv6MulticastOption pv6MulticastOption = optionValue as IPv6MulticastOption;
				if (pv6MulticastOption == null)
				{
					throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue", new object[]
					{
						"IPv6MulticastOption"
					}), "optionValue");
				}
				this.setIPv6MulticastOption(optionName, pv6MulticastOption);
				return;
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000C40D4 File Offset: 0x000C30D4
		public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (optionLevel == SocketOptionLevel.Socket && optionName == SocketOptionName.Linger)
			{
				return this.getLingerOpt();
			}
			if (optionLevel == SocketOptionLevel.IP && (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership))
			{
				return this.getMulticastOpt(optionName);
			}
			if (optionLevel == SocketOptionLevel.IPv6 && (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership))
			{
				return this.getIPv6MulticastOpt(optionName);
			}
			int num = 0;
			int num2 = 4;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, optionLevel, optionName, out num, ref num2);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "GetSocketOption", ex);
				}
				throw ex;
			}
			return num;
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000C4184 File Offset: 0x000C3184
		public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			int num = (optionValue != null) ? optionValue.Length : 0;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, optionLevel, optionName, optionValue, ref num);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "GetSocketOption", ex);
				}
				throw ex;
			}
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000C41F0 File Offset: 0x000C31F0
		public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			byte[] array = new byte[optionLength];
			int num = optionLength;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, optionLevel, optionName, array, ref num);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "GetSocketOption", ex);
				}
				throw ex;
			}
			if (optionLength != num)
			{
				byte[] array2 = new byte[num];
				Buffer.BlockCopy(array, 0, array2, 0, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000C4278 File Offset: 0x000C3278
		public bool Poll(int microSeconds, SelectMode mode)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			IntPtr intPtr = this.m_Handle.DangerousGetHandle();
			IntPtr[] array = new IntPtr[]
			{
				(IntPtr)1,
				intPtr
			};
			TimeValue timeValue = default(TimeValue);
			int num;
			if (microSeconds != -1)
			{
				Socket.MicrosecondsToTimeValue((long)((ulong)microSeconds), ref timeValue);
				num = UnsafeNclNativeMethods.OSSOCK.select(0, (mode == SelectMode.SelectRead) ? array : null, (mode == SelectMode.SelectWrite) ? array : null, (mode == SelectMode.SelectError) ? array : null, ref timeValue);
			}
			else
			{
				num = UnsafeNclNativeMethods.OSSOCK.select(0, (mode == SelectMode.SelectRead) ? array : null, (mode == SelectMode.SelectWrite) ? array : null, (mode == SelectMode.SelectError) ? array : null, IntPtr.Zero);
			}
			if (num == -1)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Poll", ex);
				}
				throw ex;
			}
			return (int)array[0] != 0 && array[1] == intPtr;
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000C4388 File Offset: 0x000C3388
		public static void Select(IList checkRead, IList checkWrite, IList checkError, int microSeconds)
		{
			if ((checkRead == null || checkRead.Count == 0) && (checkWrite == null || checkWrite.Count == 0) && (checkError == null || checkError.Count == 0))
			{
				throw new ArgumentNullException(SR.GetString("net_sockets_empty_select"));
			}
			if (checkRead != null && checkRead.Count > 65536)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_sockets_toolarge_select", new object[]
				{
					"checkRead",
					65536.ToString(NumberFormatInfo.CurrentInfo)
				}));
			}
			if (checkWrite != null && checkWrite.Count > 65536)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_sockets_toolarge_select", new object[]
				{
					"checkWrite",
					65536.ToString(NumberFormatInfo.CurrentInfo)
				}));
			}
			if (checkError != null && checkError.Count > 65536)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_sockets_toolarge_select", new object[]
				{
					"checkError",
					65536.ToString(NumberFormatInfo.CurrentInfo)
				}));
			}
			IntPtr[] array = Socket.SocketListToFileDescriptorSet(checkRead);
			IntPtr[] array2 = Socket.SocketListToFileDescriptorSet(checkWrite);
			IntPtr[] array3 = Socket.SocketListToFileDescriptorSet(checkError);
			TimeValue timeValue = default(TimeValue);
			if (microSeconds != -1)
			{
				Socket.MicrosecondsToTimeValue((long)((ulong)microSeconds), ref timeValue);
			}
			int num = UnsafeNclNativeMethods.OSSOCK.select(0, array, array2, array3, ref timeValue);
			if (num == -1)
			{
				throw new SocketException();
			}
			Socket.SelectFileDescriptor(checkRead, array);
			Socket.SelectFileDescriptor(checkWrite, array2);
			Socket.SelectFileDescriptor(checkError, array3);
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000C44FB File Offset: 0x000C34FB
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state)
		{
			if (!ComNetOS.IsWinNt)
			{
				return this.BeginDownLevelSendFile(fileName, true, callback, state);
			}
			return this.BeginSendFile(fileName, null, null, TransmitFileOptions.UseDefaultWorkerThread, callback, state);
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000C451C File Offset: 0x000C351C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", remoteEP);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
			}
			if (this.CanUseConnectEx(remoteEP))
			{
				return this.BeginConnectEx(remoteEP, true, callback, state);
			}
			EndPoint endPoint = remoteEP;
			SocketAddress socketAddress = this.CheckCacheRemote(ref endPoint, true);
			ConnectAsyncResult connectAsyncResult = new ConnectAsyncResult(this, endPoint, state, callback);
			connectAsyncResult.StartPostingAsyncOp(false);
			this.DoBeginConnect(endPoint, socketAddress, connectAsyncResult);
			connectAsyncResult.FinishPostingAsyncOp(ref this.Caches.ConnectClosureCache);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", connectAsyncResult);
			}
			return connectAsyncResult;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000C45E8 File Offset: 0x000C35E8
		public unsafe SocketInformation DuplicateAndClose(int targetProcessId)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "DuplicateAndClose", null);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			ExceptionHelper.UnrestrictedSocketPermission.Demand();
			SocketInformation result = default(SocketInformation);
			result.ProtocolInformation = new byte[Socket.protocolInformationSize];
			SocketError socketError;
			fixed (byte* protocolInformation = result.ProtocolInformation)
			{
				socketError = (SocketError)UnsafeNclNativeMethods.OSSOCK.WSADuplicateSocket(this.m_Handle, (uint)targetProcessId, protocolInformation);
			}
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException();
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "DuplicateAndClose", ex);
				}
				throw ex;
			}
			result.IsConnected = this.Connected;
			result.IsNonBlocking = !this.Blocking;
			result.IsListening = this.isListening;
			result.UseOnlyOverlappedIO = this.UseOnlyOverlappedIO;
			this.Close(-1);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "DuplicateAndClose", null);
			}
			return result;
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000C46F8 File Offset: 0x000C36F8
		internal IAsyncResult UnsafeBeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
		{
			if (this.CanUseConnectEx(remoteEP))
			{
				return this.BeginConnectEx(remoteEP, false, callback, state);
			}
			EndPoint endPoint = remoteEP;
			SocketAddress socketAddress = this.SnapshotAndSerialize(ref endPoint);
			ConnectAsyncResult connectAsyncResult = new ConnectAsyncResult(this, endPoint, state, callback);
			this.DoBeginConnect(endPoint, socketAddress, connectAsyncResult);
			return connectAsyncResult;
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000C4738 File Offset: 0x000C3738
		private void DoBeginConnect(EndPoint endPointSnapshot, SocketAddress socketAddress, LazyAsyncResult asyncResult)
		{
			EndPoint rightEndPoint = this.m_RightEndPoint;
			if (this.m_AcceptQueueOrConnectResult != null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_no_duplicate_async"));
			}
			this.m_AcceptQueueOrConnectResult = asyncResult;
			if (!this.SetAsyncEventSelect(AsyncEventBits.FdConnect))
			{
				this.m_AcceptQueueOrConnectResult = null;
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			IntPtr socketHandle = this.m_Handle.DangerousGetHandle();
			if (this.m_RightEndPoint == null)
			{
				this.m_RightEndPoint = endPointSnapshot;
			}
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAConnect(socketHandle, socketAddress.m_Buffer, socketAddress.m_Size, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			if (socketError != SocketError.Success)
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			if (socketError != SocketError.WouldBlock)
			{
				bool flag = true;
				if (socketError == SocketError.Success)
				{
					this.SetToConnected();
				}
				else
				{
					asyncResult.ErrorCode = (int)socketError;
				}
				if (Interlocked.Exchange<RegisteredWaitHandle>(ref this.m_RegisteredWait, null) == null)
				{
					flag = false;
				}
				this.UnsetAsyncEventSelect();
				this.InternalSetBlocking(true);
				if (socketError != SocketError.Success)
				{
					this.m_RightEndPoint = rightEndPoint;
					SocketException ex = new SocketException(socketError);
					this.UpdateStatusAfterSocketError(ex);
					this.m_AcceptQueueOrConnectResult = null;
					if (Socket.s_LoggingEnabled)
					{
						Logging.Exception(Logging.Sockets, this, "BeginConnect", ex);
					}
					throw ex;
				}
				if (flag)
				{
					asyncResult.InvokeCallback();
					return;
				}
			}
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000C4858 File Offset: 0x000C3858
		private bool CanUseConnectEx(EndPoint remoteEP)
		{
			return ComNetOS.IsPostWin2K && this.socketType == SocketType.Stream && (this.m_RightEndPoint != null || remoteEP.GetType() == typeof(IPEndPoint)) && (Thread.CurrentThread.IsThreadPoolThread || SettingsSectionInternal.Section.AlwaysUseCompletionPortsForConnect || this.m_IsDisconnected);
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000C48B4 File Offset: 0x000C38B4
		private void ConnectCallback()
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)this.m_AcceptQueueOrConnectResult;
			if (lazyAsyncResult.InternalPeekCompleted)
			{
				return;
			}
			NetworkEvents networkEvents = default(NetworkEvents);
			networkEvents.Events = AsyncEventBits.FdConnect;
			SocketError socketError = SocketError.OperationAborted;
			object result = null;
			try
			{
				if (!this.CleanedUp)
				{
					try
					{
						socketError = UnsafeNclNativeMethods.OSSOCK.WSAEnumNetworkEvents(this.m_Handle, this.m_AsyncEvent.SafeWaitHandle, ref networkEvents);
						if (socketError != SocketError.Success)
						{
							socketError = (SocketError)Marshal.GetLastWin32Error();
						}
						else
						{
							socketError = (SocketError)networkEvents.ErrorCodes[4];
						}
						this.UnsetAsyncEventSelect();
						this.InternalSetBlocking(true);
					}
					catch (ObjectDisposedException)
					{
						socketError = SocketError.OperationAborted;
					}
				}
				if (socketError == SocketError.Success)
				{
					this.SetToConnected();
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				result = ex;
			}
			catch
			{
				result = new Exception(SR.GetString("net_nonClsCompliantException"));
			}
			if (!lazyAsyncResult.InternalPeekCompleted)
			{
				lazyAsyncResult.ErrorCode = (int)socketError;
				lazyAsyncResult.InvokeCallback(result);
			}
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000C49AC File Offset: 0x000C39AC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", host);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			if (this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
			}
			Socket.MultipleAddressConnectAsyncResult multipleAddressConnectAsyncResult = new Socket.MultipleAddressConnectAsyncResult(null, port, this, state, requestCallback);
			multipleAddressConnectAsyncResult.StartPostingAsyncOp(false);
			IAsyncResult asyncResult = Dns.UnsafeBeginGetHostAddresses(host, new AsyncCallback(Socket.DnsCallback), multipleAddressConnectAsyncResult);
			if (asyncResult.CompletedSynchronously)
			{
				Socket.DoDnsCallback(asyncResult, multipleAddressConnectAsyncResult);
			}
			multipleAddressConnectAsyncResult.FinishPostingAsyncOp(ref this.Caches.ConnectClosureCache);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", multipleAddressConnectAsyncResult);
			}
			return multipleAddressConnectAsyncResult;
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000C4AA8 File Offset: 0x000C3AA8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", address);
			}
			if (this.CleanedUp)
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
			if (this.addressFamily != address.AddressFamily)
			{
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			IAsyncResult asyncResult = this.BeginConnect(new IPEndPoint(address, port), requestCallback, state);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000C4B50 File Offset: 0x000C3B50
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnect", addresses);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (addresses.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_invalidAddressList"), "addresses");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			if (this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
			}
			Socket.MultipleAddressConnectAsyncResult multipleAddressConnectAsyncResult = new Socket.MultipleAddressConnectAsyncResult(addresses, port, this, state, requestCallback);
			multipleAddressConnectAsyncResult.StartPostingAsyncOp(false);
			Socket.DoMultipleAddressConnectCallback(Socket.PostOneBeginConnect(multipleAddressConnectAsyncResult), multipleAddressConnectAsyncResult);
			multipleAddressConnectAsyncResult.FinishPostingAsyncOp(ref this.Caches.ConnectClosureCache);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnect", multipleAddressConnectAsyncResult);
			}
			return multipleAddressConnectAsyncResult;
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000C4C50 File Offset: 0x000C3C50
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state)
		{
			DisconnectOverlappedAsyncResult disconnectOverlappedAsyncResult = new DisconnectOverlappedAsyncResult(this, state, callback);
			disconnectOverlappedAsyncResult.StartPostingAsyncOp(false);
			this.DoBeginDisconnect(reuseSocket, disconnectOverlappedAsyncResult);
			disconnectOverlappedAsyncResult.FinishPostingAsyncOp();
			return disconnectOverlappedAsyncResult;
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000C4C80 File Offset: 0x000C3C80
		private void DoBeginDisconnect(bool reuseSocket, DisconnectOverlappedAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginDisconnect", null);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			asyncResult.SetUnmanagedStructures(null);
			SocketError socketError = SocketError.Success;
			if (!this.DisconnectEx(this.m_Handle, asyncResult.OverlappedHandle, reuseSocket ? 2 : 0, 0))
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			if (socketError == SocketError.Success)
			{
				this.SetToDisconnected();
				this.m_RemoteEndPoint = null;
			}
			socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginDisconnect", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginDisconnect", asyncResult);
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000C4D60 File Offset: 0x000C3D60
		public void Disconnect(bool reuseSocket)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Disconnect", null);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			SocketError socketError = SocketError.Success;
			if (!this.DisconnectEx_Blocking(this.m_Handle.DangerousGetHandle(), IntPtr.Zero, reuseSocket ? 2 : 0, 0))
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Disconnect", ex);
				}
				throw ex;
			}
			this.SetToDisconnected();
			this.m_RemoteEndPoint = null;
			this.InternalSetBlocking(this.willBlockInternal);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Disconnect", null);
			}
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000C4E40 File Offset: 0x000C3E40
		public void EndConnect(IAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndConnect", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = null;
			EndPoint endPoint = null;
			ConnectOverlappedAsyncResult connectOverlappedAsyncResult = asyncResult as ConnectOverlappedAsyncResult;
			if (connectOverlappedAsyncResult == null)
			{
				Socket.MultipleAddressConnectAsyncResult multipleAddressConnectAsyncResult = asyncResult as Socket.MultipleAddressConnectAsyncResult;
				if (multipleAddressConnectAsyncResult == null)
				{
					ConnectAsyncResult connectAsyncResult = asyncResult as ConnectAsyncResult;
					if (connectAsyncResult != null)
					{
						endPoint = connectAsyncResult.RemoteEndPoint;
						lazyAsyncResult = connectAsyncResult;
					}
				}
				else
				{
					endPoint = multipleAddressConnectAsyncResult.RemoteEndPoint;
					lazyAsyncResult = multipleAddressConnectAsyncResult;
				}
			}
			else
			{
				endPoint = connectOverlappedAsyncResult.RemoteEndPoint;
				lazyAsyncResult = connectOverlappedAsyncResult;
			}
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndConnect"
				}));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			lazyAsyncResult.EndCalled = true;
			this.m_AcceptQueueOrConnectResult = null;
			if (lazyAsyncResult.Result is Exception)
			{
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndConnect", (Exception)lazyAsyncResult.Result);
				}
				throw (Exception)lazyAsyncResult.Result;
			}
			if (lazyAsyncResult.ErrorCode != 0)
			{
				SocketException ex = new SocketException(lazyAsyncResult.ErrorCode, endPoint);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndConnect", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndConnect", "");
			}
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000C4FC4 File Offset: 0x000C3FC4
		public void EndDisconnect(IAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndDisconnect", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndDisconnect"
				}));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			lazyAsyncResult.EndCalled = true;
			if (lazyAsyncResult.ErrorCode != 0)
			{
				SocketException ex = new SocketException(lazyAsyncResult.ErrorCode);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndDisconnect", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndDisconnect", null);
			}
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000C50D4 File Offset: 0x000C40D4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			SocketError socketError;
			IAsyncResult result = this.BeginSend(buffer, offset, size, socketFlags, out socketError, callback, state);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000C5108 File Offset: 0x000C4108
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginSend", "");
			}
			if (this.CleanedUp)
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
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			overlappedAsyncResult.StartPostingAsyncOp(false);
			errorCode = this.DoBeginSend(buffer, offset, size, socketFlags, overlappedAsyncResult);
			if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
			{
				overlappedAsyncResult = null;
			}
			else
			{
				overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginSend", overlappedAsyncResult);
			}
			return overlappedAsyncResult;
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000C51E4 File Offset: 0x000C41E4
		internal IAsyncResult UnsafeBeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "UnsafeBeginSend", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			SocketError socketError = this.DoBeginSend(buffer, offset, size, socketFlags, overlappedAsyncResult);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "UnsafeBeginSend", overlappedAsyncResult);
			}
			return overlappedAsyncResult;
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x000C526C File Offset: 0x000C426C
		private SocketError DoBeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
		{
			SocketError socketError = SocketError.SocketError;
			try
			{
				asyncResult.SetUnmanagedStructures(buffer, offset, size, null, false, ref this.Caches.SendOverlappedCache);
				int num;
				socketError = UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, ref asyncResult.m_SingleBuffer, 1, out num, socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
				this.UpdateStatusAfterSocketError(socketError);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginSend", new SocketException(socketError));
				}
			}
			return socketError;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000C5318 File Offset: 0x000C4318
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state)
		{
			TransmitFileOverlappedAsyncResult transmitFileOverlappedAsyncResult = new TransmitFileOverlappedAsyncResult(this, state, callback);
			transmitFileOverlappedAsyncResult.StartPostingAsyncOp(false);
			this.DoBeginSendFile(fileName, preBuffer, postBuffer, flags, transmitFileOverlappedAsyncResult);
			transmitFileOverlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
			return transmitFileOverlappedAsyncResult;
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000C5358 File Offset: 0x000C4358
		private void DoBeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, TransmitFileOverlappedAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginSendFile", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsWinNt)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
			if (!this.Connected)
			{
				throw new NotSupportedException(SR.GetString("net_notconnected"));
			}
			FileStream fileStream = null;
			if (fileName != null && fileName.Length > 0)
			{
				fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			SafeHandle safeHandle = null;
			if (fileStream != null)
			{
				ExceptionHelper.UnmanagedPermission.Assert();
				try
				{
					safeHandle = fileStream.SafeFileHandle;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			SocketError socketError = SocketError.SocketError;
			try
			{
				asyncResult.SetUnmanagedStructures(preBuffer, postBuffer, fileStream, flags, ref this.Caches.SendOverlappedCache);
				bool flag;
				if (safeHandle != null)
				{
					flag = UnsafeNclNativeMethods.OSSOCK.TransmitFile(this.m_Handle, safeHandle, 0, 0, asyncResult.OverlappedHandle, asyncResult.TransmitFileBuffers, flags);
				}
				else
				{
					flag = UnsafeNclNativeMethods.OSSOCK.TransmitFile2(this.m_Handle, IntPtr.Zero, 0, 0, asyncResult.OverlappedHandle, asyncResult.TransmitFileBuffers, flags);
				}
				if (!flag)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
				else
				{
					socketError = SocketError.Success;
				}
			}
			finally
			{
				socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginSendFile", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginSendFile", socketError);
			}
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000C5510 File Offset: 0x000C4510
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			SocketError socketError;
			IAsyncResult result = this.BeginSend(buffers, socketFlags, out socketError, callback, state);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000C5540 File Offset: 0x000C4540
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginSend", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (buffers.Count == 0)
			{
				throw new ArgumentException(SR.GetString("net_sockets_zerolist", new object[]
				{
					"buffers"
				}), "buffers");
			}
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			overlappedAsyncResult.StartPostingAsyncOp(false);
			errorCode = this.DoBeginSend(buffers, socketFlags, overlappedAsyncResult);
			overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
			if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
			{
				overlappedAsyncResult = null;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginSend", overlappedAsyncResult);
			}
			return overlappedAsyncResult;
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000C5618 File Offset: 0x000C4618
		private SocketError DoBeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
		{
			SocketError socketError = SocketError.SocketError;
			try
			{
				asyncResult.SetUnmanagedStructures(buffers, ref this.Caches.SendOverlappedCache);
				int num;
				socketError = UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, asyncResult.m_WSABuffers, asyncResult.m_WSABuffers.Length, out num, socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
				this.UpdateStatusAfterSocketError(socketError);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginSend", new SocketException(socketError));
				}
			}
			return socketError;
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000C56C0 File Offset: 0x000C46C0
		public int EndSend(IAsyncResult asyncResult)
		{
			SocketError socketError;
			int result = this.EndSend(asyncResult, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000C56E4 File Offset: 0x000C46E4
		public int EndSend(IAsyncResult asyncResult, out SocketError errorCode)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndSend", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
			if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (overlappedAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndSend"
				}));
			}
			int num = (int)overlappedAsyncResult.InternalWaitForCompletion();
			overlappedAsyncResult.EndCalled = true;
			overlappedAsyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesSent(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsSent();
				}
			}
			errorCode = (SocketError)overlappedAsyncResult.ErrorCode;
			if (errorCode != SocketError.Success)
			{
				this.UpdateStatusAfterSocketError(errorCode);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndSend", new SocketException(errorCode));
					Logging.Exit(Logging.Sockets, this, "EndSend", 0);
				}
				return 0;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndSend", num);
			}
			return num;
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000C582C File Offset: 0x000C482C
		public void EndSendFile(IAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndSendFile", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsWinNt)
			{
				this.EndDownLevelSendFile(asyncResult);
				return;
			}
			if (!ComNetOS.IsWinNt)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			TransmitFileOverlappedAsyncResult transmitFileOverlappedAsyncResult = asyncResult as TransmitFileOverlappedAsyncResult;
			if (transmitFileOverlappedAsyncResult == null || transmitFileOverlappedAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (transmitFileOverlappedAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndSendFile"
				}));
			}
			transmitFileOverlappedAsyncResult.InternalWaitForCompletion();
			transmitFileOverlappedAsyncResult.EndCalled = true;
			transmitFileOverlappedAsyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
			if ((transmitFileOverlappedAsyncResult.Flags & (TransmitFileOptions.Disconnect | TransmitFileOptions.ReuseSocket)) != TransmitFileOptions.UseDefaultWorkerThread)
			{
				this.SetToDisconnected();
				this.m_RemoteEndPoint = null;
			}
			if (transmitFileOverlappedAsyncResult.ErrorCode != 0)
			{
				SocketException ex = new SocketException(transmitFileOverlappedAsyncResult.ErrorCode);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndSendFile", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndSendFile", "");
			}
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000C5978 File Offset: 0x000C4978
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginSendTo", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			EndPoint endPointSnapshot = remoteEP;
			SocketAddress socketAddress = this.CheckCacheRemote(ref endPointSnapshot, false);
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			overlappedAsyncResult.StartPostingAsyncOp(false);
			this.DoBeginSendTo(buffer, offset, size, socketFlags, endPointSnapshot, socketAddress, overlappedAsyncResult);
			overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginSendTo", overlappedAsyncResult);
			}
			return overlappedAsyncResult;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000C5A5C File Offset: 0x000C4A5C
		private void DoBeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint endPointSnapshot, SocketAddress socketAddress, OverlappedAsyncResult asyncResult)
		{
			EndPoint rightEndPoint = this.m_RightEndPoint;
			SocketError socketError = SocketError.SocketError;
			try
			{
				asyncResult.SetUnmanagedStructures(buffer, offset, size, socketAddress, false, ref this.Caches.SendOverlappedCache);
				if (this.m_RightEndPoint == null)
				{
					this.m_RightEndPoint = endPointSnapshot;
				}
				int num;
				socketError = UnsafeNclNativeMethods.OSSOCK.WSASendTo(this.m_Handle, ref asyncResult.m_SingleBuffer, 1, out num, socketFlags, asyncResult.GetSocketAddressPtr(), asyncResult.SocketAddress.Size, asyncResult.OverlappedHandle, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			catch (ObjectDisposedException)
			{
				this.m_RightEndPoint = rightEndPoint;
				throw;
			}
			finally
			{
				socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				this.m_RightEndPoint = rightEndPoint;
				asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginSendTo", ex);
				}
				throw ex;
			}
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000C5B54 File Offset: 0x000C4B54
		public int EndSendTo(IAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndSendTo", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
			if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (overlappedAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndSendTo"
				}));
			}
			int num = (int)overlappedAsyncResult.InternalWaitForCompletion();
			overlappedAsyncResult.EndCalled = true;
			overlappedAsyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesSent(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsSent();
				}
			}
			if (overlappedAsyncResult.ErrorCode != 0)
			{
				SocketException ex = new SocketException(overlappedAsyncResult.ErrorCode);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndSendTo", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndSendTo", num);
			}
			return num;
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000C5C88 File Offset: 0x000C4C88
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			SocketError socketError;
			IAsyncResult result = this.BeginReceive(buffer, offset, size, socketFlags, out socketError, callback, state);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000C5CBC File Offset: 0x000C4CBC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginReceive", "");
			}
			if (this.CleanedUp)
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
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			overlappedAsyncResult.StartPostingAsyncOp(false);
			errorCode = this.DoBeginReceive(buffer, offset, size, socketFlags, overlappedAsyncResult);
			if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
			{
				overlappedAsyncResult = null;
			}
			else
			{
				overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.ReceiveClosureCache);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginReceive", overlappedAsyncResult);
			}
			return overlappedAsyncResult;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000C5D98 File Offset: 0x000C4D98
		internal IAsyncResult UnsafeBeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "UnsafeBeginReceive", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			this.DoBeginReceive(buffer, offset, size, socketFlags, overlappedAsyncResult);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "UnsafeBeginReceive", overlappedAsyncResult);
			}
			return overlappedAsyncResult;
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000C5E0C File Offset: 0x000C4E0C
		private SocketError DoBeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
		{
			SocketError socketError = SocketError.SocketError;
			try
			{
				asyncResult.SetUnmanagedStructures(buffer, offset, size, null, false, ref this.Caches.ReceiveOverlappedCache);
				int num;
				socketError = UnsafeNclNativeMethods.OSSOCK.WSARecv(this.m_Handle, ref asyncResult.m_SingleBuffer, 1, out num, ref socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				asyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
				this.UpdateStatusAfterSocketError(socketError);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginReceive", new SocketException(socketError));
				}
				asyncResult.InvokeCallback(new SocketException(socketError));
			}
			return socketError;
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000C5EC4 File Offset: 0x000C4EC4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			SocketError socketError;
			IAsyncResult result = this.BeginReceive(buffers, socketFlags, out socketError, callback, state);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000C5EF4 File Offset: 0x000C4EF4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginReceive", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (buffers.Count == 0)
			{
				throw new ArgumentException(SR.GetString("net_sockets_zerolist", new object[]
				{
					"buffers"
				}), "buffers");
			}
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			overlappedAsyncResult.StartPostingAsyncOp(false);
			errorCode = this.DoBeginReceive(buffers, socketFlags, overlappedAsyncResult);
			if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
			{
				overlappedAsyncResult = null;
			}
			else
			{
				overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.ReceiveClosureCache);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginReceive", overlappedAsyncResult);
			}
			return overlappedAsyncResult;
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000C5FCC File Offset: 0x000C4FCC
		private SocketError DoBeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
		{
			SocketError socketError = SocketError.SocketError;
			try
			{
				asyncResult.SetUnmanagedStructures(buffers, ref this.Caches.ReceiveOverlappedCache);
				int num;
				socketError = UnsafeNclNativeMethods.OSSOCK.WSARecv(this.m_Handle, asyncResult.m_WSABuffers, asyncResult.m_WSABuffers.Length, out num, ref socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				asyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
				this.UpdateStatusAfterSocketError(socketError);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginReceive", new SocketException(socketError));
				}
			}
			return socketError;
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000C6074 File Offset: 0x000C5074
		public int EndReceive(IAsyncResult asyncResult)
		{
			SocketError socketError;
			int result = this.EndReceive(asyncResult, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return result;
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000C6098 File Offset: 0x000C5098
		public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndReceive", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
			if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (overlappedAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndReceive"
				}));
			}
			int num = (int)overlappedAsyncResult.InternalWaitForCompletion();
			overlappedAsyncResult.EndCalled = true;
			overlappedAsyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesReceived(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsReceived();
				}
			}
			errorCode = (SocketError)overlappedAsyncResult.ErrorCode;
			if (errorCode != SocketError.Success)
			{
				this.UpdateStatusAfterSocketError(errorCode);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndReceive", new SocketException(errorCode));
					Logging.Exit(Logging.Sockets, this, "EndReceive", 0);
				}
				return 0;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndReceive", num);
			}
			return num;
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000C61E0 File Offset: 0x000C51E0
		public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginReceiveMessageFrom", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (this.m_RightEndPoint == null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
			}
			ReceiveMessageOverlappedAsyncResult receiveMessageOverlappedAsyncResult = new ReceiveMessageOverlappedAsyncResult(this, state, callback);
			receiveMessageOverlappedAsyncResult.StartPostingAsyncOp(false);
			EndPoint rightEndPoint = this.m_RightEndPoint;
			EndPoint endPoint = remoteEP;
			SocketAddress socketAddress = this.CheckCacheRemote(ref endPoint, false);
			SocketError socketError = SocketError.SocketError;
			try
			{
				receiveMessageOverlappedAsyncResult.SetUnmanagedStructures(buffer, offset, size, socketAddress, socketFlags, ref this.Caches.ReceiveOverlappedCache);
				receiveMessageOverlappedAsyncResult.SocketAddressOriginal = endPoint.Serialize();
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
				}
				else if (this.addressFamily == AddressFamily.InterNetworkV6)
				{
					this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.PacketInformation, true);
				}
				if (this.m_RightEndPoint == null)
				{
					this.m_RightEndPoint = endPoint;
				}
				int num;
				socketError = this.WSARecvMsg(this.m_Handle, Marshal.UnsafeAddrOfPinnedArrayElement(receiveMessageOverlappedAsyncResult.m_MessageBuffer, 0), out num, receiveMessageOverlappedAsyncResult.OverlappedHandle, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
					if (socketError == SocketError.MessageSize)
					{
						socketError = SocketError.IOPending;
					}
				}
			}
			catch (ObjectDisposedException)
			{
				this.m_RightEndPoint = rightEndPoint;
				throw;
			}
			finally
			{
				socketError = receiveMessageOverlappedAsyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				this.m_RightEndPoint = rightEndPoint;
				receiveMessageOverlappedAsyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginReceiveMessageFrom", ex);
				}
				throw ex;
			}
			receiveMessageOverlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.ReceiveClosureCache);
			if (receiveMessageOverlappedAsyncResult.CompletedSynchronously && !receiveMessageOverlappedAsyncResult.SocketAddressOriginal.Equals(receiveMessageOverlappedAsyncResult.SocketAddress))
			{
				try
				{
					remoteEP = endPoint.Create(receiveMessageOverlappedAsyncResult.SocketAddress);
				}
				catch
				{
				}
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginReceiveMessageFrom", receiveMessageOverlappedAsyncResult);
			}
			return receiveMessageOverlappedAsyncResult;
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000C6450 File Offset: 0x000C5450
		public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndReceiveMessageFrom", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			ReceiveMessageOverlappedAsyncResult receiveMessageOverlappedAsyncResult = asyncResult as ReceiveMessageOverlappedAsyncResult;
			if (receiveMessageOverlappedAsyncResult == null || receiveMessageOverlappedAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (receiveMessageOverlappedAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndReceiveMessageFrom"
				}));
			}
			int num = (int)receiveMessageOverlappedAsyncResult.InternalWaitForCompletion();
			receiveMessageOverlappedAsyncResult.EndCalled = true;
			receiveMessageOverlappedAsyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
			receiveMessageOverlappedAsyncResult.SocketAddress.SetSize(receiveMessageOverlappedAsyncResult.GetSocketAddressSizePtr());
			SocketAddress socketAddress = endPoint.Serialize();
			if (!socketAddress.Equals(receiveMessageOverlappedAsyncResult.SocketAddress))
			{
				try
				{
					endPoint = endPoint.Create(receiveMessageOverlappedAsyncResult.SocketAddress);
				}
				catch
				{
				}
			}
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesReceived(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsReceived();
				}
			}
			if (receiveMessageOverlappedAsyncResult.ErrorCode != 0 && receiveMessageOverlappedAsyncResult.ErrorCode != 10040)
			{
				SocketException ex = new SocketException(receiveMessageOverlappedAsyncResult.ErrorCode);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndReceiveMessageFrom", ex);
				}
				throw ex;
			}
			socketFlags = receiveMessageOverlappedAsyncResult.m_flags;
			ipPacketInformation = receiveMessageOverlappedAsyncResult.m_IPPacketInformation;
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndReceiveMessageFrom", num);
			}
			return num;
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000C6604 File Offset: 0x000C5604
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginReceiveFrom", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (this.m_RightEndPoint == null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
			}
			EndPoint endPoint = remoteEP;
			SocketAddress socketAddress = this.CheckCacheRemote(ref endPoint, false);
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			overlappedAsyncResult.StartPostingAsyncOp(false);
			this.DoBeginReceiveFrom(buffer, offset, size, socketFlags, endPoint, socketAddress, overlappedAsyncResult);
			overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.ReceiveClosureCache);
			if (overlappedAsyncResult.CompletedSynchronously && !overlappedAsyncResult.SocketAddressOriginal.Equals(overlappedAsyncResult.SocketAddress))
			{
				try
				{
					remoteEP = endPoint.Create(overlappedAsyncResult.SocketAddress);
				}
				catch
				{
				}
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginReceiveFrom", overlappedAsyncResult);
			}
			return overlappedAsyncResult;
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000C6744 File Offset: 0x000C5744
		private void DoBeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint endPointSnapshot, SocketAddress socketAddress, OverlappedAsyncResult asyncResult)
		{
			EndPoint rightEndPoint = this.m_RightEndPoint;
			SocketError socketError = SocketError.SocketError;
			try
			{
				asyncResult.SetUnmanagedStructures(buffer, offset, size, socketAddress, true, ref this.Caches.ReceiveOverlappedCache);
				asyncResult.SocketAddressOriginal = endPointSnapshot.Serialize();
				if (this.m_RightEndPoint == null)
				{
					this.m_RightEndPoint = endPointSnapshot;
				}
				int num;
				socketError = UnsafeNclNativeMethods.OSSOCK.WSARecvFrom(this.m_Handle, ref asyncResult.m_SingleBuffer, 1, out num, ref socketFlags, asyncResult.GetSocketAddressPtr(), asyncResult.GetSocketAddressSizePtr(), asyncResult.OverlappedHandle, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			catch (ObjectDisposedException)
			{
				this.m_RightEndPoint = rightEndPoint;
				throw;
			}
			finally
			{
				socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				this.m_RightEndPoint = rightEndPoint;
				asyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginReceiveFrom", ex);
				}
				throw ex;
			}
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000C6844 File Offset: 0x000C5844
		public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndReceiveFrom", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
			if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (overlappedAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndReceiveFrom"
				}));
			}
			int num = (int)overlappedAsyncResult.InternalWaitForCompletion();
			overlappedAsyncResult.EndCalled = true;
			overlappedAsyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
			overlappedAsyncResult.SocketAddress.SetSize(overlappedAsyncResult.GetSocketAddressSizePtr());
			SocketAddress socketAddress = endPoint.Serialize();
			if (!socketAddress.Equals(overlappedAsyncResult.SocketAddress))
			{
				try
				{
					endPoint = endPoint.Create(overlappedAsyncResult.SocketAddress);
				}
				catch
				{
				}
			}
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesReceived(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsReceived();
				}
			}
			if (overlappedAsyncResult.ErrorCode != 0)
			{
				SocketException ex = new SocketException(overlappedAsyncResult.ErrorCode);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndReceiveFrom", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndReceiveFrom", num);
			}
			return num;
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000C69D4 File Offset: 0x000C59D4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginAccept(AsyncCallback callback, object state)
		{
			if (this.CanUseAcceptEx)
			{
				return this.BeginAccept(0, callback, state);
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginAccept", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			AcceptAsyncResult acceptAsyncResult = new AcceptAsyncResult(this, state, callback);
			acceptAsyncResult.StartPostingAsyncOp(false);
			this.DoBeginAccept(acceptAsyncResult);
			acceptAsyncResult.FinishPostingAsyncOp(ref this.Caches.AcceptClosureCache);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginAccept", acceptAsyncResult);
			}
			return acceptAsyncResult;
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000C6A6C File Offset: 0x000C5A6C
		private void DoBeginAccept(LazyAsyncResult asyncResult)
		{
			if (this.m_RightEndPoint == null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
			}
			if (!this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustlisten"));
			}
			bool flag = false;
			SocketError socketError = SocketError.Success;
			Queue acceptQueue = this.GetAcceptQueue();
			lock (this)
			{
				if (acceptQueue.Count == 0)
				{
					SocketAddress socketAddress = this.m_RightEndPoint.Serialize();
					this.InternalSetBlocking(false);
					SafeCloseSocket safeCloseSocket = null;
					try
					{
						safeCloseSocket = SafeCloseSocket.Accept(this.m_Handle, socketAddress.m_Buffer, ref socketAddress.m_Size);
						socketError = (SocketError)(safeCloseSocket.IsInvalid ? Marshal.GetLastWin32Error() : 0);
					}
					catch (ObjectDisposedException)
					{
						socketError = SocketError.NotSocket;
					}
					if (socketError != SocketError.WouldBlock)
					{
						if (socketError == SocketError.Success)
						{
							asyncResult.Result = this.CreateAcceptSocket(safeCloseSocket, this.m_RightEndPoint.Create(socketAddress), false);
						}
						else
						{
							asyncResult.ErrorCode = (int)socketError;
						}
						this.InternalSetBlocking(true);
						flag = true;
					}
					else
					{
						acceptQueue.Enqueue(asyncResult);
						if (!this.SetAsyncEventSelect(AsyncEventBits.FdAccept))
						{
							acceptQueue.Dequeue();
							throw new ObjectDisposedException(base.GetType().FullName);
						}
					}
				}
				else
				{
					acceptQueue.Enqueue(asyncResult);
				}
			}
			if (!flag)
			{
				return;
			}
			if (socketError == SocketError.Success)
			{
				asyncResult.InvokeCallback();
				return;
			}
			SocketException ex = new SocketException(socketError);
			this.UpdateStatusAfterSocketError(ex);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exception(Logging.Sockets, this, "BeginAccept", ex);
			}
			throw ex;
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000C6BDC File Offset: 0x000C5BDC
		private void CompleteAcceptResults(object nullState)
		{
			Queue acceptQueue = this.GetAcceptQueue();
			bool flag = true;
			while (flag)
			{
				LazyAsyncResult lazyAsyncResult = null;
				lock (this)
				{
					if (acceptQueue.Count == 0)
					{
						break;
					}
					lazyAsyncResult = (LazyAsyncResult)acceptQueue.Dequeue();
					if (acceptQueue.Count == 0)
					{
						flag = false;
					}
				}
				try
				{
					lazyAsyncResult.InvokeCallback(new SocketException(SocketError.OperationAborted));
				}
				catch
				{
					if (flag)
					{
						ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.CompleteAcceptResults), null);
					}
					throw;
				}
			}
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000C6C74 File Offset: 0x000C5C74
		private void AcceptCallback(object nullState)
		{
			bool flag = true;
			Queue acceptQueue = this.GetAcceptQueue();
			while (flag)
			{
				LazyAsyncResult lazyAsyncResult = null;
				SocketError socketError = SocketError.OperationAborted;
				SocketAddress socketAddress = null;
				SafeCloseSocket safeCloseSocket = null;
				Exception ex = null;
				object result = null;
				lock (this)
				{
					if (acceptQueue.Count == 0)
					{
						break;
					}
					lazyAsyncResult = (LazyAsyncResult)acceptQueue.Peek();
					if (!this.CleanedUp)
					{
						socketAddress = this.m_RightEndPoint.Serialize();
						try
						{
							safeCloseSocket = SafeCloseSocket.Accept(this.m_Handle, socketAddress.m_Buffer, ref socketAddress.m_Size);
							socketError = (SocketError)(safeCloseSocket.IsInvalid ? Marshal.GetLastWin32Error() : 0);
						}
						catch (ObjectDisposedException)
						{
							socketError = SocketError.OperationAborted;
						}
						catch (Exception ex2)
						{
							if (NclUtilities.IsFatal(ex2))
							{
								throw;
							}
							ex = ex2;
						}
						catch
						{
							ex = new Exception(SR.GetString("net_nonClsCompliantException"));
						}
					}
					if (socketError == SocketError.WouldBlock && ex == null)
					{
						if (this.SetAsyncEventSelect(AsyncEventBits.FdAccept))
						{
							break;
						}
						ex = new ObjectDisposedException(base.GetType().FullName);
					}
					if (ex != null)
					{
						result = ex;
					}
					else if (socketError == SocketError.Success)
					{
						result = this.CreateAcceptSocket(safeCloseSocket, this.m_RightEndPoint.Create(socketAddress), true);
					}
					else
					{
						lazyAsyncResult.ErrorCode = (int)socketError;
					}
					acceptQueue.Dequeue();
					if (acceptQueue.Count == 0)
					{
						if (!this.CleanedUp)
						{
							this.UnsetAsyncEventSelect();
						}
						this.InternalSetBlocking(true);
						flag = false;
					}
				}
				try
				{
					lazyAsyncResult.InvokeCallback(result);
				}
				catch
				{
					if (flag)
					{
						ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.AcceptCallback), nullState);
					}
					throw;
				}
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06002D69 RID: 11625 RVA: 0x000C6E68 File Offset: 0x000C5E68
		private bool CanUseAcceptEx
		{
			get
			{
				return ComNetOS.IsWinNt && (Thread.CurrentThread.IsThreadPoolThread || SettingsSectionInternal.Section.AlwaysUseCompletionPortsForAccept || this.m_IsDisconnected);
			}
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000C6E93 File Offset: 0x000C5E93
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state)
		{
			return this.BeginAccept(null, receiveSize, callback, state);
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000C6EA0 File Offset: 0x000C5EA0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginAccept", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (receiveSize < 0)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			AcceptOverlappedAsyncResult acceptOverlappedAsyncResult = new AcceptOverlappedAsyncResult(this, state, callback);
			acceptOverlappedAsyncResult.StartPostingAsyncOp(false);
			this.DoBeginAccept(acceptSocket, receiveSize, acceptOverlappedAsyncResult);
			acceptOverlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.AcceptClosureCache);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginAccept", acceptOverlappedAsyncResult);
			}
			return acceptOverlappedAsyncResult;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000C6F38 File Offset: 0x000C5F38
		private void DoBeginAccept(Socket acceptSocket, int receiveSize, AcceptOverlappedAsyncResult asyncResult)
		{
			if (!ComNetOS.IsWinNt)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
			if (this.m_RightEndPoint == null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
			}
			if (!this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustlisten"));
			}
			if (acceptSocket == null)
			{
				acceptSocket = new Socket(this.addressFamily, this.socketType, this.protocolType);
			}
			else if (acceptSocket.m_RightEndPoint != null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_namedmustnotbebound", new object[]
				{
					"acceptSocket"
				}));
			}
			asyncResult.AcceptSocket = acceptSocket;
			int num = this.m_RightEndPoint.Serialize().Size + 16;
			byte[] buffer = new byte[receiveSize + num * 2];
			asyncResult.SetUnmanagedStructures(buffer, num);
			SocketError socketError = SocketError.Success;
			int num2;
			if (!UnsafeNclNativeMethods.OSSOCK.AcceptEx(this.m_Handle, acceptSocket.m_Handle, Marshal.UnsafeAddrOfPinnedArrayElement(asyncResult.Buffer, 0), receiveSize, num, num, out num2, asyncResult.OverlappedHandle))
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginAccept", ex);
				}
				throw ex;
			}
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000C706C File Offset: 0x000C606C
		public Socket EndAccept(IAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndAccept", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (ComNetOS.IsWinNt && asyncResult != null && asyncResult is AcceptOverlappedAsyncResult)
			{
				byte[] array;
				int num;
				return this.EndAccept(out array, out num, asyncResult);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			AcceptAsyncResult acceptAsyncResult = asyncResult as AcceptAsyncResult;
			if (acceptAsyncResult == null || acceptAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (acceptAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndAccept"
				}));
			}
			object obj = acceptAsyncResult.InternalWaitForCompletion();
			acceptAsyncResult.EndCalled = true;
			Exception ex = obj as Exception;
			if (ex != null)
			{
				throw ex;
			}
			if (acceptAsyncResult.ErrorCode != 0)
			{
				SocketException ex2 = new SocketException(acceptAsyncResult.ErrorCode);
				this.UpdateStatusAfterSocketError(ex2);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndAccept", ex2);
				}
				throw ex2;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndAccept", obj);
			}
			return (Socket)obj;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000C71A0 File Offset: 0x000C61A0
		public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
		{
			byte[] sourceArray;
			int num;
			Socket result = this.EndAccept(out sourceArray, out num, asyncResult);
			buffer = new byte[num];
			Array.Copy(sourceArray, buffer, num);
			return result;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000C71CC File Offset: 0x000C61CC
		public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndAccept", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!ComNetOS.IsWinNt)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			AcceptOverlappedAsyncResult acceptOverlappedAsyncResult = asyncResult as AcceptOverlappedAsyncResult;
			if (acceptOverlappedAsyncResult == null || acceptOverlappedAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (acceptOverlappedAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndAccept"
				}));
			}
			Socket socket = (Socket)acceptOverlappedAsyncResult.InternalWaitForCompletion();
			bytesTransferred = acceptOverlappedAsyncResult.BytesTransferred;
			buffer = acceptOverlappedAsyncResult.Buffer;
			acceptOverlappedAsyncResult.EndCalled = true;
			if (Socket.s_PerfCountersEnabled && bytesTransferred > 0)
			{
				NetworkingPerfCounters.AddBytesReceived(bytesTransferred);
			}
			if (acceptOverlappedAsyncResult.ErrorCode != 0)
			{
				SocketException ex = new SocketException(acceptOverlappedAsyncResult.ErrorCode);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndAccept", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndAccept", socket);
			}
			return socket;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000C7304 File Offset: 0x000C6304
		public void Shutdown(SocketShutdown how)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Shutdown", how);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.shutdown(this.m_Handle, (int)how);
			socketError = (SocketError)((socketError != SocketError.SocketError) ? 0 : Marshal.GetLastWin32Error());
			if (socketError != SocketError.Success && socketError != SocketError.NotSocket)
			{
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Shutdown", ex);
				}
				throw ex;
			}
			this.SetToDisconnected();
			this.InternalSetBlocking(this.willBlockInternal);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Shutdown", "");
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06002D71 RID: 11633 RVA: 0x000C73C4 File Offset: 0x000C63C4
		private static object InternalSyncObject
		{
			get
			{
				if (Socket.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref Socket.s_InternalSyncObject, value, null);
				}
				return Socket.s_InternalSyncObject;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06002D72 RID: 11634 RVA: 0x000C73F0 File Offset: 0x000C63F0
		private Socket.CacheSet Caches
		{
			get
			{
				if (this.m_Caches == null)
				{
					this.m_Caches = new Socket.CacheSet();
				}
				return this.m_Caches;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06002D73 RID: 11635 RVA: 0x000C740C File Offset: 0x000C640C
		private Socket.DisconnectExDelegate DisconnectEx
		{
			get
			{
				if (Socket.s_DisconnectEx == null)
				{
					lock (Socket.InternalSyncObject)
					{
						if (Socket.s_DisconnectEx == null)
						{
							this.LoadDisconnectEx();
						}
					}
				}
				return Socket.s_DisconnectEx;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06002D74 RID: 11636 RVA: 0x000C7458 File Offset: 0x000C6458
		private Socket.DisconnectExDelegate_Blocking DisconnectEx_Blocking
		{
			get
			{
				if (Socket.s_DisconnectEx_Blocking == null)
				{
					lock (Socket.InternalSyncObject)
					{
						if (Socket.s_DisconnectEx_Blocking == null)
						{
							this.LoadDisconnectEx();
						}
					}
				}
				return Socket.s_DisconnectEx_Blocking;
			}
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000C74A4 File Offset: 0x000C64A4
		private void LoadDisconnectEx()
		{
			IntPtr zero = IntPtr.Zero;
			Guid guid = new Guid("{0x7fda2e11,0x8630,0x436f,{0xa0, 0x31, 0xf5, 0x36, 0xa6, 0xee, 0xc1, 0x57}}");
			int num;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl(this.m_Handle, -939524090, ref guid, sizeof(Guid), out zero, sizeof(IntPtr), out num, IntPtr.Zero, IntPtr.Zero);
			if (socketError != SocketError.Success)
			{
				throw new SocketException();
			}
			Socket.s_DisconnectEx = (Socket.DisconnectExDelegate)Marshal.GetDelegateForFunctionPointer(zero, typeof(Socket.DisconnectExDelegate));
			Socket.s_DisconnectEx_Blocking = (Socket.DisconnectExDelegate_Blocking)Marshal.GetDelegateForFunctionPointer(zero, typeof(Socket.DisconnectExDelegate_Blocking));
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06002D76 RID: 11638 RVA: 0x000C7534 File Offset: 0x000C6534
		private Socket.ConnectExDelegate ConnectEx
		{
			get
			{
				if (Socket.s_ConnectEx == null)
				{
					lock (Socket.InternalSyncObject)
					{
						if (Socket.s_ConnectEx == null)
						{
							IntPtr zero = IntPtr.Zero;
							Guid guid = new Guid("{0x25a207b9,0x0ddf3,0x4660,{0x8e,0xe9,0x76,0xe5,0x8c,0x74,0x06,0x3e}}");
							int num;
							SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl(this.m_Handle, -939524090, ref guid, sizeof(Guid), out zero, sizeof(IntPtr), out num, IntPtr.Zero, IntPtr.Zero);
							if (socketError != SocketError.Success)
							{
								throw new SocketException();
							}
							Socket.s_ConnectEx = (Socket.ConnectExDelegate)Marshal.GetDelegateForFunctionPointer(zero, typeof(Socket.ConnectExDelegate));
						}
					}
				}
				return Socket.s_ConnectEx;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06002D77 RID: 11639 RVA: 0x000C75E8 File Offset: 0x000C65E8
		private Socket.WSARecvMsgDelegate WSARecvMsg
		{
			get
			{
				if (Socket.s_WSARecvMsg == null)
				{
					lock (Socket.InternalSyncObject)
					{
						if (Socket.s_WSARecvMsg == null)
						{
							this.LoadWSARecvMsg();
						}
					}
				}
				return Socket.s_WSARecvMsg;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06002D78 RID: 11640 RVA: 0x000C7634 File Offset: 0x000C6634
		private Socket.WSARecvMsgDelegate_Blocking WSARecvMsg_Blocking
		{
			get
			{
				if (Socket.s_WSARecvMsg_Blocking == null)
				{
					lock (Socket.InternalSyncObject)
					{
						if (Socket.s_WSARecvMsg_Blocking == null)
						{
							this.LoadWSARecvMsg();
						}
					}
				}
				return Socket.s_WSARecvMsg_Blocking;
			}
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000C7680 File Offset: 0x000C6680
		private void LoadWSARecvMsg()
		{
			IntPtr zero = IntPtr.Zero;
			Guid guid = new Guid("{0xf689d7c8,0x6f1f,0x436b,{0x8a,0x53,0xe5,0x4f,0xe3,0x51,0xc3,0x22}}");
			int num;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl(this.m_Handle, -939524090, ref guid, sizeof(Guid), out zero, sizeof(IntPtr), out num, IntPtr.Zero, IntPtr.Zero);
			if (socketError != SocketError.Success)
			{
				throw new SocketException();
			}
			Socket.s_WSARecvMsg = (Socket.WSARecvMsgDelegate)Marshal.GetDelegateForFunctionPointer(zero, typeof(Socket.WSARecvMsgDelegate));
			Socket.s_WSARecvMsg_Blocking = (Socket.WSARecvMsgDelegate_Blocking)Marshal.GetDelegateForFunctionPointer(zero, typeof(Socket.WSARecvMsgDelegate_Blocking));
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06002D7A RID: 11642 RVA: 0x000C7710 File Offset: 0x000C6710
		private Socket.TransmitPacketsDelegate TransmitPackets
		{
			get
			{
				if (Socket.s_TransmitPackets == null)
				{
					lock (Socket.InternalSyncObject)
					{
						if (Socket.s_TransmitPackets == null)
						{
							this.LoadTransmitPackets();
						}
					}
				}
				return Socket.s_TransmitPackets;
			}
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000C775C File Offset: 0x000C675C
		private void LoadTransmitPackets()
		{
			IntPtr zero = IntPtr.Zero;
			Guid guid = new Guid("{0xd9689da0,0x1f90,0x11d3,{0x99,0x71,0x00,0xc0,0x4f,0x68,0xc8,0x76}}");
			int num;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl(this.m_Handle, -939524090, ref guid, sizeof(Guid), out zero, sizeof(IntPtr), out num, IntPtr.Zero, IntPtr.Zero);
			if (socketError != SocketError.Success)
			{
				throw new SocketException();
			}
			Socket.s_TransmitPackets = (Socket.TransmitPacketsDelegate)Marshal.GetDelegateForFunctionPointer(zero, typeof(Socket.TransmitPacketsDelegate));
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000C77D0 File Offset: 0x000C67D0
		private Queue GetAcceptQueue()
		{
			if (this.m_AcceptQueueOrConnectResult == null)
			{
				Interlocked.CompareExchange(ref this.m_AcceptQueueOrConnectResult, new Queue(16), null);
			}
			return (Queue)this.m_AcceptQueueOrConnectResult;
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06002D7D RID: 11645 RVA: 0x000C77F9 File Offset: 0x000C67F9
		internal bool CleanedUp
		{
			get
			{
				return this.m_IntCleanedUp == 1;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06002D7E RID: 11646 RVA: 0x000C7804 File Offset: 0x000C6804
		internal TransportType Transport
		{
			get
			{
				if (this.protocolType == ProtocolType.Tcp)
				{
					return TransportType.Tcp;
				}
				if (this.protocolType != ProtocolType.Udp)
				{
					return TransportType.All;
				}
				return TransportType.Udp;
			}
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000C7820 File Offset: 0x000C6820
		private void CheckSetOptionPermissions(SocketOptionLevel optionLevel, SocketOptionName optionName)
		{
			if ((optionLevel != SocketOptionLevel.Tcp || (optionName != SocketOptionName.Debug && optionName != SocketOptionName.AcceptConnection && optionName != SocketOptionName.AcceptConnection)) && (optionLevel != SocketOptionLevel.Udp || (optionName != SocketOptionName.Debug && optionName != SocketOptionName.ChecksumCoverage)) && (optionLevel != SocketOptionLevel.Socket || (optionName != SocketOptionName.KeepAlive && optionName != SocketOptionName.Linger && optionName != SocketOptionName.DontLinger && optionName != SocketOptionName.SendBuffer && optionName != SocketOptionName.ReceiveBuffer && optionName != SocketOptionName.SendTimeout && optionName != SocketOptionName.ExclusiveAddressUse && optionName != SocketOptionName.ReceiveTimeout)) && (optionLevel != SocketOptionLevel.IPv6 || optionName != (SocketOptionName)23))
			{
				ExceptionHelper.UnmanagedPermission.Demand();
			}
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000C78A0 File Offset: 0x000C68A0
		private SocketAddress SnapshotAndSerialize(ref EndPoint remoteEP)
		{
			IPEndPoint ipendPoint = remoteEP as IPEndPoint;
			if (ipendPoint != null)
			{
				ipendPoint = ipendPoint.Snapshot();
				remoteEP = ipendPoint;
			}
			return remoteEP.Serialize();
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000C78CC File Offset: 0x000C68CC
		private SocketAddress CheckCacheRemote(ref EndPoint remoteEP, bool isOverwrite)
		{
			IPEndPoint ipendPoint = remoteEP as IPEndPoint;
			if (ipendPoint != null)
			{
				ipendPoint = ipendPoint.Snapshot();
				remoteEP = ipendPoint;
			}
			SocketAddress socketAddress = remoteEP.Serialize();
			SocketAddress permittedRemoteAddress = this.m_PermittedRemoteAddress;
			if (permittedRemoteAddress != null && permittedRemoteAddress.Equals(socketAddress))
			{
				return permittedRemoteAddress;
			}
			if (ipendPoint != null)
			{
				SocketPermission socketPermission = new SocketPermission(NetworkAccess.Connect, this.Transport, ipendPoint.Address.ToString(), ipendPoint.Port);
				socketPermission.Demand();
			}
			else
			{
				ExceptionHelper.UnmanagedPermission.Demand();
			}
			if (this.m_PermittedRemoteAddress == null || isOverwrite)
			{
				this.m_PermittedRemoteAddress = socketAddress;
			}
			return socketAddress;
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000C7954 File Offset: 0x000C6954
		internal static void InitializeSockets()
		{
			if (!Socket.s_Initialized)
			{
				lock (Socket.InternalSyncObject)
				{
					if (!Socket.s_Initialized)
					{
						WSAData wsadata = default(WSAData);
						SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAStartup(514, out wsadata);
						if (socketError != SocketError.Success)
						{
							throw new SocketException();
						}
						if (!ComNetOS.IsWinNt)
						{
							Socket.UseOverlappedIO = true;
						}
						bool flag = true;
						bool flag2 = true;
						SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = UnsafeNclNativeMethods.OSSOCK.WSASocket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP, IntPtr.Zero, 0U, (SocketConstructorFlags)0);
						if (innerSafeCloseSocket.IsInvalid)
						{
							socketError = (SocketError)Marshal.GetLastWin32Error();
							if (socketError == SocketError.AddressFamilyNotSupported)
							{
								flag = false;
							}
						}
						innerSafeCloseSocket.Close();
						SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket2 = UnsafeNclNativeMethods.OSSOCK.WSASocket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.IP, IntPtr.Zero, 0U, (SocketConstructorFlags)0);
						if (innerSafeCloseSocket2.IsInvalid)
						{
							socketError = (SocketError)Marshal.GetLastWin32Error();
							if (socketError == SocketError.AddressFamilyNotSupported)
							{
								flag2 = false;
							}
						}
						innerSafeCloseSocket2.Close();
						flag2 = (flag2 && ComNetOS.IsPostWin2K);
						if (flag2)
						{
							Socket.s_OSSupportsIPv6 = true;
							flag2 = SettingsSectionInternal.Section.Ipv6Enabled;
						}
						Socket.s_SupportsIPv4 = flag;
						Socket.s_SupportsIPv6 = flag2;
						Socket.s_PerfCountersEnabled = SettingsSectionInternal.Section.PerformanceCountersEnabled;
						Socket.s_Initialized = true;
					}
				}
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000C7A70 File Offset: 0x000C6A70
		internal void InternalConnect(EndPoint remoteEP)
		{
			EndPoint endPointSnapshot = remoteEP;
			SocketAddress socketAddress = this.SnapshotAndSerialize(ref endPointSnapshot);
			this.DoConnect(endPointSnapshot, socketAddress);
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000C7A90 File Offset: 0x000C6A90
		private void DoConnect(EndPoint endPointSnapshot, SocketAddress socketAddress)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "Connect", endPointSnapshot);
			}
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAConnect(this.m_Handle.DangerousGetHandle(), socketAddress.m_Buffer, socketAddress.m_Size, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException(endPointSnapshot);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "Connect", ex);
				}
				throw ex;
			}
			if (this.m_RightEndPoint == null)
			{
				this.m_RightEndPoint = endPointSnapshot;
			}
			this.SetToConnected();
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "Connect", "");
			}
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000C7B44 File Offset: 0x000C6B44
		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (Socket.s_LoggingEnabled)
				{
					Logging.Enter(Logging.Sockets, this, "Dispose", null);
				}
			}
			catch (Exception exception)
			{
				if (NclUtilities.IsFatal(exception))
				{
					throw;
				}
			}
			if (!disposing)
			{
				if (this.m_Handle != null && !this.m_Handle.IsInvalid)
				{
					this.m_Handle.Dispose();
				}
				return;
			}
			int num;
			while ((num = Interlocked.CompareExchange(ref this.m_IntCleanedUp, 1, 0)) == 2)
			{
				Thread.SpinWait(1);
			}
			if (num == 1)
			{
				try
				{
					if (Socket.s_LoggingEnabled)
					{
						Logging.Exit(Logging.Sockets, this, "Dispose", null);
					}
				}
				catch (Exception exception2)
				{
					if (NclUtilities.IsFatal(exception2))
					{
						throw;
					}
				}
				return;
			}
			this.SetToDisconnected();
			AsyncEventBits asyncEventBits = AsyncEventBits.FdNone;
			if (this.m_BlockEventBits != AsyncEventBits.FdNone)
			{
				this.UnsetAsyncEventSelect();
				if (this.m_BlockEventBits == AsyncEventBits.FdConnect)
				{
					LazyAsyncResult lazyAsyncResult = this.m_AcceptQueueOrConnectResult as LazyAsyncResult;
					if (lazyAsyncResult != null && !lazyAsyncResult.InternalPeekCompleted)
					{
						asyncEventBits = AsyncEventBits.FdConnect;
					}
				}
				else if (this.m_BlockEventBits == AsyncEventBits.FdAccept)
				{
					Queue queue = this.m_AcceptQueueOrConnectResult as Queue;
					if (queue != null && queue.Count != 0)
					{
						asyncEventBits = AsyncEventBits.FdAccept;
					}
				}
			}
			try
			{
				int closeTimeout = this.m_CloseTimeout;
				if (closeTimeout == 0)
				{
					this.m_Handle.Dispose();
				}
				else
				{
					if (!this.willBlock || !this.willBlockInternal)
					{
						int num2 = 0;
						SocketError socketError = UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.m_Handle, -2147195266, ref num2);
					}
					if (closeTimeout < 0)
					{
						this.m_Handle.CloseAsIs();
					}
					else
					{
						SocketError socketError = UnsafeNclNativeMethods.OSSOCK.shutdown(this.m_Handle, 1);
						socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, ref closeTimeout, 4);
						if (socketError != SocketError.Success)
						{
							this.m_Handle.Dispose();
						}
						else
						{
							socketError = (SocketError)UnsafeNclNativeMethods.OSSOCK.recv(this.m_Handle.DangerousGetHandle(), null, 0, SocketFlags.None);
							if (socketError != SocketError.Success)
							{
								this.m_Handle.Dispose();
							}
							else
							{
								int num3 = 0;
								if (UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.m_Handle, 1074030207, ref num3) != SocketError.Success || num3 != 0)
								{
									this.m_Handle.Dispose();
								}
								else
								{
									this.m_Handle.CloseAsIs();
								}
							}
						}
					}
				}
			}
			catch (ObjectDisposedException)
			{
			}
			if (this.m_Caches != null)
			{
				OverlappedCache.InterlockedFree(ref this.m_Caches.SendOverlappedCache);
				OverlappedCache.InterlockedFree(ref this.m_Caches.ReceiveOverlappedCache);
			}
			if (asyncEventBits == AsyncEventBits.FdConnect)
			{
				ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(((LazyAsyncResult)this.m_AcceptQueueOrConnectResult).InvokeCallback), new SocketException(SocketError.OperationAborted));
			}
			else if (asyncEventBits == AsyncEventBits.FdAccept)
			{
				ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.CompleteAcceptResults), null);
			}
			if (this.m_AsyncEvent != null)
			{
				this.m_AsyncEvent.Close();
			}
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000C7DE0 File Offset: 0x000C6DE0
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000C7DF0 File Offset: 0x000C6DF0
		~Socket()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000C7E20 File Offset: 0x000C6E20
		internal void InternalShutdown(SocketShutdown how)
		{
			if (this.CleanedUp || this.m_Handle.IsInvalid)
			{
				return;
			}
			try
			{
				UnsafeNclNativeMethods.OSSOCK.shutdown(this.m_Handle, (int)how);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000C7E68 File Offset: 0x000C6E68
		private void DownLevelSendFile(string fileName)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "SendFile", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!this.Connected)
			{
				throw new NotSupportedException(SR.GetString("net_notconnected"));
			}
			this.ValidateBlockingMode();
			FileStream fileStream = null;
			if (fileName != null && fileName.Length > 0)
			{
				fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			try
			{
				SocketError socketError = SocketError.Success;
				byte[] array = new byte[64000];
				for (;;)
				{
					int num = fileStream.Read(array, 0, array.Length);
					if (num == 0)
					{
						break;
					}
					this.Send(array, 0, num, SocketFlags.None);
				}
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exit(Logging.Sockets, this, "SendFile", socketError);
				}
			}
			finally
			{
				Socket.DownLevelSendFileCleanup(fileStream);
			}
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000C7F40 File Offset: 0x000C6F40
		internal void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue, bool silent)
		{
			if (silent && (this.CleanedUp || this.m_Handle.IsInvalid))
			{
				return;
			}
			SocketError socketError = SocketError.Success;
			try
			{
				socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, optionLevel, optionName, ref optionValue, 4);
			}
			catch
			{
				if (silent && this.m_Handle.IsInvalid)
				{
					return;
				}
				throw;
			}
			if (!silent)
			{
				if (socketError == SocketError.SocketError)
				{
					SocketException ex = new SocketException();
					this.UpdateStatusAfterSocketError(ex);
					if (Socket.s_LoggingEnabled)
					{
						Logging.Exception(Logging.Sockets, this, "SetSocketOption", ex);
					}
					throw ex;
				}
				return;
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000C7FD4 File Offset: 0x000C6FD4
		private void setMulticastOption(SocketOptionName optionName, MulticastOption MR)
		{
			IPMulticastRequest ipmulticastRequest = default(IPMulticastRequest);
			ipmulticastRequest.MulticastAddress = (int)MR.Group.m_Address;
			if (MR.LocalAddress != null)
			{
				ipmulticastRequest.InterfaceAddress = (int)MR.LocalAddress.m_Address;
			}
			else
			{
				int interfaceAddress = IPAddress.HostToNetworkOrder(MR.InterfaceIndex);
				ipmulticastRequest.InterfaceAddress = interfaceAddress;
			}
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, SocketOptionLevel.IP, optionName, ref ipmulticastRequest, IPMulticastRequest.Size);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "setMulticastOption", ex);
				}
				throw ex;
			}
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000C8070 File Offset: 0x000C7070
		private void setIPv6MulticastOption(SocketOptionName optionName, IPv6MulticastOption MR)
		{
			IPv6MulticastRequest pv6MulticastRequest = default(IPv6MulticastRequest);
			pv6MulticastRequest.MulticastAddress = MR.Group.GetAddressBytes();
			pv6MulticastRequest.InterfaceIndex = (int)MR.InterfaceIndex;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, SocketOptionLevel.IPv6, optionName, ref pv6MulticastRequest, IPv6MulticastRequest.Size);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "setIPv6MulticastOption", ex);
				}
				throw ex;
			}
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000C80E8 File Offset: 0x000C70E8
		private void setLingerOption(LingerOption lref)
		{
			Linger linger = default(Linger);
			linger.OnOff = (lref.Enabled ? 1 : 0);
			linger.Time = (short)lref.LingerTime;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, SocketOptionLevel.Socket, SocketOptionName.Linger, ref linger, 4);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "setLingerOption", ex);
				}
				throw ex;
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000C8164 File Offset: 0x000C7164
		private LingerOption getLingerOpt()
		{
			Linger linger = default(Linger);
			int num = 4;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, SocketOptionLevel.Socket, SocketOptionName.Linger, out linger, ref num);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "getLingerOpt", ex);
				}
				throw ex;
			}
			return new LingerOption(linger.OnOff != 0, (int)linger.Time);
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000C81E0 File Offset: 0x000C71E0
		private MulticastOption getMulticastOpt(SocketOptionName optionName)
		{
			IPMulticastRequest ipmulticastRequest = default(IPMulticastRequest);
			int size = IPMulticastRequest.Size;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, SocketOptionLevel.IP, optionName, out ipmulticastRequest, ref size);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "getMulticastOpt", ex);
				}
				throw ex;
			}
			IPAddress group = new IPAddress(ipmulticastRequest.MulticastAddress);
			IPAddress mcint = new IPAddress(ipmulticastRequest.InterfaceAddress);
			return new MulticastOption(group, mcint);
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000C8264 File Offset: 0x000C7264
		private IPv6MulticastOption getIPv6MulticastOpt(SocketOptionName optionName)
		{
			IPv6MulticastRequest pv6MulticastRequest = default(IPv6MulticastRequest);
			int size = IPv6MulticastRequest.Size;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, SocketOptionLevel.IP, optionName, out pv6MulticastRequest, ref size);
			if (socketError == SocketError.SocketError)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "getIPv6MulticastOpt", ex);
				}
				throw ex;
			}
			return new IPv6MulticastOption(new IPAddress(pv6MulticastRequest.MulticastAddress), (long)pv6MulticastRequest.InterfaceIndex);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000C82DC File Offset: 0x000C72DC
		private SocketError InternalSetBlocking(bool desired, out bool current)
		{
			if (this.CleanedUp)
			{
				current = this.willBlock;
				return SocketError.Success;
			}
			int num = desired ? 0 : -1;
			SocketError socketError;
			try
			{
				socketError = UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.m_Handle, -2147195266, ref num);
				if (socketError == SocketError.SocketError)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			catch (ObjectDisposedException)
			{
				socketError = SocketError.NotSocket;
			}
			if (socketError == SocketError.Success)
			{
				this.willBlockInternal = (num == 0);
			}
			current = this.willBlockInternal;
			return socketError;
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000C8354 File Offset: 0x000C7354
		internal void InternalSetBlocking(bool desired)
		{
			bool flag;
			this.InternalSetBlocking(desired, out flag);
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000C836C File Offset: 0x000C736C
		private static IntPtr[] SocketListToFileDescriptorSet(IList socketList)
		{
			if (socketList == null || socketList.Count == 0)
			{
				return null;
			}
			IntPtr[] array = new IntPtr[socketList.Count + 1];
			array[0] = (IntPtr)socketList.Count;
			for (int i = 0; i < socketList.Count; i++)
			{
				if (!(socketList[i] is Socket))
				{
					throw new ArgumentException(SR.GetString("net_sockets_select", new object[]
					{
						socketList[i].GetType().FullName,
						typeof(Socket).FullName
					}), "socketList");
				}
				array[i + 1] = ((Socket)socketList[i]).m_Handle.DangerousGetHandle();
			}
			return array;
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x000C8438 File Offset: 0x000C7438
		private static void SelectFileDescriptor(IList socketList, IntPtr[] fileDescriptorSet)
		{
			if (socketList == null || socketList.Count == 0)
			{
				return;
			}
			if ((int)fileDescriptorSet[0] == 0)
			{
				socketList.Clear();
				return;
			}
			lock (socketList)
			{
				for (int i = 0; i < socketList.Count; i++)
				{
					Socket socket = socketList[i] as Socket;
					int num = 0;
					while (num < (int)fileDescriptorSet[0] && !(fileDescriptorSet[num + 1] == socket.m_Handle.DangerousGetHandle()))
					{
						num++;
					}
					if (num == (int)fileDescriptorSet[0])
					{
						socketList.RemoveAt(i--);
					}
				}
			}
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000C8508 File Offset: 0x000C7508
		private static void MicrosecondsToTimeValue(long microSeconds, ref TimeValue socketTime)
		{
			socketTime.Seconds = (int)(microSeconds / 1000000L);
			socketTime.Microseconds = (int)(microSeconds % 1000000L);
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000C8528 File Offset: 0x000C7528
		private IAsyncResult BeginConnectEx(EndPoint remoteEP, bool flowContext, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginConnectEx", "");
			}
			EndPoint endPoint = remoteEP;
			SocketAddress socketAddress = flowContext ? this.CheckCacheRemote(ref endPoint, true) : this.SnapshotAndSerialize(ref endPoint);
			if (this.m_RightEndPoint == null)
			{
				if (endPoint.AddressFamily == AddressFamily.InterNetwork)
				{
					this.InternalBind(new IPEndPoint(IPAddress.Any, 0));
				}
				else
				{
					this.InternalBind(new IPEndPoint(IPAddress.IPv6Any, 0));
				}
			}
			ConnectOverlappedAsyncResult connectOverlappedAsyncResult = new ConnectOverlappedAsyncResult(this, endPoint, state, callback);
			if (flowContext)
			{
				connectOverlappedAsyncResult.StartPostingAsyncOp(false);
			}
			connectOverlappedAsyncResult.SetUnmanagedStructures(socketAddress.m_Buffer);
			EndPoint rightEndPoint = this.m_RightEndPoint;
			if (this.m_RightEndPoint == null)
			{
				this.m_RightEndPoint = endPoint;
			}
			SocketError socketError = SocketError.Success;
			try
			{
				int num;
				if (!this.ConnectEx(this.m_Handle, Marshal.UnsafeAddrOfPinnedArrayElement(socketAddress.m_Buffer, 0), socketAddress.m_Size, IntPtr.Zero, 0, out num, connectOverlappedAsyncResult.OverlappedHandle))
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			catch
			{
				connectOverlappedAsyncResult.InternalCleanup();
				this.m_RightEndPoint = rightEndPoint;
				throw;
			}
			if (socketError == SocketError.Success)
			{
				this.SetToConnected();
			}
			socketError = connectOverlappedAsyncResult.CheckAsyncCallOverlappedResult(socketError);
			if (socketError != SocketError.Success)
			{
				this.m_RightEndPoint = rightEndPoint;
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginConnectEx", ex);
				}
				throw ex;
			}
			connectOverlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.ConnectClosureCache);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginConnectEx", connectOverlappedAsyncResult);
			}
			return connectOverlappedAsyncResult;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000C86B0 File Offset: 0x000C76B0
		internal void MultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "MultipleSend", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			WSABuffer[] array = new WSABuffer[buffers.Length];
			GCHandle[] array2 = null;
			SocketError socketError;
			try
			{
				array2 = new GCHandle[buffers.Length];
				for (int i = 0; i < buffers.Length; i++)
				{
					array2[i] = GCHandle.Alloc(buffers[i].Buffer, GCHandleType.Pinned);
					array[i].Length = buffers[i].Size;
					array[i].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(buffers[i].Buffer, buffers[i].Offset);
				}
				int num;
				socketError = UnsafeNclNativeMethods.OSSOCK.WSASend_Blocking(this.m_Handle.DangerousGetHandle(), array, array.Length, out num, socketFlags, SafeNativeOverlapped.Zero, IntPtr.Zero);
			}
			finally
			{
				if (array2 != null)
				{
					for (int j = 0; j < array2.Length; j++)
					{
						if (array2[j].IsAllocated)
						{
							array2[j].Free();
						}
					}
				}
			}
			if (socketError != SocketError.Success)
			{
				SocketException ex = new SocketException();
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "MultipleSend", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "MultipleSend", "");
			}
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000C8820 File Offset: 0x000C7820
		private static void DnsCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			Socket.MultipleAddressConnectAsyncResult multipleAddressConnectAsyncResult = (Socket.MultipleAddressConnectAsyncResult)result.AsyncState;
			try
			{
				Socket.DoDnsCallback(result, multipleAddressConnectAsyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				multipleAddressConnectAsyncResult.InvokeCallback(ex);
			}
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000C8880 File Offset: 0x000C7880
		private static void DoDnsCallback(IAsyncResult result, Socket.MultipleAddressConnectAsyncResult context)
		{
			IPAddress[] addresses = Dns.EndGetHostAddresses(result);
			context.addresses = addresses;
			Socket.DoMultipleAddressConnectCallback(Socket.PostOneBeginConnect(context), context);
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000C88A8 File Offset: 0x000C78A8
		private static object PostOneBeginConnect(Socket.MultipleAddressConnectAsyncResult context)
		{
			IPAddress ipaddress = context.addresses[context.index];
			if (ipaddress.AddressFamily == context.socket.AddressFamily)
			{
				try
				{
					EndPoint remoteEP = new IPEndPoint(ipaddress, context.port);
					context.socket.CheckCacheRemote(ref remoteEP, true);
					IAsyncResult asyncResult = context.socket.UnsafeBeginConnect(remoteEP, new AsyncCallback(Socket.MultipleAddressConnectCallback), context);
					if (asyncResult.CompletedSynchronously)
					{
						return asyncResult;
					}
				}
				catch (Exception ex)
				{
					if (ex is OutOfMemoryException || ex is StackOverflowException || ex is ThreadAbortException)
					{
						throw;
					}
					return ex;
				}
				return null;
			}
			if (context.lastException == null)
			{
				return new ArgumentException(SR.GetString("net_invalidAddressList"), "context");
			}
			return context.lastException;
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000C8974 File Offset: 0x000C7974
		private static void MultipleAddressConnectCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			Socket.MultipleAddressConnectAsyncResult multipleAddressConnectAsyncResult = (Socket.MultipleAddressConnectAsyncResult)result.AsyncState;
			try
			{
				Socket.DoMultipleAddressConnectCallback(result, multipleAddressConnectAsyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				multipleAddressConnectAsyncResult.InvokeCallback(ex);
			}
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000C89D4 File Offset: 0x000C79D4
		private static void DoMultipleAddressConnectCallback(object result, Socket.MultipleAddressConnectAsyncResult context)
		{
			while (result != null)
			{
				Exception ex = result as Exception;
				if (ex == null)
				{
					try
					{
						context.socket.EndConnect((IAsyncResult)result);
					}
					catch (Exception ex2)
					{
						if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
						{
							throw;
						}
						ex = ex2;
					}
					catch
					{
						ex = new Exception(SR.GetString("net_nonClsCompliantException"));
					}
				}
				if (ex == null)
				{
					context.InvokeCallback();
					return;
				}
				if (++context.index >= context.addresses.Length)
				{
					throw ex;
				}
				context.lastException = ex;
				result = Socket.PostOneBeginConnect(context);
			}
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000C8A90 File Offset: 0x000C7A90
		private static void DownLevelSendFileCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			Socket.DownLevelSendFileAsyncResult context = (Socket.DownLevelSendFileAsyncResult)result.AsyncState;
			Socket.DoDownLevelSendFileCallback(result, context);
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000C8ABC File Offset: 0x000C7ABC
		private static void DoDownLevelSendFileCallback(IAsyncResult result, Socket.DownLevelSendFileAsyncResult context)
		{
			try
			{
				for (;;)
				{
					if (!context.writing)
					{
						int num = context.fileStream.EndRead(result);
						if (num <= 0)
						{
							goto IL_4D;
						}
						context.writing = true;
						result = context.socket.BeginSend(context.buffer, 0, num, SocketFlags.None, new AsyncCallback(Socket.DownLevelSendFileCallback), context);
						if (!result.CompletedSynchronously)
						{
							break;
						}
					}
					else
					{
						context.socket.EndSend(result);
						context.writing = false;
						result = context.fileStream.BeginRead(context.buffer, 0, context.buffer.Length, new AsyncCallback(Socket.DownLevelSendFileCallback), context);
						if (!result.CompletedSynchronously)
						{
							break;
						}
					}
				}
				goto IL_A8;
				IL_4D:
				Socket.DownLevelSendFileCleanup(context.fileStream);
				context.InvokeCallback();
				IL_A8:;
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				Socket.DownLevelSendFileCleanup(context.fileStream);
				context.InvokeCallback(ex);
			}
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000C8BA4 File Offset: 0x000C7BA4
		private static void DownLevelSendFileCleanup(FileStream fileStream)
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream = null;
			}
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000C8BB4 File Offset: 0x000C7BB4
		private IAsyncResult BeginDownLevelSendFile(string fileName, bool flowContext, AsyncCallback callback, object state)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginSendFile", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!this.Connected)
			{
				throw new NotSupportedException(SR.GetString("net_notconnected"));
			}
			FileStream fileStream = null;
			if (fileName != null && fileName.Length > 0)
			{
				fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			Socket.DownLevelSendFileAsyncResult downLevelSendFileAsyncResult = null;
			IAsyncResult asyncResult = null;
			try
			{
				downLevelSendFileAsyncResult = new Socket.DownLevelSendFileAsyncResult(fileStream, this, state, callback);
				if (flowContext)
				{
					downLevelSendFileAsyncResult.StartPostingAsyncOp(false);
				}
				asyncResult = fileStream.BeginRead(downLevelSendFileAsyncResult.buffer, 0, downLevelSendFileAsyncResult.buffer.Length, new AsyncCallback(Socket.DownLevelSendFileCallback), downLevelSendFileAsyncResult);
			}
			catch (Exception exception)
			{
				if (!NclUtilities.IsFatal(exception))
				{
					Socket.DownLevelSendFileCleanup(fileStream);
				}
				throw;
			}
			if (asyncResult.CompletedSynchronously)
			{
				Socket.DoDownLevelSendFileCallback(asyncResult, downLevelSendFileAsyncResult);
			}
			downLevelSendFileAsyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginSendFile", 0);
			}
			return downLevelSendFileAsyncResult;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000C8CC8 File Offset: 0x000C7CC8
		internal IAsyncResult BeginMultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			overlappedAsyncResult.StartPostingAsyncOp(false);
			this.DoBeginMultipleSend(buffers, socketFlags, overlappedAsyncResult);
			overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
			return overlappedAsyncResult;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000C8D04 File Offset: 0x000C7D04
		internal IAsyncResult UnsafeBeginMultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			OverlappedAsyncResult overlappedAsyncResult = new OverlappedAsyncResult(this, state, callback);
			this.DoBeginMultipleSend(buffers, socketFlags, overlappedAsyncResult);
			return overlappedAsyncResult;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000C8D28 File Offset: 0x000C7D28
		private void DoBeginMultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "BeginMultipleSend", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			SocketError socketError = SocketError.SocketError;
			try
			{
				asyncResult.SetUnmanagedStructures(buffers, ref this.Caches.SendOverlappedCache);
				int num;
				socketError = UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, asyncResult.m_WSABuffers, asyncResult.m_WSABuffers.Length, out num, socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			finally
			{
				socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
			}
			if (socketError != SocketError.Success)
			{
				asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
				SocketException ex = new SocketException(socketError);
				this.UpdateStatusAfterSocketError(ex);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "BeginMultipleSend", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "BeginMultipleSend", asyncResult);
			}
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000C8E20 File Offset: 0x000C7E20
		private void EndDownLevelSendFile(IAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndSendFile", asyncResult);
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as Socket.DownLevelSendFileAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndSendFile"
				}));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			lazyAsyncResult.EndCalled = true;
			Exception ex = lazyAsyncResult.Result as Exception;
			if (ex != null)
			{
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndSendFile", "");
			}
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000C8EF8 File Offset: 0x000C7EF8
		internal int EndMultipleSend(IAsyncResult asyncResult)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "EndMultipleSend", asyncResult);
			}
			OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
			int num = (int)overlappedAsyncResult.InternalWaitForCompletion();
			overlappedAsyncResult.EndCalled = true;
			overlappedAsyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
			if (Socket.s_PerfCountersEnabled && num > 0)
			{
				NetworkingPerfCounters.AddBytesSent(num);
				if (this.Transport == TransportType.Udp)
				{
					NetworkingPerfCounters.IncrementDatagramsSent();
				}
			}
			if (overlappedAsyncResult.ErrorCode != 0)
			{
				SocketException ex = new SocketException(overlappedAsyncResult.ErrorCode);
				if (Socket.s_LoggingEnabled)
				{
					Logging.Exception(Logging.Sockets, this, "EndMultipleSend", ex);
				}
				throw ex;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "EndMultipleSend", num);
			}
			return num;
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000C8FB4 File Offset: 0x000C7FB4
		private Socket CreateAcceptSocket(SafeCloseSocket fd, EndPoint remoteEP, bool needCancelSelect)
		{
			Socket socket = new Socket(fd);
			return this.UpdateAcceptSocket(socket, remoteEP, needCancelSelect);
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000C8FD4 File Offset: 0x000C7FD4
		internal Socket UpdateAcceptSocket(Socket socket, EndPoint remoteEP, bool needCancelSelect)
		{
			socket.addressFamily = this.addressFamily;
			socket.socketType = this.socketType;
			socket.protocolType = this.protocolType;
			socket.m_RightEndPoint = this.m_RightEndPoint;
			socket.m_RemoteEndPoint = remoteEP;
			socket.SetToConnected();
			if (needCancelSelect)
			{
				socket.UnsetAsyncEventSelect();
			}
			socket.willBlock = this.willBlock;
			if (this.willBlock != this.willBlockInternal)
			{
				socket.InternalSetBlocking(this.willBlock);
			}
			return socket;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000C904E File Offset: 0x000C804E
		internal void SetToConnected()
		{
			if (this.m_IsConnected)
			{
				return;
			}
			this.m_IsConnected = true;
			this.m_IsDisconnected = false;
			if (Socket.s_PerfCountersEnabled)
			{
				NetworkingPerfCounters.IncrementConnectionsEstablished();
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000C9073 File Offset: 0x000C8073
		internal void SetToDisconnected()
		{
			if (!this.m_IsConnected)
			{
				return;
			}
			this.m_IsConnected = false;
			this.m_IsDisconnected = true;
			if (!this.CleanedUp)
			{
				this.UnsetAsyncEventSelect();
			}
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000C909A File Offset: 0x000C809A
		internal void UpdateStatusAfterSocketError(SocketException socketException)
		{
			this.UpdateStatusAfterSocketError((SocketError)socketException.NativeErrorCode);
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000C90A8 File Offset: 0x000C80A8
		internal void UpdateStatusAfterSocketError(SocketError errorCode)
		{
			if (this.m_IsConnected && (this.m_Handle.IsInvalid || (errorCode != SocketError.WouldBlock && errorCode != SocketError.IOPending && errorCode != SocketError.NoBufferSpaceAvailable)))
			{
				this.SetToDisconnected();
			}
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000C90E0 File Offset: 0x000C80E0
		private void UnsetAsyncEventSelect()
		{
			RegisteredWaitHandle registeredWait = this.m_RegisteredWait;
			if (registeredWait != null)
			{
				this.m_RegisteredWait = null;
				registeredWait.Unregister(null);
			}
			SocketError socketError = SocketError.NotSocket;
			try
			{
				socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(this.m_Handle, IntPtr.Zero, AsyncEventBits.FdNone);
			}
			catch (Exception exception)
			{
				if (NclUtilities.IsFatal(exception))
				{
					throw;
				}
			}
			catch
			{
			}
			if (this.m_AsyncEvent != null)
			{
				try
				{
					this.m_AsyncEvent.Reset();
				}
				catch (ObjectDisposedException)
				{
				}
			}
			if (socketError == SocketError.SocketError)
			{
				this.UpdateStatusAfterSocketError(socketError);
			}
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000C917C File Offset: 0x000C817C
		private bool SetAsyncEventSelect(AsyncEventBits blockEventBits)
		{
			if (this.m_RegisteredWait != null)
			{
				return false;
			}
			if (this.m_AsyncEvent == null)
			{
				Interlocked.CompareExchange<ManualResetEvent>(ref this.m_AsyncEvent, new ManualResetEvent(false), null);
				if (Socket.s_RegisteredWaitCallback == null)
				{
					Socket.s_RegisteredWaitCallback = new WaitOrTimerCallback(Socket.RegisteredWaitCallback);
				}
			}
			if (Interlocked.CompareExchange(ref this.m_IntCleanedUp, 2, 0) != 0)
			{
				return false;
			}
			this.m_BlockEventBits = blockEventBits;
			this.m_RegisteredWait = ThreadPool.UnsafeRegisterWaitForSingleObject(this.m_AsyncEvent, Socket.s_RegisteredWaitCallback, this, -1, true);
			Interlocked.Exchange(ref this.m_IntCleanedUp, 0);
			SocketError socketError = SocketError.NotSocket;
			try
			{
				socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(this.m_Handle, this.m_AsyncEvent.SafeWaitHandle, blockEventBits);
			}
			catch (Exception exception)
			{
				if (NclUtilities.IsFatal(exception))
				{
					throw;
				}
			}
			catch
			{
			}
			if (socketError == SocketError.SocketError)
			{
				this.UpdateStatusAfterSocketError(socketError);
			}
			this.willBlockInternal = false;
			return socketError == SocketError.Success;
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000C9268 File Offset: 0x000C8268
		private static void RegisteredWaitCallback(object state, bool timedOut)
		{
			Socket socket = (Socket)state;
			if (Interlocked.Exchange<RegisteredWaitHandle>(ref socket.m_RegisteredWait, null) != null)
			{
				AsyncEventBits blockEventBits = socket.m_BlockEventBits;
				if (blockEventBits != AsyncEventBits.FdAccept)
				{
					if (blockEventBits != AsyncEventBits.FdConnect)
					{
						return;
					}
					socket.ConnectCallback();
					return;
				}
				else
				{
					socket.AcceptCallback(null);
				}
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000C92A9 File Offset: 0x000C82A9
		private void ValidateBlockingMode()
		{
			if (this.willBlock && !this.willBlockInternal)
			{
				throw new InvalidOperationException(SR.GetString("net_invasync"));
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000C92CC File Offset: 0x000C82CC
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal void BindToCompletionPort()
		{
			if (!this.m_BoundToThreadPool && !Socket.UseOverlappedIO)
			{
				lock (this)
				{
					if (!this.m_BoundToThreadPool)
					{
						try
						{
							ThreadPool.BindHandle(this.m_Handle);
							this.m_BoundToThreadPool = true;
						}
						catch (Exception exception)
						{
							if (NclUtilities.IsFatal(exception))
							{
								throw;
							}
							this.Close(0);
							throw;
						}
					}
				}
			}
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000C9348 File Offset: 0x000C8348
		public bool AcceptAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "AcceptAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (e.m_BufferList != null)
			{
				throw new ArgumentException(SR.GetString("net_multibuffernotsupported"), "BufferList");
			}
			if (this.m_RightEndPoint == null)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
			}
			if (!this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustlisten"));
			}
			if (e.AcceptSocket == null)
			{
				e.AcceptSocket = new Socket(this.addressFamily, this.socketType, this.protocolType);
			}
			else if (e.AcceptSocket.m_RightEndPoint != null && !e.AcceptSocket.m_IsDisconnected)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_namedmustnotbebound", new object[]
				{
					"AcceptSocket"
				}));
			}
			e.StartOperationCommon(this);
			e.StartOperationAccept();
			this.BindToCompletionPort();
			SocketError socketError = SocketError.Success;
			int bytesTransferred;
			try
			{
				if (!UnsafeNclNativeMethods.OSSOCK.AcceptEx(this.m_Handle, e.AcceptSocket.m_Handle, (e.m_PtrSingleBuffer != IntPtr.Zero) ? e.m_PtrSingleBuffer : e.m_PtrAcceptBuffer, (e.m_PtrSingleBuffer != IntPtr.Zero) ? (e.Count - e.m_AcceptAddressBufferCount) : 0, e.m_AcceptAddressBufferCount / 2, e.m_AcceptAddressBufferCount / 2, out bytesTransferred, e.m_PtrNativeOverlapped))
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			catch (Exception ex)
			{
				e.Complete();
				throw ex;
			}
			bool flag;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, bytesTransferred, SocketFlags.None);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "AcceptAsync", flag);
			}
			return flag;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000C951C File Offset: 0x000C851C
		public bool ConnectAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "ConnectAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (e.m_BufferList != null)
			{
				throw new ArgumentException(SR.GetString("net_multibuffernotsupported"), "BufferList");
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (this.isListening)
			{
				throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
			}
			if (this.addressFamily != e.RemoteEndPoint.AddressFamily)
			{
				throw new NotSupportedException(SR.GetString("net_invalidversion"));
			}
			EndPoint remoteEndPoint = e.RemoteEndPoint;
			e.m_SocketAddress = this.CheckCacheRemote(ref remoteEndPoint, false);
			if (this.m_RightEndPoint == null)
			{
				if (remoteEndPoint.AddressFamily == AddressFamily.InterNetwork)
				{
					this.InternalBind(new IPEndPoint(IPAddress.Any, 0));
				}
				else
				{
					this.InternalBind(new IPEndPoint(IPAddress.IPv6Any, 0));
				}
			}
			EndPoint rightEndPoint = this.m_RightEndPoint;
			if (this.m_RightEndPoint == null)
			{
				this.m_RightEndPoint = remoteEndPoint;
			}
			e.StartOperationCommon(this);
			e.StartOperationConnect();
			this.BindToCompletionPort();
			SocketError socketError = SocketError.Success;
			int bytesTransferred;
			try
			{
				if (!this.ConnectEx(this.m_Handle, e.m_PtrSocketAddressBuffer, e.m_SocketAddress.m_Size, e.m_PtrSingleBuffer, e.Count, out bytesTransferred, e.m_PtrNativeOverlapped))
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			catch (Exception ex)
			{
				this.m_RightEndPoint = rightEndPoint;
				e.Complete();
				throw ex;
			}
			bool flag;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, bytesTransferred, SocketFlags.None);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "ConnectAsync", flag);
			}
			return flag;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000C96E0 File Offset: 0x000C86E0
		public bool DisconnectAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "DisconnectAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			e.StartOperationCommon(this);
			e.StartOperationDisconnect();
			this.BindToCompletionPort();
			SocketError socketError = SocketError.Success;
			try
			{
				if (!this.DisconnectEx(this.m_Handle, e.m_PtrNativeOverlapped, e.DisconnectReuseSocket ? 2 : 0, 0))
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
			}
			catch (Exception ex)
			{
				e.Complete();
				throw ex;
			}
			bool flag;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, 0, SocketFlags.None);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "DisconnectAsync", flag);
			}
			return flag;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000C97B8 File Offset: 0x000C87B8
		public bool ReceiveAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "ReceiveAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			e.StartOperationCommon(this);
			e.StartOperationReceive();
			this.BindToCompletionPort();
			SocketFlags socketFlags = e.m_SocketFlags;
			int bytesTransferred;
			SocketError socketError;
			try
			{
				if (e.m_Buffer != null)
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.WSARecv(this.m_Handle, ref e.m_WSABuffer, 1, out bytesTransferred, ref socketFlags, e.m_PtrNativeOverlapped, IntPtr.Zero);
				}
				else
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.WSARecv(this.m_Handle, e.m_WSABufferArray, e.m_WSABufferArray.Length, out bytesTransferred, ref socketFlags, e.m_PtrNativeOverlapped, IntPtr.Zero);
				}
			}
			catch (Exception ex)
			{
				e.Complete();
				throw ex;
			}
			if (socketError != SocketError.Success)
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			bool flag;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, bytesTransferred, socketFlags);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "ReceiveAsync", flag);
			}
			return flag;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000C98C8 File Offset: 0x000C88C8
		public bool ReceiveFromAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "ReceiveFromAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("RemoteEndPoint");
			}
			EndPoint remoteEndPoint = e.RemoteEndPoint;
			e.m_SocketAddress = this.CheckCacheRemote(ref remoteEndPoint, false);
			e.StartOperationCommon(this);
			e.StartOperationReceiveFrom();
			this.BindToCompletionPort();
			SocketFlags socketFlags = e.m_SocketFlags;
			int bytesTransferred;
			SocketError socketError;
			try
			{
				if (e.m_Buffer != null)
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.WSARecvFrom(this.m_Handle, ref e.m_WSABuffer, 1, out bytesTransferred, ref socketFlags, e.m_PtrSocketAddressBuffer, e.m_PtrSocketAddressBufferSize, e.m_PtrNativeOverlapped, IntPtr.Zero);
				}
				else
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.WSARecvFrom(this.m_Handle, e.m_WSABufferArray, e.m_WSABufferArray.Length, out bytesTransferred, ref socketFlags, e.m_PtrSocketAddressBuffer, e.m_PtrSocketAddressBufferSize, e.m_PtrNativeOverlapped, IntPtr.Zero);
				}
			}
			catch (Exception ex)
			{
				e.Complete();
				throw ex;
			}
			if (socketError != SocketError.Success)
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			bool flag;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, bytesTransferred, socketFlags);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "ReceiveFromAsync", flag);
			}
			return flag;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000C9A20 File Offset: 0x000C8A20
		public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "ReceiveMessageFromAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("RemoteEndPoint");
			}
			EndPoint remoteEndPoint = e.RemoteEndPoint;
			e.m_SocketAddress = this.CheckCacheRemote(ref remoteEndPoint, false);
			if (this.addressFamily == AddressFamily.InterNetwork)
			{
				this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
			}
			else if (this.addressFamily == AddressFamily.InterNetworkV6)
			{
				this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.PacketInformation, true);
			}
			e.StartOperationCommon(this);
			e.StartOperationReceiveMessageFrom();
			this.BindToCompletionPort();
			int bytesTransferred;
			SocketError socketError;
			try
			{
				socketError = this.WSARecvMsg(this.m_Handle, e.m_PtrWSAMessageBuffer, out bytesTransferred, e.m_PtrNativeOverlapped, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				e.Complete();
				throw ex;
			}
			if (socketError != SocketError.Success)
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			bool flag;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, bytesTransferred, SocketFlags.None);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "ReceiveMessageFromAsync", flag);
			}
			return flag;
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000C9B4C File Offset: 0x000C8B4C
		public bool SendAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "SendAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			e.StartOperationCommon(this);
			e.StartOperationSend();
			this.BindToCompletionPort();
			int bytesTransferred;
			SocketError socketError;
			try
			{
				if (e.m_Buffer != null)
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, ref e.m_WSABuffer, 1, out bytesTransferred, e.m_SocketFlags, e.m_PtrNativeOverlapped, IntPtr.Zero);
				}
				else
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, e.m_WSABufferArray, e.m_WSABufferArray.Length, out bytesTransferred, e.m_SocketFlags, e.m_PtrNativeOverlapped, IntPtr.Zero);
				}
			}
			catch (Exception ex)
			{
				e.Complete();
				throw ex;
			}
			if (socketError != SocketError.Success)
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			bool flag;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, bytesTransferred, SocketFlags.None);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "SendAsync", flag);
			}
			return flag;
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000C9C5C File Offset: 0x000C8C5C
		public bool SendPacketsAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "SendPacketsAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (!this.Connected)
			{
				throw new NotSupportedException(SR.GetString("net_notconnected"));
			}
			e.StartOperationCommon(this);
			e.StartOperationSendPackets();
			this.BindToCompletionPort();
			bool flag;
			try
			{
				flag = this.TransmitPackets(this.m_Handle, e.m_PtrSendPacketsDescriptor, e.m_SendPacketsElements.Length, e.m_SendPacketsSendSize, e.m_PtrNativeOverlapped, e.m_SendPacketsFlags);
			}
			catch (Exception ex)
			{
				e.Complete();
				throw ex;
			}
			SocketError socketError;
			if (!flag)
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			else
			{
				socketError = SocketError.Success;
			}
			bool flag2;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, 0, SocketFlags.None);
				flag2 = false;
			}
			else
			{
				flag2 = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "SendPacketsAsync", flag2);
			}
			return flag2;
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000C9D5C File Offset: 0x000C8D5C
		public bool SendToAsync(SocketAsyncEventArgs e)
		{
			if (Socket.s_LoggingEnabled)
			{
				Logging.Enter(Logging.Sockets, this, "SendToAsync", "");
			}
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("RemoteEndPoint");
			}
			EndPoint remoteEndPoint = e.RemoteEndPoint;
			e.m_SocketAddress = this.CheckCacheRemote(ref remoteEndPoint, false);
			e.StartOperationCommon(this);
			e.StartOperationSendTo();
			this.BindToCompletionPort();
			int bytesTransferred;
			SocketError socketError;
			try
			{
				if (e.m_Buffer != null)
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.WSASendTo(this.m_Handle, ref e.m_WSABuffer, 1, out bytesTransferred, e.m_SocketFlags, e.m_PtrSocketAddressBuffer, e.m_SocketAddress.m_Size, e.m_PtrNativeOverlapped, IntPtr.Zero);
				}
				else
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.WSASendTo(this.m_Handle, e.m_WSABufferArray, e.m_WSABufferArray.Length, out bytesTransferred, e.m_SocketFlags, e.m_PtrSocketAddressBuffer, e.m_SocketAddress.m_Size, e.m_PtrNativeOverlapped, IntPtr.Zero);
				}
			}
			catch (Exception ex)
			{
				e.Complete();
				throw ex;
			}
			if (socketError != SocketError.Success)
			{
				socketError = (SocketError)Marshal.GetLastWin32Error();
			}
			bool flag;
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				e.FinishOperationSyncFailure(socketError, bytesTransferred, SocketFlags.None);
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (Socket.s_LoggingEnabled)
			{
				Logging.Exit(Logging.Sockets, this, "SendToAsync", flag);
			}
			return flag;
		}

		// Token: 0x04002B0E RID: 11022
		internal const int DefaultCloseTimeout = -1;

		// Token: 0x04002B0F RID: 11023
		private const int microcnv = 1000000;

		// Token: 0x04002B10 RID: 11024
		private object m_AcceptQueueOrConnectResult;

		// Token: 0x04002B11 RID: 11025
		private SafeCloseSocket m_Handle;

		// Token: 0x04002B12 RID: 11026
		internal EndPoint m_RightEndPoint;

		// Token: 0x04002B13 RID: 11027
		internal EndPoint m_RemoteEndPoint;

		// Token: 0x04002B14 RID: 11028
		private bool m_IsConnected;

		// Token: 0x04002B15 RID: 11029
		private bool m_IsDisconnected;

		// Token: 0x04002B16 RID: 11030
		private bool willBlock = true;

		// Token: 0x04002B17 RID: 11031
		private bool willBlockInternal = true;

		// Token: 0x04002B18 RID: 11032
		private bool isListening;

		// Token: 0x04002B19 RID: 11033
		private bool m_NonBlockingConnectInProgress;

		// Token: 0x04002B1A RID: 11034
		private EndPoint m_NonBlockingConnectRightEndPoint;

		// Token: 0x04002B1B RID: 11035
		private AddressFamily addressFamily;

		// Token: 0x04002B1C RID: 11036
		private SocketType socketType;

		// Token: 0x04002B1D RID: 11037
		private ProtocolType protocolType;

		// Token: 0x04002B1E RID: 11038
		private Socket.CacheSet m_Caches;

		// Token: 0x04002B1F RID: 11039
		internal static bool UseOverlappedIO;

		// Token: 0x04002B20 RID: 11040
		private bool useOverlappedIO;

		// Token: 0x04002B21 RID: 11041
		private bool m_BoundToThreadPool;

		// Token: 0x04002B22 RID: 11042
		private ManualResetEvent m_AsyncEvent;

		// Token: 0x04002B23 RID: 11043
		private RegisteredWaitHandle m_RegisteredWait;

		// Token: 0x04002B24 RID: 11044
		private AsyncEventBits m_BlockEventBits;

		// Token: 0x04002B25 RID: 11045
		private SocketAddress m_PermittedRemoteAddress;

		// Token: 0x04002B26 RID: 11046
		private static Socket.ConnectExDelegate s_ConnectEx;

		// Token: 0x04002B27 RID: 11047
		private static Socket.DisconnectExDelegate s_DisconnectEx;

		// Token: 0x04002B28 RID: 11048
		private static Socket.DisconnectExDelegate_Blocking s_DisconnectEx_Blocking;

		// Token: 0x04002B29 RID: 11049
		private static Socket.WSARecvMsgDelegate s_WSARecvMsg;

		// Token: 0x04002B2A RID: 11050
		private static Socket.WSARecvMsgDelegate_Blocking s_WSARecvMsg_Blocking;

		// Token: 0x04002B2B RID: 11051
		private static Socket.TransmitPacketsDelegate s_TransmitPackets;

		// Token: 0x04002B2C RID: 11052
		private static object s_InternalSyncObject;

		// Token: 0x04002B2D RID: 11053
		private int m_CloseTimeout = -1;

		// Token: 0x04002B2E RID: 11054
		private int m_IntCleanedUp;

		// Token: 0x04002B2F RID: 11055
		private static readonly int protocolInformationSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.WSAPROTOCOL_INFO));

		// Token: 0x04002B30 RID: 11056
		internal static bool s_SupportsIPv4;

		// Token: 0x04002B31 RID: 11057
		internal static bool s_SupportsIPv6;

		// Token: 0x04002B32 RID: 11058
		internal static bool s_OSSupportsIPv6;

		// Token: 0x04002B33 RID: 11059
		internal static bool s_Initialized;

		// Token: 0x04002B34 RID: 11060
		private static WaitOrTimerCallback s_RegisteredWaitCallback;

		// Token: 0x04002B35 RID: 11061
		private static bool s_LoggingEnabled;

		// Token: 0x04002B36 RID: 11062
		internal static bool s_PerfCountersEnabled;

		// Token: 0x020005B2 RID: 1458
		private class CacheSet
		{
			// Token: 0x04002B37 RID: 11063
			internal CallbackClosure ConnectClosureCache;

			// Token: 0x04002B38 RID: 11064
			internal CallbackClosure AcceptClosureCache;

			// Token: 0x04002B39 RID: 11065
			internal CallbackClosure SendClosureCache;

			// Token: 0x04002B3A RID: 11066
			internal CallbackClosure ReceiveClosureCache;

			// Token: 0x04002B3B RID: 11067
			internal OverlappedCache SendOverlappedCache;

			// Token: 0x04002B3C RID: 11068
			internal OverlappedCache ReceiveOverlappedCache;
		}

		// Token: 0x020005B3 RID: 1459
		// (Invoke) Token: 0x06002DBD RID: 11709
		[SuppressUnmanagedCodeSecurity]
		private delegate bool ConnectExDelegate(SafeCloseSocket socketHandle, IntPtr socketAddress, int socketAddressSize, IntPtr buffer, int dataLength, out int bytesSent, SafeHandle overlapped);

		// Token: 0x020005B4 RID: 1460
		// (Invoke) Token: 0x06002DC1 RID: 11713
		[SuppressUnmanagedCodeSecurity]
		private delegate bool DisconnectExDelegate(SafeCloseSocket socketHandle, SafeHandle overlapped, int flags, int reserved);

		// Token: 0x020005B5 RID: 1461
		// (Invoke) Token: 0x06002DC5 RID: 11717
		[SuppressUnmanagedCodeSecurity]
		private delegate bool DisconnectExDelegate_Blocking(IntPtr socketHandle, IntPtr overlapped, int flags, int reserved);

		// Token: 0x020005B6 RID: 1462
		// (Invoke) Token: 0x06002DC9 RID: 11721
		[SuppressUnmanagedCodeSecurity]
		private delegate SocketError WSARecvMsgDelegate(SafeCloseSocket socketHandle, IntPtr msg, out int bytesTransferred, SafeHandle overlapped, IntPtr completionRoutine);

		// Token: 0x020005B7 RID: 1463
		// (Invoke) Token: 0x06002DCD RID: 11725
		[SuppressUnmanagedCodeSecurity]
		private delegate SocketError WSARecvMsgDelegate_Blocking(IntPtr socketHandle, IntPtr msg, out int bytesTransferred, IntPtr overlapped, IntPtr completionRoutine);

		// Token: 0x020005B8 RID: 1464
		// (Invoke) Token: 0x06002DD1 RID: 11729
		[SuppressUnmanagedCodeSecurity]
		private delegate bool TransmitPacketsDelegate(SafeCloseSocket socketHandle, IntPtr packetArray, int elementCount, int sendSize, SafeNativeOverlapped overlapped, TransmitFileOptions flags);

		// Token: 0x020005B9 RID: 1465
		private class MultipleAddressConnectAsyncResult : ContextAwareResult
		{
			// Token: 0x06002DD4 RID: 11732 RVA: 0x000C9ED6 File Offset: 0x000C8ED6
			internal MultipleAddressConnectAsyncResult(IPAddress[] addresses, int port, Socket socket, object myState, AsyncCallback myCallBack) : base(socket, myState, myCallBack)
			{
				this.addresses = addresses;
				this.port = port;
				this.socket = socket;
			}

			// Token: 0x17000998 RID: 2456
			// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x000C9EF8 File Offset: 0x000C8EF8
			internal EndPoint RemoteEndPoint
			{
				get
				{
					if (this.addresses != null && this.index > 0 && this.index < this.addresses.Length)
					{
						return new IPEndPoint(this.addresses[this.index], this.port);
					}
					return null;
				}
			}

			// Token: 0x04002B3D RID: 11069
			internal Socket socket;

			// Token: 0x04002B3E RID: 11070
			internal IPAddress[] addresses;

			// Token: 0x04002B3F RID: 11071
			internal int index;

			// Token: 0x04002B40 RID: 11072
			internal int port;

			// Token: 0x04002B41 RID: 11073
			internal Exception lastException;
		}

		// Token: 0x020005BA RID: 1466
		private class DownLevelSendFileAsyncResult : ContextAwareResult
		{
			// Token: 0x06002DD6 RID: 11734 RVA: 0x000C9F35 File Offset: 0x000C8F35
			internal DownLevelSendFileAsyncResult(FileStream stream, Socket socket, object myState, AsyncCallback myCallBack) : base(socket, myState, myCallBack)
			{
				this.socket = socket;
				this.fileStream = stream;
				this.buffer = new byte[64000];
			}

			// Token: 0x04002B42 RID: 11074
			internal Socket socket;

			// Token: 0x04002B43 RID: 11075
			internal FileStream fileStream;

			// Token: 0x04002B44 RID: 11076
			internal byte[] buffer;

			// Token: 0x04002B45 RID: 11077
			internal bool writing;
		}
	}
}
