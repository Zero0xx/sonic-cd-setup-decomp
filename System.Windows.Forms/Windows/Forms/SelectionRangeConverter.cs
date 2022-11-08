using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x0200060A RID: 1546
	public class SelectionRangeConverter : TypeConverter
	{
		// Token: 0x060050D8 RID: 20696 RVA: 0x00127412 File Offset: 0x00126412
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(DateTime) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x00127438 File Offset: 0x00126438
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(DateTime) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x00127460 File Offset: 0x00126460
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return new SelectionRange(DateTime.Now.Date, DateTime.Now.Date);
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
				if (array.Length == 2)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(DateTime));
					DateTime lower = (DateTime)converter.ConvertFromString(context, culture, array[0]);
					DateTime upper = (DateTime)converter.ConvertFromString(context, culture, array[1]);
					return new SelectionRange(lower, upper);
				}
				throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
				{
					text,
					"Start" + c + " End"
				}));
			}
			else
			{
				if (value is DateTime)
				{
					DateTime dateTime = (DateTime)value;
					return new SelectionRange(dateTime, dateTime);
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x00127580 File Offset: 0x00126580
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			SelectionRange selectionRange = value as SelectionRange;
			if (selectionRange != null)
			{
				if (destinationType == typeof(string))
				{
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string separator = culture.TextInfo.ListSeparator + " ";
					PropertyDescriptorCollection properties = base.GetProperties(value);
					string[] array = new string[properties.Count];
					for (int i = 0; i < properties.Count; i++)
					{
						object value2 = properties[i].GetValue(value);
						array[i] = TypeDescriptor.GetConverter(value2).ConvertToString(context, culture, value2);
					}
					return string.Join(separator, array);
				}
				if (destinationType == typeof(DateTime))
				{
					return selectionRange.Start;
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(SelectionRange).GetConstructor(new Type[]
					{
						typeof(DateTime),
						typeof(DateTime)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							selectionRange.Start,
							selectionRange.End
						});
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x001276CC File Offset: 0x001266CC
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			object result;
			try
			{
				result = new SelectionRange((DateTime)propertyValues["Start"], (DateTime)propertyValues["End"]);
			}
			catch (InvalidCastException innerException)
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"), innerException);
			}
			catch (NullReferenceException innerException2)
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"), innerException2);
			}
			return result;
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x00127744 File Offset: 0x00126744
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x00127748 File Offset: 0x00126748
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SelectionRange), attributes);
			return properties.Sort(new string[]
			{
				"Start",
				"End"
			});
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x00127784 File Offset: 0x00126784
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
