using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007C2 RID: 1986
	internal class MultiSelectRootGridEntry : SingleSelectRootGridEntry
	{
		// Token: 0x06006905 RID: 26885 RVA: 0x00181E7C File Offset: 0x00180E7C
		internal MultiSelectRootGridEntry(PropertyGridView view, object obj, IServiceProvider baseProvider, IDesignerHost host, PropertyTab tab, PropertySort sortType) : base(view, obj, baseProvider, host, tab, sortType)
		{
		}

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x06006906 RID: 26886 RVA: 0x00181E90 File Offset: 0x00180E90
		internal override bool ForceReadOnly
		{
			get
			{
				if (!this.forceReadOnlyChecked)
				{
					bool flag = false;
					foreach (object component in ((Array)this.objValue))
					{
						ReadOnlyAttribute readOnlyAttribute = (ReadOnlyAttribute)TypeDescriptor.GetAttributes(component)[typeof(ReadOnlyAttribute)];
						if ((readOnlyAttribute != null && !readOnlyAttribute.IsDefaultAttribute()) || TypeDescriptor.GetAttributes(component).Contains(InheritanceAttribute.InheritedReadOnly))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						this.flags |= 1024;
					}
					this.forceReadOnlyChecked = true;
				}
				return base.ForceReadOnly;
			}
		}

		// Token: 0x06006907 RID: 26887 RVA: 0x00181F50 File Offset: 0x00180F50
		protected override bool CreateChildren()
		{
			return this.CreateChildren(false);
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x00181F5C File Offset: 0x00180F5C
		protected override bool CreateChildren(bool diffOldChildren)
		{
			bool result;
			try
			{
				object[] rgobjs = (object[])this.objValue;
				base.ChildCollection.Clear();
				MultiPropertyDescriptorGridEntry[] mergedProperties = MultiSelectRootGridEntry.PropertyMerger.GetMergedProperties(rgobjs, this, this.PropertySort, this.CurrentTab);
				if (mergedProperties != null)
				{
					base.ChildCollection.AddRange(mergedProperties);
				}
				bool flag = this.Children.Count > 0;
				if (!flag)
				{
					this.SetFlag(524288, true);
				}
				base.CategorizePropEntries();
				result = flag;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x04003DB7 RID: 15799
		private static MultiSelectRootGridEntry.PDComparer PropertyComparer = new MultiSelectRootGridEntry.PDComparer();

		// Token: 0x020007C3 RID: 1987
		internal static class PropertyMerger
		{
			// Token: 0x0600690A RID: 26890 RVA: 0x00181FF0 File Offset: 0x00180FF0
			public static MultiPropertyDescriptorGridEntry[] GetMergedProperties(object[] rgobjs, GridEntry parentEntry, PropertySort sort, PropertyTab tab)
			{
				MultiPropertyDescriptorGridEntry[] result = null;
				try
				{
					int num = rgobjs.Length;
					if ((sort & PropertySort.Alphabetical) != PropertySort.NoSort)
					{
						ArrayList commonProperties = MultiSelectRootGridEntry.PropertyMerger.GetCommonProperties(rgobjs, true, tab, parentEntry);
						MultiPropertyDescriptorGridEntry[] array = new MultiPropertyDescriptorGridEntry[commonProperties.Count];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = new MultiPropertyDescriptorGridEntry(parentEntry.OwnerGrid, parentEntry, rgobjs, (PropertyDescriptor[])commonProperties[i], false);
						}
						result = MultiSelectRootGridEntry.PropertyMerger.SortParenEntries(array);
					}
					else
					{
						object[] array2 = new object[num - 1];
						Array.Copy(rgobjs, 1, array2, 0, num - 1);
						ArrayList arrayList = MultiSelectRootGridEntry.PropertyMerger.GetCommonProperties(array2, true, tab, parentEntry);
						ArrayList commonProperties2 = MultiSelectRootGridEntry.PropertyMerger.GetCommonProperties(new object[]
						{
							rgobjs[0]
						}, false, tab, parentEntry);
						PropertyDescriptor[] array3 = new PropertyDescriptor[commonProperties2.Count];
						for (int j = 0; j < commonProperties2.Count; j++)
						{
							array3[j] = ((PropertyDescriptor[])commonProperties2[j])[0];
						}
						arrayList = MultiSelectRootGridEntry.PropertyMerger.UnsortedMerge(array3, arrayList);
						MultiPropertyDescriptorGridEntry[] array4 = new MultiPropertyDescriptorGridEntry[arrayList.Count];
						for (int k = 0; k < array4.Length; k++)
						{
							array4[k] = new MultiPropertyDescriptorGridEntry(parentEntry.OwnerGrid, parentEntry, rgobjs, (PropertyDescriptor[])arrayList[k], false);
						}
						result = MultiSelectRootGridEntry.PropertyMerger.SortParenEntries(array4);
					}
				}
				catch
				{
				}
				return result;
			}

			// Token: 0x0600690B RID: 26891 RVA: 0x0018214C File Offset: 0x0018114C
			private static ArrayList GetCommonProperties(object[] objs, bool presort, PropertyTab tab, GridEntry parentEntry)
			{
				PropertyDescriptorCollection[] array = new PropertyDescriptorCollection[objs.Length];
				Attribute[] array2 = new Attribute[parentEntry.BrowsableAttributes.Count];
				parentEntry.BrowsableAttributes.CopyTo(array2, 0);
				for (int i = 0; i < objs.Length; i++)
				{
					PropertyDescriptorCollection propertyDescriptorCollection = tab.GetProperties(parentEntry, objs[i], array2);
					if (presort)
					{
						propertyDescriptorCollection = propertyDescriptorCollection.Sort(MultiSelectRootGridEntry.PropertyComparer);
					}
					array[i] = propertyDescriptorCollection;
				}
				ArrayList arrayList = new ArrayList();
				PropertyDescriptor[] array3 = new PropertyDescriptor[objs.Length];
				int[] array4 = new int[array.Length];
				for (int j = 0; j < array[0].Count; j++)
				{
					PropertyDescriptor propertyDescriptor = array[0][j];
					bool flag = propertyDescriptor.Attributes[typeof(MergablePropertyAttribute)].IsDefaultAttribute();
					int num = 1;
					while (flag && num < array.Length)
					{
						if (array4[num] >= array[num].Count)
						{
							flag = false;
							break;
						}
						PropertyDescriptor propertyDescriptor2 = array[num][array4[num]];
						if (propertyDescriptor.Equals(propertyDescriptor2))
						{
							array4[num]++;
							if (!propertyDescriptor2.Attributes[typeof(MergablePropertyAttribute)].IsDefaultAttribute())
							{
								flag = false;
								break;
							}
							array3[num] = propertyDescriptor2;
						}
						else
						{
							int num2 = array4[num];
							propertyDescriptor2 = array[num][num2];
							flag = false;
							while (MultiSelectRootGridEntry.PropertyComparer.Compare(propertyDescriptor2, propertyDescriptor) <= 0)
							{
								if (propertyDescriptor.Equals(propertyDescriptor2))
								{
									if (!propertyDescriptor2.Attributes[typeof(MergablePropertyAttribute)].IsDefaultAttribute())
									{
										flag = false;
										num2++;
										break;
									}
									flag = true;
									array3[num] = propertyDescriptor2;
									array4[num] = num2 + 1;
									break;
								}
								else
								{
									num2++;
									if (num2 >= array[num].Count)
									{
										break;
									}
									propertyDescriptor2 = array[num][num2];
								}
							}
							if (!flag)
							{
								array4[num] = num2;
								break;
							}
						}
						num++;
					}
					if (flag)
					{
						array3[0] = propertyDescriptor;
						arrayList.Add(array3.Clone());
					}
				}
				return arrayList;
			}

			// Token: 0x0600690C RID: 26892 RVA: 0x00182360 File Offset: 0x00181360
			private static MultiPropertyDescriptorGridEntry[] SortParenEntries(MultiPropertyDescriptorGridEntry[] entries)
			{
				MultiPropertyDescriptorGridEntry[] array = null;
				int num = 0;
				for (int i = 0; i < entries.Length; i++)
				{
					if (entries[i].ParensAroundName)
					{
						if (array == null)
						{
							array = new MultiPropertyDescriptorGridEntry[entries.Length];
						}
						array[num++] = entries[i];
						entries[i] = null;
					}
				}
				if (num > 0)
				{
					for (int j = 0; j < entries.Length; j++)
					{
						if (entries[j] != null)
						{
							array[num++] = entries[j];
						}
					}
					entries = array;
				}
				return entries;
			}

			// Token: 0x0600690D RID: 26893 RVA: 0x001823C8 File Offset: 0x001813C8
			private static ArrayList UnsortedMerge(PropertyDescriptor[] baseEntries, ArrayList sortedMergedEntries)
			{
				ArrayList arrayList = new ArrayList();
				PropertyDescriptor[] array = new PropertyDescriptor[((PropertyDescriptor[])sortedMergedEntries[0]).Length + 1];
				foreach (PropertyDescriptor propertyDescriptor in baseEntries)
				{
					PropertyDescriptor[] array2 = null;
					string strA = propertyDescriptor.Name + " " + propertyDescriptor.PropertyType.FullName;
					int j = sortedMergedEntries.Count;
					int num = j / 2;
					int num2 = 0;
					while (j > 0)
					{
						PropertyDescriptor[] array3 = (PropertyDescriptor[])sortedMergedEntries[num2 + num];
						PropertyDescriptor propertyDescriptor2 = array3[0];
						string strB = propertyDescriptor2.Name + " " + propertyDescriptor2.PropertyType.FullName;
						int num3 = string.Compare(strA, strB, false, CultureInfo.InvariantCulture);
						if (num3 == 0)
						{
							array2 = array3;
							break;
						}
						if (num3 < 0)
						{
							j = num;
						}
						else
						{
							int num4 = num + 1;
							num2 += num4;
							j -= num4;
						}
						num = j / 2;
					}
					if (array2 != null)
					{
						array[0] = propertyDescriptor;
						Array.Copy(array2, 0, array, 1, array2.Length);
						arrayList.Add(array.Clone());
					}
				}
				return arrayList;
			}
		}

		// Token: 0x020007C4 RID: 1988
		private class PDComparer : IComparer
		{
			// Token: 0x0600690E RID: 26894 RVA: 0x001824DC File Offset: 0x001814DC
			public int Compare(object obj1, object obj2)
			{
				PropertyDescriptor propertyDescriptor = obj1 as PropertyDescriptor;
				PropertyDescriptor propertyDescriptor2 = obj2 as PropertyDescriptor;
				if (propertyDescriptor == null && propertyDescriptor2 == null)
				{
					return 0;
				}
				if (propertyDescriptor == null)
				{
					return -1;
				}
				if (propertyDescriptor2 == null)
				{
					return 1;
				}
				int num = string.Compare(propertyDescriptor.Name, propertyDescriptor2.Name, false, CultureInfo.InvariantCulture);
				if (num == 0)
				{
					num = string.Compare(propertyDescriptor.PropertyType.FullName, propertyDescriptor2.PropertyType.FullName, true, CultureInfo.CurrentCulture);
				}
				return num;
			}
		}
	}
}
