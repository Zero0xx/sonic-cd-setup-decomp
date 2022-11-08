using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000262 RID: 610
	[ComVisible(true)]
	[Serializable]
	public struct DictionaryEntry
	{
		// Token: 0x060017EC RID: 6124 RVA: 0x0003D253 File Offset: 0x0003C253
		public DictionaryEntry(object key, object value)
		{
			this._key = key;
			this._value = value;
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060017ED RID: 6125 RVA: 0x0003D263 File Offset: 0x0003C263
		// (set) Token: 0x060017EE RID: 6126 RVA: 0x0003D26B File Offset: 0x0003C26B
		public object Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060017EF RID: 6127 RVA: 0x0003D274 File Offset: 0x0003C274
		// (set) Token: 0x060017F0 RID: 6128 RVA: 0x0003D27C File Offset: 0x0003C27C
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x0400098F RID: 2447
		private object _key;

		// Token: 0x04000990 RID: 2448
		private object _value;
	}
}
