using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200076E RID: 1902
	public interface ICom2PropertyPageDisplayService
	{
		// Token: 0x0600643F RID: 25663
		void ShowPropertyPage(string title, object component, int dispid, Guid pageGuid, IntPtr parentHandle);
	}
}
