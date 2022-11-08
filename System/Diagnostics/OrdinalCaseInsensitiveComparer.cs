using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000779 RID: 1913
	internal class OrdinalCaseInsensitiveComparer : IComparer
	{
		// Token: 0x06003B1B RID: 15131 RVA: 0x000FBAE8 File Offset: 0x000FAAE8
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return string.CompareOrdinal(text.ToUpperInvariant(), text2.ToUpperInvariant());
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x040033C6 RID: 13254
		internal static readonly OrdinalCaseInsensitiveComparer Default = new OrdinalCaseInsensitiveComparer();
	}
}
