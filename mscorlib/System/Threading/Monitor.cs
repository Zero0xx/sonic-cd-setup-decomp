using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x0200015D RID: 349
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public static class Monitor
	{
		// Token: 0x06001293 RID: 4755
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Enter(object obj);

		// Token: 0x06001294 RID: 4756
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ReliableEnter(object obj, ref bool tookLock);

		// Token: 0x06001295 RID: 4757
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Exit(object obj);

		// Token: 0x06001296 RID: 4758 RVA: 0x00033663 File Offset: 0x00032663
		public static bool TryEnter(object obj)
		{
			return Monitor.TryEnterTimeout(obj, 0);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0003366C File Offset: 0x0003266C
		public static bool TryEnter(object obj, int millisecondsTimeout)
		{
			return Monitor.TryEnterTimeout(obj, millisecondsTimeout);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00033678 File Offset: 0x00032678
		public static bool TryEnter(object obj, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return Monitor.TryEnterTimeout(obj, (int)num);
		}

		// Token: 0x06001299 RID: 4761
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TryEnterTimeout(object obj, int timeout);

		// Token: 0x0600129A RID: 4762
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ObjWait(bool exitContext, int millisecondsTimeout, object obj);

		// Token: 0x0600129B RID: 4763 RVA: 0x000336B9 File Offset: 0x000326B9
		public static bool Wait(object obj, int millisecondsTimeout, bool exitContext)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return Monitor.ObjWait(exitContext, millisecondsTimeout, obj);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x000336D4 File Offset: 0x000326D4
		public static bool Wait(object obj, TimeSpan timeout, bool exitContext)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return Monitor.Wait(obj, (int)num, exitContext);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00033716 File Offset: 0x00032716
		public static bool Wait(object obj, int millisecondsTimeout)
		{
			return Monitor.Wait(obj, millisecondsTimeout, false);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00033720 File Offset: 0x00032720
		public static bool Wait(object obj, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return Monitor.Wait(obj, (int)num, false);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00033762 File Offset: 0x00032762
		public static bool Wait(object obj)
		{
			return Monitor.Wait(obj, -1, false);
		}

		// Token: 0x060012A0 RID: 4768
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ObjPulse(object obj);

		// Token: 0x060012A1 RID: 4769 RVA: 0x0003376C File Offset: 0x0003276C
		public static void Pulse(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Monitor.ObjPulse(obj);
		}

		// Token: 0x060012A2 RID: 4770
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ObjPulseAll(object obj);

		// Token: 0x060012A3 RID: 4771 RVA: 0x00033782 File Offset: 0x00032782
		public static void PulseAll(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Monitor.ObjPulseAll(obj);
		}
	}
}
