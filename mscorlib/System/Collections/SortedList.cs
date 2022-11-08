using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x0200027A RID: 634
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(SortedList.SortedListDebugView))]
	[ComVisible(true)]
	[Serializable]
	public class SortedList : IDictionary, ICollection, IEnumerable, ICloneable
	{
		// Token: 0x060018D1 RID: 6353 RVA: 0x0004065A File Offset: 0x0003F65A
		public SortedList()
		{
			this.keys = SortedList.emptyArray;
			this.values = SortedList.emptyArray;
			this._size = 0;
			this.comparer = new Comparer(CultureInfo.CurrentCulture);
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00040690 File Offset: 0x0003F690
		public SortedList(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.keys = new object[initialCapacity];
			this.values = new object[initialCapacity];
			this.comparer = new Comparer(CultureInfo.CurrentCulture);
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x000406E4 File Offset: 0x0003F6E4
		public SortedList(IComparer comparer) : this()
		{
			if (comparer != null)
			{
				this.comparer = comparer;
			}
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x000406F6 File Offset: 0x0003F6F6
		public SortedList(IComparer comparer, int capacity) : this(comparer)
		{
			this.Capacity = capacity;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00040706 File Offset: 0x0003F706
		public SortedList(IDictionary d) : this(d, null)
		{
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00040710 File Offset: 0x0003F710
		public SortedList(IDictionary d, IComparer comparer) : this(comparer, (d != null) ? d.Count : 0)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d", Environment.GetResourceString("ArgumentNull_Dictionary"));
			}
			d.Keys.CopyTo(this.keys, 0);
			d.Values.CopyTo(this.values, 0);
			Array.Sort(this.keys, this.values, comparer);
			this._size = d.Count;
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0004078C File Offset: 0x0003F78C
		public virtual void Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			int num = Array.BinarySearch(this.keys, 0, this._size, key, this.comparer);
			if (num >= 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_AddingDuplicate__", new object[]
				{
					this.GetKey(num),
					key
				}));
			}
			this.Insert(~num, key, value);
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x000407FF File Offset: 0x0003F7FF
		// (set) Token: 0x060018D9 RID: 6361 RVA: 0x0004080C File Offset: 0x0003F80C
		public virtual int Capacity
		{
			get
			{
				return this.keys.Length;
			}
			set
			{
				if (value != this.keys.Length)
				{
					if (value < this._size)
					{
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
					if (value > 0)
					{
						object[] destinationArray = new object[value];
						object[] destinationArray2 = new object[value];
						if (this._size > 0)
						{
							Array.Copy(this.keys, 0, destinationArray, 0, this._size);
							Array.Copy(this.values, 0, destinationArray2, 0, this._size);
						}
						this.keys = destinationArray;
						this.values = destinationArray2;
						return;
					}
					this.keys = SortedList.emptyArray;
					this.values = SortedList.emptyArray;
				}
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x000408AD File Offset: 0x0003F8AD
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x000408B5 File Offset: 0x0003F8B5
		public virtual ICollection Keys
		{
			get
			{
				return this.GetKeyList();
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x000408BD File Offset: 0x0003F8BD
		public virtual ICollection Values
		{
			get
			{
				return this.GetValueList();
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x000408C5 File Offset: 0x0003F8C5
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x000408C8 File Offset: 0x0003F8C8
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x000408CB File Offset: 0x0003F8CB
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x000408CE File Offset: 0x0003F8CE
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

		// Token: 0x060018E1 RID: 6369 RVA: 0x000408F0 File Offset: 0x0003F8F0
		public virtual void Clear()
		{
			this.version++;
			Array.Clear(this.keys, 0, this._size);
			Array.Clear(this.values, 0, this._size);
			this._size = 0;
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0004092C File Offset: 0x0003F92C
		public virtual object Clone()
		{
			SortedList sortedList = new SortedList(this._size);
			Array.Copy(this.keys, 0, sortedList.keys, 0, this._size);
			Array.Copy(this.values, 0, sortedList.values, 0, this._size);
			sortedList._size = this._size;
			sortedList.version = this.version;
			sortedList.comparer = this.comparer;
			return sortedList;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0004099C File Offset: 0x0003F99C
		public virtual bool Contains(object key)
		{
			return this.IndexOfKey(key) >= 0;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x000409AB File Offset: 0x0003F9AB
		public virtual bool ContainsKey(object key)
		{
			return this.IndexOfKey(key) >= 0;
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x000409BA File Offset: 0x0003F9BA
		public virtual bool ContainsValue(object value)
		{
			return this.IndexOfValue(value) >= 0;
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x000409CC File Offset: 0x0003F9CC
		public virtual void CopyTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - arrayIndex < this.Count)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				DictionaryEntry dictionaryEntry = new DictionaryEntry(this.keys[i], this.values[i]);
				array.SetValue(dictionaryEntry, i + arrayIndex);
			}
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00040A7C File Offset: 0x0003FA7C
		internal virtual KeyValuePairs[] ToKeyValuePairsArray()
		{
			KeyValuePairs[] array = new KeyValuePairs[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				array[i] = new KeyValuePairs(this.keys[i], this.values[i]);
			}
			return array;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00040AC0 File Offset: 0x0003FAC0
		private void EnsureCapacity(int min)
		{
			int num = (this.keys.Length == 0) ? 16 : (this.keys.Length * 2);
			if (num < min)
			{
				num = min;
			}
			this.Capacity = num;
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x00040AF3 File Offset: 0x0003FAF3
		public virtual object GetByIndex(int index)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return this.values[index];
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00040B1F File Offset: 0x0003FB1F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedList.SortedListEnumerator(this, 0, this._size, 3);
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x00040B2F File Offset: 0x0003FB2F
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new SortedList.SortedListEnumerator(this, 0, this._size, 3);
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x00040B3F File Offset: 0x0003FB3F
		public virtual object GetKey(int index)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return this.keys[index];
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00040B6B File Offset: 0x0003FB6B
		public virtual IList GetKeyList()
		{
			if (this.keyList == null)
			{
				this.keyList = new SortedList.KeyList(this);
			}
			return this.keyList;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00040B87 File Offset: 0x0003FB87
		public virtual IList GetValueList()
		{
			if (this.valueList == null)
			{
				this.valueList = new SortedList.ValueList(this);
			}
			return this.valueList;
		}

		// Token: 0x170003AB RID: 939
		public virtual object this[object key]
		{
			get
			{
				int num = this.IndexOfKey(key);
				if (num >= 0)
				{
					return this.values[num];
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				int num = Array.BinarySearch(this.keys, 0, this._size, key, this.comparer);
				if (num >= 0)
				{
					this.values[num] = value;
					this.version++;
					return;
				}
				this.Insert(~num, key, value);
			}
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00040C30 File Offset: 0x0003FC30
		public virtual int IndexOfKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			int num = Array.BinarySearch(this.keys, 0, this._size, key, this.comparer);
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x00040C76 File Offset: 0x0003FC76
		public virtual int IndexOfValue(object value)
		{
			return Array.IndexOf<object>(this.values, value, 0, this._size);
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x00040C8C File Offset: 0x0003FC8C
		private void Insert(int index, object key, object value)
		{
			if (this._size == this.keys.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this.keys, index, this.keys, index + 1, this._size - index);
				Array.Copy(this.values, index, this.values, index + 1, this._size - index);
			}
			this.keys[index] = key;
			this.values[index] = value;
			this._size++;
			this.version++;
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00040D28 File Offset: 0x0003FD28
		public virtual void RemoveAt(int index)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this.keys, index + 1, this.keys, index, this._size - index);
				Array.Copy(this.values, index + 1, this.values, index, this._size - index);
			}
			this.keys[this._size] = null;
			this.values[this._size] = null;
			this.version++;
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00040DD4 File Offset: 0x0003FDD4
		public virtual void Remove(object key)
		{
			int num = this.IndexOfKey(key);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x00040DF4 File Offset: 0x0003FDF4
		public virtual void SetByIndex(int index, object value)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			this.values[index] = value;
			this.version++;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x00040E2F File Offset: 0x0003FE2F
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static SortedList Synchronized(SortedList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new SortedList.SyncSortedList(list);
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00040E45 File Offset: 0x0003FE45
		public virtual void TrimToSize()
		{
			this.Capacity = this._size;
		}

		// Token: 0x040009DE RID: 2526
		private const int _defaultCapacity = 16;

		// Token: 0x040009DF RID: 2527
		private object[] keys;

		// Token: 0x040009E0 RID: 2528
		private object[] values;

		// Token: 0x040009E1 RID: 2529
		private int _size;

		// Token: 0x040009E2 RID: 2530
		private int version;

		// Token: 0x040009E3 RID: 2531
		private IComparer comparer;

		// Token: 0x040009E4 RID: 2532
		private SortedList.KeyList keyList;

		// Token: 0x040009E5 RID: 2533
		private SortedList.ValueList valueList;

		// Token: 0x040009E6 RID: 2534
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x040009E7 RID: 2535
		private static object[] emptyArray = new object[0];

		// Token: 0x0200027B RID: 635
		[Serializable]
		private class SyncSortedList : SortedList
		{
			// Token: 0x060018FA RID: 6394 RVA: 0x00040E60 File Offset: 0x0003FE60
			internal SyncSortedList(SortedList list)
			{
				this._list = list;
				this._root = list.SyncRoot;
			}

			// Token: 0x170003AC RID: 940
			// (get) Token: 0x060018FB RID: 6395 RVA: 0x00040E7C File Offset: 0x0003FE7C
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

			// Token: 0x170003AD RID: 941
			// (get) Token: 0x060018FC RID: 6396 RVA: 0x00040EBC File Offset: 0x0003FEBC
			public override object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x170003AE RID: 942
			// (get) Token: 0x060018FD RID: 6397 RVA: 0x00040EC4 File Offset: 0x0003FEC4
			public override bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x170003AF RID: 943
			// (get) Token: 0x060018FE RID: 6398 RVA: 0x00040ED1 File Offset: 0x0003FED1
			public override bool IsFixedSize
			{
				get
				{
					return this._list.IsFixedSize;
				}
			}

			// Token: 0x170003B0 RID: 944
			// (get) Token: 0x060018FF RID: 6399 RVA: 0x00040EDE File Offset: 0x0003FEDE
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003B1 RID: 945
			public override object this[object key]
			{
				get
				{
					object result;
					lock (this._root)
					{
						result = this._list[key];
					}
					return result;
				}
				set
				{
					lock (this._root)
					{
						this._list[key] = value;
					}
				}
			}

			// Token: 0x06001902 RID: 6402 RVA: 0x00040F68 File Offset: 0x0003FF68
			public override void Add(object key, object value)
			{
				lock (this._root)
				{
					this._list.Add(key, value);
				}
			}

			// Token: 0x170003B2 RID: 946
			// (get) Token: 0x06001903 RID: 6403 RVA: 0x00040FA8 File Offset: 0x0003FFA8
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
			}

			// Token: 0x06001904 RID: 6404 RVA: 0x00040FE8 File Offset: 0x0003FFE8
			public override void Clear()
			{
				lock (this._root)
				{
					this._list.Clear();
				}
			}

			// Token: 0x06001905 RID: 6405 RVA: 0x00041028 File Offset: 0x00040028
			public override object Clone()
			{
				object result;
				lock (this._root)
				{
					result = this._list.Clone();
				}
				return result;
			}

			// Token: 0x06001906 RID: 6406 RVA: 0x00041068 File Offset: 0x00040068
			public override bool Contains(object key)
			{
				bool result;
				lock (this._root)
				{
					result = this._list.Contains(key);
				}
				return result;
			}

			// Token: 0x06001907 RID: 6407 RVA: 0x000410AC File Offset: 0x000400AC
			public override bool ContainsKey(object key)
			{
				bool result;
				lock (this._root)
				{
					result = this._list.ContainsKey(key);
				}
				return result;
			}

			// Token: 0x06001908 RID: 6408 RVA: 0x000410F0 File Offset: 0x000400F0
			public override bool ContainsValue(object key)
			{
				bool result;
				lock (this._root)
				{
					result = this._list.ContainsValue(key);
				}
				return result;
			}

			// Token: 0x06001909 RID: 6409 RVA: 0x00041134 File Offset: 0x00040134
			public override void CopyTo(Array array, int index)
			{
				lock (this._root)
				{
					this._list.CopyTo(array, index);
				}
			}

			// Token: 0x0600190A RID: 6410 RVA: 0x00041174 File Offset: 0x00040174
			public override object GetByIndex(int index)
			{
				object byIndex;
				lock (this._root)
				{
					byIndex = this._list.GetByIndex(index);
				}
				return byIndex;
			}

			// Token: 0x0600190B RID: 6411 RVA: 0x000411B8 File Offset: 0x000401B8
			public override IDictionaryEnumerator GetEnumerator()
			{
				IDictionaryEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._list.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x0600190C RID: 6412 RVA: 0x000411F8 File Offset: 0x000401F8
			public override object GetKey(int index)
			{
				object key;
				lock (this._root)
				{
					key = this._list.GetKey(index);
				}
				return key;
			}

			// Token: 0x0600190D RID: 6413 RVA: 0x0004123C File Offset: 0x0004023C
			public override IList GetKeyList()
			{
				IList keyList;
				lock (this._root)
				{
					keyList = this._list.GetKeyList();
				}
				return keyList;
			}

			// Token: 0x0600190E RID: 6414 RVA: 0x0004127C File Offset: 0x0004027C
			public override IList GetValueList()
			{
				IList valueList;
				lock (this._root)
				{
					valueList = this._list.GetValueList();
				}
				return valueList;
			}

			// Token: 0x0600190F RID: 6415 RVA: 0x000412BC File Offset: 0x000402BC
			public override int IndexOfKey(object key)
			{
				int result;
				lock (this._root)
				{
					result = this._list.IndexOfKey(key);
				}
				return result;
			}

			// Token: 0x06001910 RID: 6416 RVA: 0x00041300 File Offset: 0x00040300
			public override int IndexOfValue(object value)
			{
				int result;
				lock (this._root)
				{
					result = this._list.IndexOfValue(value);
				}
				return result;
			}

			// Token: 0x06001911 RID: 6417 RVA: 0x00041344 File Offset: 0x00040344
			public override void RemoveAt(int index)
			{
				lock (this._root)
				{
					this._list.RemoveAt(index);
				}
			}

			// Token: 0x06001912 RID: 6418 RVA: 0x00041384 File Offset: 0x00040384
			public override void Remove(object key)
			{
				lock (this._root)
				{
					this._list.Remove(key);
				}
			}

			// Token: 0x06001913 RID: 6419 RVA: 0x000413C4 File Offset: 0x000403C4
			public override void SetByIndex(int index, object value)
			{
				lock (this._root)
				{
					this._list.SetByIndex(index, value);
				}
			}

			// Token: 0x06001914 RID: 6420 RVA: 0x00041404 File Offset: 0x00040404
			internal override KeyValuePairs[] ToKeyValuePairsArray()
			{
				return this._list.ToKeyValuePairsArray();
			}

			// Token: 0x06001915 RID: 6421 RVA: 0x00041414 File Offset: 0x00040414
			public override void TrimToSize()
			{
				lock (this._root)
				{
					this._list.TrimToSize();
				}
			}

			// Token: 0x040009E8 RID: 2536
			private SortedList _list;

			// Token: 0x040009E9 RID: 2537
			private object _root;
		}

		// Token: 0x0200027C RID: 636
		[Serializable]
		private class SortedListEnumerator : IDictionaryEnumerator, IEnumerator, ICloneable
		{
			// Token: 0x06001916 RID: 6422 RVA: 0x00041454 File Offset: 0x00040454
			internal SortedListEnumerator(SortedList sortedList, int index, int count, int getObjRetType)
			{
				this.sortedList = sortedList;
				this.index = index;
				this.startIndex = index;
				this.endIndex = index + count;
				this.version = sortedList.version;
				this.getObjectRetType = getObjRetType;
				this.current = false;
			}

			// Token: 0x06001917 RID: 6423 RVA: 0x000414A0 File Offset: 0x000404A0
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x170003B3 RID: 947
			// (get) Token: 0x06001918 RID: 6424 RVA: 0x000414A8 File Offset: 0x000404A8
			public virtual object Key
			{
				get
				{
					if (this.version != this.sortedList.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.key;
				}
			}

			// Token: 0x06001919 RID: 6425 RVA: 0x000414F8 File Offset: 0x000404F8
			public virtual bool MoveNext()
			{
				if (this.version != this.sortedList.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.index < this.endIndex)
				{
					this.key = this.sortedList.keys[this.index];
					this.value = this.sortedList.values[this.index];
					this.index++;
					this.current = true;
					return true;
				}
				this.key = null;
				this.value = null;
				this.current = false;
				return false;
			}

			// Token: 0x170003B4 RID: 948
			// (get) Token: 0x0600191A RID: 6426 RVA: 0x00041594 File Offset: 0x00040594
			public virtual DictionaryEntry Entry
			{
				get
				{
					if (this.version != this.sortedList.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return new DictionaryEntry(this.key, this.value);
				}
			}

			// Token: 0x170003B5 RID: 949
			// (get) Token: 0x0600191B RID: 6427 RVA: 0x000415F0 File Offset: 0x000405F0
			public virtual object Current
			{
				get
				{
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					if (this.getObjectRetType == 1)
					{
						return this.key;
					}
					if (this.getObjectRetType == 2)
					{
						return this.value;
					}
					return new DictionaryEntry(this.key, this.value);
				}
			}

			// Token: 0x170003B6 RID: 950
			// (get) Token: 0x0600191C RID: 6428 RVA: 0x0004164C File Offset: 0x0004064C
			public virtual object Value
			{
				get
				{
					if (this.version != this.sortedList.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.value;
				}
			}

			// Token: 0x0600191D RID: 6429 RVA: 0x0004169C File Offset: 0x0004069C
			public virtual void Reset()
			{
				if (this.version != this.sortedList.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.index = this.startIndex;
				this.current = false;
				this.key = null;
				this.value = null;
			}

			// Token: 0x040009EA RID: 2538
			internal const int Keys = 1;

			// Token: 0x040009EB RID: 2539
			internal const int Values = 2;

			// Token: 0x040009EC RID: 2540
			internal const int DictEntry = 3;

			// Token: 0x040009ED RID: 2541
			private SortedList sortedList;

			// Token: 0x040009EE RID: 2542
			private object key;

			// Token: 0x040009EF RID: 2543
			private object value;

			// Token: 0x040009F0 RID: 2544
			private int index;

			// Token: 0x040009F1 RID: 2545
			private int startIndex;

			// Token: 0x040009F2 RID: 2546
			private int endIndex;

			// Token: 0x040009F3 RID: 2547
			private int version;

			// Token: 0x040009F4 RID: 2548
			private bool current;

			// Token: 0x040009F5 RID: 2549
			private int getObjectRetType;
		}

		// Token: 0x0200027D RID: 637
		[Serializable]
		private class KeyList : IList, ICollection, IEnumerable
		{
			// Token: 0x0600191E RID: 6430 RVA: 0x000416ED File Offset: 0x000406ED
			internal KeyList(SortedList sortedList)
			{
				this.sortedList = sortedList;
			}

			// Token: 0x170003B7 RID: 951
			// (get) Token: 0x0600191F RID: 6431 RVA: 0x000416FC File Offset: 0x000406FC
			public virtual int Count
			{
				get
				{
					return this.sortedList._size;
				}
			}

			// Token: 0x170003B8 RID: 952
			// (get) Token: 0x06001920 RID: 6432 RVA: 0x00041709 File Offset: 0x00040709
			public virtual bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003B9 RID: 953
			// (get) Token: 0x06001921 RID: 6433 RVA: 0x0004170C File Offset: 0x0004070C
			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003BA RID: 954
			// (get) Token: 0x06001922 RID: 6434 RVA: 0x0004170F File Offset: 0x0004070F
			public virtual bool IsSynchronized
			{
				get
				{
					return this.sortedList.IsSynchronized;
				}
			}

			// Token: 0x170003BB RID: 955
			// (get) Token: 0x06001923 RID: 6435 RVA: 0x0004171C File Offset: 0x0004071C
			public virtual object SyncRoot
			{
				get
				{
					return this.sortedList.SyncRoot;
				}
			}

			// Token: 0x06001924 RID: 6436 RVA: 0x00041729 File Offset: 0x00040729
			public virtual int Add(object key)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x06001925 RID: 6437 RVA: 0x0004173A File Offset: 0x0004073A
			public virtual void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x06001926 RID: 6438 RVA: 0x0004174B File Offset: 0x0004074B
			public virtual bool Contains(object key)
			{
				return this.sortedList.Contains(key);
			}

			// Token: 0x06001927 RID: 6439 RVA: 0x00041759 File Offset: 0x00040759
			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array != null && array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				Array.Copy(this.sortedList.keys, 0, array, arrayIndex, this.sortedList.Count);
			}

			// Token: 0x06001928 RID: 6440 RVA: 0x00041795 File Offset: 0x00040795
			public virtual void Insert(int index, object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x170003BC RID: 956
			public virtual object this[int index]
			{
				get
				{
					return this.sortedList.GetKey(index);
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_KeyCollectionSet"));
				}
			}

			// Token: 0x0600192B RID: 6443 RVA: 0x000417C5 File Offset: 0x000407C5
			public virtual IEnumerator GetEnumerator()
			{
				return new SortedList.SortedListEnumerator(this.sortedList, 0, this.sortedList.Count, 1);
			}

			// Token: 0x0600192C RID: 6444 RVA: 0x000417E0 File Offset: 0x000407E0
			public virtual int IndexOf(object key)
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				int num = Array.BinarySearch(this.sortedList.keys, 0, this.sortedList.Count, key, this.sortedList.comparer);
				if (num >= 0)
				{
					return num;
				}
				return -1;
			}

			// Token: 0x0600192D RID: 6445 RVA: 0x00041835 File Offset: 0x00040835
			public virtual void Remove(object key)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x0600192E RID: 6446 RVA: 0x00041846 File Offset: 0x00040846
			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x040009F6 RID: 2550
			private SortedList sortedList;
		}

		// Token: 0x0200027E RID: 638
		[Serializable]
		private class ValueList : IList, ICollection, IEnumerable
		{
			// Token: 0x0600192F RID: 6447 RVA: 0x00041857 File Offset: 0x00040857
			internal ValueList(SortedList sortedList)
			{
				this.sortedList = sortedList;
			}

			// Token: 0x170003BD RID: 957
			// (get) Token: 0x06001930 RID: 6448 RVA: 0x00041866 File Offset: 0x00040866
			public virtual int Count
			{
				get
				{
					return this.sortedList._size;
				}
			}

			// Token: 0x170003BE RID: 958
			// (get) Token: 0x06001931 RID: 6449 RVA: 0x00041873 File Offset: 0x00040873
			public virtual bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003BF RID: 959
			// (get) Token: 0x06001932 RID: 6450 RVA: 0x00041876 File Offset: 0x00040876
			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x06001933 RID: 6451 RVA: 0x00041879 File Offset: 0x00040879
			public virtual bool IsSynchronized
			{
				get
				{
					return this.sortedList.IsSynchronized;
				}
			}

			// Token: 0x170003C1 RID: 961
			// (get) Token: 0x06001934 RID: 6452 RVA: 0x00041886 File Offset: 0x00040886
			public virtual object SyncRoot
			{
				get
				{
					return this.sortedList.SyncRoot;
				}
			}

			// Token: 0x06001935 RID: 6453 RVA: 0x00041893 File Offset: 0x00040893
			public virtual int Add(object key)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x06001936 RID: 6454 RVA: 0x000418A4 File Offset: 0x000408A4
			public virtual void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x06001937 RID: 6455 RVA: 0x000418B5 File Offset: 0x000408B5
			public virtual bool Contains(object value)
			{
				return this.sortedList.ContainsValue(value);
			}

			// Token: 0x06001938 RID: 6456 RVA: 0x000418C3 File Offset: 0x000408C3
			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array != null && array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				Array.Copy(this.sortedList.values, 0, array, arrayIndex, this.sortedList.Count);
			}

			// Token: 0x06001939 RID: 6457 RVA: 0x000418FF File Offset: 0x000408FF
			public virtual void Insert(int index, object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x170003C2 RID: 962
			public virtual object this[int index]
			{
				get
				{
					return this.sortedList.GetByIndex(index);
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
				}
			}

			// Token: 0x0600193C RID: 6460 RVA: 0x0004192F File Offset: 0x0004092F
			public virtual IEnumerator GetEnumerator()
			{
				return new SortedList.SortedListEnumerator(this.sortedList, 0, this.sortedList.Count, 2);
			}

			// Token: 0x0600193D RID: 6461 RVA: 0x00041949 File Offset: 0x00040949
			public virtual int IndexOf(object value)
			{
				return Array.IndexOf<object>(this.sortedList.values, value, 0, this.sortedList.Count);
			}

			// Token: 0x0600193E RID: 6462 RVA: 0x00041968 File Offset: 0x00040968
			public virtual void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x0600193F RID: 6463 RVA: 0x00041979 File Offset: 0x00040979
			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x040009F7 RID: 2551
			private SortedList sortedList;
		}

		// Token: 0x0200027F RID: 639
		internal class SortedListDebugView
		{
			// Token: 0x06001940 RID: 6464 RVA: 0x0004198A File Offset: 0x0004098A
			public SortedListDebugView(SortedList sortedList)
			{
				if (sortedList == null)
				{
					throw new ArgumentNullException("sortedList");
				}
				this.sortedList = sortedList;
			}

			// Token: 0x170003C3 RID: 963
			// (get) Token: 0x06001941 RID: 6465 RVA: 0x000419A7 File Offset: 0x000409A7
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePairs[] Items
			{
				get
				{
					return this.sortedList.ToKeyValuePairsArray();
				}
			}

			// Token: 0x040009F8 RID: 2552
			private SortedList sortedList;
		}
	}
}
