using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E3 RID: 739
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyDescriptionAttribute : Attribute
	{
		// Token: 0x06001D03 RID: 7427 RVA: 0x00049D40 File Offset: 0x00048D40
		public AssemblyDescriptionAttribute(string description)
		{
			this.m_description = description;
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x00049D4F File Offset: 0x00048D4F
		public string Description
		{
			get
			{
				return this.m_description;
			}
		}

		// Token: 0x04000ACF RID: 2767
		private string m_description;
	}
}
