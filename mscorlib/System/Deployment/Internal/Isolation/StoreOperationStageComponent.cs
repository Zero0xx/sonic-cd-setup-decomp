using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000212 RID: 530
	internal struct StoreOperationStageComponent
	{
		// Token: 0x060015A7 RID: 5543 RVA: 0x0003722D File Offset: 0x0003622D
		public void Destroy()
		{
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0003722F File Offset: 0x0003622F
		public StoreOperationStageComponent(IDefinitionAppId app, string Manifest)
		{
			this = new StoreOperationStageComponent(app, null, Manifest);
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0003723A File Offset: 0x0003623A
		public StoreOperationStageComponent(IDefinitionAppId app, IDefinitionIdentity comp, string Manifest)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponent));
			this.Flags = StoreOperationStageComponent.OpFlags.Nothing;
			this.Application = app;
			this.Component = comp;
			this.ManifestPath = Manifest;
		}

		// Token: 0x04000888 RID: 2184
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000889 RID: 2185
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponent.OpFlags Flags;

		// Token: 0x0400088A RID: 2186
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x0400088B RID: 2187
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x0400088C RID: 2188
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x02000213 RID: 531
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400088E RID: 2190
			Nothing = 0
		}

		// Token: 0x02000214 RID: 532
		public enum Disposition
		{
			// Token: 0x04000890 RID: 2192
			Failed,
			// Token: 0x04000891 RID: 2193
			Installed,
			// Token: 0x04000892 RID: 2194
			Refreshed,
			// Token: 0x04000893 RID: 2195
			AlreadyInstalled
		}
	}
}
