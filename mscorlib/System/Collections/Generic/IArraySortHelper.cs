using System;

namespace System.Collections.Generic
{
	// Token: 0x020002A6 RID: 678
	internal interface IArraySortHelper<TKey>
	{
		// Token: 0x06001A94 RID: 6804
		void Sort(TKey[] keys, int index, int length, IComparer<TKey> comparer);

		// Token: 0x06001A95 RID: 6805
		int BinarySearch(TKey[] keys, int index, int length, TKey value, IComparer<TKey> comparer);
	}
}
