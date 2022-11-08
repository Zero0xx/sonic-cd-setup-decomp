using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200010C RID: 268
	[ComVisible(true)]
	[Serializable]
	public struct RuntimeFieldHandle : ISerializable
	{
		// Token: 0x06000F75 RID: 3957 RVA: 0x0002CC62 File Offset: 0x0002BC62
		internal unsafe RuntimeFieldHandle(void* pFieldHandle)
		{
			this.m_ptr = new IntPtr(pFieldHandle);
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x0002CC70 File Offset: 0x0002BC70
		public IntPtr Value
		{
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			get
			{
				return this.m_ptr;
			}
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002CC78 File Offset: 0x0002BC78
		internal bool IsNullHandle()
		{
			return this.m_ptr.ToPointer() == null;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0002CC89 File Offset: 0x0002BC89
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.m_ptr);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0002CC98 File Offset: 0x0002BC98
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj is RuntimeFieldHandle && ((RuntimeFieldHandle)obj).m_ptr == this.m_ptr;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0002CCC8 File Offset: 0x0002BCC8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(RuntimeFieldHandle handle)
		{
			return handle.m_ptr == this.m_ptr;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0002CCDC File Offset: 0x0002BCDC
		public static bool operator ==(RuntimeFieldHandle left, RuntimeFieldHandle right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0002CCE6 File Offset: 0x0002BCE6
		public static bool operator !=(RuntimeFieldHandle left, RuntimeFieldHandle right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000F7D RID: 3965
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetName();

		// Token: 0x06000F7E RID: 3966
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetUtf8Name();

		// Token: 0x06000F7F RID: 3967 RVA: 0x0002CCF3 File Offset: 0x0002BCF3
		internal Utf8String GetUtf8Name()
		{
			return new Utf8String(this._GetUtf8Name());
		}

		// Token: 0x06000F80 RID: 3968
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern FieldAttributes GetAttributes();

		// Token: 0x06000F81 RID: 3969
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle GetApproxDeclaringType();

		// Token: 0x06000F82 RID: 3970
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetToken();

		// Token: 0x06000F83 RID: 3971
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetValue(object instance, RuntimeTypeHandle fieldType, RuntimeTypeHandle declaringType, ref bool domainInitialized);

		// Token: 0x06000F84 RID: 3972
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetValueDirect(RuntimeTypeHandle fieldType, TypedReference obj, RuntimeTypeHandle contextType);

		// Token: 0x06000F85 RID: 3973
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetValue(object obj, object value, RuntimeTypeHandle fieldType, FieldAttributes fieldAttr, RuntimeTypeHandle declaringType, ref bool domainInitialized);

		// Token: 0x06000F86 RID: 3974
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetValueDirect(RuntimeTypeHandle fieldType, TypedReference obj, object value, RuntimeTypeHandle contextType);

		// Token: 0x06000F87 RID: 3975
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeFieldHandle GetStaticFieldForGenericType(RuntimeTypeHandle declaringType);

		// Token: 0x06000F88 RID: 3976
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool AcquiresContextFromThis();

		// Token: 0x06000F89 RID: 3977 RVA: 0x0002CD00 File Offset: 0x0002BD00
		private RuntimeFieldHandle(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			FieldInfo fieldInfo = (RuntimeFieldInfo)info.GetValue("FieldObj", typeof(RuntimeFieldInfo));
			if (fieldInfo == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
			this.m_ptr = fieldInfo.FieldHandle.Value;
			if (this.m_ptr.ToPointer() == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0002CD7C File Offset: 0x0002BD7C
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
			RuntimeFieldInfo value = (RuntimeFieldInfo)RuntimeType.GetFieldInfo(this);
			info.AddValue("FieldObj", value, typeof(RuntimeFieldInfo));
		}

		// Token: 0x0400053B RID: 1339
		private IntPtr m_ptr;
	}
}
