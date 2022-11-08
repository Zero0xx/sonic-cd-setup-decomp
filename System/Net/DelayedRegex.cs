using System;
using System.Text.RegularExpressions;

namespace System.Net
{
	// Token: 0x020004A7 RID: 1191
	[Serializable]
	internal class DelayedRegex
	{
		// Token: 0x06002477 RID: 9335 RVA: 0x0008F6D8 File Offset: 0x0008E6D8
		internal DelayedRegex(string regexString)
		{
			if (regexString == null)
			{
				throw new ArgumentNullException("regexString");
			}
			this._AsString = regexString;
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x0008F6F5 File Offset: 0x0008E6F5
		internal DelayedRegex(Regex regex)
		{
			if (regex == null)
			{
				throw new ArgumentNullException("regex");
			}
			this._AsRegex = regex;
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x0008F712 File Offset: 0x0008E712
		internal Regex AsRegex
		{
			get
			{
				if (this._AsRegex == null)
				{
					this._AsRegex = new Regex(this._AsString + "[/]?", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
				}
				return this._AsRegex;
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x0008F744 File Offset: 0x0008E744
		public override string ToString()
		{
			if (this._AsString == null)
			{
				return this._AsString = this._AsRegex.ToString();
			}
			return this._AsString;
		}

		// Token: 0x040024B9 RID: 9401
		private Regex _AsRegex;

		// Token: 0x040024BA RID: 9402
		private string _AsString;
	}
}
