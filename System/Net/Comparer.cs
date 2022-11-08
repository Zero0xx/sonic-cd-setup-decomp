using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x0200038E RID: 910
	internal class Comparer : IComparer
	{
		// Token: 0x06001C70 RID: 7280 RVA: 0x0006BD98 File Offset: 0x0006AD98
		int IComparer.Compare(object ol, object or)
		{
			Cookie cookie = (Cookie)ol;
			Cookie cookie2 = (Cookie)or;
			int result;
			if ((result = string.Compare(cookie.Name, cookie2.Name, StringComparison.OrdinalIgnoreCase)) != 0)
			{
				return result;
			}
			if ((result = string.Compare(cookie.Domain, cookie2.Domain, StringComparison.OrdinalIgnoreCase)) != 0)
			{
				return result;
			}
			if ((result = string.Compare(cookie.Path, cookie2.Path, StringComparison.Ordinal)) != 0)
			{
				return result;
			}
			return 0;
		}
	}
}
