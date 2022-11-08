using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005B0 RID: 1456
	[ComVisible(true)]
	[Serializable]
	public enum DriveType
	{
		// Token: 0x04001C26 RID: 7206
		Unknown,
		// Token: 0x04001C27 RID: 7207
		NoRootDirectory,
		// Token: 0x04001C28 RID: 7208
		Removable,
		// Token: 0x04001C29 RID: 7209
		Fixed,
		// Token: 0x04001C2A RID: 7210
		Network,
		// Token: 0x04001C2B RID: 7211
		CDRom,
		// Token: 0x04001C2C RID: 7212
		Ram
	}
}
