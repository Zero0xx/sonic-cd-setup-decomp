using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007C0 RID: 1984
	internal class MultiPropertyDescriptorGridEntry : PropertyDescriptorGridEntry
	{
		// Token: 0x060068DC RID: 26844 RVA: 0x001811C6 File Offset: 0x001801C6
		public MultiPropertyDescriptorGridEntry(PropertyGrid ownerGrid, GridEntry peParent, object[] objectArray, PropertyDescriptor[] propInfo, bool hide) : base(ownerGrid, peParent, hide)
		{
			this.mergedPd = new MergePropertyDescriptor(propInfo);
			this.objs = objectArray;
			base.Initialize(this.mergedPd);
		}

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x060068DD RID: 26845 RVA: 0x001811F4 File Offset: 0x001801F4
		public override IContainer Container
		{
			get
			{
				IContainer container = null;
				object[] array = this.objs;
				int i = 0;
				while (i < array.Length)
				{
					object obj = array[i];
					IComponent component = obj as IComponent;
					if (component == null)
					{
						container = null;
						break;
					}
					if (component.Site != null)
					{
						if (container == null)
						{
							container = component.Site.Container;
						}
						else if (container != component.Site.Container)
						{
							goto IL_48;
						}
						i++;
						continue;
					}
					IL_48:
					container = null;
					break;
				}
				return container;
			}
		}

		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x060068DE RID: 26846 RVA: 0x0018125C File Offset: 0x0018025C
		public override bool Expandable
		{
			get
			{
				bool flag = this.GetFlagSet(131072);
				if (flag && base.ChildCollection.Count > 0)
				{
					return true;
				}
				if (this.GetFlagSet(524288))
				{
					return false;
				}
				try
				{
					object[] values = this.mergedPd.GetValues(this.objs);
					for (int i = 0; i < values.Length; i++)
					{
						if (values[i] == null)
						{
							flag = false;
							break;
						}
					}
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x17001658 RID: 5720
		// (set) Token: 0x060068DF RID: 26847 RVA: 0x001812DC File Offset: 0x001802DC
		public override object PropertyValue
		{
			set
			{
				base.PropertyValue = value;
				base.RecreateChildren();
				if (this.Expanded)
				{
					this.GridEntryHost.Refresh(false);
				}
			}
		}

		// Token: 0x060068E0 RID: 26848 RVA: 0x001812FF File Offset: 0x001802FF
		protected override bool CreateChildren()
		{
			return this.CreateChildren(false);
		}

		// Token: 0x060068E1 RID: 26849 RVA: 0x00181308 File Offset: 0x00180308
		protected override bool CreateChildren(bool diffOldChildren)
		{
			bool result;
			try
			{
				if (this.mergedPd.PropertyType.IsValueType || (this.Flags & 512) != 0)
				{
					result = base.CreateChildren(diffOldChildren);
				}
				else
				{
					base.ChildCollection.Clear();
					MultiPropertyDescriptorGridEntry[] mergedProperties = MultiSelectRootGridEntry.PropertyMerger.GetMergedProperties(this.mergedPd.GetValues(this.objs), this, this.PropertySort, this.CurrentTab);
					if (mergedProperties != null)
					{
						base.ChildCollection.AddRange(mergedProperties);
					}
					bool flag = this.Children.Count > 0;
					if (!flag)
					{
						this.SetFlag(524288, true);
					}
					result = flag;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060068E2 RID: 26850 RVA: 0x001813B8 File Offset: 0x001803B8
		public override object GetChildValueOwner(GridEntry childEntry)
		{
			if (this.mergedPd.PropertyType.IsValueType || (this.Flags & 512) != 0)
			{
				return base.GetChildValueOwner(childEntry);
			}
			return this.mergedPd.GetValues(this.objs);
		}

		// Token: 0x060068E3 RID: 26851 RVA: 0x001813F4 File Offset: 0x001803F4
		public override IComponent[] GetComponents()
		{
			IComponent[] array = new IComponent[this.objs.Length];
			Array.Copy(this.objs, 0, array, 0, this.objs.Length);
			return array;
		}

		// Token: 0x060068E4 RID: 26852 RVA: 0x00181428 File Offset: 0x00180428
		public override string GetPropertyTextValue(object value)
		{
			bool flag = true;
			try
			{
				if (value == null && this.mergedPd.GetValue(this.objs, out flag) == null && !flag)
				{
					return "";
				}
			}
			catch
			{
				return "";
			}
			return base.GetPropertyTextValue(value);
		}

		// Token: 0x060068E5 RID: 26853 RVA: 0x00181480 File Offset: 0x00180480
		internal override bool NotifyChildValue(GridEntry pe, int type)
		{
			bool result = false;
			IDesignerHost designerHost = this.DesignerHost;
			DesignerTransaction designerTransaction = null;
			if (designerHost != null)
			{
				designerTransaction = designerHost.CreateTransaction();
			}
			try
			{
				result = base.NotifyChildValue(pe, type);
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
			return result;
		}

		// Token: 0x060068E6 RID: 26854 RVA: 0x001814C8 File Offset: 0x001804C8
		protected override void NotifyParentChange(GridEntry ge)
		{
			while (ge != null && ge is PropertyDescriptorGridEntry && ((PropertyDescriptorGridEntry)ge).propertyInfo.Attributes.Contains(NotifyParentPropertyAttribute.Yes))
			{
				object valueOwner = ge.GetValueOwner();
				while (!(ge is PropertyDescriptorGridEntry) || this.OwnersEqual(valueOwner, ge.GetValueOwner()))
				{
					ge = ge.ParentGridEntry;
					if (ge == null)
					{
						break;
					}
				}
				if (ge != null)
				{
					valueOwner = ge.GetValueOwner();
					IComponentChangeService componentChangeService = this.ComponentChangeService;
					if (componentChangeService != null)
					{
						Array array = valueOwner as Array;
						if (array != null)
						{
							for (int i = 0; i < array.Length; i++)
							{
								PropertyDescriptor propertyDescriptor = ((PropertyDescriptorGridEntry)ge).propertyInfo;
								if (propertyDescriptor is MergePropertyDescriptor)
								{
									propertyDescriptor = ((MergePropertyDescriptor)propertyDescriptor)[i];
								}
								if (propertyDescriptor != null)
								{
									componentChangeService.OnComponentChanging(array.GetValue(i), propertyDescriptor);
									componentChangeService.OnComponentChanged(array.GetValue(i), propertyDescriptor, null, null);
								}
							}
						}
						else
						{
							componentChangeService.OnComponentChanging(valueOwner, ((PropertyDescriptorGridEntry)ge).propertyInfo);
							componentChangeService.OnComponentChanged(valueOwner, ((PropertyDescriptorGridEntry)ge).propertyInfo, null, null);
						}
					}
				}
			}
		}

		// Token: 0x060068E7 RID: 26855 RVA: 0x001815D8 File Offset: 0x001805D8
		internal override bool NotifyValueGivenParent(object obj, int type)
		{
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.propertyInfo);
			}
			switch (type)
			{
			case 1:
			{
				object[] array = (object[])obj;
				if (array != null && array.Length > 0)
				{
					IDesignerHost designerHost = this.DesignerHost;
					DesignerTransaction designerTransaction = null;
					if (designerHost != null)
					{
						designerTransaction = designerHost.CreateTransaction(SR.GetString("PropertyGridResetValue", new object[]
						{
							this.PropertyName
						}));
					}
					try
					{
						bool flag = !(array[0] is IComponent) || ((IComponent)array[0]).Site == null;
						if (flag && !this.OnComponentChanging())
						{
							if (designerTransaction != null)
							{
								designerTransaction.Cancel();
								designerTransaction = null;
							}
							return false;
						}
						this.mergedPd.ResetValue(obj);
						if (flag)
						{
							this.OnComponentChanged();
						}
						this.NotifyParentChange(this);
					}
					finally
					{
						if (designerTransaction != null)
						{
							designerTransaction.Commit();
						}
					}
					return false;
				}
				return false;
			}
			case 3:
			case 5:
			{
				MergePropertyDescriptor mergePropertyDescriptor = this.propertyInfo as MergePropertyDescriptor;
				if (mergePropertyDescriptor != null)
				{
					object[] array2 = (object[])obj;
					if (this.eventBindings == null)
					{
						this.eventBindings = (IEventBindingService)this.GetService(typeof(IEventBindingService));
					}
					if (this.eventBindings != null)
					{
						EventDescriptor @event = this.eventBindings.GetEvent(mergePropertyDescriptor[0]);
						if (@event != null)
						{
							return base.ViewEvent(obj, null, @event, true);
						}
					}
					return false;
				}
				return base.NotifyValueGivenParent(obj, type);
			}
			}
			return base.NotifyValueGivenParent(obj, type);
		}

		// Token: 0x060068E8 RID: 26856 RVA: 0x00181760 File Offset: 0x00180760
		private bool OwnersEqual(object owner1, object owner2)
		{
			if (!(owner1 is Array))
			{
				return owner1 == owner2;
			}
			Array array = owner1 as Array;
			Array array2 = owner2 as Array;
			if (array != null && array2 != null && array.Length == array2.Length)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array.GetValue(i) != array2.GetValue(i))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060068E9 RID: 26857 RVA: 0x001817C4 File Offset: 0x001807C4
		public override bool OnComponentChanging()
		{
			if (this.ComponentChangeService != null)
			{
				int num = this.objs.Length;
				for (int i = 0; i < num; i++)
				{
					try
					{
						this.ComponentChangeService.OnComponentChanging(this.objs[i], this.mergedPd[i]);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return false;
						}
						throw ex;
					}
				}
			}
			return true;
		}

		// Token: 0x060068EA RID: 26858 RVA: 0x00181834 File Offset: 0x00180834
		public override void OnComponentChanged()
		{
			if (this.ComponentChangeService != null)
			{
				int num = this.objs.Length;
				for (int i = 0; i < num; i++)
				{
					this.ComponentChangeService.OnComponentChanged(this.objs[i], this.mergedPd[i], null, null);
				}
			}
		}

		// Token: 0x04003DAB RID: 15787
		private MergePropertyDescriptor mergedPd;

		// Token: 0x04003DAC RID: 15788
		private object[] objs;
	}
}
