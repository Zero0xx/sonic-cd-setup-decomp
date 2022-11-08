using System;
using System.Reflection.Emit;

namespace System.Security
{
	// Token: 0x02000671 RID: 1649
	internal class FrameSecurityDescriptorWithResolver : FrameSecurityDescriptor
	{
		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06003B63 RID: 15203 RVA: 0x000C9378 File Offset: 0x000C8378
		public DynamicResolver Resolver
		{
			get
			{
				return this.m_resolver;
			}
		}

		// Token: 0x04001EB6 RID: 7862
		private DynamicResolver m_resolver;
	}
}
