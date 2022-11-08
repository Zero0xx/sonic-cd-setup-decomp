using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x0200025C RID: 604
	[Obsolete("Please use IEqualityComparer instead.")]
	[ComVisible(true)]
	public interface IHashCodeProvider
	{
		// Token: 0x06001799 RID: 6041
		int GetHashCode(object obj);
	}
}
