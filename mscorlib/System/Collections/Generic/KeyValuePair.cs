using System;
using System.Text;

namespace System.Collections.Generic
{
	// Token: 0x020002A3 RID: 675
	[Serializable]
	public struct KeyValuePair<TKey, TValue>
	{
		// Token: 0x06001A40 RID: 6720 RVA: 0x00044534 File Offset: 0x00043534
		public KeyValuePair(TKey key, TValue value)
		{
			this.key = key;
			this.value = value;
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x00044544 File Offset: 0x00043544
		public TKey Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x0004454C File Offset: 0x0004354C
		public TValue Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00044554 File Offset: 0x00043554
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			if (this.Key != null)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				TKey tkey = this.Key;
				stringBuilder2.Append(tkey.ToString());
			}
			stringBuilder.Append(", ");
			if (this.Value != null)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				TValue tvalue = this.Value;
				stringBuilder3.Append(tvalue.ToString());
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x04000A32 RID: 2610
		private TKey key;

		// Token: 0x04000A33 RID: 2611
		private TValue value;
	}
}
