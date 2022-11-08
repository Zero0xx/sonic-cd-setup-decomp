using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020003BD RID: 957
	internal class FtpMethodInfo
	{
		// Token: 0x06001DE6 RID: 7654 RVA: 0x0007158F File Offset: 0x0007058F
		internal FtpMethodInfo(string method, FtpOperation operation, FtpMethodFlags flags, string httpCommand)
		{
			this.Method = method;
			this.Operation = operation;
			this.Flags = flags;
			this.HttpCommand = httpCommand;
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x000715B4 File Offset: 0x000705B4
		internal bool HasFlag(FtpMethodFlags flags)
		{
			return (this.Flags & flags) != FtpMethodFlags.None;
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001DE8 RID: 7656 RVA: 0x000715C4 File Offset: 0x000705C4
		internal bool IsCommandOnly
		{
			get
			{
				return (this.Flags & (FtpMethodFlags.IsDownload | FtpMethodFlags.IsUpload)) == FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x000715D1 File Offset: 0x000705D1
		internal bool IsUpload
		{
			get
			{
				return (this.Flags & FtpMethodFlags.IsUpload) != FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x000715E1 File Offset: 0x000705E1
		internal bool IsDownload
		{
			get
			{
				return (this.Flags & FtpMethodFlags.IsDownload) != FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x000715F1 File Offset: 0x000705F1
		internal bool HasHttpCommand
		{
			get
			{
				return (this.Flags & FtpMethodFlags.HasHttpCommand) != FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x00071605 File Offset: 0x00070605
		internal bool ShouldParseForResponseUri
		{
			get
			{
				return (this.Flags & FtpMethodFlags.ShouldParseForResponseUri) != FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001DED RID: 7661 RVA: 0x00071616 File Offset: 0x00070616
		internal bool IsUnknownMethod
		{
			get
			{
				return this.Operation == FtpOperation.Other;
			}
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x00071624 File Offset: 0x00070624
		internal static FtpMethodInfo GetMethodInfo(string method)
		{
			method = method.ToUpper(CultureInfo.InvariantCulture);
			foreach (FtpMethodInfo ftpMethodInfo in FtpMethodInfo.KnownMethodInfo)
			{
				if (method == ftpMethodInfo.Method)
				{
					return ftpMethodInfo;
				}
			}
			throw new ArgumentException(SR.GetString("net_ftp_unsupported_method"), "method");
		}

		// Token: 0x04001DFF RID: 7679
		internal string Method;

		// Token: 0x04001E00 RID: 7680
		internal FtpOperation Operation;

		// Token: 0x04001E01 RID: 7681
		internal FtpMethodFlags Flags;

		// Token: 0x04001E02 RID: 7682
		internal string HttpCommand;

		// Token: 0x04001E03 RID: 7683
		private static readonly FtpMethodInfo[] KnownMethodInfo = new FtpMethodInfo[]
		{
			new FtpMethodInfo("RETR", FtpOperation.DownloadFile, FtpMethodFlags.IsDownload | FtpMethodFlags.TakesParameter | FtpMethodFlags.HasHttpCommand, "GET"),
			new FtpMethodInfo("NLST", FtpOperation.ListDirectory, FtpMethodFlags.IsDownload | FtpMethodFlags.MayTakeParameter | FtpMethodFlags.HasHttpCommand, "GET"),
			new FtpMethodInfo("LIST", FtpOperation.ListDirectoryDetails, FtpMethodFlags.IsDownload | FtpMethodFlags.MayTakeParameter | FtpMethodFlags.HasHttpCommand, "GET"),
			new FtpMethodInfo("STOR", FtpOperation.UploadFile, FtpMethodFlags.IsUpload | FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("STOU", FtpOperation.UploadFileUnique, FtpMethodFlags.IsUpload | FtpMethodFlags.DoesNotTakeParameter | FtpMethodFlags.ShouldParseForResponseUri, null),
			new FtpMethodInfo("APPE", FtpOperation.AppendFile, FtpMethodFlags.IsUpload | FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("DELE", FtpOperation.DeleteFile, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("MDTM", FtpOperation.GetDateTimestamp, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("SIZE", FtpOperation.GetFileSize, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("RENAME", FtpOperation.Rename, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("MKD", FtpOperation.MakeDirectory, FtpMethodFlags.TakesParameter | FtpMethodFlags.ParameterIsDirectory, null),
			new FtpMethodInfo("RMD", FtpOperation.RemoveDirectory, FtpMethodFlags.TakesParameter | FtpMethodFlags.ParameterIsDirectory, null),
			new FtpMethodInfo("PWD", FtpOperation.PrintWorkingDirectory, FtpMethodFlags.DoesNotTakeParameter, null)
		};
	}
}
