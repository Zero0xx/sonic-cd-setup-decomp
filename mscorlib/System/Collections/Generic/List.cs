using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x020002A4 RID: 676
	[DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class List<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
	{
		// Token: 0x06001A44 RID: 6724 RVA: 0x000445DB File Offset: 0x000435DB
		public List()
		{
			this._items = List<T>._emptyArray;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x000445EE File Offset: 0x000435EE
		public List(int capacity)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
			}
			this._items = new T[capacity];
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x00044610 File Offset: 0x00043610
		public List(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			ICollection<T> collection2 = collection as ICollection<!0>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				this._items = new T[count];
				collection2.CopyTo(this._items, 0);
				this._size = count;
				return;
			}
			this._size = 0;
			this._items = new T[4];
			foreach (T item in collection)
			{
				this.Add(item);
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x000446AC File Offset: 0x000436AC
		// (set) Token: 0x06001A48 RID: 6728 RVA: 0x000446B8 File Offset: 0x000436B8
		public int Capacity
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
						ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
					}
					if (value > 0)
					{
						T[] array = new T[value];
						if (this._size > 0)
						{
							Array.Copy(this._items, 0, array, 0, this._size);
						}
						this._items = array;
						return;
					}
					this._items = List<T>._emptyArray;
				}
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001A49 RID: 6729 RVA: 0x0004471D File Offset: 0x0004371D
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x00044725 File Offset: 0x00043725
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001A4B RID: 6731 RVA: 0x00044728 File Offset: 0x00043728
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x0004472B File Offset: 0x0004372B
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x0004472E File Offset: 0x0004372E
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x00044731 File Offset: 0x00043731
		object ICollection.SyncRoot
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

		// Token: 0x17000404 RID: 1028
		public T this[int index]
		{
			get
			{
				if (index >= this._size)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				return this._items[index];
			}
			set
			{
				if (index >= this._size)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				this._items[index] = value;
				this._version++;
			}
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x0004479A File Offset: 0x0004379A
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && !typeof(T).IsValueType);
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x000447BB File Offset: 0x000437BB
		private static void VerifyValueType(object value)
		{
			if (!List<T>.IsCompatibleObject(value))
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
			}
		}

		// Token: 0x17000405 RID: 1029
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				List<T>.VerifyValueType(value);
				this[index] = (T)((object)value);
			}
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000447F8 File Offset: 0x000437F8
		public void Add(T item)
		{
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			this._items[this._size++] = item;
			this._version++;
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0004484E File Offset: 0x0004384E
		int IList.Add(object item)
		{
			List<T>.VerifyValueType(item);
			this.Add((T)((object)item));
			return this.Count - 1;
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0004486A File Offset: 0x0004386A
		public void AddRange(IEnumerable<T> collection)
		{
			this.InsertRange(this._size, collection);
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00044879 File Offset: 0x00043879
		public ReadOnlyCollection<T> AsReadOnly()
		{
			return new ReadOnlyCollection<T>(this);
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00044881 File Offset: 0x00043881
		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			return Array.BinarySearch<T>(this._items, index, count, item, comparer);
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x000448BD File Offset: 0x000438BD
		public int BinarySearch(T item)
		{
			return this.BinarySearch(0, this.Count, item, null);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x000448CE File Offset: 0x000438CE
		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return this.BinarySearch(0, this.Count, item, comparer);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x000448DF File Offset: 0x000438DF
		public void Clear()
		{
			if (this._size > 0)
			{
				Array.Clear(this._items, 0, this._size);
				this._size = 0;
			}
			this._version++;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x00044914 File Offset: 0x00043914
		public bool Contains(T item)
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
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int j = 0; j < this._size; j++)
			{
				if (@default.Equals(this._items[j], item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x00044980 File Offset: 0x00043980
		bool IList.Contains(object item)
		{
			return List<T>.IsCompatibleObject(item) && this.Contains((T)((object)item));
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00044998 File Offset: 0x00043998
		public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			if (converter == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.converter);
			}
			List<TOutput> list = new List<TOutput>(this._size);
			for (int i = 0; i < this._size; i++)
			{
				list._items[i] = converter(this._items[i]);
			}
			list._size = this._size;
			return list;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x000449F7 File Offset: 0x000439F7
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00044A04 File Offset: 0x00043A04
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if (array != null && array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			try
			{
				Array.Copy(this._items, 0, array, arrayIndex, this._size);
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00044A54 File Offset: 0x00043A54
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Copy(this._items, index, array, arrayIndex, count);
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00044A79 File Offset: 0x00043A79
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._size);
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00044A90 File Offset: 0x00043A90
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

		// Token: 0x06001A65 RID: 6757 RVA: 0x00044ACD File Offset: 0x00043ACD
		public bool Exists(Predicate<T> match)
		{
			return this.FindIndex(match) != -1;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x00044ADC File Offset: 0x00043ADC
		public T Find(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00044B30 File Offset: 0x00043B30
		public List<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			List<T> list = new List<T>();
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					list.Add(this._items[i]);
				}
			}
			return list;
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x00044B84 File Offset: 0x00043B84
		public int FindIndex(Predicate<T> match)
		{
			return this.FindIndex(0, this._size, match);
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x00044B94 File Offset: 0x00043B94
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return this.FindIndex(startIndex, this._size - startIndex, match);
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x00044BA8 File Offset: 0x00043BA8
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || startIndex > this._size - count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00044C10 File Offset: 0x00043C10
		public T FindLast(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = this._size - 1; i >= 0; i--)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x00044C63 File Offset: 0x00043C63
		public int FindLastIndex(Predicate<T> match)
		{
			return this.FindLastIndex(this._size - 1, this._size, match);
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00044C7A File Offset: 0x00043C7A
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return this.FindLastIndex(startIndex, startIndex + 1, match);
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x00044C88 File Offset: 0x00043C88
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			if (this._size == 0)
			{
				if (startIndex != -1)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
				}
			}
			else if (startIndex >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			int num = startIndex - count;
			for (int i = startIndex; i > num; i--)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00044D04 File Offset: 0x00043D04
		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < this._size; i++)
			{
				action(this._items[i]);
			}
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00044D3D File Offset: 0x00043D3D
		public List<T>.Enumerator GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00044D45 File Offset: 0x00043D45
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00044D52 File Offset: 0x00043D52
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00044D60 File Offset: 0x00043D60
		public List<T> GetRange(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			List<T> list = new List<T>(count);
			Array.Copy(this._items, index, list._items, 0, count);
			list._size = count;
			return list;
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00044DBA File Offset: 0x00043DBA
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this._items, item, 0, this._size);
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00044DCF File Offset: 0x00043DCF
		int IList.IndexOf(object item)
		{
			if (List<T>.IsCompatibleObject(item))
			{
				return this.IndexOf((T)((object)item));
			}
			return -1;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00044DE7 File Offset: 0x00043DE7
		public int IndexOf(T item, int index)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return Array.IndexOf<T>(this._items, item, index, this._size - index);
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x00044E10 File Offset: 0x00043E10
		public int IndexOf(T item, int index, int count)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || index > this._size - count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			return Array.IndexOf<T>(this._items, item, index, count);
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x00044E4C File Offset: 0x00043E4C
		public void Insert(int index, T item)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
			}
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this._items, index, this._items, index + 1, this._size - index);
			}
			this._items[index] = item;
			this._size++;
			this._version++;
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x00044ED8 File Offset: 0x00043ED8
		void IList.Insert(int index, object item)
		{
			List<T>.VerifyValueType(item);
			this.Insert(index, (T)((object)item));
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x00044EF0 File Offset: 0x00043EF0
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			ICollection<T> collection2 = collection as ICollection<!0>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				if (count > 0)
				{
					this.EnsureCapacity(this._size + count);
					if (index < this._size)
					{
						Array.Copy(this._items, index, this._items, index + count, this._size - index);
					}
					if (this == collection2)
					{
						Array.Copy(this._items, 0, this._items, index, index);
						Array.Copy(this._items, index + count, this._items, index * 2, this._size - index);
					}
					else
					{
						T[] array = new T[count];
						collection2.CopyTo(array, 0);
						array.CopyTo(this._items, index);
					}
					this._size += count;
				}
			}
			else
			{
				foreach (T item in collection)
				{
					this.Insert(index++, item);
				}
			}
			this._version++;
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0004501C File Offset: 0x0004401C
		public int LastIndexOf(T item)
		{
			return this.LastIndexOf(item, this._size - 1, this._size);
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00045033 File Offset: 0x00044033
		public int LastIndexOf(T item, int index)
		{
			if (index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return this.LastIndexOf(item, index, index + 1);
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x00045054 File Offset: 0x00044054
		public int LastIndexOf(T item, int index, int count)
		{
			if (this._size == 0)
			{
				return -1;
			}
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (index >= this._size || count > index + 1)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index >= this._size) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
			}
			return Array.LastIndexOf<T>(this._items, item, index, count);
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x000450B8 File Offset: 0x000440B8
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000450DB File Offset: 0x000440DB
		void IList.Remove(object item)
		{
			if (List<T>.IsCompatibleObject(item))
			{
				this.Remove((T)((object)item));
			}
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000450F4 File Offset: 0x000440F4
		public int RemoveAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int num = 0;
			while (num < this._size && !match(this._items[num]))
			{
				num++;
			}
			if (num >= this._size)
			{
				return 0;
			}
			int i = num + 1;
			while (i < this._size)
			{
				while (i < this._size && match(this._items[i]))
				{
					i++;
				}
				if (i < this._size)
				{
					this._items[num++] = this._items[i++];
				}
			}
			Array.Clear(this._items, num, this._size - num);
			int result = this._size - num;
			this._size = num;
			this._version++;
			return result;
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x000451C8 File Offset: 0x000441C8
		public void RemoveAt(int index)
		{
			if (index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this._items, index + 1, this._items, index, this._size - index);
			}
			this._items[this._size] = default(T);
			this._version++;
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00045240 File Offset: 0x00044240
		public void RemoveRange(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			if (count > 0)
			{
				this._size -= count;
				if (index < this._size)
				{
					Array.Copy(this._items, index + count, this._items, index, this._size - index);
				}
				Array.Clear(this._items, this._size, count);
				this._version++;
			}
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000452CF File Offset: 0x000442CF
		public void Reverse()
		{
			this.Reverse(0, this.Count);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000452E0 File Offset: 0x000442E0
		public void Reverse(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Reverse(this._items, index, count);
			this._version++;
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x00045332 File Offset: 0x00044332
		public void Sort()
		{
			this.Sort(0, this.Count, null);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x00045342 File Offset: 0x00044342
		public void Sort(IComparer<T> comparer)
		{
			this.Sort(0, this.Count, comparer);
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x00045354 File Offset: 0x00044354
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Sort<T>(this._items, index, count, comparer);
			this._version++;
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x000453A8 File Offset: 0x000443A8
		public void Sort(Comparison<T> comparison)
		{
			if (comparison == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			if (this._size > 0)
			{
				IComparer<T> comparer = new Array.FunctorComparer<T>(comparison);
				Array.Sort<T>(this._items, 0, this._size, comparer);
			}
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x000453E4 File Offset: 0x000443E4
		public T[] ToArray()
		{
			T[] array = new T[this._size];
			Array.Copy(this._items, 0, array, 0, this._size);
			return array;
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00045414 File Offset: 0x00044414
		public void TrimExcess()
		{
			int num = (int)((double)this._items.Length * 0.9);
			if (this._size < num)
			{
				this.Capacity = this._size;
			}
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0004544C File Offset: 0x0004444C
		public bool TrueForAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < this._size; i++)
			{
				if (!match(this._items[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000A34 RID: 2612
		private const int _defaultCapacity = 4;

		// Token: 0x04000A35 RID: 2613
		private T[] _items;

		// Token: 0x04000A36 RID: 2614
		private int _size;

		// Token: 0x04000A37 RID: 2615
		private int _version;

		// Token: 0x04000A38 RID: 2616
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x04000A39 RID: 2617
		private static T[] _emptyArray = new T[0];

		// Token: 0x020002A5 RID: 677
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x06001A8D RID: 6797 RVA: 0x00045497 File Offset: 0x00044497
			internal Enumerator(List<T> list)
			{
				this.list = list;
				this.index = 0;
				this.version = list._version;
				this.current = default(T);
			}

			// Token: 0x06001A8E RID: 6798 RVA: 0x000454BF File Offset: 0x000444BF
			public void Dispose()
			{
			}

			// Token: 0x06001A8F RID: 6799 RVA: 0x000454C4 File Offset: 0x000444C4
			public bool MoveNext()
			{
				List<T> list = this.list;
				if (this.version == list._version && this.index < list._size)
				{
					this.current = list._items[this.index];
					this.index++;
					return true;
				}
				return this.MoveNextRare();
			}

			// Token: 0x06001A90 RID: 6800 RVA: 0x00045521 File Offset: 0x00044521
			private bool MoveNextRare()
			{
				if (this.version != this.list._version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = this.list._size + 1;
				this.current = default(T);
				return false;
			}

			// Token: 0x17000406 RID: 1030
			// (get) Token: 0x06001A91 RID: 6801 RVA: 0x0004555D File Offset: 0x0004455D
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x17000407 RID: 1031
			// (get) Token: 0x06001A92 RID: 6802 RVA: 0x00045565 File Offset: 0x00044565
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this.list._size + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.Current;
				}
			}

			// Token: 0x06001A93 RID: 6803 RVA: 0x00045596 File Offset: 0x00044596
			void IEnumerator.Reset()
			{
				if (this.version != this.list._version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.current = default(T);
			}

			// Token: 0x04000A3A RID: 2618
			private List<T> list;

			// Token: 0x04000A3B RID: 2619
			private int index;

			// Token: 0x04000A3C RID: 2620
			private int version;

			// Token: 0x04000A3D RID: 2621
			private T current;
		}
	}
}
