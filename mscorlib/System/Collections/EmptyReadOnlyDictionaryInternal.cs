using System;

namespace System.Collections
{
	// Token: 0x02000263 RID: 611
	[Serializable]
	internal sealed class EmptyReadOnlyDictionaryInternal : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x060017F2 RID: 6130 RVA: 0x0003D28D File Offset: 0x0003C28D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new EmptyReadOnlyDictionaryInternal.NodeEnumerator();
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0003D294 File Offset: 0x0003C294
		public void CopyTo(Array array, int index)
		{
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
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentOutOfRange_Index"), "index");
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060017F4 RID: 6132 RVA: 0x0003D306 File Offset: 0x0003C306
		public int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x0003D309 File Offset: 0x0003C309
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x0003D30C File Offset: 0x0003C30C
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000359 RID: 857
		public object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				if (!key.GetType().IsSerializable)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotSerializable"), "key");
				}
				if (value != null && !value.GetType().IsSerializable)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotSerializable"), "value");
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x0003D3A7 File Offset: 0x0003C3A7
		public ICollection Keys
		{
			get
			{
				return new object[0];
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x0003D3AF File Offset: 0x0003C3AF
		public ICollection Values
		{
			get
			{
				return new object[0];
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0003D3B7 File Offset: 0x0003C3B7
		public bool Contains(object key)
		{
			return false;
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0003D3BC File Offset: 0x0003C3BC
		public void Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			if (!key.GetType().IsSerializable)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSerializable"), "key");
			}
			if (value != null && !value.GetType().IsSerializable)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSerializable"), "value");
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0003D437 File Offset: 0x0003C437
		public void Clear()
		{
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060017FE RID: 6142 RVA: 0x0003D448 File Offset: 0x0003C448
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x0003D44B File Offset: 0x0003C44B
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0003D44E File Offset: 0x0003C44E
		public IDictionaryEnumerator GetEnumerator()
		{
			return new EmptyReadOnlyDictionaryInternal.NodeEnumerator();
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0003D455 File Offset: 0x0003C455
		public void Remove(object key)
		{
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
		}

		// Token: 0x02000265 RID: 613
		private sealed class NodeEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06001806 RID: 6150 RVA: 0x0003D46E File Offset: 0x0003C46E
			public bool MoveNext()
			{
				return false;
			}

			// Token: 0x17000361 RID: 865
			// (get) Token: 0x06001807 RID: 6151 RVA: 0x0003D471 File Offset: 0x0003C471
			public object Current
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
			}

			// Token: 0x06001808 RID: 6152 RVA: 0x0003D482 File Offset: 0x0003C482
			public void Reset()
			{
			}

			// Token: 0x17000362 RID: 866
			// (get) Token: 0x06001809 RID: 6153 RVA: 0x0003D484 File Offset: 0x0003C484
			public object Key
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
			}

			// Token: 0x17000363 RID: 867
			// (get) Token: 0x0600180A RID: 6154 RVA: 0x0003D495 File Offset: 0x0003C495
			public object Value
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
			}

			// Token: 0x17000364 RID: 868
			// (get) Token: 0x0600180B RID: 6155 RVA: 0x0003D4A6 File Offset: 0x0003C4A6
			public DictionaryEntry Entry
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
			}
		}
	}
}
