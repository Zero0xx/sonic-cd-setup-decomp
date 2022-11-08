using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000339 RID: 825
	[Flags]
	public enum X509KeyUsageFlags
	{
		// Token: 0x04001B06 RID: 6918
		None = 0,
		// Token: 0x04001B07 RID: 6919
		EncipherOnly = 1,
		// Token: 0x04001B08 RID: 6920
		CrlSign = 2,
		// Token: 0x04001B09 RID: 6921
		KeyCertSign = 4,
		// Token: 0x04001B0A RID: 6922
		KeyAgreement = 8,
		// Token: 0x04001B0B RID: 6923
		DataEncipherment = 16,
		// Token: 0x04001B0C RID: 6924
		KeyEncipherment = 32,
		// Token: 0x04001B0D RID: 6925
		NonRepudiation = 64,
		// Token: 0x04001B0E RID: 6926
		DigitalSignature = 128,
		// Token: 0x04001B0F RID: 6927
		DecipherOnly = 32768
	}
}
