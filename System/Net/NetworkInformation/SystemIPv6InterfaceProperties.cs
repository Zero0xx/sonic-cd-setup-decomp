using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000635 RID: 1589
	internal class SystemIPv6InterfaceProperties : IPv6InterfaceProperties
	{
		// Token: 0x06003138 RID: 12600 RVA: 0x000D37F0 File Offset: 0x000D27F0
		internal SystemIPv6InterfaceProperties(uint index, uint mtu)
		{
			this.index = index;
			this.mtu = mtu;
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x000D3806 File Offset: 0x000D2806
		public override int Index
		{
			get
			{
				return (int)this.index;
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x000D380E File Offset: 0x000D280E
		public override int Mtu
		{
			get
			{
				return (int)this.mtu;
			}
		}

		// Token: 0x04002E67 RID: 11879
		private uint index;

		// Token: 0x04002E68 RID: 11880
		private uint mtu;
	}
}
