using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x02000331 RID: 817
	internal class DataGridViewColumnConverter : ExpandableObjectConverter
	{
		// Token: 0x0600346B RID: 13419 RVA: 0x000B9740 File Offset: 0x000B8740
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000B975C File Offset: 0x000B875C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			DataGridViewColumn dataGridViewColumn = value as DataGridViewColumn;
			if (destinationType == typeof(InstanceDescriptor) && dataGridViewColumn != null)
			{
				ConstructorInfo constructor;
				if (dataGridViewColumn.CellType != null)
				{
					constructor = dataGridViewColumn.GetType().GetConstructor(new Type[]
					{
						typeof(Type)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							dataGridViewColumn.CellType
						}, false);
					}
				}
				constructor = dataGridViewColumn.GetType().GetConstructor(new Type[0]);
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[0], false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
