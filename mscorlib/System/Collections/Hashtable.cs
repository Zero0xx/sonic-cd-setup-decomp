using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000266 RID: 614
	[DebuggerTypeProxy(typeof(Hashtable.HashtableDebugView))]
	[ComVisible(true)]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class Hashtable : IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback, ICloneable
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x0003D4B7 File Offset: 0x0003C4B7
		// (set) Token: 0x0600180D RID: 6157 RVA: 0x0003D4F0 File Offset: 0x0003C4F0
		[Obsolete("Please use EqualityComparer property.")]
		protected IHashCodeProvider hcp
		{
			get
			{
				if (this._keycomparer is CompatibleComparer)
				{
					return ((CompatibleComparer)this._keycomparer).HashCodeProvider;
				}
				if (this._keycomparer == null)
				{
					return null;
				}
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotMixComparisonInfrastructure"));
			}
			set
			{
				if (this._keycomparer is CompatibleComparer)
				{
					CompatibleComparer compatibleComparer = (CompatibleComparer)this._keycomparer;
					this._keycomparer = new CompatibleComparer(compatibleComparer.Comparer, value);
					return;
				}
				if (this._keycomparer == null)
				{
					this._keycomparer = new CompatibleComparer(null, value);
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotMixComparisonInfrastructure"));
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x0003D54E File Offset: 0x0003C54E
		// (set) Token: 0x0600180F RID: 6159 RVA: 0x0003D588 File Offset: 0x0003C588
		[Obsolete("Please use KeyComparer properties.")]
		protected IComparer comparer
		{
			get
			{
				if (this._keycomparer is CompatibleComparer)
				{
					return ((CompatibleComparer)this._keycomparer).Comparer;
				}
				if (this._keycomparer == null)
				{
					return null;
				}
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotMixComparisonInfrastructure"));
			}
			set
			{
				if (this._keycomparer is CompatibleComparer)
				{
					CompatibleComparer compatibleComparer = (CompatibleComparer)this._keycomparer;
					this._keycomparer = new CompatibleComparer(value, compatibleComparer.HashCodeProvider);
					return;
				}
				if (this._keycomparer == null)
				{
					this._keycomparer = new CompatibleComparer(value, null);
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotMixComparisonInfrastructure"));
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x0003D5E6 File Offset: 0x0003C5E6
		protected IEqualityComparer EqualityComparer
		{
			get
			{
				return this._keycomparer;
			}
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x0003D5EE File Offset: 0x0003C5EE
		internal Hashtable(bool trash)
		{
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0003D5F6 File Offset: 0x0003C5F6
		public Hashtable() : this(0, 1f)
		{
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0003D604 File Offset: 0x0003C604
		public Hashtable(int capacity) : this(capacity, 1f)
		{
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x0003D614 File Offset: 0x0003C614
		public Hashtable(int capacity, float loadFactor)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (loadFactor < 0.1f || loadFactor > 1f)
			{
				throw new ArgumentOutOfRangeException("loadFactor", Environment.GetResourceString("ArgumentOutOfRange_HashtableLoadFactor", new object[]
				{
					0.1,
					1.0
				}));
			}
			this.loadFactor = 0.72f * loadFactor;
			double num = (double)((float)capacity / this.loadFactor);
			if (num > 2147483647.0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_HTCapacityOverflow"));
			}
			int num2 = (num > 11.0) ? HashHelpers.GetPrime((int)num) : 11;
			this.buckets = new Hashtable.bucket[num2];
			this.loadsize = (int)(this.loadFactor * (float)num2);
			this.isWriterInProgress = false;
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0003D700 File Offset: 0x0003C700
		[Obsolete("Please use Hashtable(int, float, IEqualityComparer) instead.")]
		public Hashtable(int capacity, float loadFactor, IHashCodeProvider hcp, IComparer comparer) : this(capacity, loadFactor)
		{
			if (hcp == null && comparer == null)
			{
				this._keycomparer = null;
				return;
			}
			this._keycomparer = new CompatibleComparer(comparer, hcp);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0003D727 File Offset: 0x0003C727
		public Hashtable(int capacity, float loadFactor, IEqualityComparer equalityComparer) : this(capacity, loadFactor)
		{
			this._keycomparer = equalityComparer;
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0003D738 File Offset: 0x0003C738
		[Obsolete("Please use Hashtable(IEqualityComparer) instead.")]
		public Hashtable(IHashCodeProvider hcp, IComparer comparer) : this(0, 1f, hcp, comparer)
		{
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0003D748 File Offset: 0x0003C748
		public Hashtable(IEqualityComparer equalityComparer) : this(0, 1f, equalityComparer)
		{
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0003D757 File Offset: 0x0003C757
		[Obsolete("Please use Hashtable(int, IEqualityComparer) instead.")]
		public Hashtable(int capacity, IHashCodeProvider hcp, IComparer comparer) : this(capacity, 1f, hcp, comparer)
		{
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0003D767 File Offset: 0x0003C767
		public Hashtable(int capacity, IEqualityComparer equalityComparer) : this(capacity, 1f, equalityComparer)
		{
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0003D776 File Offset: 0x0003C776
		public Hashtable(IDictionary d) : this(d, 1f)
		{
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0003D784 File Offset: 0x0003C784
		public Hashtable(IDictionary d, float loadFactor) : this(d, loadFactor, null)
		{
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0003D78F File Offset: 0x0003C78F
		[Obsolete("Please use Hashtable(IDictionary, IEqualityComparer) instead.")]
		public Hashtable(IDictionary d, IHashCodeProvider hcp, IComparer comparer) : this(d, 1f, hcp, comparer)
		{
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0003D79F File Offset: 0x0003C79F
		public Hashtable(IDictionary d, IEqualityComparer equalityComparer) : this(d, 1f, equalityComparer)
		{
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0003D7B0 File Offset: 0x0003C7B0
		[Obsolete("Please use Hashtable(IDictionary, float, IEqualityComparer) instead.")]
		public Hashtable(IDictionary d, float loadFactor, IHashCodeProvider hcp, IComparer comparer) : this((d != null) ? d.Count : 0, loadFactor, hcp, comparer)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d", Environment.GetResourceString("ArgumentNull_Dictionary"));
			}
			IDictionaryEnumerator enumerator = d.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.Add(enumerator.Key, enumerator.Value);
			}
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0003D810 File Offset: 0x0003C810
		public Hashtable(IDictionary d, float loadFactor, IEqualityComparer equalityComparer) : this((d != null) ? d.Count : 0, loadFactor, equalityComparer)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d", Environment.GetResourceString("ArgumentNull_Dictionary"));
			}
			IDictionaryEnumerator enumerator = d.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.Add(enumerator.Key, enumerator.Value);
			}
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0003D86C File Offset: 0x0003C86C
		protected Hashtable(SerializationInfo info, StreamingContext context)
		{
			this.m_siInfo = info;
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0003D87C File Offset: 0x0003C87C
		private uint InitHash(object key, int hashsize, out uint seed, out uint incr)
		{
			uint num = (uint)(this.GetHash(key) & int.MaxValue);
			seed = num;
			incr = 1U + ((seed >> 5) + 1U) % (uint)(hashsize - 1);
			return num;
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0003D8AA File Offset: 0x0003C8AA
		public virtual void Add(object key, object value)
		{
			this.Insert(key, value, true);
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0003D8B8 File Offset: 0x0003C8B8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public virtual void Clear()
		{
			if (this.count == 0)
			{
				return;
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			for (int i = 0; i < this.buckets.Length; i++)
			{
				this.buckets[i].hash_coll = 0;
				this.buckets[i].key = null;
				this.buckets[i].val = null;
			}
			this.count = 0;
			this.occupancy = 0;
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0003D948 File Offset: 0x0003C948
		public virtual object Clone()
		{
			Hashtable.bucket[] array = this.buckets;
			Hashtable hashtable = new Hashtable(this.count, this._keycomparer);
			hashtable.version = this.version;
			hashtable.loadFactor = this.loadFactor;
			hashtable.count = 0;
			int i = array.Length;
			while (i > 0)
			{
				i--;
				object key = array[i].key;
				if (key != null && key != array)
				{
					hashtable[key] = array[i].val;
				}
			}
			return hashtable;
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0003D9C7 File Offset: 0x0003C9C7
		public virtual bool Contains(object key)
		{
			return this.ContainsKey(key);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0003D9D0 File Offset: 0x0003C9D0
		public virtual bool ContainsKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			Hashtable.bucket[] array = this.buckets;
			uint num2;
			uint num3;
			uint num = this.InitHash(key, array.Length, out num2, out num3);
			int num4 = 0;
			int num5 = (int)(num2 % (uint)array.Length);
			for (;;)
			{
				Hashtable.bucket bucket = array[num5];
				if (bucket.key == null)
				{
					break;
				}
				if ((long)(bucket.hash_coll & 2147483647) == (long)((ulong)num) && this.KeyEquals(bucket.key, key))
				{
					return true;
				}
				num5 = (int)(((long)num5 + (long)((ulong)num3)) % (long)((ulong)array.Length));
				if (bucket.hash_coll >= 0 || ++num4 >= array.Length)
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0003DA78 File Offset: 0x0003CA78
		public virtual bool ContainsValue(object value)
		{
			if (value == null)
			{
				int num = this.buckets.Length;
				while (--num >= 0)
				{
					if (this.buckets[num].key != null && this.buckets[num].key != this.buckets && this.buckets[num].val == null)
					{
						return true;
					}
				}
			}
			else
			{
				int num2 = this.buckets.Length;
				while (--num2 >= 0)
				{
					object val = this.buckets[num2].val;
					if (val != null && val.Equals(value))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0003DB14 File Offset: 0x0003CB14
		private void CopyKeys(Array array, int arrayIndex)
		{
			Hashtable.bucket[] array2 = this.buckets;
			int num = array2.Length;
			while (--num >= 0)
			{
				object key = array2[num].key;
				if (key != null && key != this.buckets)
				{
					array.SetValue(key, arrayIndex++);
				}
			}
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0003DB5C File Offset: 0x0003CB5C
		private void CopyEntries(Array array, int arrayIndex)
		{
			Hashtable.bucket[] array2 = this.buckets;
			int num = array2.Length;
			while (--num >= 0)
			{
				object key = array2[num].key;
				if (key != null && key != this.buckets)
				{
					DictionaryEntry dictionaryEntry = new DictionaryEntry(key, array2[num].val);
					array.SetValue(dictionaryEntry, arrayIndex++);
				}
			}
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0003DBC0 File Offset: 0x0003CBC0
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
			if (array.Length - arrayIndex < this.count)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
			}
			this.CopyEntries(array, arrayIndex);
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0003DC40 File Offset: 0x0003CC40
		internal virtual KeyValuePairs[] ToKeyValuePairsArray()
		{
			KeyValuePairs[] array = new KeyValuePairs[this.count];
			int num = 0;
			Hashtable.bucket[] array2 = this.buckets;
			int num2 = array2.Length;
			while (--num2 >= 0)
			{
				object key = array2[num2].key;
				if (key != null && key != this.buckets)
				{
					array[num++] = new KeyValuePairs(key, array2[num2].val);
				}
			}
			return array;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0003DCA8 File Offset: 0x0003CCA8
		private void CopyValues(Array array, int arrayIndex)
		{
			Hashtable.bucket[] array2 = this.buckets;
			int num = array2.Length;
			while (--num >= 0)
			{
				object key = array2[num].key;
				if (key != null && key != this.buckets)
				{
					array.SetValue(array2[num].val, arrayIndex++);
				}
			}
		}

		// Token: 0x17000368 RID: 872
		public virtual object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				Hashtable.bucket[] array = this.buckets;
				uint num2;
				uint num3;
				uint num = this.InitHash(key, array.Length, out num2, out num3);
				int num4 = 0;
				int num5 = (int)(num2 % (uint)array.Length);
				Hashtable.bucket bucket;
				for (;;)
				{
					int num6 = 0;
					int num7;
					do
					{
						num7 = this.version;
						bucket = array[num5];
						if (++num6 % 8 == 0)
						{
							Thread.Sleep(1);
						}
					}
					while (this.isWriterInProgress || num7 != this.version);
					if (bucket.key == null)
					{
						break;
					}
					if ((long)(bucket.hash_coll & 2147483647) == (long)((ulong)num) && this.KeyEquals(bucket.key, key))
					{
						goto Block_7;
					}
					num5 = (int)(((long)num5 + (long)((ulong)num3)) % (long)((ulong)array.Length));
					if (bucket.hash_coll >= 0 || ++num4 >= array.Length)
					{
						goto IL_D7;
					}
				}
				return null;
				Block_7:
				return bucket.val;
				IL_D7:
				return null;
			}
			set
			{
				this.Insert(key, value, false);
			}
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0003DDEC File Offset: 0x0003CDEC
		private void expand()
		{
			int prime = HashHelpers.GetPrime(this.buckets.Length * 2);
			this.rehash(prime);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0003DE10 File Offset: 0x0003CE10
		private void rehash()
		{
			this.rehash(this.buckets.Length);
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0003DE20 File Offset: 0x0003CE20
		private void UpdateVersion()
		{
			this.version++;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0003DE34 File Offset: 0x0003CE34
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private void rehash(int newsize)
		{
			this.occupancy = 0;
			Hashtable.bucket[] newBuckets = new Hashtable.bucket[newsize];
			for (int i = 0; i < this.buckets.Length; i++)
			{
				Hashtable.bucket bucket = this.buckets[i];
				if (bucket.key != null && bucket.key != this.buckets)
				{
					this.putEntry(newBuckets, bucket.key, bucket.val, bucket.hash_coll & int.MaxValue);
				}
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			this.buckets = newBuckets;
			this.loadsize = (int)(this.loadFactor * (float)newsize);
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0003DEE7 File Offset: 0x0003CEE7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Hashtable.HashtableEnumerator(this, 3);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0003DEF0 File Offset: 0x0003CEF0
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new Hashtable.HashtableEnumerator(this, 3);
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0003DEF9 File Offset: 0x0003CEF9
		protected virtual int GetHash(object key)
		{
			if (this._keycomparer != null)
			{
				return this._keycomparer.GetHashCode(key);
			}
			return key.GetHashCode();
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001837 RID: 6199 RVA: 0x0003DF16 File Offset: 0x0003CF16
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001838 RID: 6200 RVA: 0x0003DF19 File Offset: 0x0003CF19
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x0003DF1C File Offset: 0x0003CF1C
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x0003DF1F File Offset: 0x0003CF1F
		protected virtual bool KeyEquals(object item, object key)
		{
			if (object.ReferenceEquals(this.buckets, item))
			{
				return false;
			}
			if (this._keycomparer != null)
			{
				return this._keycomparer.Equals(item, key);
			}
			return item != null && item.Equals(key);
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600183B RID: 6203 RVA: 0x0003DF53 File Offset: 0x0003CF53
		public virtual ICollection Keys
		{
			get
			{
				if (this.keys == null)
				{
					this.keys = new Hashtable.KeyCollection(this);
				}
				return this.keys;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600183C RID: 6204 RVA: 0x0003DF6F File Offset: 0x0003CF6F
		public virtual ICollection Values
		{
			get
			{
				if (this.values == null)
				{
					this.values = new Hashtable.ValueCollection(this);
				}
				return this.values;
			}
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0003DF8C File Offset: 0x0003CF8C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private void Insert(object key, object nvalue, bool add)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			if (this.count >= this.loadsize)
			{
				this.expand();
			}
			else if (this.occupancy > this.loadsize && this.count > 100)
			{
				this.rehash();
			}
			uint num2;
			uint num3;
			uint num = this.InitHash(key, this.buckets.Length, out num2, out num3);
			int num4 = 0;
			int num5 = -1;
			int num6 = (int)(num2 % (uint)this.buckets.Length);
			for (;;)
			{
				if (num5 == -1 && this.buckets[num6].key == this.buckets && this.buckets[num6].hash_coll < 0)
				{
					num5 = num6;
				}
				if (this.buckets[num6].key == null || (this.buckets[num6].key == this.buckets && ((long)this.buckets[num6].hash_coll & (long)((ulong)-2147483648)) == 0L))
				{
					break;
				}
				if ((long)(this.buckets[num6].hash_coll & 2147483647) == (long)((ulong)num) && this.KeyEquals(this.buckets[num6].key, key))
				{
					goto Block_12;
				}
				if (num5 == -1 && this.buckets[num6].hash_coll >= 0)
				{
					Hashtable.bucket[] array = this.buckets;
					int num7 = num6;
					array[num7].hash_coll = (array[num7].hash_coll | int.MinValue);
					this.occupancy++;
				}
				num6 = (int)(((long)num6 + (long)((ulong)num3)) % (long)((ulong)this.buckets.Length));
				if (++num4 >= this.buckets.Length)
				{
					goto Block_16;
				}
			}
			if (num5 != -1)
			{
				num6 = num5;
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			this.buckets[num6].val = nvalue;
			this.buckets[num6].key = key;
			Hashtable.bucket[] array2 = this.buckets;
			int num8 = num6;
			array2[num8].hash_coll = (array2[num8].hash_coll | (int)num);
			this.count++;
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
			return;
			Block_12:
			if (add)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_AddingDuplicate__", new object[]
				{
					this.buckets[num6].key,
					key
				}));
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			this.buckets[num6].val = nvalue;
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
			return;
			Block_16:
			if (num5 != -1)
			{
				Thread.BeginCriticalRegion();
				this.isWriterInProgress = true;
				this.buckets[num5].val = nvalue;
				this.buckets[num5].key = key;
				Hashtable.bucket[] array3 = this.buckets;
				int num9 = num5;
				array3[num9].hash_coll = (array3[num9].hash_coll | (int)num);
				this.count++;
				this.UpdateVersion();
				this.isWriterInProgress = false;
				Thread.EndCriticalRegion();
				return;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HashInsertFailed"));
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0003E2A4 File Offset: 0x0003D2A4
		private void putEntry(Hashtable.bucket[] newBuckets, object key, object nvalue, int hashcode)
		{
			uint num = 1U + (((uint)hashcode >> 5) + 1U) % (uint)(newBuckets.Length - 1);
			int num2 = hashcode % newBuckets.Length;
			while (newBuckets[num2].key != null && newBuckets[num2].key != this.buckets)
			{
				if (newBuckets[num2].hash_coll >= 0)
				{
					int num3 = num2;
					newBuckets[num3].hash_coll = (newBuckets[num3].hash_coll | int.MinValue);
					this.occupancy++;
				}
				num2 = (int)(((long)num2 + (long)((ulong)num)) % (long)((ulong)newBuckets.Length));
			}
			newBuckets[num2].val = nvalue;
			newBuckets[num2].key = key;
			int num4 = num2;
			newBuckets[num4].hash_coll = (newBuckets[num4].hash_coll | hashcode);
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0003E360 File Offset: 0x0003D360
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public virtual void Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			uint num2;
			uint num3;
			uint num = this.InitHash(key, this.buckets.Length, out num2, out num3);
			int num4 = 0;
			int num5 = (int)(num2 % (uint)this.buckets.Length);
			for (;;)
			{
				Hashtable.bucket bucket = this.buckets[num5];
				if ((long)(bucket.hash_coll & 2147483647) == (long)((ulong)num) && this.KeyEquals(bucket.key, key))
				{
					break;
				}
				num5 = (int)(((long)num5 + (long)((ulong)num3)) % (long)((ulong)this.buckets.Length));
				if (bucket.hash_coll >= 0 || ++num4 >= this.buckets.Length)
				{
					return;
				}
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			Hashtable.bucket[] array = this.buckets;
			int num6 = num5;
			array[num6].hash_coll = (array[num6].hash_coll & int.MinValue);
			if (this.buckets[num5].hash_coll != 0)
			{
				this.buckets[num5].key = this.buckets;
			}
			else
			{
				this.buckets[num5].key = null;
			}
			this.buckets[num5].val = null;
			this.count--;
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001840 RID: 6208 RVA: 0x0003E4B5 File Offset: 0x0003D4B5
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

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001841 RID: 6209 RVA: 0x0003E4D7 File Offset: 0x0003D4D7
		public virtual int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0003E4DF File Offset: 0x0003D4DF
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Hashtable Synchronized(Hashtable table)
		{
			if (table == null)
			{
				throw new ArgumentNullException("table");
			}
			return new Hashtable.SyncHashtable(table);
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0003E4F8 File Offset: 0x0003D4F8
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("LoadFactor", this.loadFactor);
			info.AddValue("Version", this.version);
			if (this._keycomparer == null)
			{
				info.AddValue("Comparer", null, typeof(IComparer));
				info.AddValue("HashCodeProvider", null, typeof(IHashCodeProvider));
			}
			else if (this._keycomparer is CompatibleComparer)
			{
				CompatibleComparer compatibleComparer = this._keycomparer as CompatibleComparer;
				info.AddValue("Comparer", compatibleComparer.Comparer, typeof(IComparer));
				info.AddValue("HashCodeProvider", compatibleComparer.HashCodeProvider, typeof(IHashCodeProvider));
			}
			else
			{
				info.AddValue("KeyComparer", this._keycomparer, typeof(IEqualityComparer));
			}
			info.AddValue("HashSize", this.buckets.Length);
			object[] array = new object[this.count];
			object[] array2 = new object[this.count];
			this.CopyKeys(array, 0);
			this.CopyValues(array2, 0);
			info.AddValue("Keys", array, typeof(object[]));
			info.AddValue("Values", array2, typeof(object[]));
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x0003E640 File Offset: 0x0003D640
		public virtual void OnDeserialization(object sender)
		{
			if (this.buckets != null)
			{
				return;
			}
			if (this.m_siInfo == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidOnDeser"));
			}
			int num = 0;
			IComparer comparer = null;
			IHashCodeProvider hashCodeProvider = null;
			object[] array = null;
			object[] array2 = null;
			SerializationInfoEnumerator enumerator = this.m_siInfo.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				switch (name = enumerator.Name)
				{
				case "LoadFactor":
					this.loadFactor = this.m_siInfo.GetSingle("LoadFactor");
					break;
				case "HashSize":
					num = this.m_siInfo.GetInt32("HashSize");
					break;
				case "KeyComparer":
					this._keycomparer = (IEqualityComparer)this.m_siInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
					break;
				case "Comparer":
					comparer = (IComparer)this.m_siInfo.GetValue("Comparer", typeof(IComparer));
					break;
				case "HashCodeProvider":
					hashCodeProvider = (IHashCodeProvider)this.m_siInfo.GetValue("HashCodeProvider", typeof(IHashCodeProvider));
					break;
				case "Keys":
					array = (object[])this.m_siInfo.GetValue("Keys", typeof(object[]));
					break;
				case "Values":
					array2 = (object[])this.m_siInfo.GetValue("Values", typeof(object[]));
					break;
				}
			}
			this.loadsize = (int)(this.loadFactor * (float)num);
			if (this._keycomparer == null && (comparer != null || hashCodeProvider != null))
			{
				this._keycomparer = new CompatibleComparer(comparer, hashCodeProvider);
			}
			this.buckets = new Hashtable.bucket[num];
			if (array == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_MissingKeys"));
			}
			if (array2 == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_MissingValues"));
			}
			if (array.Length != array2.Length)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_KeyValueDifferentSizes"));
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_NullKey"));
				}
				this.Insert(array[i], array2[i], true);
			}
			this.version = this.m_siInfo.GetInt32("Version");
			this.m_siInfo = null;
		}

		// Token: 0x04000991 RID: 2449
		private const string LoadFactorName = "LoadFactor";

		// Token: 0x04000992 RID: 2450
		private const string VersionName = "Version";

		// Token: 0x04000993 RID: 2451
		private const string ComparerName = "Comparer";

		// Token: 0x04000994 RID: 2452
		private const string HashCodeProviderName = "HashCodeProvider";

		// Token: 0x04000995 RID: 2453
		private const string HashSizeName = "HashSize";

		// Token: 0x04000996 RID: 2454
		private const string KeysName = "Keys";

		// Token: 0x04000997 RID: 2455
		private const string ValuesName = "Values";

		// Token: 0x04000998 RID: 2456
		private const string KeyComparerName = "KeyComparer";

		// Token: 0x04000999 RID: 2457
		private Hashtable.bucket[] buckets;

		// Token: 0x0400099A RID: 2458
		private int count;

		// Token: 0x0400099B RID: 2459
		private int occupancy;

		// Token: 0x0400099C RID: 2460
		private int loadsize;

		// Token: 0x0400099D RID: 2461
		private float loadFactor;

		// Token: 0x0400099E RID: 2462
		private volatile int version;

		// Token: 0x0400099F RID: 2463
		private volatile bool isWriterInProgress;

		// Token: 0x040009A0 RID: 2464
		private ICollection keys;

		// Token: 0x040009A1 RID: 2465
		private ICollection values;

		// Token: 0x040009A2 RID: 2466
		private IEqualityComparer _keycomparer;

		// Token: 0x040009A3 RID: 2467
		private object _syncRoot;

		// Token: 0x040009A4 RID: 2468
		private SerializationInfo m_siInfo;

		// Token: 0x02000267 RID: 615
		private struct bucket
		{
			// Token: 0x040009A5 RID: 2469
			public object key;

			// Token: 0x040009A6 RID: 2470
			public object val;

			// Token: 0x040009A7 RID: 2471
			public int hash_coll;
		}

		// Token: 0x02000268 RID: 616
		[Serializable]
		private class KeyCollection : ICollection, IEnumerable
		{
			// Token: 0x06001845 RID: 6213 RVA: 0x0003E8FD File Offset: 0x0003D8FD
			internal KeyCollection(Hashtable hashtable)
			{
				this._hashtable = hashtable;
			}

			// Token: 0x06001846 RID: 6214 RVA: 0x0003E90C File Offset: 0x0003D90C
			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < this._hashtable.count)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
				}
				this._hashtable.CopyKeys(array, arrayIndex);
			}

			// Token: 0x06001847 RID: 6215 RVA: 0x0003E98B File Offset: 0x0003D98B
			public virtual IEnumerator GetEnumerator()
			{
				return new Hashtable.HashtableEnumerator(this._hashtable, 1);
			}

			// Token: 0x17000370 RID: 880
			// (get) Token: 0x06001848 RID: 6216 RVA: 0x0003E999 File Offset: 0x0003D999
			public virtual bool IsSynchronized
			{
				get
				{
					return this._hashtable.IsSynchronized;
				}
			}

			// Token: 0x17000371 RID: 881
			// (get) Token: 0x06001849 RID: 6217 RVA: 0x0003E9A6 File Offset: 0x0003D9A6
			public virtual object SyncRoot
			{
				get
				{
					return this._hashtable.SyncRoot;
				}
			}

			// Token: 0x17000372 RID: 882
			// (get) Token: 0x0600184A RID: 6218 RVA: 0x0003E9B3 File Offset: 0x0003D9B3
			public virtual int Count
			{
				get
				{
					return this._hashtable.count;
				}
			}

			// Token: 0x040009A8 RID: 2472
			private Hashtable _hashtable;
		}

		// Token: 0x02000269 RID: 617
		[Serializable]
		private class ValueCollection : ICollection, IEnumerable
		{
			// Token: 0x0600184B RID: 6219 RVA: 0x0003E9C0 File Offset: 0x0003D9C0
			internal ValueCollection(Hashtable hashtable)
			{
				this._hashtable = hashtable;
			}

			// Token: 0x0600184C RID: 6220 RVA: 0x0003E9D0 File Offset: 0x0003D9D0
			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < this._hashtable.count)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
				}
				this._hashtable.CopyValues(array, arrayIndex);
			}

			// Token: 0x0600184D RID: 6221 RVA: 0x0003EA4F File Offset: 0x0003DA4F
			public virtual IEnumerator GetEnumerator()
			{
				return new Hashtable.HashtableEnumerator(this._hashtable, 2);
			}

			// Token: 0x17000373 RID: 883
			// (get) Token: 0x0600184E RID: 6222 RVA: 0x0003EA5D File Offset: 0x0003DA5D
			public virtual bool IsSynchronized
			{
				get
				{
					return this._hashtable.IsSynchronized;
				}
			}

			// Token: 0x17000374 RID: 884
			// (get) Token: 0x0600184F RID: 6223 RVA: 0x0003EA6A File Offset: 0x0003DA6A
			public virtual object SyncRoot
			{
				get
				{
					return this._hashtable.SyncRoot;
				}
			}

			// Token: 0x17000375 RID: 885
			// (get) Token: 0x06001850 RID: 6224 RVA: 0x0003EA77 File Offset: 0x0003DA77
			public virtual int Count
			{
				get
				{
					return this._hashtable.count;
				}
			}

			// Token: 0x040009A9 RID: 2473
			private Hashtable _hashtable;
		}

		// Token: 0x0200026A RID: 618
		[Serializable]
		private class SyncHashtable : Hashtable
		{
			// Token: 0x06001851 RID: 6225 RVA: 0x0003EA84 File Offset: 0x0003DA84
			internal SyncHashtable(Hashtable table) : base(false)
			{
				this._table = table;
			}

			// Token: 0x06001852 RID: 6226 RVA: 0x0003EA94 File Offset: 0x0003DA94
			internal SyncHashtable(SerializationInfo info, StreamingContext context) : base(info, context)
			{
				this._table = (Hashtable)info.GetValue("ParentTable", typeof(Hashtable));
				if (this._table == null)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
				}
			}

			// Token: 0x06001853 RID: 6227 RVA: 0x0003EAE1 File Offset: 0x0003DAE1
			public override void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("ParentTable", this._table, typeof(Hashtable));
			}

			// Token: 0x17000376 RID: 886
			// (get) Token: 0x06001854 RID: 6228 RVA: 0x0003EB0C File Offset: 0x0003DB0C
			public override int Count
			{
				get
				{
					return this._table.Count;
				}
			}

			// Token: 0x17000377 RID: 887
			// (get) Token: 0x06001855 RID: 6229 RVA: 0x0003EB19 File Offset: 0x0003DB19
			public override bool IsReadOnly
			{
				get
				{
					return this._table.IsReadOnly;
				}
			}

			// Token: 0x17000378 RID: 888
			// (get) Token: 0x06001856 RID: 6230 RVA: 0x0003EB26 File Offset: 0x0003DB26
			public override bool IsFixedSize
			{
				get
				{
					return this._table.IsFixedSize;
				}
			}

			// Token: 0x17000379 RID: 889
			// (get) Token: 0x06001857 RID: 6231 RVA: 0x0003EB33 File Offset: 0x0003DB33
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700037A RID: 890
			public override object this[object key]
			{
				get
				{
					return this._table[key];
				}
				set
				{
					lock (this._table.SyncRoot)
					{
						this._table[key] = value;
					}
				}
			}

			// Token: 0x1700037B RID: 891
			// (get) Token: 0x0600185A RID: 6234 RVA: 0x0003EB8C File Offset: 0x0003DB8C
			public override object SyncRoot
			{
				get
				{
					return this._table.SyncRoot;
				}
			}

			// Token: 0x0600185B RID: 6235 RVA: 0x0003EB9C File Offset: 0x0003DB9C
			public override void Add(object key, object value)
			{
				lock (this._table.SyncRoot)
				{
					this._table.Add(key, value);
				}
			}

			// Token: 0x0600185C RID: 6236 RVA: 0x0003EBE4 File Offset: 0x0003DBE4
			public override void Clear()
			{
				lock (this._table.SyncRoot)
				{
					this._table.Clear();
				}
			}

			// Token: 0x0600185D RID: 6237 RVA: 0x0003EC28 File Offset: 0x0003DC28
			public override bool Contains(object key)
			{
				return this._table.Contains(key);
			}

			// Token: 0x0600185E RID: 6238 RVA: 0x0003EC36 File Offset: 0x0003DC36
			public override bool ContainsKey(object key)
			{
				return this._table.ContainsKey(key);
			}

			// Token: 0x0600185F RID: 6239 RVA: 0x0003EC44 File Offset: 0x0003DC44
			public override bool ContainsValue(object key)
			{
				bool result;
				lock (this._table.SyncRoot)
				{
					result = this._table.ContainsValue(key);
				}
				return result;
			}

			// Token: 0x06001860 RID: 6240 RVA: 0x0003EC8C File Offset: 0x0003DC8C
			public override void CopyTo(Array array, int arrayIndex)
			{
				lock (this._table.SyncRoot)
				{
					this._table.CopyTo(array, arrayIndex);
				}
			}

			// Token: 0x06001861 RID: 6241 RVA: 0x0003ECD4 File Offset: 0x0003DCD4
			public override object Clone()
			{
				object result;
				lock (this._table.SyncRoot)
				{
					result = Hashtable.Synchronized((Hashtable)this._table.Clone());
				}
				return result;
			}

			// Token: 0x06001862 RID: 6242 RVA: 0x0003ED24 File Offset: 0x0003DD24
			public override IDictionaryEnumerator GetEnumerator()
			{
				return this._table.GetEnumerator();
			}

			// Token: 0x1700037C RID: 892
			// (get) Token: 0x06001863 RID: 6243 RVA: 0x0003ED34 File Offset: 0x0003DD34
			public override ICollection Keys
			{
				get
				{
					ICollection keys;
					lock (this._table.SyncRoot)
					{
						keys = this._table.Keys;
					}
					return keys;
				}
			}

			// Token: 0x1700037D RID: 893
			// (get) Token: 0x06001864 RID: 6244 RVA: 0x0003ED7C File Offset: 0x0003DD7C
			public override ICollection Values
			{
				get
				{
					ICollection values;
					lock (this._table.SyncRoot)
					{
						values = this._table.Values;
					}
					return values;
				}
			}

			// Token: 0x06001865 RID: 6245 RVA: 0x0003EDC4 File Offset: 0x0003DDC4
			public override void Remove(object key)
			{
				lock (this._table.SyncRoot)
				{
					this._table.Remove(key);
				}
			}

			// Token: 0x06001866 RID: 6246 RVA: 0x0003EE08 File Offset: 0x0003DE08
			public override void OnDeserialization(object sender)
			{
			}

			// Token: 0x06001867 RID: 6247 RVA: 0x0003EE0A File Offset: 0x0003DE0A
			internal override KeyValuePairs[] ToKeyValuePairsArray()
			{
				return this._table.ToKeyValuePairsArray();
			}

			// Token: 0x040009AA RID: 2474
			protected Hashtable _table;
		}

		// Token: 0x0200026B RID: 619
		[Serializable]
		private class HashtableEnumerator : IDictionaryEnumerator, IEnumerator, ICloneable
		{
			// Token: 0x06001868 RID: 6248 RVA: 0x0003EE17 File Offset: 0x0003DE17
			internal HashtableEnumerator(Hashtable hashtable, int getObjRetType)
			{
				this.hashtable = hashtable;
				this.bucket = hashtable.buckets.Length;
				this.version = hashtable.version;
				this.current = false;
				this.getObjectRetType = getObjRetType;
			}

			// Token: 0x06001869 RID: 6249 RVA: 0x0003EE50 File Offset: 0x0003DE50
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x1700037E RID: 894
			// (get) Token: 0x0600186A RID: 6250 RVA: 0x0003EE58 File Offset: 0x0003DE58
			public virtual object Key
			{
				get
				{
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					return this.currentKey;
				}
			}

			// Token: 0x0600186B RID: 6251 RVA: 0x0003EE78 File Offset: 0x0003DE78
			public virtual bool MoveNext()
			{
				if (this.version != this.hashtable.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				while (this.bucket > 0)
				{
					this.bucket--;
					object key = this.hashtable.buckets[this.bucket].key;
					if (key != null && key != this.hashtable.buckets)
					{
						this.currentKey = key;
						this.currentValue = this.hashtable.buckets[this.bucket].val;
						this.current = true;
						return true;
					}
				}
				this.current = false;
				return false;
			}

			// Token: 0x1700037F RID: 895
			// (get) Token: 0x0600186C RID: 6252 RVA: 0x0003EF27 File Offset: 0x0003DF27
			public virtual DictionaryEntry Entry
			{
				get
				{
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return new DictionaryEntry(this.currentKey, this.currentValue);
				}
			}

			// Token: 0x17000380 RID: 896
			// (get) Token: 0x0600186D RID: 6253 RVA: 0x0003EF54 File Offset: 0x0003DF54
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
						return this.currentKey;
					}
					if (this.getObjectRetType == 2)
					{
						return this.currentValue;
					}
					return new DictionaryEntry(this.currentKey, this.currentValue);
				}
			}

			// Token: 0x17000381 RID: 897
			// (get) Token: 0x0600186E RID: 6254 RVA: 0x0003EFAF File Offset: 0x0003DFAF
			public virtual object Value
			{
				get
				{
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.currentValue;
				}
			}

			// Token: 0x0600186F RID: 6255 RVA: 0x0003EFD0 File Offset: 0x0003DFD0
			public virtual void Reset()
			{
				if (this.version != this.hashtable.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.current = false;
				this.bucket = this.hashtable.buckets.Length;
				this.currentKey = null;
				this.currentValue = null;
			}

			// Token: 0x040009AB RID: 2475
			internal const int Keys = 1;

			// Token: 0x040009AC RID: 2476
			internal const int Values = 2;

			// Token: 0x040009AD RID: 2477
			internal const int DictEntry = 3;

			// Token: 0x040009AE RID: 2478
			private Hashtable hashtable;

			// Token: 0x040009AF RID: 2479
			private int bucket;

			// Token: 0x040009B0 RID: 2480
			private int version;

			// Token: 0x040009B1 RID: 2481
			private bool current;

			// Token: 0x040009B2 RID: 2482
			private int getObjectRetType;

			// Token: 0x040009B3 RID: 2483
			private object currentKey;

			// Token: 0x040009B4 RID: 2484
			private object currentValue;
		}

		// Token: 0x0200026C RID: 620
		internal class HashtableDebugView
		{
			// Token: 0x06001870 RID: 6256 RVA: 0x0003F02A File Offset: 0x0003E02A
			public HashtableDebugView(Hashtable hashtable)
			{
				if (hashtable == null)
				{
					throw new ArgumentNullException("hashtable");
				}
				this.hashtable = hashtable;
			}

			// Token: 0x17000382 RID: 898
			// (get) Token: 0x06001871 RID: 6257 RVA: 0x0003F047 File Offset: 0x0003E047
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePairs[] Items
			{
				get
				{
					return this.hashtable.ToKeyValuePairsArray();
				}
			}

			// Token: 0x040009B5 RID: 2485
			private Hashtable hashtable;
		}
	}
}
