using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000395 RID: 917
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum CultureTypes
	{
		// Token: 0x04000F98 RID: 3992
		NeutralCultures = 1,
		// Token: 0x04000F99 RID: 3993
		SpecificCultures = 2,
		// Token: 0x04000F9A RID: 3994
		InstalledWin32Cultures = 4,
		// Token: 0x04000F9B RID: 3995
		AllCultures = 7,
		// Token: 0x04000F9C RID: 3996
		UserCustomCulture = 8,
		// Token: 0x04000F9D RID: 3997
		ReplacementCultures = 16,
		// Token: 0x04000F9E RID: 3998
		WindowsOnlyCultures = 32,
		// Token: 0x04000F9F RID: 3999
		FrameworkCultures = 64
	}
}
