using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020003C7 RID: 967
	internal class SyncRequestContext : RequestContextBase
	{
		// Token: 0x06001E6D RID: 7789 RVA: 0x0007467C File Offset: 0x0007367C
		internal SyncRequestContext(int size)
		{
			base.BaseConstruction(this.Allocate(size));
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x00074694 File Offset: 0x00073694
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* Allocate(int size)
		{
			if (this.m_PinnedHandle.IsAllocated)
			{
				if (base.RequestBuffer.Length == size)
				{
					return base.RequestBlob;
				}
				this.m_PinnedHandle.Free();
			}
			base.SetBuffer(size);
			if (base.RequestBuffer == null)
			{
				return null;
			}
			this.m_PinnedHandle = GCHandle.Alloc(base.RequestBuffer, GCHandleType.Pinned);
			return (UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(base.RequestBuffer, 0));
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x00074700 File Offset: 0x00073700
		internal void Reset(int size)
		{
			base.SetBlob(this.Allocate(size));
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0007470F File Offset: 0x0007370F
		protected override void OnReleasePins()
		{
			if (this.m_PinnedHandle.IsAllocated)
			{
				this.m_PinnedHandle.Free();
			}
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x00074729 File Offset: 0x00073729
		protected override void Dispose(bool disposing)
		{
			if (this.m_PinnedHandle.IsAllocated && (!NclUtilities.HasShutdownStarted || disposing))
			{
				this.m_PinnedHandle.Free();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04001E48 RID: 7752
		private GCHandle m_PinnedHandle;
	}
}
