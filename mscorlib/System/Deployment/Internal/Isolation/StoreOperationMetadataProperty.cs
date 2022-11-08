using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000226 RID: 550
	internal struct StoreOperationMetadataProperty
	{
		// Token: 0x060015BA RID: 5562 RVA: 0x0003748A File Offset: 0x0003648A
		public StoreOperationMetadataProperty(Guid PropertySet, string Name)
		{
			this = new StoreOperationMetadataProperty(PropertySet, Name, null);
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x00037495 File Offset: 0x00036495
		public StoreOperationMetadataProperty(Guid PropertySet, string Name, string Value)
		{
			this.GuidPropertySet = PropertySet;
			this.Name = Name;
			this.Value = Value;
			this.ValueSize = ((Value != null) ? new IntPtr((Value.Length + 1) * 2) : IntPtr.Zero);
		}

		// Token: 0x040008D1 RID: 2257
		public Guid GuidPropertySet;

		// Token: 0x040008D2 RID: 2258
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040008D3 RID: 2259
		[MarshalAs(UnmanagedType.SysUInt)]
		public IntPtr ValueSize;

		// Token: 0x040008D4 RID: 2260
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
