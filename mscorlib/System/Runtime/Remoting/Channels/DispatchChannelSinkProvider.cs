using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006D5 RID: 1749
	internal class DispatchChannelSinkProvider : IServerChannelSinkProvider
	{
		// Token: 0x06003F02 RID: 16130 RVA: 0x000D7F3A File Offset: 0x000D6F3A
		internal DispatchChannelSinkProvider()
		{
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x000D7F42 File Offset: 0x000D6F42
		public void GetChannelData(IChannelDataStore channelData)
		{
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x000D7F44 File Offset: 0x000D6F44
		public IServerChannelSink CreateSink(IChannelReceiver channel)
		{
			return new DispatchChannelSink();
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003F05 RID: 16133 RVA: 0x000D7F4B File Offset: 0x000D6F4B
		// (set) Token: 0x06003F06 RID: 16134 RVA: 0x000D7F4E File Offset: 0x000D6F4E
		public IServerChannelSinkProvider Next
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException();
			}
		}
	}
}
