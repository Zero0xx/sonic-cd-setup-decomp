using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.ObjectModel
{
	// Token: 0x020002AD RID: 685
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class ReadOnlyCollection<T> : IList<T>, ICollection<!0>, IEnumerable<!0>, IList, ICollection, IEnumerable
	{
		// Token: 0x06001AD2 RID: 6866 RVA: 0x00046513 File Offset: 0x00045513
		public ReadOnlyCollection(IList<T> list)
		{
			if (list == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
			}
			this.list = list;
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x0004652B File Offset: 0x0004552B
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000414 RID: 1044
		public T this[int index]
		{
			get
			{
				return this.list[index];
			}
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00046546 File Offset: 0x00045546
		public bool Contains(T value)
		{
			return this.list.Contains(value);
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x00046554 File Offset: 0x00045554
		public void CopyTo(T[] array, int index)
		{
			this.list.CopyTo(array, index);
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x00046563 File Offset: 0x00045563
		public IEnumerator<T> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00046570 File Offset: 0x00045570
		public int IndexOf(T value)
		{
			return this.list.IndexOf(value);
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x0004657E File Offset: 0x0004557E
		protected IList<T> Items
		{
			get
			{
				return this.list;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x00046586 File Offset: 0x00045586
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000417 RID: 1047
		T IList<!0>.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x000465A0 File Offset: 0x000455A0
		void ICollection<!0>.Add(T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x000465A9 File Offset: 0x000455A9
		void ICollection<!0>.Clear()
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x000465B2 File Offset: 0x000455B2
		void IList<!0>.Insert(int index, T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x000465BB File Offset: 0x000455BB
		bool ICollection<!0>.Remove(T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			return false;
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x000465C5 File Offset: 0x000455C5
		void IList<!0>.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x000465CE File Offset: 0x000455CE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x000465DB File Offset: 0x000455DB
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x000465E0 File Offset: 0x000455E0
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					ICollection collection = this.list as ICollection;
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

		// Token: 0x06001AE5 RID: 6885 RVA: 0x0004662C File Offset: 0x0004562C
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
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.list.CopyTo(array2, index);
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
			int count = this.list.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					array3[index++] = this.list[i];
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x00046730 File Offset: 0x00045730
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x00046733 File Offset: 0x00045733
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700041C RID: 1052
		object IList.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x00046752 File Offset: 0x00045752
		int IList.Add(object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			return -1;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0004675C File Offset: 0x0004575C
		void IList.Clear()
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00046765 File Offset: 0x00045765
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && !typeof(T).IsValueType);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00046786 File Offset: 0x00045786
		bool IList.Contains(object value)
		{
			return ReadOnlyCollection<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x0004679E File Offset: 0x0004579E
		int IList.IndexOf(object value)
		{
			if (ReadOnlyCollection<T>.IsCompatibleObject(value))
			{
				return this.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x000467B6 File Offset: 0x000457B6
		void IList.Insert(int index, object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x000467BF File Offset: 0x000457BF
		void IList.Remove(object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x000467C8 File Offset: 0x000457C8
		void IList.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000467D1 File Offset: 0x000457D1
		private static void VerifyValueType(object value)
		{
			if (!ReadOnlyCollection<T>.IsCompatibleObject(value))
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
			}
		}

		// Token: 0x04000A42 RID: 2626
		private IList<T> list;

		// Token: 0x04000A43 RID: 2627
		[NonSerialized]
		private object _syncRoot;
	}
}
