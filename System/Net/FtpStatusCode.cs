using System;

namespace System.Net
{
	// Token: 0x020003B6 RID: 950
	public enum FtpStatusCode
	{
		// Token: 0x04001DAC RID: 7596
		Undefined,
		// Token: 0x04001DAD RID: 7597
		RestartMarker = 110,
		// Token: 0x04001DAE RID: 7598
		ServiceTemporarilyNotAvailable = 120,
		// Token: 0x04001DAF RID: 7599
		DataAlreadyOpen = 125,
		// Token: 0x04001DB0 RID: 7600
		OpeningData = 150,
		// Token: 0x04001DB1 RID: 7601
		CommandOK = 200,
		// Token: 0x04001DB2 RID: 7602
		CommandExtraneous = 202,
		// Token: 0x04001DB3 RID: 7603
		DirectoryStatus = 212,
		// Token: 0x04001DB4 RID: 7604
		FileStatus,
		// Token: 0x04001DB5 RID: 7605
		SystemType = 215,
		// Token: 0x04001DB6 RID: 7606
		SendUserCommand = 220,
		// Token: 0x04001DB7 RID: 7607
		ClosingControl,
		// Token: 0x04001DB8 RID: 7608
		ClosingData = 226,
		// Token: 0x04001DB9 RID: 7609
		EnteringPassive,
		// Token: 0x04001DBA RID: 7610
		LoggedInProceed = 230,
		// Token: 0x04001DBB RID: 7611
		ServerWantsSecureSession = 234,
		// Token: 0x04001DBC RID: 7612
		FileActionOK = 250,
		// Token: 0x04001DBD RID: 7613
		PathnameCreated = 257,
		// Token: 0x04001DBE RID: 7614
		SendPasswordCommand = 331,
		// Token: 0x04001DBF RID: 7615
		NeedLoginAccount,
		// Token: 0x04001DC0 RID: 7616
		FileCommandPending = 350,
		// Token: 0x04001DC1 RID: 7617
		ServiceNotAvailable = 421,
		// Token: 0x04001DC2 RID: 7618
		CantOpenData = 425,
		// Token: 0x04001DC3 RID: 7619
		ConnectionClosed,
		// Token: 0x04001DC4 RID: 7620
		ActionNotTakenFileUnavailableOrBusy = 450,
		// Token: 0x04001DC5 RID: 7621
		ActionAbortedLocalProcessingError,
		// Token: 0x04001DC6 RID: 7622
		ActionNotTakenInsufficientSpace,
		// Token: 0x04001DC7 RID: 7623
		CommandSyntaxError = 500,
		// Token: 0x04001DC8 RID: 7624
		ArgumentSyntaxError,
		// Token: 0x04001DC9 RID: 7625
		CommandNotImplemented,
		// Token: 0x04001DCA RID: 7626
		BadCommandSequence,
		// Token: 0x04001DCB RID: 7627
		NotLoggedIn = 530,
		// Token: 0x04001DCC RID: 7628
		AccountNeeded = 532,
		// Token: 0x04001DCD RID: 7629
		ActionNotTakenFileUnavailable = 550,
		// Token: 0x04001DCE RID: 7630
		ActionAbortedUnknownPageType,
		// Token: 0x04001DCF RID: 7631
		FileActionAborted,
		// Token: 0x04001DD0 RID: 7632
		ActionNotTakenFilenameNotAllowed
	}
}
