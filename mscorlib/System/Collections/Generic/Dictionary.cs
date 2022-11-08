using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x0200028C RID: 652
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(Mscorlib_DictionaryDebugView<, >))]
	[Serializable]
	public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x06001993 RID: 6547 RVA: 0x0004258D File Offset: 0x0004158D
		public Dictionary() : this(0, null)
		{
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00042597 File Offset: 0x00041597
		public Dictionary(int capacity) : this(capacity, null)
		{
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000425A1 File Offset: 0x000415A1
		public Dictionary(IEqualityComparer<TKey> comparer) : this(0, comparer)
		{
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x000425AB File Offset: 0x000415AB
		public Dictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
			}
			if (capacity > 0)
			{
				this.Initialize(capacity);
			}
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			this.comparer = comparer;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x000425DA File Offset: 0x000415DA
		public Dictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, null)
		{
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x000425E4 File Offset: 0x000415E4
		public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : this((dictionary != null) ? dictionary.Count : 0, comparer)
		{
			if (dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00042658 File Offset: 0x00041658
		protected Dictionary(SerializationInfo info, StreamingContext context)
		{
			this.m_siInfo = info;
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600199A RID: 6554 RVA: 0x00042667 File Offset: 0x00041667
		public IEqualityComparer<TKey> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x0600199B RID: 6555 RVA: 0x0004266F File Offset: 0x0004166F
		public int Count
		{
			get
			{
				return this.count - this.freeCount;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x0600199C RID: 6556 RVA: 0x0004267E File Offset: 0x0004167E
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				if (this.keys == null)
				{
					this.keys = new Dictionary<TKey, TValue>.KeyCollection(this);
				}
				return this.keys;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x0600199D RID: 6557 RVA: 0x0004269A File Offset: 0x0004169A
		ICollection<TKey> IDictionary<!0, !1>.Keys
		{
			get
			{
				if (this.keys == null)
				{
					this.keys = new Dictionary<TKey, TValue>.KeyCollection(this);
				}
				return this.keys;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x0600199E RID: 6558 RVA: 0x000426B6 File Offset: 0x000416B6
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				if (this.values == null)
				{
					this.values = new Dictionary<TKey, TValue>.ValueCollection(this);
				}
				return this.values;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x000426D2 File Offset: 0x000416D2
		ICollection<TValue> IDictionary<!0, !1>.Values
		{
			get
			{
				if (this.values == null)
				{
					this.values = new Dictionary<TKey, TValue>.ValueCollection(this);
				}
				return this.values;
			}
		}

		// Token: 0x170003DA RID: 986
		public TValue this[TKey key]
		{
			get
			{
				int num = this.FindEntry(key);
				if (num >= 0)
				{
					return this.entries[num].value;
				}
				ThrowHelper.ThrowKeyNotFoundException();
				return default(TValue);
			}
			set
			{
				this.Insert(key, value, false);
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00042734 File Offset: 0x00041734
		public void Add(TKey key, TValue value)
		{
			this.Insert(key, value, true);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0004273F File Offset: 0x0004173F
		void ICollection<KeyValuePair<!0, !1>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this.Add(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00042758 File Offset: 0x00041758
		bool ICollection<KeyValuePair<!0, !1>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.FindEntry(keyValuePair.Key);
			return num >= 0 && EqualityComparer<TValue>.Default.Equals(this.entries[num].value, keyValuePair.Value);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x000427A0 File Offset: 0x000417A0
		bool ICollection<KeyValuePair<!0, !1>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.FindEntry(keyValuePair.Key);
			if (num >= 0 && EqualityComparer<TValue>.Default.Equals(this.entries[num].value, keyValuePair.Value))
			{
				this.Remove(keyValuePair.Key);
				return true;
			}
			return false;
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x000427F4 File Offset: 0x000417F4
		public void Clear()
		{
			if (this.count > 0)
			{
				for (int i = 0; i < this.buckets.Length; i++)
				{
					this.buckets[i] = -1;
				}
				Array.Clear(this.entries, 0, this.count);
				this.freeList = -1;
				this.count = 0;
				this.freeCount = 0;
				this.version++;
			}
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0004285B File Offset: 0x0004185B
		public bool ContainsKey(TKey key)
		{
			return this.FindEntry(key) >= 0;
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0004286C File Offset: 0x0004186C
		public bool ContainsValue(TValue value)
		{
			if (value == null)
			{
				for (int i = 0; i < this.count; i++)
				{
					if (this.entries[i].hashCode >= 0 && this.entries[i].value == null)
					{
						return true;
					}
				}
			}
			else
			{
				EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
				for (int j = 0; j < this.count; j++)
				{
					if (this.entries[j].hashCode >= 0 && @default.Equals(this.entries[j].value, value))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0004290C File Offset: 0x0004190C
		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (index < 0 || index > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			int num = this.count;
			Dictionary<TKey, TValue>.Entry[] array2 = this.entries;
			for (int i = 0; i < num; i++)
			{
				if (array2[i].hashCode >= 0)
				{
					array[index++] = new KeyValuePair<TKey, TValue>(array2[i].key, array2[i].value);
				}
			}
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0004299E File Offset: 0x0004199E
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x000429A7 File Offset: 0x000419A7
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<!0, !1>>.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x000429B8 File Offset: 0x000419B8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
			}
			info.AddValue("Version", this.version);
			info.AddValue("Comparer", this.comparer, typeof(IEqualityComparer<TKey>));
			info.AddValue("HashSize", (this.buckets == null) ? 0 : this.buckets.Length);
			if (this.buckets != null)
			{
				KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.Count];
				this.CopyTo(array, 0);
				info.AddValue("KeyValuePairs", array, typeof(KeyValuePair<TKey, TValue>[]));
			}
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00042A4C File Offset: 0x00041A4C
		private int FindEntry(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (this.buckets != null)
			{
				int num = this.comparer.GetHashCode(key) & int.MaxValue;
				for (int i = this.buckets[num % this.buckets.Length]; i >= 0; i = this.entries[i].next)
				{
					if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00042AE4 File Offset: 0x00041AE4
		private void Initialize(int capacity)
		{
			int prime = HashHelpers.GetPrime(capacity);
			this.buckets = new int[prime];
			for (int i = 0; i < this.buckets.Length; i++)
			{
				this.buckets[i] = -1;
			}
			this.entries = new Dictionary<TKey, TValue>.Entry[prime];
			this.freeList = -1;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00042B34 File Offset: 0x00041B34
		private void Insert(TKey key, TValue value, bool add)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (this.buckets == null)
			{
				this.Initialize(0);
			}
			int num = this.comparer.GetHashCode(key) & int.MaxValue;
			int num2 = num % this.buckets.Length;
			for (int i = this.buckets[num2]; i >= 0; i = this.entries[i].next)
			{
				if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
				{
					if (add)
					{
						ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
					}
					this.entries[i].value = value;
					this.version++;
					return;
				}
			}
			int num3;
			if (this.freeCount > 0)
			{
				num3 = this.freeList;
				this.freeList = this.entries[num3].next;
				this.freeCount--;
			}
			else
			{
				if (this.count == this.entries.Length)
				{
					this.Resize();
					num2 = num % this.buckets.Length;
				}
				num3 = this.count;
				this.count++;
			}
			this.entries[num3].hashCode = num;
			this.entries[num3].next = this.buckets[num2];
			this.entries[num3].key = key;
			this.entries[num3].value = value;
			this.buckets[num2] = num3;
			this.version++;
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x00042CD0 File Offset: 0x00041CD0
		public virtual void OnDeserialization(object sender)
		{
			if (this.m_siInfo == null)
			{
				return;
			}
			int @int = this.m_siInfo.GetInt32("Version");
			int int2 = this.m_siInfo.GetInt32("HashSize");
			this.comparer = (IEqualityComparer<TKey>)this.m_siInfo.GetValue("Comparer", typeof(IEqualityComparer<TKey>));
			if (int2 != 0)
			{
				this.buckets = new int[int2];
				for (int i = 0; i < this.buckets.Length; i++)
				{
					this.buckets[i] = -1;
				}
				this.entries = new Dictionary<TKey, TValue>.Entry[int2];
				this.freeList = -1;
				KeyValuePair<TKey, TValue>[] array = (KeyValuePair<TKey, TValue>[])this.m_siInfo.GetValue("KeyValuePairs", typeof(KeyValuePair<TKey, TValue>[]));
				if (array == null)
				{
					ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingKeyValuePairs);
				}
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j].Key == null)
					{
						ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_NullKey);
					}
					this.Insert(array[j].Key, array[j].Value, true);
				}
			}
			else
			{
				this.buckets = null;
			}
			this.version = @int;
			this.m_siInfo = null;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00042E00 File Offset: 0x00041E00
		private void Resize()
		{
			int prime = HashHelpers.GetPrime(this.count * 2);
			int[] array = new int[prime];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = -1;
			}
			Dictionary<TKey, TValue>.Entry[] array2 = new Dictionary<TKey, TValue>.Entry[prime];
			Array.Copy(this.entries, 0, array2, 0, this.count);
			for (int j = 0; j < this.count; j++)
			{
				int num = array2[j].hashCode % prime;
				array2[j].next = array[num];
				array[num] = j;
			}
			this.buckets = array;
			this.entries = array2;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00042E9C File Offset: 0x00041E9C
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (this.buckets != null)
			{
				int num = this.comparer.GetHashCode(key) & int.MaxValue;
				int num2 = num % this.buckets.Length;
				int num3 = -1;
				for (int i = this.buckets[num2]; i >= 0; i = this.entries[i].next)
				{
					if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
					{
						if (num3 < 0)
						{
							this.buckets[num2] = this.entries[i].next;
						}
						else
						{
							this.entries[num3].next = this.entries[i].next;
						}
						this.entries[i].hashCode = -1;
						this.entries[i].next = this.freeList;
						this.entries[i].key = default(TKey);
						this.entries[i].value = default(TValue);
						this.freeList = i;
						this.freeCount++;
						this.version++;
						return true;
					}
					num3 = i;
				}
			}
			return false;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00043004 File Offset: 0x00042004
		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = this.FindEntry(key);
			if (num >= 0)
			{
				value = this.entries[num].value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060019B4 RID: 6580 RVA: 0x0004303E File Offset: 0x0004203E
		bool ICollection<KeyValuePair<!0, !1>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x00043041 File Offset: 0x00042041
		void ICollection<KeyValuePair<!0, !1>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0004304C File Offset: 0x0004204C
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
			if (index < 0 || index > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			if (array is DictionaryEntry[])
			{
				DictionaryEntry[] array3 = array as DictionaryEntry[];
				Dictionary<TKey, TValue>.Entry[] array4 = this.entries;
				for (int i = 0; i < this.count; i++)
				{
					if (array4[i].hashCode >= 0)
					{
						array3[index++] = new DictionaryEntry(array4[i].key, array4[i].value);
					}
				}
				return;
			}
			object[] array5 = array as object[];
			if (array5 == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			try
			{
				int num = this.count;
				Dictionary<TKey, TValue>.Entry[] array6 = this.entries;
				for (int j = 0; j < num; j++)
				{
					if (array6[j].hashCode >= 0)
					{
						array5[index++] = new KeyValuePair<TKey, TValue>(array6[j].key, array6[j].value);
					}
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x000431C0 File Offset: 0x000421C0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060019B8 RID: 6584 RVA: 0x000431CE File Offset: 0x000421CE
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x000431D1 File Offset: 0x000421D1
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

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060019BA RID: 6586 RVA: 0x000431F3 File Offset: 0x000421F3
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x000431F6 File Offset: 0x000421F6
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060019BC RID: 6588 RVA: 0x000431F9 File Offset: 0x000421F9
		ICollection IDictionary.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060019BD RID: 6589 RVA: 0x00043201 File Offset: 0x00042201
		ICollection IDictionary.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x170003E2 RID: 994
		object IDictionary.this[object key]
		{
			get
			{
				if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
				{
					int num = this.FindEntry((TKey)((object)key));
					if (num >= 0)
					{
						return this.entries[num].value;
					}
				}
				return null;
			}
			set
			{
				Dictionary<TKey, TValue>.VerifyKey(key);
				Dictionary<TKey, TValue>.VerifyValueType(value);
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0004326A File Offset: 0x0004226A
		private static void VerifyKey(object key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (!(key is TKey))
			{
				ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
			}
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x0004328D File Offset: 0x0004228D
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return key is TKey;
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x000432A1 File Offset: 0x000422A1
		private static void VerifyValueType(object value)
		{
			if (value is TValue || (value == null && !typeof(TValue).IsValueType))
			{
				return;
			}
			ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x000432D0 File Offset: 0x000422D0
		void IDictionary.Add(object key, object value)
		{
			Dictionary<TKey, TValue>.VerifyKey(key);
			Dictionary<TKey, TValue>.VerifyValueType(value);
			this.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x000432F0 File Offset: 0x000422F0
		bool IDictionary.Contains(object key)
		{
			return Dictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00043308 File Offset: 0x00042308
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00043316 File Offset: 0x00042316
		void IDictionary.Remove(object key)
		{
			if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
			{
				this.Remove((TKey)((object)key));
			}
		}

		// Token: 0x04000A08 RID: 2568
		private const string VersionName = "Version";

		// Token: 0x04000A09 RID: 2569
		private const string HashSizeName = "HashSize";

		// Token: 0x04000A0A RID: 2570
		private const string KeyValuePairsName = "KeyValuePairs";

		// Token: 0x04000A0B RID: 2571
		private const string ComparerName = "Comparer";

		// Token: 0x04000A0C RID: 2572
		private int[] buckets;

		// Token: 0x04000A0D RID: 2573
		private Dictionary<TKey, TValue>.Entry[] entries;

		// Token: 0x04000A0E RID: 2574
		private int count;

		// Token: 0x04000A0F RID: 2575
		private int version;

		// Token: 0x04000A10 RID: 2576
		private int freeList;

		// Token: 0x04000A11 RID: 2577
		private int freeCount;

		// Token: 0x04000A12 RID: 2578
		private IEqualityComparer<TKey> comparer;

		// Token: 0x04000A13 RID: 2579
		private Dictionary<TKey, TValue>.KeyCollection keys;

		// Token: 0x04000A14 RID: 2580
		private Dictionary<TKey, TValue>.ValueCollection values;

		// Token: 0x04000A15 RID: 2581
		private object _syncRoot;

		// Token: 0x04000A16 RID: 2582
		private SerializationInfo m_siInfo;

		// Token: 0x0200028D RID: 653
		private struct Entry
		{
			// Token: 0x04000A17 RID: 2583
			public int hashCode;

			// Token: 0x04000A18 RID: 2584
			public int next;

			// Token: 0x04000A19 RID: 2585
			public TKey key;

			// Token: 0x04000A1A RID: 2586
			public TValue value;
		}

		// Token: 0x0200028E RID: 654
		[Serializable]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060019C7 RID: 6599 RVA: 0x0004332D File Offset: 0x0004232D
			internal Enumerator(Dictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
			{
				this.dictionary = dictionary;
				this.version = dictionary.version;
				this.index = 0;
				this.getEnumeratorRetType = getEnumeratorRetType;
				this.current = default(KeyValuePair<TKey, TValue>);
			}

			// Token: 0x060019C8 RID: 6600 RVA: 0x0004335C File Offset: 0x0004235C
			public bool MoveNext()
			{
				if (this.version != this.dictionary.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				while (this.index < this.dictionary.count)
				{
					if (this.dictionary.entries[this.index].hashCode >= 0)
					{
						this.current = new KeyValuePair<TKey, TValue>(this.dictionary.entries[this.index].key, this.dictionary.entries[this.index].value);
						this.index++;
						return true;
					}
					this.index++;
				}
				this.index = this.dictionary.count + 1;
				this.current = default(KeyValuePair<TKey, TValue>);
				return false;
			}

			// Token: 0x170003E3 RID: 995
			// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0004343B File Offset: 0x0004243B
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x060019CA RID: 6602 RVA: 0x00043443 File Offset: 0x00042443
			public void Dispose()
			{
			}

			// Token: 0x170003E4 RID: 996
			// (get) Token: 0x060019CB RID: 6603 RVA: 0x00043448 File Offset: 0x00042448
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					if (this.getEnumeratorRetType == 1)
					{
						return new DictionaryEntry(this.current.Key, this.current.Value);
					}
					return new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
				}
			}

			// Token: 0x060019CC RID: 6604 RVA: 0x000434CD File Offset: 0x000424CD
			void IEnumerator.Reset()
			{
				if (this.version != this.dictionary.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.current = default(KeyValuePair<TKey, TValue>);
			}

			// Token: 0x170003E5 RID: 997
			// (get) Token: 0x060019CD RID: 6605 RVA: 0x000434FC File Offset: 0x000424FC
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return new DictionaryEntry(this.current.Key, this.current.Value);
				}
			}

			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x060019CE RID: 6606 RVA: 0x00043552 File Offset: 0x00042552
			object IDictionaryEnumerator.Key
			{
				get
				{
					if (this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current.Key;
				}
			}

			// Token: 0x170003E7 RID: 999
			// (get) Token: 0x060019CF RID: 6607 RVA: 0x00043588 File Offset: 0x00042588
			object IDictionaryEnumerator.Value
			{
				get
				{
					if (this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current.Value;
				}
			}

			// Token: 0x04000A1B RID: 2587
			internal const int DictEntry = 1;

			// Token: 0x04000A1C RID: 2588
			internal const int KeyValuePair = 2;

			// Token: 0x04000A1D RID: 2589
			private Dictionary<TKey, TValue> dictionary;

			// Token: 0x04000A1E RID: 2590
			private int version;

			// Token: 0x04000A1F RID: 2591
			private int index;

			// Token: 0x04000A20 RID: 2592
			private KeyValuePair<TKey, TValue> current;

			// Token: 0x04000A21 RID: 2593
			private int getEnumeratorRetType;
		}

		// Token: 0x0200028F RID: 655
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(Mscorlib_DictionaryKeyCollectionDebugView<, >))]
		[Serializable]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
		{
			// Token: 0x060019D0 RID: 6608 RVA: 0x000435BE File Offset: 0x000425BE
			public KeyCollection(Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			// Token: 0x060019D1 RID: 6609 RVA: 0x000435D6 File Offset: 0x000425D6
			public Dictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060019D2 RID: 6610 RVA: 0x000435E4 File Offset: 0x000425E4
			public void CopyTo(TKey[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = this.dictionary.count;
				Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].key;
					}
				}
			}

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x060019D3 RID: 6611 RVA: 0x0004366F File Offset: 0x0004266F
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x170003E9 RID: 1001
			// (get) Token: 0x060019D4 RID: 6612 RVA: 0x0004367C File Offset: 0x0004267C
			bool ICollection<!0>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060019D5 RID: 6613 RVA: 0x0004367F File Offset: 0x0004267F
			void ICollection<!0>.Add(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			// Token: 0x060019D6 RID: 6614 RVA: 0x00043688 File Offset: 0x00042688
			void ICollection<!0>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			// Token: 0x060019D7 RID: 6615 RVA: 0x00043691 File Offset: 0x00042691
			bool ICollection<!0>.Contains(TKey item)
			{
				return this.dictionary.ContainsKey(item);
			}

			// Token: 0x060019D8 RID: 6616 RVA: 0x0004369F File Offset: 0x0004269F
			bool ICollection<!0>.Remove(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
				return false;
			}

			// Token: 0x060019D9 RID: 6617 RVA: 0x000436A9 File Offset: 0x000426A9
			IEnumerator<TKey> IEnumerable<!0>.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060019DA RID: 6618 RVA: 0x000436BB File Offset: 0x000426BB
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060019DB RID: 6619 RVA: 0x000436D0 File Offset: 0x000426D0
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
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TKey[] array2 = array as TKey[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if (array3 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				int count = this.dictionary.count;
				Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].key;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x170003EA RID: 1002
			// (get) Token: 0x060019DC RID: 6620 RVA: 0x000437C8 File Offset: 0x000427C8
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003EB RID: 1003
			// (get) Token: 0x060019DD RID: 6621 RVA: 0x000437CB File Offset: 0x000427CB
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			// Token: 0x04000A22 RID: 2594
			private Dictionary<TKey, TValue> dictionary;

			// Token: 0x02000290 RID: 656
			[Serializable]
			public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
			{
				// Token: 0x060019DE RID: 6622 RVA: 0x000437D8 File Offset: 0x000427D8
				internal Enumerator(Dictionary<TKey, TValue> dictionary)
				{
					this.dictionary = dictionary;
					this.version = dictionary.version;
					this.index = 0;
					this.currentKey = default(TKey);
				}

				// Token: 0x060019DF RID: 6623 RVA: 0x00043800 File Offset: 0x00042800
				public void Dispose()
				{
				}

				// Token: 0x060019E0 RID: 6624 RVA: 0x00043804 File Offset: 0x00042804
				public bool MoveNext()
				{
					if (this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					while (this.index < this.dictionary.count)
					{
						if (this.dictionary.entries[this.index].hashCode >= 0)
						{
							this.currentKey = this.dictionary.entries[this.index].key;
							this.index++;
							return true;
						}
						this.index++;
					}
					this.index = this.dictionary.count + 1;
					this.currentKey = default(TKey);
					return false;
				}

				// Token: 0x170003EC RID: 1004
				// (get) Token: 0x060019E1 RID: 6625 RVA: 0x000438BD File Offset: 0x000428BD
				public TKey Current
				{
					get
					{
						return this.currentKey;
					}
				}

				// Token: 0x170003ED RID: 1005
				// (get) Token: 0x060019E2 RID: 6626 RVA: 0x000438C5 File Offset: 0x000428C5
				object IEnumerator.Current
				{
					get
					{
						if (this.index == 0 || this.index == this.dictionary.count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.currentKey;
					}
				}

				// Token: 0x060019E3 RID: 6627 RVA: 0x000438F6 File Offset: 0x000428F6
				void IEnumerator.Reset()
				{
					if (this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					this.index = 0;
					this.currentKey = default(TKey);
				}

				// Token: 0x04000A23 RID: 2595
				private Dictionary<TKey, TValue> dictionary;

				// Token: 0x04000A24 RID: 2596
				private int index;

				// Token: 0x04000A25 RID: 2597
				private int version;

				// Token: 0x04000A26 RID: 2598
				private TKey currentKey;
			}
		}

		// Token: 0x02000291 RID: 657
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(Mscorlib_DictionaryValueCollectionDebugView<, >))]
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
		{
			// Token: 0x060019E4 RID: 6628 RVA: 0x00043925 File Offset: 0x00042925
			public ValueCollection(Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			// Token: 0x060019E5 RID: 6629 RVA: 0x0004393D File Offset: 0x0004293D
			public Dictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060019E6 RID: 6630 RVA: 0x0004394C File Offset: 0x0004294C
			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = this.dictionary.count;
				Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].value;
					}
				}
			}

			// Token: 0x170003EE RID: 1006
			// (get) Token: 0x060019E7 RID: 6631 RVA: 0x000439D7 File Offset: 0x000429D7
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x170003EF RID: 1007
			// (get) Token: 0x060019E8 RID: 6632 RVA: 0x000439E4 File Offset: 0x000429E4
			bool ICollection<!1>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060019E9 RID: 6633 RVA: 0x000439E7 File Offset: 0x000429E7
			void ICollection<!1>.Add(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			// Token: 0x060019EA RID: 6634 RVA: 0x000439F0 File Offset: 0x000429F0
			bool ICollection<!1>.Remove(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
				return false;
			}

			// Token: 0x060019EB RID: 6635 RVA: 0x000439FA File Offset: 0x000429FA
			void ICollection<!1>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			// Token: 0x060019EC RID: 6636 RVA: 0x00043A03 File Offset: 0x00042A03
			bool ICollection<!1>.Contains(TValue item)
			{
				return this.dictionary.ContainsValue(item);
			}

			// Token: 0x060019ED RID: 6637 RVA: 0x00043A11 File Offset: 0x00042A11
			IEnumerator<TValue> IEnumerable<!1>.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060019EE RID: 6638 RVA: 0x00043A23 File Offset: 0x00042A23
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060019EF RID: 6639 RVA: 0x00043A38 File Offset: 0x00042A38
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
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TValue[] array2 = array as TValue[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if (array3 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				int count = this.dictionary.count;
				Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].value;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x170003F0 RID: 1008
			// (get) Token: 0x060019F0 RID: 6640 RVA: 0x00043B30 File Offset: 0x00042B30
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003F1 RID: 1009
			// (get) Token: 0x060019F1 RID: 6641 RVA: 0x00043B33 File Offset: 0x00042B33
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			// Token: 0x04000A27 RID: 2599
			private Dictionary<TKey, TValue> dictionary;

			// Token: 0x02000292 RID: 658
			[Serializable]
			public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
			{
				// Token: 0x060019F2 RID: 6642 RVA: 0x00043B40 File Offset: 0x00042B40
				internal Enumerator(Dictionary<TKey, TValue> dictionary)
				{
					this.dictionary = dictionary;
					this.version = dictionary.version;
					this.index = 0;
					this.currentValue = default(TValue);
				}

				// Token: 0x060019F3 RID: 6643 RVA: 0x00043B68 File Offset: 0x00042B68
				public void Dispose()
				{
				}

				// Token: 0x060019F4 RID: 6644 RVA: 0x00043B6C File Offset: 0x00042B6C
				public bool MoveNext()
				{
					if (this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					while (this.index < this.dictionary.count)
					{
						if (this.dictionary.entries[this.index].hashCode >= 0)
						{
							this.currentValue = this.dictionary.entries[this.index].value;
							this.index++;
							return true;
						}
						this.index++;
					}
					this.index = this.dictionary.count + 1;
					this.currentValue = default(TValue);
					return false;
				}

				// Token: 0x170003F2 RID: 1010
				// (get) Token: 0x060019F5 RID: 6645 RVA: 0x00043C25 File Offset: 0x00042C25
				public TValue Current
				{
					get
					{
						return this.currentValue;
					}
				}

				// Token: 0x170003F3 RID: 1011
				// (get) Token: 0x060019F6 RID: 6646 RVA: 0x00043C2D File Offset: 0x00042C2D
				object IEnumerator.Current
				{
					get
					{
						if (this.index == 0 || this.index == this.dictionary.count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.currentValue;
					}
				}

				// Token: 0x060019F7 RID: 6647 RVA: 0x00043C5E File Offset: 0x00042C5E
				void IEnumerator.Reset()
				{
					if (this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					this.index = 0;
					this.currentValue = default(TValue);
				}

				// Token: 0x04000A28 RID: 2600
				private Dictionary<TKey, TValue> dictionary;

				// Token: 0x04000A29 RID: 2601
				private int index;

				// Token: 0x04000A2A RID: 2602
				private int version;

				// Token: 0x04000A2B RID: 2603
				private TValue currentValue;
			}
		}
	}
}
