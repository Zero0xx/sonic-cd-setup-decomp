using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E4 RID: 740
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyTitleAttribute : Attribute
	{
		// Token: 0x06001D05 RID: 7429 RVA: 0x00049D57 File Offset: 0x00048D57
		public AssemblyTitleAttribute(string title)
		{
			this.m_title = title;
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x00049D66 File Offset: 0x00048D66
		public string Title
		{
			get
			{
				return this.m_title;
			}
		}

		// Token: 0x04000AD0 RID: 2768
		private string m_title;
	}
}
