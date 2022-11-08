using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000350 RID: 848
	[Serializable]
	internal sealed class RtFieldInfo : RuntimeFieldInfo, ISerializable
	{
		// Token: 0x06002116 RID: 8470
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PerformVisibilityCheckOnField(IntPtr field, object target, IntPtr declaringType, FieldAttributes attr, uint invocationFlags);

		// Token: 0x06002117 RID: 8471 RVA: 0x0005216B File Offset: 0x0005116B
		internal RtFieldInfo()
		{
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x00052173 File Offset: 0x00051173
		internal RtFieldInfo(RuntimeFieldHandle handle, RuntimeType declaringType, RuntimeType.RuntimeTypeCache reflectedTypeCache, BindingFlags bindingFlags) : base(reflectedTypeCache, declaringType, bindingFlags)
		{
			this.m_fieldHandle = handle;
			this.m_fieldAttributes = this.m_fieldHandle.GetAttributes();
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x00052198 File Offset: 0x00051198
		private void GetOneTimeFlags()
		{
			Type declaringType = this.DeclaringType;
			uint num = 0U;
			if ((declaringType != null && declaringType.ContainsGenericParameters) || (declaringType == null && this.Module.Assembly.ReflectionOnly) || declaringType is ReflectionOnlyType)
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
				if ((this.m_fieldAttributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope)
				{
					num |= 16U;
				}
				if ((this.m_fieldAttributes & FieldAttributes.HasFieldRVA) != FieldAttributes.PrivateScope)
				{
					num |= 16U;
				}
				if ((this.m_fieldAttributes & FieldAttributes.FieldAccessMask) != FieldAttributes.Public || (declaringType != null && !declaringType.IsVisible))
				{
					num |= 4U;
				}
				Type fieldType = this.FieldType;
				if (fieldType.IsPointer || fieldType.IsEnum || fieldType.IsPrimitive)
				{
					num |= 32U;
				}
			}
			num |= 1U;
			this.m_invocationFlags = num;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x00052270 File Offset: 0x00051270
		private void CheckConsistency(object target)
		{
			if ((this.m_fieldAttributes & FieldAttributes.Static) == FieldAttributes.Static || this.m_declaringType.IsInstanceOfType(target))
			{
				return;
			}
			if (target == null)
			{
				throw new TargetException(Environment.GetResourceString("RFLCT.Targ_StatFldReqTarg"));
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Arg_FieldDeclTarget"), new object[]
			{
				this.Name,
				this.m_declaringType,
				target.GetType()
			}));
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000522E8 File Offset: 0x000512E8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RtFieldInfo rtFieldInfo = o as RtFieldInfo;
			return rtFieldInfo != null && rtFieldInfo.m_fieldHandle.Equals(this.m_fieldHandle);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x00052312 File Offset: 0x00051312
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal void InternalSetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture, bool doVisibilityCheck)
		{
			this.InternalSetValue(obj, value, invokeAttr, binder, culture, doVisibilityCheck, true);
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x00052324 File Offset: 0x00051324
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal void InternalSetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture, bool doVisibilityCheck, bool doCheckConsistency)
		{
			RuntimeType runtimeType = this.DeclaringType as RuntimeType;
			if ((this.m_invocationFlags & 1U) == 0U)
			{
				this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 2U) != 0U)
			{
				if (runtimeType != null && runtimeType.ContainsGenericParameters)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_UnboundGenField"));
				}
				if ((runtimeType == null && this.Module.Assembly.ReflectionOnly) || runtimeType is ReflectionOnlyType)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyField"));
				}
				throw new FieldAccessException();
			}
			else
			{
				if (doCheckConsistency)
				{
					this.CheckConsistency(obj);
				}
				value = ((RuntimeType)this.FieldType).CheckValue(value, binder, culture, invokeAttr);
				if (doVisibilityCheck && (this.m_invocationFlags & 20U) != 0U)
				{
					RtFieldInfo.PerformVisibilityCheckOnField(this.m_fieldHandle.Value, obj, this.m_declaringType.TypeHandle.Value, this.m_fieldAttributes, this.m_invocationFlags);
				}
				bool domainInitialized = false;
				if (runtimeType == null)
				{
					this.m_fieldHandle.SetValue(obj, value, this.FieldType.TypeHandle, this.m_fieldAttributes, RuntimeTypeHandle.EmptyHandle, ref domainInitialized);
					return;
				}
				domainInitialized = runtimeType.DomainInitialized;
				this.m_fieldHandle.SetValue(obj, value, this.FieldType.TypeHandle, this.m_fieldAttributes, this.DeclaringType.TypeHandle, ref domainInitialized);
				runtimeType.DomainInitialized = domainInitialized;
				return;
			}
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0005246B File Offset: 0x0005146B
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal object InternalGetValue(object obj, bool doVisibilityCheck)
		{
			return this.InternalGetValue(obj, doVisibilityCheck, true);
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x00052478 File Offset: 0x00051478
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal object InternalGetValue(object obj, bool doVisibilityCheck, bool doCheckConsistency)
		{
			RuntimeType runtimeType = this.DeclaringType as RuntimeType;
			if ((this.m_invocationFlags & 1U) == 0U)
			{
				this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 2U) != 0U)
			{
				if (runtimeType != null && this.DeclaringType.ContainsGenericParameters)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_UnboundGenField"));
				}
				if ((runtimeType == null && this.Module.Assembly.ReflectionOnly) || runtimeType is ReflectionOnlyType)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyField"));
				}
				throw new FieldAccessException();
			}
			else
			{
				if (doCheckConsistency)
				{
					this.CheckConsistency(obj);
				}
				RuntimeTypeHandle typeHandle = this.FieldType.TypeHandle;
				if (doVisibilityCheck && (this.m_invocationFlags & 4U) != 0U)
				{
					RtFieldInfo.PerformVisibilityCheckOnField(this.m_fieldHandle.Value, obj, this.m_declaringType.TypeHandle.Value, this.m_fieldAttributes, this.m_invocationFlags & 4294967279U);
				}
				bool domainInitialized = false;
				if (runtimeType == null)
				{
					return this.m_fieldHandle.GetValue(obj, typeHandle, RuntimeTypeHandle.EmptyHandle, ref domainInitialized);
				}
				domainInitialized = runtimeType.DomainInitialized;
				object value = this.m_fieldHandle.GetValue(obj, typeHandle, this.DeclaringType.TypeHandle, ref domainInitialized);
				runtimeType.DomainInitialized = domainInitialized;
				return value;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x00052599 File Offset: 0x00051599
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.m_fieldHandle.GetName();
				}
				return this.m_name;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002121 RID: 8481 RVA: 0x000525BA File Offset: 0x000515BA
		public override int MetadataToken
		{
			get
			{
				return this.m_fieldHandle.GetToken();
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002122 RID: 8482 RVA: 0x000525C8 File Offset: 0x000515C8
		public override Module Module
		{
			get
			{
				return this.m_fieldHandle.GetApproxDeclaringType().GetModuleHandle().GetModule();
			}
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000525F0 File Offset: 0x000515F0
		public override object GetValue(object obj)
		{
			return this.InternalGetValue(obj, true);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000525FA File Offset: 0x000515FA
		public override object GetRawConstantValue()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00052604 File Offset: 0x00051604
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object GetValueDirect(TypedReference obj)
		{
			if (obj.IsNull)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_TypedReference_Null"));
			}
			return this.m_fieldHandle.GetValueDirect(this.FieldType.TypeHandle, obj, (this.DeclaringType == null) ? RuntimeTypeHandle.EmptyHandle : this.DeclaringType.TypeHandle);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0005265B File Offset: 0x0005165B
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			this.InternalSetValue(obj, value, invokeAttr, binder, culture, true);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0005266C File Offset: 0x0005166C
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override void SetValueDirect(TypedReference obj, object value)
		{
			if (obj.IsNull)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_TypedReference_Null"));
			}
			this.m_fieldHandle.SetValueDirect(this.FieldType.TypeHandle, obj, value, (this.DeclaringType == null) ? RuntimeTypeHandle.EmptyHandle : this.DeclaringType.TypeHandle);
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x000526C4 File Offset: 0x000516C4
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				Type declaringType = this.DeclaringType;
				if ((declaringType == null && this.Module.Assembly.ReflectionOnly) || declaringType is ReflectionOnlyType)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInReflectionOnly"));
				}
				return this.m_fieldHandle;
			}
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x0005270B File Offset: 0x0005170B
		internal override RuntimeFieldHandle GetFieldHandle()
		{
			return this.m_fieldHandle;
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x00052713 File Offset: 0x00051713
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_fieldAttributes;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600212B RID: 8491 RVA: 0x0005271C File Offset: 0x0005171C
		public override Type FieldType
		{
			get
			{
				if (this.m_fieldType == null)
				{
					this.m_fieldType = new Signature(this.m_fieldHandle, base.DeclaringTypeHandle).FieldTypeHandle.GetRuntimeType();
				}
				return this.m_fieldType;
			}
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x0005275B File Offset: 0x0005175B
		public override Type[] GetRequiredCustomModifiers()
		{
			return new Signature(this.m_fieldHandle, base.DeclaringTypeHandle).GetCustomModifiers(1, true);
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00052775 File Offset: 0x00051775
		public override Type[] GetOptionalCustomModifiers()
		{
			return new Signature(this.m_fieldHandle, base.DeclaringTypeHandle).GetCustomModifiers(1, false);
		}

		// Token: 0x04000E01 RID: 3585
		private RuntimeFieldHandle m_fieldHandle;

		// Token: 0x04000E02 RID: 3586
		private FieldAttributes m_fieldAttributes;

		// Token: 0x04000E03 RID: 3587
		private string m_name;

		// Token: 0x04000E04 RID: 3588
		private RuntimeType m_fieldType;

		// Token: 0x04000E05 RID: 3589
		private uint m_invocationFlags;
	}
}
