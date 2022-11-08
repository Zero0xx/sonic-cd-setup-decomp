using System;

namespace System.Net.Sockets
{
	// Token: 0x020005AB RID: 1451
	public class IPv6MulticastOption
	{
		// Token: 0x06002CCA RID: 11466 RVA: 0x000C1AB8 File Offset: 0x000C0AB8
		public IPv6MulticastOption(IPAddress group, long ifindex)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (ifindex < 0L || ifindex > (long)((ulong)-1))
			{
				throw new ArgumentOutOfRangeException("ifindex");
			}
			this.Group = group;
			this.InterfaceIndex = ifindex;
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000C1AF1 File Offset: 0x000C0AF1
		public IPv6MulticastOption(IPAddress group)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			this.Group = group;
			this.InterfaceIndex = 0L;
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06002CCC RID: 11468 RVA: 0x000C1B16 File Offset: 0x000C0B16
		// (set) Token: 0x06002CCD RID: 11469 RVA: 0x000C1B1E File Offset: 0x000C0B1E
		public IPAddress Group
		{
			get
			{
				return this.m_Group;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_Group = value;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x000C1B35 File Offset: 0x000C0B35
		// (set) Token: 0x06002CCF RID: 11471 RVA: 0x000C1B3D File Offset: 0x000C0B3D
		public long InterfaceIndex
		{
			get
			{
				return this.m_Interface;
			}
			set
			{
				if (value < 0L || value > (long)((ulong)-1))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_Interface = value;
			}
		}

		// Token: 0x04002AC7 RID: 10951
		private IPAddress m_Group;

		// Token: 0x04002AC8 RID: 10952
		private long m_Interface;
	}
}
