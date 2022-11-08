using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x020000BB RID: 187
	[ComVisible(true)]
	public interface IContainer : IDisposable
	{
		// Token: 0x0600067E RID: 1662
		void Add(IComponent component);

		// Token: 0x0600067F RID: 1663
		void Add(IComponent component, string name);

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000680 RID: 1664
		ComponentCollection Components { get; }

		// Token: 0x06000681 RID: 1665
		void Remove(IComponent component);
	}
}
