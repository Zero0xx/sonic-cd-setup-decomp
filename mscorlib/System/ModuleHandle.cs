using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	// Token: 0x0200010E RID: 270
	[ComVisible(true)]
	public struct ModuleHandle
	{
		// Token: 0x06000F96 RID: 3990 RVA: 0x0002CE6D File Offset: 0x0002BE6D
		internal unsafe ModuleHandle(void* pModule)
		{
			this.m_ptr = new IntPtr(pModule);
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0002CE7B File Offset: 0x0002BE7B
		internal unsafe void* Value
		{
			get
			{
				return this.m_ptr.ToPointer();
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0002CE88 File Offset: 0x0002BE88
		internal bool IsNullHandle()
		{
			return this.m_ptr.ToPointer() == null;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0002CE99 File Offset: 0x0002BE99
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.m_ptr);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0002CEA8 File Offset: 0x0002BEA8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj is ModuleHandle && ((ModuleHandle)obj).m_ptr == this.m_ptr;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0002CED8 File Offset: 0x0002BED8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(ModuleHandle handle)
		{
			return handle.m_ptr == this.m_ptr;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0002CEEC File Offset: 0x0002BEEC
		public static bool operator ==(ModuleHandle left, ModuleHandle right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0002CEF6 File Offset: 0x0002BEF6
		public static bool operator !=(ModuleHandle left, ModuleHandle right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000F9E RID: 3998
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern RuntimeTypeHandle GetCallerType(ref StackCrawlMark stackMark);

		// Token: 0x06000F9F RID: 3999
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void* GetDynamicMethod(void* module, string name, byte[] sig, Resolver resolver);

		// Token: 0x06000FA0 RID: 4000
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetToken();

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0002CF04 File Offset: 0x0002BF04
		internal static RuntimeTypeHandle[] CopyRuntimeTypeHandles(RuntimeTypeHandle[] inHandles)
		{
			if (inHandles == null || inHandles.Length == 0)
			{
				return inHandles;
			}
			RuntimeTypeHandle[] array = new RuntimeTypeHandle[inHandles.Length];
			for (int i = 0; i < inHandles.Length; i++)
			{
				array[i] = inHandles[i];
			}
			return array;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0002CF4B File Offset: 0x0002BF4B
		private void ValidateModulePointer()
		{
			if (this.IsNullHandle())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NullModuleHandle"));
			}
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0002CF65 File Offset: 0x0002BF65
		public RuntimeTypeHandle GetRuntimeTypeHandleFromMetadataToken(int typeToken)
		{
			return this.ResolveTypeHandle(typeToken);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0002CF6E File Offset: 0x0002BF6E
		public RuntimeTypeHandle ResolveTypeHandle(int typeToken)
		{
			return this.ResolveTypeHandle(typeToken, null, null);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0002CF7C File Offset: 0x0002BF7C
		public unsafe RuntimeTypeHandle ResolveTypeHandle(int typeToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			this.ValidateModulePointer();
			if (!this.GetMetadataImport().IsValidToken(typeToken))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					typeToken,
					this
				}), new object[0]));
			}
			typeInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(typeInstantiationContext);
			methodInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(methodInstantiationContext);
			RuntimeTypeHandle result;
			if (typeInstantiationContext == null || typeInstantiationContext.Length == 0)
			{
				if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
				{
					return this.ResolveType(typeToken, null, 0, null, 0);
				}
				int methodInstCount = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr = methodInstantiationContext)
				{
					result = this.ResolveType(typeToken, null, 0, ptr, methodInstCount);
				}
			}
			else if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
			{
				int typeInstCount = typeInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr2 = typeInstantiationContext)
				{
					result = this.ResolveType(typeToken, ptr2, typeInstCount, null, 0);
				}
			}
			else
			{
				int typeInstCount2 = typeInstantiationContext.Length;
				int methodInstCount2 = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr3 = typeInstantiationContext)
				{
					fixed (RuntimeTypeHandle* ptr4 = methodInstantiationContext)
					{
						result = this.ResolveType(typeToken, ptr3, typeInstCount2, ptr4, methodInstCount2);
					}
				}
			}
			return result;
		}

		// Token: 0x06000FA6 RID: 4006
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern RuntimeTypeHandle ResolveType(int typeToken, RuntimeTypeHandle* typeInstArgs, int typeInstCount, RuntimeTypeHandle* methodInstArgs, int methodInstCount);

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0002D0D9 File Offset: 0x0002C0D9
		public RuntimeMethodHandle GetRuntimeMethodHandleFromMetadataToken(int methodToken)
		{
			return this.ResolveMethodHandle(methodToken);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0002D0E2 File Offset: 0x0002C0E2
		public RuntimeMethodHandle ResolveMethodHandle(int methodToken)
		{
			return this.ResolveMethodHandle(methodToken, null, null);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002D0F0 File Offset: 0x0002C0F0
		public unsafe RuntimeMethodHandle ResolveMethodHandle(int methodToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			this.ValidateModulePointer();
			if (!this.GetMetadataImport().IsValidToken(methodToken))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					methodToken,
					this
				}), new object[0]));
			}
			typeInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(typeInstantiationContext);
			methodInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(methodInstantiationContext);
			RuntimeMethodHandle result;
			if (typeInstantiationContext == null || typeInstantiationContext.Length == 0)
			{
				if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
				{
					return this.ResolveMethod(methodToken, null, 0, null, 0);
				}
				int methodInstCount = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr = methodInstantiationContext)
				{
					result = this.ResolveMethod(methodToken, null, 0, ptr, methodInstCount);
				}
			}
			else if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
			{
				int typeInstCount = typeInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr2 = typeInstantiationContext)
				{
					result = this.ResolveMethod(methodToken, ptr2, typeInstCount, null, 0);
				}
			}
			else
			{
				int typeInstCount2 = typeInstantiationContext.Length;
				int methodInstCount2 = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr3 = typeInstantiationContext)
				{
					fixed (RuntimeTypeHandle* ptr4 = methodInstantiationContext)
					{
						result = this.ResolveMethod(methodToken, ptr3, typeInstCount2, ptr4, methodInstCount2);
					}
				}
			}
			return result;
		}

		// Token: 0x06000FAA RID: 4010
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern RuntimeMethodHandle ResolveMethod(int methodToken, RuntimeTypeHandle* typeInstArgs, int typeInstCount, RuntimeTypeHandle* methodInstArgs, int methodInstCount);

		// Token: 0x06000FAB RID: 4011 RVA: 0x0002D24D File Offset: 0x0002C24D
		public RuntimeFieldHandle GetRuntimeFieldHandleFromMetadataToken(int fieldToken)
		{
			return this.ResolveFieldHandle(fieldToken);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0002D256 File Offset: 0x0002C256
		public RuntimeFieldHandle ResolveFieldHandle(int fieldToken)
		{
			return this.ResolveFieldHandle(fieldToken, null, null);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0002D264 File Offset: 0x0002C264
		public unsafe RuntimeFieldHandle ResolveFieldHandle(int fieldToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			this.ValidateModulePointer();
			if (!this.GetMetadataImport().IsValidToken(fieldToken))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					fieldToken,
					this
				}), new object[0]));
			}
			typeInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(typeInstantiationContext);
			methodInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(methodInstantiationContext);
			RuntimeFieldHandle result;
			if (typeInstantiationContext == null || typeInstantiationContext.Length == 0)
			{
				if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
				{
					return this.ResolveField(fieldToken, null, 0, null, 0);
				}
				int methodInstCount = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr = methodInstantiationContext)
				{
					result = this.ResolveField(fieldToken, null, 0, ptr, methodInstCount);
				}
			}
			else if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
			{
				int typeInstCount = typeInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr2 = typeInstantiationContext)
				{
					result = this.ResolveField(fieldToken, ptr2, typeInstCount, null, 0);
				}
			}
			else
			{
				int typeInstCount2 = typeInstantiationContext.Length;
				int methodInstCount2 = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr3 = typeInstantiationContext)
				{
					fixed (RuntimeTypeHandle* ptr4 = methodInstantiationContext)
					{
						result = this.ResolveField(fieldToken, ptr3, typeInstCount2, ptr4, methodInstCount2);
					}
				}
			}
			return result;
		}

		// Token: 0x06000FAE RID: 4014
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern RuntimeFieldHandle ResolveField(int fieldToken, RuntimeTypeHandle* typeInstArgs, int typeInstCount, RuntimeTypeHandle* methodInstArgs, int methodInstCount);

		// Token: 0x06000FAF RID: 4015
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Module GetModule();

		// Token: 0x06000FB0 RID: 4016
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void* _GetModuleTypeHandle();

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002D3C1 File Offset: 0x0002C3C1
		internal RuntimeTypeHandle GetModuleTypeHandle()
		{
			return new RuntimeTypeHandle(this._GetModuleTypeHandle());
		}

		// Token: 0x06000FB2 RID: 4018
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void _GetPEKind(out int peKind, out int machine);

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0002D3D0 File Offset: 0x0002C3D0
		internal void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
		{
			int num;
			int num2;
			this._GetPEKind(out num, out num2);
			peKind = (PortableExecutableKinds)num;
			machine = (ImageFileMachine)num2;
		}

		// Token: 0x06000FB4 RID: 4020
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int _GetMDStreamVersion();

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0002D3ED File Offset: 0x0002C3ED
		public int MDStreamVersion
		{
			get
			{
				return this._GetMDStreamVersion();
			}
		}

		// Token: 0x06000FB6 RID: 4022
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void* _GetMetadataImport();

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0002D3F5 File Offset: 0x0002C3F5
		internal MetadataImport GetMetadataImport()
		{
			return new MetadataImport((IntPtr)this._GetMetadataImport());
		}

		// Token: 0x0400053D RID: 1341
		public static readonly ModuleHandle EmptyHandle = new ModuleHandle(null);

		// Token: 0x0400053E RID: 1342
		private IntPtr m_ptr;
	}
}
