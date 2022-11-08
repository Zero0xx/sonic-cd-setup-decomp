using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000759 RID: 1881
	internal class Com2IDispatchConverter : Com2ExtendedTypeConverter
	{
		// Token: 0x060063CC RID: 25548 RVA: 0x0016C53C File Offset: 0x0016B53C
		public Com2IDispatchConverter(Com2PropertyDescriptor propDesc, bool allowExpand, TypeConverter baseConverter) : base(baseConverter)
		{
			this.propDesc = propDesc;
			this.allowExpand = allowExpand;
		}

		// Token: 0x060063CD RID: 25549 RVA: 0x0016C553 File Offset: 0x0016B553
		public Com2IDispatchConverter(Com2PropertyDescriptor propDesc, bool allowExpand) : base(propDesc.PropertyType)
		{
			this.propDesc = propDesc;
			this.allowExpand = allowExpand;
		}

		// Token: 0x060063CE RID: 25550 RVA: 0x0016C56F File Offset: 0x0016B56F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

		// Token: 0x060063CF RID: 25551 RVA: 0x0016C572 File Offset: 0x0016B572
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		// Token: 0x060063D0 RID: 25552 RVA: 0x0016C584 File Offset: 0x0016B584
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				return Com2IDispatchConverter.none;
			}
			string text = ComNativeDescriptor.Instance.GetName(value);
			if (text == null || text.Length == 0)
			{
				text = ComNativeDescriptor.Instance.GetClassName(value);
			}
			if (text == null)
			{
				return "(Object)";
			}
			return text;
		}

		// Token: 0x060063D1 RID: 25553 RVA: 0x0016C5E1 File Offset: 0x0016B5E1
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes);
		}

		// Token: 0x060063D2 RID: 25554 RVA: 0x0016C5EA File Offset: 0x0016B5EA
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return this.allowExpand;
		}

		// Token: 0x060063D3 RID: 25555 RVA: 0x0016C5F2 File Offset: 0x0016B5F2
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x04003B84 RID: 15236
		private Com2PropertyDescriptor propDesc;

		// Token: 0x04003B85 RID: 15237
		protected static readonly string none = SR.GetString("toStringNone");

		// Token: 0x04003B86 RID: 15238
		private bool allowExpand;
	}
}
