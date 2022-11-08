using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x02000915 RID: 2325
	[ComVisible(false)]
	public enum WellKnownSidType
	{
		// Token: 0x04002BA5 RID: 11173
		NullSid,
		// Token: 0x04002BA6 RID: 11174
		WorldSid,
		// Token: 0x04002BA7 RID: 11175
		LocalSid,
		// Token: 0x04002BA8 RID: 11176
		CreatorOwnerSid,
		// Token: 0x04002BA9 RID: 11177
		CreatorGroupSid,
		// Token: 0x04002BAA RID: 11178
		CreatorOwnerServerSid,
		// Token: 0x04002BAB RID: 11179
		CreatorGroupServerSid,
		// Token: 0x04002BAC RID: 11180
		NTAuthoritySid,
		// Token: 0x04002BAD RID: 11181
		DialupSid,
		// Token: 0x04002BAE RID: 11182
		NetworkSid,
		// Token: 0x04002BAF RID: 11183
		BatchSid,
		// Token: 0x04002BB0 RID: 11184
		InteractiveSid,
		// Token: 0x04002BB1 RID: 11185
		ServiceSid,
		// Token: 0x04002BB2 RID: 11186
		AnonymousSid,
		// Token: 0x04002BB3 RID: 11187
		ProxySid,
		// Token: 0x04002BB4 RID: 11188
		EnterpriseControllersSid,
		// Token: 0x04002BB5 RID: 11189
		SelfSid,
		// Token: 0x04002BB6 RID: 11190
		AuthenticatedUserSid,
		// Token: 0x04002BB7 RID: 11191
		RestrictedCodeSid,
		// Token: 0x04002BB8 RID: 11192
		TerminalServerSid,
		// Token: 0x04002BB9 RID: 11193
		RemoteLogonIdSid,
		// Token: 0x04002BBA RID: 11194
		LogonIdsSid,
		// Token: 0x04002BBB RID: 11195
		LocalSystemSid,
		// Token: 0x04002BBC RID: 11196
		LocalServiceSid,
		// Token: 0x04002BBD RID: 11197
		NetworkServiceSid,
		// Token: 0x04002BBE RID: 11198
		BuiltinDomainSid,
		// Token: 0x04002BBF RID: 11199
		BuiltinAdministratorsSid,
		// Token: 0x04002BC0 RID: 11200
		BuiltinUsersSid,
		// Token: 0x04002BC1 RID: 11201
		BuiltinGuestsSid,
		// Token: 0x04002BC2 RID: 11202
		BuiltinPowerUsersSid,
		// Token: 0x04002BC3 RID: 11203
		BuiltinAccountOperatorsSid,
		// Token: 0x04002BC4 RID: 11204
		BuiltinSystemOperatorsSid,
		// Token: 0x04002BC5 RID: 11205
		BuiltinPrintOperatorsSid,
		// Token: 0x04002BC6 RID: 11206
		BuiltinBackupOperatorsSid,
		// Token: 0x04002BC7 RID: 11207
		BuiltinReplicatorSid,
		// Token: 0x04002BC8 RID: 11208
		BuiltinPreWindows2000CompatibleAccessSid,
		// Token: 0x04002BC9 RID: 11209
		BuiltinRemoteDesktopUsersSid,
		// Token: 0x04002BCA RID: 11210
		BuiltinNetworkConfigurationOperatorsSid,
		// Token: 0x04002BCB RID: 11211
		AccountAdministratorSid,
		// Token: 0x04002BCC RID: 11212
		AccountGuestSid,
		// Token: 0x04002BCD RID: 11213
		AccountKrbtgtSid,
		// Token: 0x04002BCE RID: 11214
		AccountDomainAdminsSid,
		// Token: 0x04002BCF RID: 11215
		AccountDomainUsersSid,
		// Token: 0x04002BD0 RID: 11216
		AccountDomainGuestsSid,
		// Token: 0x04002BD1 RID: 11217
		AccountComputersSid,
		// Token: 0x04002BD2 RID: 11218
		AccountControllersSid,
		// Token: 0x04002BD3 RID: 11219
		AccountCertAdminsSid,
		// Token: 0x04002BD4 RID: 11220
		AccountSchemaAdminsSid,
		// Token: 0x04002BD5 RID: 11221
		AccountEnterpriseAdminsSid,
		// Token: 0x04002BD6 RID: 11222
		AccountPolicyAdminsSid,
		// Token: 0x04002BD7 RID: 11223
		AccountRasAndIasServersSid,
		// Token: 0x04002BD8 RID: 11224
		NtlmAuthenticationSid,
		// Token: 0x04002BD9 RID: 11225
		DigestAuthenticationSid,
		// Token: 0x04002BDA RID: 11226
		SChannelAuthenticationSid,
		// Token: 0x04002BDB RID: 11227
		ThisOrganizationSid,
		// Token: 0x04002BDC RID: 11228
		OtherOrganizationSid,
		// Token: 0x04002BDD RID: 11229
		BuiltinIncomingForestTrustBuildersSid,
		// Token: 0x04002BDE RID: 11230
		BuiltinPerformanceMonitoringUsersSid,
		// Token: 0x04002BDF RID: 11231
		BuiltinPerformanceLoggingUsersSid,
		// Token: 0x04002BE0 RID: 11232
		BuiltinAuthorizationAccessSid,
		// Token: 0x04002BE1 RID: 11233
		WinBuiltinTerminalServerLicenseServersSid,
		// Token: 0x04002BE2 RID: 11234
		MaxDefined = 60
	}
}
