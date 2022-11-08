using System;

namespace System.Net.Mail
{
	// Token: 0x020006D7 RID: 1751
	public enum SmtpStatusCode
	{
		// Token: 0x0400312B RID: 12587
		SystemStatus = 211,
		// Token: 0x0400312C RID: 12588
		HelpMessage = 214,
		// Token: 0x0400312D RID: 12589
		ServiceReady = 220,
		// Token: 0x0400312E RID: 12590
		ServiceClosingTransmissionChannel,
		// Token: 0x0400312F RID: 12591
		Ok = 250,
		// Token: 0x04003130 RID: 12592
		UserNotLocalWillForward,
		// Token: 0x04003131 RID: 12593
		CannotVerifyUserWillAttemptDelivery,
		// Token: 0x04003132 RID: 12594
		StartMailInput = 354,
		// Token: 0x04003133 RID: 12595
		ServiceNotAvailable = 421,
		// Token: 0x04003134 RID: 12596
		MailboxBusy = 450,
		// Token: 0x04003135 RID: 12597
		LocalErrorInProcessing,
		// Token: 0x04003136 RID: 12598
		InsufficientStorage,
		// Token: 0x04003137 RID: 12599
		ClientNotPermitted = 454,
		// Token: 0x04003138 RID: 12600
		CommandUnrecognized = 500,
		// Token: 0x04003139 RID: 12601
		SyntaxError,
		// Token: 0x0400313A RID: 12602
		CommandNotImplemented,
		// Token: 0x0400313B RID: 12603
		BadCommandSequence,
		// Token: 0x0400313C RID: 12604
		MustIssueStartTlsFirst = 530,
		// Token: 0x0400313D RID: 12605
		CommandParameterNotImplemented = 504,
		// Token: 0x0400313E RID: 12606
		MailboxUnavailable = 550,
		// Token: 0x0400313F RID: 12607
		UserNotLocalTryAlternatePath,
		// Token: 0x04003140 RID: 12608
		ExceededStorageAllocation,
		// Token: 0x04003141 RID: 12609
		MailboxNameNotAllowed,
		// Token: 0x04003142 RID: 12610
		TransactionFailed,
		// Token: 0x04003143 RID: 12611
		GeneralFailure = -1
	}
}
