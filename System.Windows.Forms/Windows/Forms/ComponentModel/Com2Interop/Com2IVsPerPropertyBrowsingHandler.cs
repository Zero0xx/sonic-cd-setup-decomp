using System;
using System.ComponentModel;
using System.Globalization;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200075F RID: 1887
	[SuppressUnmanagedCodeSecurity]
	internal class Com2IVsPerPropertyBrowsingHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x060063F3 RID: 25587 RVA: 0x0016D191 File Offset: 0x0016C191
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.IVsPerPropertyBrowsing);
			}
		}

		// Token: 0x060063F4 RID: 25588 RVA: 0x0016D1A0 File Offset: 0x0016C1A0
		public static bool AllowChildProperties(Com2PropertyDescriptor propDesc)
		{
			if (propDesc.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				bool flag = false;
				return ((NativeMethods.IVsPerPropertyBrowsing)propDesc.TargetObject).DisplayChildProperties(propDesc.DISPID, ref flag) == 0 && flag;
			}
			return false;
		}

		// Token: 0x060063F5 RID: 25589 RVA: 0x0016D1E0 File Offset: 0x0016C1E0
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetDynamicAttributes += this.OnGetDynamicAttributes;
				propDesc[i].QueryGetBaseAttributes += this.OnGetBaseAttributes;
				propDesc[i].QueryGetDisplayName += this.OnGetDisplayName;
				propDesc[i].QueryGetIsReadOnly += this.OnGetIsReadOnly;
				propDesc[i].QueryShouldSerializeValue += this.OnShouldSerializeValue;
				propDesc[i].QueryCanResetValue += this.OnCanResetPropertyValue;
				propDesc[i].QueryResetValue += this.OnResetPropertyValue;
				propDesc[i].QueryGetTypeConverterAndTypeEditor += this.OnGetTypeConverterAndTypeEditor;
			}
		}

		// Token: 0x060063F6 RID: 25590 RVA: 0x0016D2A8 File Offset: 0x0016C2A8
		private void OnGetBaseAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = sender.TargetObject as NativeMethods.IVsPerPropertyBrowsing;
			if (vsPerPropertyBrowsing == null)
			{
				return;
			}
			string[] array = new string[1];
			if (vsPerPropertyBrowsing.GetLocalizedPropertyInfo(sender.DISPID, CultureInfo.CurrentCulture.LCID, null, array) == 0 && array[0] != null)
			{
				attrEvent.Add(new DescriptionAttribute(array[0]));
			}
		}

		// Token: 0x060063F7 RID: 25591 RVA: 0x0016D2FC File Offset: 0x0016C2FC
		private void OnGetDynamicAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				if (sender.CanShow)
				{
					bool flag = sender.Attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.No);
					if (vsPerPropertyBrowsing.HideProperty(sender.DISPID, ref flag) == 0)
					{
						attrEvent.Add(flag ? BrowsableAttribute.No : BrowsableAttribute.Yes);
					}
				}
				if (typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(sender.PropertyType) && sender.CanShow)
				{
					bool flag2 = false;
					if (vsPerPropertyBrowsing.DisplayChildProperties(sender.DISPID, ref flag2) == 0 && flag2)
					{
						attrEvent.Add(BrowsableAttribute.Yes);
					}
				}
			}
		}

		// Token: 0x060063F8 RID: 25592 RVA: 0x0016D3B8 File Offset: 0x0016C3B8
		private void OnCanResetPropertyValue(Com2PropertyDescriptor sender, GetBoolValueEvent boolEvent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				bool value = boolEvent.Value;
				int hr = vsPerPropertyBrowsing.CanResetPropertyValue(sender.DISPID, ref value);
				if (NativeMethods.Succeeded(hr))
				{
					boolEvent.Value = value;
				}
			}
		}

		// Token: 0x060063F9 RID: 25593 RVA: 0x0016D404 File Offset: 0x0016C404
		private void OnGetDisplayName(Com2PropertyDescriptor sender, GetNameItemEvent nameItem)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				string[] array = new string[1];
				if (vsPerPropertyBrowsing.GetLocalizedPropertyInfo(sender.DISPID, CultureInfo.CurrentCulture.LCID, array, null) == 0 && array[0] != null)
				{
					nameItem.Name = array[0];
				}
			}
		}

		// Token: 0x060063FA RID: 25594 RVA: 0x0016D45C File Offset: 0x0016C45C
		private void OnGetIsReadOnly(Com2PropertyDescriptor sender, GetBoolValueEvent gbvevent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				bool value = false;
				if (vsPerPropertyBrowsing.IsPropertyReadOnly(sender.DISPID, ref value) == 0)
				{
					gbvevent.Value = value;
				}
			}
		}

		// Token: 0x060063FB RID: 25595 RVA: 0x0016D4A0 File Offset: 0x0016C4A0
		private void OnGetTypeConverterAndTypeEditor(Com2PropertyDescriptor sender, GetTypeConverterAndTypeEditorEvent gveevent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing && sender.CanShow && typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(sender.PropertyType))
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				bool flag = false;
				int num = vsPerPropertyBrowsing.DisplayChildProperties(sender.DISPID, ref flag);
				if (gveevent.TypeConverter is Com2IDispatchConverter)
				{
					gveevent.TypeConverter = new Com2IDispatchConverter(sender, num == 0 && flag);
					return;
				}
				gveevent.TypeConverter = new Com2IDispatchConverter(sender, num == 0 && flag, gveevent.TypeConverter);
			}
		}

		// Token: 0x060063FC RID: 25596 RVA: 0x0016D530 File Offset: 0x0016C530
		private void OnResetPropertyValue(Com2PropertyDescriptor sender, EventArgs e)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				int dispid = sender.DISPID;
				bool flag = false;
				int hr = vsPerPropertyBrowsing.CanResetPropertyValue(dispid, ref flag);
				if (NativeMethods.Succeeded(hr))
				{
					vsPerPropertyBrowsing.ResetPropertyValue(dispid);
				}
			}
		}

		// Token: 0x060063FD RID: 25597 RVA: 0x0016D57C File Offset: 0x0016C57C
		private void OnShouldSerializeValue(Com2PropertyDescriptor sender, GetBoolValueEvent gbvevent)
		{
			if (sender.TargetObject is NativeMethods.IVsPerPropertyBrowsing)
			{
				NativeMethods.IVsPerPropertyBrowsing vsPerPropertyBrowsing = (NativeMethods.IVsPerPropertyBrowsing)sender.TargetObject;
				bool flag = true;
				if (vsPerPropertyBrowsing.HasDefaultValue(sender.DISPID, ref flag) == 0 && !flag)
				{
					gbvevent.Value = true;
				}
			}
		}
	}
}
