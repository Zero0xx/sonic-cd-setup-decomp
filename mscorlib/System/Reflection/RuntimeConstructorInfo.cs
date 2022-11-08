using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x0200034E RID: 846
	[Serializable]
	internal sealed class RuntimeConstructorInfo : ConstructorInfo, ISerializable
	{
		// Token: 0x060020E6 RID: 8422 RVA: 0x000517B9 File Offset: 0x000507B9
		internal RuntimeConstructorInfo()
		{
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000517C4 File Offset: 0x000507C4
		internal RuntimeConstructorInfo(RuntimeMethodHandle handle, RuntimeTypeHandle declaringTypeHandle, RuntimeType.RuntimeTypeCache reflectedTypeCache, MethodAttributes methodAttributes, BindingFlags bindingFlags)
		{
			this.m_bindingFlags = bindingFlags;
			this.m_handle = handle;
			this.m_reflectedTypeCache = reflectedTypeCache;
			this.m_declaringType = declaringTypeHandle.GetRuntimeType();
			this.m_parameters = null;
			this.m_toString = null;
			this.m_methodAttributes = methodAttributes;
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00051810 File Offset: 0x00050810
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimeConstructorInfo runtimeConstructorInfo = o as RuntimeConstructorInfo;
			return runtimeConstructorInfo != null && runtimeConstructorInfo.m_handle.Equals(this.m_handle);
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060020E9 RID: 8425 RVA: 0x0005183A File Offset: 0x0005083A
		private Signature Signature
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

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060020EA RID: 8426 RVA: 0x00051866 File Offset: 0x00050866
		private RuntimeTypeHandle ReflectedTypeHandle
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeTypeHandle;
			}
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00051873 File Offset: 0x00050873
		private void CheckConsistency(object target)
		{
			if (target == null && base.IsStatic)
			{
				return;
			}
			if (this.m_declaringType.IsInstanceOfType(target))
			{
				return;
			}
			if (target == null)
			{
				throw new TargetException(Environment.GetResourceString("RFLCT.Targ_StatMethReqTarg"));
			}
			throw new TargetException(Environment.GetResourceString("RFLCT.Targ_ITargMismatch"));
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060020EC RID: 8428 RVA: 0x000518B2 File Offset: 0x000508B2
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000518BA File Offset: 0x000508BA
		internal override RuntimeMethodHandle GetMethodHandle()
		{
			return this.m_handle;
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060020EE RID: 8430 RVA: 0x000518C2 File Offset: 0x000508C2
		internal override bool IsOverloaded
		{
			get
			{
				return this.m_reflectedTypeCache.GetConstructorList(MemberListType.CaseSensitive, this.Name).Count > 1;
			}
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000518E0 File Offset: 0x000508E0
		internal override uint GetOneTimeSpecificFlags()
		{
			uint num = 16U;
			if ((this.DeclaringType != null && this.DeclaringType.IsAbstract) || base.IsStatic)
			{
				num |= 8U;
			}
			else if (this.DeclaringType == typeof(void))
			{
				num |= 2U;
			}
			else if (typeof(Delegate).IsAssignableFrom(this.DeclaringType))
			{
				num |= 128U;
			}
			return num;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x0005194B File Offset: 0x0005094B
		public override string ToString()
		{
			if (this.m_toString == null)
			{
				this.m_toString = "Void " + RuntimeMethodInfo.ConstructName(this);
			}
			return this.m_toString;
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x00051971 File Offset: 0x00050971
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x00051988 File Offset: 0x00050988
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
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000519D0 File Offset: 0x000509D0
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
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060020F4 RID: 8436 RVA: 0x00051A16 File Offset: 0x00050A16
		public override string Name
		{
			get
			{
				return this.m_handle.GetName();
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060020F5 RID: 8437 RVA: 0x00051A23 File Offset: 0x00050A23
		[ComVisible(true)]
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Constructor;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060020F6 RID: 8438 RVA: 0x00051A26 File Offset: 0x00050A26
		public override Type DeclaringType
		{
			get
			{
				if (!this.m_reflectedTypeCache.IsGlobal)
				{
					return this.m_declaringType;
				}
				return null;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060020F7 RID: 8439 RVA: 0x00051A3D File Offset: 0x00050A3D
		public override Type ReflectedType
		{
			get
			{
				if (!this.m_reflectedTypeCache.IsGlobal)
				{
					return this.m_reflectedTypeCache.RuntimeType;
				}
				return null;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x00051A59 File Offset: 0x00050A59
		public override int MetadataToken
		{
			get
			{
				return this.m_handle.GetMethodDef();
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x00051A68 File Offset: 0x00050A68
		public override Module Module
		{
			get
			{
				return this.m_declaringType.GetTypeHandleInternal().GetModuleHandle().GetModule();
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00051A90 File Offset: 0x00050A90
		internal override Type GetReturnType()
		{
			return this.Signature.ReturnTypeHandle.GetRuntimeType();
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00051AB0 File Offset: 0x00050AB0
		internal override ParameterInfo[] GetParametersNoCopy()
		{
			if (this.m_parameters == null)
			{
				this.m_parameters = ParameterInfo.GetParameters(this, this, this.Signature);
			}
			return this.m_parameters;
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x00051AD4 File Offset: 0x00050AD4
		public override ParameterInfo[] GetParameters()
		{
			ParameterInfo[] parametersNoCopy = this.GetParametersNoCopy();
			if (parametersNoCopy.Length == 0)
			{
				return parametersNoCopy;
			}
			ParameterInfo[] array = new ParameterInfo[parametersNoCopy.Length];
			Array.Copy(parametersNoCopy, array, parametersNoCopy.Length);
			return array;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00051B03 File Offset: 0x00050B03
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_handle.GetImplAttributes();
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x00051B10 File Offset: 0x00050B10
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

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x00051B57 File Offset: 0x00050B57
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_methodAttributes;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x00051B5F File Offset: 0x00050B5F
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.Signature.CallingConvention;
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00051B6C File Offset: 0x00050B6C
		internal static void CheckCanCreateInstance(Type declaringType, bool isVarArg)
		{
			if (declaringType == null)
			{
				throw new ArgumentNullException("declaringType");
			}
			if (declaringType is ReflectionOnlyType)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyInvoke"));
			}
			if (declaringType.IsInterface)
			{
				throw new MemberAccessException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Acc_CreateInterfaceEx"), new object[]
				{
					declaringType
				}));
			}
			if (declaringType.IsAbstract)
			{
				throw new MemberAccessException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Acc_CreateAbstEx"), new object[]
				{
					declaringType
				}));
			}
			if (declaringType.GetRootElementType() == typeof(ArgIterator))
			{
				throw new NotSupportedException();
			}
			if (isVarArg)
			{
				throw new NotSupportedException();
			}
			if (declaringType.ContainsGenericParameters)
			{
				throw new MemberAccessException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Acc_CreateGenericEx"), new object[]
				{
					declaringType
				}));
			}
			if (declaringType == typeof(void))
			{
				throw new MemberAccessException(Environment.GetResourceString("Access_Void"));
			}
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00051C67 File Offset: 0x00050C67
		internal void ThrowNoInvokeException()
		{
			RuntimeConstructorInfo.CheckCanCreateInstance(this.DeclaringType, (this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs);
			if ((this.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
			{
				throw new MemberAccessException(Environment.GetResourceString("Acc_NotClassInit"));
			}
			throw new TargetException();
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00051CA4 File Offset: 0x00050CA4
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			if (this.m_invocationFlags == 0U)
			{
				this.m_invocationFlags = this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 2U) != 0U)
			{
				this.ThrowNoInvokeException();
			}
			this.CheckConsistency(obj);
			if (obj != null)
			{
				new SecurityPermission(SecurityPermissionFlag.SkipVerification).Demand();
			}
			if ((this.m_invocationFlags & 36U) != 0U)
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
			int num = this.Signature.Arguments.Length;
			int num2 = (parameters != null) ? parameters.Length : 0;
			if (num != num2)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
			}
			if (num2 > 0)
			{
				object[] array = base.CheckArguments(parameters, binder, invokeAttr, culture, this.Signature);
				object result = this.m_handle.InvokeMethodFast(obj, array, this.Signature, this.m_methodAttributes, (this.ReflectedType != null) ? this.ReflectedType.TypeHandle : RuntimeTypeHandle.EmptyHandle);
				for (int i = 0; i < num2; i++)
				{
					parameters[i] = array[i];
				}
				return result;
			}
			return this.m_handle.InvokeMethodFast(obj, null, this.Signature, this.m_methodAttributes, (this.DeclaringType != null) ? this.DeclaringType.TypeHandle : RuntimeTypeHandle.EmptyHandle);
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00051E00 File Offset: 0x00050E00
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

		// Token: 0x06002105 RID: 8453 RVA: 0x00051E2C File Offset: 0x00050E2C
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			RuntimeTypeHandle typeHandle = this.m_declaringType.TypeHandle;
			if (this.m_invocationFlags == 0U)
			{
				this.m_invocationFlags = this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 266U) != 0U)
			{
				this.ThrowNoInvokeException();
			}
			if ((this.m_invocationFlags & 164U) != 0U)
			{
				if ((this.m_invocationFlags & 32U) != 0U)
				{
					CodeAccessPermission.DemandInternal(PermissionType.ReflectionMemberAccess);
				}
				if ((this.m_invocationFlags & 4U) != 0U)
				{
					MethodBase.PerformSecurityCheck(null, this.m_handle, this.m_declaringType.TypeHandle.Value, this.m_invocationFlags & 268435456U);
				}
				if ((this.m_invocationFlags & 128U) != 0U)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				}
			}
			int num = this.Signature.Arguments.Length;
			int num2 = (parameters != null) ? parameters.Length : 0;
			if (num != num2)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
			}
			RuntimeHelpers.RunClassConstructor(typeHandle);
			if (num2 > 0)
			{
				object[] array = base.CheckArguments(parameters, binder, invokeAttr, culture, this.Signature);
				object result = this.m_handle.InvokeConstructor(array, this.Signature, typeHandle);
				for (int i = 0; i < num2; i++)
				{
					parameters[i] = array[i];
				}
				return result;
			}
			return this.m_handle.InvokeConstructor(null, this.Signature, typeHandle);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00051F70 File Offset: 0x00050F70
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedTypeHandle.GetRuntimeType(), this.ToString(), MemberTypes.Constructor);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00051FAC File Offset: 0x00050FAC
		internal void SerializationInvoke(object target, SerializationInfo info, StreamingContext context)
		{
			this.MethodHandle.SerializationInvoke(target, this.Signature, info, context);
		}

		// Token: 0x04000DF5 RID: 3573
		private RuntimeMethodHandle m_handle;

		// Token: 0x04000DF6 RID: 3574
		private RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000DF7 RID: 3575
		private RuntimeType m_declaringType;

		// Token: 0x04000DF8 RID: 3576
		private string m_toString;

		// Token: 0x04000DF9 RID: 3577
		private MethodAttributes m_methodAttributes;

		// Token: 0x04000DFA RID: 3578
		private BindingFlags m_bindingFlags;

		// Token: 0x04000DFB RID: 3579
		private ParameterInfo[] m_parameters;

		// Token: 0x04000DFC RID: 3580
		private uint m_invocationFlags;

		// Token: 0x04000DFD RID: 3581
		private Signature m_signature;
	}
}
