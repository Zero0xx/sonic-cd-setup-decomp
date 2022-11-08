using System;
using System.Collections;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000719 RID: 1817
	internal class MessageDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x060040DA RID: 16602 RVA: 0x000DCCCF File Offset: 0x000DBCCF
		public MessageDictionaryEnumerator(MessageDictionary md, IDictionary hashtable)
		{
			this._md = md;
			if (hashtable != null)
			{
				this._enumHash = hashtable.GetEnumerator();
				return;
			}
			this._enumHash = null;
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x060040DB RID: 16603 RVA: 0x000DCCFC File Offset: 0x000DBCFC
		public object Key
		{
			get
			{
				if (this.i < 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
				}
				if (this.i < this._md._keys.Length)
				{
					return this._md._keys[this.i];
				}
				return this._enumHash.Key;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x060040DC RID: 16604 RVA: 0x000DCD58 File Offset: 0x000DBD58
		public object Value
		{
			get
			{
				if (this.i < 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
				}
				if (this.i < this._md._keys.Length)
				{
					return this._md.GetMessageValue(this.i);
				}
				return this._enumHash.Value;
			}
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x000DCDB0 File Offset: 0x000DBDB0
		public bool MoveNext()
		{
			if (this.i == -2)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
			this.i++;
			if (this.i < this._md._keys.Length)
			{
				return true;
			}
			if (this._enumHash != null && this._enumHash.MoveNext())
			{
				return true;
			}
			this.i = -2;
			return false;
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060040DE RID: 16606 RVA: 0x000DCE1C File Offset: 0x000DBE1C
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x060040DF RID: 16607 RVA: 0x000DCE29 File Offset: 0x000DBE29
		public DictionaryEntry Entry
		{
			get
			{
				return new DictionaryEntry(this.Key, this.Value);
			}
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x000DCE3C File Offset: 0x000DBE3C
		public void Reset()
		{
			this.i = -1;
			if (this._enumHash != null)
			{
				this._enumHash.Reset();
			}
		}

		// Token: 0x040020B5 RID: 8373
		private int i = -1;

		// Token: 0x040020B6 RID: 8374
		private IDictionaryEnumerator _enumHash;

		// Token: 0x040020B7 RID: 8375
		private MessageDictionary _md;
	}
}
