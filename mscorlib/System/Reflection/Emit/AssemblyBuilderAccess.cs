using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000804 RID: 2052
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum AssemblyBuilderAccess
	{
		// Token: 0x04002586 RID: 9606
		Run = 1,
		// Token: 0x04002587 RID: 9607
		Save = 2,
		// Token: 0x04002588 RID: 9608
		RunAndSave = 3,
		// Token: 0x04002589 RID: 9609
		ReflectionOnly = 6
	}
}
