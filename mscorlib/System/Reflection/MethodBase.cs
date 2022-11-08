using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x02000346 RID: 838
	[ComDefaultInterface(typeof(_MethodBase))]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class MethodBase : MemberInfo, _MethodBase
	{
		// Token: 0x06002009 RID: 8201 RVA: 0x00050280 File Offset: 0x0004F280
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHandle"));
			}
			MethodBase methodBase = RuntimeType.GetMethodBase(handle);
			if (methodBase.DeclaringType != null && methodBase.DeclaringType.IsGenericType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_MethodDeclaringTypeGeneric"), new object[]
				{
					methodBase,
					methodBase.DeclaringType.GetGenericTypeDefinition()
				}));
			}
			return methodBase;
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000502F7 File Offset: 0x0004F2F7
		[ComVisible(false)]
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHandle"));
			}
			return RuntimeType.GetMethodBase(declaringType, handle);
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0005031C File Offset: 0x0004F31C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MethodBase GetCurrentMethod()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeMethodInfo.InternalGetCurrentMethod(ref stackCrawlMark);
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x0600200D RID: 8205 RVA: 0x0005033A File Offset: 0x0004F33A
		internal virtual bool IsOverloaded
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_Method"));
			}
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x0005034B File Offset: 0x0004F34B
		internal virtual RuntimeMethodHandle GetMethodHandle()
		{
			return this.MethodHandle;
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x00050353 File Offset: 0x0004F353
		internal virtual Type GetReturnType()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0005035A File Offset: 0x0004F35A
		internal virtual ParameterInfo[] GetParametersNoCopy()
		{
			return this.GetParameters();
		}

		// Token: 0x06002011 RID: 8209
		public abstract ParameterInfo[] GetParameters();

		// Token: 0x06002012 RID: 8210
		public abstract MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002013 RID: 8211
		public abstract RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06002014 RID: 8212
		public abstract MethodAttributes Attributes { get; }

		// Token: 0x06002015 RID: 8213
		public abstract object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x00050362 File Offset: 0x0004F362
		public virtual CallingConventions CallingConvention
		{
			get
			{
				return CallingConventions.Standard;
			}
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x00050365 File Offset: 0x0004F365
		[ComVisible(true)]
		public virtual Type[] GetGenericArguments()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x00050376 File Offset: 0x0004F376
		public virtual bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x00050379 File Offset: 0x0004F379
		public virtual bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x0005037C File Offset: 0x0004F37C
		public virtual bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0005037F File Offset: 0x0004F37F
		Type _MethodBase.GetType()
		{
			return base.GetType();
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x00050387 File Offset: 0x0004F387
		bool _MethodBase.IsPublic
		{
			get
			{
				return this.IsPublic;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x0005038F File Offset: 0x0004F38F
		bool _MethodBase.IsPrivate
		{
			get
			{
				return this.IsPrivate;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x00050397 File Offset: 0x0004F397
		bool _MethodBase.IsFamily
		{
			get
			{
				return this.IsFamily;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x0005039F File Offset: 0x0004F39F
		bool _MethodBase.IsAssembly
		{
			get
			{
				return this.IsAssembly;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06002020 RID: 8224 RVA: 0x000503A7 File Offset: 0x0004F3A7
		bool _MethodBase.IsFamilyAndAssembly
		{
			get
			{
				return this.IsFamilyAndAssembly;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x000503AF File Offset: 0x0004F3AF
		bool _MethodBase.IsFamilyOrAssembly
		{
			get
			{
				return this.IsFamilyOrAssembly;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x000503B7 File Offset: 0x0004F3B7
		bool _MethodBase.IsStatic
		{
			get
			{
				return this.IsStatic;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x000503BF File Offset: 0x0004F3BF
		bool _MethodBase.IsFinal
		{
			get
			{
				return this.IsFinal;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x000503C7 File Offset: 0x0004F3C7
		bool _MethodBase.IsVirtual
		{
			get
			{
				return this.IsVirtual;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x000503CF File Offset: 0x0004F3CF
		bool _MethodBase.IsHideBySig
		{
			get
			{
				return this.IsHideBySig;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x000503D7 File Offset: 0x0004F3D7
		bool _MethodBase.IsAbstract
		{
			get
			{
				return this.IsAbstract;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x000503DF File Offset: 0x0004F3DF
		bool _MethodBase.IsSpecialName
		{
			get
			{
				return this.IsSpecialName;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x000503E7 File Offset: 0x0004F3E7
		bool _MethodBase.IsConstructor
		{
			get
			{
				return this.IsConstructor;
			}
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x000503EF File Offset: 0x0004F3EF
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object Invoke(object obj, object[] parameters)
		{
			return this.Invoke(obj, BindingFlags.Default, null, parameters, null);
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x000503FC File Offset: 0x0004F3FC
		public bool IsPublic
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x00050409 File Offset: 0x0004F409
		public bool IsPrivate
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x00050416 File Offset: 0x0004F416
		public bool IsFamily
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x00050423 File Offset: 0x0004F423
		public bool IsAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x00050430 File Offset: 0x0004F430
		public bool IsFamilyAndAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x0005043D File Offset: 0x0004F43D
		public bool IsFamilyOrAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x0005044A File Offset: 0x0004F44A
		public bool IsStatic
		{
			get
			{
				return (this.Attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x0005045B File Offset: 0x0004F45B
		public bool IsFinal
		{
			get
			{
				return (this.Attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x0005046C File Offset: 0x0004F46C
		public bool IsVirtual
		{
			get
			{
				return (this.Attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x0005047D File Offset: 0x0004F47D
		public bool IsHideBySig
		{
			get
			{
				return (this.Attributes & MethodAttributes.HideBySig) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06002034 RID: 8244 RVA: 0x00050491 File Offset: 0x0004F491
		public bool IsAbstract
		{
			get
			{
				return (this.Attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06002035 RID: 8245 RVA: 0x000504A5 File Offset: 0x0004F4A5
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06002036 RID: 8246 RVA: 0x000504B9 File Offset: 0x0004F4B9
		[ComVisible(true)]
		public bool IsConstructor
		{
			get
			{
				return (this.Attributes & MethodAttributes.RTSpecialName) != MethodAttributes.PrivateScope && this.Name.Equals(ConstructorInfo.ConstructorName);
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000504DB File Offset: 0x0004F4DB
		[ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public virtual MethodBody GetMethodBody()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06002038 RID: 8248
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetSpecialSecurityFlags(RuntimeMethodHandle method);

		// Token: 0x06002039 RID: 8249
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PerformSecurityCheck(object obj, RuntimeMethodHandle method, IntPtr parent, uint invocationFlags);

		// Token: 0x0600203A RID: 8250 RVA: 0x000504E4 File Offset: 0x0004F4E4
		internal virtual Type[] GetParameterTypes()
		{
			ParameterInfo[] parametersNoCopy = this.GetParametersNoCopy();
			Type[] array = new Type[parametersNoCopy.Length];
			for (int i = 0; i < parametersNoCopy.Length; i++)
			{
				array[i] = parametersNoCopy[i].ParameterType;
			}
			return array;
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x00050520 File Offset: 0x0004F520
		internal virtual uint GetOneTimeFlags()
		{
			RuntimeMethodHandle methodHandle = this.MethodHandle;
			uint num = 0U;
			Type declaringType = this.DeclaringType;
			if (this.ContainsGenericParameters || (declaringType != null && declaringType.ContainsGenericParameters) || (this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs || (this.Attributes & MethodAttributes.RequireSecObject) == MethodAttributes.RequireSecObject)
			{
				num |= 2U;
			}
			else
			{
				AssemblyBuilderData assemblyData = this.Module.Assembly.m_assemblyData;
				if (assemblyData != null && (assemblyData.m_access & AssemblyBuilderAccess.Run) == (AssemblyBuilderAccess)0)
				{
					num |= 2U;
				}
			}
			if (num == 0U)
			{
				num |= MethodBase.GetSpecialSecurityFlags(methodHandle);
				if ((num & 4U) == 0U)
				{
					if ((this.Attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.Public || (declaringType != null && !declaringType.IsVisible))
					{
						num |= 4U;
					}
					else if (this.IsGenericMethod)
					{
						Type[] genericArguments = this.GetGenericArguments();
						for (int i = 0; i < genericArguments.Length; i++)
						{
							if (!genericArguments[i].IsVisible)
							{
								num |= 4U;
								break;
							}
						}
					}
				}
			}
			num |= this.GetOneTimeSpecificFlags();
			return num | 1U;
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00050606 File Offset: 0x0004F606
		internal virtual uint GetOneTimeSpecificFlags()
		{
			return 0U;
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x0005060C File Offset: 0x0004F60C
		internal object[] CheckArguments(object[] parameters, Binder binder, BindingFlags invokeAttr, CultureInfo culture, Signature sig)
		{
			int num = (parameters != null) ? parameters.Length : 0;
			object[] array = new object[num];
			ParameterInfo[] array2 = null;
			for (int i = 0; i < num; i++)
			{
				object obj = parameters[i];
				RuntimeTypeHandle runtimeTypeHandle = sig.Arguments[i];
				if (obj == Type.Missing)
				{
					if (array2 == null)
					{
						array2 = this.GetParametersNoCopy();
					}
					if (array2[i].DefaultValue == DBNull.Value)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_VarMissNull"), "parameters");
					}
					obj = array2[i].DefaultValue;
				}
				if (runtimeTypeHandle.IsInstanceOfType(obj))
				{
					array[i] = obj;
				}
				else
				{
					array[i] = runtimeTypeHandle.GetRuntimeType().CheckValue(obj, binder, culture, invokeAttr);
				}
			}
			return array;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x000506C0 File Offset: 0x0004F6C0
		void _MethodBase.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x000506C7 File Offset: 0x0004F6C7
		void _MethodBase.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x000506CE File Offset: 0x0004F6CE
		void _MethodBase.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000506D5 File Offset: 0x0004F6D5
		void _MethodBase.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}
