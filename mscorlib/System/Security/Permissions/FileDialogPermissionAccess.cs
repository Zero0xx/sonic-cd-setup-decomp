using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200062A RID: 1578
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum FileDialogPermissionAccess
	{
		// Token: 0x04001D86 RID: 7558
		None = 0,
		// Token: 0x04001D87 RID: 7559
		Open = 1,
		// Token: 0x04001D88 RID: 7560
		Save = 2,
		// Token: 0x04001D89 RID: 7561
		OpenSave = 3
	}
}
