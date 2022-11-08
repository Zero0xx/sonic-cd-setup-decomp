using System;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x020005CE RID: 1486
	internal class ConnectOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002EB5 RID: 11957 RVA: 0x000CE160 File Offset: 0x000CD160
		internal ConnectOverlappedAsyncResult(Socket socket, EndPoint endPoint, object asyncState, AsyncCallback asyncCallback) : base(socket, asyncState, asyncCallback)
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000CE174 File Offset: 0x000CD174
		internal override object PostCompletion(int numBytes)
		{
			SocketError socketError = (SocketError)base.ErrorCode;
			Socket socket = (Socket)base.AsyncObject;
			if (socketError == SocketError.Success)
			{
				try
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(socket.SafeHandle, SocketOptionLevel.Socket, SocketOptionName.UpdateConnectContext, null, 0);
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
				socket.SetToConnected();
				return socket;
			}
			return null;
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06002EB7 RID: 11959 RVA: 0x000CE1E8 File Offset: 0x000CD1E8
		internal EndPoint RemoteEndPoint
		{
			get
			{
				return this.m_EndPoint;
			}
		}

		// Token: 0x04002C42 RID: 11330
		private EndPoint m_EndPoint;
	}
}
