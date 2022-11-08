using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000309 RID: 777
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct CustomAttributeType
	{
		// Token: 0x06001E64 RID: 7780 RVA: 0x0004BD23 File Offset: 0x0004AD23
		public CustomAttributeType(CustomAttributeEncoding encodedType, CustomAttributeEncoding encodedArrayType, CustomAttributeEncoding encodedEnumType, string enumName)
		{
			this.m_encodedType = encodedType;
			this.m_encodedArrayType = encodedArrayType;
			this.m_encodedEnumType = encodedEnumType;
			this.m_enumName = enumName;
			this.m_padding = this.m_encodedType;
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x0004BD4E File Offset: 0x0004AD4E
		public CustomAttributeEncoding EncodedType
		{
			get
			{
				return this.m_encodedType;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x0004BD56 File Offset: 0x0004AD56
		public CustomAttributeEncoding EncodedEnumType
		{
			get
			{
				return this.m_encodedEnumType;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x0004BD5E File Offset: 0x0004AD5E
		public CustomAttributeEncoding EncodedArrayType
		{
			get
			{
				return this.m_encodedArrayType;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001E68 RID: 7784 RVA: 0x0004BD66 File Offset: 0x0004AD66
		[ComVisible(true)]
		public string EnumName
		{
			get
			{
				return this.m_enumName;
			}
		}

		// Token: 0x04000B41 RID: 2881
		private string m_enumName;

		// Token: 0x04000B42 RID: 2882
		private CustomAttributeEncoding m_encodedType;

		// Token: 0x04000B43 RID: 2883
		private CustomAttributeEncoding m_encodedEnumType;

		// Token: 0x04000B44 RID: 2884
		private CustomAttributeEncoding m_encodedArrayType;

		// Token: 0x04000B45 RID: 2885
		private CustomAttributeEncoding m_padding;
	}
}
