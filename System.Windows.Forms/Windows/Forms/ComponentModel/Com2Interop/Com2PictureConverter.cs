using System;
using System.Drawing;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000760 RID: 1888
	internal class Com2PictureConverter : Com2DataTypeToManagedDataTypeConverter
	{
		// Token: 0x060063FF RID: 25599 RVA: 0x0016D5C8 File Offset: 0x0016C5C8
		public Com2PictureConverter(Com2PropertyDescriptor pd)
		{
			if (pd.DISPID == -522 || pd.Name.IndexOf("Icon") != -1)
			{
				this.pictureType = typeof(Icon);
			}
		}

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x06006400 RID: 25600 RVA: 0x0016D626 File Offset: 0x0016C626
		public override Type ManagedType
		{
			get
			{
				return this.pictureType;
			}
		}

		// Token: 0x06006401 RID: 25601 RVA: 0x0016D630 File Offset: 0x0016C630
		public override object ConvertNativeToManaged(object nativeValue, Com2PropertyDescriptor pd)
		{
			if (nativeValue == null)
			{
				return null;
			}
			UnsafeNativeMethods.IPicture picture = (UnsafeNativeMethods.IPicture)nativeValue;
			IntPtr handle = picture.GetHandle();
			if (this.lastManaged != null && handle == this.lastNativeHandle)
			{
				return this.lastManaged;
			}
			this.lastNativeHandle = handle;
			if (handle != IntPtr.Zero)
			{
				switch (picture.GetPictureType())
				{
				case 1:
					this.pictureType = typeof(Bitmap);
					this.lastManaged = Image.FromHbitmap(handle);
					break;
				case 3:
					this.pictureType = typeof(Icon);
					this.lastManaged = Icon.FromHandle(handle);
					break;
				}
				this.pictureRef = new WeakReference(picture);
			}
			else
			{
				this.lastManaged = null;
				this.pictureRef = null;
			}
			return this.lastManaged;
		}

		// Token: 0x06006402 RID: 25602 RVA: 0x0016D6FC File Offset: 0x0016C6FC
		public override object ConvertManagedToNative(object managedValue, Com2PropertyDescriptor pd, ref bool cancelSet)
		{
			cancelSet = false;
			if (this.lastManaged != null && this.lastManaged.Equals(managedValue) && this.pictureRef != null && this.pictureRef.IsAlive)
			{
				return this.pictureRef.Target;
			}
			this.lastManaged = managedValue;
			if (managedValue != null)
			{
				Guid guid = typeof(UnsafeNativeMethods.IPicture).GUID;
				NativeMethods.PICTDESC pictdesc = null;
				bool fOwn = false;
				if (this.lastManaged is Icon)
				{
					pictdesc = NativeMethods.PICTDESC.CreateIconPICTDESC(((Icon)this.lastManaged).Handle);
				}
				else if (this.lastManaged is Bitmap)
				{
					pictdesc = NativeMethods.PICTDESC.CreateBitmapPICTDESC(((Bitmap)this.lastManaged).GetHbitmap(), this.lastPalette);
					fOwn = true;
				}
				UnsafeNativeMethods.IPicture picture = UnsafeNativeMethods.OleCreatePictureIndirect(pictdesc, ref guid, fOwn);
				this.lastNativeHandle = picture.GetHandle();
				this.pictureRef = new WeakReference(picture);
				return picture;
			}
			this.lastManaged = null;
			this.lastNativeHandle = (this.lastPalette = IntPtr.Zero);
			this.pictureRef = null;
			return null;
		}

		// Token: 0x04003B8D RID: 15245
		private object lastManaged;

		// Token: 0x04003B8E RID: 15246
		private IntPtr lastNativeHandle;

		// Token: 0x04003B8F RID: 15247
		private WeakReference pictureRef;

		// Token: 0x04003B90 RID: 15248
		private IntPtr lastPalette = IntPtr.Zero;

		// Token: 0x04003B91 RID: 15249
		private Type pictureType = typeof(Bitmap);
	}
}
