using System;
using System.Collections;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000714 RID: 1812
	internal abstract class MessageDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x060040B0 RID: 16560 RVA: 0x000DC101 File Offset: 0x000DB101
		internal MessageDictionary(string[] keys, IDictionary idict)
		{
			this._keys = keys;
			this._dict = idict;
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x000DC117 File Offset: 0x000DB117
		internal bool HasUserData()
		{
			return this._dict != null && this._dict.Count > 0;
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x000DC132 File Offset: 0x000DB132
		internal IDictionary InternalDictionary
		{
			get
			{
				return this._dict;
			}
		}

		// Token: 0x060040B3 RID: 16563
		internal abstract object GetMessageValue(int i);

		// Token: 0x060040B4 RID: 16564
		internal abstract void SetSpecialKey(int keyNum, object value);

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x060040B5 RID: 16565 RVA: 0x000DC13A File Offset: 0x000DB13A
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x060040B6 RID: 16566 RVA: 0x000DC13D File Offset: 0x000DB13D
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x000DC140 File Offset: 0x000DB140
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x000DC143 File Offset: 0x000DB143
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x000DC146 File Offset: 0x000DB146
		public virtual bool Contains(object key)
		{
			return this.ContainsSpecialKey(key) || (this._dict != null && this._dict.Contains(key));
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x000DC16C File Offset: 0x000DB16C
		protected virtual bool ContainsSpecialKey(object key)
		{
			if (!(key is string))
			{
				return false;
			}
			string text = (string)key;
			for (int i = 0; i < this._keys.Length; i++)
			{
				if (text.Equals(this._keys[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x000DC1B0 File Offset: 0x000DB1B0
		public virtual void CopyTo(Array array, int index)
		{
			for (int i = 0; i < this._keys.Length; i++)
			{
				array.SetValue(this.GetMessageValue(i), index + i);
			}
			if (this._dict != null)
			{
				this._dict.CopyTo(array, index + this._keys.Length);
			}
		}

		// Token: 0x17000B1E RID: 2846
		public virtual object this[object key]
		{
			get
			{
				string text = key as string;
				if (text != null)
				{
					for (int i = 0; i < this._keys.Length; i++)
					{
						if (text.Equals(this._keys[i]))
						{
							return this.GetMessageValue(i);
						}
					}
					if (this._dict != null)
					{
						return this._dict[key];
					}
				}
				return null;
			}
			set
			{
				if (!this.ContainsSpecialKey(key))
				{
					if (this._dict == null)
					{
						this._dict = new Hashtable();
					}
					this._dict[key] = value;
					return;
				}
				if (key.Equals(Message.UriKey))
				{
					this.SetSpecialKey(0, value);
					return;
				}
				if (key.Equals(Message.CallContextKey))
				{
					this.SetSpecialKey(1, value);
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKey"));
			}
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x000DC2CA File Offset: 0x000DB2CA
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new MessageDictionaryEnumerator(this, this._dict);
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x000DC2D8 File Offset: 0x000DB2D8
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x000DC2DF File Offset: 0x000DB2DF
		public virtual void Add(object key, object value)
		{
			if (this.ContainsSpecialKey(key))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKey"));
			}
			if (this._dict == null)
			{
				this._dict = new Hashtable();
			}
			this._dict.Add(key, value);
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x000DC31A File Offset: 0x000DB31A
		public virtual void Clear()
		{
			if (this._dict != null)
			{
				this._dict.Clear();
			}
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x000DC32F File Offset: 0x000DB32F
		public virtual void Remove(object key)
		{
			if (this.ContainsSpecialKey(key) || this._dict == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKey"));
			}
			this._dict.Remove(key);
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x060040C3 RID: 16579 RVA: 0x000DC360 File Offset: 0x000DB360
		public virtual ICollection Keys
		{
			get
			{
				int num = this._keys.Length;
				ICollection collection = (this._dict != null) ? this._dict.Keys : null;
				if (collection != null)
				{
					num += collection.Count;
				}
				ArrayList arrayList = new ArrayList(num);
				for (int i = 0; i < this._keys.Length; i++)
				{
					arrayList.Add(this._keys[i]);
				}
				if (collection != null)
				{
					arrayList.AddRange(collection);
				}
				return arrayList;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x000DC3D0 File Offset: 0x000DB3D0
		public virtual ICollection Values
		{
			get
			{
				int num = this._keys.Length;
				ICollection collection = (this._dict != null) ? this._dict.Keys : null;
				if (collection != null)
				{
					num += collection.Count;
				}
				ArrayList arrayList = new ArrayList(num);
				for (int i = 0; i < this._keys.Length; i++)
				{
					arrayList.Add(this.GetMessageValue(i));
				}
				if (collection != null)
				{
					arrayList.AddRange(collection);
				}
				return arrayList;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x060040C5 RID: 16581 RVA: 0x000DC43C File Offset: 0x000DB43C
		public virtual int Count
		{
			get
			{
				if (this._dict != null)
				{
					return this._dict.Count + this._keys.Length;
				}
				return this._keys.Length;
			}
		}

		// Token: 0x040020A7 RID: 8359
		internal string[] _keys;

		// Token: 0x040020A8 RID: 8360
		internal IDictionary _dict;
	}
}
