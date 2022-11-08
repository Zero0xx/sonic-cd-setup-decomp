using System;

namespace System.Net
{
	// Token: 0x020004DE RID: 1246
	internal class HeaderInfo
	{
		// Token: 0x060026D0 RID: 9936 RVA: 0x0009FB36 File Offset: 0x0009EB36
		internal HeaderInfo(string name, bool requestRestricted, bool responseRestricted, bool multi, HeaderParser p)
		{
			this.HeaderName = name;
			this.IsRequestRestricted = requestRestricted;
			this.IsResponseRestricted = responseRestricted;
			this.Parser = p;
			this.AllowMultiValues = multi;
		}

		// Token: 0x04002653 RID: 9811
		internal readonly bool IsRequestRestricted;

		// Token: 0x04002654 RID: 9812
		internal readonly bool IsResponseRestricted;

		// Token: 0x04002655 RID: 9813
		internal readonly HeaderParser Parser;

		// Token: 0x04002656 RID: 9814
		internal readonly string HeaderName;

		// Token: 0x04002657 RID: 9815
		internal readonly bool AllowMultiValues;
	}
}
