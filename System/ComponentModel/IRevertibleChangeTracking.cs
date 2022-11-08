using System;

namespace System.ComponentModel
{
	// Token: 0x02000100 RID: 256
	public interface IRevertibleChangeTracking : IChangeTracking
	{
		// Token: 0x06000828 RID: 2088
		void RejectChanges();
	}
}
