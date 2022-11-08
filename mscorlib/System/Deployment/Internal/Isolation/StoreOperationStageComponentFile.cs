using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000215 RID: 533
	internal struct StoreOperationStageComponentFile
	{
		// Token: 0x060015AA RID: 5546 RVA: 0x0003726D File Offset: 0x0003626D
		public StoreOperationStageComponentFile(IDefinitionAppId App, string CompRelPath, string SrcFile)
		{
			this = new StoreOperationStageComponentFile(App, null, CompRelPath, SrcFile);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00037279 File Offset: 0x00036279
		public StoreOperationStageComponentFile(IDefinitionAppId App, IDefinitionIdentity Component, string CompRelPath, string SrcFile)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponentFile));
			this.Flags = StoreOperationStageComponentFile.OpFlags.Nothing;
			this.Application = App;
			this.Component = Component;
			this.ComponentRelativePath = CompRelPath;
			this.SourceFilePath = SrcFile;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x000372B4 File Offset: 0x000362B4
		public void Destroy()
		{
		}

		// Token: 0x04000894 RID: 2196
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000895 RID: 2197
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponentFile.OpFlags Flags;

		// Token: 0x04000896 RID: 2198
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000897 RID: 2199
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x04000898 RID: 2200
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ComponentRelativePath;

		// Token: 0x04000899 RID: 2201
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceFilePath;

		// Token: 0x02000216 RID: 534
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400089B RID: 2203
			Nothing = 0
		}

		// Token: 0x02000217 RID: 535
		public enum Disposition
		{
			// Token: 0x0400089D RID: 2205
			Failed,
			// Token: 0x0400089E RID: 2206
			Installed,
			// Token: 0x0400089F RID: 2207
			Refreshed,
			// Token: 0x040008A0 RID: 2208
			AlreadyInstalled
		}
	}
}
