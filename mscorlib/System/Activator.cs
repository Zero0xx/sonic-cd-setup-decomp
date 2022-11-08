using System;
using System.Configuration.Assemblies;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace System
{
	// Token: 0x02000046 RID: 70
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_Activator))]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class Activator : _Activator
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x00010166 File Offset: 0x0000F166
		private Activator()
		{
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001016E File Offset: 0x0000F16E
		public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture)
		{
			return Activator.CreateInstance(type, bindingAttr, binder, args, culture, null);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001017C File Offset: 0x0000F17C
		public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type is TypeBuilder)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_CreateInstanceWithTypeBuilder"));
			}
			if ((bindingAttr & (BindingFlags)255) == BindingFlags.Default)
			{
				bindingAttr |= (BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance);
			}
			if (activationAttributes != null && activationAttributes.Length > 0)
			{
				if (!type.IsMarshalByRef)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_ActivAttrOnNonMBR"));
				}
				if (!type.IsContextful && (activationAttributes.Length > 1 || !(activationAttributes[0] is UrlAttribute)))
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonUrlAttrOnMBR"));
				}
			}
			RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "type");
			}
			return runtimeType.CreateInstanceImpl(bindingAttr, binder, args, culture, activationAttributes);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001023F File Offset: 0x0000F23F
		public static object CreateInstance(Type type, params object[] args)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, args, null, null);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00010250 File Offset: 0x0000F250
		public static object CreateInstance(Type type, object[] args, object[] activationAttributes)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, args, null, activationAttributes);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00010261 File Offset: 0x0000F261
		public static object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type, false);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001026C File Offset: 0x0000F26C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, null, null, ref stackCrawlMark);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00010290 File Offset: 0x0000F290
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, activationAttributes, null, ref stackCrawlMark);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x000102B4 File Offset: 0x0000F2B4
		public static object CreateInstance(Type type, bool nonPublic)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "type");
			}
			return runtimeType.CreateInstanceImpl(!nonPublic);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00010300 File Offset: 0x0000F300
		internal static object InternalCreateInstanceWithNoMemberAccessCheck(Type type, bool nonPublic)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "type");
			}
			return runtimeType.CreateInstanceImpl(!nonPublic, false, false);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001034C File Offset: 0x0000F34C
		public static T CreateInstance<T>()
		{
			bool flag = true;
			bool flag2 = false;
			RuntimeMethodHandle emptyHandle = RuntimeMethodHandle.EmptyHandle;
			return (T)((object)RuntimeTypeHandle.CreateInstance(typeof(T) as RuntimeType, true, true, ref flag2, ref emptyHandle, ref flag));
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00010384 File Offset: 0x0000F384
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName)
		{
			return Activator.CreateInstanceFrom(assemblyFile, typeName, null);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00010390 File Offset: 0x0000F390
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes)
		{
			return Activator.CreateInstanceFrom(assemblyFile, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, activationAttributes, null);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000103B0 File Offset: 0x0000F3B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityInfo, ref stackCrawlMark);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000103D4 File Offset: 0x0000F3D4
		internal static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo, ref StackCrawlMark stackMark)
		{
			Assembly assembly;
			if (assemblyName == null)
			{
				assembly = Assembly.nGetExecutingAssembly(ref stackMark);
			}
			else
			{
				assembly = Assembly.InternalLoad(assemblyName, securityInfo, ref stackMark, false);
			}
			if (assembly == null)
			{
				return null;
			}
			Type type = assembly.GetType(typeName, true, ignoreCase);
			object obj = Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00010428 File Offset: 0x0000F428
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyFile, securityInfo);
			Type type = assembly.GetType(typeName, true, ignoreCase);
			object obj = Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00010465 File Offset: 0x0000F465
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00010480 File Offset: 0x0000F480
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000104B0 File Offset: 0x0000F4B0
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000104C8 File Offset: 0x0000F4C8
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000104F8 File Offset: 0x0000F4F8
		public static ObjectHandle CreateInstance(ActivationContext activationContext)
		{
			AppDomainManager appDomainManager = AppDomain.CurrentDomain.DomainManager;
			if (appDomainManager == null)
			{
				appDomainManager = new AppDomainManager();
			}
			return appDomainManager.ApplicationActivator.CreateInstance(activationContext);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00010528 File Offset: 0x0000F528
		public static ObjectHandle CreateInstance(ActivationContext activationContext, string[] activationCustomData)
		{
			AppDomainManager appDomainManager = AppDomain.CurrentDomain.DomainManager;
			if (appDomainManager == null)
			{
				appDomainManager = new AppDomainManager();
			}
			return appDomainManager.ApplicationActivator.CreateInstance(activationContext, activationCustomData);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00010556 File Offset: 0x0000F556
		public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName)
		{
			return Activator.CreateComInstanceFrom(assemblyName, typeName, null, AssemblyHashAlgorithm.None);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00010564 File Offset: 0x0000F564
		public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyName, null, hashValue, hashAlgorithm);
			Type type = assembly.GetType(typeName, true, false);
			object[] customAttributes = type.GetCustomAttributes(typeof(ComVisibleAttribute), false);
			if (customAttributes.Length > 0 && !((ComVisibleAttribute)customAttributes[0]).Value)
			{
				throw new TypeLoadException(Environment.GetResourceString("Argument_TypeMustBeVisibleFromCom"));
			}
			if (assembly == null)
			{
				return null;
			}
			object obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, null);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x000105E0 File Offset: 0x0000F5E0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static object GetObject(Type type, string url)
		{
			return Activator.GetObject(type, url, null);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000105EA File Offset: 0x0000F5EA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static object GetObject(Type type, string url, object state)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RemotingServices.Connect(type, url, state);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00010602 File Offset: 0x0000F602
		[Conditional("_DEBUG")]
		private static void Log(bool test, string title, string success, string failure)
		{
			if (test)
			{
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00010607 File Offset: 0x0000F607
		void _Activator.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001060E File Offset: 0x0000F60E
		void _Activator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00010615 File Offset: 0x0000F615
		void _Activator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001061C File Offset: 0x0000F61C
		void _Activator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000181 RID: 385
		internal const int LookupMask = 255;

		// Token: 0x04000182 RID: 386
		internal const BindingFlags ConLookup = BindingFlags.Instance | BindingFlags.Public;

		// Token: 0x04000183 RID: 387
		internal const BindingFlags ConstructorDefault = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
	}
}
