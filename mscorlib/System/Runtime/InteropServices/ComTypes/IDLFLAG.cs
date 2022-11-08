using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000585 RID: 1413
	[Flags]
	[Serializable]
	public enum IDLFLAG : short
	{
		// Token: 0x04001B6C RID: 7020
		IDLFLAG_NONE = 0,
		// Token: 0x04001B6D RID: 7021
		IDLFLAG_FIN = 1,
		// Token: 0x04001B6E RID: 7022
		IDLFLAG_FOUT = 2,
		// Token: 0x04001B6F RID: 7023
		IDLFLAG_FLCID = 4,
		// Token: 0x04001B70 RID: 7024
		IDLFLAG_FRETVAL = 8
	}
}
