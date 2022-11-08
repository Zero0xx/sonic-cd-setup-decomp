using System;

namespace System.Windows.Forms
{
	// Token: 0x02000605 RID: 1541
	public enum SecurityIDType
	{
		// Token: 0x040034F9 RID: 13561
		User = 1,
		// Token: 0x040034FA RID: 13562
		Group,
		// Token: 0x040034FB RID: 13563
		Domain,
		// Token: 0x040034FC RID: 13564
		Alias,
		// Token: 0x040034FD RID: 13565
		WellKnownGroup,
		// Token: 0x040034FE RID: 13566
		DeletedAccount,
		// Token: 0x040034FF RID: 13567
		Invalid,
		// Token: 0x04003500 RID: 13568
		Unknown,
		// Token: 0x04003501 RID: 13569
		Computer
	}
}
