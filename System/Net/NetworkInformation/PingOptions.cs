using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000625 RID: 1573
	public class PingOptions
	{
		// Token: 0x06003062 RID: 12386 RVA: 0x000D14A1 File Offset: 0x000D04A1
		internal PingOptions(IPOptions options)
		{
			this.ttl = (int)options.ttl;
			this.dontFragment = ((options.flags & 2) > 0);
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x000D14D7 File Offset: 0x000D04D7
		public PingOptions(int ttl, bool dontFragment)
		{
			if (ttl <= 0)
			{
				throw new ArgumentOutOfRangeException("ttl");
			}
			this.ttl = ttl;
			this.dontFragment = dontFragment;
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000D1507 File Offset: 0x000D0507
		public PingOptions()
		{
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003065 RID: 12389 RVA: 0x000D151A File Offset: 0x000D051A
		// (set) Token: 0x06003066 RID: 12390 RVA: 0x000D1522 File Offset: 0x000D0522
		public int Ttl
		{
			get
			{
				return this.ttl;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ttl = value;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x000D153A File Offset: 0x000D053A
		// (set) Token: 0x06003068 RID: 12392 RVA: 0x000D1542 File Offset: 0x000D0542
		public bool DontFragment
		{
			get
			{
				return this.dontFragment;
			}
			set
			{
				this.dontFragment = value;
			}
		}

		// Token: 0x04002E15 RID: 11797
		private const int DontFragmentFlag = 2;

		// Token: 0x04002E16 RID: 11798
		private int ttl = 128;

		// Token: 0x04002E17 RID: 11799
		private bool dontFragment;
	}
}
