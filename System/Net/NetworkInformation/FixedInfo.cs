using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062E RID: 1582
	internal struct FixedInfo
	{
		// Token: 0x060030CD RID: 12493 RVA: 0x000D2745 File Offset: 0x000D1745
		internal FixedInfo(FIXED_INFO info)
		{
			this.info = info;
			this.dnsAddresses = info.DnsServerList.ToIPAddressCollection();
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x060030CE RID: 12494 RVA: 0x000D2760 File Offset: 0x000D1760
		internal IPAddressCollection DnsAddresses
		{
			get
			{
				return this.dnsAddresses;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x060030CF RID: 12495 RVA: 0x000D2768 File Offset: 0x000D1768
		internal string HostName
		{
			get
			{
				return this.info.hostName;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060030D0 RID: 12496 RVA: 0x000D2775 File Offset: 0x000D1775
		internal string DomainName
		{
			get
			{
				return this.info.domainName;
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x000D2782 File Offset: 0x000D1782
		internal NetBiosNodeType NodeType
		{
			get
			{
				return this.info.nodeType;
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x000D278F File Offset: 0x000D178F
		internal string ScopeId
		{
			get
			{
				return this.info.scopeId;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x000D279C File Offset: 0x000D179C
		internal bool EnableRouting
		{
			get
			{
				return this.info.enableRouting;
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x000D27A9 File Offset: 0x000D17A9
		internal bool EnableProxy
		{
			get
			{
				return this.info.enableProxy;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x000D27B6 File Offset: 0x000D17B6
		internal bool EnableDns
		{
			get
			{
				return this.info.enableDns;
			}
		}

		// Token: 0x04002E43 RID: 11843
		internal FIXED_INFO info;

		// Token: 0x04002E44 RID: 11844
		internal IPAddressCollection dnsAddresses;
	}
}
