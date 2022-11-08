using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007C1 RID: 1985
	internal class SingleSelectRootGridEntry : GridEntry, IRootGridEntry
	{
		// Token: 0x060068EB RID: 26859 RVA: 0x00181880 File Offset: 0x00180880
		internal SingleSelectRootGridEntry(PropertyGridView gridEntryHost, object value, GridEntry parent, IServiceProvider baseProvider, IDesignerHost host, PropertyTab tab, PropertySort sortType) : base(gridEntryHost.OwnerGrid, parent)
		{
			this.host = host;
			this.gridEntryHost = gridEntryHost;
			this.baseProvider = baseProvider;
			this.tab = tab;
			this.objValue = value;
			this.objValueClassName = TypeDescriptor.GetClassName(this.objValue);
			this.IsExpandable = true;
			this.PropertySort = sortType;
			this.InternalExpanded = true;
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x001818E7 File Offset: 0x001808E7
		internal SingleSelectRootGridEntry(PropertyGridView view, object value, IServiceProvider baseProvider, IDesignerHost host, PropertyTab tab, PropertySort sortType) : this(view, value, null, baseProvider, host, tab, sortType)
		{
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x060068ED RID: 26861 RVA: 0x001818FC File Offset: 0x001808FC
		// (set) Token: 0x060068EE RID: 26862 RVA: 0x00181934 File Offset: 0x00180934
		public override AttributeCollection BrowsableAttributes
		{
			get
			{
				if (this.browsableAttributes == null)
				{
					this.browsableAttributes = new AttributeCollection(new Attribute[]
					{
						BrowsableAttribute.Yes
					});
				}
				return this.browsableAttributes;
			}
			set
			{
				if (value == null)
				{
					this.ResetBrowsableAttributes();
					return;
				}
				bool flag = true;
				if (this.browsableAttributes != null && value != null && this.browsableAttributes.Count == value.Count)
				{
					Attribute[] array = new Attribute[this.browsableAttributes.Count];
					Attribute[] array2 = new Attribute[value.Count];
					this.browsableAttributes.CopyTo(array, 0);
					value.CopyTo(array2, 0);
					Array.Sort(array, GridEntry.AttributeTypeSorter);
					Array.Sort(array2, GridEntry.AttributeTypeSorter);
					for (int i = 0; i < array.Length; i++)
					{
						if (!array[i].Equals(array2[i]))
						{
							flag = false;
							break;
						}
					}
				}
				else
				{
					flag = false;
				}
				this.browsableAttributes = value;
				if (!flag && this.Children != null && this.Children.Count > 0)
				{
					this.DisposeChildren();
				}
			}
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x060068EF RID: 26863 RVA: 0x001819FC File Offset: 0x001809FC
		protected override IComponentChangeService ComponentChangeService
		{
			get
			{
				if (this.changeService == null)
				{
					this.changeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				}
				return this.changeService;
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x060068F0 RID: 26864 RVA: 0x00181A27 File Offset: 0x00180A27
		internal override bool AlwaysAllowExpand
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x060068F1 RID: 26865 RVA: 0x00181A2A File Offset: 0x00180A2A
		// (set) Token: 0x060068F2 RID: 26866 RVA: 0x00181A32 File Offset: 0x00180A32
		public override PropertyTab CurrentTab
		{
			get
			{
				return this.tab;
			}
			set
			{
				this.tab = value;
			}
		}

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x060068F3 RID: 26867 RVA: 0x00181A3B File Offset: 0x00180A3B
		// (set) Token: 0x060068F4 RID: 26868 RVA: 0x00181A43 File Offset: 0x00180A43
		internal override GridEntry DefaultChild
		{
			get
			{
				return this.propDefault;
			}
			set
			{
				this.propDefault = value;
			}
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x060068F5 RID: 26869 RVA: 0x00181A4C File Offset: 0x00180A4C
		// (set) Token: 0x060068F6 RID: 26870 RVA: 0x00181A54 File Offset: 0x00180A54
		internal override IDesignerHost DesignerHost
		{
			get
			{
				return this.host;
			}
			set
			{
				this.host = value;
			}
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x060068F7 RID: 26871 RVA: 0x00181A60 File Offset: 0x00180A60
		internal override bool ForceReadOnly
		{
			get
			{
				if (!this.forceReadOnlyChecked)
				{
					ReadOnlyAttribute readOnlyAttribute = (ReadOnlyAttribute)TypeDescriptor.GetAttributes(this.objValue)[typeof(ReadOnlyAttribute)];
					if ((readOnlyAttribute != null && !readOnlyAttribute.IsDefaultAttribute()) || TypeDescriptor.GetAttributes(this.objValue).Contains(InheritanceAttribute.InheritedReadOnly))
					{
						this.flags |= 1024;
					}
					this.forceReadOnlyChecked = true;
				}
				return base.ForceReadOnly || (this.GridEntryHost != null && !this.GridEntryHost.Enabled);
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x060068F8 RID: 26872 RVA: 0x00181AF2 File Offset: 0x00180AF2
		// (set) Token: 0x060068F9 RID: 26873 RVA: 0x00181AFA File Offset: 0x00180AFA
		internal override PropertyGridView GridEntryHost
		{
			get
			{
				return this.gridEntryHost;
			}
			set
			{
				this.gridEntryHost = value;
			}
		}

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x060068FA RID: 26874 RVA: 0x00181B03 File Offset: 0x00180B03
		public override GridItemType GridItemType
		{
			get
			{
				return GridItemType.Root;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x060068FB RID: 26875 RVA: 0x00181B08 File Offset: 0x00180B08
		public override string HelpKeyword
		{
			get
			{
				HelpKeywordAttribute helpKeywordAttribute = (HelpKeywordAttribute)TypeDescriptor.GetAttributes(this.objValue)[typeof(HelpKeywordAttribute)];
				if (helpKeywordAttribute != null && !helpKeywordAttribute.IsDefaultAttribute())
				{
					return helpKeywordAttribute.HelpKeyword;
				}
				return this.objValueClassName;
			}
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x060068FC RID: 26876 RVA: 0x00181B50 File Offset: 0x00180B50
		public override string PropertyLabel
		{
			get
			{
				if (this.objValue is IComponent)
				{
					ISite site = ((IComponent)this.objValue).Site;
					if (site == null)
					{
						return this.objValue.GetType().Name;
					}
					return site.Name;
				}
				else
				{
					if (this.objValue != null)
					{
						return this.objValue.ToString();
					}
					return null;
				}
			}
		}

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x060068FD RID: 26877 RVA: 0x00181BAB File Offset: 0x00180BAB
		// (set) Token: 0x060068FE RID: 26878 RVA: 0x00181BB4 File Offset: 0x00180BB4
		public override object PropertyValue
		{
			get
			{
				return this.objValue;
			}
			set
			{
				object oldObject = this.objValue;
				this.objValue = value;
				this.objValueClassName = TypeDescriptor.GetClassName(this.objValue);
				this.ownerGrid.ReplaceSelectedObject(oldObject, value);
			}
		}

		// Token: 0x060068FF RID: 26879 RVA: 0x00181BF0 File Offset: 0x00180BF0
		protected override bool CreateChildren()
		{
			bool result = base.CreateChildren();
			this.CategorizePropEntries();
			return result;
		}

		// Token: 0x06006900 RID: 26880 RVA: 0x00181C0C File Offset: 0x00180C0C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.host = null;
				this.baseProvider = null;
				this.tab = null;
				this.gridEntryHost = null;
				this.changeService = null;
			}
			this.objValue = null;
			this.objValueClassName = null;
			this.propDefault = null;
			base.Dispose(disposing);
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x00181C5C File Offset: 0x00180C5C
		public override object GetService(Type serviceType)
		{
			object obj = null;
			if (this.host != null)
			{
				obj = this.host.GetService(serviceType);
			}
			if (obj == null && this.baseProvider != null)
			{
				obj = this.baseProvider.GetService(serviceType);
			}
			return obj;
		}

		// Token: 0x06006902 RID: 26882 RVA: 0x00181C9C File Offset: 0x00180C9C
		public void ResetBrowsableAttributes()
		{
			this.browsableAttributes = new AttributeCollection(new Attribute[]
			{
				BrowsableAttribute.Yes
			});
		}

		// Token: 0x06006903 RID: 26883 RVA: 0x00181CC4 File Offset: 0x00180CC4
		public virtual void ShowCategories(bool fCategories)
		{
			if ((this.PropertySort &= PropertySort.Categorized) != PropertySort.NoSort != fCategories)
			{
				if (fCategories)
				{
					this.PropertySort |= PropertySort.Categorized;
				}
				else
				{
					this.PropertySort &= (PropertySort)(-3);
				}
				if (this.Expandable && base.ChildCollection != null)
				{
					this.CreateChildren();
				}
			}
		}

		// Token: 0x06006904 RID: 26884 RVA: 0x00181D24 File Offset: 0x00180D24
		internal void CategorizePropEntries()
		{
			if (this.Children.Count > 0)
			{
				GridEntry[] array = new GridEntry[this.Children.Count];
				this.Children.CopyTo(array, 0);
				if ((this.PropertySort & PropertySort.Categorized) != PropertySort.NoSort)
				{
					Hashtable hashtable = new Hashtable();
					foreach (GridEntry gridEntry in array)
					{
						if (gridEntry != null)
						{
							string propertyCategory = gridEntry.PropertyCategory;
							ArrayList arrayList = (ArrayList)hashtable[propertyCategory];
							if (arrayList == null)
							{
								arrayList = new ArrayList();
								hashtable[propertyCategory] = arrayList;
							}
							arrayList.Add(gridEntry);
						}
					}
					ArrayList arrayList2 = new ArrayList();
					IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
					while (enumerator.MoveNext())
					{
						ArrayList arrayList3 = (ArrayList)enumerator.Value;
						if (arrayList3 != null)
						{
							string name = (string)enumerator.Key;
							if (arrayList3.Count > 0)
							{
								GridEntry[] array2 = new GridEntry[arrayList3.Count];
								arrayList3.CopyTo(array2, 0);
								try
								{
									arrayList2.Add(new CategoryGridEntry(this.ownerGrid, this, name, array2));
								}
								catch
								{
								}
							}
						}
					}
					array = new GridEntry[arrayList2.Count];
					arrayList2.CopyTo(array, 0);
					StringSorter.Sort(array);
					base.ChildCollection.Clear();
					base.ChildCollection.AddRange(array);
				}
			}
		}

		// Token: 0x04003DAD RID: 15789
		protected object objValue;

		// Token: 0x04003DAE RID: 15790
		protected string objValueClassName;

		// Token: 0x04003DAF RID: 15791
		protected GridEntry propDefault;

		// Token: 0x04003DB0 RID: 15792
		protected IDesignerHost host;

		// Token: 0x04003DB1 RID: 15793
		protected IServiceProvider baseProvider;

		// Token: 0x04003DB2 RID: 15794
		protected PropertyTab tab;

		// Token: 0x04003DB3 RID: 15795
		protected PropertyGridView gridEntryHost;

		// Token: 0x04003DB4 RID: 15796
		protected AttributeCollection browsableAttributes;

		// Token: 0x04003DB5 RID: 15797
		private IComponentChangeService changeService;

		// Token: 0x04003DB6 RID: 15798
		protected bool forceReadOnlyChecked;
	}
}
