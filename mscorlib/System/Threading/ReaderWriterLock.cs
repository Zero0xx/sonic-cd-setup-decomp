using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000167 RID: 359
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public sealed class ReaderWriterLock : CriticalFinalizerObject
	{
		// Token: 0x060012E3 RID: 4835 RVA: 0x000345EC File Offset: 0x000335EC
		public ReaderWriterLock()
		{
			this.PrivateInitialize();
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x000345FC File Offset: 0x000335FC
		~ReaderWriterLock()
		{
			this.PrivateDestruct();
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00034628 File Offset: 0x00033628
		public bool IsReaderLockHeld
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.PrivateGetIsReaderLockHeld();
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00034630 File Offset: 0x00033630
		public bool IsWriterLockHeld
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.PrivateGetIsWriterLockHeld();
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x00034638 File Offset: 0x00033638
		public int WriterSeqNum
		{
			get
			{
				return this.PrivateGetWriterSeqNum();
			}
		}

		// Token: 0x060012E8 RID: 4840
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AcquireReaderLockInternal(int millisecondsTimeout);

		// Token: 0x060012E9 RID: 4841 RVA: 0x00034640 File Offset: 0x00033640
		public void AcquireReaderLock(int millisecondsTimeout)
		{
			this.AcquireReaderLockInternal(millisecondsTimeout);
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0003464C File Offset: 0x0003364C
		public void AcquireReaderLock(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			this.AcquireReaderLockInternal((int)num);
		}

		// Token: 0x060012EB RID: 4843
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AcquireWriterLockInternal(int millisecondsTimeout);

		// Token: 0x060012EC RID: 4844 RVA: 0x0003468D File Offset: 0x0003368D
		public void AcquireWriterLock(int millisecondsTimeout)
		{
			this.AcquireWriterLockInternal(millisecondsTimeout);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00034698 File Offset: 0x00033698
		public void AcquireWriterLock(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			this.AcquireWriterLockInternal((int)num);
		}

		// Token: 0x060012EE RID: 4846
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ReleaseReaderLockInternal();

		// Token: 0x060012EF RID: 4847 RVA: 0x000346D9 File Offset: 0x000336D9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void ReleaseReaderLock()
		{
			this.ReleaseReaderLockInternal();
		}

		// Token: 0x060012F0 RID: 4848
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ReleaseWriterLockInternal();

		// Token: 0x060012F1 RID: 4849 RVA: 0x000346E1 File Offset: 0x000336E1
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void ReleaseWriterLock()
		{
			this.ReleaseWriterLockInternal();
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x000346EC File Offset: 0x000336EC
		public LockCookie UpgradeToWriterLock(int millisecondsTimeout)
		{
			LockCookie result = default(LockCookie);
			this.FCallUpgradeToWriterLock(ref result, millisecondsTimeout);
			return result;
		}

		// Token: 0x060012F3 RID: 4851
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void FCallUpgradeToWriterLock(ref LockCookie result, int millisecondsTimeout);

		// Token: 0x060012F4 RID: 4852 RVA: 0x0003470C File Offset: 0x0003370C
		public LockCookie UpgradeToWriterLock(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return this.UpgradeToWriterLock((int)num);
		}

		// Token: 0x060012F5 RID: 4853
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DowngradeFromWriterLockInternal(ref LockCookie lockCookie);

		// Token: 0x060012F6 RID: 4854 RVA: 0x0003474D File Offset: 0x0003374D
		public void DowngradeFromWriterLock(ref LockCookie lockCookie)
		{
			this.DowngradeFromWriterLockInternal(ref lockCookie);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00034758 File Offset: 0x00033758
		public LockCookie ReleaseLock()
		{
			LockCookie result = default(LockCookie);
			this.FCallReleaseLock(ref result);
			return result;
		}

		// Token: 0x060012F8 RID: 4856
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void FCallReleaseLock(ref LockCookie result);

		// Token: 0x060012F9 RID: 4857
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RestoreLockInternal(ref LockCookie lockCookie);

		// Token: 0x060012FA RID: 4858 RVA: 0x00034776 File Offset: 0x00033776
		public void RestoreLock(ref LockCookie lockCookie)
		{
			this.RestoreLockInternal(ref lockCookie);
		}

		// Token: 0x060012FB RID: 4859
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool PrivateGetIsReaderLockHeld();

		// Token: 0x060012FC RID: 4860
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool PrivateGetIsWriterLockHeld();

		// Token: 0x060012FD RID: 4861
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int PrivateGetWriterSeqNum();

		// Token: 0x060012FE RID: 4862
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool AnyWritersSince(int seqNum);

		// Token: 0x060012FF RID: 4863
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void PrivateInitialize();

		// Token: 0x06001300 RID: 4864
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void PrivateDestruct();

		// Token: 0x0400068E RID: 1678
		private IntPtr _hWriterEvent;

		// Token: 0x0400068F RID: 1679
		private IntPtr _hReaderEvent;

		// Token: 0x04000690 RID: 1680
		private IntPtr _hObjectHandle;

		// Token: 0x04000691 RID: 1681
		private int _dwState;

		// Token: 0x04000692 RID: 1682
		private int _dwULockID;

		// Token: 0x04000693 RID: 1683
		private int _dwLLockID;

		// Token: 0x04000694 RID: 1684
		private int _dwWriterID;

		// Token: 0x04000695 RID: 1685
		private int _dwWriterSeqNum;

		// Token: 0x04000696 RID: 1686
		private short _wWriterLevel;
	}
}
