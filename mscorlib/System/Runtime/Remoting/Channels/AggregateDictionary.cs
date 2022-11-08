using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006F5 RID: 1781
	internal class AggregateDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06003F80 RID: 16256 RVA: 0x000D8917 File Offset: 0x000D7917
		public AggregateDictionary(ICollection dictionaries)
		{
			this._dictionaries = dictionaries;
		}

		// Token: 0x17000ABC RID: 2748
		public virtual object this[object key]
		{
			get
			{
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					if (dictionary.Contains(key))
					{
						return dictionary[key];
					}
				}
				return null;
			}
			set
			{
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					if (dictionary.Contains(key))
					{
						dictionary[key] = value;
					}
				}
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003F83 RID: 16259 RVA: 0x000D89F4 File Offset: 0x000D79F4
		public virtual ICollection Keys
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					ICollection keys = dictionary.Keys;
					if (keys != null)
					{
						foreach (object value in keys)
						{
							arrayList.Add(value);
						}
					}
				}
				return arrayList;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003F84 RID: 16260 RVA: 0x000D8AA4 File Offset: 0x000D7AA4
		public virtual ICollection Values
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					ICollection values = dictionary.Values;
					if (values != null)
					{
						foreach (object value in values)
						{
							arrayList.Add(value);
						}
					}
				}
				return arrayList;
			}
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x000D8B54 File Offset: 0x000D7B54
		public virtual bool Contains(object key)
		{
			foreach (object obj in this._dictionaries)
			{
				IDictionary dictionary = (IDictionary)obj;
				if (dictionary.Contains(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003F86 RID: 16262 RVA: 0x000D8BB8 File Offset: 0x000D7BB8
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003F87 RID: 16263 RVA: 0x000D8BBB File Offset: 0x000D7BBB
		public virtual bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x000D8BBE File Offset: 0x000D7BBE
		public virtual void Add(object key, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x000D8BC5 File Offset: 0x000D7BC5
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x000D8BCC File Offset: 0x000D7BCC
		public virtual void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x000D8BD3 File Offset: 0x000D7BD3
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x000D8BDB File Offset: 0x000D7BDB
		public virtual void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003F8D RID: 16269 RVA: 0x000D8BE4 File Offset: 0x000D7BE4
		public virtual int Count
		{
			get
			{
				int num = 0;
				foreach (object obj in this._dictionaries)
				{
					IDictionary dictionary = (IDictionary)obj;
					num += dictionary.Count;
				}
				return num;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003F8E RID: 16270 RVA: 0x000D8C44 File Offset: 0x000D7C44
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003F8F RID: 16271 RVA: 0x000D8C47 File Offset: 0x000D7C47
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x000D8C4A File Offset: 0x000D7C4A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}

		// Token: 0x04002021 RID: 8225
		private ICollection _dictionaries;
	}
}
