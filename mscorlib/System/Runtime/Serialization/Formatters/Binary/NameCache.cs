using System;
using System.Collections;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007EF RID: 2031
	internal sealed class NameCache
	{
		// Token: 0x060047B9 RID: 18361 RVA: 0x000F5F18 File Offset: 0x000F4F18
		internal object GetCachedValue(string name)
		{
			this.name = name;
			return NameCache.ht[name];
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x000F5F2C File Offset: 0x000F4F2C
		internal void SetCachedValue(object value)
		{
			NameCache.ht[this.name] = value;
		}

		// Token: 0x0400248C RID: 9356
		private static Hashtable ht = new Hashtable();

		// Token: 0x0400248D RID: 9357
		private string name;
	}
}
