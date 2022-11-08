using System;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x020005CC RID: 1484
	internal class AcceptOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002EA4 RID: 11940 RVA: 0x000CDE1E File Offset: 0x000CCE1E
		internal AcceptOverlappedAsyncResult(Socket listenSocket, object asyncState, AsyncCallback asyncCallback) : base(listenSocket, asyncState, asyncCallback)
		{
			this.m_ListenSocket = listenSocket;
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x000CDE30 File Offset: 0x000CCE30
		internal override object PostCompletion(int numBytes)
		{
			SocketError socketError = (SocketError)base.ErrorCode;
			SocketAddress socketAddress = null;
			if (socketError == SocketError.Success)
			{
				this.m_LocalBytesTransferred = numBytes;
				if (Logging.On)
				{
					this.LogBuffer((long)numBytes);
				}
				socketAddress = this.m_ListenSocket.m_RightEndPoint.Serialize();
				IntPtr intPtr;
				int num;
				IntPtr source;
				UnsafeNclNativeMethods.OSSOCK.GetAcceptExSockaddrs(Marshal.UnsafeAddrOfPinnedArrayElement(this.m_Buffer, 0), this.m_Buffer.Length - this.m_AddressBufferLength * 2, this.m_AddressBufferLength, this.m_AddressBufferLength, out intPtr, out num, out source, out socketAddress.m_Size);
				Marshal.Copy(source, socketAddress.m_Buffer, 0, socketAddress.m_Size);
				try
				{
					IntPtr intPtr2 = this.m_ListenSocket.SafeHandle.DangerousGetHandle();
					socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_AcceptSocket.SafeHandle, SocketOptionLevel.Socket, SocketOptionName.UpdateAcceptContext, ref intPtr2, Marshal.SizeOf(intPtr2));
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
				}
				catch (ObjectDisposedException)
				{
					socketError = SocketError.OperationAborted;
				}
				base.ErrorCode = (int)socketError;
			}
			if (socketError == SocketError.Success)
			{
				return this.m_ListenSocket.UpdateAcceptSocket(this.m_AcceptSocket, this.m_ListenSocket.m_RightEndPoint.Create(socketAddress), false);
			}
			return null;
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x000CDF50 File Offset: 0x000CCF50
		internal void SetUnmanagedStructures(byte[] buffer, int addressBufferLength)
		{
			base.SetUnmanagedStructures(buffer);
			this.m_AddressBufferLength = addressBufferLength;
			this.m_Buffer = buffer;
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x000CDF68 File Offset: 0x000CCF68
		private void LogBuffer(long size)
		{
			IntPtr intPtr = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_Buffer, 0);
			if (intPtr != IntPtr.Zero)
			{
				if (size > -1L)
				{
					Logging.Dump(Logging.Sockets, this.m_ListenSocket, "PostCompletion", intPtr, (int)Math.Min(size, (long)this.m_Buffer.Length));
					return;
				}
				Logging.Dump(Logging.Sockets, this.m_ListenSocket, "PostCompletion", intPtr, this.m_Buffer.Length);
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x000CDFD9 File Offset: 0x000CCFD9
		internal byte[] Buffer
		{
			get
			{
				return this.m_Buffer;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06002EA9 RID: 11945 RVA: 0x000CDFE1 File Offset: 0x000CCFE1
		internal int BytesTransferred
		{
			get
			{
				return this.m_LocalBytesTransferred;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (set) Token: 0x06002EAA RID: 11946 RVA: 0x000CDFE9 File Offset: 0x000CCFE9
		internal Socket AcceptSocket
		{
			set
			{
				this.m_AcceptSocket = value;
			}
		}

		// Token: 0x04002C39 RID: 11321
		private int m_LocalBytesTransferred;

		// Token: 0x04002C3A RID: 11322
		private Socket m_ListenSocket;

		// Token: 0x04002C3B RID: 11323
		private Socket m_AcceptSocket;

		// Token: 0x04002C3C RID: 11324
		private int m_AddressBufferLength;

		// Token: 0x04002C3D RID: 11325
		private byte[] m_Buffer;
	}
}
