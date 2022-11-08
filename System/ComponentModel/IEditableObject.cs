using System;

namespace System.ComponentModel
{
	// Token: 0x020000EE RID: 238
	public interface IEditableObject
	{
		// Token: 0x060007ED RID: 2029
		void BeginEdit();

		// Token: 0x060007EE RID: 2030
		void EndEdit();

		// Token: 0x060007EF RID: 2031
		void CancelEdit();
	}
}
