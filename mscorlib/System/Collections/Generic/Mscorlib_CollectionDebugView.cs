using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200029C RID: 668
	internal sealed class Mscorlib_CollectionDebugView<T>
	{
		// Token: 0x06001A2D RID: 6701 RVA: 0x00044385 File Offset: 0x00043385
		public Mscorlib_CollectionDebugView(ICollection<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this.collection = collection;
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x000443A0 File Offset: 0x000433A0
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				T[] array = new T[this.collection.Count];
				this.collection.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04000A2D RID: 2605
		private ICollection<T> collection;
	}
}
