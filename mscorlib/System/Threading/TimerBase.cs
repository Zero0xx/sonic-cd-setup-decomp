using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000182 RID: 386
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	internal sealed class TimerBase : CriticalFinalizerObject, IDisposable
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x00036030 File Offset: 0x00035030
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
							this.DeleteTimerNative(null);
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

		// Token: 0x0600140F RID: 5135 RVA: 0x00036090 File Offset: 0x00035090
		internal void AddTimer(TimerCallback callback, object state, uint dueTime, uint period, ref StackCrawlMark stackMark)
		{
			if (callback != null)
			{
				_TimerCallback timerCallback = new _TimerCallback(callback, state, ref stackMark);
				state = timerCallback;
				this.AddTimerNative(state, dueTime, period, ref stackMark);
				this.timerDeleted = 0;
				return;
			}
			throw new ArgumentNullException("TimerCallback");
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x000360CC File Offset: 0x000350CC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool ChangeTimer(uint dueTime, uint period)
		{
			bool result = false;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag = true;
						try
						{
							if (this.timerDeleted != 0)
							{
								throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
							}
							result = this.ChangeTimerNative(dueTime, period);
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
			return result;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00036148 File Offset: 0x00035148
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool Dispose(WaitHandle notifyObject)
		{
			bool result = false;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag = true;
						try
						{
							result = this.DeleteTimerNative(notifyObject.SafeWaitHandle);
						}
						finally
						{
							this.m_lock = 0;
						}
					}
					Thread.SpinWait(1);
				}
				while (!flag);
				GC.SuppressFinalize(this);
			}
			return result;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000361B8 File Offset: 0x000351B8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Dispose()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag = true;
						try
						{
							this.DeleteTimerNative(null);
						}
						finally
						{
							this.m_lock = 0;
						}
					}
					Thread.SpinWait(1);
				}
				while (!flag);
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06001413 RID: 5139
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddTimerNative(object state, uint dueTime, uint period, ref StackCrawlMark stackMark);

		// Token: 0x06001414 RID: 5140
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool ChangeTimerNative(uint dueTime, uint period);

		// Token: 0x06001415 RID: 5141
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool DeleteTimerNative(SafeHandle notifyObject);

		// Token: 0x040006DE RID: 1758
		private IntPtr timerHandle;

		// Token: 0x040006DF RID: 1759
		private IntPtr delegateInfo;

		// Token: 0x040006E0 RID: 1760
		private int timerDeleted;

		// Token: 0x040006E1 RID: 1761
		private int m_lock;
	}
}
