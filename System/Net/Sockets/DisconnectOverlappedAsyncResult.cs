using System;

namespace System.Net.Sockets
{
	// Token: 0x020005CF RID: 1487
	internal class DisconnectOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002EB8 RID: 11960 RVA: 0x000CE1F0 File Offset: 0x000CD1F0
		internal DisconnectOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback) : base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000CE1FC File Offset: 0x000CD1FC
		internal override object PostCompletion(int numBytes)
		{
			if (base.ErrorCode == 0)
			{
				Socket socket = (Socket)base.AsyncObject;
				socket.SetToDisconnected();
				socket.m_RemoteEndPoint = null;
			}
			return base.PostCompletion(numBytes);
		}
	}
}
