using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000537 RID: 1335
	internal sealed class Semaphore : WaitHandle
	{
		// Token: 0x060028D4 RID: 10452 RVA: 0x000A9B10 File Offset: 0x000A8B10
		internal Semaphore(int initialCount, int maxCount)
		{
			lock (this)
			{
				this.Handle = UnsafeNclNativeMethods.CreateSemaphore(IntPtr.Zero, initialCount, maxCount, IntPtr.Zero);
			}
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000A9B5C File Offset: 0x000A8B5C
		internal bool ReleaseSemaphore()
		{
			return UnsafeNclNativeMethods.ReleaseSemaphore(this.Handle, 1, IntPtr.Zero);
		}
	}
}
