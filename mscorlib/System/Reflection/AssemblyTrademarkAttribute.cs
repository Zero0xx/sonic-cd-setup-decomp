using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E0 RID: 736
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyTrademarkAttribute : Attribute
	{
		// Token: 0x06001CFD RID: 7421 RVA: 0x00049CFB File Offset: 0x00048CFB
		public AssemblyTrademarkAttribute(string trademark)
		{
			this.m_trademark = trademark;
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x00049D0A File Offset: 0x00048D0A
		public string Trademark
		{
			get
			{
				return this.m_trademark;
			}
		}

		// Token: 0x04000ACC RID: 2764
		private string m_trademark;
	}
}
