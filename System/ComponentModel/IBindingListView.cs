using System;
using System.Collections;

namespace System.ComponentModel
{
	// Token: 0x020000EA RID: 234
	public interface IBindingListView : IBindingList, IList, ICollection, IEnumerable
	{
		// Token: 0x060007D6 RID: 2006
		void ApplySort(ListSortDescriptionCollection sorts);

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007D7 RID: 2007
		// (set) Token: 0x060007D8 RID: 2008
		string Filter { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060007D9 RID: 2009
		ListSortDescriptionCollection SortDescriptions { get; }

		// Token: 0x060007DA RID: 2010
		void RemoveFilter();

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060007DB RID: 2011
		bool SupportsAdvancedSorting { get; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060007DC RID: 2012
		bool SupportsFiltering { get; }
	}
}
