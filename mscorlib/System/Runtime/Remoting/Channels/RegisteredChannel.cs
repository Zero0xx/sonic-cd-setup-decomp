using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B0 RID: 1712
	internal class RegisteredChannel
	{
		// Token: 0x06003DDD RID: 15837 RVA: 0x000D3BC8 File Offset: 0x000D2BC8
		internal RegisteredChannel(IChannel chnl)
		{
			this.channel = chnl;
			this.flags = 0;
			if (chnl is IChannelSender)
			{
				this.flags |= 1;
			}
			if (chnl is IChannelReceiver)
			{
				this.flags |= 2;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06003DDE RID: 15838 RVA: 0x000D3C17 File Offset: 0x000D2C17
		internal virtual IChannel Channel
		{
			get
			{
				return this.channel;
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x000D3C1F File Offset: 0x000D2C1F
		internal virtual bool IsSender()
		{
			return (this.flags & 1) != 0;
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x000D3C2F File Offset: 0x000D2C2F
		internal virtual bool IsReceiver()
		{
			return (this.flags & 2) != 0;
		}

		// Token: 0x04001F8F RID: 8079
		private const byte SENDER = 1;

		// Token: 0x04001F90 RID: 8080
		private const byte RECEIVER = 2;

		// Token: 0x04001F91 RID: 8081
		private IChannel channel;

		// Token: 0x04001F92 RID: 8082
		private byte flags;
	}
}
