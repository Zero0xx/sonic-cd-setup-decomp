using System;

namespace System.Net
{
	// Token: 0x020004F1 RID: 1265
	internal class NestedSingleAsyncResult : LazyAsyncResult
	{
		// Token: 0x06002794 RID: 10132 RVA: 0x000A2EFE File Offset: 0x000A1EFE
		internal NestedSingleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, object result) : base(asyncObject, asyncState, asyncCallback, result)
		{
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000A2F0B File Offset: 0x000A1F0B
		internal NestedSingleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, byte[] buffer, int offset, int size) : base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffer = buffer;
			this.Offset = offset;
			this.Size = size;
		}

		// Token: 0x040026BA RID: 9914
		internal byte[] Buffer;

		// Token: 0x040026BB RID: 9915
		internal int Offset;

		// Token: 0x040026BC RID: 9916
		internal int Size;
	}
}
