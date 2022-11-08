using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200065D RID: 1629
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum KeyContainerPermissionFlags
	{
		// Token: 0x04001E6E RID: 7790
		NoFlags = 0,
		// Token: 0x04001E6F RID: 7791
		Create = 1,
		// Token: 0x04001E70 RID: 7792
		Open = 2,
		// Token: 0x04001E71 RID: 7793
		Delete = 4,
		// Token: 0x04001E72 RID: 7794
		Import = 16,
		// Token: 0x04001E73 RID: 7795
		Export = 32,
		// Token: 0x04001E74 RID: 7796
		Sign = 256,
		// Token: 0x04001E75 RID: 7797
		Decrypt = 512,
		// Token: 0x04001E76 RID: 7798
		ViewAcl = 4096,
		// Token: 0x04001E77 RID: 7799
		ChangeAcl = 8192,
		// Token: 0x04001E78 RID: 7800
		AllFlags = 13111
	}
}
