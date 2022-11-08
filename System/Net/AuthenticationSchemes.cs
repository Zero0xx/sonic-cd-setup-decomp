using System;

namespace System.Net
{
	// Token: 0x0200037A RID: 890
	[Flags]
	public enum AuthenticationSchemes
	{
		// Token: 0x04001C83 RID: 7299
		None = 0,
		// Token: 0x04001C84 RID: 7300
		Digest = 1,
		// Token: 0x04001C85 RID: 7301
		Negotiate = 2,
		// Token: 0x04001C86 RID: 7302
		Ntlm = 4,
		// Token: 0x04001C87 RID: 7303
		Basic = 8,
		// Token: 0x04001C88 RID: 7304
		Anonymous = 32768,
		// Token: 0x04001C89 RID: 7305
		IntegratedWindowsAuthentication = 6
	}
}
