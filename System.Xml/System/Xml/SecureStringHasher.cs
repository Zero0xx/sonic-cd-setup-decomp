using System;
using System.Collections.Generic;

namespace System.Xml
{
	// Token: 0x02000066 RID: 102
	internal class SecureStringHasher : IEqualityComparer<string>
	{
		// Token: 0x0600037D RID: 893 RVA: 0x00011C23 File Offset: 0x00010C23
		public SecureStringHasher()
		{
			this.hashCodeRandomizer = Environment.TickCount;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00011C36 File Offset: 0x00010C36
		public SecureStringHasher(int hashCodeRandomizer)
		{
			this.hashCodeRandomizer = hashCodeRandomizer;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00011C45 File Offset: 0x00010C45
		public int Compare(string x, string y)
		{
			return string.Compare(x, y, StringComparison.Ordinal);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00011C4F File Offset: 0x00010C4F
		public bool Equals(string x, string y)
		{
			return string.Equals(x, y, StringComparison.Ordinal);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00011C5C File Offset: 0x00010C5C
		public int GetHashCode(string key)
		{
			int num = this.hashCodeRandomizer;
			for (int i = 0; i < key.Length; i++)
			{
				num += (num << 7 ^ (int)key[i]);
			}
			num -= num >> 17;
			num -= num >> 11;
			return num - (num >> 5);
		}

		// Token: 0x040005C3 RID: 1475
		private int hashCodeRandomizer;
	}
}
