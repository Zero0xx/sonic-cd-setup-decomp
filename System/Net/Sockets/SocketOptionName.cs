using System;

namespace System.Net.Sockets
{
	// Token: 0x020005C4 RID: 1476
	public enum SocketOptionName
	{
		// Token: 0x04002BE4 RID: 11236
		Debug = 1,
		// Token: 0x04002BE5 RID: 11237
		AcceptConnection,
		// Token: 0x04002BE6 RID: 11238
		ReuseAddress = 4,
		// Token: 0x04002BE7 RID: 11239
		KeepAlive = 8,
		// Token: 0x04002BE8 RID: 11240
		DontRoute = 16,
		// Token: 0x04002BE9 RID: 11241
		Broadcast = 32,
		// Token: 0x04002BEA RID: 11242
		UseLoopback = 64,
		// Token: 0x04002BEB RID: 11243
		Linger = 128,
		// Token: 0x04002BEC RID: 11244
		OutOfBandInline = 256,
		// Token: 0x04002BED RID: 11245
		DontLinger = -129,
		// Token: 0x04002BEE RID: 11246
		ExclusiveAddressUse = -5,
		// Token: 0x04002BEF RID: 11247
		SendBuffer = 4097,
		// Token: 0x04002BF0 RID: 11248
		ReceiveBuffer,
		// Token: 0x04002BF1 RID: 11249
		SendLowWater,
		// Token: 0x04002BF2 RID: 11250
		ReceiveLowWater,
		// Token: 0x04002BF3 RID: 11251
		SendTimeout,
		// Token: 0x04002BF4 RID: 11252
		ReceiveTimeout,
		// Token: 0x04002BF5 RID: 11253
		Error,
		// Token: 0x04002BF6 RID: 11254
		Type,
		// Token: 0x04002BF7 RID: 11255
		MaxConnections = 2147483647,
		// Token: 0x04002BF8 RID: 11256
		IPOptions = 1,
		// Token: 0x04002BF9 RID: 11257
		HeaderIncluded,
		// Token: 0x04002BFA RID: 11258
		TypeOfService,
		// Token: 0x04002BFB RID: 11259
		IpTimeToLive,
		// Token: 0x04002BFC RID: 11260
		MulticastInterface = 9,
		// Token: 0x04002BFD RID: 11261
		MulticastTimeToLive,
		// Token: 0x04002BFE RID: 11262
		MulticastLoopback,
		// Token: 0x04002BFF RID: 11263
		AddMembership,
		// Token: 0x04002C00 RID: 11264
		DropMembership,
		// Token: 0x04002C01 RID: 11265
		DontFragment,
		// Token: 0x04002C02 RID: 11266
		AddSourceMembership,
		// Token: 0x04002C03 RID: 11267
		DropSourceMembership,
		// Token: 0x04002C04 RID: 11268
		BlockSource,
		// Token: 0x04002C05 RID: 11269
		UnblockSource,
		// Token: 0x04002C06 RID: 11270
		PacketInformation,
		// Token: 0x04002C07 RID: 11271
		HopLimit = 21,
		// Token: 0x04002C08 RID: 11272
		NoDelay = 1,
		// Token: 0x04002C09 RID: 11273
		BsdUrgent,
		// Token: 0x04002C0A RID: 11274
		Expedited = 2,
		// Token: 0x04002C0B RID: 11275
		NoChecksum = 1,
		// Token: 0x04002C0C RID: 11276
		ChecksumCoverage = 20,
		// Token: 0x04002C0D RID: 11277
		UpdateAcceptContext = 28683,
		// Token: 0x04002C0E RID: 11278
		UpdateConnectContext = 28688
	}
}
