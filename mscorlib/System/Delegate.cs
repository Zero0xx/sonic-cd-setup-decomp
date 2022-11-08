using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200003C RID: 60
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ComVisible(true)]
	[Serializable]
	public abstract class Delegate : ICloneable, ISerializable
	{
		// Token: 0x06000387 RID: 903 RVA: 0x0000E430 File Offset: 0x0000D430
		protected Delegate(object target, string method)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (!this.BindToMethodName(target, Type.GetTypeHandle(target), method, (DelegateBindingFlags)10))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTargMeth"));
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000E484 File Offset: 0x0000D484
		protected Delegate(Type target, string method)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (!(target is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "target");
			}
			if (target.IsGenericType && target.ContainsGenericParameters)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_UnboundGenParam"), "target");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.BindToMethodName(null, target.TypeHandle, method, (DelegateBindingFlags)37);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000E506 File Offset: 0x0000D506
		private Delegate()
		{
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000E50E File Offset: 0x0000D50E
		public object DynamicInvoke(params object[] args)
		{
			return this.DynamicInvokeImpl(args);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000E518 File Offset: 0x0000D518
		protected virtual object DynamicInvokeImpl(object[] args)
		{
			RuntimeMethodHandle methodHandle = new RuntimeMethodHandle(this.GetInvokeMethod());
			RuntimeTypeHandle typeHandle = Type.GetTypeHandle(this);
			RuntimeMethodInfo runtimeMethodInfo = (RuntimeMethodInfo)RuntimeType.GetMethodBase(typeHandle, methodHandle);
			return runtimeMethodInfo.Invoke(this, BindingFlags.Default, null, args, null, true);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000E554 File Offset: 0x0000D554
		public override bool Equals(object obj)
		{
			if (obj == null || !Delegate.InternalEqualTypes(this, obj))
			{
				return false;
			}
			Delegate @delegate = (Delegate)obj;
			if (this._target == @delegate._target && this._methodPtr == @delegate._methodPtr && this._methodPtrAux == @delegate._methodPtrAux)
			{
				return true;
			}
			if (this._methodPtrAux.IsNull())
			{
				if (!@delegate._methodPtrAux.IsNull())
				{
					return false;
				}
				if (this._target != @delegate._target)
				{
					return false;
				}
			}
			else
			{
				if (@delegate._methodPtrAux.IsNull())
				{
					return false;
				}
				if (this._methodPtrAux == @delegate._methodPtrAux)
				{
					return true;
				}
			}
			if (this._methodBase == null || @delegate._methodBase == null)
			{
				return this.FindMethodHandle().Equals(@delegate.FindMethodHandle());
			}
			return this._methodBase.Equals(@delegate._methodBase);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000E632 File Offset: 0x0000D632
		public override int GetHashCode()
		{
			return base.GetType().GetHashCode();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000E63F File Offset: 0x0000D63F
		public static Delegate Combine(Delegate a, Delegate b)
		{
			if (a == null)
			{
				return b;
			}
			return a.CombineImpl(b);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000E650 File Offset: 0x0000D650
		[ComVisible(true)]
		public static Delegate Combine(params Delegate[] delegates)
		{
			if (delegates == null || delegates.Length == 0)
			{
				return null;
			}
			Delegate @delegate = delegates[0];
			for (int i = 1; i < delegates.Length; i++)
			{
				@delegate = Delegate.Combine(@delegate, delegates[i]);
			}
			return @delegate;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000E684 File Offset: 0x0000D684
		public virtual Delegate[] GetInvocationList()
		{
			return new Delegate[]
			{
				this
			};
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000E69D File Offset: 0x0000D69D
		public MethodInfo Method
		{
			get
			{
				return this.GetMethodImpl();
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000E6A8 File Offset: 0x0000D6A8
		protected virtual MethodInfo GetMethodImpl()
		{
			if (this._methodBase == null)
			{
				RuntimeMethodHandle methodHandle = this.FindMethodHandle();
				RuntimeTypeHandle reflectedTypeHandle = methodHandle.GetDeclaringType();
				if ((reflectedTypeHandle.IsGenericTypeDefinition() || reflectedTypeHandle.HasInstantiation()) && (methodHandle.GetAttributes() & MethodAttributes.Static) == MethodAttributes.PrivateScope)
				{
					if (this._methodPtrAux == (IntPtr)0)
					{
						Type type = this._target.GetType();
						Type genericTypeDefinition = reflectedTypeHandle.GetRuntimeType().GetGenericTypeDefinition();
						while (!type.IsGenericType || type.GetGenericTypeDefinition() != genericTypeDefinition)
						{
							type = type.BaseType;
						}
						reflectedTypeHandle = type.TypeHandle;
					}
					else
					{
						MethodInfo method = base.GetType().GetMethod("Invoke");
						reflectedTypeHandle = method.GetParameters()[0].ParameterType.TypeHandle;
					}
				}
				this._methodBase = (MethodInfo)RuntimeType.GetMethodBase(reflectedTypeHandle, methodHandle);
			}
			return (MethodInfo)this._methodBase;
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000E78C File Offset: 0x0000D78C
		public object Target
		{
			get
			{
				return this.GetTarget();
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000E794 File Offset: 0x0000D794
		public static Delegate Remove(Delegate source, Delegate value)
		{
			if (source == null)
			{
				return null;
			}
			if (value == null)
			{
				return source;
			}
			if (!Delegate.InternalEqualTypes(source, value))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTypeMis"));
			}
			return source.RemoveImpl(value);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000E7C0 File Offset: 0x0000D7C0
		public static Delegate RemoveAll(Delegate source, Delegate value)
		{
			Delegate @delegate;
			do
			{
				@delegate = source;
				source = Delegate.Remove(source, value);
			}
			while (@delegate != source);
			return @delegate;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000E7E4 File Offset: 0x0000D7E4
		protected virtual Delegate CombineImpl(Delegate d)
		{
			throw new MulticastNotSupportedException(Environment.GetResourceString("Multicast_Combine"));
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000E7F5 File Offset: 0x0000D7F5
		protected virtual Delegate RemoveImpl(Delegate d)
		{
			if (!d.Equals(this))
			{
				return this;
			}
			return null;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000E803 File Offset: 0x0000D803
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000E80B File Offset: 0x0000D80B
		public static Delegate CreateDelegate(Type type, object target, string method)
		{
			return Delegate.CreateDelegate(type, target, method, false, true);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000E817 File Offset: 0x0000D817
		public static Delegate CreateDelegate(Type type, object target, string method, bool ignoreCase)
		{
			return Delegate.CreateDelegate(type, target, method, ignoreCase, true);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000E824 File Offset: 0x0000D824
		public static Delegate CreateDelegate(Type type, object target, string method, bool ignoreCase, bool throwOnBindFailure)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "type");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			Type baseType = type.BaseType;
			if (baseType == null || baseType != typeof(MulticastDelegate))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDelegate"), "type");
			}
			Delegate @delegate = Delegate.InternalAlloc(type.TypeHandle);
			if (!@delegate.BindToMethodName(target, Type.GetTypeHandle(target), method, (DelegateBindingFlags)26 | (ignoreCase ? DelegateBindingFlags.CaselessMatching : ((DelegateBindingFlags)0))))
			{
				if (throwOnBindFailure)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTargMeth"));
				}
				@delegate = null;
			}
			return @delegate;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000E8E2 File Offset: 0x0000D8E2
		public static Delegate CreateDelegate(Type type, Type target, string method)
		{
			return Delegate.CreateDelegate(type, target, method, false, true);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000E8EE File Offset: 0x0000D8EE
		public static Delegate CreateDelegate(Type type, Type target, string method, bool ignoreCase)
		{
			return Delegate.CreateDelegate(type, target, method, ignoreCase, true);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000E8FC File Offset: 0x0000D8FC
		public static Delegate CreateDelegate(Type type, Type target, string method, bool ignoreCase, bool throwOnBindFailure)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "type");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (!(target is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "target");
			}
			if (target.IsGenericType && target.ContainsGenericParameters)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_UnboundGenParam"), "target");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			Type baseType = type.BaseType;
			if (baseType == null || baseType != typeof(MulticastDelegate))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDelegate"), "type");
			}
			Delegate @delegate = Delegate.InternalAlloc(type.TypeHandle);
			if (!@delegate.BindToMethodName(null, target.TypeHandle, method, (DelegateBindingFlags)5 | (ignoreCase ? DelegateBindingFlags.CaselessMatching : ((DelegateBindingFlags)0))))
			{
				if (throwOnBindFailure)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTargMeth"));
				}
				@delegate = null;
			}
			return @delegate;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000E9FB File Offset: 0x0000D9FB
		public static Delegate CreateDelegate(Type type, MethodInfo method)
		{
			return Delegate.CreateDelegate(type, method, true);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000EA08 File Offset: 0x0000DA08
		public static Delegate CreateDelegate(Type type, MethodInfo method, bool throwOnBindFailure)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "type");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (!(method is RuntimeMethodInfo))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "method");
			}
			Type baseType = type.BaseType;
			if (baseType == null || baseType != typeof(MulticastDelegate))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDelegate"), "type");
			}
			Delegate @delegate = Delegate.InternalAlloc(type.TypeHandle);
			if (!@delegate.BindToMethodInfo(null, method.MethodHandle, method.DeclaringType.TypeHandle, (DelegateBindingFlags)132))
			{
				if (throwOnBindFailure)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTargMeth"));
				}
				@delegate = null;
			}
			return @delegate;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000EAD8 File Offset: 0x0000DAD8
		public static Delegate CreateDelegate(Type type, object firstArgument, MethodInfo method)
		{
			return Delegate.CreateDelegate(type, firstArgument, method, true);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000EAE4 File Offset: 0x0000DAE4
		public static Delegate CreateDelegate(Type type, object firstArgument, MethodInfo method, bool throwOnBindFailure)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "type");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (!(method is RuntimeMethodInfo))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "method");
			}
			Type baseType = type.BaseType;
			if (baseType == null || baseType != typeof(MulticastDelegate))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDelegate"), "type");
			}
			Delegate @delegate = Delegate.InternalAlloc(type.TypeHandle);
			if (!@delegate.BindToMethodInfo(firstArgument, method.MethodHandle, method.DeclaringType.TypeHandle, DelegateBindingFlags.RelaxedSignature))
			{
				if (throwOnBindFailure)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTargMeth"));
				}
				@delegate = null;
			}
			return @delegate;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000EBB4 File Offset: 0x0000DBB4
		public static bool operator ==(Delegate d1, Delegate d2)
		{
			if (d1 == null)
			{
				return d2 == null;
			}
			return d1.Equals(d2);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000EBC5 File Offset: 0x0000DBC5
		public static bool operator !=(Delegate d1, Delegate d2)
		{
			if (d1 == null)
			{
				return d2 != null;
			}
			return !d1.Equals(d2);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000EBDC File Offset: 0x0000DBDC
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000EBE4 File Offset: 0x0000DBE4
		internal static Delegate CreateDelegate(Type type, object target, RuntimeMethodHandle method)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "type");
			}
			if (method.IsNullHandle())
			{
				throw new ArgumentNullException("method");
			}
			Type baseType = type.BaseType;
			if (baseType == null || baseType != typeof(MulticastDelegate))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDelegate"), "type");
			}
			Delegate @delegate = Delegate.InternalAlloc(type.TypeHandle);
			if (!@delegate.BindToMethodInfo(target, method, method.GetDeclaringType(), DelegateBindingFlags.RelaxedSignature))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTargMeth"));
			}
			return @delegate;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000EC90 File Offset: 0x0000DC90
		internal static Delegate InternalCreateDelegate(Type type, object firstArgument, MethodInfo method)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			Type baseType = type.BaseType;
			if (baseType == null || baseType != typeof(MulticastDelegate))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDelegate"), "type");
			}
			Delegate @delegate = Delegate.InternalAlloc(type.TypeHandle);
			if (!@delegate.BindToMethodInfo(firstArgument, method.MethodHandle, method.DeclaringType.TypeHandle, (DelegateBindingFlags)192))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTargMeth"));
			}
			return @delegate;
		}

		// Token: 0x060003A8 RID: 936
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool BindToMethodName(object target, RuntimeTypeHandle methodType, string method, DelegateBindingFlags flags);

		// Token: 0x060003A9 RID: 937
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool BindToMethodInfo(object target, RuntimeMethodHandle method, RuntimeTypeHandle methodType, DelegateBindingFlags flags);

		// Token: 0x060003AA RID: 938
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern MulticastDelegate InternalAlloc(RuntimeTypeHandle type);

		// Token: 0x060003AB RID: 939
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern MulticastDelegate InternalAllocLike(Delegate d);

		// Token: 0x060003AC RID: 940
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalEqualTypes(object a, object b);

		// Token: 0x060003AD RID: 941
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DelegateConstruct(object target, IntPtr slot);

		// Token: 0x060003AE RID: 942
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr GetMulticastInvoke();

		// Token: 0x060003AF RID: 943
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr GetInvokeMethod();

		// Token: 0x060003B0 RID: 944
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeMethodHandle FindMethodHandle();

		// Token: 0x060003B1 RID: 945
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr GetUnmanagedCallSite();

		// Token: 0x060003B2 RID: 946
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr AdjustTarget(object target, IntPtr methodPtr);

		// Token: 0x060003B3 RID: 947
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr GetCallStub(IntPtr methodPtr);

		// Token: 0x060003B4 RID: 948 RVA: 0x0000ED21 File Offset: 0x0000DD21
		internal virtual object GetTarget()
		{
			if (!this._methodPtrAux.IsNull())
			{
				return null;
			}
			return this._target;
		}

		// Token: 0x04000109 RID: 265
		internal object _target;

		// Token: 0x0400010A RID: 266
		internal MethodBase _methodBase;

		// Token: 0x0400010B RID: 267
		internal IntPtr _methodPtr;

		// Token: 0x0400010C RID: 268
		internal IntPtr _methodPtrAux;
	}
}
