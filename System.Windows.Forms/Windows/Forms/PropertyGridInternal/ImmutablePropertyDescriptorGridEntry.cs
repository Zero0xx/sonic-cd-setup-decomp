using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007BA RID: 1978
	internal class ImmutablePropertyDescriptorGridEntry : PropertyDescriptorGridEntry
	{
		// Token: 0x060068B0 RID: 26800 RVA: 0x00180836 File Offset: 0x0017F836
		internal ImmutablePropertyDescriptorGridEntry(PropertyGrid ownerGrid, GridEntry peParent, PropertyDescriptor propInfo, bool hide) : base(ownerGrid, peParent, propInfo, hide)
		{
		}

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x060068B1 RID: 26801 RVA: 0x00180843 File Offset: 0x0017F843
		protected override bool IsPropertyReadOnly
		{
			get
			{
				return this.ShouldRenderReadOnly;
			}
		}

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x060068B2 RID: 26802 RVA: 0x0018084B File Offset: 0x0017F84B
		// (set) Token: 0x060068B3 RID: 26803 RVA: 0x00180854 File Offset: 0x0017F854
		public override object PropertyValue
		{
			get
			{
				return base.PropertyValue;
			}
			set
			{
				object valueOwner = this.GetValueOwner();
				GridEntry instanceParentGridEntry = this.InstanceParentGridEntry;
				TypeConverter typeConverter = instanceParentGridEntry.TypeConverter;
				PropertyDescriptorCollection properties = typeConverter.GetProperties(instanceParentGridEntry, valueOwner);
				IDictionary dictionary = new Hashtable(properties.Count);
				object obj = null;
				for (int i = 0; i < properties.Count; i++)
				{
					if (this.propertyInfo.Name != null && this.propertyInfo.Name.Equals(properties[i].Name))
					{
						dictionary[properties[i].Name] = value;
					}
					else
					{
						dictionary[properties[i].Name] = properties[i].GetValue(valueOwner);
					}
				}
				try
				{
					obj = typeConverter.CreateInstance(instanceParentGridEntry, dictionary);
				}
				catch (Exception ex)
				{
					if (string.IsNullOrEmpty(ex.Message))
					{
						throw new TargetInvocationException(SR.GetString("ExceptionCreatingObject", new object[]
						{
							this.InstanceParentGridEntry.PropertyType.FullName,
							ex.ToString()
						}), ex);
					}
					throw;
				}
				if (obj != null)
				{
					instanceParentGridEntry.PropertyValue = obj;
				}
			}
		}

		// Token: 0x060068B4 RID: 26804 RVA: 0x00180980 File Offset: 0x0017F980
		internal override bool NotifyValueGivenParent(object obj, int type)
		{
			return this.ParentGridEntry.NotifyValue(type);
		}

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x060068B5 RID: 26805 RVA: 0x0018098E File Offset: 0x0017F98E
		public override bool ShouldRenderReadOnly
		{
			get
			{
				return this.InstanceParentGridEntry.ShouldRenderReadOnly;
			}
		}

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x060068B6 RID: 26806 RVA: 0x0018099C File Offset: 0x0017F99C
		private GridEntry InstanceParentGridEntry
		{
			get
			{
				GridEntry parentGridEntry = this.ParentGridEntry;
				if (parentGridEntry is CategoryGridEntry)
				{
					parentGridEntry = parentGridEntry.ParentGridEntry;
				}
				return parentGridEntry;
			}
		}
	}
}
