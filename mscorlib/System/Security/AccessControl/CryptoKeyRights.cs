using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008DD RID: 2269
	[Flags]
	public enum CryptoKeyRights
	{
		// Token: 0x04002A9B RID: 10907
		ReadData = 1,
		// Token: 0x04002A9C RID: 10908
		WriteData = 2,
		// Token: 0x04002A9D RID: 10909
		ReadExtendedAttributes = 8,
		// Token: 0x04002A9E RID: 10910
		WriteExtendedAttributes = 16,
		// Token: 0x04002A9F RID: 10911
		ReadAttributes = 128,
		// Token: 0x04002AA0 RID: 10912
		WriteAttributes = 256,
		// Token: 0x04002AA1 RID: 10913
		Delete = 65536,
		// Token: 0x04002AA2 RID: 10914
		ReadPermissions = 131072,
		// Token: 0x04002AA3 RID: 10915
		ChangePermissions = 262144,
		// Token: 0x04002AA4 RID: 10916
		TakeOwnership = 524288,
		// Token: 0x04002AA5 RID: 10917
		Synchronize = 1048576,
		// Token: 0x04002AA6 RID: 10918
		FullControl = 2032027,
		// Token: 0x04002AA7 RID: 10919
		GenericAll = 268435456,
		// Token: 0x04002AA8 RID: 10920
		GenericExecute = 536870912,
		// Token: 0x04002AA9 RID: 10921
		GenericWrite = 1073741824,
		// Token: 0x04002AAA RID: 10922
		GenericRead = -2147483648
	}
}
