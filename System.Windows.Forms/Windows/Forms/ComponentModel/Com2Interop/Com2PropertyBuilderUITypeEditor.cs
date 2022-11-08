using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000762 RID: 1890
	internal class Com2PropertyBuilderUITypeEditor : Com2ExtendedUITypeEditor
	{
		// Token: 0x06006418 RID: 25624 RVA: 0x0016DD98 File Offset: 0x0016CD98
		public Com2PropertyBuilderUITypeEditor(Com2PropertyDescriptor pd, string guidString, int type, UITypeEditor baseEditor) : base(baseEditor)
		{
			this.propDesc = pd;
			this.guidString = guidString;
			this.bldrType = type;
		}

		// Token: 0x06006419 RID: 25625 RVA: 0x0016DDB8 File Offset: 0x0016CDB8
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IntPtr handle = UnsafeNativeMethods.GetFocus();
			IUIService iuiservice = (IUIService)provider.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				IWin32Window dialogOwnerWindow = iuiservice.GetDialogOwnerWindow();
				if (dialogOwnerWindow != null)
				{
					handle = dialogOwnerWindow.Handle;
				}
			}
			bool flag = false;
			object result = value;
			try
			{
				object obj = this.propDesc.TargetObject;
				if (obj is ICustomTypeDescriptor)
				{
					obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.propDesc);
				}
				NativeMethods.IProvidePropertyBuilder providePropertyBuilder = (NativeMethods.IProvidePropertyBuilder)obj;
				if (NativeMethods.Failed(providePropertyBuilder.ExecuteBuilder(this.propDesc.DISPID, this.guidString, null, new HandleRef(null, handle), ref result, ref flag)))
				{
					flag = false;
				}
			}
			catch (ExternalException)
			{
			}
			if (flag && (this.bldrType & 4) == 0)
			{
				return result;
			}
			return value;
		}

		// Token: 0x0600641A RID: 25626 RVA: 0x0016DE80 File Offset: 0x0016CE80
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x04003B9F RID: 15263
		private Com2PropertyDescriptor propDesc;

		// Token: 0x04003BA0 RID: 15264
		private string guidString;

		// Token: 0x04003BA1 RID: 15265
		private int bldrType;
	}
}
