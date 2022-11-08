using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x02000386 RID: 902
	internal sealed class InterlockedStack
	{
		// Token: 0x06001C1D RID: 7197 RVA: 0x00069EF2 File Offset: 0x00068EF2
		internal InterlockedStack()
		{
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00069F08 File Offset: 0x00068F08
		internal void Push(object pooledStream)
		{
			if (pooledStream == null)
			{
				throw new ArgumentNullException("pooledStream");
			}
			lock (this._stack.SyncRoot)
			{
				this._stack.Push(pooledStream);
				this._count = this._stack.Count;
			}
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x00069F6C File Offset: 0x00068F6C
		internal object Pop()
		{
			object result;
			lock (this._stack.SyncRoot)
			{
				object obj = null;
				if (0 < this._stack.Count)
				{
					obj = this._stack.Pop();
					this._count = this._stack.Count;
				}
				result = obj;
			}
			return result;
		}

		// Token: 0x04001CC1 RID: 7361
		private readonly Stack _stack = new Stack();

		// Token: 0x04001CC2 RID: 7362
		private int _count;
	}
}
