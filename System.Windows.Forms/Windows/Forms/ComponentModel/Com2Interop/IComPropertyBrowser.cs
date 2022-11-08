using System;
using System.ComponentModel.Design;
using Microsoft.Win32;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020005C2 RID: 1474
	public interface IComPropertyBrowser
	{
		// Token: 0x06004CE7 RID: 19687
		void DropDownDone();

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06004CE8 RID: 19688
		bool InPropertySet { get; }

		// Token: 0x140002C9 RID: 713
		// (add) Token: 0x06004CE9 RID: 19689
		// (remove) Token: 0x06004CEA RID: 19690
		event ComponentRenameEventHandler ComComponentNameChanged;

		// Token: 0x06004CEB RID: 19691
		bool EnsurePendingChangesCommitted();

		// Token: 0x06004CEC RID: 19692
		void HandleF4();

		// Token: 0x06004CED RID: 19693
		void LoadState(RegistryKey key);

		// Token: 0x06004CEE RID: 19694
		void SaveState(RegistryKey key);
	}
}
