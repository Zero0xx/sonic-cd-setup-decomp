using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x020007A0 RID: 1952
	[Serializable]
	internal class InvariantComparer : IComparer
	{
		// Token: 0x06003C0D RID: 15373 RVA: 0x00100D7D File Offset: 0x000FFD7D
		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00100D98 File Offset: 0x000FFD98
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this.m_compareInfo.Compare(text, text2);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x04003501 RID: 13569
		private CompareInfo m_compareInfo;

		// Token: 0x04003502 RID: 13570
		internal static readonly InvariantComparer Default = new InvariantComparer();
	}
}
