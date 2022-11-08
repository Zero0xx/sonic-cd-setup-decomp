using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000639 RID: 1593
	internal class SystemTcpConnectionInformation : TcpConnectionInformation
	{
		// Token: 0x06003155 RID: 12629 RVA: 0x000D403C File Offset: 0x000D303C
		internal SystemTcpConnectionInformation(MibTcpRow row)
		{
			this.state = row.state;
			int port = (int)row.localPort3 << 24 | (int)row.localPort4 << 16 | (int)row.localPort1 << 8 | (int)row.localPort2;
			int port2 = (this.state == TcpState.Listen) ? 0 : ((int)row.remotePort3 << 24 | (int)row.remotePort4 << 16 | (int)row.remotePort1 << 8 | (int)row.remotePort2);
			this.localEndPoint = new IPEndPoint((long)((ulong)row.localAddr), port);
			this.remoteEndPoint = new IPEndPoint((long)((ulong)row.remoteAddr), port2);
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06003156 RID: 12630 RVA: 0x000D40E0 File Offset: 0x000D30E0
		public override TcpState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06003157 RID: 12631 RVA: 0x000D40E8 File Offset: 0x000D30E8
		public override IPEndPoint LocalEndPoint
		{
			get
			{
				return this.localEndPoint;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06003158 RID: 12632 RVA: 0x000D40F0 File Offset: 0x000D30F0
		public override IPEndPoint RemoteEndPoint
		{
			get
			{
				return this.remoteEndPoint;
			}
		}

		// Token: 0x04002E79 RID: 11897
		private IPEndPoint localEndPoint;

		// Token: 0x04002E7A RID: 11898
		private IPEndPoint remoteEndPoint;

		// Token: 0x04002E7B RID: 11899
		private TcpState state;
	}
}
