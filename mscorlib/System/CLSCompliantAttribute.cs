using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000085 RID: 133
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	[Serializable]
	public sealed class CLSCompliantAttribute : Attribute
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x00018251 File Offset: 0x00017251
		public CLSCompliantAttribute(bool isCompliant)
		{
			this.m_compliant = isCompliant;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x00018260 File Offset: 0x00017260
		public bool IsCompliant
		{
			get
			{
				return this.m_compliant;
			}
		}

		// Token: 0x04000287 RID: 647
		private bool m_compliant;
	}
}
