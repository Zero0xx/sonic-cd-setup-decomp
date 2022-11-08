using System;
using System.Collections;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007FA RID: 2042
	internal sealed class SerObjectInfoInit
	{
		// Token: 0x0400253B RID: 9531
		internal Hashtable seenBeforeTable = new Hashtable();

		// Token: 0x0400253C RID: 9532
		internal int objectInfoIdCount = 1;

		// Token: 0x0400253D RID: 9533
		internal SerStack oiPool = new SerStack("SerObjectInfo Pool");
	}
}
