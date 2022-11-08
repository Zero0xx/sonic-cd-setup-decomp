using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200021D RID: 541
	internal struct StoreOperationUnpinDeployment
	{
		// Token: 0x060015B3 RID: 5555 RVA: 0x000373A6 File Offset: 0x000363A6
		public StoreOperationUnpinDeployment(IDefinitionAppId app, StoreApplicationReference reference)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUnpinDeployment));
			this.Flags = StoreOperationUnpinDeployment.OpFlags.Nothing;
			this.Application = app;
			this.Reference = reference.ToIntPtr();
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x000373D8 File Offset: 0x000363D8
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x040008B3 RID: 2227
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040008B4 RID: 2228
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUnpinDeployment.OpFlags Flags;

		// Token: 0x040008B5 RID: 2229
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x040008B6 RID: 2230
		public IntPtr Reference;

		// Token: 0x0200021E RID: 542
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040008B8 RID: 2232
			Nothing = 0
		}

		// Token: 0x0200021F RID: 543
		public enum Disposition
		{
			// Token: 0x040008BA RID: 2234
			Failed,
			// Token: 0x040008BB RID: 2235
			Unpinned
		}
	}
}
