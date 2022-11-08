using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000310 RID: 784
	[ComVisible(true)]
	public struct InterfaceMapping
	{
		// Token: 0x04000B6B RID: 2923
		[ComVisible(true)]
		public Type TargetType;

		// Token: 0x04000B6C RID: 2924
		[ComVisible(true)]
		public Type InterfaceType;

		// Token: 0x04000B6D RID: 2925
		[ComVisible(true)]
		public MethodInfo[] TargetMethods;

		// Token: 0x04000B6E RID: 2926
		[ComVisible(true)]
		public MethodInfo[] InterfaceMethods;
	}
}
