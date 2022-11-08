using System;
using System.IO;

namespace System.Net
{
	// Token: 0x020004CF RID: 1231
	internal sealed class SyncMemoryStream : MemoryStream
	{
		// Token: 0x0600264F RID: 9807 RVA: 0x0009BF94 File Offset: 0x0009AF94
		internal SyncMemoryStream(byte[] bytes) : base(bytes, false)
		{
			this.m_ReadTimeout = (this.m_WriteTimeout = -1);
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x0009BFBC File Offset: 0x0009AFBC
		internal SyncMemoryStream(int initialCapacity) : base(initialCapacity)
		{
			this.m_ReadTimeout = (this.m_WriteTimeout = -1);
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x0009BFE0 File Offset: 0x0009AFE0
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			int num = this.Read(buffer, offset, count);
			return new LazyAsyncResult(null, state, callback, num);
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x0009C008 File Offset: 0x0009B008
		public override int EndRead(IAsyncResult asyncResult)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
			return (int)lazyAsyncResult.InternalWaitForCompletion();
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x0009C027 File Offset: 0x0009B027
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.Write(buffer, offset, count);
			return new LazyAsyncResult(null, state, callback, null);
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x0009C040 File Offset: 0x0009B040
		public override void EndWrite(IAsyncResult asyncResult)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult;
			lazyAsyncResult.InternalWaitForCompletion();
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002655 RID: 9813 RVA: 0x0009C05B File Offset: 0x0009B05B
		public override bool CanTimeout
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002656 RID: 9814 RVA: 0x0009C05E File Offset: 0x0009B05E
		// (set) Token: 0x06002657 RID: 9815 RVA: 0x0009C066 File Offset: 0x0009B066
		public override int ReadTimeout
		{
			get
			{
				return this.m_ReadTimeout;
			}
			set
			{
				this.m_ReadTimeout = value;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x0009C06F File Offset: 0x0009B06F
		// (set) Token: 0x06002659 RID: 9817 RVA: 0x0009C077 File Offset: 0x0009B077
		public override int WriteTimeout
		{
			get
			{
				return this.m_WriteTimeout;
			}
			set
			{
				this.m_WriteTimeout = value;
			}
		}

		// Token: 0x040025E1 RID: 9697
		private int m_ReadTimeout;

		// Token: 0x040025E2 RID: 9698
		private int m_WriteTimeout;
	}
}
