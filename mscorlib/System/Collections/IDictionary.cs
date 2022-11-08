using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000260 RID: 608
	[ComVisible(true)]
	public interface IDictionary : ICollection, IEnumerable
	{
		// Token: 0x17000345 RID: 837
		object this[object key]
		{
			get;
			set;
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060017C6 RID: 6086
		ICollection Keys { get; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060017C7 RID: 6087
		ICollection Values { get; }

		// Token: 0x060017C8 RID: 6088
		bool Contains(object key);

		// Token: 0x060017C9 RID: 6089
		void Add(object key, object value);

		// Token: 0x060017CA RID: 6090
		void Clear();

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060017CB RID: 6091
		bool IsReadOnly { get; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060017CC RID: 6092
		bool IsFixedSize { get; }

		// Token: 0x060017CD RID: 6093
		IDictionaryEnumerator GetEnumerator();

		// Token: 0x060017CE RID: 6094
		void Remove(object key);
	}
}
