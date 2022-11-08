using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x020003ED RID: 1005
	internal class NetworkAddressChangePolled : IDisposable
	{
		// Token: 0x06002080 RID: 8320 RVA: 0x000802E0 File Offset: 0x0007F2E0
		internal NetworkAddressChangePolled()
		{
			Socket.InitializeSockets();
			if (Socket.SupportsIPv4)
			{
				int num = -1;
				this.ipv4Socket = SafeCloseSocketAndEvent.CreateWSASocketWithEvent(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP, true, false);
				UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.ipv4Socket, -2147195266, ref num);
			}
			if (Socket.OSSupportsIPv6)
			{
				int num = -1;
				this.ipv6Socket = SafeCloseSocketAndEvent.CreateWSASocketWithEvent(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.IP, true, false);
				UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.ipv6Socket, -2147195266, ref num);
			}
			this.Setup(StartIPOptions.Both);
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00080358 File Offset: 0x0007F358
		private void Setup(StartIPOptions startIPOptions)
		{
			if (Socket.SupportsIPv4 && (startIPOptions & StartIPOptions.StartIPv4) != StartIPOptions.None)
			{
				int num;
				SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(this.ipv4Socket.DangerousGetHandle(), 671088663, null, 0, null, 0, out num, SafeNativeOverlapped.Zero, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					NetworkInformationException ex = new NetworkInformationException();
					if ((long)ex.ErrorCode != 10035L)
					{
						this.Dispose();
						return;
					}
				}
				socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(this.ipv4Socket, this.ipv4Socket.GetEventHandle().SafeWaitHandle, AsyncEventBits.FdAddressListChange);
				if (socketError != SocketError.Success)
				{
					this.Dispose();
					return;
				}
			}
			if (Socket.OSSupportsIPv6 && (startIPOptions & StartIPOptions.StartIPv6) != StartIPOptions.None)
			{
				int num;
				SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(this.ipv6Socket.DangerousGetHandle(), 671088663, null, 0, null, 0, out num, SafeNativeOverlapped.Zero, IntPtr.Zero);
				if (socketError != SocketError.Success)
				{
					NetworkInformationException ex2 = new NetworkInformationException();
					if ((long)ex2.ErrorCode != 10035L)
					{
						this.Dispose();
						return;
					}
				}
				socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(this.ipv6Socket, this.ipv6Socket.GetEventHandle().SafeWaitHandle, AsyncEventBits.FdAddressListChange);
				if (socketError != SocketError.Success)
				{
					this.Dispose();
				}
			}
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x0008045C File Offset: 0x0007F45C
		internal bool CheckAndReset()
		{
			if (!this.disposed)
			{
				lock (this)
				{
					if (!this.disposed)
					{
						StartIPOptions startIPOptions = StartIPOptions.None;
						if (this.ipv4Socket != null && this.ipv4Socket.GetEventHandle().WaitOne(0, false))
						{
							startIPOptions |= StartIPOptions.StartIPv4;
						}
						if (this.ipv6Socket != null && this.ipv6Socket.GetEventHandle().WaitOne(0, false))
						{
							startIPOptions |= StartIPOptions.StartIPv6;
						}
						if (startIPOptions != StartIPOptions.None)
						{
							this.Setup(startIPOptions);
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000804F0 File Offset: 0x0007F4F0
		public void Dispose()
		{
			if (!this.disposed)
			{
				lock (this)
				{
					if (!this.disposed)
					{
						if (this.ipv6Socket != null)
						{
							this.ipv6Socket.Close();
							this.ipv6Socket = null;
						}
						if (this.ipv4Socket != null)
						{
							this.ipv4Socket.Close();
							this.ipv6Socket = null;
						}
						this.disposed = true;
					}
				}
			}
		}

		// Token: 0x04001FBA RID: 8122
		private bool disposed;

		// Token: 0x04001FBB RID: 8123
		private SafeCloseSocketAndEvent ipv4Socket;

		// Token: 0x04001FBC RID: 8124
		private SafeCloseSocketAndEvent ipv6Socket;
	}
}
