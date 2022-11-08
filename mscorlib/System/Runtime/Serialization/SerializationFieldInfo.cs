using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000377 RID: 887
	internal sealed class SerializationFieldInfo : FieldInfo
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x00057704 File Offset: 0x00056704
		public override Module Module
		{
			get
			{
				return this.m_field.Module;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x0600229F RID: 8863 RVA: 0x00057711 File Offset: 0x00056711
		public override int MetadataToken
		{
			get
			{
				return this.m_field.MetadataToken;
			}
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x0005771E File Offset: 0x0005671E
		internal SerializationFieldInfo(RuntimeFieldInfo field, string namePrefix)
		{
			this.m_field = field;
			this.m_serializationName = namePrefix + SerializationFieldInfo.FakeNameSeparatorString + this.m_field.Name;
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060022A1 RID: 8865 RVA: 0x00057749 File Offset: 0x00056749
		public override string Name
		{
			get
			{
				return this.m_serializationName;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x00057751 File Offset: 0x00056751
		public override Type DeclaringType
		{
			get
			{
				return this.m_field.DeclaringType;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x0005775E File Offset: 0x0005675E
		public override Type ReflectedType
		{
			get
			{
				return this.m_field.ReflectedType;
			}
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0005776B File Offset: 0x0005676B
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_field.GetCustomAttributes(inherit);
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x00057779 File Offset: 0x00056779
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_field.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x00057788 File Offset: 0x00056788
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_field.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x00057797 File Offset: 0x00056797
		public override Type FieldType
		{
			get
			{
				return this.m_field.FieldType;
			}
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000577A4 File Offset: 0x000567A4
		public override object GetValue(object obj)
		{
			return this.m_field.GetValue(obj);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000577B4 File Offset: 0x000567B4
		internal object InternalGetValue(object obj, bool requiresAccessCheck)
		{
			RtFieldInfo rtFieldInfo = this.m_field as RtFieldInfo;
			if (rtFieldInfo != null)
			{
				return rtFieldInfo.InternalGetValue(obj, requiresAccessCheck);
			}
			return this.m_field.GetValue(obj);
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000577E5 File Offset: 0x000567E5
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			this.m_field.SetValue(obj, value, invokeAttr, binder, culture);
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000577FC File Offset: 0x000567FC
		internal void InternalSetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture, bool requiresAccessCheck, bool isBinderDefault)
		{
			RtFieldInfo rtFieldInfo = this.m_field as RtFieldInfo;
			if (rtFieldInfo != null)
			{
				rtFieldInfo.InternalSetValue(obj, value, invokeAttr, binder, culture, false);
				return;
			}
			this.m_field.SetValue(obj, value, invokeAttr, binder, culture);
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x00057839 File Offset: 0x00056839
		internal RuntimeFieldInfo FieldInfo
		{
			get
			{
				return this.m_field;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x00057841 File Offset: 0x00056841
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				return this.m_field.FieldHandle;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x0005784E File Offset: 0x0005684E
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_field.Attributes;
			}
		}

		// Token: 0x04000E95 RID: 3733
		internal static readonly char FakeNameSeparatorChar = '+';

		// Token: 0x04000E96 RID: 3734
		internal static readonly string FakeNameSeparatorString = "+";

		// Token: 0x04000E97 RID: 3735
		private RuntimeFieldInfo m_field;

		// Token: 0x04000E98 RID: 3736
		private string m_serializationName;
	}
}
