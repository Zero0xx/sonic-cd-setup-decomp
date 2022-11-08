using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005E9 RID: 1513
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public sealed class InternalsVisibleToAttribute : Attribute
	{
		// Token: 0x060037E8 RID: 14312 RVA: 0x000BBC0D File Offset: 0x000BAC0D
		public InternalsVisibleToAttribute(string assemblyName)
		{
			this._assemblyName = assemblyName;
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060037E9 RID: 14313 RVA: 0x000BBC23 File Offset: 0x000BAC23
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060037EA RID: 14314 RVA: 0x000BBC2B File Offset: 0x000BAC2B
		// (set) Token: 0x060037EB RID: 14315 RVA: 0x000BBC33 File Offset: 0x000BAC33
		public bool AllInternalsVisible
		{
			get
			{
				return this._allInternalsVisible;
			}
			set
			{
				this._allInternalsVisible = value;
			}
		}

		// Token: 0x04001CED RID: 7405
		private string _assemblyName;

		// Token: 0x04001CEE RID: 7406
		private bool _allInternalsVisible = true;
	}
}
