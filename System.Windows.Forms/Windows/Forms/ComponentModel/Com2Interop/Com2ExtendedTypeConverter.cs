using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200074E RID: 1870
	internal class Com2ExtendedTypeConverter : TypeConverter
	{
		// Token: 0x06006389 RID: 25481 RVA: 0x0016B9ED File Offset: 0x0016A9ED
		public Com2ExtendedTypeConverter(TypeConverter innerConverter)
		{
			this.innerConverter = innerConverter;
		}

		// Token: 0x0600638A RID: 25482 RVA: 0x0016B9FC File Offset: 0x0016A9FC
		public Com2ExtendedTypeConverter(Type baseType)
		{
			this.innerConverter = TypeDescriptor.GetConverter(baseType);
		}

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x0600638B RID: 25483 RVA: 0x0016BA10 File Offset: 0x0016AA10
		public TypeConverter InnerConverter
		{
			get
			{
				return this.innerConverter;
			}
		}

		// Token: 0x0600638C RID: 25484 RVA: 0x0016BA18 File Offset: 0x0016AA18
		public TypeConverter GetWrappedConverter(Type t)
		{
			for (TypeConverter typeConverter = this.innerConverter; typeConverter != null; typeConverter = ((Com2ExtendedTypeConverter)typeConverter).InnerConverter)
			{
				if (t.IsInstanceOfType(typeConverter))
				{
					return typeConverter;
				}
				if (!(typeConverter is Com2ExtendedTypeConverter))
				{
					break;
				}
			}
			return null;
		}

		// Token: 0x0600638D RID: 25485 RVA: 0x0016BA51 File Offset: 0x0016AA51
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.CanConvertFrom(context, sourceType);
			}
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600638E RID: 25486 RVA: 0x0016BA71 File Offset: 0x0016AA71
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.CanConvertTo(context, destinationType);
			}
			return base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600638F RID: 25487 RVA: 0x0016BA91 File Offset: 0x0016AA91
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.ConvertFrom(context, culture, value);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06006390 RID: 25488 RVA: 0x0016BAB3 File Offset: 0x0016AAB3
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.ConvertTo(context, culture, value, destinationType);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06006391 RID: 25489 RVA: 0x0016BAD9 File Offset: 0x0016AAD9
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.CreateInstance(context, propertyValues);
			}
			return base.CreateInstance(context, propertyValues);
		}

		// Token: 0x06006392 RID: 25490 RVA: 0x0016BAF9 File Offset: 0x0016AAF9
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetCreateInstanceSupported(context);
			}
			return base.GetCreateInstanceSupported(context);
		}

		// Token: 0x06006393 RID: 25491 RVA: 0x0016BB17 File Offset: 0x0016AB17
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetProperties(context, value, attributes);
			}
			return base.GetProperties(context, value, attributes);
		}

		// Token: 0x06006394 RID: 25492 RVA: 0x0016BB39 File Offset: 0x0016AB39
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetPropertiesSupported(context);
			}
			return base.GetPropertiesSupported(context);
		}

		// Token: 0x06006395 RID: 25493 RVA: 0x0016BB57 File Offset: 0x0016AB57
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetStandardValues(context);
			}
			return base.GetStandardValues(context);
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x0016BB75 File Offset: 0x0016AB75
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetStandardValuesExclusive(context);
			}
			return base.GetStandardValuesExclusive(context);
		}

		// Token: 0x06006397 RID: 25495 RVA: 0x0016BB93 File Offset: 0x0016AB93
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetStandardValuesSupported(context);
			}
			return base.GetStandardValuesSupported(context);
		}

		// Token: 0x06006398 RID: 25496 RVA: 0x0016BBB1 File Offset: 0x0016ABB1
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.IsValid(context, value);
			}
			return base.IsValid(context, value);
		}

		// Token: 0x04003B79 RID: 15225
		private TypeConverter innerConverter;
	}
}
