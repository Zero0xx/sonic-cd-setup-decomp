using System;

namespace System.Net
{
	// Token: 0x020003F2 RID: 1010
	internal enum SecurityStatus
	{
		// Token: 0x04001FE1 RID: 8161
		OK,
		// Token: 0x04001FE2 RID: 8162
		ContinueNeeded = 590610,
		// Token: 0x04001FE3 RID: 8163
		CompleteNeeded,
		// Token: 0x04001FE4 RID: 8164
		CompAndContinue,
		// Token: 0x04001FE5 RID: 8165
		ContextExpired = 590615,
		// Token: 0x04001FE6 RID: 8166
		CredentialsNeeded = 590624,
		// Token: 0x04001FE7 RID: 8167
		Renegotiate,
		// Token: 0x04001FE8 RID: 8168
		OutOfMemory = -2146893056,
		// Token: 0x04001FE9 RID: 8169
		InvalidHandle,
		// Token: 0x04001FEA RID: 8170
		Unsupported,
		// Token: 0x04001FEB RID: 8171
		TargetUnknown,
		// Token: 0x04001FEC RID: 8172
		InternalError,
		// Token: 0x04001FED RID: 8173
		PackageNotFound,
		// Token: 0x04001FEE RID: 8174
		NotOwner,
		// Token: 0x04001FEF RID: 8175
		CannotInstall,
		// Token: 0x04001FF0 RID: 8176
		InvalidToken,
		// Token: 0x04001FF1 RID: 8177
		CannotPack,
		// Token: 0x04001FF2 RID: 8178
		QopNotSupported,
		// Token: 0x04001FF3 RID: 8179
		NoImpersonation,
		// Token: 0x04001FF4 RID: 8180
		LogonDenied,
		// Token: 0x04001FF5 RID: 8181
		UnknownCredentials,
		// Token: 0x04001FF6 RID: 8182
		NoCredentials,
		// Token: 0x04001FF7 RID: 8183
		MessageAltered,
		// Token: 0x04001FF8 RID: 8184
		OutOfSequence,
		// Token: 0x04001FF9 RID: 8185
		NoAuthenticatingAuthority,
		// Token: 0x04001FFA RID: 8186
		IncompleteMessage = -2146893032,
		// Token: 0x04001FFB RID: 8187
		IncompleteCredentials = -2146893024,
		// Token: 0x04001FFC RID: 8188
		BufferNotEnough,
		// Token: 0x04001FFD RID: 8189
		WrongPrincipal,
		// Token: 0x04001FFE RID: 8190
		TimeSkew = -2146893020,
		// Token: 0x04001FFF RID: 8191
		UntrustedRoot,
		// Token: 0x04002000 RID: 8192
		IllegalMessage,
		// Token: 0x04002001 RID: 8193
		CertUnknown,
		// Token: 0x04002002 RID: 8194
		CertExpired,
		// Token: 0x04002003 RID: 8195
		AlgorithmMismatch = -2146893007,
		// Token: 0x04002004 RID: 8196
		SecurityQosFailed,
		// Token: 0x04002005 RID: 8197
		SmartcardLogonRequired = -2146892994,
		// Token: 0x04002006 RID: 8198
		UnsupportedPreauth = -2146892989,
		// Token: 0x04002007 RID: 8199
		BadBinding = -2146892986
	}
}
