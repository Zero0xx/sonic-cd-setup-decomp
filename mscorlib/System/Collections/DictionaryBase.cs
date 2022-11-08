using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000261 RID: 609
	[ComVisible(true)]
	[Serializable]
	public abstract class DictionaryBase : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x0003CFED File Offset: 0x0003BFED
		protected Hashtable InnerHashtable
		{
			get
			{
				if (this.hashtable == null)
				{
					this.hashtable = new Hashtable();
				}
				return this.hashtable;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x0003D008 File Offset: 0x0003C008
		protected IDictionary Dictionary
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0003D00B File Offset: 0x0003C00B
		public int Count
		{
			get
			{
				if (this.hashtable != null)
				{
					return this.hashtable.Count;
				}
				return 0;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x0003D022 File Offset: 0x0003C022
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.InnerHashtable.IsReadOnly;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x0003D02F File Offset: 0x0003C02F
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.InnerHashtable.IsFixedSize;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060017D4 RID: 6100 RVA: 0x0003D03C File Offset: 0x0003C03C
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerHashtable.IsSynchronized;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060017D5 RID: 6101 RVA: 0x0003D049 File Offset: 0x0003C049
		ICollection IDictionary.Keys
		{
			get
			{
				return this.InnerHashtable.Keys;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x0003D056 File Offset: 0x0003C056
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerHashtable.SyncRoot;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x0003D063 File Offset: 0x0003C063
		ICollection IDictionary.Values
		{
			get
			{
				return this.InnerHashtable.Values;
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x0003D070 File Offset: 0x0003C070
		public void CopyTo(Array array, int index)
		{
			this.InnerHashtable.CopyTo(array, index);
		}

		// Token: 0x17000353 RID: 851
		object IDictionary.this[object key]
		{
			get
			{
				object obj = this.InnerHashtable[key];
				this.OnGet(key, obj);
				return obj;
			}
			set
			{
				this.OnValidate(key, value);
				bool flag = true;
				object obj = this.InnerHashtable[key];
				if (obj == null)
				{
					flag = this.InnerHashtable.Contains(key);
				}
				this.OnSet(key, obj, value);
				this.InnerHashtable[key] = value;
				try
				{
					this.OnSetComplete(key, obj, value);
				}
				catch
				{
					if (flag)
					{
						this.InnerHashtable[key] = obj;
					}
					else
					{
						this.InnerHashtable.Remove(key);
					}
					throw;
				}
			}
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0003D12C File Offset: 0x0003C12C
		bool IDictionary.Contains(object key)
		{
			return this.InnerHashtable.Contains(key);
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0003D13C File Offset: 0x0003C13C
		void IDictionary.Add(object key, object value)
		{
			this.OnValidate(key, value);
			this.OnInsert(key, value);
			this.InnerHashtable.Add(key, value);
			try
			{
				this.OnInsertComplete(key, value);
			}
			catch
			{
				this.InnerHashtable.Remove(key);
				throw;
			}
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0003D190 File Offset: 0x0003C190
		public void Clear()
		{
			this.OnClear();
			this.InnerHashtable.Clear();
			this.OnClearComplete();
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0003D1AC File Offset: 0x0003C1AC
		void IDictionary.Remove(object key)
		{
			if (this.InnerHashtable.Contains(key))
			{
				object value = this.InnerHashtable[key];
				this.OnValidate(key, value);
				this.OnRemove(key, value);
				this.InnerHashtable.Remove(key);
				try
				{
					this.OnRemoveComplete(key, value);
				}
				catch
				{
					this.InnerHashtable.Add(key, value);
					throw;
				}
			}
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0003D21C File Offset: 0x0003C21C
		public IDictionaryEnumerator GetEnumerator()
		{
			return this.InnerHashtable.GetEnumerator();
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0003D229 File Offset: 0x0003C229
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.InnerHashtable.GetEnumerator();
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0003D236 File Offset: 0x0003C236
		protected virtual object OnGet(object key, object currentValue)
		{
			return currentValue;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0003D239 File Offset: 0x0003C239
		protected virtual void OnSet(object key, object oldValue, object newValue)
		{
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0003D23B File Offset: 0x0003C23B
		protected virtual void OnInsert(object key, object value)
		{
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0003D23D File Offset: 0x0003C23D
		protected virtual void OnClear()
		{
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0003D23F File Offset: 0x0003C23F
		protected virtual void OnRemove(object key, object value)
		{
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0003D241 File Offset: 0x0003C241
		protected virtual void OnValidate(object key, object value)
		{
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0003D243 File Offset: 0x0003C243
		protected virtual void OnSetComplete(object key, object oldValue, object newValue)
		{
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0003D245 File Offset: 0x0003C245
		protected virtual void OnInsertComplete(object key, object value)
		{
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0003D247 File Offset: 0x0003C247
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0003D249 File Offset: 0x0003C249
		protected virtual void OnRemoveComplete(object key, object value)
		{
		}

		// Token: 0x0400098E RID: 2446
		private Hashtable hashtable;
	}
}
