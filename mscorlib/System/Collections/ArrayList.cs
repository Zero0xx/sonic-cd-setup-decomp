using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x0200024C RID: 588
	[DebuggerTypeProxy(typeof(ArrayList.ArrayListDebugView))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(true)]
	[Serializable]
	public class ArrayList : IList, ICollection, IEnumerable, ICloneable
	{
		// Token: 0x06001633 RID: 5683 RVA: 0x000385F2 File Offset: 0x000375F2
		internal ArrayList(bool trash)
		{
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x000385FA File Offset: 0x000375FA
		public ArrayList()
		{
			this._items = ArrayList.emptyArray;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00038610 File Offset: 0x00037610
		public ArrayList(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegNum", new object[]
				{
					"capacity"
				}));
			}
			this._items = new object[capacity];
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00038658 File Offset: 0x00037658
		public ArrayList(ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
			}
			this._items = new object[c.Count];
			this.AddRange(c);
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001637 RID: 5687 RVA: 0x00038690 File Offset: 0x00037690
		// (set) Token: 0x06001638 RID: 5688 RVA: 0x0003869C File Offset: 0x0003769C
		public virtual int Capacity
		{
			get
			{
				return this._items.Length;
			}
			set
			{
				if (value != this._items.Length)
				{
					if (value < this._size)
					{
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
					if (value > 0)
					{
						object[] array = new object[value];
						if (this._size > 0)
						{
							Array.Copy(this._items, 0, array, 0, this._size);
						}
						this._items = array;
						return;
					}
					this._items = new object[4];
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001639 RID: 5689 RVA: 0x0003870E File Offset: 0x0003770E
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600163A RID: 5690 RVA: 0x00038716 File Offset: 0x00037716
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x0600163B RID: 5691 RVA: 0x00038719 File Offset: 0x00037719
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x0003871C File Offset: 0x0003771C
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600163D RID: 5693 RVA: 0x0003871F File Offset: 0x0003771F
		public virtual object SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x170002F7 RID: 759
		public virtual object this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return this._items[index];
			}
			set
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this._items[index] = value;
				this._version++;
			}
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000387A8 File Offset: 0x000377A8
		public static ArrayList Adapter(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.IListWrapper(list);
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x000387C0 File Offset: 0x000377C0
		public virtual int Add(object value)
		{
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			this._items[this._size] = value;
			this._version++;
			return this._size++;
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00038818 File Offset: 0x00037818
		public virtual void AddRange(ICollection c)
		{
			this.InsertRange(this._size, c);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00038828 File Offset: 0x00037828
		public virtual int BinarySearch(int index, int count, object value, IComparer comparer)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			return Array.BinarySearch(this._items, index, count, value, comparer);
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00038888 File Offset: 0x00037888
		public virtual int BinarySearch(object value)
		{
			return this.BinarySearch(0, this.Count, value, null);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00038899 File Offset: 0x00037899
		public virtual int BinarySearch(object value, IComparer comparer)
		{
			return this.BinarySearch(0, this.Count, value, comparer);
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x000388AA File Offset: 0x000378AA
		public virtual void Clear()
		{
			if (this._size > 0)
			{
				Array.Clear(this._items, 0, this._size);
				this._size = 0;
			}
			this._version++;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x000388DC File Offset: 0x000378DC
		public virtual object Clone()
		{
			ArrayList arrayList = new ArrayList(this._size);
			arrayList._size = this._size;
			arrayList._version = this._version;
			Array.Copy(this._items, 0, arrayList._items, 0, this._size);
			return arrayList;
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x00038928 File Offset: 0x00037928
		public virtual bool Contains(object item)
		{
			if (item == null)
			{
				for (int i = 0; i < this._size; i++)
				{
					if (this._items[i] == null)
					{
						return true;
					}
				}
				return false;
			}
			for (int j = 0; j < this._size; j++)
			{
				if (this._items[j] != null && this._items[j].Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x00038985 File Offset: 0x00037985
		public virtual void CopyTo(Array array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0003898F File Offset: 0x0003798F
		public virtual void CopyTo(Array array, int arrayIndex)
		{
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			Array.Copy(this._items, 0, array, arrayIndex, this._size);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x000389C4 File Offset: 0x000379C4
		public virtual void CopyTo(int index, Array array, int arrayIndex, int count)
		{
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			Array.Copy(this._items, index, array, arrayIndex, count);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00038A1C File Offset: 0x00037A1C
		private void EnsureCapacity(int min)
		{
			if (this._items.Length < min)
			{
				int num = (this._items.Length == 0) ? 4 : (this._items.Length * 2);
				if (num < min)
				{
					num = min;
				}
				this.Capacity = num;
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00038A59 File Offset: 0x00037A59
		public static IList FixedSize(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.FixedSizeList(list);
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00038A6F File Offset: 0x00037A6F
		public static ArrayList FixedSize(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.FixedSizeArrayList(list);
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00038A85 File Offset: 0x00037A85
		public virtual IEnumerator GetEnumerator()
		{
			return new ArrayList.ArrayListEnumeratorSimple(this);
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00038A90 File Offset: 0x00037A90
		public virtual IEnumerator GetEnumerator(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			return new ArrayList.ArrayListEnumerator(this, index, count);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00038AE8 File Offset: 0x00037AE8
		public virtual int IndexOf(object value)
		{
			return Array.IndexOf(this._items, value, 0, this._size);
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00038AFD File Offset: 0x00037AFD
		public virtual int IndexOf(object value, int startIndex)
		{
			if (startIndex > this._size)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return Array.IndexOf(this._items, value, startIndex, this._size - startIndex);
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00038B34 File Offset: 0x00037B34
		public virtual int IndexOf(object value, int startIndex, int count)
		{
			if (startIndex > this._size)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0 || startIndex > this._size - count)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			return Array.IndexOf(this._items, value, startIndex, count);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00038B94 File Offset: 0x00037B94
		public virtual void Insert(int index, object value)
		{
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_ArrayListInsert"));
			}
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this._items, index, this._items, index + 1, this._size - index);
			}
			this._items[index] = value;
			this._size++;
			this._version++;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00038C2C File Offset: 0x00037C2C
		public virtual void InsertRange(int index, ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
			}
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			int count = c.Count;
			if (count > 0)
			{
				this.EnsureCapacity(this._size + count);
				if (index < this._size)
				{
					Array.Copy(this._items, index, this._items, index + count, this._size - index);
				}
				object[] array = new object[count];
				c.CopyTo(array, 0);
				array.CopyTo(this._items, index);
				this._size += count;
				this._version++;
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00038CEA File Offset: 0x00037CEA
		public virtual int LastIndexOf(object value)
		{
			return this.LastIndexOf(value, this._size - 1, this._size);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00038D01 File Offset: 0x00037D01
		public virtual int LastIndexOf(object value, int startIndex)
		{
			if (startIndex >= this._size)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return this.LastIndexOf(value, startIndex, startIndex + 1);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00038D2C File Offset: 0x00037D2C
		public virtual int LastIndexOf(object value, int startIndex, int count)
		{
			if (this._size == 0)
			{
				return -1;
			}
			if (startIndex < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((startIndex < 0) ? "startIndex" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (startIndex >= this._size || count > startIndex + 1)
			{
				throw new ArgumentOutOfRangeException((startIndex >= this._size) ? "startIndex" : "count", Environment.GetResourceString("ArgumentOutOfRange_BiggerThanCollection"));
			}
			return Array.LastIndexOf(this._items, value, startIndex, count);
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00038DAD File Offset: 0x00037DAD
		public static IList ReadOnly(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.ReadOnlyList(list);
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00038DC3 File Offset: 0x00037DC3
		public static ArrayList ReadOnly(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.ReadOnlyArrayList(list);
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00038DDC File Offset: 0x00037DDC
		public virtual void Remove(object obj)
		{
			int num = this.IndexOf(obj);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00038DFC File Offset: 0x00037DFC
		public virtual void RemoveAt(int index)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this._items, index + 1, this._items, index, this._size - index);
			}
			this._items[this._size] = null;
			this._version++;
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00038E7C File Offset: 0x00037E7C
		public virtual void RemoveRange(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (count > 0)
			{
				int i = this._size;
				this._size -= count;
				if (index < this._size)
				{
					Array.Copy(this._items, index + count, this._items, index, this._size - index);
				}
				while (i > this._size)
				{
					this._items[--i] = null;
				}
				this._version++;
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00038F34 File Offset: 0x00037F34
		public static ArrayList Repeat(object value, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			ArrayList arrayList = new ArrayList((count > 4) ? count : 4);
			for (int i = 0; i < count; i++)
			{
				arrayList.Add(value);
			}
			return arrayList;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00038F7D File Offset: 0x00037F7D
		public virtual void Reverse()
		{
			this.Reverse(0, this.Count);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00038F8C File Offset: 0x00037F8C
		public virtual void Reverse(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			Array.Reverse(this._items, index, count);
			this._version++;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00038FF8 File Offset: 0x00037FF8
		public virtual void SetRange(int index, ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
			}
			int count = c.Count;
			if (index < 0 || index > this._size - count)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count > 0)
			{
				c.CopyTo(this._items, index);
				this._version++;
			}
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00039068 File Offset: 0x00038068
		public virtual ArrayList GetRange(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			return new ArrayList.Range(this, index, count);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x000390C0 File Offset: 0x000380C0
		public virtual void Sort()
		{
			this.Sort(0, this.Count, Comparer.Default);
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x000390D4 File Offset: 0x000380D4
		public virtual void Sort(IComparer comparer)
		{
			this.Sort(0, this.Count, comparer);
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x000390E4 File Offset: 0x000380E4
		public virtual void Sort(int index, int count, IComparer comparer)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			Array.Sort(this._items, index, count, comparer);
			this._version++;
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00039150 File Offset: 0x00038150
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static IList Synchronized(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.SyncIList(list);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00039166 File Offset: 0x00038166
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static ArrayList Synchronized(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.SyncArrayList(list);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0003917C File Offset: 0x0003817C
		public virtual object[] ToArray()
		{
			object[] array = new object[this._size];
			Array.Copy(this._items, 0, array, 0, this._size);
			return array;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000391AC File Offset: 0x000381AC
		public virtual Array ToArray(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Array array = Array.CreateInstance(type, this._size);
			Array.Copy(this._items, 0, array, 0, this._size);
			return array;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000391E9 File Offset: 0x000381E9
		public virtual void TrimToSize()
		{
			this.Capacity = this._size;
		}

		// Token: 0x04000957 RID: 2391
		private const int _defaultCapacity = 4;

		// Token: 0x04000958 RID: 2392
		private object[] _items;

		// Token: 0x04000959 RID: 2393
		private int _size;

		// Token: 0x0400095A RID: 2394
		private int _version;

		// Token: 0x0400095B RID: 2395
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x0400095C RID: 2396
		private static readonly object[] emptyArray = new object[0];

		// Token: 0x0200024D RID: 589
		[Serializable]
		private class IListWrapper : ArrayList
		{
			// Token: 0x0600166C RID: 5740 RVA: 0x00039204 File Offset: 0x00038204
			internal IListWrapper(IList list)
			{
				this._list = list;
				this._version = 0;
			}

			// Token: 0x170002F8 RID: 760
			// (get) Token: 0x0600166D RID: 5741 RVA: 0x0003921A File Offset: 0x0003821A
			// (set) Token: 0x0600166E RID: 5742 RVA: 0x00039227 File Offset: 0x00038227
			public override int Capacity
			{
				get
				{
					return this._list.Count;
				}
				set
				{
					if (value < this._list.Count)
					{
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
				}
			}

			// Token: 0x170002F9 RID: 761
			// (get) Token: 0x0600166F RID: 5743 RVA: 0x0003924C File Offset: 0x0003824C
			public override int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x170002FA RID: 762
			// (get) Token: 0x06001670 RID: 5744 RVA: 0x00039259 File Offset: 0x00038259
			public override bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x170002FB RID: 763
			// (get) Token: 0x06001671 RID: 5745 RVA: 0x00039266 File Offset: 0x00038266
			public override bool IsFixedSize
			{
				get
				{
					return this._list.IsFixedSize;
				}
			}

			// Token: 0x170002FC RID: 764
			// (get) Token: 0x06001672 RID: 5746 RVA: 0x00039273 File Offset: 0x00038273
			public override bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x170002FD RID: 765
			public override object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					this._list[index] = value;
					this._version++;
				}
			}

			// Token: 0x170002FE RID: 766
			// (get) Token: 0x06001675 RID: 5749 RVA: 0x000392AB File Offset: 0x000382AB
			public override object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x06001676 RID: 5750 RVA: 0x000392B8 File Offset: 0x000382B8
			public override int Add(object obj)
			{
				int result = this._list.Add(obj);
				this._version++;
				return result;
			}

			// Token: 0x06001677 RID: 5751 RVA: 0x000392E1 File Offset: 0x000382E1
			public override void AddRange(ICollection c)
			{
				this.InsertRange(this.Count, c);
			}

			// Token: 0x06001678 RID: 5752 RVA: 0x000392F0 File Offset: 0x000382F0
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				int i = index;
				int num = index + count - 1;
				while (i <= num)
				{
					int num2 = (i + num) / 2;
					int num3 = comparer.Compare(value, this._list[num2]);
					if (num3 == 0)
					{
						return num2;
					}
					if (num3 < 0)
					{
						num = num2 - 1;
					}
					else
					{
						i = num2 + 1;
					}
				}
				return ~i;
			}

			// Token: 0x06001679 RID: 5753 RVA: 0x0003938E File Offset: 0x0003838E
			public override void Clear()
			{
				if (this._list.IsFixedSize)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
				}
				this._list.Clear();
				this._version++;
			}

			// Token: 0x0600167A RID: 5754 RVA: 0x000393C6 File Offset: 0x000383C6
			public override object Clone()
			{
				return new ArrayList.IListWrapper(this._list);
			}

			// Token: 0x0600167B RID: 5755 RVA: 0x000393D3 File Offset: 0x000383D3
			public override bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x0600167C RID: 5756 RVA: 0x000393E1 File Offset: 0x000383E1
			public override void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x0600167D RID: 5757 RVA: 0x000393F0 File Offset: 0x000383F0
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0 || arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "arrayIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				for (int i = index; i < index + count; i++)
				{
					array.SetValue(this._list[i], arrayIndex++);
				}
			}

			// Token: 0x0600167E RID: 5758 RVA: 0x000394CA File Offset: 0x000384CA
			public override IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x0600167F RID: 5759 RVA: 0x000394D8 File Offset: 0x000384D8
			public override IEnumerator GetEnumerator(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.IListWrapper.IListWrapperEnumWrapper(this, index, count);
			}

			// Token: 0x06001680 RID: 5760 RVA: 0x00039535 File Offset: 0x00038535
			public override int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x06001681 RID: 5761 RVA: 0x00039543 File Offset: 0x00038543
			public override int IndexOf(object value, int startIndex)
			{
				return this.IndexOf(value, startIndex, this._list.Count - startIndex);
			}

			// Token: 0x06001682 RID: 5762 RVA: 0x0003955C File Offset: 0x0003855C
			public override int IndexOf(object value, int startIndex, int count)
			{
				if (startIndex < 0 || startIndex > this._list.Count)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (count < 0 || startIndex > this._list.Count - count)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				int num = startIndex + count;
				if (value == null)
				{
					for (int i = startIndex; i < num; i++)
					{
						if (this._list[i] == null)
						{
							return i;
						}
					}
					return -1;
				}
				for (int j = startIndex; j < num; j++)
				{
					if (this._list[j] != null && this._list[j].Equals(value))
					{
						return j;
					}
				}
				return -1;
			}

			// Token: 0x06001683 RID: 5763 RVA: 0x0003960F File Offset: 0x0003860F
			public override void Insert(int index, object obj)
			{
				this._list.Insert(index, obj);
				this._version++;
			}

			// Token: 0x06001684 RID: 5764 RVA: 0x0003962C File Offset: 0x0003862C
			public override void InsertRange(int index, ICollection c)
			{
				if (c == null)
				{
					throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
				}
				if (index < 0 || index > this._list.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (c.Count > 0)
				{
					ArrayList arrayList = this._list as ArrayList;
					if (arrayList != null)
					{
						arrayList.InsertRange(index, c);
					}
					else
					{
						foreach (object value in c)
						{
							this._list.Insert(index++, value);
						}
					}
					this._version++;
				}
			}

			// Token: 0x06001685 RID: 5765 RVA: 0x000396D0 File Offset: 0x000386D0
			public override int LastIndexOf(object value)
			{
				return this.LastIndexOf(value, this._list.Count - 1, this._list.Count);
			}

			// Token: 0x06001686 RID: 5766 RVA: 0x000396F1 File Offset: 0x000386F1
			public override int LastIndexOf(object value, int startIndex)
			{
				return this.LastIndexOf(value, startIndex, startIndex + 1);
			}

			// Token: 0x06001687 RID: 5767 RVA: 0x00039700 File Offset: 0x00038700
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				if (this._list.Count == 0)
				{
					return -1;
				}
				if (startIndex < 0 || startIndex >= this._list.Count)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (count < 0 || count > startIndex + 1)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				int num = startIndex - count + 1;
				if (value == null)
				{
					for (int i = startIndex; i >= num; i--)
					{
						if (this._list[i] == null)
						{
							return i;
						}
					}
					return -1;
				}
				for (int j = startIndex; j >= num; j--)
				{
					if (this._list[j] != null && this._list[j].Equals(value))
					{
						return j;
					}
				}
				return -1;
			}

			// Token: 0x06001688 RID: 5768 RVA: 0x000397BC File Offset: 0x000387BC
			public override void Remove(object value)
			{
				int num = this.IndexOf(value);
				if (num >= 0)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06001689 RID: 5769 RVA: 0x000397DC File Offset: 0x000387DC
			public override void RemoveAt(int index)
			{
				this._list.RemoveAt(index);
				this._version++;
			}

			// Token: 0x0600168A RID: 5770 RVA: 0x000397F8 File Offset: 0x000387F8
			public override void RemoveRange(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (count > 0)
				{
					this._version++;
				}
				while (count > 0)
				{
					this._list.RemoveAt(index);
					count--;
				}
			}

			// Token: 0x0600168B RID: 5771 RVA: 0x00039878 File Offset: 0x00038878
			public override void Reverse(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				int i = index;
				int num = index + count - 1;
				while (i < num)
				{
					object value = this._list[i];
					this._list[i++] = this._list[num];
					this._list[num--] = value;
				}
				this._version++;
			}

			// Token: 0x0600168C RID: 5772 RVA: 0x00039924 File Offset: 0x00038924
			public override void SetRange(int index, ICollection c)
			{
				if (c == null)
				{
					throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
				}
				if (index < 0 || index > this._list.Count - c.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (c.Count > 0)
				{
					foreach (object value in c)
					{
						this._list[index++] = value;
					}
					this._version++;
				}
			}

			// Token: 0x0600168D RID: 5773 RVA: 0x000399B8 File Offset: 0x000389B8
			public override ArrayList GetRange(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.Range(this, index, count);
			}

			// Token: 0x0600168E RID: 5774 RVA: 0x00039A18 File Offset: 0x00038A18
			public override void Sort(int index, int count, IComparer comparer)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				object[] array = new object[count];
				this.CopyTo(index, array, 0, count);
				Array.Sort(array, 0, count, comparer);
				for (int i = 0; i < count; i++)
				{
					this._list[i + index] = array[i];
				}
				this._version++;
			}

			// Token: 0x0600168F RID: 5775 RVA: 0x00039AB4 File Offset: 0x00038AB4
			public override object[] ToArray()
			{
				object[] array = new object[this.Count];
				this._list.CopyTo(array, 0);
				return array;
			}

			// Token: 0x06001690 RID: 5776 RVA: 0x00039ADC File Offset: 0x00038ADC
			public override Array ToArray(Type type)
			{
				if (type == null)
				{
					throw new ArgumentNullException("type");
				}
				Array array = Array.CreateInstance(type, this._list.Count);
				this._list.CopyTo(array, 0);
				return array;
			}

			// Token: 0x06001691 RID: 5777 RVA: 0x00039B17 File Offset: 0x00038B17
			public override void TrimToSize()
			{
			}

			// Token: 0x0400095D RID: 2397
			private IList _list;

			// Token: 0x0200024E RID: 590
			[Serializable]
			private sealed class IListWrapperEnumWrapper : IEnumerator, ICloneable
			{
				// Token: 0x06001692 RID: 5778 RVA: 0x00039B19 File Offset: 0x00038B19
				private IListWrapperEnumWrapper()
				{
				}

				// Token: 0x06001693 RID: 5779 RVA: 0x00039B24 File Offset: 0x00038B24
				internal IListWrapperEnumWrapper(ArrayList.IListWrapper listWrapper, int startIndex, int count)
				{
					this._en = listWrapper.GetEnumerator();
					this._initialStartIndex = startIndex;
					this._initialCount = count;
					while (startIndex-- > 0 && this._en.MoveNext())
					{
					}
					this._remaining = count;
					this._firstCall = true;
				}

				// Token: 0x06001694 RID: 5780 RVA: 0x00039B78 File Offset: 0x00038B78
				public object Clone()
				{
					return new ArrayList.IListWrapper.IListWrapperEnumWrapper
					{
						_en = (IEnumerator)((ICloneable)this._en).Clone(),
						_initialStartIndex = this._initialStartIndex,
						_initialCount = this._initialCount,
						_remaining = this._remaining,
						_firstCall = this._firstCall
					};
				}

				// Token: 0x06001695 RID: 5781 RVA: 0x00039BD8 File Offset: 0x00038BD8
				public bool MoveNext()
				{
					if (this._firstCall)
					{
						this._firstCall = false;
						return this._remaining-- > 0 && this._en.MoveNext();
					}
					if (this._remaining < 0)
					{
						return false;
					}
					bool flag = this._en.MoveNext();
					return flag && this._remaining-- > 0;
				}

				// Token: 0x170002FF RID: 767
				// (get) Token: 0x06001696 RID: 5782 RVA: 0x00039C46 File Offset: 0x00038C46
				public object Current
				{
					get
					{
						if (this._firstCall)
						{
							throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
						}
						if (this._remaining < 0)
						{
							throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
						}
						return this._en.Current;
					}
				}

				// Token: 0x06001697 RID: 5783 RVA: 0x00039C84 File Offset: 0x00038C84
				public void Reset()
				{
					this._en.Reset();
					int initialStartIndex = this._initialStartIndex;
					while (initialStartIndex-- > 0 && this._en.MoveNext())
					{
					}
					this._remaining = this._initialCount;
					this._firstCall = true;
				}

				// Token: 0x0400095E RID: 2398
				private IEnumerator _en;

				// Token: 0x0400095F RID: 2399
				private int _remaining;

				// Token: 0x04000960 RID: 2400
				private int _initialStartIndex;

				// Token: 0x04000961 RID: 2401
				private int _initialCount;

				// Token: 0x04000962 RID: 2402
				private bool _firstCall;
			}
		}

		// Token: 0x0200024F RID: 591
		[Serializable]
		private class SyncArrayList : ArrayList
		{
			// Token: 0x06001698 RID: 5784 RVA: 0x00039CCB File Offset: 0x00038CCB
			internal SyncArrayList(ArrayList list) : base(false)
			{
				this._list = list;
				this._root = list.SyncRoot;
			}

			// Token: 0x17000300 RID: 768
			// (get) Token: 0x06001699 RID: 5785 RVA: 0x00039CE8 File Offset: 0x00038CE8
			// (set) Token: 0x0600169A RID: 5786 RVA: 0x00039D28 File Offset: 0x00038D28
			public override int Capacity
			{
				get
				{
					int capacity;
					lock (this._root)
					{
						capacity = this._list.Capacity;
					}
					return capacity;
				}
				set
				{
					lock (this._root)
					{
						this._list.Capacity = value;
					}
				}
			}

			// Token: 0x17000301 RID: 769
			// (get) Token: 0x0600169B RID: 5787 RVA: 0x00039D68 File Offset: 0x00038D68
			public override int Count
			{
				get
				{
					int count;
					lock (this._root)
					{
						count = this._list.Count;
					}
					return count;
				}
			}

			// Token: 0x17000302 RID: 770
			// (get) Token: 0x0600169C RID: 5788 RVA: 0x00039DA8 File Offset: 0x00038DA8
			public override bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x17000303 RID: 771
			// (get) Token: 0x0600169D RID: 5789 RVA: 0x00039DB5 File Offset: 0x00038DB5
			public override bool IsFixedSize
			{
				get
				{
					return this._list.IsFixedSize;
				}
			}

			// Token: 0x17000304 RID: 772
			// (get) Token: 0x0600169E RID: 5790 RVA: 0x00039DC2 File Offset: 0x00038DC2
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000305 RID: 773
			public override object this[int index]
			{
				get
				{
					object result;
					lock (this._root)
					{
						result = this._list[index];
					}
					return result;
				}
				set
				{
					lock (this._root)
					{
						this._list[index] = value;
					}
				}
			}

			// Token: 0x17000306 RID: 774
			// (get) Token: 0x060016A1 RID: 5793 RVA: 0x00039E4C File Offset: 0x00038E4C
			public override object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x060016A2 RID: 5794 RVA: 0x00039E54 File Offset: 0x00038E54
			public override int Add(object value)
			{
				int result;
				lock (this._root)
				{
					result = this._list.Add(value);
				}
				return result;
			}

			// Token: 0x060016A3 RID: 5795 RVA: 0x00039E98 File Offset: 0x00038E98
			public override void AddRange(ICollection c)
			{
				lock (this._root)
				{
					this._list.AddRange(c);
				}
			}

			// Token: 0x060016A4 RID: 5796 RVA: 0x00039ED8 File Offset: 0x00038ED8
			public override int BinarySearch(object value)
			{
				int result;
				lock (this._root)
				{
					result = this._list.BinarySearch(value);
				}
				return result;
			}

			// Token: 0x060016A5 RID: 5797 RVA: 0x00039F1C File Offset: 0x00038F1C
			public override int BinarySearch(object value, IComparer comparer)
			{
				int result;
				lock (this._root)
				{
					result = this._list.BinarySearch(value, comparer);
				}
				return result;
			}

			// Token: 0x060016A6 RID: 5798 RVA: 0x00039F60 File Offset: 0x00038F60
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				int result;
				lock (this._root)
				{
					result = this._list.BinarySearch(index, count, value, comparer);
				}
				return result;
			}

			// Token: 0x060016A7 RID: 5799 RVA: 0x00039FA8 File Offset: 0x00038FA8
			public override void Clear()
			{
				lock (this._root)
				{
					this._list.Clear();
				}
			}

			// Token: 0x060016A8 RID: 5800 RVA: 0x00039FE8 File Offset: 0x00038FE8
			public override object Clone()
			{
				object result;
				lock (this._root)
				{
					result = new ArrayList.SyncArrayList((ArrayList)this._list.Clone());
				}
				return result;
			}

			// Token: 0x060016A9 RID: 5801 RVA: 0x0003A034 File Offset: 0x00039034
			public override bool Contains(object item)
			{
				bool result;
				lock (this._root)
				{
					result = this._list.Contains(item);
				}
				return result;
			}

			// Token: 0x060016AA RID: 5802 RVA: 0x0003A078 File Offset: 0x00039078
			public override void CopyTo(Array array)
			{
				lock (this._root)
				{
					this._list.CopyTo(array);
				}
			}

			// Token: 0x060016AB RID: 5803 RVA: 0x0003A0B8 File Offset: 0x000390B8
			public override void CopyTo(Array array, int index)
			{
				lock (this._root)
				{
					this._list.CopyTo(array, index);
				}
			}

			// Token: 0x060016AC RID: 5804 RVA: 0x0003A0F8 File Offset: 0x000390F8
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				lock (this._root)
				{
					this._list.CopyTo(index, array, arrayIndex, count);
				}
			}

			// Token: 0x060016AD RID: 5805 RVA: 0x0003A13C File Offset: 0x0003913C
			public override IEnumerator GetEnumerator()
			{
				IEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._list.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x060016AE RID: 5806 RVA: 0x0003A17C File Offset: 0x0003917C
			public override IEnumerator GetEnumerator(int index, int count)
			{
				IEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._list.GetEnumerator(index, count);
				}
				return enumerator;
			}

			// Token: 0x060016AF RID: 5807 RVA: 0x0003A1C0 File Offset: 0x000391C0
			public override int IndexOf(object value)
			{
				int result;
				lock (this._root)
				{
					result = this._list.IndexOf(value);
				}
				return result;
			}

			// Token: 0x060016B0 RID: 5808 RVA: 0x0003A204 File Offset: 0x00039204
			public override int IndexOf(object value, int startIndex)
			{
				int result;
				lock (this._root)
				{
					result = this._list.IndexOf(value, startIndex);
				}
				return result;
			}

			// Token: 0x060016B1 RID: 5809 RVA: 0x0003A248 File Offset: 0x00039248
			public override int IndexOf(object value, int startIndex, int count)
			{
				int result;
				lock (this._root)
				{
					result = this._list.IndexOf(value, startIndex, count);
				}
				return result;
			}

			// Token: 0x060016B2 RID: 5810 RVA: 0x0003A28C File Offset: 0x0003928C
			public override void Insert(int index, object value)
			{
				lock (this._root)
				{
					this._list.Insert(index, value);
				}
			}

			// Token: 0x060016B3 RID: 5811 RVA: 0x0003A2CC File Offset: 0x000392CC
			public override void InsertRange(int index, ICollection c)
			{
				lock (this._root)
				{
					this._list.InsertRange(index, c);
				}
			}

			// Token: 0x060016B4 RID: 5812 RVA: 0x0003A30C File Offset: 0x0003930C
			public override int LastIndexOf(object value)
			{
				int result;
				lock (this._root)
				{
					result = this._list.LastIndexOf(value);
				}
				return result;
			}

			// Token: 0x060016B5 RID: 5813 RVA: 0x0003A350 File Offset: 0x00039350
			public override int LastIndexOf(object value, int startIndex)
			{
				int result;
				lock (this._root)
				{
					result = this._list.LastIndexOf(value, startIndex);
				}
				return result;
			}

			// Token: 0x060016B6 RID: 5814 RVA: 0x0003A394 File Offset: 0x00039394
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				int result;
				lock (this._root)
				{
					result = this._list.LastIndexOf(value, startIndex, count);
				}
				return result;
			}

			// Token: 0x060016B7 RID: 5815 RVA: 0x0003A3D8 File Offset: 0x000393D8
			public override void Remove(object value)
			{
				lock (this._root)
				{
					this._list.Remove(value);
				}
			}

			// Token: 0x060016B8 RID: 5816 RVA: 0x0003A418 File Offset: 0x00039418
			public override void RemoveAt(int index)
			{
				lock (this._root)
				{
					this._list.RemoveAt(index);
				}
			}

			// Token: 0x060016B9 RID: 5817 RVA: 0x0003A458 File Offset: 0x00039458
			public override void RemoveRange(int index, int count)
			{
				lock (this._root)
				{
					this._list.RemoveRange(index, count);
				}
			}

			// Token: 0x060016BA RID: 5818 RVA: 0x0003A498 File Offset: 0x00039498
			public override void Reverse(int index, int count)
			{
				lock (this._root)
				{
					this._list.Reverse(index, count);
				}
			}

			// Token: 0x060016BB RID: 5819 RVA: 0x0003A4D8 File Offset: 0x000394D8
			public override void SetRange(int index, ICollection c)
			{
				lock (this._root)
				{
					this._list.SetRange(index, c);
				}
			}

			// Token: 0x060016BC RID: 5820 RVA: 0x0003A518 File Offset: 0x00039518
			public override ArrayList GetRange(int index, int count)
			{
				ArrayList range;
				lock (this._root)
				{
					range = this._list.GetRange(index, count);
				}
				return range;
			}

			// Token: 0x060016BD RID: 5821 RVA: 0x0003A55C File Offset: 0x0003955C
			public override void Sort()
			{
				lock (this._root)
				{
					this._list.Sort();
				}
			}

			// Token: 0x060016BE RID: 5822 RVA: 0x0003A59C File Offset: 0x0003959C
			public override void Sort(IComparer comparer)
			{
				lock (this._root)
				{
					this._list.Sort(comparer);
				}
			}

			// Token: 0x060016BF RID: 5823 RVA: 0x0003A5DC File Offset: 0x000395DC
			public override void Sort(int index, int count, IComparer comparer)
			{
				lock (this._root)
				{
					this._list.Sort(index, count, comparer);
				}
			}

			// Token: 0x060016C0 RID: 5824 RVA: 0x0003A620 File Offset: 0x00039620
			public override object[] ToArray()
			{
				object[] result;
				lock (this._root)
				{
					result = this._list.ToArray();
				}
				return result;
			}

			// Token: 0x060016C1 RID: 5825 RVA: 0x0003A660 File Offset: 0x00039660
			public override Array ToArray(Type type)
			{
				Array result;
				lock (this._root)
				{
					result = this._list.ToArray(type);
				}
				return result;
			}

			// Token: 0x060016C2 RID: 5826 RVA: 0x0003A6A4 File Offset: 0x000396A4
			public override void TrimToSize()
			{
				lock (this._root)
				{
					this._list.TrimToSize();
				}
			}

			// Token: 0x04000963 RID: 2403
			private ArrayList _list;

			// Token: 0x04000964 RID: 2404
			private object _root;
		}

		// Token: 0x02000250 RID: 592
		[Serializable]
		private class SyncIList : IList, ICollection, IEnumerable
		{
			// Token: 0x060016C3 RID: 5827 RVA: 0x0003A6E4 File Offset: 0x000396E4
			internal SyncIList(IList list)
			{
				this._list = list;
				this._root = list.SyncRoot;
			}

			// Token: 0x17000307 RID: 775
			// (get) Token: 0x060016C4 RID: 5828 RVA: 0x0003A700 File Offset: 0x00039700
			public virtual int Count
			{
				get
				{
					int count;
					lock (this._root)
					{
						count = this._list.Count;
					}
					return count;
				}
			}

			// Token: 0x17000308 RID: 776
			// (get) Token: 0x060016C5 RID: 5829 RVA: 0x0003A740 File Offset: 0x00039740
			public virtual bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x17000309 RID: 777
			// (get) Token: 0x060016C6 RID: 5830 RVA: 0x0003A74D File Offset: 0x0003974D
			public virtual bool IsFixedSize
			{
				get
				{
					return this._list.IsFixedSize;
				}
			}

			// Token: 0x1700030A RID: 778
			// (get) Token: 0x060016C7 RID: 5831 RVA: 0x0003A75A File Offset: 0x0003975A
			public virtual bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700030B RID: 779
			public virtual object this[int index]
			{
				get
				{
					object result;
					lock (this._root)
					{
						result = this._list[index];
					}
					return result;
				}
				set
				{
					lock (this._root)
					{
						this._list[index] = value;
					}
				}
			}

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x060016CA RID: 5834 RVA: 0x0003A7E4 File Offset: 0x000397E4
			public virtual object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x060016CB RID: 5835 RVA: 0x0003A7EC File Offset: 0x000397EC
			public virtual int Add(object value)
			{
				int result;
				lock (this._root)
				{
					result = this._list.Add(value);
				}
				return result;
			}

			// Token: 0x060016CC RID: 5836 RVA: 0x0003A830 File Offset: 0x00039830
			public virtual void Clear()
			{
				lock (this._root)
				{
					this._list.Clear();
				}
			}

			// Token: 0x060016CD RID: 5837 RVA: 0x0003A870 File Offset: 0x00039870
			public virtual bool Contains(object item)
			{
				bool result;
				lock (this._root)
				{
					result = this._list.Contains(item);
				}
				return result;
			}

			// Token: 0x060016CE RID: 5838 RVA: 0x0003A8B4 File Offset: 0x000398B4
			public virtual void CopyTo(Array array, int index)
			{
				lock (this._root)
				{
					this._list.CopyTo(array, index);
				}
			}

			// Token: 0x060016CF RID: 5839 RVA: 0x0003A8F4 File Offset: 0x000398F4
			public virtual IEnumerator GetEnumerator()
			{
				IEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._list.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x060016D0 RID: 5840 RVA: 0x0003A934 File Offset: 0x00039934
			public virtual int IndexOf(object value)
			{
				int result;
				lock (this._root)
				{
					result = this._list.IndexOf(value);
				}
				return result;
			}

			// Token: 0x060016D1 RID: 5841 RVA: 0x0003A978 File Offset: 0x00039978
			public virtual void Insert(int index, object value)
			{
				lock (this._root)
				{
					this._list.Insert(index, value);
				}
			}

			// Token: 0x060016D2 RID: 5842 RVA: 0x0003A9B8 File Offset: 0x000399B8
			public virtual void Remove(object value)
			{
				lock (this._root)
				{
					this._list.Remove(value);
				}
			}

			// Token: 0x060016D3 RID: 5843 RVA: 0x0003A9F8 File Offset: 0x000399F8
			public virtual void RemoveAt(int index)
			{
				lock (this._root)
				{
					this._list.RemoveAt(index);
				}
			}

			// Token: 0x04000965 RID: 2405
			private IList _list;

			// Token: 0x04000966 RID: 2406
			private object _root;
		}

		// Token: 0x02000251 RID: 593
		[Serializable]
		private class FixedSizeList : IList, ICollection, IEnumerable
		{
			// Token: 0x060016D4 RID: 5844 RVA: 0x0003AA38 File Offset: 0x00039A38
			internal FixedSizeList(IList l)
			{
				this._list = l;
			}

			// Token: 0x1700030D RID: 781
			// (get) Token: 0x060016D5 RID: 5845 RVA: 0x0003AA47 File Offset: 0x00039A47
			public virtual int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x1700030E RID: 782
			// (get) Token: 0x060016D6 RID: 5846 RVA: 0x0003AA54 File Offset: 0x00039A54
			public virtual bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x1700030F RID: 783
			// (get) Token: 0x060016D7 RID: 5847 RVA: 0x0003AA61 File Offset: 0x00039A61
			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000310 RID: 784
			// (get) Token: 0x060016D8 RID: 5848 RVA: 0x0003AA64 File Offset: 0x00039A64
			public virtual bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x17000311 RID: 785
			public virtual object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					this._list[index] = value;
				}
			}

			// Token: 0x17000312 RID: 786
			// (get) Token: 0x060016DB RID: 5851 RVA: 0x0003AA8E File Offset: 0x00039A8E
			public virtual object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x060016DC RID: 5852 RVA: 0x0003AA9B File Offset: 0x00039A9B
			public virtual int Add(object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016DD RID: 5853 RVA: 0x0003AAAC File Offset: 0x00039AAC
			public virtual void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016DE RID: 5854 RVA: 0x0003AABD File Offset: 0x00039ABD
			public virtual bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x060016DF RID: 5855 RVA: 0x0003AACB File Offset: 0x00039ACB
			public virtual void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x060016E0 RID: 5856 RVA: 0x0003AADA File Offset: 0x00039ADA
			public virtual IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060016E1 RID: 5857 RVA: 0x0003AAE7 File Offset: 0x00039AE7
			public virtual int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x060016E2 RID: 5858 RVA: 0x0003AAF5 File Offset: 0x00039AF5
			public virtual void Insert(int index, object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016E3 RID: 5859 RVA: 0x0003AB06 File Offset: 0x00039B06
			public virtual void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016E4 RID: 5860 RVA: 0x0003AB17 File Offset: 0x00039B17
			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x04000967 RID: 2407
			private IList _list;
		}

		// Token: 0x02000252 RID: 594
		[Serializable]
		private class FixedSizeArrayList : ArrayList
		{
			// Token: 0x060016E5 RID: 5861 RVA: 0x0003AB28 File Offset: 0x00039B28
			internal FixedSizeArrayList(ArrayList l)
			{
				this._list = l;
				this._version = this._list._version;
			}

			// Token: 0x17000313 RID: 787
			// (get) Token: 0x060016E6 RID: 5862 RVA: 0x0003AB48 File Offset: 0x00039B48
			public override int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x17000314 RID: 788
			// (get) Token: 0x060016E7 RID: 5863 RVA: 0x0003AB55 File Offset: 0x00039B55
			public override bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x17000315 RID: 789
			// (get) Token: 0x060016E8 RID: 5864 RVA: 0x0003AB62 File Offset: 0x00039B62
			public override bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000316 RID: 790
			// (get) Token: 0x060016E9 RID: 5865 RVA: 0x0003AB65 File Offset: 0x00039B65
			public override bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x17000317 RID: 791
			public override object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					this._list[index] = value;
					this._version = this._list._version;
				}
			}

			// Token: 0x17000318 RID: 792
			// (get) Token: 0x060016EC RID: 5868 RVA: 0x0003ABA0 File Offset: 0x00039BA0
			public override object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x060016ED RID: 5869 RVA: 0x0003ABAD File Offset: 0x00039BAD
			public override int Add(object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016EE RID: 5870 RVA: 0x0003ABBE File Offset: 0x00039BBE
			public override void AddRange(ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016EF RID: 5871 RVA: 0x0003ABCF File Offset: 0x00039BCF
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				return this._list.BinarySearch(index, count, value, comparer);
			}

			// Token: 0x17000319 RID: 793
			// (get) Token: 0x060016F0 RID: 5872 RVA: 0x0003ABE1 File Offset: 0x00039BE1
			// (set) Token: 0x060016F1 RID: 5873 RVA: 0x0003ABEE File Offset: 0x00039BEE
			public override int Capacity
			{
				get
				{
					return this._list.Capacity;
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
				}
			}

			// Token: 0x060016F2 RID: 5874 RVA: 0x0003ABFF File Offset: 0x00039BFF
			public override void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016F3 RID: 5875 RVA: 0x0003AC10 File Offset: 0x00039C10
			public override object Clone()
			{
				return new ArrayList.FixedSizeArrayList(this._list)
				{
					_list = (ArrayList)this._list.Clone()
				};
			}

			// Token: 0x060016F4 RID: 5876 RVA: 0x0003AC40 File Offset: 0x00039C40
			public override bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x060016F5 RID: 5877 RVA: 0x0003AC4E File Offset: 0x00039C4E
			public override void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x060016F6 RID: 5878 RVA: 0x0003AC5D File Offset: 0x00039C5D
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				this._list.CopyTo(index, array, arrayIndex, count);
			}

			// Token: 0x060016F7 RID: 5879 RVA: 0x0003AC6F File Offset: 0x00039C6F
			public override IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060016F8 RID: 5880 RVA: 0x0003AC7C File Offset: 0x00039C7C
			public override IEnumerator GetEnumerator(int index, int count)
			{
				return this._list.GetEnumerator(index, count);
			}

			// Token: 0x060016F9 RID: 5881 RVA: 0x0003AC8B File Offset: 0x00039C8B
			public override int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x060016FA RID: 5882 RVA: 0x0003AC99 File Offset: 0x00039C99
			public override int IndexOf(object value, int startIndex)
			{
				return this._list.IndexOf(value, startIndex);
			}

			// Token: 0x060016FB RID: 5883 RVA: 0x0003ACA8 File Offset: 0x00039CA8
			public override int IndexOf(object value, int startIndex, int count)
			{
				return this._list.IndexOf(value, startIndex, count);
			}

			// Token: 0x060016FC RID: 5884 RVA: 0x0003ACB8 File Offset: 0x00039CB8
			public override void Insert(int index, object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016FD RID: 5885 RVA: 0x0003ACC9 File Offset: 0x00039CC9
			public override void InsertRange(int index, ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016FE RID: 5886 RVA: 0x0003ACDA File Offset: 0x00039CDA
			public override int LastIndexOf(object value)
			{
				return this._list.LastIndexOf(value);
			}

			// Token: 0x060016FF RID: 5887 RVA: 0x0003ACE8 File Offset: 0x00039CE8
			public override int LastIndexOf(object value, int startIndex)
			{
				return this._list.LastIndexOf(value, startIndex);
			}

			// Token: 0x06001700 RID: 5888 RVA: 0x0003ACF7 File Offset: 0x00039CF7
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				return this._list.LastIndexOf(value, startIndex, count);
			}

			// Token: 0x06001701 RID: 5889 RVA: 0x0003AD07 File Offset: 0x00039D07
			public override void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x06001702 RID: 5890 RVA: 0x0003AD18 File Offset: 0x00039D18
			public override void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x06001703 RID: 5891 RVA: 0x0003AD29 File Offset: 0x00039D29
			public override void RemoveRange(int index, int count)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x06001704 RID: 5892 RVA: 0x0003AD3A File Offset: 0x00039D3A
			public override void SetRange(int index, ICollection c)
			{
				this._list.SetRange(index, c);
				this._version = this._list._version;
			}

			// Token: 0x06001705 RID: 5893 RVA: 0x0003AD5C File Offset: 0x00039D5C
			public override ArrayList GetRange(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.Range(this, index, count);
			}

			// Token: 0x06001706 RID: 5894 RVA: 0x0003ADB4 File Offset: 0x00039DB4
			public override void Reverse(int index, int count)
			{
				this._list.Reverse(index, count);
				this._version = this._list._version;
			}

			// Token: 0x06001707 RID: 5895 RVA: 0x0003ADD4 File Offset: 0x00039DD4
			public override void Sort(int index, int count, IComparer comparer)
			{
				this._list.Sort(index, count, comparer);
				this._version = this._list._version;
			}

			// Token: 0x06001708 RID: 5896 RVA: 0x0003ADF5 File Offset: 0x00039DF5
			public override object[] ToArray()
			{
				return this._list.ToArray();
			}

			// Token: 0x06001709 RID: 5897 RVA: 0x0003AE02 File Offset: 0x00039E02
			public override Array ToArray(Type type)
			{
				return this._list.ToArray(type);
			}

			// Token: 0x0600170A RID: 5898 RVA: 0x0003AE10 File Offset: 0x00039E10
			public override void TrimToSize()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x04000968 RID: 2408
			private ArrayList _list;
		}

		// Token: 0x02000253 RID: 595
		[Serializable]
		private class ReadOnlyList : IList, ICollection, IEnumerable
		{
			// Token: 0x0600170B RID: 5899 RVA: 0x0003AE21 File Offset: 0x00039E21
			internal ReadOnlyList(IList l)
			{
				this._list = l;
			}

			// Token: 0x1700031A RID: 794
			// (get) Token: 0x0600170C RID: 5900 RVA: 0x0003AE30 File Offset: 0x00039E30
			public virtual int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x1700031B RID: 795
			// (get) Token: 0x0600170D RID: 5901 RVA: 0x0003AE3D File Offset: 0x00039E3D
			public virtual bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700031C RID: 796
			// (get) Token: 0x0600170E RID: 5902 RVA: 0x0003AE40 File Offset: 0x00039E40
			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700031D RID: 797
			// (get) Token: 0x0600170F RID: 5903 RVA: 0x0003AE43 File Offset: 0x00039E43
			public virtual bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x1700031E RID: 798
			public virtual object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
				}
			}

			// Token: 0x1700031F RID: 799
			// (get) Token: 0x06001712 RID: 5906 RVA: 0x0003AE6F File Offset: 0x00039E6F
			public virtual object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x06001713 RID: 5907 RVA: 0x0003AE7C File Offset: 0x00039E7C
			public virtual int Add(object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x06001714 RID: 5908 RVA: 0x0003AE8D File Offset: 0x00039E8D
			public virtual void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x06001715 RID: 5909 RVA: 0x0003AE9E File Offset: 0x00039E9E
			public virtual bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x06001716 RID: 5910 RVA: 0x0003AEAC File Offset: 0x00039EAC
			public virtual void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x06001717 RID: 5911 RVA: 0x0003AEBB File Offset: 0x00039EBB
			public virtual IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x06001718 RID: 5912 RVA: 0x0003AEC8 File Offset: 0x00039EC8
			public virtual int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x06001719 RID: 5913 RVA: 0x0003AED6 File Offset: 0x00039ED6
			public virtual void Insert(int index, object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0600171A RID: 5914 RVA: 0x0003AEE7 File Offset: 0x00039EE7
			public virtual void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0600171B RID: 5915 RVA: 0x0003AEF8 File Offset: 0x00039EF8
			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x04000969 RID: 2409
			private IList _list;
		}

		// Token: 0x02000254 RID: 596
		[Serializable]
		private class ReadOnlyArrayList : ArrayList
		{
			// Token: 0x0600171C RID: 5916 RVA: 0x0003AF09 File Offset: 0x00039F09
			internal ReadOnlyArrayList(ArrayList l)
			{
				this._list = l;
			}

			// Token: 0x17000320 RID: 800
			// (get) Token: 0x0600171D RID: 5917 RVA: 0x0003AF18 File Offset: 0x00039F18
			public override int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x17000321 RID: 801
			// (get) Token: 0x0600171E RID: 5918 RVA: 0x0003AF25 File Offset: 0x00039F25
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000322 RID: 802
			// (get) Token: 0x0600171F RID: 5919 RVA: 0x0003AF28 File Offset: 0x00039F28
			public override bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000323 RID: 803
			// (get) Token: 0x06001720 RID: 5920 RVA: 0x0003AF2B File Offset: 0x00039F2B
			public override bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x17000324 RID: 804
			public override object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
				}
			}

			// Token: 0x17000325 RID: 805
			// (get) Token: 0x06001723 RID: 5923 RVA: 0x0003AF57 File Offset: 0x00039F57
			public override object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x06001724 RID: 5924 RVA: 0x0003AF64 File Offset: 0x00039F64
			public override int Add(object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x06001725 RID: 5925 RVA: 0x0003AF75 File Offset: 0x00039F75
			public override void AddRange(ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x06001726 RID: 5926 RVA: 0x0003AF86 File Offset: 0x00039F86
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				return this._list.BinarySearch(index, count, value, comparer);
			}

			// Token: 0x17000326 RID: 806
			// (get) Token: 0x06001727 RID: 5927 RVA: 0x0003AF98 File Offset: 0x00039F98
			// (set) Token: 0x06001728 RID: 5928 RVA: 0x0003AFA5 File Offset: 0x00039FA5
			public override int Capacity
			{
				get
				{
					return this._list.Capacity;
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
				}
			}

			// Token: 0x06001729 RID: 5929 RVA: 0x0003AFB6 File Offset: 0x00039FB6
			public override void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0600172A RID: 5930 RVA: 0x0003AFC8 File Offset: 0x00039FC8
			public override object Clone()
			{
				return new ArrayList.ReadOnlyArrayList(this._list)
				{
					_list = (ArrayList)this._list.Clone()
				};
			}

			// Token: 0x0600172B RID: 5931 RVA: 0x0003AFF8 File Offset: 0x00039FF8
			public override bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x0600172C RID: 5932 RVA: 0x0003B006 File Offset: 0x0003A006
			public override void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x0600172D RID: 5933 RVA: 0x0003B015 File Offset: 0x0003A015
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				this._list.CopyTo(index, array, arrayIndex, count);
			}

			// Token: 0x0600172E RID: 5934 RVA: 0x0003B027 File Offset: 0x0003A027
			public override IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x0600172F RID: 5935 RVA: 0x0003B034 File Offset: 0x0003A034
			public override IEnumerator GetEnumerator(int index, int count)
			{
				return this._list.GetEnumerator(index, count);
			}

			// Token: 0x06001730 RID: 5936 RVA: 0x0003B043 File Offset: 0x0003A043
			public override int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x06001731 RID: 5937 RVA: 0x0003B051 File Offset: 0x0003A051
			public override int IndexOf(object value, int startIndex)
			{
				return this._list.IndexOf(value, startIndex);
			}

			// Token: 0x06001732 RID: 5938 RVA: 0x0003B060 File Offset: 0x0003A060
			public override int IndexOf(object value, int startIndex, int count)
			{
				return this._list.IndexOf(value, startIndex, count);
			}

			// Token: 0x06001733 RID: 5939 RVA: 0x0003B070 File Offset: 0x0003A070
			public override void Insert(int index, object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x06001734 RID: 5940 RVA: 0x0003B081 File Offset: 0x0003A081
			public override void InsertRange(int index, ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x06001735 RID: 5941 RVA: 0x0003B092 File Offset: 0x0003A092
			public override int LastIndexOf(object value)
			{
				return this._list.LastIndexOf(value);
			}

			// Token: 0x06001736 RID: 5942 RVA: 0x0003B0A0 File Offset: 0x0003A0A0
			public override int LastIndexOf(object value, int startIndex)
			{
				return this._list.LastIndexOf(value, startIndex);
			}

			// Token: 0x06001737 RID: 5943 RVA: 0x0003B0AF File Offset: 0x0003A0AF
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				return this._list.LastIndexOf(value, startIndex, count);
			}

			// Token: 0x06001738 RID: 5944 RVA: 0x0003B0BF File Offset: 0x0003A0BF
			public override void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x06001739 RID: 5945 RVA: 0x0003B0D0 File Offset: 0x0003A0D0
			public override void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0600173A RID: 5946 RVA: 0x0003B0E1 File Offset: 0x0003A0E1
			public override void RemoveRange(int index, int count)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0600173B RID: 5947 RVA: 0x0003B0F2 File Offset: 0x0003A0F2
			public override void SetRange(int index, ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0600173C RID: 5948 RVA: 0x0003B104 File Offset: 0x0003A104
			public override ArrayList GetRange(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.Range(this, index, count);
			}

			// Token: 0x0600173D RID: 5949 RVA: 0x0003B15C File Offset: 0x0003A15C
			public override void Reverse(int index, int count)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0600173E RID: 5950 RVA: 0x0003B16D File Offset: 0x0003A16D
			public override void Sort(int index, int count, IComparer comparer)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0600173F RID: 5951 RVA: 0x0003B17E File Offset: 0x0003A17E
			public override object[] ToArray()
			{
				return this._list.ToArray();
			}

			// Token: 0x06001740 RID: 5952 RVA: 0x0003B18B File Offset: 0x0003A18B
			public override Array ToArray(Type type)
			{
				return this._list.ToArray(type);
			}

			// Token: 0x06001741 RID: 5953 RVA: 0x0003B199 File Offset: 0x0003A199
			public override void TrimToSize()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x0400096A RID: 2410
			private ArrayList _list;
		}

		// Token: 0x02000255 RID: 597
		[Serializable]
		private sealed class ArrayListEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x06001742 RID: 5954 RVA: 0x0003B1AA File Offset: 0x0003A1AA
			internal ArrayListEnumerator(ArrayList list, int index, int count)
			{
				this.list = list;
				this.startIndex = index;
				this.index = index - 1;
				this.endIndex = this.index + count;
				this.version = list._version;
				this.currentElement = null;
			}

			// Token: 0x06001743 RID: 5955 RVA: 0x0003B1EA File Offset: 0x0003A1EA
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x06001744 RID: 5956 RVA: 0x0003B1F4 File Offset: 0x0003A1F4
			public bool MoveNext()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.index < this.endIndex)
				{
					this.currentElement = this.list[++this.index];
					return true;
				}
				this.index = this.endIndex + 1;
				return false;
			}

			// Token: 0x17000327 RID: 807
			// (get) Token: 0x06001745 RID: 5957 RVA: 0x0003B268 File Offset: 0x0003A268
			public object Current
			{
				get
				{
					if (this.index < this.startIndex)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this.index > this.endIndex)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					return this.currentElement;
				}
			}

			// Token: 0x06001746 RID: 5958 RVA: 0x0003B2B7 File Offset: 0x0003A2B7
			public void Reset()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.index = this.startIndex - 1;
			}

			// Token: 0x0400096B RID: 2411
			private ArrayList list;

			// Token: 0x0400096C RID: 2412
			private int index;

			// Token: 0x0400096D RID: 2413
			private int endIndex;

			// Token: 0x0400096E RID: 2414
			private int version;

			// Token: 0x0400096F RID: 2415
			private object currentElement;

			// Token: 0x04000970 RID: 2416
			private int startIndex;
		}

		// Token: 0x02000256 RID: 598
		[Serializable]
		private class Range : ArrayList
		{
			// Token: 0x06001747 RID: 5959 RVA: 0x0003B2EA File Offset: 0x0003A2EA
			internal Range(ArrayList list, int index, int count) : base(false)
			{
				this._baseList = list;
				this._baseIndex = index;
				this._baseSize = count;
				this._baseVersion = list._version;
				this._version = list._version;
			}

			// Token: 0x06001748 RID: 5960 RVA: 0x0003B320 File Offset: 0x0003A320
			private void InternalUpdateRange()
			{
				if (this._baseVersion != this._baseList._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnderlyingArrayListChanged"));
				}
			}

			// Token: 0x06001749 RID: 5961 RVA: 0x0003B345 File Offset: 0x0003A345
			private void InternalUpdateVersion()
			{
				this._baseVersion++;
				this._version++;
			}

			// Token: 0x0600174A RID: 5962 RVA: 0x0003B364 File Offset: 0x0003A364
			public override int Add(object value)
			{
				this.InternalUpdateRange();
				this._baseList.Insert(this._baseIndex + this._baseSize, value);
				this.InternalUpdateVersion();
				return this._baseSize++;
			}

			// Token: 0x0600174B RID: 5963 RVA: 0x0003B3A8 File Offset: 0x0003A3A8
			public override void AddRange(ICollection c)
			{
				this.InternalUpdateRange();
				if (c == null)
				{
					throw new ArgumentNullException("c");
				}
				int count = c.Count;
				if (count > 0)
				{
					this._baseList.InsertRange(this._baseIndex + this._baseSize, c);
					this.InternalUpdateVersion();
					this._baseSize += count;
				}
			}

			// Token: 0x0600174C RID: 5964 RVA: 0x0003B404 File Offset: 0x0003A404
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				int num = this._baseList.BinarySearch(this._baseIndex + index, count, value, comparer);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return num + this._baseIndex;
			}

			// Token: 0x17000328 RID: 808
			// (get) Token: 0x0600174D RID: 5965 RVA: 0x0003B487 File Offset: 0x0003A487
			// (set) Token: 0x0600174E RID: 5966 RVA: 0x0003B494 File Offset: 0x0003A494
			public override int Capacity
			{
				get
				{
					return this._baseList.Capacity;
				}
				set
				{
					if (value < this.Count)
					{
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
				}
			}

			// Token: 0x0600174F RID: 5967 RVA: 0x0003B4B4 File Offset: 0x0003A4B4
			public override void Clear()
			{
				this.InternalUpdateRange();
				if (this._baseSize != 0)
				{
					this._baseList.RemoveRange(this._baseIndex, this._baseSize);
					this.InternalUpdateVersion();
					this._baseSize = 0;
				}
			}

			// Token: 0x06001750 RID: 5968 RVA: 0x0003B4E8 File Offset: 0x0003A4E8
			public override object Clone()
			{
				this.InternalUpdateRange();
				return new ArrayList.Range(this._baseList, this._baseIndex, this._baseSize)
				{
					_baseList = (ArrayList)this._baseList.Clone()
				};
			}

			// Token: 0x06001751 RID: 5969 RVA: 0x0003B52C File Offset: 0x0003A52C
			public override bool Contains(object item)
			{
				this.InternalUpdateRange();
				if (item == null)
				{
					for (int i = 0; i < this._baseSize; i++)
					{
						if (this._baseList[this._baseIndex + i] == null)
						{
							return true;
						}
					}
					return false;
				}
				for (int j = 0; j < this._baseSize; j++)
				{
					if (this._baseList[this._baseIndex + j] != null && this._baseList[this._baseIndex + j].Equals(item))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06001752 RID: 5970 RVA: 0x0003B5B0 File Offset: 0x0003A5B0
			public override void CopyTo(Array array, int index)
			{
				this.InternalUpdateRange();
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - index < this._baseSize)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				this._baseList.CopyTo(this._baseIndex, array, index, this._baseSize);
			}

			// Token: 0x06001753 RID: 5971 RVA: 0x0003B63C File Offset: 0x0003A63C
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				this.InternalUpdateRange();
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				this._baseList.CopyTo(this._baseIndex + index, array, arrayIndex, count);
			}

			// Token: 0x17000329 RID: 809
			// (get) Token: 0x06001754 RID: 5972 RVA: 0x0003B6EE File Offset: 0x0003A6EE
			public override int Count
			{
				get
				{
					this.InternalUpdateRange();
					return this._baseSize;
				}
			}

			// Token: 0x1700032A RID: 810
			// (get) Token: 0x06001755 RID: 5973 RVA: 0x0003B6FC File Offset: 0x0003A6FC
			public override bool IsReadOnly
			{
				get
				{
					return this._baseList.IsReadOnly;
				}
			}

			// Token: 0x1700032B RID: 811
			// (get) Token: 0x06001756 RID: 5974 RVA: 0x0003B709 File Offset: 0x0003A709
			public override bool IsFixedSize
			{
				get
				{
					return this._baseList.IsFixedSize;
				}
			}

			// Token: 0x1700032C RID: 812
			// (get) Token: 0x06001757 RID: 5975 RVA: 0x0003B716 File Offset: 0x0003A716
			public override bool IsSynchronized
			{
				get
				{
					return this._baseList.IsSynchronized;
				}
			}

			// Token: 0x06001758 RID: 5976 RVA: 0x0003B723 File Offset: 0x0003A723
			public override IEnumerator GetEnumerator()
			{
				return this.GetEnumerator(0, this._baseSize);
			}

			// Token: 0x06001759 RID: 5977 RVA: 0x0003B734 File Offset: 0x0003A734
			public override IEnumerator GetEnumerator(int index, int count)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return this._baseList.GetEnumerator(this._baseIndex + index, count);
			}

			// Token: 0x0600175A RID: 5978 RVA: 0x0003B7A0 File Offset: 0x0003A7A0
			public override ArrayList GetRange(int index, int count)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.Range(this, index, count);
			}

			// Token: 0x1700032D RID: 813
			// (get) Token: 0x0600175B RID: 5979 RVA: 0x0003B7FE File Offset: 0x0003A7FE
			public override object SyncRoot
			{
				get
				{
					return this._baseList.SyncRoot;
				}
			}

			// Token: 0x0600175C RID: 5980 RVA: 0x0003B80C File Offset: 0x0003A80C
			public override int IndexOf(object value)
			{
				this.InternalUpdateRange();
				int num = this._baseList.IndexOf(value, this._baseIndex, this._baseSize);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x0600175D RID: 5981 RVA: 0x0003B848 File Offset: 0x0003A848
			public override int IndexOf(object value, int startIndex)
			{
				this.InternalUpdateRange();
				if (startIndex < 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (startIndex > this._baseSize)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				int num = this._baseList.IndexOf(value, this._baseIndex + startIndex, this._baseSize - startIndex);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x0600175E RID: 5982 RVA: 0x0003B8C0 File Offset: 0x0003A8C0
			public override int IndexOf(object value, int startIndex, int count)
			{
				this.InternalUpdateRange();
				if (startIndex < 0 || startIndex > this._baseSize)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (count < 0 || startIndex > this._baseSize - count)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				int num = this._baseList.IndexOf(value, this._baseIndex + startIndex, count);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x0600175F RID: 5983 RVA: 0x0003B940 File Offset: 0x0003A940
			public override void Insert(int index, object value)
			{
				this.InternalUpdateRange();
				if (index < 0 || index > this._baseSize)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this._baseList.Insert(this._baseIndex + index, value);
				this.InternalUpdateVersion();
				this._baseSize++;
			}

			// Token: 0x06001760 RID: 5984 RVA: 0x0003B9A0 File Offset: 0x0003A9A0
			public override void InsertRange(int index, ICollection c)
			{
				this.InternalUpdateRange();
				if (index < 0 || index > this._baseSize)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (c == null)
				{
					throw new ArgumentNullException("c");
				}
				int count = c.Count;
				if (count > 0)
				{
					this._baseList.InsertRange(this._baseIndex + index, c);
					this._baseSize += count;
					this.InternalUpdateVersion();
				}
			}

			// Token: 0x06001761 RID: 5985 RVA: 0x0003BA18 File Offset: 0x0003AA18
			public override int LastIndexOf(object value)
			{
				this.InternalUpdateRange();
				int num = this._baseList.LastIndexOf(value, this._baseIndex + this._baseSize - 1, this._baseSize);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x06001762 RID: 5986 RVA: 0x0003BA5B File Offset: 0x0003AA5B
			public override int LastIndexOf(object value, int startIndex)
			{
				return this.LastIndexOf(value, startIndex, startIndex + 1);
			}

			// Token: 0x06001763 RID: 5987 RVA: 0x0003BA68 File Offset: 0x0003AA68
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				this.InternalUpdateRange();
				if (this._baseSize == 0)
				{
					return -1;
				}
				if (startIndex >= this._baseSize)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (startIndex < 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				int num = this._baseList.LastIndexOf(value, this._baseIndex + startIndex, count);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x06001764 RID: 5988 RVA: 0x0003BAE0 File Offset: 0x0003AAE0
			public override void RemoveAt(int index)
			{
				this.InternalUpdateRange();
				if (index < 0 || index >= this._baseSize)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this._baseList.RemoveAt(this._baseIndex + index);
				this.InternalUpdateVersion();
				this._baseSize--;
			}

			// Token: 0x06001765 RID: 5989 RVA: 0x0003BB3C File Offset: 0x0003AB3C
			public override void RemoveRange(int index, int count)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (count > 0)
				{
					this._baseList.RemoveRange(this._baseIndex + index, count);
					this.InternalUpdateVersion();
					this._baseSize -= count;
				}
			}

			// Token: 0x06001766 RID: 5990 RVA: 0x0003BBC0 File Offset: 0x0003ABC0
			public override void Reverse(int index, int count)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				this._baseList.Reverse(this._baseIndex + index, count);
				this.InternalUpdateVersion();
			}

			// Token: 0x06001767 RID: 5991 RVA: 0x0003BC30 File Offset: 0x0003AC30
			public override void SetRange(int index, ICollection c)
			{
				this.InternalUpdateRange();
				if (index < 0 || index >= this._baseSize)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this._baseList.SetRange(this._baseIndex + index, c);
				if (c.Count > 0)
				{
					this.InternalUpdateVersion();
				}
			}

			// Token: 0x06001768 RID: 5992 RVA: 0x0003BC88 File Offset: 0x0003AC88
			public override void Sort(int index, int count, IComparer comparer)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				this._baseList.Sort(this._baseIndex + index, count, comparer);
				this.InternalUpdateVersion();
			}

			// Token: 0x1700032E RID: 814
			public override object this[int index]
			{
				get
				{
					this.InternalUpdateRange();
					if (index < 0 || index >= this._baseSize)
					{
						throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
					}
					return this._baseList[this._baseIndex + index];
				}
				set
				{
					this.InternalUpdateRange();
					if (index < 0 || index >= this._baseSize)
					{
						throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
					}
					this._baseList[this._baseIndex + index] = value;
					this.InternalUpdateVersion();
				}
			}

			// Token: 0x0600176B RID: 5995 RVA: 0x0003BD88 File Offset: 0x0003AD88
			public override object[] ToArray()
			{
				this.InternalUpdateRange();
				object[] array = new object[this._baseSize];
				Array.Copy(this._baseList._items, this._baseIndex, array, 0, this._baseSize);
				return array;
			}

			// Token: 0x0600176C RID: 5996 RVA: 0x0003BDC8 File Offset: 0x0003ADC8
			public override Array ToArray(Type type)
			{
				this.InternalUpdateRange();
				if (type == null)
				{
					throw new ArgumentNullException("type");
				}
				Array array = Array.CreateInstance(type, this._baseSize);
				this._baseList.CopyTo(this._baseIndex, array, 0, this._baseSize);
				return array;
			}

			// Token: 0x0600176D RID: 5997 RVA: 0x0003BE10 File Offset: 0x0003AE10
			public override void TrimToSize()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_RangeCollection"));
			}

			// Token: 0x04000971 RID: 2417
			private ArrayList _baseList;

			// Token: 0x04000972 RID: 2418
			private int _baseIndex;

			// Token: 0x04000973 RID: 2419
			private int _baseSize;

			// Token: 0x04000974 RID: 2420
			private int _baseVersion;
		}

		// Token: 0x02000257 RID: 599
		[Serializable]
		private sealed class ArrayListEnumeratorSimple : IEnumerator, ICloneable
		{
			// Token: 0x0600176E RID: 5998 RVA: 0x0003BE24 File Offset: 0x0003AE24
			internal ArrayListEnumeratorSimple(ArrayList list)
			{
				this.list = list;
				this.index = -1;
				this.version = list._version;
				this.isArrayList = (list.GetType() == typeof(ArrayList));
				this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
			}

			// Token: 0x0600176F RID: 5999 RVA: 0x0003BE74 File Offset: 0x0003AE74
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x06001770 RID: 6000 RVA: 0x0003BE7C File Offset: 0x0003AE7C
			public bool MoveNext()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.isArrayList)
				{
					if (this.index < this.list._size - 1)
					{
						this.currentElement = this.list._items[++this.index];
						return true;
					}
					this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
					this.index = this.list._size;
					return false;
				}
				else
				{
					if (this.index < this.list.Count - 1)
					{
						this.currentElement = this.list[++this.index];
						return true;
					}
					this.index = this.list.Count;
					this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
					return false;
				}
			}

			// Token: 0x1700032F RID: 815
			// (get) Token: 0x06001771 RID: 6001 RVA: 0x0003BF64 File Offset: 0x0003AF64
			public object Current
			{
				get
				{
					object obj = this.currentElement;
					if (ArrayList.ArrayListEnumeratorSimple.dummyObject != obj)
					{
						return obj;
					}
					if (this.index == -1)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
			}

			// Token: 0x06001772 RID: 6002 RVA: 0x0003BFAA File Offset: 0x0003AFAA
			public void Reset()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
				this.index = -1;
			}

			// Token: 0x04000975 RID: 2421
			private ArrayList list;

			// Token: 0x04000976 RID: 2422
			private int index;

			// Token: 0x04000977 RID: 2423
			private int version;

			// Token: 0x04000978 RID: 2424
			private object currentElement;

			// Token: 0x04000979 RID: 2425
			[NonSerialized]
			private bool isArrayList;

			// Token: 0x0400097A RID: 2426
			private static object dummyObject = new object();
		}

		// Token: 0x02000258 RID: 600
		internal class ArrayListDebugView
		{
			// Token: 0x06001774 RID: 6004 RVA: 0x0003BFED File Offset: 0x0003AFED
			public ArrayListDebugView(ArrayList arrayList)
			{
				if (arrayList == null)
				{
					throw new ArgumentNullException("arrayList");
				}
				this.arrayList = arrayList;
			}

			// Token: 0x17000330 RID: 816
			// (get) Token: 0x06001775 RID: 6005 RVA: 0x0003C00A File Offset: 0x0003B00A
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object[] Items
			{
				get
				{
					return this.arrayList.ToArray();
				}
			}

			// Token: 0x0400097B RID: 2427
			private ArrayList arrayList;
		}
	}
}
