using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002B3 RID: 691
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	[ComVisible(true)]
	[Serializable]
	public sealed class ConditionalAttribute : Attribute
	{
		// Token: 0x06001B0E RID: 6926 RVA: 0x00046DB0 File Offset: 0x00045DB0
		public ConditionalAttribute(string conditionString)
		{
			this.m_conditionString = conditionString;
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x00046DBF File Offset: 0x00045DBF
		public string ConditionString
		{
			get
			{
				return this.m_conditionString;
			}
		}

		// Token: 0x04000A51 RID: 2641
		private string m_conditionString;
	}
}
