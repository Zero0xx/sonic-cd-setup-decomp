using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x0200025B RID: 603
	[ComVisible(true)]
	[Serializable]
	public class CaseInsensitiveComparer : IComparer
	{
		// Token: 0x06001794 RID: 6036 RVA: 0x0003C9B0 File Offset: 0x0003B9B0
		public CaseInsensitiveComparer()
		{
			this.m_compareInfo = CultureInfo.CurrentCulture.CompareInfo;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0003C9C8 File Offset: 0x0003B9C8
		public CaseInsensitiveComparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this.m_compareInfo = culture.CompareInfo;
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x0003C9EA File Offset: 0x0003B9EA
		public static CaseInsensitiveComparer Default
		{
			get
			{
				return new CaseInsensitiveComparer(CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x0003C9F6 File Offset: 0x0003B9F6
		public static CaseInsensitiveComparer DefaultInvariant
		{
			get
			{
				if (CaseInsensitiveComparer.m_InvariantCaseInsensitiveComparer == null)
				{
					CaseInsensitiveComparer.m_InvariantCaseInsensitiveComparer = new CaseInsensitiveComparer(CultureInfo.InvariantCulture);
				}
				return CaseInsensitiveComparer.m_InvariantCaseInsensitiveComparer;
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0003CA14 File Offset: 0x0003BA14
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this.m_compareInfo.Compare(text, text2, CompareOptions.IgnoreCase);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x04000985 RID: 2437
		private CompareInfo m_compareInfo;

		// Token: 0x04000986 RID: 2438
		private static CaseInsensitiveComparer m_InvariantCaseInsensitiveComparer;
	}
}
