using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003C5 RID: 965
	[ComVisible(true)]
	[Serializable]
	public class SortKey
	{
		// Token: 0x06002725 RID: 10021 RVA: 0x00075100 File Offset: 0x00074100
		internal unsafe SortKey(void* pSortingFile, int win32LCID, string str, CompareOptions options)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			this.win32LCID = win32LCID;
			this.options = options;
			this.m_String = str;
			this.m_KeyData = CompareInfo.nativeCreateSortKey(pSortingFile, str, (int)options, win32LCID);
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x00075168 File Offset: 0x00074168
		internal SortKey(int win32LCID, string str, CompareOptions options)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (CultureInfo.GetNativeSortKey(win32LCID, CompareInfo.GetNativeCompareFlags(options), str, str.Length, out this.m_KeyData) < 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "str");
			}
			this.win32LCID = win32LCID;
			this.options = options;
			this.m_String = str;
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002727 RID: 10023 RVA: 0x000751ED File Offset: 0x000741ED
		public virtual string OriginalString
		{
			get
			{
				return this.m_String;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x000751F5 File Offset: 0x000741F5
		public virtual byte[] KeyData
		{
			get
			{
				return (byte[])this.m_KeyData.Clone();
			}
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x00075208 File Offset: 0x00074208
		public static int Compare(SortKey sortkey1, SortKey sortkey2)
		{
			if (sortkey1 == null || sortkey2 == null)
			{
				throw new ArgumentNullException((sortkey1 == null) ? "sortkey1" : "sortkey2");
			}
			byte[] keyData = sortkey1.m_KeyData;
			byte[] keyData2 = sortkey2.m_KeyData;
			if (keyData.Length == 0)
			{
				if (keyData2.Length == 0)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (keyData2.Length == 0)
				{
					return 1;
				}
				int num = (keyData.Length < keyData2.Length) ? keyData.Length : keyData2.Length;
				for (int i = 0; i < num; i++)
				{
					if (keyData[i] > keyData2[i])
					{
						return 1;
					}
					if (keyData[i] < keyData2[i])
					{
						return -1;
					}
				}
				return 0;
			}
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x00075288 File Offset: 0x00074288
		public override bool Equals(object value)
		{
			SortKey sortKey = value as SortKey;
			return sortKey != null && SortKey.Compare(this, sortKey) == 0;
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000752AB File Offset: 0x000742AB
		public override int GetHashCode()
		{
			return CompareInfo.GetCompareInfo(this.win32LCID).GetHashCodeOfString(this.m_String, this.options);
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000752CC File Offset: 0x000742CC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"SortKey - ",
				this.win32LCID,
				", ",
				this.options,
				", ",
				this.m_String
			});
		}

		// Token: 0x040011D8 RID: 4568
		private const CompareOptions ValidSortkeyCtorMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort);

		// Token: 0x040011D9 RID: 4569
		internal int win32LCID;

		// Token: 0x040011DA RID: 4570
		internal CompareOptions options;

		// Token: 0x040011DB RID: 4571
		internal string m_String;

		// Token: 0x040011DC RID: 4572
		internal byte[] m_KeyData;
	}
}
