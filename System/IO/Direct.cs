using System;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x0200072A RID: 1834
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal static class Direct
	{
		// Token: 0x04003204 RID: 12804
		public const int FILE_ACTION_ADDED = 1;

		// Token: 0x04003205 RID: 12805
		public const int FILE_ACTION_REMOVED = 2;

		// Token: 0x04003206 RID: 12806
		public const int FILE_ACTION_MODIFIED = 3;

		// Token: 0x04003207 RID: 12807
		public const int FILE_ACTION_RENAMED_OLD_NAME = 4;

		// Token: 0x04003208 RID: 12808
		public const int FILE_ACTION_RENAMED_NEW_NAME = 5;

		// Token: 0x04003209 RID: 12809
		public const int FILE_NOTIFY_CHANGE_FILE_NAME = 1;

		// Token: 0x0400320A RID: 12810
		public const int FILE_NOTIFY_CHANGE_DIR_NAME = 2;

		// Token: 0x0400320B RID: 12811
		public const int FILE_NOTIFY_CHANGE_NAME = 3;

		// Token: 0x0400320C RID: 12812
		public const int FILE_NOTIFY_CHANGE_ATTRIBUTES = 4;

		// Token: 0x0400320D RID: 12813
		public const int FILE_NOTIFY_CHANGE_SIZE = 8;

		// Token: 0x0400320E RID: 12814
		public const int FILE_NOTIFY_CHANGE_LAST_WRITE = 16;

		// Token: 0x0400320F RID: 12815
		public const int FILE_NOTIFY_CHANGE_LAST_ACCESS = 32;

		// Token: 0x04003210 RID: 12816
		public const int FILE_NOTIFY_CHANGE_CREATION = 64;

		// Token: 0x04003211 RID: 12817
		public const int FILE_NOTIFY_CHANGE_SECURITY = 256;
	}
}
