using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000608 RID: 1544
	internal struct IPOptions
	{
		// Token: 0x06002FC6 RID: 12230 RVA: 0x000CF148 File Offset: 0x000CE148
		internal IPOptions(PingOptions options)
		{
			this.ttl = 128;
			this.tos = 0;
			this.flags = 0;
			this.optionsSize = 0;
			this.optionsData = IntPtr.Zero;
			if (options != null)
			{
				this.ttl = (byte)options.Ttl;
				if (options.DontFragment)
				{
					this.flags = 2;
				}
			}
		}

		// Token: 0x04002DA3 RID: 11683
		internal byte ttl;

		// Token: 0x04002DA4 RID: 11684
		internal byte tos;

		// Token: 0x04002DA5 RID: 11685
		internal byte flags;

		// Token: 0x04002DA6 RID: 11686
		internal byte optionsSize;

		// Token: 0x04002DA7 RID: 11687
		internal IntPtr optionsData;
	}
}
