using System;

namespace System.Security.Principal
{
	// Token: 0x020004CE RID: 1230
	[Serializable]
	internal enum KerbLogonSubmitType
	{
		// Token: 0x040018AB RID: 6315
		KerbInteractiveLogon = 2,
		// Token: 0x040018AC RID: 6316
		KerbSmartCardLogon = 6,
		// Token: 0x040018AD RID: 6317
		KerbWorkstationUnlockLogon,
		// Token: 0x040018AE RID: 6318
		KerbSmartCardUnlockLogon,
		// Token: 0x040018AF RID: 6319
		KerbProxyLogon,
		// Token: 0x040018B0 RID: 6320
		KerbTicketLogon,
		// Token: 0x040018B1 RID: 6321
		KerbTicketUnlockLogon,
		// Token: 0x040018B2 RID: 6322
		KerbS4ULogon
	}
}
