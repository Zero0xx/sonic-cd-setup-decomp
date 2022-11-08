using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000528 RID: 1320
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TypeLibExporterFlags
	{
		// Token: 0x04001A0E RID: 6670
		None = 0,
		// Token: 0x04001A0F RID: 6671
		OnlyReferenceRegistered = 1,
		// Token: 0x04001A10 RID: 6672
		CallerResolvedReferences = 2,
		// Token: 0x04001A11 RID: 6673
		OldNames = 4,
		// Token: 0x04001A12 RID: 6674
		ExportAs32Bit = 16,
		// Token: 0x04001A13 RID: 6675
		ExportAs64Bit = 32
	}
}
