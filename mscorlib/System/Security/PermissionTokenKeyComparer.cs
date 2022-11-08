using System;
using System.Collections;
using System.Globalization;

namespace System.Security
{
	// Token: 0x0200067B RID: 1659
	[Serializable]
	internal sealed class PermissionTokenKeyComparer : IEqualityComparer
	{
		// Token: 0x06003BE6 RID: 15334 RVA: 0x000CC4D4 File Offset: 0x000CB4D4
		public PermissionTokenKeyComparer(CultureInfo culture)
		{
			this._caseSensitiveComparer = new Comparer(culture);
			this._info = culture.TextInfo;
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x000CC4F4 File Offset: 0x000CB4F4
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text == null || text2 == null)
			{
				return this._caseSensitiveComparer.Compare(a, b);
			}
			int num = this._caseSensitiveComparer.Compare(a, b);
			if (num == 0)
			{
				return 0;
			}
			if (SecurityManager._IsSameType(text, text2))
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x000CC542 File Offset: 0x000CB542
		public bool Equals(object a, object b)
		{
			return a == b || (a != null && b != null && this.Compare(a, b) == 0);
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x000CC560 File Offset: 0x000CB560
		public int GetHashCode(object obj)
		{
			string text = obj as string;
			if (text == null)
			{
				return obj.GetHashCode();
			}
			int num = text.IndexOf(',');
			if (num == -1)
			{
				num = text.Length;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				num2 = (num2 << 7 ^ (int)text[i] ^ num2 >> 25);
			}
			return num2;
		}

		// Token: 0x04001EEA RID: 7914
		private Comparer _caseSensitiveComparer;

		// Token: 0x04001EEB RID: 7915
		private TextInfo _info;
	}
}
