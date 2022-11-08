using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EC RID: 748
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDelaySignAttribute : Attribute
	{
		// Token: 0x06001D15 RID: 7445 RVA: 0x00049E1D File Offset: 0x00048E1D
		public AssemblyDelaySignAttribute(bool delaySign)
		{
			this.m_delaySign = delaySign;
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x00049E2C File Offset: 0x00048E2C
		public bool DelaySign
		{
			get
			{
				return this.m_delaySign;
			}
		}

		// Token: 0x04000AD8 RID: 2776
		private bool m_delaySign;
	}
}
