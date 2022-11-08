using System;

namespace System.Net.Sockets
{
	// Token: 0x020005AA RID: 1450
	public class MulticastOption
	{
		// Token: 0x06002CC1 RID: 11457 RVA: 0x000C198F File Offset: 0x000C098F
		public MulticastOption(IPAddress group, IPAddress mcint)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (mcint == null)
			{
				throw new ArgumentNullException("mcint");
			}
			this.Group = group;
			this.LocalAddress = mcint;
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x000C19C4 File Offset: 0x000C09C4
		public MulticastOption(IPAddress group, int interfaceIndex)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (interfaceIndex < 0 || interfaceIndex > 16777215)
			{
				throw new ArgumentOutOfRangeException("interfaceIndex");
			}
			if (!ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			this.Group = group;
			this.ifIndex = interfaceIndex;
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x000C1A21 File Offset: 0x000C0A21
		public MulticastOption(IPAddress group)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			this.Group = group;
			this.LocalAddress = IPAddress.Any;
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x000C1A49 File Offset: 0x000C0A49
		// (set) Token: 0x06002CC5 RID: 11461 RVA: 0x000C1A51 File Offset: 0x000C0A51
		public IPAddress Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x000C1A5A File Offset: 0x000C0A5A
		// (set) Token: 0x06002CC7 RID: 11463 RVA: 0x000C1A62 File Offset: 0x000C0A62
		public IPAddress LocalAddress
		{
			get
			{
				return this.localAddress;
			}
			set
			{
				this.ifIndex = 0;
				this.localAddress = value;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x000C1A72 File Offset: 0x000C0A72
		// (set) Token: 0x06002CC9 RID: 11465 RVA: 0x000C1A7A File Offset: 0x000C0A7A
		public int InterfaceIndex
		{
			get
			{
				return this.ifIndex;
			}
			set
			{
				if (value < 0 || value > 16777215)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				this.localAddress = null;
				this.ifIndex = value;
			}
		}

		// Token: 0x04002AC4 RID: 10948
		private IPAddress group;

		// Token: 0x04002AC5 RID: 10949
		private IPAddress localAddress;

		// Token: 0x04002AC6 RID: 10950
		private int ifIndex;
	}
}
