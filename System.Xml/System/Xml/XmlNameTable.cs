using System;

namespace System.Xml
{
	// Token: 0x02000024 RID: 36
	public abstract class XmlNameTable
	{
		// Token: 0x060000B5 RID: 181
		public abstract string Get(char[] array, int offset, int length);

		// Token: 0x060000B6 RID: 182
		public abstract string Get(string array);

		// Token: 0x060000B7 RID: 183
		public abstract string Add(char[] array, int offset, int length);

		// Token: 0x060000B8 RID: 184
		public abstract string Add(string array);
	}
}
