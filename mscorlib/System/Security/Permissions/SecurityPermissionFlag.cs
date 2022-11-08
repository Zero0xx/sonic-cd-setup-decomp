using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000650 RID: 1616
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum SecurityPermissionFlag
	{
		// Token: 0x04001E34 RID: 7732
		NoFlags = 0,
		// Token: 0x04001E35 RID: 7733
		Assertion = 1,
		// Token: 0x04001E36 RID: 7734
		UnmanagedCode = 2,
		// Token: 0x04001E37 RID: 7735
		SkipVerification = 4,
		// Token: 0x04001E38 RID: 7736
		Execution = 8,
		// Token: 0x04001E39 RID: 7737
		ControlThread = 16,
		// Token: 0x04001E3A RID: 7738
		ControlEvidence = 32,
		// Token: 0x04001E3B RID: 7739
		ControlPolicy = 64,
		// Token: 0x04001E3C RID: 7740
		SerializationFormatter = 128,
		// Token: 0x04001E3D RID: 7741
		ControlDomainPolicy = 256,
		// Token: 0x04001E3E RID: 7742
		ControlPrincipal = 512,
		// Token: 0x04001E3F RID: 7743
		ControlAppDomain = 1024,
		// Token: 0x04001E40 RID: 7744
		RemotingConfiguration = 2048,
		// Token: 0x04001E41 RID: 7745
		Infrastructure = 4096,
		// Token: 0x04001E42 RID: 7746
		BindingRedirects = 8192,
		// Token: 0x04001E43 RID: 7747
		AllFlags = 16383
	}
}
