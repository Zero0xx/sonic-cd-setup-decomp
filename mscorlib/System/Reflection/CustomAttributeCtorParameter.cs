using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000307 RID: 775
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct CustomAttributeCtorParameter
	{
		// Token: 0x06001E60 RID: 7776 RVA: 0x0004BD06 File Offset: 0x0004AD06
		public CustomAttributeCtorParameter(CustomAttributeType type)
		{
			this.m_type = type;
			this.m_encodedArgument = default(CustomAttributeEncodedArgument);
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0004BD1B File Offset: 0x0004AD1B
		public CustomAttributeEncodedArgument CustomAttributeEncodedArgument
		{
			get
			{
				return this.m_encodedArgument;
			}
		}

		// Token: 0x04000B3B RID: 2875
		private CustomAttributeType m_type;

		// Token: 0x04000B3C RID: 2876
		private CustomAttributeEncodedArgument m_encodedArgument;
	}
}
