using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.ObjectModel
{
	// Token: 0x020002AC RID: 684
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
	[ComVisible(false)]
	[Serializable]
	public class Collection<T> : IList<T>, ICollection<!0>, IEnumerable<!0>, IList, ICollection, IEnumerable
	{
		// Token: 0x06001AAF RID: 6831 RVA: 0x0004607D File Offset: 0x0004507D
		public Collection()
		{
			this.items = new List<T>();
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00046090 File Offset: 0x00045090
		public Collection(IList<T> list)
		{
			if (list == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
			}
			this.items = list;
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x000460A8 File Offset: 0x000450A8
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x000460B5 File Offset: 0x000450B5
		protected IList<T> Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x1700040C RID: 1036
		public T this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				if (this.items.IsReadOnly)
				{
					ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
				}
				if (index < 0 || index >= this.items.Count)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				this.SetItem(index, value);
			}
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x00046100 File Offset: 0x00045100
		public void Add(T item)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			int count = this.items.Count;
			this.InsertItem(count, item);
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00046135 File Offset: 0x00045135
		public void Clear()
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			this.ClearItems();
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00046151 File Offset: 0x00045151
		public void CopyTo(T[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x00046160 File Offset: 0x00045160
		public bool Contains(T item)
		{
			return this.items.Contains(item);
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x0004616E File Offset: 0x0004516E
		public IEnumerator<T> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x0004617B File Offset: 0x0004517B
		public int IndexOf(T item)
		{
			return this.items.IndexOf(item);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00046189 File Offset: 0x00045189
		public void Insert(int index, T item)
		{
			if (index < 0 || index > this.items.Count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
			}
			this.InsertItem(index, item);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x000461B0 File Offset: 0x000451B0
		public bool Remove(T item)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			int num = this.items.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			this.RemoveItem(num);
			return true;
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x000461EC File Offset: 0x000451EC
		public void RemoveAt(int index)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			if (index < 0 || index >= this.items.Count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this.RemoveItem(index);
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00046220 File Offset: 0x00045220
		protected virtual void ClearItems()
		{
			this.items.Clear();
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0004622D File Offset: 0x0004522D
		protected virtual void InsertItem(int index, T item)
		{
			this.items.Insert(index, item);
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x0004623C File Offset: 0x0004523C
		protected virtual void RemoveItem(int index)
		{
			this.items.RemoveAt(index);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x0004624A File Offset: 0x0004524A
		protected virtual void SetItem(int index, T item)
		{
			this.items[index] = item;
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x00046259 File Offset: 0x00045259
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return this.items.IsReadOnly;
			}
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00046266 File Offset: 0x00045266
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x00046273 File Offset: 0x00045273
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x00046278 File Offset: 0x00045278
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					ICollection collection = this.items as ICollection;
					if (collection != null)
					{
						this._syncRoot = collection.SyncRoot;
					}
					else
					{
						Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
					}
				}
				return this._syncRoot;
			}
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x000462C4 File Offset: 0x000452C4
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
			}
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.items.CopyTo(array2, index);
				return;
			}
			Type elementType = array.GetType().GetElementType();
			Type typeFromHandle = typeof(T);
			if (!elementType.IsAssignableFrom(typeFromHandle) && !typeFromHandle.IsAssignableFrom(elementType))
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			object[] array3 = array as object[];
			if (array3 == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			int count = this.items.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					array3[index++] = this.items[i];
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x17000410 RID: 1040
		object IList.this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				Collection<T>.VerifyValueType(value);
				this[index] = (T)((object)value);
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x000463F0 File Offset: 0x000453F0
		bool IList.IsReadOnly
		{
			get
			{
				return this.items.IsReadOnly;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001ACA RID: 6858 RVA: 0x00046400 File Offset: 0x00045400
		bool IList.IsFixedSize
		{
			get
			{
				IList list = this.items as IList;
				return list != null && list.IsFixedSize;
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00046424 File Offset: 0x00045424
		int IList.Add(object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			Collection<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00046454 File Offset: 0x00045454
		bool IList.Contains(object value)
		{
			return Collection<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0004646C File Offset: 0x0004546C
		int IList.IndexOf(object value)
		{
			if (Collection<T>.IsCompatibleObject(value))
			{
				return this.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x00046484 File Offset: 0x00045484
		void IList.Insert(int index, object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			Collection<T>.VerifyValueType(value);
			this.Insert(index, (T)((object)value));
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x000464AD File Offset: 0x000454AD
		void IList.Remove(object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			if (Collection<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000464D8 File Offset: 0x000454D8
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && !typeof(T).IsValueType);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x000464F9 File Offset: 0x000454F9
		private static void VerifyValueType(object value)
		{
			if (!Collection<T>.IsCompatibleObject(value))
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
			}
		}

		// Token: 0x04000A40 RID: 2624
		private IList<T> items;

		// Token: 0x04000A41 RID: 2625
		[NonSerialized]
		private object _syncRoot;
	}
}
