using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Threading
{
	// Token: 0x02000171 RID: 369
	internal sealed class RegisteredWaitHandleSafe : CriticalFinalizerObject
	{
		// Token: 0x060013AF RID: 5039 RVA: 0x000356E8 File Offset: 0x000346E8
		internal RegisteredWaitHandleSafe()
		{
			this.registeredWaitHandle = RegisteredWaitHandleSafe.InvalidHandle;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x000356FB File Offset: 0x000346FB
		internal IntPtr GetHandle()
		{
			return this.registeredWaitHandle;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00035703 File Offset: 0x00034703
		internal void SetHandle(IntPtr handle)
		{
			this.registeredWaitHandle = handle;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0003570C File Offset: 0x0003470C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal void SetWaitObject(WaitHandle waitObject)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.m_internalWaitObject = waitObject;
				if (waitObject != null)
				{
					this.m_internalWaitObject.SafeWaitHandle.DangerousAddRef(ref this.bReleaseNeeded);
				}
			}
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00035754 File Offset: 0x00034754
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool Unregister(WaitHandle waitObject)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				bool flag2 = false;
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag2 = true;
						try
						{
							if (this.ValidHandle())
							{
								flag = RegisteredWaitHandleSafe.UnregisterWaitNative(this.GetHandle(), (waitObject == null) ? null : waitObject.SafeWaitHandle);
								if (flag)
								{
									if (this.bReleaseNeeded)
									{
										this.m_internalWaitObject.SafeWaitHandle.DangerousRelease();
										this.bReleaseNeeded = false;
									}
									this.SetHandle(RegisteredWaitHandleSafe.InvalidHandle);
									this.m_internalWaitObject = null;
									GC.SuppressFinalize(this);
								}
							}
						}
						finally
						{
							this.m_lock = 0;
						}
					}
					Thread.SpinWait(1);
				}
				while (!flag2);
			}
			return flag;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0003580C File Offset: 0x0003480C
		private bool ValidHandle()
		{
			return this.registeredWaitHandle != RegisteredWaitHandleSafe.InvalidHandle && this.registeredWaitHandle != IntPtr.Zero;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00035834 File Offset: 0x00034834
		protected override void Finalize()
		{
			try
			{
				bool flag = false;
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag = true;
						try
						{
							if (this.ValidHandle())
							{
								RegisteredWaitHandleSafe.WaitHandleCleanupNative(this.registeredWaitHandle);
								if (this.bReleaseNeeded)
								{
									this.m_internalWaitObject.SafeWaitHandle.DangerousRelease();
									this.bReleaseNeeded = false;
								}
								this.SetHandle(RegisteredWaitHandleSafe.InvalidHandle);
								this.m_internalWaitObject = null;
							}
						}
						finally
						{
							this.m_lock = 0;
						}
					}
					Thread.SpinWait(1);
				}
				while (!flag);
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x060013B6 RID: 5046
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WaitHandleCleanupNative(IntPtr handle);

		// Token: 0x060013B7 RID: 5047
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool UnregisterWaitNative(IntPtr handle, SafeHandle waitObject);

		// Token: 0x040006B8 RID: 1720
		private static readonly IntPtr InvalidHandle = Win32Native.INVALID_HANDLE_VALUE;

		// Token: 0x040006B9 RID: 1721
		private IntPtr registeredWaitHandle;

		// Token: 0x040006BA RID: 1722
		private WaitHandle m_internalWaitObject;

		// Token: 0x040006BB RID: 1723
		private bool bReleaseNeeded;

		// Token: 0x040006BC RID: 1724
		private int m_lock;
	}
}
