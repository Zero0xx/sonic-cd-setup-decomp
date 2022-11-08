using System;
using System.Drawing;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000753 RID: 1875
	internal class Com2ColorConverter : Com2DataTypeToManagedDataTypeConverter
	{
		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x060063B0 RID: 25520 RVA: 0x0016BE52 File Offset: 0x0016AE52
		public override Type ManagedType
		{
			get
			{
				return typeof(Color);
			}
		}

		// Token: 0x060063B1 RID: 25521 RVA: 0x0016BE60 File Offset: 0x0016AE60
		public override object ConvertNativeToManaged(object nativeValue, Com2PropertyDescriptor pd)
		{
			int oleColor = 0;
			if (nativeValue is uint)
			{
				oleColor = (int)((uint)nativeValue);
			}
			else if (nativeValue is int)
			{
				oleColor = (int)nativeValue;
			}
			return ColorTranslator.FromOle(oleColor);
		}

		// Token: 0x060063B2 RID: 25522 RVA: 0x0016BE9A File Offset: 0x0016AE9A
		public override object ConvertManagedToNative(object managedValue, Com2PropertyDescriptor pd, ref bool cancelSet)
		{
			cancelSet = false;
			if (managedValue == null)
			{
				managedValue = Color.Black;
			}
			if (managedValue is Color)
			{
				return ColorTranslator.ToOle((Color)managedValue);
			}
			return 0;
		}
	}
}
