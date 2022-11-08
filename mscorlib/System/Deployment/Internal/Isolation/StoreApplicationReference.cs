using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000218 RID: 536
	internal struct StoreApplicationReference
	{
		// Token: 0x060015AD RID: 5549 RVA: 0x000372B6 File Offset: 0x000362B6
		public StoreApplicationReference(Guid RefScheme, string Id, string NcData)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreApplicationReference));
			this.Flags = StoreApplicationReference.RefFlags.Nothing;
			this.GuidScheme = RefScheme;
			this.Identifier = Id;
			this.NonCanonicalData = NcData;
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x000372EC File Offset: 0x000362EC
		public IntPtr ToIntPtr()
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
			Marshal.StructureToPtr(this, intPtr, false);
			return intPtr;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00037322 File Offset: 0x00036322
		public static void Destroy(IntPtr ip)
		{
			if (ip != IntPtr.Zero)
			{
				Marshal.DestroyStructure(ip, typeof(StoreApplicationReference));
				Marshal.FreeCoTaskMem(ip);
			}
		}

		// Token: 0x040008A1 RID: 2209
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040008A2 RID: 2210
		[MarshalAs(UnmanagedType.U4)]
		public StoreApplicationReference.RefFlags Flags;

		// Token: 0x040008A3 RID: 2211
		public Guid GuidScheme;

		// Token: 0x040008A4 RID: 2212
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Identifier;

		// Token: 0x040008A5 RID: 2213
		[MarshalAs(UnmanagedType.LPWStr)]
		public string NonCanonicalData;

		// Token: 0x02000219 RID: 537
		[Flags]
		public enum RefFlags
		{
			// Token: 0x040008A7 RID: 2215
			Nothing = 0
		}
	}
}
