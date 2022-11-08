using System;

namespace System.Net
{
	// Token: 0x020003BB RID: 955
	internal enum FtpOperation
	{
		// Token: 0x04001DE7 RID: 7655
		DownloadFile,
		// Token: 0x04001DE8 RID: 7656
		ListDirectory,
		// Token: 0x04001DE9 RID: 7657
		ListDirectoryDetails,
		// Token: 0x04001DEA RID: 7658
		UploadFile,
		// Token: 0x04001DEB RID: 7659
		UploadFileUnique,
		// Token: 0x04001DEC RID: 7660
		AppendFile,
		// Token: 0x04001DED RID: 7661
		DeleteFile,
		// Token: 0x04001DEE RID: 7662
		GetDateTimestamp,
		// Token: 0x04001DEF RID: 7663
		GetFileSize,
		// Token: 0x04001DF0 RID: 7664
		Rename,
		// Token: 0x04001DF1 RID: 7665
		MakeDirectory,
		// Token: 0x04001DF2 RID: 7666
		RemoveDirectory,
		// Token: 0x04001DF3 RID: 7667
		PrintWorkingDirectory,
		// Token: 0x04001DF4 RID: 7668
		Other
	}
}
