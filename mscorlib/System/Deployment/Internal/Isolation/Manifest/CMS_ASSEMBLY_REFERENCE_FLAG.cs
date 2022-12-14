using System;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000190 RID: 400
	internal enum CMS_ASSEMBLY_REFERENCE_FLAG
	{
		// Token: 0x04000706 RID: 1798
		CMS_ASSEMBLY_REFERENCE_FLAG_OPTIONAL = 1,
		// Token: 0x04000707 RID: 1799
		CMS_ASSEMBLY_REFERENCE_FLAG_VISIBLE,
		// Token: 0x04000708 RID: 1800
		CMS_ASSEMBLY_REFERENCE_FLAG_FOLLOW = 4,
		// Token: 0x04000709 RID: 1801
		CMS_ASSEMBLY_REFERENCE_FLAG_IS_PLATFORM = 8,
		// Token: 0x0400070A RID: 1802
		CMS_ASSEMBLY_REFERENCE_FLAG_CULTURE_WILDCARDED = 16,
		// Token: 0x0400070B RID: 1803
		CMS_ASSEMBLY_REFERENCE_FLAG_PROCESSOR_ARCHITECTURE_WILDCARDED = 32,
		// Token: 0x0400070C RID: 1804
		CMS_ASSEMBLY_REFERENCE_FLAG_PREREQUISITE = 128
	}
}
