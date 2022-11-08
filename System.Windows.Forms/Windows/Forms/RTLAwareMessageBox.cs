using System;

namespace System.Windows.Forms
{
	// Token: 0x020005F3 RID: 1523
	internal sealed class RTLAwareMessageBox
	{
		// Token: 0x06004F92 RID: 20370 RVA: 0x001263D1 File Offset: 0x001253D1
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			if (RTLAwareMessageBox.IsRTLResources)
			{
				options |= (MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
			}
			return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x06004F93 RID: 20371 RVA: 0x001263F3 File Offset: 0x001253F3
		public static bool IsRTLResources
		{
			get
			{
				return SR.GetString("RTL") != "RTL_False";
			}
		}
	}
}
