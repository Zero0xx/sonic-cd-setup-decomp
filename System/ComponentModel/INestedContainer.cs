using System;

namespace System.ComponentModel
{
	// Token: 0x020000F3 RID: 243
	public interface INestedContainer : IContainer, IDisposable
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007FB RID: 2043
		IComponent Owner { get; }
	}
}
