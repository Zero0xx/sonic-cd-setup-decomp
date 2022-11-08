using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200076F RID: 1903
	internal class Com2PropertyPageUITypeEditor : Com2ExtendedUITypeEditor, ICom2PropertyPageDisplayService
	{
		// Token: 0x06006440 RID: 25664 RVA: 0x0016DF4C File Offset: 0x0016CF4C
		public Com2PropertyPageUITypeEditor(Com2PropertyDescriptor pd, Guid guid, UITypeEditor baseEditor) : base(baseEditor)
		{
			this.propDesc = pd;
			this.guid = guid;
		}

		// Token: 0x06006441 RID: 25665 RVA: 0x0016DF64 File Offset: 0x0016CF64
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			try
			{
				ICom2PropertyPageDisplayService com2PropertyPageDisplayService = (ICom2PropertyPageDisplayService)provider.GetService(typeof(ICom2PropertyPageDisplayService));
				if (com2PropertyPageDisplayService == null)
				{
					com2PropertyPageDisplayService = this;
				}
				object obj = context.Instance;
				if (!obj.GetType().IsArray)
				{
					obj = this.propDesc.TargetObject;
					if (obj is ICustomTypeDescriptor)
					{
						obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.propDesc);
					}
				}
				com2PropertyPageDisplayService.ShowPropertyPage(this.propDesc.Name, obj, this.propDesc.DISPID, this.guid, focus);
			}
			catch (Exception ex)
			{
				if (provider != null)
				{
					IUIService iuiservice = (IUIService)provider.GetService(typeof(IUIService));
					if (iuiservice != null)
					{
						iuiservice.ShowError(ex, SR.GetString("ErrorTypeConverterFailed"));
					}
				}
			}
			return value;
		}

		// Token: 0x06006442 RID: 25666 RVA: 0x0016E038 File Offset: 0x0016D038
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x06006443 RID: 25667 RVA: 0x0016E03C File Offset: 0x0016D03C
		public unsafe void ShowPropertyPage(string title, object component, int dispid, Guid pageGuid, IntPtr parentHandle)
		{
			Guid[] arr = new Guid[]
			{
				pageGuid
			};
			IntPtr handle = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
			object[] array = component.GetType().IsArray ? ((object[])component) : new object[]
			{
				component
			};
			int num = array.Length;
			IntPtr[] array2 = new IntPtr[num];
			try
			{
				for (int i = 0; i < num; i++)
				{
					array2[i] = Marshal.GetIUnknownForObject(array[i]);
				}
				try
				{
					fixed (IntPtr* ptr = array2)
					{
						SafeNativeMethods.OleCreatePropertyFrame(new HandleRef(null, parentHandle), 0, 0, title, num, new HandleRef(null, (IntPtr)ptr), 1, new HandleRef(null, handle), SafeNativeMethods.GetThreadLCID(), 0, IntPtr.Zero);
					}
				}
				finally
				{
					IntPtr* ptr = null;
				}
			}
			finally
			{
				for (int j = 0; j < num; j++)
				{
					if (array2[j] != IntPtr.Zero)
					{
						Marshal.Release(array2[j]);
					}
				}
			}
		}

		// Token: 0x04003BB1 RID: 15281
		private Com2PropertyDescriptor propDesc;

		// Token: 0x04003BB2 RID: 15282
		private Guid guid;
	}
}
