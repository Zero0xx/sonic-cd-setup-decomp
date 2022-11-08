using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000223 RID: 547
	internal struct StoreOperationUninstallDeployment
	{
		// Token: 0x060015B8 RID: 5560 RVA: 0x0003744B File Offset: 0x0003644B
		public StoreOperationUninstallDeployment(IDefinitionAppId appid, StoreApplicationReference AppRef)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUninstallDeployment));
			this.Flags = StoreOperationUninstallDeployment.OpFlags.Nothing;
			this.Application = appid;
			this.Reference = AppRef.ToIntPtr();
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0003747D File Offset: 0x0003647D
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x040008C7 RID: 2247
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040008C8 RID: 2248
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUninstallDeployment.OpFlags Flags;

		// Token: 0x040008C9 RID: 2249
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x040008CA RID: 2250
		public IntPtr Reference;

		// Token: 0x02000224 RID: 548
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040008CC RID: 2252
			Nothing = 0
		}

		// Token: 0x02000225 RID: 549
		public enum Disposition
		{
			// Token: 0x040008CE RID: 2254
			Failed,
			// Token: 0x040008CF RID: 2255
			DidNotExist,
			// Token: 0x040008D0 RID: 2256
			Uninstalled
		}
	}
}
