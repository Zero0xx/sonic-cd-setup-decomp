using System;

namespace System
{
	// Token: 0x02000102 RID: 258
	[Serializable]
	internal class ReflectionOnlyType : RuntimeType
	{
		// Token: 0x06000EA8 RID: 3752 RVA: 0x0002BD3A File Offset: 0x0002AD3A
		private ReflectionOnlyType()
		{
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0002BD42 File Offset: 0x0002AD42
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInReflectionOnly"));
			}
		}
	}
}
