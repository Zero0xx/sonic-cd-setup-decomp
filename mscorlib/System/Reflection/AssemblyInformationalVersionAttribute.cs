using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E7 RID: 743
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyInformationalVersionAttribute : Attribute
	{
		// Token: 0x06001D0B RID: 7435 RVA: 0x00049D9C File Offset: 0x00048D9C
		public AssemblyInformationalVersionAttribute(string informationalVersion)
		{
			this.m_informationalVersion = informationalVersion;
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001D0C RID: 7436 RVA: 0x00049DAB File Offset: 0x00048DAB
		public string InformationalVersion
		{
			get
			{
				return this.m_informationalVersion;
			}
		}

		// Token: 0x04000AD3 RID: 2771
		private string m_informationalVersion;
	}
}
