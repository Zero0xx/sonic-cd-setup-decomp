using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200052F RID: 1327
	[Flags]
	public enum RegistrationClassContext
	{
		// Token: 0x04001A1D RID: 6685
		InProcessServer = 1,
		// Token: 0x04001A1E RID: 6686
		InProcessHandler = 2,
		// Token: 0x04001A1F RID: 6687
		LocalServer = 4,
		// Token: 0x04001A20 RID: 6688
		InProcessServer16 = 8,
		// Token: 0x04001A21 RID: 6689
		RemoteServer = 16,
		// Token: 0x04001A22 RID: 6690
		InProcessHandler16 = 32,
		// Token: 0x04001A23 RID: 6691
		Reserved1 = 64,
		// Token: 0x04001A24 RID: 6692
		Reserved2 = 128,
		// Token: 0x04001A25 RID: 6693
		Reserved3 = 256,
		// Token: 0x04001A26 RID: 6694
		Reserved4 = 512,
		// Token: 0x04001A27 RID: 6695
		NoCodeDownload = 1024,
		// Token: 0x04001A28 RID: 6696
		Reserved5 = 2048,
		// Token: 0x04001A29 RID: 6697
		NoCustomMarshal = 4096,
		// Token: 0x04001A2A RID: 6698
		EnableCodeDownload = 8192,
		// Token: 0x04001A2B RID: 6699
		NoFailureLog = 16384,
		// Token: 0x04001A2C RID: 6700
		DisableActivateAsActivator = 32768,
		// Token: 0x04001A2D RID: 6701
		EnableActivateAsActivator = 65536,
		// Token: 0x04001A2E RID: 6702
		FromDefaultContext = 131072
	}
}
