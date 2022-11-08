using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting
{
	// Token: 0x020006C3 RID: 1731
	internal class DelayLoadClientChannelEntry
	{
		// Token: 0x06003E6B RID: 15979 RVA: 0x000D6332 File Offset: 0x000D5332
		internal DelayLoadClientChannelEntry(RemotingXmlConfigFileData.ChannelEntry entry, bool ensureSecurity)
		{
			this._entry = entry;
			this._channel = null;
			this._bRegistered = false;
			this._ensureSecurity = ensureSecurity;
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06003E6C RID: 15980 RVA: 0x000D6356 File Offset: 0x000D5356
		internal IChannelSender Channel
		{
			get
			{
				if (this._channel == null && !this._bRegistered)
				{
					this._channel = (IChannelSender)RemotingConfigHandler.CreateChannelFromConfigEntry(this._entry);
					this._entry = null;
				}
				return this._channel;
			}
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x000D638B File Offset: 0x000D538B
		internal void RegisterChannel()
		{
			ChannelServices.RegisterChannel(this._channel, this._ensureSecurity);
			this._bRegistered = true;
			this._channel = null;
		}

		// Token: 0x04001FC9 RID: 8137
		private RemotingXmlConfigFileData.ChannelEntry _entry;

		// Token: 0x04001FCA RID: 8138
		private IChannelSender _channel;

		// Token: 0x04001FCB RID: 8139
		private bool _bRegistered;

		// Token: 0x04001FCC RID: 8140
		private bool _ensureSecurity;
	}
}
