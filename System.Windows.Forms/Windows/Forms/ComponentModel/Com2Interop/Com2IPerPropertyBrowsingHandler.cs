using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200075B RID: 1883
	[SuppressUnmanagedCodeSecurity]
	internal class Com2IPerPropertyBrowsingHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x060063DC RID: 25564 RVA: 0x0016CA54 File Offset: 0x0016BA54
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.IPerPropertyBrowsing);
			}
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x0016CA60 File Offset: 0x0016BA60
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetBaseAttributes += this.OnGetBaseAttributes;
				propDesc[i].QueryGetDisplayValue += this.OnGetDisplayValue;
				propDesc[i].QueryGetTypeConverterAndTypeEditor += this.OnGetTypeConverterAndTypeEditor;
			}
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x0016CABC File Offset: 0x0016BABC
		private Guid GetPropertyPageGuid(NativeMethods.IPerPropertyBrowsing target, int dispid)
		{
			Guid result;
			if (target.MapPropertyToPage(dispid, out result) == 0)
			{
				return result;
			}
			return Guid.Empty;
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x0016CAE0 File Offset: 0x0016BAE0
		internal static string GetDisplayString(NativeMethods.IPerPropertyBrowsing ppb, int dispid, ref bool success)
		{
			string[] array = new string[1];
			if (ppb.GetDisplayString(dispid, array) == 0)
			{
				success = (array[0] != null);
				return array[0];
			}
			success = false;
			return null;
		}

		// Token: 0x060063E0 RID: 25568 RVA: 0x0016CB14 File Offset: 0x0016BB14
		private void OnGetBaseAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = sender.TargetObject as NativeMethods.IPerPropertyBrowsing;
			if (perPropertyBrowsing != null)
			{
				bool flag = !Guid.Empty.Equals(this.GetPropertyPageGuid(perPropertyBrowsing, sender.DISPID));
				if (sender.CanShow && flag && typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(sender.PropertyType))
				{
					attrEvent.Add(BrowsableAttribute.Yes);
				}
			}
		}

		// Token: 0x060063E1 RID: 25569 RVA: 0x0016CB7C File Offset: 0x0016BB7C
		private void OnGetDisplayValue(Com2PropertyDescriptor sender, GetNameItemEvent gnievent)
		{
			try
			{
				if (sender.TargetObject is NativeMethods.IPerPropertyBrowsing)
				{
					if (!(sender.Converter is Com2IPerPropertyBrowsingHandler.Com2IPerPropertyEnumConverter) && !sender.ConvertingNativeType)
					{
						bool flag = true;
						string displayString = Com2IPerPropertyBrowsingHandler.GetDisplayString((NativeMethods.IPerPropertyBrowsing)sender.TargetObject, sender.DISPID, ref flag);
						if (flag)
						{
							gnievent.Name = displayString;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060063E2 RID: 25570 RVA: 0x0016CBE8 File Offset: 0x0016BBE8
		private void OnGetTypeConverterAndTypeEditor(Com2PropertyDescriptor sender, GetTypeConverterAndTypeEditorEvent gveevent)
		{
			if (sender.TargetObject is NativeMethods.IPerPropertyBrowsing)
			{
				NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = (NativeMethods.IPerPropertyBrowsing)sender.TargetObject;
				NativeMethods.CA_STRUCT ca_STRUCT = new NativeMethods.CA_STRUCT();
				NativeMethods.CA_STRUCT ca_STRUCT2 = new NativeMethods.CA_STRUCT();
				int num = 0;
				try
				{
					num = perPropertyBrowsing.GetPredefinedStrings(sender.DISPID, ca_STRUCT, ca_STRUCT2);
				}
				catch (ExternalException ex)
				{
					num = ex.ErrorCode;
				}
				if (gveevent.TypeConverter is Com2IPerPropertyBrowsingHandler.Com2IPerPropertyEnumConverter)
				{
					gveevent.TypeConverter = null;
				}
				bool flag = num == 0;
				if (flag)
				{
					OleStrCAMarshaler oleStrCAMarshaler = new OleStrCAMarshaler(ca_STRUCT);
					Int32CAMarshaler int32CAMarshaler = new Int32CAMarshaler(ca_STRUCT2);
					if (oleStrCAMarshaler.Count > 0 && int32CAMarshaler.Count > 0)
					{
						gveevent.TypeConverter = new Com2IPerPropertyBrowsingHandler.Com2IPerPropertyEnumConverter(new Com2IPerPropertyBrowsingHandler.Com2IPerPropertyBrowsingEnum(sender, this, oleStrCAMarshaler, int32CAMarshaler, true));
					}
				}
				if (!flag)
				{
					if (sender.ConvertingNativeType)
					{
						return;
					}
					Guid propertyPageGuid = this.GetPropertyPageGuid(perPropertyBrowsing, sender.DISPID);
					if (!Guid.Empty.Equals(propertyPageGuid))
					{
						gveevent.TypeEditor = new Com2PropertyPageUITypeEditor(sender, propertyPageGuid, (UITypeEditor)gveevent.TypeEditor);
					}
				}
			}
		}

		// Token: 0x0200075C RID: 1884
		private class Com2IPerPropertyEnumConverter : Com2EnumConverter
		{
			// Token: 0x060063E4 RID: 25572 RVA: 0x0016CCFC File Offset: 0x0016BCFC
			public Com2IPerPropertyEnumConverter(Com2IPerPropertyBrowsingHandler.Com2IPerPropertyBrowsingEnum items) : base(items)
			{
				this.itemsEnum = items;
			}

			// Token: 0x060063E5 RID: 25573 RVA: 0x0016CD0C File Offset: 0x0016BD0C
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
			{
				if (destType == typeof(string) && !this.itemsEnum.arraysFetched)
				{
					object value2 = this.itemsEnum.target.GetValue(this.itemsEnum.target.TargetObject);
					if (value2 == value || (value2 != null && value2.Equals(value)))
					{
						bool flag = false;
						string displayString = Com2IPerPropertyBrowsingHandler.GetDisplayString((NativeMethods.IPerPropertyBrowsing)this.itemsEnum.target.TargetObject, this.itemsEnum.target.DISPID, ref flag);
						if (flag)
						{
							return displayString;
						}
					}
				}
				return base.ConvertTo(context, culture, value, destType);
			}

			// Token: 0x04003B87 RID: 15239
			private Com2IPerPropertyBrowsingHandler.Com2IPerPropertyBrowsingEnum itemsEnum;
		}

		// Token: 0x0200075D RID: 1885
		private class Com2IPerPropertyBrowsingEnum : Com2Enum
		{
			// Token: 0x060063E6 RID: 25574 RVA: 0x0016CDA4 File Offset: 0x0016BDA4
			public Com2IPerPropertyBrowsingEnum(Com2PropertyDescriptor targetObject, Com2IPerPropertyBrowsingHandler handler, OleStrCAMarshaler names, Int32CAMarshaler values, bool allowUnknowns) : base(new string[0], new object[0], allowUnknowns)
			{
				this.target = targetObject;
				this.nameMarshaller = names;
				this.valueMarshaller = values;
				this.handler = handler;
				this.arraysFetched = false;
			}

			// Token: 0x1700150F RID: 5391
			// (get) Token: 0x060063E7 RID: 25575 RVA: 0x0016CDDE File Offset: 0x0016BDDE
			public override object[] Values
			{
				get
				{
					this.EnsureArrays();
					return base.Values;
				}
			}

			// Token: 0x17001510 RID: 5392
			// (get) Token: 0x060063E8 RID: 25576 RVA: 0x0016CDEC File Offset: 0x0016BDEC
			public override string[] Names
			{
				get
				{
					this.EnsureArrays();
					return base.Names;
				}
			}

			// Token: 0x060063E9 RID: 25577 RVA: 0x0016CDFC File Offset: 0x0016BDFC
			private void EnsureArrays()
			{
				if (this.arraysFetched)
				{
					return;
				}
				this.arraysFetched = true;
				try
				{
					object[] items = this.nameMarshaller.Items;
					object[] items2 = this.valueMarshaller.Items;
					NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = (NativeMethods.IPerPropertyBrowsing)this.target.TargetObject;
					int num = 0;
					if (items.Length > 0)
					{
						object[] array = new object[items2.Length];
						NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
						Type propertyType = this.target.PropertyType;
						for (int i = items.Length - 1; i >= 0; i--)
						{
							int dwCookie = (int)items2[i];
							if (items[i] != null && items[i] is string)
							{
								variant.vt = 0;
								int predefinedValue = perPropertyBrowsing.GetPredefinedValue(this.target.DISPID, dwCookie, variant);
								if (predefinedValue == 0 && variant.vt != 0)
								{
									array[i] = variant.ToObject();
									if (array[i].GetType() != propertyType)
									{
										if (propertyType.IsEnum)
										{
											array[i] = Enum.ToObject(propertyType, array[i]);
										}
										else
										{
											try
											{
												array[i] = Convert.ChangeType(array[i], propertyType, CultureInfo.InvariantCulture);
											}
											catch
											{
											}
										}
									}
								}
								variant.Clear();
								if (predefinedValue == 0)
								{
									num++;
								}
								else if (num > 0)
								{
									Array.Copy(items, i, items, i + 1, num);
									Array.Copy(array, i, array, i + 1, num);
								}
							}
						}
						string[] array2 = new string[num];
						Array.Copy(items, 0, array2, 0, num);
						base.PopulateArrays(array2, array);
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x060063EA RID: 25578 RVA: 0x0016CFB0 File Offset: 0x0016BFB0
			protected override void PopulateArrays(string[] names, object[] values)
			{
			}

			// Token: 0x060063EB RID: 25579 RVA: 0x0016CFB2 File Offset: 0x0016BFB2
			public override object FromString(string s)
			{
				this.EnsureArrays();
				return base.FromString(s);
			}

			// Token: 0x060063EC RID: 25580 RVA: 0x0016CFC4 File Offset: 0x0016BFC4
			public override string ToString(object v)
			{
				if (this.target.IsCurrentValue(v))
				{
					bool flag = false;
					string displayString = Com2IPerPropertyBrowsingHandler.GetDisplayString((NativeMethods.IPerPropertyBrowsing)this.target.TargetObject, this.target.DISPID, ref flag);
					if (flag)
					{
						return displayString;
					}
				}
				this.EnsureArrays();
				return base.ToString(v);
			}

			// Token: 0x04003B88 RID: 15240
			internal Com2PropertyDescriptor target;

			// Token: 0x04003B89 RID: 15241
			private Com2IPerPropertyBrowsingHandler handler;

			// Token: 0x04003B8A RID: 15242
			private OleStrCAMarshaler nameMarshaller;

			// Token: 0x04003B8B RID: 15243
			private Int32CAMarshaler valueMarshaller;

			// Token: 0x04003B8C RID: 15244
			internal bool arraysFetched;
		}
	}
}
