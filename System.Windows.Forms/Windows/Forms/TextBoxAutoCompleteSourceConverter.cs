using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200065E RID: 1630
	internal class TextBoxAutoCompleteSourceConverter : EnumConverter
	{
		// Token: 0x060055A3 RID: 21923 RVA: 0x00137433 File Offset: 0x00136433
		public TextBoxAutoCompleteSourceConverter(Type type) : base(type)
		{
		}

		// Token: 0x060055A4 RID: 21924 RVA: 0x0013743C File Offset: 0x0013643C
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues(context);
			ArrayList arrayList = new ArrayList();
			int count = standardValues.Count;
			for (int i = 0; i < count; i++)
			{
				string text = standardValues[i].ToString();
				if (!text.Equals("ListItems"))
				{
					arrayList.Add(standardValues[i]);
				}
			}
			return new TypeConverter.StandardValuesCollection(arrayList);
		}
	}
}
