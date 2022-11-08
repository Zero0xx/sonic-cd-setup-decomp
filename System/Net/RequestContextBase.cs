using System;

namespace System.Net
{
	// Token: 0x020003C5 RID: 965
	internal abstract class RequestContextBase : IDisposable
	{
		// Token: 0x06001E58 RID: 7768 RVA: 0x0007442E File Offset: 0x0007342E
		protected unsafe void BaseConstruction(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* requestBlob)
		{
			if (requestBlob == null)
			{
				GC.SuppressFinalize(this);
				return;
			}
			this.m_MemoryBlob = requestBlob;
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x00074443 File Offset: 0x00073443
		internal void ReleasePins()
		{
			this.m_OriginalBlobAddress = this.m_MemoryBlob;
			this.UnsetBlob();
			this.OnReleasePins();
		}

		// Token: 0x06001E5A RID: 7770
		protected abstract void OnReleasePins();

		// Token: 0x06001E5B RID: 7771 RVA: 0x0007445D File Offset: 0x0007345D
		public void Close()
		{
			this.Dispose();
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00074465 File Offset: 0x00073465
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0007446E File Offset: 0x0007346E
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x00074470 File Offset: 0x00073470
		~RequestContextBase()
		{
			this.Dispose(false);
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x000744A0 File Offset: 0x000734A0
		internal unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* RequestBlob
		{
			get
			{
				return this.m_MemoryBlob;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001E60 RID: 7776 RVA: 0x000744A8 File Offset: 0x000734A8
		internal byte[] RequestBuffer
		{
			get
			{
				return this.m_BackingBuffer;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x000744B0 File Offset: 0x000734B0
		internal uint Size
		{
			get
			{
				return (uint)this.m_BackingBuffer.Length;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x000744BC File Offset: 0x000734BC
		internal unsafe IntPtr OriginalBlobAddress
		{
			get
			{
				UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* memoryBlob = this.m_MemoryBlob;
				return (IntPtr)((void*)((memoryBlob == null) ? this.m_OriginalBlobAddress : memoryBlob));
			}
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x000744E3 File Offset: 0x000734E3
		protected unsafe void SetBlob(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* requestBlob)
		{
			if (requestBlob == null)
			{
				this.UnsetBlob();
				return;
			}
			if (this.m_MemoryBlob == null)
			{
				GC.ReRegisterForFinalize(this);
			}
			this.m_MemoryBlob = requestBlob;
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x00074508 File Offset: 0x00073508
		protected void UnsetBlob()
		{
			if (this.m_MemoryBlob != null)
			{
				GC.SuppressFinalize(this);
			}
			this.m_MemoryBlob = null;
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x00074522 File Offset: 0x00073522
		protected void SetBuffer(int size)
		{
			this.m_BackingBuffer = ((size == 0) ? null : new byte[size]);
		}

		// Token: 0x04001E43 RID: 7747
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* m_MemoryBlob;

		// Token: 0x04001E44 RID: 7748
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* m_OriginalBlobAddress;

		// Token: 0x04001E45 RID: 7749
		private byte[] m_BackingBuffer;
	}
}
