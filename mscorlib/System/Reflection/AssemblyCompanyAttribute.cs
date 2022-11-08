using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E2 RID: 738
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCompanyAttribute : Attribute
	{
		// Token: 0x06001D01 RID: 7425 RVA: 0x00049D29 File Offset: 0x00048D29
		public AssemblyCompanyAttribute(string company)
		{
			this.m_company = company;
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x00049D38 File Offset: 0x00048D38
		public string Company
		{
			get
			{
				return this.m_company;
			}
		}

		// Token: 0x04000ACE RID: 2766
		private string m_company;
	}
}
