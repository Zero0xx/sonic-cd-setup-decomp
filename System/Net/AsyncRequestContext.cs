using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003C6 RID: 966
	internal class AsyncRequestContext : RequestContextBase
	{
		// Token: 0x06001E67 RID: 7783 RVA: 0x0007453E File Offset: 0x0007353E
		internal AsyncRequestContext(ListenerAsyncResult result)
		{
			this.m_Result = result;
			base.BaseConstruction(this.Allocate(0U));
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0007455C File Offset: 0x0007355C
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* Allocate(uint size)
		{
			uint num = (size != 0U) ? size : ((base.RequestBuffer == null) ? 4096U : base.Size);
			if (this.m_NativeOverlapped != null && (ulong)num != (ulong)((long)base.RequestBuffer.Length))
			{
				NativeOverlapped* nativeOverlapped = this.m_NativeOverlapped;
				this.m_NativeOverlapped = null;
				Overlapped.Free(nativeOverlapped);
			}
			if (this.m_NativeOverlapped == null)
			{
				base.SetBuffer(checked((int)num));
				this.m_NativeOverlapped = new Overlapped
				{
					AsyncResult = this.m_Result
				}.Pack(ListenerAsyncResult.IOCallback, base.RequestBuffer);
				return (UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(base.RequestBuffer, 0));
			}
			return base.RequestBlob;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x00074604 File Offset: 0x00073604
		internal unsafe void Reset(ulong requestId, uint size)
		{
			base.SetBlob(this.Allocate(size));
			base.RequestBlob->RequestId = requestId;
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x00074620 File Offset: 0x00073620
		protected unsafe override void OnReleasePins()
		{
			if (this.m_NativeOverlapped != null)
			{
				NativeOverlapped* nativeOverlapped = this.m_NativeOverlapped;
				this.m_NativeOverlapped = null;
				Overlapped.Free(nativeOverlapped);
			}
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0007464C File Offset: 0x0007364C
		protected override void Dispose(bool disposing)
		{
			if (this.m_NativeOverlapped != null && (!NclUtilities.HasShutdownStarted || disposing))
			{
				Overlapped.Free(this.m_NativeOverlapped);
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x00074674 File Offset: 0x00073674
		internal unsafe NativeOverlapped* NativeOverlapped
		{
			get
			{
				return this.m_NativeOverlapped;
			}
		}

		// Token: 0x04001E46 RID: 7750
		private unsafe NativeOverlapped* m_NativeOverlapped;

		// Token: 0x04001E47 RID: 7751
		private ListenerAsyncResult m_Result;
	}
}
