using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200075A RID: 1882
	[SuppressUnmanagedCodeSecurity]
	internal class Com2IManagedPerPropertyBrowsingHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x060063D5 RID: 25557 RVA: 0x0016C606 File Offset: 0x0016B606
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.IManagedPerPropertyBrowsing);
			}
		}

		// Token: 0x060063D6 RID: 25558 RVA: 0x0016C614 File Offset: 0x0016B614
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetDynamicAttributes += this.OnGetAttributes;
			}
		}

		// Token: 0x060063D7 RID: 25559 RVA: 0x0016C648 File Offset: 0x0016B648
		private void OnGetAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			object targetObject = sender.TargetObject;
			if (targetObject is NativeMethods.IManagedPerPropertyBrowsing)
			{
				Attribute[] componentAttributes = Com2IManagedPerPropertyBrowsingHandler.GetComponentAttributes((NativeMethods.IManagedPerPropertyBrowsing)targetObject, sender.DISPID);
				if (componentAttributes != null)
				{
					for (int i = 0; i < componentAttributes.Length; i++)
					{
						attrEvent.Add(componentAttributes[i]);
					}
				}
			}
		}

		// Token: 0x060063D8 RID: 25560 RVA: 0x0016C690 File Offset: 0x0016B690
		internal static Attribute[] GetComponentAttributes(NativeMethods.IManagedPerPropertyBrowsing target, int dispid)
		{
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			if (target.GetPropertyAttributes(dispid, ref num, ref zero, ref zero2) != 0 || num == 0)
			{
				return new Attribute[0];
			}
			ArrayList arrayList = new ArrayList();
			string[] stringsFromPtr = Com2IManagedPerPropertyBrowsingHandler.GetStringsFromPtr(zero, num);
			object[] variantsFromPtr = Com2IManagedPerPropertyBrowsingHandler.GetVariantsFromPtr(zero2, num);
			if (stringsFromPtr.Length != variantsFromPtr.Length)
			{
				return new Attribute[0];
			}
			int num2 = stringsFromPtr.Length;
			int i = 0;
			while (i < stringsFromPtr.Length)
			{
				string text = stringsFromPtr[i];
				Type type = Type.GetType(text);
				Assembly assembly = null;
				if (type != null)
				{
					assembly = type.Assembly;
				}
				if (type != null)
				{
					goto IL_168;
				}
				string str = "";
				int num3 = text.LastIndexOf(',');
				if (num3 != -1)
				{
					str = text.Substring(num3);
					text = text.Substring(0, num3);
				}
				int num4 = text.LastIndexOf('.');
				if (num4 != -1)
				{
					string name = text.Substring(num4 + 1);
					if (assembly == null)
					{
						type = Type.GetType(text.Substring(0, num4) + str);
					}
					else
					{
						type = assembly.GetType(text.Substring(0, num4) + str);
					}
					if (type != null && typeof(Attribute).IsAssignableFrom(type))
					{
						if (type == null)
						{
							goto IL_168;
						}
						FieldInfo field = type.GetField(name);
						if (field == null || !field.IsStatic)
						{
							goto IL_168;
						}
						object value = field.GetValue(null);
						if (!(value is Attribute))
						{
							goto IL_168;
						}
						arrayList.Add(value);
					}
				}
				IL_22F:
				i++;
				continue;
				IL_168:
				if (!typeof(Attribute).IsAssignableFrom(type))
				{
					goto IL_22F;
				}
				if (!Convert.IsDBNull(variantsFromPtr[i]) && variantsFromPtr[i] != null)
				{
					ConstructorInfo[] constructors = type.GetConstructors();
					for (int j = 0; j < constructors.Length; j++)
					{
						ParameterInfo[] parameters = constructors[j].GetParameters();
						if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(variantsFromPtr[i].GetType()))
						{
							try
							{
								Attribute value2 = (Attribute)Activator.CreateInstance(type, new object[]
								{
									variantsFromPtr[i]
								});
								arrayList.Add(value2);
							}
							catch
							{
							}
						}
					}
					goto IL_22F;
				}
				try
				{
					Attribute value2 = (Attribute)Activator.CreateInstance(type);
					arrayList.Add(value2);
				}
				catch
				{
				}
				goto IL_22F;
			}
			Attribute[] array = new Attribute[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x0016C914 File Offset: 0x0016B914
		private static string[] GetStringsFromPtr(IntPtr ptr, int cStrings)
		{
			if (ptr != IntPtr.Zero)
			{
				string[] array = new string[cStrings];
				for (int i = 0; i < cStrings; i++)
				{
					try
					{
						IntPtr intPtr = Marshal.ReadIntPtr(ptr, i * 4);
						if (intPtr != IntPtr.Zero)
						{
							array[i] = Marshal.PtrToStringUni(intPtr);
							SafeNativeMethods.SysFreeString(new HandleRef(null, intPtr));
						}
						else
						{
							array[i] = "";
						}
					}
					catch (Exception)
					{
					}
				}
				try
				{
					Marshal.FreeCoTaskMem(ptr);
				}
				catch (Exception)
				{
				}
				return array;
			}
			return new string[0];
		}

		// Token: 0x060063DA RID: 25562 RVA: 0x0016C9AC File Offset: 0x0016B9AC
		private static object[] GetVariantsFromPtr(IntPtr ptr, int cVariants)
		{
			if (ptr != IntPtr.Zero)
			{
				object[] array = new object[cVariants];
				for (int i = 0; i < cVariants; i++)
				{
					try
					{
						IntPtr intPtr = (IntPtr)((long)ptr + (long)(i * 16));
						if (intPtr != IntPtr.Zero)
						{
							array[i] = Marshal.GetObjectForNativeVariant(intPtr);
							SafeNativeMethods.VariantClear(new HandleRef(null, intPtr));
						}
						else
						{
							array[i] = Convert.DBNull;
						}
					}
					catch (Exception)
					{
					}
				}
				try
				{
					Marshal.FreeCoTaskMem(ptr);
				}
				catch (Exception)
				{
				}
				return array;
			}
			return new object[cVariants];
		}
	}
}
