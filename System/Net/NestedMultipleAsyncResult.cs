using System;

namespace System.Net
{
	// Token: 0x020004F0 RID: 1264
	internal class NestedMultipleAsyncResult : LazyAsyncResult
	{
		// Token: 0x06002793 RID: 10131 RVA: 0x000A2EAC File Offset: 0x000A1EAC
		internal NestedMultipleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, BufferOffsetSize[] buffers) : base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffers = buffers;
			this.Size = 0;
			for (int i = 0; i < this.Buffers.Length; i++)
			{
				this.Size += this.Buffers[i].Size;
			}
		}

		// Token: 0x040026B8 RID: 9912
		internal BufferOffsetSize[] Buffers;

		// Token: 0x040026B9 RID: 9913
		internal int Size;
	}
}
