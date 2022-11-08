using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200023D RID: 573
	internal struct IStore_BindingResult_BoundVersion
	{
		// Token: 0x0400091F RID: 2335
		[MarshalAs(UnmanagedType.U2)]
		public ushort Revision;

		// Token: 0x04000920 RID: 2336
		[MarshalAs(UnmanagedType.U2)]
		public ushort Build;

		// Token: 0x04000921 RID: 2337
		[MarshalAs(UnmanagedType.U2)]
		public ushort Minor;

		// Token: 0x04000922 RID: 2338
		[MarshalAs(UnmanagedType.U2)]
		public ushort Major;
	}
}
