using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200021A RID: 538
	internal struct StoreOperationPinDeployment
	{
		// Token: 0x060015B0 RID: 5552 RVA: 0x00037347 File Offset: 0x00036347
		public StoreOperationPinDeployment(IDefinitionAppId AppId, StoreApplicationReference Ref)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationPinDeployment));
			this.Flags = StoreOperationPinDeployment.OpFlags.NeverExpires;
			this.Application = AppId;
			this.Reference = Ref.ToIntPtr();
			this.ExpirationTime = 0L;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00037381 File Offset: 0x00036381
		public StoreOperationPinDeployment(IDefinitionAppId AppId, DateTime Expiry, StoreApplicationReference Ref)
		{
			this = new StoreOperationPinDeployment(AppId, Ref);
			this.Flags |= StoreOperationPinDeployment.OpFlags.NeverExpires;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00037399 File Offset: 0x00036399
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x040008A8 RID: 2216
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040008A9 RID: 2217
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationPinDeployment.OpFlags Flags;

		// Token: 0x040008AA RID: 2218
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x040008AB RID: 2219
		[MarshalAs(UnmanagedType.I8)]
		public long ExpirationTime;

		// Token: 0x040008AC RID: 2220
		public IntPtr Reference;

		// Token: 0x0200021B RID: 539
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040008AE RID: 2222
			Nothing = 0,
			// Token: 0x040008AF RID: 2223
			NeverExpires = 1
		}

		// Token: 0x0200021C RID: 540
		public enum Disposition
		{
			// Token: 0x040008B1 RID: 2225
			Failed,
			// Token: 0x040008B2 RID: 2226
			Pinned
		}
	}
}
