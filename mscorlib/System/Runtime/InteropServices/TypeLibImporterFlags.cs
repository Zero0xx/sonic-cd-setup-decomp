using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000527 RID: 1319
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TypeLibImporterFlags
	{
		// Token: 0x04001A01 RID: 6657
		None = 0,
		// Token: 0x04001A02 RID: 6658
		PrimaryInteropAssembly = 1,
		// Token: 0x04001A03 RID: 6659
		UnsafeInterfaces = 2,
		// Token: 0x04001A04 RID: 6660
		SafeArrayAsSystemArray = 4,
		// Token: 0x04001A05 RID: 6661
		TransformDispRetVals = 8,
		// Token: 0x04001A06 RID: 6662
		PreventClassMembers = 16,
		// Token: 0x04001A07 RID: 6663
		SerializableValueClasses = 32,
		// Token: 0x04001A08 RID: 6664
		ImportAsX86 = 256,
		// Token: 0x04001A09 RID: 6665
		ImportAsX64 = 512,
		// Token: 0x04001A0A RID: 6666
		ImportAsItanium = 1024,
		// Token: 0x04001A0B RID: 6667
		ImportAsAgnostic = 2048,
		// Token: 0x04001A0C RID: 6668
		ReflectionOnlyLoading = 4096
	}
}
