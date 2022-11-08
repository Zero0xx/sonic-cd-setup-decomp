using System;
using System.Runtime.InteropServices;

namespace System.Net.Mail
{
	// Token: 0x0200068E RID: 1678
	[StructLayout(LayoutKind.Sequential)]
	internal class _METADATA_HANDLE_INFO
	{
		// Token: 0x060033D2 RID: 13266 RVA: 0x000DB1D9 File Offset: 0x000DA1D9
		private _METADATA_HANDLE_INFO()
		{
			this.dwMDPermissions = 0;
			this.dwMDSystemChangeNumber = 0;
		}

		// Token: 0x04002FDD RID: 12253
		internal int dwMDPermissions;

		// Token: 0x04002FDE RID: 12254
		internal int dwMDSystemChangeNumber;
	}
}
