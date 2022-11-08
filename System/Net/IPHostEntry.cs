using System;

namespace System.Net
{
	// Token: 0x02000423 RID: 1059
	public class IPHostEntry
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x0600210D RID: 8461 RVA: 0x000827AE File Offset: 0x000817AE
		// (set) Token: 0x0600210E RID: 8462 RVA: 0x000827B6 File Offset: 0x000817B6
		public string HostName
		{
			get
			{
				return this.hostName;
			}
			set
			{
				this.hostName = value;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x0600210F RID: 8463 RVA: 0x000827BF File Offset: 0x000817BF
		// (set) Token: 0x06002110 RID: 8464 RVA: 0x000827C7 File Offset: 0x000817C7
		public string[] Aliases
		{
			get
			{
				return this.aliases;
			}
			set
			{
				this.aliases = value;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x000827D0 File Offset: 0x000817D0
		// (set) Token: 0x06002112 RID: 8466 RVA: 0x000827D8 File Offset: 0x000817D8
		public IPAddress[] AddressList
		{
			get
			{
				return this.addressList;
			}
			set
			{
				this.addressList = value;
			}
		}

		// Token: 0x0400215A RID: 8538
		private string hostName;

		// Token: 0x0400215B RID: 8539
		private string[] aliases;

		// Token: 0x0400215C RID: 8540
		private IPAddress[] addressList;
	}
}
