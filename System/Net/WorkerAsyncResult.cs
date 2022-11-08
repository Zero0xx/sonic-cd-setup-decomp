using System;

namespace System.Net
{
	// Token: 0x0200054B RID: 1355
	internal class WorkerAsyncResult : LazyAsyncResult
	{
		// Token: 0x06002923 RID: 10531 RVA: 0x000AC2F1 File Offset: 0x000AB2F1
		public WorkerAsyncResult(object asyncObject, object asyncState, AsyncCallback savedAsyncCallback, byte[] buffer, int offset, int end) : base(asyncObject, asyncState, savedAsyncCallback)
		{
			this.Buffer = buffer;
			this.Offset = offset;
			this.End = end;
		}

		// Token: 0x04002836 RID: 10294
		public byte[] Buffer;

		// Token: 0x04002837 RID: 10295
		public int Offset;

		// Token: 0x04002838 RID: 10296
		public int End;

		// Token: 0x04002839 RID: 10297
		public bool IsWrite;

		// Token: 0x0400283A RID: 10298
		public WorkerAsyncResult ParentResult;

		// Token: 0x0400283B RID: 10299
		public bool HeaderDone;

		// Token: 0x0400283C RID: 10300
		public bool HandshakeDone;
	}
}
