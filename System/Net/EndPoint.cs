using System;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x020003A7 RID: 935
	[Serializable]
	public abstract class EndPoint
	{
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001D42 RID: 7490 RVA: 0x0006FF43 File Offset: 0x0006EF43
		public virtual AddressFamily AddressFamily
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x0006FF4A File Offset: 0x0006EF4A
		public virtual SocketAddress Serialize()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0006FF51 File Offset: 0x0006EF51
		public virtual EndPoint Create(SocketAddress socketAddress)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}
	}
}
