using System;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007FB RID: 2043
	internal sealed class SerObjectInfoCache
	{
		// Token: 0x0400253E RID: 9534
		internal string fullTypeName;

		// Token: 0x0400253F RID: 9535
		internal string assemblyString;

		// Token: 0x04002540 RID: 9536
		internal MemberInfo[] memberInfos;

		// Token: 0x04002541 RID: 9537
		internal string[] memberNames;

		// Token: 0x04002542 RID: 9538
		internal Type[] memberTypes;
	}
}
