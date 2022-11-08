using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020005AE RID: 1454
	public class PaddingConverter : TypeConverter
	{
		// Token: 0x06004B5F RID: 19295 RVA: 0x00110EC0 File Offset: 0x0010FEC0
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x00110ED9 File Offset: 0x0010FED9
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x00110EF4 File Offset: 0x0010FEF4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			text = text.Trim();
			if (text.Length == 0)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			char c = culture.TextInfo.ListSeparator[0];
			string[] array = text.Split(new char[]
			{
				c
			});
			int[] array2 = new int[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length == 4)
			{
				return new Padding(array2[0], array2[1], array2[2], array2[3]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
			{
				"value",
				text,
				"left, top, right, bottom"
			}));
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x00110FEC File Offset: 0x0010FFEC
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Padding)
			{
				if (destinationType == typeof(string))
				{
					Padding padding = (Padding)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string separator = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[4];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, padding.Left);
					array[num++] = converter.ConvertToString(context, culture, padding.Top);
					array[num++] = converter.ConvertToString(context, culture, padding.Right);
					array[num++] = converter.ConvertToString(context, culture, padding.Bottom);
					return string.Join(separator, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					Padding padding2 = (Padding)value;
					if (padding2.ShouldSerializeAll())
					{
						return new InstanceDescriptor(typeof(Padding).GetConstructor(new Type[]
						{
							typeof(int)
						}), new object[]
						{
							padding2.All
						});
					}
					return new InstanceDescriptor(typeof(Padding).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int)
					}), new object[]
					{
						padding2.Left,
						padding2.Top,
						padding2.Right,
						padding2.Bottom
					});
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x001111F8 File Offset: 0x001101F8
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			Padding padding = (Padding)context.PropertyDescriptor.GetValue(context.Instance);
			int num = (int)propertyValues["All"];
			if (padding.All != num)
			{
				return new Padding(num);
			}
			return new Padding((int)propertyValues["Left"], (int)propertyValues["Top"], (int)propertyValues["Right"], (int)propertyValues["Bottom"]);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x001112A9 File Offset: 0x001102A9
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x001112AC File Offset: 0x001102AC
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Padding), attributes);
			return properties.Sort(new string[]
			{
				"All",
				"Left",
				"Top",
				"Right",
				"Bottom"
			});
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x00111300 File Offset: 0x00110300
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
