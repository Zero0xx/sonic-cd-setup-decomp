using System;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting
{
	// Token: 0x02000735 RID: 1845
	[Serializable]
	internal sealed class ChannelInfo : IChannelInfo
	{
		// Token: 0x06004208 RID: 16904 RVA: 0x000E08FC File Offset: 0x000DF8FC
		internal ChannelInfo()
		{
			this.ChannelData = ChannelServices.CurrentChannelData;
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004209 RID: 16905 RVA: 0x000E090F File Offset: 0x000DF90F
		// (set) Token: 0x0600420A RID: 16906 RVA: 0x000E0917 File Offset: 0x000DF917
		public object[] ChannelData
		{
			get
			{
				return this.channelData;
			}
			set
			{
				this.channelData = value;
			}
		}

		// Token: 0x04002119 RID: 8473
		private object[] channelData;
	}
}
