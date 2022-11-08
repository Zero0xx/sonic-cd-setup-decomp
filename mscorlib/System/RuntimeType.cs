using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System
{
	// Token: 0x020000F8 RID: 248
	[Serializable]
	internal class RuntimeType : Type, ISerializable, ICloneable
	{
		// Token: 0x06000DE8 RID: 3560
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PrepareMemberInfoCache(RuntimeTypeHandle rt);

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00026F59 File Offset: 0x00025F59
		internal static MethodBase GetMethodBase(ModuleHandle scope, int typeMetadataToken)
		{
			return RuntimeType.GetMethodBase(scope.ResolveMethodHandle(typeMetadataToken));
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00026F68 File Offset: 0x00025F68
		internal static MethodBase GetMethodBase(Module scope, int typeMetadataToken)
		{
			return RuntimeType.GetMethodBase(scope.GetModuleHandle(), typeMetadataToken);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00026F76 File Offset: 0x00025F76
		internal static MethodBase GetMethodBase(RuntimeMethodHandle methodHandle)
		{
			return RuntimeType.GetMethodBase(RuntimeTypeHandle.EmptyHandle, methodHandle);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00026F84 File Offset: 0x00025F84
		internal static MethodBase GetMethodBase(RuntimeTypeHandle reflectedTypeHandle, RuntimeMethodHandle methodHandle)
		{
			if (methodHandle.IsDynamicMethod())
			{
				Resolver resolver = methodHandle.GetResolver();
				if (resolver != null)
				{
					return resolver.GetDynamicMethod();
				}
				return null;
			}
			else
			{
				Type type = methodHandle.GetDeclaringType().GetRuntimeType();
				RuntimeType runtimeType = reflectedTypeHandle.GetRuntimeType();
				RuntimeTypeHandle[] methodInstantiation = null;
				bool flag = false;
				if (runtimeType == null)
				{
					runtimeType = (type as RuntimeType);
				}
				if (runtimeType.IsArray)
				{
					MethodBase[] array = runtimeType.GetMember(methodHandle.GetName(), MemberTypes.Constructor | MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) as MethodBase[];
					bool flag2 = false;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].GetMethodHandle() == methodHandle)
						{
							flag2 = true;
						}
					}
					if (!flag2)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethodHandle"), new object[]
						{
							runtimeType.ToString(),
							type.ToString()
						}));
					}
					type = runtimeType;
				}
				else if (!type.IsAssignableFrom(runtimeType))
				{
					if (!type.IsGenericType)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethodHandle"), new object[]
						{
							runtimeType.ToString(),
							type.ToString()
						}));
					}
					Type genericTypeDefinition = type.GetGenericTypeDefinition();
					Type type2;
					for (type2 = runtimeType; type2 != null; type2 = type2.BaseType)
					{
						Type type3 = type2;
						if (type3.IsGenericType && !type2.IsGenericTypeDefinition)
						{
							type3 = type3.GetGenericTypeDefinition();
						}
						if (type3.Equals(genericTypeDefinition))
						{
							break;
						}
					}
					if (type2 == null)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethodHandle"), new object[]
						{
							runtimeType.ToString(),
							type.ToString()
						}));
					}
					type = type2;
					methodInstantiation = methodHandle.GetMethodInstantiation();
					bool flag3 = methodHandle.IsGenericMethodDefinition();
					methodHandle = methodHandle.GetMethodFromCanonical(type.GetTypeHandleInternal());
					if (!flag3)
					{
						flag = true;
					}
				}
				if (type.IsValueType)
				{
					methodHandle = methodHandle.GetUnboxingStub();
				}
				if (flag || (type.GetTypeHandleInternal().HasInstantiation() && !type.GetTypeHandleInternal().IsGenericTypeDefinition() && !methodHandle.HasMethodInstantiation()))
				{
					methodHandle = methodHandle.GetInstantiatingStub(type.GetTypeHandleInternal(), methodInstantiation);
				}
				if (methodHandle.IsConstructor())
				{
					return runtimeType.Cache.GetConstructor(type.GetTypeHandleInternal(), methodHandle);
				}
				if (methodHandle.HasMethodInstantiation() && !methodHandle.IsGenericMethodDefinition())
				{
					return runtimeType.Cache.GetGenericMethodInfo(methodHandle);
				}
				return runtimeType.Cache.GetMethod(type.GetTypeHandleInternal(), methodHandle);
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x000271FA File Offset: 0x000261FA
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x00027207 File Offset: 0x00026207
		internal bool DomainInitialized
		{
			get
			{
				return this.Cache.DomainInitialized;
			}
			set
			{
				this.Cache.DomainInitialized = value;
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00027215 File Offset: 0x00026215
		internal static FieldInfo GetFieldInfo(RuntimeFieldHandle fieldHandle)
		{
			return RuntimeType.GetFieldInfo(fieldHandle.GetApproxDeclaringType(), fieldHandle);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00027224 File Offset: 0x00026224
		internal static FieldInfo GetFieldInfo(RuntimeTypeHandle reflectedTypeHandle, RuntimeFieldHandle fieldHandle)
		{
			if (reflectedTypeHandle.IsNullHandle())
			{
				reflectedTypeHandle = fieldHandle.GetApproxDeclaringType();
			}
			else
			{
				RuntimeTypeHandle approxDeclaringType = fieldHandle.GetApproxDeclaringType();
				if (!reflectedTypeHandle.Equals(approxDeclaringType) && (!fieldHandle.AcquiresContextFromThis() || !approxDeclaringType.GetCanonicalHandle().Equals(reflectedTypeHandle.GetCanonicalHandle())))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveFieldHandle"), new object[]
					{
						reflectedTypeHandle.GetRuntimeType().ToString(),
						approxDeclaringType.GetRuntimeType().ToString()
					}));
				}
			}
			return reflectedTypeHandle.GetRuntimeType().Cache.GetField(fieldHandle);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x000272CC File Offset: 0x000262CC
		internal static PropertyInfo GetPropertyInfo(RuntimeTypeHandle reflectedTypeHandle, int tkProperty)
		{
			CerArrayList<RuntimePropertyInfo> propertyList = reflectedTypeHandle.GetRuntimeType().Cache.GetPropertyList(MemberListType.All, null);
			for (int i = 0; i < propertyList.Count; i++)
			{
				RuntimePropertyInfo runtimePropertyInfo = propertyList[i];
				if (runtimePropertyInfo.MetadataToken == tkProperty)
				{
					return runtimePropertyInfo;
				}
			}
			throw new SystemException();
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00027318 File Offset: 0x00026318
		private static void ThrowIfTypeNeverValidGenericArgument(Type type)
		{
			if (type.IsPointer || type.IsByRef || type == typeof(void))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NeverValidGenericArgument"), new object[]
				{
					type.ToString()
				}));
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00027370 File Offset: 0x00026370
		internal static void SanityCheckGenericArguments(Type[] genericArguments, Type[] genericParamters)
		{
			if (genericArguments == null)
			{
				throw new ArgumentNullException();
			}
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (genericArguments[i] == null)
				{
					throw new ArgumentNullException();
				}
				RuntimeType.ThrowIfTypeNeverValidGenericArgument(genericArguments[i]);
			}
			if (genericArguments.Length != genericParamters.Length)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NotEnoughGenArguments", new object[]
				{
					genericArguments.Length,
					genericParamters.Length
				}), new object[0]));
			}
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x000273F0 File Offset: 0x000263F0
		internal static void ValidateGenericArguments(MemberInfo definition, Type[] genericArguments, Exception e)
		{
			RuntimeTypeHandle[] array = null;
			RuntimeTypeHandle[] array2 = null;
			Type[] genericArguments2;
			if (definition is Type)
			{
				Type type = (Type)definition;
				genericArguments2 = type.GetGenericArguments();
				array = new RuntimeTypeHandle[genericArguments.Length];
				for (int i = 0; i < genericArguments.Length; i++)
				{
					array[i] = genericArguments[i].GetTypeHandleInternal();
				}
			}
			else
			{
				MethodInfo methodInfo = (MethodInfo)definition;
				genericArguments2 = methodInfo.GetGenericArguments();
				array2 = new RuntimeTypeHandle[genericArguments.Length];
				for (int j = 0; j < genericArguments.Length; j++)
				{
					array2[j] = genericArguments[j].GetTypeHandleInternal();
				}
				Type declaringType = methodInfo.DeclaringType;
				if (declaringType != null)
				{
					array = declaringType.GetTypeHandleInternal().GetInstantiation();
				}
			}
			for (int k = 0; k < genericArguments.Length; k++)
			{
				Type type2 = genericArguments[k];
				Type type3 = genericArguments2[k];
				if (!type3.GetTypeHandleInternal().SatisfiesConstraints(array, array2, type2.GetTypeHandleInternal()))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_GenConstraintViolation"), new object[]
					{
						k.ToString(CultureInfo.CurrentCulture),
						type2.ToString(),
						definition.ToString(),
						type3.ToString()
					}), e);
				}
			}
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00027540 File Offset: 0x00026540
		private static void SplitName(string fullname, out string name, out string ns)
		{
			name = null;
			ns = null;
			if (fullname == null)
			{
				return;
			}
			int num = fullname.LastIndexOf(".", StringComparison.Ordinal);
			if (num == -1)
			{
				name = fullname;
				return;
			}
			ns = fullname.Substring(0, num);
			int num2 = fullname.Length - ns.Length - 1;
			if (num2 != 0)
			{
				name = fullname.Substring(num + 1, num2);
				return;
			}
			name = "";
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x000275A0 File Offset: 0x000265A0
		internal static BindingFlags FilterPreCalculate(bool isPublic, bool isInherited, bool isStatic)
		{
			BindingFlags bindingFlags = isPublic ? BindingFlags.Public : BindingFlags.NonPublic;
			if (isInherited)
			{
				bindingFlags |= BindingFlags.DeclaredOnly;
				if (isStatic)
				{
					bindingFlags |= (BindingFlags.Static | BindingFlags.FlattenHierarchy);
				}
				else
				{
					bindingFlags |= BindingFlags.Instance;
				}
			}
			else if (isStatic)
			{
				bindingFlags |= BindingFlags.Static;
			}
			else
			{
				bindingFlags |= BindingFlags.Instance;
			}
			return bindingFlags;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000275DC File Offset: 0x000265DC
		private static void FilterHelper(BindingFlags bindingFlags, ref string name, bool allowPrefixLookup, out bool prefixLookup, out bool ignoreCase, out MemberListType listType)
		{
			prefixLookup = false;
			ignoreCase = false;
			if (name != null)
			{
				if ((bindingFlags & BindingFlags.IgnoreCase) != BindingFlags.Default)
				{
					name = name.ToLower(CultureInfo.InvariantCulture);
					ignoreCase = true;
					listType = MemberListType.CaseInsensitive;
				}
				else
				{
					listType = MemberListType.CaseSensitive;
				}
				if (allowPrefixLookup && name.EndsWith("*", StringComparison.Ordinal))
				{
					name = name.Substring(0, name.Length - 1);
					prefixLookup = true;
					listType = MemberListType.All;
					return;
				}
			}
			else
			{
				listType = MemberListType.All;
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00027648 File Offset: 0x00026648
		private static void FilterHelper(BindingFlags bindingFlags, ref string name, out bool ignoreCase, out MemberListType listType)
		{
			bool flag;
			RuntimeType.FilterHelper(bindingFlags, ref name, false, out flag, out ignoreCase, out listType);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00027661 File Offset: 0x00026661
		private static bool FilterApplyPrefixLookup(MemberInfo memberInfo, string name, bool ignoreCase)
		{
			if (ignoreCase)
			{
				if (!memberInfo.Name.ToLower(CultureInfo.InvariantCulture).StartsWith(name, StringComparison.Ordinal))
				{
					return false;
				}
			}
			else if (!memberInfo.Name.StartsWith(name, StringComparison.Ordinal))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00027694 File Offset: 0x00026694
		private static bool FilterApplyBase(MemberInfo memberInfo, BindingFlags bindingFlags, bool isPublic, bool isNonProtectedInternal, bool isStatic, string name, bool prefixLookup)
		{
			if (isPublic)
			{
				if ((bindingFlags & BindingFlags.Public) == BindingFlags.Default)
				{
					return false;
				}
			}
			else if ((bindingFlags & BindingFlags.NonPublic) == BindingFlags.Default)
			{
				return false;
			}
			bool flag = memberInfo.DeclaringType != memberInfo.ReflectedType;
			if ((bindingFlags & BindingFlags.DeclaredOnly) != BindingFlags.Default && flag)
			{
				return false;
			}
			if (memberInfo.MemberType != MemberTypes.TypeInfo && memberInfo.MemberType != MemberTypes.NestedType)
			{
				if (isStatic)
				{
					if ((bindingFlags & BindingFlags.FlattenHierarchy) == BindingFlags.Default && flag)
					{
						return false;
					}
					if ((bindingFlags & BindingFlags.Static) == BindingFlags.Default)
					{
						return false;
					}
				}
				else if ((bindingFlags & BindingFlags.Instance) == BindingFlags.Default)
				{
					return false;
				}
			}
			if (prefixLookup && !RuntimeType.FilterApplyPrefixLookup(memberInfo, name, (bindingFlags & BindingFlags.IgnoreCase) != BindingFlags.Default))
			{
				return false;
			}
			if ((bindingFlags & BindingFlags.DeclaredOnly) == BindingFlags.Default && flag && isNonProtectedInternal && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.Default && !isStatic && (bindingFlags & BindingFlags.Instance) != BindingFlags.Default)
			{
				MethodInfo methodInfo = memberInfo as MethodInfo;
				if (methodInfo == null)
				{
					return false;
				}
				if (!methodInfo.IsVirtual && !methodInfo.IsAbstract)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00027758 File Offset: 0x00026758
		private static bool FilterApplyType(Type type, BindingFlags bindingFlags, string name, bool prefixLookup, string ns)
		{
			bool isPublic = type.IsNestedPublic || type.IsPublic;
			bool isStatic = false;
			return RuntimeType.FilterApplyBase(type, bindingFlags, isPublic, type.IsNestedAssembly, isStatic, name, prefixLookup) && (ns == null || type.Namespace.Equals(ns));
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000277A4 File Offset: 0x000267A4
		private static bool FilterApplyMethodBaseInfo(MethodBase methodBase, BindingFlags bindingFlags, string name, CallingConventions callConv, Type[] argumentTypes, bool prefixLookup)
		{
			bindingFlags ^= BindingFlags.DeclaredOnly;
			RuntimeMethodInfo runtimeMethodInfo = methodBase as RuntimeMethodInfo;
			BindingFlags bindingFlags2;
			if (runtimeMethodInfo == null)
			{
				RuntimeConstructorInfo runtimeConstructorInfo = methodBase as RuntimeConstructorInfo;
				bindingFlags2 = runtimeConstructorInfo.BindingFlags;
			}
			else
			{
				bindingFlags2 = runtimeMethodInfo.BindingFlags;
			}
			return (bindingFlags & bindingFlags2) == bindingFlags2 && (!prefixLookup || RuntimeType.FilterApplyPrefixLookup(methodBase, name, (bindingFlags & BindingFlags.IgnoreCase) != BindingFlags.Default)) && RuntimeType.FilterApplyMethodBaseInfo(methodBase, bindingFlags, callConv, argumentTypes);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00027800 File Offset: 0x00026800
		private static bool FilterApplyMethodBaseInfo(MethodBase methodBase, BindingFlags bindingFlags, CallingConventions callConv, Type[] argumentTypes)
		{
			if ((callConv & CallingConventions.Any) == (CallingConventions)0)
			{
				if ((callConv & CallingConventions.VarArgs) != (CallingConventions)0 && (methodBase.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
				{
					return false;
				}
				if ((callConv & CallingConventions.Standard) != (CallingConventions)0 && (methodBase.CallingConvention & CallingConventions.Standard) == (CallingConventions)0)
				{
					return false;
				}
			}
			if (argumentTypes != null)
			{
				ParameterInfo[] parametersNoCopy = methodBase.GetParametersNoCopy();
				if (argumentTypes.Length != parametersNoCopy.Length)
				{
					if ((bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetProperty | BindingFlags.SetProperty)) == BindingFlags.Default)
					{
						return false;
					}
					bool flag = false;
					bool flag2 = argumentTypes.Length > parametersNoCopy.Length;
					if (flag2)
					{
						if ((methodBase.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
						{
							flag = true;
						}
					}
					else if ((bindingFlags & BindingFlags.OptionalParamBinding) == BindingFlags.Default)
					{
						flag = true;
					}
					else if (!parametersNoCopy[argumentTypes.Length].IsOptional)
					{
						flag = true;
					}
					if (flag)
					{
						if (parametersNoCopy.Length == 0)
						{
							return false;
						}
						bool flag3 = argumentTypes.Length < parametersNoCopy.Length - 1;
						if (flag3)
						{
							return false;
						}
						ParameterInfo parameterInfo = parametersNoCopy[parametersNoCopy.Length - 1];
						if (!parameterInfo.ParameterType.IsArray)
						{
							return false;
						}
						if (!parameterInfo.IsDefined(typeof(ParamArrayAttribute), false))
						{
							return false;
						}
					}
				}
				else if ((bindingFlags & BindingFlags.ExactBinding) != BindingFlags.Default && (bindingFlags & BindingFlags.InvokeMethod) == BindingFlags.Default)
				{
					for (int i = 0; i < parametersNoCopy.Length; i++)
					{
						if (argumentTypes[i] != null && parametersNoCopy[i].ParameterType != argumentTypes[i])
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00027916 File Offset: 0x00026916
		private RuntimeType(RuntimeTypeHandle typeHandle)
		{
			this.m_handle = typeHandle;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00027925 File Offset: 0x00026925
		internal RuntimeType()
		{
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00027930 File Offset: 0x00026930
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimeType runtimeType = o as RuntimeType;
			return runtimeType != null && runtimeType.m_handle.Equals(this.m_handle);
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000E01 RID: 3585 RVA: 0x0002795C File Offset: 0x0002695C
		private new RuntimeType.RuntimeTypeCache Cache
		{
			get
			{
				if (this.m_cache.IsNull())
				{
					IntPtr gchandle = this.m_handle.GetGCHandle(GCHandleType.WeakTrackResurrection);
					if (!Interlocked.CompareExchange(ref this.m_cache, gchandle, (IntPtr)0).IsNull())
					{
						this.m_handle.FreeGCHandle(gchandle);
					}
				}
				RuntimeType.RuntimeTypeCache runtimeTypeCache = GCHandle.InternalGet(this.m_cache) as RuntimeType.RuntimeTypeCache;
				if (runtimeTypeCache == null)
				{
					runtimeTypeCache = new RuntimeType.RuntimeTypeCache(this);
					RuntimeType.RuntimeTypeCache runtimeTypeCache2 = GCHandle.InternalCompareExchange(this.m_cache, runtimeTypeCache, null, false) as RuntimeType.RuntimeTypeCache;
					if (runtimeTypeCache2 != null)
					{
						runtimeTypeCache = runtimeTypeCache2;
					}
					if (RuntimeType.s_typeCache == null)
					{
						RuntimeType.s_typeCache = new RuntimeType.TypeCacheQueue();
					}
				}
				return runtimeTypeCache;
			}
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000279F4 File Offset: 0x000269F4
		private MethodInfo[] GetMethodCandidates(string name, BindingFlags bindingAttr, CallingConventions callConv, Type[] types, bool allowPrefixLookup)
		{
			bool flag;
			bool ignoreCase;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out ignoreCase, out listType);
			List<MethodInfo> list = new List<MethodInfo>();
			CerArrayList<RuntimeMethodInfo> methodList = this.Cache.GetMethodList(listType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < methodList.Count; i++)
			{
				RuntimeMethodInfo runtimeMethodInfo = methodList[i];
				if ((bindingAttr & runtimeMethodInfo.BindingFlags) == runtimeMethodInfo.BindingFlags && RuntimeType.FilterApplyMethodBaseInfo(runtimeMethodInfo, bindingAttr, callConv, types) && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimeMethodInfo, name, ignoreCase)))
				{
					list.Add(runtimeMethodInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00027A88 File Offset: 0x00026A88
		private ConstructorInfo[] GetConstructorCandidates(string name, BindingFlags bindingAttr, CallingConventions callConv, Type[] types, bool allowPrefixLookup)
		{
			bool flag;
			bool ignoreCase;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out ignoreCase, out listType);
			List<ConstructorInfo> list = new List<ConstructorInfo>();
			CerArrayList<RuntimeConstructorInfo> constructorList = this.Cache.GetConstructorList(listType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < constructorList.Count; i++)
			{
				RuntimeConstructorInfo runtimeConstructorInfo = constructorList[i];
				if ((bindingAttr & runtimeConstructorInfo.BindingFlags) == runtimeConstructorInfo.BindingFlags && RuntimeType.FilterApplyMethodBaseInfo(runtimeConstructorInfo, bindingAttr, callConv, types) && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimeConstructorInfo, name, ignoreCase)))
				{
					list.Add(runtimeConstructorInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00027B1C File Offset: 0x00026B1C
		private PropertyInfo[] GetPropertyCandidates(string name, BindingFlags bindingAttr, Type[] types, bool allowPrefixLookup)
		{
			bool flag;
			bool ignoreCase;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out ignoreCase, out listType);
			List<PropertyInfo> list = new List<PropertyInfo>();
			CerArrayList<RuntimePropertyInfo> propertyList = this.Cache.GetPropertyList(listType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < propertyList.Count; i++)
			{
				RuntimePropertyInfo runtimePropertyInfo = propertyList[i];
				if ((bindingAttr & runtimePropertyInfo.BindingFlags) == runtimePropertyInfo.BindingFlags && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimePropertyInfo, name, ignoreCase)) && (types == null || runtimePropertyInfo.GetIndexParameters().Length == types.Length))
				{
					list.Add(runtimePropertyInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00027BB4 File Offset: 0x00026BB4
		private EventInfo[] GetEventCandidates(string name, BindingFlags bindingAttr, bool allowPrefixLookup)
		{
			bool flag;
			bool ignoreCase;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out ignoreCase, out listType);
			List<EventInfo> list = new List<EventInfo>();
			CerArrayList<RuntimeEventInfo> eventList = this.Cache.GetEventList(listType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < eventList.Count; i++)
			{
				RuntimeEventInfo runtimeEventInfo = eventList[i];
				if ((bindingAttr & runtimeEventInfo.BindingFlags) == runtimeEventInfo.BindingFlags && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimeEventInfo, name, ignoreCase)))
				{
					list.Add(runtimeEventInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00027C3C File Offset: 0x00026C3C
		private FieldInfo[] GetFieldCandidates(string name, BindingFlags bindingAttr, bool allowPrefixLookup)
		{
			bool flag;
			bool ignoreCase;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out ignoreCase, out listType);
			List<FieldInfo> list = new List<FieldInfo>();
			CerArrayList<RuntimeFieldInfo> fieldList = this.Cache.GetFieldList(listType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < fieldList.Count; i++)
			{
				RuntimeFieldInfo runtimeFieldInfo = fieldList[i];
				if ((bindingAttr & runtimeFieldInfo.BindingFlags) == runtimeFieldInfo.BindingFlags && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimeFieldInfo, name, ignoreCase)))
				{
					list.Add(runtimeFieldInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00027CC4 File Offset: 0x00026CC4
		private Type[] GetNestedTypeCandidates(string fullname, BindingFlags bindingAttr, bool allowPrefixLookup)
		{
			bindingAttr &= ~BindingFlags.Static;
			string name;
			string ns;
			RuntimeType.SplitName(fullname, out name, out ns);
			bool prefixLookup;
			bool flag;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out prefixLookup, out flag, out listType);
			List<Type> list = new List<Type>();
			CerArrayList<RuntimeType> nestedTypeList = this.Cache.GetNestedTypeList(listType, name);
			for (int i = 0; i < nestedTypeList.Count; i++)
			{
				RuntimeType runtimeType = nestedTypeList[i];
				if (RuntimeType.FilterApplyType(runtimeType, bindingAttr, name, prefixLookup, ns))
				{
					list.Add(runtimeType);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00027D45 File Offset: 0x00026D45
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.GetMethodCandidates(null, bindingAttr, CallingConventions.Any, null, false);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00027D52 File Offset: 0x00026D52
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.GetConstructorCandidates(null, bindingAttr, CallingConventions.Any, null, false);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00027D5F File Offset: 0x00026D5F
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.GetPropertyCandidates(null, bindingAttr, null, false);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00027D6B File Offset: 0x00026D6B
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.GetEventCandidates(null, bindingAttr, false);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00027D76 File Offset: 0x00026D76
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.GetFieldCandidates(null, bindingAttr, false);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00027D84 File Offset: 0x00026D84
		public override Type[] GetInterfaces()
		{
			CerArrayList<RuntimeType> interfaceList = this.Cache.GetInterfaceList(MemberListType.All, null);
			Type[] array = new Type[interfaceList.Count];
			for (int i = 0; i < interfaceList.Count; i++)
			{
				JitHelpers.UnsafeSetArrayElement(array, i, interfaceList[i]);
			}
			return array;
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00027DCB File Offset: 0x00026DCB
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this.GetNestedTypeCandidates(null, bindingAttr, false);
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00027DD8 File Offset: 0x00026DD8
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			MethodInfo[] methodCandidates = this.GetMethodCandidates(null, bindingAttr, CallingConventions.Any, null, false);
			ConstructorInfo[] constructorCandidates = this.GetConstructorCandidates(null, bindingAttr, CallingConventions.Any, null, false);
			PropertyInfo[] propertyCandidates = this.GetPropertyCandidates(null, bindingAttr, null, false);
			EventInfo[] eventCandidates = this.GetEventCandidates(null, bindingAttr, false);
			FieldInfo[] fieldCandidates = this.GetFieldCandidates(null, bindingAttr, false);
			Type[] nestedTypeCandidates = this.GetNestedTypeCandidates(null, bindingAttr, false);
			MemberInfo[] array = new MemberInfo[methodCandidates.Length + constructorCandidates.Length + propertyCandidates.Length + eventCandidates.Length + fieldCandidates.Length + nestedTypeCandidates.Length];
			int num = 0;
			Array.Copy(methodCandidates, 0, array, num, methodCandidates.Length);
			num += methodCandidates.Length;
			Array.Copy(constructorCandidates, 0, array, num, constructorCandidates.Length);
			num += constructorCandidates.Length;
			Array.Copy(propertyCandidates, 0, array, num, propertyCandidates.Length);
			num += propertyCandidates.Length;
			Array.Copy(eventCandidates, 0, array, num, eventCandidates.Length);
			num += eventCandidates.Length;
			Array.Copy(fieldCandidates, 0, array, num, fieldCandidates.Length);
			num += fieldCandidates.Length;
			Array.Copy(nestedTypeCandidates, 0, array, num, nestedTypeCandidates.Length);
			num += nestedTypeCandidates.Length;
			return array;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00027ED8 File Offset: 0x00026ED8
		public override InterfaceMapping GetInterfaceMap(Type ifaceType)
		{
			if (this.IsGenericParameter)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_GenericParameter"));
			}
			if (ifaceType == null)
			{
				throw new ArgumentNullException("ifaceType");
			}
			if (!(ifaceType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "ifaceType");
			}
			RuntimeType runtimeType = ifaceType as RuntimeType;
			RuntimeTypeHandle typeHandleInternal = runtimeType.GetTypeHandleInternal();
			int firstSlotForInterface = this.GetTypeHandleInternal().GetFirstSlotForInterface(runtimeType.GetTypeHandleInternal());
			int interfaceMethodSlots = typeHandleInternal.GetInterfaceMethodSlots();
			int num = 0;
			for (int i = 0; i < interfaceMethodSlots; i++)
			{
				if ((typeHandleInternal.GetMethodAt(i).GetAttributes() & MethodAttributes.Static) != MethodAttributes.PrivateScope)
				{
					num++;
				}
			}
			int num2 = interfaceMethodSlots - num;
			InterfaceMapping result;
			result.InterfaceType = ifaceType;
			result.TargetType = this;
			result.InterfaceMethods = new MethodInfo[num2];
			result.TargetMethods = new MethodInfo[num2];
			for (int j = 0; j < interfaceMethodSlots; j++)
			{
				RuntimeMethodHandle runtimeMethodHandle = typeHandleInternal.GetMethodAt(j);
				if ((typeHandleInternal.GetMethodAt(j).GetAttributes() & MethodAttributes.Static) == MethodAttributes.PrivateScope)
				{
					bool flag = typeHandleInternal.HasInstantiation() && !typeHandleInternal.IsGenericTypeDefinition();
					if (flag)
					{
						runtimeMethodHandle = runtimeMethodHandle.GetInstantiatingStubIfNeeded(typeHandleInternal);
					}
					MethodBase methodBase = RuntimeType.GetMethodBase(typeHandleInternal, runtimeMethodHandle);
					result.InterfaceMethods[j] = (MethodInfo)methodBase;
					int num3;
					if (firstSlotForInterface == -1)
					{
						num3 = this.GetTypeHandleInternal().GetInterfaceMethodImplementationSlot(typeHandleInternal, runtimeMethodHandle);
					}
					else
					{
						num3 = firstSlotForInterface + j;
					}
					if (num3 != -1)
					{
						RuntimeTypeHandle typeHandleInternal2 = this.GetTypeHandleInternal();
						RuntimeMethodHandle methodHandle = typeHandleInternal2.GetMethodAt(num3);
						flag = (typeHandleInternal2.HasInstantiation() && !typeHandleInternal2.IsGenericTypeDefinition());
						if (flag)
						{
							methodHandle = methodHandle.GetInstantiatingStubIfNeeded(typeHandleInternal2);
						}
						MethodBase methodBase2 = RuntimeType.GetMethodBase(typeHandleInternal2, methodHandle);
						result.TargetMethods[j] = (MethodInfo)methodBase2;
					}
				}
			}
			return result;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x000280B0 File Offset: 0x000270B0
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConv, Type[] types, ParameterModifier[] modifiers)
		{
			MethodInfo[] methodCandidates = this.GetMethodCandidates(name, bindingAttr, callConv, types, false);
			if (methodCandidates.Length == 0)
			{
				return null;
			}
			if (types == null || types.Length == 0)
			{
				if (methodCandidates.Length == 1)
				{
					return methodCandidates[0];
				}
				if (types == null)
				{
					for (int i = 1; i < methodCandidates.Length; i++)
					{
						MethodInfo m = methodCandidates[i];
						if (!System.DefaultBinder.CompareMethodSigAndName(m, methodCandidates[0]))
						{
							throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
						}
					}
					return System.DefaultBinder.FindMostDerivedNewSlotMeth(methodCandidates, methodCandidates.Length) as MethodInfo;
				}
			}
			if (binder == null)
			{
				binder = Type.DefaultBinder;
			}
			return binder.SelectMethod(bindingAttr, methodCandidates, types, modifiers) as MethodInfo;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00028144 File Offset: 0x00027144
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			ConstructorInfo[] constructorCandidates = this.GetConstructorCandidates(null, bindingAttr, CallingConventions.Any, types, false);
			if (binder == null)
			{
				binder = Type.DefaultBinder;
			}
			if (constructorCandidates.Length == 0)
			{
				return null;
			}
			if (types.Length == 0 && constructorCandidates.Length == 1)
			{
				ParameterInfo[] parametersNoCopy = constructorCandidates[0].GetParametersNoCopy();
				if (parametersNoCopy == null || parametersNoCopy.Length == 0)
				{
					return constructorCandidates[0];
				}
			}
			if ((bindingAttr & BindingFlags.ExactBinding) != BindingFlags.Default)
			{
				return System.DefaultBinder.ExactBinding(constructorCandidates, types, modifiers) as ConstructorInfo;
			}
			return binder.SelectMethod(bindingAttr, constructorCandidates, types, modifiers) as ConstructorInfo;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000281BC File Offset: 0x000271BC
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			PropertyInfo[] propertyCandidates = this.GetPropertyCandidates(name, bindingAttr, types, false);
			if (binder == null)
			{
				binder = Type.DefaultBinder;
			}
			if (propertyCandidates.Length == 0)
			{
				return null;
			}
			if (types == null || types.Length == 0)
			{
				if (propertyCandidates.Length == 1)
				{
					if (returnType != null && returnType != propertyCandidates[0].PropertyType)
					{
						return null;
					}
					return propertyCandidates[0];
				}
				else if (returnType == null)
				{
					throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
				}
			}
			if ((bindingAttr & BindingFlags.ExactBinding) != BindingFlags.Default)
			{
				return System.DefaultBinder.ExactPropertyBinding(propertyCandidates, returnType, types, modifiers);
			}
			return binder.SelectProperty(bindingAttr, propertyCandidates, returnType, types, modifiers);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00028250 File Offset: 0x00027250
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			bool flag;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, out flag, out listType);
			CerArrayList<RuntimeEventInfo> eventList = this.Cache.GetEventList(listType, name);
			EventInfo eventInfo = null;
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < eventList.Count; i++)
			{
				RuntimeEventInfo runtimeEventInfo = eventList[i];
				if ((bindingAttr & runtimeEventInfo.BindingFlags) == runtimeEventInfo.BindingFlags)
				{
					if (eventInfo != null)
					{
						throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
					}
					eventInfo = runtimeEventInfo;
				}
			}
			return eventInfo;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000282D0 File Offset: 0x000272D0
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			bool flag;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, out flag, out listType);
			CerArrayList<RuntimeFieldInfo> fieldList = this.Cache.GetFieldList(listType, name);
			FieldInfo fieldInfo = null;
			bindingAttr ^= BindingFlags.DeclaredOnly;
			bool flag2 = false;
			for (int i = 0; i < fieldList.Count; i++)
			{
				RuntimeFieldInfo runtimeFieldInfo = fieldList[i];
				if ((bindingAttr & runtimeFieldInfo.BindingFlags) == runtimeFieldInfo.BindingFlags)
				{
					if (fieldInfo != null)
					{
						if (runtimeFieldInfo.DeclaringType == fieldInfo.DeclaringType)
						{
							throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
						}
						if (fieldInfo.DeclaringType.IsInterface && runtimeFieldInfo.DeclaringType.IsInterface)
						{
							flag2 = true;
						}
					}
					if (fieldInfo == null || runtimeFieldInfo.DeclaringType.IsSubclassOf(fieldInfo.DeclaringType) || fieldInfo.DeclaringType.IsInterface)
					{
						fieldInfo = runtimeFieldInfo;
					}
				}
			}
			if (flag2 && fieldInfo.DeclaringType.IsInterface)
			{
				throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
			}
			return fieldInfo;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000283CC File Offset: 0x000273CC
		public override Type GetInterface(string fullname, bool ignoreCase)
		{
			if (fullname == null)
			{
				throw new ArgumentNullException();
			}
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
			bindingFlags &= ~BindingFlags.Static;
			if (ignoreCase)
			{
				bindingFlags |= BindingFlags.IgnoreCase;
			}
			string name;
			string ns;
			RuntimeType.SplitName(fullname, out name, out ns);
			MemberListType listType;
			RuntimeType.FilterHelper(bindingFlags, ref name, out ignoreCase, out listType);
			CerArrayList<RuntimeType> interfaceList = this.Cache.GetInterfaceList(listType, name);
			RuntimeType runtimeType = null;
			for (int i = 0; i < interfaceList.Count; i++)
			{
				RuntimeType runtimeType2 = interfaceList[i];
				if (RuntimeType.FilterApplyType(runtimeType2, bindingFlags, name, false, ns))
				{
					if (runtimeType != null)
					{
						throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
					}
					runtimeType = runtimeType2;
				}
			}
			return runtimeType;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00028464 File Offset: 0x00027464
		public override Type GetNestedType(string fullname, BindingFlags bindingAttr)
		{
			if (fullname == null)
			{
				throw new ArgumentNullException();
			}
			bindingAttr &= ~BindingFlags.Static;
			string name;
			string ns;
			RuntimeType.SplitName(fullname, out name, out ns);
			bool flag;
			MemberListType listType;
			RuntimeType.FilterHelper(bindingAttr, ref name, out flag, out listType);
			CerArrayList<RuntimeType> nestedTypeList = this.Cache.GetNestedTypeList(listType, name);
			RuntimeType runtimeType = null;
			for (int i = 0; i < nestedTypeList.Count; i++)
			{
				RuntimeType runtimeType2 = nestedTypeList[i];
				if (RuntimeType.FilterApplyType(runtimeType2, bindingAttr, name, false, ns))
				{
					if (runtimeType != null)
					{
						throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
					}
					runtimeType = runtimeType2;
				}
			}
			return runtimeType;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000284F0 File Offset: 0x000274F0
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			MethodInfo[] array = new MethodInfo[0];
			ConstructorInfo[] array2 = new ConstructorInfo[0];
			PropertyInfo[] array3 = new PropertyInfo[0];
			EventInfo[] array4 = new EventInfo[0];
			FieldInfo[] array5 = new FieldInfo[0];
			Type[] array6 = new Type[0];
			if ((type & MemberTypes.Method) != (MemberTypes)0)
			{
				array = this.GetMethodCandidates(name, bindingAttr, CallingConventions.Any, null, true);
			}
			if ((type & MemberTypes.Constructor) != (MemberTypes)0)
			{
				array2 = this.GetConstructorCandidates(name, bindingAttr, CallingConventions.Any, null, true);
			}
			if ((type & MemberTypes.Property) != (MemberTypes)0)
			{
				array3 = this.GetPropertyCandidates(name, bindingAttr, null, true);
			}
			if ((type & MemberTypes.Event) != (MemberTypes)0)
			{
				array4 = this.GetEventCandidates(name, bindingAttr, true);
			}
			if ((type & MemberTypes.Field) != (MemberTypes)0)
			{
				array5 = this.GetFieldCandidates(name, bindingAttr, true);
			}
			if ((type & (MemberTypes.TypeInfo | MemberTypes.NestedType)) != (MemberTypes)0)
			{
				array6 = this.GetNestedTypeCandidates(name, bindingAttr, true);
			}
			if (type <= MemberTypes.Property)
			{
				switch (type)
				{
				case MemberTypes.Constructor:
					return array2;
				case MemberTypes.Event:
					return array4;
				case MemberTypes.Constructor | MemberTypes.Event:
				case MemberTypes.Constructor | MemberTypes.Field:
				case MemberTypes.Event | MemberTypes.Field:
				case MemberTypes.Constructor | MemberTypes.Event | MemberTypes.Field:
					break;
				case MemberTypes.Field:
					return array5;
				case MemberTypes.Method:
					return array;
				case MemberTypes.Constructor | MemberTypes.Method:
				{
					MethodBase[] array7 = new MethodBase[array.Length + array2.Length];
					Array.Copy(array, array7, array.Length);
					Array.Copy(array2, 0, array7, array.Length, array2.Length);
					return array7;
				}
				default:
					if (type == MemberTypes.Property)
					{
						return array3;
					}
					break;
				}
			}
			else
			{
				if (type == MemberTypes.TypeInfo)
				{
					return array6;
				}
				if (type == MemberTypes.NestedType)
				{
					return array6;
				}
			}
			MemberInfo[] array8 = new MemberInfo[array.Length + array2.Length + array3.Length + array4.Length + array5.Length + array6.Length];
			int num = 0;
			if (array.Length > 0)
			{
				Array.Copy(array, 0, array8, num, array.Length);
			}
			num += array.Length;
			if (array2.Length > 0)
			{
				Array.Copy(array2, 0, array8, num, array2.Length);
			}
			num += array2.Length;
			if (array3.Length > 0)
			{
				Array.Copy(array3, 0, array8, num, array3.Length);
			}
			num += array3.Length;
			if (array4.Length > 0)
			{
				Array.Copy(array4, 0, array8, num, array4.Length);
			}
			num += array4.Length;
			if (array5.Length > 0)
			{
				Array.Copy(array5, 0, array8, num, array5.Length);
			}
			num += array5.Length;
			if (array6.Length > 0)
			{
				Array.Copy(array6, 0, array8, num, array6.Length);
			}
			num += array6.Length;
			return array8;
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x000286F8 File Offset: 0x000276F8
		public override Module Module
		{
			get
			{
				return this.GetTypeHandleInternal().GetModuleHandle().GetModule();
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0002871C File Offset: 0x0002771C
		public override Assembly Assembly
		{
			get
			{
				return this.GetTypeHandleInternal().GetAssemblyHandle().GetAssembly();
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x0002873F File Offset: 0x0002773F
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this.m_handle;
			}
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00028747 File Offset: 0x00027747
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override RuntimeTypeHandle GetTypeHandleInternal()
		{
			return this.m_handle;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00028750 File Offset: 0x00027750
		internal override TypeCode GetTypeCodeInternal()
		{
			TypeCode typeCode = this.Cache.TypeCode;
			if (typeCode != TypeCode.Empty)
			{
				return typeCode;
			}
			switch (this.GetTypeHandleInternal().GetCorElementType())
			{
			case CorElementType.Boolean:
				typeCode = TypeCode.Boolean;
				goto IL_113;
			case CorElementType.Char:
				typeCode = TypeCode.Char;
				goto IL_113;
			case CorElementType.I1:
				typeCode = TypeCode.SByte;
				goto IL_113;
			case CorElementType.U1:
				typeCode = TypeCode.Byte;
				goto IL_113;
			case CorElementType.I2:
				typeCode = TypeCode.Int16;
				goto IL_113;
			case CorElementType.U2:
				typeCode = TypeCode.UInt16;
				goto IL_113;
			case CorElementType.I4:
				typeCode = TypeCode.Int32;
				goto IL_113;
			case CorElementType.U4:
				typeCode = TypeCode.UInt32;
				goto IL_113;
			case CorElementType.I8:
				typeCode = TypeCode.Int64;
				goto IL_113;
			case CorElementType.U8:
				typeCode = TypeCode.UInt64;
				goto IL_113;
			case CorElementType.R4:
				typeCode = TypeCode.Single;
				goto IL_113;
			case CorElementType.R8:
				typeCode = TypeCode.Double;
				goto IL_113;
			case CorElementType.String:
				typeCode = TypeCode.String;
				goto IL_113;
			case CorElementType.ValueType:
				if (this == Convert.ConvertTypes[15])
				{
					typeCode = TypeCode.Decimal;
					goto IL_113;
				}
				if (this == Convert.ConvertTypes[16])
				{
					typeCode = TypeCode.DateTime;
					goto IL_113;
				}
				if (base.IsEnum)
				{
					typeCode = Type.GetTypeCode(Enum.GetUnderlyingType(this));
					goto IL_113;
				}
				typeCode = TypeCode.Object;
				goto IL_113;
			}
			if (this == Convert.ConvertTypes[2])
			{
				typeCode = TypeCode.DBNull;
			}
			else if (this == Convert.ConvertTypes[18])
			{
				typeCode = TypeCode.String;
			}
			else
			{
				typeCode = TypeCode.Object;
			}
			IL_113:
			this.Cache.TypeCode = typeCode;
			return typeCode;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x00028880 File Offset: 0x00027880
		public override MethodBase DeclaringMethod
		{
			get
			{
				if (!this.IsGenericParameter)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
				}
				RuntimeMethodHandle declaringMethod = this.GetTypeHandleInternal().GetDeclaringMethod();
				if (declaringMethod.IsNullHandle())
				{
					return null;
				}
				return RuntimeType.GetMethodBase(declaringMethod.GetDeclaringType(), declaringMethod);
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x000288CC File Offset: 0x000278CC
		public override bool IsInstanceOfType(object o)
		{
			return this.GetTypeHandleInternal().IsInstanceOfType(o);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x000288E8 File Offset: 0x000278E8
		[ComVisible(true)]
		public override bool IsSubclassOf(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			for (Type baseType = this.BaseType; baseType != null; baseType = baseType.BaseType)
			{
				if (baseType == type)
				{
					return true;
				}
			}
			return type == typeof(object) && type != this;
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00028930 File Offset: 0x00027930
		public override Type BaseType
		{
			get
			{
				if (base.IsInterface)
				{
					return null;
				}
				if (this.m_handle.IsGenericVariable())
				{
					Type[] genericParameterConstraints = this.GetGenericParameterConstraints();
					Type type = typeof(object);
					foreach (Type type2 in genericParameterConstraints)
					{
						if (!type2.IsInterface)
						{
							if (type2.IsGenericParameter)
							{
								GenericParameterAttributes genericParameterAttributes = type2.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
								if ((genericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.None && (genericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.None)
								{
									goto IL_5A;
								}
							}
							type = type2;
						}
						IL_5A:;
					}
					if (type == typeof(object))
					{
						GenericParameterAttributes genericParameterAttributes2 = this.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
						if ((genericParameterAttributes2 & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.None)
						{
							type = typeof(ValueType);
						}
					}
					return type;
				}
				return this.m_handle.GetBaseTypeHandle().GetRuntimeType();
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x000289E0 File Offset: 0x000279E0
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x000289E3 File Offset: 0x000279E3
		public override string FullName
		{
			get
			{
				return this.Cache.GetFullName();
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x000289F0 File Offset: 0x000279F0
		public override string AssemblyQualifiedName
		{
			get
			{
				if (!this.IsGenericTypeDefinition && this.ContainsGenericParameters)
				{
					return null;
				}
				return Assembly.CreateQualifiedName(this.Assembly.FullName, this.FullName);
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00028A1C File Offset: 0x00027A1C
		public override string Namespace
		{
			get
			{
				string nameSpace = this.Cache.GetNameSpace();
				if (nameSpace == null || nameSpace.Length == 0)
				{
					return null;
				}
				return nameSpace;
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00028A43 File Offset: 0x00027A43
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.m_handle.GetAttributes();
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x00028A50 File Offset: 0x00027A50
		public override Guid GUID
		{
			get
			{
				Guid result = default(Guid);
				this.GetGUID(ref result);
				return result;
			}
		}

		// Token: 0x06000E28 RID: 3624
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetGUID(ref Guid result);

		// Token: 0x06000E29 RID: 3625 RVA: 0x00028A70 File Offset: 0x00027A70
		protected override bool IsContextfulImpl()
		{
			return this.GetTypeHandleInternal().IsContextful();
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00028A8C File Offset: 0x00027A8C
		protected override bool IsByRefImpl()
		{
			CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
			return corElementType == CorElementType.ByRef;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00028AB0 File Offset: 0x00027AB0
		protected override bool IsPrimitiveImpl()
		{
			CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
			return (corElementType >= CorElementType.Boolean && corElementType <= CorElementType.R8) || corElementType == CorElementType.I || corElementType == CorElementType.U;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00028AE4 File Offset: 0x00027AE4
		protected override bool IsPointerImpl()
		{
			CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
			return corElementType == CorElementType.Ptr;
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00028B08 File Offset: 0x00027B08
		protected override bool IsCOMObjectImpl()
		{
			return this.GetTypeHandleInternal().IsComObject(false);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00028B24 File Offset: 0x00027B24
		internal override bool HasProxyAttributeImpl()
		{
			return this.GetTypeHandleInternal().HasProxyAttribute();
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00028B3F File Offset: 0x00027B3F
		protected override bool HasElementTypeImpl()
		{
			return base.IsArray || base.IsPointer || base.IsByRef;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00028B5C File Offset: 0x00027B5C
		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				if (!this.IsGenericParameter)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
				}
				GenericParameterAttributes result;
				this.GetTypeHandleInternal().GetModuleHandle().GetMetadataImport().GetGenericParamProps(this.MetadataToken, out result);
				return result;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x00028BA8 File Offset: 0x00027BA8
		internal override bool IsSzArray
		{
			get
			{
				CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
				return corElementType == CorElementType.SzArray;
			}
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00028BCC File Offset: 0x00027BCC
		protected override bool IsArrayImpl()
		{
			CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
			return corElementType == CorElementType.Array || corElementType == CorElementType.SzArray;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00028BF4 File Offset: 0x00027BF4
		public override int GetArrayRank()
		{
			if (!this.IsArrayImpl())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_HasToBeArrayClass"));
			}
			return this.GetTypeHandleInternal().GetArrayRank();
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00028C28 File Offset: 0x00027C28
		public override Type GetElementType()
		{
			return this.GetTypeHandleInternal().GetElementType().GetRuntimeType();
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00028C4C File Offset: 0x00027C4C
		public override Type[] GetGenericArguments()
		{
			RuntimeTypeHandle[] instantiation = this.GetRootElementType().GetTypeHandleInternal().GetInstantiation();
			Type[] array;
			if (instantiation != null)
			{
				array = new Type[instantiation.Length];
				for (int i = 0; i < instantiation.Length; i++)
				{
					array[i] = instantiation[i].GetRuntimeType();
				}
			}
			else
			{
				array = new Type[0];
			}
			return array;
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00028CA4 File Offset: 0x00027CA4
		public override Type MakeGenericType(Type[] instantiation)
		{
			if (instantiation == null)
			{
				throw new ArgumentNullException("instantiation");
			}
			Type[] array = new Type[instantiation.Length];
			for (int i = 0; i < instantiation.Length; i++)
			{
				array[i] = instantiation[i];
			}
			instantiation = array;
			if (!this.IsGenericTypeDefinition)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_NotGenericTypeDefinition"), new object[]
				{
					this
				}));
			}
			for (int j = 0; j < instantiation.Length; j++)
			{
				if (instantiation[j] == null)
				{
					throw new ArgumentNullException();
				}
				if (!(instantiation[j] is RuntimeType))
				{
					return new TypeBuilderInstantiation(this, instantiation);
				}
			}
			Type[] genericArguments = this.GetGenericArguments();
			RuntimeType.SanityCheckGenericArguments(instantiation, genericArguments);
			RuntimeTypeHandle[] array2 = new RuntimeTypeHandle[instantiation.Length];
			for (int k = 0; k < instantiation.Length; k++)
			{
				array2[k] = instantiation[k].GetTypeHandleInternal();
			}
			Type result = null;
			try
			{
				result = this.m_handle.Instantiate(array2).GetRuntimeType();
			}
			catch (TypeLoadException ex)
			{
				RuntimeType.ValidateGenericArguments(this, instantiation, ex);
				throw ex;
			}
			return result;
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x00028DBC File Offset: 0x00027DBC
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return this.m_handle.IsGenericTypeDefinition();
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00028DC9 File Offset: 0x00027DC9
		public override bool IsGenericParameter
		{
			get
			{
				return this.m_handle.IsGenericVariable();
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x00028DD6 File Offset: 0x00027DD6
		public override int GenericParameterPosition
		{
			get
			{
				if (!this.IsGenericParameter)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
				}
				return this.m_handle.GetGenericVariableIndex();
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00028DFC File Offset: 0x00027DFC
		public override Type GetGenericTypeDefinition()
		{
			if (!this.IsGenericType)
			{
				throw new InvalidOperationException();
			}
			return this.m_handle.GetGenericTypeDefinition().GetRuntimeType();
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00028E2C File Offset: 0x00027E2C
		public override bool IsGenericType
		{
			get
			{
				return !base.HasElementType && this.GetTypeHandleInternal().HasInstantiation();
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00028E54 File Offset: 0x00027E54
		public override bool ContainsGenericParameters
		{
			get
			{
				return this.GetRootElementType().GetTypeHandleInternal().ContainsGenericVariables();
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00028E74 File Offset: 0x00027E74
		public override Type[] GetGenericParameterConstraints()
		{
			if (!this.IsGenericParameter)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
			}
			RuntimeTypeHandle[] constraints = this.m_handle.GetConstraints();
			Type[] array = new Type[constraints.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = constraints[i].GetRuntimeType();
			}
			return array;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00028ECC File Offset: 0x00027ECC
		public override Type MakePointerType()
		{
			return this.m_handle.MakePointer().GetRuntimeType();
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00028EEC File Offset: 0x00027EEC
		public override Type MakeByRefType()
		{
			return this.m_handle.MakeByRef().GetRuntimeType();
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00028F0C File Offset: 0x00027F0C
		public override Type MakeArrayType()
		{
			return this.m_handle.MakeSZArray().GetRuntimeType();
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00028F2C File Offset: 0x00027F2C
		public override Type MakeArrayType(int rank)
		{
			if (rank <= 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_handle.MakeArray(rank).GetRuntimeType();
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00028F57 File Offset: 0x00027F57
		public override StructLayoutAttribute StructLayoutAttribute
		{
			get
			{
				return (StructLayoutAttribute)StructLayoutAttribute.GetCustomAttribute(this);
			}
		}

		// Token: 0x06000E43 RID: 3651
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanValueSpecialCast(IntPtr valueType, IntPtr targetType);

		// Token: 0x06000E44 RID: 3652
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object AllocateObjectForByRef(RuntimeTypeHandle type, object value);

		// Token: 0x06000E45 RID: 3653
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ForceEnUSLcidComInvoking();

		// Token: 0x06000E46 RID: 3654 RVA: 0x00028F64 File Offset: 0x00027F64
		internal object CheckValue(object value, Binder binder, CultureInfo culture, BindingFlags invokeAttr)
		{
			if (this.IsInstanceOfType(value))
			{
				return value;
			}
			bool isByRef = base.IsByRef;
			if (isByRef)
			{
				Type elementType = this.GetElementType();
				if (elementType.IsInstanceOfType(value) || value == null)
				{
					return RuntimeType.AllocateObjectForByRef(elementType.TypeHandle, value);
				}
			}
			else
			{
				if (value == null)
				{
					return value;
				}
				if (this == RuntimeType.s_typedRef)
				{
					return value;
				}
			}
			bool flag = base.IsPointer || base.IsEnum || base.IsPrimitive;
			if (flag)
			{
				Pointer pointer = value as Pointer;
				Type type;
				if (pointer != null)
				{
					type = pointer.GetPointerType();
				}
				else
				{
					type = value.GetType();
				}
				if (RuntimeType.CanValueSpecialCast(type.TypeHandle.Value, this.TypeHandle.Value))
				{
					if (pointer != null)
					{
						return pointer.GetPointerValue();
					}
					return value;
				}
			}
			if ((invokeAttr & BindingFlags.ExactBinding) == BindingFlags.ExactBinding)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Arg_ObjObjEx"), new object[]
				{
					value.GetType(),
					this
				}));
			}
			if (binder != null && binder != Type.DefaultBinder)
			{
				value = binder.ChangeType(value, this, culture);
				if (this.IsInstanceOfType(value))
				{
					return value;
				}
				if (isByRef)
				{
					Type elementType2 = this.GetElementType();
					if (elementType2.IsInstanceOfType(value) || value == null)
					{
						return RuntimeType.AllocateObjectForByRef(elementType2.TypeHandle, value);
					}
				}
				else if (value == null)
				{
					return value;
				}
				if (flag)
				{
					Pointer pointer2 = value as Pointer;
					Type type2;
					if (pointer2 != null)
					{
						type2 = pointer2.GetPointerType();
					}
					else
					{
						type2 = value.GetType();
					}
					if (RuntimeType.CanValueSpecialCast(type2.TypeHandle.Value, this.TypeHandle.Value))
					{
						if (pointer2 != null)
						{
							return pointer2.GetPointerValue();
						}
						return value;
					}
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Arg_ObjObjEx"), new object[]
			{
				value.GetType(),
				this
			}));
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00029140 File Offset: 0x00028140
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object InvokeMember(string name, BindingFlags bindingFlags, Binder binder, object target, object[] providedArgs, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParams)
		{
			if (this.IsGenericParameter)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_GenericParameter"));
			}
			if ((bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty)) == BindingFlags.Default)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_NoAccessSpec"), "bindingFlags");
			}
			if ((bindingFlags & (BindingFlags)255) == BindingFlags.Default)
			{
				bindingFlags |= (BindingFlags.Instance | BindingFlags.Public);
				if ((bindingFlags & BindingFlags.CreateInstance) == BindingFlags.Default)
				{
					bindingFlags |= BindingFlags.Static;
				}
			}
			if (namedParams != null)
			{
				if (providedArgs != null)
				{
					if (namedParams.Length > providedArgs.Length)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_NamedParamTooBig"), "namedParams");
					}
				}
				else if (namedParams.Length != 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_NamedParamTooBig"), "namedParams");
				}
			}
			if (target != null && target.GetType().IsCOMObject)
			{
				if ((bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty)) == BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_COMAccess"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.GetProperty) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~(BindingFlags.InvokeMethod | BindingFlags.GetProperty)) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PropSetGet"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.InvokeMethod) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~(BindingFlags.InvokeMethod | BindingFlags.GetProperty)) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PropSetInvoke"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.SetProperty) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~BindingFlags.SetProperty) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_COMPropSetPut"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.PutDispProperty) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~BindingFlags.PutDispProperty) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_COMPropSetPut"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.PutRefDispProperty) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~BindingFlags.PutRefDispProperty) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_COMPropSetPut"), "bindingFlags");
				}
				if (RemotingServices.IsTransparentProxy(target))
				{
					return ((MarshalByRefObject)target).InvokeMember(name, bindingFlags, binder, providedArgs, modifiers, culture, namedParams);
				}
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				bool[] byrefModifiers = (modifiers == null) ? null : modifiers[0].IsByRefArray;
				int culture2;
				if (culture == null)
				{
					culture2 = (RuntimeType.forceInvokingWithEnUS ? 1033 : Thread.CurrentThread.CurrentCulture.LCID);
				}
				else
				{
					culture2 = culture.LCID;
				}
				return this.InvokeDispMethod(name, bindingFlags, target, providedArgs, byrefModifiers, culture2, namedParams);
			}
			else
			{
				if (namedParams != null && Array.IndexOf<string>(namedParams, null) != -1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_NamedParamNull"), "namedParams");
				}
				int num = (providedArgs != null) ? providedArgs.Length : 0;
				if (binder == null)
				{
					binder = Type.DefaultBinder;
				}
				Binder defaultBinder = Type.DefaultBinder;
				if ((bindingFlags & BindingFlags.CreateInstance) != BindingFlags.Default)
				{
					if ((bindingFlags & BindingFlags.CreateInstance) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty)) != BindingFlags.Default)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_CreatInstAccess"), "bindingFlags");
					}
					return Activator.CreateInstance(this, bindingFlags, binder, providedArgs, culture);
				}
				else
				{
					if ((bindingFlags & (BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty)) != BindingFlags.Default)
					{
						bindingFlags |= BindingFlags.SetProperty;
					}
					if (name == null)
					{
						throw new ArgumentNullException("name");
					}
					if (name.Length == 0 || name.Equals("[DISPID=0]"))
					{
						name = this.GetDefaultMemberName();
						if (name == null)
						{
							name = "ToString";
						}
					}
					bool flag = (bindingFlags & BindingFlags.GetField) != BindingFlags.Default;
					bool flag2 = (bindingFlags & BindingFlags.SetField) != BindingFlags.Default;
					if (flag || flag2)
					{
						if (flag)
						{
							if (flag2)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_FldSetGet"), "bindingFlags");
							}
							if ((bindingFlags & BindingFlags.SetProperty) != BindingFlags.Default)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_FldGetPropSet"), "bindingFlags");
							}
						}
						else
						{
							if (providedArgs == null)
							{
								throw new ArgumentNullException("providedArgs");
							}
							if ((bindingFlags & BindingFlags.GetProperty) != BindingFlags.Default)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_FldSetPropGet"), "bindingFlags");
							}
							if ((bindingFlags & BindingFlags.InvokeMethod) != BindingFlags.Default)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_FldSetInvoke"), "bindingFlags");
							}
						}
						FieldInfo fieldInfo = null;
						FieldInfo[] array = this.GetMember(name, MemberTypes.Field, bindingFlags) as FieldInfo[];
						if (array.Length == 1)
						{
							fieldInfo = array[0];
						}
						else if (array.Length > 0)
						{
							fieldInfo = binder.BindToField(bindingFlags, array, flag ? Empty.Value : providedArgs[0], culture);
						}
						if (fieldInfo != null)
						{
							if (fieldInfo.FieldType.IsArray || fieldInfo.FieldType == typeof(Array))
							{
								int num2;
								if ((bindingFlags & BindingFlags.GetField) != BindingFlags.Default)
								{
									num2 = num;
								}
								else
								{
									num2 = num - 1;
								}
								if (num2 > 0)
								{
									int[] array2 = new int[num2];
									for (int i = 0; i < num2; i++)
									{
										try
										{
											array2[i] = ((IConvertible)providedArgs[i]).ToInt32(null);
										}
										catch (InvalidCastException)
										{
											throw new ArgumentException(Environment.GetResourceString("Arg_IndexMustBeInt"));
										}
									}
									Array array3 = (Array)fieldInfo.GetValue(target);
									if ((bindingFlags & BindingFlags.GetField) != BindingFlags.Default)
									{
										return array3.GetValue(array2);
									}
									array3.SetValue(providedArgs[num2], array2);
									return null;
								}
							}
							if (flag)
							{
								if (num != 0)
								{
									throw new ArgumentException(Environment.GetResourceString("Arg_FldGetArgErr"), "bindingFlags");
								}
								return fieldInfo.GetValue(target);
							}
							else
							{
								if (num != 1)
								{
									throw new ArgumentException(Environment.GetResourceString("Arg_FldSetArgErr"), "bindingFlags");
								}
								fieldInfo.SetValue(target, providedArgs[0], bindingFlags, binder, culture);
								return null;
							}
						}
						else if ((bindingFlags & (BindingFlags)16773888) == BindingFlags.Default)
						{
							throw new MissingFieldException(this.FullName, name);
						}
					}
					bool flag3 = (bindingFlags & BindingFlags.GetProperty) != BindingFlags.Default;
					bool flag4 = (bindingFlags & BindingFlags.SetProperty) != BindingFlags.Default;
					if (flag3 || flag4)
					{
						if (flag3)
						{
							if (flag4)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PropSetGet"), "bindingFlags");
							}
						}
						else if ((bindingFlags & BindingFlags.InvokeMethod) != BindingFlags.Default)
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PropSetInvoke"), "bindingFlags");
						}
					}
					MethodInfo[] array4 = null;
					MethodInfo methodInfo = null;
					if ((bindingFlags & BindingFlags.InvokeMethod) != BindingFlags.Default)
					{
						MethodInfo[] array5 = this.GetMember(name, MemberTypes.Method, bindingFlags) as MethodInfo[];
						ArrayList arrayList = null;
						foreach (MethodInfo methodInfo2 in array5)
						{
							if (RuntimeType.FilterApplyMethodBaseInfo(methodInfo2, bindingFlags, null, CallingConventions.Any, new Type[num], false))
							{
								if (methodInfo == null)
								{
									methodInfo = methodInfo2;
								}
								else
								{
									if (arrayList == null)
									{
										arrayList = new ArrayList(array5.Length);
										arrayList.Add(methodInfo);
									}
									arrayList.Add(methodInfo2);
								}
							}
						}
						if (arrayList != null)
						{
							array4 = new MethodInfo[arrayList.Count];
							arrayList.CopyTo(array4);
						}
					}
					if ((methodInfo == null && flag3) || flag4)
					{
						PropertyInfo[] array6 = this.GetMember(name, MemberTypes.Property, bindingFlags) as PropertyInfo[];
						ArrayList arrayList2 = null;
						for (int k = 0; k < array6.Length; k++)
						{
							MethodInfo methodInfo3;
							if (flag4)
							{
								methodInfo3 = array6[k].GetSetMethod(true);
							}
							else
							{
								methodInfo3 = array6[k].GetGetMethod(true);
							}
							if (methodInfo3 != null && RuntimeType.FilterApplyMethodBaseInfo(methodInfo3, bindingFlags, null, CallingConventions.Any, new Type[num], false))
							{
								if (methodInfo == null)
								{
									methodInfo = methodInfo3;
								}
								else
								{
									if (arrayList2 == null)
									{
										arrayList2 = new ArrayList(array6.Length);
										arrayList2.Add(methodInfo);
									}
									arrayList2.Add(methodInfo3);
								}
							}
						}
						if (arrayList2 != null)
						{
							array4 = new MethodInfo[arrayList2.Count];
							arrayList2.CopyTo(array4);
						}
					}
					if (methodInfo == null)
					{
						throw new MissingMethodException(this.FullName, name);
					}
					if (array4 == null && num == 0 && methodInfo.GetParametersNoCopy().Length == 0 && (bindingFlags & BindingFlags.OptionalParamBinding) == BindingFlags.Default)
					{
						return methodInfo.Invoke(target, bindingFlags, binder, providedArgs, culture);
					}
					if (array4 == null)
					{
						array4 = new MethodInfo[]
						{
							methodInfo
						};
					}
					if (providedArgs == null)
					{
						providedArgs = new object[0];
					}
					object obj = null;
					MethodBase methodBase = null;
					try
					{
						methodBase = binder.BindToMethod(bindingFlags, array4, ref providedArgs, modifiers, culture, namedParams, out obj);
					}
					catch (MissingMethodException)
					{
					}
					if (methodBase == null)
					{
						throw new MissingMethodException(this.FullName, name);
					}
					object result = ((MethodInfo)methodBase).Invoke(target, bindingFlags, binder, providedArgs, culture);
					if (obj != null)
					{
						binder.ReorderArgumentArray(ref providedArgs, obj);
					}
					return result;
				}
			}
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x000298D4 File Offset: 0x000288D4
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x000298DC File Offset: 0x000288DC
		public override int GetHashCode()
		{
			long num = (long)this.GetTypeHandleInternal().Value;
			return (int)num;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000298FF File Offset: 0x000288FF
		public override string ToString()
		{
			return this.Cache.GetToString();
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0002990C File Offset: 0x0002890C
		public object Clone()
		{
			return this;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0002990F File Offset: 0x0002890F
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, this);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00029926 File Offset: 0x00028926
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType, inherit);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00029940 File Offset: 0x00028940
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType, inherit);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00029988 File Offset: 0x00028988
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.IsDefined(this, runtimeType, inherit);
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x000299CF File Offset: 0x000289CF
		public override string Name
		{
			get
			{
				return this.Cache.GetName();
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x000299DC File Offset: 0x000289DC
		public override MemberTypes MemberType
		{
			get
			{
				if (base.IsPublic || base.IsNotPublic)
				{
					return MemberTypes.TypeInfo;
				}
				return MemberTypes.NestedType;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x000299F6 File Offset: 0x000289F6
		public override Type DeclaringType
		{
			get
			{
				return this.Cache.GetEnclosingType();
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x00029A03 File Offset: 0x00028A03
		public override Type ReflectedType
		{
			get
			{
				return this.DeclaringType;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00029A0B File Offset: 0x00028A0B
		public override int MetadataToken
		{
			get
			{
				return this.m_handle.GetToken();
			}
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00029A18 File Offset: 0x00028A18
		internal void CreateInstanceCheckThis()
		{
			if (this is ReflectionOnlyType)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ReflectionOnlyInvoke"));
			}
			if (this.ContainsGenericParameters)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Acc_CreateGenericEx"), new object[]
				{
					this
				}));
			}
			Type rootElementType = this.GetRootElementType();
			if (rootElementType == typeof(ArgIterator))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Acc_CreateArgIterator"), new object[0]));
			}
			if (rootElementType == typeof(void))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Acc_CreateVoid"), new object[0]));
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00029ACC File Offset: 0x00028ACC
		internal object CreateInstanceImpl(BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			this.CreateInstanceCheckThis();
			object result = null;
			try
			{
				try
				{
					if (activationAttributes != null)
					{
						ActivationServices.PushActivationAttributes(this, activationAttributes);
					}
					if (args == null)
					{
						args = new object[0];
					}
					int num = args.Length;
					if (binder == null)
					{
						binder = Type.DefaultBinder;
					}
					if (num == 0 && (bindingAttr & BindingFlags.Public) != BindingFlags.Default && (bindingAttr & BindingFlags.Instance) != BindingFlags.Default && (this.IsGenericCOMObjectImpl() || this.IsSubclassOf(typeof(ValueType))))
					{
						result = this.CreateInstanceImpl((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default);
					}
					else
					{
						MethodBase[] constructors = this.GetConstructors(bindingAttr);
						ArrayList arrayList = new ArrayList(constructors.Length);
						Type[] array = new Type[num];
						for (int i = 0; i < num; i++)
						{
							if (args[i] != null)
							{
								array[i] = args[i].GetType();
							}
						}
						for (int j = 0; j < constructors.Length; j++)
						{
							MethodBase methodBase = constructors[j];
							if (RuntimeType.FilterApplyMethodBaseInfo(constructors[j], bindingAttr, null, CallingConventions.Any, array, false))
							{
								arrayList.Add(constructors[j]);
							}
						}
						MethodBase[] array2 = new MethodBase[arrayList.Count];
						arrayList.CopyTo(array2);
						if (array2 != null && array2.Length == 0)
						{
							array2 = null;
						}
						if (array2 == null)
						{
							if (activationAttributes != null)
							{
								ActivationServices.PopActivationAttributes(this);
								activationAttributes = null;
							}
							throw new MissingMethodException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingConstructor_Name"), new object[]
							{
								this.FullName
							}));
						}
						if (num == 0 && array2.Length == 1 && (bindingAttr & BindingFlags.OptionalParamBinding) == BindingFlags.Default)
						{
							result = Activator.CreateInstance(this, true);
						}
						else
						{
							object obj = null;
							MethodBase methodBase2;
							try
							{
								methodBase2 = binder.BindToMethod(bindingAttr, array2, ref args, null, culture, null, out obj);
							}
							catch (MissingMethodException)
							{
								methodBase2 = null;
							}
							if (methodBase2 == null)
							{
								if (activationAttributes != null)
								{
									ActivationServices.PopActivationAttributes(this);
									activationAttributes = null;
								}
								throw new MissingMethodException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingConstructor_Name"), new object[]
								{
									this.FullName
								}));
							}
							if (typeof(Delegate).IsAssignableFrom(methodBase2.DeclaringType))
							{
								new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
							}
							result = ((ConstructorInfo)methodBase2).Invoke(bindingAttr, binder, args, culture);
							if (obj != null)
							{
								binder.ReorderArgumentArray(ref args, obj);
							}
						}
					}
				}
				finally
				{
					if (activationAttributes != null)
					{
						ActivationServices.PopActivationAttributes(this);
						activationAttributes = null;
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return result;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00029D38 File Offset: 0x00028D38
		private object CreateInstanceSlow(bool publicOnly, bool fillCache)
		{
			RuntimeMethodHandle emptyHandle = RuntimeMethodHandle.EmptyHandle;
			bool bNeedSecurityCheck = true;
			bool flag = false;
			bool noCheck = false;
			this.CreateInstanceCheckThis();
			if (!fillCache)
			{
				noCheck = true;
			}
			object result = RuntimeTypeHandle.CreateInstance(this, publicOnly, noCheck, ref flag, ref emptyHandle, ref bNeedSecurityCheck);
			if (flag && fillCache)
			{
				RuntimeType.ActivatorCache activatorCache = RuntimeType.s_ActivatorCache;
				if (activatorCache == null)
				{
					activatorCache = new RuntimeType.ActivatorCache();
					Thread.MemoryBarrier();
					RuntimeType.s_ActivatorCache = activatorCache;
				}
				RuntimeType.ActivatorCacheEntry entry = new RuntimeType.ActivatorCacheEntry(this, emptyHandle, bNeedSecurityCheck);
				Thread.MemoryBarrier();
				activatorCache.SetEntry(entry);
			}
			return result;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00029DAA File Offset: 0x00028DAA
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal object CreateInstanceImpl(bool publicOnly)
		{
			return this.CreateInstanceImpl(publicOnly, false, true);
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00029DB8 File Offset: 0x00028DB8
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal object CreateInstanceImpl(bool publicOnly, bool skipVisibilityChecks, bool fillCache)
		{
			RuntimeTypeHandle typeHandle = this.TypeHandle;
			RuntimeType.ActivatorCache activatorCache = RuntimeType.s_ActivatorCache;
			if (activatorCache != null)
			{
				RuntimeType.ActivatorCacheEntry entry = activatorCache.GetEntry(this);
				if (entry != null)
				{
					if (publicOnly && entry.m_ctor != null && (entry.m_hCtorMethodHandle.GetAttributes() & MethodAttributes.MemberAccessMask) != MethodAttributes.Public)
					{
						throw new MissingMethodException(Environment.GetResourceString("Arg_NoDefCTor"));
					}
					object obj = typeHandle.Allocate();
					if (entry.m_ctor != null)
					{
						if (!skipVisibilityChecks && entry.m_bNeedSecurityCheck)
						{
							MethodBase.PerformSecurityCheck(obj, entry.m_hCtorMethodHandle, this.TypeHandle.Value, 268435456U);
						}
						try
						{
							entry.m_ctor(obj);
						}
						catch (Exception inner)
						{
							throw new TargetInvocationException(inner);
						}
					}
					return obj;
				}
			}
			return this.CreateInstanceSlow(publicOnly, fillCache);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00029E80 File Offset: 0x00028E80
		internal bool SupportsInterface(object o)
		{
			return this.TypeHandle.SupportsInterface(o);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00029E9C File Offset: 0x00028E9C
		internal void InvalidateCachedNestedType()
		{
			this.Cache.InvalidateCachedNestedType();
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00029EA9 File Offset: 0x00028EA9
		internal bool IsGenericCOMObjectImpl()
		{
			return this.m_handle.IsComObject(true);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00029EB8 File Offset: 0x00028EB8
		internal static bool CanCastTo(RuntimeType fromType, RuntimeType toType)
		{
			return fromType.GetTypeHandleInternal().CanCastTo(toType.GetTypeHandleInternal());
		}

		// Token: 0x06000E5E RID: 3678
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object _CreateEnum(IntPtr enumType, long value);

		// Token: 0x06000E5F RID: 3679 RVA: 0x00029ED9 File Offset: 0x00028ED9
		internal static object CreateEnum(RuntimeTypeHandle enumType, long value)
		{
			return RuntimeType._CreateEnum(enumType.Value, value);
		}

		// Token: 0x06000E60 RID: 3680
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object InvokeDispMethod(string name, BindingFlags invokeAttr, object target, object[] args, bool[] byrefModifiers, int culture, string[] namedParameters);

		// Token: 0x06000E61 RID: 3681
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Type GetTypeFromProgIDImpl(string progID, string server, bool throwOnError);

		// Token: 0x06000E62 RID: 3682
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Type GetTypeFromCLSIDImpl(Guid clsid, string server, bool throwOnError);

		// Token: 0x06000E63 RID: 3683 RVA: 0x00029EE8 File Offset: 0x00028EE8
		internal static Type PrivateGetType(string typeName, bool throwOnError, bool ignoreCase, ref StackCrawlMark stackMark)
		{
			return RuntimeType.PrivateGetType(typeName, throwOnError, ignoreCase, false, ref stackMark);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00029EF4 File Offset: 0x00028EF4
		internal static Type PrivateGetType(string typeName, bool throwOnError, bool ignoreCase, bool reflectionOnly, ref StackCrawlMark stackMark)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("TypeName");
			}
			return RuntimeTypeHandle.GetTypeByName(typeName, throwOnError, ignoreCase, reflectionOnly, ref stackMark).GetRuntimeType();
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00029F24 File Offset: 0x00028F24
		private object ForwardCallToInvokeMember(string memberName, BindingFlags flags, object target, int[] aWrapperTypes, ref MessageData msgData)
		{
			ParameterModifier[] array = null;
			object obj = null;
			Message message = new Message();
			message.InitFields(msgData);
			MethodInfo methodInfo = (MethodInfo)message.GetMethodBase();
			object[] args = message.Args;
			int num = args.Length;
			ParameterInfo[] parametersNoCopy = methodInfo.GetParametersNoCopy();
			if (num > 0)
			{
				ParameterModifier parameterModifier = new ParameterModifier(num);
				for (int i = 0; i < num; i++)
				{
					if (parametersNoCopy[i].ParameterType.IsByRef)
					{
						parameterModifier[i] = true;
					}
				}
				array = new ParameterModifier[]
				{
					parameterModifier
				};
				if (aWrapperTypes != null)
				{
					this.WrapArgsForInvokeCall(args, aWrapperTypes);
				}
			}
			if (methodInfo.ReturnType == typeof(void))
			{
				flags |= BindingFlags.IgnoreReturn;
			}
			try
			{
				obj = this.InvokeMember(memberName, flags, null, target, args, array, null, null);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			for (int j = 0; j < num; j++)
			{
				if (array[0][j] && args[j] != null)
				{
					Type elementType = parametersNoCopy[j].ParameterType.GetElementType();
					if (elementType != args[j].GetType())
					{
						args[j] = this.ForwardCallBinder.ChangeType(args[j], elementType, null);
					}
				}
			}
			if (obj != null)
			{
				Type returnType = methodInfo.ReturnType;
				if (returnType != obj.GetType())
				{
					obj = this.ForwardCallBinder.ChangeType(obj, returnType, null);
				}
			}
			RealProxy.PropagateOutParameters(message, args, obj);
			return obj;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0002A09C File Offset: 0x0002909C
		private void WrapArgsForInvokeCall(object[] aArgs, int[] aWrapperTypes)
		{
			int num = aArgs.Length;
			for (int i = 0; i < num; i++)
			{
				if (aWrapperTypes[i] != 0)
				{
					if ((aWrapperTypes[i] & 65536) != 0)
					{
						Type type = null;
						bool flag = false;
						RuntimeType.DispatchWrapperType dispatchWrapperType = (RuntimeType.DispatchWrapperType)(aWrapperTypes[i] & -65537);
						if (dispatchWrapperType <= RuntimeType.DispatchWrapperType.Error)
						{
							switch (dispatchWrapperType)
							{
							case RuntimeType.DispatchWrapperType.Unknown:
								type = typeof(UnknownWrapper);
								break;
							case RuntimeType.DispatchWrapperType.Dispatch:
								type = typeof(DispatchWrapper);
								break;
							default:
								if (dispatchWrapperType == RuntimeType.DispatchWrapperType.Error)
								{
									type = typeof(ErrorWrapper);
								}
								break;
							}
						}
						else if (dispatchWrapperType != RuntimeType.DispatchWrapperType.Currency)
						{
							if (dispatchWrapperType == RuntimeType.DispatchWrapperType.BStr)
							{
								type = typeof(BStrWrapper);
								flag = true;
							}
						}
						else
						{
							type = typeof(CurrencyWrapper);
						}
						Array array = (Array)aArgs[i];
						int length = array.Length;
						object[] array2 = (object[])Array.CreateInstance(type, length);
						ConstructorInfo constructor;
						if (flag)
						{
							constructor = type.GetConstructor(new Type[]
							{
								typeof(string)
							});
						}
						else
						{
							constructor = type.GetConstructor(new Type[]
							{
								typeof(object)
							});
						}
						for (int j = 0; j < length; j++)
						{
							if (flag)
							{
								array2[j] = constructor.Invoke(new object[]
								{
									(string)array.GetValue(j)
								});
							}
							else
							{
								array2[j] = constructor.Invoke(new object[]
								{
									array.GetValue(j)
								});
							}
						}
						aArgs[i] = array2;
					}
					else
					{
						RuntimeType.DispatchWrapperType dispatchWrapperType2 = (RuntimeType.DispatchWrapperType)aWrapperTypes[i];
						if (dispatchWrapperType2 <= RuntimeType.DispatchWrapperType.Error)
						{
							switch (dispatchWrapperType2)
							{
							case RuntimeType.DispatchWrapperType.Unknown:
								aArgs[i] = new UnknownWrapper(aArgs[i]);
								break;
							case RuntimeType.DispatchWrapperType.Dispatch:
								aArgs[i] = new DispatchWrapper(aArgs[i]);
								break;
							default:
								if (dispatchWrapperType2 == RuntimeType.DispatchWrapperType.Error)
								{
									aArgs[i] = new ErrorWrapper(aArgs[i]);
								}
								break;
							}
						}
						else if (dispatchWrapperType2 != RuntimeType.DispatchWrapperType.Currency)
						{
							if (dispatchWrapperType2 == RuntimeType.DispatchWrapperType.BStr)
							{
								aArgs[i] = new BStrWrapper((string)aArgs[i]);
							}
						}
						else
						{
							aArgs[i] = new CurrencyWrapper(aArgs[i]);
						}
					}
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0002A293 File Offset: 0x00029293
		private OleAutBinder ForwardCallBinder
		{
			get
			{
				if (RuntimeType.s_ForwardCallBinder == null)
				{
					RuntimeType.s_ForwardCallBinder = new OleAutBinder();
				}
				return RuntimeType.s_ForwardCallBinder;
			}
		}

		// Token: 0x040004E1 RID: 1249
		private const BindingFlags MemberBindingMask = (BindingFlags)255;

		// Token: 0x040004E2 RID: 1250
		private const BindingFlags InvocationMask = BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty;

		// Token: 0x040004E3 RID: 1251
		private const BindingFlags BinderNonCreateInstance = BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x040004E4 RID: 1252
		private const BindingFlags BinderGetSetProperty = BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x040004E5 RID: 1253
		private const BindingFlags BinderSetInvokeProperty = BindingFlags.InvokeMethod | BindingFlags.SetProperty;

		// Token: 0x040004E6 RID: 1254
		private const BindingFlags BinderGetSetField = BindingFlags.GetField | BindingFlags.SetField;

		// Token: 0x040004E7 RID: 1255
		private const BindingFlags BinderSetInvokeField = BindingFlags.InvokeMethod | BindingFlags.SetField;

		// Token: 0x040004E8 RID: 1256
		private const BindingFlags BinderNonFieldGetSet = (BindingFlags)16773888;

		// Token: 0x040004E9 RID: 1257
		private const BindingFlags ClassicBindingMask = BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty;

		// Token: 0x040004EA RID: 1258
		private IntPtr m_cache;

		// Token: 0x040004EB RID: 1259
		private RuntimeTypeHandle m_handle;

		// Token: 0x040004EC RID: 1260
		private static RuntimeType.TypeCacheQueue s_typeCache = null;

		// Token: 0x040004ED RID: 1261
		private static Type s_typedRef = typeof(TypedReference);

		// Token: 0x040004EE RID: 1262
		private static bool forceInvokingWithEnUS = RuntimeType.ForceEnUSLcidComInvoking();

		// Token: 0x040004EF RID: 1263
		private static RuntimeType.ActivatorCache s_ActivatorCache;

		// Token: 0x040004F0 RID: 1264
		private static OleAutBinder s_ForwardCallBinder;

		// Token: 0x020000F9 RID: 249
		[Serializable]
		internal class RuntimeTypeCache
		{
			// Token: 0x06000E69 RID: 3689 RVA: 0x0002A2CC File Offset: 0x000292CC
			internal static void Prejitinit_HACK()
			{
				if (!RuntimeType.RuntimeTypeCache.s_dontrunhack)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeMethodInfo> memberInfoCache = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeMethodInfo>(null);
						CerArrayList<RuntimeMethodInfo> cerArrayList = null;
						memberInfoCache.Insert(ref cerArrayList, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeConstructorInfo> memberInfoCache2 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeConstructorInfo>(null);
						CerArrayList<RuntimeConstructorInfo> cerArrayList2 = null;
						memberInfoCache2.Insert(ref cerArrayList2, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeFieldInfo> memberInfoCache3 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeFieldInfo>(null);
						CerArrayList<RuntimeFieldInfo> cerArrayList3 = null;
						memberInfoCache3.Insert(ref cerArrayList3, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeType> memberInfoCache4 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeType>(null);
						CerArrayList<RuntimeType> cerArrayList4 = null;
						memberInfoCache4.Insert(ref cerArrayList4, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimePropertyInfo> memberInfoCache5 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimePropertyInfo>(null);
						CerArrayList<RuntimePropertyInfo> cerArrayList5 = null;
						memberInfoCache5.Insert(ref cerArrayList5, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeEventInfo> memberInfoCache6 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeEventInfo>(null);
						CerArrayList<RuntimeEventInfo> cerArrayList6 = null;
						memberInfoCache6.Insert(ref cerArrayList6, "dummy", MemberListType.All);
					}
				}
			}

			// Token: 0x06000E6A RID: 3690 RVA: 0x0002A394 File Offset: 0x00029394
			internal RuntimeTypeCache(RuntimeType runtimeType)
			{
				this.m_typeCode = TypeCode.Empty;
				this.m_runtimeType = runtimeType;
				this.m_runtimeTypeHandle = runtimeType.GetTypeHandleInternal();
				this.m_isGlobal = this.m_runtimeTypeHandle.GetModuleHandle().GetModuleTypeHandle().Equals(this.m_runtimeTypeHandle);
				RuntimeType.RuntimeTypeCache.s_dontrunhack = true;
				RuntimeType.RuntimeTypeCache.Prejitinit_HACK();
			}

			// Token: 0x06000E6B RID: 3691 RVA: 0x0002A3F4 File Offset: 0x000293F4
			private string ConstructName(ref string name, bool nameSpace, bool fullinst, bool assembly)
			{
				if (name == null)
				{
					name = this.RuntimeTypeHandle.ConstructName(nameSpace, fullinst, assembly);
				}
				return name;
			}

			// Token: 0x06000E6C RID: 3692 RVA: 0x0002A41C File Offset: 0x0002941C
			private CerArrayList<T> GetMemberList<T>(ref RuntimeType.RuntimeTypeCache.MemberInfoCache<T> m_cache, MemberListType listType, string name, RuntimeType.RuntimeTypeCache.CacheType cacheType) where T : MemberInfo
			{
				RuntimeType.RuntimeTypeCache.MemberInfoCache<T> memberCache = this.GetMemberCache<T>(ref m_cache);
				return memberCache.GetMemberList(listType, name, cacheType);
			}

			// Token: 0x06000E6D RID: 3693 RVA: 0x0002A43C File Offset: 0x0002943C
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<T> GetMemberCache<T>(ref RuntimeType.RuntimeTypeCache.MemberInfoCache<T> m_cache) where T : MemberInfo
			{
				RuntimeType.RuntimeTypeCache.MemberInfoCache<T> memberInfoCache = m_cache;
				if (memberInfoCache == null)
				{
					RuntimeType.RuntimeTypeCache.MemberInfoCache<T> memberInfoCache2 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<T>(this);
					memberInfoCache = Interlocked.CompareExchange<RuntimeType.RuntimeTypeCache.MemberInfoCache<T>>(ref m_cache, memberInfoCache2, null);
					if (memberInfoCache == null)
					{
						memberInfoCache = memberInfoCache2;
					}
				}
				return memberInfoCache;
			}

			// Token: 0x170001CC RID: 460
			// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0002A465 File Offset: 0x00029465
			// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0002A46D File Offset: 0x0002946D
			internal bool DomainInitialized
			{
				get
				{
					return this.m_bIsDomainInitialized;
				}
				set
				{
					this.m_bIsDomainInitialized = value;
				}
			}

			// Token: 0x06000E70 RID: 3696 RVA: 0x0002A476 File Offset: 0x00029476
			internal string GetName()
			{
				return this.ConstructName(ref this.m_name, false, false, false);
			}

			// Token: 0x06000E71 RID: 3697 RVA: 0x0002A488 File Offset: 0x00029488
			internal string GetNameSpace()
			{
				if (this.m_namespace == null)
				{
					Type type = this.m_runtimeType;
					type = type.GetRootElementType();
					while (type.IsNested)
					{
						type = type.DeclaringType;
					}
					this.m_namespace = type.GetTypeHandleInternal().GetModuleHandle().GetMetadataImport().GetNamespace(type.MetadataToken).ToString();
				}
				return this.m_namespace;
			}

			// Token: 0x06000E72 RID: 3698 RVA: 0x0002A4FB File Offset: 0x000294FB
			internal string GetToString()
			{
				return this.ConstructName(ref this.m_toString, true, false, false);
			}

			// Token: 0x06000E73 RID: 3699 RVA: 0x0002A50C File Offset: 0x0002950C
			internal string GetFullName()
			{
				if (!this.m_runtimeType.IsGenericTypeDefinition && this.m_runtimeType.ContainsGenericParameters)
				{
					return null;
				}
				return this.ConstructName(ref this.m_fullname, true, true, false);
			}

			// Token: 0x170001CD RID: 461
			// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0002A539 File Offset: 0x00029539
			// (set) Token: 0x06000E75 RID: 3701 RVA: 0x0002A541 File Offset: 0x00029541
			internal TypeCode TypeCode
			{
				get
				{
					return this.m_typeCode;
				}
				set
				{
					this.m_typeCode = value;
				}
			}

			// Token: 0x06000E76 RID: 3702 RVA: 0x0002A54C File Offset: 0x0002954C
			internal RuntimeType GetEnclosingType()
			{
				if ((this.m_whatsCached & RuntimeType.RuntimeTypeCache.WhatsCached.EnclosingType) == RuntimeType.RuntimeTypeCache.WhatsCached.Nothing)
				{
					this.m_enclosingType = this.RuntimeTypeHandle.GetDeclaringType().GetRuntimeType();
					this.m_whatsCached |= RuntimeType.RuntimeTypeCache.WhatsCached.EnclosingType;
				}
				return this.m_enclosingType;
			}

			// Token: 0x170001CE RID: 462
			// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0002A593 File Offset: 0x00029593
			internal bool IsGlobal
			{
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				get
				{
					return this.m_isGlobal;
				}
			}

			// Token: 0x170001CF RID: 463
			// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0002A59B File Offset: 0x0002959B
			internal RuntimeType RuntimeType
			{
				get
				{
					return this.m_runtimeType;
				}
			}

			// Token: 0x170001D0 RID: 464
			// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0002A5A3 File Offset: 0x000295A3
			internal RuntimeTypeHandle RuntimeTypeHandle
			{
				get
				{
					return this.m_runtimeTypeHandle;
				}
			}

			// Token: 0x06000E7A RID: 3706 RVA: 0x0002A5AB File Offset: 0x000295AB
			internal void InvalidateCachedNestedType()
			{
				this.m_nestedClassesCache = null;
			}

			// Token: 0x06000E7B RID: 3707 RVA: 0x0002A5B4 File Offset: 0x000295B4
			internal MethodInfo GetGenericMethodInfo(RuntimeMethodHandle genericMethod)
			{
				if (RuntimeType.RuntimeTypeCache.s_methodInstantiations == null)
				{
					Interlocked.CompareExchange<CerHashtable<RuntimeMethodInfo, RuntimeMethodInfo>>(ref RuntimeType.RuntimeTypeCache.s_methodInstantiations, new CerHashtable<RuntimeMethodInfo, RuntimeMethodInfo>(), null);
				}
				RuntimeMethodInfo runtimeMethodInfo = new RuntimeMethodInfo(genericMethod, genericMethod.GetDeclaringType(), this, genericMethod.GetAttributes(), (BindingFlags)(-1));
				RuntimeMethodInfo runtimeMethodInfo2 = RuntimeType.RuntimeTypeCache.s_methodInstantiations[runtimeMethodInfo];
				if (runtimeMethodInfo2 != null)
				{
					return runtimeMethodInfo2;
				}
				bool flag = false;
				bool flag2 = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(RuntimeType.RuntimeTypeCache.s_methodInstantiations, ref flag);
					runtimeMethodInfo2 = RuntimeType.RuntimeTypeCache.s_methodInstantiations[runtimeMethodInfo];
					if (runtimeMethodInfo2 != null)
					{
						return runtimeMethodInfo2;
					}
					RuntimeType.RuntimeTypeCache.s_methodInstantiations.Preallocate(1);
					flag2 = true;
				}
				finally
				{
					if (flag2)
					{
						RuntimeType.RuntimeTypeCache.s_methodInstantiations[runtimeMethodInfo] = runtimeMethodInfo;
					}
					if (flag)
					{
						Monitor.Exit(RuntimeType.RuntimeTypeCache.s_methodInstantiations);
					}
				}
				return runtimeMethodInfo;
			}

			// Token: 0x06000E7C RID: 3708 RVA: 0x0002A66C File Offset: 0x0002966C
			internal CerArrayList<RuntimeMethodInfo> GetMethodList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeMethodInfo>(ref this.m_methodInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Method);
			}

			// Token: 0x06000E7D RID: 3709 RVA: 0x0002A67D File Offset: 0x0002967D
			internal CerArrayList<RuntimeConstructorInfo> GetConstructorList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeConstructorInfo>(ref this.m_constructorInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Constructor);
			}

			// Token: 0x06000E7E RID: 3710 RVA: 0x0002A68E File Offset: 0x0002968E
			internal CerArrayList<RuntimePropertyInfo> GetPropertyList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimePropertyInfo>(ref this.m_propertyInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Property);
			}

			// Token: 0x06000E7F RID: 3711 RVA: 0x0002A69F File Offset: 0x0002969F
			internal CerArrayList<RuntimeEventInfo> GetEventList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeEventInfo>(ref this.m_eventInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Event);
			}

			// Token: 0x06000E80 RID: 3712 RVA: 0x0002A6B0 File Offset: 0x000296B0
			internal CerArrayList<RuntimeFieldInfo> GetFieldList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeFieldInfo>(ref this.m_fieldInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Field);
			}

			// Token: 0x06000E81 RID: 3713 RVA: 0x0002A6C1 File Offset: 0x000296C1
			internal CerArrayList<RuntimeType> GetInterfaceList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeType>(ref this.m_interfaceCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Interface);
			}

			// Token: 0x06000E82 RID: 3714 RVA: 0x0002A6D2 File Offset: 0x000296D2
			internal CerArrayList<RuntimeType> GetNestedTypeList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeType>(ref this.m_nestedClassesCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.NestedType);
			}

			// Token: 0x06000E83 RID: 3715 RVA: 0x0002A6E3 File Offset: 0x000296E3
			internal MethodBase GetMethod(RuntimeTypeHandle declaringType, RuntimeMethodHandle method)
			{
				this.GetMemberCache<RuntimeMethodInfo>(ref this.m_methodInfoCache);
				return this.m_methodInfoCache.AddMethod(declaringType, method, RuntimeType.RuntimeTypeCache.CacheType.Method);
			}

			// Token: 0x06000E84 RID: 3716 RVA: 0x0002A700 File Offset: 0x00029700
			internal MethodBase GetConstructor(RuntimeTypeHandle declaringType, RuntimeMethodHandle constructor)
			{
				this.GetMemberCache<RuntimeConstructorInfo>(ref this.m_constructorInfoCache);
				return this.m_constructorInfoCache.AddMethod(declaringType, constructor, RuntimeType.RuntimeTypeCache.CacheType.Constructor);
			}

			// Token: 0x06000E85 RID: 3717 RVA: 0x0002A71D File Offset: 0x0002971D
			internal FieldInfo GetField(RuntimeFieldHandle field)
			{
				this.GetMemberCache<RuntimeFieldInfo>(ref this.m_fieldInfoCache);
				return this.m_fieldInfoCache.AddField(field);
			}

			// Token: 0x040004F1 RID: 1265
			private RuntimeType.RuntimeTypeCache.WhatsCached m_whatsCached;

			// Token: 0x040004F2 RID: 1266
			private RuntimeTypeHandle m_runtimeTypeHandle;

			// Token: 0x040004F3 RID: 1267
			private RuntimeType m_runtimeType;

			// Token: 0x040004F4 RID: 1268
			private RuntimeType m_enclosingType;

			// Token: 0x040004F5 RID: 1269
			private TypeCode m_typeCode;

			// Token: 0x040004F6 RID: 1270
			private string m_name;

			// Token: 0x040004F7 RID: 1271
			private string m_fullname;

			// Token: 0x040004F8 RID: 1272
			private string m_toString;

			// Token: 0x040004F9 RID: 1273
			private string m_namespace;

			// Token: 0x040004FA RID: 1274
			private bool m_isGlobal;

			// Token: 0x040004FB RID: 1275
			private bool m_bIsDomainInitialized;

			// Token: 0x040004FC RID: 1276
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeMethodInfo> m_methodInfoCache;

			// Token: 0x040004FD RID: 1277
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeConstructorInfo> m_constructorInfoCache;

			// Token: 0x040004FE RID: 1278
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeFieldInfo> m_fieldInfoCache;

			// Token: 0x040004FF RID: 1279
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeType> m_interfaceCache;

			// Token: 0x04000500 RID: 1280
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeType> m_nestedClassesCache;

			// Token: 0x04000501 RID: 1281
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimePropertyInfo> m_propertyInfoCache;

			// Token: 0x04000502 RID: 1282
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeEventInfo> m_eventInfoCache;

			// Token: 0x04000503 RID: 1283
			private static CerHashtable<RuntimeMethodInfo, RuntimeMethodInfo> s_methodInstantiations;

			// Token: 0x04000504 RID: 1284
			private static bool s_dontrunhack;

			// Token: 0x020000FA RID: 250
			internal enum WhatsCached
			{
				// Token: 0x04000506 RID: 1286
				Nothing,
				// Token: 0x04000507 RID: 1287
				EnclosingType
			}

			// Token: 0x020000FB RID: 251
			internal enum CacheType
			{
				// Token: 0x04000509 RID: 1289
				Method,
				// Token: 0x0400050A RID: 1290
				Constructor,
				// Token: 0x0400050B RID: 1291
				Field,
				// Token: 0x0400050C RID: 1292
				Property,
				// Token: 0x0400050D RID: 1293
				Event,
				// Token: 0x0400050E RID: 1294
				Interface,
				// Token: 0x0400050F RID: 1295
				NestedType
			}

			// Token: 0x020000FC RID: 252
			private struct Filter
			{
				// Token: 0x06000E86 RID: 3718 RVA: 0x0002A738 File Offset: 0x00029738
				public unsafe Filter(byte* pUtf8Name, int cUtf8Name, MemberListType listType)
				{
					this.m_name = new Utf8String((void*)pUtf8Name, cUtf8Name);
					this.m_listType = listType;
				}

				// Token: 0x06000E87 RID: 3719 RVA: 0x0002A74E File Offset: 0x0002974E
				public bool Match(Utf8String name)
				{
					if (this.m_listType == MemberListType.CaseSensitive)
					{
						return this.m_name.Equals(name);
					}
					return this.m_listType != MemberListType.CaseInsensitive || this.m_name.EqualsCaseInsensitive(name);
				}

				// Token: 0x04000510 RID: 1296
				private Utf8String m_name;

				// Token: 0x04000511 RID: 1297
				private MemberListType m_listType;
			}

			// Token: 0x020000FD RID: 253
			[Serializable]
			private class MemberInfoCache<T> where T : MemberInfo
			{
				// Token: 0x06000E88 RID: 3720 RVA: 0x0002A77D File Offset: 0x0002977D
				static MemberInfoCache()
				{
					RuntimeType.PrepareMemberInfoCache(typeof(RuntimeType.RuntimeTypeCache.MemberInfoCache<T>).TypeHandle);
				}

				// Token: 0x06000E89 RID: 3721 RVA: 0x0002A793 File Offset: 0x00029793
				internal MemberInfoCache(RuntimeType.RuntimeTypeCache runtimeTypeCache)
				{
					Mda.MemberInfoCacheCreation();
					this.m_runtimeTypeCache = runtimeTypeCache;
					this.m_cacheComplete = false;
				}

				// Token: 0x06000E8A RID: 3722 RVA: 0x0002A7B0 File Offset: 0x000297B0
				internal MethodBase AddMethod(RuntimeTypeHandle declaringType, RuntimeMethodHandle method, RuntimeType.RuntimeTypeCache.CacheType cacheType)
				{
					object obj = null;
					MethodAttributes attributes = method.GetAttributes();
					bool isPublic = (attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
					bool isStatic = (attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
					bool isInherited = declaringType.Value != this.ReflectedTypeHandle.Value;
					BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(isPublic, isInherited, isStatic);
					switch (cacheType)
					{
					case RuntimeType.RuntimeTypeCache.CacheType.Method:
						obj = new List<RuntimeMethodInfo>(1)
						{
							new RuntimeMethodInfo(method, declaringType, this.m_runtimeTypeCache, attributes, bindingFlags)
						};
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Constructor:
						obj = new List<RuntimeConstructorInfo>(1)
						{
							new RuntimeConstructorInfo(method, declaringType, this.m_runtimeTypeCache, attributes, bindingFlags)
						};
						break;
					}
					CerArrayList<T> cerArrayList = new CerArrayList<T>((List<T>)obj);
					this.Insert(ref cerArrayList, null, MemberListType.HandleToInfo);
					return (MethodBase)((object)cerArrayList[0]);
				}

				// Token: 0x06000E8B RID: 3723 RVA: 0x0002A884 File Offset: 0x00029884
				internal FieldInfo AddField(RuntimeFieldHandle field)
				{
					List<RuntimeFieldInfo> list = new List<RuntimeFieldInfo>(1);
					FieldAttributes attributes = field.GetAttributes();
					bool isPublic = (attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;
					bool isStatic = (attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope;
					bool isInherited = field.GetApproxDeclaringType().Value != this.ReflectedTypeHandle.Value;
					BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(isPublic, isInherited, isStatic);
					list.Add(new RtFieldInfo(field, this.ReflectedType, this.m_runtimeTypeCache, bindingFlags));
					CerArrayList<T> cerArrayList = new CerArrayList<T>((List<T>)list);
					this.Insert(ref cerArrayList, null, MemberListType.HandleToInfo);
					return (FieldInfo)((object)cerArrayList[0]);
				}

				// Token: 0x06000E8C RID: 3724 RVA: 0x0002A928 File Offset: 0x00029928
				private unsafe CerArrayList<T> Populate(string name, MemberListType listType, RuntimeType.RuntimeTypeCache.CacheType cacheType)
				{
					if (name == null || name.Length == 0 || (cacheType == RuntimeType.RuntimeTypeCache.CacheType.Constructor && name.FirstChar != '.' && name.FirstChar != '*'))
					{
						RuntimeType.RuntimeTypeCache.Filter filter = new RuntimeType.RuntimeTypeCache.Filter(null, 0, listType);
						List<T> list = null;
						switch (cacheType)
						{
						case RuntimeType.RuntimeTypeCache.CacheType.Method:
							list = (this.PopulateMethods(filter) as List<T>);
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Constructor:
							list = (this.PopulateConstructors(filter) as List<T>);
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Field:
							list = (this.PopulateFields(filter) as List<T>);
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Property:
							list = (this.PopulateProperties(filter) as List<T>);
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Event:
							list = (this.PopulateEvents(filter) as List<T>);
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Interface:
							list = (this.PopulateInterfaces(filter) as List<T>);
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.NestedType:
							list = (this.PopulateNestedClasses(filter) as List<T>);
							break;
						}
						CerArrayList<T> result = new CerArrayList<T>(list);
						this.Insert(ref result, name, listType);
						return result;
					}
					IntPtr intPtr2;
					IntPtr intPtr = intPtr2 = name;
					if (intPtr != 0)
					{
						intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
					}
					char* chars = intPtr2;
					int byteCount = Encoding.UTF8.GetByteCount(chars, name.Length);
					byte* ptr = stackalloc byte[1 * byteCount];
					Encoding.UTF8.GetBytes(chars, name.Length, ptr, byteCount);
					RuntimeType.RuntimeTypeCache.Filter filter2 = new RuntimeType.RuntimeTypeCache.Filter(ptr, byteCount, listType);
					List<T> list2 = null;
					switch (cacheType)
					{
					case RuntimeType.RuntimeTypeCache.CacheType.Method:
						list2 = (this.PopulateMethods(filter2) as List<T>);
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Constructor:
						list2 = (this.PopulateConstructors(filter2) as List<T>);
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Field:
						list2 = (this.PopulateFields(filter2) as List<T>);
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Property:
						list2 = (this.PopulateProperties(filter2) as List<T>);
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Event:
						list2 = (this.PopulateEvents(filter2) as List<T>);
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Interface:
						list2 = (this.PopulateInterfaces(filter2) as List<T>);
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.NestedType:
						list2 = (this.PopulateNestedClasses(filter2) as List<T>);
						break;
					}
					CerArrayList<T> result2 = new CerArrayList<T>(list2);
					this.Insert(ref result2, name, listType);
					return result2;
				}

				// Token: 0x06000E8D RID: 3725 RVA: 0x0002AB18 File Offset: 0x00029B18
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
				internal void Insert(ref CerArrayList<T> list, string name, MemberListType listType)
				{
					bool flag = false;
					bool flag2 = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.ReliableEnter(this, ref flag);
						if (listType == MemberListType.CaseSensitive)
						{
							if (this.m_csMemberInfos == null)
							{
								this.m_csMemberInfos = new CerHashtable<string, CerArrayList<T>>();
							}
							else
							{
								this.m_csMemberInfos.Preallocate(1);
							}
						}
						else if (listType == MemberListType.CaseInsensitive)
						{
							if (this.m_cisMemberInfos == null)
							{
								this.m_cisMemberInfos = new CerHashtable<string, CerArrayList<T>>();
							}
							else
							{
								this.m_cisMemberInfos.Preallocate(1);
							}
						}
						if (this.m_root == null)
						{
							this.m_root = new CerArrayList<T>(list.Count);
						}
						else
						{
							this.m_root.Preallocate(list.Count);
						}
						flag2 = true;
					}
					finally
					{
						try
						{
							if (flag2)
							{
								if (listType == MemberListType.CaseSensitive)
								{
									CerArrayList<T> cerArrayList = this.m_csMemberInfos[name];
									if (cerArrayList == null)
									{
										this.MergeWithGlobalList(list);
										this.m_csMemberInfos[name] = list;
									}
									else
									{
										list = cerArrayList;
									}
								}
								else if (listType == MemberListType.CaseInsensitive)
								{
									CerArrayList<T> cerArrayList2 = this.m_cisMemberInfos[name];
									if (cerArrayList2 == null)
									{
										this.MergeWithGlobalList(list);
										this.m_cisMemberInfos[name] = list;
									}
									else
									{
										list = cerArrayList2;
									}
								}
								else
								{
									this.MergeWithGlobalList(list);
								}
								if (listType == MemberListType.All)
								{
									this.m_cacheComplete = true;
								}
							}
						}
						finally
						{
							if (flag)
							{
								Monitor.Exit(this);
							}
						}
					}
				}

				// Token: 0x06000E8E RID: 3726 RVA: 0x0002AC58 File Offset: 0x00029C58
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				private void MergeWithGlobalList(CerArrayList<T> list)
				{
					int count = this.m_root.Count;
					for (int i = 0; i < list.Count; i++)
					{
						T value = list[i];
						T t = default(T);
						for (int j = 0; j < count; j++)
						{
							t = this.m_root[j];
							if (value.CacheEquals(t))
							{
								list.Replace(i, t);
								break;
							}
						}
						if (list[i] != t)
						{
							this.m_root.Add(value);
						}
					}
				}

				// Token: 0x06000E8F RID: 3727 RVA: 0x0002ACF0 File Offset: 0x00029CF0
				private unsafe List<RuntimeMethodInfo> PopulateMethods(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeMethodInfo> list = new List<RuntimeMethodInfo>();
					RuntimeTypeHandle declaringTypeHandle = this.ReflectedTypeHandle;
					bool flag = (declaringTypeHandle.GetAttributes() & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask;
					if (flag)
					{
						bool flag2 = declaringTypeHandle.HasInstantiation() && !declaringTypeHandle.IsGenericTypeDefinition();
						foreach (RuntimeMethodHandle runtimeMethodHandle in declaringTypeHandle.IntroducedMethods)
						{
							if (filter.Match(runtimeMethodHandle.GetUtf8Name()))
							{
								MethodAttributes attributes = runtimeMethodHandle.GetAttributes();
								bool isPublic = (attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
								bool isStatic = (attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
								bool isInherited = false;
								BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(isPublic, isInherited, isStatic);
								if ((attributes & MethodAttributes.RTSpecialName) == MethodAttributes.PrivateScope && !runtimeMethodHandle.IsILStub())
								{
									RuntimeMethodHandle handle = flag2 ? runtimeMethodHandle.GetInstantiatingStubIfNeeded(declaringTypeHandle) : runtimeMethodHandle;
									RuntimeMethodInfo item = new RuntimeMethodInfo(handle, declaringTypeHandle, this.m_runtimeTypeCache, attributes, bindingFlags);
									list.Add(item);
								}
							}
						}
					}
					else
					{
						while (declaringTypeHandle.IsGenericVariable())
						{
							declaringTypeHandle = declaringTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
						}
						bool* ptr = stackalloc bool[1 * declaringTypeHandle.GetNumVirtuals()];
						bool isValueType = declaringTypeHandle.GetRuntimeType().IsValueType;
						while (!declaringTypeHandle.IsNullHandle())
						{
							bool flag3 = declaringTypeHandle.HasInstantiation() && !declaringTypeHandle.IsGenericTypeDefinition();
							int numVirtuals = declaringTypeHandle.GetNumVirtuals();
							foreach (RuntimeMethodHandle runtimeMethodHandle2 in declaringTypeHandle.IntroducedMethods)
							{
								if (filter.Match(runtimeMethodHandle2.GetUtf8Name()))
								{
									MethodAttributes attributes2 = runtimeMethodHandle2.GetAttributes();
									MethodAttributes methodAttributes = attributes2 & MethodAttributes.MemberAccessMask;
									if ((attributes2 & MethodAttributes.RTSpecialName) == MethodAttributes.PrivateScope && !runtimeMethodHandle2.IsILStub())
									{
										bool flag4 = false;
										int num = 0;
										if ((attributes2 & MethodAttributes.Virtual) != MethodAttributes.PrivateScope)
										{
											num = runtimeMethodHandle2.GetSlot();
											flag4 = (num < numVirtuals);
										}
										bool flag5 = methodAttributes == MethodAttributes.Private;
										bool flag6 = flag4 && flag5;
										bool flag7 = declaringTypeHandle.Value != this.ReflectedTypeHandle.Value;
										if (!flag7 || !flag5 || flag6)
										{
											if (flag4)
											{
												if (ptr[num])
												{
													continue;
												}
												ptr[num] = true;
											}
											else if (isValueType && (attributes2 & (MethodAttributes.Virtual | MethodAttributes.Abstract)) != MethodAttributes.PrivateScope)
											{
												continue;
											}
											bool isPublic2 = methodAttributes == MethodAttributes.Public;
											bool isStatic2 = (attributes2 & MethodAttributes.Static) != MethodAttributes.PrivateScope;
											BindingFlags bindingFlags2 = RuntimeType.FilterPreCalculate(isPublic2, flag7, isStatic2);
											RuntimeMethodHandle handle2 = flag3 ? runtimeMethodHandle2.GetInstantiatingStubIfNeeded(declaringTypeHandle) : runtimeMethodHandle2;
											RuntimeMethodInfo item2 = new RuntimeMethodInfo(handle2, declaringTypeHandle, this.m_runtimeTypeCache, attributes2, bindingFlags2);
											list.Add(item2);
										}
									}
								}
							}
							declaringTypeHandle = declaringTypeHandle.GetBaseTypeHandle();
						}
					}
					return list;
				}

				// Token: 0x06000E90 RID: 3728 RVA: 0x0002AF88 File Offset: 0x00029F88
				private List<RuntimeConstructorInfo> PopulateConstructors(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeConstructorInfo> list = new List<RuntimeConstructorInfo>();
					if (this.ReflectedType.IsGenericParameter)
					{
						return list;
					}
					RuntimeTypeHandle reflectedTypeHandle = this.ReflectedTypeHandle;
					bool flag = reflectedTypeHandle.HasInstantiation() && !reflectedTypeHandle.IsGenericTypeDefinition();
					foreach (RuntimeMethodHandle runtimeMethodHandle in reflectedTypeHandle.IntroducedMethods)
					{
						if (filter.Match(runtimeMethodHandle.GetUtf8Name()))
						{
							MethodAttributes attributes = runtimeMethodHandle.GetAttributes();
							if ((attributes & MethodAttributes.RTSpecialName) != MethodAttributes.PrivateScope && !runtimeMethodHandle.IsILStub())
							{
								bool isPublic = (attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
								bool isStatic = (attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
								bool isInherited = false;
								BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(isPublic, isInherited, isStatic);
								RuntimeMethodHandle handle = flag ? runtimeMethodHandle.GetInstantiatingStubIfNeeded(reflectedTypeHandle) : runtimeMethodHandle;
								RuntimeConstructorInfo item = new RuntimeConstructorInfo(handle, this.ReflectedTypeHandle, this.m_runtimeTypeCache, attributes, bindingFlags);
								list.Add(item);
							}
						}
					}
					return list;
				}

				// Token: 0x06000E91 RID: 3729 RVA: 0x0002B078 File Offset: 0x0002A078
				private List<RuntimeFieldInfo> PopulateFields(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeFieldInfo> list = new List<RuntimeFieldInfo>();
					RuntimeTypeHandle declaringTypeHandle = this.ReflectedTypeHandle;
					while (declaringTypeHandle.IsGenericVariable())
					{
						declaringTypeHandle = declaringTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
					}
					while (!declaringTypeHandle.IsNullHandle())
					{
						this.PopulateRtFields(filter, declaringTypeHandle, list);
						this.PopulateLiteralFields(filter, declaringTypeHandle, list);
						declaringTypeHandle = declaringTypeHandle.GetBaseTypeHandle();
					}
					if (this.ReflectedType.IsGenericParameter)
					{
						Type[] interfaces = this.ReflectedTypeHandle.GetRuntimeType().BaseType.GetInterfaces();
						for (int i = 0; i < interfaces.Length; i++)
						{
							this.PopulateLiteralFields(filter, interfaces[i].GetTypeHandleInternal(), list);
							this.PopulateRtFields(filter, interfaces[i].GetTypeHandleInternal(), list);
						}
					}
					else
					{
						RuntimeTypeHandle[] interfaces2 = this.ReflectedTypeHandle.GetInterfaces();
						if (interfaces2 != null)
						{
							for (int j = 0; j < interfaces2.Length; j++)
							{
								this.PopulateLiteralFields(filter, interfaces2[j], list);
								this.PopulateRtFields(filter, interfaces2[j], list);
							}
						}
					}
					return list;
				}

				// Token: 0x06000E92 RID: 3730 RVA: 0x0002B180 File Offset: 0x0002A180
				private unsafe void PopulateRtFields(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, List<RuntimeFieldInfo> list)
				{
					int** ptr = stackalloc int*[sizeof(int*) * 64];
					int num = 64;
					if (!declaringTypeHandle.GetFields(ptr, &num))
					{
						fixed (int** ptr2 = new int*[num])
						{
							declaringTypeHandle.GetFields(ptr2, &num);
							this.PopulateRtFields(filter, ptr2, num, declaringTypeHandle, list);
						}
						return;
					}
					if (num > 0)
					{
						this.PopulateRtFields(filter, ptr, num, declaringTypeHandle, list);
					}
				}

				// Token: 0x06000E93 RID: 3731 RVA: 0x0002B1F4 File Offset: 0x0002A1F4
				private unsafe void PopulateRtFields(RuntimeType.RuntimeTypeCache.Filter filter, int** ppFieldHandles, int count, RuntimeTypeHandle declaringTypeHandle, List<RuntimeFieldInfo> list)
				{
					bool flag = declaringTypeHandle.HasInstantiation() && !declaringTypeHandle.ContainsGenericVariables();
					bool flag2 = !declaringTypeHandle.Equals(this.ReflectedTypeHandle);
					for (int i = 0; i < count; i++)
					{
						RuntimeFieldHandle staticFieldForGenericType = new RuntimeFieldHandle(*(IntPtr*)(ppFieldHandles + (IntPtr)i * (IntPtr)sizeof(int*) / (IntPtr)sizeof(int*)));
						if (filter.Match(staticFieldForGenericType.GetUtf8Name()))
						{
							FieldAttributes attributes = staticFieldForGenericType.GetAttributes();
							FieldAttributes fieldAttributes = attributes & FieldAttributes.FieldAccessMask;
							if (!flag2 || fieldAttributes != FieldAttributes.Private)
							{
								bool isPublic = fieldAttributes == FieldAttributes.Public;
								bool flag3 = (attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope;
								BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(isPublic, flag2, flag3);
								if (flag && flag3)
								{
									staticFieldForGenericType = staticFieldForGenericType.GetStaticFieldForGenericType(declaringTypeHandle);
								}
								RuntimeFieldInfo item = new RtFieldInfo(staticFieldForGenericType, declaringTypeHandle.GetRuntimeType(), this.m_runtimeTypeCache, bindingFlags);
								list.Add(item);
							}
						}
					}
				}

				// Token: 0x06000E94 RID: 3732 RVA: 0x0002B2C8 File Offset: 0x0002A2C8
				private unsafe void PopulateLiteralFields(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, List<RuntimeFieldInfo> list)
				{
					int token = declaringTypeHandle.GetToken();
					if (System.Reflection.MetadataToken.IsNullToken(token))
					{
						return;
					}
					MetadataImport metadataImport = declaringTypeHandle.GetModuleHandle().GetMetadataImport();
					int num = metadataImport.EnumFieldsCount(token);
					int* ptr = stackalloc int[4 * num];
					metadataImport.EnumFields(token, ptr, num);
					for (int i = 0; i < num; i++)
					{
						int num2 = ptr[i];
						Utf8String name = metadataImport.GetName(num2);
						if (filter.Match(name))
						{
							FieldAttributes fieldAttributes;
							metadataImport.GetFieldDefProps(num2, out fieldAttributes);
							FieldAttributes fieldAttributes2 = fieldAttributes & FieldAttributes.FieldAccessMask;
							if ((fieldAttributes & FieldAttributes.Literal) != FieldAttributes.PrivateScope)
							{
								bool flag = !declaringTypeHandle.Equals(this.ReflectedTypeHandle);
								if (!flag || fieldAttributes2 != FieldAttributes.Private)
								{
									bool isPublic = fieldAttributes2 == FieldAttributes.Public;
									bool isStatic = (fieldAttributes & FieldAttributes.Static) != FieldAttributes.PrivateScope;
									BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(isPublic, flag, isStatic);
									RuntimeFieldInfo item = new MdFieldInfo(num2, fieldAttributes, declaringTypeHandle, this.m_runtimeTypeCache, bindingFlags);
									list.Add(item);
								}
							}
						}
					}
				}

				// Token: 0x06000E95 RID: 3733 RVA: 0x0002B3BC File Offset: 0x0002A3BC
				private static void AddElementTypes(Type template, IList<Type> types)
				{
					if (!template.HasElementType)
					{
						return;
					}
					RuntimeType.RuntimeTypeCache.MemberInfoCache<T>.AddElementTypes(template.GetElementType(), types);
					for (int i = 0; i < types.Count; i++)
					{
						if (template.IsArray)
						{
							if (template.IsSzArray)
							{
								types[i] = types[i].MakeArrayType();
							}
							else
							{
								types[i] = types[i].MakeArrayType(template.GetArrayRank());
							}
						}
						else if (template.IsPointer)
						{
							types[i] = types[i].MakePointerType();
						}
					}
				}

				// Token: 0x06000E96 RID: 3734 RVA: 0x0002B44C File Offset: 0x0002A44C
				private List<RuntimeType> PopulateInterfaces(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeType> list = new List<RuntimeType>();
					RuntimeTypeHandle reflectedTypeHandle = this.ReflectedTypeHandle;
					if (!reflectedTypeHandle.IsGenericVariable())
					{
						RuntimeTypeHandle[] interfaces = this.ReflectedTypeHandle.GetInterfaces();
						if (interfaces != null)
						{
							for (int i = 0; i < interfaces.Length; i++)
							{
								RuntimeType runtimeType = interfaces[i].GetRuntimeType();
								if (filter.Match(runtimeType.GetTypeHandleInternal().GetUtf8Name()))
								{
									list.Add(runtimeType);
								}
							}
						}
						if (this.ReflectedType.IsSzArray)
						{
							Type elementType = this.ReflectedType.GetElementType();
							if (!elementType.IsPointer)
							{
								Type type = typeof(IList<>).MakeGenericType(new Type[]
								{
									elementType
								});
								if (type.IsAssignableFrom(this.ReflectedType))
								{
									if (filter.Match(type.GetTypeHandleInternal().GetUtf8Name()))
									{
										list.Add(type as RuntimeType);
									}
									Type[] interfaces2 = type.GetInterfaces();
									for (int j = 0; j < interfaces2.Length; j++)
									{
										Type type2 = interfaces2[j];
										if (type2.IsGenericType && filter.Match(type2.GetTypeHandleInternal().GetUtf8Name()))
										{
											list.Add(interfaces2[j] as RuntimeType);
										}
									}
								}
							}
						}
					}
					else
					{
						List<RuntimeType> list2 = new List<RuntimeType>();
						foreach (Type type3 in reflectedTypeHandle.GetRuntimeType().GetGenericParameterConstraints())
						{
							if (type3.IsInterface)
							{
								list2.Add(type3 as RuntimeType);
							}
							Type[] interfaces3 = type3.GetInterfaces();
							for (int l = 0; l < interfaces3.Length; l++)
							{
								list2.Add(interfaces3[l] as RuntimeType);
							}
						}
						Hashtable hashtable = new Hashtable();
						for (int m = 0; m < list2.Count; m++)
						{
							Type type4 = list2[m];
							if (!hashtable.Contains(type4))
							{
								hashtable[type4] = type4;
							}
						}
						Type[] array = new Type[hashtable.Values.Count];
						hashtable.Values.CopyTo(array, 0);
						for (int n = 0; n < array.Length; n++)
						{
							if (filter.Match(array[n].GetTypeHandleInternal().GetUtf8Name()))
							{
								list.Add(array[n] as RuntimeType);
							}
						}
					}
					return list;
				}

				// Token: 0x06000E97 RID: 3735 RVA: 0x0002B6B4 File Offset: 0x0002A6B4
				private unsafe List<RuntimeType> PopulateNestedClasses(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeType> list = new List<RuntimeType>();
					RuntimeTypeHandle runtimeTypeHandle = this.ReflectedTypeHandle;
					if (runtimeTypeHandle.IsGenericVariable())
					{
						while (runtimeTypeHandle.IsGenericVariable())
						{
							runtimeTypeHandle = runtimeTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
						}
					}
					int token = runtimeTypeHandle.GetToken();
					if (System.Reflection.MetadataToken.IsNullToken(token))
					{
						return list;
					}
					ModuleHandle moduleHandle = runtimeTypeHandle.GetModuleHandle();
					MetadataImport metadataImport = moduleHandle.GetMetadataImport();
					int num = metadataImport.EnumNestedTypesCount(token);
					int* ptr = stackalloc int[4 * num];
					metadataImport.EnumNestedTypes(token, ptr, num);
					int i = 0;
					while (i < num)
					{
						RuntimeTypeHandle runtimeTypeHandle2 = default(RuntimeTypeHandle);
						try
						{
							runtimeTypeHandle2 = moduleHandle.ResolveTypeHandle(ptr[i]);
						}
						catch (TypeLoadException)
						{
							goto IL_C3;
						}
						goto IL_98;
						IL_C3:
						i++;
						continue;
						IL_98:
						if (filter.Match(runtimeTypeHandle2.GetRuntimeType().GetTypeHandleInternal().GetUtf8Name()))
						{
							list.Add(runtimeTypeHandle2.GetRuntimeType());
							goto IL_C3;
						}
						goto IL_C3;
					}
					return list;
				}

				// Token: 0x06000E98 RID: 3736 RVA: 0x0002B7A4 File Offset: 0x0002A7A4
				private List<RuntimeEventInfo> PopulateEvents(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					Hashtable csEventInfos = new Hashtable();
					RuntimeTypeHandle declaringTypeHandle = this.ReflectedTypeHandle;
					List<RuntimeEventInfo> list = new List<RuntimeEventInfo>();
					if ((declaringTypeHandle.GetAttributes() & TypeAttributes.ClassSemanticsMask) != TypeAttributes.ClassSemanticsMask)
					{
						while (declaringTypeHandle.IsGenericVariable())
						{
							declaringTypeHandle = declaringTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
						}
						while (!declaringTypeHandle.IsNullHandle())
						{
							this.PopulateEvents(filter, declaringTypeHandle, csEventInfos, list);
							declaringTypeHandle = declaringTypeHandle.GetBaseTypeHandle();
						}
					}
					else
					{
						this.PopulateEvents(filter, declaringTypeHandle, csEventInfos, list);
					}
					return list;
				}

				// Token: 0x06000E99 RID: 3737 RVA: 0x0002B820 File Offset: 0x0002A820
				private unsafe void PopulateEvents(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, Hashtable csEventInfos, List<RuntimeEventInfo> list)
				{
					int token = declaringTypeHandle.GetToken();
					if (System.Reflection.MetadataToken.IsNullToken(token))
					{
						return;
					}
					MetadataImport metadataImport = declaringTypeHandle.GetModuleHandle().GetMetadataImport();
					int num = metadataImport.EnumEventsCount(token);
					int* ptr = stackalloc int[4 * num];
					metadataImport.EnumEvents(token, ptr, num);
					this.PopulateEvents(filter, declaringTypeHandle, metadataImport, ptr, num, csEventInfos, list);
				}

				// Token: 0x06000E9A RID: 3738 RVA: 0x0002B878 File Offset: 0x0002A878
				private unsafe void PopulateEvents(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, MetadataImport scope, int* tkAssociates, int cAssociates, Hashtable csEventInfos, List<RuntimeEventInfo> list)
				{
					for (int i = 0; i < cAssociates; i++)
					{
						int num = tkAssociates[i];
						Utf8String name = scope.GetName(num);
						if (filter.Match(name))
						{
							bool flag;
							RuntimeEventInfo runtimeEventInfo = new RuntimeEventInfo(num, declaringTypeHandle.GetRuntimeType(), this.m_runtimeTypeCache, ref flag);
							if ((declaringTypeHandle.Equals(this.m_runtimeTypeCache.RuntimeTypeHandle) || !flag) && csEventInfos[runtimeEventInfo.Name] == null)
							{
								csEventInfos[runtimeEventInfo.Name] = runtimeEventInfo;
								list.Add(runtimeEventInfo);
							}
						}
					}
				}

				// Token: 0x06000E9B RID: 3739 RVA: 0x0002B908 File Offset: 0x0002A908
				private List<RuntimePropertyInfo> PopulateProperties(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					Hashtable csPropertyInfos = new Hashtable();
					RuntimeTypeHandle declaringTypeHandle = this.ReflectedTypeHandle;
					List<RuntimePropertyInfo> list = new List<RuntimePropertyInfo>();
					if ((declaringTypeHandle.GetAttributes() & TypeAttributes.ClassSemanticsMask) != TypeAttributes.ClassSemanticsMask)
					{
						while (declaringTypeHandle.IsGenericVariable())
						{
							declaringTypeHandle = declaringTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
						}
						while (!declaringTypeHandle.IsNullHandle())
						{
							this.PopulateProperties(filter, declaringTypeHandle, csPropertyInfos, list);
							declaringTypeHandle = declaringTypeHandle.GetBaseTypeHandle();
						}
					}
					else
					{
						this.PopulateProperties(filter, declaringTypeHandle, csPropertyInfos, list);
					}
					return list;
				}

				// Token: 0x06000E9C RID: 3740 RVA: 0x0002B984 File Offset: 0x0002A984
				private unsafe void PopulateProperties(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, Hashtable csPropertyInfos, List<RuntimePropertyInfo> list)
				{
					int token = declaringTypeHandle.GetToken();
					if (System.Reflection.MetadataToken.IsNullToken(token))
					{
						return;
					}
					MetadataImport metadataImport = declaringTypeHandle.GetModuleHandle().GetMetadataImport();
					int num = metadataImport.EnumPropertiesCount(token);
					int* ptr = stackalloc int[4 * num];
					metadataImport.EnumProperties(token, ptr, num);
					this.PopulateProperties(filter, declaringTypeHandle, ptr, num, csPropertyInfos, list);
				}

				// Token: 0x06000E9D RID: 3741 RVA: 0x0002B9DC File Offset: 0x0002A9DC
				private unsafe void PopulateProperties(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, int* tkAssociates, int cProperties, Hashtable csPropertyInfos, List<RuntimePropertyInfo> list)
				{
					for (int i = 0; i < cProperties; i++)
					{
						int num = tkAssociates[i];
						Utf8String name = declaringTypeHandle.GetRuntimeType().Module.MetadataImport.GetName(num);
						if (filter.Match(name))
						{
							bool flag;
							RuntimePropertyInfo runtimePropertyInfo = new RuntimePropertyInfo(num, declaringTypeHandle.GetRuntimeType(), this.m_runtimeTypeCache, ref flag);
							if (declaringTypeHandle.Equals(this.m_runtimeTypeCache.RuntimeTypeHandle) || !flag)
							{
								List<RuntimePropertyInfo> list2 = csPropertyInfos[runtimePropertyInfo.Name] as List<RuntimePropertyInfo>;
								if (list2 == null)
								{
									list2 = new List<RuntimePropertyInfo>();
									csPropertyInfos[runtimePropertyInfo.Name] = list2;
								}
								else
								{
									for (int j = 0; j < list2.Count; j++)
									{
										if (runtimePropertyInfo.EqualsSig(list2[j]))
										{
											list2 = null;
											break;
										}
									}
								}
								if (list2 != null)
								{
									list2.Add(runtimePropertyInfo);
									list.Add(runtimePropertyInfo);
								}
							}
						}
					}
				}

				// Token: 0x06000E9E RID: 3742 RVA: 0x0002BAD0 File Offset: 0x0002AAD0
				internal CerArrayList<T> GetMemberList(MemberListType listType, string name, RuntimeType.RuntimeTypeCache.CacheType cacheType)
				{
					switch (listType)
					{
					case MemberListType.All:
						if (this.m_cacheComplete)
						{
							return this.m_root;
						}
						return this.Populate(null, listType, cacheType);
					case MemberListType.CaseSensitive:
					{
						if (this.m_csMemberInfos == null)
						{
							return this.Populate(name, listType, cacheType);
						}
						CerArrayList<T> cerArrayList = this.m_csMemberInfos[name];
						if (cerArrayList == null)
						{
							return this.Populate(name, listType, cacheType);
						}
						return cerArrayList;
					}
					default:
					{
						if (this.m_cisMemberInfos == null)
						{
							return this.Populate(name, listType, cacheType);
						}
						CerArrayList<T> cerArrayList = this.m_cisMemberInfos[name];
						if (cerArrayList == null)
						{
							return this.Populate(name, listType, cacheType);
						}
						return cerArrayList;
					}
					}
				}

				// Token: 0x170001D1 RID: 465
				// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0002BB65 File Offset: 0x0002AB65
				internal RuntimeTypeHandle ReflectedTypeHandle
				{
					get
					{
						return this.m_runtimeTypeCache.RuntimeTypeHandle;
					}
				}

				// Token: 0x170001D2 RID: 466
				// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0002BB74 File Offset: 0x0002AB74
				internal RuntimeType ReflectedType
				{
					get
					{
						return this.ReflectedTypeHandle.GetRuntimeType();
					}
				}

				// Token: 0x04000512 RID: 1298
				private CerHashtable<string, CerArrayList<T>> m_csMemberInfos;

				// Token: 0x04000513 RID: 1299
				private CerHashtable<string, CerArrayList<T>> m_cisMemberInfos;

				// Token: 0x04000514 RID: 1300
				private CerArrayList<T> m_root;

				// Token: 0x04000515 RID: 1301
				private bool m_cacheComplete;

				// Token: 0x04000516 RID: 1302
				private RuntimeType.RuntimeTypeCache m_runtimeTypeCache;
			}
		}

		// Token: 0x020000FE RID: 254
		private class TypeCacheQueue
		{
			// Token: 0x06000EA1 RID: 3745 RVA: 0x0002BB8F File Offset: 0x0002AB8F
			internal TypeCacheQueue()
			{
				this.liveCache = new object[4];
			}

			// Token: 0x04000517 RID: 1303
			private const int QUEUE_SIZE = 4;

			// Token: 0x04000518 RID: 1304
			private object[] liveCache;
		}

		// Token: 0x020000FF RID: 255
		private class ActivatorCacheEntry
		{
			// Token: 0x06000EA2 RID: 3746 RVA: 0x0002BBA3 File Offset: 0x0002ABA3
			internal ActivatorCacheEntry(Type t, RuntimeMethodHandle rmh, bool bNeedSecurityCheck)
			{
				this.m_type = t;
				this.m_bNeedSecurityCheck = bNeedSecurityCheck;
				this.m_hCtorMethodHandle = rmh;
			}

			// Token: 0x04000519 RID: 1305
			internal Type m_type;

			// Token: 0x0400051A RID: 1306
			internal CtorDelegate m_ctor;

			// Token: 0x0400051B RID: 1307
			internal RuntimeMethodHandle m_hCtorMethodHandle;

			// Token: 0x0400051C RID: 1308
			internal bool m_bNeedSecurityCheck;

			// Token: 0x0400051D RID: 1309
			internal bool m_bFullyInitialized;
		}

		// Token: 0x02000100 RID: 256
		private class ActivatorCache
		{
			// Token: 0x06000EA3 RID: 3747 RVA: 0x0002BBC0 File Offset: 0x0002ABC0
			private void InitializeDelegateCreator()
			{
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
				permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
				Thread.MemoryBarrier();
				this.delegateCreatePermissions = permissionSet;
				ConstructorInfo constructor = typeof(CtorDelegate).GetConstructor(new Type[]
				{
					typeof(object),
					typeof(IntPtr)
				});
				Thread.MemoryBarrier();
				this.delegateCtorInfo = constructor;
			}

			// Token: 0x06000EA4 RID: 3748 RVA: 0x0002BC38 File Offset: 0x0002AC38
			private void InitializeCacheEntry(RuntimeType.ActivatorCacheEntry ace)
			{
				if (!ace.m_type.IsValueType)
				{
					if (this.delegateCtorInfo == null)
					{
						this.InitializeDelegateCreator();
					}
					this.delegateCreatePermissions.Assert();
					CtorDelegate ctor = (CtorDelegate)this.delegateCtorInfo.Invoke(new object[]
					{
						null,
						ace.m_hCtorMethodHandle.GetFunctionPointer()
					});
					Thread.MemoryBarrier();
					ace.m_ctor = ctor;
				}
				ace.m_bFullyInitialized = true;
			}

			// Token: 0x06000EA5 RID: 3749 RVA: 0x0002BCAC File Offset: 0x0002ACAC
			internal RuntimeType.ActivatorCacheEntry GetEntry(Type t)
			{
				int num = this.hash_counter;
				for (int i = 0; i < 16; i++)
				{
					RuntimeType.ActivatorCacheEntry activatorCacheEntry = this.cache[num];
					if (activatorCacheEntry != null && activatorCacheEntry.m_type == t)
					{
						if (!activatorCacheEntry.m_bFullyInitialized)
						{
							this.InitializeCacheEntry(activatorCacheEntry);
						}
						return activatorCacheEntry;
					}
					num = (num + 1 & 15);
				}
				return null;
			}

			// Token: 0x06000EA6 RID: 3750 RVA: 0x0002BCFC File Offset: 0x0002ACFC
			internal void SetEntry(RuntimeType.ActivatorCacheEntry ace)
			{
				int num = this.hash_counter - 1 & 15;
				this.hash_counter = num;
				this.cache[num] = ace;
			}

			// Token: 0x0400051E RID: 1310
			private const int CACHE_SIZE = 16;

			// Token: 0x0400051F RID: 1311
			private int hash_counter;

			// Token: 0x04000520 RID: 1312
			private RuntimeType.ActivatorCacheEntry[] cache = new RuntimeType.ActivatorCacheEntry[16];

			// Token: 0x04000521 RID: 1313
			private ConstructorInfo delegateCtorInfo;

			// Token: 0x04000522 RID: 1314
			private PermissionSet delegateCreatePermissions;
		}

		// Token: 0x02000101 RID: 257
		[Flags]
		private enum DispatchWrapperType
		{
			// Token: 0x04000524 RID: 1316
			Unknown = 1,
			// Token: 0x04000525 RID: 1317
			Dispatch = 2,
			// Token: 0x04000526 RID: 1318
			Record = 4,
			// Token: 0x04000527 RID: 1319
			Error = 8,
			// Token: 0x04000528 RID: 1320
			Currency = 16,
			// Token: 0x04000529 RID: 1321
			BStr = 32,
			// Token: 0x0400052A RID: 1322
			SafeArray = 65536
		}
	}
}
