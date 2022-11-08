using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036E RID: 878
	[Serializable]
	internal class MemberHolder
	{
		// Token: 0x06002288 RID: 8840 RVA: 0x00057234 File Offset: 0x00056234
		internal MemberHolder(Type type, StreamingContext ctx)
		{
			this.memberType = type;
			this.context = ctx;
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x0005724A File Offset: 0x0005624A
		public override int GetHashCode()
		{
			return this.memberType.GetHashCode();
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00057258 File Offset: 0x00056258
		public override bool Equals(object obj)
		{
			if (!(obj is MemberHolder))
			{
				return false;
			}
			MemberHolder memberHolder = (MemberHolder)obj;
			return memberHolder.memberType == this.memberType && memberHolder.context.State == this.context.State;
		}

		// Token: 0x04000E8C RID: 3724
		internal MemberInfo[] members;

		// Token: 0x04000E8D RID: 3725
		internal Type memberType;

		// Token: 0x04000E8E RID: 3726
		internal StreamingContext context;
	}
}
