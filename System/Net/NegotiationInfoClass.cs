using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000546 RID: 1350
	internal class NegotiationInfoClass
	{
		// Token: 0x06002911 RID: 10513 RVA: 0x000AB714 File Offset: 0x000AA714
		internal NegotiationInfoClass(SafeHandle safeHandle, int negotiationState)
		{
			if (safeHandle.IsInvalid)
			{
				return;
			}
			IntPtr ptr = safeHandle.DangerousGetHandle();
			if (negotiationState == 0 || negotiationState == 1)
			{
				IntPtr intPtr = Marshal.ReadIntPtr(ptr, SecurityPackageInfo.NameOffest);
				string text = null;
				if (intPtr != IntPtr.Zero)
				{
					text = (ComNetOS.IsWin9x ? Marshal.PtrToStringAnsi(intPtr) : Marshal.PtrToStringUni(intPtr));
				}
				if (string.Compare(text, "Kerberos", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "Kerberos";
					return;
				}
				if (string.Compare(text, "NTLM", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "NTLM";
					return;
				}
				if (string.Compare(text, "WDigest", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "WDigest";
					return;
				}
				this.AuthenticationPackage = text;
			}
		}

		// Token: 0x04002817 RID: 10263
		internal const string NTLM = "NTLM";

		// Token: 0x04002818 RID: 10264
		internal const string Kerberos = "Kerberos";

		// Token: 0x04002819 RID: 10265
		internal const string WDigest = "WDigest";

		// Token: 0x0400281A RID: 10266
		internal const string Negotiate = "Negotiate";

		// Token: 0x0400281B RID: 10267
		internal string AuthenticationPackage;
	}
}
