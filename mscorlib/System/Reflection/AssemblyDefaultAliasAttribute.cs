using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E6 RID: 742
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyDefaultAliasAttribute : Attribute
	{
		// Token: 0x06001D09 RID: 7433 RVA: 0x00049D85 File Offset: 0x00048D85
		public AssemblyDefaultAliasAttribute(string defaultAlias)
		{
			this.m_defaultAlias = defaultAlias;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x00049D94 File Offset: 0x00048D94
		public string DefaultAlias
		{
			get
			{
				return this.m_defaultAlias;
			}
		}

		// Token: 0x04000AD2 RID: 2770
		private string m_defaultAlias;
	}
}
