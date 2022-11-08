using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005D5 RID: 1493
	public abstract class IcmpV4Statistics
	{
		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06002EE2 RID: 12002
		public abstract long AddressMaskRepliesReceived { get; }

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06002EE3 RID: 12003
		public abstract long AddressMaskRepliesSent { get; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06002EE4 RID: 12004
		public abstract long AddressMaskRequestsReceived { get; }

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06002EE5 RID: 12005
		public abstract long AddressMaskRequestsSent { get; }

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06002EE6 RID: 12006
		public abstract long DestinationUnreachableMessagesReceived { get; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06002EE7 RID: 12007
		public abstract long DestinationUnreachableMessagesSent { get; }

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06002EE8 RID: 12008
		public abstract long EchoRepliesReceived { get; }

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06002EE9 RID: 12009
		public abstract long EchoRepliesSent { get; }

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06002EEA RID: 12010
		public abstract long EchoRequestsReceived { get; }

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06002EEB RID: 12011
		public abstract long EchoRequestsSent { get; }

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06002EEC RID: 12012
		public abstract long ErrorsReceived { get; }

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06002EED RID: 12013
		public abstract long ErrorsSent { get; }

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06002EEE RID: 12014
		public abstract long MessagesReceived { get; }

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06002EEF RID: 12015
		public abstract long MessagesSent { get; }

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06002EF0 RID: 12016
		public abstract long ParameterProblemsReceived { get; }

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06002EF1 RID: 12017
		public abstract long ParameterProblemsSent { get; }

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06002EF2 RID: 12018
		public abstract long RedirectsReceived { get; }

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06002EF3 RID: 12019
		public abstract long RedirectsSent { get; }

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06002EF4 RID: 12020
		public abstract long SourceQuenchesReceived { get; }

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06002EF5 RID: 12021
		public abstract long SourceQuenchesSent { get; }

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002EF6 RID: 12022
		public abstract long TimeExceededMessagesReceived { get; }

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002EF7 RID: 12023
		public abstract long TimeExceededMessagesSent { get; }

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002EF8 RID: 12024
		public abstract long TimestampRepliesReceived { get; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06002EF9 RID: 12025
		public abstract long TimestampRepliesSent { get; }

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06002EFA RID: 12026
		public abstract long TimestampRequestsReceived { get; }

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06002EFB RID: 12027
		public abstract long TimestampRequestsSent { get; }
	}
}
