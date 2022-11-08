using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x02000104 RID: 260
	[ComVisible(true)]
	public interface ITypeDescriptorContext : IServiceProvider
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000832 RID: 2098
		IContainer Container { get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000833 RID: 2099
		object Instance { get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000834 RID: 2100
		PropertyDescriptor PropertyDescriptor { get; }

		// Token: 0x06000835 RID: 2101
		bool OnComponentChanging();

		// Token: 0x06000836 RID: 2102
		void OnComponentChanged();
	}
}
