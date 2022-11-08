using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000AA RID: 170
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class BindingList<T> : Collection<T>, IBindingList, IList, ICollection, IEnumerable, ICancelAddNew, IRaiseItemChangedEvents
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x00017C4F File Offset: 0x00016C4F
		public BindingList()
		{
			this.Initialize();
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00017C87 File Offset: 0x00016C87
		public BindingList(IList<T> list) : base(list)
		{
			this.Initialize();
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00017CC0 File Offset: 0x00016CC0
		private void Initialize()
		{
			this.allowNew = this.ItemTypeHasDefaultConstructor;
			if (typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T)))
			{
				this.raiseItemChangedEvents = true;
				foreach (T item in base.Items)
				{
					this.HookPropertyChanged(item);
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00017D3C File Offset: 0x00016D3C
		private bool ItemTypeHasDefaultConstructor
		{
			get
			{
				Type typeFromHandle = typeof(T);
				return typeFromHandle.IsPrimitive || typeFromHandle.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new Type[0], null) != null;
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000600 RID: 1536 RVA: 0x00017D78 File Offset: 0x00016D78
		// (remove) Token: 0x06000601 RID: 1537 RVA: 0x00017DB4 File Offset: 0x00016DB4
		public event AddingNewEventHandler AddingNew
		{
			add
			{
				bool flag = this.AllowNew;
				this.onAddingNew = (AddingNewEventHandler)Delegate.Combine(this.onAddingNew, value);
				if (flag != this.AllowNew)
				{
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
			remove
			{
				bool flag = this.AllowNew;
				this.onAddingNew = (AddingNewEventHandler)Delegate.Remove(this.onAddingNew, value);
				if (flag != this.AllowNew)
				{
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00017DF0 File Offset: 0x00016DF0
		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			if (this.onAddingNew != null)
			{
				this.onAddingNew(this, e);
			}
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00017E08 File Offset: 0x00016E08
		private object FireAddingNew()
		{
			AddingNewEventArgs addingNewEventArgs = new AddingNewEventArgs(null);
			this.OnAddingNew(addingNewEventArgs);
			return addingNewEventArgs.NewObject;
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000604 RID: 1540 RVA: 0x00017E29 File Offset: 0x00016E29
		// (remove) Token: 0x06000605 RID: 1541 RVA: 0x00017E42 File Offset: 0x00016E42
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Combine(this.onListChanged, value);
			}
			remove
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Remove(this.onListChanged, value);
			}
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00017E5B File Offset: 0x00016E5B
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if (this.onListChanged != null)
			{
				this.onListChanged(this, e);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00017E72 File Offset: 0x00016E72
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x00017E7A File Offset: 0x00016E7A
		public bool RaiseListChangedEvents
		{
			get
			{
				return this.raiseListChangedEvents;
			}
			set
			{
				if (this.raiseListChangedEvents != value)
				{
					this.raiseListChangedEvents = value;
				}
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00017E8C File Offset: 0x00016E8C
		public void ResetBindings()
		{
			this.FireListChanged(ListChangedType.Reset, -1);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00017E96 File Offset: 0x00016E96
		public void ResetItem(int position)
		{
			this.FireListChanged(ListChangedType.ItemChanged, position);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00017EA0 File Offset: 0x00016EA0
		private void FireListChanged(ListChangedType type, int index)
		{
			if (this.raiseListChangedEvents)
			{
				this.OnListChanged(new ListChangedEventArgs(type, index));
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00017EB8 File Offset: 0x00016EB8
		protected override void ClearItems()
		{
			this.EndNew(this.addNewPos);
			if (this.raiseItemChangedEvents)
			{
				foreach (T item in base.Items)
				{
					this.UnhookPropertyChanged(item);
				}
			}
			base.ClearItems();
			this.FireListChanged(ListChangedType.Reset, -1);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00017F28 File Offset: 0x00016F28
		protected override void InsertItem(int index, T item)
		{
			this.EndNew(this.addNewPos);
			base.InsertItem(index, item);
			if (this.raiseItemChangedEvents)
			{
				this.HookPropertyChanged(item);
			}
			this.FireListChanged(ListChangedType.ItemAdded, index);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00017F58 File Offset: 0x00016F58
		protected override void RemoveItem(int index)
		{
			if (!this.allowRemove && (this.addNewPos < 0 || this.addNewPos != index))
			{
				throw new NotSupportedException();
			}
			this.EndNew(this.addNewPos);
			if (this.raiseItemChangedEvents)
			{
				this.UnhookPropertyChanged(base[index]);
			}
			base.RemoveItem(index);
			this.FireListChanged(ListChangedType.ItemDeleted, index);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00017FB5 File Offset: 0x00016FB5
		protected override void SetItem(int index, T item)
		{
			if (this.raiseItemChangedEvents)
			{
				this.UnhookPropertyChanged(base[index]);
			}
			base.SetItem(index, item);
			if (this.raiseItemChangedEvents)
			{
				this.HookPropertyChanged(item);
			}
			this.FireListChanged(ListChangedType.ItemChanged, index);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00017FEB File Offset: 0x00016FEB
		public virtual void CancelNew(int itemIndex)
		{
			if (this.addNewPos >= 0 && this.addNewPos == itemIndex)
			{
				this.RemoveItem(this.addNewPos);
				this.addNewPos = -1;
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00018012 File Offset: 0x00017012
		public virtual void EndNew(int itemIndex)
		{
			if (this.addNewPos >= 0 && this.addNewPos == itemIndex)
			{
				this.addNewPos = -1;
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001802D File Offset: 0x0001702D
		public T AddNew()
		{
			return (T)((object)((IBindingList)this).AddNew());
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001803C File Offset: 0x0001703C
		object IBindingList.AddNew()
		{
			object obj = this.AddNewCore();
			this.addNewPos = ((obj != null) ? base.IndexOf((T)((object)obj)) : -1);
			return obj;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x00018069 File Offset: 0x00017069
		private bool AddingNewHandled
		{
			get
			{
				return this.onAddingNew != null && this.onAddingNew.GetInvocationList().Length > 0;
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00018088 File Offset: 0x00017088
		protected virtual object AddNewCore()
		{
			object obj = this.FireAddingNew();
			if (obj == null)
			{
				Type typeFromHandle = typeof(T);
				obj = SecurityUtils.SecureCreateInstance(typeFromHandle);
			}
			base.Add((T)((object)obj));
			return obj;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x000180BE File Offset: 0x000170BE
		// (set) Token: 0x06000617 RID: 1559 RVA: 0x000180E0 File Offset: 0x000170E0
		public bool AllowNew
		{
			get
			{
				if (this.userSetAllowNew || this.allowNew)
				{
					return this.allowNew;
				}
				return this.AddingNewHandled;
			}
			set
			{
				bool flag = this.AllowNew;
				this.userSetAllowNew = true;
				this.allowNew = value;
				if (flag != value)
				{
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001810E File Offset: 0x0001710E
		bool IBindingList.AllowNew
		{
			get
			{
				return this.AllowNew;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00018116 File Offset: 0x00017116
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x0001811E File Offset: 0x0001711E
		public bool AllowEdit
		{
			get
			{
				return this.allowEdit;
			}
			set
			{
				if (this.allowEdit != value)
				{
					this.allowEdit = value;
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x00018138 File Offset: 0x00017138
		bool IBindingList.AllowEdit
		{
			get
			{
				return this.AllowEdit;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00018140 File Offset: 0x00017140
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00018148 File Offset: 0x00017148
		public bool AllowRemove
		{
			get
			{
				return this.allowRemove;
			}
			set
			{
				if (this.allowRemove != value)
				{
					this.allowRemove = value;
					this.FireListChanged(ListChangedType.Reset, -1);
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00018162 File Offset: 0x00017162
		bool IBindingList.AllowRemove
		{
			get
			{
				return this.AllowRemove;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001816A File Offset: 0x0001716A
		bool IBindingList.SupportsChangeNotification
		{
			get
			{
				return this.SupportsChangeNotificationCore;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00018172 File Offset: 0x00017172
		protected virtual bool SupportsChangeNotificationCore
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00018175 File Offset: 0x00017175
		bool IBindingList.SupportsSearching
		{
			get
			{
				return this.SupportsSearchingCore;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001817D File Offset: 0x0001717D
		protected virtual bool SupportsSearchingCore
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x00018180 File Offset: 0x00017180
		bool IBindingList.SupportsSorting
		{
			get
			{
				return this.SupportsSortingCore;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x00018188 File Offset: 0x00017188
		protected virtual bool SupportsSortingCore
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x0001818B File Offset: 0x0001718B
		bool IBindingList.IsSorted
		{
			get
			{
				return this.IsSortedCore;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00018193 File Offset: 0x00017193
		protected virtual bool IsSortedCore
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00018196 File Offset: 0x00017196
		PropertyDescriptor IBindingList.SortProperty
		{
			get
			{
				return this.SortPropertyCore;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001819E File Offset: 0x0001719E
		protected virtual PropertyDescriptor SortPropertyCore
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000181A1 File Offset: 0x000171A1
		ListSortDirection IBindingList.SortDirection
		{
			get
			{
				return this.SortDirectionCore;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x000181A9 File Offset: 0x000171A9
		protected virtual ListSortDirection SortDirectionCore
		{
			get
			{
				return ListSortDirection.Ascending;
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000181AC File Offset: 0x000171AC
		void IBindingList.ApplySort(PropertyDescriptor prop, ListSortDirection direction)
		{
			this.ApplySortCore(prop, direction);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000181B6 File Offset: 0x000171B6
		protected virtual void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000181BD File Offset: 0x000171BD
		void IBindingList.RemoveSort()
		{
			this.RemoveSortCore();
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000181C5 File Offset: 0x000171C5
		protected virtual void RemoveSortCore()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000181CC File Offset: 0x000171CC
		int IBindingList.Find(PropertyDescriptor prop, object key)
		{
			return this.FindCore(prop, key);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000181D6 File Offset: 0x000171D6
		protected virtual int FindCore(PropertyDescriptor prop, object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000181DD File Offset: 0x000171DD
		void IBindingList.AddIndex(PropertyDescriptor prop)
		{
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000181DF File Offset: 0x000171DF
		void IBindingList.RemoveIndex(PropertyDescriptor prop)
		{
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000181E4 File Offset: 0x000171E4
		private void HookPropertyChanged(T item)
		{
			INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				if (this.propertyChangedEventHandler == null)
				{
					this.propertyChangedEventHandler = new PropertyChangedEventHandler(this.Child_PropertyChanged);
				}
				notifyPropertyChanged.PropertyChanged += this.propertyChangedEventHandler;
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00018228 File Offset: 0x00017228
		private void UnhookPropertyChanged(T item)
		{
			INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
			if (notifyPropertyChanged != null && this.propertyChangedEventHandler != null)
			{
				notifyPropertyChanged.PropertyChanged -= this.propertyChangedEventHandler;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00018258 File Offset: 0x00017258
		private void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.RaiseListChangedEvents)
			{
				if (sender == null || e == null || string.IsNullOrEmpty(e.PropertyName))
				{
					this.ResetBindings();
					return;
				}
				T t;
				try
				{
					t = (T)((object)sender);
				}
				catch (InvalidCastException)
				{
					this.ResetBindings();
					return;
				}
				int num = this.lastChangeIndex;
				if (num >= 0 && num < base.Count)
				{
					T t2 = base[num];
					if (t2.Equals(t))
					{
						goto IL_7B;
					}
				}
				num = base.IndexOf(t);
				this.lastChangeIndex = num;
				IL_7B:
				if (num == -1)
				{
					this.UnhookPropertyChanged(t);
					this.ResetBindings();
					return;
				}
				if (this.itemTypeProperties == null)
				{
					this.itemTypeProperties = TypeDescriptor.GetProperties(typeof(T));
				}
				PropertyDescriptor propDesc = this.itemTypeProperties.Find(e.PropertyName, true);
				ListChangedEventArgs e2 = new ListChangedEventArgs(ListChangedType.ItemChanged, num, propDesc);
				this.OnListChanged(e2);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x00018344 File Offset: 0x00017344
		bool IRaiseItemChangedEvents.RaisesItemChangedEvents
		{
			get
			{
				return this.raiseItemChangedEvents;
			}
		}

		// Token: 0x040008FA RID: 2298
		private int addNewPos = -1;

		// Token: 0x040008FB RID: 2299
		private bool raiseListChangedEvents = true;

		// Token: 0x040008FC RID: 2300
		private bool raiseItemChangedEvents;

		// Token: 0x040008FD RID: 2301
		[NonSerialized]
		private PropertyDescriptorCollection itemTypeProperties;

		// Token: 0x040008FE RID: 2302
		[NonSerialized]
		private PropertyChangedEventHandler propertyChangedEventHandler;

		// Token: 0x040008FF RID: 2303
		[NonSerialized]
		private AddingNewEventHandler onAddingNew;

		// Token: 0x04000900 RID: 2304
		[NonSerialized]
		private ListChangedEventHandler onListChanged;

		// Token: 0x04000901 RID: 2305
		[NonSerialized]
		private int lastChangeIndex = -1;

		// Token: 0x04000902 RID: 2306
		private bool allowNew = true;

		// Token: 0x04000903 RID: 2307
		private bool allowEdit = true;

		// Token: 0x04000904 RID: 2308
		private bool allowRemove = true;

		// Token: 0x04000905 RID: 2309
		private bool userSetAllowNew;
	}
}
