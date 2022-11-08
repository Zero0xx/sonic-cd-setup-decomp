using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007B7 RID: 1975
	internal class PropertyDescriptorGridEntry : GridEntry
	{
		// Token: 0x06006885 RID: 26757 RVA: 0x0017F5BF File Offset: 0x0017E5BF
		internal PropertyDescriptorGridEntry(PropertyGrid ownerGrid, GridEntry peParent, bool hide) : base(ownerGrid, peParent)
		{
			this.activeXHide = hide;
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x0017F5DB File Offset: 0x0017E5DB
		internal PropertyDescriptorGridEntry(PropertyGrid ownerGrid, GridEntry peParent, PropertyDescriptor propInfo, bool hide) : base(ownerGrid, peParent)
		{
			this.activeXHide = hide;
			this.Initialize(propInfo);
		}

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x06006887 RID: 26759 RVA: 0x0017F600 File Offset: 0x0017E600
		public override bool AllowMerge
		{
			get
			{
				MergablePropertyAttribute mergablePropertyAttribute = (MergablePropertyAttribute)this.propertyInfo.Attributes[typeof(MergablePropertyAttribute)];
				return mergablePropertyAttribute == null || mergablePropertyAttribute.IsDefaultAttribute();
			}
		}

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x06006888 RID: 26760 RVA: 0x0017F638 File Offset: 0x0017E638
		internal override AttributeCollection Attributes
		{
			get
			{
				return this.propertyInfo.Attributes;
			}
		}

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x06006889 RID: 26761 RVA: 0x0017F648 File Offset: 0x0017E648
		public override string HelpKeyword
		{
			get
			{
				if (this.helpKeyword == null)
				{
					object valueOwner = this.GetValueOwner();
					if (valueOwner == null)
					{
						return null;
					}
					HelpKeywordAttribute helpKeywordAttribute = (HelpKeywordAttribute)this.propertyInfo.Attributes[typeof(HelpKeywordAttribute)];
					if (helpKeywordAttribute != null && !helpKeywordAttribute.IsDefaultAttribute())
					{
						return helpKeywordAttribute.HelpKeyword;
					}
					if (this is ImmutablePropertyDescriptorGridEntry)
					{
						this.helpKeyword = this.PropertyName;
						GridEntry gridEntry = this;
						while (gridEntry.ParentGridEntry != null)
						{
							gridEntry = gridEntry.ParentGridEntry;
							if (gridEntry.PropertyValue == valueOwner || (valueOwner.GetType().IsValueType && valueOwner.GetType() == gridEntry.PropertyValue.GetType()))
							{
								this.helpKeyword = gridEntry.PropertyName + "." + this.helpKeyword;
								break;
							}
						}
					}
					else
					{
						Type type = this.propertyInfo.ComponentType;
						string str;
						if (type.IsCOMObject)
						{
							str = TypeDescriptor.GetClassName(valueOwner);
						}
						else
						{
							Type type2 = valueOwner.GetType();
							if (!type.IsPublic || !type.IsAssignableFrom(type2))
							{
								PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(type2)[this.PropertyName];
								if (propertyDescriptor != null)
								{
									type = propertyDescriptor.ComponentType;
								}
								else
								{
									type = null;
								}
							}
							if (type == null)
							{
								str = TypeDescriptor.GetClassName(valueOwner);
							}
							else
							{
								str = type.FullName;
							}
						}
						this.helpKeyword = str + "." + this.propertyInfo.Name;
					}
				}
				return this.helpKeyword;
			}
		}

		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x0600688A RID: 26762 RVA: 0x0017F7B2 File Offset: 0x0017E7B2
		internal override string LabelToolTipText
		{
			get
			{
				if (this.toolTipText == null)
				{
					return base.LabelToolTipText;
				}
				return this.toolTipText;
			}
		}

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x0600688B RID: 26763 RVA: 0x0017F7C9 File Offset: 0x0017E7C9
		internal override string HelpKeywordInternal
		{
			get
			{
				return this.PropertyLabel;
			}
		}

		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x0600688C RID: 26764 RVA: 0x0017F7D1 File Offset: 0x0017E7D1
		internal override bool Enumerable
		{
			get
			{
				return base.Enumerable && !this.IsPropertyReadOnly;
			}
		}

		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x0600688D RID: 26765 RVA: 0x0017F7E6 File Offset: 0x0017E7E6
		protected virtual bool IsPropertyReadOnly
		{
			get
			{
				return this.propertyInfo.IsReadOnly;
			}
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x0600688E RID: 26766 RVA: 0x0017F7F3 File Offset: 0x0017E7F3
		public override bool IsValueEditable
		{
			get
			{
				return this.exceptionConverter == null && !this.IsPropertyReadOnly && base.IsValueEditable;
			}
		}

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x0600688F RID: 26767 RVA: 0x0017F80D File Offset: 0x0017E80D
		public override bool NeedsDropDownButton
		{
			get
			{
				return base.NeedsDropDownButton && !this.IsPropertyReadOnly;
			}
		}

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x06006890 RID: 26768 RVA: 0x0017F824 File Offset: 0x0017E824
		internal bool ParensAroundName
		{
			get
			{
				if (255 == this.parensAroundName)
				{
					if (((ParenthesizePropertyNameAttribute)this.propertyInfo.Attributes[typeof(ParenthesizePropertyNameAttribute)]).NeedParenthesis)
					{
						this.parensAroundName = 1;
					}
					else
					{
						this.parensAroundName = 0;
					}
				}
				return this.parensAroundName == 1;
			}
		}

		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x06006891 RID: 26769 RVA: 0x0017F880 File Offset: 0x0017E880
		public override string PropertyCategory
		{
			get
			{
				string text = this.propertyInfo.Category;
				if (text == null || text.Length == 0)
				{
					text = base.PropertyCategory;
				}
				return text;
			}
		}

		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x06006892 RID: 26770 RVA: 0x0017F8AC File Offset: 0x0017E8AC
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propertyInfo;
			}
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x06006893 RID: 26771 RVA: 0x0017F8B4 File Offset: 0x0017E8B4
		public override string PropertyDescription
		{
			get
			{
				return this.propertyInfo.Description;
			}
		}

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x06006894 RID: 26772 RVA: 0x0017F8C4 File Offset: 0x0017E8C4
		public override string PropertyLabel
		{
			get
			{
				string text = this.propertyInfo.DisplayName;
				if (this.ParensAroundName)
				{
					text = "(" + text + ")";
				}
				return text;
			}
		}

		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x06006895 RID: 26773 RVA: 0x0017F8F7 File Offset: 0x0017E8F7
		public override string PropertyName
		{
			get
			{
				if (this.propertyInfo != null)
				{
					return this.propertyInfo.Name;
				}
				return null;
			}
		}

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x06006896 RID: 26774 RVA: 0x0017F90E File Offset: 0x0017E90E
		public override Type PropertyType
		{
			get
			{
				return this.propertyInfo.PropertyType;
			}
		}

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x06006897 RID: 26775 RVA: 0x0017F91C File Offset: 0x0017E91C
		// (set) Token: 0x06006898 RID: 26776 RVA: 0x0017F97C File Offset: 0x0017E97C
		public override object PropertyValue
		{
			get
			{
				object result;
				try
				{
					object propertyValueCore = this.GetPropertyValueCore(this.GetValueOwner());
					if (this.exceptionConverter != null)
					{
						this.SetFlagsAndExceptionInfo(0, null, null);
					}
					result = propertyValueCore;
				}
				catch (Exception ex)
				{
					if (this.exceptionConverter == null)
					{
						this.SetFlagsAndExceptionInfo(0, new PropertyDescriptorGridEntry.ExceptionConverter(), new PropertyDescriptorGridEntry.ExceptionEditor());
					}
					result = ex;
				}
				return result;
			}
			set
			{
				this.SetPropertyValue(this.GetValueOwner(), value, false, null);
			}
		}

		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x06006899 RID: 26777 RVA: 0x0017F98E File Offset: 0x0017E98E
		private IPropertyValueUIService PropertyValueUIService
		{
			get
			{
				if (!this.pvSvcChecked && this.pvSvc == null)
				{
					this.pvSvc = (IPropertyValueUIService)this.GetService(typeof(IPropertyValueUIService));
					this.pvSvcChecked = true;
				}
				return this.pvSvc;
			}
		}

		// Token: 0x0600689A RID: 26778 RVA: 0x0017F9C8 File Offset: 0x0017E9C8
		private void SetFlagsAndExceptionInfo(int flags, PropertyDescriptorGridEntry.ExceptionConverter converter, PropertyDescriptorGridEntry.ExceptionEditor editor)
		{
			this.Flags = flags;
			this.exceptionConverter = converter;
			this.exceptionEditor = editor;
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x0600689B RID: 26779 RVA: 0x0017F9E0 File Offset: 0x0017E9E0
		public override bool ShouldRenderReadOnly
		{
			get
			{
				if (base.ForceReadOnly || this.forceRenderReadOnly)
				{
					return true;
				}
				if (this.propertyInfo.IsReadOnly && !this.readOnlyVerified && this.GetFlagSet(128))
				{
					Type propertyType = this.PropertyType;
					if (propertyType != null && (propertyType.IsArray || propertyType.IsValueType || propertyType.IsPrimitive))
					{
						this.SetFlag(128, false);
						this.SetFlag(256, true);
						this.forceRenderReadOnly = true;
					}
				}
				this.readOnlyVerified = true;
				return base.ShouldRenderReadOnly && !this.isSerializeContentsProp && !base.NeedsCustomEditorButton;
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x0600689C RID: 26780 RVA: 0x0017FA84 File Offset: 0x0017EA84
		internal override TypeConverter TypeConverter
		{
			get
			{
				if (this.exceptionConverter != null)
				{
					return this.exceptionConverter;
				}
				if (this.converter == null)
				{
					this.converter = this.propertyInfo.Converter;
				}
				return base.TypeConverter;
			}
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x0600689D RID: 26781 RVA: 0x0017FAB4 File Offset: 0x0017EAB4
		internal override UITypeEditor UITypeEditor
		{
			get
			{
				if (this.exceptionEditor != null)
				{
					return this.exceptionEditor;
				}
				this.editor = (UITypeEditor)this.propertyInfo.GetEditor(typeof(UITypeEditor));
				return base.UITypeEditor;
			}
		}

		// Token: 0x0600689E RID: 26782 RVA: 0x0017FAEC File Offset: 0x0017EAEC
		internal override void EditPropertyValue(PropertyGridView iva)
		{
			base.EditPropertyValue(iva);
			if (!this.IsValueEditable)
			{
				RefreshPropertiesAttribute refreshPropertiesAttribute = (RefreshPropertiesAttribute)this.propertyInfo.Attributes[typeof(RefreshPropertiesAttribute)];
				if (refreshPropertiesAttribute != null && !refreshPropertiesAttribute.RefreshProperties.Equals(RefreshProperties.None))
				{
					this.GridEntryHost.Refresh(refreshPropertiesAttribute != null && refreshPropertiesAttribute.Equals(RefreshPropertiesAttribute.All));
				}
			}
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x0017FB60 File Offset: 0x0017EB60
		internal override Point GetLabelToolTipLocation(int mouseX, int mouseY)
		{
			if (this.pvUIItems != null)
			{
				for (int i = 0; i < this.pvUIItems.Length; i++)
				{
					if (this.uiItemRects[i].Contains(mouseX, this.GridEntryHost.GetGridEntryHeight() / 2))
					{
						this.toolTipText = this.pvUIItems[i].ToolTip;
						return new Point(mouseX, mouseY);
					}
				}
			}
			this.toolTipText = null;
			return base.GetLabelToolTipLocation(mouseX, mouseY);
		}

		// Token: 0x060068A0 RID: 26784 RVA: 0x0017FBD4 File Offset: 0x0017EBD4
		protected object GetPropertyValueCore(object target)
		{
			if (this.propertyInfo == null)
			{
				return null;
			}
			if (target is ICustomTypeDescriptor)
			{
				target = ((ICustomTypeDescriptor)target).GetPropertyOwner(this.propertyInfo);
			}
			object value;
			try
			{
				value = this.propertyInfo.GetValue(target);
			}
			catch
			{
				throw;
			}
			return value;
		}

		// Token: 0x060068A1 RID: 26785 RVA: 0x0017FC2C File Offset: 0x0017EC2C
		protected void Initialize(PropertyDescriptor propInfo)
		{
			this.propertyInfo = propInfo;
			this.isSerializeContentsProp = (this.propertyInfo.SerializationVisibility == DesignerSerializationVisibility.Content);
			if (!this.activeXHide && this.IsPropertyReadOnly)
			{
				this.SetFlag(1, false);
			}
			if (this.isSerializeContentsProp && this.TypeConverter.GetPropertiesSupported())
			{
				this.SetFlag(131072, true);
			}
		}

		// Token: 0x060068A2 RID: 26786 RVA: 0x0017FC90 File Offset: 0x0017EC90
		protected virtual void NotifyParentChange(GridEntry ge)
		{
			while (ge != null && ge is PropertyDescriptorGridEntry && ((PropertyDescriptorGridEntry)ge).propertyInfo.Attributes.Contains(NotifyParentPropertyAttribute.Yes))
			{
				object valueOwner = ge.GetValueOwner();
				while (!(ge is PropertyDescriptorGridEntry) || valueOwner == ge.GetValueOwner())
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
						componentChangeService.OnComponentChanging(valueOwner, ((PropertyDescriptorGridEntry)ge).propertyInfo);
						componentChangeService.OnComponentChanged(valueOwner, ((PropertyDescriptorGridEntry)ge).propertyInfo, null, null);
					}
					ge.ClearCachedValues(false);
					PropertyGridView gridEntryHost = this.GridEntryHost;
					if (gridEntryHost != null)
					{
						gridEntryHost.InvalidateGridEntryValue(ge);
					}
				}
			}
		}

		// Token: 0x060068A3 RID: 26787 RVA: 0x0017FD40 File Offset: 0x0017ED40
		internal override bool NotifyValueGivenParent(object obj, int type)
		{
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.propertyInfo);
			}
			switch (type)
			{
			case 1:
				this.SetPropertyValue(obj, null, true, SR.GetString("PropertyGridResetValue", new object[]
				{
					this.PropertyName
				}));
				if (this.pvUIItems != null)
				{
					for (int i = 0; i < this.pvUIItems.Length; i++)
					{
						this.pvUIItems[i].Reset();
					}
				}
				this.pvUIItems = null;
				return false;
			case 2:
				try
				{
					return this.propertyInfo.CanResetValue(obj) || (this.pvUIItems != null && this.pvUIItems.Length > 0);
				}
				catch
				{
					if (this.exceptionConverter == null)
					{
						this.Flags = 0;
						this.exceptionConverter = new PropertyDescriptorGridEntry.ExceptionConverter();
						this.exceptionEditor = new PropertyDescriptorGridEntry.ExceptionEditor();
					}
					return false;
				}
				break;
			case 3:
			case 5:
				goto IL_12A;
			case 4:
				break;
			default:
				return false;
			}
			try
			{
				return this.propertyInfo.ShouldSerializeValue(obj);
			}
			catch
			{
				if (this.exceptionConverter == null)
				{
					this.Flags = 0;
					this.exceptionConverter = new PropertyDescriptorGridEntry.ExceptionConverter();
					this.exceptionEditor = new PropertyDescriptorGridEntry.ExceptionEditor();
				}
				return false;
			}
			IL_12A:
			if (this.eventBindings == null)
			{
				this.eventBindings = (IEventBindingService)this.GetService(typeof(IEventBindingService));
			}
			if (this.eventBindings != null)
			{
				EventDescriptor @event = this.eventBindings.GetEvent(this.propertyInfo);
				if (@event != null)
				{
					return this.ViewEvent(obj, null, null, true);
				}
			}
			return false;
		}

		// Token: 0x060068A4 RID: 26788 RVA: 0x0017FEE4 File Offset: 0x0017EEE4
		public override void OnComponentChanged()
		{
			base.OnComponentChanged();
			this.NotifyParentChange(this);
		}

		// Token: 0x060068A5 RID: 26789 RVA: 0x0017FEF4 File Offset: 0x0017EEF4
		public override bool OnMouseClick(int x, int y, int count, MouseButtons button)
		{
			if (this.pvUIItems != null && count == 2 && (button & MouseButtons.Left) == MouseButtons.Left)
			{
				for (int i = 0; i < this.pvUIItems.Length; i++)
				{
					if (this.uiItemRects[i].Contains(x, this.GridEntryHost.GetGridEntryHeight() / 2))
					{
						this.pvUIItems[i].InvokeHandler(this, this.propertyInfo, this.pvUIItems[i]);
						return true;
					}
				}
			}
			return base.OnMouseClick(x, y, count, button);
		}

		// Token: 0x060068A6 RID: 26790 RVA: 0x0017FF80 File Offset: 0x0017EF80
		public override void PaintLabel(Graphics g, Rectangle rect, Rectangle clipRect, bool selected, bool paintFullLabel)
		{
			base.PaintLabel(g, rect, clipRect, selected, paintFullLabel);
			IPropertyValueUIService propertyValueUIService = this.PropertyValueUIService;
			if (propertyValueUIService == null)
			{
				return;
			}
			this.pvUIItems = propertyValueUIService.GetPropertyUIValueItems(this, this.propertyInfo);
			if (this.pvUIItems != null)
			{
				if (this.uiItemRects == null || this.uiItemRects.Length != this.pvUIItems.Length)
				{
					this.uiItemRects = new Rectangle[this.pvUIItems.Length];
				}
				for (int i = 0; i < this.pvUIItems.Length; i++)
				{
					this.uiItemRects[i] = new Rectangle(rect.Right - 9 * (i + 1), (rect.Height - 8) / 2, 8, 8);
					g.DrawImage(this.pvUIItems[i].Image, this.uiItemRects[i]);
				}
				this.GridEntryHost.LabelPaintMargin = 9 * this.pvUIItems.Length;
			}
		}

		// Token: 0x060068A7 RID: 26791 RVA: 0x00180070 File Offset: 0x0017F070
		private object SetPropertyValue(object obj, object objVal, bool reset, string undoText)
		{
			DesignerTransaction designerTransaction = null;
			try
			{
				object propertyValueCore = this.GetPropertyValueCore(obj);
				if (objVal != null && objVal.Equals(propertyValueCore))
				{
					return objVal;
				}
				base.ClearCachedValues();
				IDesignerHost designerHost = this.DesignerHost;
				if (designerHost != null)
				{
					string description = (undoText == null) ? SR.GetString("PropertyGridSetValue", new object[]
					{
						this.propertyInfo.Name
					}) : undoText;
					designerTransaction = designerHost.CreateTransaction(description);
				}
				bool flag = !(obj is IComponent) || ((IComponent)obj).Site == null;
				if (flag)
				{
					try
					{
						if (this.ComponentChangeService != null)
						{
							this.ComponentChangeService.OnComponentChanging(obj, this.propertyInfo);
						}
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return propertyValueCore;
						}
						throw ex;
					}
				}
				bool internalExpanded = this.InternalExpanded;
				int oldCount = -1;
				if (internalExpanded)
				{
					oldCount = base.ChildCount;
				}
				RefreshPropertiesAttribute refreshPropertiesAttribute = (RefreshPropertiesAttribute)this.propertyInfo.Attributes[typeof(RefreshPropertiesAttribute)];
				bool flag2 = internalExpanded || (refreshPropertiesAttribute != null && !refreshPropertiesAttribute.RefreshProperties.Equals(RefreshProperties.None));
				if (flag2)
				{
					this.DisposeChildren();
				}
				EventDescriptor eventDescriptor = null;
				if (obj != null && objVal is string)
				{
					if (this.eventBindings == null)
					{
						this.eventBindings = (IEventBindingService)this.GetService(typeof(IEventBindingService));
					}
					if (this.eventBindings != null)
					{
						eventDescriptor = this.eventBindings.GetEvent(this.propertyInfo);
					}
					if (eventDescriptor == null)
					{
						object component = obj;
						if (this.propertyInfo is MergePropertyDescriptor && obj is Array)
						{
							Array array = obj as Array;
							if (array.Length > 0)
							{
								component = array.GetValue(0);
							}
						}
						eventDescriptor = TypeDescriptor.GetEvents(component)[this.propertyInfo.Name];
					}
				}
				bool flag3 = false;
				try
				{
					if (reset)
					{
						this.propertyInfo.ResetValue(obj);
					}
					else if (eventDescriptor != null)
					{
						this.ViewEvent(obj, (string)objVal, eventDescriptor, false);
					}
					else
					{
						this.SetPropertyValueCore(obj, objVal, true);
					}
					flag3 = true;
					if (flag && this.ComponentChangeService != null)
					{
						this.ComponentChangeService.OnComponentChanged(obj, this.propertyInfo, null, objVal);
					}
					this.NotifyParentChange(this);
				}
				finally
				{
					if (flag2 && this.GridEntryHost != null)
					{
						base.RecreateChildren(oldCount);
						if (flag3)
						{
							this.GridEntryHost.Refresh(refreshPropertiesAttribute != null && refreshPropertiesAttribute.Equals(RefreshPropertiesAttribute.All));
						}
					}
				}
			}
			catch (CheckoutException ex2)
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
				if (ex2 == CheckoutException.Canceled)
				{
					return null;
				}
				throw;
			}
			catch
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
				throw;
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
			return obj;
		}

		// Token: 0x060068A8 RID: 26792 RVA: 0x00180390 File Offset: 0x0017F390
		protected void SetPropertyValueCore(object obj, object value, bool doUndo)
		{
			if (this.propertyInfo == null)
			{
				return;
			}
			Cursor value2 = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				object obj2 = obj;
				if (obj2 is ICustomTypeDescriptor)
				{
					obj2 = ((ICustomTypeDescriptor)obj2).GetPropertyOwner(this.propertyInfo);
				}
				bool flag = false;
				if (this.ParentGridEntry != null)
				{
					Type propertyType = this.ParentGridEntry.PropertyType;
					flag = (propertyType.IsValueType || propertyType.IsArray);
				}
				if (obj2 != null)
				{
					this.propertyInfo.SetValue(obj2, value);
					if (flag)
					{
						GridEntry parentGridEntry = this.ParentGridEntry;
						if (parentGridEntry != null && parentGridEntry.IsValueEditable)
						{
							parentGridEntry.PropertyValue = obj;
						}
					}
				}
			}
			finally
			{
				Cursor.Current = value2;
			}
		}

		// Token: 0x060068A9 RID: 26793 RVA: 0x00180444 File Offset: 0x0017F444
		protected bool ViewEvent(object obj, string newHandler, EventDescriptor eventdesc, bool alwaysNavigate)
		{
			object propertyValueCore = this.GetPropertyValueCore(obj);
			string text = propertyValueCore as string;
			if (text == null && propertyValueCore != null && this.TypeConverter != null && this.TypeConverter.CanConvertTo(typeof(string)))
			{
				text = this.TypeConverter.ConvertToString(propertyValueCore);
			}
			if (newHandler == null && !string.IsNullOrEmpty(text))
			{
				newHandler = text;
			}
			else if (text == newHandler && !string.IsNullOrEmpty(newHandler))
			{
				return true;
			}
			IComponent component = obj as IComponent;
			if (component == null && this.propertyInfo is MergePropertyDescriptor)
			{
				Array array = obj as Array;
				if (array != null && array.Length > 0)
				{
					component = (array.GetValue(0) as IComponent);
				}
			}
			if (component == null)
			{
				return false;
			}
			if (this.propertyInfo.IsReadOnly)
			{
				return false;
			}
			if (eventdesc == null)
			{
				if (this.eventBindings == null)
				{
					this.eventBindings = (IEventBindingService)this.GetService(typeof(IEventBindingService));
				}
				if (this.eventBindings != null)
				{
					eventdesc = this.eventBindings.GetEvent(this.propertyInfo);
				}
			}
			IDesignerHost designerHost = this.DesignerHost;
			DesignerTransaction designerTransaction = null;
			try
			{
				if (eventdesc.EventType == null)
				{
					return false;
				}
				if (designerHost != null)
				{
					string str = (component.Site != null) ? component.Site.Name : component.GetType().Name;
					designerTransaction = this.DesignerHost.CreateTransaction(SR.GetString("WindowsFormsSetEvent", new object[]
					{
						str + "." + this.PropertyName
					}));
				}
				if (this.eventBindings == null)
				{
					ISite site = component.Site;
					if (site != null)
					{
						this.eventBindings = (IEventBindingService)site.GetService(typeof(IEventBindingService));
					}
				}
				if (newHandler == null && this.eventBindings != null)
				{
					newHandler = this.eventBindings.CreateUniqueMethodName(component, eventdesc);
				}
				if (newHandler != null)
				{
					if (this.eventBindings != null)
					{
						bool flag = false;
						foreach (object obj2 in this.eventBindings.GetCompatibleMethods(eventdesc))
						{
							string b = (string)obj2;
							if (newHandler == b)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							alwaysNavigate = true;
						}
					}
					this.propertyInfo.SetValue(obj, newHandler);
				}
				if (alwaysNavigate && this.eventBindings != null)
				{
					PropertyDescriptorGridEntry.targetBindingService = this.eventBindings;
					PropertyDescriptorGridEntry.targetComponent = component;
					PropertyDescriptorGridEntry.targetEventdesc = eventdesc;
					Application.Idle += PropertyDescriptorGridEntry.ShowCodeIdle;
				}
			}
			catch
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
				throw;
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
			return true;
		}

		// Token: 0x060068AA RID: 26794 RVA: 0x0018071C File Offset: 0x0017F71C
		private static void ShowCodeIdle(object sender, EventArgs e)
		{
			Application.Idle -= PropertyDescriptorGridEntry.ShowCodeIdle;
			if (PropertyDescriptorGridEntry.targetBindingService != null)
			{
				PropertyDescriptorGridEntry.targetBindingService.ShowCode(PropertyDescriptorGridEntry.targetComponent, PropertyDescriptorGridEntry.targetEventdesc);
				PropertyDescriptorGridEntry.targetBindingService = null;
				PropertyDescriptorGridEntry.targetComponent = null;
				PropertyDescriptorGridEntry.targetEventdesc = null;
			}
		}

		// Token: 0x04003D87 RID: 15751
		private const int IMAGE_SIZE = 8;

		// Token: 0x04003D88 RID: 15752
		private const byte ParensAroundNameUnknown = 255;

		// Token: 0x04003D89 RID: 15753
		private const byte ParensAroundNameNo = 0;

		// Token: 0x04003D8A RID: 15754
		private const byte ParensAroundNameYes = 1;

		// Token: 0x04003D8B RID: 15755
		internal PropertyDescriptor propertyInfo;

		// Token: 0x04003D8C RID: 15756
		private TypeConverter exceptionConverter;

		// Token: 0x04003D8D RID: 15757
		private UITypeEditor exceptionEditor;

		// Token: 0x04003D8E RID: 15758
		private bool isSerializeContentsProp;

		// Token: 0x04003D8F RID: 15759
		private byte parensAroundName = byte.MaxValue;

		// Token: 0x04003D90 RID: 15760
		private IPropertyValueUIService pvSvc;

		// Token: 0x04003D91 RID: 15761
		protected IEventBindingService eventBindings;

		// Token: 0x04003D92 RID: 15762
		private bool pvSvcChecked;

		// Token: 0x04003D93 RID: 15763
		private PropertyValueUIItem[] pvUIItems;

		// Token: 0x04003D94 RID: 15764
		private Rectangle[] uiItemRects;

		// Token: 0x04003D95 RID: 15765
		private bool readOnlyVerified;

		// Token: 0x04003D96 RID: 15766
		private bool forceRenderReadOnly;

		// Token: 0x04003D97 RID: 15767
		private string helpKeyword;

		// Token: 0x04003D98 RID: 15768
		private string toolTipText;

		// Token: 0x04003D99 RID: 15769
		private bool activeXHide;

		// Token: 0x04003D9A RID: 15770
		private static IEventBindingService targetBindingService;

		// Token: 0x04003D9B RID: 15771
		private static IComponent targetComponent;

		// Token: 0x04003D9C RID: 15772
		private static EventDescriptor targetEventdesc;

		// Token: 0x020007B8 RID: 1976
		private class ExceptionConverter : TypeConverter
		{
			// Token: 0x060068AB RID: 26795 RVA: 0x00180768 File Offset: 0x0017F768
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType != typeof(string))
				{
					throw base.GetConvertToException(value, destinationType);
				}
				if (value is Exception)
				{
					Exception ex = (Exception)value;
					if (ex.InnerException != null)
					{
						ex = ex.InnerException;
					}
					return ex.Message;
				}
				return null;
			}
		}

		// Token: 0x020007B9 RID: 1977
		private class ExceptionEditor : UITypeEditor
		{
			// Token: 0x060068AD RID: 26797 RVA: 0x001807BC File Offset: 0x0017F7BC
			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				Exception ex = value as Exception;
				if (ex != null)
				{
					IUIService iuiservice = null;
					if (context != null)
					{
						iuiservice = (IUIService)context.GetService(typeof(IUIService));
					}
					if (iuiservice != null)
					{
						iuiservice.ShowError(ex);
					}
					else
					{
						string text = ex.Message;
						if (text == null || text.Length == 0)
						{
							text = ex.ToString();
						}
						RTLAwareMessageBox.Show(null, text, SR.GetString("PropertyGridExceptionInfo"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					}
				}
				return value;
			}

			// Token: 0x060068AE RID: 26798 RVA: 0x0018082B File Offset: 0x0017F82B
			public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}
		}
	}
}
