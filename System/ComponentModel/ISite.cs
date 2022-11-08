using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x020000BD RID: 189
	[ComVisible(true)]
	public interface ISite : IServiceProvider
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600068F RID: 1679
		IComponent Component { get; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000690 RID: 1680
		IContainer Container { get; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000691 RID: 1681
		bool DesignMode { get; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000692 RID: 1682
		// (set) Token: 0x06000693 RID: 1683
		string Name { get; set; }
	}
}
