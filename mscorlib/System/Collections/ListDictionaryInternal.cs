using System;
using System.Threading;

namespace System.Collections
{
	// Token: 0x0200026F RID: 623
	[Serializable]
	internal class ListDictionaryInternal : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x17000385 RID: 901
		public object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				for (ListDictionaryInternal.DictionaryNode next = this.head; next != null; next = next.next)
				{
					if (next.key.Equals(key))
					{
						return next.value;
					}
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
				this.version++;
				ListDictionaryInternal.DictionaryNode dictionaryNode = null;
				ListDictionaryInternal.DictionaryNode next = this.head;
				while (next != null && !next.key.Equals(key))
				{
					dictionaryNode = next;
					next = next.next;
				}
				if (next != null)
				{
					next.value = value;
					return;
				}
				ListDictionaryInternal.DictionaryNode dictionaryNode2 = new ListDictionaryInternal.DictionaryNode();
				dictionaryNode2.key = key;
				dictionaryNode2.value = value;
				if (dictionaryNode != null)
				{
					dictionaryNode.next = dictionaryNode2;
				}
				else
				{
					this.head = dictionaryNode2;
				}
				this.count++;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x0600187B RID: 6267 RVA: 0x0003F37F File Offset: 0x0003E37F
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x0600187C RID: 6268 RVA: 0x0003F387 File Offset: 0x0003E387
		public ICollection Keys
		{
			get
			{
				return new ListDictionaryInternal.NodeKeyValueCollection(this, true);
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x0003F390 File Offset: 0x0003E390
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600187E RID: 6270 RVA: 0x0003F393 File Offset: 0x0003E393
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x0003F396 File Offset: 0x0003E396
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001880 RID: 6272 RVA: 0x0003F399 File Offset: 0x0003E399
		public object SyncRoot
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

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x0003F3BB File Offset: 0x0003E3BB
		public ICollection Values
		{
			get
			{
				return new ListDictionaryInternal.NodeKeyValueCollection(this, false);
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0003F3C4 File Offset: 0x0003E3C4
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
			this.version++;
			ListDictionaryInternal.DictionaryNode dictionaryNode = null;
			ListDictionaryInternal.DictionaryNode next;
			for (next = this.head; next != null; next = next.next)
			{
				if (next.key.Equals(key))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_AddingDuplicate__", new object[]
					{
						next.key,
						key
					}));
				}
				dictionaryNode = next;
			}
			if (next != null)
			{
				next.value = value;
				return;
			}
			ListDictionaryInternal.DictionaryNode dictionaryNode2 = new ListDictionaryInternal.DictionaryNode();
			dictionaryNode2.key = key;
			dictionaryNode2.value = value;
			if (dictionaryNode != null)
			{
				dictionaryNode.next = dictionaryNode2;
			}
			else
			{
				this.head = dictionaryNode2;
			}
			this.count++;
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x0003F4C8 File Offset: 0x0003E4C8
		public void Clear()
		{
			this.count = 0;
			this.head = null;
			this.version++;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0003F4E8 File Offset: 0x0003E4E8
		public bool Contains(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			for (ListDictionaryInternal.DictionaryNode next = this.head; next != null; next = next.next)
			{
				if (next.key.Equals(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0003F534 File Offset: 0x0003E534
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
			for (ListDictionaryInternal.DictionaryNode next = this.head; next != null; next = next.next)
			{
				array.SetValue(new DictionaryEntry(next.key, next.value), index);
				index++;
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0003F5DB File Offset: 0x0003E5DB
		public IDictionaryEnumerator GetEnumerator()
		{
			return new ListDictionaryInternal.NodeEnumerator(this);
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0003F5E3 File Offset: 0x0003E5E3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ListDictionaryInternal.NodeEnumerator(this);
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0003F5EC File Offset: 0x0003E5EC
		public void Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			this.version++;
			ListDictionaryInternal.DictionaryNode dictionaryNode = null;
			ListDictionaryInternal.DictionaryNode next = this.head;
			while (next != null && !next.key.Equals(key))
			{
				dictionaryNode = next;
				next = next.next;
			}
			if (next == null)
			{
				return;
			}
			if (next == this.head)
			{
				this.head = next.next;
			}
			else
			{
				dictionaryNode.next = next.next;
			}
			this.count--;
		}

		// Token: 0x040009B9 RID: 2489
		private ListDictionaryInternal.DictionaryNode head;

		// Token: 0x040009BA RID: 2490
		private int version;

		// Token: 0x040009BB RID: 2491
		private int count;

		// Token: 0x040009BC RID: 2492
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x02000270 RID: 624
		private class NodeEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06001889 RID: 6281 RVA: 0x0003F679 File Offset: 0x0003E679
			public NodeEnumerator(ListDictionaryInternal list)
			{
				this.list = list;
				this.version = list.version;
				this.start = true;
				this.current = null;
			}

			// Token: 0x1700038D RID: 909
			// (get) Token: 0x0600188A RID: 6282 RVA: 0x0003F6A2 File Offset: 0x0003E6A2
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x1700038E RID: 910
			// (get) Token: 0x0600188B RID: 6283 RVA: 0x0003F6AF File Offset: 0x0003E6AF
			public DictionaryEntry Entry
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return new DictionaryEntry(this.current.key, this.current.value);
				}
			}

			// Token: 0x1700038F RID: 911
			// (get) Token: 0x0600188C RID: 6284 RVA: 0x0003F6E4 File Offset: 0x0003E6E4
			public object Key
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.current.key;
				}
			}

			// Token: 0x17000390 RID: 912
			// (get) Token: 0x0600188D RID: 6285 RVA: 0x0003F709 File Offset: 0x0003E709
			public object Value
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.current.value;
				}
			}

			// Token: 0x0600188E RID: 6286 RVA: 0x0003F730 File Offset: 0x0003E730
			public bool MoveNext()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.start)
				{
					this.current = this.list.head;
					this.start = false;
				}
				else if (this.current != null)
				{
					this.current = this.current.next;
				}
				return this.current != null;
			}

			// Token: 0x0600188F RID: 6287 RVA: 0x0003F7A7 File Offset: 0x0003E7A7
			public void Reset()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.start = true;
				this.current = null;
			}

			// Token: 0x040009BD RID: 2493
			private ListDictionaryInternal list;

			// Token: 0x040009BE RID: 2494
			private ListDictionaryInternal.DictionaryNode current;

			// Token: 0x040009BF RID: 2495
			private int version;

			// Token: 0x040009C0 RID: 2496
			private bool start;
		}

		// Token: 0x02000271 RID: 625
		private class NodeKeyValueCollection : ICollection, IEnumerable
		{
			// Token: 0x06001890 RID: 6288 RVA: 0x0003F7DA File Offset: 0x0003E7DA
			public NodeKeyValueCollection(ListDictionaryInternal list, bool isKeys)
			{
				this.list = list;
				this.isKeys = isKeys;
			}

			// Token: 0x06001891 RID: 6289 RVA: 0x0003F7F0 File Offset: 0x0003E7F0
			void ICollection.CopyTo(Array array, int index)
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
				if (array.Length - index < this.list.Count)
				{
					throw new ArgumentException(Environment.GetResourceString("ArgumentOutOfRange_Index"), "index");
				}
				for (ListDictionaryInternal.DictionaryNode dictionaryNode = this.list.head; dictionaryNode != null; dictionaryNode = dictionaryNode.next)
				{
					array.SetValue(this.isKeys ? dictionaryNode.key : dictionaryNode.value, index);
					index++;
				}
			}

			// Token: 0x17000391 RID: 913
			// (get) Token: 0x06001892 RID: 6290 RVA: 0x0003F8A4 File Offset: 0x0003E8A4
			int ICollection.Count
			{
				get
				{
					int num = 0;
					for (ListDictionaryInternal.DictionaryNode dictionaryNode = this.list.head; dictionaryNode != null; dictionaryNode = dictionaryNode.next)
					{
						num++;
					}
					return num;
				}
			}

			// Token: 0x17000392 RID: 914
			// (get) Token: 0x06001893 RID: 6291 RVA: 0x0003F8D0 File Offset: 0x0003E8D0
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000393 RID: 915
			// (get) Token: 0x06001894 RID: 6292 RVA: 0x0003F8D3 File Offset: 0x0003E8D3
			object ICollection.SyncRoot
			{
				get
				{
					return this.list.SyncRoot;
				}
			}

			// Token: 0x06001895 RID: 6293 RVA: 0x0003F8E0 File Offset: 0x0003E8E0
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ListDictionaryInternal.NodeKeyValueCollection.NodeKeyValueEnumerator(this.list, this.isKeys);
			}

			// Token: 0x040009C1 RID: 2497
			private ListDictionaryInternal list;

			// Token: 0x040009C2 RID: 2498
			private bool isKeys;

			// Token: 0x02000272 RID: 626
			private class NodeKeyValueEnumerator : IEnumerator
			{
				// Token: 0x06001896 RID: 6294 RVA: 0x0003F8F3 File Offset: 0x0003E8F3
				public NodeKeyValueEnumerator(ListDictionaryInternal list, bool isKeys)
				{
					this.list = list;
					this.isKeys = isKeys;
					this.version = list.version;
					this.start = true;
					this.current = null;
				}

				// Token: 0x17000394 RID: 916
				// (get) Token: 0x06001897 RID: 6295 RVA: 0x0003F923 File Offset: 0x0003E923
				public object Current
				{
					get
					{
						if (this.current == null)
						{
							throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
						}
						if (!this.isKeys)
						{
							return this.current.value;
						}
						return this.current.key;
					}
				}

				// Token: 0x06001898 RID: 6296 RVA: 0x0003F95C File Offset: 0x0003E95C
				public bool MoveNext()
				{
					if (this.version != this.list.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					if (this.start)
					{
						this.current = this.list.head;
						this.start = false;
					}
					else if (this.current != null)
					{
						this.current = this.current.next;
					}
					return this.current != null;
				}

				// Token: 0x06001899 RID: 6297 RVA: 0x0003F9D3 File Offset: 0x0003E9D3
				public void Reset()
				{
					if (this.version != this.list.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					this.start = true;
					this.current = null;
				}

				// Token: 0x040009C3 RID: 2499
				private ListDictionaryInternal list;

				// Token: 0x040009C4 RID: 2500
				private ListDictionaryInternal.DictionaryNode current;

				// Token: 0x040009C5 RID: 2501
				private int version;

				// Token: 0x040009C6 RID: 2502
				private bool isKeys;

				// Token: 0x040009C7 RID: 2503
				private bool start;
			}
		}

		// Token: 0x02000273 RID: 627
		[Serializable]
		private class DictionaryNode
		{
			// Token: 0x040009C8 RID: 2504
			public object key;

			// Token: 0x040009C9 RID: 2505
			public object value;

			// Token: 0x040009CA RID: 2506
			public ListDictionaryInternal.DictionaryNode next;
		}
	}
}
