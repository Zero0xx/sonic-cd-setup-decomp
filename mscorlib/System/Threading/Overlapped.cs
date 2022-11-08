using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000163 RID: 355
	[ComVisible(true)]
	public class Overlapped
	{
		// Token: 0x060012C1 RID: 4801 RVA: 0x000340C7 File Offset: 0x000330C7
		public Overlapped()
		{
			this.m_overlappedData = OverlappedDataCache.GetOverlappedData(this);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x000340DC File Offset: 0x000330DC
		public Overlapped(int offsetLo, int offsetHi, IntPtr hEvent, IAsyncResult ar)
		{
			this.m_overlappedData = OverlappedDataCache.GetOverlappedData(this);
			this.m_overlappedData.m_nativeOverlapped.OffsetLow = offsetLo;
			this.m_overlappedData.m_nativeOverlapped.OffsetHigh = offsetHi;
			this.m_overlappedData.UserHandle = hEvent;
			this.m_overlappedData.m_asyncResult = ar;
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00034136 File Offset: 0x00033136
		[Obsolete("This constructor is not 64-bit compatible.  Use the constructor that takes an IntPtr for the event handle.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public Overlapped(int offsetLo, int offsetHi, int hEvent, IAsyncResult ar) : this(offsetLo, offsetHi, new IntPtr(hEvent), ar)
		{
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060012C4 RID: 4804 RVA: 0x00034148 File Offset: 0x00033148
		// (set) Token: 0x060012C5 RID: 4805 RVA: 0x00034155 File Offset: 0x00033155
		public IAsyncResult AsyncResult
		{
			get
			{
				return this.m_overlappedData.m_asyncResult;
			}
			set
			{
				this.m_overlappedData.m_asyncResult = value;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x00034163 File Offset: 0x00033163
		// (set) Token: 0x060012C7 RID: 4807 RVA: 0x00034175 File Offset: 0x00033175
		public int OffsetLow
		{
			get
			{
				return this.m_overlappedData.m_nativeOverlapped.OffsetLow;
			}
			set
			{
				this.m_overlappedData.m_nativeOverlapped.OffsetLow = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x00034188 File Offset: 0x00033188
		// (set) Token: 0x060012C9 RID: 4809 RVA: 0x0003419A File Offset: 0x0003319A
		public int OffsetHigh
		{
			get
			{
				return this.m_overlappedData.m_nativeOverlapped.OffsetHigh;
			}
			set
			{
				this.m_overlappedData.m_nativeOverlapped.OffsetHigh = value;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x000341B0 File Offset: 0x000331B0
		// (set) Token: 0x060012CB RID: 4811 RVA: 0x000341D0 File Offset: 0x000331D0
		[Obsolete("This property is not 64-bit compatible.  Use EventHandleIntPtr instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public int EventHandle
		{
			get
			{
				return this.m_overlappedData.UserHandle.ToInt32();
			}
			set
			{
				this.m_overlappedData.UserHandle = new IntPtr(value);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x000341E3 File Offset: 0x000331E3
		// (set) Token: 0x060012CD RID: 4813 RVA: 0x000341F0 File Offset: 0x000331F0
		[ComVisible(false)]
		public IntPtr EventHandleIntPtr
		{
			get
			{
				return this.m_overlappedData.UserHandle;
			}
			set
			{
				this.m_overlappedData.UserHandle = value;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x000341FE File Offset: 0x000331FE
		internal _IOCompletionCallback iocbHelper
		{
			get
			{
				return this.m_overlappedData.m_iocbHelper;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0003420B File Offset: 0x0003320B
		internal IOCompletionCallback UserCallback
		{
			get
			{
				return this.m_overlappedData.m_iocb;
			}
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00034218 File Offset: 0x00033218
		[Obsolete("This method is not safe.  Use Pack (iocb, userData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[CLSCompliant(false)]
		public unsafe NativeOverlapped* Pack(IOCompletionCallback iocb)
		{
			return this.Pack(iocb, null);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00034222 File Offset: 0x00033222
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe NativeOverlapped* Pack(IOCompletionCallback iocb, object userData)
		{
			return this.m_overlappedData.Pack(iocb, userData);
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00034231 File Offset: 0x00033231
		[Obsolete("This method is not safe.  Use UnsafePack (iocb, userData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[CLSCompliant(false)]
		[SecurityPermission(SecurityAction.LinkDemand, ControlEvidence = true, ControlPolicy = true)]
		public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback iocb)
		{
			return this.UnsafePack(iocb, null);
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0003423B File Offset: 0x0003323B
		[CLSCompliant(false)]
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, ControlEvidence = true, ControlPolicy = true)]
		public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback iocb, object userData)
		{
			return this.m_overlappedData.UnsafePack(iocb, userData);
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0003424C File Offset: 0x0003324C
		[CLSCompliant(false)]
		public unsafe static Overlapped Unpack(NativeOverlapped* nativeOverlappedPtr)
		{
			if (nativeOverlappedPtr == null)
			{
				throw new ArgumentNullException("nativeOverlappedPtr");
			}
			return OverlappedData.GetOverlappedFromNative(nativeOverlappedPtr).m_overlapped;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00034278 File Offset: 0x00033278
		[CLSCompliant(false)]
		public unsafe static void Free(NativeOverlapped* nativeOverlappedPtr)
		{
			if (nativeOverlappedPtr == null)
			{
				throw new ArgumentNullException("nativeOverlappedPtr");
			}
			Overlapped overlapped = OverlappedData.GetOverlappedFromNative(nativeOverlappedPtr).m_overlapped;
			OverlappedData.FreeNativeOverlapped(nativeOverlappedPtr);
			OverlappedData overlappedData = overlapped.m_overlappedData;
			overlapped.m_overlappedData = null;
			OverlappedDataCache.CacheOverlappedData(overlappedData);
		}

		// Token: 0x04000680 RID: 1664
		private OverlappedData m_overlappedData;
	}
}
