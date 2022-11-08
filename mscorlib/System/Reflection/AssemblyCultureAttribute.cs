using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E9 RID: 745
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCultureAttribute : Attribute
	{
		// Token: 0x06001D0F RID: 7439 RVA: 0x00049DD8 File Offset: 0x00048DD8
		public AssemblyCultureAttribute(string culture)
		{
			this.m_culture = culture;
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001D10 RID: 7440 RVA: 0x00049DE7 File Offset: 0x00048DE7
		public string Culture
		{
			get
			{
				return this.m_culture;
			}
		}

		// Token: 0x04000AD5 RID: 2773
		private string m_culture;
	}
}
