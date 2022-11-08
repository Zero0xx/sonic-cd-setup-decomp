using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000351 RID: 849
	[Serializable]
	internal sealed class MdFieldInfo : RuntimeFieldInfo, ISerializable
	{
		// Token: 0x0600212E RID: 8494 RVA: 0x0005278F File Offset: 0x0005178F
		internal MdFieldInfo(int tkField, FieldAttributes fieldAttributes, RuntimeTypeHandle declaringTypeHandle, RuntimeType.RuntimeTypeCache reflectedTypeCache, BindingFlags bindingFlags) : base(reflectedTypeCache, declaringTypeHandle.GetRuntimeType(), bindingFlags)
		{
			this.m_tkField = tkField;
			this.m_name = null;
			this.m_fieldAttributes = fieldAttributes;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000527B8 File Offset: 0x000517B8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			MdFieldInfo mdFieldInfo = o as MdFieldInfo;
			return mdFieldInfo != null && mdFieldInfo.m_tkField == this.m_tkField && this.m_declaringType.GetTypeHandleInternal().GetModuleHandle().Equals(mdFieldInfo.m_declaringType.GetTypeHandleInternal().GetModuleHandle());
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x00052810 File Offset: 0x00051810
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.Module.MetadataImport.GetName(this.m_tkField).ToString();
				}
				return this.m_name;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002131 RID: 8497 RVA: 0x00052858 File Offset: 0x00051858
		public override int MetadataToken
		{
			get
			{
				return this.m_tkField;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x00052860 File Offset: 0x00051860
		public override Module Module
		{
			get
			{
				return this.m_declaringType.Module;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002133 RID: 8499 RVA: 0x0005286D File Offset: 0x0005186D
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002134 RID: 8500 RVA: 0x00052874 File Offset: 0x00051874
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_fieldAttributes;
			}
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0005287C File Offset: 0x0005187C
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object GetValueDirect(TypedReference obj)
		{
			return this.GetValue(null);
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00052885 File Offset: 0x00051885
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override void SetValueDirect(TypedReference obj, object value)
		{
			throw new FieldAccessException(Environment.GetResourceString("Acc_ReadOnly"));
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00052896 File Offset: 0x00051896
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object GetValue(object obj)
		{
			return this.GetValue(false);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x0005289F File Offset: 0x0005189F
		public override object GetRawConstantValue()
		{
			return this.GetValue(true);
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000528A8 File Offset: 0x000518A8
		internal object GetValue(bool raw)
		{
			object value = MdConstant.GetValue(this.Module.MetadataImport, this.m_tkField, this.FieldType.GetTypeHandleInternal(), raw);
			if (value == DBNull.Value)
			{
				throw new NotSupportedException(Environment.GetResourceString("Arg_EnumLitValueNotFound"));
			}
			return value;
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000528F1 File Offset: 0x000518F1
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw new FieldAccessException(Environment.GetResourceString("Acc_ReadOnly"));
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x00052904 File Offset: 0x00051904
		public override Type FieldType
		{
			get
			{
				if (this.m_fieldType == null)
				{
					ConstArray sigOfFieldDef = this.Module.MetadataImport.GetSigOfFieldDef(this.m_tkField);
					this.m_fieldType = new Signature(sigOfFieldDef.Signature.ToPointer(), sigOfFieldDef.Length, this.m_declaringType.GetTypeHandleInternal()).FieldTypeHandle.GetRuntimeType();
				}
				return this.m_fieldType;
			}
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x00052972 File Offset: 0x00051972
		public override Type[] GetRequiredCustomModifiers()
		{
			return new Type[0];
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0005297A File Offset: 0x0005197A
		public override Type[] GetOptionalCustomModifiers()
		{
			return new Type[0];
		}

		// Token: 0x04000E06 RID: 3590
		private int m_tkField;

		// Token: 0x04000E07 RID: 3591
		private string m_name;

		// Token: 0x04000E08 RID: 3592
		private Type m_fieldType;

		// Token: 0x04000E09 RID: 3593
		private FieldAttributes m_fieldAttributes;
	}
}
