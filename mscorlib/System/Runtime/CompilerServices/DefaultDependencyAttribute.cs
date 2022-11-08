using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F2 RID: 1522
	[AttributeUsage(AttributeTargets.Assembly)]
	[Serializable]
	public sealed class DefaultDependencyAttribute : Attribute
	{
		// Token: 0x060037F5 RID: 14325 RVA: 0x000BBCB3 File Offset: 0x000BACB3
		public DefaultDependencyAttribute(LoadHint loadHintArgument)
		{
			this.loadHint = loadHintArgument;
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x000BBCC2 File Offset: 0x000BACC2
		public LoadHint LoadHint
		{
			get
			{
				return this.loadHint;
			}
		}

		// Token: 0x04001D03 RID: 7427
		private LoadHint loadHint;
	}
}
