using System;

namespace System.Net.Mail
{
	// Token: 0x0200069D RID: 1693
	[Flags]
	public enum DeliveryNotificationOptions
	{
		// Token: 0x0400302B RID: 12331
		None = 0,
		// Token: 0x0400302C RID: 12332
		OnSuccess = 1,
		// Token: 0x0400302D RID: 12333
		OnFailure = 2,
		// Token: 0x0400302E RID: 12334
		Delay = 4,
		// Token: 0x0400302F RID: 12335
		Never = 134217728
	}
}
