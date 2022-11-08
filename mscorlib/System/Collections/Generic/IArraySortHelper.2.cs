using System;

namespace System.Collections.Generic
{
	// Token: 0x020002A9 RID: 681
	internal interface IArraySortHelper<TKey, TValue>
	{
		// Token: 0x06001AA4 RID: 6820
		void Sort(TKey[] keys, TValue[] values, int index, int length, IComparer<TKey> comparer);
	}
}
