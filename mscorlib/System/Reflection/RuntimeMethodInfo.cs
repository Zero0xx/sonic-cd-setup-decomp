using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x0200034D RID: 845
	[Serializable]
	internal sealed class RuntimeMethodInfo : MethodInfo, ISerializable
	{
		// Token: 0x060020B4 RID: 8372 RVA: 0x00050BC0 File Offset: 0x0004FBC0
		internal static string ConstructParameters(ParameterInfo[] parameters, CallingConventions callingConvention)
		{
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			return RuntimeMethodInfo.ConstructParameters(array, callingConvention);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00050BF8 File Offset: 0x0004FBF8
		internal static string ConstructParameters(Type[] parameters, CallingConventions callingConvention)
		{
			string text = "";
			string str = "";
			foreach (Type type in parameters)
			{
				text += str;
				text += type.SigToString();
				if (type.IsByRef)
				{
					text = text.TrimEnd(new char[]
					{
						'&'
					});
					text += " ByRef";
				}
				str = ", ";
			}
			if ((callingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				text += str;
				text += "...";
			}
			return text;
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00050C84 File Offset: 0x0004FC84
		internal static string ConstructName(MethodBase mi)
		{
			string str = null;
			str += mi.Name;
			RuntimeMethodInfo runtimeMethodInfo = mi as RuntimeMethodInfo;
			if (runtimeMethodInfo != null && runtimeMethodInfo.IsGenericMethod)
			{
				str += runtimeMethodInfo.m_handle.ConstructInstantiation();
			}
			return str + "(" + RuntimeMethodInfo.ConstructParameters(mi.GetParametersNoCopy(), mi.CallingConvention) + ")";
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00050CE7 File Offset: 0x0004FCE7
		internal RuntimeMethodInfo()
		{
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00050CF0 File Offset: 0x0004FCF0
		internal RuntimeMethodInfo(RuntimeMethodHandle handle, RuntimeTypeHandle declaringTypeHandle, RuntimeType.RuntimeTypeCache reflectedTypeCache, MethodAttributes methodAttributes, BindingFlags bindingFlags)
		{
			this.m_toString = null;
			this.m_bindingFlags = bindingFlags;
			this.m_handle = handle;
			this.m_reflectedTypeCache = reflectedTypeCache;
			this.m_parameters = null;
			this.m_methodAttributes = methodAttributes;
			this.m_declaringType = declaringTypeHandle.GetRuntimeType();
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x00050D3C File Offset: 0x0004FD3C
		private RuntimeTypeHandle ReflectedTypeHandle
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeTypeHandle;
			}
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00050D49 File Offset: 0x0004FD49
		internal ParameterInfo[] FetchNonReturnParameters()
		{
			if (this.m_parameters == null)
			{
				this.m_parameters = ParameterInfo.GetParameters(this, this, this.Signature);
			}
			return this.m_parameters;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00050D6C File Offset: 0x0004FD6C
		internal ParameterInfo FetchReturnParameter()
		{
			if (this.m_returnParameter == null)
			{
				this.m_returnParameter = ParameterInfo.GetReturnParameter(this, this, this.Signature);
			}
			return this.m_returnParameter;
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00050D90 File Offset: 0x0004FD90
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimeMethodInfo runtimeMethodInfo = o as RuntimeMethodInfo;
			return runtimeMethodInfo != null && runtimeMethodInfo.m_handle.Equals(this.m_handle);
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x00050DBA File Offset: 0x0004FDBA
		internal Signature Signature
		{
			get
			{
				if (this.m_signature == null)
				{
					this.m_signature = new Signature(this.m_handle, this.m_declaringType.GetTypeHandleInternal());
				}
				return this.m_signature;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x00050DE6 File Offset: 0x0004FDE6
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x00050DEE File Offset: 0x0004FDEE
		internal override RuntimeMethodHandle GetMethodHandle()
		{
			return this.m_handle;
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00050DF8 File Offset: 0x0004FDF8
		internal override MethodInfo GetParentDefinition()
		{
			if (!base.IsVirtual || this.m_declaringType.IsInterface)
			{
				return null;
			}
			Type baseType = this.m_declaringType.BaseType;
			if (baseType == null)
			{
				return null;
			}
			int slot = this.m_handle.GetSlot();
			if (baseType.GetTypeHandleInternal().GetNumVirtuals() <= slot)
			{
				return null;
			}
			return (MethodInfo)RuntimeType.GetMethodBase(baseType.GetTypeHandleInternal(), baseType.GetTypeHandleInternal().GetMethodAt(slot));
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x00050E6C File Offset: 0x0004FE6C
		internal override uint GetOneTimeFlags()
		{
			uint num = 0U;
			if (this.ReturnType.IsByRef)
			{
				num = 2U;
			}
			return num | base.GetOneTimeFlags();
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x00050E94 File Offset: 0x0004FE94
		public override string ToString()
		{
			if (this.m_toString == null)
			{
				this.m_toString = this.ReturnType.SigToString() + " " + RuntimeMethodInfo.ConstructName(this);
			}
			return this.m_toString;
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x00050EC8 File Offset: 0x0004FEC8
		public override int GetHashCode()
		{
			return this.GetMethodHandle().GetHashCode();
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x00050EEC File Offset: 0x0004FEEC
		public override bool Equals(object obj)
		{
			if (!this.IsGenericMethod)
			{
				return obj == this;
			}
			RuntimeMethodInfo runtimeMethodInfo = obj as RuntimeMethodInfo;
			RuntimeMethodHandle left = this.GetMethodHandle().StripMethodInstantiation();
			RuntimeMethodHandle right = runtimeMethodInfo.GetMethodHandle().StripMethodInstantiation();
			if (left != right)
			{
				return false;
			}
			if (runtimeMethodInfo == null || !runtimeMethodInfo.IsGenericMethod)
			{
				return false;
			}
			Type[] genericArguments = this.GetGenericArguments();
			Type[] genericArguments2 = runtimeMethodInfo.GetGenericArguments();
			if (genericArguments.Length != genericArguments2.Length)
			{
				return false;
			}
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (genericArguments[i] != genericArguments2[i])
				{
					return false;
				}
			}
			if (runtimeMethodInfo.IsGenericMethod)
			{
				if (this.DeclaringType != runtimeMethodInfo.DeclaringType)
				{
					return false;
				}
				if (this.ReflectedType != runtimeMethodInfo.ReflectedType)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00050FA7 File Offset: 0x0004FFA7
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType, inherit);
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x00050FC0 File Offset: 0x0004FFC0
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

		// Token: 0x060020C7 RID: 8391 RVA: 0x00051008 File Offset: 0x00050008
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

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060020C8 RID: 8392 RVA: 0x0005104F File Offset: 0x0005004F
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.m_handle.GetName();
				}
				return this.m_name;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x00051070 File Offset: 0x00050070
		public override Type DeclaringType
		{
			get
			{
				if (this.m_reflectedTypeCache.IsGlobal)
				{
					return null;
				}
				return this.m_declaringType;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x00051087 File Offset: 0x00050087
		public override Type ReflectedType
		{
			get
			{
				if (this.m_reflectedTypeCache.IsGlobal)
				{
					return null;
				}
				return this.m_reflectedTypeCache.RuntimeType;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060020CB RID: 8395 RVA: 0x000510A3 File Offset: 0x000500A3
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060020CC RID: 8396 RVA: 0x000510A6 File Offset: 0x000500A6
		public override int MetadataToken
		{
			get
			{
				return this.m_handle.GetMethodDef();
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x000510B3 File Offset: 0x000500B3
		public override Module Module
		{
			get
			{
				return this.m_declaringType.Module;
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000510C0 File Offset: 0x000500C0
		internal override ParameterInfo[] GetParametersNoCopy()
		{
			this.FetchNonReturnParameters();
			return this.m_parameters;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000510D0 File Offset: 0x000500D0
		public override ParameterInfo[] GetParameters()
		{
			this.FetchNonReturnParameters();
			if (this.m_parameters.Length == 0)
			{
				return this.m_parameters;
			}
			ParameterInfo[] array = new ParameterInfo[this.m_parameters.Length];
			Array.Copy(this.m_parameters, array, this.m_parameters.Length);
			return array;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00051118 File Offset: 0x00050118
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_handle.GetImplAttributes();
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060020D1 RID: 8401 RVA: 0x00051125 File Offset: 0x00050125
		internal override bool IsOverloaded
		{
			get
			{
				return this.m_reflectedTypeCache.GetMethodList(MemberListType.CaseSensitive, this.Name).Count > 1;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060020D2 RID: 8402 RVA: 0x00051144 File Offset: 0x00050144
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				Type declaringType = this.DeclaringType;
				if ((declaringType == null && this.Module.Assembly.ReflectionOnly) || declaringType is ReflectionOnlyType)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInReflectionOnly"));
				}
				return this.m_handle;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060020D3 RID: 8403 RVA: 0x0005118B File Offset: 0x0005018B
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_methodAttributes;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060020D4 RID: 8404 RVA: 0x00051193 File Offset: 0x00050193
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.Signature.CallingConvention;
			}
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000511A0 File Offset: 0x000501A0
		[ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public override MethodBody GetMethodBody()
		{
			MethodBody methodBody = this.m_handle.GetMethodBody(this.ReflectedTypeHandle);
			if (methodBody != null)
			{
				methodBody.m_methodBase = this;
			}
			return methodBody;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000511CA File Offset: 0x000501CA
		private void CheckConsistency(object target)
		{
			if ((this.m_methodAttributes & MethodAttributes.Static) == MethodAttributes.Static || this.m_declaringType.IsInstanceOfType(target))
			{
				return;
			}
			if (target == null)
			{
				throw new TargetException(Environment.GetResourceString("RFLCT.Targ_StatMethReqTarg"));
			}
			throw new TargetException(Environment.GetResourceString("RFLCT.Targ_ITargMismatch"));
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x0005120C File Offset: 0x0005020C
		private void ThrowNoInvokeException()
		{
			Type declaringType = this.DeclaringType;
			if ((declaringType == null && this.Module.Assembly.ReflectionOnly) || declaringType is ReflectionOnlyType)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyInvoke"));
			}
			if (this.DeclaringType.GetRootElementType() == typeof(ArgIterator))
			{
				throw new NotSupportedException();
			}
			if ((this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				throw new NotSupportedException();
			}
			if (this.DeclaringType.ContainsGenericParameters || this.ContainsGenericParameters)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_UnboundGenParam"));
			}
			if (base.IsAbstract)
			{
				throw new MemberAccessException();
			}
			if (this.ReturnType.IsByRef)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ByRefReturn"));
			}
			throw new TargetException();
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000512D0 File Offset: 0x000502D0
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(obj, invokeAttr, binder, parameters, culture, false);
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000512E0 File Offset: 0x000502E0
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture, bool skipVisibilityChecks)
		{
			int num = this.Signature.Arguments.Length;
			int num2 = (parameters != null) ? parameters.Length : 0;
			if ((this.m_invocationFlags & 1U) == 0U)
			{
				this.m_invocationFlags = this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 2U) != 0U)
			{
				this.ThrowNoInvokeException();
			}
			this.CheckConsistency(obj);
			if (num != num2)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
			}
			if (num2 > 65535)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("NotSupported_TooManyArgs"));
			}
			if (!skipVisibilityChecks && (this.m_invocationFlags & 36U) != 0U)
			{
				if ((this.m_invocationFlags & 32U) != 0U)
				{
					CodeAccessPermission.DemandInternal(PermissionType.ReflectionMemberAccess);
				}
				if ((this.m_invocationFlags & 4U) != 0U)
				{
					MethodBase.PerformSecurityCheck(obj, this.m_handle, this.m_declaringType.TypeHandle.Value, this.m_invocationFlags);
				}
			}
			RuntimeTypeHandle typeOwner = RuntimeTypeHandle.EmptyHandle;
			if (!this.m_reflectedTypeCache.IsGlobal)
			{
				typeOwner = this.m_declaringType.TypeHandle;
			}
			if (num2 == 0)
			{
				return this.m_handle.InvokeMethodFast(obj, null, this.Signature, this.m_methodAttributes, typeOwner);
			}
			object[] array = base.CheckArguments(parameters, binder, invokeAttr, culture, this.Signature);
			object result = this.m_handle.InvokeMethodFast(obj, array, this.Signature, this.m_methodAttributes, typeOwner);
			for (int i = 0; i < num2; i++)
			{
				parameters[i] = array[i];
			}
			return result;
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060020DA RID: 8410 RVA: 0x00051438 File Offset: 0x00050438
		public override Type ReturnType
		{
			get
			{
				return this.Signature.ReturnTypeHandle.GetRuntimeType();
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060020DB RID: 8411 RVA: 0x00051458 File Offset: 0x00050458
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return this.ReturnParameter;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060020DC RID: 8412 RVA: 0x00051460 File Offset: 0x00050460
		public override ParameterInfo ReturnParameter
		{
			get
			{
				this.FetchReturnParameter();
				return this.m_returnParameter;
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00051470 File Offset: 0x00050470
		public override MethodInfo GetBaseDefinition()
		{
			if (!base.IsVirtual || base.IsStatic || this.m_declaringType == null || this.m_declaringType.IsInterface)
			{
				return this;
			}
			int slot = this.m_handle.GetSlot();
			Type type = this.DeclaringType;
			Type type2 = this.DeclaringType;
			RuntimeMethodHandle methodHandle = default(RuntimeMethodHandle);
			do
			{
				RuntimeTypeHandle typeHandleInternal = type.GetTypeHandleInternal();
				int numVirtuals = typeHandleInternal.GetNumVirtuals();
				if (numVirtuals <= slot)
				{
					break;
				}
				methodHandle = typeHandleInternal.GetMethodAt(slot);
				type2 = type;
				type = type.BaseType;
			}
			while (type != null);
			return (MethodInfo)RuntimeType.GetMethodBase(type2.GetTypeHandleInternal(), methodHandle);
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00051504 File Offset: 0x00050504
		public override MethodInfo MakeGenericMethod(params Type[] methodInstantiation)
		{
			if (methodInstantiation == null)
			{
				throw new ArgumentNullException("methodInstantiation");
			}
			Type[] array = new Type[methodInstantiation.Length];
			for (int i = 0; i < methodInstantiation.Length; i++)
			{
				array[i] = methodInstantiation[i];
			}
			methodInstantiation = array;
			if (!this.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_NotGenericMethodDefinition"), new object[]
				{
					this
				}));
			}
			for (int j = 0; j < methodInstantiation.Length; j++)
			{
				if (methodInstantiation[j] == null)
				{
					throw new ArgumentNullException();
				}
				if (!(methodInstantiation[j] is RuntimeType))
				{
					return MethodBuilderInstantiation.MakeGenericMethod(this, methodInstantiation);
				}
			}
			Type[] genericArguments = this.GetGenericArguments();
			RuntimeType.SanityCheckGenericArguments(methodInstantiation, genericArguments);
			RuntimeTypeHandle[] array2 = new RuntimeTypeHandle[methodInstantiation.Length];
			for (int k = 0; k < methodInstantiation.Length; k++)
			{
				array2[k] = methodInstantiation[k].GetTypeHandleInternal();
			}
			MethodInfo result = null;
			try
			{
				result = (RuntimeType.GetMethodBase(this.m_reflectedTypeCache.RuntimeTypeHandle, this.m_handle.GetInstantiatingStub(this.m_declaringType.GetTypeHandleInternal(), array2)) as MethodInfo);
			}
			catch (VerificationException ex)
			{
				RuntimeType.ValidateGenericArguments(this, methodInstantiation, ex);
				throw ex;
			}
			return result;
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00051630 File Offset: 0x00050630
		public override Type[] GetGenericArguments()
		{
			RuntimeTypeHandle[] methodInstantiation = this.m_handle.GetMethodInstantiation();
			RuntimeType[] array;
			if (methodInstantiation != null)
			{
				array = new RuntimeType[methodInstantiation.Length];
				for (int i = 0; i < methodInstantiation.Length; i++)
				{
					array[i] = methodInstantiation[i].GetRuntimeType();
				}
			}
			else
			{
				array = new RuntimeType[0];
			}
			return array;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x0005167E File Offset: 0x0005067E
		public override MethodInfo GetGenericMethodDefinition()
		{
			if (!this.IsGenericMethod)
			{
				throw new InvalidOperationException();
			}
			return RuntimeType.GetMethodBase(this.m_declaringType.GetTypeHandleInternal(), this.m_handle.StripMethodInstantiation()) as MethodInfo;
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060020E1 RID: 8417 RVA: 0x000516AE File Offset: 0x000506AE
		public override bool IsGenericMethod
		{
			get
			{
				return this.m_handle.HasMethodInstantiation();
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060020E2 RID: 8418 RVA: 0x000516BB File Offset: 0x000506BB
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return this.m_handle.IsGenericMethodDefinition();
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x000516C8 File Offset: 0x000506C8
		public override bool ContainsGenericParameters
		{
			get
			{
				if (this.DeclaringType != null && this.DeclaringType.ContainsGenericParameters)
				{
					return true;
				}
				if (!this.IsGenericMethod)
				{
					return false;
				}
				Type[] genericArguments = this.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (genericArguments[i].ContainsGenericParameters)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00051718 File Offset: 0x00050718
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.m_reflectedTypeCache.IsGlobal)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GlobalMethodSerialization"));
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedTypeHandle.GetRuntimeType(), this.ToString(), MemberTypes.Method, (this.IsGenericMethod & !this.IsGenericMethodDefinition) ? this.GetGenericArguments() : null);
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x0005178C File Offset: 0x0005078C
		internal static MethodBase InternalGetCurrentMethod(ref StackCrawlMark stackMark)
		{
			RuntimeMethodHandle methodHandle = RuntimeMethodHandle.GetCurrentMethod(ref stackMark);
			if (methodHandle.IsNullHandle())
			{
				return null;
			}
			methodHandle = methodHandle.GetTypicalMethodDefinition();
			return RuntimeType.GetMethodBase(methodHandle);
		}

		// Token: 0x04000DEA RID: 3562
		private RuntimeMethodHandle m_handle;

		// Token: 0x04000DEB RID: 3563
		private RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000DEC RID: 3564
		private string m_name;

		// Token: 0x04000DED RID: 3565
		private string m_toString;

		// Token: 0x04000DEE RID: 3566
		private ParameterInfo[] m_parameters;

		// Token: 0x04000DEF RID: 3567
		private ParameterInfo m_returnParameter;

		// Token: 0x04000DF0 RID: 3568
		private BindingFlags m_bindingFlags;

		// Token: 0x04000DF1 RID: 3569
		private MethodAttributes m_methodAttributes;

		// Token: 0x04000DF2 RID: 3570
		private Signature m_signature;

		// Token: 0x04000DF3 RID: 3571
		private RuntimeType m_declaringType;

		// Token: 0x04000DF4 RID: 3572
		private uint m_invocationFlags;
	}
}
