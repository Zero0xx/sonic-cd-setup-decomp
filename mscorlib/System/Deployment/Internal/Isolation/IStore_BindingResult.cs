using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200023E RID: 574
	internal struct IStore_BindingResult
	{
		// Token: 0x04000923 RID: 2339
		[MarshalAs(UnmanagedType.U4)]
		public uint Flags;

		// Token: 0x04000924 RID: 2340
		[MarshalAs(UnmanagedType.U4)]
		public uint Disposition;

		// Token: 0x04000925 RID: 2341
		public IStore_BindingResult_BoundVersion Component;

		// Token: 0x04000926 RID: 2342
		public Guid CacheCoherencyGuid;

		// Token: 0x04000927 RID: 2343
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Reserved;
	}
}
