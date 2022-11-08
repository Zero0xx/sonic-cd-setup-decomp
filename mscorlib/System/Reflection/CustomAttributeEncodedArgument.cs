using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000305 RID: 773
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct CustomAttributeEncodedArgument
	{
		// Token: 0x06001E58 RID: 7768
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ParseAttributeArguments(IntPtr pCa, int cCa, ref CustomAttributeCtorParameter[] CustomAttributeCtorParameters, ref CustomAttributeNamedParameter[] CustomAttributeTypedArgument, IntPtr assembly);

		// Token: 0x06001E59 RID: 7769 RVA: 0x0004BC44 File Offset: 0x0004AC44
		internal static void ParseAttributeArguments(ConstArray attributeBlob, ref CustomAttributeCtorParameter[] customAttributeCtorParameters, ref CustomAttributeNamedParameter[] customAttributeNamedParameters, Module customAttributeModule)
		{
			if (customAttributeModule == null)
			{
				throw new ArgumentNullException("customAttributeModule");
			}
			if (customAttributeNamedParameters == null)
			{
				customAttributeNamedParameters = new CustomAttributeNamedParameter[0];
			}
			CustomAttributeCtorParameter[] array = customAttributeCtorParameters;
			CustomAttributeNamedParameter[] array2 = customAttributeNamedParameters;
			CustomAttributeEncodedArgument.ParseAttributeArguments(attributeBlob.Signature, attributeBlob.Length, ref array, ref array2, (IntPtr)customAttributeModule.Assembly.AssemblyHandle.Value);
			customAttributeCtorParameters = array;
			customAttributeNamedParameters = array2;
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x0004BCA6 File Offset: 0x0004ACA6
		public CustomAttributeType CustomAttributeType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001E5B RID: 7771 RVA: 0x0004BCAE File Offset: 0x0004ACAE
		public long PrimitiveValue
		{
			get
			{
				return this.m_primitiveValue;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x0004BCB6 File Offset: 0x0004ACB6
		public CustomAttributeEncodedArgument[] ArrayValue
		{
			get
			{
				return this.m_arrayValue;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x0004BCBE File Offset: 0x0004ACBE
		public string StringValue
		{
			get
			{
				return this.m_stringValue;
			}
		}

		// Token: 0x04000B32 RID: 2866
		private long m_primitiveValue;

		// Token: 0x04000B33 RID: 2867
		private CustomAttributeEncodedArgument[] m_arrayValue;

		// Token: 0x04000B34 RID: 2868
		private string m_stringValue;

		// Token: 0x04000B35 RID: 2869
		private CustomAttributeType m_type;
	}
}
