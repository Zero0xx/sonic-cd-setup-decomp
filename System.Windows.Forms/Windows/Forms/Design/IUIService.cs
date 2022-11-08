using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000781 RID: 1921
	[Guid("06A9C74B-5E32-4561-BE73-381B37869F4F")]
	public interface IUIService
	{
		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x060064FD RID: 25853
		IDictionary Styles { get; }

		// Token: 0x060064FE RID: 25854
		bool CanShowComponentEditor(object component);

		// Token: 0x060064FF RID: 25855
		IWin32Window GetDialogOwnerWindow();

		// Token: 0x06006500 RID: 25856
		void SetUIDirty();

		// Token: 0x06006501 RID: 25857
		bool ShowComponentEditor(object component, IWin32Window parent);

		// Token: 0x06006502 RID: 25858
		DialogResult ShowDialog(Form form);

		// Token: 0x06006503 RID: 25859
		void ShowError(string message);

		// Token: 0x06006504 RID: 25860
		void ShowError(Exception ex);

		// Token: 0x06006505 RID: 25861
		void ShowError(Exception ex, string message);

		// Token: 0x06006506 RID: 25862
		void ShowMessage(string message);

		// Token: 0x06006507 RID: 25863
		void ShowMessage(string message, string caption);

		// Token: 0x06006508 RID: 25864
		DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons);

		// Token: 0x06006509 RID: 25865
		bool ShowToolWindow(Guid toolWindow);
	}
}
