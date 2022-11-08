using System;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x02000346 RID: 838
	public abstract class ChannelBinding : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001A6D RID: 6765 RVA: 0x0005C509 File Offset: 0x0005B509
		protected ChannelBinding() : base(true)
		{
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001A6E RID: 6766
		public abstract int Size { get; }
	}
}
