using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200064A RID: 1610
	internal class TableLayoutPanelCellPositionTypeConverter : TypeConverter
	{
		// Token: 0x060054BF RID: 21695 RVA: 0x00134E64 File Offset: 0x00133E64
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x00134E7D File Offset: 0x00133E7D
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060054C1 RID: 21697 RVA: 0x00134E98 File Offset: 0x00133E98
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text = ((string)value).Trim();
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
			if (array2.Length == 2)
			{
				return new TableLayoutPanelCellPosition(array2[0], array2[1]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
			{
				text,
				"column, row"
			}));
		}

		// Token: 0x060054C2 RID: 21698 RVA: 0x00134F84 File Offset: 0x00133F84
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is TableLayoutPanelCellPosition)
			{
				TableLayoutPanelCellPosition tableLayoutPanelCellPosition = (TableLayoutPanelCellPosition)value;
				return new InstanceDescriptor(typeof(TableLayoutPanelCellPosition).GetConstructor(new Type[]
				{
					typeof(int),
					typeof(int)
				}), new object[]
				{
					tableLayoutPanelCellPosition.Column,
					tableLayoutPanelCellPosition.Row
				});
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x00135025 File Offset: 0x00134025
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return new TableLayoutPanelCellPosition((int)propertyValues["Column"], (int)propertyValues["Row"]);
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x00135051 File Offset: 0x00134051
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x00135054 File Offset: 0x00134054
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(TableLayoutPanelCellPosition), attributes);
			return properties.Sort(new string[]
			{
				"Column",
				"Row"
			});
		}

		// Token: 0x060054C6 RID: 21702 RVA: 0x00135090 File Offset: 0x00134090
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
