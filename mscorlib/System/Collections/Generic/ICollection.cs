using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x0200028A RID: 650
	[TypeDependency("System.SZArrayHelper")]
	public interface ICollection<T> : IEnumerable<T>, IEnumerable
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001984 RID: 6532
		int Count { get; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001985 RID: 6533
		bool IsReadOnly { get; }

		// Token: 0x06001986 RID: 6534
		void Add(T item);

		// Token: 0x06001987 RID: 6535
		void Clear();

		// Token: 0x06001988 RID: 6536
		bool Contains(T item);

		// Token: 0x06001989 RID: 6537
		void CopyTo(T[] array, int arrayIndex);

		// Token: 0x0600198A RID: 6538
		bool Remove(T item);
	}
}
