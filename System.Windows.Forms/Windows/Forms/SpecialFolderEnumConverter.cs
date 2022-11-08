using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000614 RID: 1556
	internal class SpecialFolderEnumConverter : AlphaSortedEnumConverter
	{
		// Token: 0x06005105 RID: 20741 RVA: 0x00128E25 File Offset: 0x00127E25
		public SpecialFolderEnumConverter(Type type) : base(type)
		{
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x00128E30 File Offset: 0x00127E30
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues(context);
			ArrayList arrayList = new ArrayList();
			int count = standardValues.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				if (standardValues[i] is Environment.SpecialFolder && standardValues[i].Equals(Environment.SpecialFolder.Personal))
				{
					if (!flag)
					{
						flag = true;
						arrayList.Add(standardValues[i]);
					}
				}
				else
				{
					arrayList.Add(standardValues[i]);
				}
			}
			return new TypeConverter.StandardValuesCollection(arrayList);
		}
	}
}
