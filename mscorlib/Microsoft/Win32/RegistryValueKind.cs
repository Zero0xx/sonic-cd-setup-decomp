using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x02000477 RID: 1143
	[ComVisible(true)]
	public enum RegistryValueKind
	{
		// Token: 0x04001792 RID: 6034
		String = 1,
		// Token: 0x04001793 RID: 6035
		ExpandString,
		// Token: 0x04001794 RID: 6036
		Binary,
		// Token: 0x04001795 RID: 6037
		DWord,
		// Token: 0x04001796 RID: 6038
		MultiString = 7,
		// Token: 0x04001797 RID: 6039
		QWord = 11,
		// Token: 0x04001798 RID: 6040
		Unknown = 0
	}
}
