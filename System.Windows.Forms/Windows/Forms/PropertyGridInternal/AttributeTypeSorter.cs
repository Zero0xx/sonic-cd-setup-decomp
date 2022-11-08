using System;
using System.Collections;
using System.Globalization;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007B0 RID: 1968
	internal class AttributeTypeSorter : IComparer
	{
		// Token: 0x06006852 RID: 26706 RVA: 0x0017E054 File Offset: 0x0017D054
		private static string GetTypeIdString(Attribute a)
		{
			object typeId = a.TypeId;
			if (typeId == null)
			{
				return "";
			}
			string text;
			if (AttributeTypeSorter.typeIds == null)
			{
				AttributeTypeSorter.typeIds = new Hashtable();
				text = null;
			}
			else
			{
				text = (AttributeTypeSorter.typeIds[typeId] as string);
			}
			if (text == null)
			{
				text = typeId.ToString();
				AttributeTypeSorter.typeIds[typeId] = text;
			}
			return text;
		}

		// Token: 0x06006853 RID: 26707 RVA: 0x0017E0B0 File Offset: 0x0017D0B0
		public int Compare(object obj1, object obj2)
		{
			Attribute attribute = obj1 as Attribute;
			Attribute attribute2 = obj2 as Attribute;
			if (attribute == null && attribute2 == null)
			{
				return 0;
			}
			if (attribute == null)
			{
				return -1;
			}
			if (attribute2 == null)
			{
				return 1;
			}
			return string.Compare(AttributeTypeSorter.GetTypeIdString(attribute), AttributeTypeSorter.GetTypeIdString(attribute2), false, CultureInfo.InvariantCulture);
		}

		// Token: 0x04003D6C RID: 15724
		private static IDictionary typeIds;
	}
}
