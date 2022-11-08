using System;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000768 RID: 1896
	internal class CategoryEntry
	{
		// Token: 0x06003A6B RID: 14955 RVA: 0x000F84BC File Offset: 0x000F74BC
		internal CategoryEntry(NativeMethods.PERF_OBJECT_TYPE perfObject)
		{
			this.NameIndex = perfObject.ObjectNameTitleIndex;
			this.HelpIndex = perfObject.ObjectHelpTitleIndex;
			this.CounterIndexes = new int[perfObject.NumCounters];
			this.HelpIndexes = new int[perfObject.NumCounters];
		}

		// Token: 0x04003328 RID: 13096
		internal int NameIndex;

		// Token: 0x04003329 RID: 13097
		internal int HelpIndex;

		// Token: 0x0400332A RID: 13098
		internal int[] CounterIndexes;

		// Token: 0x0400332B RID: 13099
		internal int[] HelpIndexes;
	}
}
