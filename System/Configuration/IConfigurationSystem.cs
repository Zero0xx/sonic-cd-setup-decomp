using System;
using System.Runtime.InteropServices;

namespace System.Configuration
{
	// Token: 0x020006F7 RID: 1783
	[ComVisible(false)]
	public interface IConfigurationSystem
	{
		// Token: 0x060036FF RID: 14079
		object GetConfig(string configKey);

		// Token: 0x06003700 RID: 14080
		void Init();
	}
}
