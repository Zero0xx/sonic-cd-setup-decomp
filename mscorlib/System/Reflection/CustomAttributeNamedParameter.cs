using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000306 RID: 774
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct CustomAttributeNamedParameter
	{
		// Token: 0x06001E5E RID: 7774 RVA: 0x0004BCC6 File Offset: 0x0004ACC6
		public CustomAttributeNamedParameter(string argumentName, CustomAttributeEncoding fieldOrProperty, CustomAttributeType type)
		{
			if (argumentName == null)
			{
				throw new ArgumentNullException("argumentName");
			}
			this.m_argumentName = argumentName;
			this.m_fieldOrProperty = fieldOrProperty;
			this.m_padding = fieldOrProperty;
			this.m_type = type;
			this.m_encodedArgument = default(CustomAttributeEncodedArgument);
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x0004BCFE File Offset: 0x0004ACFE
		public CustomAttributeEncodedArgument EncodedArgument
		{
			get
			{
				return this.m_encodedArgument;
			}
		}

		// Token: 0x04000B36 RID: 2870
		private string m_argumentName;

		// Token: 0x04000B37 RID: 2871
		private CustomAttributeEncoding m_fieldOrProperty;

		// Token: 0x04000B38 RID: 2872
		private CustomAttributeEncoding m_padding;

		// Token: 0x04000B39 RID: 2873
		private CustomAttributeType m_type;

		// Token: 0x04000B3A RID: 2874
		private CustomAttributeEncodedArgument m_encodedArgument;
	}
}
