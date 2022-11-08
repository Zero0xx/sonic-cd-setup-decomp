using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200022C RID: 556
	internal struct StoreOperationScavenge
	{
		// Token: 0x060015C3 RID: 5571 RVA: 0x00037734 File Offset: 0x00036734
		public StoreOperationScavenge(bool Light, ulong SizeLimit, ulong RunLimit, uint ComponentLimit)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationScavenge));
			this.Flags = StoreOperationScavenge.OpFlags.Nothing;
			if (Light)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.Light;
			}
			this.SizeReclaimationLimit = SizeLimit;
			if (SizeLimit != 0UL)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitSize;
			}
			this.RuntimeLimit = RunLimit;
			if (RunLimit != 0UL)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitTime;
			}
			this.ComponentCountLimit = ComponentLimit;
			if (ComponentLimit != 0U)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitCount;
			}
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x000377BC File Offset: 0x000367BC
		public StoreOperationScavenge(bool Light)
		{
			this = new StoreOperationScavenge(Light, 0UL, 0UL, 0U);
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x000377CA File Offset: 0x000367CA
		public void Destroy()
		{
		}

		// Token: 0x040008E8 RID: 2280
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040008E9 RID: 2281
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationScavenge.OpFlags Flags;

		// Token: 0x040008EA RID: 2282
		[MarshalAs(UnmanagedType.U8)]
		public ulong SizeReclaimationLimit;

		// Token: 0x040008EB RID: 2283
		[MarshalAs(UnmanagedType.U8)]
		public ulong RuntimeLimit;

		// Token: 0x040008EC RID: 2284
		[MarshalAs(UnmanagedType.U4)]
		public uint ComponentCountLimit;

		// Token: 0x0200022D RID: 557
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040008EE RID: 2286
			Nothing = 0,
			// Token: 0x040008EF RID: 2287
			Light = 1,
			// Token: 0x040008F0 RID: 2288
			LimitSize = 2,
			// Token: 0x040008F1 RID: 2289
			LimitTime = 4,
			// Token: 0x040008F2 RID: 2290
			LimitCount = 8
		}
	}
}
