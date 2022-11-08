using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	// Token: 0x0200010B RID: 267
	[ComVisible(true)]
	[Serializable]
	public struct RuntimeMethodHandle : ISerializable
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0002CA03 File Offset: 0x0002BA03
		internal static RuntimeMethodHandle EmptyHandle
		{
			get
			{
				return new RuntimeMethodHandle(null);
			}
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0002CA0C File Offset: 0x0002BA0C
		internal unsafe RuntimeMethodHandle(void* pMethod)
		{
			this.m_ptr = new IntPtr(pMethod);
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0002CA1A File Offset: 0x0002BA1A
		internal RuntimeMethodHandle(IntPtr pMethod)
		{
			this.m_ptr = pMethod;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0002CA24 File Offset: 0x0002BA24
		private RuntimeMethodHandle(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MethodInfo methodInfo = (RuntimeMethodInfo)info.GetValue("MethodObj", typeof(RuntimeMethodInfo));
			this.m_ptr = methodInfo.MethodHandle.Value;
			if (this.m_ptr.ToPointer() == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0002CA90 File Offset: 0x0002BA90
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
			RuntimeMethodInfo value = (RuntimeMethodInfo)RuntimeType.GetMethodBase(this);
			info.AddValue("MethodObj", value, typeof(RuntimeMethodInfo));
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0002CAF1 File Offset: 0x0002BAF1
		public IntPtr Value
		{
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			get
			{
				return this.m_ptr;
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0002CAF9 File Offset: 0x0002BAF9
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.m_ptr);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0002CB08 File Offset: 0x0002BB08
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj is RuntimeMethodHandle && ((RuntimeMethodHandle)obj).m_ptr == this.m_ptr;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0002CB38 File Offset: 0x0002BB38
		public static bool operator ==(RuntimeMethodHandle left, RuntimeMethodHandle right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0002CB42 File Offset: 0x0002BB42
		public static bool operator !=(RuntimeMethodHandle left, RuntimeMethodHandle right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0002CB4F File Offset: 0x0002BB4F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(RuntimeMethodHandle handle)
		{
			return handle.m_ptr == this.m_ptr;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0002CB63 File Offset: 0x0002BB63
		internal bool IsNullHandle()
		{
			return this.m_ptr.ToPointer() == null;
		}

		// Token: 0x06000F4B RID: 3915
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetFunctionPointer();

		// Token: 0x06000F4C RID: 3916
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void _CheckLinktimeDemands(void* module, int metadataToken);

		// Token: 0x06000F4D RID: 3917 RVA: 0x0002CB74 File Offset: 0x0002BB74
		internal void CheckLinktimeDemands(Module module, int metadataToken)
		{
			this._CheckLinktimeDemands(module.ModuleHandle.Value, metadataToken);
		}

		// Token: 0x06000F4E RID: 3918
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern bool _IsVisibleFromModule(void* source);

		// Token: 0x06000F4F RID: 3919 RVA: 0x0002CB98 File Offset: 0x0002BB98
		internal bool IsVisibleFromModule(Module source)
		{
			return this._IsVisibleFromModule(source.ModuleHandle.Value);
		}

		// Token: 0x06000F50 RID: 3920
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool _IsVisibleFromType(IntPtr source);

		// Token: 0x06000F51 RID: 3921 RVA: 0x0002CBB9 File Offset: 0x0002BBB9
		internal bool IsVisibleFromType(RuntimeTypeHandle source)
		{
			return this._IsVisibleFromType(source.Value);
		}

		// Token: 0x06000F52 RID: 3922
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* _GetCurrentMethod(ref StackCrawlMark stackMark);

		// Token: 0x06000F53 RID: 3923 RVA: 0x0002CBC8 File Offset: 0x0002BBC8
		internal static RuntimeMethodHandle GetCurrentMethod(ref StackCrawlMark stackMark)
		{
			return new RuntimeMethodHandle(RuntimeMethodHandle._GetCurrentMethod(ref stackMark));
		}

		// Token: 0x06000F54 RID: 3924
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern MethodAttributes GetAttributes();

		// Token: 0x06000F55 RID: 3925
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern MethodImplAttributes GetImplAttributes();

		// Token: 0x06000F56 RID: 3926
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string ConstructInstantiation();

		// Token: 0x06000F57 RID: 3927
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle GetDeclaringType();

		// Token: 0x06000F58 RID: 3928
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetSlot();

		// Token: 0x06000F59 RID: 3929
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetMethodDef();

		// Token: 0x06000F5A RID: 3930
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetName();

		// Token: 0x06000F5B RID: 3931
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetUtf8Name();

		// Token: 0x06000F5C RID: 3932 RVA: 0x0002CBD5 File Offset: 0x0002BBD5
		internal Utf8String GetUtf8Name()
		{
			return new Utf8String(this._GetUtf8Name());
		}

		// Token: 0x06000F5D RID: 3933
		[DebuggerStepThrough]
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object _InvokeMethodFast(object target, object[] arguments, ref SignatureStruct sig, MethodAttributes methodAttributes, RuntimeTypeHandle typeOwner);

		// Token: 0x06000F5E RID: 3934 RVA: 0x0002CBE4 File Offset: 0x0002BBE4
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal object InvokeMethodFast(object target, object[] arguments, Signature sig, MethodAttributes methodAttributes, RuntimeTypeHandle typeOwner)
		{
			SignatureStruct signature = sig.m_signature;
			object result = this._InvokeMethodFast(target, arguments, ref signature, methodAttributes, typeOwner);
			sig.m_signature = signature;
			return result;
		}

		// Token: 0x06000F5F RID: 3935
		[DebuggerHidden]
		[DebuggerStepThrough]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object _InvokeConstructor(object[] args, ref SignatureStruct signature, IntPtr declaringType);

		// Token: 0x06000F60 RID: 3936 RVA: 0x0002CC0F File Offset: 0x0002BC0F
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal object InvokeConstructor(object[] args, SignatureStruct signature, RuntimeTypeHandle declaringType)
		{
			return this._InvokeConstructor(args, ref signature, declaringType.Value);
		}

		// Token: 0x06000F61 RID: 3937
		[DebuggerHidden]
		[DebuggerStepThrough]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _SerializationInvoke(object target, ref SignatureStruct declaringTypeSig, SerializationInfo info, StreamingContext context);

		// Token: 0x06000F62 RID: 3938 RVA: 0x0002CC21 File Offset: 0x0002BC21
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal void SerializationInvoke(object target, SignatureStruct declaringTypeSig, SerializationInfo info, StreamingContext context)
		{
			this._SerializationInvoke(target, ref declaringTypeSig, info, context);
		}

		// Token: 0x06000F63 RID: 3939
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsILStub();

		// Token: 0x06000F64 RID: 3940
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle[] GetMethodInstantiation();

		// Token: 0x06000F65 RID: 3941
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasMethodInstantiation();

		// Token: 0x06000F66 RID: 3942
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeMethodHandle GetInstantiatingStub(RuntimeTypeHandle declaringTypeHandle, RuntimeTypeHandle[] methodInstantiation);

		// Token: 0x06000F67 RID: 3943
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeMethodHandle GetUnboxingStub();

		// Token: 0x06000F68 RID: 3944 RVA: 0x0002CC2F File Offset: 0x0002BC2F
		internal RuntimeMethodHandle GetInstantiatingStubIfNeeded(RuntimeTypeHandle declaringTypeHandle)
		{
			return this.GetInstantiatingStub(declaringTypeHandle, null);
		}

		// Token: 0x06000F69 RID: 3945
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeMethodHandle GetMethodFromCanonical(RuntimeTypeHandle declaringTypeHandle);

		// Token: 0x06000F6A RID: 3946
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsGenericMethodDefinition();

		// Token: 0x06000F6B RID: 3947
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetTypicalMethodDefinition();

		// Token: 0x06000F6C RID: 3948 RVA: 0x0002CC39 File Offset: 0x0002BC39
		internal RuntimeMethodHandle GetTypicalMethodDefinition()
		{
			return new RuntimeMethodHandle(this._GetTypicalMethodDefinition());
		}

		// Token: 0x06000F6D RID: 3949
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _StripMethodInstantiation();

		// Token: 0x06000F6E RID: 3950 RVA: 0x0002CC46 File Offset: 0x0002BC46
		internal RuntimeMethodHandle StripMethodInstantiation()
		{
			return new RuntimeMethodHandle(this._StripMethodInstantiation());
		}

		// Token: 0x06000F6F RID: 3951
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsDynamicMethod();

		// Token: 0x06000F70 RID: 3952
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Resolver GetResolver();

		// Token: 0x06000F71 RID: 3953
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Destroy();

		// Token: 0x06000F72 RID: 3954
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern MethodBody _GetMethodBody(IntPtr declaringType);

		// Token: 0x06000F73 RID: 3955 RVA: 0x0002CC53 File Offset: 0x0002BC53
		internal MethodBody GetMethodBody(RuntimeTypeHandle declaringType)
		{
			return this._GetMethodBody(declaringType.Value);
		}

		// Token: 0x06000F74 RID: 3956
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsConstructor();

		// Token: 0x0400053A RID: 1338
		private IntPtr m_ptr;
	}
}
