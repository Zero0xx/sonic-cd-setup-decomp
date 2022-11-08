using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Assemblies
{
	// Token: 0x0200085F RID: 2143
	[ComVisible(true)]
	[Serializable]
	public enum AssemblyVersionCompatibility
	{
		// Token: 0x04002880 RID: 10368
		SameMachine = 1,
		// Token: 0x04002881 RID: 10369
		SameProcess,
		// Token: 0x04002882 RID: 10370
		SameDomain
	}
}
