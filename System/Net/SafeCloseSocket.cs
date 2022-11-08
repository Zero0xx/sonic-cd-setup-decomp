using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000526 RID: 1318
	[SuppressUnmanagedCodeSecurity]
	internal class SafeCloseSocket : SafeHandleMinusOneIsInvalid
	{
		// Token: 0x06002867 RID: 10343 RVA: 0x000A74D2 File Offset: 0x000A64D2
		protected SafeCloseSocket() : base(true)
		{
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002868 RID: 10344 RVA: 0x000A74DB File Offset: 0x000A64DB
		public override bool IsInvalid
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return base.IsClosed || base.IsInvalid;
			}
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x000A74ED File Offset: 0x000A64ED
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void SetInnerSocket(SafeCloseSocket.InnerSafeCloseSocket socket)
		{
			this.m_InnerSocket = socket;
			base.SetHandle(socket.DangerousGetHandle());
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x000A7504 File Offset: 0x000A6504
		private static SafeCloseSocket CreateSocket(SafeCloseSocket.InnerSafeCloseSocket socket)
		{
			SafeCloseSocket safeCloseSocket = new SafeCloseSocket();
			SafeCloseSocket.CreateSocket(socket, safeCloseSocket);
			return safeCloseSocket;
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x000A7520 File Offset: 0x000A6520
		protected static void CreateSocket(SafeCloseSocket.InnerSafeCloseSocket socket, SafeCloseSocket target)
		{
			if (socket != null && socket.IsInvalid)
			{
				target.SetHandleAsInvalid();
				return;
			}
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				socket.DangerousAddRef(ref flag);
			}
			catch
			{
				if (flag)
				{
					socket.DangerousRelease();
					flag = false;
				}
			}
			finally
			{
				if (flag)
				{
					target.SetInnerSocket(socket);
					socket.Close();
				}
				else
				{
					target.SetHandleAsInvalid();
				}
			}
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000A7598 File Offset: 0x000A6598
		internal unsafe static SafeCloseSocket CreateWSASocket(byte* pinnedBuffer)
		{
			return SafeCloseSocket.CreateSocket(SafeCloseSocket.InnerSafeCloseSocket.CreateWSASocket(pinnedBuffer));
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000A75A5 File Offset: 0x000A65A5
		internal static SafeCloseSocket CreateWSASocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
		{
			return SafeCloseSocket.CreateSocket(SafeCloseSocket.InnerSafeCloseSocket.CreateWSASocket(addressFamily, socketType, protocolType));
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000A75B4 File Offset: 0x000A65B4
		internal static SafeCloseSocket Accept(SafeCloseSocket socketHandle, byte[] socketAddress, ref int socketAddressSize)
		{
			return SafeCloseSocket.CreateSocket(SafeCloseSocket.InnerSafeCloseSocket.Accept(socketHandle, socketAddress, ref socketAddressSize));
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000A75C4 File Offset: 0x000A65C4
		protected override bool ReleaseHandle()
		{
			this.m_Released = true;
			SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = (this.m_InnerSocket == null) ? null : Interlocked.Exchange<SafeCloseSocket.InnerSafeCloseSocket>(ref this.m_InnerSocket, null);
			if (innerSafeCloseSocket != null)
			{
				innerSafeCloseSocket.DangerousRelease();
			}
			return true;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000A75FC File Offset: 0x000A65FC
		internal void CloseAsIs()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = (this.m_InnerSocket == null) ? null : Interlocked.Exchange<SafeCloseSocket.InnerSafeCloseSocket>(ref this.m_InnerSocket, null);
				base.Close();
				if (innerSafeCloseSocket != null)
				{
					while (!this.m_Released)
					{
						Thread.SpinWait(1);
					}
					innerSafeCloseSocket.BlockingRelease();
				}
			}
		}

		// Token: 0x0400277F RID: 10111
		private SafeCloseSocket.InnerSafeCloseSocket m_InnerSocket;

		// Token: 0x04002780 RID: 10112
		private volatile bool m_Released;

		// Token: 0x02000527 RID: 1319
		internal class InnerSafeCloseSocket : SafeHandleMinusOneIsInvalid
		{
			// Token: 0x06002871 RID: 10353 RVA: 0x000A765C File Offset: 0x000A665C
			protected InnerSafeCloseSocket() : base(true)
			{
			}

			// Token: 0x17000844 RID: 2116
			// (get) Token: 0x06002872 RID: 10354 RVA: 0x000A7665 File Offset: 0x000A6665
			public override bool IsInvalid
			{
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				get
				{
					return base.IsClosed || base.IsInvalid;
				}
			}

			// Token: 0x06002873 RID: 10355 RVA: 0x000A7678 File Offset: 0x000A6678
			protected override bool ReleaseHandle()
			{
				SocketError socketError;
				if (this.m_Blockable)
				{
					socketError = UnsafeNclNativeMethods.SafeNetHandles.closesocket(this.handle);
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
					if (socketError != SocketError.WouldBlock)
					{
						return socketError == SocketError.Success;
					}
					int num = 0;
					socketError = UnsafeNclNativeMethods.SafeNetHandles.ioctlsocket(this.handle, -2147195266, ref num);
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
					if (socketError == SocketError.InvalidArgument)
					{
						socketError = UnsafeNclNativeMethods.SafeNetHandles.WSAEventSelect(this.handle, IntPtr.Zero, AsyncEventBits.FdNone);
						socketError = UnsafeNclNativeMethods.SafeNetHandles.ioctlsocket(this.handle, -2147195266, ref num);
					}
					if (socketError == SocketError.Success)
					{
						socketError = UnsafeNclNativeMethods.SafeNetHandles.closesocket(this.handle);
						if (socketError == SocketError.SocketError)
						{
							socketError = (SocketError)Marshal.GetLastWin32Error();
						}
						if (socketError != SocketError.WouldBlock)
						{
							return socketError == SocketError.Success;
						}
					}
				}
				Linger linger;
				linger.OnOff = 1;
				linger.Time = 0;
				socketError = UnsafeNclNativeMethods.SafeNetHandles.setsockopt(this.handle, SocketOptionLevel.Socket, SocketOptionName.Linger, ref linger, 4);
				if (socketError == SocketError.SocketError)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
				if (socketError != SocketError.Success && socketError != SocketError.InvalidArgument && socketError != SocketError.ProtocolOption)
				{
					return false;
				}
				socketError = UnsafeNclNativeMethods.SafeNetHandles.closesocket(this.handle);
				return socketError == SocketError.Success;
			}

			// Token: 0x06002874 RID: 10356 RVA: 0x000A777D File Offset: 0x000A677D
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			internal void BlockingRelease()
			{
				this.m_Blockable = true;
				base.DangerousRelease();
			}

			// Token: 0x06002875 RID: 10357 RVA: 0x000A778C File Offset: 0x000A678C
			internal unsafe static SafeCloseSocket.InnerSafeCloseSocket CreateWSASocket(byte* pinnedBuffer)
			{
				SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = UnsafeNclNativeMethods.OSSOCK.WSASocket(AddressFamily.Unknown, SocketType.Unknown, ProtocolType.Unknown, pinnedBuffer, 0U, SocketConstructorFlags.WSA_FLAG_OVERLAPPED);
				if (innerSafeCloseSocket.IsInvalid)
				{
					innerSafeCloseSocket.SetHandleAsInvalid();
				}
				return innerSafeCloseSocket;
			}

			// Token: 0x06002876 RID: 10358 RVA: 0x000A77B4 File Offset: 0x000A67B4
			internal static SafeCloseSocket.InnerSafeCloseSocket CreateWSASocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
			{
				SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = UnsafeNclNativeMethods.OSSOCK.WSASocket(addressFamily, socketType, protocolType, IntPtr.Zero, 0U, SocketConstructorFlags.WSA_FLAG_OVERLAPPED);
				if (innerSafeCloseSocket.IsInvalid)
				{
					innerSafeCloseSocket.SetHandleAsInvalid();
				}
				return innerSafeCloseSocket;
			}

			// Token: 0x06002877 RID: 10359 RVA: 0x000A77E0 File Offset: 0x000A67E0
			internal static SafeCloseSocket.InnerSafeCloseSocket Accept(SafeCloseSocket socketHandle, byte[] socketAddress, ref int socketAddressSize)
			{
				SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = UnsafeNclNativeMethods.SafeNetHandles.accept(socketHandle.DangerousGetHandle(), socketAddress, ref socketAddressSize);
				if (innerSafeCloseSocket.IsInvalid)
				{
					innerSafeCloseSocket.SetHandleAsInvalid();
				}
				return innerSafeCloseSocket;
			}

			// Token: 0x04002781 RID: 10113
			private static readonly byte[] tempBuffer = new byte[1];

			// Token: 0x04002782 RID: 10114
			private bool m_Blockable;
		}
	}
}
