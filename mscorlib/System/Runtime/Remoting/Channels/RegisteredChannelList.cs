using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B1 RID: 1713
	internal class RegisteredChannelList
	{
		// Token: 0x06003DE1 RID: 15841 RVA: 0x000D3C3F File Offset: 0x000D2C3F
		internal RegisteredChannelList()
		{
			this._channels = new RegisteredChannel[0];
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x000D3C53 File Offset: 0x000D2C53
		internal RegisteredChannelList(RegisteredChannel[] channels)
		{
			this._channels = channels;
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06003DE3 RID: 15843 RVA: 0x000D3C62 File Offset: 0x000D2C62
		internal RegisteredChannel[] RegisteredChannels
		{
			get
			{
				return this._channels;
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x000D3C6A File Offset: 0x000D2C6A
		internal int Count
		{
			get
			{
				if (this._channels == null)
				{
					return 0;
				}
				return this._channels.Length;
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x000D3C7E File Offset: 0x000D2C7E
		internal IChannel GetChannel(int index)
		{
			return this._channels[index].Channel;
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x000D3C8D File Offset: 0x000D2C8D
		internal bool IsSender(int index)
		{
			return this._channels[index].IsSender();
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x000D3C9C File Offset: 0x000D2C9C
		internal bool IsReceiver(int index)
		{
			return this._channels[index].IsReceiver();
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06003DE8 RID: 15848 RVA: 0x000D3CAC File Offset: 0x000D2CAC
		internal int ReceiverCount
		{
			get
			{
				if (this._channels == null)
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this._channels.Length; i++)
				{
					if (this.IsReceiver(i))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x000D3CE8 File Offset: 0x000D2CE8
		internal int FindChannelIndex(IChannel channel)
		{
			for (int i = 0; i < this._channels.Length; i++)
			{
				if (channel == this.GetChannel(i))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x000D3D18 File Offset: 0x000D2D18
		internal int FindChannelIndex(string name)
		{
			for (int i = 0; i < this._channels.Length; i++)
			{
				if (string.Compare(name, this.GetChannel(i).ChannelName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x04001F93 RID: 8083
		private RegisteredChannel[] _channels;
	}
}
