using System;
using System.Drawing;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000757 RID: 1879
	internal class Com2FontConverter : Com2DataTypeToManagedDataTypeConverter
	{
		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x060063C2 RID: 25538 RVA: 0x0016C238 File Offset: 0x0016B238
		public override bool AllowExpand
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x060063C3 RID: 25539 RVA: 0x0016C23B File Offset: 0x0016B23B
		public override Type ManagedType
		{
			get
			{
				return typeof(Font);
			}
		}

		// Token: 0x060063C4 RID: 25540 RVA: 0x0016C248 File Offset: 0x0016B248
		public override object ConvertNativeToManaged(object nativeValue, Com2PropertyDescriptor pd)
		{
			UnsafeNativeMethods.IFont font = nativeValue as UnsafeNativeMethods.IFont;
			if (font == null)
			{
				this.lastHandle = IntPtr.Zero;
				this.lastFont = Control.DefaultFont;
				return this.lastFont;
			}
			IntPtr hfont = font.GetHFont();
			if (hfont == this.lastHandle && this.lastFont != null)
			{
				return this.lastFont;
			}
			this.lastHandle = hfont;
			try
			{
				Font font2 = Font.FromHfont(this.lastHandle);
				try
				{
					this.lastFont = ControlPaint.FontInPoints(font2);
				}
				finally
				{
					font2.Dispose();
				}
			}
			catch (ArgumentException)
			{
				this.lastFont = Control.DefaultFont;
			}
			return this.lastFont;
		}

		// Token: 0x060063C5 RID: 25541 RVA: 0x0016C2FC File Offset: 0x0016B2FC
		public override object ConvertManagedToNative(object managedValue, Com2PropertyDescriptor pd, ref bool cancelSet)
		{
			if (managedValue == null)
			{
				managedValue = Control.DefaultFont;
			}
			cancelSet = true;
			if (this.lastFont != null && this.lastFont.Equals(managedValue))
			{
				return null;
			}
			this.lastFont = (Font)managedValue;
			UnsafeNativeMethods.IFont font = (UnsafeNativeMethods.IFont)pd.GetNativeValue(pd.TargetObject);
			if (font != null)
			{
				bool flag = ControlPaint.FontToIFont(this.lastFont, font);
				if (flag)
				{
					this.lastFont = null;
					this.ConvertNativeToManaged(font, pd);
				}
			}
			return null;
		}

		// Token: 0x04003B82 RID: 15234
		private IntPtr lastHandle = IntPtr.Zero;

		// Token: 0x04003B83 RID: 15235
		private Font lastFont;
	}
}
