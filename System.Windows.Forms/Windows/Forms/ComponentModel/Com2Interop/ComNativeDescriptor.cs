using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000776 RID: 1910
	internal class ComNativeDescriptor : TypeDescriptionProvider
	{
		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x0600646E RID: 25710 RVA: 0x0016F7F0 File Offset: 0x0016E7F0
		internal static ComNativeDescriptor Instance
		{
			get
			{
				if (ComNativeDescriptor.handler == null)
				{
					ComNativeDescriptor.handler = new ComNativeDescriptor();
				}
				return ComNativeDescriptor.handler;
			}
		}

		// Token: 0x0600646F RID: 25711 RVA: 0x0016F808 File Offset: 0x0016E808
		public static object GetNativePropertyValue(object component, string propertyName, ref bool succeeded)
		{
			return ComNativeDescriptor.Instance.GetPropertyValue(component, propertyName, ref succeeded);
		}

		// Token: 0x06006470 RID: 25712 RVA: 0x0016F817 File Offset: 0x0016E817
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			return new ComNativeDescriptor.ComTypeDescriptor(this, instance);
		}

		// Token: 0x06006471 RID: 25713 RVA: 0x0016F820 File Offset: 0x0016E820
		internal string GetClassName(object component)
		{
			string text = null;
			if (component is NativeMethods.IVsPerPropertyBrowsing)
			{
				int className = ((NativeMethods.IVsPerPropertyBrowsing)component).GetClassName(ref text);
				if (NativeMethods.Succeeded(className) && text != null)
				{
					return text;
				}
			}
			UnsafeNativeMethods.ITypeInfo typeInfo = Com2TypeInfoProcessor.FindTypeInfo(component, true);
			if (typeInfo == null)
			{
				return "";
			}
			if (typeInfo != null)
			{
				string text2 = null;
				try
				{
					typeInfo.GetDocumentation(-1, ref text, ref text2, null, null);
					while (text != null && text.Length > 0 && text[0] == '_')
					{
						text = text.Substring(1);
					}
					return text;
				}
				catch
				{
				}
			}
			return "";
		}

		// Token: 0x06006472 RID: 25714 RVA: 0x0016F8B8 File Offset: 0x0016E8B8
		internal TypeConverter GetConverter(object component)
		{
			return TypeDescriptor.GetConverter(typeof(IComponent));
		}

		// Token: 0x06006473 RID: 25715 RVA: 0x0016F8C9 File Offset: 0x0016E8C9
		internal object GetEditor(object component, Type baseEditorType)
		{
			return TypeDescriptor.GetEditor(component.GetType(), baseEditorType);
		}

		// Token: 0x06006474 RID: 25716 RVA: 0x0016F8D8 File Offset: 0x0016E8D8
		internal string GetName(object component)
		{
			if (!(component is UnsafeNativeMethods.IDispatch))
			{
				return "";
			}
			int nameDispId = Com2TypeInfoProcessor.GetNameDispId((UnsafeNativeMethods.IDispatch)component);
			if (nameDispId != -1)
			{
				bool flag = false;
				object propertyValue = this.GetPropertyValue(component, nameDispId, ref flag);
				if (flag && propertyValue != null)
				{
					return propertyValue.ToString();
				}
			}
			return "";
		}

		// Token: 0x06006475 RID: 25717 RVA: 0x0016F924 File Offset: 0x0016E924
		internal object GetPropertyValue(object component, string propertyName, ref bool succeeded)
		{
			if (!(component is UnsafeNativeMethods.IDispatch))
			{
				return null;
			}
			UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)component;
			string[] rgszNames = new string[]
			{
				propertyName
			};
			int[] array = new int[]
			{
				-1
			};
			Guid empty = Guid.Empty;
			try
			{
				int idsOfNames = dispatch.GetIDsOfNames(ref empty, rgszNames, 1, SafeNativeMethods.GetThreadLCID(), array);
				if (array[0] == -1 || NativeMethods.Failed(idsOfNames))
				{
					return null;
				}
			}
			catch
			{
				return null;
			}
			return this.GetPropertyValue(component, array[0], ref succeeded);
		}

		// Token: 0x06006476 RID: 25718 RVA: 0x0016F9B0 File Offset: 0x0016E9B0
		internal object GetPropertyValue(object component, int dispid, ref bool succeeded)
		{
			if (!(component is UnsafeNativeMethods.IDispatch))
			{
				return null;
			}
			object[] array = new object[1];
			if (this.GetPropertyValue(component, dispid, array) == 0)
			{
				succeeded = true;
				return array[0];
			}
			succeeded = false;
			return null;
		}

		// Token: 0x06006477 RID: 25719 RVA: 0x0016F9E4 File Offset: 0x0016E9E4
		internal int GetPropertyValue(object component, int dispid, object[] retval)
		{
			if (!(component is UnsafeNativeMethods.IDispatch))
			{
				return -2147467262;
			}
			UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)component;
			try
			{
				Guid empty = Guid.Empty;
				NativeMethods.tagEXCEPINFO tagEXCEPINFO = new NativeMethods.tagEXCEPINFO();
				int num;
				try
				{
					num = dispatch.Invoke(dispid, ref empty, SafeNativeMethods.GetThreadLCID(), 2, new NativeMethods.tagDISPPARAMS(), retval, tagEXCEPINFO, null);
					if (num == -2147352567)
					{
						num = tagEXCEPINFO.scode;
					}
				}
				catch (ExternalException ex)
				{
					num = ex.ErrorCode;
				}
				return num;
			}
			catch
			{
			}
			return -2147467259;
		}

		// Token: 0x06006478 RID: 25720 RVA: 0x0016FA74 File Offset: 0x0016EA74
		internal bool IsNameDispId(object obj, int dispid)
		{
			return obj != null && obj.GetType().IsCOMObject && dispid == Com2TypeInfoProcessor.GetNameDispId((UnsafeNativeMethods.IDispatch)obj);
		}

		// Token: 0x06006479 RID: 25721 RVA: 0x0016FA98 File Offset: 0x0016EA98
		private void CheckClear(object component)
		{
			if (++this.clearCount % 25 == 0)
			{
				lock (this.nativeProps)
				{
					this.clearCount = 0;
					List<object> list = null;
					foreach (object obj2 in this.nativeProps)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
						Com2Properties com2Properties = dictionaryEntry.Value as Com2Properties;
						if (com2Properties != null && com2Properties.TooOld)
						{
							if (list == null)
							{
								list = new List<object>();
							}
							list.Add(dictionaryEntry.Key);
						}
					}
					if (list != null)
					{
						for (int i = list.Count - 1; i >= 0; i--)
						{
							object key = list[i];
							Com2Properties com2Properties = this.nativeProps[key] as Com2Properties;
							if (com2Properties != null)
							{
								com2Properties.Disposed -= this.OnPropsInfoDisposed;
								com2Properties.Dispose();
								this.nativeProps.Remove(key);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600647A RID: 25722 RVA: 0x0016FBC4 File Offset: 0x0016EBC4
		private Com2Properties GetPropsInfo(object component)
		{
			this.CheckClear(component);
			Com2Properties com2Properties = (Com2Properties)this.nativeProps[component];
			if (com2Properties == null || !com2Properties.CheckValid())
			{
				com2Properties = Com2TypeInfoProcessor.GetProperties(component);
				if (com2Properties != null)
				{
					com2Properties.Disposed += this.OnPropsInfoDisposed;
					lock (this.nativeProps)
					{
						this.nativeProps.SetWeak(component, com2Properties);
					}
					com2Properties.AddExtendedBrowsingHandlers(this.extendedBrowsingHandlers);
				}
			}
			return com2Properties;
		}

		// Token: 0x0600647B RID: 25723 RVA: 0x0016FC54 File Offset: 0x0016EC54
		internal AttributeCollection GetAttributes(object component)
		{
			ArrayList arrayList = new ArrayList();
			if (component is NativeMethods.IManagedPerPropertyBrowsing)
			{
				object[] componentAttributes = Com2IManagedPerPropertyBrowsingHandler.GetComponentAttributes((NativeMethods.IManagedPerPropertyBrowsing)component, -1);
				for (int i = 0; i < componentAttributes.Length; i++)
				{
					arrayList.Add(componentAttributes[i]);
				}
			}
			if (Com2ComponentEditor.NeedsComponentEditor(component))
			{
				EditorAttribute value = new EditorAttribute(typeof(Com2ComponentEditor), typeof(ComponentEditor));
				arrayList.Add(value);
			}
			if (arrayList == null || arrayList.Count == 0)
			{
				return this.staticAttrs;
			}
			Attribute[] array = new Attribute[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return new AttributeCollection(array);
		}

		// Token: 0x0600647C RID: 25724 RVA: 0x0016FCF0 File Offset: 0x0016ECF0
		internal PropertyDescriptor GetDefaultProperty(object component)
		{
			this.CheckClear(component);
			Com2Properties propsInfo = this.GetPropsInfo(component);
			if (propsInfo != null)
			{
				return propsInfo.DefaultProperty;
			}
			return null;
		}

		// Token: 0x0600647D RID: 25725 RVA: 0x0016FD17 File Offset: 0x0016ED17
		internal EventDescriptorCollection GetEvents(object component)
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x0600647E RID: 25726 RVA: 0x0016FD1F File Offset: 0x0016ED1F
		internal EventDescriptorCollection GetEvents(object component, Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x0600647F RID: 25727 RVA: 0x0016FD27 File Offset: 0x0016ED27
		internal EventDescriptor GetDefaultEvent(object component)
		{
			return null;
		}

		// Token: 0x06006480 RID: 25728 RVA: 0x0016FD2C File Offset: 0x0016ED2C
		internal PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			Com2Properties propsInfo = this.GetPropsInfo(component);
			if (propsInfo == null)
			{
				return PropertyDescriptorCollection.Empty;
			}
			PropertyDescriptorCollection result;
			try
			{
				propsInfo.AlwaysValid = true;
				PropertyDescriptor[] properties = propsInfo.Properties;
				result = new PropertyDescriptorCollection(properties);
			}
			finally
			{
				propsInfo.AlwaysValid = false;
			}
			return result;
		}

		// Token: 0x06006481 RID: 25729 RVA: 0x0016FD7C File Offset: 0x0016ED7C
		private void OnPropsInfoDisposed(object sender, EventArgs e)
		{
			Com2Properties com2Properties = sender as Com2Properties;
			if (com2Properties != null)
			{
				com2Properties.Disposed -= this.OnPropsInfoDisposed;
				lock (this.nativeProps)
				{
					object obj2 = com2Properties.TargetObject;
					if (obj2 == null && this.nativeProps.ContainsValue(com2Properties))
					{
						foreach (object obj3 in this.nativeProps)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
							if (dictionaryEntry.Value == com2Properties)
							{
								obj2 = dictionaryEntry.Key;
								break;
							}
						}
						if (obj2 == null)
						{
							return;
						}
					}
					this.nativeProps.Remove(obj2);
				}
			}
		}

		// Token: 0x06006482 RID: 25730 RVA: 0x0016FE54 File Offset: 0x0016EE54
		internal static void ResolveVariantTypeConverterAndTypeEditor(object propertyValue, ref TypeConverter currentConverter, Type editorType, ref object currentEditor)
		{
			if (propertyValue != null && propertyValue != null && !Convert.IsDBNull(propertyValue))
			{
				Type type = propertyValue.GetType();
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (converter != null && converter.GetType() != typeof(TypeConverter))
				{
					currentConverter = converter;
				}
				object editor = TypeDescriptor.GetEditor(type, editorType);
				if (editor != null)
				{
					currentEditor = editor;
				}
			}
		}

		// Token: 0x04003BCE RID: 15310
		private const int CLEAR_INTERVAL = 25;

		// Token: 0x04003BCF RID: 15311
		private static ComNativeDescriptor handler;

		// Token: 0x04003BD0 RID: 15312
		private AttributeCollection staticAttrs = new AttributeCollection(new Attribute[]
		{
			BrowsableAttribute.Yes,
			DesignTimeVisibleAttribute.No
		});

		// Token: 0x04003BD1 RID: 15313
		private WeakHashtable nativeProps = new WeakHashtable();

		// Token: 0x04003BD2 RID: 15314
		private Hashtable extendedBrowsingHandlers = new Hashtable();

		// Token: 0x04003BD3 RID: 15315
		private int clearCount;

		// Token: 0x02000777 RID: 1911
		private sealed class ComTypeDescriptor : ICustomTypeDescriptor
		{
			// Token: 0x06006484 RID: 25732 RVA: 0x0016FEF4 File Offset: 0x0016EEF4
			internal ComTypeDescriptor(ComNativeDescriptor handler, object instance)
			{
				this._handler = handler;
				this._instance = instance;
			}

			// Token: 0x06006485 RID: 25733 RVA: 0x0016FF0A File Offset: 0x0016EF0A
			AttributeCollection ICustomTypeDescriptor.GetAttributes()
			{
				return this._handler.GetAttributes(this._instance);
			}

			// Token: 0x06006486 RID: 25734 RVA: 0x0016FF1D File Offset: 0x0016EF1D
			string ICustomTypeDescriptor.GetClassName()
			{
				return this._handler.GetClassName(this._instance);
			}

			// Token: 0x06006487 RID: 25735 RVA: 0x0016FF30 File Offset: 0x0016EF30
			string ICustomTypeDescriptor.GetComponentName()
			{
				return this._handler.GetName(this._instance);
			}

			// Token: 0x06006488 RID: 25736 RVA: 0x0016FF43 File Offset: 0x0016EF43
			TypeConverter ICustomTypeDescriptor.GetConverter()
			{
				return this._handler.GetConverter(this._instance);
			}

			// Token: 0x06006489 RID: 25737 RVA: 0x0016FF56 File Offset: 0x0016EF56
			EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
			{
				return this._handler.GetDefaultEvent(this._instance);
			}

			// Token: 0x0600648A RID: 25738 RVA: 0x0016FF69 File Offset: 0x0016EF69
			PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
			{
				return this._handler.GetDefaultProperty(this._instance);
			}

			// Token: 0x0600648B RID: 25739 RVA: 0x0016FF7C File Offset: 0x0016EF7C
			object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
			{
				return this._handler.GetEditor(this._instance, editorBaseType);
			}

			// Token: 0x0600648C RID: 25740 RVA: 0x0016FF90 File Offset: 0x0016EF90
			EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
			{
				return this._handler.GetEvents(this._instance);
			}

			// Token: 0x0600648D RID: 25741 RVA: 0x0016FFA3 File Offset: 0x0016EFA3
			EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
			{
				return this._handler.GetEvents(this._instance, attributes);
			}

			// Token: 0x0600648E RID: 25742 RVA: 0x0016FFB7 File Offset: 0x0016EFB7
			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
			{
				return this._handler.GetProperties(this._instance, null);
			}

			// Token: 0x0600648F RID: 25743 RVA: 0x0016FFCB File Offset: 0x0016EFCB
			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
			{
				return this._handler.GetProperties(this._instance, attributes);
			}

			// Token: 0x06006490 RID: 25744 RVA: 0x0016FFDF File Offset: 0x0016EFDF
			object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
			{
				return this._instance;
			}

			// Token: 0x04003BD4 RID: 15316
			private ComNativeDescriptor _handler;

			// Token: 0x04003BD5 RID: 15317
			private object _instance;
		}
	}
}
