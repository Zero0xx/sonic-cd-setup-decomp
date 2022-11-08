using System;

namespace System.ComponentModel
{
	// Token: 0x020000F4 RID: 244
	public interface INestedSite : ISite, IServiceProvider
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007FC RID: 2044
		string FullName { get; }
	}
}
