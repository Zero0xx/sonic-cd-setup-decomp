using System;

namespace System.Net.Sockets
{
	// Token: 0x020005BB RID: 1467
	internal class ConnectAsyncResult : ContextAwareResult
	{
		// Token: 0x06002DD7 RID: 11735 RVA: 0x000C9F5F File Offset: 0x000C8F5F
		internal ConnectAsyncResult(object myObject, EndPoint endPoint, object myState, AsyncCallback myCallBack) : base(myObject, myState, myCallBack)
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06002DD8 RID: 11736 RVA: 0x000C9F72 File Offset: 0x000C8F72
		internal EndPoint RemoteEndPoint
		{
			get
			{
				return this.m_EndPoint;
			}
		}

		// Token: 0x04002B46 RID: 11078
		private EndPoint m_EndPoint;
	}
}
