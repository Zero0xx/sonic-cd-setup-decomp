using System;
using System.Globalization;

namespace System
{
	// Token: 0x0200002B RID: 43
	[Serializable]
	internal sealed class OrdinalComparer : StringComparer
	{
		// Token: 0x06000216 RID: 534 RVA: 0x000096C5 File Offset: 0x000086C5
		internal OrdinalComparer(bool ignoreCase)
		{
			this._ignoreCase = ignoreCase;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000096D4 File Offset: 0x000086D4
		public override int Compare(string x, string y)
		{
			if (object.ReferenceEquals(x, y))
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			if (this._ignoreCase)
			{
				return TextInfo.CompareOrdinalIgnoreCase(x, y);
			}
			return string.CompareOrdinal(x, y);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00009702 File Offset: 0x00008702
		public override bool Equals(string x, string y)
		{
			if (object.ReferenceEquals(x, y))
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			if (this._ignoreCase)
			{
				return x.Length == y.Length && TextInfo.CompareOrdinalIgnoreCase(x, y) == 0;
			}
			return x.Equals(y);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00009741 File Offset: 0x00008741
		public override int GetHashCode(string obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (this._ignoreCase)
			{
				return TextInfo.GetHashCodeOrdinalIgnoreCase(obj);
			}
			return obj.GetHashCode();
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009768 File Offset: 0x00008768
		public override bool Equals(object obj)
		{
			OrdinalComparer ordinalComparer = obj as OrdinalComparer;
			return ordinalComparer != null && this._ignoreCase == ordinalComparer._ignoreCase;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009790 File Offset: 0x00008790
		public override int GetHashCode()
		{
			string text = "OrdinalComparer";
			int hashCode = text.GetHashCode();
			if (!this._ignoreCase)
			{
				return hashCode;
			}
			return ~hashCode;
		}

		// Token: 0x040000A4 RID: 164
		private bool _ignoreCase;
	}
}
