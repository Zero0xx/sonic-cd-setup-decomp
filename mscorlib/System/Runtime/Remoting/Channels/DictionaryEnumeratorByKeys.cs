using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006F4 RID: 1780
	internal class DictionaryEnumeratorByKeys : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x06003F79 RID: 16249 RVA: 0x000D889D File Offset: 0x000D789D
		public DictionaryEnumeratorByKeys(IDictionary properties)
		{
			this._properties = properties;
			this._keyEnum = properties.Keys.GetEnumerator();
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x000D88BD File Offset: 0x000D78BD
		public bool MoveNext()
		{
			return this._keyEnum.MoveNext();
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x000D88CA File Offset: 0x000D78CA
		public void Reset()
		{
			this._keyEnum.Reset();
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06003F7C RID: 16252 RVA: 0x000D88D7 File Offset: 0x000D78D7
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06003F7D RID: 16253 RVA: 0x000D88E4 File Offset: 0x000D78E4
		public DictionaryEntry Entry
		{
			get
			{
				return new DictionaryEntry(this.Key, this.Value);
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003F7E RID: 16254 RVA: 0x000D88F7 File Offset: 0x000D78F7
		public object Key
		{
			get
			{
				return this._keyEnum.Current;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003F7F RID: 16255 RVA: 0x000D8904 File Offset: 0x000D7904
		public object Value
		{
			get
			{
				return this._properties[this.Key];
			}
		}

		// Token: 0x0400201F RID: 8223
		private IDictionary _properties;

		// Token: 0x04002020 RID: 8224
		private IEnumerator _keyEnum;
	}
}
