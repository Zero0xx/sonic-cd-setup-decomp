using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000336 RID: 822
	[Flags]
	public enum X509VerificationFlags
	{
		// Token: 0x04001AEE RID: 6894
		NoFlag = 0,
		// Token: 0x04001AEF RID: 6895
		IgnoreNotTimeValid = 1,
		// Token: 0x04001AF0 RID: 6896
		IgnoreCtlNotTimeValid = 2,
		// Token: 0x04001AF1 RID: 6897
		IgnoreNotTimeNested = 4,
		// Token: 0x04001AF2 RID: 6898
		IgnoreInvalidBasicConstraints = 8,
		// Token: 0x04001AF3 RID: 6899
		AllowUnknownCertificateAuthority = 16,
		// Token: 0x04001AF4 RID: 6900
		IgnoreWrongUsage = 32,
		// Token: 0x04001AF5 RID: 6901
		IgnoreInvalidName = 64,
		// Token: 0x04001AF6 RID: 6902
		IgnoreInvalidPolicy = 128,
		// Token: 0x04001AF7 RID: 6903
		IgnoreEndRevocationUnknown = 256,
		// Token: 0x04001AF8 RID: 6904
		IgnoreCtlSignerRevocationUnknown = 512,
		// Token: 0x04001AF9 RID: 6905
		IgnoreCertificateAuthorityRevocationUnknown = 1024,
		// Token: 0x04001AFA RID: 6906
		IgnoreRootRevocationUnknown = 2048,
		// Token: 0x04001AFB RID: 6907
		AllFlags = 4095
	}
}
