using System;

namespace System.Diagnostics
{
	// Token: 0x020002B0 RID: 688
	[Serializable]
	internal abstract class AssertFilter
	{
		// Token: 0x06001B0A RID: 6922
		public abstract AssertFilters AssertFailure(string condition, string message, StackTrace location);
	}
}
