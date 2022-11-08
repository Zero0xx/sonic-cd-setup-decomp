using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E1 RID: 737
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyProductAttribute : Attribute
	{
		// Token: 0x06001CFF RID: 7423 RVA: 0x00049D12 File Offset: 0x00048D12
		public AssemblyProductAttribute(string product)
		{
			this.m_product = product;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x00049D21 File Offset: 0x00048D21
		public string Product
		{
			get
			{
				return this.m_product;
			}
		}

		// Token: 0x04000ACD RID: 2765
		private string m_product;
	}
}
