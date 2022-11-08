using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200074D RID: 1869
	internal class Com2PropertyDescriptor : PropertyDescriptor, ICloneable
	{
		// Token: 0x06006341 RID: 25409 RVA: 0x0016A4EC File Offset: 0x001694EC
		static Com2PropertyDescriptor()
		{
			Com2PropertyDescriptor.oleConverters[Com2PropertyDescriptor.GUID_COLOR] = typeof(Com2ColorConverter);
			Com2PropertyDescriptor.oleConverters[typeof(SafeNativeMethods.IFontDisp).GUID] = typeof(Com2FontConverter);
			Com2PropertyDescriptor.oleConverters[typeof(UnsafeNativeMethods.IFont).GUID] = typeof(Com2FontConverter);
			Com2PropertyDescriptor.oleConverters[typeof(UnsafeNativeMethods.IPictureDisp).GUID] = typeof(Com2PictureConverter);
			Com2PropertyDescriptor.oleConverters[typeof(UnsafeNativeMethods.IPicture).GUID] = typeof(Com2PictureConverter);
		}

		// Token: 0x06006342 RID: 25410 RVA: 0x0016A634 File Offset: 0x00169634
		public Com2PropertyDescriptor(int dispid, string name, Attribute[] attrs, bool readOnly, Type propType, object typeData, bool hrHidden) : base(name, attrs)
		{
			this.baseReadOnly = readOnly;
			this.readOnly = readOnly;
			this.baseAttrs = attrs;
			this.SetNeedsRefresh(32768, true);
			this.hrHidden = hrHidden;
			this.SetNeedsRefresh(4, readOnly);
			this.propertyType = propType;
			this.dispid = dispid;
			if (typeData != null)
			{
				this.typeData = typeData;
				if (typeData is Com2Enum)
				{
					this.converter = new Com2EnumConverter((Com2Enum)typeData);
				}
				else if (typeData is Guid)
				{
					this.valueConverter = this.CreateOleTypeConverter((Type)Com2PropertyDescriptor.oleConverters[(Guid)typeData]);
				}
			}
			this.canShow = true;
			if (attrs != null)
			{
				for (int i = 0; i < attrs.Length; i++)
				{
					if (attrs[i].Equals(BrowsableAttribute.No) && !hrHidden)
					{
						this.canShow = false;
						break;
					}
				}
			}
			if (this.canShow && (propType == typeof(object) || (this.valueConverter == null && propType == typeof(UnsafeNativeMethods.IDispatch))))
			{
				this.typeHide = true;
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x06006343 RID: 25411 RVA: 0x0016A74C File Offset: 0x0016974C
		// (set) Token: 0x06006344 RID: 25412 RVA: 0x0016A7EA File Offset: 0x001697EA
		protected Attribute[] BaseAttributes
		{
			get
			{
				if (this.GetNeedsRefresh(32768))
				{
					this.SetNeedsRefresh(32768, false);
					int num = (this.baseAttrs == null) ? 0 : this.baseAttrs.Length;
					ArrayList arrayList = new ArrayList();
					if (num != 0)
					{
						arrayList.AddRange(this.baseAttrs);
					}
					this.OnGetBaseAttributes(new GetAttributesEvent(arrayList));
					if (arrayList.Count != num)
					{
						this.baseAttrs = new Attribute[arrayList.Count];
					}
					if (this.baseAttrs != null)
					{
						arrayList.CopyTo(this.baseAttrs, 0);
					}
					else
					{
						this.baseAttrs = new Attribute[0];
					}
				}
				return this.baseAttrs;
			}
			set
			{
				this.baseAttrs = value;
			}
		}

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x06006345 RID: 25413 RVA: 0x0016A7F4 File Offset: 0x001697F4
		public override AttributeCollection Attributes
		{
			get
			{
				if (this.AttributesValid || this.InAttrQuery)
				{
					return base.Attributes;
				}
				this.AttributeArray = this.BaseAttributes;
				ArrayList arrayList = null;
				if (this.typeHide && this.canShow)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList(this.AttributeArray);
					}
					arrayList.Add(new BrowsableAttribute(false));
				}
				else if (this.hrHidden)
				{
					object targetObject = this.TargetObject;
					if (targetObject != null)
					{
						int propertyValue = new ComNativeDescriptor().GetPropertyValue(targetObject, this.dispid, new object[1]);
						if (NativeMethods.Succeeded(propertyValue))
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList(this.AttributeArray);
							}
							arrayList.Add(new BrowsableAttribute(true));
							this.hrHidden = false;
						}
					}
				}
				this.inAttrQuery = true;
				try
				{
					ArrayList arrayList2 = new ArrayList();
					this.OnGetDynamicAttributes(new GetAttributesEvent(arrayList2));
					if (arrayList2.Count != 0 && arrayList == null)
					{
						arrayList = new ArrayList(this.AttributeArray);
					}
					for (int i = 0; i < arrayList2.Count; i++)
					{
						Attribute value = (Attribute)arrayList2[i];
						arrayList.Add(value);
					}
				}
				finally
				{
					this.inAttrQuery = false;
				}
				this.SetNeedsRefresh(1, false);
				if (arrayList != null)
				{
					Attribute[] array = new Attribute[arrayList.Count];
					arrayList.CopyTo(array, 0);
					this.AttributeArray = array;
				}
				return base.Attributes;
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x06006346 RID: 25414 RVA: 0x0016A950 File Offset: 0x00169950
		protected bool AttributesValid
		{
			get
			{
				bool flag = !this.GetNeedsRefresh(1);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.Attributes, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					flag = !getRefreshStateEvent.Value;
					this.SetNeedsRefresh(1, getRefreshStateEvent.Value);
				}
				return flag;
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x06006347 RID: 25415 RVA: 0x0016A99A File Offset: 0x0016999A
		public bool CanShow
		{
			get
			{
				return this.canShow;
			}
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x06006348 RID: 25416 RVA: 0x0016A9A2 File Offset: 0x001699A2
		public override Type ComponentType
		{
			get
			{
				return typeof(UnsafeNativeMethods.IDispatch);
			}
		}

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x06006349 RID: 25417 RVA: 0x0016A9B0 File Offset: 0x001699B0
		public override TypeConverter Converter
		{
			get
			{
				if (this.TypeConverterValid)
				{
					return this.converter;
				}
				object obj = null;
				this.GetTypeConverterAndTypeEditor(ref this.converter, typeof(UITypeEditor), ref obj);
				if (!this.TypeEditorValid)
				{
					this.editor = obj;
					this.SetNeedsRefresh(64, false);
				}
				this.SetNeedsRefresh(32, false);
				return this.converter;
			}
		}

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x0600634A RID: 25418 RVA: 0x0016AA0D File Offset: 0x00169A0D
		public bool ConvertingNativeType
		{
			get
			{
				return this.valueConverter != null;
			}
		}

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x0600634B RID: 25419 RVA: 0x0016AA1B File Offset: 0x00169A1B
		protected virtual object DefaultValue
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x0600634C RID: 25420 RVA: 0x0016AA1E File Offset: 0x00169A1E
		public int DISPID
		{
			get
			{
				return this.dispid;
			}
		}

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x0600634D RID: 25421 RVA: 0x0016AA28 File Offset: 0x00169A28
		public override string DisplayName
		{
			get
			{
				if (!this.DisplayNameValid)
				{
					GetNameItemEvent getNameItemEvent = new GetNameItemEvent(base.DisplayName);
					this.OnGetDisplayName(getNameItemEvent);
					this.displayName = getNameItemEvent.NameString;
					this.SetNeedsRefresh(2, false);
				}
				return this.displayName;
			}
		}

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x0600634E RID: 25422 RVA: 0x0016AA6C File Offset: 0x00169A6C
		protected bool DisplayNameValid
		{
			get
			{
				bool flag = this.displayName != null && !this.GetNeedsRefresh(2);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.DisplayName, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					this.SetNeedsRefresh(2, getRefreshStateEvent.Value);
					flag = !getRefreshStateEvent.Value;
				}
				return flag;
			}
		}

		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x0600634F RID: 25423 RVA: 0x0016AAC1 File Offset: 0x00169AC1
		protected EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList();
				}
				return this.events;
			}
		}

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x06006350 RID: 25424 RVA: 0x0016AADC File Offset: 0x00169ADC
		protected bool InAttrQuery
		{
			get
			{
				return this.inAttrQuery;
			}
		}

		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x06006351 RID: 25425 RVA: 0x0016AAE4 File Offset: 0x00169AE4
		public override bool IsReadOnly
		{
			get
			{
				if (!this.ReadOnlyValid)
				{
					this.readOnly |= this.Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes);
					GetBoolValueEvent getBoolValueEvent = new GetBoolValueEvent(this.readOnly);
					this.OnGetIsReadOnly(getBoolValueEvent);
					this.readOnly = getBoolValueEvent.Value;
					this.SetNeedsRefresh(4, false);
				}
				return this.readOnly;
			}
		}

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x06006353 RID: 25427 RVA: 0x0016AB5B File Offset: 0x00169B5B
		// (set) Token: 0x06006352 RID: 25426 RVA: 0x0016AB52 File Offset: 0x00169B52
		internal Com2Properties PropertyManager
		{
			get
			{
				return this.com2props;
			}
			set
			{
				this.com2props = value;
			}
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x06006354 RID: 25428 RVA: 0x0016AB63 File Offset: 0x00169B63
		public override Type PropertyType
		{
			get
			{
				if (this.valueConverter != null)
				{
					return this.valueConverter.ManagedType;
				}
				return this.propertyType;
			}
		}

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x06006355 RID: 25429 RVA: 0x0016AB80 File Offset: 0x00169B80
		protected bool ReadOnlyValid
		{
			get
			{
				if (this.baseReadOnly)
				{
					return true;
				}
				bool flag = !this.GetNeedsRefresh(4);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.ReadOnly, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					this.SetNeedsRefresh(4, getRefreshStateEvent.Value);
					flag = !getRefreshStateEvent.Value;
				}
				return flag;
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x06006356 RID: 25430 RVA: 0x0016ABD4 File Offset: 0x00169BD4
		public virtual object TargetObject
		{
			get
			{
				if (this.com2props != null)
				{
					return this.com2props.TargetObject;
				}
				return null;
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x06006357 RID: 25431 RVA: 0x0016ABEC File Offset: 0x00169BEC
		protected bool TypeConverterValid
		{
			get
			{
				bool flag = this.converter != null && !this.GetNeedsRefresh(32);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.TypeConverter, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					this.SetNeedsRefresh(32, getRefreshStateEvent.Value);
					flag = !getRefreshStateEvent.Value;
				}
				return flag;
			}
		}

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06006358 RID: 25432 RVA: 0x0016AC44 File Offset: 0x00169C44
		protected bool TypeEditorValid
		{
			get
			{
				bool flag = this.editor != null && !this.GetNeedsRefresh(64);
				if (this.queryRefresh)
				{
					GetRefreshStateEvent getRefreshStateEvent = new GetRefreshStateEvent(Com2ShouldRefreshTypes.TypeEditor, !flag);
					this.OnShouldRefresh(getRefreshStateEvent);
					this.SetNeedsRefresh(64, getRefreshStateEvent.Value);
					flag = !getRefreshStateEvent.Value;
				}
				return flag;
			}
		}

		// Token: 0x140003EB RID: 1003
		// (add) Token: 0x06006359 RID: 25433 RVA: 0x0016AC9B File Offset: 0x00169C9B
		// (remove) Token: 0x0600635A RID: 25434 RVA: 0x0016ACAE File Offset: 0x00169CAE
		public event GetBoolValueEventHandler QueryCanResetValue
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventCanResetValue, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventCanResetValue, value);
			}
		}

		// Token: 0x140003EC RID: 1004
		// (add) Token: 0x0600635B RID: 25435 RVA: 0x0016ACC1 File Offset: 0x00169CC1
		// (remove) Token: 0x0600635C RID: 25436 RVA: 0x0016ACD4 File Offset: 0x00169CD4
		public event GetAttributesEventHandler QueryGetBaseAttributes
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetBaseAttributes, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetBaseAttributes, value);
			}
		}

		// Token: 0x140003ED RID: 1005
		// (add) Token: 0x0600635D RID: 25437 RVA: 0x0016ACE7 File Offset: 0x00169CE7
		// (remove) Token: 0x0600635E RID: 25438 RVA: 0x0016ACFA File Offset: 0x00169CFA
		public event GetAttributesEventHandler QueryGetDynamicAttributes
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetDynamicAttributes, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetDynamicAttributes, value);
			}
		}

		// Token: 0x140003EE RID: 1006
		// (add) Token: 0x0600635F RID: 25439 RVA: 0x0016AD0D File Offset: 0x00169D0D
		// (remove) Token: 0x06006360 RID: 25440 RVA: 0x0016AD20 File Offset: 0x00169D20
		public event GetNameItemEventHandler QueryGetDisplayName
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetDisplayName, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetDisplayName, value);
			}
		}

		// Token: 0x140003EF RID: 1007
		// (add) Token: 0x06006361 RID: 25441 RVA: 0x0016AD33 File Offset: 0x00169D33
		// (remove) Token: 0x06006362 RID: 25442 RVA: 0x0016AD46 File Offset: 0x00169D46
		public event GetNameItemEventHandler QueryGetDisplayValue
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetDisplayValue, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetDisplayValue, value);
			}
		}

		// Token: 0x140003F0 RID: 1008
		// (add) Token: 0x06006363 RID: 25443 RVA: 0x0016AD59 File Offset: 0x00169D59
		// (remove) Token: 0x06006364 RID: 25444 RVA: 0x0016AD6C File Offset: 0x00169D6C
		public event GetBoolValueEventHandler QueryGetIsReadOnly
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetIsReadOnly, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetIsReadOnly, value);
			}
		}

		// Token: 0x140003F1 RID: 1009
		// (add) Token: 0x06006365 RID: 25445 RVA: 0x0016AD7F File Offset: 0x00169D7F
		// (remove) Token: 0x06006366 RID: 25446 RVA: 0x0016AD92 File Offset: 0x00169D92
		public event GetTypeConverterAndTypeEditorEventHandler QueryGetTypeConverterAndTypeEditor
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventGetTypeConverterAndTypeEditor, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventGetTypeConverterAndTypeEditor, value);
			}
		}

		// Token: 0x140003F2 RID: 1010
		// (add) Token: 0x06006367 RID: 25447 RVA: 0x0016ADA5 File Offset: 0x00169DA5
		// (remove) Token: 0x06006368 RID: 25448 RVA: 0x0016ADB8 File Offset: 0x00169DB8
		public event Com2EventHandler QueryResetValue
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventResetValue, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventResetValue, value);
			}
		}

		// Token: 0x140003F3 RID: 1011
		// (add) Token: 0x06006369 RID: 25449 RVA: 0x0016ADCB File Offset: 0x00169DCB
		// (remove) Token: 0x0600636A RID: 25450 RVA: 0x0016ADDE File Offset: 0x00169DDE
		public event GetBoolValueEventHandler QueryShouldSerializeValue
		{
			add
			{
				this.Events.AddHandler(Com2PropertyDescriptor.EventShouldSerializeValue, value);
			}
			remove
			{
				this.Events.RemoveHandler(Com2PropertyDescriptor.EventShouldSerializeValue, value);
			}
		}

		// Token: 0x0600636B RID: 25451 RVA: 0x0016ADF4 File Offset: 0x00169DF4
		public override bool CanResetValue(object component)
		{
			if (component is ICustomTypeDescriptor)
			{
				component = ((ICustomTypeDescriptor)component).GetPropertyOwner(this);
			}
			if (component == this.TargetObject)
			{
				GetBoolValueEvent getBoolValueEvent = new GetBoolValueEvent(false);
				this.OnCanResetValue(getBoolValueEvent);
				return getBoolValueEvent.Value;
			}
			return false;
		}

		// Token: 0x0600636C RID: 25452 RVA: 0x0016AE36 File Offset: 0x00169E36
		public object Clone()
		{
			return new Com2PropertyDescriptor(this.dispid, this.Name, (Attribute[])this.baseAttrs.Clone(), this.readOnly, this.propertyType, this.typeData, this.hrHidden);
		}

		// Token: 0x0600636D RID: 25453 RVA: 0x0016AE74 File Offset: 0x00169E74
		private Com2DataTypeToManagedDataTypeConverter CreateOleTypeConverter(Type t)
		{
			if (t == null)
			{
				return null;
			}
			ConstructorInfo constructor = t.GetConstructor(new Type[]
			{
				typeof(Com2PropertyDescriptor)
			});
			Com2DataTypeToManagedDataTypeConverter result;
			if (constructor != null)
			{
				result = (Com2DataTypeToManagedDataTypeConverter)constructor.Invoke(new object[]
				{
					this
				});
			}
			else
			{
				result = (Com2DataTypeToManagedDataTypeConverter)Activator.CreateInstance(t);
			}
			return result;
		}

		// Token: 0x0600636E RID: 25454 RVA: 0x0016AECC File Offset: 0x00169ECC
		protected override AttributeCollection CreateAttributeCollection()
		{
			return new AttributeCollection(this.AttributeArray);
		}

		// Token: 0x0600636F RID: 25455 RVA: 0x0016AEDC File Offset: 0x00169EDC
		private TypeConverter GetBaseTypeConverter()
		{
			if (this.PropertyType == null)
			{
				return new TypeConverter();
			}
			TypeConverter typeConverter = null;
			TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)this.Attributes[typeof(TypeConverterAttribute)];
			if (typeConverterAttribute != null)
			{
				string converterTypeName = typeConverterAttribute.ConverterTypeName;
				if (converterTypeName != null && converterTypeName.Length > 0)
				{
					Type type = Type.GetType(converterTypeName);
					if (type != null && typeof(TypeConverter).IsAssignableFrom(type))
					{
						try
						{
							typeConverter = (TypeConverter)Activator.CreateInstance(type);
							if (typeConverter != null)
							{
								this.refreshState |= 8192;
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}
			if (typeConverter == null)
			{
				if (!typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(this.PropertyType))
				{
					typeConverter = base.Converter;
				}
				else
				{
					typeConverter = new Com2IDispatchConverter(this, false);
				}
			}
			if (typeConverter == null)
			{
				typeConverter = new TypeConverter();
			}
			return typeConverter;
		}

		// Token: 0x06006370 RID: 25456 RVA: 0x0016AFB4 File Offset: 0x00169FB4
		private object GetBaseTypeEditor(Type editorBaseType)
		{
			if (this.PropertyType == null)
			{
				return null;
			}
			object obj = null;
			EditorAttribute editorAttribute = (EditorAttribute)this.Attributes[typeof(EditorAttribute)];
			if (editorAttribute != null)
			{
				string editorBaseTypeName = editorAttribute.EditorBaseTypeName;
				if (editorBaseTypeName != null && editorBaseTypeName.Length > 0)
				{
					Type type = Type.GetType(editorBaseTypeName);
					if (type != null && type == editorBaseType)
					{
						Type type2 = Type.GetType(editorAttribute.EditorTypeName);
						if (type2 != null)
						{
							try
							{
								obj = Activator.CreateInstance(type2);
								if (obj != null)
								{
									this.refreshState |= 16384;
								}
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}
			if (obj == null)
			{
				obj = base.GetEditor(editorBaseType);
			}
			return obj;
		}

		// Token: 0x06006371 RID: 25457 RVA: 0x0016B05C File Offset: 0x0016A05C
		public virtual string GetDisplayValue(string defaultValue)
		{
			GetNameItemEvent getNameItemEvent = new GetNameItemEvent(defaultValue);
			this.OnGetDisplayValue(getNameItemEvent);
			return (getNameItemEvent.Name == null) ? null : getNameItemEvent.Name.ToString();
		}

		// Token: 0x06006372 RID: 25458 RVA: 0x0016B090 File Offset: 0x0016A090
		public override object GetEditor(Type editorBaseType)
		{
			if (this.TypeEditorValid)
			{
				return this.editor;
			}
			if (this.PropertyType == null)
			{
				return null;
			}
			if (editorBaseType == typeof(UITypeEditor))
			{
				TypeConverter typeConverter = null;
				this.GetTypeConverterAndTypeEditor(ref typeConverter, editorBaseType, ref this.editor);
				if (!this.TypeConverterValid)
				{
					this.converter = typeConverter;
					this.SetNeedsRefresh(32, false);
				}
				this.SetNeedsRefresh(64, false);
			}
			else
			{
				this.editor = base.GetEditor(editorBaseType);
			}
			return this.editor;
		}

		// Token: 0x06006373 RID: 25459 RVA: 0x0016B10C File Offset: 0x0016A10C
		public object GetNativeValue(object component)
		{
			if (component == null)
			{
				return null;
			}
			if (component is ICustomTypeDescriptor)
			{
				component = ((ICustomTypeDescriptor)component).GetPropertyOwner(this);
			}
			if (component == null || !Marshal.IsComObject(component) || !(component is UnsafeNativeMethods.IDispatch))
			{
				return null;
			}
			UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)component;
			object[] array = new object[1];
			NativeMethods.tagEXCEPINFO pExcepInfo = new NativeMethods.tagEXCEPINFO();
			Guid empty = Guid.Empty;
			int num = dispatch.Invoke(this.dispid, ref empty, SafeNativeMethods.GetThreadLCID(), 2, new NativeMethods.tagDISPPARAMS(), array, pExcepInfo, null);
			int num2 = num;
			if (num2 == -2147352567)
			{
				return null;
			}
			switch (num2)
			{
			case 0:
			case 1:
				if (array[0] == null || Convert.IsDBNull(array[0]))
				{
					this.lastValue = null;
				}
				else
				{
					this.lastValue = array[0];
				}
				return this.lastValue;
			default:
				throw new ExternalException(SR.GetString("DispInvokeFailed", new object[]
				{
					"GetValue",
					num
				}), num);
			}
		}

		// Token: 0x06006374 RID: 25460 RVA: 0x0016B1F8 File Offset: 0x0016A1F8
		private bool GetNeedsRefresh(int mask)
		{
			return (this.refreshState & mask) != 0;
		}

		// Token: 0x06006375 RID: 25461 RVA: 0x0016B208 File Offset: 0x0016A208
		public override object GetValue(object component)
		{
			this.lastValue = this.GetNativeValue(component);
			if (this.ConvertingNativeType && this.lastValue != null)
			{
				this.lastValue = this.valueConverter.ConvertNativeToManaged(this.lastValue, this);
			}
			else if (this.lastValue != null && this.propertyType != null && this.propertyType.IsEnum && this.lastValue.GetType().IsPrimitive)
			{
				try
				{
					this.lastValue = Enum.ToObject(this.propertyType, this.lastValue);
				}
				catch
				{
				}
			}
			return this.lastValue;
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x0016B2B0 File Offset: 0x0016A2B0
		public void GetTypeConverterAndTypeEditor(ref TypeConverter typeConverter, Type editorBaseType, ref object typeEditor)
		{
			TypeConverter typeConverter2 = typeConverter;
			object obj = typeEditor;
			if (typeConverter2 == null)
			{
				typeConverter2 = this.GetBaseTypeConverter();
			}
			if (obj == null)
			{
				obj = this.GetBaseTypeEditor(editorBaseType);
			}
			if ((this.refreshState & 8192) == 0 && this.PropertyType == typeof(Com2Variant))
			{
				Type type = this.PropertyType;
				object value = this.GetValue(this.TargetObject);
				if (value != null)
				{
					value.GetType();
				}
				ComNativeDescriptor.ResolveVariantTypeConverterAndTypeEditor(value, ref typeConverter2, editorBaseType, ref obj);
			}
			if (typeConverter2 is Com2PropertyDescriptor.Com2PropDescMainConverter)
			{
				typeConverter2 = ((Com2PropertyDescriptor.Com2PropDescMainConverter)typeConverter2).InnerConverter;
			}
			GetTypeConverterAndTypeEditorEvent getTypeConverterAndTypeEditorEvent = new GetTypeConverterAndTypeEditorEvent(typeConverter2, obj);
			this.OnGetTypeConverterAndTypeEditor(getTypeConverterAndTypeEditorEvent);
			typeConverter2 = getTypeConverterAndTypeEditorEvent.TypeConverter;
			obj = getTypeConverterAndTypeEditorEvent.TypeEditor;
			if (typeConverter2 == null)
			{
				typeConverter2 = this.GetBaseTypeConverter();
			}
			if (obj == null)
			{
				obj = this.GetBaseTypeEditor(editorBaseType);
			}
			Type type2 = typeConverter2.GetType();
			if (type2 != typeof(TypeConverter) && type2 != typeof(Com2PropertyDescriptor.Com2PropDescMainConverter))
			{
				typeConverter2 = new Com2PropertyDescriptor.Com2PropDescMainConverter(this, typeConverter2);
			}
			typeConverter = typeConverter2;
			typeEditor = obj;
		}

		// Token: 0x06006377 RID: 25463 RVA: 0x0016B399 File Offset: 0x0016A399
		public bool IsCurrentValue(object value)
		{
			return value == this.lastValue || (this.lastValue != null && this.lastValue.Equals(value));
		}

		// Token: 0x06006378 RID: 25464 RVA: 0x0016B3BC File Offset: 0x0016A3BC
		protected void OnCanResetValue(GetBoolValueEvent gvbe)
		{
			this.RaiseGetBoolValueEvent(Com2PropertyDescriptor.EventCanResetValue, gvbe);
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x0016B3CC File Offset: 0x0016A3CC
		protected void OnGetBaseAttributes(GetAttributesEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetAttributesEventHandler getAttributesEventHandler = (GetAttributesEventHandler)this.Events[Com2PropertyDescriptor.EventGetBaseAttributes];
				if (getAttributesEventHandler != null)
				{
					getAttributesEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x0016B430 File Offset: 0x0016A430
		protected void OnGetDisplayName(GetNameItemEvent gnie)
		{
			this.RaiseGetNameItemEvent(Com2PropertyDescriptor.EventGetDisplayName, gnie);
		}

		// Token: 0x0600637B RID: 25467 RVA: 0x0016B43E File Offset: 0x0016A43E
		protected void OnGetDisplayValue(GetNameItemEvent gnie)
		{
			this.RaiseGetNameItemEvent(Com2PropertyDescriptor.EventGetDisplayValue, gnie);
		}

		// Token: 0x0600637C RID: 25468 RVA: 0x0016B44C File Offset: 0x0016A44C
		protected void OnGetDynamicAttributes(GetAttributesEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetAttributesEventHandler getAttributesEventHandler = (GetAttributesEventHandler)this.Events[Com2PropertyDescriptor.EventGetDynamicAttributes];
				if (getAttributesEventHandler != null)
				{
					getAttributesEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x0600637D RID: 25469 RVA: 0x0016B4B0 File Offset: 0x0016A4B0
		protected void OnGetIsReadOnly(GetBoolValueEvent gvbe)
		{
			this.RaiseGetBoolValueEvent(Com2PropertyDescriptor.EventGetIsReadOnly, gvbe);
		}

		// Token: 0x0600637E RID: 25470 RVA: 0x0016B4C0 File Offset: 0x0016A4C0
		protected void OnGetTypeConverterAndTypeEditor(GetTypeConverterAndTypeEditorEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetTypeConverterAndTypeEditorEventHandler getTypeConverterAndTypeEditorEventHandler = (GetTypeConverterAndTypeEditorEventHandler)this.Events[Com2PropertyDescriptor.EventGetTypeConverterAndTypeEditor];
				if (getTypeConverterAndTypeEditorEventHandler != null)
				{
					getTypeConverterAndTypeEditorEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x0016B524 File Offset: 0x0016A524
		protected void OnResetValue(EventArgs e)
		{
			this.RaiseCom2Event(Com2PropertyDescriptor.EventResetValue, e);
		}

		// Token: 0x06006380 RID: 25472 RVA: 0x0016B532 File Offset: 0x0016A532
		protected void OnShouldSerializeValue(GetBoolValueEvent gvbe)
		{
			this.RaiseGetBoolValueEvent(Com2PropertyDescriptor.EventShouldSerializeValue, gvbe);
		}

		// Token: 0x06006381 RID: 25473 RVA: 0x0016B540 File Offset: 0x0016A540
		protected void OnShouldRefresh(GetRefreshStateEvent gvbe)
		{
			this.RaiseGetBoolValueEvent(Com2PropertyDescriptor.EventShouldRefresh, gvbe);
		}

		// Token: 0x06006382 RID: 25474 RVA: 0x0016B550 File Offset: 0x0016A550
		private void RaiseGetBoolValueEvent(object key, GetBoolValueEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetBoolValueEventHandler getBoolValueEventHandler = (GetBoolValueEventHandler)this.Events[key];
				if (getBoolValueEventHandler != null)
				{
					getBoolValueEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x06006383 RID: 25475 RVA: 0x0016B5B0 File Offset: 0x0016A5B0
		private void RaiseCom2Event(object key, EventArgs e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				Com2EventHandler com2EventHandler = (Com2EventHandler)this.Events[key];
				if (com2EventHandler != null)
				{
					com2EventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x06006384 RID: 25476 RVA: 0x0016B610 File Offset: 0x0016A610
		private void RaiseGetNameItemEvent(object key, GetNameItemEvent e)
		{
			try
			{
				this.com2props.AlwaysValid = this.com2props.CheckValid();
				GetNameItemEventHandler getNameItemEventHandler = (GetNameItemEventHandler)this.Events[key];
				if (getNameItemEventHandler != null)
				{
					getNameItemEventHandler(this, e);
				}
			}
			finally
			{
				this.com2props.AlwaysValid = false;
			}
		}

		// Token: 0x06006385 RID: 25477 RVA: 0x0016B670 File Offset: 0x0016A670
		public override void ResetValue(object component)
		{
			if (component is ICustomTypeDescriptor)
			{
				component = ((ICustomTypeDescriptor)component).GetPropertyOwner(this);
			}
			if (component == this.TargetObject)
			{
				this.OnResetValue(EventArgs.Empty);
			}
		}

		// Token: 0x06006386 RID: 25478 RVA: 0x0016B69C File Offset: 0x0016A69C
		internal void SetNeedsRefresh(int mask, bool value)
		{
			if (value)
			{
				this.refreshState |= mask;
				return;
			}
			this.refreshState &= ~mask;
		}

		// Token: 0x06006387 RID: 25479 RVA: 0x0016B6C0 File Offset: 0x0016A6C0
		public override void SetValue(object component, object value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException(SR.GetString("COM2ReadonlyProperty", new object[]
				{
					this.Name
				}));
			}
			if (component is ICustomTypeDescriptor)
			{
				component = ((ICustomTypeDescriptor)component).GetPropertyOwner(this);
			}
			if (component == null || !Marshal.IsComObject(component) || !(component is UnsafeNativeMethods.IDispatch))
			{
				return;
			}
			if (this.valueConverter != null)
			{
				bool flag = false;
				value = this.valueConverter.ConvertManagedToNative(value, this, ref flag);
				if (flag)
				{
					return;
				}
			}
			UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)component;
			NativeMethods.tagDISPPARAMS tagDISPPARAMS = new NativeMethods.tagDISPPARAMS();
			NativeMethods.tagEXCEPINFO tagEXCEPINFO = new NativeMethods.tagEXCEPINFO();
			tagDISPPARAMS.cArgs = 1;
			tagDISPPARAMS.cNamedArgs = 1;
			int[] array = new int[]
			{
				-3
			};
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				tagDISPPARAMS.rgdispidNamedArgs = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
				IntPtr intPtr = Marshal.AllocCoTaskMem(16);
				SafeNativeMethods.VariantInit(new HandleRef(null, intPtr));
				Marshal.GetNativeVariantForObject(value, intPtr);
				tagDISPPARAMS.rgvarg = intPtr;
				try
				{
					Guid guid = Guid.Empty;
					int num = dispatch.Invoke(this.dispid, ref guid, SafeNativeMethods.GetThreadLCID(), 4, tagDISPPARAMS, null, tagEXCEPINFO, new IntPtr[1]);
					string text = null;
					if (num == -2147352567 && tagEXCEPINFO.scode != 0)
					{
						num = tagEXCEPINFO.scode;
						text = tagEXCEPINFO.bstrDescription;
					}
					int num2 = num;
					if (num2 != -2147467260 && num2 != -2147221492)
					{
						switch (num2)
						{
						case 0:
						case 1:
							this.OnValueChanged(component, EventArgs.Empty);
							this.lastValue = value;
							break;
						default:
							if (dispatch is UnsafeNativeMethods.ISupportErrorInfo)
							{
								guid = typeof(UnsafeNativeMethods.IDispatch).GUID;
								if (NativeMethods.Succeeded(((UnsafeNativeMethods.ISupportErrorInfo)dispatch).InterfaceSupportsErrorInfo(ref guid)))
								{
									UnsafeNativeMethods.IErrorInfo errorInfo = null;
									UnsafeNativeMethods.GetErrorInfo(0, ref errorInfo);
									string text2 = null;
									if (errorInfo != null && NativeMethods.Succeeded(errorInfo.GetDescription(ref text2)))
									{
										text = text2;
									}
								}
							}
							else if (text == null)
							{
								StringBuilder stringBuilder = new StringBuilder(256);
								if (SafeNativeMethods.FormatMessage(4608, NativeMethods.NullHandleRef, num, CultureInfo.CurrentCulture.LCID, stringBuilder, 255, NativeMethods.NullHandleRef) == 0)
								{
									text = string.Format(CultureInfo.CurrentCulture, SR.GetString("DispInvokeFailed", new object[]
									{
										"SetValue",
										num
									}), new object[0]);
								}
								else
								{
									text = stringBuilder.ToString();
									while ((text.Length > 0 && text[text.Length - 1] == '\n') || text[text.Length - 1] == '\r')
									{
										text = text.Substring(0, text.Length - 1);
									}
								}
							}
							throw new ExternalException(text, num);
						}
					}
				}
				finally
				{
					SafeNativeMethods.VariantClear(new HandleRef(null, intPtr));
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			finally
			{
				gchandle.Free();
			}
		}

		// Token: 0x06006388 RID: 25480 RVA: 0x0016B9CC File Offset: 0x0016A9CC
		public override bool ShouldSerializeValue(object component)
		{
			GetBoolValueEvent getBoolValueEvent = new GetBoolValueEvent(false);
			this.OnShouldSerializeValue(getBoolValueEvent);
			return getBoolValueEvent.Value;
		}

		// Token: 0x04003B5A RID: 15194
		private EventHandlerList events;

		// Token: 0x04003B5B RID: 15195
		private bool baseReadOnly;

		// Token: 0x04003B5C RID: 15196
		private bool readOnly;

		// Token: 0x04003B5D RID: 15197
		private Type propertyType;

		// Token: 0x04003B5E RID: 15198
		private int dispid;

		// Token: 0x04003B5F RID: 15199
		private TypeConverter converter;

		// Token: 0x04003B60 RID: 15200
		private object editor;

		// Token: 0x04003B61 RID: 15201
		private string displayName;

		// Token: 0x04003B62 RID: 15202
		private object typeData;

		// Token: 0x04003B63 RID: 15203
		private int refreshState;

		// Token: 0x04003B64 RID: 15204
		private bool queryRefresh;

		// Token: 0x04003B65 RID: 15205
		private Com2Properties com2props;

		// Token: 0x04003B66 RID: 15206
		private Attribute[] baseAttrs;

		// Token: 0x04003B67 RID: 15207
		private object lastValue;

		// Token: 0x04003B68 RID: 15208
		private bool typeHide;

		// Token: 0x04003B69 RID: 15209
		private bool canShow;

		// Token: 0x04003B6A RID: 15210
		private bool hrHidden;

		// Token: 0x04003B6B RID: 15211
		private bool inAttrQuery;

		// Token: 0x04003B6C RID: 15212
		private static readonly object EventGetBaseAttributes = new object();

		// Token: 0x04003B6D RID: 15213
		private static readonly object EventGetDynamicAttributes = new object();

		// Token: 0x04003B6E RID: 15214
		private static readonly object EventShouldRefresh = new object();

		// Token: 0x04003B6F RID: 15215
		private static readonly object EventGetDisplayName = new object();

		// Token: 0x04003B70 RID: 15216
		private static readonly object EventGetDisplayValue = new object();

		// Token: 0x04003B71 RID: 15217
		private static readonly object EventGetIsReadOnly = new object();

		// Token: 0x04003B72 RID: 15218
		private static readonly object EventGetTypeConverterAndTypeEditor = new object();

		// Token: 0x04003B73 RID: 15219
		private static readonly object EventShouldSerializeValue = new object();

		// Token: 0x04003B74 RID: 15220
		private static readonly object EventCanResetValue = new object();

		// Token: 0x04003B75 RID: 15221
		private static readonly object EventResetValue = new object();

		// Token: 0x04003B76 RID: 15222
		private static readonly Guid GUID_COLOR = new Guid("{66504301-BE0F-101A-8BBB-00AA00300CAB}");

		// Token: 0x04003B77 RID: 15223
		private static IDictionary oleConverters = new SortedList();

		// Token: 0x04003B78 RID: 15224
		private Com2DataTypeToManagedDataTypeConverter valueConverter;

		// Token: 0x0200074F RID: 1871
		private class Com2PropDescMainConverter : Com2ExtendedTypeConverter
		{
			// Token: 0x06006399 RID: 25497 RVA: 0x0016BBD1 File Offset: 0x0016ABD1
			public Com2PropDescMainConverter(Com2PropertyDescriptor pd, TypeConverter baseConverter) : base(baseConverter)
			{
				this.pd = pd;
			}

			// Token: 0x0600639A RID: 25498 RVA: 0x0016BBE4 File Offset: 0x0016ABE4
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				object obj = base.ConvertTo(context, culture, value, destinationType);
				if (destinationType != typeof(string) || !this.pd.IsCurrentValue(value) || this.pd.PropertyType.IsEnum)
				{
					return obj;
				}
				Com2EnumConverter com2EnumConverter = (Com2EnumConverter)base.GetWrappedConverter(typeof(Com2EnumConverter));
				if (com2EnumConverter == null)
				{
					return this.pd.GetDisplayValue((string)obj);
				}
				return com2EnumConverter.ConvertTo(value, destinationType);
			}

			// Token: 0x0600639B RID: 25499 RVA: 0x0016BC64 File Offset: 0x0016AC64
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(value, attributes);
				if (propertyDescriptorCollection != null && propertyDescriptorCollection.Count > 0)
				{
					propertyDescriptorCollection = propertyDescriptorCollection.Sort();
					PropertyDescriptor[] array = new PropertyDescriptor[propertyDescriptorCollection.Count];
					propertyDescriptorCollection.CopyTo(array, 0);
					propertyDescriptorCollection = new PropertyDescriptorCollection(array, true);
				}
				return propertyDescriptorCollection;
			}

			// Token: 0x0600639C RID: 25500 RVA: 0x0016BCAC File Offset: 0x0016ACAC
			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				if (this.subprops == 0)
				{
					if (!base.GetPropertiesSupported(context))
					{
						this.subprops = 2;
					}
					else if ((this.pd.valueConverter != null && this.pd.valueConverter.AllowExpand) || Com2IVsPerPropertyBrowsingHandler.AllowChildProperties(this.pd))
					{
						this.subprops = 1;
					}
				}
				return this.subprops == 1;
			}

			// Token: 0x04003B7A RID: 15226
			private const int CheckSubprops = 0;

			// Token: 0x04003B7B RID: 15227
			private const int AllowSubprops = 1;

			// Token: 0x04003B7C RID: 15228
			private const int SupressSubprops = 2;

			// Token: 0x04003B7D RID: 15229
			private Com2PropertyDescriptor pd;

			// Token: 0x04003B7E RID: 15230
			private int subprops;
		}
	}
}
