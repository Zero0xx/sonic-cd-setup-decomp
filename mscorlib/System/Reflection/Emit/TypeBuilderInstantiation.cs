using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200084D RID: 2125
	internal sealed class TypeBuilderInstantiation : Type
	{
		// Token: 0x06004D2E RID: 19758 RVA: 0x0010E9B1 File Offset: 0x0010D9B1
		internal TypeBuilderInstantiation(Type type, Type[] inst)
		{
			this.m_type = type;
			this.m_inst = inst;
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x0010E9C7 File Offset: 0x0010D9C7
		public override string ToString()
		{
			return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.ToString);
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06004D30 RID: 19760 RVA: 0x0010E9D0 File Offset: 0x0010D9D0
		public override Type DeclaringType
		{
			get
			{
				return this.m_type.DeclaringType;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0010E9DD File Offset: 0x0010D9DD
		public override Type ReflectedType
		{
			get
			{
				return this.m_type.ReflectedType;
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06004D32 RID: 19762 RVA: 0x0010E9EA File Offset: 0x0010D9EA
		public override string Name
		{
			get
			{
				return this.m_type.Name;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06004D33 RID: 19763 RVA: 0x0010E9F7 File Offset: 0x0010D9F7
		public override Module Module
		{
			get
			{
				return this.m_type.Module;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06004D34 RID: 19764 RVA: 0x0010EA04 File Offset: 0x0010DA04
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0010EA0B File Offset: 0x0010DA0B
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType("*".ToCharArray(), this, 0);
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x0010EA1E File Offset: 0x0010DA1E
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType("&".ToCharArray(), this, 0);
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x0010EA31 File Offset: 0x0010DA31
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x0010EA44 File Offset: 0x0010DA44
		public override Type MakeArrayType(int rank)
		{
			if (rank <= 0)
			{
				throw new IndexOutOfRangeException();
			}
			string text = "";
			for (int i = 1; i < rank; i++)
			{
				text += ",";
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[]
			{
				text
			});
			return SymbolType.FormCompoundType(text2.ToCharArray(), this, 0);
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06004D39 RID: 19769 RVA: 0x0010EAA2 File Offset: 0x0010DAA2
		public override Guid GUID
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x0010EAA9 File Offset: 0x0010DAA9
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06004D3B RID: 19771 RVA: 0x0010EAB0 File Offset: 0x0010DAB0
		public override Assembly Assembly
		{
			get
			{
				return this.m_type.Assembly;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06004D3C RID: 19772 RVA: 0x0010EABD File Offset: 0x0010DABD
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06004D3D RID: 19773 RVA: 0x0010EAC4 File Offset: 0x0010DAC4
		public override string FullName
		{
			get
			{
				if (this.m_strFullQualName == null)
				{
					this.m_strFullQualName = TypeNameBuilder.ToString(this, TypeNameBuilder.Format.FullName);
				}
				return this.m_strFullQualName;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06004D3E RID: 19774 RVA: 0x0010EAE1 File Offset: 0x0010DAE1
		public override string Namespace
		{
			get
			{
				return this.m_type.Namespace;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06004D3F RID: 19775 RVA: 0x0010EAEE File Offset: 0x0010DAEE
		public override string AssemblyQualifiedName
		{
			get
			{
				return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.AssemblyQualifiedName);
			}
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x0010EAF8 File Offset: 0x0010DAF8
		internal Type Substitute(Type[] substitutes)
		{
			Type[] genericArguments = this.GetGenericArguments();
			Type[] array = new Type[genericArguments.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Type type = genericArguments[i];
				if (type is TypeBuilderInstantiation)
				{
					array[i] = (type as TypeBuilderInstantiation).Substitute(substitutes);
				}
				else if (type is GenericTypeParameterBuilder)
				{
					array[i] = substitutes[type.GenericParameterPosition];
				}
				else
				{
					array[i] = type;
				}
			}
			return this.GetGenericTypeDefinition().MakeGenericType(array);
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06004D41 RID: 19777 RVA: 0x0010EB68 File Offset: 0x0010DB68
		public override Type BaseType
		{
			get
			{
				Type baseType = this.m_type.BaseType;
				if (baseType == null)
				{
					return null;
				}
				TypeBuilderInstantiation typeBuilderInstantiation = baseType as TypeBuilderInstantiation;
				if (typeBuilderInstantiation == null)
				{
					return baseType;
				}
				return typeBuilderInstantiation.Substitute(this.GetGenericArguments());
			}
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x0010EB9E File Offset: 0x0010DB9E
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0010EBA5 File Offset: 0x0010DBA5
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x0010EBAC File Offset: 0x0010DBAC
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x0010EBB3 File Offset: 0x0010DBB3
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x0010EBBA File Offset: 0x0010DBBA
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0010EBC1 File Offset: 0x0010DBC1
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x0010EBC8 File Offset: 0x0010DBC8
		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0010EBCF File Offset: 0x0010DBCF
		public override Type[] GetInterfaces()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0010EBD6 File Offset: 0x0010DBD6
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x0010EBDD File Offset: 0x0010DBDD
		public override EventInfo[] GetEvents()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x0010EBE4 File Offset: 0x0010DBE4
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0010EBEB File Offset: 0x0010DBEB
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x0010EBF2 File Offset: 0x0010DBF2
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x0010EBF9 File Offset: 0x0010DBF9
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x0010EC00 File Offset: 0x0010DC00
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x0010EC07 File Offset: 0x0010DC07
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x0010EC0E File Offset: 0x0010DC0E
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0010EC15 File Offset: 0x0010DC15
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0010EC1C File Offset: 0x0010DC1C
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.m_type.Attributes;
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0010EC29 File Offset: 0x0010DC29
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x0010EC2C File Offset: 0x0010DC2C
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x0010EC2F File Offset: 0x0010DC2F
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x0010EC32 File Offset: 0x0010DC32
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x0010EC35 File Offset: 0x0010DC35
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x0010EC38 File Offset: 0x0010DC38
		public override Type GetElementType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x0010EC3F File Offset: 0x0010DC3F
		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06004D5C RID: 19804 RVA: 0x0010EC42 File Offset: 0x0010DC42
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x0010EC45 File Offset: 0x0010DC45
		public override Type[] GetGenericArguments()
		{
			return this.m_inst;
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06004D5E RID: 19806 RVA: 0x0010EC4D File Offset: 0x0010DC4D
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06004D5F RID: 19807 RVA: 0x0010EC50 File Offset: 0x0010DC50
		public override bool IsGenericType
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004D60 RID: 19808 RVA: 0x0010EC53 File Offset: 0x0010DC53
		public override bool IsGenericParameter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004D61 RID: 19809 RVA: 0x0010EC56 File Offset: 0x0010DC56
		public override int GenericParameterPosition
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x0010EC5D File Offset: 0x0010DC5D
		protected override bool IsValueTypeImpl()
		{
			return this.m_type.IsValueType;
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004D63 RID: 19811 RVA: 0x0010EC6C File Offset: 0x0010DC6C
		public override bool ContainsGenericParameters
		{
			get
			{
				for (int i = 0; i < this.m_inst.Length; i++)
				{
					if (this.m_inst[i].ContainsGenericParameters)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06004D64 RID: 19812 RVA: 0x0010EC9E File Offset: 0x0010DC9E
		public override MethodBase DeclaringMethod
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x0010ECA1 File Offset: 0x0010DCA1
		public override Type GetGenericTypeDefinition()
		{
			return this.m_type;
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x0010ECA9 File Offset: 0x0010DCA9
		public override Type MakeGenericType(params Type[] inst)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericTypeDefinition"));
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x0010ECBA File Offset: 0x0010DCBA
		public override bool IsAssignableFrom(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x0010ECC1 File Offset: 0x0010DCC1
		[ComVisible(true)]
		public override bool IsSubclassOf(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x0010ECC8 File Offset: 0x0010DCC8
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x0010ECCF File Offset: 0x0010DCCF
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x0010ECD6 File Offset: 0x0010DCD6
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04002846 RID: 10310
		private Type m_type;

		// Token: 0x04002847 RID: 10311
		private Type[] m_inst;

		// Token: 0x04002848 RID: 10312
		private string m_strFullQualName;
	}
}
