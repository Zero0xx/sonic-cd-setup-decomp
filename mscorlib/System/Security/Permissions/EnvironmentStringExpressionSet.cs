using System;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x02000623 RID: 1571
	[Serializable]
	internal class EnvironmentStringExpressionSet : StringExpressionSet
	{
		// Token: 0x06003899 RID: 14489 RVA: 0x000BEEDD File Offset: 0x000BDEDD
		public EnvironmentStringExpressionSet() : base(true, null, false)
		{
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x000BEEE8 File Offset: 0x000BDEE8
		public EnvironmentStringExpressionSet(string str) : base(true, str, false)
		{
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x000BEEF3 File Offset: 0x000BDEF3
		protected override StringExpressionSet CreateNewEmpty()
		{
			return new EnvironmentStringExpressionSet();
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x000BEEFA File Offset: 0x000BDEFA
		protected override bool StringSubsetString(string left, string right, bool ignoreCase)
		{
			if (!ignoreCase)
			{
				return string.Compare(left, right, StringComparison.Ordinal) == 0;
			}
			return string.Compare(left, right, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x000BEF16 File Offset: 0x000BDF16
		protected override string ProcessWholeString(string str)
		{
			return str;
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x000BEF19 File Offset: 0x000BDF19
		protected override string ProcessSingleString(string str)
		{
			return str;
		}
	}
}
