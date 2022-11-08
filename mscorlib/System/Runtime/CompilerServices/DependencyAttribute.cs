using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F3 RID: 1523
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[Serializable]
	public sealed class DependencyAttribute : Attribute
	{
		// Token: 0x060037F7 RID: 14327 RVA: 0x000BBCCA File Offset: 0x000BACCA
		public DependencyAttribute(string dependentAssemblyArgument, LoadHint loadHintArgument)
		{
			this.dependentAssembly = dependentAssemblyArgument;
			this.loadHint = loadHintArgument;
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060037F8 RID: 14328 RVA: 0x000BBCE0 File Offset: 0x000BACE0
		public string DependentAssembly
		{
			get
			{
				return this.dependentAssembly;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060037F9 RID: 14329 RVA: 0x000BBCE8 File Offset: 0x000BACE8
		public LoadHint LoadHint
		{
			get
			{
				return this.loadHint;
			}
		}

		// Token: 0x04001D04 RID: 7428
		private string dependentAssembly;

		// Token: 0x04001D05 RID: 7429
		private LoadHint loadHint;
	}
}
