using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E7 RID: 1511
	internal class SystemGatewayIPAddressInformation : GatewayIPAddressInformation
	{
		// Token: 0x06002FA7 RID: 12199 RVA: 0x000CEE64 File Offset: 0x000CDE64
		internal SystemGatewayIPAddressInformation(IPAddress address)
		{
			this.address = address;
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06002FA8 RID: 12200 RVA: 0x000CEE73 File Offset: 0x000CDE73
		public override IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x04002CB3 RID: 11443
		private IPAddress address;
	}
}
