using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System
{
	// Token: 0x02000109 RID: 265
	[ComVisible(true)]
	[Serializable]
	public struct RuntimeTypeHandle : ISerializable
	{
		// Token: 0x06000EDF RID: 3807
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsInstanceOfType(object o);

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0002C508 File Offset: 0x0002B508
		internal unsafe static IntPtr GetTypeHelper(IntPtr th, IntPtr pGenericArgs, int cGenericArgs, IntPtr pModifiers, int cModifiers)
		{
			RuntimeTypeHandle runtimeTypeHandle = new RuntimeTypeHandle(th.ToPointer());
			Type type = runtimeTypeHandle.GetRuntimeType();
			if (type == null)
			{
				return th;
			}
			if (cGenericArgs > 0)
			{
				Type[] array = new Type[cGenericArgs];
				void** value = (void**)pGenericArgs.ToPointer();
				for (int i = 0; i < array.Length; i++)
				{
					RuntimeTypeHandle handle = new RuntimeTypeHandle((void*)Marshal.ReadIntPtr((IntPtr)((void*)value), i * sizeof(void*)));
					array[i] = Type.GetTypeFromHandle(handle);
					if (array[i] == null)
					{
						return (IntPtr)0;
					}
				}
				type = type.MakeGenericType(array);
			}
			if (cModifiers > 0)
			{
				int* value2 = (int*)pModifiers.ToPointer();
				for (int j = 0; j < cModifiers; j++)
				{
					if ((byte)Marshal.ReadInt32((IntPtr)((void*)value2), j * 4) == 15)
					{
						type = type.MakePointerType();
					}
					else if ((byte)Marshal.ReadInt32((IntPtr)((void*)value2), j * 4) == 16)
					{
						type = type.MakeByRefType();
					}
					else if ((byte)Marshal.ReadInt32((IntPtr)((void*)value2), j * 4) == 29)
					{
						type = type.MakeArrayType();
					}
					else
					{
						type = type.MakeArrayType(Marshal.ReadInt32((IntPtr)((void*)value2), ++j * 4));
					}
				}
			}
			return type.GetTypeHandleInternal().Value;
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0002C63F File Offset: 0x0002B63F
		public static bool operator ==(RuntimeTypeHandle left, object right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0002C64F File Offset: 0x0002B64F
		public static bool operator ==(object left, RuntimeTypeHandle right)
		{
			return right.Equals(left);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0002C65F File Offset: 0x0002B65F
		public static bool operator !=(RuntimeTypeHandle left, object right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0002C672 File Offset: 0x0002B672
		public static bool operator !=(object left, RuntimeTypeHandle right)
		{
			return !right.Equals(left);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0002C685 File Offset: 0x0002B685
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.m_ptr);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0002C694 File Offset: 0x0002B694
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj is RuntimeTypeHandle && ((RuntimeTypeHandle)obj).m_ptr == this.m_ptr;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0002C6C4 File Offset: 0x0002B6C4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(RuntimeTypeHandle handle)
		{
			return handle.m_ptr == this.m_ptr;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x0002C6D8 File Offset: 0x0002B6D8
		public IntPtr Value
		{
			get
			{
				return this.m_ptr;
			}
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0002C6E0 File Offset: 0x0002B6E0
		internal unsafe RuntimeTypeHandle(void* rth)
		{
			this.m_ptr = new IntPtr(rth);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0002C6EE File Offset: 0x0002B6EE
		internal bool IsNullHandle()
		{
			return this.m_ptr.ToPointer() == null;
		}

		// Token: 0x06000EEB RID: 3819
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object CreateInstance(RuntimeType type, bool publicOnly, bool noCheck, ref bool canBeCached, ref RuntimeMethodHandle ctor, ref bool bNeedSecurityCheck);

		// Token: 0x06000EEC RID: 3820
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object CreateCaInstance(RuntimeMethodHandle ctor);

		// Token: 0x06000EED RID: 3821
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object Allocate();

		// Token: 0x06000EEE RID: 3822
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object CreateInstanceForAnotherGenericParameter(Type genericParameter);

		// Token: 0x06000EEF RID: 3823 RVA: 0x0002C6FF File Offset: 0x0002B6FF
		internal RuntimeType GetRuntimeType()
		{
			if (!this.IsNullHandle())
			{
				return (RuntimeType)Type.GetTypeFromHandle(this);
			}
			return null;
		}

		// Token: 0x06000EF0 RID: 3824
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern CorElementType GetCorElementType();

		// Token: 0x06000EF1 RID: 3825
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetAssemblyHandle();

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0002C71B File Offset: 0x0002B71B
		internal AssemblyHandle GetAssemblyHandle()
		{
			return new AssemblyHandle(this._GetAssemblyHandle());
		}

		// Token: 0x06000EF3 RID: 3827
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetModuleHandle();

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0002C728 File Offset: 0x0002B728
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public ModuleHandle GetModuleHandle()
		{
			return new ModuleHandle(this._GetModuleHandle());
		}

		// Token: 0x06000EF5 RID: 3829
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetBaseTypeHandle();

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0002C735 File Offset: 0x0002B735
		internal RuntimeTypeHandle GetBaseTypeHandle()
		{
			return new RuntimeTypeHandle(this._GetBaseTypeHandle());
		}

		// Token: 0x06000EF7 RID: 3831
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern TypeAttributes GetAttributes();

		// Token: 0x06000EF8 RID: 3832
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetElementType();

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0002C742 File Offset: 0x0002B742
		internal RuntimeTypeHandle GetElementType()
		{
			return new RuntimeTypeHandle(this._GetElementType());
		}

		// Token: 0x06000EFA RID: 3834
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle GetCanonicalHandle();

		// Token: 0x06000EFB RID: 3835
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetArrayRank();

		// Token: 0x06000EFC RID: 3836
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetToken();

		// Token: 0x06000EFD RID: 3837
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetMethodAt(int slot);

		// Token: 0x06000EFE RID: 3838 RVA: 0x0002C74F File Offset: 0x0002B74F
		internal RuntimeMethodHandle GetMethodAt(int slot)
		{
			return new RuntimeMethodHandle(this._GetMethodAt(slot));
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0002C75D File Offset: 0x0002B75D
		internal RuntimeTypeHandle.IntroducedMethodEnumerator IntroducedMethods
		{
			get
			{
				return new RuntimeTypeHandle.IntroducedMethodEnumerator(this);
			}
		}

		// Token: 0x06000F00 RID: 3840
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RuntimeMethodHandle GetFirstIntroducedMethod(RuntimeTypeHandle type);

		// Token: 0x06000F01 RID: 3841
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RuntimeMethodHandle GetNextIntroducedMethod(RuntimeMethodHandle method);

		// Token: 0x06000F02 RID: 3842
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern bool GetFields(int** result, int* count);

		// Token: 0x06000F03 RID: 3843
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle[] GetInterfaces();

		// Token: 0x06000F04 RID: 3844
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle[] GetConstraints();

		// Token: 0x06000F05 RID: 3845
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr GetGCHandle(GCHandleType type);

		// Token: 0x06000F06 RID: 3846
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void FreeGCHandle(IntPtr handle);

		// Token: 0x06000F07 RID: 3847
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetMethodFromToken(int tkMethodDef);

		// Token: 0x06000F08 RID: 3848 RVA: 0x0002C76A File Offset: 0x0002B76A
		internal RuntimeMethodHandle GetMethodFromToken(int tkMethodDef)
		{
			return new RuntimeMethodHandle(this._GetMethodFromToken(tkMethodDef));
		}

		// Token: 0x06000F09 RID: 3849
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetNumVirtuals();

		// Token: 0x06000F0A RID: 3850
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetInterfaceMethodSlots();

		// Token: 0x06000F0B RID: 3851
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetFirstSlotForInterface(IntPtr interfaceHandle);

		// Token: 0x06000F0C RID: 3852 RVA: 0x0002C778 File Offset: 0x0002B778
		internal int GetFirstSlotForInterface(RuntimeTypeHandle interfaceHandle)
		{
			return this.GetFirstSlotForInterface(interfaceHandle.Value);
		}

		// Token: 0x06000F0D RID: 3853
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetInterfaceMethodImplementationSlot(IntPtr interfaceHandle, IntPtr interfaceMethodHandle);

		// Token: 0x06000F0E RID: 3854 RVA: 0x0002C787 File Offset: 0x0002B787
		internal int GetInterfaceMethodImplementationSlot(RuntimeTypeHandle interfaceHandle, RuntimeMethodHandle interfaceMethodHandle)
		{
			return this.GetInterfaceMethodImplementationSlot(interfaceHandle.Value, interfaceMethodHandle.Value);
		}

		// Token: 0x06000F0F RID: 3855
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsComObject(bool isGenericCOM);

		// Token: 0x06000F10 RID: 3856
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsContextful();

		// Token: 0x06000F11 RID: 3857
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsInterface();

		// Token: 0x06000F12 RID: 3858
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsVisible();

		// Token: 0x06000F13 RID: 3859
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool _IsVisibleFromModule(IntPtr module);

		// Token: 0x06000F14 RID: 3860 RVA: 0x0002C79D File Offset: 0x0002B79D
		internal bool IsVisibleFromModule(ModuleHandle module)
		{
			return this._IsVisibleFromModule((IntPtr)module.Value);
		}

		// Token: 0x06000F15 RID: 3861
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasProxyAttribute();

		// Token: 0x06000F16 RID: 3862
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string ConstructName(bool nameSpace, bool fullInst, bool assembly);

		// Token: 0x06000F17 RID: 3863
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetUtf8Name();

		// Token: 0x06000F18 RID: 3864 RVA: 0x0002C7B1 File Offset: 0x0002B7B1
		internal Utf8String GetUtf8Name()
		{
			return new Utf8String(this._GetUtf8Name());
		}

		// Token: 0x06000F19 RID: 3865
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool CanCastTo(IntPtr target);

		// Token: 0x06000F1A RID: 3866 RVA: 0x0002C7BE File Offset: 0x0002B7BE
		internal bool CanCastTo(RuntimeTypeHandle target)
		{
			return this.CanCastTo(target.Value);
		}

		// Token: 0x06000F1B RID: 3867
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle GetDeclaringType();

		// Token: 0x06000F1C RID: 3868
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetDeclaringMethod();

		// Token: 0x06000F1D RID: 3869 RVA: 0x0002C7CD File Offset: 0x0002B7CD
		internal RuntimeMethodHandle GetDeclaringMethod()
		{
			return new RuntimeMethodHandle(this._GetDeclaringMethod());
		}

		// Token: 0x06000F1E RID: 3870
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetDefaultConstructor();

		// Token: 0x06000F1F RID: 3871 RVA: 0x0002C7DA File Offset: 0x0002B7DA
		internal RuntimeMethodHandle GetDefaultConstructor()
		{
			return new RuntimeMethodHandle(this._GetDefaultConstructor());
		}

		// Token: 0x06000F20 RID: 3872
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool SupportsInterface(object target);

		// Token: 0x06000F21 RID: 3873
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void* _GetTypeByName(string name, bool throwOnError, bool ignoreCase, bool reflectionOnly, ref StackCrawlMark stackMark, bool loadTypeFromPartialName);

		// Token: 0x06000F22 RID: 3874 RVA: 0x0002C7E8 File Offset: 0x0002B7E8
		internal static RuntimeTypeHandle GetTypeByName(string name, bool throwOnError, bool ignoreCase, bool reflectionOnly, ref StackCrawlMark stackMark)
		{
			if (name != null && name.Length != 0)
			{
				return new RuntimeTypeHandle(RuntimeTypeHandle._GetTypeByName(name, throwOnError, ignoreCase, reflectionOnly, ref stackMark, false));
			}
			if (throwOnError)
			{
				throw new TypeLoadException(Environment.GetResourceString("Arg_TypeLoadNullStr"));
			}
			return default(RuntimeTypeHandle);
		}

		// Token: 0x06000F23 RID: 3875
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void* _GetTypeByNameUsingCARules(string name, IntPtr scope);

		// Token: 0x06000F24 RID: 3876 RVA: 0x0002C830 File Offset: 0x0002B830
		internal static Type GetTypeByNameUsingCARules(string name, Module scope)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException();
			}
			return new RuntimeTypeHandle(RuntimeTypeHandle._GetTypeByNameUsingCARules(name, (IntPtr)scope.GetModuleHandle().Value)).GetRuntimeType();
		}

		// Token: 0x06000F25 RID: 3877
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle[] GetInstantiation();

		// Token: 0x06000F26 RID: 3878
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _Instantiate(RuntimeTypeHandle[] inst);

		// Token: 0x06000F27 RID: 3879 RVA: 0x0002C874 File Offset: 0x0002B874
		internal RuntimeTypeHandle Instantiate(RuntimeTypeHandle[] inst)
		{
			return new RuntimeTypeHandle(this._Instantiate(inst));
		}

		// Token: 0x06000F28 RID: 3880
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _MakeArray(int rank);

		// Token: 0x06000F29 RID: 3881 RVA: 0x0002C882 File Offset: 0x0002B882
		internal RuntimeTypeHandle MakeArray(int rank)
		{
			return new RuntimeTypeHandle(this._MakeArray(rank));
		}

		// Token: 0x06000F2A RID: 3882
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _MakeSZArray();

		// Token: 0x06000F2B RID: 3883 RVA: 0x0002C890 File Offset: 0x0002B890
		internal RuntimeTypeHandle MakeSZArray()
		{
			return new RuntimeTypeHandle(this._MakeSZArray());
		}

		// Token: 0x06000F2C RID: 3884
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _MakeByRef();

		// Token: 0x06000F2D RID: 3885 RVA: 0x0002C89D File Offset: 0x0002B89D
		internal RuntimeTypeHandle MakeByRef()
		{
			return new RuntimeTypeHandle(this._MakeByRef());
		}

		// Token: 0x06000F2E RID: 3886
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _MakePointer();

		// Token: 0x06000F2F RID: 3887 RVA: 0x0002C8AA File Offset: 0x0002B8AA
		internal RuntimeTypeHandle MakePointer()
		{
			return new RuntimeTypeHandle(this._MakePointer());
		}

		// Token: 0x06000F30 RID: 3888
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasInstantiation();

		// Token: 0x06000F31 RID: 3889
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetGenericTypeDefinition();

		// Token: 0x06000F32 RID: 3890 RVA: 0x0002C8B7 File Offset: 0x0002B8B7
		internal RuntimeTypeHandle GetGenericTypeDefinition()
		{
			return new RuntimeTypeHandle(this._GetGenericTypeDefinition());
		}

		// Token: 0x06000F33 RID: 3891
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsGenericTypeDefinition();

		// Token: 0x06000F34 RID: 3892
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsGenericVariable();

		// Token: 0x06000F35 RID: 3893
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetGenericVariableIndex();

		// Token: 0x06000F36 RID: 3894
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool ContainsGenericVariables();

		// Token: 0x06000F37 RID: 3895
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool SatisfiesConstraints(RuntimeTypeHandle[] typeContext, RuntimeTypeHandle[] methodContext, RuntimeTypeHandle toType);

		// Token: 0x06000F38 RID: 3896 RVA: 0x0002C8C4 File Offset: 0x0002B8C4
		private RuntimeTypeHandle(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			Type type = (RuntimeType)info.GetValue("TypeObj", typeof(RuntimeType));
			this.m_ptr = type.TypeHandle.Value;
			if (this.m_ptr.ToPointer() == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0002C930 File Offset: 0x0002B930
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.m_ptr.ToPointer() == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidFieldState"));
			}
			RuntimeType value = (RuntimeType)Type.GetTypeFromHandle(this);
			info.AddValue("TypeObj", value, typeof(RuntimeType));
		}

		// Token: 0x04000535 RID: 1333
		private const int MAX_CLASS_NAME = 1024;

		// Token: 0x04000536 RID: 1334
		internal static readonly RuntimeTypeHandle EmptyHandle = new RuntimeTypeHandle(null);

		// Token: 0x04000537 RID: 1335
		private IntPtr m_ptr;

		// Token: 0x0200010A RID: 266
		internal struct IntroducedMethodEnumerator
		{
			// Token: 0x06000F3B RID: 3899 RVA: 0x0002C99F File Offset: 0x0002B99F
			internal IntroducedMethodEnumerator(RuntimeTypeHandle type)
			{
				this._method = RuntimeTypeHandle.GetFirstIntroducedMethod(type);
				this._firstCall = true;
			}

			// Token: 0x06000F3C RID: 3900 RVA: 0x0002C9B4 File Offset: 0x0002B9B4
			public bool MoveNext()
			{
				if (this._firstCall)
				{
					this._firstCall = false;
				}
				else if (!this._method.IsNullHandle())
				{
					this._method = RuntimeTypeHandle.GetNextIntroducedMethod(this._method);
				}
				return !this._method.IsNullHandle();
			}

			// Token: 0x170001DA RID: 474
			// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0002C9F3 File Offset: 0x0002B9F3
			public RuntimeMethodHandle Current
			{
				get
				{
					return this._method;
				}
			}

			// Token: 0x06000F3E RID: 3902 RVA: 0x0002C9FB File Offset: 0x0002B9FB
			public RuntimeTypeHandle.IntroducedMethodEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x04000538 RID: 1336
			private RuntimeMethodHandle _method;

			// Token: 0x04000539 RID: 1337
			private bool _firstCall;
		}
	}
}
