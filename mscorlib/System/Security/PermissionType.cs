using System;

namespace System.Security
{
	// Token: 0x0200066E RID: 1646
	[Serializable]
	internal enum PermissionType
	{
		// Token: 0x04001E98 RID: 7832
		SecurityUnmngdCodeAccess,
		// Token: 0x04001E99 RID: 7833
		SecuritySkipVerification,
		// Token: 0x04001E9A RID: 7834
		ReflectionTypeInfo,
		// Token: 0x04001E9B RID: 7835
		SecurityAssert,
		// Token: 0x04001E9C RID: 7836
		ReflectionMemberAccess,
		// Token: 0x04001E9D RID: 7837
		SecuritySerialization,
		// Token: 0x04001E9E RID: 7838
		ReflectionRestrictedMemberAccess,
		// Token: 0x04001E9F RID: 7839
		FullTrust,
		// Token: 0x04001EA0 RID: 7840
		SecurityBindingRedirects,
		// Token: 0x04001EA1 RID: 7841
		UIPermission,
		// Token: 0x04001EA2 RID: 7842
		EnvironmentPermission,
		// Token: 0x04001EA3 RID: 7843
		FileDialogPermission,
		// Token: 0x04001EA4 RID: 7844
		FileIOPermission,
		// Token: 0x04001EA5 RID: 7845
		ReflectionPermission,
		// Token: 0x04001EA6 RID: 7846
		SecurityPermission,
		// Token: 0x04001EA7 RID: 7847
		SecurityControlEvidence = 16,
		// Token: 0x04001EA8 RID: 7848
		SecurityControlPrincipal
	}
}
