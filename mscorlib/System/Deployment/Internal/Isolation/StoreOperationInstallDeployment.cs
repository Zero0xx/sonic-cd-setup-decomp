using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000220 RID: 544
	internal struct StoreOperationInstallDeployment
	{
		// Token: 0x060015B5 RID: 5557 RVA: 0x000373E5 File Offset: 0x000363E5
		public StoreOperationInstallDeployment(IDefinitionAppId App, StoreApplicationReference reference)
		{
			this = new StoreOperationInstallDeployment(App, true, reference);
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x000373F0 File Offset: 0x000363F0
		public StoreOperationInstallDeployment(IDefinitionAppId App, bool UninstallOthers, StoreApplicationReference reference)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationInstallDeployment));
			this.Flags = StoreOperationInstallDeployment.OpFlags.Nothing;
			this.Application = App;
			if (UninstallOthers)
			{
				this.Flags |= StoreOperationInstallDeployment.OpFlags.UninstallOthers;
			}
			this.Reference = reference.ToIntPtr();
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0003743E File Offset: 0x0003643E
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x040008BC RID: 2236
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040008BD RID: 2237
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationInstallDeployment.OpFlags Flags;

		// Token: 0x040008BE RID: 2238
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x040008BF RID: 2239
		public IntPtr Reference;

		// Token: 0x02000221 RID: 545
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040008C1 RID: 2241
			Nothing = 0,
			// Token: 0x040008C2 RID: 2242
			UninstallOthers = 1
		}

		// Token: 0x02000222 RID: 546
		public enum Disposition
		{
			// Token: 0x040008C4 RID: 2244
			Failed,
			// Token: 0x040008C5 RID: 2245
			AlreadyInstalled,
			// Token: 0x040008C6 RID: 2246
			Installed
		}
	}
}
