using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000326 RID: 806
	internal struct MetadataImport
	{
		// Token: 0x06001EC1 RID: 7873 RVA: 0x0004D7A3 File Offset: 0x0004C7A3
		public override int GetHashCode()
		{
			return this.m_metadataImport2.GetHashCode();
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0004D7B6 File Offset: 0x0004C7B6
		public override bool Equals(object obj)
		{
			return obj is MetadataImport && this.Equals((MetadataImport)obj);
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0004D7CE File Offset: 0x0004C7CE
		internal bool Equals(MetadataImport import)
		{
			return import.m_metadataImport2 == this.m_metadataImport2;
		}

		// Token: 0x06001EC4 RID: 7876
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetMarshalAs(IntPtr pNativeType, int cNativeType, out int unmanagedType, out int safeArraySubType, out string safeArrayUserDefinedSubType, out int arraySubType, out int sizeParamIndex, out int sizeConst, out string marshalType, out string marshalCookie, out int iidParamIndex);

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0004D7E4 File Offset: 0x0004C7E4
		internal static void GetMarshalAs(ConstArray nativeType, out UnmanagedType unmanagedType, out VarEnum safeArraySubType, out string safeArrayUserDefinedSubType, out UnmanagedType arraySubType, out int sizeParamIndex, out int sizeConst, out string marshalType, out string marshalCookie, out int iidParamIndex)
		{
			int num;
			int num2;
			int num3;
			MetadataImport._GetMarshalAs(nativeType.Signature, nativeType.Length, out num, out num2, out safeArrayUserDefinedSubType, out num3, out sizeParamIndex, out sizeConst, out marshalType, out marshalCookie, out iidParamIndex);
			unmanagedType = (UnmanagedType)num;
			safeArraySubType = (VarEnum)num2;
			arraySubType = (UnmanagedType)num3;
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0004D81F File Offset: 0x0004C81F
		internal static void ThrowError(int hResult)
		{
			throw new MetadataException(hResult);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0004D827 File Offset: 0x0004C827
		internal MetadataImport(IntPtr metadataImport2)
		{
			this.m_metadataImport2 = metadataImport2;
		}

		// Token: 0x06001EC8 RID: 7880
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _Enum(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int type, int parent, int* result, int count);

		// Token: 0x06001EC9 RID: 7881
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _EnumCount(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int type, int parent, out int count);

		// Token: 0x06001ECA RID: 7882 RVA: 0x0004D830 File Offset: 0x0004C830
		public unsafe void Enum(int type, int parent, int* result, int count)
		{
			MetadataImport._Enum(this.m_metadataImport2, out MetadataArgs.Skip, type, parent, result, count);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0004D848 File Offset: 0x0004C848
		public int EnumCount(int type, int parent)
		{
			int result = 0;
			MetadataImport._EnumCount(this.m_metadataImport2, out MetadataArgs.Skip, type, parent, out result);
			return result;
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x0004D86D File Offset: 0x0004C86D
		public unsafe void EnumNestedTypes(int mdTypeDef, int* result, int count)
		{
			this.Enum(33554432, mdTypeDef, result, count);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0004D87D File Offset: 0x0004C87D
		public int EnumNestedTypesCount(int mdTypeDef)
		{
			return this.EnumCount(33554432, mdTypeDef);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0004D88B File Offset: 0x0004C88B
		public unsafe void EnumCustomAttributes(int mdToken, int* result, int count)
		{
			this.Enum(201326592, mdToken, result, count);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0004D89B File Offset: 0x0004C89B
		public int EnumCustomAttributesCount(int mdToken)
		{
			return this.EnumCount(201326592, mdToken);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0004D8A9 File Offset: 0x0004C8A9
		public unsafe void EnumParams(int mdMethodDef, int* result, int count)
		{
			this.Enum(134217728, mdMethodDef, result, count);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0004D8B9 File Offset: 0x0004C8B9
		public int EnumParamsCount(int mdMethodDef)
		{
			return this.EnumCount(134217728, mdMethodDef);
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0004D8C8 File Offset: 0x0004C8C8
		public unsafe void GetAssociates(int mdPropEvent, AssociateRecord* result, int count)
		{
			int* ptr = stackalloc int[4 * (count * 2)];
			this.Enum(100663296, mdPropEvent, ptr, count);
			for (int i = 0; i < count; i++)
			{
				result[i].MethodDefToken = ptr[i * 2];
				result[i].Semantics = (MethodSemanticsAttributes)ptr[i * 2 + 1];
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0004D92B File Offset: 0x0004C92B
		public int GetAssociatesCount(int mdPropEvent)
		{
			return this.EnumCount(100663296, mdPropEvent);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0004D939 File Offset: 0x0004C939
		public unsafe void EnumFields(int mdTypeDef, int* result, int count)
		{
			this.Enum(67108864, mdTypeDef, result, count);
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x0004D949 File Offset: 0x0004C949
		public int EnumFieldsCount(int mdTypeDef)
		{
			return this.EnumCount(67108864, mdTypeDef);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0004D957 File Offset: 0x0004C957
		public unsafe void EnumProperties(int mdTypeDef, int* result, int count)
		{
			this.Enum(385875968, mdTypeDef, result, count);
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0004D967 File Offset: 0x0004C967
		public int EnumPropertiesCount(int mdTypeDef)
		{
			return this.EnumCount(385875968, mdTypeDef);
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0004D975 File Offset: 0x0004C975
		public unsafe void EnumEvents(int mdTypeDef, int* result, int count)
		{
			this.Enum(335544320, mdTypeDef, result, count);
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0004D985 File Offset: 0x0004C985
		public int EnumEventsCount(int mdTypeDef)
		{
			return this.EnumCount(335544320, mdTypeDef);
		}

		// Token: 0x06001EDA RID: 7898
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetDefaultValue(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, out long value, out int length, out int corElementType);

		// Token: 0x06001EDB RID: 7899 RVA: 0x0004D994 File Offset: 0x0004C994
		public void GetDefaultValue(int mdToken, out long value, out int length, out CorElementType corElementType)
		{
			int num;
			MetadataImport._GetDefaultValue(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, out value, out length, out num);
			corElementType = (CorElementType)num;
		}

		// Token: 0x06001EDC RID: 7900
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetUserString(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** name, out int length);

		// Token: 0x06001EDD RID: 7901 RVA: 0x0004D9BC File Offset: 0x0004C9BC
		public unsafe string GetUserString(int mdToken)
		{
			void* ptr;
			int num;
			MetadataImport._GetUserString(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &ptr, out num);
			if (ptr == null)
			{
				return null;
			}
			char[] array = new char[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (char)Marshal.ReadInt16((IntPtr)((void*)((byte*)ptr + (IntPtr)i * 2)));
			}
			return new string(array);
		}

		// Token: 0x06001EDE RID: 7902
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetName(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** name);

		// Token: 0x06001EDF RID: 7903 RVA: 0x0004DA14 File Offset: 0x0004CA14
		public unsafe Utf8String GetName(int mdToken)
		{
			void* pStringHeap;
			MetadataImport._GetName(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &pStringHeap);
			return new Utf8String(pStringHeap);
		}

		// Token: 0x06001EE0 RID: 7904
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetNamespace(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** namesp);

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0004DA3C File Offset: 0x0004CA3C
		public unsafe Utf8String GetNamespace(int mdToken)
		{
			void* pStringHeap;
			MetadataImport._GetNamespace(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &pStringHeap);
			return new Utf8String(pStringHeap);
		}

		// Token: 0x06001EE2 RID: 7906
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetEventProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** name, out int eventAttributes);

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0004DA64 File Offset: 0x0004CA64
		public unsafe void GetEventProps(int mdToken, out void* name, out EventAttributes eventAttributes)
		{
			void* ptr;
			int num;
			MetadataImport._GetEventProps(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &ptr, out num);
			name = ptr;
			eventAttributes = (EventAttributes)num;
		}

		// Token: 0x06001EE4 RID: 7908
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetFieldDefProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, out int fieldAttributes);

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0004DA90 File Offset: 0x0004CA90
		public void GetFieldDefProps(int mdToken, out FieldAttributes fieldAttributes)
		{
			int num;
			MetadataImport._GetFieldDefProps(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, out num);
			fieldAttributes = (FieldAttributes)num;
		}

		// Token: 0x06001EE6 RID: 7910
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetPropertyProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** name, out int propertyAttributes, out ConstArray signature);

		// Token: 0x06001EE7 RID: 7911 RVA: 0x0004DAB4 File Offset: 0x0004CAB4
		public unsafe void GetPropertyProps(int mdToken, out void* name, out PropertyAttributes propertyAttributes, out ConstArray signature)
		{
			void* ptr;
			int num;
			MetadataImport._GetPropertyProps(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &ptr, out num, out signature);
			name = ptr;
			propertyAttributes = (PropertyAttributes)num;
		}

		// Token: 0x06001EE8 RID: 7912
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetParentToken(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, out int tkParent);

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0004DAE0 File Offset: 0x0004CAE0
		public int GetParentToken(int tkToken)
		{
			int result;
			MetadataImport._GetParentToken(this.m_metadataImport2, out MetadataArgs.Skip, tkToken, out result);
			return result;
		}

		// Token: 0x06001EEA RID: 7914
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetParamDefProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int parameterToken, out int sequence, out int attributes);

		// Token: 0x06001EEB RID: 7915 RVA: 0x0004DB04 File Offset: 0x0004CB04
		public void GetParamDefProps(int parameterToken, out int sequence, out ParameterAttributes attributes)
		{
			int num;
			MetadataImport._GetParamDefProps(this.m_metadataImport2, out MetadataArgs.Skip, parameterToken, out sequence, out num);
			attributes = (ParameterAttributes)num;
		}

		// Token: 0x06001EEC RID: 7916
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetGenericParamProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int genericParameter, out int flags);

		// Token: 0x06001EED RID: 7917 RVA: 0x0004DB28 File Offset: 0x0004CB28
		public void GetGenericParamProps(int genericParameter, out GenericParameterAttributes attributes)
		{
			int num;
			MetadataImport._GetGenericParamProps(this.m_metadataImport2, out MetadataArgs.Skip, genericParameter, out num);
			attributes = (GenericParameterAttributes)num;
		}

		// Token: 0x06001EEE RID: 7918
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetScopeProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, out Guid mvid);

		// Token: 0x06001EEF RID: 7919 RVA: 0x0004DB4B File Offset: 0x0004CB4B
		public void GetScopeProps(out Guid mvid)
		{
			MetadataImport._GetScopeProps(this.m_metadataImport2, out MetadataArgs.Skip, out mvid);
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0004DB5E File Offset: 0x0004CB5E
		public ConstArray GetMethodSignature(MetadataToken token)
		{
			if (token.IsMemberRef)
			{
				return this.GetMemberRefProps(token);
			}
			return this.GetSigOfMethodDef(token);
		}

		// Token: 0x06001EF1 RID: 7921
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetSigOfMethodDef(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int methodToken, ref ConstArray signature);

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0004DB84 File Offset: 0x0004CB84
		public ConstArray GetSigOfMethodDef(int methodToken)
		{
			ConstArray result = default(ConstArray);
			MetadataImport._GetSigOfMethodDef(this.m_metadataImport2, out MetadataArgs.Skip, methodToken, ref result);
			return result;
		}

		// Token: 0x06001EF3 RID: 7923
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetSignatureFromToken(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int methodToken, ref ConstArray signature);

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0004DBB0 File Offset: 0x0004CBB0
		public ConstArray GetSignatureFromToken(int token)
		{
			ConstArray result = default(ConstArray);
			MetadataImport._GetSignatureFromToken(this.m_metadataImport2, out MetadataArgs.Skip, token, ref result);
			return result;
		}

		// Token: 0x06001EF5 RID: 7925
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetMemberRefProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int memberTokenRef, out ConstArray signature);

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0004DBDC File Offset: 0x0004CBDC
		public ConstArray GetMemberRefProps(int memberTokenRef)
		{
			ConstArray result = default(ConstArray);
			MetadataImport._GetMemberRefProps(this.m_metadataImport2, out MetadataArgs.Skip, memberTokenRef, out result);
			return result;
		}

		// Token: 0x06001EF7 RID: 7927
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetCustomAttributeProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int customAttributeToken, out int constructorToken, out ConstArray signature);

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0004DC05 File Offset: 0x0004CC05
		public void GetCustomAttributeProps(int customAttributeToken, out int constructorToken, out ConstArray signature)
		{
			MetadataImport._GetCustomAttributeProps(this.m_metadataImport2, out MetadataArgs.Skip, customAttributeToken, out constructorToken, out signature);
		}

		// Token: 0x06001EF9 RID: 7929
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetClassLayout(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int typeTokenDef, out int packSize, out int classSize);

		// Token: 0x06001EFA RID: 7930 RVA: 0x0004DC1A File Offset: 0x0004CC1A
		public void GetClassLayout(int typeTokenDef, out int packSize, out int classSize)
		{
			MetadataImport._GetClassLayout(this.m_metadataImport2, out MetadataArgs.Skip, typeTokenDef, out packSize, out classSize);
		}

		// Token: 0x06001EFB RID: 7931
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool _GetFieldOffset(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int typeTokenDef, int fieldTokenDef, out int offset);

		// Token: 0x06001EFC RID: 7932 RVA: 0x0004DC2F File Offset: 0x0004CC2F
		public bool GetFieldOffset(int typeTokenDef, int fieldTokenDef, out int offset)
		{
			return MetadataImport._GetFieldOffset(this.m_metadataImport2, out MetadataArgs.Skip, typeTokenDef, fieldTokenDef, out offset);
		}

		// Token: 0x06001EFD RID: 7933
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetSigOfFieldDef(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int fieldToken, ref ConstArray fieldMarshal);

		// Token: 0x06001EFE RID: 7934 RVA: 0x0004DC44 File Offset: 0x0004CC44
		public ConstArray GetSigOfFieldDef(int fieldToken)
		{
			ConstArray result = default(ConstArray);
			MetadataImport._GetSigOfFieldDef(this.m_metadataImport2, out MetadataArgs.Skip, fieldToken, ref result);
			return result;
		}

		// Token: 0x06001EFF RID: 7935
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetFieldMarshal(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int fieldToken, ref ConstArray fieldMarshal);

		// Token: 0x06001F00 RID: 7936 RVA: 0x0004DC70 File Offset: 0x0004CC70
		public ConstArray GetFieldMarshal(int fieldToken)
		{
			ConstArray result = default(ConstArray);
			MetadataImport._GetFieldMarshal(this.m_metadataImport2, out MetadataArgs.Skip, fieldToken, ref result);
			return result;
		}

		// Token: 0x06001F01 RID: 7937
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetPInvokeMap(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int token, out int attributes, void** importName, void** importDll);

		// Token: 0x06001F02 RID: 7938 RVA: 0x0004DC9C File Offset: 0x0004CC9C
		public unsafe void GetPInvokeMap(int token, out PInvokeAttributes attributes, out string importName, out string importDll)
		{
			int num;
			void* pStringHeap;
			void* pStringHeap2;
			MetadataImport._GetPInvokeMap(this.m_metadataImport2, out MetadataArgs.Skip, token, out num, &pStringHeap, &pStringHeap2);
			importName = new Utf8String(pStringHeap).ToString();
			importDll = new Utf8String(pStringHeap2).ToString();
			attributes = (PInvokeAttributes)num;
		}

		// Token: 0x06001F03 RID: 7939
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool _IsValidToken(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int token);

		// Token: 0x06001F04 RID: 7940 RVA: 0x0004DCF3 File Offset: 0x0004CCF3
		public bool IsValidToken(int token)
		{
			return MetadataImport._IsValidToken(this.m_metadataImport2, out MetadataArgs.Skip, token);
		}

		// Token: 0x04000D23 RID: 3363
		private IntPtr m_metadataImport2;

		// Token: 0x04000D24 RID: 3364
		internal static readonly MetadataImport EmptyImport = new MetadataImport((IntPtr)0);

		// Token: 0x04000D25 RID: 3365
		internal static Guid IID_IMetaDataImport = new Guid(3530420970U, 32600, 16771, 134, 190, 48, 174, 41, 167, 93, 141);

		// Token: 0x04000D26 RID: 3366
		internal static Guid IID_IMetaDataAssemblyImport = new Guid(3999418123U, 59723, 16974, 155, 124, 47, 0, 201, 36, 159, 147);

		// Token: 0x04000D27 RID: 3367
		internal static Guid IID_IMetaDataTables = new Guid(3639966123U, 16429, 19342, 130, 217, 93, 99, 177, 6, 92, 104);
	}
}
