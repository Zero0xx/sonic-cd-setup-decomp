using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200030C RID: 780
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DefaultMemberAttribute : Attribute
	{
		// Token: 0x06001E9F RID: 7839 RVA: 0x0004D56B File Offset: 0x0004C56B
		public DefaultMemberAttribute(string memberName)
		{
			this.m_memberName = memberName;
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x0004D57A File Offset: 0x0004C57A
		public string MemberName
		{
			get
			{
				return this.m_memberName;
			}
		}

		// Token: 0x04000B48 RID: 2888
		private string m_memberName;
	}
}
