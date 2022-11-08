using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection.Emit
{
	// Token: 0x02000820 RID: 2080
	[ComVisible(true)]
	public sealed class DynamicMethod : MethodInfo
	{
		// Token: 0x060049D1 RID: 18897 RVA: 0x00100EB0 File Offset: 0x000FFEB0
		private DynamicMethod()
		{
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00100EB8 File Offset: 0x000FFEB8
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes)
		{
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, null, false, true);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x00100EDC File Offset: 0x000FFEDC
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, bool restrictedSkipVisibility)
		{
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, null, restrictedSkipVisibility, true);
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x00100F00 File Offset: 0x000FFF00
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(m, ref stackCrawlMark, false);
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, m, false, false);
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x00100F30 File Offset: 0x000FFF30
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(m, ref stackCrawlMark, skipVisibility);
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, m, skipVisibility, false);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x00100F64 File Offset: 0x000FFF64
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(m, ref stackCrawlMark, skipVisibility);
			this.Init(name, attributes, callingConvention, returnType, parameterTypes, null, m, skipVisibility, false);
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x00100F98 File Offset: 0x000FFF98
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(owner, ref stackCrawlMark, false);
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, owner, null, false, false);
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x00100FC8 File Offset: 0x000FFFC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(owner, ref stackCrawlMark, skipVisibility);
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, owner, null, skipVisibility, false);
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x00100FFC File Offset: 0x000FFFFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(owner, ref stackCrawlMark, skipVisibility);
			this.Init(name, attributes, callingConvention, returnType, parameterTypes, owner, null, skipVisibility, false);
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x00101030 File Offset: 0x00100030
		private static void CheckConsistency(MethodAttributes attributes, CallingConventions callingConvention)
		{
			if ((attributes & ~MethodAttributes.MemberAccessMask) != MethodAttributes.Static)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
			}
			if ((attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.Public)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
			}
			if (callingConvention != CallingConventions.Standard && callingConvention != CallingConventions.VarArgs)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
			}
			if (callingConvention == CallingConventions.VarArgs)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
			}
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x00101098 File Offset: 0x00100098
		private static Module GetDynamicMethodsModule()
		{
			if (DynamicMethod.s_anonymouslyHostedDynamicMethodsModule != null)
			{
				return DynamicMethod.s_anonymouslyHostedDynamicMethodsModule;
			}
			lock (DynamicMethod.s_anonymouslyHostedDynamicMethodsModuleLock)
			{
				if (DynamicMethod.s_anonymouslyHostedDynamicMethodsModule != null)
				{
					return DynamicMethod.s_anonymouslyHostedDynamicMethodsModule;
				}
				ConstructorInfo constructor = typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes);
				CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(constructor, new object[0]);
				CustomAttributeBuilder[] assemblyAttributes = new CustomAttributeBuilder[]
				{
					customAttributeBuilder
				};
				AssemblyName name = new AssemblyName("Anonymously Hosted DynamicMethods Assembly");
				AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run, assemblyAttributes);
				AppDomain.CurrentDomain.PublishAnonymouslyHostedDynamicMethodsAssembly(assemblyBuilder.InternalAssembly);
				DynamicMethod.s_anonymouslyHostedDynamicMethodsModule = assemblyBuilder.ManifestModule;
			}
			return DynamicMethod.s_anonymouslyHostedDynamicMethodsModule;
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x00101158 File Offset: 0x00100158
		private void Init(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] signature, Type owner, Module m, bool skipVisibility, bool transparentMethod)
		{
			DynamicMethod.CheckConsistency(attributes, callingConvention);
			if (signature != null)
			{
				this.m_parameterTypes = new RuntimeType[signature.Length];
				for (int i = 0; i < signature.Length; i++)
				{
					if (signature[i] == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_InvalidTypeInSignature"));
					}
					this.m_parameterTypes[i] = (signature[i].UnderlyingSystemType as RuntimeType);
					if (this.m_parameterTypes[i] == null || this.m_parameterTypes[i] == typeof(void))
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_InvalidTypeInSignature"));
					}
				}
			}
			else
			{
				this.m_parameterTypes = new RuntimeType[0];
			}
			this.m_returnType = ((returnType == null) ? ((RuntimeType)typeof(void)) : (returnType.UnderlyingSystemType as RuntimeType));
			if (this.m_returnType == null || this.m_returnType.IsByRef)
			{
				throw new NotSupportedException(Environment.GetResourceString("Arg_InvalidTypeInRetType"));
			}
			if (transparentMethod)
			{
				this.m_module = DynamicMethod.GetDynamicMethodsModule().ModuleHandle;
				if (skipVisibility)
				{
					this.m_restrictedSkipVisibility = true;
				}
				this.m_creationContext = CompressedStack.Capture();
			}
			else
			{
				this.m_typeOwner = ((owner != null) ? (owner.UnderlyingSystemType as RuntimeType) : null);
				if (this.m_typeOwner != null && (this.m_typeOwner.HasElementType || this.m_typeOwner.ContainsGenericParameters || this.m_typeOwner.IsGenericParameter || this.m_typeOwner.IsInterface))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidTypeForDynamicMethod"));
				}
				this.m_module = ((m != null) ? m.ModuleHandle : this.m_typeOwner.Module.ModuleHandle);
				this.m_skipVisibility = skipVisibility;
			}
			this.m_ilGenerator = null;
			this.m_fInitLocals = true;
			this.m_method = new RuntimeMethodHandle(null);
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_dynMethod = new DynamicMethod.RTDynamicMethod(this, name, attributes, callingConvention);
		}

		// Token: 0x060049DD RID: 18909 RVA: 0x0010133C File Offset: 0x0010033C
		private static void PerformSecurityCheck(Module m, ref StackCrawlMark stackMark, bool skipVisibility)
		{
			if (m == null)
			{
				throw new ArgumentNullException("m");
			}
			if (m.Equals(DynamicMethod.s_anonymouslyHostedDynamicMethodsModule))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"), "m");
			}
			if (skipVisibility)
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
			}
			RuntimeTypeHandle callerType = ModuleHandle.GetCallerType(ref stackMark);
			if (!m.Assembly.AssemblyHandle.Equals(callerType.GetAssemblyHandle()) || m == typeof(object).Module)
			{
				PermissionSet permissionSet;
				PermissionSet permissionSet2;
				m.Assembly.nGetGrantSet(out permissionSet, out permissionSet2);
				if (permissionSet == null)
				{
					permissionSet = new PermissionSet(PermissionState.Unrestricted);
				}
				CodeAccessSecurityEngine.ReflectionTargetDemandHelper(PermissionType.SecurityControlEvidence, permissionSet);
			}
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x001013E0 File Offset: 0x001003E0
		private static void PerformSecurityCheck(Type owner, ref StackCrawlMark stackMark, bool skipVisibility)
		{
			if (owner == null || (owner = (owner.UnderlyingSystemType as RuntimeType)) == null)
			{
				throw new ArgumentNullException("owner");
			}
			RuntimeTypeHandle callerType = ModuleHandle.GetCallerType(ref stackMark);
			if (skipVisibility)
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
			}
			else if (!callerType.Equals(owner.TypeHandle))
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
			}
			if (!owner.Assembly.AssemblyHandle.Equals(callerType.GetAssemblyHandle()) || owner.Module == typeof(object).Module)
			{
				PermissionSet permissionSet;
				PermissionSet permissionSet2;
				owner.Assembly.nGetGrantSet(out permissionSet, out permissionSet2);
				if (permissionSet == null)
				{
					permissionSet = new PermissionSet(PermissionState.Unrestricted);
				}
				CodeAccessSecurityEngine.ReflectionTargetDemandHelper(PermissionType.SecurityControlEvidence, permissionSet);
			}
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x00101490 File Offset: 0x00100490
		[ComVisible(true)]
		public Delegate CreateDelegate(Type delegateType)
		{
			if (this.m_restrictedSkipVisibility)
			{
				RuntimeHelpers._CompileMethod(this.GetMethodDescriptor().Value);
			}
			MulticastDelegate multicastDelegate = (MulticastDelegate)Delegate.CreateDelegate(delegateType, null, this.GetMethodDescriptor());
			multicastDelegate.StoreDynamicMethod(this.GetMethodInfo());
			return multicastDelegate;
		}

		// Token: 0x060049E0 RID: 18912 RVA: 0x001014D8 File Offset: 0x001004D8
		[ComVisible(true)]
		public Delegate CreateDelegate(Type delegateType, object target)
		{
			if (this.m_restrictedSkipVisibility)
			{
				RuntimeHelpers._CompileMethod(this.GetMethodDescriptor().Value);
			}
			MulticastDelegate multicastDelegate = (MulticastDelegate)Delegate.CreateDelegate(delegateType, target, this.GetMethodDescriptor());
			multicastDelegate.StoreDynamicMethod(this.GetMethodInfo());
			return multicastDelegate;
		}

		// Token: 0x060049E1 RID: 18913 RVA: 0x00101520 File Offset: 0x00100520
		internal RuntimeMethodHandle GetMethodDescriptor()
		{
			if (this.m_method.IsNullHandle())
			{
				lock (this)
				{
					if (this.m_method.IsNullHandle())
					{
						if (this.m_DynamicILInfo != null)
						{
							this.m_method = this.m_DynamicILInfo.GetCallableMethod(this.m_module.Value);
						}
						else
						{
							if (this.m_ilGenerator == null || this.m_ilGenerator.m_length == 0)
							{
								throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_BadEmptyMethodBody"), new object[]
								{
									this.Name
								}));
							}
							this.m_method = this.m_ilGenerator.GetCallableMethod(this.m_module.Value);
						}
					}
				}
			}
			return this.m_method;
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x001015F8 File Offset: 0x001005F8
		public override string ToString()
		{
			return this.m_dynMethod.ToString();
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x060049E3 RID: 18915 RVA: 0x00101605 File Offset: 0x00100605
		public override string Name
		{
			get
			{
				return this.m_dynMethod.Name;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x00101612 File Offset: 0x00100612
		public override Type DeclaringType
		{
			get
			{
				return this.m_dynMethod.DeclaringType;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x060049E5 RID: 18917 RVA: 0x0010161F File Offset: 0x0010061F
		public override Type ReflectedType
		{
			get
			{
				return this.m_dynMethod.ReflectedType;
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x060049E6 RID: 18918 RVA: 0x0010162C File Offset: 0x0010062C
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_dynMethod.MetadataTokenInternal;
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x060049E7 RID: 18919 RVA: 0x00101639 File Offset: 0x00100639
		public override Module Module
		{
			get
			{
				return this.m_dynMethod.Module;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x060049E8 RID: 18920 RVA: 0x00101646 File Offset: 0x00100646
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x060049E9 RID: 18921 RVA: 0x00101657 File Offset: 0x00100657
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_dynMethod.Attributes;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x060049EA RID: 18922 RVA: 0x00101664 File Offset: 0x00100664
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_dynMethod.CallingConvention;
			}
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x00101671 File Offset: 0x00100671
		public override MethodInfo GetBaseDefinition()
		{
			return this;
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x00101674 File Offset: 0x00100674
		public override ParameterInfo[] GetParameters()
		{
			return this.m_dynMethod.GetParameters();
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x00101681 File Offset: 0x00100681
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_dynMethod.GetMethodImplementationFlags();
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x00101690 File Offset: 0x00100690
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			RuntimeMethodHandle methodDescriptor = this.GetMethodDescriptor();
			if ((this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_CallToVarArg"));
			}
			RuntimeTypeHandle[] array = new RuntimeTypeHandle[this.m_parameterTypes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.m_parameterTypes[i].TypeHandle;
			}
			Signature signature = new Signature(methodDescriptor, array, this.m_returnType.TypeHandle, this.CallingConvention);
			int num = signature.Arguments.Length;
			int num2 = (parameters != null) ? parameters.Length : 0;
			if (num != num2)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
			}
			object result;
			if (num2 > 0)
			{
				object[] array2 = base.CheckArguments(parameters, binder, invokeAttr, culture, signature);
				result = methodDescriptor.InvokeMethodFast(null, array2, signature, this.Attributes, RuntimeTypeHandle.EmptyHandle);
				for (int j = 0; j < num2; j++)
				{
					parameters[j] = array2[j];
				}
			}
			else
			{
				result = methodDescriptor.InvokeMethodFast(null, null, signature, this.Attributes, RuntimeTypeHandle.EmptyHandle);
			}
			GC.KeepAlive(this);
			return result;
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x001017A5 File Offset: 0x001007A5
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_dynMethod.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x001017B4 File Offset: 0x001007B4
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_dynMethod.GetCustomAttributes(inherit);
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x001017C2 File Offset: 0x001007C2
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_dynMethod.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060049F2 RID: 18930 RVA: 0x001017D1 File Offset: 0x001007D1
		public override Type ReturnType
		{
			get
			{
				return this.m_dynMethod.ReturnType;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x060049F3 RID: 18931 RVA: 0x001017DE File Offset: 0x001007DE
		public override ParameterInfo ReturnParameter
		{
			get
			{
				return this.m_dynMethod.ReturnParameter;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x001017EB File Offset: 0x001007EB
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return this.m_dynMethod.ReturnTypeCustomAttributes;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060049F5 RID: 18933 RVA: 0x001017F8 File Offset: 0x001007F8
		internal override bool IsOverloaded
		{
			get
			{
				return this.m_dynMethod.IsOverloaded;
			}
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x00101808 File Offset: 0x00100808
		public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string parameterName)
		{
			if (position < 0 || position > this.m_parameterTypes.Length)
			{
				throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_ParamSequence"));
			}
			position--;
			if (position >= 0)
			{
				ParameterInfo[] array = this.m_dynMethod.LoadParameters();
				array[position].SetName(parameterName);
				array[position].SetAttributes(attributes);
			}
			return null;
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x0010185C File Offset: 0x0010085C
		public DynamicILInfo GetDynamicILInfo()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			if (this.m_DynamicILInfo != null)
			{
				return this.m_DynamicILInfo;
			}
			return this.GetDynamicILInfo(new DynamicScope());
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x00101884 File Offset: 0x00100884
		internal DynamicILInfo GetDynamicILInfo(DynamicScope scope)
		{
			if (this.m_DynamicILInfo == null)
			{
				byte[] signature = SignatureHelper.GetMethodSigHelper(null, this.CallingConvention, this.ReturnType, null, null, this.m_parameterTypes, null, null).GetSignature(true);
				this.m_DynamicILInfo = new DynamicILInfo(scope, this, signature);
			}
			return this.m_DynamicILInfo;
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x001018D0 File Offset: 0x001008D0
		public ILGenerator GetILGenerator()
		{
			return this.GetILGenerator(64);
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x001018DC File Offset: 0x001008DC
		public ILGenerator GetILGenerator(int streamSize)
		{
			if (this.m_ilGenerator == null)
			{
				byte[] signature = SignatureHelper.GetMethodSigHelper(null, this.CallingConvention, this.ReturnType, null, null, this.m_parameterTypes, null, null).GetSignature(true);
				this.m_ilGenerator = new DynamicILGenerator(this, signature, streamSize);
			}
			return this.m_ilGenerator;
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060049FB RID: 18939 RVA: 0x00101928 File Offset: 0x00100928
		// (set) Token: 0x060049FC RID: 18940 RVA: 0x00101930 File Offset: 0x00100930
		public bool InitLocals
		{
			get
			{
				return this.m_fInitLocals;
			}
			set
			{
				this.m_fInitLocals = value;
			}
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00101939 File Offset: 0x00100939
		internal MethodInfo GetMethodInfo()
		{
			return this.m_dynMethod;
		}

		// Token: 0x040025CB RID: 9675
		private RuntimeType[] m_parameterTypes;

		// Token: 0x040025CC RID: 9676
		private RuntimeType m_returnType;

		// Token: 0x040025CD RID: 9677
		private DynamicILGenerator m_ilGenerator;

		// Token: 0x040025CE RID: 9678
		private DynamicILInfo m_DynamicILInfo;

		// Token: 0x040025CF RID: 9679
		private bool m_fInitLocals;

		// Token: 0x040025D0 RID: 9680
		internal RuntimeMethodHandle m_method;

		// Token: 0x040025D1 RID: 9681
		internal ModuleHandle m_module;

		// Token: 0x040025D2 RID: 9682
		internal bool m_skipVisibility;

		// Token: 0x040025D3 RID: 9683
		internal RuntimeType m_typeOwner;

		// Token: 0x040025D4 RID: 9684
		private DynamicMethod.RTDynamicMethod m_dynMethod;

		// Token: 0x040025D5 RID: 9685
		internal DynamicResolver m_resolver;

		// Token: 0x040025D6 RID: 9686
		internal bool m_restrictedSkipVisibility;

		// Token: 0x040025D7 RID: 9687
		internal CompressedStack m_creationContext;

		// Token: 0x040025D8 RID: 9688
		private static Module s_anonymouslyHostedDynamicMethodsModule;

		// Token: 0x040025D9 RID: 9689
		private static readonly object s_anonymouslyHostedDynamicMethodsModuleLock = new object();

		// Token: 0x02000821 RID: 2081
		internal class RTDynamicMethod : MethodInfo
		{
			// Token: 0x060049FF RID: 18943 RVA: 0x0010194D File Offset: 0x0010094D
			private RTDynamicMethod()
			{
			}

			// Token: 0x06004A00 RID: 18944 RVA: 0x00101955 File Offset: 0x00100955
			internal RTDynamicMethod(DynamicMethod owner, string name, MethodAttributes attributes, CallingConventions callingConvention)
			{
				this.m_owner = owner;
				this.m_name = name;
				this.m_attributes = attributes;
				this.m_callingConvention = callingConvention;
			}

			// Token: 0x06004A01 RID: 18945 RVA: 0x0010197A File Offset: 0x0010097A
			public override string ToString()
			{
				return this.ReturnType.SigToString() + " " + RuntimeMethodInfo.ConstructName(this);
			}

			// Token: 0x17000CB5 RID: 3253
			// (get) Token: 0x06004A02 RID: 18946 RVA: 0x00101997 File Offset: 0x00100997
			public override string Name
			{
				get
				{
					return this.m_name;
				}
			}

			// Token: 0x17000CB6 RID: 3254
			// (get) Token: 0x06004A03 RID: 18947 RVA: 0x0010199F File Offset: 0x0010099F
			public override Type DeclaringType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000CB7 RID: 3255
			// (get) Token: 0x06004A04 RID: 18948 RVA: 0x001019A2 File Offset: 0x001009A2
			public override Type ReflectedType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000CB8 RID: 3256
			// (get) Token: 0x06004A05 RID: 18949 RVA: 0x001019A5 File Offset: 0x001009A5
			internal override int MetadataTokenInternal
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17000CB9 RID: 3257
			// (get) Token: 0x06004A06 RID: 18950 RVA: 0x001019A8 File Offset: 0x001009A8
			public override Module Module
			{
				get
				{
					return this.m_owner.m_module.GetModule();
				}
			}

			// Token: 0x17000CBA RID: 3258
			// (get) Token: 0x06004A07 RID: 18951 RVA: 0x001019BA File Offset: 0x001009BA
			public override RuntimeMethodHandle MethodHandle
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
				}
			}

			// Token: 0x17000CBB RID: 3259
			// (get) Token: 0x06004A08 RID: 18952 RVA: 0x001019CB File Offset: 0x001009CB
			public override MethodAttributes Attributes
			{
				get
				{
					return this.m_attributes;
				}
			}

			// Token: 0x17000CBC RID: 3260
			// (get) Token: 0x06004A09 RID: 18953 RVA: 0x001019D3 File Offset: 0x001009D3
			public override CallingConventions CallingConvention
			{
				get
				{
					return this.m_callingConvention;
				}
			}

			// Token: 0x06004A0A RID: 18954 RVA: 0x001019DB File Offset: 0x001009DB
			public override MethodInfo GetBaseDefinition()
			{
				return this;
			}

			// Token: 0x06004A0B RID: 18955 RVA: 0x001019E0 File Offset: 0x001009E0
			public override ParameterInfo[] GetParameters()
			{
				ParameterInfo[] array = this.LoadParameters();
				ParameterInfo[] array2 = new ParameterInfo[array.Length];
				Array.Copy(array, array2, array.Length);
				return array2;
			}

			// Token: 0x06004A0C RID: 18956 RVA: 0x00101A08 File Offset: 0x00100A08
			public override MethodImplAttributes GetMethodImplementationFlags()
			{
				return MethodImplAttributes.NoInlining;
			}

			// Token: 0x06004A0D RID: 18957 RVA: 0x00101A0B File Offset: 0x00100A0B
			public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "this");
			}

			// Token: 0x06004A0E RID: 18958 RVA: 0x00101A24 File Offset: 0x00100A24
			public override object[] GetCustomAttributes(Type attributeType, bool inherit)
			{
				if (attributeType == null)
				{
					throw new ArgumentNullException("attributeType");
				}
				if (attributeType.IsAssignableFrom(typeof(MethodImplAttribute)))
				{
					return new object[]
					{
						new MethodImplAttribute(this.GetMethodImplementationFlags())
					};
				}
				return new object[0];
			}

			// Token: 0x06004A0F RID: 18959 RVA: 0x00101A70 File Offset: 0x00100A70
			public override object[] GetCustomAttributes(bool inherit)
			{
				return new object[]
				{
					new MethodImplAttribute(this.GetMethodImplementationFlags())
				};
			}

			// Token: 0x06004A10 RID: 18960 RVA: 0x00101A93 File Offset: 0x00100A93
			public override bool IsDefined(Type attributeType, bool inherit)
			{
				if (attributeType == null)
				{
					throw new ArgumentNullException("attributeType");
				}
				return attributeType.IsAssignableFrom(typeof(MethodImplAttribute));
			}

			// Token: 0x06004A11 RID: 18961 RVA: 0x00101AB8 File Offset: 0x00100AB8
			internal override Type GetReturnType()
			{
				return this.m_owner.m_returnType;
			}

			// Token: 0x17000CBD RID: 3261
			// (get) Token: 0x06004A12 RID: 18962 RVA: 0x00101AC5 File Offset: 0x00100AC5
			public override ParameterInfo ReturnParameter
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000CBE RID: 3262
			// (get) Token: 0x06004A13 RID: 18963 RVA: 0x00101AC8 File Offset: 0x00100AC8
			public override ICustomAttributeProvider ReturnTypeCustomAttributes
			{
				get
				{
					return this.GetEmptyCAHolder();
				}
			}

			// Token: 0x17000CBF RID: 3263
			// (get) Token: 0x06004A14 RID: 18964 RVA: 0x00101AD0 File Offset: 0x00100AD0
			internal override bool IsOverloaded
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06004A15 RID: 18965 RVA: 0x00101AD4 File Offset: 0x00100AD4
			internal ParameterInfo[] LoadParameters()
			{
				if (this.m_parameters == null)
				{
					RuntimeType[] parameterTypes = this.m_owner.m_parameterTypes;
					ParameterInfo[] array = new ParameterInfo[parameterTypes.Length];
					for (int i = 0; i < parameterTypes.Length; i++)
					{
						array[i] = new ParameterInfo(this, null, parameterTypes[i], i);
					}
					if (this.m_parameters == null)
					{
						this.m_parameters = array;
					}
				}
				return this.m_parameters;
			}

			// Token: 0x06004A16 RID: 18966 RVA: 0x00101B2F File Offset: 0x00100B2F
			private ICustomAttributeProvider GetEmptyCAHolder()
			{
				return new DynamicMethod.RTDynamicMethod.EmptyCAHolder();
			}

			// Token: 0x040025DA RID: 9690
			internal DynamicMethod m_owner;

			// Token: 0x040025DB RID: 9691
			private ParameterInfo[] m_parameters;

			// Token: 0x040025DC RID: 9692
			private string m_name;

			// Token: 0x040025DD RID: 9693
			private MethodAttributes m_attributes;

			// Token: 0x040025DE RID: 9694
			private CallingConventions m_callingConvention;

			// Token: 0x02000822 RID: 2082
			private class EmptyCAHolder : ICustomAttributeProvider
			{
				// Token: 0x06004A17 RID: 18967 RVA: 0x00101B36 File Offset: 0x00100B36
				internal EmptyCAHolder()
				{
				}

				// Token: 0x06004A18 RID: 18968 RVA: 0x00101B3E File Offset: 0x00100B3E
				object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit)
				{
					return new object[0];
				}

				// Token: 0x06004A19 RID: 18969 RVA: 0x00101B46 File Offset: 0x00100B46
				object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit)
				{
					return new object[0];
				}

				// Token: 0x06004A1A RID: 18970 RVA: 0x00101B4E File Offset: 0x00100B4E
				bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit)
				{
					return false;
				}
			}
		}
	}
}
