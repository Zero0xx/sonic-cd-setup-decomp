using System;

namespace System.Net.Sockets
{
	// Token: 0x020005B0 RID: 1456
	[Serializable]
	public struct SocketInformation
	{
		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x000C1B5B File Offset: 0x000C0B5B
		// (set) Token: 0x06002CD1 RID: 11473 RVA: 0x000C1B63 File Offset: 0x000C0B63
		public byte[] ProtocolInformation
		{
			get
			{
				return this.protocolInformation;
			}
			set
			{
				this.protocolInformation = value;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06002CD2 RID: 11474 RVA: 0x000C1B6C File Offset: 0x000C0B6C
		// (set) Token: 0x06002CD3 RID: 11475 RVA: 0x000C1B74 File Offset: 0x000C0B74
		public SocketInformationOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06002CD4 RID: 11476 RVA: 0x000C1B7D File Offset: 0x000C0B7D
		// (set) Token: 0x06002CD5 RID: 11477 RVA: 0x000C1B8D File Offset: 0x000C0B8D
		internal bool IsNonBlocking
		{
			get
			{
				return (this.options & SocketInformationOptions.NonBlocking) != (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.NonBlocking;
					return;
				}
				this.options &= ~SocketInformationOptions.NonBlocking;
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x000C1BB0 File Offset: 0x000C0BB0
		// (set) Token: 0x06002CD7 RID: 11479 RVA: 0x000C1BC0 File Offset: 0x000C0BC0
		internal bool IsConnected
		{
			get
			{
				return (this.options & SocketInformationOptions.Connected) != (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.Connected;
					return;
				}
				this.options &= ~SocketInformationOptions.Connected;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06002CD8 RID: 11480 RVA: 0x000C1BE3 File Offset: 0x000C0BE3
		// (set) Token: 0x06002CD9 RID: 11481 RVA: 0x000C1BF3 File Offset: 0x000C0BF3
		internal bool IsListening
		{
			get
			{
				return (this.options & SocketInformationOptions.Listening) != (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.Listening;
					return;
				}
				this.options &= ~SocketInformationOptions.Listening;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06002CDA RID: 11482 RVA: 0x000C1C16 File Offset: 0x000C0C16
		// (set) Token: 0x06002CDB RID: 11483 RVA: 0x000C1C26 File Offset: 0x000C0C26
		internal bool UseOnlyOverlappedIO
		{
			get
			{
				return (this.options & SocketInformationOptions.UseOnlyOverlappedIO) != (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.UseOnlyOverlappedIO;
					return;
				}
				this.options &= ~SocketInformationOptions.UseOnlyOverlappedIO;
			}
		}

		// Token: 0x04002B0C RID: 11020
		private byte[] protocolInformation;

		// Token: 0x04002B0D RID: 11021
		private SocketInformationOptions options;
	}
}
