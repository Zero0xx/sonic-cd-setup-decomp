using System;
using System.Collections;

namespace System.ComponentModel
{
	// Token: 0x020000A7 RID: 167
	public interface IBindingList : IList, ICollection, IEnumerable
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060005E8 RID: 1512
		bool AllowNew { get; }

		// Token: 0x060005E9 RID: 1513
		object AddNew();

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060005EA RID: 1514
		bool AllowEdit { get; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060005EB RID: 1515
		bool AllowRemove { get; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060005EC RID: 1516
		bool SupportsChangeNotification { get; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060005ED RID: 1517
		bool SupportsSearching { get; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060005EE RID: 1518
		bool SupportsSorting { get; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060005EF RID: 1519
		bool IsSorted { get; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060005F0 RID: 1520
		PropertyDescriptor SortProperty { get; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060005F1 RID: 1521
		ListSortDirection SortDirection { get; }

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060005F2 RID: 1522
		// (remove) Token: 0x060005F3 RID: 1523
		event ListChangedEventHandler ListChanged;

		// Token: 0x060005F4 RID: 1524
		void AddIndex(PropertyDescriptor property);

		// Token: 0x060005F5 RID: 1525
		void ApplySort(PropertyDescriptor property, ListSortDirection direction);

		// Token: 0x060005F6 RID: 1526
		int Find(PropertyDescriptor property, object key);

		// Token: 0x060005F7 RID: 1527
		void RemoveIndex(PropertyDescriptor property);

		// Token: 0x060005F8 RID: 1528
		void RemoveSort();
	}
}
