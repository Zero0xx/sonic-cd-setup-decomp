using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000162 RID: 354
	internal sealed class OverlappedData : CriticalFinalizerObject
	{
		// Token: 0x060012B6 RID: 4790 RVA: 0x00033EC4 File Offset: 0x00032EC4
		internal OverlappedData(OverlappedDataCacheLine cacheLine)
		{
			this.m_cacheLine = cacheLine;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00033ED4 File Offset: 0x00032ED4
		~OverlappedData()
		{
			if (this.m_cacheLine != null && !this.m_cacheLine.Removed && !Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
			{
				OverlappedDataCache.CacheOverlappedData(this);
				GC.ReRegisterForFinalize(this);
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00033F30 File Offset: 0x00032F30
		internal void ReInitialize()
		{
			this.m_asyncResult = null;
			this.m_iocb = null;
			this.m_iocbHelper = null;
			this.m_overlapped = null;
			this.m_userObject = null;
			this.m_pinSelf = (IntPtr)0;
			this.m_userObjectInternal = (IntPtr)0;
			this.m_AppDomainId = 0;
			this.m_nativeOverlapped.EventHandle = (IntPtr)0;
			this.m_isArray = 0;
			this.m_nativeOverlapped.InternalHigh = (IntPtr)0;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00033FA8 File Offset: 0x00032FA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal unsafe NativeOverlapped* Pack(IOCompletionCallback iocb, object userData)
		{
			if (!this.m_pinSelf.IsNull())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_Overlapped_Pack"));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (iocb != null)
			{
				this.m_iocbHelper = new _IOCompletionCallback(iocb, ref stackCrawlMark);
				this.m_iocb = iocb;
			}
			else
			{
				this.m_iocbHelper = null;
				this.m_iocb = null;
			}
			this.m_userObject = userData;
			if (this.m_userObject != null)
			{
				if (this.m_userObject.GetType() == typeof(object[]))
				{
					this.m_isArray = 1;
				}
				else
				{
					this.m_isArray = 0;
				}
			}
			return this.AllocateNativeOverlapped();
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00034038 File Offset: 0x00033038
		[SecurityPermission(SecurityAction.LinkDemand, ControlEvidence = true, ControlPolicy = true)]
		internal unsafe NativeOverlapped* UnsafePack(IOCompletionCallback iocb, object userData)
		{
			if (!this.m_pinSelf.IsNull())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_Overlapped_Pack"));
			}
			this.m_userObject = userData;
			if (this.m_userObject != null)
			{
				if (this.m_userObject.GetType() == typeof(object[]))
				{
					this.m_isArray = 1;
				}
				else
				{
					this.m_isArray = 0;
				}
			}
			this.m_iocb = iocb;
			this.m_iocbHelper = null;
			return this.AllocateNativeOverlapped();
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x000340AC File Offset: 0x000330AC
		// (set) Token: 0x060012BC RID: 4796 RVA: 0x000340B9 File Offset: 0x000330B9
		[ComVisible(false)]
		internal IntPtr UserHandle
		{
			get
			{
				return this.m_nativeOverlapped.EventHandle;
			}
			set
			{
				this.m_nativeOverlapped.EventHandle = value;
			}
		}

		// Token: 0x060012BD RID: 4797
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern NativeOverlapped* AllocateNativeOverlapped();

		// Token: 0x060012BE RID: 4798
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void FreeNativeOverlapped(NativeOverlapped* nativeOverlappedPtr);

		// Token: 0x060012BF RID: 4799
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern OverlappedData GetOverlappedFromNative(NativeOverlapped* nativeOverlappedPtr);

		// Token: 0x060012C0 RID: 4800
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void CheckVMForIOPacket(out NativeOverlapped* pOVERLAP, out uint errorCode, out uint numBytes);

		// Token: 0x04000673 RID: 1651
		internal IAsyncResult m_asyncResult;

		// Token: 0x04000674 RID: 1652
		internal IOCompletionCallback m_iocb;

		// Token: 0x04000675 RID: 1653
		internal _IOCompletionCallback m_iocbHelper;

		// Token: 0x04000676 RID: 1654
		internal Overlapped m_overlapped;

		// Token: 0x04000677 RID: 1655
		private object m_userObject;

		// Token: 0x04000678 RID: 1656
		internal OverlappedDataCacheLine m_cacheLine;

		// Token: 0x04000679 RID: 1657
		private IntPtr m_pinSelf;

		// Token: 0x0400067A RID: 1658
		private IntPtr m_userObjectInternal;

		// Token: 0x0400067B RID: 1659
		private int m_AppDomainId;

		// Token: 0x0400067C RID: 1660
		internal short m_slot;

		// Token: 0x0400067D RID: 1661
		private byte m_isArray;

		// Token: 0x0400067E RID: 1662
		private byte m_toBeCleaned;

		// Token: 0x0400067F RID: 1663
		internal NativeOverlapped m_nativeOverlapped;
	}
}
