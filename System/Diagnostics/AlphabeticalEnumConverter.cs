using System;
using System.Collections;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x0200073D RID: 1853
	internal class AlphabeticalEnumConverter : EnumConverter
	{
		// Token: 0x0600387E RID: 14462 RVA: 0x000EEA6A File Offset: 0x000EDA6A
		public AlphabeticalEnumConverter(Type type) : base(type)
		{
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x000EEA74 File Offset: 0x000EDA74
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (base.Values == null)
			{
				Array values = Enum.GetValues(base.EnumType);
				object[] array = new object[values.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.ConvertTo(context, null, values.GetValue(i), typeof(string));
				}
				Array.Sort(array, values, 0, values.Length, System.Collections.Comparer.Default);
				base.Values = new TypeConverter.StandardValuesCollection(values);
			}
			return base.Values;
		}
	}
}
