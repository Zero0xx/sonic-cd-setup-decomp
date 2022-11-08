using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200022A RID: 554
	internal struct StoreOperationSetCanonicalizationContext
	{
		// Token: 0x060015C1 RID: 5569 RVA: 0x00037705 File Offset: 0x00036705
		public StoreOperationSetCanonicalizationContext(string Bases, string Exports)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationSetCanonicalizationContext));
			this.Flags = StoreOperationSetCanonicalizationContext.OpFlags.Nothing;
			this.BaseAddressFilePath = Bases;
			this.ExportsFilePath = Exports;
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x00037731 File Offset: 0x00036731
		public void Destroy()
		{
		}

		// Token: 0x040008E2 RID: 2274
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040008E3 RID: 2275
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationSetCanonicalizationContext.OpFlags Flags;

		// Token: 0x040008E4 RID: 2276
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BaseAddressFilePath;

		// Token: 0x040008E5 RID: 2277
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ExportsFilePath;

		// Token: 0x0200022B RID: 555
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040008E7 RID: 2279
			Nothing = 0
		}
	}
}
