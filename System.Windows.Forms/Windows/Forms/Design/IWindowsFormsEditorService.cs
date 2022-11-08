using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000782 RID: 1922
	public interface IWindowsFormsEditorService
	{
		// Token: 0x0600650A RID: 25866
		void CloseDropDown();

		// Token: 0x0600650B RID: 25867
		void DropDownControl(Control control);

		// Token: 0x0600650C RID: 25868
		DialogResult ShowDialog(Form dialog);
	}
}
