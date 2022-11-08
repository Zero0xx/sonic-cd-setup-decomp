using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008EC RID: 2284
	public enum ResourceType
	{
		// Token: 0x04002AD4 RID: 10964
		Unknown,
		// Token: 0x04002AD5 RID: 10965
		FileObject,
		// Token: 0x04002AD6 RID: 10966
		Service,
		// Token: 0x04002AD7 RID: 10967
		Printer,
		// Token: 0x04002AD8 RID: 10968
		RegistryKey,
		// Token: 0x04002AD9 RID: 10969
		LMShare,
		// Token: 0x04002ADA RID: 10970
		KernelObject,
		// Token: 0x04002ADB RID: 10971
		WindowObject,
		// Token: 0x04002ADC RID: 10972
		DSObject,
		// Token: 0x04002ADD RID: 10973
		DSObjectAll,
		// Token: 0x04002ADE RID: 10974
		ProviderDefined,
		// Token: 0x04002ADF RID: 10975
		WmiGuidObject,
		// Token: 0x04002AE0 RID: 10976
		RegistryWow6432Key
	}
}
