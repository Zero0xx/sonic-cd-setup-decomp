using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000709 RID: 1801
	public class TreeViewImageIndexConverter : ImageIndexConverter
	{
		// Token: 0x17001457 RID: 5207
		// (get) Token: 0x0600603A RID: 24634 RVA: 0x0015EC8C File Offset: 0x0015DC8C
		protected override bool IncludeNoneAsStandardValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x0015EC90 File Offset: 0x0015DC90
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				if (string.Compare(text, SR.GetString("toStringDefault"), true, culture) == 0)
				{
					return -1;
				}
				if (string.Compare(text, SR.GetString("toStringNone"), true, culture) == 0)
				{
					return -2;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x0015ECE8 File Offset: 0x0015DCE8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is int)
			{
				int num = (int)value;
				if (num == -1)
				{
					return SR.GetString("toStringDefault");
				}
				if (num == -2)
				{
					return SR.GetString("toStringNone");
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x0015ED4C File Offset: 0x0015DD4C
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				object obj = context.Instance;
				PropertyDescriptor propertyDescriptor = ImageListUtils.GetImageListProperty(context.PropertyDescriptor, ref obj);
				while (obj != null && propertyDescriptor == null)
				{
					PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
					foreach (object obj2 in properties)
					{
						PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj2;
						if (typeof(ImageList).IsAssignableFrom(propertyDescriptor2.PropertyType))
						{
							propertyDescriptor = propertyDescriptor2;
							break;
						}
					}
					if (propertyDescriptor == null)
					{
						PropertyDescriptor propertyDescriptor3 = properties[base.ParentImageListProperty];
						if (propertyDescriptor3 != null)
						{
							obj = propertyDescriptor3.GetValue(obj);
						}
						else
						{
							obj = null;
						}
					}
				}
				if (propertyDescriptor != null)
				{
					ImageList imageList = (ImageList)propertyDescriptor.GetValue(obj);
					if (imageList != null)
					{
						int num = imageList.Images.Count + 2;
						object[] array = new object[num];
						array[num - 2] = -1;
						array[num - 1] = -2;
						for (int i = 0; i < num - 2; i++)
						{
							array[i] = i;
						}
						return new TypeConverter.StandardValuesCollection(array);
					}
				}
			}
			return new TypeConverter.StandardValuesCollection(new object[]
			{
				-1,
				-2
			});
		}
	}
}
