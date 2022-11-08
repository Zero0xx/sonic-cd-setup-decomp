using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200060A RID: 1546
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	[Serializable]
	public sealed class RuntimeCompatibilityAttribute : Attribute
	{
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06003806 RID: 14342 RVA: 0x000BBD64 File Offset: 0x000BAD64
		// (set) Token: 0x06003807 RID: 14343 RVA: 0x000BBD6C File Offset: 0x000BAD6C
		public bool WrapNonExceptionThrows
		{
			get
			{
				return this.m_wrapNonExceptionThrows;
			}
			set
			{
				this.m_wrapNonExceptionThrows = value;
			}
		}

		// Token: 0x04001D08 RID: 7432
		private bool m_wrapNonExceptionThrows;
	}
}
