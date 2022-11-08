using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200029D RID: 669
	internal sealed class Mscorlib_DictionaryKeyCollectionDebugView<TKey, TValue>
	{
		// Token: 0x06001A2F RID: 6703 RVA: 0x000443CC File Offset: 0x000433CC
		public Mscorlib_DictionaryKeyCollectionDebugView(ICollection<TKey> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x000443E4 File Offset: 0x000433E4
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public TKey[] Items
		{
			get
			{
				TKey[] array = new TKey[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04000A2E RID: 2606
		private ICollection<TKey> collection;
	}
}
