using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200032E RID: 814
	[Flags]
	public enum X509ChainStatusFlags
	{
		// Token: 0x04001ABF RID: 6847
		NoError = 0,
		// Token: 0x04001AC0 RID: 6848
		NotTimeValid = 1,
		// Token: 0x04001AC1 RID: 6849
		NotTimeNested = 2,
		// Token: 0x04001AC2 RID: 6850
		Revoked = 4,
		// Token: 0x04001AC3 RID: 6851
		NotSignatureValid = 8,
		// Token: 0x04001AC4 RID: 6852
		NotValidForUsage = 16,
		// Token: 0x04001AC5 RID: 6853
		UntrustedRoot = 32,
		// Token: 0x04001AC6 RID: 6854
		RevocationStatusUnknown = 64,
		// Token: 0x04001AC7 RID: 6855
		Cyclic = 128,
		// Token: 0x04001AC8 RID: 6856
		InvalidExtension = 256,
		// Token: 0x04001AC9 RID: 6857
		InvalidPolicyConstraints = 512,
		// Token: 0x04001ACA RID: 6858
		InvalidBasicConstraints = 1024,
		// Token: 0x04001ACB RID: 6859
		InvalidNameConstraints = 2048,
		// Token: 0x04001ACC RID: 6860
		HasNotSupportedNameConstraint = 4096,
		// Token: 0x04001ACD RID: 6861
		HasNotDefinedNameConstraint = 8192,
		// Token: 0x04001ACE RID: 6862
		HasNotPermittedNameConstraint = 16384,
		// Token: 0x04001ACF RID: 6863
		HasExcludedNameConstraint = 32768,
		// Token: 0x04001AD0 RID: 6864
		PartialChain = 65536,
		// Token: 0x04001AD1 RID: 6865
		CtlNotTimeValid = 131072,
		// Token: 0x04001AD2 RID: 6866
		CtlNotSignatureValid = 262144,
		// Token: 0x04001AD3 RID: 6867
		CtlNotValidForUsage = 524288,
		// Token: 0x04001AD4 RID: 6868
		OfflineRevocation = 16777216,
		// Token: 0x04001AD5 RID: 6869
		NoIssuanceChainPolicy = 33554432
	}
}
