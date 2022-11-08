using System;

namespace System.Net
{
	// Token: 0x0200049E RID: 1182
	public enum WebExceptionStatus
	{
		// Token: 0x0400246A RID: 9322
		Success,
		// Token: 0x0400246B RID: 9323
		NameResolutionFailure,
		// Token: 0x0400246C RID: 9324
		ConnectFailure,
		// Token: 0x0400246D RID: 9325
		ReceiveFailure,
		// Token: 0x0400246E RID: 9326
		SendFailure,
		// Token: 0x0400246F RID: 9327
		PipelineFailure,
		// Token: 0x04002470 RID: 9328
		RequestCanceled,
		// Token: 0x04002471 RID: 9329
		ProtocolError,
		// Token: 0x04002472 RID: 9330
		ConnectionClosed,
		// Token: 0x04002473 RID: 9331
		TrustFailure,
		// Token: 0x04002474 RID: 9332
		SecureChannelFailure,
		// Token: 0x04002475 RID: 9333
		ServerProtocolViolation,
		// Token: 0x04002476 RID: 9334
		KeepAliveFailure,
		// Token: 0x04002477 RID: 9335
		Pending,
		// Token: 0x04002478 RID: 9336
		Timeout,
		// Token: 0x04002479 RID: 9337
		ProxyNameResolutionFailure,
		// Token: 0x0400247A RID: 9338
		UnknownError,
		// Token: 0x0400247B RID: 9339
		MessageLengthLimitExceeded,
		// Token: 0x0400247C RID: 9340
		CacheEntryNotFound,
		// Token: 0x0400247D RID: 9341
		RequestProhibitedByCachePolicy,
		// Token: 0x0400247E RID: 9342
		RequestProhibitedByProxy
	}
}
