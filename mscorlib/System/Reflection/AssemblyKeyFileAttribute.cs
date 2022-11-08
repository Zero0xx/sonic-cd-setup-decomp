using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EB RID: 747
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyKeyFileAttribute : Attribute
	{
		// Token: 0x06001D13 RID: 7443 RVA: 0x00049E06 File Offset: 0x00048E06
		public AssemblyKeyFileAttribute(string keyFile)
		{
			this.m_keyFile = keyFile;
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x00049E15 File Offset: 0x00048E15
		public string KeyFile
		{
			get
			{
				return this.m_keyFile;
			}
		}

		// Token: 0x04000AD7 RID: 2775
		private string m_keyFile;
	}
}
