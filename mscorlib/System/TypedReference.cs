using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200012F RID: 303
	[CLSCompliant(false)]
	[ComVisible(true)]
	public struct TypedReference
	{
		// Token: 0x060010B5 RID: 4277 RVA: 0x0002EE28 File Offset: 0x0002DE28
		[CLSCompliant(false)]
		[ReflectionPermission(SecurityAction.LinkDemand, MemberAccess = true)]
		public unsafe static TypedReference MakeTypedReference(object target, FieldInfo[] flds)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (flds == null)
			{
				throw new ArgumentNullException("flds");
			}
			if (flds.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayZeroError"));
			}
			RuntimeFieldHandle[] array = new RuntimeFieldHandle[flds.Length];
			Type type = target.GetType();
			for (int i = 0; i < flds.Length; i++)
			{
				FieldInfo fieldInfo = flds[i];
				if (!(fieldInfo is RuntimeFieldInfo))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeFieldInfo"));
				}
				if (fieldInfo.IsInitOnly || fieldInfo.IsStatic)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_TypedReferenceInvalidField"));
				}
				if (type != fieldInfo.DeclaringType && !type.IsSubclassOf(fieldInfo.DeclaringType))
				{
					throw new MissingMemberException(Environment.GetResourceString("MissingMemberTypeRef"));
				}
				Type fieldType = fieldInfo.FieldType;
				if (fieldType.IsPrimitive)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_TypeRefPrimitve"));
				}
				if (i < flds.Length - 1 && !fieldType.IsValueType)
				{
					throw new MissingMemberException(Environment.GetResourceString("MissingMemberNestErr"));
				}
				array[i] = fieldInfo.FieldHandle;
				type = fieldType;
			}
			TypedReference result = default(TypedReference);
			TypedReference.InternalMakeTypedReference((void*)(&result), target, array, type.TypeHandle);
			return result;
		}

		// Token: 0x060010B6 RID: 4278
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void InternalMakeTypedReference(void* result, object target, RuntimeFieldHandle[] flds, RuntimeTypeHandle lastFieldType);

		// Token: 0x060010B7 RID: 4279 RVA: 0x0002EF5E File Offset: 0x0002DF5E
		public override int GetHashCode()
		{
			if (this.Type == IntPtr.Zero)
			{
				return 0;
			}
			return __reftype(this).GetHashCode();
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0002EF86 File Offset: 0x0002DF86
		public override bool Equals(object o)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NYI"));
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0002EF97 File Offset: 0x0002DF97
		public unsafe static object ToObject(TypedReference value)
		{
			return TypedReference.InternalToObject((void*)(&value));
		}

		// Token: 0x060010BA RID: 4282
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern object InternalToObject(void* value);

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x0002EFA1 File Offset: 0x0002DFA1
		internal bool IsNull
		{
			get
			{
				return this.Value.IsNull() && this.Type.IsNull();
			}
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0002EFBD File Offset: 0x0002DFBD
		public static Type GetTargetType(TypedReference value)
		{
			return __reftype(value);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0002EFC7 File Offset: 0x0002DFC7
		public static RuntimeTypeHandle TargetTypeToken(TypedReference value)
		{
			return __reftype(value).TypeHandle;
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0002EFD6 File Offset: 0x0002DFD6
		[CLSCompliant(false)]
		public unsafe static void SetTypedReference(TypedReference target, object value)
		{
			TypedReference.InternalSetTypedReference((void*)(&target), value);
		}

		// Token: 0x060010BF RID: 4287
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void InternalSetTypedReference(void* target, object value);

		// Token: 0x040005B7 RID: 1463
		private IntPtr Value;

		// Token: 0x040005B8 RID: 1464
		private IntPtr Type;
	}
}
