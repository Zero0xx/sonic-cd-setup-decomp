using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200029E RID: 670
	internal sealed class Mscorlib_DictionaryValueCollectionDebugView<TKey, TValue>
	{
		// Token: 0x06001A31 RID: 6705 RVA: 0x00044410 File Offset: 0x00043410
		public Mscorlib_DictionaryValueCollectionDebugView(ICollection<TValue> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001A32 RID: 6706 RVA: 0x00044428 File Offset: 0x00043428
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public TValue[] Items
		{
			get
			{
				TValue[] array = new TValue[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04000A2F RID: 2607
		private ICollection<TValue> collection;
	}
}
