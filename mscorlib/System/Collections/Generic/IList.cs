using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x020002A1 RID: 673
	[TypeDependency("System.SZArrayHelper")]
	public interface IList<T> : ICollection<!0>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x170003FA RID: 1018
		T this[int index]
		{
			get;
			set;
		}

		// Token: 0x06001A39 RID: 6713
		int IndexOf(T item);

		// Token: 0x06001A3A RID: 6714
		void Insert(int index, T item);

		// Token: 0x06001A3B RID: 6715
		void RemoveAt(int index);
	}
}
