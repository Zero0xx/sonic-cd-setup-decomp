using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Resources
{
	// Token: 0x02000430 RID: 1072
	[ComVisible(true)]
	public interface IResourceReader : IEnumerable, IDisposable
	{
		// Token: 0x06002BC9 RID: 11209
		void Close();

		// Token: 0x06002BCA RID: 11210
		IDictionaryEnumerator GetEnumerator();
	}
}
