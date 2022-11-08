using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036B RID: 875
	internal class TypeLoadExceptionHolder
	{
		// Token: 0x0600227F RID: 8831 RVA: 0x0005711B File Offset: 0x0005611B
		internal TypeLoadExceptionHolder(string typeName)
		{
			this.m_typeName = typeName;
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x0005712A File Offset: 0x0005612A
		internal string TypeName
		{
			get
			{
				return this.m_typeName;
			}
		}

		// Token: 0x04000E87 RID: 3719
		private string m_typeName;
	}
}
