using System;

namespace System.Net.Sockets
{
	// Token: 0x020005C1 RID: 1473
	public enum SocketError
	{
		// Token: 0x04002BA3 RID: 11171
		Success,
		// Token: 0x04002BA4 RID: 11172
		SocketError = -1,
		// Token: 0x04002BA5 RID: 11173
		Interrupted = 10004,
		// Token: 0x04002BA6 RID: 11174
		AccessDenied = 10013,
		// Token: 0x04002BA7 RID: 11175
		Fault,
		// Token: 0x04002BA8 RID: 11176
		InvalidArgument = 10022,
		// Token: 0x04002BA9 RID: 11177
		TooManyOpenSockets = 10024,
		// Token: 0x04002BAA RID: 11178
		WouldBlock = 10035,
		// Token: 0x04002BAB RID: 11179
		InProgress,
		// Token: 0x04002BAC RID: 11180
		AlreadyInProgress,
		// Token: 0x04002BAD RID: 11181
		NotSocket,
		// Token: 0x04002BAE RID: 11182
		DestinationAddressRequired,
		// Token: 0x04002BAF RID: 11183
		MessageSize,
		// Token: 0x04002BB0 RID: 11184
		ProtocolType,
		// Token: 0x04002BB1 RID: 11185
		ProtocolOption,
		// Token: 0x04002BB2 RID: 11186
		ProtocolNotSupported,
		// Token: 0x04002BB3 RID: 11187
		SocketNotSupported,
		// Token: 0x04002BB4 RID: 11188
		OperationNotSupported,
		// Token: 0x04002BB5 RID: 11189
		ProtocolFamilyNotSupported,
		// Token: 0x04002BB6 RID: 11190
		AddressFamilyNotSupported,
		// Token: 0x04002BB7 RID: 11191
		AddressAlreadyInUse,
		// Token: 0x04002BB8 RID: 11192
		AddressNotAvailable,
		// Token: 0x04002BB9 RID: 11193
		NetworkDown,
		// Token: 0x04002BBA RID: 11194
		NetworkUnreachable,
		// Token: 0x04002BBB RID: 11195
		NetworkReset,
		// Token: 0x04002BBC RID: 11196
		ConnectionAborted,
		// Token: 0x04002BBD RID: 11197
		ConnectionReset,
		// Token: 0x04002BBE RID: 11198
		NoBufferSpaceAvailable,
		// Token: 0x04002BBF RID: 11199
		IsConnected,
		// Token: 0x04002BC0 RID: 11200
		NotConnected,
		// Token: 0x04002BC1 RID: 11201
		Shutdown,
		// Token: 0x04002BC2 RID: 11202
		TimedOut = 10060,
		// Token: 0x04002BC3 RID: 11203
		ConnectionRefused,
		// Token: 0x04002BC4 RID: 11204
		HostDown = 10064,
		// Token: 0x04002BC5 RID: 11205
		HostUnreachable,
		// Token: 0x04002BC6 RID: 11206
		ProcessLimit = 10067,
		// Token: 0x04002BC7 RID: 11207
		SystemNotReady = 10091,
		// Token: 0x04002BC8 RID: 11208
		VersionNotSupported,
		// Token: 0x04002BC9 RID: 11209
		NotInitialized,
		// Token: 0x04002BCA RID: 11210
		Disconnecting = 10101,
		// Token: 0x04002BCB RID: 11211
		TypeNotFound = 10109,
		// Token: 0x04002BCC RID: 11212
		HostNotFound = 11001,
		// Token: 0x04002BCD RID: 11213
		TryAgain,
		// Token: 0x04002BCE RID: 11214
		NoRecovery,
		// Token: 0x04002BCF RID: 11215
		NoData,
		// Token: 0x04002BD0 RID: 11216
		IOPending = 997,
		// Token: 0x04002BD1 RID: 11217
		OperationAborted = 995
	}
}
