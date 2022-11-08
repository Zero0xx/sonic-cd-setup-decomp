using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x02000700 RID: 1792
	internal class ReadOnlyNameValueCollection : NameValueCollection
	{
		// Token: 0x0600372E RID: 14126 RVA: 0x000EACC6 File Offset: 0x000E9CC6
		internal ReadOnlyNameValueCollection(IEqualityComparer equalityComparer) : base(equalityComparer)
		{
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x000EACCF File Offset: 0x000E9CCF
		internal ReadOnlyNameValueCollection(ReadOnlyNameValueCollection value) : base(value)
		{
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x000EACD8 File Offset: 0x000E9CD8
		internal void SetReadOnly()
		{
			base.IsReadOnly = true;
		}
	}
}
