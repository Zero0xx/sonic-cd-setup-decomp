using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000264 RID: 612
	[ComVisible(true)]
	public interface IDictionaryEnumerator : IEnumerator
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001802 RID: 6146
		object Key { get; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001803 RID: 6147
		object Value { get; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001804 RID: 6148
		DictionaryEntry Entry { get; }
	}
}
