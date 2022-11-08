using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200034F RID: 847
	[Serializable]
	internal abstract class RuntimeFieldInfo : FieldInfo
	{
		// Token: 0x06002108 RID: 8456 RVA: 0x00051FD5 File Offset: 0x00050FD5
		protected RuntimeFieldInfo()
		{
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00051FDD File Offset: 0x00050FDD
		protected RuntimeFieldInfo(RuntimeType.RuntimeTypeCache reflectedTypeCache, RuntimeType declaringType, BindingFlags bindingFlags)
		{
			this.m_bindingFlags = bindingFlags;
			this.m_declaringType = declaringType;
			this.m_reflectedTypeCache = reflectedTypeCache;
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x0600210A RID: 8458 RVA: 0x00051FFA File Offset: 0x00050FFA
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600210B RID: 8459 RVA: 0x00052002 File Offset: 0x00051002
		private RuntimeTypeHandle ReflectedTypeHandle
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeTypeHandle;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600210C RID: 8460 RVA: 0x00052010 File Offset: 0x00051010
		internal RuntimeTypeHandle DeclaringTypeHandle
		{
			get
			{
				Type declaringType = this.DeclaringType;
				if (declaringType == null)
				{
					return this.Module.GetModuleHandle().GetModuleTypeHandle();
				}
				return declaringType.GetTypeHandleInternal();
			}
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00052041 File Offset: 0x00051041
		internal virtual RuntimeFieldHandle GetFieldHandle()
		{
			return this.FieldHandle;
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x00052049 File Offset: 0x00051049
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x0600210F RID: 8463 RVA: 0x0005204C File Offset: 0x0005104C
		public override Type ReflectedType
		{
			get
			{
				if (!this.m_reflectedTypeCache.IsGlobal)
				{
					return this.m_reflectedTypeCache.RuntimeType;
				}
				return null;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x00052068 File Offset: 0x00051068
		public override Type DeclaringType
		{
			get
			{
				if (!this.m_reflectedTypeCache.IsGlobal)
				{
					return this.m_declaringType;
				}
				return null;
			}
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0005207F File Offset: 0x0005107F
		public override string ToString()
		{
			return this.FieldType.SigToString() + " " + this.Name;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x0005209C File Offset: 0x0005109C
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000520B4 File Offset: 0x000510B4
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000520FC File Offset: 0x000510FC
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00052142 File Offset: 0x00051142
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedType, this.ToString(), MemberTypes.Field);
		}

		// Token: 0x04000DFE RID: 3582
		private BindingFlags m_bindingFlags;

		// Token: 0x04000DFF RID: 3583
		protected RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000E00 RID: 3584
		protected RuntimeType m_declaringType;
	}
}
