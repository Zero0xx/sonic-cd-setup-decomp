using System;

namespace System.Net
{
	// Token: 0x02000598 RID: 1432
	internal class BufferAsyncResult : LazyAsyncResult
	{
		// Token: 0x06002C1F RID: 11295 RVA: 0x000BDE53 File Offset: 0x000BCE53
		public BufferAsyncResult(object asyncObject, BufferOffsetSize[] buffers, object asyncState, AsyncCallback asyncCallback) : base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffers = buffers;
			this.IsWrite = true;
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000BDE6D File Offset: 0x000BCE6D
		public BufferAsyncResult(object asyncObject, byte[] buffer, int offset, int count, object asyncState, AsyncCallback asyncCallback) : this(asyncObject, buffer, offset, count, false, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000BDE7F File Offset: 0x000BCE7F
		public BufferAsyncResult(object asyncObject, byte[] buffer, int offset, int count, bool isWrite, object asyncState, AsyncCallback asyncCallback) : base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffer = buffer;
			this.Offset = offset;
			this.Count = count;
			this.IsWrite = isWrite;
		}

		// Token: 0x040029FB RID: 10747
		public byte[] Buffer;

		// Token: 0x040029FC RID: 10748
		public BufferOffsetSize[] Buffers;

		// Token: 0x040029FD RID: 10749
		public int Offset;

		// Token: 0x040029FE RID: 10750
		public int Count;

		// Token: 0x040029FF RID: 10751
		public bool IsWrite;
	}
}
