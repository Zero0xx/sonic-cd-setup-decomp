using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200066F RID: 1647
	internal class CachedItemHdcInfo : IDisposable
	{
		// Token: 0x0600568C RID: 22156 RVA: 0x0013AEB1 File Offset: 0x00139EB1
		internal CachedItemHdcInfo()
		{
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x0013AEDC File Offset: 0x00139EDC
		~CachedItemHdcInfo()
		{
			this.Dispose();
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x0013AF08 File Offset: 0x00139F08
		public HandleRef GetCachedItemDC(HandleRef toolStripHDC, Size bitmapSize)
		{
			if (this.cachedHDCSize.Width < bitmapSize.Width || this.cachedHDCSize.Height < bitmapSize.Height)
			{
				if (this.cachedItemHDC.Handle == IntPtr.Zero)
				{
					IntPtr handle = UnsafeNativeMethods.CreateCompatibleDC(toolStripHDC);
					this.cachedItemHDC = new HandleRef(this, handle);
				}
				this.cachedItemBitmap = new HandleRef(this, SafeNativeMethods.CreateCompatibleBitmap(toolStripHDC, bitmapSize.Width, bitmapSize.Height));
				IntPtr intPtr = SafeNativeMethods.SelectObject(this.cachedItemHDC, this.cachedItemBitmap);
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.ExternalDeleteObject(new HandleRef(null, intPtr));
					intPtr = IntPtr.Zero;
				}
				this.cachedHDCSize = bitmapSize;
			}
			return this.cachedItemHDC;
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x0013AFCC File Offset: 0x00139FCC
		private void DeleteCachedItemHDC()
		{
			if (this.cachedItemHDC.Handle != IntPtr.Zero)
			{
				if (this.cachedItemBitmap.Handle != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(this.cachedItemBitmap);
					this.cachedItemBitmap = NativeMethods.NullHandleRef;
				}
				UnsafeNativeMethods.DeleteCompatibleDC(this.cachedItemHDC);
			}
			this.cachedItemHDC = NativeMethods.NullHandleRef;
			this.cachedItemBitmap = NativeMethods.NullHandleRef;
			this.cachedHDCSize = Size.Empty;
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x0013B04B File Offset: 0x0013A04B
		public void Dispose()
		{
			this.DeleteCachedItemHDC();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400376C RID: 14188
		private HandleRef cachedItemHDC = NativeMethods.NullHandleRef;

		// Token: 0x0400376D RID: 14189
		private Size cachedHDCSize = Size.Empty;

		// Token: 0x0400376E RID: 14190
		private HandleRef cachedItemBitmap = NativeMethods.NullHandleRef;
	}
}
