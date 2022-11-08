using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EA RID: 746
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyVersionAttribute : Attribute
	{
		// Token: 0x06001D11 RID: 7441 RVA: 0x00049DEF File Offset: 0x00048DEF
		public AssemblyVersionAttribute(string version)
		{
			this.m_version = version;
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001D12 RID: 7442 RVA: 0x00049DFE File Offset: 0x00048DFE
		public string Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x04000AD6 RID: 2774
		private string m_version;
	}
}
