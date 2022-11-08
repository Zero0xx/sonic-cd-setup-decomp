using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200084E RID: 2126
	[ComVisible(true)]
	public sealed class GenericTypeParameterBuilder : Type
	{
		// Token: 0x06004D6C RID: 19820 RVA: 0x0010ECDD File Offset: 0x0010DCDD
		internal GenericTypeParameterBuilder(TypeBuilder type)
		{
			this.m_type = type;
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x0010ECEC File Offset: 0x0010DCEC
		public override string ToString()
		{
			return this.m_type.Name;
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x0010ECFC File Offset: 0x0010DCFC
		public override bool Equals(object o)
		{
			GenericTypeParameterBuilder genericTypeParameterBuilder = o as GenericTypeParameterBuilder;
			return genericTypeParameterBuilder != null && genericTypeParameterBuilder.m_type == this.m_type;
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x0010ED23 File Offset: 0x0010DD23
		public override int GetHashCode()
		{
			return this.m_type.GetHashCode();
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06004D70 RID: 19824 RVA: 0x0010ED30 File Offset: 0x0010DD30
		public override Type DeclaringType
		{
			get
			{
				return this.m_type.DeclaringType;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06004D71 RID: 19825 RVA: 0x0010ED3D File Offset: 0x0010DD3D
		public override Type ReflectedType
		{
			get
			{
				return this.m_type.ReflectedType;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06004D72 RID: 19826 RVA: 0x0010ED4A File Offset: 0x0010DD4A
		public override string Name
		{
			get
			{
				return this.m_type.Name;
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06004D73 RID: 19827 RVA: 0x0010ED57 File Offset: 0x0010DD57
		public override Module Module
		{
			get
			{
				return this.m_type.Module;
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06004D74 RID: 19828 RVA: 0x0010ED64 File Offset: 0x0010DD64
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_type.MetadataTokenInternal;
			}
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x0010ED71 File Offset: 0x0010DD71
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType("*".ToCharArray(), this, 0);
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x0010ED84 File Offset: 0x0010DD84
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType("&".ToCharArray(), this, 0);
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x0010ED97 File Offset: 0x0010DD97
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);
		}

		// Token: 0x06004D78 RID: 19832 RVA: 0x0010EDAC File Offset: 0x0010DDAC
		public override Type MakeArrayType(int rank)
		{
			if (rank <= 0)
			{
				throw new IndexOutOfRangeException();
			}
			string text = "";
			if (rank == 1)
			{
				text = "*";
			}
			else
			{
				for (int i = 1; i < rank; i++)
				{
					text += ",";
				}
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[]
			{
				text
			});
			return SymbolType.FormCompoundType(text2.ToCharArray(), this, 0) as SymbolType;
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06004D79 RID: 19833 RVA: 0x0010EE20 File Offset: 0x0010DE20
		public override Guid GUID
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x0010EE27 File Offset: 0x0010DE27
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06004D7B RID: 19835 RVA: 0x0010EE2E File Offset: 0x0010DE2E
		public override Assembly Assembly
		{
			get
			{
				return this.m_type.Assembly;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06004D7C RID: 19836 RVA: 0x0010EE3B File Offset: 0x0010DE3B
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06004D7D RID: 19837 RVA: 0x0010EE42 File Offset: 0x0010DE42
		public override string FullName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06004D7E RID: 19838 RVA: 0x0010EE45 File Offset: 0x0010DE45
		public override string Namespace
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06004D7F RID: 19839 RVA: 0x0010EE48 File Offset: 0x0010DE48
		public override string AssemblyQualifiedName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06004D80 RID: 19840 RVA: 0x0010EE4B File Offset: 0x0010DE4B
		public override Type BaseType
		{
			get
			{
				return this.m_type.BaseType;
			}
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x0010EE58 File Offset: 0x0010DE58
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0010EE5F File Offset: 0x0010DE5F
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0010EE66 File Offset: 0x0010DE66
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0010EE6D File Offset: 0x0010DE6D
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0010EE74 File Offset: 0x0010DE74
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0010EE7B File Offset: 0x0010DE7B
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0010EE82 File Offset: 0x0010DE82
		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0010EE89 File Offset: 0x0010DE89
		public override Type[] GetInterfaces()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0010EE90 File Offset: 0x0010DE90
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0010EE97 File Offset: 0x0010DE97
		public override EventInfo[] GetEvents()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0010EE9E File Offset: 0x0010DE9E
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x0010EEA5 File Offset: 0x0010DEA5
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0010EEAC File Offset: 0x0010DEAC
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0010EEB3 File Offset: 0x0010DEB3
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x0010EEBA File Offset: 0x0010DEBA
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x0010EEC1 File Offset: 0x0010DEC1
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x0010EEC8 File Offset: 0x0010DEC8
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x0010EECF File Offset: 0x0010DECF
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x0010EED6 File Offset: 0x0010DED6
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x0010EEDD File Offset: 0x0010DEDD
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x0010EEE0 File Offset: 0x0010DEE0
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x0010EEE3 File Offset: 0x0010DEE3
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x0010EEE6 File Offset: 0x0010DEE6
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x0010EEE9 File Offset: 0x0010DEE9
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x0010EEEC File Offset: 0x0010DEEC
		public override Type GetElementType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x0010EEF3 File Offset: 0x0010DEF3
		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x0010EEF6 File Offset: 0x0010DEF6
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x0010EEF9 File Offset: 0x0010DEF9
		public override Type[] GetGenericArguments()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06004D9D RID: 19869 RVA: 0x0010EF00 File Offset: 0x0010DF00
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x0010EF03 File Offset: 0x0010DF03
		public override bool IsGenericType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x0010EF06 File Offset: 0x0010DF06
		public override bool IsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06004DA0 RID: 19872 RVA: 0x0010EF09 File Offset: 0x0010DF09
		public override int GenericParameterPosition
		{
			get
			{
				return this.m_type.GenericParameterPosition;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06004DA1 RID: 19873 RVA: 0x0010EF16 File Offset: 0x0010DF16
		public override bool ContainsGenericParameters
		{
			get
			{
				return this.m_type.ContainsGenericParameters;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06004DA2 RID: 19874 RVA: 0x0010EF23 File Offset: 0x0010DF23
		public override MethodBase DeclaringMethod
		{
			get
			{
				return this.m_type.DeclaringMethod;
			}
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x0010EF30 File Offset: 0x0010DF30
		public override Type GetGenericTypeDefinition()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x0010EF37 File Offset: 0x0010DF37
		public override Type MakeGenericType(params Type[] typeArguments)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericTypeDefinition"));
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x0010EF48 File Offset: 0x0010DF48
		protected override bool IsValueTypeImpl()
		{
			return false;
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x0010EF4B File Offset: 0x0010DF4B
		public override bool IsAssignableFrom(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x0010EF52 File Offset: 0x0010DF52
		[ComVisible(true)]
		public override bool IsSubclassOf(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x0010EF59 File Offset: 0x0010DF59
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x0010EF60 File Offset: 0x0010DF60
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x0010EF67 File Offset: 0x0010DF67
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x0010EF6E File Offset: 0x0010DF6E
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (this.m_type.m_ca == null)
			{
				this.m_type.m_ca = new ArrayList();
			}
			this.m_type.m_ca.Add(new TypeBuilder.CustAttr(con, binaryAttribute));
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x0010EFA5 File Offset: 0x0010DFA5
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (this.m_type.m_ca == null)
			{
				this.m_type.m_ca = new ArrayList();
			}
			this.m_type.m_ca.Add(new TypeBuilder.CustAttr(customBuilder));
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x0010EFDC File Offset: 0x0010DFDC
		public void SetBaseTypeConstraint(Type baseTypeConstraint)
		{
			this.m_type.CheckContext(new Type[]
			{
				baseTypeConstraint
			});
			this.m_type.SetParent(baseTypeConstraint);
		}

		// Token: 0x06004DAE RID: 19886 RVA: 0x0010F00C File Offset: 0x0010E00C
		[ComVisible(true)]
		public void SetInterfaceConstraints(params Type[] interfaceConstraints)
		{
			this.m_type.CheckContext(interfaceConstraints);
			this.m_type.SetInterfaces(interfaceConstraints);
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x0010F026 File Offset: 0x0010E026
		public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
		{
			this.m_type.m_genParamAttributes = genericParameterAttributes;
		}

		// Token: 0x04002849 RID: 10313
		internal TypeBuilder m_type;
	}
}
