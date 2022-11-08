using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200061D RID: 1565
	public enum OperationalStatus
	{
		// Token: 0x04002DDF RID: 11743
		Up = 1,
		// Token: 0x04002DE0 RID: 11744
		Down,
		// Token: 0x04002DE1 RID: 11745
		Testing,
		// Token: 0x04002DE2 RID: 11746
		Unknown,
		// Token: 0x04002DE3 RID: 11747
		Dormant,
		// Token: 0x04002DE4 RID: 11748
		NotPresent,
		// Token: 0x04002DE5 RID: 11749
		LowerLayerDown
	}
}
