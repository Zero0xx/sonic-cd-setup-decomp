using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000313 RID: 787
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum ResourceLocation
	{
		// Token: 0x04000B73 RID: 2931
		Embedded = 1,
		// Token: 0x04000B74 RID: 2932
		ContainedInAnotherAssembly = 2,
		// Token: 0x04000B75 RID: 2933
		ContainedInManifestFile = 4
	}
}
