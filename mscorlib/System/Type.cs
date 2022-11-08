using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Cache;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	// Token: 0x020000F7 RID: 247
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_Type))]
	[ComVisible(true)]
	[Serializable]
	public abstract class Type : MemberInfo, _Type, IReflect
	{
		// Token: 0x06000D3E RID: 3390 RVA: 0x00025CB4 File Offset: 0x00024CB4
		static Type()
		{
			System.__Filters @object = new System.__Filters();
			Type.FilterAttribute = new MemberFilter(@object.FilterAttribute);
			Type.FilterName = new MemberFilter(@object.FilterName);
			Type.FilterNameIgnoreCase = new MemberFilter(@object.FilterIgnoreCase);
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00025D4E File Offset: 0x00024D4E
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.TypeInfo;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00025D52 File Offset: 0x00024D52
		public override Type DeclaringType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x00025D55 File Offset: 0x00024D55
		public virtual MethodBase DeclaringMethod
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00025D58 File Offset: 0x00024D58
		public override Type ReflectedType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00025D5B File Offset: 0x00024D5B
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00025D64 File Offset: 0x00024D64
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName, bool throwOnError, bool ignoreCase)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeType.PrivateGetType(typeName, throwOnError, ignoreCase, ref stackCrawlMark);
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00025D80 File Offset: 0x00024D80
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName, bool throwOnError)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeType.PrivateGetType(typeName, throwOnError, false, ref stackCrawlMark);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00025D9C File Offset: 0x00024D9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeType.PrivateGetType(typeName, false, false, ref stackCrawlMark);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00025DB8 File Offset: 0x00024DB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type ReflectionOnlyGetType(string typeName, bool throwIfNotFound, bool ignoreCase)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeType.PrivateGetType(typeName, throwIfNotFound, ignoreCase, true, ref stackCrawlMark);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00025DD2 File Offset: 0x00024DD2
		public virtual Type MakePointerType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00025DD9 File Offset: 0x00024DD9
		public virtual StructLayoutAttribute StructLayoutAttribute
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00025DE0 File Offset: 0x00024DE0
		public virtual Type MakeByRefType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00025DE7 File Offset: 0x00024DE7
		public virtual Type MakeArrayType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00025DEE File Offset: 0x00024DEE
		public virtual Type MakeArrayType(int rank)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00025DF5 File Offset: 0x00024DF5
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Type GetTypeFromProgID(string progID)
		{
			return RuntimeType.GetTypeFromProgIDImpl(progID, null, false);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00025DFF File Offset: 0x00024DFF
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Type GetTypeFromProgID(string progID, bool throwOnError)
		{
			return RuntimeType.GetTypeFromProgIDImpl(progID, null, throwOnError);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00025E09 File Offset: 0x00024E09
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Type GetTypeFromProgID(string progID, string server)
		{
			return RuntimeType.GetTypeFromProgIDImpl(progID, server, false);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00025E13 File Offset: 0x00024E13
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Type GetTypeFromProgID(string progID, string server, bool throwOnError)
		{
			return RuntimeType.GetTypeFromProgIDImpl(progID, server, throwOnError);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00025E1D File Offset: 0x00024E1D
		public static Type GetTypeFromCLSID(Guid clsid)
		{
			return RuntimeType.GetTypeFromCLSIDImpl(clsid, null, false);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00025E27 File Offset: 0x00024E27
		public static Type GetTypeFromCLSID(Guid clsid, bool throwOnError)
		{
			return RuntimeType.GetTypeFromCLSIDImpl(clsid, null, throwOnError);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00025E31 File Offset: 0x00024E31
		public static Type GetTypeFromCLSID(Guid clsid, string server)
		{
			return RuntimeType.GetTypeFromCLSIDImpl(clsid, server, false);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00025E3B File Offset: 0x00024E3B
		public static Type GetTypeFromCLSID(Guid clsid, string server, bool throwOnError)
		{
			return RuntimeType.GetTypeFromCLSIDImpl(clsid, server, throwOnError);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00025E48 File Offset: 0x00024E48
		internal string SigToString()
		{
			Type type = this;
			while (type.HasElementType)
			{
				type = type.GetElementType();
			}
			if (type.IsNested)
			{
				return this.Name;
			}
			string text = this.ToString();
			if (type.IsPrimitive || type == typeof(void) || type == typeof(TypedReference))
			{
				text = text.Substring("System.".Length);
			}
			return text;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00025EB2 File Offset: 0x00024EB2
		public static TypeCode GetTypeCode(Type type)
		{
			if (type == null)
			{
				return TypeCode.Empty;
			}
			return type.GetTypeCodeInternal();
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00025EC0 File Offset: 0x00024EC0
		internal virtual TypeCode GetTypeCodeInternal()
		{
			if (this is SymbolType)
			{
				return TypeCode.Object;
			}
			if (this is TypeBuilder)
			{
				TypeBuilder typeBuilder = (TypeBuilder)this;
				if (!typeBuilder.IsEnum)
				{
					return TypeCode.Object;
				}
			}
			if (this != this.UnderlyingSystemType)
			{
				return Type.GetTypeCode(this.UnderlyingSystemType);
			}
			return TypeCode.Object;
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000D59 RID: 3417
		public abstract Guid GUID { get; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00025F08 File Offset: 0x00024F08
		public static Binder DefaultBinder
		{
			get
			{
				if (Type.defaultBinder == null)
				{
					Type.CreateBinder();
				}
				return Type.defaultBinder as Binder;
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00025F20 File Offset: 0x00024F20
		private static void CreateBinder()
		{
			if (Type.defaultBinder == null)
			{
				object value = new DefaultBinder();
				Interlocked.CompareExchange(ref Type.defaultBinder, value, null);
			}
		}

		// Token: 0x06000D5C RID: 3420
		public abstract object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x06000D5D RID: 3421 RVA: 0x00025F48 File Offset: 0x00024F48
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, CultureInfo culture)
		{
			return this.InvokeMember(name, invokeAttr, binder, target, args, null, culture, null);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00025F68 File Offset: 0x00024F68
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args)
		{
			return this.InvokeMember(name, invokeAttr, binder, target, args, null, null, null);
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000D5F RID: 3423
		public new abstract Module Module { get; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000D60 RID: 3424
		public abstract Assembly Assembly { get; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000D61 RID: 3425
		public virtual extern RuntimeTypeHandle TypeHandle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x06000D62 RID: 3426 RVA: 0x00025F85 File Offset: 0x00024F85
		internal virtual RuntimeTypeHandle GetTypeHandleInternal()
		{
			return this.TypeHandle;
		}

		// Token: 0x06000D63 RID: 3427
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern RuntimeTypeHandle GetTypeHandle(object o);

		// Token: 0x06000D64 RID: 3428
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Type GetTypeFromHandle(RuntimeTypeHandle handle);

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000D65 RID: 3429
		public abstract string FullName { get; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000D66 RID: 3430
		public abstract string Namespace { get; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000D67 RID: 3431
		public abstract string AssemblyQualifiedName { get; }

		// Token: 0x06000D68 RID: 3432 RVA: 0x00025F8D File Offset: 0x00024F8D
		public virtual int GetArrayRank()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000D69 RID: 3433
		public abstract Type BaseType { get; }

		// Token: 0x06000D6A RID: 3434 RVA: 0x00025FA0 File Offset: 0x00024FA0
		[ComVisible(true)]
		public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetConstructorImpl(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00025FEC File Offset: 0x00024FEC
		[ComVisible(true)]
		public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetConstructorImpl(bindingAttr, binder, CallingConventions.Any, types, modifiers);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00026031 File Offset: 0x00025031
		[ComVisible(true)]
		public ConstructorInfo GetConstructor(Type[] types)
		{
			return this.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, types, null);
		}

		// Token: 0x06000D6D RID: 3437
		protected abstract ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000D6E RID: 3438 RVA: 0x0002603E File Offset: 0x0002503E
		[ComVisible(true)]
		public ConstructorInfo[] GetConstructors()
		{
			return this.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x06000D6F RID: 3439
		[ComVisible(true)]
		public abstract ConstructorInfo[] GetConstructors(BindingFlags bindingAttr);

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x00026048 File Offset: 0x00025048
		[ComVisible(true)]
		public ConstructorInfo TypeInitializer
		{
			get
			{
				return this.GetConstructorImpl(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, Type.EmptyTypes, null);
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0002605C File Offset: 0x0002505C
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000260B8 File Offset: 0x000250B8
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, bindingAttr, binder, CallingConventions.Any, types, modifiers);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00026110 File Offset: 0x00025110
		public MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, types, modifiers);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00026164 File Offset: 0x00025164
		public MethodInfo GetMethod(string name, Type[] types)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, types, null);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x000261B8 File Offset: 0x000251B8
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetMethodImpl(name, bindingAttr, null, CallingConventions.Any, null, null);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x000261D4 File Offset: 0x000251D4
		public MethodInfo GetMethod(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, null, null);
		}

		// Token: 0x06000D77 RID: 3447
		protected abstract MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000D78 RID: 3448 RVA: 0x000261F1 File Offset: 0x000251F1
		public MethodInfo[] GetMethods()
		{
			return this.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D79 RID: 3449
		public abstract MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x06000D7A RID: 3450
		public abstract FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x06000D7B RID: 3451 RVA: 0x000261FB File Offset: 0x000251FB
		public FieldInfo GetField(string name)
		{
			return this.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00026206 File Offset: 0x00025206
		public FieldInfo[] GetFields()
		{
			return this.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D7D RID: 3453
		public abstract FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x06000D7E RID: 3454 RVA: 0x00026210 File Offset: 0x00025210
		public Type GetInterface(string name)
		{
			return this.GetInterface(name, false);
		}

		// Token: 0x06000D7F RID: 3455
		public abstract Type GetInterface(string name, bool ignoreCase);

		// Token: 0x06000D80 RID: 3456
		public abstract Type[] GetInterfaces();

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002621C File Offset: 0x0002521C
		public virtual Type[] FindInterfaces(TypeFilter filter, object filterCriteria)
		{
			if (filter == null)
			{
				throw new ArgumentNullException("filter");
			}
			Type[] interfaces = this.GetInterfaces();
			int num = 0;
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (!filter(interfaces[i], filterCriteria))
				{
					interfaces[i] = null;
				}
				else
				{
					num++;
				}
			}
			if (num == interfaces.Length)
			{
				return interfaces;
			}
			Type[] array = new Type[num];
			num = 0;
			for (int j = 0; j < interfaces.Length; j++)
			{
				if (interfaces[j] != null)
				{
					array[num++] = interfaces[j];
				}
			}
			return array;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00026299 File Offset: 0x00025299
		public EventInfo GetEvent(string name)
		{
			return this.GetEvent(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D83 RID: 3459
		public abstract EventInfo GetEvent(string name, BindingFlags bindingAttr);

		// Token: 0x06000D84 RID: 3460 RVA: 0x000262A4 File Offset: 0x000252A4
		public virtual EventInfo[] GetEvents()
		{
			return this.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D85 RID: 3461
		public abstract EventInfo[] GetEvents(BindingFlags bindingAttr);

		// Token: 0x06000D86 RID: 3462 RVA: 0x000262AE File Offset: 0x000252AE
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			return this.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000262DC File Offset: 0x000252DC
		public PropertyInfo GetProperty(string name, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			return this.GetPropertyImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, returnType, types, modifiers);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00026308 File Offset: 0x00025308
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetPropertyImpl(name, bindingAttr, null, null, null, null);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00026324 File Offset: 0x00025324
		public PropertyInfo GetProperty(string name, Type returnType, Type[] types)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			return this.GetPropertyImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, returnType, types, null);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0002634F File Offset: 0x0002534F
		public PropertyInfo GetProperty(string name, Type[] types)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			return this.GetPropertyImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, null, types, null);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0002637A File Offset: 0x0002537A
		public PropertyInfo GetProperty(string name, Type returnType)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (returnType == null)
			{
				throw new ArgumentNullException("returnType");
			}
			return this.GetPropertyImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, returnType, null, null);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000263A5 File Offset: 0x000253A5
		public PropertyInfo GetProperty(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetPropertyImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, null, null, null);
		}

		// Token: 0x06000D8D RID: 3469
		protected abstract PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000D8E RID: 3470
		public abstract PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x06000D8F RID: 3471 RVA: 0x000263C2 File Offset: 0x000253C2
		public PropertyInfo[] GetProperties()
		{
			return this.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x000263CC File Offset: 0x000253CC
		public Type[] GetNestedTypes()
		{
			return this.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D91 RID: 3473
		public abstract Type[] GetNestedTypes(BindingFlags bindingAttr);

		// Token: 0x06000D92 RID: 3474 RVA: 0x000263D6 File Offset: 0x000253D6
		public Type GetNestedType(string name)
		{
			return this.GetNestedType(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D93 RID: 3475
		public abstract Type GetNestedType(string name, BindingFlags bindingAttr);

		// Token: 0x06000D94 RID: 3476 RVA: 0x000263E1 File Offset: 0x000253E1
		public MemberInfo[] GetMember(string name)
		{
			return this.GetMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000263EC File Offset: 0x000253EC
		public virtual MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return this.GetMember(name, MemberTypes.All, bindingAttr);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x000263FB File Offset: 0x000253FB
		public virtual MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0002640C File Offset: 0x0002540C
		public MemberInfo[] GetMembers()
		{
			return this.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D98 RID: 3480
		public abstract MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x06000D99 RID: 3481 RVA: 0x00026418 File Offset: 0x00025418
		public virtual MemberInfo[] GetDefaultMembers()
		{
			string text = (string)base.Cache[CacheObjType.DefaultMember];
			if (text == null)
			{
				CustomAttributeData customAttributeData = null;
				for (Type type = this; type != null; type = type.BaseType)
				{
					IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(type);
					for (int i = 0; i < customAttributes.Count; i++)
					{
						if (customAttributes[i].Constructor.DeclaringType == typeof(DefaultMemberAttribute))
						{
							customAttributeData = customAttributes[i];
							break;
						}
					}
					if (customAttributeData != null)
					{
						break;
					}
				}
				if (customAttributeData == null)
				{
					return new MemberInfo[0];
				}
				text = (customAttributeData.ConstructorArguments[0].Value as string);
				base.Cache[CacheObjType.DefaultMember] = text;
			}
			MemberInfo[] array = this.GetMember(text);
			if (array == null)
			{
				array = new MemberInfo[0];
			}
			return array;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x000264E0 File Offset: 0x000254E0
		internal virtual string GetDefaultMemberName()
		{
			string text = (string)base.Cache[CacheObjType.DefaultMember];
			if (text == null)
			{
				object[] customAttributes = this.GetCustomAttributes(typeof(DefaultMemberAttribute), true);
				if (customAttributes.Length > 1)
				{
					throw new ExecutionEngineException(Environment.GetResourceString("ExecutionEngine_InvalidAttribute"));
				}
				if (customAttributes.Length == 0)
				{
					return null;
				}
				text = ((DefaultMemberAttribute)customAttributes[0]).MemberName;
				base.Cache[CacheObjType.DefaultMember] = text;
			}
			return text;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00026550 File Offset: 0x00025550
		public virtual MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
		{
			MethodInfo[] array = null;
			ConstructorInfo[] array2 = null;
			FieldInfo[] array3 = null;
			PropertyInfo[] array4 = null;
			EventInfo[] array5 = null;
			Type[] array6 = null;
			int num = 0;
			if ((memberType & MemberTypes.Method) != (MemberTypes)0)
			{
				array = this.GetMethods(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (!filter(array[i], filterCriteria))
						{
							array[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array.Length;
				}
			}
			if ((memberType & MemberTypes.Constructor) != (MemberTypes)0)
			{
				array2 = this.GetConstructors(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array2.Length; i++)
					{
						if (!filter(array2[i], filterCriteria))
						{
							array2[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array2.Length;
				}
			}
			if ((memberType & MemberTypes.Field) != (MemberTypes)0)
			{
				array3 = this.GetFields(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array3.Length; i++)
					{
						if (!filter(array3[i], filterCriteria))
						{
							array3[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array3.Length;
				}
			}
			if ((memberType & MemberTypes.Property) != (MemberTypes)0)
			{
				array4 = this.GetProperties(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array4.Length; i++)
					{
						if (!filter(array4[i], filterCriteria))
						{
							array4[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array4.Length;
				}
			}
			if ((memberType & MemberTypes.Event) != (MemberTypes)0)
			{
				array5 = this.GetEvents();
				if (filter != null)
				{
					for (int i = 0; i < array5.Length; i++)
					{
						if (!filter(array5[i], filterCriteria))
						{
							array5[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array5.Length;
				}
			}
			if ((memberType & MemberTypes.NestedType) != (MemberTypes)0)
			{
				array6 = this.GetNestedTypes(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array6.Length; i++)
					{
						if (!filter(array6[i], filterCriteria))
						{
							array6[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array6.Length;
				}
			}
			MemberInfo[] array7 = new MemberInfo[num];
			num = 0;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						array7[num++] = array[i];
					}
				}
			}
			if (array2 != null)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i] != null)
					{
						array7[num++] = array2[i];
					}
				}
			}
			if (array3 != null)
			{
				for (int i = 0; i < array3.Length; i++)
				{
					if (array3[i] != null)
					{
						array7[num++] = array3[i];
					}
				}
			}
			if (array4 != null)
			{
				for (int i = 0; i < array4.Length; i++)
				{
					if (array4[i] != null)
					{
						array7[num++] = array4[i];
					}
				}
			}
			if (array5 != null)
			{
				for (int i = 0; i < array5.Length; i++)
				{
					if (array5[i] != null)
					{
						array7[num++] = array5[i];
					}
				}
			}
			if (array6 != null)
			{
				for (int i = 0; i < array6.Length; i++)
				{
					if (array6[i] != null)
					{
						array7[num++] = array6[i];
					}
				}
			}
			return array7;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x00026835 File Offset: 0x00025835
		public bool IsNested
		{
			get
			{
				return this.DeclaringType != null;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00026843 File Offset: 0x00025843
		public TypeAttributes Attributes
		{
			get
			{
				return this.GetAttributeFlagsImpl();
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x0002684B File Offset: 0x0002584B
		public virtual GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00026854 File Offset: 0x00025854
		public bool IsVisible
		{
			get
			{
				return this.GetTypeHandleInternal().IsVisible();
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0002686F File Offset: 0x0002586F
		public bool IsNotPublic
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0002687C File Offset: 0x0002587C
		public bool IsPublic
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.Public;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00026889 File Offset: 0x00025889
		public bool IsNestedPublic
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x00026896 File Offset: 0x00025896
		public bool IsNestedPrivate
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x000268A3 File Offset: 0x000258A3
		public bool IsNestedFamily
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x000268B0 File Offset: 0x000258B0
		public bool IsNestedAssembly
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedAssembly;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x000268BD File Offset: 0x000258BD
		public bool IsNestedFamANDAssem
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamANDAssem;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x000268CA File Offset: 0x000258CA
		public bool IsNestedFamORAssem
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.VisibilityMask;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x000268D7 File Offset: 0x000258D7
		public bool IsAutoLayout
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.LayoutMask) == TypeAttributes.NotPublic;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x000268E5 File Offset: 0x000258E5
		public bool IsLayoutSequential
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x000268F3 File Offset: 0x000258F3
		public bool IsExplicitLayout
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x00026902 File Offset: 0x00025902
		public bool IsClass
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic && !this.IsSubclassOf(Type.valueType);
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00026920 File Offset: 0x00025920
		public bool IsInterface
		{
			get
			{
				if (this is RuntimeType)
				{
					return this.GetTypeHandleInternal().IsInterface();
				}
				return (this.GetAttributeFlagsImpl() & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x00026951 File Offset: 0x00025951
		public bool IsValueType
		{
			get
			{
				return this.IsValueTypeImpl();
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x00026959 File Offset: 0x00025959
		public bool IsAbstract
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.Abstract) != TypeAttributes.NotPublic;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0002696D File Offset: 0x0002596D
		public bool IsSealed
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.Sealed) != TypeAttributes.NotPublic;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00026981 File Offset: 0x00025981
		public bool IsEnum
		{
			get
			{
				return this.IsSubclassOf(Type.enumType);
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0002698E File Offset: 0x0002598E
		public bool IsSpecialName
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.SpecialName) != TypeAttributes.NotPublic;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x000269A2 File Offset: 0x000259A2
		public bool IsImport
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.Import) != TypeAttributes.NotPublic;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x000269B6 File Offset: 0x000259B6
		public bool IsSerializable
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.Serializable) != TypeAttributes.NotPublic || this.QuickSerializationCastCheck();
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x000269D0 File Offset: 0x000259D0
		private bool QuickSerializationCastCheck()
		{
			for (Type type = this.UnderlyingSystemType; type != null; type = type.BaseType)
			{
				if (type == typeof(Enum) || type == typeof(Delegate))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00026A0D File Offset: 0x00025A0D
		public bool IsAnsiClass
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.StringFormatMask) == TypeAttributes.NotPublic;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00026A1E File Offset: 0x00025A1E
		public bool IsUnicodeClass
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.StringFormatMask) == TypeAttributes.UnicodeClass;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00026A33 File Offset: 0x00025A33
		public bool IsAutoClass
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.StringFormatMask) == TypeAttributes.AutoClass;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00026A48 File Offset: 0x00025A48
		public bool IsArray
		{
			get
			{
				return this.IsArrayImpl();
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00026A50 File Offset: 0x00025A50
		internal virtual bool IsSzArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00026A53 File Offset: 0x00025A53
		public virtual bool IsGenericType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00026A56 File Offset: 0x00025A56
		public virtual bool IsGenericTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00026A59 File Offset: 0x00025A59
		public virtual bool IsGenericParameter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00026A5C File Offset: 0x00025A5C
		public virtual int GenericParameterPosition
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00026A70 File Offset: 0x00025A70
		public virtual bool ContainsGenericParameters
		{
			get
			{
				if (this.HasElementType)
				{
					return this.GetRootElementType().ContainsGenericParameters;
				}
				if (this.IsGenericParameter)
				{
					return true;
				}
				if (!this.IsGenericType)
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

		// Token: 0x06000DBF RID: 3519 RVA: 0x00026AC7 File Offset: 0x00025AC7
		public virtual Type[] GetGenericParameterConstraints()
		{
			if (!this.IsGenericParameter)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00026AE6 File Offset: 0x00025AE6
		public bool IsByRef
		{
			get
			{
				return this.IsByRefImpl();
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00026AEE File Offset: 0x00025AEE
		public bool IsPointer
		{
			get
			{
				return this.IsPointerImpl();
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00026AF6 File Offset: 0x00025AF6
		public bool IsPrimitive
		{
			get
			{
				return this.IsPrimitiveImpl();
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00026AFE File Offset: 0x00025AFE
		public bool IsCOMObject
		{
			get
			{
				return this.IsCOMObjectImpl();
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00026B06 File Offset: 0x00025B06
		public bool HasElementType
		{
			get
			{
				return this.HasElementTypeImpl();
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00026B0E File Offset: 0x00025B0E
		public bool IsContextful
		{
			get
			{
				return this.IsContextfulImpl();
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00026B16 File Offset: 0x00025B16
		public bool IsMarshalByRef
		{
			get
			{
				return this.IsMarshalByRefImpl();
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00026B1E File Offset: 0x00025B1E
		internal bool HasProxyAttribute
		{
			get
			{
				return this.HasProxyAttributeImpl();
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00026B28 File Offset: 0x00025B28
		protected virtual bool IsValueTypeImpl()
		{
			return this != Type.valueType && this != Type.enumType && this.IsSubclassOf(Type.valueType);
		}

		// Token: 0x06000DC9 RID: 3529
		protected abstract TypeAttributes GetAttributeFlagsImpl();

		// Token: 0x06000DCA RID: 3530
		protected abstract bool IsArrayImpl();

		// Token: 0x06000DCB RID: 3531
		protected abstract bool IsByRefImpl();

		// Token: 0x06000DCC RID: 3532
		protected abstract bool IsPointerImpl();

		// Token: 0x06000DCD RID: 3533
		protected abstract bool IsPrimitiveImpl();

		// Token: 0x06000DCE RID: 3534
		protected abstract bool IsCOMObjectImpl();

		// Token: 0x06000DCF RID: 3535 RVA: 0x00026B54 File Offset: 0x00025B54
		public virtual Type MakeGenericType(params Type[] typeArguments)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00026B65 File Offset: 0x00025B65
		protected virtual bool IsContextfulImpl()
		{
			return typeof(ContextBoundObject).IsAssignableFrom(this);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00026B77 File Offset: 0x00025B77
		protected virtual bool IsMarshalByRefImpl()
		{
			return typeof(MarshalByRefObject).IsAssignableFrom(this);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x00026B89 File Offset: 0x00025B89
		internal virtual bool HasProxyAttributeImpl()
		{
			return false;
		}

		// Token: 0x06000DD3 RID: 3539
		public abstract Type GetElementType();

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00026B8C File Offset: 0x00025B8C
		public virtual Type[] GetGenericArguments()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00026B9D File Offset: 0x00025B9D
		public virtual Type GetGenericTypeDefinition()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x06000DD6 RID: 3542
		protected abstract bool HasElementTypeImpl();

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00026BB0 File Offset: 0x00025BB0
		internal virtual Type GetRootElementType()
		{
			Type type = this;
			while (type.HasElementType)
			{
				type = type.GetElementType();
			}
			return type;
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000DD8 RID: 3544
		public abstract Type UnderlyingSystemType { get; }

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00026BD4 File Offset: 0x00025BD4
		[ComVisible(true)]
		public virtual bool IsSubclassOf(Type c)
		{
			Type type = this;
			if (type == c)
			{
				return false;
			}
			while (type != null)
			{
				if (type == c)
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00026BFC File Offset: 0x00025BFC
		public virtual bool IsInstanceOfType(object o)
		{
			if (this is RuntimeType)
			{
				return this.IsInstanceOfType(o);
			}
			if (o == null)
			{
				return false;
			}
			if (RemotingServices.IsTransparentProxy(o))
			{
				return null != RemotingServices.CheckCast(o, this);
			}
			if (this.IsInterface && o.GetType().IsCOMObject && this is RuntimeType)
			{
				return ((RuntimeType)this).SupportsInterface(o);
			}
			return this.IsAssignableFrom(o.GetType());
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00026C6C File Offset: 0x00025C6C
		public virtual bool IsAssignableFrom(Type c)
		{
			if (c == null)
			{
				return false;
			}
			try
			{
				RuntimeType runtimeType = c.UnderlyingSystemType as RuntimeType;
				RuntimeType runtimeType2 = this.UnderlyingSystemType as RuntimeType;
				if (runtimeType != null && runtimeType2 != null)
				{
					return RuntimeType.CanCastTo(runtimeType, runtimeType2);
				}
				TypeBuilder typeBuilder = c as TypeBuilder;
				if (typeBuilder == null)
				{
					return false;
				}
				if (TypeBuilder.IsTypeEqual(this, c))
				{
					return true;
				}
				if (typeBuilder.IsSubclassOf(this))
				{
					return true;
				}
				if (!this.IsInterface)
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
			catch (ArgumentException)
			{
			}
			if (this.IsInterface)
			{
				Type[] interfaces2 = c.GetInterfaces();
				for (int j = 0; j < interfaces2.Length; j++)
				{
					if (this == interfaces2[j])
					{
						return true;
					}
				}
			}
			else
			{
				if (this.IsGenericParameter)
				{
					Type[] genericParameterConstraints = this.GetGenericParameterConstraints();
					for (int k = 0; k < genericParameterConstraints.Length; k++)
					{
						if (!genericParameterConstraints[k].IsAssignableFrom(c))
						{
							return false;
						}
					}
					return true;
				}
				while (c != null)
				{
					if (c == this)
					{
						return true;
					}
					c = c.BaseType;
				}
			}
			return false;
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00026DC8 File Offset: 0x00025DC8
		public override string ToString()
		{
			return "Type: " + this.Name;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00026DDC File Offset: 0x00025DDC
		public static Type[] GetTypeArray(object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			Type[] array = new Type[args.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (args[i] == null)
				{
					throw new ArgumentNullException();
				}
				array[i] = args[i].GetType();
			}
			return array;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00026E25 File Offset: 0x00025E25
		public override bool Equals(object o)
		{
			return o != null && o is Type && this.UnderlyingSystemType == ((Type)o).UnderlyingSystemType;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00026E49 File Offset: 0x00025E49
		public bool Equals(Type o)
		{
			return o != null && this.UnderlyingSystemType == o.UnderlyingSystemType;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00026E60 File Offset: 0x00025E60
		public override int GetHashCode()
		{
			Type underlyingSystemType = this.UnderlyingSystemType;
			if (underlyingSystemType != this)
			{
				return underlyingSystemType.GetHashCode();
			}
			return base.GetHashCode();
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00026E85 File Offset: 0x00025E85
		[ComVisible(true)]
		public virtual InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00026E98 File Offset: 0x00025E98
		internal static Type ResolveTypeRelativeTo(string typeName, int offset, int count, Type serverType)
		{
			Type type = Type.ResolveTypeRelativeToBaseTypes(typeName, offset, count, serverType);
			if (type == null)
			{
				Type[] interfaces = serverType.GetInterfaces();
				foreach (Type type2 in interfaces)
				{
					string fullName = type2.FullName;
					if (fullName.Length == count && string.CompareOrdinal(typeName, offset, fullName, 0, count) == 0)
					{
						return type2;
					}
				}
			}
			return type;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00026EFC File Offset: 0x00025EFC
		internal static Type ResolveTypeRelativeToBaseTypes(string typeName, int offset, int count, Type serverType)
		{
			if (typeName == null || serverType == null)
			{
				return null;
			}
			string fullName = serverType.FullName;
			if (fullName.Length == count && string.CompareOrdinal(typeName, offset, fullName, 0, count) == 0)
			{
				return serverType;
			}
			return Type.ResolveTypeRelativeToBaseTypes(typeName, offset, count, serverType.BaseType);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00026F3D File Offset: 0x00025F3D
		void _Type.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00026F44 File Offset: 0x00025F44
		void _Type.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00026F4B File Offset: 0x00025F4B
		void _Type.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00026F52 File Offset: 0x00025F52
		void _Type.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040004D6 RID: 1238
		private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x040004D7 RID: 1239
		public static readonly MemberFilter FilterAttribute;

		// Token: 0x040004D8 RID: 1240
		public static readonly MemberFilter FilterName;

		// Token: 0x040004D9 RID: 1241
		public static readonly MemberFilter FilterNameIgnoreCase;

		// Token: 0x040004DA RID: 1242
		public static readonly object Missing = System.Reflection.Missing.Value;

		// Token: 0x040004DB RID: 1243
		public static readonly char Delimiter = '.';

		// Token: 0x040004DC RID: 1244
		public static readonly Type[] EmptyTypes = new Type[0];

		// Token: 0x040004DD RID: 1245
		private static object defaultBinder;

		// Token: 0x040004DE RID: 1246
		private static readonly Type valueType = typeof(ValueType);

		// Token: 0x040004DF RID: 1247
		private static readonly Type enumType = typeof(Enum);

		// Token: 0x040004E0 RID: 1248
		private static readonly Type objectType = typeof(object);
	}
}
