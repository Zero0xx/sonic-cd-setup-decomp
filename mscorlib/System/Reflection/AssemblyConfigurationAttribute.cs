using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E5 RID: 741
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyConfigurationAttribute : Attribute
	{
		// Token: 0x06001D07 RID: 7431 RVA: 0x00049D6E File Offset: 0x00048D6E
		public AssemblyConfigurationAttribute(string configuration)
		{
			this.m_configuration = configuration;
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x00049D7D File Offset: 0x00048D7D
		public string Configuration
		{
			get
			{
				return this.m_configuration;
			}
		}

		// Token: 0x04000AD1 RID: 2769
		private string m_configuration;
	}
}
