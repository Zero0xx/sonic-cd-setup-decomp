using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x02000825 RID: 2085
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_FieldBuilder))]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class FieldBuilder : FieldInfo, _FieldBuilder
	{
		// Token: 0x06004A30 RID: 18992 RVA: 0x00101DE0 File Offset: 0x00100DE0
		internal FieldBuilder(TypeBuilder typeBuilder, string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			if (fieldName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "fieldName");
			}
			if (fieldName[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "fieldName");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == typeof(void))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadFieldType"));
			}
			this.m_fieldName = fieldName;
			this.m_typeBuilder = typeBuilder;
			this.m_fieldType = type;
			this.m_Attributes = (attributes & ~FieldAttributes.ReservedMask);
			SignatureHelper fieldSigHelper = SignatureHelper.GetFieldSigHelper(this.m_typeBuilder.Module);
			fieldSigHelper.AddArgument(type, requiredCustomModifiers, optionalCustomModifiers);
			int sigLength;
			byte[] signature = fieldSigHelper.InternalGetSignature(out sigLength);
			this.m_fieldTok = TypeBuilder.InternalDefineField(typeBuilder.TypeToken.Token, fieldName, signature, sigLength, this.m_Attributes, this.m_typeBuilder.Module);
			this.m_tkField = new FieldToken(this.m_fieldTok, type);
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x00101EEE File Offset: 0x00100EEE
		internal void SetData(byte[] data, int size)
		{
			if (data != null)
			{
				this.m_data = new byte[data.Length];
				Array.Copy(data, this.m_data, data.Length);
			}
			this.m_typeBuilder.Module.InternalSetFieldRVAContent(this.m_tkField.Token, data, size);
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x00101F2D File Offset: 0x00100F2D
		internal TypeBuilder GetTypeBuilder()
		{
			return this.m_typeBuilder;
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06004A33 RID: 18995 RVA: 0x00101F35 File Offset: 0x00100F35
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_fieldTok;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06004A34 RID: 18996 RVA: 0x00101F3D File Offset: 0x00100F3D
		public override Module Module
		{
			get
			{
				return this.m_typeBuilder.Module;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06004A35 RID: 18997 RVA: 0x00101F4A File Offset: 0x00100F4A
		public override string Name
		{
			get
			{
				return this.m_fieldName;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06004A36 RID: 18998 RVA: 0x00101F52 File Offset: 0x00100F52
		public override Type DeclaringType
		{
			get
			{
				if (this.m_typeBuilder.m_isHiddenGlobalType)
				{
					return null;
				}
				return this.m_typeBuilder;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06004A37 RID: 18999 RVA: 0x00101F69 File Offset: 0x00100F69
		public override Type ReflectedType
		{
			get
			{
				if (this.m_typeBuilder.m_isHiddenGlobalType)
				{
					return null;
				}
				return this.m_typeBuilder;
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06004A38 RID: 19000 RVA: 0x00101F80 File Offset: 0x00100F80
		public override Type FieldType
		{
			get
			{
				return this.m_fieldType;
			}
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x00101F88 File Offset: 0x00100F88
		public override object GetValue(object obj)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x00101F99 File Offset: 0x00100F99
		public override void SetValue(object obj, object val, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06004A3B RID: 19003 RVA: 0x00101FAA File Offset: 0x00100FAA
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06004A3C RID: 19004 RVA: 0x00101FBB File Offset: 0x00100FBB
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_Attributes;
			}
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x00101FC3 File Offset: 0x00100FC3
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x00101FD4 File Offset: 0x00100FD4
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x00101FE5 File Offset: 0x00100FE5
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x00101FF6 File Offset: 0x00100FF6
		public FieldToken GetToken()
		{
			return this.m_tkField;
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x00102000 File Offset: 0x00101000
		public void SetOffset(int iOffset)
		{
			this.m_typeBuilder.ThrowIfCreated();
			TypeBuilder.InternalSetFieldOffset(this.m_typeBuilder.Module, this.GetToken().Token, iOffset);
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x00102038 File Offset: 0x00101038
		[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void SetMarshal(UnmanagedMarshal unmanagedMarshal)
		{
			this.m_typeBuilder.ThrowIfCreated();
			if (unmanagedMarshal == null)
			{
				throw new ArgumentNullException("unmanagedMarshal");
			}
			byte[] array = unmanagedMarshal.InternalGetBytes();
			TypeBuilder.InternalSetMarshalInfo(this.m_typeBuilder.Module, this.GetToken().Token, array, array.Length);
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x00102088 File Offset: 0x00101088
		public void SetConstant(object defaultValue)
		{
			this.m_typeBuilder.ThrowIfCreated();
			TypeBuilder.SetConstantValue(this.m_typeBuilder.Module, this.GetToken().Token, this.m_fieldType, defaultValue);
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x001020C8 File Offset: 0x001010C8
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			ModuleBuilder moduleBuilder = this.m_typeBuilder.Module as ModuleBuilder;
			this.m_typeBuilder.ThrowIfCreated();
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			TypeBuilder.InternalCreateCustomAttribute(this.m_tkField.Token, moduleBuilder.GetConstructorToken(con).Token, binaryAttribute, moduleBuilder, false);
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x00102130 File Offset: 0x00101130
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.m_typeBuilder.ThrowIfCreated();
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			ModuleBuilder mod = this.m_typeBuilder.Module as ModuleBuilder;
			customBuilder.CreateCustomAttribute(mod, this.m_tkField.Token);
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x00102179 File Offset: 0x00101179
		void _FieldBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x00102180 File Offset: 0x00101180
		void _FieldBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x00102187 File Offset: 0x00101187
		void _FieldBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x0010218E File Offset: 0x0010118E
		void _FieldBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040025E6 RID: 9702
		private int m_fieldTok;

		// Token: 0x040025E7 RID: 9703
		private FieldToken m_tkField;

		// Token: 0x040025E8 RID: 9704
		private TypeBuilder m_typeBuilder;

		// Token: 0x040025E9 RID: 9705
		private string m_fieldName;

		// Token: 0x040025EA RID: 9706
		private FieldAttributes m_Attributes;

		// Token: 0x040025EB RID: 9707
		private Type m_fieldType;

		// Token: 0x040025EC RID: 9708
		internal byte[] m_data;
	}
}
