using System;

namespace System.ComponentModel
{
	// Token: 0x020000EB RID: 235
	public interface IChangeTracking
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060007DD RID: 2013
		bool IsChanged { get; }

		// Token: 0x060007DE RID: 2014
		void AcceptChanges();
	}
}
