using System;

namespace System.Diagnostics
{
	// Token: 0x020002B1 RID: 689
	internal class DefaultFilter : AssertFilter
	{
		// Token: 0x06001B0C RID: 6924 RVA: 0x00046D9F File Offset: 0x00045D9F
		internal DefaultFilter()
		{
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x00046DA7 File Offset: 0x00045DA7
		public override AssertFilters AssertFailure(string condition, string message, StackTrace location)
		{
			return (AssertFilters)Assert.ShowDefaultAssertDialog(condition, message);
		}
	}
}
