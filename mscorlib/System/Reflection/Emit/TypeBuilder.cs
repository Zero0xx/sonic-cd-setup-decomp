using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x0200084B RID: 2123
	[ComDefaultInterface(typeof(_TypeBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class TypeBuilder : Type, _TypeBuilder
	{
		// Token: 0x06004C6E RID: 19566 RVA: 0x0010BAF0 File Offset: 0x0010AAF0
		public static MethodInfo GetMethod(Type type, MethodInfo method)
		{
			if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
			}
			if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedGenericMethodDefinition"), "method");
			}
			if (method.DeclaringType == null || !method.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MethodNeedGenericDeclaringType"), "method");
			}
			if (type.GetGenericTypeDefinition() != method.DeclaringType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidMethodDeclaringType"), "type");
			}
			if (type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (!(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
			}
			return MethodOnTypeBuilderInstantiation.GetMethod(method, type as TypeBuilderInstantiation);
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x0010BBD0 File Offset: 0x0010ABD0
		public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
		{
			if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
			}
			if (!constructor.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ConstructorNeedGenericDeclaringType"), "constructor");
			}
			if (!(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
			}
			if (type is TypeBuilder && type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (type.GetGenericTypeDefinition() != constructor.DeclaringType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidConstructorDeclaringType"), "type");
			}
			return ConstructorOnTypeBuilderInstantiation.GetConstructor(constructor, type as TypeBuilderInstantiation);
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x0010BC8C File Offset: 0x0010AC8C
		public static FieldInfo GetField(Type type, FieldInfo field)
		{
			if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
			}
			if (!field.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_FieldNeedGenericDeclaringType"), "field");
			}
			if (!(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
			}
			if (type is TypeBuilder && type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (type.GetGenericTypeDefinition() != field.DeclaringType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFieldDeclaringType"), "type");
			}
			return FieldOnTypeBuilderInstantiation.GetField(field, type as TypeBuilderInstantiation);
		}

		// Token: 0x06004C71 RID: 19569
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetParentType(int tdTypeDef, int tkParent, Module module);

		// Token: 0x06004C72 RID: 19570 RVA: 0x0010BD45 File Offset: 0x0010AD45
		private static void InternalSetParentType(int tdTypeDef, int tkParent, Module module)
		{
			TypeBuilder._InternalSetParentType(tdTypeDef, tkParent, module.InternalModule);
		}

		// Token: 0x06004C73 RID: 19571
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalAddInterfaceImpl(int tdTypeDef, int tkInterface, Module module);

		// Token: 0x06004C74 RID: 19572 RVA: 0x0010BD54 File Offset: 0x0010AD54
		private static void InternalAddInterfaceImpl(int tdTypeDef, int tkInterface, Module module)
		{
			TypeBuilder._InternalAddInterfaceImpl(tdTypeDef, tkInterface, module.InternalModule);
		}

		// Token: 0x06004C75 RID: 19573
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineMethod(int handle, string name, byte[] signature, int sigLength, MethodAttributes attributes, Module module);

		// Token: 0x06004C76 RID: 19574 RVA: 0x0010BD63 File Offset: 0x0010AD63
		internal static int InternalDefineMethod(int handle, string name, byte[] signature, int sigLength, MethodAttributes attributes, Module module)
		{
			return TypeBuilder._InternalDefineMethod(handle, name, signature, sigLength, attributes, module.InternalModule);
		}

		// Token: 0x06004C77 RID: 19575
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineMethodSpec(int handle, byte[] signature, int sigLength, Module module);

		// Token: 0x06004C78 RID: 19576 RVA: 0x0010BD77 File Offset: 0x0010AD77
		internal static int InternalDefineMethodSpec(int handle, byte[] signature, int sigLength, Module module)
		{
			return TypeBuilder._InternalDefineMethodSpec(handle, signature, sigLength, module.InternalModule);
		}

		// Token: 0x06004C79 RID: 19577
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineField(int handle, string name, byte[] signature, int sigLength, FieldAttributes attributes, Module module);

		// Token: 0x06004C7A RID: 19578 RVA: 0x0010BD87 File Offset: 0x0010AD87
		internal static int InternalDefineField(int handle, string name, byte[] signature, int sigLength, FieldAttributes attributes, Module module)
		{
			return TypeBuilder._InternalDefineField(handle, name, signature, sigLength, attributes, module.InternalModule);
		}

		// Token: 0x06004C7B RID: 19579
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetMethodIL(int methodHandle, bool isInitLocals, byte[] body, byte[] LocalSig, int sigLength, int maxStackSize, int numExceptions, __ExceptionInstance[] exceptions, int[] tokenFixups, int[] rvaFixups, Module module);

		// Token: 0x06004C7C RID: 19580 RVA: 0x0010BD9C File Offset: 0x0010AD9C
		internal static void InternalSetMethodIL(int methodHandle, bool isInitLocals, byte[] body, byte[] LocalSig, int sigLength, int maxStackSize, int numExceptions, __ExceptionInstance[] exceptions, int[] tokenFixups, int[] rvaFixups, Module module)
		{
			TypeBuilder._InternalSetMethodIL(methodHandle, isInitLocals, body, LocalSig, sigLength, maxStackSize, numExceptions, exceptions, tokenFixups, rvaFixups, module.InternalModule);
		}

		// Token: 0x06004C7D RID: 19581
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, Module module, bool toDisk, bool updateCompilerFlags);

		// Token: 0x06004C7E RID: 19582 RVA: 0x0010BDC5 File Offset: 0x0010ADC5
		internal static void InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, Module module, bool toDisk, bool updateCompilerFlags)
		{
			TypeBuilder._InternalCreateCustomAttribute(tkAssociate, tkConstructor, attr, module.InternalModule, toDisk, updateCompilerFlags);
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x0010BDDC File Offset: 0x0010ADDC
		internal static void InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, Module module, bool toDisk)
		{
			byte[] array = null;
			if (attr != null)
			{
				array = new byte[attr.Length];
				Array.Copy(attr, array, attr.Length);
			}
			TypeBuilder.InternalCreateCustomAttribute(tkAssociate, tkConstructor, array, module.InternalModule, toDisk, false);
		}

		// Token: 0x06004C80 RID: 19584
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetPInvokeData(Module module, string DllName, string name, int token, int linkType, int linkFlags);

		// Token: 0x06004C81 RID: 19585 RVA: 0x0010BE12 File Offset: 0x0010AE12
		internal static void InternalSetPInvokeData(Module module, string DllName, string name, int token, int linkType, int linkFlags)
		{
			TypeBuilder._InternalSetPInvokeData(module.InternalModule, DllName, name, token, linkType, linkFlags);
		}

		// Token: 0x06004C82 RID: 19586
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineProperty(Module module, int handle, string name, int attributes, byte[] signature, int sigLength, int notifyChanging, int notifyChanged);

		// Token: 0x06004C83 RID: 19587 RVA: 0x0010BE26 File Offset: 0x0010AE26
		internal static int InternalDefineProperty(Module module, int handle, string name, int attributes, byte[] signature, int sigLength, int notifyChanging, int notifyChanged)
		{
			return TypeBuilder._InternalDefineProperty(module.InternalModule, handle, name, attributes, signature, sigLength, notifyChanging, notifyChanged);
		}

		// Token: 0x06004C84 RID: 19588
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineEvent(Module module, int handle, string name, int attributes, int tkEventType);

		// Token: 0x06004C85 RID: 19589 RVA: 0x0010BE3E File Offset: 0x0010AE3E
		internal static int InternalDefineEvent(Module module, int handle, string name, int attributes, int tkEventType)
		{
			return TypeBuilder._InternalDefineEvent(module.InternalModule, handle, name, attributes, tkEventType);
		}

		// Token: 0x06004C86 RID: 19590
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalDefineMethodSemantics(Module module, int tkAssociation, MethodSemanticsAttributes semantics, int tkMethod);

		// Token: 0x06004C87 RID: 19591 RVA: 0x0010BE50 File Offset: 0x0010AE50
		internal static void InternalDefineMethodSemantics(Module module, int tkAssociation, MethodSemanticsAttributes semantics, int tkMethod)
		{
			TypeBuilder._InternalDefineMethodSemantics(module.InternalModule, tkAssociation, semantics, tkMethod);
		}

		// Token: 0x06004C88 RID: 19592
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalDefineMethodImpl(Module module, int tkType, int tkBody, int tkDecl);

		// Token: 0x06004C89 RID: 19593 RVA: 0x0010BE60 File Offset: 0x0010AE60
		internal static void InternalDefineMethodImpl(Module module, int tkType, int tkBody, int tkDecl)
		{
			TypeBuilder._InternalDefineMethodImpl(module.InternalModule, tkType, tkBody, tkDecl);
		}

		// Token: 0x06004C8A RID: 19594
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetMethodImpl(Module module, int tkMethod, MethodImplAttributes MethodImplAttributes);

		// Token: 0x06004C8B RID: 19595 RVA: 0x0010BE70 File Offset: 0x0010AE70
		internal static void InternalSetMethodImpl(Module module, int tkMethod, MethodImplAttributes MethodImplAttributes)
		{
			TypeBuilder._InternalSetMethodImpl(module.InternalModule, tkMethod, MethodImplAttributes);
		}

		// Token: 0x06004C8C RID: 19596
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalSetParamInfo(Module module, int tkMethod, int iSequence, ParameterAttributes iParamAttributes, string strParamName);

		// Token: 0x06004C8D RID: 19597 RVA: 0x0010BE7F File Offset: 0x0010AE7F
		internal static int InternalSetParamInfo(Module module, int tkMethod, int iSequence, ParameterAttributes iParamAttributes, string strParamName)
		{
			return TypeBuilder._InternalSetParamInfo(module.InternalModule, tkMethod, iSequence, iParamAttributes, strParamName);
		}

		// Token: 0x06004C8E RID: 19598
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalGetTokenFromSig(Module module, byte[] signature, int sigLength);

		// Token: 0x06004C8F RID: 19599 RVA: 0x0010BE91 File Offset: 0x0010AE91
		internal static int InternalGetTokenFromSig(Module module, byte[] signature, int sigLength)
		{
			return TypeBuilder._InternalGetTokenFromSig(module.InternalModule, signature, sigLength);
		}

		// Token: 0x06004C90 RID: 19600
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetFieldOffset(Module module, int fdToken, int iOffset);

		// Token: 0x06004C91 RID: 19601 RVA: 0x0010BEA0 File Offset: 0x0010AEA0
		internal static void InternalSetFieldOffset(Module module, int fdToken, int iOffset)
		{
			TypeBuilder._InternalSetFieldOffset(module.InternalModule, fdToken, iOffset);
		}

		// Token: 0x06004C92 RID: 19602
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetClassLayout(Module module, int tdToken, PackingSize iPackingSize, int iTypeSize);

		// Token: 0x06004C93 RID: 19603 RVA: 0x0010BEAF File Offset: 0x0010AEAF
		internal static void InternalSetClassLayout(Module module, int tdToken, PackingSize iPackingSize, int iTypeSize)
		{
			TypeBuilder._InternalSetClassLayout(module.InternalModule, tdToken, iPackingSize, iTypeSize);
		}

		// Token: 0x06004C94 RID: 19604
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetMarshalInfo(Module module, int tk, byte[] ubMarshal, int ubSize);

		// Token: 0x06004C95 RID: 19605 RVA: 0x0010BEBF File Offset: 0x0010AEBF
		internal static void InternalSetMarshalInfo(Module module, int tk, byte[] ubMarshal, int ubSize)
		{
			TypeBuilder._InternalSetMarshalInfo(module.InternalModule, tk, ubMarshal, ubSize);
		}

		// Token: 0x06004C96 RID: 19606
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetConstantValue(Module module, int tk, ref Variant var);

		// Token: 0x06004C97 RID: 19607 RVA: 0x0010BECF File Offset: 0x0010AECF
		private static void InternalSetConstantValue(Module module, int tk, ref Variant var)
		{
			TypeBuilder._InternalSetConstantValue(module.InternalModule, tk, ref var);
		}

		// Token: 0x06004C98 RID: 19608
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalAddDeclarativeSecurity(Module module, int parent, SecurityAction action, byte[] blob);

		// Token: 0x06004C99 RID: 19609 RVA: 0x0010BEDE File Offset: 0x0010AEDE
		internal static void InternalAddDeclarativeSecurity(Module module, int parent, SecurityAction action, byte[] blob)
		{
			TypeBuilder._InternalAddDeclarativeSecurity(module.InternalModule, parent, action, blob);
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x0010BEF0 File Offset: 0x0010AEF0
		private static bool IsPublicComType(Type type)
		{
			Type declaringType = type.DeclaringType;
			if (declaringType != null)
			{
				if (TypeBuilder.IsPublicComType(declaringType) && (type.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic)
				{
					return true;
				}
			}
			else if ((type.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x0010BF2C File Offset: 0x0010AF2C
		internal static bool IsTypeEqual(Type t1, Type t2)
		{
			if (t1 == t2)
			{
				return true;
			}
			TypeBuilder typeBuilder = null;
			TypeBuilder typeBuilder2 = null;
			Type type;
			if (t1 is TypeBuilder)
			{
				typeBuilder = (TypeBuilder)t1;
				type = typeBuilder.m_runtimeType;
			}
			else
			{
				type = t1;
			}
			Type type2;
			if (t2 is TypeBuilder)
			{
				typeBuilder2 = (TypeBuilder)t2;
				type2 = typeBuilder2.m_runtimeType;
			}
			else
			{
				type2 = t2;
			}
			return (typeBuilder != null && typeBuilder2 != null && typeBuilder == typeBuilder2) || (type != null && type2 != null && type == type2);
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x0010BF94 File Offset: 0x0010AF94
		internal static void SetConstantValue(Module module, int tk, Type destType, object value)
		{
			if (value == null)
			{
				if (destType.IsValueType)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_ConstantNull"));
				}
			}
			else
			{
				Type type = value.GetType();
				if (!destType.IsEnum)
				{
					if (destType != type)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ConstantDoesntMatch"));
					}
					switch (Type.GetTypeCode(type))
					{
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
					case TypeCode.String:
						break;
					default:
						if (type != typeof(DateTime))
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_ConstantNotSupported"));
						}
						break;
					}
				}
				else if (destType.UnderlyingSystemType != type)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_ConstantDoesntMatch"));
				}
			}
			Variant variant = new Variant(value);
			TypeBuilder.InternalSetConstantValue(module.InternalModule, tk, ref variant);
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x0010C088 File Offset: 0x0010B088
		private TypeBuilder(TypeBuilder genTypeDef, GenericTypeParameterBuilder[] inst)
		{
			this.m_genTypeDef = genTypeDef;
			this.m_DeclaringType = genTypeDef.m_DeclaringType;
			this.m_typeParent = genTypeDef.m_typeParent;
			this.m_runtimeType = genTypeDef.m_runtimeType;
			this.m_tdType = genTypeDef.m_tdType;
			this.m_strName = genTypeDef.m_strName;
			this.m_bIsGenParam = false;
			this.m_bIsGenTypeDef = false;
			this.m_module = genTypeDef.m_module;
			this.m_inst = inst;
			this.m_hasBeenCreated = true;
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x0010C106 File Offset: 0x0010B106
		internal TypeBuilder(string szName, int genParamPos, MethodBuilder declMeth)
		{
			this.m_declMeth = declMeth;
			this.m_DeclaringType = (TypeBuilder)this.m_declMeth.DeclaringType;
			this.m_module = (ModuleBuilder)declMeth.Module;
			this.InitAsGenericParam(szName, genParamPos);
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x0010C144 File Offset: 0x0010B144
		private TypeBuilder(string szName, int genParamPos, TypeBuilder declType)
		{
			this.m_DeclaringType = declType;
			this.m_module = (ModuleBuilder)declType.Module;
			this.InitAsGenericParam(szName, genParamPos);
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x0010C16C File Offset: 0x0010B16C
		private void InitAsGenericParam(string szName, int genParamPos)
		{
			this.m_strName = szName;
			this.m_genParamPos = genParamPos;
			this.m_bIsGenParam = true;
			this.m_bIsGenTypeDef = false;
			this.m_typeInterfaces = new Type[0];
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x0010C198 File Offset: 0x0010B198
		internal TypeBuilder(string name, TypeAttributes attr, Type parent, Module module, PackingSize iPackingSize, int iTypeSize, TypeBuilder enclosingType)
		{
			this.Init(name, attr, parent, null, module, iPackingSize, iTypeSize, enclosingType);
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x0010C1C0 File Offset: 0x0010B1C0
		internal TypeBuilder(string name, TypeAttributes attr, Type parent, Type[] interfaces, Module module, PackingSize iPackingSize, TypeBuilder enclosingType)
		{
			this.Init(name, attr, parent, interfaces, module, iPackingSize, 0, enclosingType);
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x0010C1E5 File Offset: 0x0010B1E5
		internal TypeBuilder(ModuleBuilder module)
		{
			this.m_tdType = new TypeToken(33554432);
			this.m_isHiddenGlobalType = true;
			this.m_module = module;
			this.m_listMethods = new ArrayList();
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x0010C218 File Offset: 0x0010B218
		private void Init(string fullname, TypeAttributes attr, Type parent, Type[] interfaces, Module module, PackingSize iPackingSize, int iTypeSize, TypeBuilder enclosingType)
		{
			this.m_bIsGenTypeDef = false;
			int[] array = null;
			this.m_bIsGenParam = false;
			this.m_hasBeenCreated = false;
			this.m_runtimeType = null;
			this.m_isHiddenGlobalType = false;
			this.m_isHiddenType = false;
			this.m_module = (ModuleBuilder)module;
			this.m_DeclaringType = enclosingType;
			Assembly assembly = this.m_module.Assembly;
			this.m_underlyingSystemType = null;
			if (fullname == null)
			{
				throw new ArgumentNullException("fullname");
			}
			if (fullname.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "fullname");
			}
			if (fullname[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "fullname");
			}
			if (fullname.Length > 1023)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TypeNameTooLong"), "fullname");
			}
			assembly.m_assemblyData.CheckTypeNameConflict(fullname, enclosingType);
			if (enclosingType != null && ((attr & TypeAttributes.VisibilityMask) == TypeAttributes.Public || (attr & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadNestedTypeFlags"), "attr");
			}
			if (interfaces != null)
			{
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (interfaces[i] == null)
					{
						throw new ArgumentNullException("interfaces");
					}
				}
				array = new int[interfaces.Length];
				for (int i = 0; i < interfaces.Length; i++)
				{
					array[i] = this.m_module.GetTypeTokenInternal(interfaces[i]).Token;
				}
			}
			int num = fullname.LastIndexOf('.');
			if (num == -1 || num == 0)
			{
				this.m_strNameSpace = string.Empty;
				this.m_strName = fullname;
			}
			else
			{
				this.m_strNameSpace = fullname.Substring(0, num);
				this.m_strName = fullname.Substring(num + 1);
			}
			this.VerifyTypeAttributes(attr);
			this.m_iAttr = attr;
			this.SetParent(parent);
			this.m_listMethods = new ArrayList();
			this.SetInterfaces(interfaces);
			this.m_constructorCount = 0;
			int tkParent = 0;
			if (this.m_typeParent != null)
			{
				tkParent = this.m_module.GetTypeTokenInternal(this.m_typeParent).Token;
			}
			int tkEnclosingType = 0;
			if (enclosingType != null)
			{
				tkEnclosingType = enclosingType.m_tdType.Token;
			}
			this.m_tdType = new TypeToken(this.InternalDefineClass(fullname, tkParent, array, this.m_iAttr, this.m_module, Guid.Empty, tkEnclosingType, 0));
			this.m_iPackingSize = iPackingSize;
			this.m_iTypeSize = iTypeSize;
			if (this.m_iPackingSize != PackingSize.Unspecified || this.m_iTypeSize != 0)
			{
				TypeBuilder.InternalSetClassLayout(this.Module, this.m_tdType.Token, this.m_iPackingSize, this.m_iTypeSize);
			}
			if (TypeBuilder.IsPublicComType(this) && assembly is AssemblyBuilder)
			{
				AssemblyBuilder assemblyBuilder = (AssemblyBuilder)assembly;
				if (assemblyBuilder.IsPersistable() && !this.m_module.IsTransient())
				{
					assemblyBuilder.m_assemblyData.AddPublicComType(this);
				}
			}
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x0010C4C0 File Offset: 0x0010B4C0
		private MethodBuilder DefinePInvokeMethodHelper(string name, string dllName, string importName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.CheckContext(new Type[]
			{
				returnType
			});
			this.CheckContext(new Type[][]
			{
				returnTypeRequiredCustomModifiers,
				returnTypeOptionalCustomModifiers,
				parameterTypes
			});
			this.CheckContext(parameterTypeRequiredCustomModifiers);
			this.CheckContext(parameterTypeOptionalCustomModifiers);
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefinePInvokeMethodHelperNoLock(name, dllName, importName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
				}
			}
			return this.DefinePInvokeMethodHelperNoLock(name, dllName, importName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x0010C594 File Offset: 0x0010B594
		private MethodBuilder DefinePInvokeMethodHelperNoLock(string name, string dllName, string importName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.ThrowIfCreated();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (dllName == null)
			{
				throw new ArgumentNullException("dllName");
			}
			if (dllName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "dllName");
			}
			if (importName == null)
			{
				throw new ArgumentNullException("importName");
			}
			if (importName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "importName");
			}
			if ((this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadPInvokeOnInterface"));
			}
			if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadPInvokeMethod"));
			}
			attributes |= MethodAttributes.PinvokeImpl;
			MethodBuilder methodBuilder = new MethodBuilder(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, this.m_module, this, false);
			int num;
			methodBuilder.GetMethodSignature().InternalGetSignature(out num);
			if (this.m_listMethods.Contains(methodBuilder))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MethodRedefined"));
			}
			this.m_listMethods.Add(methodBuilder);
			MethodToken token = methodBuilder.GetToken();
			int num2 = 0;
			switch (nativeCallConv)
			{
			case CallingConvention.Winapi:
				num2 = 256;
				break;
			case CallingConvention.Cdecl:
				num2 = 512;
				break;
			case CallingConvention.StdCall:
				num2 = 768;
				break;
			case CallingConvention.ThisCall:
				num2 = 1024;
				break;
			case CallingConvention.FastCall:
				num2 = 1280;
				break;
			}
			switch (nativeCharSet)
			{
			case CharSet.None:
				num2 = num2;
				break;
			case CharSet.Ansi:
				num2 |= 2;
				break;
			case CharSet.Unicode:
				num2 |= 4;
				break;
			case CharSet.Auto:
				num2 |= 6;
				break;
			}
			TypeBuilder.InternalSetPInvokeData(this.m_module, dllName, importName, token.Token, 0, num2);
			methodBuilder.SetToken(token);
			return methodBuilder;
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x0010C764 File Offset: 0x0010B764
		private FieldBuilder DefineDataHelper(string name, byte[] data, int size, FieldAttributes attributes)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (size <= 0 || size >= 4128768)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadSizeForData"));
			}
			this.ThrowIfCreated();
			string text = "$ArrayType$" + size;
			Type type = this.m_module.FindTypeBuilderWithName(text, false);
			TypeBuilder typeBuilder = type as TypeBuilder;
			if (typeBuilder == null)
			{
				TypeAttributes attr = TypeAttributes.Public | TypeAttributes.ExplicitLayout | TypeAttributes.Sealed;
				typeBuilder = this.m_module.DefineType(text, attr, typeof(ValueType), PackingSize.Size1, size);
				typeBuilder.m_isHiddenType = true;
				typeBuilder.CreateType();
			}
			FieldBuilder fieldBuilder = this.DefineField(name, typeBuilder, attributes | FieldAttributes.Static);
			fieldBuilder.SetData(data, size);
			return fieldBuilder;
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x0010C830 File Offset: 0x0010B830
		private void VerifyTypeAttributes(TypeAttributes attr)
		{
			if (this.DeclaringType == null)
			{
				if ((attr & TypeAttributes.VisibilityMask) != TypeAttributes.NotPublic && (attr & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrNestedVisibilityOnNonNestedType"));
				}
			}
			else if ((attr & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic || (attr & TypeAttributes.VisibilityMask) == TypeAttributes.Public)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrNonNestedVisibilityNestedType"));
			}
			if ((attr & TypeAttributes.LayoutMask) != TypeAttributes.NotPublic && (attr & TypeAttributes.LayoutMask) != TypeAttributes.SequentialLayout && (attr & TypeAttributes.LayoutMask) != TypeAttributes.ExplicitLayout)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrInvalidLayout"));
			}
			if ((attr & TypeAttributes.ReservedMask) != TypeAttributes.NotPublic)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrReservedBitsSet"));
			}
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x0010C8B9 File Offset: 0x0010B8B9
		public bool IsCreated()
		{
			return this.m_hasBeenCreated;
		}

		// Token: 0x06004CAA RID: 19626
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalDefineClass(string fullname, int tkParent, int[] interfaceTokens, TypeAttributes attr, Module module, Guid guid, int tkEnclosingType, int tkTypeDef);

		// Token: 0x06004CAB RID: 19627 RVA: 0x0010C8C4 File Offset: 0x0010B8C4
		private int InternalDefineClass(string fullname, int tkParent, int[] interfaceTokens, TypeAttributes attr, Module module, Guid guid, int tkEnclosingType, int tkTypeDef)
		{
			return this._InternalDefineClass(fullname, tkParent, interfaceTokens, attr, module.InternalModule, guid, tkEnclosingType, tkTypeDef);
		}

		// Token: 0x06004CAC RID: 19628
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalDefineGenParam(string name, int tkParent, int position, int attributes, int[] interfaceTokens, Module module, int tkTypeDef);

		// Token: 0x06004CAD RID: 19629 RVA: 0x0010C8E9 File Offset: 0x0010B8E9
		private int InternalDefineGenParam(string name, int tkParent, int position, int attributes, int[] interfaceTokens, Module module, int tkTypeDef)
		{
			return this._InternalDefineGenParam(name, tkParent, position, attributes, interfaceTokens, module.InternalModule, tkTypeDef);
		}

		// Token: 0x06004CAE RID: 19630
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type _TermCreateClass(int handle, Module module);

		// Token: 0x06004CAF RID: 19631 RVA: 0x0010C901 File Offset: 0x0010B901
		private Type TermCreateClass(int handle, Module module)
		{
			return this._TermCreateClass(handle, module.InternalModule);
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x0010C910 File Offset: 0x0010B910
		internal void ThrowIfCreated()
		{
			if (this.IsCreated())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
			}
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x0010C92A File Offset: 0x0010B92A
		public override string ToString()
		{
			return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.ToString);
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x0010C933 File Offset: 0x0010B933
		public override Type DeclaringType
		{
			get
			{
				return this.m_DeclaringType;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x0010C93B File Offset: 0x0010B93B
		public override Type ReflectedType
		{
			get
			{
				return this.m_DeclaringType;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x0010C943 File Offset: 0x0010B943
		public override string Name
		{
			get
			{
				return this.m_strName;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06004CB5 RID: 19637 RVA: 0x0010C94B File Offset: 0x0010B94B
		public override Module Module
		{
			get
			{
				return this.m_module;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x0010C953 File Offset: 0x0010B953
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_tdType.Token;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x0010C960 File Offset: 0x0010B960
		public override Guid GUID
		{
			get
			{
				if (this.m_runtimeType == null)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
				}
				return this.m_runtimeType.GUID;
			}
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x0010C988 File Offset: 0x0010B988
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x0010C9C5 File Offset: 0x0010B9C5
		public override Assembly Assembly
		{
			get
			{
				return this.m_module.Assembly;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x0010C9D2 File Offset: 0x0010B9D2
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x0010C9E3 File Offset: 0x0010B9E3
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

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x0010CA00 File Offset: 0x0010BA00
		public override string Namespace
		{
			get
			{
				return this.m_strNameSpace;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06004CBD RID: 19645 RVA: 0x0010CA08 File Offset: 0x0010BA08
		public override string AssemblyQualifiedName
		{
			get
			{
				return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.AssemblyQualifiedName);
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06004CBE RID: 19646 RVA: 0x0010CA11 File Offset: 0x0010BA11
		public override Type BaseType
		{
			get
			{
				return this.m_typeParent;
			}
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0010CA19 File Offset: 0x0010BA19
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x0010CA45 File Offset: 0x0010BA45
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetConstructors(bindingAttr);
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x0010CA6B File Offset: 0x0010BA6B
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			if (types == null)
			{
				return this.m_runtimeType.GetMethod(name, bindingAttr);
			}
			return this.m_runtimeType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x0010CAAB File Offset: 0x0010BAAB
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetMethods(bindingAttr);
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x0010CAD1 File Offset: 0x0010BAD1
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetField(name, bindingAttr);
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x0010CAF8 File Offset: 0x0010BAF8
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetFields(bindingAttr);
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x0010CB1E File Offset: 0x0010BB1E
		public override Type GetInterface(string name, bool ignoreCase)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetInterface(name, ignoreCase);
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x0010CB48 File Offset: 0x0010BB48
		public override Type[] GetInterfaces()
		{
			if (this.m_runtimeType != null)
			{
				return this.m_runtimeType.GetInterfaces();
			}
			if (this.m_typeInterfaces == null)
			{
				return new Type[0];
			}
			Type[] array = new Type[this.m_typeInterfaces.Length];
			Array.Copy(this.m_typeInterfaces, array, this.m_typeInterfaces.Length);
			return array;
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x0010CB9B File Offset: 0x0010BB9B
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetEvent(name, bindingAttr);
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x0010CBC2 File Offset: 0x0010BBC2
		public override EventInfo[] GetEvents()
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetEvents();
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x0010CBE7 File Offset: 0x0010BBE7
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x0010CBF8 File Offset: 0x0010BBF8
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetProperties(bindingAttr);
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x0010CC1E File Offset: 0x0010BC1E
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x0010CC44 File Offset: 0x0010BC44
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x0010CC6B File Offset: 0x0010BC6B
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetMember(name, type, bindingAttr);
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x0010CC93 File Offset: 0x0010BC93
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetInterfaceMap(interfaceType);
		}

		// Token: 0x06004CCF RID: 19663 RVA: 0x0010CCB9 File Offset: 0x0010BCB9
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetEvents(bindingAttr);
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x0010CCDF File Offset: 0x0010BCDF
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetMembers(bindingAttr);
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x0010CD08 File Offset: 0x0010BD08
		public override bool IsAssignableFrom(Type c)
		{
			if (TypeBuilder.IsTypeEqual(c, this))
			{
				return true;
			}
			RuntimeType runtimeType = c as RuntimeType;
			TypeBuilder typeBuilder = c as TypeBuilder;
			if (typeBuilder != null && typeBuilder.m_runtimeType != null)
			{
				runtimeType = typeBuilder.m_runtimeType;
			}
			if (runtimeType != null)
			{
				return this.m_runtimeType != null && this.m_runtimeType.IsAssignableFrom(runtimeType);
			}
			if (typeBuilder == null)
			{
				return false;
			}
			if (typeBuilder.IsSubclassOf(this))
			{
				return true;
			}
			if (!base.IsInterface)
			{
				return false;
			}
			Type[] interfaces = typeBuilder.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (TypeBuilder.IsTypeEqual(interfaces[i], this))
				{
					return true;
				}
				if (interfaces[i].IsSubclassOf(this))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x0010CDA4 File Offset: 0x0010BDA4
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.m_iAttr;
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x0010CDAC File Offset: 0x0010BDAC
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0010CDAF File Offset: 0x0010BDAF
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x0010CDB2 File Offset: 0x0010BDB2
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x0010CDB5 File Offset: 0x0010BDB5
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x0010CDB8 File Offset: 0x0010BDB8
		protected override bool IsCOMObjectImpl()
		{
			return (this.GetAttributeFlagsImpl() & TypeAttributes.Import) != TypeAttributes.NotPublic;
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x0010CDCB File Offset: 0x0010BDCB
		public override Type GetElementType()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x0010CDDC File Offset: 0x0010BDDC
		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x0010CDE0 File Offset: 0x0010BDE0
		[ComVisible(true)]
		public override bool IsSubclassOf(Type c)
		{
			if (TypeBuilder.IsTypeEqual(this, c))
			{
				return false;
			}
			for (Type baseType = this.BaseType; baseType != null; baseType = baseType.BaseType)
			{
				if (TypeBuilder.IsTypeEqual(baseType, c))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06004CDB RID: 19675 RVA: 0x0010CE19 File Offset: 0x0010BE19
		public override Type UnderlyingSystemType
		{
			get
			{
				if (this.m_runtimeType != null)
				{
					return this.m_runtimeType.UnderlyingSystemType;
				}
				if (!base.IsEnum)
				{
					return this;
				}
				if (this.m_underlyingSystemType == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoUnderlyingTypeOnEnum"));
				}
				return this.m_underlyingSystemType;
			}
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x0010CE57 File Offset: 0x0010BE57
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType("*".ToCharArray(), this, 0);
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x0010CE6A File Offset: 0x0010BE6A
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType("&".ToCharArray(), this, 0);
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x0010CE7D File Offset: 0x0010BE7D
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);
		}

		// Token: 0x06004CDF RID: 19679 RVA: 0x0010CE90 File Offset: 0x0010BE90
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
			return SymbolType.FormCompoundType(text2.ToCharArray(), this, 0);
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x0010CEFA File Offset: 0x0010BEFA
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return CustomAttribute.GetCustomAttributes(this.m_runtimeType, typeof(object) as RuntimeType, inherit);
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x0010CF30 File Offset: 0x0010BF30
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this.m_runtimeType, runtimeType, inherit);
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x0010CF94 File Offset: 0x0010BF94
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "caType");
			}
			return CustomAttribute.IsDefined(this.m_runtimeType, runtimeType, inherit);
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x0010CFEA File Offset: 0x0010BFEA
		internal void ThrowIfGeneric()
		{
			if (this.IsGenericType && !this.IsGenericTypeDefinition)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06004CE4 RID: 19684 RVA: 0x0010D002 File Offset: 0x0010C002
		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				return this.m_genParamAttributes;
			}
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x0010D00A File Offset: 0x0010C00A
		internal void SetInterfaces(params Type[] interfaces)
		{
			this.ThrowIfCreated();
			if (interfaces == null)
			{
				this.m_typeInterfaces = new Type[0];
				return;
			}
			this.m_typeInterfaces = new Type[interfaces.Length];
			Array.Copy(interfaces, this.m_typeInterfaces, interfaces.Length);
		}

		// Token: 0x06004CE6 RID: 19686 RVA: 0x0010D040 File Offset: 0x0010C040
		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			if (this.m_inst != null)
			{
				throw new InvalidOperationException();
			}
			if (names == null)
			{
				throw new ArgumentNullException("names");
			}
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == null)
				{
					throw new ArgumentNullException("names");
				}
			}
			if (names.Length == 0)
			{
				throw new ArgumentException();
			}
			this.m_bIsGenTypeDef = true;
			this.m_inst = new GenericTypeParameterBuilder[names.Length];
			for (int j = 0; j < names.Length; j++)
			{
				this.m_inst[j] = new GenericTypeParameterBuilder(new TypeBuilder(names[j], j, this));
			}
			return this.m_inst;
		}

		// Token: 0x06004CE7 RID: 19687 RVA: 0x0010D0D2 File Offset: 0x0010C0D2
		public override Type MakeGenericType(params Type[] typeArguments)
		{
			this.CheckContext(typeArguments);
			if (!this.IsGenericTypeDefinition)
			{
				throw new InvalidOperationException();
			}
			return new TypeBuilderInstantiation(this, typeArguments);
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x0010D0F0 File Offset: 0x0010C0F0
		public override Type[] GetGenericArguments()
		{
			return this.m_inst;
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06004CE9 RID: 19689 RVA: 0x0010D0F8 File Offset: 0x0010C0F8
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return this.m_bIsGenTypeDef;
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06004CEA RID: 19690 RVA: 0x0010D100 File Offset: 0x0010C100
		public override bool IsGenericType
		{
			get
			{
				return this.m_inst != null;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06004CEB RID: 19691 RVA: 0x0010D10E File Offset: 0x0010C10E
		public override bool IsGenericParameter
		{
			get
			{
				return this.m_bIsGenParam;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06004CEC RID: 19692 RVA: 0x0010D116 File Offset: 0x0010C116
		public override int GenericParameterPosition
		{
			get
			{
				return this.m_genParamPos;
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06004CED RID: 19693 RVA: 0x0010D11E File Offset: 0x0010C11E
		public override MethodBase DeclaringMethod
		{
			get
			{
				return this.m_declMeth;
			}
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x0010D126 File Offset: 0x0010C126
		public override Type GetGenericTypeDefinition()
		{
			if (this.IsGenericTypeDefinition)
			{
				return this;
			}
			if (this.m_genTypeDef == null)
			{
				throw new InvalidOperationException();
			}
			return this.m_genTypeDef;
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x0010D148 File Offset: 0x0010C148
		public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					this.DefineMethodOverrideNoLock(methodInfoBody, methodInfoDeclaration);
					return;
				}
			}
			this.DefineMethodOverrideNoLock(methodInfoBody, methodInfoDeclaration);
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x0010D1AC File Offset: 0x0010C1AC
		private void DefineMethodOverrideNoLock(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
		{
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			if (methodInfoBody == null)
			{
				throw new ArgumentNullException("methodInfoBody");
			}
			if (methodInfoDeclaration == null)
			{
				throw new ArgumentNullException("methodInfoDeclaration");
			}
			if (methodInfoBody.DeclaringType != this)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentException_BadMethodImplBody"));
			}
			MethodToken methodTokenInternal = this.m_module.GetMethodTokenInternal(methodInfoBody);
			MethodToken methodTokenInternal2 = this.m_module.GetMethodTokenInternal(methodInfoDeclaration);
			TypeBuilder.InternalDefineMethodImpl(this.m_module, this.m_tdType.Token, methodTokenInternal.Token, methodTokenInternal2.Token);
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x0010D238 File Offset: 0x0010C238
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x0010D246 File Offset: 0x0010C246
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes)
		{
			return this.DefineMethod(name, attributes, CallingConventions.Standard, null, null);
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x0010D253 File Offset: 0x0010C253
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
		{
			return this.DefineMethod(name, attributes, callingConvention, null, null);
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x0010D260 File Offset: 0x0010C260
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x0010D280 File Offset: 0x0010C280
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineMethodNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
				}
			}
			return this.DefineMethodNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x0010D304 File Offset: 0x0010C304
		private MethodBuilder DefineMethodNoLock(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.CheckContext(new Type[]
			{
				returnType
			});
			this.CheckContext(new Type[][]
			{
				returnTypeRequiredCustomModifiers,
				returnTypeOptionalCustomModifiers,
				parameterTypes
			});
			this.CheckContext(parameterTypeRequiredCustomModifiers);
			this.CheckContext(parameterTypeOptionalCustomModifiers);
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (parameterTypes != null)
			{
				if (parameterTypeOptionalCustomModifiers != null && parameterTypeOptionalCustomModifiers.Length != parameterTypes.Length)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[]
					{
						"parameterTypeOptionalCustomModifiers",
						"parameterTypes"
					}));
				}
				if (parameterTypeRequiredCustomModifiers != null && parameterTypeRequiredCustomModifiers.Length != parameterTypes.Length)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[]
					{
						"parameterTypeRequiredCustomModifiers",
						"parameterTypes"
					}));
				}
			}
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			if (!this.m_isHiddenGlobalType && (this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask && (attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope && (attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadAttributeOnInterfaceMethod"));
			}
			MethodBuilder methodBuilder = new MethodBuilder(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, this.m_module, this, false);
			if (!this.m_isHiddenGlobalType && (methodBuilder.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope && methodBuilder.Name.Equals(ConstructorInfo.ConstructorName))
			{
				this.m_constructorCount++;
			}
			this.m_listMethods.Add(methodBuilder);
			return methodBuilder;
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x0010D494 File Offset: 0x0010C494
		[ComVisible(true)]
		public ConstructorBuilder DefineTypeInitializer()
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineTypeInitializerNoLock();
				}
			}
			return this.DefineTypeInitializerNoLock();
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x0010D4F8 File Offset: 0x0010C4F8
		private ConstructorBuilder DefineTypeInitializerNoLock()
		{
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			MethodAttributes attributes = MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName;
			return new ConstructorBuilder(ConstructorInfo.TypeConstructorName, attributes, CallingConventions.Standard, null, this.m_module, this);
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x0010D530 File Offset: 0x0010C530
		[ComVisible(true)]
		public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineDefaultConstructorNoLock(attributes);
				}
			}
			return this.DefineDefaultConstructorNoLock(attributes);
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x0010D598 File Offset: 0x0010C598
		private ConstructorBuilder DefineDefaultConstructorNoLock(MethodAttributes attributes)
		{
			this.ThrowIfGeneric();
			ConstructorInfo constructorInfo = null;
			if (this.m_typeParent is TypeBuilderInstantiation)
			{
				Type type = this.m_typeParent.GetGenericTypeDefinition();
				if (type is TypeBuilder)
				{
					type = ((TypeBuilder)type).m_runtimeType;
				}
				if (type == null)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
				}
				Type type2 = type.MakeGenericType(this.m_typeParent.GetGenericArguments());
				if (type2 is TypeBuilderInstantiation)
				{
					constructorInfo = TypeBuilder.GetConstructor(type2, type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
				}
				else
				{
					constructorInfo = type2.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
				}
			}
			if (constructorInfo == null)
			{
				constructorInfo = this.m_typeParent.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			}
			if (constructorInfo == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoParentDefaultConstructor"));
			}
			ConstructorBuilder constructorBuilder = this.DefineConstructor(attributes, CallingConventions.Standard, null);
			this.m_constructorCount++;
			ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, constructorInfo);
			ilgenerator.Emit(OpCodes.Ret);
			constructorBuilder.m_ReturnILGen = false;
			return constructorBuilder;
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x0010D6A7 File Offset: 0x0010C6A7
		[ComVisible(true)]
		public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes)
		{
			return this.DefineConstructor(attributes, callingConvention, parameterTypes, null, null);
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x0010D6B4 File Offset: 0x0010C6B4
		[ComVisible(true)]
		public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineConstructorNoLock(attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
				}
			}
			return this.DefineConstructorNoLock(attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x0010D728 File Offset: 0x0010C728
		private ConstructorBuilder DefineConstructorNoLock(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			this.CheckContext(parameterTypes);
			this.CheckContext(requiredCustomModifiers);
			this.CheckContext(optionalCustomModifiers);
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			string name;
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				name = ConstructorInfo.ConstructorName;
			}
			else
			{
				name = ConstructorInfo.TypeConstructorName;
			}
			attributes |= MethodAttributes.SpecialName;
			ConstructorBuilder result = new ConstructorBuilder(name, attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers, this.m_module, this);
			this.m_constructorCount++;
			return result;
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x0010D79C File Offset: 0x0010C79C
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.ThrowIfGeneric();
			return this.DefinePInvokeMethodHelper(name, dllName, name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x0010D7CC File Offset: 0x0010C7CC
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethodHelper(name, dllName, entryName, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0010D7F4 File Offset: 0x0010C7F4
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.ThrowIfGeneric();
			return this.DefinePInvokeMethodHelper(name, dllName, entryName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x0010D828 File Offset: 0x0010C828
		public TypeBuilder DefineNestedType(string name)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name);
				}
			}
			return this.DefineNestedTypeNoLock(name);
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x0010D890 File Offset: 0x0010C890
		private TypeBuilder DefineNestedTypeNoLock(string name)
		{
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, TypeAttributes.NestedPrivate, null, null, this.m_module, PackingSize.Unspecified, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x0010D8C8 File Offset: 0x0010C8C8
		[ComVisible(true)]
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr, parent, interfaces);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr, parent, interfaces);
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x0010D938 File Offset: 0x0010C938
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			this.CheckContext(new Type[]
			{
				parent
			});
			this.CheckContext(interfaces);
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, interfaces, this.m_module, PackingSize.Unspecified, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x0010D98C File Offset: 0x0010C98C
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr, parent);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr, parent);
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x0010D9F8 File Offset: 0x0010C9F8
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent)
		{
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, null, this.m_module, PackingSize.Unspecified, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x0010DA30 File Offset: 0x0010CA30
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr);
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x0010DA98 File Offset: 0x0010CA98
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr)
		{
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, null, null, this.m_module, PackingSize.Unspecified, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x0010DAD0 File Offset: 0x0010CAD0
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr, parent, typeSize);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr, parent, typeSize);
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x0010DB40 File Offset: 0x0010CB40
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, int typeSize)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, this.m_module, PackingSize.Unspecified, typeSize, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x0010DB74 File Offset: 0x0010CB74
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr, parent, packSize);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr, parent, packSize);
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x0010DBE4 File Offset: 0x0010CBE4
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, PackingSize packSize)
		{
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, null, this.m_module, packSize, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x0010DC1D File Offset: 0x0010CC1D
		public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes)
		{
			return this.DefineField(fieldName, type, null, null, attributes);
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x0010DC2C File Offset: 0x0010CC2C
		public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineFieldNoLock(fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
				}
			}
			return this.DefineFieldNoLock(fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x0010DCA0 File Offset: 0x0010CCA0
		private FieldBuilder DefineFieldNoLock(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			this.CheckContext(new Type[]
			{
				type
			});
			this.CheckContext(requiredCustomModifiers);
			if (this.m_underlyingSystemType == null && base.IsEnum && (attributes & FieldAttributes.Static) == FieldAttributes.PrivateScope)
			{
				this.m_underlyingSystemType = type;
			}
			return new FieldBuilder(this, fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
		}

		// Token: 0x06004D10 RID: 19728 RVA: 0x0010DD00 File Offset: 0x0010CD00
		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineInitializedDataNoLock(name, data, attributes);
				}
			}
			return this.DefineInitializedDataNoLock(name, data, attributes);
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x0010DD6C File Offset: 0x0010CD6C
		private FieldBuilder DefineInitializedDataNoLock(string name, byte[] data, FieldAttributes attributes)
		{
			this.ThrowIfGeneric();
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			return this.DefineDataHelper(name, data, data.Length, attributes);
		}

		// Token: 0x06004D12 RID: 19730 RVA: 0x0010DD90 File Offset: 0x0010CD90
		public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineUninitializedDataNoLock(name, size, attributes);
				}
			}
			return this.DefineUninitializedDataNoLock(name, size, attributes);
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x0010DDFC File Offset: 0x0010CDFC
		private FieldBuilder DefineUninitializedDataNoLock(string name, int size, FieldAttributes attributes)
		{
			this.ThrowIfGeneric();
			return this.DefineDataHelper(name, null, size, attributes);
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x0010DE10 File Offset: 0x0010CE10
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineProperty(name, attributes, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x0010DE2C File Offset: 0x0010CE2C
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			return this.DefineProperty(name, attributes, (CallingConventions)0, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x0010DE50 File Offset: 0x0010CE50
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefinePropertyNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
				}
			}
			return this.DefinePropertyNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x0010DED4 File Offset: 0x0010CED4
		private PropertyBuilder DefinePropertyNoLock(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.ThrowIfGeneric();
			this.CheckContext(new Type[]
			{
				returnType
			});
			this.CheckContext(new Type[][]
			{
				returnTypeRequiredCustomModifiers,
				returnTypeOptionalCustomModifiers,
				parameterTypes
			});
			this.CheckContext(parameterTypeRequiredCustomModifiers);
			this.CheckContext(parameterTypeOptionalCustomModifiers);
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			this.ThrowIfCreated();
			SignatureHelper propertySigHelper = SignatureHelper.GetPropertySigHelper(this.m_module, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
			int sigLength;
			byte[] signature = propertySigHelper.InternalGetSignature(out sigLength);
			PropertyToken prToken = new PropertyToken(TypeBuilder.InternalDefineProperty(this.m_module, this.m_tdType.Token, name, (int)attributes, signature, sigLength, 0, 0));
			return new PropertyBuilder(this.m_module, name, propertySigHelper, attributes, returnType, prToken, this);
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x0010DFB8 File Offset: 0x0010CFB8
		public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineEventNoLock(name, attributes, eventtype);
				}
			}
			return this.DefineEventNoLock(name, attributes, eventtype);
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x0010E024 File Offset: 0x0010D024
		private EventBuilder DefineEventNoLock(string name, EventAttributes attributes, Type eventtype)
		{
			this.CheckContext(new Type[]
			{
				eventtype
			});
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "name");
			}
			int token = this.m_module.GetTypeTokenInternal(eventtype).Token;
			EventToken evToken = new EventToken(TypeBuilder.InternalDefineEvent(this.m_module, this.m_tdType.Token, name, (int)attributes, token));
			return new EventBuilder(this.m_module, name, attributes, token, this, evToken);
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x0010E0E0 File Offset: 0x0010D0E0
		public Type CreateType()
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.CreateTypeNoLock();
				}
			}
			return this.CreateTypeNoLock();
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x0010E144 File Offset: 0x0010D144
		internal void CheckContext(params Type[][] typess)
		{
			((AssemblyBuilder)this.Module.Assembly).CheckContext(typess);
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x0010E15C File Offset: 0x0010D15C
		internal void CheckContext(params Type[] types)
		{
			((AssemblyBuilder)this.Module.Assembly).CheckContext(types);
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x0010E174 File Offset: 0x0010D174
		private Type CreateTypeNoLock()
		{
			if (this.IsCreated())
			{
				return this.m_runtimeType;
			}
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			if (this.m_typeInterfaces == null)
			{
				this.m_typeInterfaces = new Type[0];
			}
			int[] array = new int[this.m_typeInterfaces.Length];
			for (int i = 0; i < this.m_typeInterfaces.Length; i++)
			{
				array[i] = this.m_module.GetTypeTokenInternal(this.m_typeInterfaces[i]).Token;
			}
			int num = 0;
			if (this.m_typeParent != null)
			{
				num = this.m_module.GetTypeTokenInternal(this.m_typeParent).Token;
			}
			if (this.IsGenericParameter)
			{
				int[] array2 = new int[this.m_typeInterfaces.Length];
				if (this.m_typeParent != null)
				{
					array2 = new int[this.m_typeInterfaces.Length + 1];
					array2[array2.Length - 1] = num;
				}
				for (int j = 0; j < this.m_typeInterfaces.Length; j++)
				{
					array2[j] = this.m_module.GetTypeTokenInternal(this.m_typeInterfaces[j]).Token;
				}
				int tkParent = (this.m_declMeth == null) ? this.m_DeclaringType.m_tdType.Token : this.m_declMeth.GetToken().Token;
				this.m_tdType = new TypeToken(this.InternalDefineGenParam(this.m_strName, tkParent, this.m_genParamPos, (int)this.m_genParamAttributes, array2, this.m_module, 0));
				if (this.m_ca != null)
				{
					foreach (object obj in this.m_ca)
					{
						TypeBuilder.CustAttr custAttr = (TypeBuilder.CustAttr)obj;
						custAttr.Bake(this.m_module, this.MetadataTokenInternal);
					}
				}
				this.m_hasBeenCreated = true;
				return this;
			}
			if ((this.m_tdType.Token & 16777215) != 0 && (num & 16777215) != 0)
			{
				TypeBuilder.InternalSetParentType(this.m_tdType.Token, num, this.m_module);
			}
			if (this.m_inst != null)
			{
				foreach (GenericTypeParameterBuilder type in this.m_inst)
				{
					if (type is GenericTypeParameterBuilder)
					{
						((GenericTypeParameterBuilder)type).m_type.CreateType();
					}
				}
			}
			if (!this.m_isHiddenGlobalType && this.m_constructorCount == 0 && (this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic && !base.IsValueType && (this.m_iAttr & (TypeAttributes.Abstract | TypeAttributes.Sealed)) != (TypeAttributes.Abstract | TypeAttributes.Sealed))
			{
				this.DefineDefaultConstructor(MethodAttributes.Public);
			}
			int count = this.m_listMethods.Count;
			for (int l = 0; l < count; l++)
			{
				MethodBuilder methodBuilder = (MethodBuilder)this.m_listMethods[l];
				if (methodBuilder.IsGenericMethodDefinition)
				{
					methodBuilder.GetToken();
				}
				MethodAttributes attributes = methodBuilder.Attributes;
				if ((methodBuilder.GetMethodImplementationFlags() & (MethodImplAttributes)135) == MethodImplAttributes.IL && (attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.PrivateScope)
				{
					int sigLength;
					byte[] localSig = methodBuilder.GetLocalsSignature().InternalGetSignature(out sigLength);
					if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope && (this.m_iAttr & TypeAttributes.Abstract) == TypeAttributes.NotPublic)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadTypeAttributesNotAbstract"));
					}
					byte[] body = methodBuilder.GetBody();
					if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
					{
						if (body != null)
						{
							throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadMethodBody"));
						}
					}
					else if (body == null || body.Length == 0)
					{
						if (methodBuilder.m_ilGenerator != null)
						{
							methodBuilder.CreateMethodBodyHelper(methodBuilder.GetILGenerator());
						}
						body = methodBuilder.GetBody();
						if ((body == null || body.Length == 0) && !methodBuilder.m_canBeRuntimeImpl)
						{
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_BadEmptyMethodBody"), new object[]
							{
								methodBuilder.Name
							}));
						}
					}
					int maxStackSize;
					if (methodBuilder.m_ilGenerator != null)
					{
						maxStackSize = methodBuilder.m_ilGenerator.GetMaxStackSize();
					}
					else
					{
						maxStackSize = 16;
					}
					__ExceptionInstance[] exceptionInstances = methodBuilder.GetExceptionInstances();
					int[] tokenFixups = methodBuilder.GetTokenFixups();
					int[] rvafixups = methodBuilder.GetRVAFixups();
					__ExceptionInstance[] array3 = null;
					int[] array4 = null;
					int[] array5 = null;
					if (exceptionInstances != null)
					{
						array3 = new __ExceptionInstance[exceptionInstances.Length];
						Array.Copy(exceptionInstances, array3, exceptionInstances.Length);
					}
					if (tokenFixups != null)
					{
						array4 = new int[tokenFixups.Length];
						Array.Copy(tokenFixups, array4, tokenFixups.Length);
					}
					if (rvafixups != null)
					{
						array5 = new int[rvafixups.Length];
						Array.Copy(rvafixups, array5, rvafixups.Length);
					}
					TypeBuilder.InternalSetMethodIL(methodBuilder.GetToken().Token, methodBuilder.InitLocals, body, localSig, sigLength, maxStackSize, methodBuilder.GetNumberOfExceptions(), array3, array4, array5, this.m_module);
					if (this.Assembly.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
					{
						methodBuilder.ReleaseBakedStructures();
					}
				}
			}
			this.m_hasBeenCreated = true;
			Type type2 = this.TermCreateClass(this.m_tdType.Token, this.m_module);
			if (!this.m_isHiddenGlobalType)
			{
				this.m_runtimeType = (RuntimeType)type2;
				if (this.m_DeclaringType != null && this.m_DeclaringType.m_runtimeType != null)
				{
					this.m_DeclaringType.m_runtimeType.InvalidateCachedNestedType();
				}
				return type2;
			}
			return null;
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06004D1E RID: 19742 RVA: 0x0010E68C File Offset: 0x0010D68C
		public int Size
		{
			get
			{
				return this.m_iTypeSize;
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06004D1F RID: 19743 RVA: 0x0010E694 File Offset: 0x0010D694
		public PackingSize PackingSize
		{
			get
			{
				return this.m_iPackingSize;
			}
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x0010E69C File Offset: 0x0010D69C
		public void SetParent(Type parent)
		{
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			this.CheckContext(new Type[]
			{
				parent
			});
			if (parent != null)
			{
				this.m_typeParent = parent;
				return;
			}
			if ((this.m_iAttr & TypeAttributes.ClassSemanticsMask) != TypeAttributes.ClassSemanticsMask)
			{
				this.m_typeParent = typeof(object);
				return;
			}
			if ((this.m_iAttr & TypeAttributes.Abstract) == TypeAttributes.NotPublic)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadInterfaceNotAbstract"));
			}
			this.m_typeParent = null;
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x0010E718 File Offset: 0x0010D718
		[ComVisible(true)]
		public void AddInterfaceImplementation(Type interfaceType)
		{
			this.ThrowIfGeneric();
			this.CheckContext(new Type[]
			{
				interfaceType
			});
			if (interfaceType == null)
			{
				throw new ArgumentNullException("interfaceType");
			}
			this.ThrowIfCreated();
			TypeToken typeTokenInternal = this.m_module.GetTypeTokenInternal(interfaceType);
			TypeBuilder.InternalAddInterfaceImpl(this.m_tdType.Token, typeTokenInternal.Token, this.m_module);
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x0010E77C File Offset: 0x0010D77C
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					this.AddDeclarativeSecurityNoLock(action, pset);
					return;
				}
			}
			this.AddDeclarativeSecurityNoLock(action, pset);
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x0010E7E0 File Offset: 0x0010D7E0
		private void AddDeclarativeSecurityNoLock(SecurityAction action, PermissionSet pset)
		{
			this.ThrowIfGeneric();
			if (pset == null)
			{
				throw new ArgumentNullException("pset");
			}
			if (!Enum.IsDefined(typeof(SecurityAction), action) || action == SecurityAction.RequestMinimum || action == SecurityAction.RequestOptional || action == SecurityAction.RequestRefuse)
			{
				throw new ArgumentOutOfRangeException("action");
			}
			this.ThrowIfCreated();
			byte[] blob = null;
			if (!pset.IsEmpty())
			{
				blob = pset.EncodeXml();
			}
			TypeBuilder.InternalAddDeclarativeSecurity(this.m_module, this.m_tdType.Token, action, blob);
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06004D24 RID: 19748 RVA: 0x0010E860 File Offset: 0x0010D860
		public TypeToken TypeToken
		{
			get
			{
				if (this.IsGenericParameter)
				{
					this.ThrowIfCreated();
				}
				return this.m_tdType;
			}
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x0010E878 File Offset: 0x0010D878
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.ThrowIfGeneric();
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			TypeBuilder.InternalCreateCustomAttribute(this.m_tdType.Token, this.m_module.GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x0010E8D3 File Offset: 0x0010D8D3
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.ThrowIfGeneric();
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			customBuilder.CreateCustomAttribute(this.m_module, this.m_tdType.Token);
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x0010E900 File Offset: 0x0010D900
		void _TypeBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x0010E907 File Offset: 0x0010D907
		void _TypeBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x0010E90E File Offset: 0x0010D90E
		void _TypeBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x0010E915 File Offset: 0x0010D915
		void _TypeBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002827 RID: 10279
		public const int UnspecifiedTypeSize = 0;

		// Token: 0x04002828 RID: 10280
		internal ArrayList m_ca;

		// Token: 0x04002829 RID: 10281
		internal MethodBuilder m_currentMethod;

		// Token: 0x0400282A RID: 10282
		private TypeToken m_tdType;

		// Token: 0x0400282B RID: 10283
		private ModuleBuilder m_module;

		// Token: 0x0400282C RID: 10284
		internal string m_strName;

		// Token: 0x0400282D RID: 10285
		private string m_strNameSpace;

		// Token: 0x0400282E RID: 10286
		private string m_strFullQualName;

		// Token: 0x0400282F RID: 10287
		private Type m_typeParent;

		// Token: 0x04002830 RID: 10288
		private Type[] m_typeInterfaces;

		// Token: 0x04002831 RID: 10289
		internal TypeAttributes m_iAttr;

		// Token: 0x04002832 RID: 10290
		internal GenericParameterAttributes m_genParamAttributes;

		// Token: 0x04002833 RID: 10291
		internal ArrayList m_listMethods;

		// Token: 0x04002834 RID: 10292
		private int m_constructorCount;

		// Token: 0x04002835 RID: 10293
		private int m_iTypeSize;

		// Token: 0x04002836 RID: 10294
		private PackingSize m_iPackingSize;

		// Token: 0x04002837 RID: 10295
		private TypeBuilder m_DeclaringType;

		// Token: 0x04002838 RID: 10296
		private Type m_underlyingSystemType;

		// Token: 0x04002839 RID: 10297
		internal bool m_isHiddenGlobalType;

		// Token: 0x0400283A RID: 10298
		internal bool m_isHiddenType;

		// Token: 0x0400283B RID: 10299
		internal bool m_hasBeenCreated;

		// Token: 0x0400283C RID: 10300
		internal RuntimeType m_runtimeType;

		// Token: 0x0400283D RID: 10301
		private int m_genParamPos;

		// Token: 0x0400283E RID: 10302
		private GenericTypeParameterBuilder[] m_inst;

		// Token: 0x0400283F RID: 10303
		private bool m_bIsGenParam;

		// Token: 0x04002840 RID: 10304
		private bool m_bIsGenTypeDef;

		// Token: 0x04002841 RID: 10305
		private MethodBuilder m_declMeth;

		// Token: 0x04002842 RID: 10306
		private TypeBuilder m_genTypeDef;

		// Token: 0x0200084C RID: 2124
		internal class CustAttr
		{
			// Token: 0x06004D2B RID: 19755 RVA: 0x0010E91C File Offset: 0x0010D91C
			public CustAttr(ConstructorInfo con, byte[] binaryAttribute)
			{
				if (con == null)
				{
					throw new ArgumentNullException("con");
				}
				if (binaryAttribute == null)
				{
					throw new ArgumentNullException("binaryAttribute");
				}
				this.m_con = con;
				this.m_binaryAttribute = binaryAttribute;
			}

			// Token: 0x06004D2C RID: 19756 RVA: 0x0010E94E File Offset: 0x0010D94E
			public CustAttr(CustomAttributeBuilder customBuilder)
			{
				if (customBuilder == null)
				{
					throw new ArgumentNullException("customBuilder");
				}
				this.m_customBuilder = customBuilder;
			}

			// Token: 0x06004D2D RID: 19757 RVA: 0x0010E96C File Offset: 0x0010D96C
			public void Bake(ModuleBuilder module, int token)
			{
				if (this.m_customBuilder == null)
				{
					TypeBuilder.InternalCreateCustomAttribute(token, module.GetConstructorToken(this.m_con).Token, this.m_binaryAttribute, module, false);
					return;
				}
				this.m_customBuilder.CreateCustomAttribute(module, token);
			}

			// Token: 0x04002843 RID: 10307
			private ConstructorInfo m_con;

			// Token: 0x04002844 RID: 10308
			private byte[] m_binaryAttribute;

			// Token: 0x04002845 RID: 10309
			private CustomAttributeBuilder m_customBuilder;
		}
	}
}
