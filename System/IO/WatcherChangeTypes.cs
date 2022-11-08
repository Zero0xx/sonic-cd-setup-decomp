using System;

namespace System.IO
{
	// Token: 0x02000731 RID: 1841
	[Flags]
	public enum WatcherChangeTypes
	{
		// Token: 0x0400321F RID: 12831
		Created = 1,
		// Token: 0x04003220 RID: 12832
		Deleted = 2,
		// Token: 0x04003221 RID: 12833
		Changed = 4,
		// Token: 0x04003222 RID: 12834
		Renamed = 8,
		// Token: 0x04003223 RID: 12835
		All = 15
	}
}
